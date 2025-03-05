using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Messages.Commands;
using UseCases.Appeals.Messages;
using WebAPI.Responses;

namespace WebAPI.Controllers.Appeals
{
    public class AppealMessageController : ControllerResponseBase
    {
        private IAppealMessageManager AppealMessageManager;

        public AppealMessageController(IAppealMessageManager appealMessageManager)
        {
            AppealMessageManager = appealMessageManager;
        }
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public ActionResult<SuccessResponse> Create([FromForm] ICollection<IFormFile> files, [FromForm] CreateAppealMessageCommand command)
        {
            command.Files = Map(files);

            AppealMessageManager.Create(command);

            return Ok();
        }
        [HttpPut]
        public ActionResult<SuccessResponse> Update(UpdateAppealMessageCommand command)
        {
            AppealMessageManager.Update(command);

            return Ok();
        }
        [HttpDelete]
        public ActionResult<SuccessResponse> Delete(DeleteAppealMessageCommand command)
        {
            AppealMessageManager.Delete(command);

            return Ok();
        }
    }
}
