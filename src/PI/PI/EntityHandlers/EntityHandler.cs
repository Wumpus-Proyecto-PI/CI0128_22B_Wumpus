using PI.EntityModels;

namespace PI.EntityHandlers
{
    public class EntityHandler: IDisposable
    {
        protected DataBaseContext? Contexto = null;

        public EntityHandler(DataBaseContext contexto) {
            Contexto = contexto;
        }

        ~EntityHandler()
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
