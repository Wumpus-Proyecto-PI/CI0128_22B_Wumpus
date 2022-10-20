using PI.Models;

namespace PI.Handlers
{
    public class ComponenteHandler : Handler
    {
        public ComponenteHandler() : base() { }

        public int AgregarComponente(ComponenteModel componente)
        {
            int filasAfectadas = 0;
            string consulta = "EXEC AgregarComponente @nombreComponente='" + componente.Nombre.ToString() + "'" +
                "@nombreProducto='" + componente.NombreProducto.ToString() + "'@fechaAnalisis='" +componente.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +"'" +
                "@monto='" + componente.Costo.ToString() + "'@cantidad='" + componente.Cantidad.ToString() + "'" +
                "@unidad='" + componente.Unidad.ToString()+ "'";

            filasAfectadas = enviarConsulta(consulta);
            return filasAfectadas;
        }

        public int BorrarComponente(ComponenteModel componente)
        {
            int filasAfectadas = 0;
            string consulta = "EXEC BorrarComponente @nombreComponente='" + componente.Nombre.ToString() + "'" +
                "@nombreProducto='" + componente.NombreProducto.ToString() + "'@fechaAnalisis='" + componente.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +"'";

            filasAfectadas = enviarConsulta(consulta);
            return filasAfectadas;
        }
    }
}
