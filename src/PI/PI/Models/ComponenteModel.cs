namespace PI.Models
{
    public class ComponenteModel
    {
        // Nombre del producto al que el componente esta asociado
        public string NombreProducto { get; set; }

        // Fecha del analisis al que pertenece el componente
        public DateTime FechaAnalisis { get; set; }

        // Nombre del componente/rubro de un produto 
        public string Nombre { get; set; }

        // Costo del componente 
        public decimal Costo { get; set; }

        // Cantidad del componente 
        public decimal Cantidad { get; set; }

        // Representa si el componente se calcula en porcentaje o en unidad normal
        public string Unidad { get; set; }

    }
}
