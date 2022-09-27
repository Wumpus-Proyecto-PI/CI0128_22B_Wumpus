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
    public class GastoFijoHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public GastoFijoHandler()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion = builder.Configuration.GetConnectionString("WumpusTEST");
            conexion = new SqlConnection(rutaConexion);
        }
        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }
        // Crea una lista de los gastos fijos existentes en la BD
        public List<GastoFijoModel> ObtenerGastosFijos()
        {
            List<GastoFijoModel> gastosFijos = new List<GastoFijoModel>();
            string consulta = "SELECT * FROM Gasto_Fijo";
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
            string consulta = "INSERT INTO Gasto_fijo (nombre, fechaAnalisis, monto) VALUES ('" + Nombre + "','2002-09-09 12:00:00 AM,'" + monto + ");";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        public double obtenerSalarios()
        {
            double totalSalarios = 0.0;

            string consulta = "select SUM(cantidadPlazas*SalarioBruto) as TotalSalarios FROM PUESTO";
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            totalSalarios = Convert.ToDouble(tablaResultado.Rows[0]["TotalSalarios"]); 

            return totalSalarios;
        }


    }
}