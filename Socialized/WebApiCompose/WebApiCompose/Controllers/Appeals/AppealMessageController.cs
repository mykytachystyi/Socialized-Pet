using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Messages.CreateAppealMessage;
using UseCases.Appeals.Messages.DeleteAppealMessage;
using UseCases.Appeals.Messages.UpdateAppealMessage;
using MediatR;

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
        public async Task<ActionResult> Create([FromForm] ICollection<IFormFile> files, [FromForm] CreateAppealMessageCommand command)
        {
            command.Files = Map(files);

            return Ok(await Sender.Send(command));
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateAppealMessageCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteAppealMessageCommand command)
        {
            return Ok(await Sender.Send(command));
        }
    }
}