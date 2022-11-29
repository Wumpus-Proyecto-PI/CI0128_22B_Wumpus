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

        // Agrega un Egreso a la base de datos
        public void AgregarEgreso(EgresoModel Egreso) 
        {
            string consulta = "EXEC AgregarEgresoMes @fechaAnalisis='" + Egreso.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff")
                + "', @nombreMes ='" + Egreso.Mes + "', @tipo=" + Egreso.Tipo + ", @monto=" + Egreso.Monto;
            enviarConsultaVoid(consulta);
        }

        // Agrego un Ingreso a la base de datos
        public void AgregarIngreso(IngresoModel Ingreso)
        {
            string consulta = "EXEC AgregarEgresoMes @fechaAnalisis='" + Ingreso.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff")
                + "', @nombreMes ='" + Ingreso.Mes + "', @tipo=" + Ingreso.Tipo + ", @monto=" + Ingreso.Monto;
            enviarConsultaVoid(consulta);
        }

        //public void crearMeses(DateTime fechaAnalisis)
        //{
        //    string consulta = "EXEC crearMesesDeAnalisis @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
        //    enviarConsultaVoid(consulta);
        //}

        // Metodo que crea los ingresos en la base de datos
        public void CrearIngresos(DateTime fechaAnalisis)
        {
            string consulta = "EXEC crearIngresosPorMes @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsultaVoid(consulta);
        }

        // Metodo que crea los egresos en la base de datos
        public void CrearEgresos(DateTime fechaAnalisis)
        {
            string consulta = "EXEC crearEgresosPorMes @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsultaVoid(consulta);
        }

        // Metodo que crea el flujo de caja (ingresos y egresos)
        public void CrearFlujoDeCaja(DateTime fechaAnalisis)
        {
            //this.crearMeses(fechaAnalisis);
            this.CrearIngresos(fechaAnalisis);
            this.CrearEgresos(fechaAnalisis);
        }

        // Metodo que obtiene los ingresos de la base de datos segun una fecha del analisis
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

        // Metodo que actualiza un ingreso
        public void ActualizarIngreso(IngresoModel ingreso)
        {
            string consulta = "UPDATE INGRESO" +
                              " SET monto = " + ingreso.Monto +
                              " WHERE fechaAnalisis = '" + ingreso.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                              "' AND mes = '" + ingreso.Mes +
                              "' AND tipo = '" + ingreso.Tipo + "'";
            enviarConsultaVoid(consulta);
        }

        // Metodo que obtiene el monto total de los ingresos de un mes 
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

        // Metodo que obtiene los egresos segun una fecha de analisis
        public List<EgresoModel> ObtenerEgresos(DateTime fechaAnalisis)
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

        // Metodo que actualiza un egreso
        public void ActualizarEgreso(EgresoModel egreso)
        {
            string consulta = "UPDATE EGRESO" +
                              " SET monto = " + egreso.Monto +
                              " WHERE fechaAnalisis = '" + egreso.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                              "' AND mes = '" + egreso.Mes +
                              "' AND tipo = '" + egreso.Tipo + "'";
            enviarConsultaVoid(consulta);
        }

        // Metodo que obtiene el monto total de los egresos de un mes 
        public decimal ObtenerMontoTotalDeEgresosPorMes(MesModel mes)
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

        // Metodo que obtiene la fraccion de la inversion inicial que posee el mes 
        public decimal ObtenerInversionDelMes(MesModel mes)
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

        // Metodo que obtiene el monto total de las inversiones de todos los meses 
        public decimal ObtenerMontoTotalInversiones(DateTime fechaAnalisis)
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

        // Metodo que actualiza la fraccion de la inversion inicial que posee el mes 
        public void ActualizarInversionPorMes(MesModel mes)
        {
            string consulta = "UPDATE MES" +
                             " SET inversionPorMes = " + mes.InversionPorMes +
                             " WHERE fechaAnalisis = '" + mes.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                             "' AND nombre = '" + mes.NombreMes + "'";
            enviarConsultaVoid(consulta);
        }
    }
}
