using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PawPortMVC.DAL;
using PawPortMVC.Models;

namespace PawPortMVC.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult Index(string categoria, string clinica)
        {
            string filtroCat = string.IsNullOrWhiteSpace(categoria) ? "Todos" : categoria;
            string filtroClinica = string.IsNullOrWhiteSpace(clinica) ? "Todas" : clinica;

            List<ProductoInventario> lista = new List<ProductoInventario>();
            foreach (ProductoInventario p in Datos.Inventario)
            {
                bool pasaCat = filtroCat == "Todos" || p.Categoria == filtroCat;
                bool pasaClinica = filtroClinica == "Todas" || p.Clinica == filtroClinica;
                if (pasaCat && pasaClinica)
                {
                    lista.Add(p);
                }
            }

            ViewBag.Categoria = filtroCat;
            ViewBag.ClinicaFiltro = filtroClinica;
            ViewBag.Clinicas = Datos.Clinicas;
            ViewBag.Compras = Datos.Compras;
            ViewBag.TotalProductos = Datos.Inventario.Sum(i => i.StockActual);
            ViewBag.ProductosBajoStock = Datos.Inventario.Count(i => i.StockActual > 0 && i.StockActual <= i.StockMinimo);
            ViewBag.ProductosAgotados = Datos.Inventario.Count(i => i.StockActual <= 0);
            ViewBag.ValorInventario = Datos.Inventario.Sum(i => i.StockActual * i.Precio);
            return View(lista);
        }

        [HttpPost]
        public IActionResult Agregar(string nombre, string categoria, string clinica, int stockActual, int stockMinimo, decimal precio, string proveedor, string fechaCaducidad)
        {
            if (!string.IsNullOrWhiteSpace(nombre) && precio > 0 && stockActual >= 0)
            {
                Datos.InsertarProductoSql(
                    nombre.Trim(),
                    string.IsNullOrWhiteSpace(categoria) ? "Medicamentos" : categoria,
                    string.IsNullOrWhiteSpace(clinica) ? "PawPort Central San Salvador" : clinica,
                    stockActual,
                    stockMinimo >= 0 ? stockMinimo : 5,
                    precio,
                    string.IsNullOrWhiteSpace(fechaCaducidad) ? "2027-12-31" : fechaCaducidad
                );
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Editar(int id, string nombre, string categoria, int stockActual, int stockMinimo, decimal precio)
        {
            ProductoInventario? prod = Datos.Inventario.FirstOrDefault(i => i.Id == id);
            if (prod != null && !string.IsNullOrWhiteSpace(nombre) && stockActual >= 0 && precio > 0)
            {
                prod.Nombre = nombre.Trim();
                prod.Categoria = categoria;
                prod.StockActual = stockActual;
                prod.StockMinimo = stockMinimo;
                prod.Precio = precio;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            ProductoInventario? prod = Datos.Inventario.FirstOrDefault(i => i.Id == id);
            if (prod != null)
            {
                Datos.Inventario.Remove(prod);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RegistrarCompra(int idProducto, string proveedor, int cantidad, decimal costoUnitario)
        {
            ProductoInventario? prod = Datos.Inventario.FirstOrDefault(i => i.Id == idProducto);
            if (prod != null && cantidad > 0 && costoUnitario > 0)
            {

                int idCompra = Datos.Compras.Count > 0 ? Datos.Compras.Max(c => c.Id) + 1 : 1;
                Datos.Compras.Insert(0, new CompraInventario
                {
                    Id = idCompra,
                    Fecha = DateTime.Today.ToString("yyyy-MM-dd"),
                    Producto = prod.Nombre,
                    Proveedor = string.IsNullOrWhiteSpace(proveedor) ? prod.Proveedor : proveedor.Trim(),
                    Clinica = prod.Clinica,
                    Cantidad = cantidad,
                    CostoUnitario = costoUnitario
                });
                Datos.ActualizarStockCompraSql(idProducto, cantidad);
            }
            return RedirectToAction("Index");
        }
    }
}
