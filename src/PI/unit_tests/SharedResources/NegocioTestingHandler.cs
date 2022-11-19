using PI.Handlers;
using PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace unit_tests.SharedResources
{
    // brief: clase utilizada para insertar un negocio de pruebas a un usuario
    public class NegocioTestingHandler : NegocioHandler
    {
        // modelo del negocio ficticio de testing
        public NegocioModel? NegocioFicticio { get; private set; } = null;

        // el nombre del negocio ficticio
        public string NombreNegocioFicticio { get; } = "Negocio ficticio";

        // metodo para insertar en la base el negocio ficticio
        // si se desea insertar un negocio difernete se puede usar el metodo IngresarNegocio de NegocioHandler
        public NegocioModel IngresarNegocioFicticio(string idUsuario, string tipoNegocio = "Emprendimiento")
        {
            NegocioFicticio = base.IngresarNegocio(NombreNegocioFicticio, tipoNegocio, idUsuario);
            return NegocioFicticio;
        }

        // metodo que elimina el negocio ficticio
        public void EliminarNegocioFicticio()
        {
            base.EliminarNegocio(NegocioFicticio.ID.ToString());
        }
    }
}
