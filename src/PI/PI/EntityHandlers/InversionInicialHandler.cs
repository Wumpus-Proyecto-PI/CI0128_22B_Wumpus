using PI.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace PI.EntityHandlers
{
    public class InversionInicialHandler : EntityHandler
    {
        public InversionInicialHandler(DataBaseContext contexto) : base(contexto) { }

        public async Task<List<InversionInicial>> ObtenerGastosInicialesAsync(DateTime fechaAnalisis)
        {
            List<InversionInicial> gastosIniciales = new List<InversionInicial>();

            gastosIniciales = await base.Contexto.InversionInicial.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();

            return gastosIniciales;
        }

        // Recibe la fecha del análisis al que se quiere insertar el gasto inicial y lo inserta en la base de datos
        public async Task<int> IngresarGastoInicialAsync(InversionInicial gastoInicial)
        {
            await base.Contexto.InversionInicial.AddAsync(gastoInicial);

            return await base.Contexto.SaveChangesAsync();
        }

        // Recibe la fecha del análisis del que se quiere eliminar el gasto inicial y lo elimina en la base de datos el gasto inicial que coincida con el nombre pasada por parámetro.
        public async Task<int> EliminarGastoInicialAsync(DateTime fechaAnalisis, string nombreGastoInicial)
        {
            base.Contexto.InversionInicial.Remove(await base.Contexto.InversionInicial.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == nombreGastoInicial).FirstOrDefaultAsync());

            return await base.Contexto.SaveChangesAsync();
        }

        // Recibe la fecha del análisis del que se quiere obtener la sumatoria de los valores de los gastos iniciales del análisis pasado por parámetro.
        public async Task<decimal> ObtenerMontoTotalAsync(DateTime fechaAnalisis)
        {
            var total = await base.Contexto.InversionInicial.Where(x => x.FechaAnalisis == fechaAnalisis).SumAsync(x => x.Valor) ?? 0.0m;

            return total;
        }
    }
}
