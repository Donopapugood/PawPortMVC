using System;
using System.IO;
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

        public IActionResult Index()
        {
            return View(Datos.Mascotas);
        }

        [HttpPost]
        public IActionResult Registrar(string nombre, string especie, string raza, int edad, IFormFile foto)
        {
            if (string.IsNullOrWhiteSpace(nombre) || edad < 0)
            {
                ViewBag.MensajeClase = "msg-error";
                ViewBag.Mensaje = "Escribe el nombre y una edad válida.";
                return View("Index", Datos.Mascotas);
            }

            string imagen = "/recursos/img/mascota.jpg";
            if (foto != null && foto.Length > 0)
            {
                string extension = Path.GetExtension(foto.FileName).ToLower();
                if (extension != ".jpg" && extension != ".png")
                {
                    ViewBag.MensajeClase = "msg-error";
                    ViewBag.Mensaje = "La foto debe ser .jpg o .png.";
                    return View("Index", Datos.Mascotas);
                }

                string carpeta = Path.Combine(_entorno.WebRootPath, "recursos", "img");
                string archivo = Path.GetFileName(foto.FileName);
                using (FileStream flujo = new FileStream(Path.Combine(carpeta, archivo), FileMode.Create))
                {
                    foto.CopyTo(flujo);
                }
                imagen = "/recursos/img/" + archivo;
            }

            Datos.Mascotas.Add(new Mascota
            {
                Nombre = nombre,
                Especie = especie,
                Raza = raza,
                Edad = edad,
                Imagen = imagen,
                Vacunas = "Pendiente"
            });

            ViewBag.MensajeClase = "msg-ok";
            ViewBag.Mensaje = "Mascota registrada correctamente.";
            return View("Index", Datos.Mascotas);
        }
    }
}