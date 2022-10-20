using PI.Models;
using PI.Handlers;

namespace PI.Services
{
    public class AnalisisRentabilidadService
    {
        // método para calcular el margen de un producto
        // Parameter: precio  del producto
        // Parameter: costoVariable total del producto
        // Return: el valor del margen del producto
        public decimal calcularMargen(decimal precio, decimal costoVariable)
        {
            return precio - costoVariable;
        }

        // método para calcular el margen ponderado de un producto
        // Parameter: porcentajeVentas  del producto
        // Parameter: margen del producto
        // Return: el valor del margen ponderado del producto
        public decimal calcularMargenPonderado(decimal porcentajeVentas, decimal margen)
        {
            return porcentajeVentas * margen;
        }

        // método para calcular la meta de ventas de un producto en unidades
        // Parameter: porcentajeVentas del producto
        // Parameter: gastosFijosMensuales totales del negocio
        // Parameetr: gananciaMensual meta indicada por el usuario
        // Parameetr: margenPonderado calculado con la funcion @calcularMargen
        // Return: las unidades meta que a vender
        public int calcularMetaVentasUnidades(decimal porcentajeVentas, decimal gastosFijosMensuales, decimal gananciaMensual, decimal margenPonderado)
        {
            decimal resultado = (porcentajeVentas * (gastosFijosMensuales + gananciaMensual)) / margenPonderado
            return Convert.ToInt32(resultado);
        }
    }
}
