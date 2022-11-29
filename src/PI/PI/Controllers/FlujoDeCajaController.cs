using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using PI.Services;
using PI.Views.Shared.Components.Producto;

namespace PI.Controllers
{
    public class FlujoDeCajaController : Controller
    {
        // Recibe la fecha del análisis que se quiere consultar en flujo de caja
        // Retorna la vista de la pantalla correspondiente al flujo de caja
        public IActionResult IndexFlujoDeCaja(string fechaAnalisis)
        {
            // Handlers necesarios para consultar la base de datos
            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            ProductoHandler productoHandler = new ProductoHandler();
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            AnalisisHandler analisisHandler = new AnalisisHandler();
            InversionInicialHandler inversionInicialHandler = new InversionInicialHandler();

            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            // Acciones para calcular datos que se envian a la vista
            flujoDeCajaHandler.CrearFlujoDeCaja(fechaCreacionAnalisis);
            List<MesModel> meses = flujoDeCajaHandler.ObtenerMeses(fechaCreacionAnalisis);
            List<ProductoModel> productos = productoHandler.ObtenerProductos(fechaCreacionAnalisis);
            decimal totalGastosFijos = gastoFijoHandler.obtenerTotalAnual(fechaCreacionAnalisis);
            decimal gananciaMensual = analisisHandler.ObtenerGananciaMensual(fechaCreacionAnalisis);

            // Convierte los porcentajes a valores válidos (divide entre 100).
            ConfigAnalisisModel configuracionAnalisis = analisisHandler.ObtenerConfigAnalisis(fechaCreacionAnalisis);
            decimal seguroSocial = configuracionAnalisis.PorcentajeSS / 100;
            decimal prestaciones = configuracionAnalisis.PorcentajePL / 100;

            // Actualiza los gastos fijos de la estructura organizativa para mostrarlos en la sección de flujo de caja.
            gastoFijoHandler.actualizarGastosPredeterminados(fechaCreacionAnalisis, seguroSocial, prestaciones);

            // Datos enviados a la vista
            ViewData["Title"] = "Flujo de caja";
            ViewData["TituloPaso"] = ViewData["Title"];
            ViewBag.Ingresos = flujoDeCajaHandler.ObtenerIngresos(fechaCreacionAnalisis);
            ViewBag.Egresos = flujoDeCajaHandler.ObtenerEgresos(fechaCreacionAnalisis);
            ViewBag.Meses = meses;
            ViewBag.flujoMensual = FlujoCajaService.ActualizarFlujosMensuales(meses);
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            ViewBag.BotonRetorno = "Progreso";
            ViewBag.GastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaCreacionAnalisis);
            ViewBag.Iniciado = analisisHandler.ObtenerTipoAnalisis(fechaCreacionAnalisis);
            ViewBag.MetaDeVentasMensual = AnalisisRentabilidadService.calcularTotalMetaMoneda(productos, totalGastosFijos, gananciaMensual);
            ViewData["NombreNegocio"] = inversionInicialHandler.obtenerNombreNegocio(fechaCreacionAnalisis);
            ViewBag.InversionInicial = inversionInicialHandler.ObtenerMontoTotal(fechaAnalisis);

            return View();
        }
    }
}
