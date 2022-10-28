namespace PI.Models
{
    public class AnalisisModel
    {

        // Atributos
        public DateTime FechaCreacion { get; set; } // representa la fecha unica de creacion del analisis
        public ConfigAnalisisModel Configuracion { get; set; } = new ConfigAnalisisModel(); // configuracion del analisis (clase)
    }
}
