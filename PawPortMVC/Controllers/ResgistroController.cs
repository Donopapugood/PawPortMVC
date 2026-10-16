using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class RegistroController : Controller
    {
        private readonly UsuarioDatos _usuarioDatos;

        public RegistroController(UsuarioDatos usuarioDatos)
        {
            _usuarioDatos = usuarioDatos;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string nombre, string apellido, string correo, string telefono, string rol, string clave, string clave2, bool terminos)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Completa nombre, correo y contraseña.";
                return View();
            }

            if (clave.Length < 4)
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "La contraseña debe tener al menos 4 caracteres.";
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
                ViewBag.Mensaje = "Debes aceptar los términos y condiciones.";
                return View();
            }

            string rolElegido = string.IsNullOrWhiteSpace(rol) ? "Propietario" : rol;
            bool guardadoEnSql = _usuarioDatos.RegistrarUsuario(nombre, apellido, correo, telefono, rolElegido, clave, out string errorSql);

            if (!guardadoEnSql && errorSql.Contains("Ya existe"))
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = errorSql;
                return View();
            }

            bool existeRespaldo = Datos.UsuariosRespaldo.Any(u => u.Usuario.CorreoElectronico.Equals(correo.Trim(), System.StringComparison.OrdinalIgnoreCase));
            if (!existeRespaldo)
            {

                int nuevoId = Datos.UsuariosRespaldo.Count + 1;
                Datos.UsuariosRespaldo.Add((new Usuario
                {
                    IdUsuario = nuevoId,
                    IdSucursal = rolElegido == "Propietario" ? null : 1,
                    Nombre = nombre.Trim(),
                    Apellido = string.IsNullOrWhiteSpace(apellido) ? "" : apellido.Trim(),
                    CorreoElectronico = correo.Trim(),
                    Rol = rolElegido,
                    Estado = true
                }, clave));
            }

            ViewBag.MensajeClase = "msg-ok";
            ViewBag.Mensaje = "¡Cuenta creada exitosamente en PawPortDB para " + nombre + "! Ya puedes iniciar sesión con tu correo.";
            return View();
        }
    }
}
