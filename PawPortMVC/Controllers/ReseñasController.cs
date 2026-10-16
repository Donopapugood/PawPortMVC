using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class ReseñasController : Controller
    {
        public IActionResult Index(string pais)
        {
            string filtroPais = string.IsNullOrWhiteSpace(pais) ? "Todos" : pais;
            var lista = filtroPais == "Todos"
                ? Datos.Resenas
                : Datos.Resenas.Where(r => r.Pais == filtroPais).ToList();

            ViewBag.Pais = filtroPais;
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.Promedio = Datos.Resenas.Count > 0 ? Datos.Resenas.Average(r => r.Calificacion).ToString("0.0") : "4.8";
            ViewBag.Total = Datos.Resenas.Count;
            return View(lista);
        }

        [HttpPost]
        public IActionResult Publicar(string autor, string pais, string clinica, int calificacion, string comentario)
        {
            if (!string.IsNullOrWhiteSpace(comentario))
            {
                int nuevoId = Datos.Resenas.Count > 0 ? Datos.Resenas.Max(r => r.Id) + 1 : 1;
                string nombreAutor = !string.IsNullOrWhiteSpace(autor)
                    ? autor.Trim()
                    : (HttpContext.Session.GetString("Nombre") ?? "Propietario Verificado");

                Datos.Resenas.Insert(0, new Reseña
                {
                    Id = nuevoId,
                    Autor = nombreAutor,
                    Pais = string.IsNullOrWhiteSpace(pais) ? "El Salvador" : pais.Trim(),
                    Clinica = string.IsNullOrWhiteSpace(clinica) ? "Veterinaria Escalón Central" : clinica,
                    Calificacion = calificacion >= 1 && calificacion <= 5 ? calificacion : 5,
                    Comentario = comentario.Trim(),
                    Fecha = "Hoy"
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            Reseña? r = Datos.Resenas.FirstOrDefault(x => x.Id == id);
            if (r != null)
            {
                Datos.Resenas.Remove(r);
            }
            return RedirectToAction("Index");
        }
    }
}