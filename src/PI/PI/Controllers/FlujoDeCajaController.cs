using Microsoft.AspNetCore.Mvc;
using PI.EntityHandlers;
using PI.Services;
using PI.EntityModels;

namespace PI.Controllers
{
	public class FlujoDeCajaController : Controller
	{
		private FlujoDeCajaHandler? FlujoDeCajaHandler;

		public FlujoDeCajaController(DataBaseContext context)
		{
			FlujoDeCajaHandler = new FlujoDeCajaHandler(context);
		}
		// Recibe la fecha del análisis que se quiere consultar en flujo de caja
		// Retorna la vista de la pantalla correspondiente al flujo de caja
		public async Task<IActionResult> IndexFlujoDeCaja(string fechaAnalisis)
		{
			DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

			// Acciones para calcular datos que se envian a la vista
			await FlujoDeCajaHandler.CrearIngresoPorMesAsync(fechaCreacionAnalisis);
			await FlujoDeCajaHandler.CrearEgresoPorMesAsync(fechaCreacionAnalisis);

			List<Mes> meses = await FlujoDeCajaHandler.ObtenerMesesAsync(fechaCreacionAnalisis);
			List<PI.EntityModels.Producto> productos = await FlujoDeCajaHandler.ObtenerProductosAsync(fechaCreacionAnalisis);
			decimal totalGastosFijos = await FlujoDeCajaHandler.ObtenerTotalAnualAsync(fechaCreacionAnalisis);
			decimal gananciaMensual = await FlujoDeCajaHandler.ObtenerGananciaMensual(fechaCreacionAnalisis);

			// Convierte los porcentajes a valores válidos (divide entre 100).
			Configuracion configuracionAnalisis = await FlujoDeCajaHandler.ObtenerConfigAnalisis(fechaCreacionAnalisis);
			decimal seguroSocial = configuracionAnalisis.PorcentajeSs / 100 ?? 0.0m;
			decimal prestaciones = configuracionAnalisis.PorcentajePl / 100 ?? 0.0m;

			// Actualiza los gastos fijos de la estructura organizativa para mostrarlos en la sección de flujo de caja.
			int escrituras = await FlujoDeCajaHandler.ActualizarGastosPredeterminadosAsync(fechaCreacionAnalisis, seguroSocial, prestaciones);

			// Datos enviados a la vista
			ViewData["Title"] = "Flujo de caja";
			ViewData["TituloPaso"] = ViewData["Title"];
			ViewBag.Ingresos = await FlujoDeCajaHandler.ObtenerIngresosAsync(fechaCreacionAnalisis);
			ViewBag.Egresos = await FlujoDeCajaHandler.ObtenerEgresosAsync(fechaCreacionAnalisis);
			ViewBag.Meses = meses;
			// ViewBag.flujoMensual = FlujoCajaService.ActualizarFlujosMensuales(meses);
			ViewBag.fechaAnalisis = fechaCreacionAnalisis;
			ViewBag.BotonRetorno = "Progreso";
			// ViewBag.GastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaCreacionAnalisis);
			ViewBag.GastosFijos = await FlujoDeCajaHandler.ObtenerGastosFijosAsync(fechaCreacionAnalisis);
            
			//         ViewBag.Iniciado = analisisHandler.ObtenerTipoAnalisis(fechaCreacionAnalisis);
			ViewBag.Iniciado = await FlujoDeCajaHandler.ObtenerTipoAnalisisAsync(fechaCreacionAnalisis);

			//         ViewBag.MetaDeVentasMensual = AnalisisRentabilidadService.calcularTotalMetaMoneda(productos, totalGastosFijos, gananciaMensual);
			ViewBag.MetaDeVentasMensual = 0.0m;

            //         ViewData["NombreNegocio"] = inversionInicialHandler.obtenerNombreNegocio(fechaCreacionAnalisis);
            ViewData["NombreNegocio"] = await FlujoDeCajaHandler.ObtenerNombreNegocioAsync(fechaCreacionAnalisis);

            //         ViewBag.InversionInicial = inversionInicialHandler.ObtenerMontoTotal(fechaAnalisis);
            ViewBag.InversionInicial = await FlujoDeCajaHandler.ObtenerMontoTotalAsync(fechaCreacionAnalisis);

            return View();
		}
	}
}
