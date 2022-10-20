using PI.Models;

namespace PI.Handlers
{
    public class GastoVariableHandler : Handler
    {
        public GastoVariableHandler() : base() { }

        public int InsertarGastoVariable(string nombreGasto, GastoVariableModel gastoVar)
        {
            int filasAfectadas = 0;
            return filasAfectadas;
        }

        public int EliminarGastoVariable(GastoVariableModel gastoVar)
        {
            int filasAfectadas = 0;
            return filasAfectadas;
        }
    }
}
