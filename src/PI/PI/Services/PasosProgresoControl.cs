using PI.Models;
using PI.Handlers;

namespace PI.Services
{
    public class PasosProgresoControl
    {
        protected int NumeroPaso = 1;
        private List<PasosProgresoControl> PasosControles = new(); 

        private void ConstruirPasos()
        {
            // se agrega el controlado de cada paso
            PasosControles.Add(new GastosFijosControl());
            PasosControles.Add(new GastoVariableControl());
            PasosControles.Add(new RentabilidadControl());
            PasosControles.Add(new InversionInicialControl());
            PasosControles.Add(new FlujoDeCajaControl());
            // aqui se agregan los controladores de los pasos nuevos

        }

        // Brief: metodo director que llama al metodo plantilla de los hijos para determinar el paso maximo al que puede entrar el usuario
        // Param: recibe el analisis del cual determinar el paso maximo al que puede ingresar el usuario
        // Return: retorna el indice del paso maximo al que puede ingresar el usuario
        public int EstaActivoMaximo(AnalisisModel analisis)
        {
            // construimos instancias de cada paso y se agregan a la lista de pasos
            ConstruirPasos();

            // se asigna 1 al paso que se retorna. Como minimo se puede entrar al paso de estructura organizativa.
            int pasoActivoMaximo = this.NumeroPaso;

            // buscamos el primer paso que no este activo porque este no se puede acceder
            PasosProgresoControl? pasoMaximoAccesible = PasosControles.Find(x => x.EstaActivo(analisis) == false);

            // si el find retorna no nulo es que no se econctro un paso no accesible
            if (pasoMaximoAccesible is not null)
            {
                // asignamos el numero de paso a la variable
                // le restamos porque pasoMaximoAccesible es el paso al que no se puede acceder, su posicion menos 1 es maximo paso que si se peude acceder
                pasoActivoMaximo = pasoMaximoAccesible.NumeroPaso - 1;
            } else
            {
                // si es nulo el resultado de la busqueda indicamos que el maximo paso accesible es el ultimo paso que haya en la lista
                // esto porque si se obtuvo nulo es que no hubo nigun paso no activo en las lista, por lo tanto todos esta activos
                // le sumamos 1 porque en la lista de pasos no se incluye el paso default que es la estructura organizativa
                pasoActivoMaximo = PasosControles.Count + 1;
            }

            return pasoActivoMaximo;
        }

        // metodo plantilla que implementan los hijos para determinar si un paso esta activo
        // Param: recibe el analisis del cual determinar el paso maximo al que puede ingresar el usuario
        // Return: verdadero si el paso esta activo y se puede ingresar
        protected virtual bool EstaActivo(AnalisisModel analisis)
        {
            // nunca se llama este metodo desde una instancia del padre
            // como es virtual la funcion se debe implementar en el padre, no obstante no se va a usar
            // por eso su valor de retorno no tiene importancia
            return false;
        }
    }

    public class GastosFijosControl : PasosProgresoControl
    {
        // Se crea instancia del handler
        private EstructuraOrgHandler EstructuraOrgHandler = new();
        public GastosFijosControl()
        {
            base.NumeroPaso = 2;
        }

        // Indica si el analisis posee puestos
        // (Retorna un bool que indica si hay puestos o no | Parametros: modelo del analisis que se desea verificar)
        // Se encarga de verificar si existen puestos dentro de un análisis.
        override protected bool EstaActivo(AnalisisModel analisis)
        {
            bool resultado = false;
            
            // Se obtiene de la base de datos los diferentes puestos del Análisis.
            List<PuestoModel> puestos = EstructuraOrgHandler.ObtenerListaDePuestos(analisis.FechaCreacion);

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
        // se crea instancia del handler
        private GastoFijoHandler GastoFijoHandler = new();

        public GastoVariableControl()
        {
            base.NumeroPaso = 3;
        }

        // Indica si el analisis posee gastos fijos
        // (Retorna un bool que indica si hay gastos fijos o no | Parametros: modelo del analisis que se desea verificar)
        // Determina si un análisis contiene gastos fijos.
        override protected bool EstaActivo(AnalisisModel analisis)
        {
            bool resultado = false;
            
            
            // mediante el handler, se obtiene de la base de datos la cantidad de gastos fijos que contiene un análisis.
            List<GastoFijoModel> gastosFijos = GastoFijoHandler.ObtenerGastosFijos(analisis.FechaCreacion);

            // se revisa si hay mas de 4 gastos fijos porque siempre existen los gastos de: 
            // salarios, beneficios, prestaciones laboralos y seguro social
            if (gastosFijos.Count > 4)
            {
                resultado = true;
            }
            return resultado;
        }
    }

    public class RentabilidadControl : PasosProgresoControl
    {
        private ProductoHandler ProductoHandler = new();

        public RentabilidadControl()
        {
            base.NumeroPaso = 4;
        }

        // metodo que verifica si el paso de analisis rentabilidad esta disponible o no.
        // recibe el enalisis del cual determiar si el paso esta disponible
        // retorna true si el paso esta disponible
        override protected bool EstaActivo(AnalisisModel analisis)
        {
            bool resultado = false;

            // creamos un handlere de producto para acceder a la base de datos
            
            // verificamos si hay al menos un producto en la base de datos
            if (ProductoHandler.ObtenerProductos(analisis.FechaCreacion).Count > 0)
            {
                resultado = true;
            }
            return resultado;
        }
    }

    public class InversionInicialControl : PasosProgresoControl
    {
        private ProductoHandler ProductoHandler = new();
        public InversionInicialControl()
        {
            base.NumeroPaso = 5;
        }

        // método que verifica si es posible que exista una meta de ventas en el análisis de rentabilidad.
        // detalle: sirve para determinar si la tarjeta de la inversión inicial se debe habilitar.
        // se asume que si un producto tiene valores en algunos de sus atributos, la meta de ventas ha sido calculada.
        override protected bool EstaActivo(AnalisisModel analisis)
        {
            bool resultado = false;

            List<ProductoModel> productos = ProductoHandler.ObtenerProductos(analisis.FechaCreacion);

            for (int actual = 0; actual < productos.Count && resultado == false; ++actual)
            {
                if (productos[actual].Precio > 0
                    && productos[actual].PorcentajeDeVentas > 0)
                {
                    resultado = true;
                }
            }
            return resultado;
        }
    }


    public class FlujoDeCajaControl : PasosProgresoControl
    {
        private InversionInicialHandler InversionInicialHandler = new();
        private ProductoHandler ProductoHandler = new();
        public FlujoDeCajaControl()
        {
            base.NumeroPaso = 6;
        }

        // método que verifica si se puede acceder al paso de flujo de caja
        // detalle: si el negocio esta en marcha solo revisamos que hayan productos con precio o porcentaje
        // si no esta iniciado revisamos que haya al menos un gasto inicial
        override protected bool EstaActivo(AnalisisModel analisis)
        {
            bool resultado = false;
            // si el estado es false, el negocio no esta iniciado
            // es decir que si esta disponible el paso de inversion inicial
            if (analisis.Configuracion.EstadoNegocio == false)
            {
                List<GastoInicialModel> gastos = InversionInicialHandler.ObtenerGastosIniciales(analisis.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                if (gastos.Count > 0)
                {
                    resultado = true;
                }
            } else
            {
                // si es true el estado del negocio significa que esta en marcha
                // es decir que el paso de inversion inicial no esta disponible
                List<ProductoModel> productos = ProductoHandler.ObtenerProductos(analisis.FechaCreacion);

                for (int actual = 0; actual < productos.Count && resultado == false; ++actual)
                {
                    if (productos[actual].Precio > 0
                        && productos[actual].PorcentajeDeVentas > 0)
                    {
                        resultado = true;
                    }
                }
            }

            return resultado;
        }
    }
}

