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
            string rol = HttpContext.Session.GetString("Rol") ?? "AdminCentral";
            string nombre = HttpContext.Session.GetString("Nombre") ?? "Administrador";

            ViewBag.NombreUsuario = nombre;
            ViewBag.RolUsuario = rol;
            ViewBag.TotalCitasHoy = Datos.Citas.Count;
            ViewBag.TotalPacientes = Datos.Mascotas.Count;
            ViewBag.CalificacionPromedio = Datos.Clinicas.Count > 0 ? Datos.Clinicas.Average(c => c.Calificacion).ToString("0.0") : "4.8";
            ViewBag.TotalVeterinarios = Datos.Veterinarios.Count;
            ViewBag.TotalClinicas = Datos.Clinicas.Count;
            ViewBag.IngresosRed = Datos.Facturas.Where(f => f.Estado == "Pagada").Sum(f => f.Monto);
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.Expedientes = Datos.Expedientes;
            ViewBag.Facturas = Datos.Facturas;
            ViewBag.Reseñas = Datos.Reseñas.Take(3).ToList();
            ViewBag.AlertasStock = Datos.Inventario.Where(p => p.StockActual <= p.StockMinimo).ToList();
            return View(Datos.Citas);
        }
    }
}
