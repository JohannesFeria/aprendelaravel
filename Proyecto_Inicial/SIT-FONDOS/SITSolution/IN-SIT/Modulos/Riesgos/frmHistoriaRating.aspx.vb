Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Imports System.IO
Imports ParametrosSIT

Partial Class Modulos_Riesgos_frmHistoriaRating
    Inherits BasePage
    Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
    Private Sub GenerarReporte()
        Dim fechaInicio As Integer = 0, fechaFin As Integer = 0

        If Me.tbFechaInicio.Text.Length > 0 Then
            fechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
        End If
        If Me.tbFechaFin.Text.Length > 0 Then
            fechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
        End If

        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtRegInv As DataTable
            If ddlTabla.SelectedValue = "0" Then
                dtRegInv = oOrdenPreOrdenInversionBM.SeleccionaRatingValorHistoria(txtISIN.Text, fechaInicio, fechaFin)
            Else
                dtRegInv = oOrdenPreOrdenInversionBM.SeleccionaRatingTerceroHistoria(txtCodigoTercero.Text, fechaInicio, fechaFin)
            End If
            If dtRegInv.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String, Nombre As String
                Nombre = ddlTabla.SelectedItem.Text + "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                Dim Indice As Integer = 1
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "Plantilla_Rating.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oSheet.Name = ddlTabla.SelectedItem.Text
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                DatosExcel(dtRegInv, 4, oSheet, oCells)
                oExcel.Cells.EntireColumn.AutoFit()

                If ddlTabla.SelectedValue = "0" Then
                    oCells.Range("E:G").Delete() 'Eliminamos las columnas innecesarias para el reporte
                End If

                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Nombre)
                Response.WriteFile(sFile)
                Response.End()
            Else
                AlertaJS("No existen registros que mostrar.")
            End If
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
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        oCells(2, 2) = Now.Date.ToString
        For Each dr In dt.Rows
            Dim i As Integer = 0
            Do While i <= dt.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                i = i + 1
            Loop
            FilaInicial += 1
        Next
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTabla.SelectedIndexChanged
        If ddlTabla.SelectedValue = "0" Then
            pnTerceros.Visible = False
            pnValores.Visible = True
        Else
            pnTerceros.Visible = True
            pnValores.Visible = False
        End If
        'gvReporte.DataSource = Nothing
        'gvReporte.DataBind()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim fechaInicio As Integer = 0, fechaFin As Integer = 0

            If Me.tbFechaInicio.Text.Length > 0 Then
                fechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
            End If
            If Me.tbFechaFin.Text.Length > 0 Then
                fechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
            End If

            gvReporte.Dispose()

            'Dim habilitarColumnas As Boolean = (ddlTabla.SelectedValue = "1")
            If (ddlTabla.SelectedValue = "0") Then

                gvReporte.Columns(1).HeaderText = "Nemónico"
                gvReporte.Columns(2).HeaderText = "ISIN"
                gvReporte.Columns(10).HeaderText = "Fecha Clasif."

                gvReporte.Columns(0).Visible = True 'Tipo Negocio
                gvReporte.Columns(3).Visible = True 'Codigo Instrumento
                gvReporte.Columns(4).Visible = True 'Emisor
                gvReporte.Columns(8).Visible = True 'Clasificadora

                gvReporte.Columns(6).Visible = False 'RatingOficial
                gvReporte.Columns(7).Visible = False 'RatingFF
                gvReporte.Columns(9).Visible = False 'LineaPlazo


            Else
                gvReporte.Columns(1).HeaderText = "Descripción"
                gvReporte.Columns(2).HeaderText = "Código"
                gvReporte.Columns(10).HeaderText = "Fecha"

                gvReporte.Columns(0).Visible = False 'Tipo Negocio
                gvReporte.Columns(3).Visible = False 'Codigo Instrumento
                gvReporte.Columns(4).Visible = False 'Emisor
                gvReporte.Columns(8).Visible = False 'Clasificadora

                gvReporte.Columns(6).Visible = True 'RatingOficial
                gvReporte.Columns(7).Visible = True 'RatingFF
                gvReporte.Columns(9).Visible = True 'LineaPlazo
            End If



            If ddlTabla.SelectedValue = "0" Then
                gvReporte.DataSource = oOrdenPreOrdenInversionBM.SeleccionaRatingValorHistoria(txtISIN.Text, fechaInicio, fechaFin)
            Else
                gvReporte.DataSource = oOrdenPreOrdenInversionBM.SeleccionaRatingTerceroHistoria(txtCodigoTercero.Text, fechaInicio, fechaFin)
            End If
            gvReporte.DataBind()



        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnGenera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenera.Click
        Try
            GenerarReporte()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("SS_DatosModal") Is Nothing Then
            If hdModal.Value = "Nemonico" Then
                Dim datos As String()
                datos = CType(Session("SS_DatosModal"), String())
                txtISIN.Text = datos(0)
                txtMnemonico.Text = datos(1)
                txtSBS.Text = datos(2)
            Else
                txtEmisor.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                txtCodigoTercero.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
            End If
            Session.Remove("SS_DatosModal")


        End If

        If Not Page.IsPostBack Then
            tbFechaInicio.Text = Now.ToString("dd/MM/yyyy")
            tbFechaFin.Text = Now.ToString("dd/MM/yyyy")
            CargarControles()
            btnBuscar_Click(btnBuscar, Nothing)
        End If
    End Sub

    Protected Sub btnImportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("~/Modulos/Parametria/Tablas/Entidades/frmImportarRating.aspx?frmPadre=frmHistoriaRating")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub CargarControles()

        Dim dtParametrosGenarales As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM

        dtParametrosGenarales = oParametrosGeneralesBM.Listar(TIPO_NEGOCIO, Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlNegocio, dtParametrosGenarales, "Valor", "Nombre", True)
    End Sub
End Class