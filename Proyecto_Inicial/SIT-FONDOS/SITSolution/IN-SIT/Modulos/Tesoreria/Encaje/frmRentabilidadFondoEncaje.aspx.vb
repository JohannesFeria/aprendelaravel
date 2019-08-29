Imports Sit.BusinessLayer
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports ParametrosSIT
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmRentabilidadFondoEncaje
    Inherits BasePage

#Region " /* Métodos de la Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                FechaPortafolio()
                CargarGrillaPortafolio()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        Try
            FechaPortafolio()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try        
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim FechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            Dim FechaInicio As String = tbFechaInicio.Text.Trim
            'Dim FechaFin As String = tbFechaFin.Text.Trim
            If Validar() Then
                EjecutarJS(String.Format("ShowReport('{0}','{1}','{2}');", ddlFondo.SelectedValue, FechaInicio, FechaFin))
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

    Private Sub btnProvision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProvision.Click
        Try
            GeneraReporteProvisionContable()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        End Try
    End Sub

    Private Sub btnRentabilidadTotalInstrumentos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRentabilidadTotalInstrumentos.Click
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtResumen As DataTable = UIUtility.GeneraTablaResumenTotalInstrumentos(tbFechaInicio.Text, tbFechaFin.Text, ddlFondo.SelectedValue, DatosRequest)
            Dim FechaInicio As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            Dim FechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            Dim dtRentEncaje As DataTable = New ReporteGestionBM().RentabilidadEncajeFondo(FechaInicio, FechaFin, ddlFondo.SelectedValue, DatosRequest).Tables(0)
            sTemplate = RutaPlantillas() & "\" & "PlantillaResumenRentabilidadEncajeTotalInstrumentos.xls"
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "ReporteResumenRentabilidadEncajeTotalInstrumentos_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls" 'HDG OT 65195 20120515

            If File.Exists(sFile) Then File.Delete(sFile)
            oExcel.Visible = False : oExcel.DisplayAlerts = False

            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            oCells(1, 2) = ddlFondo.Items(ddlFondo.SelectedIndex).Text.Trim
            oCells(2, 2) = tbFechaInicio.Text
            oCells(3, 2) = tbFechaFin.Text
            oCells(5, 3) = dtRentEncaje.Rows(0)("PorcRentEncaje").ToString

            oSheet.SaveAs(sFile)

            Dim drResumen As DataRow
            Dim n As Integer = 7
            Dim k As Integer = 0
            Dim ni As Integer = 7
            Dim nf As Integer = 0
            Dim nr As Integer = 0

            For Each drResumen In dtResumen.Rows
                oCells(n, 1) = drResumen("TipoInstrumento").ToString
                oCells(n, 2) = drResumen("CodigoNemonico").ToString
                oCells(n, 3) = drResumen("Condicion").ToString
                oCells(n, 4) = drResumen("UtilidadTotal").ToString
                oCells(n, 5) = "=D" & n & "-F" & n
                oCells(n, 6) = "=+$C$5*D" & n

                If drResumen("MetodoCalculoRenta").ToString = "1" Then
                    'COSTO VENTA PROMEDIO
                    oCells(n, 7) = Val(drResumen("ValorVenta").ToString)
                    oCells(n, 8) = Val(drResumen("CostoValorVendido").ToString)
                    oCells(n, 9) = Val(drResumen("UtilidadVentaTotal").ToString)
                    oCells(n, 10) = "=I" & n & "-K" & n
                    oCells(n, 11) = "=+I" & n & "*$C$5"

                    If Val(drResumen("TotalInteresDividendo").ToString) <> 0 Then
                        oCells(n, 13) = "=(+X" & n & "/W" & n & ")*L" & n
                        oCells(n, 14) = "=+L" & n & "*$C$5"
                    End If

                    oCells(n, 17) = "=(D" & n & "-I" & n & ")-L" & n
                    oCells(n, 18) = "=(E" & n & "-J" & n & ")-M" & n
                    oCells(n, 19) = "=+(F" & n & "-K" & n & ")-N" & n
                Else 'VALORACIÓN VECTOR PRECIO
                    If Val(drResumen("TotalInteresDividendo").ToString) <> 0 Then
                        oCells(n, 13) = "=(+X" & n & "/W" & n & ")*L" & n
                        oCells(n, 14) = "=+L" & n & "*$C$5"
                    End If
                    oCells(n, 20) = "=D" & n & "-L" & n
                    oCells(n, 21) = "=E" & n & "-M" & n
                    oCells(n, 22) = "=F" & n & "-N" & n
                End If
                oCells(n, 12) = Val(drResumen("TotalInteresDividendo").ToString)
                oCells(n, 15) = IIf(Val(drResumen("TotalInteres").ToString) > 0, "=+L" & n & "*$C$5", "0")
                oCells(n, 16) = IIf(Val(drResumen("TotalDividendo").ToString) > 0, "=+L" & n & "*$C$5", "0")
                oCells(n, 23) = "=D" & n & "-I" & n
                oCells(n, 24) = "=E" & n & "-J" & n

                n = n + 1
            Next

            nf = n - 1
            n = n + 1
            k = n
            oCells(n, 1) = "RENTABILIDAD INVERSIONES"
            oCells(n, 4) = "=SUMA(D7:D" & n - 2 & ")"
            oCells(n, 5) = "=SUMA(E7:E" & n - 2 & ")"
            oCells(n, 6) = "=SUMA(F7:F" & n - 2 & ")"
            oCells(n, 7) = "=SUMA(G7:G" & n - 2 & ")"
            oCells(n, 8) = "=SUMA(H7:H" & n - 2 & ")"
            oCells(n, 9) = "=SUMA(I7:I" & n - 2 & ")"
            oCells(n, 10) = "=SUMA(J7:J" & n - 2 & ")"
            oCells(n, 11) = "=SUMA(K7:K" & n - 2 & ")"
            oCells(n, 12) = "=SUMA(L7:L" & n - 2 & ")"
            oCells(n, 13) = "=SUMA(M7:M" & n - 2 & ")"
            oCells(n, 14) = "=SUMA(N7:N" & n - 2 & ")"
            oCells(n, 15) = "=SUMA(O7:O" & n - 2 & ")"
            oCells(n, 16) = "=SUMA(P7:P" & n - 2 & ")"
            oCells(n, 17) = "=SUMA(Q7:Q" & n - 2 & ")"
            oCells(n, 18) = "=SUMA(R7:R" & n - 2 & ")"
            oCells(n, 19) = "=SUMA(S7:S" & n - 2 & ")"
            oCells(n, 20) = "=SUMA(T7:T" & n - 2 & ")"
            oCells(n, 21) = "=SUMA(U7:U" & n - 2 & ")"
            oCells(n, 22) = "=SUMA(V7:V" & n - 2 & ")"
            oSheet.Range("D" & n & ":V" & n).Font.Bold = True

            n = n + 1
            nr = n
            oCells(n, 1) = "RENTABILIDAD OTROS"
            oCells(n, 4) = "=+D" & n + 1 & "-D" & n - 1
            oCells(n, 5) = "=+E" & n + 1 & "-E" & n - 1
            oCells(n, 6) = "=+F" & n + 1 & "-F" & n - 1
            oCells(n, 7) = ""
            oCells(n, 8) = ""
            oCells(n, 9) = "=+D" & n & "-D" & n + 3
            oCells(n, 10) = "=+I" & n & "-K" & n
            oCells(n, 11) = "=+I" & n & "*$C$5"
            oCells(n, 12) = ""
            oCells(n, 13) = ""
            oCells(n, 14) = ""
            oCells(n, 15) = ""
            oCells(n, 16) = ""
            oCells(n, 17) = "=+D" & n + 3
            oCells(n, 18) = "=+Q" & n & "-S" & n
            oCells(n, 19) = "=+Q" & n & "*$C$5"
            oCells(n, 20) = ""
            oCells(n, 21) = ""
            oCells(n, 22) = ""

            n = n + 1
            oCells(n, 1) = "RENTABILIDAD TOTAL"
            oCells(n, 4) = "=+E" & n & "+F" & n
            oCells(n, 5) = dtRentEncaje.Rows(0)("RentabilidadFondo").ToString
            oCells(n, 6) = dtRentEncaje.Rows(0)("RentabilidadEncaje").ToString
            oCells(n, 7) = "=+G" & n - 2 & "+G" & n - 1
            oCells(n, 8) = "=+H" & n - 2 & "+H" & n - 1
            oCells(n, 9) = "=+I" & n - 2 & "+I" & n - 1
            oCells(n, 10) = "=+J" & n - 2 & "+J" & n - 1
            oCells(n, 11) = "=+K" & n - 2 & "+K" & n - 1
            oCells(n, 12) = "=+L" & n - 2 & "+L" & n - 1
            oCells(n, 13) = "=+M" & n - 2 & "+M" & n - 1
            oCells(n, 14) = "=+N" & n - 2 & "+N" & n - 1
            oCells(n, 15) = "=+O" & n - 2 & "+O" & n - 1
            oCells(n, 16) = "=+P" & n - 2 & "+P" & n - 1
            oCells(n, 17) = "=+Q" & n - 2 & "+Q" & n - 1
            oCells(n, 18) = "=+R" & n - 2 & "+R" & n - 1
            oCells(n, 19) = "=+S" & n - 2 & "+S" & n - 1
            oCells(n, 20) = "=+T" & n - 2 & "+T" & n - 1
            oCells(n, 21) = "=+U" & n - 2 & "+U" & n - 1
            oCells(n, 22) = "=+V" & n - 2 & "+V" & n - 1
            oSheet.Range("D" & n & ":V" & n).Font.Bold = True
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThick
            n = n + 2
            oCells(n, 1) = "PROVISIÓN VALORACIÓN DERIVADOS (CxC y CxP)"
            Dim sProvDer As String
            oCells(n, 4) = sProvDer
            oSheet.Range("D" & n).Font.Bold = True
            oSheet.Range("A" & k & ":A" & n).Font.Bold = True

            Dim dtCondiciones As DataTable
            Dim drCondicion As DataRow

            dtCondiciones = UIUtility.SelectDistinct(dtResumen, "Condicion")
            dtCondiciones.Columns.Add("Celdas", GetType(String))

            n = n + 2
            oCells(n, 1) = "SUBTOTALES CONDICIÓN"
            oSheet.Range("A" & n).Font.Bold = True
            oCells.Range("A" & n).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
            n = n + 1
            k = n
            For Each drCondicion In dtCondiciones.Rows
                If Not drCondicion("Condicion") Is DBNull.Value Then
                    oCells(n, 1) = drCondicion("Condicion")
                    oCells(n, 4) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",D" & ni & ":D" & nf & ")"
                    oCells(n, 5) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",E" & ni & ":E" & nf & ")"
                    oCells(n, 6) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",F" & ni & ":F" & nf & ")"
                    oCells(n, 7) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",G" & ni & ":G" & nf & ")"
                    oCells(n, 8) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",H" & ni & ":H" & nf & ")"
                    oCells(n, 9) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",I" & ni & ":I" & nf & ")"
                    oCells(n, 10) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",J" & ni & ":J" & nf & ")"
                    oCells(n, 11) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",K" & ni & ":K" & nf & ")"
                    oCells(n, 12) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",L" & ni & ":L" & nf & ")"
                    oCells(n, 13) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",M" & ni & ":M" & nf & ")"
                    oCells(n, 14) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",N" & ni & ":N" & nf & ")"
                    oCells(n, 15) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",O" & ni & ":O" & nf & ")"
                    oCells(n, 16) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",P" & ni & ":P" & nf & ")"
                    oCells(n, 17) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",Q" & ni & ":Q" & nf & ")"
                    oCells(n, 18) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",R" & ni & ":R" & nf & ")"
                    oCells(n, 19) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",S" & ni & ":S" & nf & ")"
                    oCells(n, 20) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",T" & ni & ":T" & nf & ")"
                    oCells(n, 21) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",U" & ni & ":U" & nf & ")"
                    oCells(n, 22) = "=SUMAR.SI(C" & ni & ":C" & nf & ",A" & n & ",V" & ni & ":V" & nf & ")"
                    n = n + 1
                End If
            Next

            oCells(n, 1) = "OTROS"
            oCells(n, 4) = "=+D" & nr
            oCells(n, 5) = "=+E" & nr
            oCells(n, 6) = "=+F" & nr
            oCells(n, 7) = "=+G" & nr
            oCells(n, 8) = "=+H" & nr
            oCells(n, 9) = "=+I" & nr
            oCells(n, 10) = "=+J" & nr
            oCells(n, 11) = "=+K" & nr
            oCells(n, 12) = "=+L" & nr
            oCells(n, 13) = "=+M" & nr
            oCells(n, 14) = "=+N" & nr
            oCells(n, 15) = "=+O" & nr
            oCells(n, 16) = "=+P" & nr
            oCells(n, 17) = "=+Q" & nr
            oCells(n, 18) = "=+R" & nr
            oCells(n, 19) = "=+S" & nr
            oCells(n, 20) = "=+T" & nr
            oCells(n, 21) = "=+U" & nr
            oCells(n, 22) = "=+V" & nr
            n = n + 1

            oCells(n, 1) = "TOTAL"
            oSheet.Range("A" & n).Font.Bold = True
            oCells(n, 4) = "=SUMA(D" & k & ":D" & n - 1 & ")"
            oCells(n, 5) = "=SUMA(E" & k & ":E" & n - 1 & ")"
            oCells(n, 6) = "=SUMA(F" & k & ":F" & n - 1 & ")"
            oCells(n, 7) = "=SUMA(G" & k & ":G" & n - 1 & ")"
            oCells(n, 8) = "=SUMA(H" & k & ":H" & n - 1 & ")"
            oCells(n, 9) = "=SUMA(I" & k & ":I" & n - 1 & ")"
            oCells(n, 10) = "=SUMA(J" & k & ":J" & n - 1 & ")"
            oCells(n, 11) = "=SUMA(K" & k & ":K" & n - 1 & ")"
            oCells(n, 12) = "=SUMA(L" & k & ":L" & n - 1 & ")"
            oCells(n, 13) = "=SUMA(M" & k & ":M" & n - 1 & ")"
            oCells(n, 14) = "=SUMA(N" & k & ":N" & n - 1 & ")"
            oCells(n, 15) = "=SUMA(O" & k & ":O" & n - 1 & ")"
            oCells(n, 16) = "=SUMA(P" & k & ":P" & n - 1 & ")"
            oCells(n, 17) = "=SUMA(Q" & k & ":Q" & n - 1 & ")"
            oCells(n, 18) = "=SUMA(R" & k & ":R" & n - 1 & ")"
            oCells(n, 19) = "=SUMA(S" & k & ":S" & n - 1 & ")"
            oCells(n, 20) = "=SUMA(T" & k & ":T" & n - 1 & ")"
            oCells(n, 21) = "=SUMA(U" & k & ":U" & n - 1 & ")"
            oCells(n, 22) = "=SUMA(V" & k & ":V" & n - 1 & ")"
            oSheet.Range("D" & n & ":V" & n).Font.Bold = True
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlThin
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
            oCells.Range("D" & n & ":V" & n).Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThick
            oBook.Save()
            oBook.Close()
            Dim filepath As String = sFile.Replace("\", "\\")
            Dim filename As String = System.IO.Path.GetFileName(filepath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & filename & """")
            Response.Flush()
            Response.WriteFile(filepath)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
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

    Private Sub btnResumenPorTipoRenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResumenPorTipoRenta.Click
        'declaracion de variables
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Dim sFile As String, sTemplate As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            sTemplate = RutaPlantillas() & "\" & "PlantillaResumenPorTipoRenta.xls"
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "ReporteResumenPorTipoRenta_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls" 'HDG OT 65195 20120515
            If File.Exists(sFile) Then File.Delete(sFile)
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(3, 2) = tbFechaInicio.Text
            oCells(4, 2) = tbFechaFin.Text
            oSheet.SaveAs(sFile)
            Dim dtResumen As DataTable
            Dim drResumen As DataRow
            Dim n As Integer = 7
            'Dim item As ListItem
            'Dim condiciones As Object
            Dim i As Integer
            Dim k As Integer
            Dim drCondicion As DataRow
            Dim Portafolio As String
            Dim Celdas As String
            Dim dtCondiciones As DataTable
            Dim UtilidadEncajeVenta As Double = 0
            Dim UtilidadEncajeIntDiv As Double = 0
            Dim UtilidadEncajeValoracion As Double = 0
            Dim SumUtilidadEncajeVenta As Double = 0
            Dim SumUtilidadEncajeIntDiv As Double = 0
            Dim SumUtilidadEncajeValoracion As Double = 0
            Dim UtilidadEncajeInt As Double = 0
            Dim UtilidadEncajeDiv As Double = 0
            Dim SumUtilidadEncajeInt As Double = 0
            Dim SumUtilidadEncajeDiv As Double = 0
            Dim FechaInicio As Decimal = 0
            Dim FechaFin As Decimal = 0
            Dim PorcRentEncaje As Decimal = 0
            Dim UtilidadTotal As Decimal = 0
            Dim UtilidadVentaTotal As Decimal = 0
            Dim UtilidadTotal2 As Decimal = 0
            Dim UtilidadEncajeIntDiv2 As Double = 0
            Dim UtilidadEncajeValorVectorPrecio As Double = 0
            Dim SumUtilidadEncajeValorVectorPrecio As Double = 0
            Dim UtilidadEncajeTotal As Decimal = 0
            Dim UtilidadEncajeTotalB As Decimal = 0
            Dim TotSumUtilidadEncajeTotal As Decimal = 0
            Dim TotSumUtilidadEncajeTotalB As Decimal = 0
            Dim TotSumUtilidadEncajeVenta As Double = 0
            Dim TotSumUtilidadEncajeValoracion As Double = 0
            Dim RentabilidadFondo As Double = 0
            Dim RentabilidadEncaje As Double = 0
            Dim ProvisionValoracion As Double = 0
            Dim CeldasOtros As String
            n = 6
            For i = 0 To dgPortafolio.Rows.Count - 1
                Dim txtProvision As TextBox
                Portafolio = dgPortafolio.DataKeys(i)("CodigoPortafolio").ToString().Trim()
                oCells(n, 1) = dgPortafolio.Rows(i).Cells(1).Text.Trim()
                oCells(n, 1).HorizontalAlignment = -4108
                oSheet.Range("A" & n & ":G" & n).Merge()
                n = n + 1
                oSheet.Range("A" & n & ":G" & n).WrapText = True
                oSheet.Range("A" & n & ":G" & n).HorizontalAlignment = Excel.Constants.xlCenter
                oSheet.Range("A" & n & ":G" & n).VerticalAlignment = Excel.Constants.xlCenter
                oCells(n, 1) = "CONDICIÓN"
                oCells(n, 2) = "Utilidad Encaje Total"
                oCells(n, 3) = "Utilidad Encaje Venta"
                oCells(n, 4) = "Encaje Intereses"
                oCells(n, 5) = "Encaje Dividendos"
                oCells(n, 6) = "Utilidad Encaje Valoración"
                oCells(n, 7) = "Utilidad Encaje Valoración Vector Precio"
                oSheet.Range("A" & (n - 1) & ":G" & n).Borders.ColorIndex = 1
                oSheet.Range("A" & (n - 1) & ":G" & n).Font.Bold = True
                n = n + 1
                k = n
                dtResumen = UIUtility.GeneraTablaResumenTotalInstrumentos(tbFechaInicio.Text, tbFechaFin.Text, Portafolio, DatosRequest)
                TotSumUtilidadEncajeTotal = 0
                TotSumUtilidadEncajeTotalB = 0
                TotSumUtilidadEncajeVenta = 0
                TotSumUtilidadEncajeValoracion = 0
                RentabilidadFondo = 0
                RentabilidadEncaje = 0
                PorcRentEncaje = 0
                ProvisionValoracion = 0
                FechaInicio = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
                FechaFin = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
                Dim dtRentEncaje As DataTable = New ReporteGestionBM().RentabilidadEncajeFondo(FechaInicio, FechaFin, Portafolio, DatosRequest).Tables(0)
                RentabilidadFondo = Val(dtRentEncaje.Rows(0)("RentabilidadFondo").ToString)
                RentabilidadEncaje = Val(dtRentEncaje.Rows(0)("RentabilidadEncaje").ToString)
                PorcRentEncaje = Val(dtRentEncaje.Rows(0)("PorcRentEncaje").ToString)
                txtProvision = CType(dgPortafolio.Rows(i).FindControl("txtProvDer"), TextBox)
                ProvisionValoracion = ToNullDouble(txtProvision.Text.Trim())
                dtCondiciones = UIUtility.SelectDistinct(dtResumen, "Condicion")
                dtCondiciones.Columns.Add("Celdas", GetType(String))
                For Each drCondicion In dtCondiciones.Rows
                    If Not drCondicion("Condicion") Is DBNull.Value Then
                        UtilidadEncajeVenta = 0
                        UtilidadEncajeIntDiv = 0
                        UtilidadEncajeValoracion = 0
                        SumUtilidadEncajeVenta = 0
                        SumUtilidadEncajeIntDiv = 0
                        SumUtilidadEncajeValoracion = 0
                        'ini HDG OT 65195 20120515
                        UtilidadEncajeInt = 0
                        UtilidadEncajeDiv = 0
                        SumUtilidadEncajeInt = 0
                        SumUtilidadEncajeDiv = 0
                        UtilidadTotal = 0
                        UtilidadVentaTotal = 0
                        UtilidadTotal2 = 0
                        UtilidadEncajeIntDiv2 = 0
                        UtilidadEncajeValorVectorPrecio = 0
                        SumUtilidadEncajeValorVectorPrecio = 0
                        UtilidadEncajeTotal = 0
                        UtilidadEncajeTotal = Val(dtResumen.Compute("sum(UtilidadTotal)", "Condicion='" & drCondicion("Condicion") & "'")) * PorcRentEncaje
                        UtilidadEncajeTotalB = Val(dtResumen.Compute("sum(UtilidadTotal)", "Condicion='" & drCondicion("Condicion") & "'"))
                        oCells(n, 1) = drCondicion("Condicion")
                        oCells(n, 2) = UtilidadEncajeTotal
                        For Each drResumen In dtResumen.Rows
                            If drResumen("Condicion") = drCondicion("Condicion") Then
                                If Val(drResumen("UtilidadVentaTotal").ToString) = 0 Then
                                    UtilidadEncajeVenta = 0
                                Else
                                    UtilidadEncajeVenta = Val(drResumen("UtilidadVentaTotal").ToString) * PorcRentEncaje  'HDG OT 65195 20120515
                                End If
                                SumUtilidadEncajeVenta = SumUtilidadEncajeVenta + UtilidadEncajeVenta

                                If drResumen("MetodoCalculoRenta") = "2" Then
                                    UtilidadEncajeIntDiv = 0
                                    UtilidadEncajeValoracion = 0
                                    If Val(drResumen("UtilidadTotal").ToString) = 0 Then
                                        UtilidadTotal2 = 0
                                    Else
                                        UtilidadTotal2 = Val(drResumen("UtilidadTotal").ToString) * PorcRentEncaje
                                    End If
                                    If Val(drResumen("TotalInteresDividendo").ToString) = 0 Then
                                        UtilidadEncajeIntDiv2 = 0
                                    Else
                                        UtilidadEncajeIntDiv2 = Val(drResumen("TotalInteresDividendo").ToString) * PorcRentEncaje
                                    End If
                                    UtilidadEncajeValorVectorPrecio = UtilidadTotal2 - UtilidadEncajeIntDiv2
                                Else
                                    UtilidadEncajeValorVectorPrecio = 0
                                    If Val(drResumen("UtilidadTotal").ToString) = 0 Then
                                        UtilidadTotal = 0
                                    Else
                                        UtilidadTotal = Val(drResumen("UtilidadTotal").ToString) * PorcRentEncaje
                                    End If
                                    If Val(drResumen("UtilidadVentaTotal").ToString) = 0 Then
                                        UtilidadVentaTotal = 0
                                    Else
                                        UtilidadVentaTotal = Val(drResumen("UtilidadVentaTotal").ToString) * PorcRentEncaje
                                    End If
                                    If Val(drResumen("TotalInteresDividendo").ToString) = 0 Then
                                        UtilidadEncajeIntDiv = 0
                                    Else
                                        UtilidadEncajeIntDiv = Val(drResumen("TotalInteresDividendo").ToString) * PorcRentEncaje
                                    End If
                                    UtilidadEncajeValoracion = (UtilidadTotal - UtilidadVentaTotal) - UtilidadEncajeIntDiv
                                End If
                                SumUtilidadEncajeIntDiv = SumUtilidadEncajeIntDiv + UtilidadEncajeIntDiv
                                SumUtilidadEncajeValoracion = SumUtilidadEncajeValoracion + UtilidadEncajeValoracion
                                SumUtilidadEncajeValorVectorPrecio = SumUtilidadEncajeValorVectorPrecio + UtilidadEncajeValorVectorPrecio    'HDG OT 65195 20120515
                                If Val(drResumen("TotalInteres").ToString) = 0 Then
                                    UtilidadEncajeInt = 0
                                Else
                                    UtilidadEncajeInt = Val(drResumen("TotalInteres").ToString) * PorcRentEncaje
                                End If
                                SumUtilidadEncajeInt = SumUtilidadEncajeInt + UtilidadEncajeInt

                                If Val(drResumen("TotalDividendo").ToString) = 0 Then
                                    UtilidadEncajeDiv = 0
                                Else
                                    UtilidadEncajeDiv = Val(drResumen("TotalDividendo").ToString) * PorcRentEncaje
                                End If
                                SumUtilidadEncajeDiv = SumUtilidadEncajeDiv + UtilidadEncajeDiv
                            End If
                        Next

                        oCells(n, 3) = SumUtilidadEncajeVenta
                        oCells(n, 4) = SumUtilidadEncajeInt
                        oCells(n, 5) = SumUtilidadEncajeDiv
                        oCells(n, 6) = SumUtilidadEncajeValoracion
                        oCells(n, 7) = SumUtilidadEncajeValorVectorPrecio
                        TotSumUtilidadEncajeTotal = TotSumUtilidadEncajeTotal + UtilidadEncajeTotal
                        TotSumUtilidadEncajeTotalB = TotSumUtilidadEncajeTotalB + UtilidadEncajeTotalB
                        TotSumUtilidadEncajeVenta = TotSumUtilidadEncajeVenta + SumUtilidadEncajeVenta
                        TotSumUtilidadEncajeValoracion = TotSumUtilidadEncajeValoracion + SumUtilidadEncajeValoracion
                        drCondicion("Celdas") = drCondicion("Celdas") & "B" & n & "+"
                        n = n + 1
                    End If
                Next
                oCells(n, 1) = "OTROS"
                oCells(n, 2) = RentabilidadEncaje - TotSumUtilidadEncajeTotal
                oCells(n, 3) = (((RentabilidadFondo + RentabilidadEncaje) - TotSumUtilidadEncajeTotalB) - ProvisionValoracion) * PorcRentEncaje
                oCells(n, 4) = 0
                oCells(n, 5) = 0
                oCells(n, 6) = ProvisionValoracion * PorcRentEncaje
                oCells(n, 7) = 0
                CeldasOtros = CeldasOtros & "+B" & n
                n = n + 1
                oCells(n, 1) = "Total general"
                oCells(n, 2) = "=SUMA(B" & k & ":B" & (n - 1) & ")"
                oCells(n, 3) = "=SUMA(C" & k & ":C" & (n - 1) & ")"
                oCells(n, 4) = "=SUMA(D" & k & ":D" & (n - 1) & ")"
                oCells(n, 5) = "=SUMA(E" & k & ":E" & (n - 1) & ")"
                oCells(n, 6) = "=SUMA(F" & k & ":F" & (n - 1) & ")"
                oCells(n, 7) = "=SUMA(G" & k & ":G" & (n - 1) & ")"
                oSheet.Range("A" & k & ":G" & n).Borders.ColorIndex = 1
                oSheet.Range("A" & n & ":G" & n).Font.Bold = True
                n = n + 4
            Next
            Dim dtPortafolio As DataTable = ViewState("tablaportafolio")
            Dim j As Integer = 0
            Dim fondos As String = ""
            For j = 0 To dtPortafolio.Rows.Count - 1
                If fondos = "" Then
                    fondos = fondos & dtPortafolio.Rows(j)("Descripcion").ToString.Trim
                Else
                    fondos = fondos & ", " & dtPortafolio.Rows(j)("Descripcion").ToString.Trim
                End If
            Next
            oCells(n, 1) = "RESUMEN RENTABILIDAD ENCAJE: " & fondos
            oCells(n, 1).HorizontalAlignment = -4108
            oSheet.Range("A" & n & ":G" & n).Merge()
            n = n + 1
            oSheet.Range("A" & n & ":G" & n).WrapText = True
            oSheet.Range("A" & n & ":G" & n).HorizontalAlignment = Excel.Constants.xlCenter
            oSheet.Range("A" & n & ":G" & n).VerticalAlignment = Excel.Constants.xlCenter
            oCells(n, 1) = "CONDICIÓN"
            oCells(n, 2) = "Utilidad Encaje Total"
            oCells(n, 3) = "Utilidad Encaje Venta"
            oCells(n, 4) = "Encaje Intereses"
            oCells(n, 5) = "Encaje Dividendos"
            oCells(n, 6) = "Utilidad Encaje Valoración"
            oCells(n, 7) = "Utilidad Encaje Valoración Vector Precio"
            oSheet.Range("A" & (n - 1) & ":G" & n).Borders.ColorIndex = 1
            oSheet.Range("A" & (n - 1) & ":G" & n).Font.Bold = True
            n = n + 1
            k = n

            For Each drCondicion In dtCondiciones.Rows
                If Not drCondicion("Condicion") Is DBNull.Value Then
                    oCells(n, 1) = drCondicion("Condicion")
                    Celdas = drCondicion("Celdas")
                    Celdas = "=" & Celdas.Substring(0, Celdas.Length - 1)
                    oCells(n, 2) = Celdas
                    oCells(n, 3) = Celdas.Replace("B", "C")
                    oCells(n, 4) = Celdas.Replace("B", "D")
                    oCells(n, 5) = Celdas.Replace("B", "E")
                    oCells(n, 6) = Celdas.Replace("B", "F")
                    oCells(n, 7) = Celdas.Replace("B", "G")
                    n = n + 1
                End If
            Next
            CeldasOtros = "=" & CeldasOtros
            oCells(n, 1) = "OTROS"
            oCells(n, 2) = CeldasOtros
            oCells(n, 3) = CeldasOtros.Replace("B", "C")
            oCells(n, 4) = CeldasOtros.Replace("B", "D")
            oCells(n, 5) = CeldasOtros.Replace("B", "E")
            oCells(n, 6) = CeldasOtros.Replace("B", "F")
            oCells(n, 7) = CeldasOtros.Replace("B", "G")
            n = n + 1
            oCells(n, 1) = "TOTAL"
            oCells(n, 2) = "=SUMA(B" & k & ":B" & (n - 1) & ")"
            oCells(n, 3) = "=SUMA(C" & k & ":C" & (n - 1) & ")"
            oCells(n, 4) = "=SUMA(D" & k & ":D" & (n - 1) & ")"
            oCells(n, 5) = "=SUMA(E" & k & ":E" & (n - 1) & ")"
            oCells(n, 6) = "=SUMA(F" & k & ":F" & (n - 1) & ")"
            oCells(n, 7) = "=SUMA(G" & k & ":G" & (n - 1) & ")"
            oSheet.Range("A" & k & ":G" & n).Borders.ColorIndex = 1
            oSheet.Range("A" & n & ":G" & n).Font.Bold = True
            oBook.Save()
            oBook.Close()
            Dim filepath As String = sFile.Replace("\", "\\")
            Dim filename As String = System.IO.Path.GetFileName(filepath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & filename & """")
            Response.Flush()
            Response.WriteFile(filepath)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
    End Sub
    'OT10689 - Fin.
#End Region

#Region " /* Métodos Personalizados */ "

    Private Sub CargarGrillaPortafolio()
        Dim portafolio As DataTable = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        dgPortafolio.DataSource = portafolio
        dgPortafolio.DataBind()
    End Sub

    Private Sub CargarCombos()
        Dim tablaPortafolio As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        'tablaPortafolio = oPortafolioBM.Listar(Me.DatosRequest, "A").Tables(0)
        tablaPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondo, tablaPortafolio, "CodigoPortafolio", "Descripcion", True)
        ViewState("tablaportafolio") = tablaPortafolio
    End Sub

    Private Sub FechaPortafolio()
        Dim fechaFin As Decimal = UIUtility.ObtenerFechaMaximaNegocio()
        Dim temp As String = fechaFin.ToString.Substring(0, 6) & "01"
        Dim FechaInicio As Decimal = CDec(temp)
        tbFechaFin.Text = UIUtility.ConvertirFechaaString(fechaFin)
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(FechaInicio)
    End Sub

    Private Function Validar() As Boolean
        Dim sFechaInicio As String = tbFechaInicio.Text.Trim
        Dim sFechaFin As String = tbFechaFin.Text.Trim
        If sFechaInicio.Trim.Length < 10 Then
            AlertaJS(ObtenerMensaje("ALERT46"))
            Return False
            Exit Function
        ElseIf sFechaFin.Trim.Length < 10 Then            
            AlertaJS(ObtenerMensaje("ALERT47"))
            Return False
            Exit Function
        ElseIf sFechaInicio.Trim.Length = 0 Then
            AlertaJS(ObtenerMensaje("ALERT20"))
            Return False
            Exit Function
        ElseIf sFechaFin.Trim.Length = 0 Then
            AlertaJS(ObtenerMensaje("ALERT19"))
            Return False
            Exit Function
        End If
        If sFechaInicio.Trim <> "" And sFechaFin.Trim <> "" Then
            If sFechaInicio.Substring(6, 4) & sFechaInicio.Substring(3, 2) & sFechaInicio.Substring(0, 2) > _
                sFechaFin.Substring(6, 4) & sFechaFin.Substring(3, 2) & sFechaFin.Substring(0, 2) Then
                AlertaJS(ObtenerMensaje("ALERT48"))
                Return False
                Exit Function
            End If
        End If
        If sFechaInicio.Trim <> "" Then
            If Not IsDate(sFechaInicio) Then
                AlertaJS(ObtenerMensaje("ALERT44"))
                Return False
                Exit Function
            End If
        End If
        If sFechaFin.Trim <> "" Then
            If Not IsDate(sFechaFin) Then
                AlertaJS(ObtenerMensaje("ALERT45"))
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function

    Private Sub GeneraReporteProvisionContable()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim oReporte As New EncajeDetalleBM
            Dim ds As DataSet
            ds = oReporte.ProvisionContableImpuesto(UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text.Trim()), UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text.Trim()))
            Dim dtDetalle As DataTable = ds.Tables(0)

            Dim sFile As String, sTemplate As String
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "ReporteProvisionContableImpuesto_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"

            If File.Exists(sFile) Then File.Delete(sFile)

            sTemplate = RutaPlantillas() & "\" & "PlantillaProvisionContableImpuesto.xls"

            oExcel.Visible = False : oExcel.DisplayAlerts = False
            Dim p As String = 0
            'Start a new workbook
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate) 'Load colorful template with chart
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheet.Cells

            Dim n As Int32 = 0
            Dim i As Integer

            For i = 0 To dtDetalle.Rows.Count - 1

                oCells(i + 2, 1) = dtDetalle.Rows(i)("CodigoPortafolioSBS") 'CodigoPortafolio
                oCells(i + 2, 2) = dtDetalle.Rows(i)("CodigoTipoInstrumento") 'TipoInstrumento
                oCells(i + 2, 3) = dtDetalle.Rows(i)("CodigoNemonico") 'CodigoNemonico
                oCells(i + 2, 4) = dtDetalle.Rows(i)("UtilidadTotal") 'UtilidadTotal
                oCells(i + 2, 5) = "=D" & (i + 2) & "-F" & (i + 2) 'dtDetalle.Rows(i)(4) 'UtilidadFondo
                oCells(i + 2, 6) = dtDetalle.Rows(i)("UtilidadEncaje") 'UtilidadEncaje
                oCells(i + 2, 7) = dtDetalle.Rows(i)("Condicion") 'Condicion
                oCells(i + 2, 8) = dtDetalle.Rows(i)("FechaEmision")
                n = i
            Next

            oSheet.Range("A1:H" & (n + 2)).Borders.ColorIndex = 1

            oSheet.SaveAs(sFile) 'Save Detalle

            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells

            Dim dtCondiciones As DataTable = UIUtility.SelectDistinct(dtDetalle, "CondicionImpuesto", "Condicion")
            dtCondiciones.Columns.Add("Celdas", GetType(String))
            Dim drCondicion As DataRow
            Dim k As Integer
            Dim NroCondiciones As Integer = dtCondiciones.Rows.Count
            Dim Celdas As String
            n = 3

            For i = 0 To dgPortafolio.Rows.Count - 1
                oCells(n, 1) = dgPortafolio.Rows(i).Cells(1).Text.Trim & " : UTILIDAD ENCAJE"
                n = n + 1
                oCells(n, 1) = "CONDICIÓN"
                oCells(n, 2) = "TOTAL"
                oSheet.Range("A" & (n - 1) & ":B" & n).Borders.ColorIndex = 1
                oSheet.Range("A" & (n - 1) & ":B" & n).Font.Bold = True
                n = n + 1
                k = n

                For Each drCondicion In dtCondiciones.Rows
                    If Not drCondicion("CondicionImpuesto") Is DBNull.Value Then
                        oCells(n, 1) = drCondicion("Condicion")
                        oCells(n, 2) = dtDetalle.Compute("sum(UtilidadEncaje)", "CodigoPortafolioSBS='" & i & "' and CondicionImpuesto=" & drCondicion("CondicionImpuesto"))
                        drCondicion("Celdas") = drCondicion("Celdas") & "B" & n & "+"
                        n = n + 1
                    End If
                Next
                oCells(n, 1) = "Total general"
                oCells(n, 2) = "=SUMA(B" & k & ":B" & (n - 1) & ")"
                oSheet.Range("A" & n & ":B" & n).Borders.ColorIndex = 1
                oSheet.Range("A" & n & ":B" & n).Font.Bold = True
                n = n + 4
            Next

            n = 4
            For Each drCondicion In dtCondiciones.Rows
                If Not drCondicion("CondicionImpuesto") Is DBNull.Value Then
                    oCells(n, 4) = drCondicion("Condicion")
                    Celdas = drCondicion("Celdas")
                    Celdas = Celdas.Substring(0, Celdas.Length - 1)
                    oCells(n, 5) = "=" & Celdas
                    n = n + 1
                End If
            Next
            oCells(n, 4) = "TOTAL"
            oCells(n, 5) = "=SUMA(E4:E" & (n - 1) & ")"
            oSheet.Range("D4:E" & n).Borders.ColorIndex = 1
            oSheet.Range("D" & n & ":E" & n).Font.Bold = True
            oCells(1, 1) = "RENTABILIDAD DEL ENCAJE:  " + ObtenerMes(tbFechaInicio.Text) + " A " + ObtenerMes(tbFechaFin.Text) + " " + tbFechaInicio.Text.Substring(6, 4)
            oBook.Save()
            oBook.Close()
            Dim filepath As String = sFile.Replace("\", "\\")
            Dim filename As String = System.IO.Path.GetFileName(filepath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & filename & """")
            Response.Flush()
            Response.WriteFile(filepath)
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

    Private Function ObtenerMes(ByVal st As String) As String ' OT 61609 REQ 37 20101122 PLD
        Dim Mes As String = st
        Mes = Mes.Substring(3, 2)
        Select Case Mes
            Case "01"
                Return "ENERO"
            Case "02"
                Return "FEBRERO"
            Case "03"
                Return "MARZO"
            Case "04"
                Return "ABRIL"
            Case "05"
                Return "MAYO"
            Case "06"
                Return "JUNIO"
            Case "07"
                Return "JULIO"
            Case "08"
                Return "AGOSTO"
            Case "09"
                Return "SEPTIEMBRE"
            Case "10"
                Return "OCTUBRE"
            Case "11"
                Return "NOVIEMBRE"
            Case "12"
                Return "DICIEMBRE"
        End Select

    End Function

#End Region

End Class
