using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using PI.Services;
using PI.Views.Shared.Components.Producto;

namespace PI.Controllers
{
    public class FlujoDeCajaController : Controller
    {
        public IActionResult IndexFlujoDeCaja(string fechaAnalisis)
        {
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            flujoDeCajaHandler.crearFlujoDeCaja(fechaCreacionAnalisis);

            ViewBag.Ingresos = flujoDeCajaHandler.obtenerIngresos(fechaCreacionAnalisis);

            // Datos que ocupa la vista
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            // Título de la pestaña en el navegador.
            ViewData["Title"] = "Flujo de caja";
            // Título del paso en el que se está en el layout
            ViewData["TituloPaso"] = ViewData["Title"];
            // Muestra el botón de regreso hacia el progreso (pasos) del análisis.
            ViewBag.BotonRetorno = "Progreso";

            ProductoHandler productoHandler = new ProductoHandler();
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            ViewBag.GastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaCreacionAnalisis);
            AnalisisHandler analisisHandler = new AnalisisHandler();

            List<ProductoModel> productos = productoHandler.ObtenerProductos(fechaCreacionAnalisis);
            decimal totalGastosFijos = gastoFijoHandler.obtenerTotalAnual(fechaCreacionAnalisis);
            decimal gananciaMensual = analisisHandler.ObtenerGananciaMensual(fechaCreacionAnalisis);
            ViewBag.Iniciado = analisisHandler.ObtenerTipoAnalisis(fechaCreacionAnalisis);

            ViewBag.MetaDeVentasMensual = AnalisisRentabilidadService.calcularTotalMetaMoneda(productos, totalGastosFijos, gananciaMensual);

            InversionInicialHandler inversionInicialHandler = new InversionInicialHandler();
            ViewData["NombreNegocio"] = inversionInicialHandler.obtenerNombreNegocio(fechaCreacionAnalisis);
            ViewBag.InversionInicial = inversionInicialHandler.ObtenerMontoTotal(fechaAnalisis);

            return View();
        }
    }
}
