using System.Text;
using Core;
using Core.FileControl;
using Domain.Users;
using Domain.Admins;
using Domain.Appeals.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using WebAPI.Middleware;
using UseCases.Admins.Emails;
using UseCases.Users.Emails;
using FluentValidation;
using UseCases.Behaviors;
using MediatR;
using UseCases.Admins.Commands.Authentication;
using UseCases.Appeals.Files.CreateAppealMessageFile;
using Core.Providers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(AuthenticationCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(AuthenticationCommand).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(builder.Configuration.GetSection("MailSettings").Get<MailSettings>());
builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminEmailManager, AdminEmailManager>();
builder.Services.AddScoped<ISmtpSender, SmtpOauthSender>(); 

builder.Services.AddSingleton(builder.Configuration.GetSection("AwsSettings").Get<AwsSettings>());

builder.Services.AddScoped<IFileManager, AwsUploader>();
builder.Services.AddScoped<TextEncryptionProvider>();
builder.Services.AddScoped<IEncryptionProvider, HmacSha256Provider>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailMessanger, EmailMessanger>();

builder.Services.AddScoped<IAppealRepository, AppealRepository>();
builder.Services.AddScoped<IAppealMessageRepository, AppealMessageRepository>();

builder.Services.AddScoped<IAppealFileRepository, AppealFileRepository>();

builder.Services.AddScoped<IAppealMessageReplyRepository, AppealMessageReplyRepository>();

builder.Services.AddScoped<ICreateAppealFilesAdditionalToMessage, CreateAppealMessageFileCommandHandler>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOption =>
{
    var key = builder.Configuration.GetValue<string>("JwtConfig:Key");
    var keyBytes = Encoding.ASCII.GetBytes(key);
    jwtOption.SaveToken = true;
    jwtOption.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SocializedAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scopeDatabase  = app.Services.CreateScope())
{
    var dbContext = scopeDatabase.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.EnsureCreated();
}

app.MapSwagger().RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.ApplyMigrationsAsync();

app.Run();