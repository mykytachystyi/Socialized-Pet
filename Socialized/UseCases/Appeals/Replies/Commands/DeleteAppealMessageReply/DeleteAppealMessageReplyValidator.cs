using FluentValidation;

namespace UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;

public class DeleteAppealMessageReplyValidator : AbstractValidator<DeleteAppealMessageReplyCommand>
{
    public DeleteAppealMessageReplyValidator()
    {
        RuleFor(x => x.ReplyId)
            .NotEmpty().WithMessage("Reply ID is required");
    }
}
