using PI.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_tests.SharedResources
{
    public class HandlerGenerico : Handler
    {
        public void EnviarConsultaGenerica(string consulta)
        {
            enviarConsultaVoid(consulta);
        }

        public DataTable CrearTablaConsultaGenerico(string consulta)
        {
            return CrearTablaConsulta(consulta);
        }
    }
}
