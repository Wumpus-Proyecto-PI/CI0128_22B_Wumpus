using System.Globalization;

namespace PI.Services
{
    public class FormatManager
    {
        // Se encargad e pasar ya sea un int o un decimal a un string con formato estadístico.
        public static string ToFormatoEstadistico (Object input) {
            if (input.GetType() == typeof(decimal) || input.GetType() == typeof(int))
            {
                return String.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", input);
            }
            else {
                Console.WriteLine("El tipo de dato ingresado no es válido");
                return "";
            }
        }

        // Valida si un decimal es válido de acuerdo a los parámetros definidos para el proyecto.
        public static bool ValidarInputDecimal(decimal monto) {
            bool valido = true;
            if (monto < 0 || Convert.ToString(monto).Length > 16) {
                valido = false;
            }
            return valido;        
        }

        public static bool ValidarInputInt(int monto)
        {
            bool valido = true;
            if (Convert.ToString(monto).Length > 10)
            {
                valido = false;
            }
            return valido;
        }
    }
}
