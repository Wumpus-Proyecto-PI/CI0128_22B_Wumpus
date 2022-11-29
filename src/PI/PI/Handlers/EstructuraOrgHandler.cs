using System;
using System.Collections.Generic;
using PI.Models;
using PI.Handlers;
using PI.Services;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace PI.Handlers
{
    // Clase handler que se encarga de manipular informaci�n de la estructura organizativa en la base de datos 
    // esto incluye puestos y beneficios, actualizar, insertar y eliminar
    // Esta clase hereda de Handler, clase que contiene el string de conexi�n y m�todos de consulta
    public class EstructuraOrgHandler : Handler
    {
        // constructor llama al construtor del padre que contiene el string de conexi�n
        public EstructuraOrgHandler() : base() { }

        // M�todo que permite ingresar un puesto nuevo a la base de datos
        // Recibe el string nombrePuesto a ingresar y un modelPuesto.
        // EL string nombrePuesto es el nombre antes de modificaciones. Esto se usa para actualizaciones.
        // El modelo es para acceder a todos los datos de tal puesto
        public void InsertarPuesto(string nombrePuesto, PuestoModel puesotAInsertar)
        {
            if (FormatManager.EsAlfanumerico(nombrePuesto) && FormatManager.EsAlfanumerico(puesotAInsertar.Nombre)) {
                // primero se revisa si ya existe el puesto en la base
                if (ExistePuestoEnBase(nombrePuesto, puesotAInsertar.FechaAnalisis))
                {
                    // si existe el puesto lo actualizamos
                    ActualizarPuesto(nombrePuesto, puesotAInsertar);
                } else {
                    // en caso de no existir lo insertamos como un nuevo puesto
                    // en estaconsulta de sql convertimos los decimales seg�n si es con coma o punto para evitar errores
                    string insert = "INSERT INTO PUESTO values ('"
                    + puesotAInsertar.Nombre + "', '"
                    + puesotAInsertar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', "
                    + puesotAInsertar.Plazas.ToString() + ", "
                    + puesotAInsertar.SalarioBruto.ToString().Replace(",", ".") + ", "
                    + puesotAInsertar.Beneficios.ToString().Replace(",", ".") + ")";

                    // realizamos la consulta
                    // este m�todo es heredado del padre y permite enviar consultas de actualizaci�n, borrado e inserci�n   
                    base.enviarConsulta(insert);
                }
            }
        }

        // M�todo que permite revisar si ya existe un puesto en la base
        // Recibe el nombre el puesto y la fecha del an�lisis dodne buscarlo
        // Retorna verdadero si existe el puesto. Caso contrario, retorna false
        public bool ExistePuestoEnBase(string nombrePuesto, DateTime fechaAnalisis)
        {
            // booleano para determinar si se encontr� el peusto
            bool encontrado = false;

            // consulta sql para determinar si el puesto existe
            string consulta = "SELECT * FROM PUESTO WHERE "
                + "nombre='" + nombrePuesto + "' and "
                + "fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            // m�todo heredado de la clase padre para realizar consultas de tipo SELECT.
            DataTable tablaResultadoPuestos = base.CrearTablaConsulta(consulta);
            
            // revisamos si la consutla retorno algo
            if (tablaResultadoPuestos.Rows.Count > 0 && tablaResultadoPuestos.Rows[0].IsNull("nombre") == false)
            {
                // si retorno algo, el puesto existe
                encontrado = true;
            }

            // retornamos si se encontr� el puesto o no
            return encontrado;
        }

        // M�todo que permite actualizar un puesto ya existente
        // Recibe el nombre el puesto anterior y el modelo a insertar
        public void ActualizarPuesto(string nombrePuesto, PuestoModel puestoInsertar)
        {
            // consulta sql con la cual actualizamos el puesto. Aqu� t�mbien casteamos los decimales seg�n si tiene punto o decimal
            string update = "UPDATE PUESTO SET "
                + "nombre='" + puestoInsertar.Nombre + "', "
                + "cantidadPlazas='" + puestoInsertar.Plazas.ToString() + "', "
                + "salarioBruto= " + puestoInsertar.SalarioBruto.ToString().Replace(",", ".") + ", "
                + "beneficios= " + puestoInsertar.Beneficios.ToString().Replace(",", ".")
                + " WHERE nombre='" + nombrePuesto + "' and "
                + "fechaAnalisis='" + puestoInsertar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            base.enviarConsulta(update);
        }


        // M�todo que retorna una lista con todos los puestos de un an�lisis
        // Recibe la fecha del an�lisis a consultar
        // Retorna la listas de los puestos
        public List<PuestoModel> ObtenerListaDePuestos(DateTime fechaAnalisis)
        {
            // consulta sql que llama procedure que retorna todos los puestos de una an�lisis
            string consulta = "execute ObtenerPuestos @fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            // m�todo de la clase padre para realizar consultas
            DataTable tablaResultadoPuestos = base.CrearTablaConsulta(consulta);

            // lista para almacenar los puestos
            List<PuestoModel> puestos = new List<PuestoModel>();
            // iteramos por las filas de la consulta para crear los puestos
            foreach (DataRow fila in tablaResultadoPuestos.Rows)
            {
                // ingresamos todos los datos de cada puesto
                PuestoModel puesto = new PuestoModel();
                puesto.Nombre = Convert.ToString(fila["nombre"]);
                puesto.Plazas = Convert.ToInt32(fila["cantidadPlazas"]);
                puesto.SalarioBruto = Convert.ToDecimal(fila["salarioBruto"]);
                puesto.FechaAnalisis = Convert.ToDateTime(fila["fechaAnalisis"]);
                puesto.Beneficios = Convert.ToDecimal(fila["beneficios"]);

                puestos.Add(puesto);
            }

            return puestos;
        }

        // M�todo para eliminar un puesto de un an�lisis
        public void EliminarPuesto(PuestoModel puestoELiminar)
        {
            string delete = "DELETE FROM PUESTO WHERE fechaAnalisis='" + 
                puestoELiminar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' " 
                + "and nombre='" + puestoELiminar.Nombre + "';";
            base.enviarConsulta(delete);
        }
    }
}