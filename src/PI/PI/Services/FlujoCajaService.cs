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
            decimal totalIngresoPorMes = flujoDeCajaHandler.obtenerMontoTotalDeIngresosPorMes(mesModel.NombreMes, mesModel.FechaAnalisis);

            decimal totalEgresoPorMes = flujoDeCajaHandler.obtenerMontoTotalDeEgresosPorMes(mesModel);

            return totalIngresoPorMes - totalEgresoPorMes;
        }
        public static void InicializarFlujosMensuales(List<MesModel> meses, ref List<string> flujoMensual)
        {
            for (int mes = 0; mes < meses.Count; ++mes)
            {
                flujoMensual.Add((FormatManager.ToFormatoEstadistico(FlujoCajaService.CalcularFlujoMensual(meses[mes]))));
            }
        }

        public static List<string> ActualizarFlujosMensuales(List<MesModel> meses)
        {
            List<string> flujoMensual = new();
            for (int mes = 0; mes < meses.Count; ++mes)
            {
                flujoMensual.Add(FormatManager.ToFormatoEstadistico(FlujoCajaService.CalcularFlujoMensual(meses[mes])));
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
