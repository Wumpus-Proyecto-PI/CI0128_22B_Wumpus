using System.Data.SqlClient;
using System.Data;
using PI.Models;

namespace PI.Handlers
{
    public class AnalisisHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public AnalisisHandler()
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

        // Obtiene los analisis de un negocio 
        public List<AnalisisModel> ObtenerAnalisis(string IDNegocio)
        {
            List<AnalisisModel> analisisDelNegocio = new List<AnalisisModel>();
            

            string consulta = "SELECT IDNegocio, A.fechaCreacion from ANALISIS as A inner join Negocio as N " +
                "on A.IDNegocio = N.ID Where IDNegocio = " + IDNegocio + "";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                DateTime fechaAnalisisActual = (DateTime)columna["fechaCreacion"];
                bool tipoAnalisisActual = Convert.ToBoolean(ObtenerTipoAnalisis(fechaAnalisisActual));
                analisisDelNegocio.Add(
                new AnalisisModel
                {
                    FechaCreacion = Convert.ToDateTime(fechaAnalisisActual),
                    Configuracion = { TipoNegocio = tipoAnalisisActual, fechaAnalisis = fechaAnalisisActual}
                }
                );
            }

            return analisisDelNegocio;
        }
/*        // Obtiene la fecha de creacion de un analisis
        public DateTime obtenerFechaCreacion()
        {
            string consulta = "SELECT A.fechaCreacion from ANALISIS as A inner join Negocio as N " +
                "on A.IDNegocio = N.ID Where IDNegocio = " + IDNegocio + "";
        }*/

        // Obtiene el tipo de analisis
        public Boolean ObtenerTipoAnalisis(DateTime FechaCreacion)
        {
            Boolean tipo = false;

            string fecha = FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string consulta = "SELECT tipoNegocio FROM CONFIGURACION as C join ANALISIS as A on C.fechaAnalisis = A.FechaCreacion " +
                "WHERE C.FechaAnalisis = '" + fecha + "' ";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                tipo = Convert.ToBoolean(tablaResultado.Rows[0]["tipoNegocio"]);
            }

            return tipo;
        }

        // Agrega un analisis al negocio indicado

        public void IngresarAnalisis(string idNegocio, string tipo)
        {
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string consulta = "INSERT INTO Analisis (idNegocio,FechaCreacion) VALUES (" + idNegocio + ", '" + sqlFormattedDate + "');";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();

            // Crea una config por defecto asociada al analisis
            CrearConfigPorDefecto(sqlFormattedDate, tipo);
        }

        // Ingresa una configuracion con la fecha del analisis indicada
        public void CrearConfigPorDefecto(string fecha, string tipo)
        {
            int tipoAnalisis;
            if (tipo == "Emprendimiento")
            {
                tipoAnalisis = 0; 
            } else
            {
                tipoAnalisis = 1;
            }
            string consulta = "INSERT INTO CONFIGURACION (fechaAnalisis, tipoNegocio) VALUES ('" + fecha + "', " + tipoAnalisis + ")";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }
    }
}
