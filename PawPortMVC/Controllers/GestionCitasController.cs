using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class GestionCitasController : Controller
    {
        public IActionResult Index(string estado)
        {
            string filtro = string.IsNullOrEmpty(estado) ? "Todas" : estado;

            List<Cita> lista = new List<Cita>();
            foreach (Cita c in Datos.Citas)
            {
                if (filtro == "Todas" || c.Estado == filtro)
                    lista.Add(c);
            }

            ViewBag.Estado = filtro;
            return View(lista);
        }

        [HttpPost]
        public IActionResult Confirmar(int id, string estado)
        {
            foreach (Cita c in Datos.Citas)
            {
                if (c.Id == id)
                    c.Estado = "Confirmada";
            }
            return RedirectToAction("Index", new { estado = estado });
        }

        [HttpPost]
        public IActionResult Cancelar(int id, string estado)
        {
            foreach (Cita c in Datos.Citas)
            {
                if (c.Id == id)
                    c.Estado = "Cancelada";
            }
            return RedirectToAction("Index", new { estado = estado });
        }
    }
}