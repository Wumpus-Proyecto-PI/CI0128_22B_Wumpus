using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using PI.Models;
using PI.Handlers;
using System.Data.Common;
using PI.Controllers;

namespace PI.Handlers
{
    public class NegocioHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public NegocioHandler()
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

        // Crea una lista de los negocios existentes en la BD
        public List<NegocioModel> ObtenerNegocios()
        {
            List<NegocioModel> negocios = new List<NegocioModel>();
            AnalisisHandler analisisHandler = new AnalisisHandler();
            string consulta = "SELECT * FROM Negocio ORDER BY FechaCreacion DESC";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                negocios.Add(
                new NegocioModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    ID = Convert.ToInt32(columna["id"]),
                    CorreoUsuario = Convert.ToString(columna["correoUsuario"]),
                    Analisis = analisisHandler.ObtenerAnalisis(Convert.ToString(columna["id"])),
                    FechaCreacion = DateOnly.FromDateTime((DateTime)columna["fechacreacion"]) 
                }
                );
            }
            return negocios;
        }

        // Obtiene el siguiente ID para el negocio según el mayor ID existente actualmente +1.
        public int getNextID()
        {
            string consulta = "SELECT MAX(ID) AS ID FROM Negocio";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            int result = 1;  // Valor por omisión (inicial)
            // Verifica que haya devuelto una tupla
            if (tablaResultado.Rows.Count > 0)
            {
                // Verifica que no sea nulo
                DataRow lastID = tablaResultado.Rows[0];
                if (!lastID.IsNull("ID"))
                {
                    result = Convert.ToInt32(lastID["ID"]) + 1;

                }
            }
            return result;
        }

        // Inserta el nuevo negocio a la base de datos.
        public void IngresarNegocio(string Nombre, string tipo) 
        {
            AnalisisHandler analisisHandler = new AnalisisHandler();

            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            int nextID = getNextID();
            
            string consulta = "INSERT INTO Negocio (ID,Nombre,FechaCreacion) VALUES (" + nextID + ",'" + Nombre + "', '" + sqlFormattedDate + "');";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();

            analisisHandler.IngresarAnalisis(Convert.ToString(nextID), tipo);
        }
    }
}
