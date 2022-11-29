namespace PI.Models
{
    public class EgresoModel
    {
        // Mes al que pertenece el egreso
        public string Mes { get; set; } = "";

        // Tipo del egreso
        public string Tipo { get; set; } = "";

        // Monto asignado al egreso
        public decimal Monto { get; set; } = 0M;

        // Fecha del analisis al que pertenece el egreso
        public DateTime FechaAnalisis { get; set; }

    }
}
