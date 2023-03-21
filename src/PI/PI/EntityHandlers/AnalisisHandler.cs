using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public class AnalisisHandler : EntityHandler
    {
        public AnalisisHandler(DataBaseContext context) : base(context) { }

        public async Task<int> ObtenerTipoAnalisisAsync(DateTime fechaAnalisis)
        {
            return (await base.Contexto.Configuracion.FindAsync(fechaAnalisis)).TipoNegocio;
        }

        public async Task<Configuracion> ObtenerConfigAnalisisAsync(DateTime fechaAnalisis)
        {
            return await base.Contexto.Configuracion.AsNoTracking().Where(config => config.FechaAnalisis == fechaAnalisis).FirstOrDefaultAsync();
        }
    }
}
