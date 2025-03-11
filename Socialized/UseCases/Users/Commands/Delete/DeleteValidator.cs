using FluentValidation;

namespace UseCases.Users.Commands.Delete;

public class DeleteValidator : AbstractValidator<DeleteCommand>
{
    public DeleteValidator()
    {
        RuleFor(x => x.UserToken)
            .NotEmpty().WithMessage("Токен користувача не може бути порожнім.");
    }
}
