using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

        // handler de testing usado para manejar la creacion de puestos semilla
        PuestoTestingHandler? PuestoTestingHandler = null;

        // en la inicializacion creamos un negocio de testing en nuestro usuario de testing y extraemos el analisis que genera
        [TestInitialize]
        public void Setup()
        {

            // creamos un negocio ficticio al usuario de testing de wumpus
            NegocioTestingHandler = new();

            // tambien le creamos un analisis vacio para realizar las pruebas
            AnalisisHandler = new();

            // creamos instancia de handler de testing usado para manejar la creacion de puestos semilla
            PuestoTestingHandler = new();

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
            PuestoTestingHandler = null;
        }

        // brief: metodo que compara si dos listas de puesto model son iguales
        // details: se compara cada uno de los atributos del puesto model
        // return: true si las dos listas son iguales y false en caso contrario
        static public bool SonIgualesListasPuestos(List<PuestoModel> esperada, List<PuestoModel> actual)
        {
            esperada = esperada.OrderBy(x => x.Nombre).ToList();
            actual = actual.OrderBy(x => x.Nombre).ToList();

            bool listasIguales = true;
            if (esperada.Count != actual.Count)
            {
                listasIguales = false;
            }
            else
            {
                for (int i = 0; i < esperada.Count && listasIguales == true; ++i)
                {
                    listasIguales = SonIgualesPuestos(esperada[i], actual[i]);
                }
            }
            return listasIguales;
        }

        // brief: metodo que compara si dos puestos son iguales
        // details: se compara cada uno de los atributos del puesto model
        // return: true si los dos puestos son iguales y false en caso contrario
        static public bool SonIgualesPuestos(PuestoModel esperada, PuestoModel actual)
        {
            bool puestosIguales = false;

            if (esperada.Nombre == actual.Nombre
                && esperada.Plazas == actual.Plazas
                && esperada.SalarioBruto == actual.SalarioBruto
                && esperada.Beneficios == actual.Beneficios
                && esperada.FechaAnalisis == actual.FechaAnalisis
                )
            {
                puestosIguales = true;
            }

            return puestosIguales;
        }

        // este test prueba que al insertar un nuevo puesto no se modifiquen los puestos ya existentes
        [TestMethod]
        public void InsertarPuesto_NoModificaOtrosPuestos()
        {
            // arrange

            // ingresamos una lista de puestos inicial
            List<PuestoModel> puestosPreInsercion = PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);

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
            List<PuestoModel> puestosPostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);
            bool FueInsertado = puestosPostInsercion.Exists(x => x.Nombre == nuevoPuesto.Nombre);

            // removemos de la lista el que insertamos para comparar si el resto de puestos se vieron afectados
            puestosPostInsercion.RemoveAll(x => x.Nombre == nuevoPuesto.Nombre);

            bool puestoIguales = SonIgualesListasPuestos(puestosPreInsercion, puestosPostInsercion);

            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");
            Assert.IsTrue(FueInsertado, "'Nuevo puesto' no se insertó en la base");
        }

        // este test prueba que al eliminar puesto no se modifiquen los puestos ya existentes
        [TestMethod]
        public void EliminarPuesto_NoModificaOtrosPuestos()
        {
            // arrange

            // ingresamos una lista de puestos inicial
            List<PuestoModel> puestosPreInsercion = PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);
           
            // deseamos elminar el puesto de la posicion [1]
            PuestoModel puestoELiminar = puestosPreInsercion[1];

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // accion
            // eliminamos el [1] puesto de la lista de puestos semilla
            estructuraOrgHandler.EliminarPuesto(puestoELiminar);

            // assert
            // obtenemos los puestos de la base
            List<PuestoModel> puestosPostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);
            bool FueInsertado = puestosPostInsercion.Exists(x => x.Nombre == puestoELiminar.Nombre);

            // removemos de la lista de puestos semilla el que eliminamos en la base para comparar 
            // las listas antes y despues del borrado en la base
            puestosPreInsercion.RemoveAt(1);

            bool puestoIguales = SonIgualesListasPuestos(puestosPreInsercion, puestosPostInsercion);

            // si las dos listas esta igual antes y despues de la elminacion no se vio afectado otro puesto por el query
            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");

            // revisamos que no exista el puesto eliminado en la lista de puesto que leimos de la base
            Assert.IsFalse(FueInsertado, $"'{puestoELiminar}' sí se insertó en la base y no fue eliminado");
        }

        // este test prueba que al actualizar el nombre de un puesto en la base se modifique el correspondiente puesto
        [TestMethod]
        public void ActualizarNombre_NoModificaOtrosPuestos()
        {
            // arrange

            // ingresamos una lista de puestos inicial
            List<PuestoModel> puestosPreInsercion = PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);

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
            List<PuestoModel> puestosPostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);
            bool ExisteViejoNombre = puestosPostInsercion.Exists(x => x.Nombre == puestoActualizar.Nombre);
            
            PuestoModel? puestoActualizadoEnBase = puestosPostInsercion.Find(x => x.Nombre == nuevoPuesto.Nombre);
            bool puestoActualizado = false;

            if (puestoActualizadoEnBase != null)
            {
                puestoActualizado = SonIgualesPuestos(puestoActualizadoEnBase, nuevoPuesto); ;
            }
            // removemos de la lista de puestos semilla el que eliminamos en la base para comparar 
            // las listas antes y despues del borrado en la base
            puestosPreInsercion.RemoveAt(1);
            puestosPreInsercion.Insert(1, nuevoPuesto);

            bool puestoIguales = SonIgualesListasPuestos(puestosPreInsercion, puestosPostInsercion);

            // si las dos listas esta igual antes y despues de la elminacion no se vio afectado otro puesto por el query
            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");

            // revisamos que no exista el puesto antes de ser actualizado en la lista de puesto que leimos de la base
            Assert.IsFalse(ExisteViejoNombre, $"No se modificó el nombre anterior '{puestoActualizar.Nombre}'");

            // revisamos que si exista el nuevo nombre del puesot que actualizamos en la lista de puesto que leimos de la base
            Assert.IsTrue(puestoActualizado, $"Sí se modificó el nombre {puestoActualizar.Nombre} y se asignó el nombre '{nuevoPuesto.Nombre}'");
        }

        // este test prueba que se puede ingresar un puesto con un salario que contiene decimales
        // details: esta prueba la hacemos porque la insercion de valores decimales nos ha dado varios proeblemas en sql
        // esto porque sql soo acepat deimales con el punto como separador y en las computadoras en español el separador es una coma
        // al ser coma el separador sql tira excpecion porque considera que es un parametro aparte por la coma que separar los decimales en lugar de un numero con decimales
        [TestMethod]
        public void IngresarPuestoConSalarioConDecimales_NoTiraExcepcionAlIngresarEnBase()
        {
            // arrange

            // creamos un puesto que tiene valores decimales en el salario bruto
            PuestoModel puestoConSalarioDecimal = new (){
                Nombre = "Salario decimales",
                Plazas = 45,
                SalarioBruto = 15440.89m,
                Beneficios = 0m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // accion
            // assert
            // la accion y el assert se realizan dentro del siguiente metodo
            IngresarPuestoNoTiraExpeciones(puestoConSalarioDecimal);

            // leemos los puestos. En este caso deberia haber solo uno
            List<PuestoModel> puestosBasePostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);

            // revisamos que solo se haya ingresado solo un puesto en la base
            Assert.AreEqual(1, puestosBasePostInsercion.Count, "En la base se encontró más de un puesto cuando solo se ingresó uno");
        }

        // este test prueba que se puede ingresar un puesto con un valor de beneficios que contiene decimales
        // details: ver @details de la prueba IngresarPuestoConSalarioConDecimales_NoTiraExcepcion porque en se de la misma situacion con los beneficios
        [TestMethod]
        public void IngresarPuestoConBeneficiosConDecimales_NoTiraExcepcionAlIngresarEnBase()
        {
            // arrange

            // creamos un puesto que tiene valores decimales en el salario bruto
            PuestoModel puestoConSalarioDecimal = new()
            {
                Nombre = "Salario decimales",
                Plazas = 45,
                SalarioBruto = 0m,
                Beneficios = 456778.45m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // accion
            // la accion y el assert se realizan dentro del siguiente metodo
            IngresarPuestoNoTiraExpeciones(puestoConSalarioDecimal);

            // assert
            // leemos los puestos. En este caso deberia haber solo uno
            List<PuestoModel> puestosBasePostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);

            // revisamos que solo se haya ingresado solo un puesto en la base
            Assert.AreEqual(1, puestosBasePostInsercion.Count, "En la base se encontró más de un puesto cuando solo se ingresó uno");
        }

        // metodo que ingresa un puesto y verifica que se haya ingresado sin tirar excepciones
        // details: este metodos solo hace assert para revisar que no se hayan tirado excepciones y que el puesto ingresado se ingreso correctamente en la base
        private void IngresarPuestoNoTiraExpeciones(PuestoModel puestoAInsertar, string nombrePreActualizacion = "")
        {
            // creamos handler que contiene el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // accion
            try
            {
                estructuraOrgHandler.InsertarPuesto(nombrePreActualizacion, puestoAInsertar);
            }
            catch (Exception e)
            {
                // assert
                // en caso de tirar una exepcion la prueba falla y se muestra el error
                Assert.Fail(
                    string.Format("Se dio la excepción de tipo {0} con el mensaje: {1}", e.GetType(), e.Message)
               );
            }

            // assert

            // leemos los puestos luesgo de la insercion
            List<PuestoModel> puestosBasePostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);

            // buscamos el puesto que se ingreso
            // solo buscamos por el nombre porque este es unico ya que es llave primaria en la base de datos
            PuestoModel? puestoIngresado = puestosBasePostInsercion.Find(x => x.Nombre == puestoAInsertar.Nombre);

            // si es nulo el puestoIngresado significa que no se ingreso en la base de datos
            Assert.IsNotNull(puestoIngresado, "El puesto no se ingresó en la base de datos");

            // revisamos que el puesto ingresado y el esperado sean iguales
            // asi nos aseguramos de que se ingreso correctamente
            Assert.IsTrue(SonIgualesPuestos(puestoAInsertar, puestoIngresado), "El puesto leído de la base y el esperado no son iguales");
        }
    }
}
