using Microsoft.AspNetCore.Mvc;

namespace Growell.Areas.Users.Controllers
{
    [Area("Users")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
