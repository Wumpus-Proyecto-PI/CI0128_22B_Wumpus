using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using PI;
using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.DanielE
{
    // class: clase de testing para el modelo puesto y su interaction con la base de datos
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
            // id del usuario: 81727ffe-84fc-4263-b7e4-e763664968d9
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
                Beneficios = 4567m,
                SalarioBruto = 7894m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler= new();

            // action
            IngresarPuestoNoTiraExpeciones(nuevoPuesto);

            // assert
            // obtenemos los puestos de la base
            List<PuestoModel> puestosPostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);
            bool FueInsertado = puestosPostInsercion.Exists(x => x.Nombre == nuevoPuesto.Nombre);

            // removemos de la lista el que insertamos para comparar si el resto de puestos se vieron afectados
            puestosPostInsercion.RemoveAll(x => x.Nombre == nuevoPuesto.Nombre);

            bool puestoIguales = SonIgualesListasPuestos(puestosPreInsercion, puestosPostInsercion);

            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");
            Assert.IsTrue(FueInsertado, $"'{nuevoPuesto.Nombre}' no se insertó en la base");
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

            // action
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
                Beneficios = 4567m,
                SalarioBruto = 7894m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // action
            ActualizarPuestoNoTiraExpeciones(nuevoPuesto, puestoActualizar.Nombre);

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

            // si las dos listas esta igual antes y despues de la eliminacion no se vio afectado otro puesto por el query
            Assert.IsTrue(puestoIguales, "Los puestos pre-inserción son diferentes a los puestos post-inserción");

            // revisamos que no exista el puesto antes de ser actualizado en la lista de puesto que leimos de la base
            Assert.IsFalse(ExisteViejoNombre, $"No se modificó el nombre anterior '{puestoActualizar.Nombre}'");

            // revisamos que si exista el nuevo nombre del puesto que actualizamos en la lista de puesto que leimos de la base
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

            // action
            // assert
            // la action y el assert se realizan dentro del siguiente metodo
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

            // action
            // la action y el assert se realizan dentro del siguiente metodo
            IngresarPuestoNoTiraExpeciones(puestoConSalarioDecimal);

            // assert
            // leemos los puestos. En este caso deberia haber solo uno
            List<PuestoModel> puestosBasePostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);

            // revisamos que solo se haya ingresado solo un puesto en la base
            Assert.AreEqual(1, puestosBasePostInsercion.Count, "En la base se encontró más de un puesto cuando solo se ingresó uno");
        }

        // este test prueba que se puede actualizar un puesto con un valor de salario bruto que contiene decimales
        // details: ver @details de la prueba IngresarPuestoConSalarioConDecimales_NoTiraExcepcion porque en se de la misma situacion al ingresar y actualizar un decimal en la base
        [TestMethod]
        public void ActualizarPuesto_ConSalarioConDecimales_NoTiraExcepciones()
        {
            // arrange

            // insertamos los puesto semilla en la base de datos
            List<PuestoModel> puestosPreActualizacion = PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);


            // action
            // actualizamos el salario del puesto [1] con un valor que contiene decimales
            // modificamos directsmente el arreglo puestosPreActualizacion para comparar al final con puestosPostActualizacion
            puestosPreActualizacion[1].SalarioBruto = 4587.56m;
            ActualizarPuestoNoTiraExpeciones(puestosPreActualizacion[1], puestosPreActualizacion[1].Nombre);

            // assert
            // leemos los puestos. En este caso deberia haber solo uno
            List<PuestoModel> puestoPostActualizacion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);

            // revisamos que los puestos despues de la actualizacion sean iguales a los esperados
            Assert.IsTrue(SonIgualesListasPuestos(puestosPreActualizacion, puestoPostActualizacion), "La lista de puestos en la base no es igual a la esperada");
        }

        // este test prueba que se puede actualizar un puesto con un valor de beneficios que contiene decimales
        // details: ver @details de la prueba IngresarPuestoConSalarioConDecimales_NoTiraExcepcion porque en se de la misma situacion al ingresar y actualizar un decimal en la base
        [TestMethod]
        public void ActualizarPuesto_ConBeneficiosConDecimales_NoTiraExcepciones()
        {
            // arrange

            // insertamos los puesto semilla en la base de datos
            List<PuestoModel> puestosPreActualizacion = PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);


            // action
            // actualizamos el salario del puesto [1] con un valor que contiene decimales
            // modificamos directsmente el arreglo puestosPreActualizacion para comparar al final con puestosPostActualizacion
            puestosPreActualizacion[1].Beneficios = 4587.56m;
            ActualizarPuestoNoTiraExpeciones(puestosPreActualizacion[1], puestosPreActualizacion[1].Nombre);

            // assert
            // leemos los puestos. En este caso deberia haber solo uno
            List<PuestoModel> puestoPostActualizacion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);

            // revisamos que los puestos despues de la actualizacion sean iguales a los esperados
            Assert.IsTrue(SonIgualesListasPuestos(puestosPreActualizacion, puestoPostActualizacion), "La lista de puestos en la base no es igual a la esperada");
        }

        [TestMethod]
        public void ObtenerPuestosRetornaPuestosDelAnalisisCorrespondiente()
        {
            // arrange
            // creamos un segundo analisis en la base para el negocio actual
            // para verificar que obtener puestos de un analisis nos devuelve solo los del analisis correspondiente
            AnalisisHandler analisisHandler= new ();
            DateTime fechaSegundoAnalisis = analisisHandler.IngresarAnalisis(NegocioFicticio.ID, "En marcha");

            // insertamos en el primer analisis (analisis ficticio) del negocio ficticio una lista de puestos semilla
            List<PuestoModel> puestosEsperadosPrimerAnalisis = PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);

            // creamos una lista de puestos para insertar en el segundo analisis del negocio ficticio
            // esta lista debe ser diferente a la ingresadas en el primr analisis con el metodo PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);
            List<PuestoModel> puestosEsperadosSegundoAnalisis = new()
            {
                new PuestoModel() {
                    Nombre = "Cajero",
                    Plazas = 45,
                    SalarioBruto = 7894m,
                    Beneficios = 8963m,
                    FechaAnalisis = fechaSegundoAnalisis
                },
                new PuestoModel() {
                    Nombre = "Repartidor",
                    Plazas = 11,
                    SalarioBruto = 4568m,
                    Beneficios = 9632m,
                    FechaAnalisis = fechaSegundoAnalisis
                },
                new PuestoModel() {
                    Nombre = "Conserje",
                    Plazas = 4,
                    SalarioBruto = 7894m,
                    Beneficios = 1234m,
                    FechaAnalisis = fechaSegundoAnalisis
                },
                new PuestoModel() {
                    Nombre = "Fotografo",
                    Plazas = 6,
                    SalarioBruto = 4567m,
                    Beneficios = 78941m,
                    FechaAnalisis = fechaSegundoAnalisis
                }
            };
            // insertamos en el segundo analisis del negocio ficticio la lista de puestos semilla anterior
            puestosEsperadosSegundoAnalisis = PuestoTestingHandler.InsertarPuestosSemillaEnBase(puestosEsperadosSegundoAnalisis);

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new EstructuraOrgHandler();

            // action
            // esta primera lista deberia ser igual al que se ingreso en el metodo PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);
            List<PuestoModel> puestosEnBaseDePrimerAnalisis = estructuraOrgHandler.ObtenerListaDePuestos(AnalisisFicticio.FechaCreacion);

            // esta lista deberia de estar vacia porque no se le ingreso ningun puesto
            List<PuestoModel> puestosEnBaseDeSegundoAnalisis = estructuraOrgHandler.ObtenerListaDePuestos(fechaSegundoAnalisis);

            // assert
            // revisamos que los puestos del primer analisis son iguales a los esperados
            Assert.IsTrue(SonIgualesListasPuestos(puestosEsperadosPrimerAnalisis, puestosEnBaseDePrimerAnalisis), "Los puestos esperados del primer análisis no son iguales a los leídos de la base");

            // revisamos que los puestos del segundo analisis son iguales a los esperados
            Assert.IsTrue(SonIgualesListasPuestos(puestosEsperadosSegundoAnalisis, puestosEnBaseDeSegundoAnalisis), "Los puestos esperados del segundo análisis no son iguales a los leídos de la base");

            // revisamos que los puestos de los dos analisis sean diferentes porque se les ingreso listas diferentes de puestos
            Assert.IsFalse(SonIgualesListasPuestos(puestosEnBaseDePrimerAnalisis, puestosEnBaseDeSegundoAnalisis), "Los puestos leídos del primer análisis y del segundo análisis son iguales y no deberían");
        }

        [TestMethod]
        public void ExisteEnBase_RetornaVerdadero_CuandoEncuentraElPuesto()
        {
            // arrange 
            // creamos puesto a ingresar en la base para revisar si existe
            PuestoModel existePuesto = new PuestoModel
            {
                Nombre = "Puesto existe",
                Plazas = 48,
                Beneficios = 4567m,
                SalarioBruto = 7894m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };
            PuestoTestingHandler.InsertarPuestosSemillaEnBase(existePuesto);

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // action
            bool existeEnBase = estructuraOrgHandler.ExistePuestoEnBase(existePuesto.Nombre, existePuesto.FechaAnalisis);

            // assert

            // verificamos que el metodo si devuelva true porque el puesto si existe en la base ya que si lo insertamos
            Assert.IsTrue(existeEnBase, $"El puesto {existePuesto.Nombre} no existe en la base y no debería");
        }

        [TestMethod]
        public void ExisteEnBase_RetornaFalso_CuandoNoEncuentraElPuesto()
        {
            // arrange 
            // creamos puesto a ingresar en la base para revisar si existe
            PuestoModel noExistePuesto = new PuestoModel
            {
                Nombre = "No existe puesto",
                Plazas = 48,
                Beneficios = 4567m,
                SalarioBruto = 7894m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // action
            bool existeEnBase = estructuraOrgHandler.ExistePuestoEnBase(noExistePuesto.Nombre, noExistePuesto.FechaAnalisis);

            // assert

            // verificamos que el metodo si devuelva true porque el puesto si existe en la base ya que si lo insertamos
            Assert.IsFalse(existeEnBase, $"El puesto {noExistePuesto.Nombre} si existe en la base y no debería");
        }

        // // metodos que asisten a los metodos de testing

        // metodo que ingresa un puesto y verifica que se haya ingresado sin tirar excepciones
        // details: este metodos solo hace assert para revisar que no se hayan tirado excepciones y que el puesto ingresado se ingreso correctamente en la base
        private void IngresarPuestoNoTiraExpeciones(PuestoModel puestoAInsertar)
        {
            // creamos handler que contiene el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // action
            try
            {
                estructuraOrgHandler.InsertarPuesto("", puestoAInsertar);
            }
            catch (Exception e)
            {
                // assert
                // en caso de tirar una excepcion la prueba falla y se muestra el error
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

        // metodo que ingresa un puesto y verifica que se haya ingresado sin tirar excepciones
        // details: este metodos solo hace assert para revisar que no se hayan tirado excepciones y que el puesto ingresado se ingreso correctamente en la base
        private void ActualizarPuestoNoTiraExpeciones(PuestoModel puestoAInsertar, string nombrePreActualizacion)
        {
            // creamos handler que contiene el metodo que deseamos probar
            EstructuraOrgHandler estructuraOrgHandler = new();

            // action
            try
            {
                estructuraOrgHandler.ActualizarPuesto(nombrePreActualizacion, puestoAInsertar);
            }
            catch (Exception e)
            {
                // assert
                // en caso de tirar una excepcion la prueba falla y se muestra el error
                Assert.Fail(
                    string.Format("Se dio la excepción de tipo {0} con el mensaje: {1}", e.GetType(), e.Message)
               );
            }

            // assert

            // leemos los puestos luego de la insercion
            List<PuestoModel> puestosBasePostInsercion = PuestoTestingHandler.LeerPuestosDeBase(AnalisisFicticio.FechaCreacion);

            // buscamos el puesto que se ingreso
            // solo buscamos por el nombre porque este es unico ya que es llave primaria en la base de datos
            PuestoModel? puestoIngresado = puestosBasePostInsercion.Find(x => x.Nombre == puestoAInsertar.Nombre);

            // si es nulo el puestoIngresado significa que no se ingreso en la base de datos
            Assert.IsNotNull(puestoIngresado, "El puesto no se ingresó en la base de datos");

            // revisamos que el puesto ingresado y el esperado sean iguales
            // asi nos aseguramos de que se ingreso correctamente
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
                && esperada.Beneficios == actual.Beneficios)
            {
                puestosIguales = true;
            }

            return puestosIguales;
        }
    }
}
