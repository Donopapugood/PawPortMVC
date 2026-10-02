using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class ExplorarClinicasController : Controller
    {
        public IActionResult Index(string buscar)
        {
            string texto = buscar == null ? "" : buscar.Trim().ToLower();

            List<Clinica> lista = new List<Clinica>();
            foreach (Clinica c in Datos.Clinicas)
            {
                if (c.Nombre.ToLower().Contains(texto) || c.Ciudad.ToLower().Contains(texto))
                    lista.Add(c);
            }

            ViewBag.Buscar = buscar;
            return View(lista);
        }
    }
}