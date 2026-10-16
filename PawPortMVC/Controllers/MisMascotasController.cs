using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class MisMascotasController : Controller
    {
        private readonly IWebHostEnvironment _entorno;

        public MisMascotasController(IWebHostEnvironment entorno)
        {
            _entorno = entorno;
        }

        public IActionResult Index(int? idMascota)
        {
            Mascota? seleccionada = idMascota.HasValue
                ? Datos.Mascotas.FirstOrDefault(m => m.Id == idMascota.Value)
                : Datos.Mascotas.FirstOrDefault();

            ViewBag.MascotaSeleccionada = seleccionada;
            ViewBag.Expedientes = seleccionada != null
                ? Datos.Expedientes.Where(e => e.Mascota == seleccionada.Nombre).ToList()
                : Datos.Expedientes;
            ViewBag.Recordatorios = Datos.Recordatorios.Where(r => !r.Completado).ToList();
            return View(Datos.Mascotas);
        }

        [HttpPost]
        public IActionResult Registrar(string nombre, string especie, string raza, int edad, double peso, string sexo, string propietario, string alergias, IFormFile? foto)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(raza) || edad < 0 || peso <= 0)
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Completa nombre, raza, una edad válida y peso mayor a 0.";
                CargarVistaPorDefecto();
                return View("Index", Datos.Mascotas);
            }

            string imagen = "/recursos/Img/Patitas.png";
            if (foto != null && foto.Length > 0)
            {
                string extension = Path.GetExtension(foto.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    ViewBag.MensajeClase = "msg-error";
                    ViewBag.Mensaje = "La fotografía debe estar en formato .jpg o .png.";
                    CargarVistaPorDefecto();
                    return View("Index", Datos.Mascotas);
                }

                string carpeta = Path.Combine(_entorno.WebRootPath, "recursos", "Img");
                Directory.CreateDirectory(carpeta);
                string archivo = Path.GetFileName(foto.FileName);
                using (FileStream flujo = new FileStream(Path.Combine(carpeta, archivo), FileMode.Create))
                {
                    foto.CopyTo(flujo);
                }
                imagen = "/recursos/Img/" + archivo;
            }

            int nuevoId = Datos.Mascotas.Count > 0 ? Datos.Mascotas.Max(m => m.Id) + 1 : 1;
            string dueno = !string.IsNullOrWhiteSpace(propietario)
                ? propietario.Trim()
                : (HttpContext.Session.GetString("Nombre") != null
                    ? HttpContext.Session.GetString("Nombre") + " " + HttpContext.Session.GetString("Apellido")
                    : "Maria Hernandez");

            Datos.Mascotas.Add(new Mascota
            {
                Id = nuevoId,
                Nombre = nombre.Trim(),
                Especie = especie,
                Raza = raza.Trim(),
                Edad = edad,
                Peso = peso,
                Sexo = string.IsNullOrWhiteSpace(sexo) ? "Hembra" : sexo,
                Propietario = dueno.Trim(),
                Imagen = imagen,
                Vacunas = "Pendiente",
                Estado = "Activo",
                UltimaVisita = DateTime.Today.ToString("dd/MM/yyyy"),
                Alergias = string.IsNullOrWhiteSpace(alergias) ? "Ninguna registrada" : alergias.Trim()
            });

            Datos.InsertarMascotaSql(nombre, especie, raza, edad, peso, sexo ?? "Hembra", "Variado");
            ViewBag.MensajeClase = "msg-ok";
            ViewBag.Mensaje = "Mascota " + nombre + " registrada correctamente en PawPortDB.";
            CargarVistaPorDefecto();
            return View("Index", Datos.Mascotas);
        }

        [HttpPost]
        public IActionResult Editar(int id, string nombre, string especie, string raza, int edad, double peso, string vacunas, string alergias)
        {
            Mascota? mascota = Datos.Mascotas.FirstOrDefault(m => m.Id == id);
            if (mascota != null && !string.IsNullOrWhiteSpace(nombre) && edad >= 0 && peso > 0)
            {
                mascota.Nombre = nombre.Trim();
                mascota.Especie = especie;
                mascota.Raza = string.IsNullOrWhiteSpace(raza) ? mascota.Raza : raza.Trim();
                mascota.Edad = edad;
                mascota.Peso = peso;
                mascota.Vacunas = vacunas;
                mascota.Alergias = string.IsNullOrWhiteSpace(alergias) ? "Ninguna registrada" : alergias.Trim();
                ViewBag.MensajeClase = "msg-ok";
                ViewBag.Mensaje = "Datos de " + mascota.Nombre + " actualizados correctamente.";
            }
            else
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Verifica que los datos para actualizar sean válidos.";
            }

            CargarVistaPorDefecto();
            return View("Index", Datos.Mascotas);
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            Mascota? mascota = Datos.Mascotas.FirstOrDefault(m => m.Id == id);
            if (mascota != null)
            {
                Datos.Mascotas.Remove(mascota);
                ViewBag.MensajeClase = "msg-ok";
                ViewBag.Mensaje = "Mascota eliminada correctamente.";
            }
            CargarVistaPorDefecto();
            return View("Index", Datos.Mascotas);
        }

        private void CargarVistaPorDefecto()
        {
            Mascota? seleccionada = Datos.Mascotas.FirstOrDefault();
            ViewBag.MascotaSeleccionada = seleccionada;
            ViewBag.Expedientes = seleccionada != null
                ? Datos.Expedientes.Where(e => e.Mascota == seleccionada.Nombre).ToList()
                : Datos.Expedientes;
            ViewBag.Recordatorios = Datos.Recordatorios.Where(r => !r.Completado).ToList();
        }
    }
}
