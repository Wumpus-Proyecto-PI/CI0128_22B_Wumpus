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
                }
                );
            }
            return gastosFijos;
        }


        // Inserta el nuevo gasto fijo a la BD.
        public void ingresarGastoFijo(string nombreAnterior, string Nombre, string monto, DateTime fechaAnalisis)
        {
            // TODO arreglar el datetime para que esté asociado al análisis realmente.
            string consulta = "EXECUTE insertarGastoFijo '"
                + nombreAnterior + "', '"
                + Nombre + "', '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") 
                + "', " + monto + ";";

            enviarConsulta(consulta);
        }

        public void eliminarGastoFijo(string Nombre, DateTime fechaAnalisis)
        {
            // TODO arreglar el datetime para que esté asociado al análisis realmente.
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
    }
}