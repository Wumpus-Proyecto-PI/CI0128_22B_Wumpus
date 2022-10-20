using PI.Models;

namespace PI.Handlers
{
    public class ProductoHandler : Handler
    {
        public ProductoHandler() : base() { }

        public int InsertarProducto(string nombreGasto, ProductoModel gastoVar)
        {
            int filasAfectadas = 0;
            return filasAfectadas;
        }

        public int EliminarProducto(ProductoModel gastoVar)
        {
            int filasAfectadas = 0;
            return filasAfectadas;
        }
    }
}
