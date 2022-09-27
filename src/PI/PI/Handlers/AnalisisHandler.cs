using System.Data.SqlClient;
using System.Data;

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

        // Obtiene el tipo de analisis
        public Boolean ObtenerTipoAnalisis(String FechaCreacion)
        {
            Boolean tipo = false;
            string consulta = "SELECT tipoNegocio FROM CONFIGURACION as C join ANALISIS as A on C.fechaAnalisis = A.FechaCreacion WHERE C.FechaAnalsis = "+FechaCreacion+" ";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                tipo = Convert.ToBoolean(tablaResultado.Rows[0]);
            }

            return tipo;
        }
    }
}
