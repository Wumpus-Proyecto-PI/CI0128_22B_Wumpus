using PI.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace PI.Services
{
    public class PasosProgresoControl
    {
        protected int NumeroPaso;
        private List<IControlPasos>? PasosControles = null;

        /// <summary>
        /// Controlador vacio que llaman los hijos al ser construidos
        /// </summary>
        protected PasosProgresoControl() { 
            this.NumeroPaso = 1;
        }

        /// <summary>
        /// Controlador que recibe por injección de dependecias cada controlador de pasos
        /// </summary>
        /// <param name="gastosFijosControl"></param>
        /// <param name="gastoVariableControl"></param>
        /// <param name="rentabilidadControl"></param>
        /// <param name="inversionInicialControl"></param>
        /// <param name="FlujoDeCajaControl"></param>
        [ActivatorUtilitiesConstructor]
        public PasosProgresoControl(GastosFijosControl gastosFijosControl, 
            GastoVariableControl gastoVariableControl,
            RentabilidadControl rentabilidadControl,
            InversionInicialControl inversionInicialControl,
            FlujoDeCajaControl FlujoDeCajaControl)
        {
            this.NumeroPaso = 1;
            int asignarNumPaso = this.NumeroPaso + 1;
            PasosControles = new();
            // se agrega el controlado de cada paso
            gastosFijosControl.NumeroPaso = asignarNumPaso++;
            PasosControles.Add(gastosFijosControl);

            gastoVariableControl.NumeroPaso = asignarNumPaso++;
            PasosControles.Add(gastoVariableControl);

            rentabilidadControl.NumeroPaso = asignarNumPaso++;
            PasosControles.Add(rentabilidadControl);

            inversionInicialControl.NumeroPaso = asignarNumPaso++;
            PasosControles.Add(inversionInicialControl);

            FlujoDeCajaControl.NumeroPaso = asignarNumPaso++;
            PasosControles.Add(FlujoDeCajaControl);
            // aqui se agregan los controladores de los pasos nuevos
        }

        // Brief: metodo director que llama al metodo plantilla de los hijos para determinar el paso maximo al que puede entrar el usuario
        // Param: recibe el analisis del cual determinar el paso maximo al que puede ingresar el usuario
        // Return: retorna el indice del paso maximo al que puede ingresar el usuario
        public async Task<int> EstaActivoMaximoAsync(Analisis analisis)
        {
            // construimos instancias de cada paso y se agregan a la lista de pasos

            // se asigna 1 al paso que se retorna. Como minimo se puede entrar al paso de estructura organizativa.
            int pasoActivoMaximo = this.NumeroPaso;

            // buscamos el primer paso que no este activo porque este no se puede acceder
            IControlPasos? pasoMaximoAccesible = null;
            for (int i = 0; i < PasosControles.Count && pasoMaximoAccesible is null; ++i)
            {
                if ((await PasosControles[i].EstaActivoAsync(analisis) == false))
                {
                    pasoMaximoAccesible = PasosControles[i];
                }
            }

            // si el find retorna no nulo es que no se econtro un paso no accesible
            if (pasoMaximoAccesible is not null)
            {
                // asignamos el numero de paso a la variable
                // le restamos porque pasoMaximoAccesible es el paso al que no se puede acceder, su posicion menos 1 es maximo paso que si se peude acceder
                pasoActivoMaximo = pasoMaximoAccesible.GetNumeroPaso() - 1;
            } else
            {
                // si es nulo el resultado de la busqueda indicamos que el maximo paso accesible es el ultimo paso que haya en la lista
                // esto porque si se obtuvo nulo es que no hubo nigun paso no activo en las lista, por lo tanto todos esta activos
                // le sumamos 1 porque en la lista de pasos no se incluye el paso default que es la estructura organizativa
                pasoActivoMaximo = PasosControles.Last().GetNumeroPaso();
            }

            return pasoActivoMaximo;
        }
    }

    interface IControlPasos
    {
        public Task<bool> EstaActivoAsync(Analisis analisis);

        public int GetNumeroPaso();
    }


    public class GastosFijosControl : PasosProgresoControl, IControlPasos
    {
        // Se crea instancia del handler
        private DataBaseContext? Context = null;
        public GastosFijosControl(DataBaseContext context)
        {
            Context = context;
        }

        // Indica si el analisis posee puestos
        // (Retorna un bool que indica si hay puestos o no | Parametros: modelo del analisis que se desea verificar)
        // Se encarga de verificar si existen puestos dentro de un análisis.
        public async Task<bool> EstaActivoAsync(Analisis analisis)
        {
            return await this.Context.Puestos.AnyAsync(puesto => puesto.FechaAnalisis == analisis.FechaCreacion);
        }

        public int GetNumeroPaso()
        {
            return base.NumeroPaso;
        }
    }

    public class GastoVariableControl : PasosProgresoControl, IControlPasos
    {

        private DataBaseContext? Context = null;
        public GastoVariableControl(DataBaseContext context)
        {
            Context = context;
        }

        // Indica si el analisis posee gastos fijos
        // (Retorna un bool que indica si hay gastos fijos o no | Parametros: modelo del analisis que se desea verificar)
        // Determina si un análisis contiene gastos fijos.
        public async Task<bool> EstaActivoAsync(Analisis analisis)
        {
            int cantidad = await this.Context.GastosFijos.CountAsync(gastoFijo => gastoFijo.FechaAnalisis == analisis.FechaCreacion);
            return cantidad > 4;
        }

        public int GetNumeroPaso()
        {
            return base.NumeroPaso;
        }
    }

    public class RentabilidadControl : PasosProgresoControl, IControlPasos
    {
        private DataBaseContext? Context = null;
        public RentabilidadControl(DataBaseContext context)
        {
            Context = context;
        }

        // metodo que verifica si el paso de analisis rentabilidad esta disponible o no.
        // recibe el enalisis del cual determiar si el paso esta disponible
        // retorna true si el paso esta disponible
        public async Task<bool> EstaActivoAsync(Analisis analisis)
        {
            return await this.Context.Productos.AnyAsync(producto => producto.FechaAnalisis == analisis.FechaCreacion);
        }

        public int GetNumeroPaso()
        {
            return base.NumeroPaso;
        }
    }

    public class InversionInicialControl : PasosProgresoControl, IControlPasos
    {
        private DataBaseContext? Context = null;
        public InversionInicialControl(DataBaseContext context)
        {
            Context = context;
        }

        // método que verifica si es posible que exista una meta de ventas en el análisis de rentabilidad.
        // detalle: sirve para determinar si la tarjeta de la inversión inicial se debe habilitar.
        // se asume que si un producto tiene valores en algunos de sus atributos, la meta de ventas ha sido calculada.
        public async Task<bool> EstaActivoAsync(Analisis analisis)
        {
            return await this.Context.Productos.Where(producto => producto.FechaAnalisis == analisis.FechaCreacion).AllAsync(
                producto => producto.Precio > 0 && producto.PorcentajeDeVentas > 0);
        }

        public int GetNumeroPaso()
        {
            return base.NumeroPaso;
        }
    }

    public class FlujoDeCajaControl : PasosProgresoControl, IControlPasos
    {
        private DataBaseContext? Context = null;
        public FlujoDeCajaControl(DataBaseContext context)
        {
            Context = context;
        }

        // método que verifica si se puede acceder al paso de flujo de caja
        // detalle: si el negocio esta en marcha solo revisamos que hayan productos con precio o porcentaje
        // si no esta iniciado revisamos que haya al menos un gasto inicial
        public async Task<bool> EstaActivoAsync(Analisis analisis)
        {
            bool estaActivo = false;
            if (analisis.Configuracion.TipoNegocio == 0)
            {
                estaActivo = await Context.InversionInicial.AnyAsync(inversionInicial => inversionInicial.FechaAnalisis == analisis.FechaCreacion);
            } else
            {
                estaActivo = await this.Context.Productos.AllAsync(
                producto => producto.FechaAnalisis == analisis.FechaCreacion &&
                producto.Precio > 0 && producto.PorcentajeDeVentas > 0);
            }

            return estaActivo;
        }

        public int GetNumeroPaso()
        {
            return base.NumeroPaso;
        }
    }
}

