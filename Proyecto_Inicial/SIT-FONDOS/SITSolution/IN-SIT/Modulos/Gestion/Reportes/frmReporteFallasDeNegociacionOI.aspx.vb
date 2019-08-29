Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.IO
Imports Microsoft.Office
Imports System.Data
Imports System.Globalization
Imports System.Threading

Partial Class Modulos_Gestion_Reportes_frmReporteFallasDeNegociacionOI
    Inherits BasePage

#Region "Inicializador"
    Dim oPortafolioBM As New PortafolioBM
    Dim oParametrosBM As New ParametrosGeneralesBM
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oUtilDM As New UtilDM
    Dim dsPortafolio As DataSet
    Dim dtParametro As DataTable
    Dim dtMotivos As DataTable
    Dim dtFechas As DataTable
    Dim dtData As DataTable
    Dim dtPortafolios As DataTable
#End Region

#Region "Metodos Personalizados"
    Private Sub CargarPortafolio()
        dsPortafolio = oPortafolioBM.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
        Dim dtPortafolio As DataTable = dsPortafolio.Tables(0)
        dtPortafolio.DefaultView.RowFilter = "CodigoNegocio='FOND' AND CodigoPortafolioSBS<>'" + PORTAFOLIO_MULTIFONDOS + "'"
        HelpCombo.LlenarComboBoxBusquedas(ddlPortafolio, dtPortafolio.DefaultView.Table, "CodigoPortafolioSBS", "Descripcion", True)
    End Sub

    Private Sub CargarOpcion()
        dtParametro = oParametrosBM.Listar(ParametrosSIT.OPC_REPORTE_FALLAS_OI, DatosRequest)
        HelpCombo.LlenarComboBox(ddlOpcion, dtParametro, "Valor", "Nombre", False)
    End Sub

    Private Sub CargarFechas()
        tbFechaInicio.Text = oUtilDM.RetornarFechaNegocio()
        tbFechaFin.Text = String.Format("{0:dd/MM/yyyy}", Date.Today)
    End Sub

    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim sFile As String, sTemplate As String
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim oldCulture As CultureInfo
        oldCulture = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim oDs As New DataSet
            Dim decFechaInicio As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            Dim decFechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            Dim opcion As Decimal = Val(ddlOpcion.SelectedValue)
            Dim portafolio As String = IIf(ddlPortafolio.SelectedValue = "Todos", "", ddlPortafolio.SelectedValue)
            oDs = oOrdenInversionBM.GenerarReporteDeFallasOI(decFechaInicio, decFechaFin, opcion, portafolio)

            dtFechas = oDs.Tables(0)
            dtMotivos = oDs.Tables(1)
            dtData = oDs.Tables(2)
            dtPortafolios = oDs.Tables(3)

            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RFL_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"

            Dim nColumna As Int32
            Dim nFila As Int32
            Dim nFila2 As Int32
            Dim n As Int32

            nColumna = 4
            nFila = 6
            nFila2 = 6
            n = 1
            If File.Exists(sFile) Then File.Delete(sFile)

            sTemplate = RutaPlantillas() & "\" & "PlantillaReporteFallasNegOI.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False

            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)

            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            Select Case opcion
                Case ParametrosSIT.OPC_OI_MODIFICADA
                    oCells(3, 2) = "Reporte mensual de fallas de operaciones Modificadas"
                Case ParametrosSIT.OPC_OI_ELIMINADA
                    oCells(3, 2) = "Reporte mensual de fallas de operaciones Eliminadas"
            End Select

            While n < dtFechas.Rows.Count
                oCells(5, nColumna).Select()
                oCells(5, nColumna).Copy()
                nColumna = nColumna + 1
                oCells(5, nColumna).Select()
                oCells(5, nColumna).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)

                nColumna = nColumna - 1
                oCells(6, nColumna).Select()
                oCells(6, nColumna).Copy()
                nColumna = nColumna + 1
                oCells(6, nColumna).Select()
                oCells(6, nColumna).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                n = n + 1
            End While
            nColumna = 4
            For Each drFecha As DataRow In dtFechas.Rows
                oCells(5, nColumna).value = drFecha("FechaInicio").ToString()
                nColumna = nColumna + 1
            Next
            Dim i As Integer = 0
            Dim j As Integer = 0
            nColumna = 4

            If portafolio <> "" Then
                For Each drData As DataRow In dtData.Rows
                    If drData("MotivoDesc") = dtMotivos.Rows(i)("MotivoDesc") Then
                        oCells(nFila2, 2) = drData("MotivoDesc")
                        oCells(nFila2, 3) = drData("CodigoPortafolioSBS")
                        oCells(nFila2, nColumna) = drData("Cantidad")
                        nColumna = nColumna + 1
                    Else
                        nColumna = 4
                        oSheet.Rows(nFila2 & ":" & nFila2).Copy()
                        oSheet.Rows(nFila2 + 1 & ":" & nFila2 + 1).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                        oSheet.Rows(nFila2 + 1 & ":" & nFila2 + 1).Select()
                        oSheet.Rows(nFila2 + 1 & ":" & nFila2 + 1).Copy()
                        oSheet.Rows(nFila2 + 2 & ":" & nFila2 + 2).Select()
                        oSheet.Rows(nFila2 + 2 & ":" & nFila2 + 2).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                        nFila2 = nFila2 + 1

                        oCells(nFila2, 3) = "Var %"
                        Dim nColumnaS As Integer = nColumna
                        For x As Integer = 1 To dtFechas.Rows.Count - 1
                            nColumnaS = nColumnaS + 1
                            oCells(nFila2, nColumnaS).Formula = "=IF(" & HelpExcel.LetraColumna(nColumnaS - 1) & nFila2 - 1 & " <> 0," & HelpExcel.LetraColumna(nColumnaS) & nFila2 - 1 & "/" & HelpExcel.LetraColumna(nColumnaS - 1) & nFila2 - 1 & " - 1,0)"
                            oCells(nFila2, nColumnaS).NumberFormat = "0.00%"
                        Next
                        nFila2 = nFila2 + 1
                        oSheet.Range(oCells(nFila2 - 2, 2), oCells(nFila2 - 1, 2)).Merge()
                        oCells(nFila2, nColumna) = drData("Cantidad")
                        nColumna = nColumna + 1
                        i = i + 1
                    End If
                Next
                If dtData.Rows.Count > 0 Then
                    oSheet.Rows(nFila2 & ":" & nFila2).Select()
                    oSheet.Rows(nFila2 & ":" & nFila2).Copy()
                    oSheet.Rows(nFila2 + 1 & ":" & nFila2 + 1).Select()
                    oSheet.Rows(nFila2 + 1 & ":" & nFila2 + 1).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                    nFila2 = nFila2 + 1
                    oCells(nFila2, 3) = "Var %"
                    Dim nColumnaS As Integer = 4
                    For x As Integer = 1 To dtFechas.Rows.Count - 1
                        nColumnaS = nColumnaS + 1
                        oCells(nFila2, nColumnaS).Formula = "=IF(" & HelpExcel.LetraColumna(nColumnaS - 1) & nFila2 - 1 & " <> 0," & HelpExcel.LetraColumna(nColumnaS) & nFila2 - 1 & "/" & HelpExcel.LetraColumna(nColumnaS - 1) & nFila2 - 1 & " - 1,0)"
                        oCells(nFila2, nColumnaS).NumberFormat = "0.00%"
                    Next
                    oSheet.Range(oCells(nFila2 - 1, 2), oCells(nFila2, 2)).Merge()
                End If
            Else
                nFila2 = 6
                For Each drData As DataRow In dtData.Rows
                    If drData("MotivoDesc") = dtMotivos.Rows(i)("MotivoDesc") Then
                        oCells(nFila2, 2) = drData("MotivoDesc")
                        If drData("CodigoPortafolioSBS") = dtPortafolios.Rows(j)("CodigoPortafolioSBS") Then
                            oCells(nFila, 3) = drData("CodigoPortafolioSBS")
                            oCells(nFila, nColumna) = drData("Cantidad")
                            nColumna = nColumna + 1
                        Else
                            nColumna = 4
                            oSheet.Rows(nFila & ":" & nFila).Select()
                            oSheet.Rows(nFila & ":" & nFila).Copy()
                            oSheet.Rows(nFila + 1 & ":" & nFila + 1).Select()
                            oSheet.Rows(nFila + 1 & ":" & nFila + 1).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                            nFila = nFila + 1
                            oCells(nFila, nColumna) = drData("Cantidad")
                            nColumna = nColumna + 1
                            j = j + 1
                        End If
                    Else
                        j = 0
                        nColumna = 4

                        oSheet.Rows(nFila & ":" & nFila).Select()
                        oSheet.Rows(nFila & ":" & nFila).Copy()
                        oSheet.Rows(nFila + 1 & ":" & nFila + 1).Select()
                        oSheet.Rows(nFila + 1 & ":" & nFila + 1).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                        oSheet.Rows(nFila + 1 & ":" & nFila + 1).Select()
                        oSheet.Rows(nFila + 1 & ":" & nFila + 1).Copy()
                        oSheet.Rows(nFila + 2 & ":" & nFila + 2).Select()
                        oSheet.Rows(nFila + 2 & ":" & nFila + 2).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                        oSheet.Rows(nFila + 2 & ":" & nFila + 2).Select()
                        oSheet.Rows(nFila + 2 & ":" & nFila + 2).Copy()
                        oSheet.Rows(nFila + 3 & ":" & nFila + 3).Select()
                        oSheet.Rows(nFila + 3 & ":" & nFila + 3).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)

                        nFila = nFila + 1
                        oCells(nFila, 3) = "Total"
                        Dim nColumnaT As Integer = nColumna
                        For x As Integer = 1 To dtFechas.Rows.Count
                            oCells(nFila, nColumnaT).Formula = "=SUM(" & HelpExcel.LetraColumna(nColumnaT) & nFila - 3 & ":" & HelpExcel.LetraColumna(nColumnaT) & nFila - 1 & ")"
                            nColumnaT = nColumnaT + 1
                        Next

                        nFila = nFila + 1
                        oCells(nFila, 3) = "Var %"
                        Dim nColumnaV As Integer = nColumna
                        For a As Integer = 1 To dtFechas.Rows.Count - 1
                            nColumnaV = nColumnaV + 1
                            oCells(nFila, nColumnaV).Formula = "=IF(" & HelpExcel.LetraColumna(nColumnaV - 1) & nFila - 1 & " <> 0," & HelpExcel.LetraColumna(nColumnaV) & nFila - 1 & "/" & HelpExcel.LetraColumna(nColumnaV - 1) & nFila - 1 & " - 1,0)"
                            oCells(nFila, nColumnaV).NumberFormat = "0.00%"
                        Next

                        nFila = nFila + 1
                        oCells(nFila, nColumna) = drData("Cantidad")
                        nColumna = nColumna + 1
                        nFila2 = nFila
                        i = i + 1
                        oSheet.Range(oCells(nFila - 5, 2), oCells(nFila - 1, 2)).Merge()
                    End If
                Next
                If dtData.Rows.Count > 0 Then
                    nColumna = 4
                    oSheet.Rows(nFila & ":" & nFila).Select()
                    oSheet.Rows(nFila & ":" & nFila).Copy()
                    oSheet.Rows(nFila + 1 & ":" & nFila + 1).Select()
                    oSheet.Rows(nFila + 1 & ":" & nFila + 1).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)
                    oSheet.Rows(nFila + 1 & ":" & nFila + 1).Select()
                    oSheet.Rows(nFila + 1 & ":" & nFila + 1).Copy()
                    oSheet.Rows(nFila + 2 & ":" & nFila + 2).Select()
                    oSheet.Rows(nFila + 2 & ":" & nFila + 2).PasteSpecial(Paste:=Excel.XlPasteType.xlPasteFormats)

                    nFila = nFila + 1
                    oCells(nFila, 3) = "Total"
                    Dim nColumnaT As Integer = nColumna
                    For x As Integer = 1 To dtFechas.Rows.Count
                        oCells(nFila, nColumnaT).Formula = "=SUM(" & HelpExcel.LetraColumna(nColumnaT) & nFila - 3 & ":" & HelpExcel.LetraColumna(nColumnaT) & nFila - 1 & ")"
                        nColumnaT = nColumnaT + 1
                    Next

                    nFila = nFila + 1
                    oCells(nFila, 3) = "Var %"
                    Dim nColumnaV As Integer = nColumna
                    For a As Integer = 1 To dtFechas.Rows.Count - 1
                        nColumnaV = nColumnaV + 1
                        oCells(nFila, nColumnaV).Formula = "=IF(" & HelpExcel.LetraColumna(nColumnaV - 1) & nFila - 1 & " <> 0," & HelpExcel.LetraColumna(nColumnaV) & nFila - 1 & "/" & HelpExcel.LetraColumna(nColumnaV - 1) & nFila - 1 & " - 1,0)"
                        oCells(nFila, nColumnaV).NumberFormat = "0.00%"
                    Next
                    oSheet.Range(oCells(nFila - 4, 2), oCells(nFila, 2)).Merge()
                End If
            End If
            oSheet.SaveAs(sFile)
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
            Response.WriteFile(sFile)
            Response.End()
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

#Region "Eventos"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPortafolio()
            CargarOpcion()
            CargarFechas()
        End If
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarReporte()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
#End Region
End Class
