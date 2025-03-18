using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Files.CreateAppealMessageFile;
using UseCases.Appeals.Files.Models;

namespace WebAPI.Controllers.Appeals
{
    public class AppealFileController : ControllerResponseBase
    {
        private ISender Sender;

        public AppealFileController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult<IEnumerable<AppealFileResponse>>> Create(
            [FromQuery] long messageId, ICollection<IFormFile> files)
        {
            var filesDto = Map(files);

            return Ok(await Sender.Send(new CreateAppealMessageFileCommand { MessageId = messageId, Upload = filesDto }));
        }
    }
}