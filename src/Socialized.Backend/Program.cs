using System.Text;
using Domain.Users;
using Domain.Appeals.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using WebAPI.Middleware;
using FluentValidation;
using UseCases.Behaviors;
using MediatR;
using UseCases.Appeals.Files.CreateAppealMessageFile;
using Core.Providers.Hmac;
using Core.Providers.TextEncrypt;
using Core.FileControl.Aws;
using Core.FileControl.CurrentFileSystem;
using Core.SmtpMailing;
using Core.Providers.Rand;
using Domain.Appeals;
using UseCases.Mapping;
using UseCases.Users.DefaultUser.Emails;
using UseCases.Users.DefaultAdmin.Emails;
using UseCases.Users.DefaultUser.Commands.LoginUser;
using Mapster;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddSingleton(Log.Logger);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger.Information("Connection string: {connectionString}", connectionString);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(LoginUserCommand).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
builder.Services.AddScoped<IAdminEmailManager, AdminEmailManager>();
builder.Services.AddScoped<ISmtpSender, SmtpOauthSender>(); 

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));

builder.Services.AddScoped<IFileManager, AwsUploader>();
builder.Services.AddScoped<ITextEncryptionProvider, TextEncryptionProvider>();
builder.Services.AddScoped<IEncryptionProvider, HmacSha256Provider>();
builder.Services.AddScoped<IRandomizer,  Randomizer>();

builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Appeal>, Repository<Appeal>>();
builder.Services.AddScoped<IRepository<AppealFile>, Repository<AppealFile>>();
builder.Services.AddScoped<IRepository<AppealMessage>, Repository<AppealMessage>>();
builder.Services.AddScoped<IAppealQueryRepository, AppealQueryRepository>();

builder.Services.AddScoped<IEmailMessanger, EmailMessanger>();

builder.Services.AddScoped<IAppealQueryRepository, AppealQueryRepository>();

builder.Services.AddScoped<ICreateAppealFilesAdditionalToMessage, CreateAppealMessageFileCommandHandler>();

var config = TypeAdapterConfig.GlobalSettings;
    config.Scan(typeof(MappingConfig).Assembly);
builder.Services.AddSingleton(config);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOption =>
{
    var key = builder.Configuration.GetValue<string>("JwtConfig:Key") ?? "";
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

app.MapSwagger().RequireAuthorization();

// Configure the HTTP request pipeline.System.ArgumentException: 'Couldn't set user Arg_ParamName_Name
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

await app.ApplyMigrationsAsync();

app.Run();

public partial class Program { }