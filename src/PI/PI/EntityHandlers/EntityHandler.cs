using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public abstract class EntityHandler
    {
        protected DataBaseContext? Contexto = null;

        public EntityHandler(DataBaseContext contexto)
        {
            Contexto = contexto;
        }


		#region GastoFijoHandler
		public async Task<List<GastoFijo>> ObtenerGastosFijosAsync(DateTime fechaAnalisis)
		{
			return await Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).ToListAsync();
		}

		public async Task<decimal> ObtenerTotalAnualAsync(DateTime fechaAnalisis)
		{
			return await Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).SumAsync(gastoFijo => gastoFijo.Monto) ?? 0.0m;
		}

		public async Task<decimal> ObtenerTotalMensualAsync(string nombreMes, DateTime fechaAnalisis)
		{
			return await ObtenerTotalAnualAsync(fechaAnalisis) / 12;
		}

        #endregion

        #region AnalisisHandler

        public async Task<Analisis> ObtenerUnAnalisis(DateTime fechaCreacion)
        {
            return await Contexto.Analisis.AsNoTracking().Where(x => x.FechaCreacion == fechaCreacion).FirstOrDefaultAsync();
        }

        public async Task<bool> ObtenerTipoAnalisisAsync(DateTime fechaAnalisis)
		{
			bool tipo = Convert.ToBoolean((await Contexto.Configuracion.FindAsync(fechaAnalisis)).TipoNegocio);
			return tipo;
		}

        // Obtiene la configuracion del analisis especificado
        // (Retorna una clase con la configuracion del analisis | Parametros: fecha del analisis)
        public async Task<Configuracion> ObtenerConfigAnalisisAsync(DateTime fechaAnalisis)
        {
            Configuracion config = await Contexto.Configuracion.Where(x => x.FechaAnalisis == fechaAnalisis).FirstOrDefaultAsync();
            return config;
        }

        #endregion

        #region NegocioHandler

        // TODO: Cambiar nombre del metodo a ObtenerNegocioDeAnalisis
        public async Task<string> ObtenerNombreNegocioAsync(DateTime fechaAnalisis)
		{
			var nombreNegocio = from negocio in Contexto.Negocios
								join analisis in Contexto.Analisis
								on negocio.Id equals analisis.IdNegocio
								where analisis.FechaCreacion == fechaAnalisis
								select negocio.Nombre;
			return await nombreNegocio.FirstOrDefaultAsync() ?? "Sin nombre";
		}

        public async Task<string> ObtenerNombreNegocioAsync(int idNegocio)
        {
            return await Contexto.Negocios.Where(x => x.Id == idNegocio).Select(x => x.Nombre).FirstOrDefaultAsync();
        }
        // metodo que retorna un negocio segun la fecha de una analisis
        public async Task<Negocio> ObtenerNegocioDeAnalisisAsync(DateTime fechaAnalisis)
		{
			var nombreNegocio = from negocio in Contexto.Negocios
								join analisis in Contexto.Analisis
								on negocio.Id equals analisis.IdNegocio
								where analisis.FechaCreacion == fechaAnalisis
								select negocio;

			return await nombreNegocio.FirstOrDefaultAsync();
		}
		#endregion

		#region InversionInicialHandler
		// TODO: Renombrar metodo. Este obtiene el total de la inversion inicial.
		public async Task<decimal> ObtenerMontoTotalAsync(DateTime fechaAnalisis)
		{
			return await Contexto.InversionInicial.Where(inversionInicial => inversionInicial.FechaAnalisis == fechaAnalisis)
				.SumAsync(inversionInicial => inversionInicial.Valor) ?? 0.0m;
		}
		#endregion

		#region FlujoDeCajaHandler

		public async Task<List<Ingreso>> ObtenerIngresosAsync(DateTime fechaAnalisis)
		{
			return await Contexto.Ingresos.AsNoTracking().Where(ingreso => ingreso.FechaAnalisis == fechaAnalisis).ToListAsync();
		}

		public async Task<List<Egreso>> ObtenerEgresosAsync(DateTime fechaAnalisis)
		{
			return await Contexto.Egresos.AsNoTracking().Where(ingreso => ingreso.FechaAnalisis == fechaAnalisis).ToListAsync();
		}
		public async Task<decimal> ObtenerMontoTotalDeIngresosPorMesAsync(string nombreMes, DateTime fechaAnalisis)
		{
			return await Contexto.Ingresos
				.Where(ingreso => ingreso.Mes == nombreMes
				&& ingreso.FechaAnalisis == fechaAnalisis)
				.SumAsync(Ingreso => Ingreso.Monto);
		}

		public async Task<decimal> ObtenerMontoTotalDeEgresosPorMesAsync(string nombreMes, DateTime fechaAnalisis)
		{
			return await Contexto.Egresos
				.Where(egreso => egreso.Mes == nombreMes
				&& egreso.FechaAnalisis == fechaAnalisis)
				.SumAsync(egreso => egreso.Monto);
		}

		public async Task<decimal> ObtenerInversionDeMesAsync(string nombreMes, DateTime fechaAnalisis)
		{
			return (await Contexto.Meses.FindAsync(nombreMes, fechaAnalisis)).InversionPorMes ?? 0.0m;
		}


		#endregion
	}
}
