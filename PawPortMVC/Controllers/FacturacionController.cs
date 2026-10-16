using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class FacturacionController : Controller
    {
        public IActionResult Index(string estado)
        {
            string rol = HttpContext.Session.GetString("Rol") ?? "Propietario";
            string filtro = string.IsNullOrWhiteSpace(estado) ? "Todas" : estado;

            List<Factura> lista = Datos.Facturas
                .Where(f => filtro == "Todas" || f.Estado == filtro)
                .ToList();

            ViewBag.Rol = rol;
            ViewBag.Estado = filtro;
            ViewBag.Mascotas = Datos.Mascotas;
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.TotalCobrado = Datos.Facturas.Where(f => f.Estado == "Pagada").Sum(f => f.Monto);
            ViewBag.TotalPendiente = Datos.Facturas.Where(f => f.Estado == "Pendiente").Sum(f => f.Monto);
            return View(lista);
        }

        [HttpPost]
        public IActionResult Emitir(string propietario, string mascota, string clinica, string concepto, decimal monto, string metodoPago, string estado)
        {
            if (!string.IsNullOrWhiteSpace(propietario) && !string.IsNullOrWhiteSpace(concepto) && monto > 0)
            {
                Datos.InsertarFacturaSql(
                    string.IsNullOrWhiteSpace(clinica) ? "PawPort Central San Salvador" : clinica,
                    monto,
                    string.IsNullOrWhiteSpace(estado) ? "Pendiente" : estado
                );
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, string nuevoEstado)
        {
            Factura? fac = Datos.Facturas.FirstOrDefault(f => f.Id == id);
            if (fac != null && !string.IsNullOrWhiteSpace(nuevoEstado))
            {
                fac.Estado = nuevoEstado;
                Datos.CambiarEstadoFacturaSql(id, nuevoEstado);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            Factura? fac = Datos.Facturas.FirstOrDefault(f => f.Id == id);
            if (fac != null)
            {
                Datos.Facturas.Remove(fac);
            }
            return RedirectToAction("Index");
        }
    }
}
