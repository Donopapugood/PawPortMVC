using Microsoft.AspNetCore.Mvc;

namespace PawPortMVC.Controllers
{
    public class PortafolioController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Portafolio/Index.cshtml");
        }
    }
}