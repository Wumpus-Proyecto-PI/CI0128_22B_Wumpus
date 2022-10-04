namespace PI.Models
{
    public class PuestoModel
    {
        // nombre del puesto, no deben haber dos iguales
        public string Nombre { get; set; } = String.Empty;

        // numero de plazas que puede tener un puesto
        public int Plazas { get; set; } = 1;

        // salario neto que se gana en el puesto
        public decimal SalarioNeto { get; set; } = 0M;

        // salario bruto es el salario luego de impuestos y otros
        public decimal SalarioBruto { get; set; } = 0M;

        // beneficios que tiene el puesto
        public List<BeneficioModel> Beneficios { get; set; } = new List<BeneficioModel>();

        // lista de puestos subordinados de este puesto
        public List<PuestoModel> Subordinados { get; set; } = new List<PuestoModel>();

        // dateTime del analisis al que pertenece el puesto
        public DateTime FechaAnalisis { get; set; }

    }
}
