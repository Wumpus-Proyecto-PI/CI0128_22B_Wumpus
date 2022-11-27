using PI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_tests.SharedResources
{
    public class GastosFijosTestingHandler : HandlerGenerico
    {

        // brief: metodo que retorna todos los gastos fijos que hay en la base de datos de un analisis
        public List<GastoFijoModel> leerGastosFijosDeBase(DateTime fechaAnalisis)
        {
            List<GastoFijoModel> gastosFijos = new List<GastoFijoModel>();

            string consulta = "EXEC obtGastosFijosList @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            DataTable tablaResultado = base.CrearTablaConsultaGenerico(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                gastosFijos.Add(
                new GastoFijoModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),

                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Monto = Convert.ToDecimal(columna["monto"]),
                    orden = Convert.ToInt32(columna["orden"])
                }
                );
            }
            // Acomoda en las primeras 4 posiciones los gastos de las estorg
            return gastosFijos;
        }
    }
}
