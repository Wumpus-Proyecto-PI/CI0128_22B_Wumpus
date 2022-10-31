using System.ComponentModel.DataAnnotations;
using System;
namespace PI.Models
{
    public class AnalisisModel
    {
        // Representa la fecha unica de creacion del analisis
        public DateTime FechaCreacion { get; set; } 
        // Configuracion del analisis (clase)
        public ConfigAnalisisModel Configuracion { get; set; } = new ConfigAnalisisModel();
        // Representa la ganancia monetaria que el usuario final desea ganar mensualmente.

        [RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$")]
        public decimal GananciaMensual { get; set; }
        // Indica si el análisis ha sido finalizado o no. 1: en curso. 2: finalizado.
        public int EstadoAnalisis { get; set; } 
    }
}
