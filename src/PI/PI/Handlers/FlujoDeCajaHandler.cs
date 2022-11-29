using PI.Models;
using System.Data;

namespace PI.Handlers
{
    public class FlujoDeCajaHandler : Handler
    {
        public FlujoDeCajaHandler() : base() { }

        // Obtiene los Egresos de un determinado mes en un análisis
        public List<EgresoModel> ObtenerEgresosMes(string NombreMes, DateTime FechaAnalisis) 
        {
            string consulta = "EXEC ObtenerEgresosMes @fechaAnalisis ='" + FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', @nombreMes ='" + NombreMes +"';";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            List<EgresoModel> egresos = new List<EgresoModel>();
            foreach (DataRow columna in tablaResultado.Rows)
            {
                egresos.Add(
                new EgresoModel
                {
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Tipo = Convert.ToString(columna["tipo"]),
                    Monto = Convert.ToDecimal(columna["monto"]),
                    Mes = Convert.ToString(columna["mes"])
                }
                ) ;
            }
            return egresos;
        }

        // Obtiene los Ingresos de un determinado mes en un análisis
        public List<IngresoModel> ObtenerIngresosMes(string NombreMes, DateTime FechaAnalisis)
        {
            string consulta = "EXEC ObtenerIngresosMes @fechaAnalisis ='" + FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', @nombreMes ='" + NombreMes + "';";
            Console.WriteLine(consulta);
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            List<IngresoModel> ingresos = new List<IngresoModel>();
            foreach (DataRow columna in tablaResultado.Rows)
            {
                ingresos.Add(
                new IngresoModel
                {
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Tipo = Convert.ToString(columna["tipo"]),
                    Monto = Convert.ToDecimal(columna["monto"]),
                    Mes = Convert.ToString(columna["mes"])
                }
                );
            }
            return ingresos;
        }

        // obtiene los meses segun una fecha de analisis
        public List<MesModel> ObtenerMeses(DateTime fechaAnalisis)
        {
            string consulta = "SELECT * FROM MES WHERE fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            List<MesModel> meses = new List<MesModel>();
            foreach (DataRow columna in tablaResultado.Rows)
            {
                meses.Add(
                new MesModel
                {
                    NombreMes = Convert.ToString(columna["nombre"]),
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    InversionPorMes = Convert.ToDecimal(columna["inversionPorMes"]),
 
                }
                );
            }
            return meses;
        }

        // Llama procedure de la base de datos que crea todos los ingresos de un analisis con monto en 0
        public void CrearIngresos(DateTime fechaAnalisis)
        {
            string consulta = "EXEC crearIngresosPorMes @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsultaVoid(consulta);
        }

        public void CrearEgresos(DateTime fechaAnalisis)
        {
            string consulta = "EXEC crearEgresosPorMes @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsultaVoid(consulta);
        }

        // Crea todos los ingresos y egresos de un análisis
        public void CrearFlujoDeCaja(DateTime fechaAnalisis)
        {
            this.CrearIngresos(fechaAnalisis);
            this.CrearEgresos(fechaAnalisis);
        }

        // Obtiene todos los ingresos de un análisis desde la base de datos
        public List<IngresoModel> ObtenerIngresos(DateTime fechaAnalisis)
        {
            string consulta = "SELECT * FROM INGRESO WHERE fechaAnalisis = '" +  fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            List<IngresoModel> ingresos = new List<IngresoModel>();
            foreach (DataRow columna in tablaResultado.Rows)
            {
                ingresos.Add(
                new IngresoModel
                {
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Tipo = Convert.ToString(columna["tipo"]),
                    Monto = Convert.ToDecimal(columna["monto"]),
                    Mes = Convert.ToString(columna["mes"])
                }
                );
            }
            return ingresos;
        }

        // Actualiza el monto de un ingreso existente en la base de datos
        public void ActualizarIngreso(IngresoModel ingreso)
        {
            string consulta = "UPDATE INGRESO" +
                              " SET monto = " + ingreso.Monto +
                              " WHERE fechaAnalisis = '" + ingreso.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                              "' AND mes = '" + ingreso.Mes +
                              "' AND tipo = '" + ingreso.Tipo + "'";
            enviarConsultaVoid(consulta);
        }

        // Retorna el monto total de los ingresos en un determinado mes de un análisis
        public decimal ObtenerMontoTotalDeIngresosPorMes(string mes, DateTime fechaAnalisis)
        {
            decimal total = 0.0m;

            string consulta = "SELECT SUM(monto) as total" +
                             " FROM INGRESO" +
                             " WHERE fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                             "' AND mes = '" + mes + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (!tablaResultado.Rows[0].IsNull("total"))
            {
                total = Convert.ToDecimal(tablaResultado.Rows[0]["total"]);
            }

            return total;
        }

        public List<EgresoModel> obtenerEgresos(DateTime fechaAnalisis)
        {
            string consulta = "SELECT * FROM EGRESO WHERE fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            List<EgresoModel> egresos = new List<EgresoModel>();
            foreach (DataRow columna in tablaResultado.Rows)
            {
                egresos.Add(
                new EgresoModel
                {
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Tipo = Convert.ToString(columna["tipo"]),
                    Monto = Convert.ToDecimal(columna["monto"]),
                    Mes = Convert.ToString(columna["mes"])
                }
                );
            }
            return egresos;
        }

        public void actualizarEgreso(EgresoModel egreso)
        {
            string consulta = "UPDATE EGRESO" +
                              " SET monto = " + egreso.Monto +
                              " WHERE fechaAnalisis = '" + egreso.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                              "' AND mes = '" + egreso.Mes +
                              "' AND tipo = '" + egreso.Tipo + "'";
            enviarConsultaVoid(consulta);
        }

        public decimal obtenerMontoTotalDeEgresosPorMes(MesModel mes)
        {
            decimal total = 0.0m;

            string consulta = "SELECT SUM(monto) as total" +
                             " FROM EGRESO" +
                             " WHERE fechaAnalisis = '" + mes.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                             "' AND mes = '" + mes.NombreMes + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (!tablaResultado.Rows[0].IsNull("total"))
            {
                total = Convert.ToDecimal(tablaResultado.Rows[0]["total"]);
            }

            return total;
        }

        public decimal obtenerInversionDelMes(MesModel mes)
        {
            decimal inversion = 0.0m;

            string consulta = "SELECT inversionPorMes as inversion" +
                             " FROM MES" +
                             " WHERE fechaAnalisis = '" + mes.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND nombre = '" + mes.NombreMes + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (!tablaResultado.Rows[0].IsNull("inversion"))
            {
                inversion = Convert.ToDecimal(tablaResultado.Rows[0]["inversion"]);
            }

            return inversion;
        }

        public decimal obtenerMontoTotalInversiones(DateTime fechaAnalisis)
        {
            decimal total = 0.0m;

            string consulta = "SELECT SUM(inversionPorMes) as total" +
                             " FROM MES" +
                             " WHERE fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (!tablaResultado.Rows[0].IsNull("total"))
            {
                total = Convert.ToDecimal(tablaResultado.Rows[0]["total"]);
            }

            return total;
        }

        public void actualizarInversionPorMes(MesModel mes)
        {
            string consulta = "UPDATE MES" +
                             " SET inversionPorMes = " + mes.InversionPorMes +
                             " WHERE fechaAnalisis = '" + mes.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                             "' AND nombre = '" + mes.NombreMes + "'";
            enviarConsultaVoid(consulta);
        }
    }
}
