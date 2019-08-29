'OT 10362 - 18/05/2017 - Carlos Espejo
'Descripcion: Reporte de Precios por instrumentos
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office
Partial Class Modulos_Inversiones_Reportes_frmPrecioInstrumento
    Inherits BasePage
    Dim oValoresBM As New ValoresBM
    Function Rango(ByVal Numero As Integer) As String
        Dim array() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                 "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV",
                                 "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS",
                                 "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP",
                                 "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM",
                                 "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ",
                                 "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ", "FA", "FB", "FC", "FD", "FE", "FF", "FG",
                                 "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ"}
        Return array(Numero - 1)
    End Function
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Session("SS_DatosModal") Is Nothing Then
            Dim datos As String()
            datos = CType(Session("SS_DatosModal"), String())
            txtCodIsin.Text = HttpUtility.HtmlDecode(CType(Session("SS_DatosModal"), String())(0).ToString())
            txtnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
            Session.Remove("SS_DatosModal")
        End If
        If Not Page.IsPostBack Then
            tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaFin.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
        End If
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As System.EventArgs) Handles btnAgregar.Click
        Try
            Dim IT As New ListItem
            If Not (txtCodIsin.Text = "" Or txtnemonico.Text = "") Then
                If BuscarRepetido(txtCodIsin.Text) Then
                    IT.Value = txtCodIsin.Text
                    IT.Text = txtnemonico.Text
                    lbxValor.Items.Add(IT)
                    txtCodIsin.Text = ""
                    txtnemonico.Text = ""
                Else
                    AlertaJS("Instrumento ya en lista.")
                End If
            Else
                AlertaJS("Debe usar el boton con la imagen de una lupa para buscar el instrumento.", "$('#lkbModalMnemonico').focus();")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Function BuscarRepetido(Valor As String) As Boolean
        For Each LI As ListItem In lbxValor.Items
            If Valor = LI.Value Then
                Return False
            End If
        Next
        Return True
    End Function
    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim IT As New ListItem
            If tbFechaInicio.Text = "" Then
                AlertaJS("Ingresa la fecha Inicial.")
            ElseIf tbFechaFin.Text = "" Then
                AlertaJS("Ingresa la fecha Final.")
            ElseIf lbxValor.Items.Count = 0 Then
                AlertaJS("Seleccione al menos un ISIN valido.")
            Else
                'Borrar datos anteriores
                oValoresBM.BorrarPrecioISINDetalle(Usuario)
                'Ingresa instumentos a tabla temporal
                For Each LI As ListItem In lbxValor.Items
                    oValoresBM.InsertarPrecioISINDetalle(Usuario, LI.Value, LI.Text)
                Next
                'Generar Reporte
                GenerarReporte()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range, Hoja As Integer)
        If Hoja = 1 Then
            'Fechas
            oCells(2, 4) = tbFechaInicio.Text
            oCells(3, 4) = tbFechaFin.Text
            'Lista de instrumentos
            For Each LI As ListItem In lbxValor.Items
                oCells(FilaInicial, 3) = LI.Value
                FilaInicial += 1
            Next
        Else
            'Lista de instrumentos
            Dim DTFiltrado As DataTable, Columna As Integer, ColumnaExcel As Integer, FilaCargado As Integer
            ColumnaExcel = 2
            Dim DTOrden As DataTable = oValoresBM.ListaOrdenPrecioInstrumento(Usuario)
            For Each DROrden As DataRow In DTOrden.Rows
                FilaCargado = FilaInicial
                'Consulta de filtrado
                Dim query = From order In dt.AsEnumerable() Where order.Field(Of String)("CodigoIsin") = DROrden("CodigoIsin") Select order
                If Not (query Is Nothing Or query.Count() = 0) Then
                    DTFiltrado = query.CopyToDataTable
                    If ColumnaExcel > 2 Then
                        oSheet.Range("C2:I" + (DTFiltrado.Rows.Count - 1 + FilaInicial).ToString).Copy( _
                        oSheet.Range(Rango(ColumnaExcel + 1) + "2:" + Rango(ColumnaExcel + 6) + (DTFiltrado.Rows.Count - 1 + FilaInicial).ToString))
                    End If
                    For Each dr In DTFiltrado.Rows
                        oCells(FilaCargado, 2) = dr(0)
                        Columna = 1
                        Do While Columna <= DTFiltrado.Columns.Count - 1
                            oCells(FilaCargado, Columna + ColumnaExcel) = dr(Columna)
                            Columna = Columna + 1
                        Loop
                        'Copiar los formatos
                        FilaCargado += 1
                        If ColumnaExcel = 2 And FilaCargado < DTFiltrado.Rows.Count + FilaInicial Then
                            oSheet.Range("B3:I3").Copy(oSheet.Range(Rango(ColumnaExcel) + FilaCargado.ToString + ":" + Rango(ColumnaExcel + 6) + FilaCargado.ToString))
                        End If
                    Next
                    ColumnaExcel += DTFiltrado.Columns.Count - 1
                End If
            Next
        End If
    End Sub
    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtCE As DataTable = oValoresBM.ListaPrecioInstrumento(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), _
            UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), Usuario)
            If dtCE.Rows.Count > 0 Then
                Dim sFile As String, sTemplate As String
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "CXC_" & Usuario.ToString() & _
                String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                Dim Indice As Integer = 3
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "PlantillaVariacionTasasPIP.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                'Hoja 1
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                DatosExcel(dtCE, 7, oSheet, oCells, 1)
                'Hoja 2
                oSheet = CType(oSheets.Item(2), Excel.Worksheet)
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                DatosExcel(dtCE, 3, oSheet, oCells, 2)
                oExcel.Cells.EntireColumn.AutoFit()
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "CM_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & _
                String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
                Response.WriteFile(sFile)
                Response.End()
            Else
                AlertaJS("No existen registros que mostrar con los filtros ingresados.")
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
    Protected Sub btnBorrar_Click(sender As Object, e As System.EventArgs) Handles btnBorrar.Click
        Try
            lbxValor.Items.RemoveAt(lbxValor.SelectedIndex)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub btnRegresar_Click(sender As Object, e As System.EventArgs) Handles btnRegresar.Click
        lbxValor.Items.Clear()
    End Sub
End Class