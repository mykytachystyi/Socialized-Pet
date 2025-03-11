using FluentValidation;

namespace UseCases.Admins.Commands.CreateAdmin;

public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
{
    public CreateAdminCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress()
            .WithMessage("Invalid email address");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50)
            .WithMessage("First name cannot be longer than 50 characters");
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50)
            .WithMessage("Last name cannot be longer than 50 characters");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100)
            .WithMessage("Password should be minimum 6 characters long");
    }
}
