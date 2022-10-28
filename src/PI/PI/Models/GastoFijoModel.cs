namespace PI.Models
{
    // Clase que representa un gasto fijo (un gasto que no depende de la venta/compra de un producto): su nombre y su monto.
    public class GastoFijoModel
    {
        // Nombre del gasto fijo. Debe ser único dentro del contexto de un análisis.
        public string Nombre { get; set; } = String.Empty;

        // Monto mensual (cantidad monetaria) del gasto fijo.
        // TODO cambiar a monto anual.
        public decimal Monto { get; set; } = 0M;
        
        // Fecha del análisis a la que pertenece el gasto fijo.
        public DateTime FechaAnalisis { get; set; }

        // Orden de inserción del gasto fijo. Se asigna el valor cuando se crea según el último orden. Ej: ordenMayor + 1.
        // TODO posible: eliminar atributo para considerar ordenar la vista alfabéticamente. Sug: Order by nombre en la consulta de la BD.
        public int orden { get; set; }
    }
}
