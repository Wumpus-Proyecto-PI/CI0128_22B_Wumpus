using System;
using System.Collections.Generic;
using PI.Models;
using PI.Handlers;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace PI.Handlers
{
    // Clase handler que se encarga de manipular información de la estructura organizativa en la base de datos 
    // esto incluye puestos y beneficios, actualizar, insertar y eliminar
    // Esta clase hereda de Handler, clase que contiene el string de conexión y métodos de consulta
    public class EstructuraOrgHandler : Handler
    {
        // constructor llama al construtor del padre que contiene el string de conexión
        public EstructuraOrgHandler() : base() { }

        // Método que permite ingresar un puesto nuevo a la base de datos
        // Recibe el string nombrePuesto a ingresar y un modelPuesto.
        // EL string nombrePuesto es el nombre antes de modificaciones. Esto se usa para actualizaciones.
        // El modelo es para acceder a todos los datos de tal puesto
        // Este método retorna la cantidad de filas actualizadas
        public int InsertarPuesto(string nombrePuesto, PuestoModel puesotAInsertar)
        {
            // cuenta las filas afectadas en la base de datos
            int filasAfectadas = 0;

            // primero se revisa si ya existe el puesto en la base
            if (existePuestoEnBase(nombrePuesto, puesotAInsertar.FechaAnalisis))
            {
                // si existe el peusto lo actualizamos
                ActualizarPuesto(nombrePuesto, puesotAInsertar);
            } else
            {
                // en caso de no existir lo insertamos como un nuevo puesto
                // en estaconsulta de sql convertimos los decimales según si es con coma o punto para evitar errores
                string insert = "DECLARE @salarioTemp varchar(20) SET @salarioTemp = '"+ puesotAInsertar.SalarioBruto.ToString() 
                + "' SET @salarioTemp = REPLACE(@salarioTemp, ',', '.') INSERT INTO PUESTO values ("
                + "'" + puesotAInsertar.Nombre + "', "
                + "'" + puesotAInsertar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', "
                + puesotAInsertar.Plazas.ToString() + ", "
                + "dbo.convertTOdecimal ( @salarioTemp))";

                // realizamos la consulta
                // este método es heredado del padre y permite enviar consultas de actualización, borrado e inserción   
                filasAfectadas = enviarConsulta(insert);
            }
            return filasAfectadas;
        }

        // Método que permite revisar si ya existe un puesto en la base
        // Recibe el nombre el puesto y la fecha del análisis dodne buscarlo
        // Retorna verdadero si existe el puesto. Caso contrario, retorna false
        public bool existePuestoEnBase(string nombrePuesto, DateTime fechaAnalisis)
        {
            // booleano para determinar si se encontró el peusto
            bool encontrado = false;
            // consulta sql para determinar si el peusto existe
            string consulta = "SELECT * FROM PUESTO WHERE "
                + "nombre='" + nombrePuesto + "' and "
                + "fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            // método heredado de la clase padre para realizar consultas de tipo SELECT.
            DataTable tablaResultadoPuestos = CrearTablaConsulta(consulta);
            
            // revisamos si la consutla retorno algo
            if (tablaResultadoPuestos.Rows.Count > 0 && tablaResultadoPuestos.Rows[0].IsNull("nombre") == false)
            {
                // si retorno algo, el puesto existe
                encontrado = true;
            }

            // retornamos si se encontró el puesto o no
            return encontrado;
        }

        // Método que permite actualizar un puesto ya existente
        // Recibe el nombre el puesto anterior y el modelo a insertar
        // Retorna true si se se dio un error en la consulta 
        public bool ActualizarPuesto(string nombrePuesto, PuestoModel puestoInsertar)
        {
            bool error = false;

            // consulta sql con la cual actualizamos el puesto. Aquí támbien casteamos los decimales según si tiene punto o decimal
            string update = "DECLARE @salarioTemp varchar(20) SET @salarioTemp = '"+ puestoInsertar.SalarioBruto.ToString() 
                + "' SET @salarioTemp = REPLACE(@salarioTemp, ',', '.') UPDATE PUESTO SET "
                + "nombre='" + puestoInsertar.Nombre + "', "
                + "cantidadPlazas='" + puestoInsertar.Plazas.ToString() + "', "
                + "salarioBruto= dbo.convertTOdecimal ( @salarioTemp)"
                + "WHERE "
                + "nombre='" + nombrePuesto + "' and "
                + "fechaAnalisis='" + puestoInsertar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "';";
            int filasAfectadas = enviarConsulta(update);
            if (filasAfectadas < 0)
            {
                error = true;
            }
            return error;
        }


        // Método que retorna una lista con todos los puestos de un análisis
        // Recibe la fecha del análisis a consultar
        // Retorna la listas de los puestos
        public List<PuestoModel> ObtenerListaDePuestos(DateTime fechaAnalisis)
        {
            // consulta sql que llama procedure que retorna todos los puestos de una análisis
            string consulta = "execute ObtenerPuestos @fechaAnalisis = '" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            // método de la clase padre para realizar consultas
            DataTable tablaResultadoPuestos = CrearTablaConsulta(consulta);

            // lista para almacenar los puestos
            List<PuestoModel> puestos = new List<PuestoModel>();
            // iteramos por las filas de la consulta para crear los puestos
            foreach (DataRow fila in tablaResultadoPuestos.Rows)
            {
                // ingresamos todos los datos de cada puesto
                PuestoModel puesto = new PuestoModel();
                Console.WriteLine(puesto.Nombre);
                puesto.Nombre = Convert.ToString(fila["nombre"]);
                puesto.Plazas = Convert.ToInt32(fila["cantidadPlazas"]);
                puesto.SalarioBruto = Convert.ToDecimal(fila["salarioBruto"]);
                puesto.FechaAnalisis = (DateTime)fila["fechaAnalisis"];

                // los beneficios se cargan con otro método que carga beneficios según el puesto y el análisis
                puesto.Beneficios = ObtenerBeneficios(puesto.Nombre, fechaAnalisis);
                puestos.Add(puesto);
            }

            return puestos;
        }

        // Método para eliminar un puesto de un análisis
        public int EliminarPuesto(PuestoModel puestoELiminar)
        {
            string delete = "DELETE FROM PUESTO WHERE fechaAnalisis='" + 
                puestoELiminar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' " 
                + "and nombre='" + puestoELiminar.Nombre + "';";
            return enviarConsulta(delete);
        }

        // Método encargado de obtener de la base de datos, los beneficios correspondientes a un puesto.
        // Tomando como parámetros tanto el nombre como la fecha de análisis de dicho puesto.
        private List<BeneficioModel> ObtenerBeneficios(string nombrePuesto, DateTime fechaAnalisis)
        {
            List < BeneficioModel > resultadoBeneficios = new List<BeneficioModel>();

            // consulta para extraer los beneficios
            string consulta = "SELECT nombre, monto, cantidadPlazas FROM BENEFICIO WHERE " 
                + "nombrePuesto='" + nombrePuesto + "' and " 
                + "fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultadoBeneficios = CrearTablaConsulta(consulta);

            foreach (DataRow beneficio in tablaResultadoBeneficios.Rows)
            {
                resultadoBeneficios.Add(new BeneficioModel
                {
                    nombreBeneficio = Convert.ToString(beneficio["nombre"]),
                    monto = Convert.ToDecimal(beneficio["monto"]),
                    plazasPorBeneficio = Convert.ToInt16(beneficio["cantidadPlazas"])
                });
            }

            return resultadoBeneficios;
        }

        // Método encargado de agregar un beneficio a la base de datos. El modelo del Beneficio a agregar es pasado como parámetro.
        public List<BeneficioModel> AgregarBeneficio(BeneficioModel b)
        {
            List<BeneficioModel> resultadoBeneficios = new List<BeneficioModel>();

            // consulta para extraer los beneficios y devolver la tabla de beneficios con dicho beneficio ya agregado
            string consulta = "DECLARE @montoTemp varchar(20) SET @montoTemp = '"+ b.monto.ToString() 
                + "' SET @montoTemp = REPLACE(@montoTemp, ',', '.') INSERT INTO BENEFICIO VALUES('" + b.nombrePuesto + "','" 
                + b.fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "','" 
                + b.nombreBeneficio + "', dbo.convertTOdecimal (@montoTemp),"
                + b.plazasPorBeneficio.ToString() + ") SELECT * FROM BENEFICIO";
            DataTable tablaResultadoBeneficios = CrearTablaConsulta(consulta);

            foreach (DataRow beneficio in tablaResultadoBeneficios.Rows)
            {
                resultadoBeneficios.Add(new BeneficioModel
                {
                    nombreBeneficio = Convert.ToString(beneficio["nombre"]),
                    monto = Convert.ToDecimal(beneficio["monto"]),
                    plazasPorBeneficio = Convert.ToInt16(beneficio["cantidadPlazas"]),
                    nombrePuesto = Convert.ToString(beneficio["nombrePuesto"]),
                    fechaAnalisis = Convert.ToDateTime(beneficio["fechaAnalisis"])
                });
            }

            return resultadoBeneficios;
        }

        // Método encargado de eliminar un beneficio de la base de datos, a partir de un Modelo de Beneficio, el cual es pasado por parámetro.
        public List<BeneficioModel> BorrarBeneficio(BeneficioModel b)
        {
            List<BeneficioModel> resultadoBeneficios = new List<BeneficioModel>();

            Console.WriteLine(b.fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            // consulta para extraer los beneficios
            string consulta = "DECLARE @montoTemp varchar(20) SET @montoTemp = '"+ b.monto.ToString() 
                + "' SET @montoTemp = REPLACE(@montoTemp, ',', '.') DELETE FROM BENEFICIO WHERE nombre ='" + b.nombreBeneficio + "' and fechaAnalisis ='" + b.fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and nombrePuesto ='" + b.nombrePuesto + "' and monto = dbo.convertTOdecimal(@montoTemp) and cantidadPlazas=" + b.plazasPorBeneficio.ToString();

            //string consulta = "DELETE FROM BENEFICIO WHERE nombre ='" + b.nombreBeneficio + "' and fechaAnalisis ='" + b.fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and nombrePuesto ='" + b.nombrePuesto + "' and monto =" + b.monto.ToString() + "and cantidadPlazas=" + b.plazasPorBeneficio.ToString();
            DataTable tablaResultadoBeneficios = CrearTablaConsulta(consulta);

            foreach (DataRow beneficio in tablaResultadoBeneficios.Rows)
            {
                resultadoBeneficios.Add(new BeneficioModel
                {
                    nombreBeneficio = Convert.ToString(beneficio["nombre"]),
                    monto = Convert.ToDecimal(beneficio["monto"]),
                    plazasPorBeneficio = Convert.ToInt16(beneficio["cantidadPlazas"]),
                    nombrePuesto = Convert.ToString(beneficio["nombrePuesto"]),
                    fechaAnalisis = Convert.ToDateTime(beneficio["fechaAnalisis"])
                });
            }

            return resultadoBeneficios;
        }


        /* 
         * Importante!
         * Métodos para obtener los puestos con estructura no son utilizados en el primer sprint 
         * Los métodos siguientes cargan los puestos tomando encuenta la jerarquía
         */

        /*
         * Estos métodos se deben revisar y mejorar para sigueintes sprints
         */

        // Método que obtiene los puestos de un análisis tomando en cuenta la jerarquía
        // Recibe la fecha del análisis de donde extraer los puestos
        // Retorna el puesto raíz que contiene el restos de los peustos como subordinados
        public PuestoModel ObtenerEstructuraOrg(DateOnly fechaAnalisis)
        {
            // extraer los puestos
            // nombrePuesto es la llave primaria de puesto
            string fechaAnalisisStr = fechaAnalisis.ToString("yyyy-mm-dd");
            string consulta = "SELECT TOP 1 nombre FROM PUESTO WHERE fechaAnalisis='" + fechaAnalisisStr + "'";
            DataTable tablaResultadoPuestos = CrearTablaConsulta(consulta);
            var filas = tablaResultadoPuestos.Rows;
            // puesto raiz que contiene los otros puestos como subordinados
            // ObtenerUnPuesto es metodo recursivo que va a agregar todos los subordinados de la estructura
            PuestoModel raizEstructura = ObtenerUnPuesto(Convert.ToString(filas[0]["nombre"]), fechaAnalisisStr);

            return raizEstructura;
        }

        // Método que retorna un solo puestos y sus subordinados de forma recursiva
        // Recibe el nombre del puesto, la fecha del análisis y un booleano que indica si se desea cargar los subordinados del puesto
        // Retorna un puesto con sus subordinados si se indica
        public PuestoModel ObtenerUnPuesto(string nombrePuesto, string fechaAnalisis, bool extraerSubordinados = true)
        {
            // extraeamos la lista de puestos 
            string consulta = "SELECT TOP 1 * FROM PUESTO WHERE"
                + "'fechaAnalisis='" + fechaAnalisis + "' and "
                + "nombre='" + nombrePuesto + "'";
            DataTable tablaResultadoPuestos = CrearTablaConsulta(consulta);
            var filas = tablaResultadoPuestos.Rows;

            if (tablaResultadoPuestos == null && tablaResultadoPuestos.Rows.Count <= 0)
            {
                return null;
            }

            PuestoModel puestoActual = new PuestoModel
            {
                Nombre = nombrePuesto,
                Plazas = Convert.ToInt16(filas[0]["plazasPorPuesto"]),
                SalarioBruto = Convert.ToDecimal(filas[0]["salarioBruto"]),
            };
            // puestoActual.Beneficios = ObtenerBeneficios(puestoActual.Nombre, fechaAnalisis);

            // si se indica que desean extraer tambien los subordinados
            // si esta en falso se evita un query muy grande que puede tomar tiempo
            if (extraerSubordinados)
            {
                // extraer los puesto subordinados
                consulta = "SELECT * FROM ES_EMPLEADO_DE as E join Puesto as P on "
                    + "E.nombreEmpleado = P.nombre"
                    + "WHERE E.nombreJefe='" + puestoActual.Nombre + "'"
                    + "AND P.fechaAnalisis='" + fechaAnalisis + "'"
                    + "AND E.fechaEmpleado='" + fechaAnalisis + "'"
                    + "AND E.fechaJefe='" + fechaAnalisis + "'";

                DataTable tablaSubordinados = CrearTablaConsulta(consulta);
                if (tablaSubordinados != null && tablaSubordinados.Rows.Count > 0)
                {
                    foreach (DataRow fila in tablaSubordinados.Rows)
                    {
                        PuestoModel subordinado = ObtenerUnPuesto(Convert.ToString(fila["nombre"]), fechaAnalisis, true);

                        if (subordinado != null)
                        {
                            puestoActual.Subordinados.Add(subordinado);
                        }
                    }
                }
                else
                {
                    return null;
                }
            }

            return puestoActual;
        }

    }
}