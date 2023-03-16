using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

namespace PI.Services
{
    public class ProceduresServices
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
    }
}
