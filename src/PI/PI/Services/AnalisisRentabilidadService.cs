using PI.Models;
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

        // método para calcular el precio considerando la comision de ventas de un producto
        // Parameter: precio  del producto
        // Parameter: comision de ventas del producto
        // Return: el valor del precio considerando la comision de ventas de un producto
        public static decimal CalcularComision(decimal precio, decimal comision) 
        {
            decimal resultado = 0;

            if (comision > 0)
            {
                resultado = precio * (comision / 100);
            }

            return resultado;
        }

        // método para calcular la meta de ventas de un producto en unidades
        // Parameter: porcentajeVentas del producto
        // Parameter: gastosFijosMensuales totales del negocio
        // Parameetr: gananciaMensual meta indicada por el usuario
        // Parameetr: margenPonderadTotal se calcula sumando el margen ponderado de cada producto
        // Return: las unidades meta a vender de un producto para cumplir la meta de ganancia mensual
        public static decimal CalcularMetaVentasUnidad(decimal porcentajeVentas, decimal gastosFijosMensuales, decimal gananciaMensual, decimal margenPonderadoTotal)
        {
            decimal resultado;
            try
            {
                // meta de ventas en unidad = (ganancia mensual + gastos fijos mensuales) / margen ponderado total * porcentaje de ventas
                resultado = (gastosFijosMensuales + gananciaMensual) / margenPonderadoTotal * (porcentajeVentas / 100);
 
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
        public static decimal CalcularMetaVentasMoneda(decimal precio, decimal metaVentasUnidad)
        {
            return precio * metaVentasUnidad;
        }

        // Calcula el monto total de gastos fijos dado una lista de gastos fijos y otra de puestos.
        public static decimal CalcularGastosFijosTotalesMensuales(decimal gastosFijosAnuales) { 
            
            return gastosFijosAnuales / 12;
        }

        // Calcula el punto de equilibrio de un producto dado su monto de gastosFijos, precio y costoVariable
        public static decimal CalcularPuntoEquilibrio(decimal gastosFijosMensuales, decimal precio, decimal costoVariable) {
            decimal resultado = 0;
            if ((gastosFijosMensuales >= 0 && precio >= 0 && costoVariable >= 0)
                && (gastosFijosMensuales <= 999999999999999999.99M 
                && precio <= 999999999999999999.99M 
                && costoVariable <= 999999999999999999.99M))
            {
                decimal denominador = (precio - costoVariable);
                if (denominador != 0)
                {
                    resultado = gastosFijosMensuales / denominador;
                }
            }
            return resultado;
        }

        #region Total meta de ventas para flujo de caja
        // Metodo que calcula la meta en moneda en la base de datos
        public static decimal calcularTotalMetaMoneda(List<ProductoModel> productos, decimal montoGastosFijos, decimal GananciaMensual)
        {
            decimal margenPonderadoTotal = calcularMargenPonderadoTotal(productos);
            decimal total = 0.0m;

            foreach (ProductoModel productActual in productos)
            {
                total += calcularMetaMoneda(productActual, montoGastosFijos, GananciaMensual, margenPonderadoTotal);
            }
            return total;
        }

        // Metodo que calcula y retorna la meta en moneda de un producto
        public static decimal calcularMetaMoneda(ProductoModel productoActual, decimal montoGastosFijosMensuales, decimal GananciaMensual, decimal margenPonderadoTotal)
        {
            decimal metaVentasUnidad = CalcularMetaVentasUnidad(productoActual.PorcentajeDeVentas, montoGastosFijosMensuales, GananciaMensual, margenPonderadoTotal);

            return CalcularMetaVentasMoneda(productoActual.Precio, metaVentasUnidad);
        }

        // Se encarga de calcular el maregn ponderado total de todos los proudcto
        public static decimal calcularMargenPonderadoTotal(List<ProductoModel> productos)
        {
            decimal result = 0.0m;
            decimal margenActual = 0.0m;
            foreach (var productoActual in productos)
            {
                margenActual = CalcularMargen(productoActual.Precio, productoActual.CostoVariable);

                result += CalcularMargenPonderado(productoActual.PorcentajeDeVentas, margenActual);
            }
            return result;
        }
        #endregion
    }
}
