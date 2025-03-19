using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Commands.CreateAppeal;
using UseCases.Appeals.Models;
using UseCases.Appeals.Queries.GetAppealsByAdmin;
using UseCases.Appeals.Queries.GetAppealsByUser;

namespace WebAPI.Controllers.Appeals
{
    public class AppealsController : ControllerResponseBase
    {
        private ISender Sender;

        public AppealsController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        public async Task<ActionResult<AppealResponse>> Create(CreateAppealCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppealResponse>>> GetAppealsByUser(
            [FromQuery] string userToken, [FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetAppealsByUserQuery { UserToken = userToken, Since = since, Count = count}));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AppealResponse>>>
            GetAppealsByAdmin([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetAppealsByAdminQuery { Since = since, Count = count }));
        }
    }
}