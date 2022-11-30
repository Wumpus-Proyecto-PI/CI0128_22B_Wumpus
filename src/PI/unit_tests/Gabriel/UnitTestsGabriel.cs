using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
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
        private decimal[] valoresValidos = {123456788765432112.12M, 0M, 999999999999999999.99M, 12.34M, 365367583.67M};
        private decimal[] valoresNegativos = {-123456788765432112.12M, -1M, -999999999999999999.99M, -12.34M, -365367583.67M};
        private decimal[] valoresMayoresA182 = {1111111111111111111111.90M, 1000000000000000000M, 12345678910111213141516171819.87M
                , Decimal.MaxValue, Decimal.MaxValue-0.1M};

        [TestMethod]

        // Encargado de validar que el cálculo del punto de equilibrio se esté dando correctamente
        // en el caso en el que todos los parámetros son números positivos entre 0 y 999999999999999999.99
        // Este último se debe a que la base de datos está trabajando con números decimal 18,2
        public void CalcularPuntoEquilibrio_TodosParametrosValidos() {

            for (int i = 0; i < 5; i += 1) 
            {
                for (int j = 0; j < 5; j += 1) 
                {
                    for (int k = 0; k < 5; k += 1) 
                    {
                        if (k != j && k != i && j != i) 
                        {
                            decimal resultado = 0;
                            // Se hace que los tres parámetros que toma el método sean números aleatorios en el rango previamente mencionado.
                            decimal precio = valoresValidos[i];
                            decimal costoVariable = valoresValidos[j];
                            decimal montoGastosFijos = valoresValidos[k];

                            decimal denominador = (precio - costoVariable);
                            if (denominador != 0)
                            {
                                resultado = montoGastosFijos / denominador;
                            }

                            Assert.AreEqual(resultado, AnalisisRentabilidadService.CalcularPuntoEquilibrio(valoresValidos[k], valoresValidos[i], valoresValidos[j]));
                        }
                    }
                }
            }
        }

        [TestMethod]
        // Verifica que no se produzca una división entre 0 en caso de que los parámetros costoVariable y Precio sean iguales
        public void CalcularPuntoEquilibrio_DivisionEntreCero() {
            for (int i = 0; i < 5; i += 1)
            {
                    decimal precio = valoresValidos[i];
                    decimal costoVariable = precio;
                    decimal montoGastosFijos = valoresValidos[i];
                    Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_PrecioNegativo()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (i != j)
                        {
                            decimal precio = valoresNegativos[k];
                            decimal costoVariable = valoresValidos[i];
                            decimal montoGastosFijos = valoresValidos[j];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_MontoGastosFijosNegativo()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (i != j)
                        {
                            decimal precio = valoresValidos[i];
                            decimal costoVariable = valoresValidos[j];
                            decimal montoGastosFijos = valoresNegativos[k];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
            }
        }


        [TestMethod]
        public void CalcularPuntoEquilibrio_CostoVariableNegativo()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (i != j)
                        {
                            decimal precio = valoresValidos[i];
                            decimal costoVariable = valoresNegativos[k];
                            decimal montoGastosFijos = valoresValidos[j];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_TodosParametrosNegativos()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (k != j && k != i && j != i)
                        {
                            decimal costoVariable = valoresNegativos[i];
                            decimal montoGastosFijos = valoresNegativos[j];
                            decimal precio = valoresNegativos[k];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_MontoGastosFijosMayorA182()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (i != j)
                        {
                            decimal precio = valoresValidos[i];
                            decimal costoVariable = valoresValidos[j];
                            decimal montoGastosFijos = valoresMayoresA182[k];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_CostoVariableMayorA182()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (i != j)
                        {
                            decimal precio = valoresValidos[i];
                            decimal costoVariable = valoresMayoresA182[k];
                            decimal montoGastosFijos = valoresValidos[j];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_PrecioMayorA182()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (i != j)
                        {
                            decimal precio = valoresMayoresA182[j];
                            decimal costoVariable = valoresValidos[i];
                            decimal montoGastosFijos = valoresValidos[j];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void CalcularPuntoEquilibrio_TodosParametrosMayoresA182()
        {
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 5; j += 1)
                {
                    for (int k = 0; k < 5; k += 1)
                    {
                        if (k != j && k != i && j != i)
                        {
                            decimal costoVariable = valoresMayoresA182[i];
                            decimal montoGastosFijos = valoresMayoresA182[j];
                            decimal precio = valoresMayoresA182[k];
                            Assert.AreEqual(0, AnalisisRentabilidadService.CalcularPuntoEquilibrio(montoGastosFijos, precio, costoVariable));
                        }
                    }
                }
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
