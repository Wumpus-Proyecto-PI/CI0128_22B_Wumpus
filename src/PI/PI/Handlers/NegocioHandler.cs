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
    // Handler de negocio
    public class NegocioHandler : Handler
    {
        public NegocioHandler() : base() { }

        // Crea una lista de los negocios existentes en la BD
        // (Retorna la lista con los negocios de la base)
        public List<NegocioModel> ObtenerNegocios(string idUsuario)
        {
            List<NegocioModel> negocios = new List<NegocioModel>();
            AnalisisHandler analisisHandler = new AnalisisHandler();
            string consulta = $"exec ObtenerNegocios @idUsuario = '{idUsuario}'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            // Crea los modelos de negocio correspondientes segun la tabla obtenida de la base de datos
            foreach (DataRow columna in tablaResultado.Rows)
            {
                DateTime fechaUltAnalisis = analisisHandler.UltimaFechaCreacion(Convert.ToString(columna["id"]));
                negocios.Add(
                new NegocioModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    ID = Convert.ToInt32(columna["id"]),
                    idUsuario = Convert.ToString(columna["idUsuario"]),
                    Analisis = analisisHandler.ObtenerAnalisis(Convert.ToInt32(columna["id"])),
                    FechaCreacion = DateOnly.FromDateTime((DateTime)columna["fechacreacion"]),
                    TipoUltimoAnalisis = analisisHandler.ObtenerTipoAnalisis(fechaUltAnalisis)
                }
                );
            }
            return negocios;
        }

        // Obtiene el siguiente ID para el negocio según el mayor ID existente actualmente +1
        // (Retorna un entero con el proximo ID de negocio)
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

        // Inserta el nuevo negocio a la base de datos
        // (Retorna el negocio creado que fue ingresado | Parametros: nombre, estado del negocio, correo del usuario)
        public NegocioModel IngresarNegocio(string Nombre, string tipo, string idUsuario) 
        {
            AnalisisHandler analisisHandler = new AnalisisHandler();

            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            int nextID = getNextID();

            // String con la consulta para insertar el negocio en la base de datos 
            string consulta = $"exec IngresarNegocio @nextID = {nextID}, @nombre = '{Nombre}', @idUsuario = '{idUsuario}', @fechaCreacion = '{sqlFormattedDate}'";
            enviarConsultaVoid(consulta);

            // Le ingresa un analisis por defecto al negocio recien creado 
            analisisHandler.IngresarAnalisis(Convert.ToInt32(nextID), tipo);

            // Crea un modelo para el negocio recien creado 
            NegocioModel negocioIngresado = new NegocioModel();
            negocioIngresado.Nombre = Nombre;
            negocioIngresado.idUsuario = idUsuario;
            negocioIngresado.FechaCreacion = DateOnly.FromDateTime(myDateTime);
            negocioIngresado.ID = nextID;

            return negocioIngresado;

        }

        // Elimina el negocio con el id indicado
        // (Parametros: id del negocio que se desea eliminar)
        public void EliminarNegocio(string idNegocio) 
        { 
            string consulta = "exec BorrarNegocio @idNegocio =" + idNegocio + "";
            enviarConsultaVoid(consulta);
        }
    }
}
