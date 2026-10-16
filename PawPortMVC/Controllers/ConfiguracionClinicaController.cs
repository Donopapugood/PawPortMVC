using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class ConfiguracionClinicaController : Controller
    {
        public IActionResult Index(int? idClinica)
        {
            Clinica? seleccionada = idClinica.HasValue
                ? Datos.Clinicas.FirstOrDefault(c => c.Id == idClinica.Value)
                : Datos.Clinicas.FirstOrDefault();

            ViewBag.ClinicaActual = seleccionada;
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.Especialidades = Datos.EspecialidadesRed;
            ViewBag.Veterinarios = Datos.Veterinarios;
            return View();
        }

        [HttpPost]
        public IActionResult ActualizarClinica(int id, string nombre, string pais, string ciudad, string direccion, string telefono, string correo, string especialidad, string horarioLunesViernes, string descripcion)
        {
            Clinica? c = Datos.Clinicas.FirstOrDefault(x => x.Id == id);
            if (c != null && !string.IsNullOrWhiteSpace(nombre))
            {
                c.Nombre = nombre.Trim();
                c.Pais = string.IsNullOrWhiteSpace(pais) ? c.Pais : pais.Trim();
                c.Ciudad = string.IsNullOrWhiteSpace(ciudad) ? c.Ciudad : ciudad.Trim();
                c.Direccion = string.IsNullOrWhiteSpace(direccion) ? c.Direccion : direccion.Trim();
                c.Telefono = string.IsNullOrWhiteSpace(telefono) ? c.Telefono : telefono.Trim();
                c.Correo = string.IsNullOrWhiteSpace(correo) ? c.Correo : correo.Trim();
                c.Especialidad = string.IsNullOrWhiteSpace(especialidad) ? c.Especialidad : especialidad;
                c.HorarioLunesViernes = string.IsNullOrWhiteSpace(horarioLunesViernes) ? c.HorarioLunesViernes : horarioLunesViernes.Trim();
                c.Descripcion = string.IsNullOrWhiteSpace(descripcion) ? c.Descripcion : descripcion.Trim();
            }
            return RedirectToAction("Index", new { idClinica = id });
        }

        [HttpPost]
        public IActionResult RegistrarSucursal(string nombre, string pais, string ciudad, string direccion, string telefono, string correo, string especialidad, string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(ciudad))
            {
                Datos.InsertarSucursalSql(
                    nombre.Trim(),
                    string.IsNullOrWhiteSpace(direccion) ? ciudad.Trim() : direccion.Trim(),
                    string.IsNullOrWhiteSpace(telefono) ? "2200-0000" : telefono.Trim(),
                    string.IsNullOrWhiteSpace(correo) ? "contacto@pawport.example" : correo.Trim()
                );
                int nuevoId = Datos.Clinicas.Count > 0 ? Datos.Clinicas.Max(c => c.Id) : 1;
                return RedirectToAction("Index", new { idClinica = nuevoId });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarSucursal(int id)
        {
            if (Datos.Clinicas.Count > 1)
            {
                Clinica? c = Datos.Clinicas.FirstOrDefault(x => x.Id == id);
                if (c != null)
                {
                    Datos.Clinicas.Remove(c);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AgregarVeterinario(string nombre, string especialidad, string clinica, string correo, string telefono, string horario)
        {
            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(especialidad))
            {
                int nuevoId = Datos.Veterinarios.Count > 0 ? Datos.Veterinarios.Max(v => v.Id) + 1 : 1;
                Datos.Veterinarios.Add(new Veterinario
                {
                    Id = nuevoId,
                    Nombre = nombre.Trim(),
                    Especialidad = especialidad,
                    Clinica = string.IsNullOrWhiteSpace(clinica) ? "PawPort Central San Salvador" : clinica,
                    Correo = string.IsNullOrWhiteSpace(correo) ? "medico@pawport.example" : correo.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(telefono) ? "7000-0000" : telefono.Trim(),
                    Horario = string.IsNullOrWhiteSpace(horario) ? "Lunes a Viernes 08:00 - 16:00" : horario.Trim(),
                    Estado = "Activo"
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarVeterinario(int id, string nombre, string especialidad, string clinica, string horario)
        {
            Veterinario? vet = Datos.Veterinarios.FirstOrDefault(v => v.Id == id);
            if (vet != null && !string.IsNullOrWhiteSpace(nombre))
            {
                vet.Nombre = nombre.Trim();
                vet.Especialidad = especialidad;
                vet.Clinica = clinica;
                vet.Horario = string.IsNullOrWhiteSpace(horario) ? vet.Horario : horario.Trim();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarVeterinario(int id)
        {
            Veterinario? vet = Datos.Veterinarios.FirstOrDefault(v => v.Id == id);
            if (vet != null)
            {
                Datos.Veterinarios.Remove(vet);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AgregarEspecialidad(string nuevaEspecialidad)
        {
            if (!string.IsNullOrWhiteSpace(nuevaEspecialidad) && !Datos.EspecialidadesRed.Contains(nuevaEspecialidad.Trim()))
            {
                Datos.EspecialidadesRed.Add(nuevaEspecialidad.Trim());
            }
            return RedirectToAction("Index");
        }
    }
}
