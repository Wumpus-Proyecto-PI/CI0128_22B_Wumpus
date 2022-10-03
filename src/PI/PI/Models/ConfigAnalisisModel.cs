namespace PI.Models
{
    public class ConfigAnalisisModel
    {
        public DateTime fechaAnalisis { get; set; }
        public bool TipoNegocio { set; get; }
        public decimal PorcentajeSS { get; set; }
        public decimal PorcentajePL { get; set; }
    }
}
