using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using PI.Services;

namespace PI.Controllers
{
    public class RentabilidadController : Controller
    {
        public IActionResult Index(string fecha)
        {
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            AnalisisHandler analisisHandler = new AnalisisHandler();
            ProductoHandler productoHandler = new ProductoHandler();
            List<ProductoModel> productos = productoHandler.obtenerProductos(fechaConversion);
            AnalisisModel model = analisisHandler.ObtenerUnAnalisis(ViewBag.fechaAnalisis);
            ViewBag.AnalisisActual = model;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            EstructuraOrgHandler estructuraOrgHandler = new EstructuraOrgHandler();

            ViewBag.MontoGastosFijos = AnalisisRentabilidadService.CalcularGastosFijos(
                gastoFijoHandler.ObtenerGastosFijos(ViewBag.fechaAnalisis), estructuraOrgHandler.ObtenerListaDePuestos(ViewBag.fechaAnalisis));

            ViewData["TituloPaso"] = "Análisis de rentabilidad";
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];

            return View(productos);
        }
    }
}