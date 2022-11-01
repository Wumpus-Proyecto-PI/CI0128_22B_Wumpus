using PI.Models;
using System.Data;

namespace PI.Handlers
{
    public class ComponenteHandler : Handler
    {
        public ComponenteHandler() : base() { }

        // Agrega el modelo de componente que se la pasa por parametro a la base de datos
        public int AgregarComponente(ComponenteModel componente)
        {
            int filasAfectadas = 0;
            string consulta = "EXEC AgregarComponente @nombreComponente='" + componente.Nombre.ToString() + "'" +
                ",@nombreProducto='" + componente.NombreProducto.ToString() + "',@fechaAnalisis='" +componente.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +"'" +
                ",@monto='" + componente.Costo.ToString() + "',@cantidad='" + componente.Cantidad.ToString() + "'" +
                ",@unidad='" + componente.Unidad.ToString()+ "'";

            filasAfectadas = enviarConsulta(consulta);
            return filasAfectadas;
        }

        // Borra el modelo de componente que se la pasa por parametro a la base de datos
        public int BorrarComponente(ComponenteModel componente)
        {
            int filasAfectadas = 0;
            string consulta = "EXEC BorrarComponente @nombreComponente='" + componente.Nombre.ToString() + "'" +
                ",@nombreProducto='" + componente.NombreProducto.ToString() + "',@fechaAnalisis='" + componente.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +"'";

            filasAfectadas = enviarConsulta(consulta);
            return filasAfectadas;
        }

        // Obtiene los componentes de un producto seleccionado 
        public List<ComponenteModel> ObtenerComponentes(string nombreProducto, DateTime fechaAnalisis)
        {
            List<ComponenteModel> componentes = new List<ComponenteModel>();
            string consulta = "EXEC ObtenerComponentes @nombreProducto='" + nombreProducto.ToString() + "',@fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                ComponenteModel componente = new ComponenteModel
                {
                    Nombre = Convert.ToString(columna["nombreComponente"]),
                    NombreProducto = Convert.ToString(columna["nombreProducto"]),
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Unidad = Convert.ToInt32(columna["unidad"])
                };
                if (columna["monto"] != DBNull.Value)
                {
                    componente.Costo = Convert.ToDecimal(columna["monto"]);
                }
                if (columna["cantidad"] != DBNull.Value)
                {
                    if (componente.Unidad == 1) { 
                        componente.Cantidad = Convert.ToDecimal(columna["cantidad"]) * 100;
                
                    }
                    else
                    {
                        componente.Cantidad = Convert.ToDecimal(columna["cantidad"]);
                    }
                }
                componentes.Add(componente);

            }

            return componentes;
        }

    }
}
