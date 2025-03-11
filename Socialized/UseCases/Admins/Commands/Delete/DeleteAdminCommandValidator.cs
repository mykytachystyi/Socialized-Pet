using FluentValidation;

namespace UseCases.Admins.Commands.Delete;

public class DeleteAdminCommandValidator : AbstractValidator<DeleteAdminCommand>
{
    public DeleteAdminCommandValidator()
    {
        RuleFor(x => x.AdminId)
            .NotEmpty().WithMessage("AdminId не може бути порожнім.")
            .GreaterThan(0).WithMessage("AdminId має бути більше 0.");
    }
}
