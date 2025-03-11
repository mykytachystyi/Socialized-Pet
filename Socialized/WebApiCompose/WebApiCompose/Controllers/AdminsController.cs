using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Responses;
using UseCases.Admins;
using WebAPI.Middleware;
using UseCases.Admins.Commands.Authentication;
using UseCases.Admins.Commands.ChangePassword;
using UseCases.Admins.Commands.CreateAdmin;
using UseCases.Admins.Commands.Delete;
using UseCases.Admins.Commands.SetupPassword;

namespace WebAPI.Controllers
{
    public class AdminsController : ControllerResponseBase
    {
        private IAdminManager AdminManager;
        private IJwtTokenManager JwtTokenManager;

        public AdminsController(IAdminManager adminManager, IJwtTokenManager jwtTokenManager)
        {
            AdminManager = adminManager;
            JwtTokenManager = jwtTokenManager;
        }
        [HttpPost]
        [Authorize]
        public ActionResult<DataResponse> Create(CreateAdminCommand command)
        {
            var result = AdminManager.Create(command);

            return Ok(result);
        }
        [HttpPost]
        [ActionName("Authentication")]
        public ActionResult<DataResponse> Authentication(AuthenticationCommand command)
        {
            var result = AdminManager.Authentication(command);

            var token = JwtTokenManager.Authenticate(result);

            return Ok(new { AdminToken = token });
        }
        [HttpPost]
        [ActionName("SetupPassword")]
        public ActionResult<dynamic> SetupPassword(SetupPasswordCommand command)
        {
            AdminManager.SetupPassword(command);

            return Ok();
        }
        [HttpDelete]
        [Authorize]
        public ActionResult<dynamic> Delete(DeleteAdminCommand command)
        {
            AdminManager.Delete(command);

            return Ok();
        }
        [HttpPost]
        [ActionName("RecoveryPassword")]
        public ActionResult<dynamic> RecoveryPassword(string adminEmail)
        {
            AdminManager.CreateCodeForRecoveryPassword(adminEmail);

            return Ok();
        }
        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult<dynamic> ChangePassword(ChangePasswordCommand command)
        {
            AdminManager.ChangePassword(command);

            return Ok();
        }
        [HttpGet]
        [Authorize]
        [ActionName("Admins")]
        public ActionResult<dynamic> GetAdmins([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            long adminId = GetAdminIdByToken();

            var result = AdminManager.GetAdmins(adminId, since, count);

            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        [ActionName("Users")]
        public ActionResult<dynamic> GetUsers([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            var result = AdminManager.GetUsers(since, count);

            return Ok(result);
        }
    }
}