namespace PI.Models
{
    public class AnalisisModel
    {

        // Atributos
        public DateTime FechaCreacion { get; set; }
        public ConfigAnalisis Configuracion { get; set; } = new ConfigAnalisis();
    }
}
