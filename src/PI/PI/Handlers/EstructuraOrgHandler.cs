using System;
using System.Collections.Generic;
using PI.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace PI.Handlers
{
    public class EstructuraOrgHandler : Handler
    {
        public EstructuraOrgHandler() : base() { }

        public List<NegocioModel> ObtenerNegocios()
        {
            List<NegocioModel> negocios = new List<NegocioModel>();
            string consulta = "SELECT * FROM NEGOCIO";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                negocios.Add(
                new NegocioModel
                {
                    id = Convert.ToInt32(columna["id"]),
                    nombre = Convert.ToString(columna["nombre"]),
                    correoUsuario = Convert.ToString(columna["correoUsuario"]),
                });
            }
            return negocios;
        }

        public int InsertarPuesto(string nombrePuesto, PuestoModel puesotAInsertar)
        {
            int filasAfectadas = 0;
            if (existePuestoEnBase(nombrePuesto, puesotAInsertar.FechaAnalisis))
            {
                ActualizarPuesto(nombrePuesto, puesotAInsertar);
            } else
            {
                string insert = "INSERT INTO PUESTO values ("
                + "'" + puesotAInsertar.Nombre + "', "
                + "'" + puesotAInsertar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', "
                + puesotAInsertar.Plazas.ToString() + ", "
                + "'" + puesotAInsertar.SalarioBruto.ToString() + "'"
                + ")";
                filasAfectadas = enviarConsulta(insert);
            }
            return filasAfectadas;
        }

        public bool existePuestoEnBase(string nombrePuesto, DateTime fechaAnalisis)
        {
            bool encontrado = false;
            string consulta = "SELECT * FROM PUESTO WHERE "
                + "nombre='" + nombrePuesto + "' and "
                + "fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultadoPuestos = CrearTablaConsulta(consulta);
            if (tablaResultadoPuestos.Rows.Count > 0 && tablaResultadoPuestos.Rows[0].IsNull("nombre") == false)
            {
                encontrado = true;
            }
            return encontrado;
        }

        public bool ActualizarPuesto(string nombrePuesto, PuestoModel puestoInsertar)
        {
            bool error = false;
            string update = "UPDATE PUESTO SET "
                + "nombre='" + puestoInsertar.Nombre + "', "
                + "cantidadPlazas='" + puestoInsertar.Plazas.ToString() + "', "
                + "salarioBruto='" + puestoInsertar.SalarioBruto.ToString() + "' "
                + "WHERE "
                + "nombre='" + nombrePuesto + "' and "
                + "fechaAnalisis='" + puestoInsertar.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "';";
            int filasAfectadas = enviarConsulta(update);
            return error;
        }

        public List<PuestoModel> ObtenerListaDePuestos(DateTime fechaAnalisis)
        {
            string consulta = "SELECT * FROM PUESTO WHERE fechaAnalisis='" + fechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable tablaResultadoPuestos = CrearTablaConsulta(consulta);

            List<PuestoModel> puestos = new List<PuestoModel>();
            foreach (DataRow fila in tablaResultadoPuestos.Rows)
            {
                PuestoModel puesto = new PuestoModel();
                Console.WriteLine(puesto.Nombre);
                puesto.Nombre = Convert.ToString(fila["nombre"]);
                puesto.Plazas = Convert.ToInt16(fila["cantidadPlazas"]);
                puesto.SalarioBruto = Convert.ToDecimal(fila["salarioBruto"]);
                puesto.FechaAnalisis = (DateTime)fila["fechaAnalisis"];
                //puesto.Beneficios = ObtenerBeneficios(puesto.Nombre, fechaAnalisisStr);
                puestos.Add(puesto);
            }

            return puestos;
        }

        private List<BeneficioModel> ObtenerBeneficios(string nombrePuesto, string fechaAnalisis)
        {
            List < BeneficioModel > resultadoBeneficios = new List<BeneficioModel>();

            // consulta para extraer los beneficios
            string consulta = "SELECT * FROM BENEFICIO WHERE " 
                + "nombrePuesto='" + nombrePuesto + "' and " 
                + "fechaAnalisis='" + fechaAnalisis + "'";
            DataTable tablaResultadoBeneficios = CrearTablaConsulta(consulta);

            foreach (DataRow beneficio in tablaResultadoBeneficios.Rows)
            {
                resultadoBeneficios.Add(new BeneficioModel
                {
                    nombreBeneficio = Convert.ToString(beneficio["nombreBeneficio"]),
                    monto = Convert.ToDecimal(beneficio["monto"]),
                    plazasPorBeneficio = Convert.ToInt16(beneficio["PlazaPorBeneficio"])
                });
            }

            return resultadoBeneficios;
        }


        /* metodos para obtener los puestos con estructura. No son utilizados en el primer sprint */
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

        public PuestoModel ObtenerUnPuesto (string nombrePuesto, string fechaAnalisis, bool extraerSubordinados = true)
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

            PuestoModel puestoActual = new PuestoModel{
                Nombre = nombrePuesto,
                Plazas = Convert.ToInt16(filas[0]["plazasPorPuesto"]),
                SalarioBruto = Convert.ToDecimal(filas[0]["salarioBruto"]),
            };
            puestoActual.Beneficios = ObtenerBeneficios(puestoActual.Nombre, fechaAnalisis);
            
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
                } else {
                    return null;
                }
            }

            return puestoActual;
        }

    }
}