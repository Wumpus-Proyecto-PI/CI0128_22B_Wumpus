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
    }
}
