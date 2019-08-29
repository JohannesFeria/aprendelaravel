Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports Microsoft.Office
Imports System.Data
Imports System.Globalization
Imports System.Threading
Imports System.IO

Partial Class Modulos_Gestion_Reportes_frmDetallePosicionBancoxTipoInst
    Inherits BasePage

#Region "Variables"
    Dim oUtil As New UtilDM
#End Region

#Region "/* Metodos Personalizados */"
    Public Sub CargarCombos()
        Dim DtablaEntidad As New DataTable
        Dim oEntidadBM As New EntidadBM
        Dim oPortafolioBM As New PortafolioBM
        Try
            DtablaEntidad = oEntidadBM.ListarEntidadFinanciera(DatosRequest).Tables(0)
            HelpCombo.LlenarComboBox(ddlIntermediarios, DtablaEntidad, "CodigoEntidad", "NombreCompleto", True)
            ddlIntermediarios.SelectedIndex = 0

            Dim dt As DataTable = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ExportarExcel(ByVal Archivo As String, ByVal dt As DataTable)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim sFile As String = "", sTemplate As String = ""
        Dim iRow As Integer = 0, iCol As Integer = 0, n As Long = 0, n2 As Long = 0
        Dim oExcel As Excel.Application
        Dim dr As DataRow, ary() As Object
        Dim oCells As Excel.Range
        Dim oldCulture As CultureInfo

        oldCulture = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)

        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Archivo & "_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            sTemplate = RutaPlantillas() & "\Plantilla" & Archivo & ".xls"
            oExcel.DisplayAlerts = False
            oExcel.Visible = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            'Título de Excel
            oCells(1, 2) = ddlPortafolio.SelectedItem.Text
            oCells(3, 1) = dt.Rows(0)(0).ToString
            'Output Data 
            For iRow = 0 To dt.Rows.Count - 1
                dr = dt.Rows.Item(iRow)
                ary = dr.ItemArray
                n = iRow + 4
                n2 = n + 1
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                oCells(n, 1) = ary(1).ToString
                oCells(n, 2) = ary(2).ToString
            Next iRow
            oSheet.Rows(n2 & ":" & n2).Delete(Excel.XlDirection.xlUp)
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            If sFile <> "" Then
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
                Response.WriteFile(sFile)
                Response.End()
            End If
            Thread.CurrentThread.CurrentCulture = oldCulture
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
#Region "/* Eventos */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Try
                CargarCombos()
                tbFechaInicio.Text = oUtil.RetornarFechaSistema
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub
    Private Sub ibVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Dim strPortafolio As String = ""
        Dim strCodigoIntermediario As String = ""
        Dim decFechaIni As Decimal = 0
        Dim ds As DataSet
        Try
            strPortafolio = IIf(ddlPortafolio.SelectedValue = Constantes.M_STR_TEXTO_TODOS, "", ddlPortafolio.SelectedValue)
            strCodigoIntermediario = ddlIntermediarios.SelectedValue
            If ddlIntermediarios.SelectedValue = "" Then
                AlertaJS("Debe ingresar un Emisor")
                Exit Sub
            End If
            If Trim(tbFechaInicio.Text) = "" Then
                AlertaJS("Debe ingresar la fecha")
                Exit Sub
            End If
            decFechaIni = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
            ds = New ReporteGestionBM().DetallePosicionBancos(strPortafolio, strCodigoIntermediario, decFechaIni)
            ExportarExcel("DetallePosicionBancos", CType(ds.Tables(0), DataTable))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
#End Region

End Class
