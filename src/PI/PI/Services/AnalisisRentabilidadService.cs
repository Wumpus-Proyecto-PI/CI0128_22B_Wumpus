﻿using PI.Models;
using PI.Handlers;

namespace PI.Services
{
    public class AnalisisRentabilidadService
    {
        // método para calcular el margen de un producto
        // Parameter: precio  del producto
        // Parameter: costoVariable total del producto
        // Return: el valor del margen del producto
        public decimal CalcularMargen(decimal precio, decimal costoVariable)
        {
            return precio - costoVariable;
        }

        // método para calcular el margen ponderado de un producto
        // Parameter: porcentajeVentas  del producto
        // Parameter: margen del producto
        // Return: el valor del margen ponderado del producto
        public decimal CalcularMargenPonderado(decimal porcentajeVentas, decimal margen)
        {
            return porcentajeVentas * margen;
        }

        // método para calcular la meta de ventas de un producto en unidades
        // Parameter: porcentajeVentas del producto
        // Parameter: gastosFijosMensuales totales del negocio
        // Parameetr: gananciaMensual meta indicada por el usuario
        // Parameetr: margenPonderado calculado con la funcion @calcularMargen
        // Return: las unidades meta a vender de un producto para cumplir la meta de ganancia mensual
        public int CalcularMetaVentasUnidades(decimal porcentajeVentas, decimal gastosFijosMensuales, decimal gananciaMensual, decimal margenPonderado)
        {
            decimal resultado = (porcentajeVentas * (gastosFijosMensuales + gananciaMensual)) / margenPonderado;
            return Convert.ToInt32(resultado);
        }

        // método para calcular la meta de ventas de un producto en unidades
        // Parameter: precio  del producto
        // Parameter: metaVentasUnidad que se calcula con el metodo @calcularMetaVentasUnidades
        // Return: la meta a vender en moneda de un producto para cumplir la meta de ganancia mensual
        public decimal CalcularMetaVentasUnidades(decimal precio, int metaVentasUnidad)
        {
            return precio * metaVentasUnidad;
        }

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

                for (int indexBeneficios = 0; indexBeneficios < puestoActual.Beneficios.Count
                     ; indexBeneficios += 1) 
                {
                    resultado += (puestoActual.Beneficios[indexBeneficios].monto
                          * puestoActual.Beneficios[indexBeneficios].plazasPorBeneficio) * 12;
                }
            }
            return resultado;
        }

        public static decimal CalcularMetaEnMoneda(decimal precio, int metaEnUnidades) { 
            return precio*metaEnUnidades;
        }

        public static decimal CalcularPuntoEquilibrio(decimal gastosFijos, decimal precio, decimal costoVariable) {
            return gastosFijos / (precio - costoVariable);
        }



    }
}
