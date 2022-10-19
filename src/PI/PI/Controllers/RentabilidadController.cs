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
            ProductoHandler productoHandler = new ProductoHandler();
            List<ProductoModel> productos = productoHandler.obtenerProductos(fechaConversion);
            return View(productos);
        }
    }
}