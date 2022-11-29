using PI.Models;
using PI.Handlers;
using System;
using System.Diagnostics.Metrics;

namespace PI.Services
{
    public class FlujoCajaService
    {

        // Método para calcular el flujo mensual del negocio
        public static decimal CalcularFlujoMensual(MesModel mesModel)
        {
            FlujoDeCajaHandler flujoDeCajaHandler = new();

            // Obtiene los ingresos del mes.
            decimal totalIngresoPorMes = flujoDeCajaHandler.ObtenerMontoTotalDeIngresosPorMes(mesModel.NombreMes, mesModel.FechaAnalisis);
            // Obtiene los egresos del mes.
            decimal totalEgresoPorMes = flujoDeCajaHandler.ObtenerMontoTotalDeEgresosPorMes(mesModel);

            // Suma los gastos fijos y la inversión inicial a los egresos.
            GastoFijoHandler gastoFijoHandler = new();
            totalEgresoPorMes += gastoFijoHandler.obtenerTotalAnual(mesModel.FechaAnalisis) / 12;
            totalEgresoPorMes += flujoDeCajaHandler.ObtenerInversionDelMes(mesModel);

            return totalIngresoPorMes - totalEgresoPorMes;
        }

        // Retorna una lista con el flujo mensual de cada mes recalculado.
        public static List<string> ActualizarFlujosMensuales(List<MesModel> meses)
        {
            List<string> flujoMensual = new();
            for (int mes = 0; mes < meses.Count; ++mes)
            {
                flujoMensual.Add(FormatManager.ToFormatoEstadistico(CalcularFlujoMensual(meses[mes])));
            }
            return flujoMensual;
        }

        public static decimal CalcularEgresosTipo(string Tipo, List<EgresoModel> Egresos)
        {
            decimal total = 0;
            for (int i = 0; i < Egresos.Count; i += 1) {
                if (Egresos[i].Tipo == Tipo) {
                    total += Egresos[i].Monto;
                }
            }
            return total;
        }

        public static decimal CalcularIngresosTipo(string Tipo, List<IngresoModel> Ingresos)
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



    }
}
