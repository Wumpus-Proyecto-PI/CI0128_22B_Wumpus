using PI.Models;
using PI.Handlers;

namespace PI.Services
{
    public class PasosProgresoControl
    {
        protected int NumeroPaso = 1;


        public int determinarPasoActivoMaximo(AnalisisModel analisis)
        {
            int pasoActivoMaximo = this.NumeroPaso;


            return pasoActivoMaximo;
        }

        protected virtual bool determinarPasoAtivo(AnalisisModel analisis)
        {
            return false;
        }
    }
    public class GastosFijosControl: PasosProgresoControl
    {
        public GastosFijosControl()
        {
            this.NumeroPaso = 2;
        }


        override protected bool determinarPasoAtivo(AnalisisModel analisis)
        {
            bool resultado = false;
            // Se crea instancia del handler
            EstructuraOrgHandler estHandler = new EstructuraOrgHandler();

            // Se obtiene de la base de datos los diferentes puestos del Análisis.
            List<PuestoModel> puestos = estHandler.ObtenerListaDePuestos(analisis.FechaCreacion);

            // Se determina si la cantidad de puestos que posee es mayor a 0
            if (puestos.Count > 0)
            {
                resultado = true;
            }
            return resultado;
        }
    }

    public class GastoVariableControl : PasosProgresoControl
    {
        public GastoVariableControl()
        {
            this.NumeroPaso = 3;
        }


        override protected bool determinarPasoAtivo(AnalisisModel analisis)
        {
            bool resultado = false;
            // Se crea instancia del handler
            GastoFijoHandler gastosHandler = new GastoFijoHandler();
            
            // Mediante el handler, se obtiene de la base de datos la cantidad de gastos fijos que contiene un análisis.
            List<GastoFijoModel> gastosFijos = gastosHandler.ObtenerGastosFijos(analisis.FechaCreacion);

            // Por cada uno de los gastos fijos obtenidos, se verifica si corresponde a uno de los gastos fijos por defecto de los análisis.
            // Si alguno de los gastos fijos obtenidos es diferente a todos ellos, se determina que si se le han agregado gastos fijos al análisis.
            for (int i = 0; i < gastosFijos.Count(); i += 1)
            {
                if (gastosFijos[i].Nombre != "Seguridad social" && gastosFijos[i].Nombre != "Prestaciones laborales" && gastosFijos[i].Nombre != "Beneficios de empleados" && gastosFijos[i].Nombre != "Salarios netos")
                {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }
    }

    public class RentabilidadControl : PasosProgresoControl
    {
        public RentabilidadControl()
        {
            this.NumeroPaso = 4;
        }


        override protected bool determinarPasoAtivo(AnalisisModel analisis)
        {
            bool resultado = false;

            // creamos un handlere de producto para acceder a la base de datos
            ProductoHandler productoHandler = new ProductoHandler();
            
            // verificamos si hay al menos un producto en la base de datos
            if (productoHandler.obtenerProductos(analisis.FechaCreacion).Count > 0)
            {
                resultado = true;
            }
            return resultado;
        }
    }
}

