using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using PI.EntityModels;
using System.Linq;

namespace PI.Services
{
    public class ProceduresServices : IDisposable
    {
        private DataBaseContext? Contexto = null;

        public ProceduresServices(DataBaseContext contexto)
        {
            Contexto = contexto;
        }

        ~ProceduresServices()
        {
            Contexto = null;
        }

        public void Dispose()
        {
            Contexto?.Dispose();
            Contexto = null;
        }
    }
}
