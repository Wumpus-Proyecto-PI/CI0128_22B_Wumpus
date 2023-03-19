using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public class NegocioHandler : EntityHandler
    {
        public NegocioHandler (DataBaseContext context) : base (context) { }

        public async Task<List<Negocio>> ObtenerNegociosAsync(string userId)
        {
             return await Contexto.Negocios.AsNoTracking().Where(x => x.IdUsuario == userId).ToListAsync();
        }

        public async Task<int> AgregarNegocioAsync(Negocio nuevoNegocio) {
            await base.Contexto.Negocios.AddAsync(nuevoNegocio);
            return await base.Contexto.SaveChangesAsync();
        }

        public async Task<int> EliminarNegocioAsync(int idNegocio)
        {
            base.Contexto.Negocios.Remove(await Contexto.Negocios.FindAsync(idNegocio));
            return await base.Contexto.SaveChangesAsync();
        }

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
    }
}
