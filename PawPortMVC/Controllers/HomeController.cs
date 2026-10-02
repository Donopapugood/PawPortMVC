using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;

namespace PawPortMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Clinicas = Datos.Clinicas;
            return View();
        }

        [HttpPost]
        public IActionResult Contacto(string nombre, string correo, string mensaje)
        {
            ViewBag.Clinicas = Datos.Clinicas;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(mensaje))
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Completa todos los campos.";
                return View("Index");
            }

            ViewBag.MensajeClase = "msg-ok";
            ViewBag.Mensaje = "¡Gracias " + nombre + "! Te responderemos pronto.";
            return View("Index");
        }
    }
}