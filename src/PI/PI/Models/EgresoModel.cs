﻿namespace PI.Models
{
    public class EgresoModel
    {
        public string Mes { get; set; } = "";
        public bool Tipo { get; set; } = false;
        public decimal Monto { get; set; } = 0M;
        public DateTime FechaAnalisis { get; set; }

    }
}
