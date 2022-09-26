namespace PI.Models
{
    public class AnalisisModel
    {
        // Clase anidada
        public class ConfigAnalisisModel
        {
            public int TipoNegocio { set; get; }
        }

        // Atributos
        public DateTime FechaCreacion { get; set; }
        public ConfigAnalisisModel Configuracion { get; set; }
    }
}
