using Microsoft.AspNetCore.Mvc;

namespace ChefManager.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
