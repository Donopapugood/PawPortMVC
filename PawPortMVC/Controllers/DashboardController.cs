using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;

namespace PawPortMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View(Datos.Citas);
        }
    }
}