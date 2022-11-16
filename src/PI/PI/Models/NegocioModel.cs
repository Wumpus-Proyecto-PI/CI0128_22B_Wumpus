namespace PI.Models;
using System;
    
    // Clase que representa un negocio: su id, su nombre, usuario asociado y la información de análisis que le corresponden.
    public class NegocioModel
    {
    // Nombre del negocio
    public string Nombre { get; set; }

    // Correo del usuario que creó el negocio
    public string idUsuario { get; set; }

    // Id único entre objetos Negocio
    public int ID { get; set; }

    // Análisis creados en el negocio
    public List<AnalisisModel> Analisis { set; get; }

    // Fecha de creación del negocio
    public DateOnly FechaCreacion { set; get; }

    // Estado del negocio respecto al análisis más recientemente creado
    // true: ya está en marcha | false: no ha iniciado
    public bool TipoUltimoAnalisis { set; get; }

}
