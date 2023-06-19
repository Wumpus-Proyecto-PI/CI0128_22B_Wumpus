using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PI.EntityModels;
using PI.EntityHandlers;
using Microsoft.EntityFrameworkCore;
using System;

namespace PI.Controllers
{
    public class ProductoController : ManejadorUsuariosController
    {

        private ProductoHandler? ProductoHandler = null;
        private AnalisisHandler? AnalisisHandler = null;
        private NegocioHandler? NegocioHandler = null;
        private int NumeroPaso;


        public ProductoController(NegocioHandler negocioHandler, ProductoHandler productoHandler, AnalisisHandler analisisHandler)
        {
            ProductoHandler = productoHandler;
            AnalisisHandler = analisisHandler;
            NegocioHandler = negocioHandler;
            NumeroPaso = 3;
        }


        public async Task<IActionResult> Index(string fecha)
        {
            // para que se muestre el boton de volver al analisis
            ViewBag.BotonRetorno = "Progreso";

            // asignamos el título del paso en el que se está en el layout
            ViewData["TituloPaso"] = "Gastos variables";

            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];

            // convertimos a Datetime la fecha del análisis porque de esta forma lo utiliza la vista
            DateTime fechaAnalisis = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            // enviamos la fecha a la vista con viewbag
            ViewBag.FechaAnalisis = fechaAnalisis;

            // asignamos el nombre del negocio en la vista
            // este se extrae de la base de datos con la fecha del análisis
            ViewData["NombreNegocio"] = await NegocioHandler.ObtenerNombreNegocioAsync(fechaAnalisis);

            ViewBag.NumeroPasoActual = NumeroPaso;

            // cargamos una lista con todos los productos que existen en la base de datos
            List<Producto> productos = await ProductoHandler.ObtenerProductosAsync(fechaAnalisis);
            return View(productos);
        }

    }
}
