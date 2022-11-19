namespace PI.Models
{
    public class EgresoModel
    {
        public string Mes { get; set; } = "";
        public int Tipo { get; set; } = 0;
        public decimal Monto { get; set; } = 0M;
        public DateTime FechaAnalisis { get; set; }

    }
}
