using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI;
using PI.Handlers;
using PI.Models;
using UnitTestsResources;

namespace unit_tests.DanielE
{
    [TestClass]
    public class PuestoModelTesting
    {
        private NegocioHandler? NegocioHandler = null;

        private AnalisisHandler? AnalisisHandler = null;

        private NegocioModel? NegocioFicticio = null;

        private AnalisisModel? AnalisisFicticio = null;

        private readonly string TestingUserId = "e690ef97-31c4-4064-bede-93aeedaf6857";

        private HandlerGenerico handlerGenerico = new();

        [TestInitialize]
        public void Setup()
        {

            // creamos un negocio ficticio al usuario de testing de wumpus
            // tambien le creamos un analisis vacio para realizar las pruebas
            NegocioHandler = new();
            AnalisisHandler = new();

            // para que el test exista debe existir el siguiente usuario en la base
            // usuario: wumpustest@gmail.com 
            // id del usuario: e690ef97-31c4-4064-bede-93aeedaf6857
            NegocioFicticio = NegocioHandler.IngresarNegocio("Negocio Ficticio", "Emprendimiento", TestingUserId);
            AnalisisFicticio = AnalisisHandler.ObtenerAnalisisMasReciente(NegocioFicticio.ID);
        }

        [TestCleanup]
        public void CleanUp()
        {
            // eliminar el negocio elimina todos lso datos relacionados a el
            NegocioHandler.EliminarNegocio(NegocioFicticio.ID.ToString());
            NegocioFicticio = null;
            AnalisisHandler = null;
            NegocioHandler = null;
            AnalisisFicticio = null;
        }

        public List<PuestoModel> InsertarListaPuestosSemillaEnBase()
        {
            List<PuestoModel> puestosSemilla = new List<PuestoModel>();
            puestosSemilla.Add(new PuestoModel
            {
                Nombre = "Jefe",
                Plazas = 1,
                SalarioBruto = 4400.56m,
                Beneficios = 1500.89m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            });
            puestosSemilla.Add(new PuestoModel
            {
                Nombre = "Empacador",
                Plazas = 25,
                SalarioBruto = 3698.56m,
                Beneficios = 45687.78m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            });
            puestosSemilla.Add(new PuestoModel
            {
                Nombre = "Cajero",
                Plazas = 8,
                SalarioBruto = 4567.56m,
                Beneficios = 4578.08m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            });

            string insert = "";
            foreach (var puesto in puestosSemilla)
            {
                insert = "INSERT INTO PUESTO values ("
                + "'" + puesto.Nombre + "', "
                + "'" + puesto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', "
                + puesto.Plazas.ToString() + ", "
                + puesto.SalarioBruto + ", "
                + puesto.Beneficios + ")";

                handlerGenerico.EnviarConsultaGenerica(insert);
            }

            return puestosSemilla;
        }

        public List<PuestoModel> LeerPuestosDeBase()
        {
            List<PuestoModel> puestoEnBase = new List<PuestoModel>();

            DataTable resultadoConsulta = handlerGenerico.CrearTablaConsultaGenerico("SELECT * FROM PUESTO WHERE fechaAnalisis=" +
                AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff") + " order by orden ASC");
            // iteramos por las filas de la consulta para crear los puestos
            foreach (DataRow fila in resultadoConsulta.Rows)
            {
                // ingresamos todos los datos de cada puesto
                PuestoModel puesto = new PuestoModel();
                Console.WriteLine(puesto.Nombre);
                puesto.Nombre = Convert.ToString(fila["nombre"]);
                puesto.Plazas = Convert.ToInt32(fila["cantidadPlazas"]);
                puesto.SalarioBruto = Convert.ToDecimal(fila["salarioBruto"]);
                puesto.FechaAnalisis = (DateTime)fila["fechaAnalisis"];
                puesto.Beneficios = Convert.ToDecimal(fila["beneficios"]);

                // los beneficios se cargan con otro método que carga beneficios según el puesto y el análisis
                puestoEnBase.Add(puesto);
            }
            return puestoEnBase;
        }

        [TestMethod]
        public void InsertarPuesto_NoModificaOtrosPuestos()
        {
            // arrange
            // ingresamos una lista de puestos inicial
            InsertarListaPuestosSemillaEnBase();

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler= new();

            

            // accion


            // assert
        }
    }
}
