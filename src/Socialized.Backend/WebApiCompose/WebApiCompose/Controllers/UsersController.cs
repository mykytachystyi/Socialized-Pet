using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Users.Commands.Activate;
using UseCases.Users.Commands.ChangeOldPassword;
using UseCases.Users.Commands.ChangePassword;
using UseCases.Users.Commands.CheckRecoveryCode;
using UseCases.Users.Commands.CreateUser;
using UseCases.Users.Commands.Delete;
using UseCases.Users.Commands.LoginUser;
using UseCases.Users.Commands.LogOut;
using UseCases.Users.Commands.RecoveryPassword;
using UseCases.Users.Commands.RegistrationEmail;
using UseCases.Users.Models;

namespace WebAPI.Controllers
{
    public class UsersController : ControllerResponseBase
    {
        private ISender Sender;

        public UsersController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> Registration(CreateUserCommand command)
        {
            return Ok( await Sender.Send(command));
        }
        [HttpGet]
        public async Task<ActionResult<RegistrationEmailResponse>> RegistrationEmail([FromQuery] string email)
        {
            var culture = GetCulture();

            return Ok(await Sender.Send(new RegistrationEmailCommand { Culture = culture, UserEmail = email }));
        }
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Login(LoginUserCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult<LogOutResponse>> LogOut(LogOutCommand command)
        {
            return Ok(await Sender.Send(command));
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
        public async Task<ActionResult<ChangeOldPasswordResponse>> ChangeOldPassword(ChangeOldPasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpGet]
        public async Task<ActionResult<ActivateResponse>> Activate([FromQuery] string hash)
        {
            return Ok(await Sender.Send(new ActivateCommand { Hash = hash }));
        }
        [HttpPost]
        public async Task<ActionResult<DeleteResponse>> Delete(DeleteCommand command)
        {
            return Ok(await Sender.Send(command));
        }
    }
}