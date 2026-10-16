using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;

namespace PawPortMVC.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index(string clinica)
        {
            string filtro = string.IsNullOrWhiteSpace(clinica) ? "Todas" : clinica;

            var citasFiltradas = filtro == "Todas"
                ? Datos.Citas
                : Datos.Citas.Where(c => c.Clinica == filtro).ToList();

            var facturasFiltradas = filtro == "Todas"
                ? Datos.Facturas
                : Datos.Facturas.Where(f => f.Clinica == filtro).ToList();

            ViewBag.ClinicaFiltro = filtro;
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.IngresosTotales = facturasFiltradas.Where(f => f.Estado == "Pagada").Sum(f => f.Monto);
            ViewBag.ConsultasRealizadas = citasFiltradas.Count;
            ViewBag.TotalMascotas = Datos.Mascotas.Count;
            ViewBag.TotalExpedientes = Datos.Expedientes.Count;
            ViewBag.TotalPerros = Datos.Mascotas.Count(m => m.Especie == "Perro");
            ViewBag.TotalGatos = Datos.Mascotas.Count(m => m.Especie == "Gato");
            ViewBag.TotalOtros = Datos.Mascotas.Count(m => m.Especie != "Perro" && m.Especie != "Gato");
            ViewBag.Facturas = facturasFiltradas;
            return View();
        }

        public IActionResult ExportarCsv(string tipo)
        {
            StringBuilder sb = new StringBuilder();
            string nombreArchivo = "reporte_pawport.csv";

            if (tipo == "inventario")
            {
                nombreArchivo = "reporte_inventario_pawport.csv";
                sb.AppendLine("ID,Producto,Categoria,Clinica,StockActual,StockMinimo,PrecioUSD,Estado");
                foreach (var p in Datos.Inventario)
                {
                    sb.AppendLine($"{p.Id},\"{p.Nombre}\",{p.Categoria},\"{p.Clinica}\",{p.StockActual},{p.StockMinimo},{p.Precio},{p.EstadoStock}");
                }
            }
            else if (tipo == "financiero")
            {
                nombreArchivo = "reporte_financiero_pawport.csv";
                sb.AppendLine("Factura,Fecha,Propietario,Mascota,Clinica,Concepto,MontoUSD,MetodoPago,Estado");
                foreach (var f in Datos.Facturas)
                {
                    sb.AppendLine($"{f.NumeroFactura},{f.Fecha},\"{f.Propietario}\",\"{f.Mascota}\",\"{f.Clinica}\",\"{f.Concepto}\",{f.Monto},{f.MetodoPago},{f.Estado}");
                }
            }
            else
            {
                nombreArchivo = "reporte_general_clinicas.csv";
                sb.AppendLine("ID,Clinica,Pais,Ciudad,Especialidad,Calificacion,Reseñas");
                foreach (var c in Datos.Clinicas)
                {
                    sb.AppendLine($"{c.Id},\"{c.Nombre}\",\"{c.Pais}\",\"{c.Ciudad}\",\"{c.Especialidad}\",{c.Calificacion},{c.TotalReseñas}");
                }
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", nombreArchivo);
        }
    }
}
