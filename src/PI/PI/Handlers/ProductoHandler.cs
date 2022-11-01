using PI.Models;
using System.Data;

namespace PI.Handlers
{
    public class ProductoHandler : Handler
    {
        public ProductoHandler() : base() { }

        // Inserta el modelo del producto que se le pasa por parametro a la base de datos
        public int InsertarProducto(string nombreProducto, ProductoModel producto)
        {
            int filasAfectadas = 0;
            string consulta = "EXEC InsertarProducto @nombreProducto='" + producto.Nombre.ToString() + "',@nombreAnterior='" + nombreProducto.ToString() + "',@fechaAnalisis='" + producto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" +
                ",@lote='" + producto.Lote.ToString() + "',@porcentajeDeVentas='" + producto.PorcentajeDeVentas.ToString() + "',@precio='" + producto.Precio.ToString() + "',@costoVariable='" + producto.CostoVariable.ToString() + "'";

            filasAfectadas = enviarConsulta(consulta);
            return filasAfectadas;
        }

        // Elimina el modelo del producto que se le pasa por parametro a la base de datos
        public int EliminarProducto(ProductoModel producto)
        {
            int filasAfectadas = 0;
            string consulta = "EXEC EliminarProducto @nombreProducto='" + producto.Nombre.ToString() + "',@fechaAnalisis='" 
                + producto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +"'";

            filasAfectadas = enviarConsulta(consulta);

            return filasAfectadas;
        }

        // Método que obtiene los productos de un analisis
        // Devuele una lista de ProductoModel
        public List<ProductoModel> ObtenerProductos(DateTime fechaAnalisis)
        {
            List<ProductoModel> productos = new List<ProductoModel>();

            ComponenteHandler componenteHandler = new ComponenteHandler();

            string consulta = "EXEC ObtenerProductos @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                ProductoModel nuevoProducto = new ProductoModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Lote = Convert.ToInt32(columna["lote"]),
                    Componentes = componenteHandler.ObtenerComponentes(Convert.ToString(columna["nombre"]), Convert.ToDateTime(columna["fechaAnalisis"]))
                };
                if (columna["porcentajeDeVentas"] != DBNull.Value)
                {
                    nuevoProducto.PorcentajeDeVentas = Convert.ToDecimal(columna["porcentajeDeVentas"]);
                }
                if (columna["precio"] != DBNull.Value)
                {
                    nuevoProducto.Precio = Convert.ToDecimal(columna["precio"]);
                }
                if (columna["costoVariable"] != DBNull.Value)
                {
                    nuevoProducto.CostoVariable = Convert.ToDecimal(columna["costoVariable"]);
                }
                productos.Add(nuevoProducto);
            }
            return productos;
        }

        // Método que actualiza el porcentaje de ventas de un producto en la base de datos
        public void ActualizarPorcentajeVentas(ProductoModel producto, DateTime fechaAnalisis)
        {
            string consulta = "UPDATE PRODUCTO " +
                              "SET porcentajeDeVentas = " + producto.PorcentajeDeVentas.ToString() +
                              " WHERE nombre = '" + producto.Nombre.ToString() + "' AND fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsulta(consulta);
        }

        // Método que actualiza el precio de un producto en la base de datos
        public void ActualizarPrecio(ProductoModel producto, DateTime fechaAnalisis)
        {
            string consulta = "UPDATE PRODUCTO " +
                              "SET precio = " + producto.Precio.ToString() +
                              " WHERE nombre = '" + producto.Nombre.ToString() + "' AND fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsulta(consulta);
        }

        // Obtiene el porcentaje de ventas del producto de la base de datos que concuerde con los parametros de fechaAnalisis y nombreProducto
        public decimal ObtenerPorcentajeVentas(DateTime fechaAnalisis, string nombreProducto)
        {
            decimal porcentaje = 0;
            string consulta = "EXEC ObtenerPorcentajeVentas @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', @nombreProducto='" + nombreProducto + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows[0].IsNull("porcentajeDeVentas") == false)
            {
                porcentaje = Convert.ToDecimal(tablaResultado.Rows[0]["porcentajeDeVentas"]);
            }

            return porcentaje;
        }

        // Obtiene el porcentaje de ventas total del producto de la base de datos que concuerde con los parametros de fechaAnalisis y nombreProducto
        public decimal ObtenerPorcentajeVentasTotal(DateTime fechaAnalisis)
        {
            decimal total = 0;
            string consulta = "EXEC ObtenerPorcentajeDeVentasTotal @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows[0].IsNull("totalPorcentajeVentas") == false)
            {
                total = Convert.ToDecimal(tablaResultado.Rows[0]["totalPorcentajeVentas"]);
            }

            return total;
        }

    }
}
