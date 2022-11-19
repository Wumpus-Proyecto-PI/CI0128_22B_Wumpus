using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI;
using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.DanielE
{
    [TestClass]
    public class PuestoModelTesting
    {
        private NegocioTestingHandler? NegocioTestingHandler = null;

        private AnalisisHandler? AnalisisHandler = null;

        private NegocioModel? NegocioFicticio = null;

        private AnalisisModel? AnalisisFicticio = null;

        private HandlerGenerico HandlerGenerico = new();

        [TestInitialize]
        public void Setup()
        {

            // creamos un negocio ficticio al usuario de testing de wumpus
            // tambien le creamos un analisis vacio para realizar las pruebas
            NegocioTestingHandler = new();
            AnalisisHandler = new();

            // para que el test exista debe existir el siguiente usuario en la base
            // usuario: wumpustest@gmail.com 
            // id del usuario: e690ef97-31c4-4064-bede-93aeedaf6857
            NegocioFicticio = NegocioTestingHandler.IngresarNegocioFicticio(TestingUserModel.UserId, "Emprendimiento");
            AnalisisFicticio = AnalisisHandler.ObtenerAnalisisMasReciente(NegocioFicticio.ID);
        }

        [TestCleanup]
        public void CleanUp()
        {
            // eliminar el negocio elimina todos lso datos relacionados a el
            NegocioTestingHandler.EliminarNegocioFicticio();
            NegocioTestingHandler = null;
            NegocioFicticio = null;
            AnalisisHandler = null;
            AnalisisFicticio = null;
        }

        [TestMethod]
        public void InsertarPuesto_NoModificaOtrosPuestos()
        {
            // arrange

            PuestoTestingHandler puestoTestingHandler = new();
            // ingresamos una lista de puestos inicial
            List<PuestoModel> puestosPreInsercion = puestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);

            // creamos puesto que vamos a insertar de prueba
            PuestoModel nuevoPuesto = new PuestoModel
            {
                Nombre = "Nuevo Puesto",
                Plazas = 48,
                Beneficios = 4567.89m,
                SalarioBruto = 7894.45m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler= new();

            // accion
            estructuraOrgHandler.InsertarPuesto("", nuevoPuesto);

            // assert
            // obtenemos los puestos de la base
            List<PuestoModel> puestosPostInsercion = puestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);
            bool FueInsertado = puestosPostInsercion.Exists(x => x.Nombre == nuevoPuesto.Nombre);

            // removemos de la lista el que insertamos para comparar si el resto de puestos se vieron afectados
            puestosPostInsercion.RemoveAll(x => x.Nombre == nuevoPuesto.Nombre);

            bool puestoIguales = PuestoTestingHandler.SonIgualesListasPuestos(puestosPreInsercion, puestosPostInsercion);

            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");
            Assert.IsTrue(FueInsertado, "'Nuevo puesto' no se insertó en la base");
        }
    }
}
