using DocumentFormat.OpenXml.InkML;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public class AnalisisHandler : EntityHandler
    {
        public AnalisisHandler(DataBaseContext context) : base(context) { }

        public async Task<Analisis> ObtenerUnAnalisis(DateTime fechaCreacion)
        {
            return await Contexto.Analisis.Include(x => x.Configuracion).FirstOrDefaultAsync(x => x.FechaCreacion == fechaCreacion);
        }

        public async Task<Analisis> ObtenerAnalisisMasReciente(int idNegocio)
        {

            return await Contexto.Analisis.AsNoTracking().Where(x => x.IdNegocio == idNegocio)
                .OrderByDescending(x => x.FechaCreacion).Take(1).FirstOrDefaultAsync();
        }

        public async Task<List<Analisis>> ObtenerAnalisis(int idNegocio)
        {
            List<Analisis> lista = await Contexto.Analisis.AsNoTracking().Where(x => x.IdNegocio == idNegocio).Include(x => x.Configuracion).OrderBy(x => x.FechaCreacion).ToListAsync();
            return lista;
        }

        public async Task<DateTime> UltimaFechaCreacion(int idNegocio)
        {
            return await Contexto.Analisis.Join(Contexto.Negocios, A => A.IdNegocio, N => N.Id, (A, N) => new { Analisis = A, Negocios = N }).Where(x => x.Analisis.IdNegocio == idNegocio).MaxAsync(x => x.Analisis.FechaCreacion);
        }

        public async Task<int> ObtenerEstadoAnalisis(DateTime fechaCreacion)
        {
            return await Contexto.Analisis.Join(Contexto.Configuracion, A => A.FechaCreacion, C => C.FechaAnalisis, (A, C) => new { Analisis = A, Configuracion = C }).Where(x => x.Configuracion.FechaAnalisis == fechaCreacion).Select(x => x.Configuracion.TipoNegocio).FirstOrDefaultAsync();
        }

        public async Task<Configuracion> CrearConfigPorDefecto(DateTime fecha, string tipo)
        {
            int tipoAnalisis = 0;
            if (tipo != "Emprendimiento")
            {
                tipoAnalisis = 1;
            }

            Configuracion config = new Configuracion
            {
                FechaAnalisis = fecha,
                TipoNegocio = tipoAnalisis,
                PorcentajePl = 0.0M,
                PorcentajeSs = 0.0M
            };

            await Contexto.Configuracion.AddAsync(config);
            await Contexto.SaveChangesAsync();
            return config;

        }
        public async Task<DateTime> IngresarAnalisis(int idNegocio, string tipo)
        {
            DateTime fecha = DateTime.Now;
            Analisis analisis = new Analisis
            {
                IdNegocio = idNegocio,
                FechaCreacion = fecha
            };
            await Contexto.Analisis.AddAsync(analisis);
            await CrearConfigPorDefecto(fecha, tipo);

            await Contexto.SaveChangesAsync();
            return fecha;
        }

        public async Task ActualizarConfiguracionAnalisis(Configuracion config)
        {
            Configuracion ConfigPorActualizar =await Contexto.Configuracion.Where(x => x.FechaAnalisis == config.FechaAnalisis).FirstOrDefaultAsync();
            if (config.PorcentajeSs >= 0)
            {
                ConfigPorActualizar.PorcentajeSs = config.PorcentajeSs;
            }
            if (config.PorcentajePl >= 0)
            {
                ConfigPorActualizar.PorcentajePl = config.PorcentajePl;
            }
            await Contexto.SaveChangesAsync();
        }

        public async Task ActualizarGanaciaMensual(decimal monto, DateTime fecha)
        {
            Analisis AnalisisPorActualizar = await Contexto.Analisis.Where(x => x.FechaCreacion == fecha).FirstOrDefaultAsync();
            AnalisisPorActualizar.GananciaMensual = monto;
            await Contexto.SaveChangesAsync();
        }

        public async Task<decimal?> ObtenerGananciaMensual(DateTime fechaCreacion)
        {
            return await Contexto.Analisis.Where(x => x.FechaCreacion == fechaCreacion).Select(x => x.GananciaMensual).FirstOrDefaultAsync();
        }

        public async Task EliminarAnalisis(DateTime fechaCreacion)
        {
            Analisis AnalisisPorBorrar = await Contexto.Analisis.Where(x => x.FechaCreacion == fechaCreacion).FirstOrDefaultAsync();
            Contexto.Analisis.Remove(AnalisisPorBorrar);
            await Contexto.SaveChangesAsync();

        }

        public async Task<Configuracion> ObtenerConfigAnalisis(DateTime fechaAnalisis)
        {
            return await Contexto.Configuracion.Where(x => x.FechaAnalisis == fechaAnalisis).FirstOrDefaultAsync();
        }

    }
}