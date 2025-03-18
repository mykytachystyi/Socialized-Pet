using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Responses;
using WebAPI.Middleware;
using UseCases.Admins.Commands.Authentication;
using UseCases.Admins.Commands.ChangePassword;
using UseCases.Admins.Commands.CreateAdmin;
using UseCases.Admins.Commands.Delete;
using UseCases.Admins.Commands.SetupPassword;
using MediatR;
using UseCases.Admins.Commands.CreateCodeForRecoveryPassword;
using UseCases.Admins.Queries.GetAdmins;
using UseCases.Admins.Queries.GetUsers;

namespace WebAPI.Controllers
{
    public class AdminsController : ControllerResponseBase
    {
        private ISender Sender;
        private IJwtTokenManager JwtTokenManager;

        public AdminsController(ISender sender, IJwtTokenManager jwtTokenManager)
        {
            Sender = sender;
            JwtTokenManager = jwtTokenManager;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<DataResponse>> Create(CreateAdminCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult<DataResponse>> Authentication(AuthenticationCommand command)
        {
            var result = await Sender.Send(command);

            var token = JwtTokenManager.Authenticate(result);

            return Ok(new { AdminToken = token });
        }
        [HttpPost]
        public async Task<ActionResult> SetupPassword(SetupPasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> Delete(DeleteAdminCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult> RecoveryPassword(string adminEmail)
        {
            return Ok(await Sender.Send(new CreateCodeForRecoveryPasswordCommand { AdminEmail = adminEmail}));
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAdmins([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            long adminId = GetAdminIdByToken();

            return Ok(await Sender.Send(new GetAdminsQuery { AdminId = adminId, Since = since, Count = count}));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUsers([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetUsersQuery { Since = since, Count = count }));
        }
    }
}