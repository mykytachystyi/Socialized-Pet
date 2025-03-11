using MediatR;
using UseCases.Appeals.Models;

namespace UseCases.Appeals.Commands.CreateAppeal;

public record class CreateAppealCommand : IRequest<AppealResponse>
{
    public required string UserToken { get; set; }
    public required string Subject { get; set; }
}