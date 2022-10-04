namespace PI.Models
{
    public class BeneficioModel
    {
        public string nombrePuesto {get; set; }
        public string nombreBeneficio {get;set;}
        public DateTime fechaAnalisis {get;set; }
        public decimal monto { get; set; } = 0.0m;
        public int plazasPorBeneficio { get; set; } = 1;

    }
}
