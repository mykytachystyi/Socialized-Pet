using MediatR;

namespace UseCases.Users.Commands.Delete;

public class DeleteCommand : IRequest<DeleteResponse>
{
    public string UserToken { get; set; }
}
public record DeleteResponse(bool Success, string Message);