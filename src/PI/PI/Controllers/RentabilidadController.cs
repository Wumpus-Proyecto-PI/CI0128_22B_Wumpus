using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using PI.Services;

namespace PI.Controllers
{
    public class RentabilidadController : ManejadorUsuariosController
    {
        // // Retorna una lista con todos los modelos de producto existentes y el título del paso.
        public IActionResult Index(string fecha)
        {
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            AnalisisHandler analisisHandler = new AnalisisHandler();
            ProductoHandler productoHandler = new ProductoHandler();
            List<ProductoModel> productos = productoHandler.ObtenerProductos(fechaConversion);
            AnalisisModel model = analisisHandler.ObtenerUnAnalisis(ViewBag.fechaAnalisis);
            ViewBag.AnalisisActual = model;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            ViewBag.MontoGastosFijosMensuales = AnalisisRentabilidadService.CalcularGastosFijosTotalesMensuales(gastoFijoHandler.obtenerTotalAnual(fechaConversion));
            // para que se muestre el boton de volver al analisis
            ViewBag.BotonRetorno = "Progreso";

            // asignamos el nombre del negocio en la vista
            // este se extrae de la base de datos con la fecha del análisis
            ViewData["NombreNegocio"] = gastoFijoHandler.obtenerNombreNegocio(fechaConversion);

            ViewData["TituloPaso"] = "Análisis de rentabilidad";
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];

            return View(productos);
        }
    }
}