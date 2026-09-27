using Microsoft.AspNetCore.Mvc;

namespace thucung.Controllers
{
    public class IntroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
