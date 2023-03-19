using DocumentFormat.OpenXml.Presentation;
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

        public async Task<decimal> ObtenerTotalAnualAsync(DateTime fechaAnalisis)
        {
            return await Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).SumAsync(gastoFijo => gastoFijo.Monto) ?? 0.0m;
        }

        public async Task<decimal> ObtenerTotalMensualAsync(DateTime fechaAnalisis)
        {
            return await ObtenerTotalAnualAsync(fechaAnalisis) / 12;
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
            PI.EntityModels.GastoFijo gastoFijo = await this.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Salarios netos").FirstOrDefaultAsync();
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
                Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await Contexto.SaveChangesAsync();
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
            List<Puesto> puestos = await Contexto.Puestos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
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
            PI.EntityModels.GastoFijo gastoFijo = await this.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Seguridad social").FirstOrDefaultAsync();
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
                Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await Contexto.SaveChangesAsync();
        }

        // procedure reemplazado
        public async Task<decimal> ObtenerGastoSeguroSocialAsync(DateTime fechaAnalisis, decimal seguroSocial)
        {
            return await ObtenerSumaSalarios(fechaAnalisis) * seguroSocial;
        }

        // Crea o actualiza el gasto fijo de salarios netos
        public async Task<int> ActualizarPrestacionesAsync(DateTime fechaAnalisis, decimal prestaciones)
        {
            PI.EntityModels.GastoFijo gastoFijo = await this.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Prestaciones laborales").FirstOrDefaultAsync();
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
                Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await Contexto.SaveChangesAsync();
        }

        // procedure reemplazado
        public async Task<decimal> ObtenerGastoPrestacionesAsync(DateTime fechaAnalisis, decimal prestaciones)
        {
            return await ObtenerSumaSalarios(fechaAnalisis) * prestaciones;
        }

        // Crea o actualiza el gasto fijo de salarios netos
        public async Task<int> ActualizarBeneficiosAsync(DateTime fechaAnalisis)
        {
            PI.EntityModels.GastoFijo gastoFijo = await this.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Beneficios de empleados").FirstOrDefaultAsync();
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
                Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await Contexto.SaveChangesAsync();
        }

        // procedure reemplazado
        public async Task<decimal> ObtenerTotalBeneficiosAsync(DateTime fechaAnalisis)
        {
            List<Puesto> puestos = await Contexto.Puestos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
            decimal totalSalarios = 0.0m;
            foreach (var puesto in puestos)
            {
                totalSalarios = (puesto.CantidadPlazas * puesto.Beneficios) ?? 0.0m;
            }
            return totalSalarios * 12;
        }

        #endregion

        #region AnalisisHandler


        public async Task<int> ObtenerTipoAnalisisAsync(DateTime fechaAnalisis)
        {
            return (await Contexto.Configuracion.FindAsync(fechaAnalisis)).TipoNegocio;
        }

        public async Task<Configuracion> ObtenerConfigAnalisisAsync(DateTime fechaAnalisis)
        {
            return await Contexto.Configuracion.AsNoTracking().Where(config => config.FechaAnalisis == fechaAnalisis).FirstOrDefaultAsync();
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
