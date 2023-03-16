using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

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

        public async Task<List<Ingreso>> ObtenerIngresosAsync(DateTime fechaAnalisis)
        {
            return await Contexto.Ingresos.AsNoTracking().Where(ingreso => ingreso.FechaAnalisis == fechaAnalisis).ToListAsync();
        }

        public async Task<List<Egreso>> ObtenerEgresosAsync(DateTime fechaAnalisis)
        {
            return await Contexto.Egresos.AsNoTracking().Where(ingreso => ingreso.FechaAnalisis == fechaAnalisis).ToListAsync();
        }

        public async Task<int> ObtenerTipoAnalisisAsync(DateTime fechaAnalisis)
        {
            return (await Contexto.Configuracion.FindAsync(fechaAnalisis)).TipoNegocio;
        }

        public async Task<List<GastoFijo>> ObtenerGastosFijosAsync(DateTime fechaAnalisis)
        {
            return await Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).ToListAsync();
        }

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

        // TODO: Renombrar metodo. Este obtiene el total de la inversion inicial.
        public async Task<decimal> ObtenerMontoTotalAsync(DateTime fechaAnalisis)
        {
            return await Contexto.InversionInicial.Where(inversionInicial => inversionInicial.FechaAnalisis == fechaAnalisis)
                .SumAsync(inversionInicial => inversionInicial.Valor) ?? 0.0m;
        }

        public void Dispose()
        {
            Contexto?.Dispose();
            Contexto = null;
        }
    }
}
