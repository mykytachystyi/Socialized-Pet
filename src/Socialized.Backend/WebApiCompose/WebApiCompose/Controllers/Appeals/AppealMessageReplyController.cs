using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;
using UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;
using UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;
using UseCases.Appeals.Replies.Models;

namespace WebAPI.Controllers.Appeals
{
    public class AppealMessageReplyController : ControllerResponseBase
    {
        private ISender Sender;

        public AppealMessageReplyController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AppealReplyResponse>> Create(CreateAppealMessageReplyCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<UpdateAppealMessageReplyResponse>> Update(UpdateAppealMessageReplyCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<DeleteAppealMessageReplyResponse>> Delete([FromQuery] long replyId)
        {
            return Ok(await Sender.Send(new DeleteAppealMessageReplyCommand { ReplyId = replyId }));
        }
    }
}