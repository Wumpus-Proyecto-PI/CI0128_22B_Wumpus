using Microsoft.AspNetCore.Mvc;
using PI.Models;
using System.Data;

namespace PI.Handlers
{
    public class InversionInicialHandler : Handler
    {
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

        public void IngresarGastoInicial(string fechaAnalisis, GastoInicialModel GastoInicial)
        {
            string consulta = $"EXEC IngresarGastoInicial @fechaAnalisis = '{fechaAnalisis}', @nombre = '{GastoInicial.Nombre}', @valor = {GastoInicial.Monto}";

            enviarConsultaVoid(consulta);
        }

        public void EliminarGastoInicial(string fechaAnalisis, string nombreGastoInicial)
        {
            string consulta = $"EXEC EliminarGastoInicial @nombre= '{nombreGastoInicial}', @fechaAnalisis='{fechaAnalisis}'";

            enviarConsultaVoid(consulta);
        }
    }
}
