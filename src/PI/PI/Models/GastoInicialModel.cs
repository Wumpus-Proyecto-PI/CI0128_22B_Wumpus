namespace PI.Models
{
    // Representa un gasto de la sección "inversión inicial".
    public class GastoInicialModel
    {
        public string? Nombre { get; set; }  // representa el nombre del gasto de la inversión inicial.
        public decimal Monto { get; set; }  // representa el costo monetario del gasto de la inversión inicial.
        
        public DateTime FechaAnalisis { get; set; }  // Fecha del análisis a la que pertenece el gasto inicial.
    }
}
