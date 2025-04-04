﻿using MediatR;

namespace UseCases.Appeals.Messages.Commands.DeleteAppealMessage;

public class DeleteAppealMessageCommand : IRequest<DeleteAppealMessageResponse>
{
    public long UserId { get; set; }
    public long MessageId { get; set; }
}

public record DeleteAppealMessageResponse(bool Success, string Message);
