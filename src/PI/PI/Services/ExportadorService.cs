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
            hojaAnalisisRentabilidad.Cell("A6").Value = "Gastos fijos mensuales";
            hojaAnalisisRentabilidad.Cell("A8").Value = "Nombre de producto";
            hojaAnalisisRentabilidad.Cell("B8").Value = "Precio";
            hojaAnalisisRentabilidad.Cell("C8").Value = "Porcentaje de ventas";
            hojaAnalisisRentabilidad.Cell("D8").Value = "Costo variable";
            hojaAnalisisRentabilidad.Cell("E8").Value = "Comisión";
            hojaAnalisisRentabilidad.Cell("F8").Value = "Margen";
            hojaAnalisisRentabilidad.Cell("G8").Value = "Margen ponderado";
            hojaAnalisisRentabilidad.Cell("H8").Value = $"Punto de equilibrio{Environment.NewLine}Unidad";
            hojaAnalisisRentabilidad.Cell("I8").Value = $"Punto de equilibrio{Environment.NewLine}Monto";
            hojaAnalisisRentabilidad.Cell("J8").Value = $"Meta de ventas{Environment.NewLine}Unidad";
            hojaAnalisisRentabilidad.Cell("K8").Value = $"Meta de ventas{Environment.NewLine}Monto";
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

            hojaAnalisisRentabilidad.Cell("C2").Value = $"{nombreNegocio}{Environment.NewLine}{estadoNegocioEnAnalisis}";

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            // añade fecha de creación del análisis
            hojaAnalisisRentabilidad.Cell("C3").Value = analisis.FechaCreacion.ToString("dd/MMM/yyyy", new CultureInfo("es-Es"));
            hojaAnalisisRentabilidad.Cell("C5").Value = analisis.GananciaMensual;
            hojaAnalisisRentabilidad.Cell("C6").Value = $"{gastoFijoHandler.obtenerTotalAnual(fechaCreacionAnalisis) / 12}";
        }
        
        public void AñadirEstiloRentabilidad()
        {
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.FontSize = 16;
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Range("A1:K1").Merge();
            hojaAnalisisRentabilidad.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            hojaAnalisisRentabilidad.Range("A8","K8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            hojaAnalisisRentabilidad.Range("A8","K8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Font.SetFontColor(XLColor.FromHtml("#FFFFFF"));

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
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = productos[productoActual].Nombre; // productos[productoActual].Nombre;
                celdaActual = ++columnaActual + filaActual.ToString();

                // B Precio
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = productos[productoActual].Precio; // productos[productoActual].Precio;
                celdaActual = ++columnaActual + filaActual.ToString();

                // C Porcentaje de ventas
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = $"{productos[productoActual].PorcentajeDeVentas}%"; // productos [productoActual].PorcentajeDeVentas
                hojaAnalisisRentabilidad.Cell(celdaActual).CellRight();
                celdaActual = ++columnaActual + filaActual.ToString();

                // D Costo Variable
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = productos[productoActual].CostoVariable; // productos[productoActual].CostoVariable;
                celdaActual = ++columnaActual + filaActual.ToString();

                // E Comisión
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = 2; // TODO productos[productoActual].Comision;
                celdaActual = ++columnaActual + filaActual.ToString();

                columnaActual = 'A';
                ++filaActual;
                ++productoActual;
            }
        }

        public void FormatearCeldasRentabilidad(string celdaInicial, string celdaFinal, int cantidadProductos)
        {
            // por rango (dentro de tabla)
            hojaAnalisisRentabilidad.Range(celdaInicial, celdaFinal).Style.NumberFormat.Format = "#,##0.00";
            
            // ganancia mensual y gastos fijos
            hojaAnalisisRentabilidad.Range("C5", "C6").Style.NumberFormat.Format = "#,##0.00";

        }
        public void AgregarFormulas(int cantidadProductos)
        {
            int productoActual = 0;
            char columnaActual = 'F'; // hasta K
            int filaActual = 9;
            string celdaActual = "";
            while (productoActual < 1)
            {
                celdaActual = columnaActual + filaActual.ToString();
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
        public MemoryStream obtenerReporte(string fechaAnalisis)
        {
            reportarAnalisisDeRentabilidad(fechaAnalisis);
            using (MemoryStream stream = new MemoryStream())
            {
                libro.SaveAs(stream);
                return stream;
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
            FormatearCeldasRentabilidad("B9", $"K{productos.Count() + 8}", productos.Count());


            hojaAnalisisRentabilidad.Columns().AdjustToContents();
            libro.RecalculateAllFormulas();
        }
    }
}