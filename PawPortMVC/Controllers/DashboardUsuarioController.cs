using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class DashboardUsuarioController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre") ?? "Alejandra";
            ViewBag.Clinicas = Datos.Clinicas.Take(3).ToList();
            ViewBag.Mascotas = Datos.Mascotas;
            ViewBag.Citas = Datos.Citas;
            ViewBag.Recordatorios = Datos.Recordatorios.Where(r => !r.Completado).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CompletarRecordatorio(int id)
        {
            Recordatorio? rec = Datos.Recordatorios.FirstOrDefault(r => r.Id == id);
            if (rec != null)
            {
                rec.Completado = true;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AgregarRecordatorio(string mascota, string tipo, string mensaje, string fechaLimite)
        {
            if (!string.IsNullOrWhiteSpace(mascota) && !string.IsNullOrWhiteSpace(mensaje))
            {
                int nuevoId = Datos.Recordatorios.Count > 0 ? Datos.Recordatorios.Max(r => r.Id) + 1 : 1;
                Datos.Recordatorios.Add(new Recordatorio
                {
                    Id = nuevoId,
                    Mascota = mascota,
                    Propietario = HttpContext.Session.GetString("Nombre") ?? "Alejandra Gómez",
                    Tipo = string.IsNullOrWhiteSpace(tipo) ? "Vacuna" : tipo,
                    Mensaje = mensaje.Trim(),
                    FechaLimite = string.IsNullOrWhiteSpace(fechaLimite) ? "2026-11-01" : fechaLimite,
                    Completado = false
                });
            }
            return RedirectToAction("Index");
        }
    }
}
