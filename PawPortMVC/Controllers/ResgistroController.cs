using Microsoft.AspNetCore.Mvc;

namespace PawPortMVC.Controllers
{
    public class RegistroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string nombre, string correo, string telefono, string clave, string clave2, bool terminos)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Completa nombre, correo y contraseña.";
                return View();
            }
            if (clave != clave2)
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Las contraseñas no coinciden.";
                return View();
            }
            if (!terminos)
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Debes aceptar los términos.";
                return View();
            }

            ViewBag.MensajeClase = "msg-ok";
            ViewBag.Mensaje = "¡Cuenta creada! Bienvenido, " + nombre + ".";
            return View();
        }
    }
}