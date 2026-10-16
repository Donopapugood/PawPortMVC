using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Index(string mascota, string propietario, string clinica, string veterinario, string especialidad, string fecha, string hora, string motivo, string notas)
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

            if (string.IsNullOrWhiteSpace(mascota) || string.IsNullOrWhiteSpace(clinica) || string.IsNullOrWhiteSpace(motivo))
            {
                ViewBag.ResumenClase = "msg-error";
                ViewBag.Resumen = "Completa la mascota, la clínica y el motivo de la consulta.";
                return View();
            }

            Mascota? objMascota = Datos.Mascotas.FirstOrDefault(m => m.Nombre == mascota);
            string nombrePropietario = !string.IsNullOrWhiteSpace(propietario)
                ? propietario.Trim()
                : (objMascota != null ? objMascota.Propietario : (HttpContext.Session.GetString("Nombre") ?? "Maria Hernandez"));

            int nuevoId = Datos.Citas.Count > 0 ? Datos.Citas.Max(c => c.Id) + 1 : 1;
            Datos.Citas.Add(new Cita
            {
                Id = nuevoId,
                Fecha = fechaCita.ToString("yyyy-MM-dd"),
                Hora = hora,
                Mascota = mascota,
                Especie = objMascota != null ? objMascota.Especie : "Perro",
                Propietario = nombrePropietario,
                Clinica = clinica,
                Veterinario = string.IsNullOrWhiteSpace(veterinario) ? "Dr(a). Ana Lopez" : veterinario,
                Especialidad = especialidad,
                Motivo = motivo.Trim(),
                Notas = string.IsNullOrWhiteSpace(notas) ? "" : notas.Trim(),
                Estado = "Programada"
            });

            int idRec = Datos.Recordatorios.Count > 0 ? Datos.Recordatorios.Max(r => r.Id) + 1 : 1;
            Datos.Recordatorios.Add(new Recordatorio
            {
                Id = idRec,
                Mascota = mascota,
                Propietario = nombrePropietario,
                Tipo = "Cita",
                Mensaje = "Cita de " + especialidad + " en " + clinica + " a las " + hora + ".",
                FechaLimite = fechaCita.ToString("yyyy-MM-dd"),
                Completado = false
            });

            Datos.InsertarCitaSql(mascota, clinica, fechaCita.ToString("yyyy-MM-dd"), hora, motivo.Trim());
            ViewBag.ResumenClase = "msg-ok";
            ViewBag.Resumen = "¡Cita registrada en PawPortDB! Solicitud #" + nuevoId + " para " + mascota + " en " + clinica + " (" + especialidad + ") el "
                + fechaCita.ToString("dd/MM/yyyy") + " a las " + hora + ".";
            return View();
        }

        private void CargarListas()
        {
            ViewBag.Mascotas = Datos.Mascotas;
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.Veterinarios = Datos.Veterinarios;
            ViewBag.Especialidades = Datos.EspecialidadesRed;
        }
    }
}
