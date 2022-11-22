namespace PI.Models
{
    public class IngresoModel
    {
        public string Mes { get; set; } = "";
        public string Tipo { get; set; } = "";
        public decimal Monto { get; set; } = 0M;
        public DateTime FechaAnalisis { get; set; }
    }
}
