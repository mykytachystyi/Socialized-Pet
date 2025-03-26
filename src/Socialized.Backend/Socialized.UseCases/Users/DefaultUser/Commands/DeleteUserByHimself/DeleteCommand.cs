using MediatR;

namespace UseCases.Users.DefaultUser.Commands.Delete;

public class DeleteCommand : IRequest<DeleteResponse>
{
    public long UserId { get; set; }
}
public record DeleteResponse(bool Success, string Message);