Imports Sit.BusinessLayer
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data.OleDb
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Tesoreria_Encaje_frmRentabilidadEncaje
    Inherits BasePage

#Region " /* Métodos de la Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If chkIndAnio.Checked Then
                'btnDetallePorInstrumento.Attributes.Add("onclick", "return ValidaFechas();")
            Else
                Dim cadenaScript As String = "<script language='javascript'>$(document).ready(function () { $(""#btnDetallePorInstrumento"").click(function () { ShowProgress(); }); }); </script>"
                Page.RegisterStartupScript("JQueryClickFunction", cadenaScript) 'Migra CMB 20120815
            End If
            If Not Page.IsPostBack Then
                CargarCombos()
                FechaPortafolio()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbNemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
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
            Dim FechaInicio As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            Dim FechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            If Validar() Then
                EjecutarJS("ShowPopupImprimir(" & FechaFin & ");")
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

    Private Sub btnDetallePorInstrumento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetallePorInstrumento.Click
        Try
            If chkIndAnio.Checked Then
                If ValidarFechas() = True Then
                    GeneraReporteDetallePorInstrumento3()
                End If
            Else
                GeneraReporteDetallePorInstrumento()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        End Try
    End Sub

    Private Sub chkIndAnio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIndAnio.CheckedChanged
        Try
            If chkIndAnio.Checked Then
                ddlAnio.Enabled = True
                ddlFondo.Enabled = False
                tbNemonico.Enabled = False
                lkbBuscarMnemonico.Enabled = False
                iptRuta.Disabled = False
            Else
                ddlAnio.Enabled = False
                ddlFondo.Enabled = True
                tbNemonico.Enabled = True
                lkbBuscarMnemonico.Enabled = True
                iptRuta.Disabled = True
            End If
            ddlAnio.SelectedValue = SALDO_ANIO_ANTERIOR
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

#End Region

#Region " /* Métodos Personalizados */ "

    Private Sub CargarCombos()
        Dim tablaPortafolio As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        tablaPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondo, tablaPortafolio, "CodigoPortafolio", "Descripcion", False)
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As New DataTable
        dt = oParametrosGeneralesBM.Listar("ANIOSALINI", DatosRequest)
        HelpCombo.LlenarComboBox(ddlAnio, dt, "Valor", "Nombre", False)
        ddlAnio.SelectedValue = "2009"
    End Sub

    Private Sub FechaPortafolio()
        Dim fechaFin As Decimal = UIUtility.ObtenerFechaMaximaNegocio()
        Dim temp As String = fechaFin.ToString.Substring(0, 6) & "01"
        Dim FechaInicio As Decimal = CDec(temp)
        tbFechaFin.Text = UIUtility.ConvertirFechaaString(fechaFin)
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(FechaInicio)
    End Sub

    Private Function ValidarFechas() As Boolean
        Dim FechaIni As String = tbFechaInicio.Text.Trim
        Dim FechaFin As String = tbFechaFin.Text.Trim
        Dim len As String = FechaFin.Length
        Dim AnioIni As String = FechaIni.Substring(6, 4)
        Dim Anio As String = ddlAnio.SelectedValue
        Dim FecIni As String = FechaIni.Substring(6, 4) + FechaIni.Substring(3, 2) + FechaIni.Substring(0, 2)
        Dim FecFin As String = FechaFin.Substring(6, 4) + FechaFin.Substring(3, 2) + FechaFin.Substring(0, 2)

        If FecIni > FecFin Then
            AlertaJS("La fecha inicio debe ser menor o igual a la fecha fin")
            Return False
        ElseIf AnioIni <= Anio Then
            AlertaJS("El rango de fechas inicio y fin debe ser mayor al saldo inicial del año anterior")
            Return False
        Else
            Return True
        End If
    End Function

    Private Function Validar() As Boolean
        Dim sFechaInicio As String = tbFechaInicio.Text.Trim
        Dim sFechaFin As String = tbFechaFin.Text.Trim
        If Me.tbNemonico.Text = "" Then
            AlertaJS(ObtenerMensaje("ALERT113"))
            Return False
            Exit Function
        End If
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

    Private Sub GeneraReporteDetallePorInstrumento()
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
            Dim dtOperaciones As DataTable = UIUtility.GeneraTablaDetallePorInstrumento(tbNemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), ddlFondo.SelectedValue, DatosRequest)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "ReporteDetalleRentabilidadEncajePorInstrumento" & "_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".xls"
            Dim n As Integer
            Dim dr As DataRow
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaDetalleRentabilidadEncajePorInstrumento.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            Dim p As String = 0
            'Start a new workbook
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(1, 2) = tbNemonico.Text
            oCells(2, 2) = ddlFondo.SelectedItem.Text
            oSheet.SaveAs(sFile)
            n = 6
            Dim TotalVentas As Decimal = 0
            Dim TotalVentasMO As Decimal = 0
            For Each dr In dtOperaciones.Rows
                oCells(n, 1) = dr("Fecha")

                Select Case dr("TipoOperacion")
                    Case "SALDO_INICIAL"
                        oCells(n, 2) = dr("Unidades")
                        oCells(n, 3) = dr("Precio")
                        oCells(n, 4) = dr("TipoCambio")
                        oCells(n, 5) = "=B" & n & "*C" & n & "*D" & n
                        oCells(n, 6) = "=B" & n & "*C" & n
                    Case "COMPRA"
                        oCells(n, 2) = dr("Unidades")
                        oCells(n, 3) = dr("Precio")
                        oCells(n, 4) = dr("TipoCambio")
                        oCells(n, 5) = "=B" & n & "*C" & n & "*D" & n
                        oCells(n, 6) = "=B" & n & "*C" & n

                        'KARDEX
                        oCells(n, 12) = dr("UnidadesKardex")
                        oCells(n, 13) = "=N" & n & "/L" & n
                        oCells(n, 14) = dr("ImporteKardex")
                        oCells(n, 15) = "=P" & n & "/L" & n
                        oCells(n, 16) = dr("ImporteKardexMO")
                    Case "VENTA"
                        oCells(n, 7) = dr("Unidades")
                        oCells(n, 8) = dr("Precio")
                        oCells(n, 18) = dr("PrecioMO")
                        oCells(n, 9) = dr("TipoCambio")
                        oCells(n, 10) = "=G" & n & "*H" & n & "*I" & n
                        oCells(n, 11) = "=G" & n & "*R" & n
                    Case "TOTAL_VENTAS"
                        TotalVentas = dr("ImporteKardex")
                        TotalVentasMO = dr("ImporteKardex")
                End Select

                n = n + 1
            Next

            '********************** INI CMB OT 61609 20101124 **********************
            Dim intSuma As Integer = n
            oCells(n, 2) = "=SUMA(B7:B" & (n - 1) & ")"
            oCells(n, 5) = "=SUMA(E7:E" & (n - 1) & ")"
            oCells(n, 6) = "=SUMA(F7:F" & (n - 1) & ")"
            oCells(n, 7) = "=SUMA(G7:G" & (n - 1) & ")"
            oCells(n, 10) = "=SUMA(J7:J" & (n - 1) & ")"
            oCells(n, 11) = "=SUMA(K7:K" & (n - 1) & ")"
            'Titulos
            n = n + 2
            oCells(n, 2) = "RESUMEN"
            oCells(n, 3) = "UNIDADES"
            oCells(n, 4) = "PRECIO"
            oCells(n, 5) = "IMPORTE S/."
            oCells(n, 6) = "IMPORTE"
            n = n + 1

            Dim intSaldoInicial As Integer = 0
            intSaldoInicial = n
            oCells(n, 1) = "=A6"
            oCells(n, 2) = "S.Inicial"
            oCells(n, 3) = "=B6"
            oCells(n, 4) = "=SI(C" & n & "=0,0,F" & n & "/C" & n & ")"
            oCells(n, 5) = "=E6"
            oCells(n, 6) = "=F6"
            oSheet.Range(oCells(n, 4), oCells(n, 4)).NumberFormat = "###,###,##0.0000000"
            oSheet.Range(oCells(n, 5), oCells(n, 6)).NumberFormat = "###,###,##0.00"
            n = n + 1

            'Compra
            Dim intCompra As Integer = n
            oCells(n, 2) = "Total Compra"
            oCells(n, 3) = "=B" & intSuma
            'oCells(n, 4) = ""
            oCells(n, 5) = "=E" & intSuma
            oCells(n, 6) = "=F" & intSuma
            oSheet.Range(oCells(n, 5), oCells(n, 6)).NumberFormat = "###,###,##0.00"
            oSheet.Range(oCells(n, 3), oCells(n, 3)).NumberFormat = "###,###,##0"
            n = n + 1

            'Venta
            Dim intVenta As Integer = n
            oCells(n, 2) = "Costo Venta"
            oCells(n, 3) = "=G" & intSuma
            oCells(n, 5) = "=J" & intSuma
            oCells(n, 6) = "=K" & intSuma
            oCells(n, 7) = "(B)"
            oSheet.Range(oCells(n, 5), oCells(n, 6)).NumberFormat = "###,###,##0.00"
            oSheet.Range(oCells(n, 3), oCells(n, 3)).NumberFormat = "###,###,##0"
            n = n + 1

            'Saldo Final
            Dim intSaldoFinal As Integer = n
            oCells(n, 1) = tbFechaFin.Text
            oCells(n, 2) = "S. Final"
            oCells(n, 3) = "=C" & intSaldoInicial & "+C" & intCompra & "-C" & intVenta
            oCells(n, 4) = "=SI(C" & intSaldoFinal & "=0,0,F" & intSaldoFinal & "/C" & intSaldoFinal & ")"
            oCells(n, 5) = "=E" & intSaldoInicial & "+E" & intCompra & "-E" & intVenta
            oCells(n, 6) = "=F" & intSaldoInicial & "+F" & intCompra & "-F" & intVenta
            oSheet.Range(oCells(n, 5), oCells(n, 6)).NumberFormat = "###,###,##0.00"
            oSheet.Range(oCells(n, 3), oCells(n, 3)).NumberFormat = "###,###,##0"
            oSheet.Range(oCells(n, 4), oCells(n, 4)).NumberFormat = "###,###,##0.0000000"
            n = n + 2

            'Venta (A)
            Dim intVentaTotal As Integer = n
            oCells(n, 2) = "Venta"
            oSheet.Range(oCells(n, 2), oCells(n, 2)).Font.Bold = True
            oCells(n, 3) = "=G" & intSuma
            oCells(n, 5) = TotalVentas
            oCells(n, 7) = "(A)"
            oSheet.Range(oCells(n, 5), oCells(n, 5)).NumberFormat = "###,###,##0.00"
            oSheet.Range(oCells(n, 3), oCells(n, 3)).NumberFormat = "###,###,##0"
            n = n + 2

            'Resumen
            'Obtener Utilidades
            Dim dtUtilidades As DataTable
            dtUtilidades = New EncajeDetalleBM().ObtenerUtilidadPorNemonico(ddlFondo.SelectedValue.ToString, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text), tbNemonico.Text, DatosRequest).Tables(0)
            'Titulos
            oCells(n, 5) = "UTILIDAD TOTAL"
            oSheet.Range(oCells(n, 5), oCells(n, 7)).Font.Bold = True
            n = n + 1
            'Fila - Rentabilidad Total
            Dim intRentabilidadTotal As Integer = n
            Dim fechaInicio As DateTime = CType(tbFechaInicio.Text, DateTime)
            Dim fechaFin As DateTime = CType(tbFechaFin.Text, DateTime)
            Dim mesFechaInicio As String = fechaInicio.ToString("m", Global.System.Globalization.CultureInfo.CurrentCulture)
            Dim mesFechaFin As String = fechaFin.ToString("m", Global.System.Globalization.CultureInfo.CurrentCulture)
            oSheet.Range(oCells(n, 3), oCells(n, 4)).Merge()
            oCells(n, 3) = "Rentabilidad Total (" & mesFechaInicio.Substring(3, 3) & " - " & mesFechaFin.Substring(3, 3) & ")"
            If dtUtilidades.Rows.Count > 0 Then
                oCells(n, 5) = dtUtilidades.Rows(0)(0)
                oSheet.Range(oCells(n, 3), oCells(n, 7)).Font.Bold = True
                oSheet.Range(oCells(n, 5), oCells(n, 7)).NumberFormat = "###,###,##0.00"
            End If
            oSheet.Range(oCells(n, 3), oCells(n, 5)).BorderAround()
            n = n + 1
            Dim intRentabilidadVenta As Integer = n
            oSheet.Range(oCells(n, 3), oCells(n, 4)).Merge()
            oCells(n, 3) = "Rentabilidad por Venta (A - B)"
            oCells(n, 5) = "=E" & intVentaTotal & "-E" & intVenta
            oSheet.Range(oCells(n, 5), oCells(n, 7)).NumberFormat = "###,###,##0.00"
            n = n + 1

            'Fila - Rentabilidad Valoración
            oSheet.Range(oCells(n, 3), oCells(n, 4)).Merge()
            oCells(n, 3) = "Rentabilidad Valoración"
            oCells(n, 5) = "=E" & intRentabilidadTotal & "-E" & intRentabilidadVenta
            oSheet.Range(oCells(n, 5), oCells(n, 7)).NumberFormat = "###,###,##0.00"
            '********************** FIN CMB OT 61609 20101124 **********************
            oBook.Save()
            oBook.Close()
            EjecutarJS("DownloadFile('" & sFile.Replace("\", "\\") & "');")
            Dim url As String = sFile.Replace("\", "\\")
            Dim path__1 As String = url
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

    Private Sub GeneraReporteDetallePorInstrumento3()
        Dim strRuta As String = (New ParametrosGeneralesBM().Listar("RUTA_TEMP", Nothing)).Rows(0)("Valor")
        If Not iptRuta.Value.ToString.Trim.Equals("") Then
            CargarArchivoOrigen(strRuta)
            ReporteEncajeJob()
        Else
            AlertaJS("Especifique la ruta de un archivo")
        End If
    End Sub

    Private Sub CargarArchivoOrigen(ByVal strRuta As String)
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
            iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)
            If Dir(strRuta & "\" & fInfo.Name) <> "" Then
                If fInfo.Extension = ".xls" Then
                    Try
                        LeerExcel(strRuta & "\" & fInfo.Name)
                    Catch ex As Exception
                        AlertaJS(ex.Message.ToString)
                    Finally
                        File.Delete(strRuta & "\" & fInfo.Name)
                    End Try
                Else
                    If fInfo.Extension.Equals("") Then
                        AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
                    Else
                        File.Delete(strRuta & "\" & fInfo.Name)
                        AlertaJS("El tipo de archivo no es válido")
                    End If
                End If
            Else
                File.Delete(strRuta & "\" & fInfo.Name)
                AlertaJS("El archivo no existe")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub LeerExcel(ByVal strRuta)
        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub CargarArchivo(ByVal strRuta As String)
        Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim strMensaje As String = ""
        Try
            oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
            'oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()
            oCmd.CommandText = "SELECT DISTINCT [Instrumento], [Fondo] FROM [Instrumentos$] "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "Instrumentos")
            ActualizarInstrumentosPorExcel(oDs.Tables(0), "Instrumentos", strMensaje)
            oConn.Close()
        Catch ex As OleDbException
            Select Case ex.ErrorCode
                Case -2147467259
                    AlertaJS("El excel en este momento se encuentra abierto")
                Case -2147217865
                    AlertaJS("Error al leer el archivo Excel")
                Case Else
                    AlertaJS("Ocurrió un error no esperado. Revisar el archivo Excel")
            End Select
            oConn.Close()
        Catch ex As Exception
            AlertaJS("Error al leer el archivo Excel")
            oConn.Close()
        End Try
    End Sub

    Private Sub ActualizarInstrumentosPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oEncajeDetalleBM As New EncajeDetalleBM
            If data.Columns.Count = 2 Then
                oEncajeDetalleBM.EliminarTablaTmpNemonicosFondoRenta(DatosRequest)
                If oEncajeDetalleBM.ActualizarInstrumentosPorExcel(data, DatosRequest, strmensaje) Then
                    'strmensaje &= "Los datos de " & strReferencia & " cargaron correctamente\n"
                Else
                    strmensaje &= "Los datos de " & strReferencia & " son inconsistentes \n"
                End If
            Else
                strmensaje &= "La pestaña " & strReferencia & " no tiene la estructura adecuada\n"
            End If
        End If
    End Sub

    Private Sub ReporteEncajeJob()
        Dim PrefijoFolder As String = "RepRenMnemo_"
        Dim RutaTemp As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
        Dim fechaActual As String = System.DateTime.Now.ToString("yyyyMMdd")
        Dim foldersAsesoria() As String = Directory.GetDirectories(RutaTemp, PrefijoFolder & "*")
        'Dim folderActual As String = RutaTemp & PrefijoFolder & fechaActual & "\"
        Dim folderActual As String = PrefijoFolder & fechaActual    'Migra HDG 20120807
        Dim RutafolderActual As String = RutaTemp & folderActual & "\"  'Migra HDG 20120807
        Dim cont As Integer
        Try
            For cont = 0 To foldersAsesoria.Length - 1
                Directory.Delete(foldersAsesoria(cont), True)
            Next
            Directory.CreateDirectory(RutafolderActual) 'Migra HDG 20120807
        Catch ex As Exception
        End Try

        Dim dFechaini As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        Dim dFechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
        Dim sAnio As String = ddlAnio.SelectedValue
        Dim dt As DataTable = DatosRequest.Tables(0)
        Dim sUsuario As String = CType(dt.Select(dt.Columns(0).ColumnName & "='Usuario'")(0)(1), String)
        Dim Variable As String = "TmpRutaDestino,TmpFechaIni,TmpFechaFin,TmpAnioSaldo,TmpUsuario"
        Dim Parametros As String = folderActual + "," + dFechaini.ToString + "," + dFechaFin.ToString + "," + sAnio + "," + sUsuario
        Dim obj As New JobBM

        Me.lblTime.Text = obj.EjecutarJob("DTS_SIT_RepRentabDetMnemo" & DateTime.Today.ToString("_yyyyMMdd") & System.DateTime.Now.ToString("_hhmmss"), "Procesa en Masivo Reportes de Rentabilidad Detallado por Mnemónico", Variable, Parametros, "", "", ConfigurationSettings.AppSettings(SERVIDORETL))

    End Sub

#End Region

End Class

