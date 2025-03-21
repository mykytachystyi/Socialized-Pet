using MediatR;

namespace UseCases.Users.DefaultUser.Commands.Activate;

public record class ActivateCommand : IRequest<ActivateResponse>
{
    public string Hash { get; init; }
}
public record class ActivateResponse(bool Success, string Message);
