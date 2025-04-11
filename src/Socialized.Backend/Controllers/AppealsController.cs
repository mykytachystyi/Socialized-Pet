using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Appeals.Commands.CreateAppeal;
using UseCases.Appeals.Models;
using UseCases.Appeals.Queries.GetAppealsByAdmin;
using UseCases.Appeals.Queries.GetAppealsByUser;
using WebAPI.Controllers;

namespace WebApiCompose.Controllers
{
    public class AppealsController : ControllerResponseBase
    {
        private ISender Sender;

        public AppealsController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        [Authorize(Roles = IdentityRoleConverter.DefaultUser)]
        public async Task<ActionResult<AppealResponse>> Create(CreateAppealCommand command)
        {
            return Ok(await Sender.Send(new CreateAppealWithUserCommand 
            { 
                UserId = GetIdByJwtToken(), 
                Subject = command.Subject 
            }));
        }
        [HttpGet]
        [Authorize(Roles = IdentityRoleConverter.DefaultUser)]
        public async Task<ActionResult<IEnumerable<AppealResponse>>> GetAppealsByUser(
            [FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetAppealsByUserQuery
            {
                UserId = GetIdByJwtToken(),
                Since = since,
                Count = count
            }));
        }
        [HttpGet]
        [Authorize(Roles = IdentityRoleConverter.DefaultAdmin)]
        public async Task<ActionResult<IEnumerable<AppealResponse>>>
            GetAppealsByAdmin([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetAppealsByAdminQuery { Since = since, Count = count }));
        }
    }
}