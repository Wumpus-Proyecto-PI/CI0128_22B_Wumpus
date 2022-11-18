using PI.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsResources
{
    public class HandlerGenerico : Handler
    {
        public void EnviarConsultaGenerica(string consulta)
        {
            base.enviarConsultaVoid(consulta);
        }

        public DataTable CrearTablaConsultaGenerico(string consulta)
        {
            return base.CrearTablaConsulta(consulta);
        }
    }
}
