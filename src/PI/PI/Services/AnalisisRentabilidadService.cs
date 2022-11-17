﻿using PI.Models;
using PI.Handlers;
using System;
namespace PI.Services
{
    public class AnalisisRentabilidadService
    {
        // método para calcular el margen de un producto
        // Parameter: precio  del producto
        // Parameter: costoVariable total del producto
        // Return: el valor del margen del producto
        public static decimal CalcularMargen(decimal precio, decimal costoVariable)
        {
            return precio - costoVariable;
        }

        // método para calcular el margen ponderado de un producto
        // Parameter: porcentajeVentas  del producto
        // Parameter: margen del producto
        // Return: el valor del margen ponderado del producto
        public static decimal CalcularMargenPonderado(decimal porcentajeVentas, decimal margen)
        {
            return (porcentajeVentas / 100) * margen;
        }

        // método para calcular la meta de ventas de un producto en unidades
        // Parameter: porcentajeVentas del producto
        // Parameter: gastosFijosMensuales totales del negocio
        // Parameetr: gananciaMensual meta indicada por el usuario
        // Parameetr: margenPonderadTotal se calcula sumando el margen ponderado de cada producto
        // Return: las unidades meta a vender de un producto para cumplir la meta de ganancia mensual
        public static int CalcularMetaVentasUnidad(decimal porcentajeVentas, decimal gastosFijosMensuales, decimal gananciaMensual, decimal margenPonderadoTotal)
        {
            int resultado;
            try
            {
                decimal temp = 0;
                // meta de ventas en unidad = (ganancia mensual + gastos fijos mensuales) / margen ponderado total * porcentaje de ventas
                temp = (gastosFijosMensuales + gananciaMensual) / margenPonderadoTotal * (porcentajeVentas / 100);
                resultado = Convert.ToInt32(Math.Ceiling(temp));
            } catch (DivideByZeroException)
            {
                resultado = 0;
            }
            return resultado;
        }

        // método para calcular la meta de ventas de un producto en moneda
        // Parameter: precio  del producto
        // Parameter: metaVentasUnidad que se calcula con el metodo @calcularMetaVentasUnidades
        // Return: la meta a vender en moneda de un producto para cumplir la meta de ganancia mensual
        public static decimal CalcularMetaVentasMoneda(decimal precio, int metaVentasUnidad)
        {
            return precio * metaVentasUnidad;
        }

        // Calcula el monto total de gastos fijos dado una lista de gastos fijos y otra de puestos.
        public static decimal CalcularGastosFijosTotalesMensuales(decimal gastosFijosAnuales) { 
            
            return gastosFijosAnuales / 12;
        }

        // Calcula el punto de equilibrio de un producto dado su monto de gastosFijos, precio y costoVariable
        public static decimal CalcularPuntoEquilibrio(decimal gastosFijos, decimal precio, decimal costoVariable) {
            decimal resultado = 0;
            decimal denominador = (precio - costoVariable);
            if (denominador > 0) {
                resultado = gastosFijos / denominador;
            }
            return resultado;
        }



    }
}
