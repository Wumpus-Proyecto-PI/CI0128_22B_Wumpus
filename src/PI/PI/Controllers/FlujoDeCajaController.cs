using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using PI.Services;
using PI.Views.Shared.Components.Producto;
using PI.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace PI.Controllers
{
	public class FlujoDeCajaController : Controller
	{
		private DataBaseContext? DataBaseContext;
		private ProceduresServices? Procedures;

		public FlujoDeCajaController(DataBaseContext context, ProceduresServices procedures)
		{
			DataBaseContext = context;
			Procedures = procedures;
		}
		// Recibe la fecha del análisis que se quiere consultar en flujo de caja
		// Retorna la vista de la pantalla correspondiente al flujo de caja
		public async Task<IActionResult> IndexFlujoDeCaja(string fechaAnalisis)
		{
			DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

			// Acciones para calcular datos que se envian a la vista
			await CrearIngresoPorMesAsync(fechaCreacionAnalisis);
			await CrearEgresoPorMesAsync(fechaCreacionAnalisis);

			List<Mes> meses = await ObtenerMesesAsync(fechaCreacionAnalisis);
			List<PI.EntityModels.Producto> productos = await ObtenerProductosAsync(fechaCreacionAnalisis);
			decimal totalGastosFijos = await ObtenerTotalAnualAsync(fechaCreacionAnalisis);
			decimal gananciaMensual = await ObtenerGananciaMensual(fechaCreacionAnalisis);

			// Convierte los porcentajes a valores válidos (divide entre 100).
			Configuracion configuracionAnalisis = await ObtenerConfigAnalisis(fechaCreacionAnalisis);
			decimal seguroSocial = configuracionAnalisis.PorcentajeSs / 100 ?? 0.0m;
			decimal prestaciones = configuracionAnalisis.PorcentajePl / 100 ?? 0.0m;

			// Actualiza los gastos fijos de la estructura organizativa para mostrarlos en la sección de flujo de caja.
			int escrituras = await Procedures.ActualizarGastosPredeterminadosAsync(fechaCreacionAnalisis, seguroSocial, prestaciones);

			// Datos enviados a la vista
			ViewData["Title"] = "Flujo de caja";
			ViewData["TituloPaso"] = ViewData["Title"];
			ViewBag.Ingresos = await Procedures.ObtenerIngresosAsync(fechaCreacionAnalisis);
			ViewBag.Egresos = await Procedures.ObtenerEgresosAsync(fechaCreacionAnalisis);
			ViewBag.Meses = meses;
			// ViewBag.flujoMensual = FlujoCajaService.ActualizarFlujosMensuales(meses);
			ViewBag.fechaAnalisis = fechaCreacionAnalisis;
			ViewBag.BotonRetorno = "Progreso";
			// ViewBag.GastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaCreacionAnalisis);
			ViewBag.GastosFijos = await Procedures.ObtenerGastosFijosAsync(fechaCreacionAnalisis);
            
			//         ViewBag.Iniciado = analisisHandler.ObtenerTipoAnalisis(fechaCreacionAnalisis);
			ViewBag.Iniciado = await Procedures.ObtenerTipoAnalisisAsync(fechaCreacionAnalisis);

			//         ViewBag.MetaDeVentasMensual = AnalisisRentabilidadService.calcularTotalMetaMoneda(productos, totalGastosFijos, gananciaMensual);
			ViewBag.MetaDeVentasMensual = 0.0m;

            //         ViewData["NombreNegocio"] = inversionInicialHandler.obtenerNombreNegocio(fechaCreacionAnalisis);
            ViewData["NombreNegocio"] = await Procedures.ObtenerNombreNegocioAsync(fechaCreacionAnalisis);

            //         ViewBag.InversionInicial = inversionInicialHandler.ObtenerMontoTotal(fechaAnalisis);
            ViewBag.InversionInicial = await Procedures.ObtenerMontoTotalAsync(fechaCreacionAnalisis);

            return View();
		}
		public void GenerarMesesIngresos(string tipoIngreso, DateTime fechaAnalisis, List<Ingreso> ingresos)
		{
			for (int i = 1; i < 7; ++i)
			{
				ingresos.Add(
					new Ingreso
					{
						Mes = $"Mes {i}",
						FechaAnalisis = fechaAnalisis,
						Tipo = tipoIngreso
					});
			}
		}

		public void GenerarMesesEgresos(string tipoIngreso, DateTime fechaAnalisis, List<Egreso> Egresos)
		{
			for (int i = 1; i < 7; ++i)
			{
				Egresos.Add(
					new Egreso
					{
						Mes = $"Mes {i}",
						FechaAnalisis = fechaAnalisis,
						Tipo = tipoIngreso
					});
			}
		}
		public async Task<int> CrearIngresoPorMesAsync(DateTime fechaAnalisis)
		{
			var ingresos = await DataBaseContext.Ingresos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
			if (ingresos.Any() == false)
			{
				GenerarMesesIngresos("contado", fechaAnalisis, ingresos);
				GenerarMesesIngresos("credito", fechaAnalisis, ingresos);
				GenerarMesesIngresos("otros", fechaAnalisis, ingresos);
				return await DataBaseContext.SaveChangesAsync();
			}
			return 0;
		}

		public async Task<int> CrearEgresoPorMesAsync(DateTime fechaAnalisis)
		{
			var egresos = await DataBaseContext.Egresos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
			if (egresos.Any() == false)
			{
				GenerarMesesEgresos("contado", fechaAnalisis, egresos);
				GenerarMesesEgresos("credito", fechaAnalisis, egresos);
				GenerarMesesEgresos("otros", fechaAnalisis, egresos);
                return await DataBaseContext.SaveChangesAsync();
			}
            return 0;
        }

        // obtiene los meses segun una fecha de analisis
        public async Task<List<Mes>> ObtenerMesesAsync(DateTime fechaAnalisis)
		{
			return await DataBaseContext.Meses.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
		}

		// Método que obtiene los productos de un analisis
		// Devuele una lista de ProductoModel
		public async Task<List<PI.EntityModels.Producto>> ObtenerProductosAsync(DateTime fechaAnalisis)
		{
			return await DataBaseContext.Productos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
		}

		// Obtiene y retorna la suma total mensual de los montos de gastos fijos
		public async Task<decimal> ObtenerTotalAnualAsync(DateTime fechaAnalisis)
		{
			decimal totalAnual = 0.0m;

			var suma = await DataBaseContext.GastosFijos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).SumAsync(i => i.Monto);

			return suma ?? 0.0m;
		}

		public async Task<decimal> ObtenerGananciaMensual(DateTime fechaAnalisis)
		{
			return (await DataBaseContext.Analisis.FindAsync(fechaAnalisis)).GananciaMensual ?? 0.0m;
		}

		// Obtiene la configuracion del analisis especificado
		// (Retorna una clase con la configuracion del analisis | Parametros: fecha del analisis)
		public async Task<Configuracion> ObtenerConfigAnalisis(DateTime fechaAnalisis)
		{
			Configuracion config = await DataBaseContext.Configuracion.FindAsync(fechaAnalisis);
			return config;
		}
	}
}
