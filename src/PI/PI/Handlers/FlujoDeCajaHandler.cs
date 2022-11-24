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
            string consulta = "EXEC ObtenerEgresosMes @fechaAnalisis ='" + FechaAnalisis.ToString() +"', @nombreMes ='" + NombreMes +"';";
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
            string consulta = "EXEC ObtenerEgresosMes @fechaAnalisis ='" + FechaAnalisis.ToString() + "', @nombreMes ='" + NombreMes + "';";
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
            string consulta = "EXEC AgregarEgresoMes @fechaAnalisis='" + Egreso.FechaAnalisis.ToString()
                + "', @nombreMes ='" + Egreso.Mes + "', @tipo=" + Egreso.Tipo + ", @monto=" + Egreso.Monto;
            enviarConsultaVoid(consulta);
        }

        // Agrego un Ingreso a la base de datos
        public void AgregarIngreso(IngresoModel Ingreso)
        {
            string consulta = "EXEC AgregarEgresoMes @fechaAnalisis='" + Ingreso.FechaAnalisis.ToString()
                + "', @nombreMes ='" + Ingreso.Mes + "', @tipo=" + Ingreso.Tipo + ", @monto=" + Ingreso.Monto;
            enviarConsultaVoid(consulta);
        }

        //public void crearMeses(DateTime fechaAnalisis)
        //{
        //    string consulta = "EXEC crearMesesDeAnalisis @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
        //    enviarConsultaVoid(consulta);
        //}

        public void crearIngresos(DateTime fechaAnalisis)
        {
            string consulta = "EXEC crearIngresosPorMes @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsultaVoid(consulta);
        }

        public void crearEgresos(DateTime fechaAnalisis)
        {
            string consulta = "EXEC crearEgresosPorMes @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsultaVoid(consulta);
        }

        public void crearFlujoDeCaja(DateTime fechaAnalisis)
        {
            //this.crearMeses(fechaAnalisis);
            this.crearIngresos(fechaAnalisis);
            this.crearEgresos(fechaAnalisis);
        }

        public List<IngresoModel> obtenerIngresos(DateTime fechaAnalisis)
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

        public void actualizarIngreso(IngresoModel ingreso)
        {
            string consulta = "UPDATE INGRESO" +
                              " SET monto = " + ingreso.Monto +
                              " WHERE fechaAnalisis = '" + ingreso.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                              "' AND mes = '" + ingreso.Mes +
                              "' AND tipo = '" + ingreso.Tipo + "'";
            enviarConsultaVoid(consulta);
        }

        public decimal obtenerMontoTotalDeIngresosPorMes(string mes, DateTime fechaAnalisis)
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

        public decimal obtenerMontoTotalDeEgresosPorMes(string mes, DateTime fechaAnalisis)
        {
            decimal total = 0.0m;

            string consulta = "SELECT SUM(monto) as total" +
                             " FROM EGRESO" +
                             " WHERE fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                             "' AND mes = '" + mes + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (!tablaResultado.Rows[0].IsNull("total"))
            {
                total = Convert.ToDecimal(tablaResultado.Rows[0]["total"]);
            }

            return total;
        }
    }
}
