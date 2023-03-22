using Microsoft.AspNetCore.Mvc;
using PI.EntityModels;
using PI.EntityHandlers;
using PI.Services;

namespace PI.Controllers
{
    public class RentabilidadController : ManejadorUsuariosController
    {

        private ProductoHandler? ProductoHandler = null;

        public RentabilidadController( ProductoHandler productoHandler )
        {
            ProductoHandler = productoHandler;
        }

        // // Retorna una lista con todos los modelos de producto existentes y el título del paso.
        public async Task<IActionResult> Index(string fecha)
        {
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;


            List<Producto> productos = await ProductoHandler.ObtenerProductosAsync(fechaConversion);
            Analisis model = await ProductoHandler.ObtenerUnAnalisis(ViewBag.fechaAnalisis);
            ViewBag.AnalisisActual = model;

            decimal totalAnualGastosFijos = await ProductoHandler.ObtenerTotalAnualAsync(fechaConversion);
            ViewBag.MontoGastosFijosMensuales = AnalisisRentabilidadService.CalcularGastosFijosTotalesMensuales(totalAnualGastosFijos);
            // para que se muestre el boton de volver al analisis
            ViewBag.BotonRetorno = "Progreso";

            // asignamos el nombre del negocio en la vista
            // este se extrae de la base de datos con la fecha del análisis
            ViewData["NombreNegocio"] = await ProductoHandler.ObtenerNombreNegocioAsync(fechaConversion);

            ViewData["TituloPaso"] = "Análisis de rentabilidad";
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];

            return View(productos);
        }
    }
}