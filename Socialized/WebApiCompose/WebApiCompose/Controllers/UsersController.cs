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
        public async Task<ActionResult> Registration(CreateUserCommand command)
        {
            return Ok( await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult> RegistrationEmail([FromQuery] string email)
        {
            var culture = GetCulture();

            return Ok(await Sender.Send(new RegistrationEmailCommand { Culture = culture, UserEmail = email }));
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult> LogOut([FromBody] string userToken)
        {
            return Ok(await Sender.Send(new LogOutCommand { UserToken = userToken }));
        }
        [HttpPost]
        public async Task<ActionResult> RecoveryPassword([FromQuery] string email)
        {
            var culture = GetCulture();

            return Ok(await Sender.Send(new RecoveryPasswordCommand { UserEmail = email, Culture = culture }));
        }
        [HttpPost]
        public async Task<ActionResult> CheckRecoveryCode(CheckRecoveryCodeCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangeUserPasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult> ChangeOldPassword(ChangeOldPasswordCommand command)
        {
            return Ok(await Sender.Send(command));
        }
        [HttpGet]
        public async Task<ActionResult> Activate([FromQuery] string hash)
        {
            return Ok(await Sender.Send(new ActivateCommand { Hash = hash }));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] string userToken)
        {
            return Ok(await Sender.Send(new DeleteCommand { UserToken = userToken }));
        }
    }
}