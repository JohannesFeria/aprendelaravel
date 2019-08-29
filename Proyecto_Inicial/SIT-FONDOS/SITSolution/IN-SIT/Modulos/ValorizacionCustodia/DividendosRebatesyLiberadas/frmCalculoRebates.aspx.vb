Imports System.Web
Imports System.Security
Imports System.Reflection
Imports System.Diagnostics
Imports System.Configuration
Imports System.Text
Imports SIT.BusinessEntities
Imports SIT.BusinessLayer
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data

Partial Class Modulos_ValorizacionCustodia_DividendosRebatesyLiberadas_frmCalculoRebates
    Inherits BasePage

    Dim strMensajeObli As String = ""

#Region " /* Métodos de la Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                UIUtility.CargarMonedaOI(dllMoneda)
                Me.tbFechaInicio.Text = New UtilDM().RetornarFechaNegocio()
                Me.tbFechaFin.Text = New UtilDM().RetornarFechaNegocio()
            End If

            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datos As String()

                datos = CType(Session("SS_DatosModal"), String())
                txtMnemonico.Text = datos(1)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            Exportar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Exportar")
        End Try        
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

#End Region

#Region " /* Métodos Personaliz"

    Private Function GetISIN() As String
        Return ""
    End Function

    Private Function GetSBS() As String
        Return ""
    End Function

    Private Sub Exportar()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oSheetSBS As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim sFile As String, sTemplate As String
            Dim oDs As New DataSet
            Dim dtResumen As New DataTable
            Dim dtDetalle As New DataTable
            Dim dtTotales As New DataTable
            oDs = New DividendosRebatesLiberadasBM().ObtenerDatosReporteRebates(UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text), Me.txtMnemonico.Text)

            dtResumen = oDs.Tables(0)
            dtDetalle = oDs.Tables(1)
            dtTotales = oDs.Tables(2)

            Dim dtParametriaReb As New DataTable
            dtParametriaReb = New DividendosRebatesLiberadasBM().ObtenerCodigoRebateDetalleCabecera(Me.txtMnemonico.Text)
            If dtParametriaReb.Rows.Count > 0 Then
                If (dtParametriaReb.Rows(0)("SumatoriaFondos") = "S") Then
                    dtParametriaReb = New DataTable
                    dtParametriaReb = New DividendosRebatesLiberadasBM().ObtenerCodigoRebateDetalle(Me.txtMnemonico.Text)
                    For Each objRowDetalle As DataRow In dtDetalle.Rows
                        Dim intIndice As Integer = dtParametriaReb.Select(String.Format("{0} >= ImporteIni and {0} <= ImporteFin", objRowDetalle("TotalInversion").ToString())).Length
                        If (intIndice > 0) Then
                            objRowDetalle("RebateAnual") = (dtParametriaReb.Select(String.Format("{0} >= ImporteIni and {0} <= ImporteFin", objRowDetalle("TotalInversion").ToString())))(0)("PorcRebate")
                        End If
                    Next
                End If
            End If
            Dim datosRequest As DataSet
            Dim dr As DataRow
            Dim n As Int32 = 0
            If dtResumen.Rows.Count > 0 And dtDetalle.Rows.Count > 0 And dtTotales.Rows.Count > 0 Then
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", datosRequest).Rows(0)("Valor") & "ReporteRebate_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutasPlantillas() & "\" & "PlantillaRebates.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                'Cabecera
                oCells(5, 2) = dtResumen.Rows(0).Item("Titulo")
                oCells(9, 3) = dtResumen.Rows(0).Item("CodigoISIN")
                oCells(11, 3) = dtResumen.Rows(0).Item("Descripcion")
                oCells(9, 9) = dtResumen.Rows(0).Item("CodigoNemonico")
                oCells(11, 9) = dtResumen.Rows(0).Item("Moneda")
                Dim s_Solo_Fechas = (From row In dtDetalle.AsEnumerable()
                                     Select row.Field(Of String)("Fecha")).Distinct()
                'Detalle
                Dim fecha As String = String.Empty
                For Each s_Fecha In s_Solo_Fechas
                    fecha = s_Fecha
                    Dim existe = Nothing
                    Try
                        existe = (From dt In dtDetalle.AsEnumerable() Where dt.Field(Of String)("Fecha") = fecha
                                 Select dt).CopyToDataTable()
                        oCells(n + 16, 2) = fecha
                    Catch ex As Exception
                        existe = Nothing
                    End Try
                    Dim ncol As Integer = 5
                    Dim totalinv As Decimal = 0
                    Dim rebatre As Decimal = 0

                    If Not existe Is Nothing Then
                        For Each drdetalle In existe.Rows
                            oCells(n + 16, 3) = drdetalle("Nav")
                            oSheet.Range(oCells(n + 16, 3), oCells(n + 16, 3)).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ "
                            ncol += 1
                            oCells(14, ncol) = drdetalle("Portafolio")
                            Dim codporta As String = drdetalle("CodigoPortafolio").ToString()
                            Dim TotalFondo = Nothing
                            Try
                                TotalFondo = (From dt In dtTotales.AsEnumerable() Where dt.Field(Of String)("CodigoPortafolio") = codporta
                                 Select dt).CopyToDataTable()
                            Catch ex As Exception
                                TotalFondo = Nothing
                            End Try
                            For Each drto In TotalFondo.Rows
                                oCells(14, ncol + 2) = drto("TotalFondo").ToString()
                                oSheet.Range(oCells(14, ncol + 2), oCells(14, ncol + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                                oSheet.Range(oCells(14, ncol + 2), oCells(14, ncol + 2)).Borders.Weight = 3
                                oSheet.Range(oCells(14, ncol + 2), oCells(14, ncol + 2)).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ "
                                Exit For
                            Next
                            oSheet.Range(oCells(14, ncol), oCells(14, ncol + 1)).Merge()
                            oSheet.Range(oCells(14, ncol), oCells(14, ncol + 1)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                            oSheet.Range(oCells(14, ncol), oCells(14, ncol + 1)).Borders.Weight = 3
                            oCells(15, ncol) = "UNIDADES"
                            oSheet.Columns(ncol).ColumnWidth = 12.43
                            oSheet.Range(oCells(15, ncol), oCells(15, ncol + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                            oSheet.Range(oCells(15, ncol), oCells(15, ncol + 2)).Borders.Weight = 3
                            oCells(n + 16, ncol) = drdetalle("Unidades")
                            oSheet.Range(oCells(n + 16, ncol), oCells(n + 16, ncol)).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ "
                            ncol += 1

                            oCells(15, ncol) = "IMPORTE"
                            oSheet.Columns(ncol).ColumnWidth = 13
                            oCells(n + 16, ncol) = drdetalle("Importe")
                            oSheet.Range(oCells(n + 16, ncol), oCells(n + 16, ncol)).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ "
                            ncol += 1

                            oCells(15, ncol) = "REBATE"
                            oSheet.Columns(ncol).ColumnWidth = 11
                            oCells(n + 16, ncol) = drdetalle("Rebate")
                            oSheet.Range(oCells(n + 16, ncol), oCells(n + 16, ncol)).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ "
                            ncol += 1

                            oSheet.Columns(ncol).ColumnWidth = 0.83

                            totalinv += ToNullDecimal(drdetalle("Importe"))
                            rebatre = ToNullDecimal(drdetalle("RebateAnual"))
                        Next
                        ncol += 1
                        oCells(15, ncol) = "TOTAL INVERSION"
                        oSheet.Range(oCells(15, ncol), oCells(15, ncol)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                        oSheet.Range(oCells(15, ncol), oCells(15, ncol)).Borders.Weight = 3
                        oSheet.Columns(ncol).ColumnWidth = 18
                        oCells(n + 16, ncol) = totalinv
                        oSheet.Range(oCells(n + 16, ncol), oCells(n + 16, ncol)).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ "
                        oSheet.Columns(ncol + 1).ColumnWidth = 0.83
                        ncol += 2
                        oCells(15, ncol) = "REBATE" & Environment.NewLine & "ANUAL%"
                        oSheet.Range(oCells(15, ncol), oCells(15, ncol)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                        oSheet.Range(oCells(15, ncol), oCells(15, ncol)).Borders.Weight = 3
                        oSheet.Columns(ncol).ColumnWidth = 10.71
                        oCells(n + 16, ncol) = rebatre
                        oSheet.Range(oCells(n + 16, ncol), oCells(n + 16, ncol)).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ "
                    End If
                    n = n + 1
                Next
                oSheet.SaveAs(sFile)
                oBook.Save()
                oBook.Close()
                Dim filepath As String = sFile.Replace("\", "\\")
                Dim filename As String = System.IO.Path.GetFileName(filepath)
                Response.Clear()
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", "attachment; filename=""" & filename & """")
                Response.Flush()
                Response.WriteFile(filepath)
            Else
                AlertaJS("Verifique, no existen datos para procesar")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Exportar")
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

    Protected Function RutasPlantillas() As String  'Solo mientras no tenemos session borrarlo
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("018", DatosRequest)
        Return oArchivoPlanoBE.Tables(0).Rows(0).Item(4).ToString()
    End Function

#End Region

End Class