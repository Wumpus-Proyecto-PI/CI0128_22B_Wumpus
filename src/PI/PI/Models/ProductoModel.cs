namespace PI.Models
{
    public class ProductoModel
    {
        public string Nombre { get; set; }
        public DateTime FechaAnalisis { get; set; }
        public int Lote { get; set; }
        public decimal PorcentajeVentas { get; set; }
        public decimal Precio { get; set; }
    }
}
