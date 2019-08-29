Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Valorizacion_y_Custodia_Custodia_frmReporteServiciosBBH
    Inherits BasePage
    Dim oUtil As New UtilDM

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                tbFechaInicio.Text = oUtil.RetornarFechaSistema
                tbFechaFin.Text = oUtil.RetornarFechaSistema            
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Try
            Dim strPortafolio As String = ""
            Dim decFechaIni As Decimal = 0
            Dim decFechaFin As Decimal = 0
            Dim ds As DataSet
            Try
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
                    ds = New LocacionBBHBM().ComisionCustodioTrasacciones(strPortafolio, decFechaIni, decFechaFin)
                    ExportarExcel("ComisionCustodioTrans", ds)
                Else
                    AlertaJS("La fecha de inicio no puede ser mayor que la fecha de fin...!")
                End If
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Imprimir")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la Página")
        End Try
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Public Sub CargarCombos()        
        Try
            Dim tablaPortafolio As New DataTable
            Dim oPortafolioBM As New PortafolioBM
            tablaPortafolio = oPortafolioBM.Listar(Me.DatosRequest, "A").Tables(0)
            HelpCombo.LlenarComboBoxBusquedas(Me.ddlPortafolio, tablaPortafolio, "CodigoPortafolioSBS", "Descripcion", False)
            ddlPortafolio.SelectedValue = PORTAFOLIO_MULTIFONDOS
            ddlPortafolio.Items.RemoveAt(ddlPortafolio.SelectedIndex)
            ddlPortafolio.SelectedValue = PORTAFOLIO_ADMINISTRA
            ddlPortafolio.Items.RemoveAt(ddlPortafolio.SelectedIndex)
            ddlPortafolio.SelectedIndex = 0
        Catch ex As Exception
            Throw ex
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
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "SBBH_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls" 'CMB OT 62087 20110427 REQ 6
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
            oSheet.Name = oSheet.Name & ddlPortafolio.SelectedValue.Substring(ddlPortafolio.SelectedValue.Length - 1, 1)
            oCells = oSheet.Cells
            oCells(9, 4) = tbFechaInicio.Text
            oCells(9, 6) = tbFechaFin.Text
            oCells(11, 3) = ddlPortafolio.SelectedValue
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oSheet.Name = oSheet.Name & ddlPortafolio.SelectedValue.Substring(ddlPortafolio.SelectedValue.Length - 1, 1)
            oCells = oSheet.Cells
            'Título de Excel
            oCells(7, 1) = oCells(7, 1).Value & tbFechaInicio.Text & " al " & tbFechaFin.Text
            'Output Data
            n = 12
            For Each dr In dt.Rows
                n2 = n + 1
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                oCells(n, 2) = dr("Mercado")
                If dr("CodigoLocacion") = "13" Then
                    oCells(n, 4) = dr("Valor")
                    oCells(n, 6) = dr("Tasa_Custodio")
                    oCells(n, 7) = "=+D" & n & "*F" & n
                Else
                    oCells(n, 3) = dr("Valor")
                    oCells(n, 5) = dr("Tasa_Custodio")
                End If
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            dt = ds.Tables(1)
            n = n + 6
            For Each dr In dt.Rows
                n2 = n + 1
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                oCells(n, 2) = dr("Mercado")
                oCells(n, 3) = dr("Valor")
                oCells(n, 4) = dr("Precio_Trans")
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            'Save in a temporary file
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            'Quit Excel and thoroughly deallocate everything
            EjecutarJS("window.open('" & sFile.Replace("\", "\\") & "')")
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
