using PI.Models;
using PI.Handlers;

namespace PI.Services
{
    public class PasosProgresoControl
    {
        protected int NumeroPaso = 1;
        private readonly List<PasosProgresoControl> PasosControles = new(); 

        private void construirPasos()
        {
            // se agrega el controlado de cada paso
            PasosControles.Add(new GastosFijosControl());
            PasosControles.Add(new GastoVariableControl());
            PasosControles.Add(new RentabilidadControl());
            PasosControles.Add(new InversionInicialControl());
            // aqui se agregan los controladores de los pasos nuevos

        }

        // Brief: metodo director que llama al metodo plantilla de los hijos para determinar el paso maximo al que puede entrar el usuario
        // Param: recibe el analisis del cual determinar el paso maximo al que puede ingresar el usuario
        // Return: retorna el indice del paso maximo al que puede ingresar el usuario
        public int DeterminarPasoActivoMaximo(AnalisisModel analisis)
        {
            // construimos instancias de cada paso y se agregan a la lista de pasos
            construirPasos();

            // se asigna 1 al paso que se retorna. Como minimo se puede entrar al paso de estrutura organizativa.
            int pasoActivoMaximo = this.NumeroPaso;

            // buscamos el primer paso que no este activo porque este no se peude acceder
            PasosProgresoControl? pasoMaximoAccesible = PasosControles.Find(x => x.DeterminarPasoAtivo(analisis) == false);

            // asiganamos el numero de paso a la variable de retorno si nos nulo el resultado de la busqueda
            if (pasoMaximoAccesible is not null)
            {
                pasoActivoMaximo = pasoMaximoAccesible.NumeroPaso - 1;
            } else
            {
                // si es nulo el resultado de la busqueda indicamos que el maximo paso accesible es el ultimo paso que haya en la lista
                // esto porque si se obtuvo nulo es que no hubo nigun paso no activo en las lista, por lo tanto todos esta activos
                // le sumamos 1 porque en la lita de pasos no se incluye el paso default que es la estrutura organizativa
                pasoActivoMaximo = PasosControles.Count + 1;
            }

            return pasoActivoMaximo;
        }

        // metodo plantilla que implementan los hijos para determinar si un paso esta activo
        // Param: recibe el analisis del cual determinar el paso maximo al que puede ingresar el usuario
        // Return: verdadero si el paso esta activo y se puede ingresar
        protected virtual bool DeterminarPasoAtivo(AnalisisModel analisis)
        {
            return false;
        }
    }

    public class GastosFijosControl : PasosProgresoControl
    {
        public GastosFijosControl()
        {
            base.NumeroPaso = 2;
        }

        // Indica si el analisis posee puestos
        // (Retorna un bool que indica si hay puestos o no | Parametros: modelo del analisis que se desea verificar)
        // Se encarga de verificar si existen puestos dentro de un análisis.
        override protected bool DeterminarPasoAtivo(AnalisisModel analisis)
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
            base.NumeroPaso = 3;
        }

        // Indica si el analisis posee gastos fijos
        // (Retorna un bool que indica si hay gastos fijos o no | Parametros: modelo del analisis que se desea verificar)
        // Determina si un análisis contiene gastos fijos.
        override protected bool DeterminarPasoAtivo(AnalisisModel analisis)
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
            base.NumeroPaso = 4;
        }

        // metodo que verifica si el paso de analisis rentabilidad esta disponible o no.
        // recibe el enalisis del cual determiar si el paso esta disponible
        // retorna true si el paso esta disponible
        override protected bool DeterminarPasoAtivo(AnalisisModel analisis)
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

    public class InversionInicialControl : PasosProgresoControl
    {
        public InversionInicialControl()
        {
            base.NumeroPaso = 5;
        }

        // método que verifica si es posible que exista una meta de ventas en el análisis de rentabilidad.
        // detalle: sirve para determinar si la tarjeta de la inversión inicial se debe habilitar.
        // se asume que si un producto tiene valores en algunos de sus atributos, la meta de ventas ha sido calculada.
        override protected bool DeterminarPasoAtivo(AnalisisModel analisis)
        {
            bool resultado = false;
            ProductoHandler productoHandler = new ProductoHandler();
            List<ProductoModel> productos = productoHandler.obtenerProductos(analisis.FechaCreacion);
            for (int actual = 0; actual < productos.Count && resultado == false; ++actual)
            {
                if (productos[actual].Precio > 0
                    && productos[actual].CostoVariable > 0
                    && productos[actual].PorcentajeDeVentas > 0)
                {
                    resultado = true;
                }
            }
            return resultado;
        }
    }
}

