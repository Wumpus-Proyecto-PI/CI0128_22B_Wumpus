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
            return porcentajeVentas * margen;
        }

        // método para calcular la meta de ventas de un producto en unidades
        // Parameter: porcentajeVentas del producto
        // Parameter: gastosFijosMensuales totales del negocio
        // Parameetr: gananciaMensual meta indicada por el usuario
        // Parameetr: margenPonderado calculado con la funcion @calcularMargen
        // Return: las unidades meta a vender de un producto para cumplir la meta de ganancia mensual
        public static int CalcularMetaVentasUnidad(decimal porcentajeVentas, decimal gastosFijosMensuales, decimal gananciaMensual, decimal margenPonderado)
        {
            int resultado;
            try
            {
                decimal temp = 0;
                temp = (porcentajeVentas * (gastosFijosMensuales + gananciaMensual)) / margenPonderado;
                resultado = Convert.ToInt32(Math.Ceiling(temp));
            } catch
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
        public static decimal CalcularGastosFijos(List<GastoFijoModel> gastosFijos, List<PuestoModel> puestos) { 
            decimal resultado = 0;
            PuestoModel puestoActual;
            for (int index = 0; index < gastosFijos.Count; index += 1) {
                if (gastosFijos[index].Nombre != "Beneficios de empleados" && gastosFijos[index].Nombre != "Salarios netos") {
                    resultado += gastosFijos.ElementAt(index).Monto;
                }
            }
            for (int index = 0; index < puestos.Count; index += 1)
            {
                puestoActual = puestos[index];
                resultado += (puestoActual.Plazas * puestoActual.SalarioBruto) * 12;

                resultado += puestoActual.Beneficios * 12;
               
            }
            return resultado;
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

        #region Flujo de caja
        // Metodo que calcula la meta en moneda en la base de datos
        public static decimal calcularTotalMetaMoneda(List<ProductoModel> productos, decimal montoGastosFijos, decimal GananciaMensual)
        {
            decimal total = 0.0m;

            foreach (ProductoModel productActual in productos)
            {
                // TODO: se debe enviar el costo variable total del producto en lugar de 100
                total += calcularMetaMoneda(productActual, montoGastosFijos, GananciaMensual);
            }
            return total;
        }

        // Metodo que calcula y retorna la meta en moneda de un producto
        public static decimal calcularMetaMoneda(ProductoModel productoActual, decimal montoGastosFijos, decimal GananciaMensual)
        {
            decimal margen = CalcularMargen(productoActual.Precio, productoActual.CostoVariable);

            decimal margenPonderado = CalcularMargenPonderado(productoActual.PorcentajeDeVentas, margen);

            int metaVentasUnidad = CalcularMetaVentasUnidad(productoActual.PorcentajeDeVentas, montoGastosFijos, /*this.analisis.*/GananciaMensual, margenPonderado);

            return CalcularMetaVentasMoneda(productoActual.Precio, metaVentasUnidad);
        }
        #endregion
    }
}
