using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using PI.Controllers;
using System.Data;
using System.Data.SqlClient;
using PI.Models;
using PI.Views.Shared.Components.GastoFijo;

namespace PI.Handlers
{
    public class GastoFijoHandler : Handler
    {
        public GastoFijoHandler(): base() { }

        // Crea una lista de los gastos fijos existentes en la BD
        public List<GastoFijoModel> ObtenerGastosFijos(DateTime fechaAnalisis)
        {
            List<GastoFijoModel> gastosFijos = new List<GastoFijoModel>();
            string consulta = "SELECT * FROM Gasto_Fijo WHERE "
                + "fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                gastosFijos.Add(
                new GastoFijoModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Monto = Convert.ToDecimal(columna["monto"]),
                    orden = Convert.ToInt32(columna["orden"])
                }
                );
            }
            return gastosFijos;
        }
        public int getNextOrden()
        {
            string consulta = "SELECT MAX(orden) AS orden FROM Gasto_Fijo";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            int result = 1;  // Valor por omisión (inicial)
            // Verifica que haya devuelto una tupla
            if (tablaResultado.Rows.Count > 0)
            {
                // Verifica que no sea nulo
                DataRow lastOrder = tablaResultado.Rows[0];
                if (!lastOrder.IsNull("orden"))
                {
                    result = Convert.ToInt32(lastOrder["orden"]) + 1;
                }
            }
            return result;
        }

        // Inserta el nuevo gasto fijo a la BD.
        public void ingresarGastoFijo(string nombreAnterior, string Nombre, string monto, DateTime fechaAnalisis)
        {
            // TODO arreglar el datetime para que est� asociado al an�lisis realmente.
            string consulta = "EXECUTE insertarGastoFijo '"
                + nombreAnterior + "', '"
                + Nombre + "', '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") 
                + "', " + monto + ","
                + getNextOrden() + ";";

            enviarConsulta(consulta);
        }

        public void eliminarGastoFijo(string Nombre, DateTime fechaAnalisis)
        {
            // TODO arreglar el datetime para que est� asociado al an�lisis realmente.
            string consulta = "EXECUTE eliminarGastoFijo '"
                + Nombre + "', '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "';";
            enviarConsulta(consulta);
        }

        public decimal obtenerTotalMensual(DateTime fechaAnalisis)
        {
            decimal totalMensual = 0.0m;

            string consulta = "EXEC obtSumGastosFijos '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (!tablaResultado.Rows[0].IsNull("totalMensual"))
            {
                totalMensual = Convert.ToDecimal(tablaResultado.Rows[0]["totalMensual"]);
            }

            return totalMensual;
        }

        public string obtenerNombreNegocio (DateTime fechaAnalisis)
        {
            string consulta = "EXEC obtNombreNegocio '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            string nombreNegocio = "";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0 && !tablaResultado.Rows[0].IsNull("nombre"))
            {
                nombreNegocio = Convert.ToString(tablaResultado.Rows[0]["nombre"]);
            }
            else
            {
                nombreNegocio = "Sin nombre";
            }

            return nombreNegocio;
        }

        public decimal obtenerSeguroSocial (DateTime fechaAnalisis, decimal porcentajeSeguroSocial)
        {
            decimal totalSalarios = 0.0m;

            string consulta = "SELECT dbo.obtGastoSeguroSocial ('" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '0.2') AS SeguroSocial";
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            if (!tablaResultado.Rows[0].IsNull("SalariosNeto"))
            {
                totalSalarios = Convert.ToDecimal(tablaResultado.Rows[0]["SalariosNeto"]);
            }

            return totalSalarios;
        }

        public void actualizarSalariosNeto(DateTime fechaAnalisis, decimal seguroSocial, decimal prestaciones)
        {
            string consulta = "EXEC actualizarSalariosNeto '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + seguroSocial.ToString() + "', '" + prestaciones.ToString() + "'";
            enviarConsulta(consulta);
        }

        public void actualizarSeguroSocial(DateTime fechaAnalisis, decimal seguroSocial)
        {
            string consulta = "EXEC actualizarGastoSeguroSocial '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + seguroSocial.ToString() + "'";
            enviarConsulta(consulta);
        }
        public void actualizarPrestaciones(DateTime fechaAnalisis, decimal prestaciones)
        {
            string consulta = "EXEC actualizarGastoPrestaciones '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + prestaciones.ToString() + "'";
            enviarConsulta(consulta);
        }

        public void actualizarBeneficios(DateTime fechaAnalisis)
        {
            string consulta = "EXEC actualizarBeneficios '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsulta(consulta);
        }
    }
}