using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;

namespace PawPortMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("Nombre") ?? "Dr. Carlos Mendoza";
            ViewBag.RolUsuario = HttpContext.Session.GetString("Rol") ?? "AdministradorSucursal";
            ViewBag.TotalCitasHoy = Datos.Citas.Count;
            ViewBag.TotalPacientes = Datos.Mascotas.Count;
            ViewBag.CalificacionPromedio = Datos.Clinicas.Count > 0 ? Datos.Clinicas.Average(c => c.Calificacion).ToString("0.0") : "4.8";
            ViewBag.TotalVeterinarios = Datos.Veterinarios.Count;
            ViewBag.Resenas = Datos.Resenas.Take(3).ToList();
            ViewBag.AlertasStock = Datos.Inventario.Where(p => p.StockActual <= p.StockMinimo).ToList();
            return View(Datos.Citas);
        }
    }
}