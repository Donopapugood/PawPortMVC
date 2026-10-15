using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioDatos _usuarioDatos;

        public LoginController(UsuarioDatos usuarioDatos)
        {
            _usuarioDatos = usuarioDatos;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.Error =
                    "Ingresa tu correo y contraseña.";

                return View();
            }

            Usuario? usuario =
                _usuarioDatos.ValidarLogin(correo, clave);

            if (usuario == null)
            {
                ViewBag.Error =
                    "Correo o contraseña incorrectos.";

                return View();
            }

            HttpContext.Session.SetInt32(
                "IdUsuario",
                usuario.IdUsuario
            );

            HttpContext.Session.SetString(
                "Nombre",
                usuario.Nombre
            );

            HttpContext.Session.SetString(
                "Apellido",
                usuario.Apellido
            );

            HttpContext.Session.SetString(
                "Rol",
                usuario.Rol
            );

            if (usuario.IdSucursal.HasValue)
            {
                HttpContext.Session.SetInt32(
                    "IdSucursal",
                    usuario.IdSucursal.Value
                );
            }

            if (usuario.Rol == "Propietario")
            {
                return RedirectToAction(
                    "Index",
                    "MisMascotas"
                );
            }

            if (usuario.Rol == "Veterinario" ||
                usuario.Rol == "AdministradorSucursal" ||
                usuario.Rol == "AdminCentral")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard"
                );
            }

            HttpContext.Session.Clear();

            ViewBag.Error =
                "El usuario no tiene un rol válido.";

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}