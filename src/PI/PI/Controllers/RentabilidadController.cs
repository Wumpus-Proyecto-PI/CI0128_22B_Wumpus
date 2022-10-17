using Microsoft.AspNetCore.Mvc;
using PI.Models;

namespace PI.Controllers
{
    public class RentabilidadController : Controller
    {
        public IActionResult Index(string fecha)
        {
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;
            ViewBag.porcentajeVentas = "23";
            var productos = GetListOfProducts();
            return View(productos);
        }

        private List<ProductoModel> GetListOfProducts()
        {
            List<ProductoModel> productos = new List<ProductoModel>();
            productos.Add(new ProductoModel
            {
                nombre = "Empanadas de carne",
                porcentajeVentas = "33%",
                precio = "250"
            });

            productos.Add(new ProductoModel
            {
                nombre = "Empanadas de pollo",
                porcentajeVentas = "33%",
                precio = "250"
            });

            productos.Add(new ProductoModel
            {
                nombre = "Empanadas de queso",
                porcentajeVentas = "33%",
                precio = "250"
            });
            return productos;
        }
    }
}