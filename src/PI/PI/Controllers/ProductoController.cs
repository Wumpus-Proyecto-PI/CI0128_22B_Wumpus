using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;

namespace PI.Controllers
{
    public class ProductoController : ManejadorUsuariosController
    {
        // GET: GastosVariables
        public IActionResult Index(string fecha)
        {
            // para que se muestre el boton de volver al analisis
            ViewBag.BotonRetorno = "Progreso";

            // asignamos el título del paso en el que se está en el layout
            ViewData["TituloPaso"] = "Gastos variables";

            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];

            ProductoHandler productoHandler = new ProductoHandler();

            // convertimos a Datetime la fecha del análisis porque de esta forma lo utiliza la vista
            DateTime fechaAnalisis = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            // enviamos la fecha a la vista con viewbag
            ViewBag.FechaAnalisis = fechaAnalisis;

            // asignamos el nombre del negocio en la vista
            // este se extrae de la base de datos con la fecha del análisis
            ViewData["NombreNegocio"] = productoHandler.obtenerNombreNegocio(fechaAnalisis);

            // cargamos una lista con todos los productos que existen en la base de datos
            List<ProductoModel> productos = productoHandler.ObtenerProductos(fechaAnalisis);
            return View(productos);
        }

    }
}
