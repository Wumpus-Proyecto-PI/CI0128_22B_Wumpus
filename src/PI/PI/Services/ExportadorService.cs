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
    // Servicio encargado de exportar el libro de reporte con la hoja de análisis de rentabilidad y del flujo de caja
    public class ExportadorService
    {
        // Archivo xlsx
        XLWorkbook libro = new XLWorkbook();
        IXLWorksheet? hojaAnalisisRentabilidad;
        IXLWorksheet? hojaFlujoCaja;

        // Una columna del libro a cada caracteristica de un producto para el análisis de rentabilidad.
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

        // Invoca la creación del archivo xlsx y lo retorna en memoria.
        public MemoryStream obtenerReporte(string fechaAnalisis)
        {
            ReportarAnalisisDeRentabilidad(fechaAnalisis);
            ReportarFlujoDeCaja(fechaAnalisis);
            using (MemoryStream stream = new MemoryStream())
            {
                libro.SaveAs(stream);
                return stream;
            }
        }

        // Crea la hoja referente al análisis de rentabilidad y todos sus valores.
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
            FormatearCeldasRentabilidad(cantidadProductos); 
            AñadirEstiloRentabilidad($"K{cantidadProductos + 8 + 1}"); // 8: filas anteriores + 1 fila del total.

            hojaAnalisisRentabilidad.Columns().AdjustToContents();  // ajusta el ancho de la columna al contenido de la celda.
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
            hojaAnalisisRentabilidad.Cell($"{NombreProductoRentabilidad}8").Value = "Nombre de producto";
            hojaAnalisisRentabilidad.Cell($"{PrecioRentabilidad}8").Value = "Precio";
            hojaAnalisisRentabilidad.Cell($"{PorcentajeVentasRentabilidad}8").Value = "Porcentaje de ventas";
            hojaAnalisisRentabilidad.Cell($"{CostoVariableRentabilidad}8").Value = "Costo variable";
            hojaAnalisisRentabilidad.Cell($"{ComisionVentasRentabilidad}8").Value = "Comisión";
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
        // Encargado de insertar en la hoja Flujo de Caja, los diferentes títulos de la hoja
        public void InsertarTitulosFlujoDeCaja() {
            hojaFlujoCaja.Cell("A1").Value = "Flujo de caja";

            hojaFlujoCaja.Cell("A3").Value = "Ingresos";
            hojaFlujoCaja.Cell("B3").Value = "Mes 1";
            hojaFlujoCaja.Cell("C3").Value = "Mes 2";
            hojaFlujoCaja.Cell("D3").Value = "Mes 3";
            hojaFlujoCaja.Cell("E3").Value = "Mes 4";
            hojaFlujoCaja.Cell("F3").Value = "Mes 5";
            hojaFlujoCaja.Cell("G3").Value = "Mes 6";

            hojaFlujoCaja.Cell("A4").Value = "Ingresos por ventas de contado";
            hojaFlujoCaja.Cell("A5").Value = "Ingresos por ventas a crédito";
            hojaFlujoCaja.Cell("A6").Value = "Otros ingresos";
            hojaFlujoCaja.Cell("A7").Value = "Total ingresos";

            hojaFlujoCaja.Cell("A9").Value = "Egresos";
            hojaFlujoCaja.Cell("B9").Value = "Mes 1";
            hojaFlujoCaja.Cell("C9").Value = "Mes 2";
            hojaFlujoCaja.Cell("D9").Value = "Mes 3";
            hojaFlujoCaja.Cell("E9").Value = "Mes 4";
            hojaFlujoCaja.Cell("F9").Value = "Mes 5";
            hojaFlujoCaja.Cell("G9").Value = "Mes 6";

            hojaFlujoCaja.Cell("A10").Value = "Egresos por compras de contado";
            hojaFlujoCaja.Cell("A11").Value = "Egresos por compras de crédito";
            hojaFlujoCaja.Cell("A12").Value = "Otros egresos";
            hojaFlujoCaja.Cell("A13").Value = "Inversión inicial";

        }

        // Encargado de ingresar los diferentes valores numéricos referentes a los ingresos y egresos sin tomar en cuanta los gastos fijos
        public void AgregarValoresNumericosIngresosEgresos(DateTime FechaAnalisis)
        {
            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            decimal IngresosContado;
            decimal IngresosOtros;
            decimal IngresosCredito;
            decimal EgresosContado;
            decimal EgresosCredito;
            decimal EgresosOtros;
            decimal TotalIngresos;

            List<EgresoModel> EgresosActuales;
            List<IngresoModel> IngresosActuales;
            List<MesModel> MesesAnalisis = flujoDeCajaHandler.ObtenerMeses(FechaAnalisis);

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

                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "4").Value = IngresosContado;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "5").Value = IngresosCredito;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "6").Value = IngresosOtros;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "7").Value = TotalIngresos;

                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "10").Value = EgresosContado;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "11").Value = EgresosCredito;
                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "12").Value = EgresosOtros;

                hojaFlujoCaja.Cell("" + ColumnasExcel[i - 1] + "13").Value = MesesAnalisis[i-1].InversionPorMes;
            }

        }

        // Agrega a la hoja de Flujo de Caja, los diferentes gastos fijos y sus atributos.
        public void AgregarValoresDeGastosFijos(DateTime FechaAnalisis)
        {
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            List<GastoFijoModel> gastosFijos = gastoFijoHandler.ObtenerGastosFijos(FechaAnalisis);

            decimal divisor = 12;
            for (int i = 0; i < gastosFijos.Count; i += 1)
            {
                hojaFlujoCaja.Cell("A" + (i + 14)).Value = gastosFijos[i].Nombre;
                hojaFlujoCaja.Cell("B" + (i + 14)).Value = gastosFijos[i].Monto/divisor;
                hojaFlujoCaja.Cell("C" + (i + 14)).Value = gastosFijos[i].Monto/divisor;
                hojaFlujoCaja.Cell("D" + (i + 14)).Value = gastosFijos[i].Monto/divisor;
                hojaFlujoCaja.Cell("E" + (i + 14)).Value = gastosFijos[i].Monto/divisor;
                hojaFlujoCaja.Cell("F" + (i + 14)).Value = gastosFijos[i].Monto/divisor;
                hojaFlujoCaja.Cell("G" + (i + 14)).Value = gastosFijos[i].Monto/divisor;
            }
            hojaFlujoCaja.Cell("A" + (gastosFijos.Count + 14)).Value = "Total Egresos";
            hojaFlujoCaja.Cell("B" + (gastosFijos.Count + 14)).FormulaA1 = "SUM(B" + (gastosFijos.Count + 13) + ":B10)";
            hojaFlujoCaja.Cell("C" + (gastosFijos.Count + 14)).FormulaA1 = "SUM(C" + (gastosFijos.Count + 13) + ":C10)";
            hojaFlujoCaja.Cell("D" + (gastosFijos.Count + 14)).FormulaA1 = "SUM(D" + (gastosFijos.Count + 13) + ":D10)";
            hojaFlujoCaja.Cell("E" + (gastosFijos.Count + 14)).FormulaA1 = "SUM(E" + (gastosFijos.Count + 13) + ":E10)";
            hojaFlujoCaja.Cell("F" + (gastosFijos.Count + 14)).FormulaA1 = "SUM(F" + (gastosFijos.Count + 13) + ":F10)";
            hojaFlujoCaja.Cell("G" + (gastosFijos.Count + 14)).FormulaA1 = "SUM(G" + (gastosFijos.Count + 13) + ":G10)";
        }

        // Agrega a la tabla Flujo de Caja los valores  y fórmulas referentes al flujo mensual.
        public void AgregarFlujoMensual(DateTime FechaAnalisis) {
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            int NumeroCeldaTotalEgresos = gastoFijoHandler.ObtenerGastosFijos(FechaAnalisis).Count+16;

            hojaFlujoCaja.Cell("A"+NumeroCeldaTotalEgresos).Value = "Flujo Mensual";
            hojaFlujoCaja.Cell("B" + NumeroCeldaTotalEgresos).FormulaA1 = "B7-"+"B" + (NumeroCeldaTotalEgresos - 2);
            hojaFlujoCaja.Cell("C" + NumeroCeldaTotalEgresos).FormulaA1 = "C7-" + "C" + (NumeroCeldaTotalEgresos - 2);
            hojaFlujoCaja.Cell("D" + NumeroCeldaTotalEgresos).FormulaA1 = "D7-" + "D" + (NumeroCeldaTotalEgresos - 2);
            hojaFlujoCaja.Cell("E" + NumeroCeldaTotalEgresos).FormulaA1 = "E7-" + "E" + (NumeroCeldaTotalEgresos - 2);
            hojaFlujoCaja.Cell("F" + NumeroCeldaTotalEgresos).FormulaA1 = "F7-" + "F" + (NumeroCeldaTotalEgresos - 2);
            hojaFlujoCaja.Cell("G" + NumeroCeldaTotalEgresos).FormulaA1 = "G7-" + "G" + (NumeroCeldaTotalEgresos - 2);
        }

        // Encargado de agregar estilo a la hoja de Flujo de Caja
        public void AgregarEstiloFlujoDeCaja(DateTime FechaAnalisis) {
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            int FilaFlujoMensual = gastoFijoHandler.ObtenerGastosFijos(FechaAnalisis).Count + 16;
            int FilaTotalEgresos = FilaFlujoMensual-2;
            char[] ColumnasExcel = { 'B', 'C', 'D', 'E', 'F', 'G' };
            for (int i = 0; i < 6; i+=1) {
                hojaFlujoCaja.Column(ColumnasExcel[i]).Style.NumberFormat.Format = "#,##0.00";
            }

            hojaFlujoCaja.Column("A").Width = 30;
            hojaFlujoCaja.Range("A3","G3").Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            hojaFlujoCaja.Range("A3", "G3").Style.Font.FontColor = XLColor.White;
            hojaFlujoCaja.Cell("A3").Style.Font.SetBold();
            hojaFlujoCaja.Cell("A3").Style.Font.SetFontSize(12);
            hojaFlujoCaja.Range("A7", "G7").Style.Fill.BackgroundColor = XLColor.LightGray;
            hojaFlujoCaja.Cell("A7").Style.Font.SetBold();

            hojaFlujoCaja.Range("A9", "G9").Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            hojaFlujoCaja.Range("A9", "G9").Style.Font.FontColor = XLColor.White;
            hojaFlujoCaja.Cell("A9").Style.Font.SetBold();
            hojaFlujoCaja.Cell("A9").Style.Font.SetFontSize(12);
            hojaFlujoCaja.Range("A"+ FilaTotalEgresos, "G"+ FilaTotalEgresos).Style.Fill.BackgroundColor = XLColor.LightGray;
            hojaFlujoCaja.Cell("A"+FilaTotalEgresos).Style.Font.SetBold();

            hojaFlujoCaja.Range("A" + FilaFlujoMensual, "G" + FilaFlujoMensual).Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            hojaFlujoCaja.Range("A" + FilaFlujoMensual, "G" + FilaFlujoMensual).Style.Font.FontColor = XLColor.White;
            hojaFlujoCaja.Cell("A"+FilaFlujoMensual).Style.Font.SetBold();
        }

        // Encargado de generar el reporte de flujo de caja
        public void ReportarFlujoDeCaja(string fechaAnalisis)
        {
            hojaFlujoCaja = libro.AddWorksheet("Flujo de Caja");
            InsertarTitulosFlujoDeCaja();

            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            AgregarValoresNumericosIngresosEgresos(fechaCreacionAnalisis);
            AgregarValoresDeGastosFijos(fechaCreacionAnalisis);
            AgregarFlujoMensual(fechaCreacionAnalisis);
            AgregarEstiloFlujoDeCaja(fechaCreacionAnalisis);
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
        // Detalle: El costo variable sí contempla una fórmula debido a la comisión de ventas.
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

                // C Comisión de ventas
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
        }

        // Agrega el formato estadístico a cada celda que lo necesita.
        public void FormatearCeldasRentabilidad(int cantidadProductos)
        {
            // ganancia mensual y gastos fijos
            hojaAnalisisRentabilidad.Range($"{PorcentajeVentasRentabilidad}5", $"{PorcentajeVentasRentabilidad}6").Style.NumberFormat.Format = "#,##0.00";

            // porcentajes y comisión de ventas
            hojaAnalisisRentabilidad.Range($"{PorcentajeVentasRentabilidad}{9}", $"{ComisionVentasRentabilidad}{8 + cantidadProductos + 1}").SetDataType(XLDataType.Number);
            hojaAnalisisRentabilidad.Range($"{PorcentajeVentasRentabilidad}{9}", $"{ComisionVentasRentabilidad}{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#.00%";

            // columna precio
            hojaAnalisisRentabilidad.Range($"{PrecioRentabilidad}9", $"{PrecioRentabilidad}{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#,##0.00";
            
            // rango: desde costo variable a meta de ventas.
            hojaAnalisisRentabilidad.Range($"{CostoVariableRentabilidad}9", $"{MetaVentasMontoRentabilidad}{8 + cantidadProductos + 1}").Style.NumberFormat.Format = "#,##0.00";
        }

        // Agrega estilos (decoración visual) a la hoja de análisis de rentabilidad
        public void AñadirEstiloRentabilidad(string celdaFinal)
        {
            // Agrego estilo a la primer celda del título.
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.FontSize = 16;
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Cell("A1").Style.Font.SetFontColor(XLColor.FromHtml("#4472C4"));
            hojaAnalisisRentabilidad.Cell("A2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            hojaAnalisisRentabilidad.Range("A1:K1").Merge();
            hojaAnalisisRentabilidad.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Agrego estilo a los títulos de la tabla.
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            hojaAnalisisRentabilidad.Range("A8", "K8").Style.Font.SetFontColor(XLColor.FromHtml("#FFFFFF"));

            // Agrego borde a la ganancia mensual y gastos fijos.
            hojaAnalisisRentabilidad.Cell("A5").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("A6").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("B5").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("B5").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("B6").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Cell("B6").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A5", "B6").Style.Font.SetBold();

            // Agrego bordes a la tabla.
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("A8", celdaFinal).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            // Agrego borde al punto de equilibrio y meta de ventas.
            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            hojaAnalisisRentabilidad.Range("H7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // celdaFinal = K{filaTotales}. Extraigo todo menos la columna para obtener la fila de los rubros totales.
            int filaTotales = Convert.ToInt32(celdaFinal.Substring(1));
            hojaAnalisisRentabilidad.Range($"A{filaTotales}", $"K{filaTotales}").Style.Font.SetBold();
            hojaAnalisisRentabilidad.Range($"A{filaTotales}", $"K{filaTotales}").Style.Fill.BackgroundColor = XLColor.FromHtml("#8E8E8E");
            hojaAnalisisRentabilidad.Range($"A{filaTotales}", $"K{filaTotales}").Style.Font.SetFontColor(XLColor.FromHtml("#FFFFFF"));
        }
    }
}