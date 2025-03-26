using FluentValidation;

namespace UseCases.Appeals.Files.CreateAppealMessageFile;

public class CreateAppealMessageFileValidator : AbstractValidator<CreateAppealMessageFileCommand>
{
    public CreateAppealMessageFileValidator()
    {
        RuleFor(x => x.MessageId)
            .NotEmpty().WithMessage("Message ID is required");
        RuleFor(x => x.Upload)
            .NotEmpty().WithMessage("Upload is required");
    }
}
