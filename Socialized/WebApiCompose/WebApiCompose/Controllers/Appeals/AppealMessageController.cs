using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Messages.CreateAppealMessage;
using UseCases.Appeals.Messages.DeleteAppealMessage;
using UseCases.Appeals.Messages.UpdateAppealMessage;
using MediatR;
using System.Text.Json;

namespace WebAPI.Controllers.Appeals
{
    public class AppealMessageController : ControllerResponseBase
    {
        private ISender Sender;

        public AppealMessageController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult> Create([FromForm] ICollection<IFormFile> files, [FromForm] string commandJson)
        {
            var command = JsonSerializer.Deserialize<CreateAppealMessageCommand>(commandJson);
            command.Files = Map(files);

            return Ok(await Sender.Send(command));
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateAppealMessageCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] long messageId)
        {
            return Ok(await Sender.Send(new DeleteAppealMessageCommand {  MessageId = messageId }));
        }
    }
}