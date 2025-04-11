using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Users.DefaultUser.Commands.Activate;
using UseCases.Users.DefaultUser.Commands.ChangeOldPassword;
using UseCases.Users.DefaultUser.Commands.ChangePassword;
using UseCases.Users.DefaultUser.Commands.CheckRecoveryCode;
using UseCases.Users.DefaultUser.Commands.CreateUser;
using UseCases.Users.DefaultUser.Commands.Delete;
using UseCases.Users.DefaultUser.Commands.LoginUser;
using UseCases.Users.DefaultUser.Commands.RecoveryPassword;
using UseCases.Users.DefaultUser.Commands.RegistrationEmail;
using UseCases.Users.DefaultUser.Models;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    public class UsersController : ControllerResponseBase
    {
        private ISender Sender;
        private IJwtTokenManager JwtTokenManager;

        public UsersController(ISender sender, IJwtTokenManager jwtTokenManager)
        {
            Sender = sender;
            JwtTokenManager = jwtTokenManager;
        }
        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> Registration(CreateUserCommand request)
        {
            var command = new CreateUserWithRoleCommand((int)IdentityRole.DefaultUser, GetCulture())
            { 
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password
            };

            return Ok( await Sender.Send(command));
        }
        [HttpGet]
        public async Task<ActionResult<RegistrationEmailResponse>> RegistrationEmail([FromQuery] string email)
        {
            var culture = GetCulture();

            return Ok(await Sender.Send(new RegistrationEmailCommand { Culture = culture, UserEmail = email }));
        }
        [HttpPost]
        public async Task<ActionResult<LoginTokenResponse>> Login(LoginUserCommand command)
        {
            var result = await Sender.Send(new LoginUserWithRoleCommand
            {
                Email = command.Email,
                Password = command.Password,
                Role = (int) IdentityRole.DefaultUser
            });

            var token = JwtTokenManager.Authenticate(result);

            return Ok(new LoginTokenResponse(token));
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
        [Authorize(Roles = IdentityRoleConverter.DefaultUser)]
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
        public async Task<ActionResult<ActivateResponse>> Activate([FromQuery] string hash)
        {
            return Ok(await Sender.Send(new ActivateCommand { Hash = hash }));
        }
        [HttpDelete]
        [Authorize(Roles = IdentityRoleConverter.DefaultUser)]
        public async Task<ActionResult<DeleteResponse>> Delete()
        {
            return Ok(await Sender.Send(new DeleteCommand { UserId = GetIdByJwtToken() }));
        }
    }
}