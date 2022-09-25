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
    public class EstructuraOrgHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public EstructuraOrgHandler()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("BaseDeDatos");
            conexion = new SqlConnection(rutaConexion);
        }

        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta,
            conexion);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }

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

        public List<PuestoModel> ObtenerPuestos(DateOnly fechaAnalisis)
        {
            // extraer los puestos
            // nombrePuesto es la llave primaria de puesto
            string fechaAnalisisStr = fechaAnalisis.ToString("yyyy-mm-dd");
            string consulta = "SELECT * FROM PUESTO WHERE fechaAnalisis='" + fechaAnalisisStr + "'";
            // puestos y sus beneficios
            List<PuestoModel> puestos = new List<PuestoModel>();
            DataTable tablaResultadoPuestos = CrearTablaConsulta(consulta);


            foreach (DataRow columna in tablaResultadoPuestos.Rows)
            {
                PuestoModel puesActual = new PuestoModel{
                    Nombre = Convert.ToString(columna["nombre"]),
                    Plazas = Convert.ToInt16(columna["plazasPorPuesto"]),
                    SalarioBruto = Convert.ToDecimal(columna["salarioBruto"]),
                    Beneficios = new List<BeneficioModel>()
                };

                // se agregan los beneficios del puesto
                puesActual.Beneficios = ObtenerBeneficios(puesActual.Nombre, fechaAnalisisStr);

                // se agregan los subordinados del puesto
                puesActual.Subordinados = ObtenerSubordinados(puesActual.Nombre, fechaAnalisisStr, puestos);

                puestos.Add(puesActual);
            }
            return puestos;
        }

        private List<BeneficioModel> ObtenerBeneficios(string nombrePuesto, string fechaAnalisis)
        {
            List < BeneficioModel > resultadoBeneficios = new List<BeneficioModel>();

            // consulta para extraer los beneficios
            string consulta = "SELECT * FROM BENEFICIO_PUESTO WHERE " 
                + "nombrePuesto='" + nombrePuesto + "' and" 
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

        private List<PuestoModel> ObtenerSubordinados(string nombrePuestoJefe, string fechaAnalisis, List<PuestoModel> puestos)
        {
            List < PuestoModel > resultadoSubordinados = new List<PuestoModel>();

            // extraer los puesto subordinados
            string consulta = "SELECT puestoEmpleado FROM ES_EMPLEADO_DE WHERE " 
                + "puestoJefe='" + nombrePuestoJefe + "' and" 
                + "fechaAnalisis='" + fechaAnalisis + "'";

            DataTable tablaSubordinados = CrearTablaConsulta(consulta);

            return resultadoBeneficios;
        }
    }
}