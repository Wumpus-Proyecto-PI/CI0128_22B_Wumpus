using System.Data.SqlClient;
using System.Data;
using PI.Models;

namespace PI.Handlers
{
    public class AnalisisHandler : Handler
    {
        // Handler de analisis
        public AnalisisHandler(): base() { }

        // Obtiene un analisis especifico
        // (Retorna la clase analisis | Parametros: fecha de creacion del analisis que se necesita)
        public AnalisisModel ObtenerUnAnalisis(DateTime fechaCreacion)
        {
            // String con la consulta que obtiene el analisis con la fecha que se ingresa como argumento 
            string consulta = "SELECT * FROM ANALISIS Where fechaCreacion='" 
                + fechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            DateTime fechaAnalisisActual = (DateTime)tablaResultado.Rows[0]["fechaCreacion"];
            bool tipoAnalisisActual = Convert.ToBoolean(ObtenerTipoAnalisis(fechaAnalisisActual));
            
            // Crea el modelo del analisis obtenido 
            AnalisisModel analisisResultado = new AnalisisModel
            {
                FechaCreacion = Convert.ToDateTime(tablaResultado.Rows[0]["fechaCreacion"]),
                Configuracion = { EstadoNegocio = tipoAnalisisActual, fechaAnalisis = fechaAnalisisActual },
                GananciaMensual = Convert.ToDecimal(tablaResultado.Rows[0]["gananciaMensual"]),
                EstadoAnalisis = Convert.ToInt32(tablaResultado.Rows[0]["estadoAnalisis"]),
            };

            return analisisResultado;
        }

        // Devuelve el analisis con la fehca mas reciente
        // (Retorna la clase con el analisis obtenido | Parametros: id del negocio del que se quiere obtener el analisis)
        public AnalisisModel ObtenerAnalisisMasReciente(int ideNegocio)
        {
            // String con la consulta que obtiene de la base de datos el analisis con mas reciente respecto al id del negocio indicado
            string consulta = "SELECT TOP 1 * FROM ANALISIS Where idNegocio="
                + ideNegocio.ToString() + " ORDER BY ANALISIS.fechaCreacion DESC";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            DateTime fechaAnalisisActual = (DateTime)tablaResultado.Rows[0]["fechaCreacion"];
            bool tipoAnalisisActual = Convert.ToBoolean(ObtenerTipoAnalisis(fechaAnalisisActual));
            
            // Crea el modelo del analisis obtenido 
            AnalisisModel analisisMasReciente = new AnalisisModel
            {
                FechaCreacion = Convert.ToDateTime(tablaResultado.Rows[0]["fechaCreacion"]),
                GananciaMensual = Convert.ToDecimal(tablaResultado.Rows[0]["gananciaMensual"]),
                EstadoAnalisis = Convert.ToInt32(tablaResultado.Rows[0]["estadoAnalisis"]),
                Configuracion = { EstadoNegocio = tipoAnalisisActual, fechaAnalisis = fechaAnalisisActual }
            };
            
            return analisisMasReciente;
        }

        // Obtiene los analisis de un negocio
        // (Retorna una lista con los analisis del negocio | Parametros: id del negocio)
        public List<AnalisisModel> ObtenerAnalisis(int IDNegocio)
        {
            List<AnalisisModel> analisisDelNegocio = new List<AnalisisModel>();
            
            // string que tiene la consulta para obtener la lista de analisis de un negocio indicado
            string consulta = "SELECT IDNegocio, A.fechaCreacion, A.gananciaMensual, A.estadoAnalisis from ANALISIS as A inner join Negocio as N " +

                "on A.IDNegocio = N.ID Where IDNegocio = " + IDNegocio + " ORDER BY A.fechaCreacion desc";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                // Crea la instancia de clase para los analisis obtenidos 
                DateTime fechaAnalisisActual = (DateTime)columna["fechaCreacion"];
                bool tipoAnalisisActual = Convert.ToBoolean(ObtenerTipoAnalisis(fechaAnalisisActual));
                analisisDelNegocio.Add(
                new AnalisisModel
                {
                    FechaCreacion = Convert.ToDateTime(fechaAnalisisActual),
                    GananciaMensual = Convert.ToDecimal(columna["gananciaMensual"]),
                    EstadoAnalisis = Convert.ToInt32(columna["estadoAnalisis"]),
                    Configuracion = { EstadoNegocio = tipoAnalisisActual, fechaAnalisis = fechaAnalisisActual }
                }
                );
            }

            return analisisDelNegocio;
        }
        // Obtiene la fecha de creacion de un analisis
        // (Retorna la fecha de creacion de un analisis | Parametros: id del negocio del cual se obtiene la fecha del analisis mas reciente)
        public DateTime UltimaFechaCreacion(string IDNegocio)
        {
            // String con la consulta para obtener la fehca mas reciente de los analisis dentro de un negocio especifico 
            string consulta = "SELECT Max(A.fechaCreacion) as UltimoAnalisis from ANALISIS as A inner join Negocio as N " +
                "on A.IDNegocio = N.ID Where IDNegocio = " + IDNegocio + "";
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            DateTime fecha = (DateTime)tablaResultado.Rows[0]["UltimoAnalisis"];
 
            return fecha;
        }

        // Obtiene el estado de un analisis en especifico
        // (Retorna un Boolean que indica el estado del analisis | Parametros: fecha de creacion del analisis)
        public Boolean ObtenerTipoAnalisis(DateTime FechaCreacion)
        {
            Boolean tipo = false;

            string fecha = FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // string con la consulta que obtiene el tipo que posee el analisis indicado de la base de datos
            string consulta = "SELECT tipoNegocio FROM CONFIGURACION as C join ANALISIS as A on C.fechaAnalisis = A.FechaCreacion " +
                "WHERE C.FechaAnalisis = '" + fecha + "' ";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                tipo = Convert.ToBoolean(tablaResultado.Rows[0]["tipoNegocio"]);
            }

            return tipo;
        }

        // Agrega un analisis al negocio indicado
        // (Parametros: id del negocio al que se le quiere ingresar un analisis, tipo del analisis que se va a ingresar)

        public DateTime IngresarAnalisis(int idNegocio, string tipo)
        {
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // string con consulta que inserta el analisis con los valores ingresados a la base de datos
            string consulta = "INSERT INTO Analisis (idNegocio,FechaCreacion) VALUES (" + idNegocio + ", '" + sqlFormattedDate + "');";
            enviarConsultaVoid(consulta);

            // Crea una config por defecto asociada al analisis
            CrearConfigPorDefecto(sqlFormattedDate, tipo);
            return myDateTime;
        }

        // Ingresa una configuracion con la fecha del analisis indicada
        // (Parametros: fecha del analisis a la que se le da la configuracion, tipo o estado del anailsis)
        public void CrearConfigPorDefecto(string fecha, string tipo)
        {
            int tipoAnalisis;
            if (tipo == "Emprendimiento")
            {
                tipoAnalisis = 0; 
            } else
            {
                tipoAnalisis = 1;
            }
            // String con la consulta que inserta la configuracion al analisis correspondiente 
            string consulta = "INSERT INTO CONFIGURACION (fechaAnalisis, tipoNegocio) VALUES ('" + fecha + "', " + tipoAnalisis + ")";
            enviarConsultaVoid(consulta);
        }

        // Obtiene la configuracion del analisis especificado
        // (Retorna una clase con la configuracion del analisis | Parametros: fecha del analisis)
        public ConfigAnalisisModel ObtenerConfigAnalisis(DateTime fechaAnalisis)
        {
            // String que llama a la funcion de la base de datos que obtiene los analisis
            string consulta = "execute ObtenerConfigAnalisis @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            // Se crea una clase de configuracion con los resultados de la consulta
            ConfigAnalisisModel resultado = new ConfigAnalisisModel
            {
                fechaAnalisis = Convert.ToDateTime(tablaResultado.Rows[0]["fechaAnalisis"]),
                EstadoNegocio = Convert.ToBoolean(tablaResultado.Rows[0]["tipoNegocio"]),
                PorcentajePL = Convert.ToDecimal(tablaResultado.Rows[0]["porcentajePL"]),
                PorcentajeSS = Convert.ToDecimal(tablaResultado.Rows[0]["porcentajeSS"])
            };
            return resultado;
        }

        // Actualiza la configuracion del analisis
        // (Parametros: clase de la configuracion que se desea establecer en el analisis)
        public void ActualizarConfiguracionAnalisis(ConfigAnalisisModel configuracionNueva)
        {
            // String que llama a la funcion de la base de datos que modifica la configuracion de un analisis especifico 
            string actualizar = "execute insertarConfiguracionAnalisis @fechaAnalisis='"
                + configuracionNueva.fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff")
                + "', @procentajeSS="
                + configuracionNueva.PorcentajeSS.ToString()
                + ",@procentajePL=" + configuracionNueva.PorcentajePL.ToString();
            enviarConsultaVoid(actualizar);
        }

        public bool ActualizarGananciaMensual(decimal monto, DateTime fechaAnalisis) {
            Console.WriteLine(fechaAnalisis);
            var consulta = @"EXEC ActualizarGananciaMensual @ganancia = " + monto + ", @fecha = " + "'" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            var cmdParaConsulta = new SqlCommand(consulta, conexion);

            conexion.Open();
            bool exito = cmdParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }
        public decimal ObtenerGananciaMensual(DateTime FechaCreacion)
        {
            decimal GananciaMensual = 0.0m;

            string fecha = FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // string con la consulta que obtiene el tipo que posee el analisis indicado de la base de datos
            string consulta = "SELECT gananciaMensual FROM analisis" + "WHERE fechaCreacion = '" + fecha + "' ";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                GananciaMensual = Convert.ToDecimal(tablaResultado.Rows[0]["gananciaMensual"]);
            }

            return GananciaMensual;
        }

    }
}
