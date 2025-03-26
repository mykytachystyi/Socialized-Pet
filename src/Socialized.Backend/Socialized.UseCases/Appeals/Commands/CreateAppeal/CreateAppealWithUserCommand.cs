using MediatR;
using UseCases.Appeals.Models;

namespace UseCases.Appeals.Commands.CreateAppeal;

public record class CreateAppealWithUserCommand : CreateAppealCommand, IRequest<AppealResponse>
{
    public required long UserId { get; set; }
}
public record class CreateAppealCommand
{
    public required string Subject { get; set; }
}