using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Messages.CreateAppealMessage;
using UseCases.Appeals.Messages.DeleteAppealMessage;
using UseCases.Appeals.Messages.UpdateAppealMessage;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Controllers;

namespace WebApiCompose.Controllers
{
    public class AppealMessageController : ControllerResponseBase
    {
        private ISender Sender;

        public AppealMessageController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        [Authorize]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult> Create([FromForm] ICollection<IFormFile> files, [FromForm] string commandJson)
        {
            var command = JsonSerializer.Deserialize<CreateAppealMessageWithUserCommand>(commandJson);
            
            command!.Files = Map(files);

            command.UserId = GetIdByJwtToken();

            return Ok(await Sender.Send(command));
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(UpdateAppealMessageCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> Delete([FromQuery] long messageId)
        {
            return Ok(await Sender.Send(new DeleteAppealMessageCommand { MessageId = messageId }));
        }
    }
}