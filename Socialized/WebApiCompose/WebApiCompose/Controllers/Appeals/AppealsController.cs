using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Commands.CreateAppeal;
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
        public async Task<ActionResult> Create(CreateAppealCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpGet]
        public async Task<ActionResult> GetAppealsByUser([FromQuery] string userToken, 
            [FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetAppealsByUserQuery { UserToken = userToken, Since = since, Count = count}));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAppealsByAdmin([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetAppealsByAdminQuery { Since = since, Count = count }));
        }
    }
}