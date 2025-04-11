using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Middleware;
using MediatR;
using UseCases.Users.DefaultUser.Models;
using Domain.Enums;
using UseCases.Users.DefaultUser.Commands.LoginUser;
using UseCases.Users.DefaultUser.Commands.CreateUser;
using UseCases.Users.DefaultAdmin.Commands.SetupPassword;
using UseCases.Users.DefaultAdmin.Commands.DeleteAdmin;
using UseCases.Users.DefaultUser.Commands.ChangeOldPassword;
using UseCases.Users.DefaultUser.Commands.RecoveryPassword;
using UseCases.Users.DefaultAdmin.Queries.GetUsers;
using UseCases.Users.DefaultUser.Commands.ChangePassword;
using UseCases.Users.DefaultUser.Commands.CheckRecoveryCode;
using UseCases.Users.DefaultAdmin.Models;

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
        [Authorize(Roles = IdentityRoleConverter.DefaultAdmin)]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserCommand command)
        {
            return Ok(await Sender.Send(new CreateUserWithRoleCommand((int) IdentityRole.DefaultAdmin, "en_EN") 
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Password = command.Password
            }));
        }
        [HttpPost]
        public async Task<ActionResult<LoginTokenResponse>> Login(LoginUserCommand command)
        {
            var result = await Sender.Send(new LoginUserWithRoleCommand
            {
                Email = command.Email,
                Password = command.Password,
                Role = (int)IdentityRole.DefaultAdmin
            });

            var token = JwtTokenManager.Authenticate(result);

            return Ok(new LoginTokenResponse(token));
        }
        [HttpPost]
        public async Task<ActionResult<SetupPasswordResponse>> SetupPassword(SetupPasswordCommand request)
        {
            var command = new SetupPasswordWithAdminCommand
            {
                AdminId = GetIdByJwtToken(), Password = request.Password
            }; 

            return Ok(await Sender.Send(command));
        }
        [HttpDelete]
        [Authorize(Roles = IdentityRoleConverter.DefaultAdmin)]
        public async Task<ActionResult<DeleteAdminResponse>> Delete([FromQuery] long adminId)
        {
            return Ok(await Sender.Send(new DeleteAdminCommand { AdminId = adminId }));
        }
        [HttpGet]
        public async Task<ActionResult<RecoveryPasswordResponse>> RecoveryPassword([FromQuery] string email)
        {
            var culture = GetCulture();

            return Ok(await Sender.Send(new RecoveryPasswordCommand { UserEmail = email, Culture = culture }));
        }
        [HttpPost]
        public async Task<ActionResult<CheckRecoveryCodeResponse>> CheckRecoveryCode(CheckRecoveryCodeCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult<ChangeUserPasswordResponse>> ChangePassword(ChangeUserPasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        [Authorize(Roles = IdentityRoleConverter.DefaultAdmin)]
        public async Task<ActionResult<ChangeOldPasswordResponse>> ChangeOldPassword(ChangeOldPasswordCommand command)
        {
            return Ok(await Sender.Send(new ChangeOldPasswordWithUserCommand
            {
                NewPassword = command.NewPassword,
                OldPassword = command.OldPassword,
                UserId = GetIdByJwtToken()
            }));
        }
        [HttpGet]
        [Authorize(Roles = IdentityRoleConverter.DefaultAdmin)]
        public async Task<ActionResult<IEnumerable<UserResponse>>> 
            GetAdmins([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            long adminId = GetIdByJwtToken();

            return Ok(await Sender.Send(new GetUsersQuery 
            { 
                AdminId = adminId, 
                Since = since, 
                Count = count, 
                Role = (int) IdentityRole.DefaultAdmin 
            }));
        }
        [HttpGet]
        [Authorize(Roles = IdentityRoleConverter.DefaultAdmin)]
        public async Task<ActionResult<IEnumerable<UserResponse>>> 
            GetUsers([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            long adminId = GetIdByJwtToken();

            return Ok(await Sender.Send(new GetUsersQuery
            {
                AdminId = adminId,
                Since = since,
                Count = count,
                Role = (int)IdentityRole.DefaultUser
            }));
        }
    }
}