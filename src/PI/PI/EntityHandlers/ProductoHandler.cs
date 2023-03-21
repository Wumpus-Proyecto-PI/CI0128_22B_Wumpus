using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using PI.EntityModels;
using PI.Services;
using System.Collections.Generic;
using System.Text.Json;

namespace PI.EntityHandlers
{
    public class ProductoHandler : EntityHandler
    {
        public ProductoHandler(DataBaseContext context) : base(context) { }

        // Inserta el modelo del producto que se le pasa por parametro a la base de datos
        public async Task<int> InsertarProductoAsync(string nombreProducto, Producto producto)
        {

            if (FormatManager.EsAlfanumerico(nombreProducto) && FormatManager.EsAlfanumerico(producto.Nombre))
            {
                if (producto.Lote < 0m)
                {
                    throw new Exception("El valor del lote debe ser un número positivo", new ArgumentOutOfRangeException());
                }
                if (producto.Precio < 0m)
                {
                    throw new Exception("El valor del monto debe ser un número positivo", new ArgumentOutOfRangeException());
                }

                Producto productoEnBase = await base.Contexto.Productos.Where(p => p.Nombre == producto.Nombre && p.FechaAnalisis == producto.FechaAnalisis).FirstOrDefaultAsync();

                if ( productoEnBase == null )
                {
                    await base.Contexto.Productos.AddAsync(producto);
                }
                else
                {
                    Contexto.Productos.Remove(productoEnBase);
                    await base.Contexto.SaveChangesAsync();
                    await base.Contexto.Productos.AddAsync(producto);
                }
            }

            return await base.Contexto.SaveChangesAsync();
        }

        // Elimina el modelo del producto que se le pasa por parametro a la base de datos
        public async Task<int> EliminarProductoAsync(string nombre, DateTime fechaAnalisis)
        {
            base.Contexto.Productos.Remove(await base.Contexto.Productos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == nombre).FirstOrDefaultAsync());
            return await base.Contexto.SaveChangesAsync();
        }


        // Método que obtiene los productos de un analisis
        // Devuele una lista de ProductoModel
        public async Task<List<Producto>> ObtenerProductosAsync(DateTime fechaAnalisis)
        {
            List<Producto> productos = await base.Contexto.Productos
                .Where(p => p.FechaAnalisis == fechaAnalisis)
                .ToListAsync();

            // Carga los componentes para cada producto
            foreach (Producto producto in productos)
            {
                await base.Contexto.Entry(producto).Collection(p => p.Componentes).LoadAsync();
            }

            return productos;
        }

    }
}
