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
        public void ingresarGastoFijo(string Nombre, string monto, DateTime fechaAnalisis)
        {
            // TODO arreglar el datetime para que esté asociado al análisis realmente.
            string consulta = "INSERT INTO GASTO_FIJO (nombre, fechaAnalisis, monto) " +
                "VALUES ('" + Nombre + ", '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +"', " + monto + ");";

            enviarConsulta(consulta);
        }

        public void existeGastoFijoEnBase ()
        {

        }

        public void actualizarGastoFijo()
        {

        }

        public decimal obtenerSalarios(DateTime fechaAnalisis)
        {
            decimal totalSalarios = 0.0m;

            string consulta = "select SUM(cantidadPlazas*SalarioBruto) as TotalSalarios FROM PUESTO WHERE "
                + "fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"; ;
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            totalSalarios = Convert.ToDecimal(tablaResultado.Rows[0]["TotalSalarios"]); 

            return totalSalarios;
        }


    }
}