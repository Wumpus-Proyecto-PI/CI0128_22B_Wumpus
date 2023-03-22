using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public abstract class EntityHandler : IDisposable
    {
        protected DataBaseContext? Contexto = null;

        public EntityHandler(DataBaseContext contexto)
        {
            Contexto = contexto;
        }

        ~EntityHandler()
        {
            Contexto = null;
        }

        public void Dispose()
        {
            Contexto?.Dispose();
            Contexto = null;
        }

        public async Task<string> ObtenerNombreDeNegocio(DateTime fechaAnalisis)
        {
            // string a retornar con el nombre del negocio
            string nombreNegocio = await Contexto.Negocios
                .Join(Contexto.Analisis,
                    a => a.Id,
                    b => b.IdNegocio,
                    (a, b) => new { Negocios = a, Analisis = b })
                .Where(x => x.Analisis.FechaCreacion == fechaAnalisis)
                .Select(x => x.Negocios.Nombre)
                .Distinct().FirstOrDefaultAsync();


            if (string.IsNullOrEmpty(nombreNegocio))
            {
                // existe el caso de que un negocio no tenga nombre en nuestro producto
                nombreNegocio = "Sin nombre";
            }

            return nombreNegocio;
        }

        public async Task<Negocio> ObtenerNegocioDeAnalisis(DateTime fechaAnalisis)
        {
            Analisis analisis = await Contexto.Analisis.Where(x => x.FechaCreacion == fechaAnalisis).FirstOrDefaultAsync();
            Negocio negocio = await Contexto.Negocios.Where(x => x.Id == analisis.IdNegocio).FirstOrDefaultAsync();
            return negocio;
        }

        public async Task<string> ObtenerNombreDeNegocio(int idNegocio)
        {
            return await Contexto.Negocios.Where(x => x.Id == idNegocio).Select(x => x.Nombre).FirstOrDefaultAsync();
        }
    }
}