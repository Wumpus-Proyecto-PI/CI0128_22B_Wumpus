using DocumentFormat.OpenXml.InkML;
using PI.EntityModels;

namespace PI.EntityHandlers
{
    public class ComponenteHandler : EntityHandler
    {
           
        public ComponenteHandler(DataBaseContext context) : base(context) { }

        
        public async Task<int> AgregarComponenteAsync(Componente componente)
        {
            await base.Contexto.Componentes.AddAsync(componente);
            return await base.Contexto.SaveChangesAsync();
        }

    }
}
