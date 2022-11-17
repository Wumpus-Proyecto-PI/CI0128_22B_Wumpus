using ClosedXML.Excel;
using PI.Handlers;
using PI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace PI.Service
{
    public class ExportadorService
    {
        XLWorkbook libro = new XLWorkbook();
        IXLWorksheet hojaAnalisisRentabilidad = null;

        public void InsertarEncabezadoRentabilidad()
        {
            hojaAnalisisRentabilidad.Cell("A1").Value = "Análisis de rentabilidad";
            hojaAnalisisRentabilidad.Cell("A2").Value = "Nombre y estado del negocio";
            hojaAnalisisRentabilidad.Cell("A3").Value = "Fecha de creación del análisis";

            hojaAnalisisRentabilidad.Cell("A5").Value = "Ganancia mensual";
            hojaAnalisisRentabilidad.Cell("A6").Value = "Gastos fijos";
            hojaAnalisisRentabilidad.Cell("A8").Value = "Nombre de producto";
            hojaAnalisisRentabilidad.Cell("B8").Value = "Precio";
            hojaAnalisisRentabilidad.Cell("C8").Value = "Porcentaje de ventas";
            hojaAnalisisRentabilidad.Cell("D8").Value = "Costo variable";
            hojaAnalisisRentabilidad.Cell("E8").Value = "Comisión";
            hojaAnalisisRentabilidad.Cell("F8").Value = "Margen";
            hojaAnalisisRentabilidad.Cell("G8").Value = "Margen ponderado";
            hojaAnalisisRentabilidad.Cell("H8").Value = "Punto de equilibrio. Unidad";
            hojaAnalisisRentabilidad.Cell("I8").Value = "Punto de equilibrio. Monto";
            hojaAnalisisRentabilidad.Cell("J8").Value = "Meta de ventas. Unidad";
            hojaAnalisisRentabilidad.Cell("K8").Value = "Meta de ventas. Monto";
        }
        public void AñadirEstiloRentabilidad()
        {
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.FontSize = 16;
            hojaAnalisisRentabilidad.Range("A1:J1").Row(1).Merge();
            hojaAnalisisRentabilidad.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            CentrarFilaRentabilidad('A', 'J', 8);
        }

        public void CentrarFilaRentabilidad(char columnaInicial, char columnaFinal, int fila)
        {
            string celdaActual = "";

            for (char columnaActual = columnaInicial; columnaActual != columnaFinal; ++columnaActual)
            {
                celdaActual = columnaActual + fila.ToString();
                hojaAnalisisRentabilidad.Cell(celdaActual).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }
        }
        public void reportarAnalisisDeRentabilidad(string fechaAnalisis)
        {
            hojaAnalisisRentabilidad = libro.AddWorksheet("Analisis de Rentabilidad");
            InsertarEncabezadoRentabilidad();
            AñadirEstiloRentabilidad();
            
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            AgregarValoresDeEncabezado(fechaCreacionAnalisis);

            ProductoHandler productoHandler = new ProductoHandler();
            List<ProductoModel> productos = productoHandler.ObtenerProductos(fechaCreacionAnalisis);

            AgregarValoresDeProductos(fechaCreacionAnalisis, ref productos);
            AgregarFormulas(productos.Count());


            hojaAnalisisRentabilidad.Columns().AdjustToContents();
            libro.RecalculateAllFormulas();
        }
        public void AgregarValoresDeProductos(DateTime fechaCreacionAnalisis, ref List<ProductoModel> productos)
        {
            int productoActual = 0;
            char columnaActual = 'A'; // hasta E
            int filaActual = 9;
            while (productoActual < 1)
            {
                // A Nombre de producto
                string celdaActual = columnaActual + filaActual.ToString();
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = "nombre"; // productos[productoActual].Nombre;
                celdaActual = ++columnaActual + filaActual.ToString();

                // B Precio
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = 3; // productos[productoActual].Precio;
                celdaActual = ++columnaActual + filaActual.ToString();

                // C Porcentaje de ventas
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = 50; // productos[productoActual].PorcentajeDeVentas;
                celdaActual = ++columnaActual + filaActual.ToString();

                // D Costo Variable
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = 1000; // productos[productoActual].CostoVariable;
                celdaActual = ++columnaActual + filaActual.ToString();

                // E Comisión
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = 2; // productos[productoActual].Comision;
                celdaActual = ++columnaActual + filaActual.ToString();

                columnaActual = 'A';
                ++filaActual;
                ++productoActual;
            }
        }

        public void AgregarFormulas(int cantidadProductos)
        {
            int productoActual = 0;
            char columnaActual = 'F'; // hasta K
            int filaActual = 9;
            while (productoActual < 1)
            {
                string celdaActual = columnaActual + filaActual.ToString();
                // F Margen
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B{filaActual}-D{filaActual}"; // Precio - costo variable
                celdaActual = ++columnaActual + filaActual.ToString();

                // G Margen ponderado
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"C{filaActual}*F{filaActual}"; // %Ventas * margen
                celdaActual = ++columnaActual + filaActual.ToString();

                // H Punto de equilibrio Unidad
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"C6 / (B{filaActual}-D{filaActual})"; // gastosFijos / (precio - costoVariable)
                celdaActual = ++columnaActual + filaActual.ToString();

                // I Punto de equilibrio Monto
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B{filaActual}*H{filaActual}"; // (precio * puntoEquilibrioUnidad)
                celdaActual = ++columnaActual + filaActual.ToString();

                // J Meta de ventas Unidad
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"C{filaActual}*(C6 + C5) / G{filaActual}"; // (%Ventas * (gastosFijosMensuales + gananciaMensual)) / margenPonderado
                celdaActual = ++columnaActual + filaActual.ToString();

                // K Meta de ventas Monto
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B{filaActual} * J{filaActual}"; // precio * metaVentasUnidad
                columnaActual = 'F';
                ++filaActual;
                ++productoActual;
            }
        }
        public void AgregarValoresDeEncabezado(DateTime fechaCreacionAnalisis)
        {
            // añade nombre del negocio
            Handler handler = new Handler();
            string nombreNegocio = handler.obtenerNombreNegocio(fechaCreacionAnalisis);

            // añade estado del negocio
            AnalisisHandler analisisHandler = new AnalisisHandler();
            AnalisisModel analisis = analisisHandler.ObtenerUnAnalisis(fechaCreacionAnalisis);
            string estadoNegocioEnAnalisis = analisis.Configuracion.EstadoNegocio == true ? "En marcha" : "No iniciado";

            hojaAnalisisRentabilidad.Cell("C2").Value = nombreNegocio + " - " + estadoNegocioEnAnalisis;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            // añade fecha de creación del análisis
            hojaAnalisisRentabilidad.Cell("C3").Value = analisis.FechaCreacion.ToString("dd/MMM/yyyy", new CultureInfo("es-Es"));
            hojaAnalisisRentabilidad.Cell("C5").Value = analisis.GananciaMensual;
            hojaAnalisisRentabilidad.Cell("C6").Value = gastoFijoHandler.obtenerTotalAnual(fechaCreacionAnalisis);
        }
        
        // solo funciona para columnas de una letra. Ej: para AA no funciona.
        public void AsignarValorEnColumna(char columna, int inicio, int final, string valor)
        {
            string celdaActual = "";
            for (int filaActual = inicio; filaActual < final; ++filaActual)
            {
                celdaActual = columna + filaActual.ToString();
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = valor;
            }
        }

        public void AsignarFormulaEnColumna(char columna, int inicio, int final, string formula)
        {
            string celdaActual = "";
            for (int filaActual = inicio; filaActual < final; ++filaActual)
            {
                celdaActual = columna + filaActual.ToString();
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = formula;
            }
        }
        public MemoryStream obtenerReporte(string fechaAnalisis)
        {
            reportarAnalisisDeRentabilidad(fechaAnalisis);
            using (MemoryStream stream = new MemoryStream())
            {
                libro.SaveAs(stream);
                return stream;
            }
        }
    }
}