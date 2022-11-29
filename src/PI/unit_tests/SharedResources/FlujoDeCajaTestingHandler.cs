using DocumentFormat.OpenXml.Vml;
using PI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_tests.SharedResources
{
    public class FlujoDeCajaTestingHandler : HandlerGenerico
    {


        // brief: metodo que retorna un ingreso especifico de la base de datos de un analisis
        public IngresoModel obtenerIngreso(DateTime fechaAnalisis, string nombreMes, string tipo)
        {
            IngresoModel ingreso = new();

            string consulta = $"SELECT * FROM INGRESO WHERE fechaAnalisis = '{fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff")}' AND mes = '{nombreMes}' AND tipo = '{tipo}'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                ingreso.FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]);
                ingreso.Tipo = Convert.ToString(columna["tipo"]);
                ingreso.Mes = Convert.ToString(columna["mes"]);
                ingreso.Monto = Convert.ToDecimal(columna["monto"]);
            }

            return ingreso;
        }
    }
}
