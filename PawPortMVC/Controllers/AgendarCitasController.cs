using System;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class AgendarCitaController : Controller
    {
        public IActionResult Index(string clinica)
        {
            CargarListas();
            ViewBag.ClinicaElegida = clinica;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string mascota, string clinica, string especialidad, string fecha, string hora, string motivo)
        {
            CargarListas();
            ViewBag.ClinicaElegida = clinica;

            DateTime fechaCita;
            if (!DateTime.TryParse(fecha, out fechaCita) || fechaCita.Date < DateTime.Today)
            {
                ViewBag.ResumenClase = "msg-error";
                ViewBag.Resumen = "Elige una fecha válida (hoy o posterior).";
                return View();
            }
            if (string.IsNullOrWhiteSpace(motivo))
            {
                ViewBag.ResumenClase = "msg-error";
                ViewBag.Resumen = "Escribe el motivo de la consulta.";
                return View();
            }

            ViewBag.ResumenClase = "msg-ok";
            ViewBag.Resumen = "Cita solicitada para " + mascota + " en " + clinica + " (" + especialidad + ") el "
                + fechaCita.ToString("dd/MM/yyyy") + " a las " + hora + ".";
            return View();
        }

        private void CargarListas()
        {
            ViewBag.Mascotas = Datos.Mascotas;
            ViewBag.Clinicas = Datos.Clinicas;
        }
    }
}