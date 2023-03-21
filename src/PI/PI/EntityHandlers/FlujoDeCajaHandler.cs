using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public class FlujoDeCajaHandler : EntityHandler
    {
        public FlujoDeCajaHandler(DataBaseContext context) : base (context) { }
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
			var ingresos = await base.Contexto.Ingresos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
			if (ingresos.Any() == false)
			{
				GenerarMesesIngresos("contado", fechaAnalisis, ingresos);
				GenerarMesesIngresos("credito", fechaAnalisis, ingresos);
				GenerarMesesIngresos("otros", fechaAnalisis, ingresos);
				return await base.Contexto.SaveChangesAsync();
			}
			return 0;
		}

		public async Task<int> CrearEgresoPorMesAsync(DateTime fechaAnalisis)
		{
			var egresos = await base.Contexto.Egresos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
			if (egresos.Any() == false)
			{
				GenerarMesesEgresos("contado", fechaAnalisis, egresos);
				GenerarMesesEgresos("credito", fechaAnalisis, egresos);
				GenerarMesesEgresos("otros", fechaAnalisis, egresos);
				return await base.Contexto.SaveChangesAsync();
			}
			return 0;
		}

		// obtiene los meses segun una fecha de analisis
		public async Task<List<Mes>> ObtenerMesesAsync(DateTime fechaAnalisis)
		{
			return await base.Contexto.Meses.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
		}

		// Método que obtiene los productos de un analisis
		// Devuele una lista de ProductoModel
		public async Task<List<PI.EntityModels.Producto>> ObtenerProductosAsync(DateTime fechaAnalisis)
		{
			return await base.Contexto.Productos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
		}

		// Obtiene y retorna la suma total mensual de los montos de gastos fijos
		public async Task<decimal> ObtenerTotalAnualAsync(DateTime fechaAnalisis)
		{
			decimal totalAnual = 0.0m;

			var suma = await base.Contexto.GastosFijos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).SumAsync(i => i.Monto);

			return suma ?? 0.0m;
		}

		public async Task<decimal> ObtenerGananciaMensual(DateTime fechaAnalisis)
		{
			return (await base.Contexto.Analisis.FindAsync(fechaAnalisis)).GananciaMensual ?? 0.0m;
		}

		// Obtiene la configuracion del analisis especificado
		// (Retorna una clase con la configuracion del analisis | Parametros: fecha del analisis)
		public async Task<Configuracion> ObtenerConfigAnalisis(DateTime fechaAnalisis)
		{
			Configuracion config = await base.Contexto.Configuracion.FindAsync(fechaAnalisis);
			return config;
		}

		public async Task<int> ActualizarGastosPredeterminadosAsync(DateTime fechaConversion, decimal seguroSocial, decimal prestaciones)
		{
			int escrituras = 0;
			escrituras += await ActualizarSalariosNetoAsync(fechaConversion, seguroSocial, prestaciones);
			escrituras += await ActualizarSeguroSocialAsync(fechaConversion, seguroSocial);
			escrituras += await ActualizarPrestacionesAsync(fechaConversion, prestaciones);
			escrituras += await ActualizarBeneficiosAsync(fechaConversion);
			return escrituras;
		}

		// Crea o actualiza el gasto fijo de salarios netos
		public async Task<int> ActualizarSalariosNetoAsync(DateTime fechaAnalisis, decimal seguroSocial, decimal prestaciones)
		{
			PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Salarios netos").FirstOrDefaultAsync();
			if (gastoFijo != null)
			{
				gastoFijo.Monto = await ObtenerTotalSalariosNetoAsync(fechaAnalisis, seguroSocial, prestaciones);
			}
			else
			{
				GastoFijo gastoNuevo = new GastoFijo
				{
					Nombre = "Salarios netos",
					FechaAnalisis = fechaAnalisis,
					Monto = await ObtenerTotalSalariosNetoAsync(fechaAnalisis, seguroSocial, prestaciones),
					Orden = 1
				};
				base.Contexto.GastosFijos.Add(gastoNuevo);
			}
			return await base.Contexto.SaveChangesAsync();
		}

		// procedure reemplazado
		public async Task<decimal> ObtenerTotalSalariosNetoAsync(DateTime fechaAnalisis, decimal seguroSocial, decimal Prestaciones)
		{

			decimal sumaSalarios = await ObtenerSumaSalarios(fechaAnalisis);
			decimal gastoSs = await ObtenerGastoSeguroSocial(fechaAnalisis, sumaSalarios, seguroSocial);
			decimal gastoPl = await ObtenerGastoPrestaciones(fechaAnalisis, sumaSalarios, Prestaciones);

			return sumaSalarios - gastoSs - gastoPl;
		}
		// procedure reemplazado
		public async Task<decimal> ObtenerSumaSalarios(DateTime fechaAnalisis)
		{
			List<Puesto> puestos = await base.Contexto.Puestos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
			decimal totalSalarios = 0.0m;
			foreach (var puesto in puestos)
			{
				totalSalarios = (puesto.CantidadPlazas * puesto.SalarioBruto) ?? 0.0m;
			}
			return totalSalarios * 12;
		}
		// procedure reemplazado
		public async Task<decimal> ObtenerGastoSeguroSocial(DateTime fechaAnalisis, decimal sumaSalarios, decimal porcentajeSs)
		{
			return sumaSalarios * porcentajeSs;
		}
		// procedure reemplazado
		public async Task<decimal> ObtenerGastoPrestaciones(DateTime fechaAnalisis, decimal sumaSalarios, decimal porcentajePl)
		{
			return sumaSalarios * porcentajePl;
		}


		// Crea o actualiza el gasto fijo de seguro social
		public async Task<int> ActualizarSeguroSocialAsync(DateTime fechaAnalisis, decimal seguroSocial)
		{
			PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Seguridad social").FirstOrDefaultAsync();
			if (gastoFijo != null)
			{
				gastoFijo.Monto = await ObtenerGastoSeguroSocialAsync(fechaAnalisis, seguroSocial);
			}
			else
			{
				GastoFijo gastoNuevo = new GastoFijo
				{
					Nombre = "Seguridad social",
					FechaAnalisis = fechaAnalisis,
					Monto = await ObtenerGastoSeguroSocialAsync(fechaAnalisis, seguroSocial),
					Orden = 2
				};
				base.Contexto.GastosFijos.Add(gastoNuevo);
			}
			return await base.Contexto.SaveChangesAsync();
		}

		// procedure reemplazado
		public async Task<decimal> ObtenerGastoSeguroSocialAsync(DateTime fechaAnalisis, decimal seguroSocial)
		{
			return await ObtenerSumaSalarios(fechaAnalisis) * seguroSocial;
		}

		// Crea o actualiza el gasto fijo de salarios netos
		public async Task<int> ActualizarPrestacionesAsync(DateTime fechaAnalisis, decimal prestaciones)
		{
			PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Prestaciones laborales").FirstOrDefaultAsync();
			if (gastoFijo != null)
			{
				gastoFijo.Monto = await ObtenerGastoPrestacionesAsync(fechaAnalisis, prestaciones);
			}
			else
			{
				GastoFijo gastoNuevo = new GastoFijo
				{
					Nombre = "Prestaciones laborales",
					FechaAnalisis = fechaAnalisis,
					Monto = await ObtenerGastoPrestacionesAsync(fechaAnalisis, prestaciones),
					Orden = 3
				};
				base.Contexto.GastosFijos.Add(gastoNuevo);
			}
			return await base.Contexto.SaveChangesAsync();
		}

		// procedure reemplazado
		public async Task<decimal> ObtenerGastoPrestacionesAsync(DateTime fechaAnalisis, decimal prestaciones)
		{
			return await ObtenerSumaSalarios(fechaAnalisis) * prestaciones;
		}

		// Crea o actualiza el gasto fijo de salarios netos
		public async Task<int> ActualizarBeneficiosAsync(DateTime fechaAnalisis)
		{
			PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Beneficios de empleados").FirstOrDefaultAsync();
			if (gastoFijo != null)
			{
				gastoFijo.Monto = await ObtenerTotalBeneficiosAsync(fechaAnalisis);
			}
			else
			{
				GastoFijo gastoNuevo = new GastoFijo
				{
					Nombre = "Beneficios de empleados",
					FechaAnalisis = fechaAnalisis,
					Monto = await ObtenerTotalBeneficiosAsync(fechaAnalisis),
					Orden = 4
				};
				base.Contexto.GastosFijos.Add(gastoNuevo);
			}
			return await base.Contexto.SaveChangesAsync();
		}

		// procedure reemplazado
		public async Task<decimal> ObtenerTotalBeneficiosAsync(DateTime fechaAnalisis)
		{
			List<Puesto> puestos = await base.Contexto.Puestos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
			decimal totalSalarios = 0.0m;
			foreach (var puesto in puestos)
			{
				totalSalarios = (puesto.CantidadPlazas * puesto.Beneficios) ?? 0.0m;
			}
			return totalSalarios * 12;
		}
	}
}
