using PI.Models;
using PI.Handlers;
using System;
namespace PI.Services
{
    public class FlujoCajaService
    {
        // Método para calcular el flujo mensual del negocio
        public static decimal CalcularFlujoMensual(decimal totalIngresos, decimal totalEgresos)
        {
            return totalIngresos - totalEgresos;
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
