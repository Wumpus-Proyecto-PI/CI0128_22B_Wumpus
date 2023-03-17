using Microsoft.EntityFrameworkCore;
using PI.EntityModels;
using System.Linq;

namespace PI.Services
{
    public class ProceduresServices : IDisposable
    {
        private DataBaseContext? Contexto = null;

        public ProceduresServices(DataBaseContext contexto)
        {
            Contexto = contexto;
        }

        ~ProceduresServices()
        {
            Contexto = null;
        }

        public void Dispose()
        {
            Contexto?.Dispose();
            Contexto = null;
        }

        #region GastoFijoHandler
        public async Task<List<GastoFijo>> ObtenerGastosFijosAsync(DateTime fechaAnalisis)
        {
            return await Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).ToListAsync();
        }

        public async Task<decimal> ObtenerTotalAnualAsync(string nombreMes, DateTime fechaAnalisis)
        {
            return await Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).SumAsync(gastoFijo => gastoFijo.Monto) ?? 0.0m;
        }

        public async Task<decimal> ObtenerTotalMensualAsync(string nombreMes, DateTime fechaAnalisis)
        {
            return await ObtenerTotalAnualAsync(nombreMes, fechaAnalisis) / 12;
        }

        #endregion

        #region AnalisisHandler


        public async Task<int> ObtenerTipoAnalisisAsync(DateTime fechaAnalisis)
        {
            return (await Contexto.Configuracion.FindAsync(fechaAnalisis)).TipoNegocio;
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
