namespace PI.Models
{
    public class ProductoModel
    {
        // Nombre del producto/gasto variable 
        public string Nombre { get; set; }

        // Fecha del analisis al que corresponde 
        public DateTime FechaAnalisis { get; set; }

        // Lote del producto 
        public int Lote { get; set; }

        // Porecentaje de ventas del producto 
        public decimal PorcentajeDeVentas { get; set; } 

        // Precio del producto 
        public decimal Precio { get; set; }

        // Lista de componentes asociados al producto 
        public List<ComponenteModel> Componentes { get; set; } = new List<ComponenteModel>();

        // Comision de ventas del producto

        public decimal ComisionDeVentas { get; set; } = 0;

        // Costo Variable del producto 
        public decimal CostoVariable { get; set; }  

    }
}
