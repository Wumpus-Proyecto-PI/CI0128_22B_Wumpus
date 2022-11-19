using ClosedXML.Excel;
using PI.Handlers;
using PI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace PI.Service
{
    // Servicio encargado de exportar el libro de reporte con la hoja de análisis de rentabilidad y del flujo de caja
    public class ExportadorService
    {
        // Archivo xlsx
        XLWorkbook libro = new XLWorkbook();
        IXLWorksheet? hojaAnalisisRentabilidad;
        IXLWorksheet? hojaFlujoCaja;

        // Invoca la creación del archivo xlsx y lo retorna en memoria.
        public MemoryStream obtenerReporte(string fechaAnalisis)
        {
            ReportarAnalisisDeRentabilidad(fechaAnalisis);
            // TODO agregar reporte de flujo de caja
            using (MemoryStream stream = new MemoryStream())
            {
                libro.SaveAs(stream);
                return stream;
            }
        }

        // Crea la hoja referente al análisis de rentabilidad.
        public void ReportarAnalisisDeRentabilidad(string fechaAnalisis)
        {
            hojaAnalisisRentabilidad = libro.AddWorksheet("Analisis de Rentabilidad");
            InsertarEncabezadoRentabilidad();

            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            AgregarValoresDeEncabezado(fechaCreacionAnalisis);

            ProductoHandler productoHandler = new ProductoHandler();
            List<ProductoModel> productos = productoHandler.ObtenerProductos(fechaCreacionAnalisis);
            int cantidadProductos = productos.Count;

            AgregarValoresDeProductos(ref productos);
            AgregarFormulasRentabilidad(cantidadProductos);
            AgregarTotalesRentabilidad(cantidadProductos);
            FormatearCeldasRentabilidad(cantidadProductos); // 8: filas anteriores + 1 fila del total.
            AñadirEstiloRentabilidad($"K{cantidadProductos + 8 + 1}");

            hojaAnalisisRentabilidad.Columns().AdjustToContents();
            libro.RecalculateAllFormulas();
        }

        // Inserta los valores predeterminados del encabezado de la hoja de análisis de rentabilidad
        public void InsertarEncabezadoRentabilidad()
        {
            hojaAnalisisRentabilidad.Cell("A1").Value = "Análisis de rentabilidad";
            hojaAnalisisRentabilidad.Cell("A2").Value = $"Nombre{Environment.NewLine}Estado del negocio";
            hojaAnalisisRentabilidad.Cell("A3").Value = "Fecha de creación del análisis";

            hojaAnalisisRentabilidad.Cell("A5").Value = "Ganancia mensual";
            hojaAnalisisRentabilidad.Cell("A6").Value = "Gastos fijos mensuales";
            hojaAnalisisRentabilidad.Cell("A8").Value = "Nombre de producto";
            hojaAnalisisRentabilidad.Cell("B8").Value = "Precio";
            hojaAnalisisRentabilidad.Cell("C8").Value = "Porcentaje de ventas";
            hojaAnalisisRentabilidad.Cell("D8").Value = "Costo variable";
            hojaAnalisisRentabilidad.Cell("E8").Value = "Comisión";
            hojaAnalisisRentabilidad.Cell("F8").Value = "Margen";
            hojaAnalisisRentabilidad.Cell("G8").Value = "Margen ponderado";

            hojaAnalisisRentabilidad.Range("H7","I7").Merge();
            hojaAnalisisRentabilidad.Cell("H7").Value = $"Punto de equilibrio";
            hojaAnalisisRentabilidad.Cell("H8").Value = "Unidad";
            hojaAnalisisRentabilidad.Cell("I8").Value = "Monto";

            hojaAnalisisRentabilidad.Range("J7","K7").Merge();
            hojaAnalisisRentabilidad.Cell("J7").Value = $"Meta de ventas";
            hojaAnalisisRentabilidad.Cell("J8").Value = "Unidad";
            hojaAnalisisRentabilidad.Cell("K8").Value = "Monto";
        }

        // Inserta los valores cargados del encabezado de la hoja de análisis de rentabilidad
        public void AgregarValoresDeEncabezado(DateTime fechaCreacionAnalisis)
        {
            // añade nombre del negocio
            Handler handler = new Handler();
            string nombreNegocio = handler.obtenerNombreNegocio(fechaCreacionAnalisis);

            // añade estado del negocio
            AnalisisHandler analisisHandler = new AnalisisHandler();
            AnalisisModel analisis = analisisHandler.ObtenerUnAnalisis(fechaCreacionAnalisis);
            string estadoNegocioEnAnalisis = analisis.Configuracion.EstadoNegocio == true ? "En marcha" : "No iniciado";
            hojaAnalisisRentabilidad.Cell("B2").Value = $"{nombreNegocio}{Environment.NewLine}{estadoNegocioEnAnalisis}";

            // añade fecha de creación del análisis
            hojaAnalisisRentabilidad.Cell("B3").Value = analisis.FechaCreacion.ToString("dd/MMM/yyyy", new CultureInfo("es-Es"));

            // añade la ganancia mensual y los gastos fijos mensuales
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            hojaAnalisisRentabilidad.Cell("B5").Value = analisis.GananciaMensual;
            hojaAnalisisRentabilidad.Cell("B6").Value = $"{gastoFijoHandler.obtenerTotalAnual(fechaCreacionAnalisis) / 12}";
        }

        // Agrega los valores (que no son fórmulas) que posee cada producto a la hoja de análisis de rentabilidad.
        public void AgregarValoresDeProductos(ref List<ProductoModel> productos)
        {
            int productoActual = 0;
            char columnaActual = 'A'; // Llega hasta la columna E 
            int filaActual = 9;  // Inicio de la tabla de productos.
            // Itera sobre las filas de la hoja para agregar los valores de cada producto.
            while (productoActual < productos.Count())
            {
                // A Nombre de producto
                string celdaActual = columnaActual + filaActual.ToString();
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = productos[productoActual].Nombre;
                celdaActual = ++columnaActual + filaActual.ToString();

                // B Precio
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = productos[productoActual].Precio;
                celdaActual = ++columnaActual + filaActual.ToString();

                // C Porcentaje de ventas
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = $"{productos[productoActual].PorcentajeDeVentas}%";
                hojaAnalisisRentabilidad.Cell(celdaActual).CellRight();
                celdaActual = ++columnaActual + filaActual.ToString();

                // D Costo Variable
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = productos[productoActual].CostoVariable;
                celdaActual = ++columnaActual + filaActual.ToString();

                // E Comisión
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = 0.0; // TODO productos[productoActual].Comision;
                celdaActual = ++columnaActual + filaActual.ToString();

                // itera al siguiente producto
                columnaActual = 'A';
                ++filaActual;
                ++productoActual;
            }
        }

        // Agrega las formulas para cada producto a la hoja de análisis de rentabilidad.
        public void AgregarFormulasRentabilidad(int cantidadProductos)
        {
            int productoActual = 0;
            char columnaActual = 'F';  // Llega hasta la columna K
            int filaActual = 9;  // Inicio de la tabla de productos.
            int filaTotales = 8 + cantidadProductos + 1;
            string celdaActual = "";
            while (productoActual < cantidadProductos)
            {
                celdaActual = columnaActual + filaActual.ToString();
                // F Margen
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B{filaActual}-D{filaActual}"; // Precio - costo variable
                celdaActual = ++columnaActual + filaActual.ToString();

                // G Margen ponderado
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"C{filaActual}*F{filaActual}"; // %Ventas * margen
                celdaActual = ++columnaActual + filaActual.ToString();

                // H Punto de equilibrio Unidad
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B6 / (B{filaActual}-D{filaActual})"; // gastosFijos / (precio - costoVariable)
                celdaActual = ++columnaActual + filaActual.ToString();

                // I Punto de equilibrio Monto
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B{filaActual}*H{filaActual}"; // (precio * puntoEquilibrioUnidad)
                celdaActual = ++columnaActual + filaActual.ToString();

                // J Meta de ventas Unidad
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"(B6+B5)/G{filaTotales}*C{filaActual}"; // (gastosFijoMensuales+gananciaMensual)/totalMargenPonderado*%Ventas
                celdaActual = ++columnaActual + filaActual.ToString();

                // K Meta de ventas Monto
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B{filaActual} * J{filaActual}"; // precio * metaVentasUnidad
                columnaActual = 'F';
                ++filaActual;
                ++productoActual;
            }
        }

        // Agrega la fila de los valores totales de la hoja de análisis de rentabilidad.
        public void AgregarTotalesRentabilidad(int cantidadProductos)
        {
            int filaTotales = 8 + cantidadProductos + 1;
            hojaAnalisisRentabilidad.Cell($"A{filaTotales}").Value = "TOTALES";
            char columnaActual = 'B';
            while (columnaActual != 'L') // L = K+1
            {
                hojaAnalisisRentabilidad.Cell($"{columnaActual}{filaTotales}").FormulaA1 = $"SUM({columnaActual}9:{columnaActual}{filaTotales - 1})";
                ++columnaActual;
            }
            hojaAnalisisRentabilidad.Range($"A{filaTotales}", $"K{filaTotales}").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Range($"A{filaTotales}", $"K{filaTotales}").Style.Fill.BackgroundColor = XLColor.FromHtml("#8E8E8E");
            hojaAnalisisRentabilidad.Range($"A{filaTotales}", $"K{filaTotales}").Style.Font.SetFontColor(XLColor.FromHtml("#FFFFFF"));
        }

        // Agrega el formato estadístico a cada celda que lo necesita.
        public void FormatearCeldasRentabilidad(int cantidadProductos)
        {
            // ganancia mensual y gastos fijos
            hojaAnalisisRentabilidad.Range("C5", "C6").Style.NumberFormat.Format = "#,##0.00";

            // porcentajes de ventas
            hojaAnalisisRentabilidad.Range($"C{9}", $"C{8 + cantidadProductos + 1}").SetDataType(XLDataType.Number);
            hojaAnalisisRentabilidad.Range($"C{9}", $"C{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#.00%";

            // columna precio
            hojaAnalisisRentabilidad.Range("B9", $"B{8 + cantidadProductos}").Style.NumberFormat.Format = "#,##0.00";
            
            // rango: desde costo variable a meta de ventas.
            hojaAnalisisRentabilidad.Range("D9", $"K{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#,##0.00";
        }

        // Agrega estilos (decoración visual) a la hoja de análisis de rentabilidad
        public void AñadirEstiloRentabilidad(string celdaFinal)
        {
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.FontSize = 16;
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.SetFontColor(XLColor.FromHtml("#4472C4"));
            hojaAnalisisRentabilidad.Cell("A2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            hojaAnalisisRentabilidad.Range("A1:K1").Merge();
            hojaAnalisisRentabilidad.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Font.SetFontColor(XLColor.FromHtml("#FFFFFF"));


            hojaAnalisisRentabilidad.Cell("A5").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("A6").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            hojaAnalisisRentabilidad.Cell("B5").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("B5").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("B6").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("B6").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A5", "B6").Style.Font.SetBold();

            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }
    }
}