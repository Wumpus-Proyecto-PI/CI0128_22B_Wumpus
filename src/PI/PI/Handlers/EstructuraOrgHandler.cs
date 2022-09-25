using System;
using System.Collections.Generic;
using PI.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.Configuration;
namespace PI.Handlers
{
    public class EstructuraOrgHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public EstructuraOrgHandler()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("BaseDeDatos");
            conexion = new SqlConnection(rutaConexion);
        }
        private DataTable CrearTablaConsulta(string consulta)
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
        public List<NegocioModel> ObtenerNegocios()
        {
            List<NegocioModel> negocios = new List<NegocioModel>();
            string consulta = "SELECT * FROM NEGOCIO";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                negocios.Add(
                new NegocioModel
                {
                    id = Convert.ToInt32(columna["id"]),
                    nombre = Convert.ToString(columna["nombre"]),
                    correoUsuario = Convert.ToString(columna["correoUsuario"]),
                });
            }
            return negocios;
        }
    }
}