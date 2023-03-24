using PI.EntityModels;
using PI.EntityHandlers;
using System.Diagnostics.Metrics;

namespace PI.Services
{
    public class FlujoCajaService
    {
        FlujoDeCajaHandler? FlujoDeCajaHandler = null;
        GastoFijoHandler? GastoFijoHandler = null;
        public FlujoCajaService(FlujoDeCajaHandler flujoDeCajaHandler, GastoFijoHandler gastoFijoHandler) 
        {
            FlujoDeCajaHandler = flujoDeCajaHandler;
            GastoFijoHandler = gastoFijoHandler;
        }

        public async Task ActualizarFlujoMensualAsync(List<Mes> meses, List<string> flujosMensuales)
        {
            for (int mes = 0; mes < meses.Count; ++mes)
            {
                // Obtiene los ingresos del mes.
                decimal totalIngresoPorMes = await FlujoDeCajaHandler.ObtenerMontoTotalDeIngresosPorMesAsync(meses[mes].Nombre, meses[mes].FechaAnalisis);

                // Obtiene los egresos del mes.
                decimal totalEgresoPorMes = await FlujoDeCajaHandler.ObtenerMontoTotalDeEgresosPorMesAsync(meses[mes].Nombre, meses[mes].FechaAnalisis);

                // Suma los gastos fijos y la inversión inicial a los egresos.
                decimal gastosFijosTotalMensual = await GastoFijoHandler.ObtenerTotalMensualAsync(meses[mes].Nombre, meses[mes].FechaAnalisis);
                decimal totalInversionInicialMes = meses[mes].InversionPorMes ?? 0.0m;

                string flujoActual = FlujoCajaService.CalcularFlujoMensual(totalIngresoPorMes, totalEgresoPorMes, gastosFijosTotalMensual, totalInversionInicialMes);
                flujosMensuales[mes] = flujoActual;
            }
        }


        /// <param>totalIngresoPorMes</param> el código que llama a la funcion envía el valor leído de la base de datos.
        /// <param>totalEgresoPorMes</param> el código que llama a la funcion envía el valor leído de la base de datos.
        public static string CalcularFlujoMensual(decimal totalIngresoPorMes, decimal totalEgresoPorMes, decimal gastosFijosTotalMensual, decimal totalInversionInicialMes)
        {
            totalEgresoPorMes += gastosFijosTotalMensual;
            totalEgresoPorMes += totalInversionInicialMes;

            return FormatManager.ToFormatoEstadistico(totalIngresoPorMes - totalEgresoPorMes);
        }

        public static decimal CalcularEgresosTipo(string Tipo, List<Egreso> Egresos)
        {
            decimal total = 0;
            for (int i = 0; i < Egresos.Count; i += 1) {
                if (Egresos[i].Tipo == Tipo) {
                    total += Egresos[i].Monto;
                }
            }
            return total;
        }

        public static decimal CalcularIngresosTipo(string Tipo, List<Ingreso> Ingresos)
        {
            decimal total = 0;
            for (int i = 0; i < Ingresos.Count; i += 1)
            {
                if (Ingresos[i].Tipo == Tipo)
                {
                    total += Ingresos[i].Monto;
                }
            }
            return total;
        }

        public async Task ActualizarTotalesIngresosAsync(List<Mes> meses, List<string> ingresosTotales)
        {
            for (int i = 0; i < meses.Count; ++i)
            {
                ingresosTotales[i] = FormatManager.ToFormatoEstadistico(await FlujoDeCajaHandler.ObtenerMontoTotalDeIngresosPorMesAsync(meses[i].Nombre, meses[i].FechaAnalisis));
            }
        }

        public async Task ActualizarTotalesEgresosAsync(List<Mes> meses, List<string> egresosTotales, decimal totalGastosFijosMensual)
        {
            for (int i = 0; i < meses.Count; ++i)
            {
                egresosTotales[i] = FormatManager.ToFormatoEstadistico(await CalcularEgresoMensualTotalAsync(meses[i], totalGastosFijosMensual));
            }
        }

        public async Task<decimal> CalcularEgresoMensualTotalAsync(Mes mes, decimal totalGastosFijosMensual)
        {
            decimal total = 0;

            decimal totalEgresos = await FlujoDeCajaHandler.ObtenerMontoTotalDeEgresosPorMesAsync(mes.Nombre, mes.FechaAnalisis);
            decimal inversionInicialDelMes = mes.InversionPorMes ?? 0.0m;

            total = totalEgresos + inversionInicialDelMes + totalGastosFijosMensual;

            return total;
        }
    }
}
