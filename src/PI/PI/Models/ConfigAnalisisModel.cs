namespace PI.Models
{
    public class ConfigAnalisisModel
    {
        // Fecha del análisis a la cual pertenece la configuración
        public DateTime fechaAnalisis { get; set; }
        // Tipo de negocio: Si es en marcha o nuevo.
        public bool TipoNegocio { set; get; }
        // El porcentaje de Seguro Social que se calcula sobre los salarios
        public decimal PorcentajeSS { get; set; }
        // El porcentaje de Prestaciones Laborales que se calcula sobre los salarios
        public decimal PorcentajePL { get; set; }
    }
}
