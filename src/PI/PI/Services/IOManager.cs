using System.Globalization;

namespace PI.Services
{
    public class IOManager
    {
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

        public static bool ValidarInputDecimal(decimal monto) {
            bool valido = true;
            if (monto < 0 || Convert.ToString(monto).Length > 18) {
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
