using Microsoft.AspNetCore.Mvc;

namespace NoiPurchasingSystem.Areas.Purchasing.Controllers
{
    [Area("Purchasing")]
    [Route("Purchasing/[Controller]/[Action]")]
    //[Authorize(Roles = "Purchasing")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
