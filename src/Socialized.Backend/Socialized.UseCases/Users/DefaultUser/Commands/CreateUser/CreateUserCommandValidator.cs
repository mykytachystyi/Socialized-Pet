﻿using FluentValidation;

namespace UseCases.Users.DefaultUser.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(RuleFor => RuleFor.Email)
            .NotEmpty().WithMessage("Email is required");

        RuleFor(RuleFor => RuleFor.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First Name can't be longer than 50 characters");

        RuleFor(RuleFor => RuleFor.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name can't be longer than 50 characters");

        RuleFor(RuleFor => RuleFor.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password should be minimum 6 characters long");
    }
}
