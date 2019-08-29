Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports ParametrosSIT
Imports System.IO

Partial Class Modulos_Tesoreria_Encaje_frmRepInteresDividendos
    Inherits BasePage


#Region "Variables"
    Dim oUtil As New UtilDM
#End Region

#Region "/* Eventos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                Me.tbFechaInicio.Text = oUtil.RetornarFechaSistema
                Me.tbFechaFin.Text = oUtil.RetornarFechaSistema
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim strPortafolio As String = ""
            Dim decFechaIni As Decimal = 0
            Dim decFechaFin As Decimal = 0
            Dim ds As DataSet
            strPortafolio = ddlPortafolio.SelectedValue
            If Trim(tbFechaInicio.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de inicio...!")
                Exit Sub
            End If
            If Trim(tbFechaFin.Text) = "" Then
                AlertaJS("Debe ingresar la fecha de fin...!")
                Exit Sub
            End If
            decFechaIni = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
            decFechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
            If decFechaIni <= decFechaFin Then
                ds = New ReporteGestionBM().DetInteresesDividendos(strPortafolio, decFechaIni, decFechaFin)
                ExportarExcel("RepInteresDividendos", ds)
            Else
                AlertaJS("La fecha de inicio no puede ser mayor que la fecha de fin...!")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Imprimir")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Public Sub CargarCombos()
        Dim tablaPortafolio As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        Try
            tablaPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBoxBusquedas(Me.ddlPortafolio, tablaPortafolio, "CodigoPortafolio", "Descripcion", False)
            ddlPortafolio.SelectedIndex = 0
        Catch ex As Exception
            Throw ex
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub ExportarExcel(ByVal Archivo As String, ByVal ds As DataSet)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim sFile As String = "", sTemplate As String = ""
        Dim iRow As Integer = 0, iCol As Integer = 0, n As Long = 0, n2 As Long = 0
        Dim oExcel As Excel.Application
        Dim dr As DataRow
        Dim oCells As Excel.Range
        Dim dt As DataTable
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RID_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls" 'CMB OT 62087 20110427 REQ 6
            dt = ds.Tables(0)
            sTemplate = RutaPlantillas() & "\Plantilla" & Archivo & ".xls"
            oExcel.DisplayAlerts = False
            oExcel.Visible = False
            'Start a new workbook
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            'Título de Excel
            oCells(4, 2) = ddlPortafolio.Items(ddlPortafolio.SelectedValue).Text.Trim
            oCells(5, 2) = tbFechaInicio.Text
            oCells(6, 2) = tbFechaFin.Text

            'Output Data 
            n = 9
            Dim ImporteOrigen As Double, Intereses = 0, Dividendos = 0
            For Each dr In dt.Rows
                oCells(n, 1) = dr("TipoInstrumento")
                oCells(n, 2) = dr("CodigoMnemonico")
                oCells(n, 3) = dr("Condicion")
                oCells(n, 4) = dr("Moneda")
                oCells(n, 5) = dr("TipoCambio")
                oCells(n, 6) = dr("FechaOperacion")
                oCells(n, 7) = dr("ImporteOrigen")
                oCells(n, 8) = dr("Intereses")
                oCells(n, 9) = dr("Dividendos")

                ImporteOrigen += ToNullDouble(dr("ImporteOrigen"))
                Intereses += ToNullDouble(dr("Intereses"))
                Dividendos += ToNullDouble(dr("Dividendos"))
                n = n + 1
            Next

            oCells(n, 1) = "TOTAL"
            oCells(n, 7) = ImporteOrigen
            oCells(n, 8) = Intereses
            oCells(n, 9) = Dividendos
            oSheet.Range("A" & n & ":I" & n).Font.Bold = True
            'Save in a temporary file
            oSheet.SaveAs(sFile)
            oBook.Close()
            Dim path__1 As String = sFile.Replace("\", "\\")
            Dim name As String = Path.GetFileName(path__1)
            Response.AppendHeader("content-disposition", Convert.ToString("attachment; filename=") & name)
            Response.ContentType = "Application/msword"
            Response.WriteFile(path__1)
            Response.[End]()
        Catch ex As Exception
            Throw ex
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

#End Region

End Class
