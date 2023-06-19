using Microsoft.AspNetCore.Mvc;
using PI.EntityHandlers;
using PI.Services;
using PI.EntityModels;

namespace PI.Controllers
{
	public class FlujoDeCajaController : Controller
	{
		FlujoDeCajaHandler? FlujoDeCajaHandler = null;
		GastoFijoHandler? GastoFijoHandler = null;
		FlujoCajaService? FlujoCajaService = null;
        public const int CantidadMeses = 6;

		public FlujoDeCajaController(FlujoDeCajaHandler? flujoDeCajaHandler, GastoFijoHandler? gastoFijoHandler, FlujoCajaService? flujoCajaService)
		{
			FlujoDeCajaHandler = flujoDeCajaHandler;
			GastoFijoHandler = gastoFijoHandler;
            FlujoCajaService = flujoCajaService;

        }
		// Recibe la fecha del análisis que se quiere consultar en flujo de caja
		// Retorna la vista de la pantalla correspondiente al flujo de caja
		public async Task<IActionResult> IndexFlujoDeCaja(string fechaAnalisis)
		{
			DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;

            // Acciones para calcular datos que se envian a la vista
            await FlujoDeCajaHandler.CrearIngresoPorMesAsync(fechaCreacionAnalisis);
			await FlujoDeCajaHandler.CrearEgresoPorMesAsync(fechaCreacionAnalisis);

			List<Mes> meses = await FlujoDeCajaHandler.ObtenerMesesAsync(fechaCreacionAnalisis);
			List<PI.EntityModels.Producto> productos = await FlujoDeCajaHandler.ObtenerProductosAsync(fechaCreacionAnalisis);
			decimal totalGastosFijos = await FlujoDeCajaHandler.ObtenerTotalAnualAsync(fechaCreacionAnalisis);
			decimal gananciaMensual = await FlujoDeCajaHandler.ObtenerGananciaMensual(fechaCreacionAnalisis);

			// Convierte los porcentajes a valores válidos (divide entre 100).
			Configuracion configuracionAnalisis = await FlujoDeCajaHandler.ObtenerConfigAnalisisAsync(fechaCreacionAnalisis);
			decimal seguroSocial = configuracionAnalisis.PorcentajeSs / 12 ?? 0.0m;
			decimal prestaciones = configuracionAnalisis.PorcentajePl / 12 ?? 0.0m ;

			// Actualiza los gastos fijos de la estructura organizativa para mostrarlos en la sección de flujo de caja.
			await FlujoDeCajaHandler.ActualizarGastosPredeterminadosAsync(fechaCreacionAnalisis, seguroSocial, prestaciones);

            List<string> ingresosTotales = Enumerable.Repeat(string.Empty, CantidadMeses).ToList();
            await FlujoCajaService.ActualizarTotalesIngresosAsync(meses, ingresosTotales);

            List<string> egresosTotales = Enumerable.Repeat(string.Empty, CantidadMeses).ToList();
            await FlujoCajaService.ActualizarTotalesEgresosAsync(meses, egresosTotales, await GastoFijoHandler.ObtenerTotalMensualAsync(fechaCreacionAnalisis));

            // Datos enviados a la vista
            ViewData["Title"] = "Flujo de caja";
			ViewData["TituloPaso"] = ViewData["Title"];
			ViewBag.Ingresos = await FlujoDeCajaHandler.ObtenerIngresosAsync(fechaCreacionAnalisis);
			ViewBag.Egresos = await FlujoDeCajaHandler.ObtenerEgresosAsync(fechaCreacionAnalisis);
			ViewBag.Meses = meses;
			ViewBag.flujoMensual = Enumerable.Repeat(string.Empty, CantidadMeses).ToList();
			ViewBag.fechaAnalisis = fechaCreacionAnalisis;
			ViewBag.BotonRetorno = "Progreso";
			ViewBag.GastosFijos = await GastoFijoHandler.ObtenerGastosFijosAsync(fechaCreacionAnalisis);
			ViewBag.Iniciado = await FlujoDeCajaHandler.ObtenerTipoAnalisisAsync(fechaCreacionAnalisis);
			ViewBag.IngresosTotales = ingresosTotales;
			ViewBag.EgresosTotales = egresosTotales;

            ViewBag.MetaDeVentasMensual = AnalisisRentabilidadService.calcularTotalMetaMoneda(productos, totalGastosFijos, gananciaMensual);
            ViewData["NombreNegocio"] = await FlujoDeCajaHandler.ObtenerNombreNegocioAsync(fechaCreacionAnalisis);
            ViewBag.InversionInicial = await FlujoDeCajaHandler.ObtenerMontoTotalAsync(fechaCreacionAnalisis);

            return View();
		}
	}
}
