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

            ViewBag.Egresos = flujoDeCajaHandler.obtenerEgresos(fechaCreacionAnalisis);

            List<MesModel> meses = flujoDeCajaHandler.ObtenerMeses(fechaCreacionAnalisis);
            ViewBag.Meses = meses;
            ViewBag.flujoMensual = FlujoCajaService.ActualizarFlujosMensuales(meses);

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
            AnalisisHandler analisisHandler = new AnalisisHandler();

            // Convierte los porcentajes a valores válidos (divide entre 100).
            ConfigAnalisisModel configuracionAnalisis = analisisHandler.ObtenerConfigAnalisis(fechaCreacionAnalisis);
            decimal seguroSocial = configuracionAnalisis.PorcentajeSS / 100;
            decimal prestaciones = configuracionAnalisis.PorcentajePL / 100;

            // Actualiza los gastos fijos de la estructura organizativa para mostrarlos en la sección de flujo de caja.
            gastoFijoHandler.actualizarGastosPredeterminados(fechaCreacionAnalisis, seguroSocial, prestaciones);

            ViewBag.GastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaCreacionAnalisis);

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
