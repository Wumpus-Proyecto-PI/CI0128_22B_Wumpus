namespace PI.Models
{
    public class MesModel
    {
        // Nombre del mes
        public string NombreMes { get; set; }

        // Fecha del analisis a los que pertenece el mes 
        public DateTime FechaAnalisis { get; set; }   

        // Fraccion de la inversion inicial que posee el mes 
        public decimal InversionPorMes { get; set; }
    }
}
