using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;

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
            ViewData["TituloPaso"] = "Análisis de rentabilidad";
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];

            return View(productos);
        }
    }
}