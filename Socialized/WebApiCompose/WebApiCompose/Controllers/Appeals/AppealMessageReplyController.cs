using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;
using UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;
using UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;

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
        public async Task<ActionResult> Create(CreateAppealMessageReplyCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(UpdateAppealMessageReplyCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> Delete(DeleteAppealMessageReplyCommand command)
        {
            return Ok(await Sender.Send(command));
        }
    }
}