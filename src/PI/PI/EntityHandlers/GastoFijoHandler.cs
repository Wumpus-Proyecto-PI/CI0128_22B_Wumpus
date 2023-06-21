using Microsoft.EntityFrameworkCore;
using System.Linq;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public class GastoFijoHandler : EntityHandler
    {
        public GastoFijoHandler(DataBaseContext context) : base(context) { }

        public async Task<List<GastoFijo>> ObtenerGastosFijosAsync(DateTime fechaAnalisis)
        {
            return await base.Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).ToListAsync();
        }

        public async Task<decimal> ObtenerTotalAnualAsync(DateTime fechaAnalisis)
        {
            return await base.Contexto.GastosFijos.Where(gastoFijo => gastoFijo.FechaAnalisis == fechaAnalisis).SumAsync(gastoFijo => gastoFijo.Monto) ?? 0.0m;
        }

        public async Task<decimal> ObtenerTotalMensualAsync(DateTime fechaAnalisis)
        {
            return await ObtenerTotalAnualAsync(fechaAnalisis) / 12;
        }

        public async Task<int> IngresarGastoFijoAsync(string nombreAnterior, string Nombre, decimal monto, DateTime fechaAnalisis)
        {

            PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == nombreAnterior).FirstOrDefaultAsync();

            if (Convert.ToDecimal(monto) < 0)
            {
                throw new Exception("El valor del monto debe ser un número positivo", new ArgumentOutOfRangeException());
            }

            if (gastoFijo != null)
            {
                Contexto.GastosFijos.Remove(gastoFijo);

                GastoFijo gastoNuevo = new GastoFijo
                {
                    Nombre = Nombre,
                    FechaAnalisis = fechaAnalisis,
                    Monto = monto,
                };
                await base.Contexto.GastosFijos.AddAsync(gastoNuevo);
            }
            else
            {
                GastoFijo gastoNuevo = new GastoFijo
                {
                    Nombre = Nombre,
                    FechaAnalisis = fechaAnalisis,
                    Monto = monto,
                };
                await base.Contexto.GastosFijos.AddAsync(gastoNuevo);
            }

            return await base.Contexto.SaveChangesAsync();
        }

        public async Task<int> EliminarGastoFijoAsync(string nombre, DateTime fechaAnalisis)
        {
            base.Contexto.GastosFijos.Remove(await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == nombre).FirstOrDefaultAsync());
            return await base.Contexto.SaveChangesAsync();
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
            PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Salarios netos").FirstOrDefaultAsync();
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
                };
                base.Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await base.Contexto.SaveChangesAsync();
        }

        // procedure reemplazado
        public async Task<decimal> ObtenerTotalSalariosNetoAsync(DateTime fechaAnalisis, decimal seguroSocial, decimal Prestaciones)
        {

            decimal sumaSalarios = await ObtenerSumaSalarios(fechaAnalisis);
            decimal gastoSs =  ObtenerGastoSeguroSocial(fechaAnalisis, sumaSalarios, seguroSocial);
            decimal gastoPl =  ObtenerGastoPrestaciones(fechaAnalisis, sumaSalarios, Prestaciones);

            return sumaSalarios - gastoSs - gastoPl;
        }
        // procedure reemplazado
        public async Task<decimal> ObtenerSumaSalarios(DateTime fechaAnalisis)
        {
            List<Puesto> puestos = await Contexto.Puestos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
            decimal totalSalarios = 0.0m;
            foreach (var puesto in puestos)
            {
                totalSalarios += (puesto.CantidadPlazas * puesto.SalarioBruto) ?? 0.0m;
            }
            return totalSalarios * 12;
        }
        // procedure reemplazado
        public decimal ObtenerGastoSeguroSocial(DateTime fechaAnalisis, decimal sumaSalarios, decimal porcentajeSs)
        {
            return sumaSalarios * porcentajeSs;
        }
        // procedure reemplazado
        public  decimal ObtenerGastoPrestaciones(DateTime fechaAnalisis, decimal sumaSalarios, decimal porcentajePl)
        {
            return sumaSalarios * porcentajePl;
        }


        // Crea o actualiza el gasto fijo de seguro social
        public async Task<int> ActualizarSeguroSocialAsync(DateTime fechaAnalisis, decimal seguroSocial)
        {
            PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Seguridad social").FirstOrDefaultAsync();
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
                };
                base.Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await base.Contexto.SaveChangesAsync();
        }

        // procedure reemplazado
        public async Task<decimal> ObtenerGastoSeguroSocialAsync(DateTime fechaAnalisis, decimal seguroSocial)
        {
            return await ObtenerSumaSalarios(fechaAnalisis) * seguroSocial;
        }

        // Crea o actualiza el gasto fijo de salarios netos
        public async Task<int> ActualizarPrestacionesAsync(DateTime fechaAnalisis, decimal prestaciones)
        {
            PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Prestaciones laborales").FirstOrDefaultAsync();
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
                };
                base.Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await base.Contexto.SaveChangesAsync();
        }

        // procedure reemplazado
        public async Task<decimal> ObtenerGastoPrestacionesAsync(DateTime fechaAnalisis, decimal prestaciones)
        {
            return await ObtenerSumaSalarios(fechaAnalisis) * prestaciones;
        }

        // Crea o actualiza el gasto fijo de salarios netos
        public async Task<int> ActualizarBeneficiosAsync(DateTime fechaAnalisis)
        {
            PI.EntityModels.GastoFijo gastoFijo = await base.Contexto.GastosFijos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == "Beneficios de empleados").FirstOrDefaultAsync();
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
                };
                base.Contexto.GastosFijos.Add(gastoNuevo);
            }
            return await base.Contexto.SaveChangesAsync();
        }

        // procedure reemplazado
        public async Task<decimal> ObtenerTotalBeneficiosAsync(DateTime fechaAnalisis)
        {
            List<Puesto> puestos = await base.Contexto.Puestos.AsNoTracking().Where(x => x.FechaAnalisis == fechaAnalisis).ToListAsync();
            decimal totalBeneficios = 0.0m;
            foreach (var puesto in puestos)
            {
                totalBeneficios += (puesto.CantidadPlazas * puesto.Beneficios) ?? 0.0m;
            }
            return totalBeneficios * 12;
        }
    }
}
