using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Files.CreateAppealMessageFile;
using UseCases.Appeals.Files.Models;
using WebAPI.Controllers;

namespace WebApiCompose.Controllers
{
    public class AppealFileController : ControllerResponseBase
    {
        private ISender Sender;

        public AppealFileController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        [Authorize]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult<IEnumerable<AppealFileResponse>>> Create(
            [FromQuery] long messageId, ICollection<IFormFile> files)
        {
            var filesDto = Map(files);

            return Ok(await Sender.Send(new CreateAppealMessageFileCommand
            {
                UserId = GetIdByJwtToken(),
                MessageId = messageId,
                Upload = filesDto
            }));
        }
    }
}