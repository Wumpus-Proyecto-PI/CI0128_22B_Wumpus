using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using PI.Controllers;
using System.Data;
using System.Data.SqlClient;
using PI.Models;
using PI.Views.Shared.Components.GastoFijo;

namespace PI.Handlers
{   
    // Handler de GastoFijo, clase encargada de recuperar datos de las tablas en la base de datos
    public class GastoFijoHandler : Handler
    {
        // Constructor de la clase
        public GastoFijoHandler(): base() { }

        // Crea una lista de los gastos fijos existentes en la BD
        // Retorna la lista de los gastos fijos
        public List<GastoFijoModel> ObtenerGastosFijos(DateTime fechaAnalisis)
        {
            List<GastoFijoModel> gastosFijos = new List<GastoFijoModel>();

            string consulta = "EXEC obtGastosFijosList @fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                gastosFijos.Add(
                new GastoFijoModel
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    
                    FechaAnalisis = Convert.ToDateTime(columna["fechaAnalisis"]),
                    Monto = Convert.ToDecimal(columna["monto"]),
                    orden = Convert.ToInt32(columna["orden"])
                }
                );
            }
            // Acomoda en las primeras 4 posiciones los gastos de las estorg
            acomodarGastosFijos(gastosFijos);
            return gastosFijos;
        }

        // Recupera el último número de orden de los gastos fijos en la base de datos.
        // Retorna el último número de orden + 1, el cual es el número de orden para el siguiente gasto fijo.
        public int getNextOrden()
        {
            string consulta = "SELECT MAX(orden) AS orden FROM Gasto_Fijo";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            int result = 1;  // Valor por omisión (inicial)
            // Verifica que haya devuelto una tupla
            if (tablaResultado.Rows.Count > 0)
            {
                // Verifica que no sea nulo
                DataRow lastOrder = tablaResultado.Rows[0];
                if (!lastOrder.IsNull("orden"))
                {
                    result = Convert.ToInt32(lastOrder["orden"]) + 1;
                }
            }
            return result;
        }

        // Inserta el nuevo gasto fijo a la BD.
        public void ingresarGastoFijo(string nombreAnterior, string Nombre, string monto, DateTime fechaAnalisis)
        {
            // TODO arreglar el datetime para que est� asociado al an�lisis realmente.
            string consulta = "EXECUTE insertarGastoFijo '"
                + nombreAnterior + "', '"
                + Nombre + "', '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") 
                + "', '" + monto + "',"
                + getNextOrden() + ";";

            enviarConsulta(consulta);
        }

        // Elimina un gasto fijo de la BD
        public void eliminarGastoFijo(string Nombre, DateTime fechaAnalisis)
        {
            // TODO arreglar el datetime para que est� asociado al an�lisis realmente.
            string consulta = "EXECUTE eliminarGastoFijo '"
                + Nombre + "', '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "';";
            enviarConsulta(consulta);
        }

        // Obtiene y retorna la suma total mensual de los montos de gastos fijos
        public decimal obtenerTotalAnual(DateTime fechaAnalisis)
        {
            decimal totalAnual = 0.0m;

            string consulta = "EXEC obtSumGastosFijos '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (!tablaResultado.Rows[0].IsNull("totalAnual"))
            {
                totalAnual = Convert.ToDecimal(tablaResultado.Rows[0]["totalAnual"]);
            }

            return totalAnual;
        }

        // Crea o actualiza el gasto fijo de salarios netos
        public void actualizarSalariosNeto(DateTime fechaAnalisis, decimal seguroSocial, decimal prestaciones)
        {
            // Envia consulta a la base de datos, donde se encuentra el procedimiento almacenado encargado de calcular los salarios netos.
            string consulta = "EXEC actualizarSalariosNeto '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + seguroSocial.ToString() + "', '" + prestaciones.ToString() + "'";
            enviarConsulta(consulta);
        }

        // Crea o actualiza el gasto fijo de seguro social
        public void actualizarSeguroSocial(DateTime fechaAnalisis, decimal seguroSocial)
        {
            // Envia consulta a la base de datos, donde se encuentra el procedimiento almacenado encargado de calcular el monto de seguro social.
            string consulta = "EXEC actualizarGastoSeguroSocial '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + seguroSocial.ToString() + "'";
            enviarConsulta(consulta);
        }

        // Crea o actualiza el gasto fijo de salarios netos
        public void actualizarPrestaciones(DateTime fechaAnalisis, decimal prestaciones)
        {
            // Envia consulta a la base de datos, donde se encuentra el procedimiento almacenado encargado de calcular el monto de las prestaciones laborales.
            string consulta = "EXEC actualizarGastoPrestaciones @fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', @porcentaje = '" + prestaciones.ToString() + "';";
            enviarConsulta(consulta);
        }

        // Crea o actualiza el gasto fijo de salarios netos
        public void actualizarBeneficios(DateTime fechaAnalisis)
        {
            // Envia consulta a la base de datos, donde se encuentra el procedimiento almacenado encargado de calcular el monto total de los beneficios.
            string consulta = "EXEC actualizarBeneficios '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            enviarConsulta(consulta);
        }

        // Reordena la lista de gastos fijos para que los de la estructura siempre esten al comienzo 
        public void acomodarGastosFijos(List<GastoFijoModel> gastosFijos)
        {
            GastoFijoModel Beneficios = gastosFijos.Find(x => x.Nombre == "Beneficios de empleados");
            GastoFijoModel Prestaciones = gastosFijos.Find(x => x.Nombre == "Prestaciones laborales");
            GastoFijoModel Salarios = gastosFijos.Find(x => x.Nombre == "Salarios netos");
            GastoFijoModel Seguridad = gastosFijos.Find(x => x.Nombre == "Seguridad social");

            gastosFijos.Remove(Beneficios);
            gastosFijos.Remove(Prestaciones);
            gastosFijos.Remove(Salarios);
            gastosFijos.Remove(Seguridad);
            
            if (Beneficios != null)
            {
                gastosFijos.Insert(0, Beneficios);
            }
            if (Prestaciones != null)
            {
                gastosFijos.Insert(1, Prestaciones);
            }
            if (Salarios != null)
            {
                gastosFijos.Insert(2, Salarios);
            }
            if (Seguridad != null)
            {
                gastosFijos.Insert(3, Seguridad);
            }
        }
    }
}