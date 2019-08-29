Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.Data
Imports ParametrosSIT
Imports System.IO
Imports System.Globalization
Partial Class Modulos_Gestion_Reportes_frmVectorVariacion
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oVectorPrecio As New VectorPrecioBM
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        lblTitulo.InnerText = "Vector Variación"
        lblFechaDsc1.InnerText = "Fecha"
        divFechaDsc1.Attributes.Add("class", "col-md-3")
        If (Not IsPostBack) Then
            tbFechaValoracion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaValoracion2.Text = tbFechaValoracion.Text
        End If
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            GenerarReporte()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim sFile As String, sTemplate As String
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oSheetTemplate As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim Indice As Integer = 1
        Dim i As Int32 = 1
        Dim exitDo As Boolean = False
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RV_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaVectorVariacion.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            Dim dteFechaHoy As Date = tbFechaValoracion.Text
            Dim dteFechaAyer As Date = dteFechaHoy.AddDays(-1)
            Dim dsvVecVar As DataSet = New ReporteGestionBM().Reporte_VectorVariacion(UIUtility.ConvertirFechaaDecimal(dteFechaHoy), UIUtility.ConvertirFechaaDecimal(dteFechaAyer))
            'Tabla 1 RESUMEN DE PRECIOS
            oSheet = oBook.Sheets(1)
            oCells = oSheet.Cells
            oCells(7, 2) = "FECHA: " + tbFechaValoracion.Text
            oCells(2, 12) = Now.ToLongDateString
            oCells(3, 12) = Usuario
            Dim dtResumen As DataTable = dsvVecVar.Tables(0)
            Dim FilaInicial As Integer = 11
            For Each dr In dtResumen.Rows
                oCells(FilaInicial, 2) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("Fecha")))
                oCells(FilaInicial, 3) = dr("CodigoTipoInstrumento")
                oCells(FilaInicial, 4) = dr("CodigoEntidad")
                oCells(FilaInicial, 5) = dr("CodigoMnemonico")
                oCells(FilaInicial, 6) = dr("SerieSAFP")
                oCells(FilaInicial, 7) = dr("CodigoSBS")
                oCells(FilaInicial, 8) = dr("ValorPrecio")
                oCells(FilaInicial, 9) = dr("Signo")
                oCells(FilaInicial, 10) = dr("Variacion")
                oCells(FilaInicial, 11) = dr("VariacionPorcentual")
                oCells(FilaInicial, 12) = dr("CodigoISIN")
                oCells(FilaInicial, 13) = dr("Sinonimo")
                oCells(FilaInicial, 14) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaVencimiento")))
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Reporte adicionando Pago dividendo en hoja: 1_Resumen_Diferencia | 21/05/18
                oCells(FilaInicial, 19) = dr("PagaDividendo")
                oCells(FilaInicial, 20) = dr("FrecuenciaPago")
                oCells(FilaInicial, 21) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("UltimaFechaPagoDividendo")))
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Reporte adicionando  Pago dividendo en hoja: 1_Resumen_Diferencia | 21/05/18
                oCells(FilaInicial, 22) = dr("EntidadExt")
                FilaInicial += 1
            Next
            'Tabla 2 DIFERENCIA DE PRECIOS
            oSheet = oBook.Sheets(2)
            oCells = oSheet.Cells
            oCells(7, 2) = "FECHA: " + tbFechaValoracion.Text
            oCells(2, 12) = Now.ToLongDateString
            oCells(3, 12) = Usuario
            Dim dtDiferencia As DataTable = dsvVecVar.Tables(1)
            FilaInicial = 11
            For Each dr In dtDiferencia.Rows
                oCells(FilaInicial, 2) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("Fecha")))
                oCells(FilaInicial, 3) = dr("CodigoTipoInstrumento")
                oCells(FilaInicial, 4) = dr("CodigoEntidad")
                oCells(FilaInicial, 5) = dr("CodigoMnemonico")
                oCells(FilaInicial, 6) = dr("SerieSAFP")
                oCells(FilaInicial, 7) = dr("CodigoSBS")
                oCells(FilaInicial, 8) = dr("ValorPrecio")
                oCells(FilaInicial, 9) = dr("Signo")
                oCells(FilaInicial, 10) = dr("Variacion")
                oCells(FilaInicial, 11) = dr("VariacionPorcentual")
                oCells(FilaInicial, 12) = dr("CodigoISIN")
                oCells(FilaInicial, 13) = dr("Sinonimo")
                oCells(FilaInicial, 14) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaVencimiento")))
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Reporte adicionando Pago dividendo en hoja: 2_Vector_Diferencia  | 21/05/18
                oCells(FilaInicial, 19) = dr("PagaDividendo")
                oCells(FilaInicial, 20) = dr("FrecuenciaPago")
                oCells(FilaInicial, 21) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("UltimaFechaPagoDividendo")))
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Reporte adicionando Pago dividendo en hoja: 2_Vector_Diferencia | 21/05/18
                oCells(FilaInicial, 22) = dr("EntidadExt")
                FilaInicial += 1
            Next
            'Tabla 3 LISTADO DE PRECIOS
            oSheet = oBook.Sheets(3)
            oCells = oSheet.Cells
            oCells(7, 2) = "FECHA: " + tbFechaValoracion.Text
            oCells(2, 11) = Now.ToLongDateString
            oCells(3, 11) = Usuario
            Dim dtPrecios As DataTable = dsvVecVar.Tables(2)
            FilaInicial = 11
            For Each dr In dtPrecios.Rows
                oCells(FilaInicial, 2) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("Fecha")))
                oCells(FilaInicial, 3) = dr("CodigoTipoInstrumento")
                oCells(FilaInicial, 4) = dr("CodigoEntidad")
                oCells(FilaInicial, 5) = dr("CodigoMnemonico")
                oCells(FilaInicial, 6) = dr("SerieSAFP")
                oCells(FilaInicial, 7) = dr("CodigoSBS")
                oCells(FilaInicial, 8) = dr("ValorPrecio")
                oCells(FilaInicial, 9) = dr("Signo")
                oCells(FilaInicial, 10) = dr("Variacion")
                oCells(FilaInicial, 11) = dr("CodigoISIN")
                oCells(FilaInicial, 16) = dr("EntidadExt")
                FilaInicial += 1
            Next
            'Tabla 4 COMPOSICION DE CARTERA
            oSheet = oBook.Sheets(4)
            oCells = oSheet.Cells
            oCells(7, 2) = "FECHA: " + tbFechaValoracion.Text
            oCells(2, 12) = Now.ToLongDateString
            oCells(3, 12) = Usuario
            Dim dtCartera As DataTable = dsvVecVar.Tables(3)
            FilaInicial = 11
            For Each dr In dtCartera.Rows
                oCells(FilaInicial, 2) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("Fecha")))
                oCells(FilaInicial, 3) = IIf(dr("Portafolio").Equals(DBNull.Value), "", dr("Portafolio"))
                oCells(FilaInicial, 4) = dr("CodigoEmision")
                oCells(FilaInicial, 5) = dr("CodigoISIN")
                oCells(FilaInicial, 6) = "'" + dr("CodigoSBS")
                oCells(FilaInicial, 7) = IIf(dr("Emisor").Equals(DBNull.Value), "", dr("Emisor"))
                oCells(FilaInicial, 8) = dr("TipoTitulo")
                oCells(FilaInicial, 9) = dr("Sinonimo")
                oCells(FilaInicial, 10) = dr("Moneda")
                oCells(FilaInicial, 11) = dr("FechaEmision")
                oCells(FilaInicial, 12) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaVencimiento")))
                oCells(FilaInicial, 13) = dr("Unidades")
                oCells(FilaInicial, 14) = IIf(dr("ValorNominalOrigen").Equals(DBNull.Value), 0, dr("ValorNominalOrigen"))
                oCells(FilaInicial, 15) = IIf(dr("ValorNominalLocal").Equals(DBNull.Value), 0, dr("ValorNominalLocal"))
                oCells(FilaInicial, 16) = dr("Precio")
                oCells(FilaInicial, 17) = dr("TipoCambio")
                oCells(FilaInicial, 18) = dr("ValoracionMonedaLocal")
                oCells(FilaInicial, 19) = dr("ValoracionMonedaPortafolio")
                oCells(FilaInicial, 20) = dr("TIR360")
                oCells(FilaInicial, 21) = dr("TIR365")
                oCells(FilaInicial, 22) = dr("DuracionModificada")
                oCells(FilaInicial, 23) = dr("DuracionxValoracion")
                oCells(FilaInicial, 24) = dr("Tipo instrumento")
                oCells(FilaInicial, 25) = dr("Sector")
                oCells(FilaInicial, 26) = IIf(dr("Calificacion").Equals(DBNull.Value), "", dr("Calificacion"))
                FilaInicial += 1
            Next
            'Tabla 5 VENCIMIENTO DE CUOTA
            oSheet = oBook.Sheets(5)
            oCells = oSheet.Cells
            oCells(7, 2) = "FECHA: " + tbFechaValoracion.Text
            oCells(2, 10) = Now.ToLongDateString
            oCells(3, 10) = Usuario
            Dim dtVencimientos As DataTable = dsvVecVar.Tables(4)
            FilaInicial = 11
            For Each dr In dtVencimientos.Rows
                oCells(FilaInicial, 2) = dr("Portafolio")
                oCells(FilaInicial, 3) = dr("CodigoNemonico")
                oCells(FilaInicial, 4) = dr("CodigoISIN")
                oCells(FilaInicial, 5) = dr("CodigoMoneda")
                oCells(FilaInicial, 6) = UIUtility.ConvertirDecimalAStringFormatoFecha(CDec(dr("FechaPagoCupon")))
                oCells(FilaInicial, 7) = dr("MontoIntereses")
                oCells(FilaInicial, 8) = dr("Amortizacion")
                oCells(FilaInicial, 9) = dr("PagoAmortizacion")
                oCells(FilaInicial, 10) = dr("TotalPagar")
                oCells(FilaInicial, 11) = dr("TasaCupon")
                oCells(FilaInicial, 12) = dr("BaseCupon")
                oCells(FilaInicial, 13) = dr("Periodicidad")
                oCells(FilaInicial, 14) = dr("DiasPago")
                oCells(FilaInicial, 15) = dr("TipoAmortizacion")
                oCells(FilaInicial, 16) = dr("DiferenciaDias")
                oCells(FilaInicial, 17) = dr("BaseCalculoDias")
                oCells(FilaInicial, 18) = dr("CodigoIndicador")
                oCells(FilaInicial, 19) = dr("Emisor")
              
                FilaInicial += 1
            Next
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "RV_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
End Class