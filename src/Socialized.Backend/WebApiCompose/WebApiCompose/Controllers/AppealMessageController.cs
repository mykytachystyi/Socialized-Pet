using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Messages.Commands.DeleteAppealMessage;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Controllers;
using UseCases.Appeals.Messages.Commands.CreateAppealMessage;
using UseCases.Appeals.Messages.Commands.UpdateAppealMessage;
using UseCases.Appeals.Messages.Models;
using UseCases.Appeals.Messages.Queries.GetAppealMessages;

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
        public async Task<ActionResult> Create([FromForm] ICollection<IFormFile> files, [FromQuery] string commandJson)
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
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AppealMessageResponse>>>
            Get([FromQuery] long appealId, [FromQuery] int since, [FromQuery] int count)
        {
            return Ok(await Sender.Send(new GetAppealMessagesCommand
            {
                AppealId = appealId,
                Since = since,
                Count = count
            }));
        }
    }
}