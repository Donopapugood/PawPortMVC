using Microsoft.AspNetCore.Mvc;

namespace PawPortMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string correo, string clave, string rol)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.Error = "Ingresa tu correo y contraseña.";
                return View();
            }

            if (rol == "Veterinario")
                return RedirectToAction("Index", "Dashboard");

            return RedirectToAction("Index", "MisMascotas");
        }
    }
}