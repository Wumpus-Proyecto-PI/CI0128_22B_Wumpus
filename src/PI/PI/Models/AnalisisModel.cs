namespace PI.Models
{
    public class AnalisisModel
    {

        // Atributos
        public DateTime FechaCreacion { get; set; }
        public ConfigAnalisisModel Configuracion { get; set; } = new ConfigAnalisisModel();
    }
}
