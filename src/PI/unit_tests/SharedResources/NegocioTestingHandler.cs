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
    public class NegocioTestingHandler : NegocioHandler
    {
        public NegocioModel? NegocioFicticio { get; private set; } = null;
        public string NombreNegocioFicticio { get; } = "Negocio ficticio";

        public NegocioModel IngresarNegocioFicticio(string idUsuario, string tipoNegocio = "Emprendimiento")
        {
            NegocioFicticio = base.IngresarNegocio(NombreNegocioFicticio, tipoNegocio, idUsuario);
            return NegocioFicticio;
        }

        public void EliminarNegocioFicticio()
        {
            try
            {
                base.EliminarNegocio(NegocioFicticio.ID.ToString());
            } catch(NullReferenceException) { 
                throw new NullReferenceException("El negocio actual es nulo.");
            }
        }
    }
}
