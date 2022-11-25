using ClosedXML.Excel;
using PI.Handlers;
using PI.Models;
using PI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using PI.Views.Shared.Components.GastoFijo;

namespace PI.Service
{
    // Servicio encargado de exportar el libro de reporte con la hoja de an�lisis de rentabilidad y del flujo de caja
    public class ExportadorService
    {
        // Archivo xlsx
        XLWorkbook libro = new XLWorkbook();
        IXLWorksheet? hojaAnalisisRentabilidad;
        IXLWorksheet? hojaFlujoCaja;

        // Una columna del libro a cada caracteristica de un producto.
        const char NombreProductoRentabilidad = 'A';
        const char PorcentajeVentasRentabilidad = 'B';
        const char ComisionVentasRentabilidad = 'C';
        const char PrecioRentabilidad = 'D';
        const char CostoVariableRentabilidad = 'E';
        const char MargenRentabilidad = 'F';
        const char MargenPonderadoRentabilidad = 'G';
        const char PtoEquilibrioUnidadRentabilidad = 'H';
        const char PtoEquilibrioMontoRentabilidad = 'I';
        const char MetaVentasUnidadRentabilidad = 'J';
        const char MetaVentasMontoRentabilidad = 'K';

        // Invoca la creaci�n del archivo xlsx y lo retorna en memoria.
        public MemoryStream obtenerReporte(string fechaAnalisis)
        {
            ReportarAnalisisDeRentabilidad(fechaAnalisis);
            ReportarFlujoDeCaja(fechaAnalisis);
            // TODO agregar reporte de flujo de caja
            using (MemoryStream stream = new MemoryStream())
            {
                libro.SaveAs(stream);
                return stream;
            }
        }

        // Crea la hoja referente al an�lisis de rentabilidad.
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
            A�adirEstiloRentabilidad($"K{cantidadProductos + 8 + 1}");

            hojaAnalisisRentabilidad.Columns().AdjustToContents();
            libro.RecalculateAllFormulas();
        }

        // Inserta los valores predeterminados del encabezado de la hoja de an�lisis de rentabilidad
        public void InsertarEncabezadoRentabilidad()
        {
            hojaAnalisisRentabilidad.Cell("A1").Value = "An�lisis de rentabilidad";
            hojaAnalisisRentabilidad.Cell("A2").Value = $"Nombre{Environment.NewLine}Estado del negocio";
            hojaAnalisisRentabilidad.Cell("A3").Value = "Fecha de creaci�n del an�lisis";

            hojaAnalisisRentabilidad.Cell("A5").Value = "Ganancia mensual";
            hojaAnalisisRentabilidad.Cell("A6").Value = "Gastos fijos mensuales";
            hojaAnalisisRentabilidad.Cell($"{NombreProductoRentabilidad}8").Value = "Nombre de producto";
            hojaAnalisisRentabilidad.Cell($"{PrecioRentabilidad}8").Value = "Precio";
            hojaAnalisisRentabilidad.Cell($"{PorcentajeVentasRentabilidad}8").Value = "Porcentaje de ventas";
            hojaAnalisisRentabilidad.Cell($"{CostoVariableRentabilidad}8").Value = "Costo variable";
            hojaAnalisisRentabilidad.Cell($"{ComisionVentasRentabilidad}8").Value = "Comisi�n";
            hojaAnalisisRentabilidad.Cell($"{MargenRentabilidad}8").Value = "Margen";
            hojaAnalisisRentabilidad.Cell($"{MargenPonderadoRentabilidad}8").Value = "Margen ponderado";

            hojaAnalisisRentabilidad.Range("H7","I7").Merge();
            hojaAnalisisRentabilidad.Cell("H7").Value = $"Punto de equilibrio";
            hojaAnalisisRentabilidad.Cell($"{PtoEquilibrioUnidadRentabilidad}8").Value = "Unidad";
            hojaAnalisisRentabilidad.Cell($"{PtoEquilibrioMontoRentabilidad}8").Value = "Monto";

            hojaAnalisisRentabilidad.Range("J7","K7").Merge();
            hojaAnalisisRentabilidad.Cell("J7").Value = $"Meta de ventas";
            hojaAnalisisRentabilidad.Cell($"{MetaVentasUnidadRentabilidad}8").Value = "Unidad";
            hojaAnalisisRentabilidad.Cell($"{MetaVentasMontoRentabilidad}8").Value = "Monto";
        }

        public void InsertarEncabezadoFlujoDeCaja() {
            hojaFlujoCaja.Range("A1", "C1").Merge();
            hojaFlujoCaja.Cell("A1").Value = "Flujo de caja";
            hojaFlujoCaja.Cell("D1").Value = "Meta mensual de ventas";
            hojaFlujoCaja.Cell("F1").Value = "Inversi�n inicial";

            hojaFlujoCaja.Cell("A3").Value = "Ingresos";
            hojaFlujoCaja.Cell("B3").Value = "Mes 1";
            hojaFlujoCaja.Cell("C3").Value = "Mes 2";
            hojaFlujoCaja.Cell("D3").Value = "Mes 3";
            hojaFlujoCaja.Cell("E3").Value = "Mes 4";
            hojaFlujoCaja.Cell("F3").Value = "Mes 5";
            hojaFlujoCaja.Cell("G3").Value = "Mes 6";

            hojaFlujoCaja.Cell("A4").Value = "Ingresos por ventas de contado";
            hojaFlujoCaja.Cell("A5").Value = "Ingresos por ventas a cr�dito";
            hojaFlujoCaja.Cell("A6").Value = "Otros ingresos";
            hojaFlujoCaja.Cell("A7").Value = "Total ingresos";

            hojaFlujoCaja.Cell("A8").Value = "Egresos";
            hojaFlujoCaja.Cell("B8").Value = "Mes 1";
            hojaFlujoCaja.Cell("C8").Value = "Mes 2";
            hojaFlujoCaja.Cell("D8").Value = "Mes 3";
            hojaFlujoCaja.Cell("E8").Value = "Mes 4";
            hojaFlujoCaja.Cell("F8").Value = "Mes 5";
            hojaFlujoCaja.Cell("G8").Value = "Mes 6";

            hojaFlujoCaja.Cell("A9").Value = "Egresos por compras de contado";
            hojaFlujoCaja.Cell("A10").Value = "Egresos por compras de cr�dito";
            hojaFlujoCaja.Cell("A11").Value = "Otros egresos";

        }

        public void AgregarValoresDeEncabezadoFlujoDeCaja(DateTime FechaAnalisis)
        {
            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            decimal IngresosContado;
            decimal IngresosOtros;
            decimal IngresosCredito;
            decimal EgresosContado;
            decimal EgresosCredito;
            decimal EgresosOtros;
            decimal TotalIngresos;
            decimal TotalEgresos;

            List<EgresoModel> EgresosActuales;
            List<IngresoModel> IngresosActuales;

            char[] ColumnasExcel = { 'B', 'C', 'D', 'E', 'F', 'G' };
            for (int i = 1; i < 7; i += 1)
            {
                IngresosActuales = flujoDeCajaHandler.ObtenerIngresosMes("Mes " + i, FechaAnalisis);
                Console.WriteLine("Count: " + IngresosActuales.Count);
                EgresosActuales = flujoDeCajaHandler.ObtenerEgresosMes("Mes " + i, FechaAnalisis);

                IngresosContado = FlujoCajaService.CalcularIngresosTipo("contado", IngresosActuales);
                IngresosOtros = FlujoCajaService.CalcularIngresosTipo("otros", IngresosActuales);
                IngresosCredito = FlujoCajaService.CalcularIngresosTipo("credito", IngresosActuales);
                TotalIngresos = IngresosContado + IngresosOtros + IngresosCredito;

                EgresosContado = FlujoCajaService.CalcularEgresosTipo("contado", EgresosActuales);
                EgresosCredito = FlujoCajaService.CalcularEgresosTipo("credito", EgresosActuales);
                EgresosOtros = FlujoCajaService.CalcularEgresosTipo("otros", EgresosActuales);
                TotalEgresos = EgresosContado + EgresosCredito;


                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "4").Value = IngresosContado;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "5").Value = IngresosCredito;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "6").Value = IngresosOtros;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "7").Value = TotalIngresos;

                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "9").Value = EgresosContado;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "10").Value = EgresosCredito;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "11").Value = EgresosOtros;



            }

        }

        public void AgregarValoresDeGastosFijos(DateTime FechaAnalisis)
        {
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            List<GastoFijoModel> gastosFijos = gastoFijoHandler.ObtenerGastosFijos(FechaAnalisis);
            for (int i = 0; i < gastosFijos.Count; i += 1)
            {
                hojaFlujoCaja.Cell("A" + (i + 12)).Value = gastosFijos[i].Nombre;
                hojaFlujoCaja.Cell("B" + (i + 12)).Value = gastosFijos[i].Monto;
                hojaFlujoCaja.Cell("C" + (i + 12)).Value = gastosFijos[i].Monto;
                hojaFlujoCaja.Cell("D" + (i + 12)).Value = gastosFijos[i].Monto;
                hojaFlujoCaja.Cell("E" + (i + 12)).Value = gastosFijos[i].Monto;
                hojaFlujoCaja.Cell("F" + (i + 12)).Value = gastosFijos[i].Monto;
                hojaFlujoCaja.Cell("G" + (i + 12)).Value = gastosFijos[i].Monto;
            }
            hojaFlujoCaja.Cell("A" + (gastosFijos.Count + 12)).Value = "Total Egresos";
            hojaFlujoCaja.Cell("B" + (gastosFijos.Count + 12)).FormulaA1 = "SUM(B" + (gastosFijos.Count + 11) + ":B9)";
            hojaFlujoCaja.Cell("C" + (gastosFijos.Count + 12)).FormulaA1 = "SUM(C" + (gastosFijos.Count + 11) + ":C9)";
            hojaFlujoCaja.Cell("D" + (gastosFijos.Count + 12)).FormulaA1 = "SUM(D" + (gastosFijos.Count + 11) + ":D9)";
            hojaFlujoCaja.Cell("E" + (gastosFijos.Count + 12)).FormulaA1 = "SUM(E" + (gastosFijos.Count + 11) + ":E9)";
            hojaFlujoCaja.Cell("F" + (gastosFijos.Count + 12)).FormulaA1 = "SUM(F" + (gastosFijos.Count + 11) + ":F9)";
            hojaFlujoCaja.Cell("G" + (gastosFijos.Count + 12)).FormulaA1 = "SUM(G" + (gastosFijos.Count + 11) + ":G9)";
        }

        public void AgregarFlujoMensual(DateTime FechaAnalisis) {
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            int NumeroCeldaTotalEgresos = gastoFijoHandler.ObtenerGastosFijos(FechaAnalisis).Count+13;

            hojaFlujoCaja.Cell("A"+NumeroCeldaTotalEgresos).Value = "Flujo Mensual";
            hojaFlujoCaja.Cell("B" + NumeroCeldaTotalEgresos).FormulaA1 = "B"+(NumeroCeldaTotalEgresos-1)+"+B7";
            hojaFlujoCaja.Cell("C" + NumeroCeldaTotalEgresos).FormulaA1 = "C" + (NumeroCeldaTotalEgresos - 1) + "+C7";
            hojaFlujoCaja.Cell("D" + NumeroCeldaTotalEgresos).FormulaA1 = "D" + (NumeroCeldaTotalEgresos - 1) + "+D7";
            hojaFlujoCaja.Cell("E" + NumeroCeldaTotalEgresos).FormulaA1 = "E" + (NumeroCeldaTotalEgresos - 1) + "+E7";
            hojaFlujoCaja.Cell("F" + NumeroCeldaTotalEgresos).FormulaA1 = "F" + (NumeroCeldaTotalEgresos - 1) + "+F7";
            hojaFlujoCaja.Cell("G" + NumeroCeldaTotalEgresos).FormulaA1 = "G" + (NumeroCeldaTotalEgresos - 1) + "+G7";
        }

        public void ReportarFlujoDeCaja(string fechaAnalisis)
        {
            hojaFlujoCaja = libro.AddWorksheet("Flujo de Caja");
            InsertarEncabezadoFlujoDeCaja();

            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            AgregarValoresDeEncabezadoFlujoDeCaja(fechaCreacionAnalisis);
            AgregarValoresDeGastosFijos(fechaCreacionAnalisis);
            AgregarFlujoMensual(fechaCreacionAnalisis);
        }


        // Inserta los valores cargados del encabezado de la hoja de an�lisis de rentabilidad
        public void AgregarValoresDeEncabezado(DateTime fechaCreacionAnalisis)
        {
            // a�ade nombre del negocio
            Handler handler = new Handler();
            string nombreNegocio = handler.obtenerNombreNegocio(fechaCreacionAnalisis);

            // a�ade estado del negocio
            AnalisisHandler analisisHandler = new AnalisisHandler();
            AnalisisModel analisis = analisisHandler.ObtenerUnAnalisis(fechaCreacionAnalisis);
            string estadoNegocioEnAnalisis = analisis.Configuracion.EstadoNegocio == true ? "En marcha" : "No iniciado";
            hojaAnalisisRentabilidad.Cell("B2").Value = $"{nombreNegocio}{Environment.NewLine}{estadoNegocioEnAnalisis}";

            // a�ade fecha de creaci�n del an�lisis
            hojaAnalisisRentabilidad.Cell("B3").Value = analisis.FechaCreacion.ToString("dd/MMM/yyyy", new CultureInfo("es-Es"));

            // a�ade la ganancia mensual y los gastos fijos mensuales
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            hojaAnalisisRentabilidad.Cell("B5").Value = analisis.GananciaMensual;
            hojaAnalisisRentabilidad.Cell("B6").Value = $"{gastoFijoHandler.obtenerTotalAnual(fechaCreacionAnalisis) / 12}";
        }

        // Agrega los valores (que no son f�rmulas) que posee cada producto a la hoja de an�lisis de rentabilidad.
        // Detalle: El costo variable s� contempla una f�rmula debido a la comisi�n de ventas.
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

                // B Porcentaje de ventas
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = $"{productos[productoActual].PorcentajeDeVentas}%";
                hojaAnalisisRentabilidad.Cell(celdaActual).CellRight();
                celdaActual = ++columnaActual + filaActual.ToString();

                // C Comisi�n de ventas
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = $"{productos[productoActual].ComisionDeVentas}%";
                celdaActual = ++columnaActual + filaActual.ToString();

                // D Precio
                hojaAnalisisRentabilidad.Cell(celdaActual).Value = productos[productoActual].Precio;
                celdaActual = ++columnaActual + filaActual.ToString();

                // E Costo Variable
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = @$"
                    {productos[productoActual].CostoVariable}+{ComisionVentasRentabilidad}{filaActual}*{PrecioRentabilidad}{filaActual}"; // costoVariable + precio * comisionVentas
                celdaActual = ++columnaActual + filaActual.ToString();

                // itera al siguiente producto
                columnaActual = 'A';
                ++filaActual;
                ++productoActual;
            }
        }

        // Agrega las formulas para cada producto a la hoja de an�lisis de rentabilidad.
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
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"{PrecioRentabilidad}{filaActual}-{CostoVariableRentabilidad}{filaActual}"; // Precio - costo variable
                celdaActual = ++columnaActual + filaActual.ToString();

                // G Margen ponderado
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"{PorcentajeVentasRentabilidad}{filaActual}*{MargenRentabilidad}{filaActual}"; // %Ventas * margen
                celdaActual = ++columnaActual + filaActual.ToString();

                // H Punto de equilibrio Unidad
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"B6 / ({PrecioRentabilidad}{filaActual}-{CostoVariableRentabilidad}{filaActual})"; // gastosFijos / (precio - costoVariable)
                celdaActual = ++columnaActual + filaActual.ToString();

                // I Punto de equilibrio Monto
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"{PrecioRentabilidad}{filaActual}*{PtoEquilibrioUnidadRentabilidad}{filaActual}"; // (precio * puntoEquilibrioUnidad)
                celdaActual = ++columnaActual + filaActual.ToString();

                // J Meta de ventas Unidad
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"(B6+B5)/{MargenPonderadoRentabilidad}{filaTotales}*{PorcentajeVentasRentabilidad}{filaActual}"; // (gastosFijoMensuales+gananciaMensual)/totalMargenPonderado*%Ventas
                celdaActual = ++columnaActual + filaActual.ToString();

                // K Meta de ventas Monto
                hojaAnalisisRentabilidad.Cell(celdaActual).FormulaA1 = $"{PrecioRentabilidad}{filaActual} * {MetaVentasUnidadRentabilidad}{filaActual}"; // precio * metaVentasUnidad
                columnaActual = 'F';
                ++filaActual;
                ++productoActual;
            }
        }

        // Agrega la fila de los valores totales de la hoja de an�lisis de rentabilidad.
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

        // Agrega el formato estad�stico a cada celda que lo necesita.
        public void FormatearCeldasRentabilidad(int cantidadProductos)
        {
            // ganancia mensual y gastos fijos
            hojaAnalisisRentabilidad.Range($"{PorcentajeVentasRentabilidad}5", $"{PorcentajeVentasRentabilidad}6").Style.NumberFormat.Format = "#,##0.00";

            // porcentajes y comisi�n de ventas
            hojaAnalisisRentabilidad.Range($"{PorcentajeVentasRentabilidad}{9}", $"{ComisionVentasRentabilidad}{8 + cantidadProductos + 1}").SetDataType(XLDataType.Number);
            hojaAnalisisRentabilidad.Range($"{PorcentajeVentasRentabilidad}{9}", $"{ComisionVentasRentabilidad}{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#.00%";

            // columna precio
            hojaAnalisisRentabilidad.Range($"{PrecioRentabilidad}9", $"{PrecioRentabilidad}{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#,##0.00";
            
            // rango: desde costo variable a meta de ventas.
            hojaAnalisisRentabilidad.Range($"{CostoVariableRentabilidad}9", $"{MetaVentasMontoRentabilidad}{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#,##0.00";
        }

        // Agrega estilos (decoraci�n visual) a la hoja de an�lisis de rentabilidad
        public void A�adirEstiloRentabilidad(string celdaFinal)
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