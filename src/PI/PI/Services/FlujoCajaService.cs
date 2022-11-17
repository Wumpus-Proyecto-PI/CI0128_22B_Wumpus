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



    }
}
