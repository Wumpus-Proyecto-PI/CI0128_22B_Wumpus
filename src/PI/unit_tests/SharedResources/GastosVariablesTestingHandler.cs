using PI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_tests.SharedResources
{
    public class GastosVariablesTestingHandler : HandlerGenerico
    {
        public List<ProductoModel> leerProductosDeBase(DateTime fechaAnalisis)
        {
            List<ProductoModel> productos = new List<ProductoModel>();

            string consulta = "EXEC ObtenerProductos @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            DataTable tablaResultado = base.CrearTablaConsultaGenerico(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                productos.Add(
                new ProductoModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Lote = Convert.ToInt32(columna["lote"]),
                    PorcentajeDeVentas = Convert.ToDecimal(columna["porcentajeDeVentas"]),
                    Precio = Convert.ToDecimal(columna["precio"]),
                    CostoVariable = Convert.ToDecimal(columna["costoVariable"]),
                    ComisionDeVentas = Convert.ToDecimal(columna["comisionDeVentas"])
                }
                );
            }
            return productos;
        }
    }
}
