using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class GestionCitasController : Controller
    {
        public IActionResult Index(string estado, string clinica, int? idSeleccionada)
        {
            string filtro = string.IsNullOrEmpty(estado) ? "Todas" : estado;
            string filtroClinica = string.IsNullOrEmpty(clinica) ? "Todas" : clinica;

            List<Cita> lista = new List<Cita>();
            foreach (Cita c in Datos.Citas)
            {
                bool pasaEstado = filtro == "Todas" || c.Estado == filtro;
                bool pasaClinica = filtroClinica == "Todas" || c.Clinica == filtroClinica;
                if (pasaEstado && pasaClinica)
                {
                    lista.Add(c);
                }
            }

            Cita? seleccionada = idSeleccionada.HasValue
                ? Datos.Citas.FirstOrDefault(c => c.Id == idSeleccionada.Value)
                : lista.FirstOrDefault();

            ViewBag.Estado = filtro;
            ViewBag.ClinicaFiltro = filtroClinica;
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.Mascotas = Datos.Mascotas;
            ViewBag.Veterinarios = Datos.Veterinarios;
            ViewBag.CitaSeleccionada = seleccionada;
            return View(lista);
        }

        [HttpPost]
        public IActionResult Crear(string mascota, string propietario, string clinica, string veterinario, string especialidad, string fecha, string hora, string motivo)
        {
            if (!string.IsNullOrWhiteSpace(mascota) && !string.IsNullOrWhiteSpace(motivo))
            {
                string f = string.IsNullOrWhiteSpace(fecha) ? DateTime.Today.ToString("yyyy-MM-dd") : fecha;
                string h = string.IsNullOrWhiteSpace(hora) ? "09:00" : hora;
                string cl = string.IsNullOrWhiteSpace(clinica) ? "PawPort Central San Salvador" : clinica;
                Datos.InsertarCitaSql(mascota.Trim(), cl, f, h, motivo.Trim());
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, string nuevoEstado, string estado)
        {
            Cita? cita = Datos.Citas.FirstOrDefault(c => c.Id == id);
            if (cita != null && !string.IsNullOrWhiteSpace(nuevoEstado))
            {
                cita.Estado = nuevoEstado;
                Datos.CambiarEstadoCitaSql(id, nuevoEstado);
            }
            return RedirectToAction("Index", new { estado = estado, idSeleccionada = id });
        }

        [HttpPost]
        public IActionResult Editar(int id, string fecha, string hora, string veterinario, string motivo, string estado)
        {
            Cita? cita = Datos.Citas.FirstOrDefault(c => c.Id == id);
            if (cita != null)
            {
                if (!string.IsNullOrWhiteSpace(fecha)) cita.Fecha = fecha;
                if (!string.IsNullOrWhiteSpace(hora)) cita.Hora = hora;
                if (!string.IsNullOrWhiteSpace(veterinario)) cita.Veterinario = veterinario;
                if (!string.IsNullOrWhiteSpace(motivo)) cita.Motivo = motivo.Trim();
            }
            return RedirectToAction("Index", new { estado = estado, idSeleccionada = id });
        }

        [HttpPost]
        public IActionResult Eliminar(int id, string estado)
        {
            Cita? cita = Datos.Citas.FirstOrDefault(c => c.Id == id);
            if (cita != null)
            {
                Datos.Citas.Remove(cita);
            }
            return RedirectToAction("Index", new { estado = estado });
        }
    }
}
