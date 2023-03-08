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
    // Clase handler generalizada para uso de handlers
    // De esta clase heredan los otros handlers para usar métodos comunes y atributos comunes
    public class Handler : IDisposable
    {
        protected SqlConnection? conexion = null; // objeto que conecta con la base de datos
        protected string rutaConexion; // connection string que indica a cual base conectarse

        public void Dispose()
        {
            if (conexion != null)
            {
                conexion.Close();
                conexion.Dispose();
                conexion = null;
            }
        }

        // Constructor
        // Recibe e nombre del conexi´´on string a utlizar
        public Handler(string connectionStr)
        {
            // objeto que permite acceder al conection string del appsettings.json           
            var builder = WebApplication.CreateBuilder();
            
            // cargamos la ruta de conexión
            rutaConexion = builder.Configuration.GetConnectionString(connectionStr);

            // nos conectamos a la base de datos
            conexion = new SqlConnection(rutaConexion);
        }

        // Constructor default que utiliza el mismo string de conexión simepre
        public Handler()
        {
            // objeto que permite acceder al conection string del appsettings.json           
            var builder = WebApplication.CreateBuilder();

            // cargamos la ruta de conexión predeterminada del appsettings.json    
            rutaConexion = builder.Configuration.GetConnectionString("BaseDeDatos");

            // nos conectamos a la base de datos
            conexion = new SqlConnection(rutaConexion);
        }

        ~Handler()
        {
            this.Dispose();
        }
        

        // Método que realiza un consulta y retorna la tabla con el resultado
        // Recibe la la consulta sql a realizar
        protected DataTable CrearTablaConsulta(string consulta)
        {
            
            using (SqlCommand comandoParaConsulta = new SqlCommand(consulta,
                conexion))
            using (SqlDataAdapter adaptadorParaTabla = new
                SqlDataAdapter(comandoParaConsulta))
            {

                // se genera la consulta

                // se crea una tabla para el resultado
                DataTable consultaFormatoTabla = new DataTable();
                // abrimos conexión y enviamos la consulta
                conexion.Open();
                adaptadorParaTabla.Fill(consultaFormatoTabla);

                // cerramos conexión
                conexion.Close();
                return consultaFormatoTabla;
            }
        }

        // Método que realiza un consulta y retorna la tabla con el resultado
        // Recibe la la consulta sql a realizar
        protected void LlenarDataConsulta(string consulta, DataTable consultaFormatoTabla)
        {

            using (SqlCommand comandoParaConsulta = new SqlCommand(consulta,
                conexion))
            using (SqlDataAdapter adaptadorParaTabla = new
                SqlDataAdapter(comandoParaConsulta))
            {

                // se genera la consulta

                // abrimos conexión y enviamos la consulta
                conexion.Open();
                adaptadorParaTabla.Fill(consultaFormatoTabla);

                // cerramos conexión
                conexion.Close();
            }
        }

        // Método para ennviar consultas como update, insert y delete
        // Recibe la la consulta sql a realizar
        // Retorna la cantidad de filas afectadas en la base de datos
        protected int enviarConsulta(string insert)
        {
            int filasAfectadas = 0;
            using (SqlCommand comando = new SqlCommand(insert, conexion))
            {
                // se genera la consulta

                // abrimos conexión y enviamos la consulta
                conexion.Open();
                filasAfectadas = comando.ExecuteNonQuery();

                // cerramos conexión
                conexion.Close();

                return filasAfectadas;
            }
        }

        // Método para ennviar consultas como update, insert y delete pero no retorna nada
        // Recibe la la consulta sql a realizar
        protected void enviarConsultaVoid(string insert)
        {
            using (SqlCommand comando = new SqlCommand(insert, conexion))
            {
                // abrimos conexión y enviamos la consulta
                conexion.Open();
                comando.ExecuteNonQuery();

                // cerramos conexión
                conexion.Close();
            }
        }

        // Método que retirna el nombre de un negocio a partir de un análisis
        // Recibe la fecha de análisis del cual se quiere saber el nombre
        // Retorna el nombre del negocio al que pertenece
        public string obtenerNombreNegocio(DateTime fechaAnalisis)
        {
            // consulta sql 
            string consulta = "EXEC obtNombreNegocio '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            
            // string a retornar con el nombre del negocio
            string nombreNegocio = "";

            // se realiza la consulta
            using (DataTable tablaResultado = CrearTablaConsulta(consulta))
            {

                // Se revisa si la consulta retorno algo
                if (tablaResultado.Rows.Count > 0 && !tablaResultado.Rows[0].IsNull("nombre"))
                {
                    // si tiene algo la tabla resultado, se asigna el nombre del negocio
                    nombreNegocio = Convert.ToString(tablaResultado.Rows[0]["nombre"]);
                }
                else
                {
                    // existe el caso de que un negocio no tenga nombre en nuestro producto
                    nombreNegocio = "Sin nombre";
                }

                return nombreNegocio;
            }

        }

        public NegocioModel obtenerNegocioDeAnalisis(DateTime fechaAnalisis)
        {
            // consulta sql 
            string consulta = "EXEC ObtenerNegocioDeAnalisis '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            // NegocioModel a retornar
            NegocioModel negocioObtenido = new NegocioModel();

            // se realiza la consulta
            using (DataTable tablaResultado = CrearTablaConsulta(consulta))
            {
                // Se revisa si la consulta retorno algo
                if (tablaResultado.Rows.Count > 0 && !tablaResultado.Rows[0].IsNull("nombre"))
                {
                    // si tiene algo la tabla resultado, se asigna el nombre del negocio
                    negocioObtenido.Nombre = Convert.ToString(tablaResultado.Rows[0]["nombre"]);
                    negocioObtenido.idUsuario = Convert.ToString(tablaResultado.Rows[0]["idUsuario"]);
                    negocioObtenido.ID = Convert.ToInt32(tablaResultado.Rows[0]["id"]);
                    negocioObtenido.FechaCreacion = DateOnly.FromDateTime((DateTime)tablaResultado.Rows[0]["FechaCreacion"]);
                }

                return negocioObtenido;
            }
        }

        public string obtenerNombreNegocio(int IDNegocio)
        {
            // consulta sql 
            string consulta = "EXEC obtNombreNegocioPorID @IDNegocio = " + IDNegocio + ";";

            // string a retornar con el nombre del negocio
            string nombreNegocio = "";

            // se realiza la consulta
            using (DataTable tablaResultado = CrearTablaConsulta(consulta))
            {
                // Se revisa si la consulta retorno algo
                if (tablaResultado.Rows.Count > 0 && !tablaResultado.Rows[0].IsNull("nombre"))
                {
                    // si tiene algo la tabla resultado, se asigna el nombre del negocio
                    nombreNegocio = Convert.ToString(tablaResultado.Rows[0]["nombre"]);
                }
                else
                {
                    // existe el caso de que un negocio no tenga nombre en nuestro producto
                    nombreNegocio = "Sin nombre";
                }

                return nombreNegocio;
            }
        }

       
    }
}