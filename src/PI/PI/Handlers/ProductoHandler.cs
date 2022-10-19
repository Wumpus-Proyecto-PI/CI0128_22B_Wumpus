using PI.Models;
using System.Data;

namespace PI.Handlers
{
    public class ProductoHandler : Handler
    {
        public ProductoHandler() : base() { }

        public List<ProductoModel> obtenerProductos(DateTime fechaAnalisis)
        {
            List<ProductoModel> productos = new List<ProductoModel>();

            string consulta = "EXEC ObtenerProductos @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                ProductoModel nuevoProducto = new ProductoModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Lote = Convert.ToInt32(columna["lote"]),
                };
                if (columna["porcentajeDeVentas"] != DBNull.Value)
                {
                    nuevoProducto.PorcentajeVentas = Convert.ToDecimal(columna["porcentajeDeVentas"]);
                }
                if (columna["precio"] != DBNull.Value)
                {
                    nuevoProducto.Precio = Convert.ToDecimal(columna["precio"]);
                }
                productos.Add(nuevoProducto);
            }
            return productos;
        }
    }
}
