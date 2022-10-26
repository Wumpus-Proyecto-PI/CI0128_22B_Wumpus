using Microsoft.AspNetCore.Mvc;
using PI.Models;
using System.Data;

namespace PI.Handlers
{    
    // Handler de inversión inicial, clase encargada de recuperar y consultar datos de las tablas en la base de datos.
    public class InversionInicialHandler : Handler
    {
        // Crea una lista de los gastos fijos existentes en la BD y la retorna.
        public List<GastoInicialModel> ObtenerGastosIniciales(string fechaAnalisis)
        {
            List<GastoInicialModel> GastosIniciales = new List<GastoInicialModel>();

            string consulta = $"EXEC ObtenerGastosIniciales @fechaAnalisis = '{fechaAnalisis}'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);

            foreach (DataRow row in tablaResultado.Rows)
            {
                GastosIniciales.Add(
                    new GastoInicialModel
                    {
                        Nombre = Convert.ToString(row["nombre"]),
                        Monto = Convert.ToDecimal(row["valor"]),
                        FechaAnalisis = Convert.ToDateTime(row["fechaAnalisis"]),
                    }
                );
            }
            return GastosIniciales;
        }

        // Inserta en la base de datos, el modelo de gasto inicial en el análisis pasado por parámetro.
        public void IngresarGastoInicial(string fechaAnalisis, GastoInicialModel GastoInicial)
        {
            string consulta = $"EXEC IngresarGastoInicial @fechaAnalisis = '{fechaAnalisis}', @nombre = '{GastoInicial.Nombre}', @valor = {GastoInicial.Monto}";

            enviarConsultaVoid(consulta);
        }

        // Elimina de la base de datos, el gasto inicial que coincida con el nombre pasada por parámetro en el análisis respectivo.
        public void EliminarGastoInicial(string fechaAnalisis, string nombreGastoInicial)
        {
            string consulta = $"EXEC EliminarGastoInicial @nombre= '{nombreGastoInicial}', @fechaAnalisis='{fechaAnalisis}'";

            enviarConsultaVoid(consulta);
        }
    }
}
