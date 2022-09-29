namespace PI.Models
{
    public class GastoFijoModel
    {
        public string Nombre { get; set; } = String.Empty;
        public decimal Monto { get; set; } = 0M;
        public DateTime FechaAnalisis { get; set; }
    }
}
