﻿using PI.Models;
using System.Data;

namespace PI.Handlers
{
    public class ProductoHandler : Handler
    {
        public ProductoHandler() : base() { }

        // Método que obtiene los productos de un analisis
        // Devuele una lista de ProductoModel
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

        // Método que actualiza el porcentaje de ventas de un producto en la base de datos
        public void actualizarPorcentajeVentas(ProductoModel producto, DateTime fechaAnalisis)
        {
            string consulta = "UPDATE PRODUCTO " +
                              "SET porcentajeDeVentas = " + producto.PorcentajeVentas.ToString() +
                              " WHERE nombre = '" + producto.Nombre.ToString() + "' AND fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsulta(consulta);
        }
    }
}