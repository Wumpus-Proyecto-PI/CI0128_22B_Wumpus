using Microsoft.AspNetCore.Mvc;
using PI.Models;
using System.Data;

namespace PI.Handlers
{    
    // Handler de inversión inicial, clase encargada de recuperar y consultar datos de las tablas en la base de datos.
    public class InversionInicialHandler : Handler
    {
        // // Recibe la fecha del análisis del que se quiere obtener los gastos iniciales.
        // Crea una lista de los gastos iniciales existentes en la BD
        // Retorna la lista de los gastos iniciales.
        public List<GastoInicialModel> ObtenerGastosIniciales(string fechaAnalisis)
        {
            List<GastoInicialModel> gastosIniciales = new List<GastoInicialModel>();

            string consulta = $"EXEC ObtenerGastosIniciales @fechaAnalisis = '{fechaAnalisis}'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);

            foreach (DataRow row in tablaResultado.Rows)
            {
                gastosIniciales.Add(
                    new GastoInicialModel
                    {
                        Nombre = Convert.ToString(row["nombre"]),
                        Monto = Convert.ToDecimal(row["valor"]),
                        FechaAnalisis = Convert.ToDateTime(row["fechaAnalisis"]),
                    }
                );
            }
            return gastosIniciales;
        }

        // Recibe la fecha del análisis al que se quiere insertar el gasto inicial y lo inserta en la base de datos
        public void IngresarGastoInicial(string fechaAnalisis, GastoInicialModel gastoInicial)
        {
            string consulta = $"EXEC IngresarGastoInicial @fechaAnalisis = '{fechaAnalisis}', @nombre = '{gastoInicial.Nombre}', @valor = {gastoInicial.Monto}";

            enviarConsultaVoid(consulta);
        }

        // Recibe la fecha del análisis del que se quiere eliminar el gasto inicial y lo elimina en la base de datos el gasto inicial que coincida con el nombre pasada por parámetro.
        public void EliminarGastoInicial(string fechaAnalisis, string nombreGastoInicial)
        {
            string consulta = $"EXEC EliminarGastoInicial @nombre= '{nombreGastoInicial}', @fechaAnalisis='{fechaAnalisis}'";

            enviarConsultaVoid(consulta);
        }

        // Recibe la fecha del análisis del que se quiere obtener la sumatoria de los montos de los gastos iniciales del análisis pasado por parámetro.
        public decimal ObtenerMontoTotal (string fechaAnalisis)
        {
            decimal total = 0.0m;

            string consulta = $"EXEC ObtenerSumInversionInicial @fechaAnalisis = '{fechaAnalisis}' ";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows[0].IsNull("total") == false)
            {
                total = Convert.ToDecimal(tablaResultado.Rows[0]["total"]);
            }

            return total;
        }
    }
}
