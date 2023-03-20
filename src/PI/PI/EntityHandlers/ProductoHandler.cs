using Microsoft.EntityFrameworkCore;
using PI.EntityModels;
using PI.Services;

namespace PI.EntityHandlers
{
    public class ProductoHandler : EntityHandler
    {
        public ProductoHandler(DataBaseContext context) : base(context) { }

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

                await base.Contexto.Productos.AddAsync(producto);
                //string consulta = "EXEC InsertarProducto @nombreProducto='" + producto.Nombre.ToString() + "',@nombreAnterior='" + nombreProducto.ToString() + "',@fechaAnalisis='" + producto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" +
                //    ",@lote='" + producto.Lote.ToString().Replace(",", ".") + "',@porcentajeDeVentas='" + producto.PorcentajeDeVentas.ToString().Replace(",", ".") + "',@precio='" + producto.Precio.ToString().Replace(",", ".") + "',@costoVariable='" + producto.CostoVariable.ToString().Replace(",", ".") + "',@comisionDeVentas='" + producto.ComisionDeVentas.ToString().Replace(",", ".") + "'";

                //filasAfectadas = enviarConsulta(consulta);
            }

            return await base.Contexto.SaveChangesAsync();
        }

        public async Task<int> EliminarProductoAsync(string nombre, DateTime fechaAnalisis)
        {
            base.Contexto.Productos.Remove(await base.Contexto.Productos.Where(x => x.FechaAnalisis == fechaAnalisis && x.Nombre == nombre).FirstOrDefaultAsync());
            return await base.Contexto.SaveChangesAsync();
        }
    }
}
