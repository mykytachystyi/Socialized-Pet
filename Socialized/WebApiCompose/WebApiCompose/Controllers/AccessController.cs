using Microsoft.AspNetCore.Mvc;
using WebAPI.Responses;
using UseCases.Packages;
using UseCases.Packages.Command;

namespace WebAPI.Controllers
{
    public class AccessController : ControllerResponseBase
    {
        private IPackageManager PackageManager;
        public AccessController(IPackageManager packageManager)
        {
            PackageManager = packageManager;
        }  

        [HttpGet]
        [ActionName("AccessPackages")]
        public ActionResult<DataResponse> AccessPackages()
        {
            var packages = PackageManager.GetPackageAccess();

            return Ok(packages);
        }
        [HttpGet]
        [ActionName("Discounts")]
        public ActionResult<DataResponse> Discounts()
        {
            var discounts = PackageManager.GetDiscountPackageAccess();

            return Ok(discounts);
        }
        [HttpPost]
        [ActionName("GetClientToken")]
        public ActionResult<dynamic> GetClientToken(GetClientTokenForPayCommand command)
        {            
            var result = PackageManager.GetClientTokenForPay(command);

            return Ok(new { ClientToken = result });
        }
        [HttpPost]
        [ActionName("PackagePay")]
        public ActionResult<dynamic> PackagePay(PayForPackageCommand command)
        {
            PackageManager.PayForPackage(command);

            return Ok();
        }
    }
}