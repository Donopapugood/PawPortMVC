using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class PacientesController : Controller
    {
        public IActionResult Index(string buscar, string especie, string estado, int? idMascota)
        {
            string texto = buscar == null ? "" : buscar.Trim().ToLower();
            string filtroEspecie = string.IsNullOrWhiteSpace(especie) ? "Todas" : especie;
            string filtroEstado = string.IsNullOrWhiteSpace(estado) ? "Todos" : estado;

            List<Mascota> lista = new List<Mascota>();
            foreach (Mascota m in Datos.Mascotas)
            {
                bool pasaTexto = string.IsNullOrEmpty(texto)
                    || m.Nombre.ToLower().Contains(texto)
                    || m.Propietario.ToLower().Contains(texto)
                    || m.Raza.ToLower().Contains(texto);
                bool pasaEspecie = filtroEspecie == "Todas" || m.Especie == filtroEspecie;
                bool pasaEstado = filtroEstado == "Todos" || m.Estado == filtroEstado;

                if (pasaTexto && pasaEspecie && pasaEstado)
                {
                    lista.Add(m);
                }
            }

            Mascota? seleccionada = idMascota.HasValue
                ? Datos.Mascotas.FirstOrDefault(m => m.Id == idMascota.Value)
                : (lista.FirstOrDefault() ?? Datos.Mascotas.FirstOrDefault());

            ViewBag.Buscar = buscar;
            ViewBag.Especie = filtroEspecie;
            ViewBag.Estado = filtroEstado;
            ViewBag.MascotaSeleccionada = seleccionada;
            ViewBag.ExpedientesMascota = seleccionada != null
                ? Datos.Expedientes.Where(e => e.Mascota == seleccionada.Nombre).ToList()
                : new List<Expediente>();
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.Veterinarios = Datos.Veterinarios;
            ViewBag.Medicamentos = Datos.Inventario.Where(i => i.Categoria == "Medicamentos" || i.Categoria == "Vacunas").ToList();
            return View(lista);
        }

        [HttpPost]
        public IActionResult RegistrarExpediente(int idMascota, string clinica, string veterinario, string diagnostico, string tratamiento, string medicamentoRecetado, string vacunaAplicada, double pesoRegistrado, string notas, string estadoPaciente)
        {
            Mascota? mascota = Datos.Mascotas.FirstOrDefault(m => m.Id == idMascota);
            if (mascota != null && !string.IsNullOrWhiteSpace(diagnostico) && !string.IsNullOrWhiteSpace(tratamiento))
            {
                int nuevoId = Datos.Expedientes.Count > 0 ? Datos.Expedientes.Max(e => e.Id) + 1 : 1;
                Datos.Expedientes.Insert(0, new Expediente
                {
                    Id = nuevoId,
                    IdMascota = mascota.Id,
                    Mascota = mascota.Nombre,
                    Propietario = mascota.Propietario,
                    Clinica = string.IsNullOrWhiteSpace(clinica) ? "Veterinaria Escalón Central" : clinica,
                    Veterinario = string.IsNullOrWhiteSpace(veterinario) ? "Dr. Carlos Mendoza" : veterinario,
                    Fecha = DateTime.Today.ToString("dd/MM/yyyy"),
                    Diagnostico = diagnostico.Trim(),
                    Tratamiento = tratamiento.Trim(),
                    MedicamentoRecetado = string.IsNullOrWhiteSpace(medicamentoRecetado) ? "Ninguno" : medicamentoRecetado,
                    VacunaAplicada = string.IsNullOrWhiteSpace(vacunaAplicada) ? "Ninguna" : vacunaAplicada.Trim(),
                    PesoRegistrado = pesoRegistrado > 0 ? pesoRegistrado : mascota.Peso,
                    Notas = string.IsNullOrWhiteSpace(notas) ? "" : notas.Trim()
                });

                if (pesoRegistrado > 0) mascota.Peso = pesoRegistrado;
                mascota.UltimaVisita = DateTime.Today.ToString("dd/MM/yyyy");
                if (!string.IsNullOrWhiteSpace(vacunaAplicada)) mascota.Vacunas = "Al día";
                if (!string.IsNullOrWhiteSpace(estadoPaciente)) mascota.Estado = estadoPaciente;
            }

            return RedirectToAction("Index", new { idMascota = idMascota });
        }

        [HttpPost]
        public IActionResult EliminarExpediente(int id, int idMascota)
        {
            Expediente? exp = Datos.Expedientes.FirstOrDefault(e => e.Id == id);
            if (exp != null)
            {
                Datos.Expedientes.Remove(exp);
            }
            return RedirectToAction("Index", new { idMascota = idMascota });
        }
    }
}
