using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class ExplorarClinicasController : Controller
    {
        public IActionResult Index(string buscar, string pais, string especialidad)
        {
            string texto = buscar == null ? "" : buscar.Trim().ToLower();
            string filtroPais = string.IsNullOrWhiteSpace(pais) ? "Todos" : pais;
            string filtroEsp = string.IsNullOrWhiteSpace(especialidad) ? "Todas" : especialidad;

            List<Clinica> lista = new List<Clinica>();
            foreach (Clinica c in Datos.Clinicas)
            {
                bool coincideTexto = string.IsNullOrEmpty(texto)
                    || c.Nombre.ToLower().Contains(texto)
                    || c.Ciudad.ToLower().Contains(texto)
                    || c.Pais.ToLower().Contains(texto)
                    || c.Especialidad.ToLower().Contains(texto);

                bool coincidePais = filtroPais == "Todos" || c.Pais == filtroPais;
                bool coincideEsp = filtroEsp == "Todas" || c.Especialidad == filtroEsp;

                if (coincideTexto && coincidePais && coincideEsp)
                {
                    lista.Add(c);
                }
            }

            ViewBag.Buscar = buscar;
            ViewBag.Pais = filtroPais;
            ViewBag.Especialidad = filtroEsp;
            ViewBag.Paises = Datos.Clinicas.Select(c => c.Pais).Distinct().ToList();
            ViewBag.Especialidades = Datos.EspecialidadesRed;
            return View(lista);
        }

        public IActionResult Detalle(int id)
        {
            Clinica? clinica = Datos.Clinicas.FirstOrDefault(c => c.Id == id) ?? Datos.Clinicas.FirstOrDefault();
            if (clinica == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Veterinarios = Datos.Veterinarios.Where(v => v.Clinica == clinica.Nombre).ToList();
            ViewBag.Reseñas = Datos.Reseñas.Where(r => r.Clinica == clinica.Nombre).ToList();
            ViewBag.Especialidades = Datos.EspecialidadesRed;
            return View(clinica);
        }
    }
}
