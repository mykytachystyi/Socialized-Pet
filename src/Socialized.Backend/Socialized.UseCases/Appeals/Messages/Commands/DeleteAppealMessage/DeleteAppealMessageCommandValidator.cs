using FluentValidation;

namespace UseCases.Appeals.Messages.Commands.DeleteAppealMessage;

public class DeleteAppealMessageCommandValidator : AbstractValidator<DeleteAppealMessageCommand>
{
    public DeleteAppealMessageCommandValidator()
    {
        RuleFor(x => x.MessageId).NotEmpty().WithMessage("Message ID is required");
    }
}