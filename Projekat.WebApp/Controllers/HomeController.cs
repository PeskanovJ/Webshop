using Microsoft.AspNetCore.Mvc;

namespace Projekat.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
