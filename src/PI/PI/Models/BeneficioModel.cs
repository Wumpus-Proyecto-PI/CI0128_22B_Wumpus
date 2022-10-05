namespace PI.Models
{
    public class BeneficioModel
    {
        // Nombre del puesto al que el beneficio pertenece.
        public string nombrePuesto {get; set; }
        // Nombre del beneficio
        public string nombreBeneficio {get;set;}
        // La fecha del análisis a la cual pertenece el beneficio
        public DateTime fechaAnalisis {get;set; }
        // El monto monetario que representa el beneficio
        public decimal monto { get; set; } = 0.0m;
        // La cantidad de empleados que obtienen el beneficio
        public int plazasPorBeneficio { get; set; } = 1;

    }
}
