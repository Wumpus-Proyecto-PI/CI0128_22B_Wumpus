namespace PI.Models;
using System;

public class NegocioModel
{
    public string Nombre { get; set; }
    public string CorreoUsuario { get; set; }
    public int ID { get; set; }
    public List<AnalisisModel> Analisis { set; get; }

    public DateOnly FechaCreacion { set; get; }

}
