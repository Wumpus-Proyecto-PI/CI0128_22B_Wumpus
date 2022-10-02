using System;
using System.Collections.Generic;
using PI.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace PI.Handlers
{
    public class Handler
    {
        protected SqlConnection conexion;
        protected string rutaConexion;

        public Handler(string connectionStr)
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString(connectionStr);
            conexion = new SqlConnection(rutaConexion);
        }
        public Handler()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("BaseDeDatos");
            conexion = new SqlConnection(rutaConexion);
        }
        protected DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta,
            conexion);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }

        protected int enviarConsulta(string insert)
        {
            int filasAfectadas = 0;
            SqlCommand comando = new SqlCommand(insert, conexion);
            conexion.Open();
            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
            return filasAfectadas;
        }

        protected void enviarConsultaVoid(string insert)
        {
            SqlCommand comando = new SqlCommand(insert, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        public string obtenerNombreNegocio(DateTime fechaAnalisis)
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
    }
}