using System.ComponentModel.DataAnnotations;
using System;
namespace PI.Models
{
    public class AnalisisModel
    {

        // Atributos
        public DateTime FechaCreacion { get; set; } // representa la fecha unica de creacion del analisis
        public ConfigAnalisisModel Configuracion { get; set; } = new ConfigAnalisisModel(); // configuracion del analisis (clase)
        public bool Finalizado { get; set; } // Indica si el análisis ha sido finalizado o no.

        [Range(0.0, Double.MaxValue,
            ErrorMessage = "Ingresó un número muy pequeño o muy grande")]
        public decimal gananciaMensual { get; set; }
        public int estadoAnalisis { get; set; } // Indica si el análisis ha sido finalizado o no. 1: en curso. 2: finalizado.
    }
}
