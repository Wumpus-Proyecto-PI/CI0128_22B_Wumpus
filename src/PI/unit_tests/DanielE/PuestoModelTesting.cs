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
    // class: clase de testing para el modelo puesto y su interaccion con la base de datos
    [TestClass]
    public class PuestoModelTesting
    {
        // handler de testing uqe permite crear un negocio de pruebas
        private NegocioTestingHandler? NegocioTestingHandler = null;

        // handler de analisis que nos permite obtener analisis del negocio
        private AnalisisHandler? AnalisisHandler = null;

        // negocio ficticio creado para la prueba
        private NegocioModel? NegocioFicticio = null;

        // analisis ficticio creado para la prueba
        private AnalisisModel? AnalisisFicticio = null;

        // en la inicializacion creamos un negocio de testing en nuestro usuario de testing y extraemos el analisis que genera
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

        // en el cleanup eliminamos el negocio que creamos
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

        // este test prueba que al insertar un nuevo puesto no se modifiquen los puestos ya existentes
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

        // este test prueba que al eliminar puesto no se modifiquen los puestos ya existentes
        [TestMethod]
        public void EliminarPuesto_NoModificaOtrosPuestos()
        {
            // arrange

            PuestoTestingHandler puestoTestingHandler = new();
            // ingresamos una lista de puestos inicial
            List<PuestoModel> puestosPreInsercion = puestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);
           
            // deseamos elminar el puesto de la posicion [1]
            PuestoModel puestoELiminar = puestosPreInsercion[1];

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // accion
            // eliminamos el [1] puesto de la lista de puestos semilla
            estructuraOrgHandler.EliminarPuesto(puestoELiminar);

            // assert
            // obtenemos los puestos de la base
            List<PuestoModel> puestosPostInsercion = puestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);
            bool FueInsertado = puestosPostInsercion.Exists(x => x.Nombre == puestoELiminar.Nombre);

            // removemos de la lista de puestos semilla el que eliminamos en la base para comparar 
            // las listas antes y despues del borrado en la base
            puestosPreInsercion.RemoveAt(1);

            bool puestoIguales = PuestoTestingHandler.SonIgualesListasPuestos(puestosPreInsercion, puestosPostInsercion);

            // si las dos listas esta igual antes y despues de la elminacion no se vio afectado otro puesto por el query
            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");

            // revisamos que no exista el puesto eliminado en la lista de puesto que leimos de la base
            Assert.IsFalse(FueInsertado, $"'{puestoELiminar}' sí se insertó en la base y no fue eliminado");
        }

        // este test prueba que al eliminar puesto no se modifiquen los puestos ya existentes
        [TestMethod]
        public void ActualizarNombre_NoModificaOtrosPuestos()
        {
            // arrange

            PuestoTestingHandler puestoTestingHandler = new();
            // ingresamos una lista de puestos inicial
            List<PuestoModel> puestosPreInsercion = puestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);

            // deseamos actualizar el nombre del puesto de la posicion [1]
            PuestoModel puestoActualizar = puestosPreInsercion[1];

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
            EstructuraOrgHandler estructuraOrgHandler = new();

            // accion
            // eliminamos el [1] puesto de la lista de puestos semilla
            estructuraOrgHandler.InsertarPuesto(puestoActualizar.Nombre, nuevoPuesto);

            // assert
            // obtenemos los puestos de la base
            List<PuestoModel> puestosPostInsercion = puestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);
            bool ExisteViejoNombre = puestosPostInsercion.Exists(x => x.Nombre == puestoActualizar.Nombre);
            
            PuestoModel puestoActualizadoEnBase = puestosPostInsercion.Find(x => x.Nombre == nuevoPuesto.Nombre);
            bool puestoActualizado = false;

            if (puestoActualizadoEnBase != null 
                && puestoActualizadoEnBase.Nombre == nuevoPuesto.Nombre 
                && puestoActualizadoEnBase.Plazas == nuevoPuesto.Plazas
                && puestoActualizadoEnBase.Beneficios == nuevoPuesto.Beneficios
                && puestoActualizadoEnBase.SalarioBruto == nuevoPuesto.SalarioBruto
                && puestoActualizadoEnBase.FechaAnalisis == nuevoPuesto.FechaAnalisis)
            {
                puestoActualizado = true;
            }
            // removemos de la lista de puestos semilla el que eliminamos en la base para comparar 
            // las listas antes y despues del borrado en la base
            puestosPreInsercion.RemoveAt(1);
            puestosPreInsercion.Insert(1, nuevoPuesto);

            bool puestoIguales = PuestoTestingHandler.SonIgualesListasPuestos(puestosPreInsercion, puestosPostInsercion);

            // si las dos listas esta igual antes y despues de la elminacion no se vio afectado otro puesto por el query
            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");

            // revisamos que no exista el puesto antes de ser actualizado en la lista de puesto que leimos de la base
            Assert.IsFalse(ExisteViejoNombre, $"No se modificó el nombre anterior '{puestoActualizar}'");

            // revisamos que si exista el nuevo nombre del puesot que actualizamos en la lista de puesto que leimos de la base
            Assert.IsTrue(puestoActualizado, $"Sí se modificó el nombre y se asignó el nombre '{nuevoPuesto}'");
        }
    }
}
