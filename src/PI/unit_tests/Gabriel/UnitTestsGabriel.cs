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
using PI.Services;
using unit_tests.SharedResources;

namespace unit_tests.Gabriel
{
    // class: clase de testing para el modelo puesto y su interaction con la base de datos
    [TestClass]
    public class UnitTestsGabriel
    {
        [TestMethod]

        // Encargado de validar que el cálculo del punto de equilibrio se esté dando correctamente
        // en el caso en el que todos los parámetros son números positivos entre 0 y 999999999999999999.99
        // Este último se debe a que la base de datos está trabajando con números decimal 18,2
        public void CalcularPuntoEquilibrio_TodosParametrosValidos() {

            for (int i = 0; i < 1000; i += 1) {

                decimal resultado = 0;
                // Se hace que los tres parámetros que toma el método sean números aleatorios en el rango previamente mencionado.
                decimal precio = GenerarDecimalAleatorio();
                decimal costoVariable = GenerarDecimalAleatorio();
                decimal montoGastosFijos = GenerarDecimalAleatorio();

                decimal denominador = (precio - costoVariable);
                if (denominador != 0)
                {
                    resultado = montoGastosFijos / denominador;
                }

                Assert.AreEqual(resultado, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        // Verifica que no se produzca una división entre 0 en caso de que los parámetros costoVariable y Precio sean iguales
        public void CalcularPuntoEquilibrio_DivisionEntreCero() {
            for (int i = 0; i < 1000; i += 1) {
                // Se inicializan precio y costoVariables con el mismo valor, un número aleatorio entre 0 y 999999999999999999.99
                decimal precio = GenerarDecimalAleatorio();
                decimal costoVariable = precio;
                decimal montoGastosFijos = GenerarDecimalAleatorio();
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_PrecioNegativo()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal precio = -1 * GenerarDecimalAleatorio();
                decimal costoVariable = GenerarDecimalAleatorio();
                decimal montoGastosFijos = GenerarDecimalAleatorio();
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_MontoGastosFijosNegativo()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal precio = GenerarDecimalAleatorio();
                decimal costoVariable = GenerarDecimalAleatorio();
                decimal montoGastosFijos = GenerarDecimalAleatorio()*-1;
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }


        [TestMethod]
        public void CalcularPuntoEquilibrio_CostoVariableNegativo()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal precio =  GenerarDecimalAleatorio();
                decimal costoVariable = GenerarDecimalAleatorio() * -1;
                decimal montoGastosFijos = GenerarDecimalAleatorio();
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_TodosParametrosNegativos()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal costoVariable = GenerarDecimalAleatorio() * -1;
                decimal montoGastosFijos = GenerarDecimalAleatorio() * -1;
                decimal precio = GenerarDecimalAleatorio() * -1;
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_MontoGastosFijosMayorA182()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal precio = GenerarDecimalAleatorio();
                decimal costoVariable = GenerarDecimalAleatorio();
                decimal montoGastosFijos = 999999999999999999.99M + GenerarDecimalAleatorio();
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_CostoVariableMayorA182()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal precio = GenerarDecimalAleatorio();
                decimal montoGastosFijos = GenerarDecimalAleatorio();
                decimal costoVariable = 999999999999999999.99M + GenerarDecimalAleatorio();
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_PrecioMayorA182()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal costoVariable = GenerarDecimalAleatorio();
                decimal montoGastosFijos = GenerarDecimalAleatorio();
                decimal precio = 999999999999999999.99M + GenerarDecimalAleatorio();
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_TodosParametrosMayoresA182()
        {
            for (int i = 0; i < 1000; i += 1)
            {
                decimal costoVariable = 999999999999999999.99M + GenerarDecimalAleatorio();
                decimal montoGastosFijos = 999999999999999999.99M + GenerarDecimalAleatorio();
                decimal precio = 999999999999999999.99M + GenerarDecimalAleatorio();
                Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }



        public decimal GenerarDecimalAleatorio() {
            var random = new Random();
            string stringResult = Convert.ToString(random.Next(0, 99999999)) 
                + Convert.ToString(random.Next(0, 99999999)) + "." + Convert.ToString(random.Next(0, 99));
            Console.WriteLine(Convert.ToDecimal(stringResult));
            return Convert.ToDecimal(stringResult);
        }
    }
}
