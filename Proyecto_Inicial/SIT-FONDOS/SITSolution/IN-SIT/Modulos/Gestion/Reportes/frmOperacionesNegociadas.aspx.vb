Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.IO
Imports Microsoft.Office
Imports ParametrosSIT
Imports System.Data
Imports System.Threading
Imports System.Globalization

Partial Class Modulos_Gestion_Reportes_frmOperacionesNegociadas
    Inherits BasePage

#Region "Inicializador"
    Dim oUtilDM As New UtilDM
    Dim sFiles As String = ""
#End Region

#Region "Metodos Personalizados"
    Private Sub CargarFechas()
        tbFechaInicio.Text = oUtilDM.RetornarFechaNegocio()
        tbFechaFin.Text = String.Format("{0:dd/MM/yyyy}", Date.Today)
    End Sub

    Public Sub CargarTipoRenta()
        Dim tablaTipoRenta As New Data.DataTable
        Dim oTipoRentaBM As New TipoRentaBM

        tablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", True)
        ddlTipoRenta.SelectedValue = TR_DERIVADOS
        ddlTipoRenta.SelectedItem.Text = TIPO_RENTA_DERIVADOS
        ddlTipoRenta.SelectedValue = TR_RENTA_VARIABLE
    End Sub

    Public Sub CargarOperadores(Optional ByVal pTipoRenta As String = "")
        Dim oRolAprobadoresTraderBM As New RolAprobadoresTraderBM
        Dim strTipoRenta As String = IIf(ddlTipoRenta.SelectedValue = TR_DERIVADOS.ToString, TR_RENTA_FIJA, (IIf(ddlTipoRenta.SelectedValue = Constantes.M_STR_TEXTO_TODOS, Constantes.M_STR_TEXTO_INICIAL, ddlTipoRenta.SelectedValue)))

        If pTipoRenta <> Constantes.M_STR_TEXTO_INICIAL Then
            strTipoRenta = pTipoRenta
        End If
        dgLista.DataSource = oRolAprobadoresTraderBM.SeleccionarOperadores(strTipoRenta)
        dgLista.DataBind()
    End Sub

    Private Sub GenerarReporte()
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            Dim NomHojas As New StringBuilder
            Dim sHojas As String()
            Dim rutas As New StringBuilder
            Dim sFiles As String = ""
            Dim sFile As String = ""
            Dim sTemplate As String, sNomFile As String
            Dim sFecha As String, sHora As String, sRutaTemp As String
            Dim oDs As New DataSet
            Dim dtNegociaciones As New DataTable
            Dim dtPatrimonio As New DataTable
            Dim dr As DataRow
            Dim oLimiteIntermediarioBM As New LimiteIntermediarioBM
            Dim decFechaInicio As Decimal = 0
            Dim decFechaFin As Decimal = 0
            Dim tipoRenta As String = ""
            Dim codigoUsuario As String = ""
            Dim Mercado As String = ""
            Dim oUsuario As New UsuariosNotificaBE
            Dim oRow As UsuariosNotificaBE.UsuariosNotificaRow

            Dim n As Integer = 0
            Dim n2 As Long = 0
            Dim htPortafolio As New Hashtable
            Dim oldCulture As CultureInfo
            'OT10689 - Inicio. Kill process excel
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)

            oldCulture = Thread.CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)

            decFechaInicio = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
            decFechaFin = UIUtility.ConvertirFechaaDecimal(tbFechaFin.Text)
            tipoRenta = IIf(ddlTipoRenta.SelectedValue = Constantes.M_STR_TEXTO_TODOS, Constantes.M_STR_TEXTO_INICIAL, ddlTipoRenta.SelectedValue)

            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks

            Select Case tipoRenta
                Case TR_RENTA_VARIABLE
                    sTemplate = RutaPlantillas() & "\" & "PlantillaReporteOperacionxTraderRV.xls"
                Case TR_RENTA_FIJA
                    sTemplate = RutaPlantillas() & "\" & "PlantillaReporteOperacionxTraderRF.xls"
                Case TR_DERIVADOS
                    sTemplate = RutaPlantillas() & "\" & "PlantillaReporteOperacionxTraderFX.xls"
                Case Else
                    sTemplate = RutaPlantillas() & "\" & "PlantillaReporteOperacionxTraderRF.xls"
            End Select

            sFecha = String.Format("{0:yyyyMMdd}", DateTime.Today)
            sRutaTemp = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
            For Each fila As GridViewRow In dgLista.Rows
                htPortafolio = New Hashtable
                If fila.RowType = DataControlRowType.DataRow Then
                    If CType(fila.FindControl("chkSeleccion"), CheckBox).Checked = True Then

                        codigoUsuario = fila.Cells(1).Text

                        If codigoUsuario = REP_CONSOLIDADO Then codigoUsuario = ""

                        oDs = oLimiteIntermediarioBM.GenerarReporteOperacionesNegociadas(decFechaInicio, decFechaFin, tipoRenta, codigoUsuario)

                        codigoUsuario = fila.Cells(1).Text

                        If oDs.Tables(0).Rows.Count > 0 Then

                            dtNegociaciones = oDs.Tables(0)
                            dtPatrimonio = oDs.Tables(1)
                            sHora = String.Format("{0:HHMMss}", DateTime.Now)
                            sNomFile = "ON_" & Usuario.ToString() & sFecha & sHora & ".xls"
                            sFile = sRutaTemp & sNomFile

                            If File.Exists(sFile) Then File.Delete(sFile)

                            oBooks.Open(sTemplate)
                            oBook = oBooks.Item(1)

                            oSheets = oBook.Worksheets
                            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                            oCells = oSheet.Cells

                            oSheet.SaveAs(sFile)

                            Dim primerFondo As Boolean = True
                            Dim _col As Integer = 3
                            Dim i As Integer = 0
                            Dim _letra As String = "", _letra2 As String = ""
                            Dim _totalFondos As Integer = oDs.Tables(2).Rows.Count
                            Dim intemediario As String = ""
                            'Registrar las columnas de los fondos
                            For Each drPortafolio As DataRow In oDs.Tables(2).Rows
                                If primerFondo Then
                                    primerFondo = False
                                    htPortafolio.Add(drPortafolio("CodigoPortafolioSBS").ToString & "C", _col)
                                    htPortafolio.Add(drPortafolio("CodigoPortafolioSBS").ToString & "V", _col + _totalFondos + 1)
                                Else
                                    htPortafolio.Add(drPortafolio("CodigoPortafolioSBS").ToString & "C", _col)
                                    htPortafolio.Add(drPortafolio("CodigoPortafolioSBS").ToString & "V", _col + _totalFondos + 1)
                                    _letra = UIUtility.ObtenerLetra(_col - 1)
                                    oSheet.Columns(_letra & ":" & _letra).Copy()
                                    _letra = UIUtility.ObtenerLetra(_col)
                                    oSheet.Columns(_letra & ":" & _letra).Insert(Excel.XlInsertShiftDirection.xlShiftToRight, Nothing)
                                    _letra = UIUtility.ObtenerLetra((2 * _col) - 2)
                                    oSheet.Columns(_letra & ":" & _letra).Copy()
                                    _letra = UIUtility.ObtenerLetra((2 * _col) - 1)
                                    oSheet.Columns(_letra & ":" & _letra).Insert(Excel.XlInsertShiftDirection.xlShiftToRight, Nothing)
                                End If
                                oCells(8, _col).value = drPortafolio("Descripcion")
                                oCells(8, (2 * _col) - 1).value = drPortafolio("Descripcion")
                                _col += 1
                            Next
                            _letra = UIUtility.ObtenerLetra(_totalFondos + 2)
                            oCells.Range("C6", _letra & "6").Merge()
                            _letra = UIUtility.ObtenerLetra(_totalFondos + 4)
                            _letra2 = UIUtility.ObtenerLetra((2 * _totalFondos) + 3)
                            oCells.Range(_letra & "6", _letra2 & "6").Merge()
                            oCells(11, (2 * _totalFondos) + 5).Formula = String.Format("=SUM(C11:{0}11)", _letra2)
                            oCells(15, (2 * _totalFondos) + 5).Formula = String.Format("=SUM(C15:{0}15)", _letra2)


                            oCells(2, (2 * _totalFondos) + 14) = dtPatrimonio.Rows(0)(0)

                            If codigoUsuario = REP_CONSOLIDADO Then

                                oCells(2, 3) = REP_CONSOLIDADO
                                oCells(1, 2) = oCells(1, 2).Value & tbFechaInicio.Text & " al " & tbFechaFin.Text

                                n = 11
                                Mercado = MERCADO_LOCAL

                                For Each dr In dtNegociaciones.Rows
                                    If dr("mercado") <> Mercado Then
                                        oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
                                        Mercado = dr("mercado")
                                        n += 3
                                    End If
                                    If intemediario <> dr("NombreIntermediario") Then
                                        intemediario = dr("NombreIntermediario")
                                        n2 = n + 1
                                        oSheet.Rows(n & ":" & n).Copy()
                                        oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                                        oSheet.Application.CutCopyMode = False
                                        oCells(n, 2).value = dr("NombreIntermediario")
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "C")) = dr("MM_C")
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "V")) = dr("MM_V")
                                        oCells(n, (2 * _totalFondos) + 6).value = dr("PBS")
                                        oCells(n, (2 * _totalFondos) + 7).value = dr("Comision")
                                        If tipoRenta = TR_RENTA_VARIABLE Then
                                            oCells(n, (2 * _totalFondos) + 10).value = dr("Limite")
                                        End If
                                        n += 1
                                    Else
                                        n -= 1
                                        Dim numero As Decimal = ToNullDecimal(oCells(n, (2 * _totalFondos) + 6).value)
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "C")) = dr("MM_C")
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "V")) = dr("MM_V")

                                        oCells(n, (2 * _totalFondos) + 6).value = numero + ToNullDecimal(dr("PBS"))
                                        oCells(n, (2 * _totalFondos) + 7).value += dr("Comision")
                                        n += 1
                                    End If
                                Next
                                oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)

                                oBook.Save()
                                oBook.Close()
                                rutas.Append(sFile & "&")
                                NomHojas.Append("Total" & "&")
                            Else

                                If Not oRow Is Nothing Then
                                    oRow.Delete()
                                End If

                                oRow = CType(oUsuario.UsuariosNotifica.NewRow(), UsuariosNotificaBE.UsuariosNotificaRow)
                                oRow.CodigoUsuario = codigoUsuario
                                oRow.Nombre = Constantes.M_STR_TEXTO_INICIAL
                                oRow.Apellido = Constantes.M_STR_TEXTO_INICIAL
                                oRow.CodigoCentroCosto = Constantes.M_STR_TEXTO_INICIAL
                                oUsuario.UsuariosNotifica.AddUsuariosNotificaRow(oRow)
                                oUsuario.UsuariosNotifica.AcceptChanges()

                                oCells(2, 3) = New LimiteBM().SeleccionarPersonal(oUsuario).Rows(0)("Nombre")
                                oCells(1, 2) = oCells(1, 2).Value & tbFechaInicio.Text & " al " & tbFechaFin.Text

                                n = 11
                                Mercado = MERCADO_LOCAL
                                For Each dr In dtNegociaciones.Rows
                                    If dr("mercado") <> Mercado Then
                                        oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
                                        Mercado = dr("mercado")
                                        n = n + 3
                                    End If
                                    If intemediario <> dr("NombreIntermediario") Then
                                        intemediario = dr("NombreIntermediario")
                                        n2 = n + 1
                                        oSheet.Rows(n & ":" & n).Copy()
                                        oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                                        oSheet.Application.CutCopyMode = False
                                        oCells(n, 2).value = dr("NombreIntermediario")
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "C")) = dr("MM_C")
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "V")) = dr("MM_V")
                                        oCells(n, (2 * _totalFondos) + 6).value = dr("PBS")
                                        oCells(n, (2 * _totalFondos) + 7).value = dr("Comision")
                                        If tipoRenta = TR_RENTA_VARIABLE Then
                                            oCells(n, (2 * _totalFondos) + 10).value = dr("Limite")
                                        End If
                                        n += 1
                                    Else
                                        n -= 1
                                        Dim m As Decimal = ToNullDecimal(oCells(n, (2 * _totalFondos) + 6).value)
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "C")) = dr("MM_C")
                                        oCells(n, htPortafolio(dr("codigoPortafolioSBS") & "V")) = dr("MM_V")
                                        oCells(n, (2 * _totalFondos) + 6).value = m + ToNullDecimal(dr("PBS"))
                                        oCells(n, (2 * _totalFondos) + 7).value += dr("Comision")
                                        n += 1
                                    End If
                                Next
                                oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)

                                oBook.Save()
                                oBook.Close()
                                rutas.Append(sFile & "&")
                                NomHojas.Append(codigoUsuario & "&")
                            End If
                        End If
                    End If
                End If
            Next

            Dim nombreNuevoArchivo As String
            Dim oBaseBook As Excel.Workbook
            Dim PrefijoFolder As String = "RON_"
            Dim foldersAsesoria() As String = Directory.GetDirectories(sRutaTemp, PrefijoFolder & "*")
            Dim folderActual As String = sRutaTemp & PrefijoFolder & sFecha
            Dim cont As Integer

            sFiles = rutas.ToString
            If sFiles <> Constantes.M_STR_TEXTO_INICIAL Then
                For cont = 0 To foldersAsesoria.Length - 1
                    If Not foldersAsesoria(cont).Equals(folderActual) Then
                        Try
                            Directory.Delete(foldersAsesoria(cont), True)
                        Catch ex As Exception
                        End Try
                    End If
                Next
                If Not Directory.Exists(folderActual) Then
                    Directory.CreateDirectory(folderActual)
                End If

                sHora = String.Format("{0:HHMMss}", DateTime.Now)
                nombreNuevoArchivo = PrefijoFolder & sFecha & sHora & ".xls"
                Dim rutaArchivo = sRutaTemp & nombreNuevoArchivo


                If File.Exists(rutaArchivo) Then
                    File.Delete(rutaArchivo)
                End If

                sHojas = NomHojas.ToString.Split(New Char() {"&"})
                Dim i As Integer = 1
                oBaseBook = oBooks.Add()
                For Each savedDoc As String In sFiles.Split(New Char() {"&"})
                    If File.Exists(savedDoc) Then
                        oBook = oBooks.Open(savedDoc)
                        oBook.Worksheets.Copy(After:=oBaseBook.Sheets(oBaseBook.Sheets.Count))
                        oBaseBook.Sheets(i + 3).Name = sHojas(i - 1)
                        oBook.Close()
                        i += 1
                    End If
                Next
                If oBaseBook.Sheets.Count > 3 Then
                    oBaseBook.Sheets(3).Delete()
                    oBaseBook.Sheets(2).Delete()
                    oBaseBook.Sheets(1).Delete()
                End If

                sFile = folderActual & "\" & nombreNuevoArchivo
                oBaseBook.SaveAs(sFile, Excel.XlFileFormat.xlWorkbookNormal, Nothing, Nothing, False, False, Excel.XlSaveAsAccessMode.xlShared, False, False, Nothing, Nothing, Nothing)
                oBaseBook.Close()
            End If

            If sFile <> "" Then
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(sFile))
                Response.WriteFile(sFile)
                Response.End()
            End If

            Thread.CurrentThread.CurrentCulture = oldCulture

            For Each savedDoc As String In sFiles.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next

        Catch ex As Exception
            For Each savedDoc As String In sFiles.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
            AlertaJS(ex.Message.ToString())
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
            CargarTipoRenta()
            CargarOperadores()
            CargarFechas()
        End If
    End Sub

    Private Sub ibExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarReporte()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub ddlTipoRenta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoRenta.SelectedIndexChanged
        If ddlTipoRenta.SelectedValue = Constantes.M_STR_TEXTO_TODOS Then
            dgLista.Enabled = False
            CargarOperadores(TR_DERIVADOS)
        Else
            dgLista.Enabled = True
            CargarOperadores()
        End If
    End Sub

#End Region
End Class
