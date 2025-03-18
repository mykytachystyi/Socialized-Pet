using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
using WebApiCompose.Responses;
using UseCases.Admins.Models;
using UseCases.Users.Commands.RecoveryPassword;
using UseCases.Users.Models;

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
        public async Task<ActionResult<AdminResponse>> Create(CreateAdminCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult<AdminTokenResponse>> Authentication(AuthenticationCommand command)
        {
            var result = await Sender.Send(command);

            var token = JwtTokenManager.Authenticate(result);

            return Ok(new AdminTokenResponse { AdminToken = token });
        }
        [HttpPost]
        public async Task<ActionResult<SetupPasswordResponse>> SetupPassword(SetupPasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<DeleteAdminResponse>> Delete(DeleteAdminCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult<RecoveryPasswordResponse>> RecoveryPassword(string adminEmail)
        {
            return Ok(await Sender.Send(new CreateCodeForRecoveryPasswordCommand { AdminEmail = adminEmail}));
        }
        [HttpPost]
        public async Task<ActionResult<ChangePasswordResponse>> ChangePassword(ChangePasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdminResponse>>> 
            GetAdmins([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            long adminId = GetAdminIdByToken();

            return Ok(await Sender.Send(new GetAdminsQuery { AdminId = adminId, Since = since, Count = count}));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponse>>> 
            GetUsers([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            return Ok(await Sender.Send(new GetUsersQuery { Since = since, Count = count }));
        }
    }
}