Imports System.Runtime.InteropServices.Marshal
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class Modulos_Valorizacion_y_Custodia_Custodia_frmCargarConciliacionCustodios
    Inherits BasePage
    Dim sPortafolioCodigo As String = String.Empty
    Dim sPortafolioDescripcion As String = String.Empty
    Dim sCodigoCustodio As String = String.Empty
    Dim oCustodioArchivoBM As New CustodioArchivoBM
    Dim oUtil As New UtilDM
    Dim rep As New ReportDocument
    Dim rep2 As New ReportDocument
    Dim rep3 As New ReportDocument
    Dim rep4 As New ReportDocument
    Dim rep5 As New ReportDocument
    Dim rep6 As New ReportDocument

    Dim lstReportes As List
#Region " /* Funciones Personalizadas*/"
    Private Function VerificaExistenDiferencias() As Boolean
        Dim sFechaOperacion As String = tbFechaOperacion.Text
        sPortafolioCodigo = ddlPortafolio.SelectedValue.ToString
        sCodigoCustodio = dlCustodio.SelectedValue.ToString
        Dim oInfCusDS As New DataTable
        Dim objInformacionCustodio As New CustodioArchivoBM
        oInfCusDS = objInformacionCustodio.InstrumentosDiferencias(sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2), sPortafolioCodigo, sCodigoCustodio, DatosRequest).Tables(0)
        If oInfCusDS.Rows.Count > 0 Then
            Return True
            Exit Function
        End If
        objInformacionCustodio = Nothing
        oInfCusDS = Nothing
        Return False
    End Function
    Private Sub CargarGrilla()
        Dim sFechaOperacion As String = tbFechaOperacion.Text
        sPortafolioCodigo = ddlPortafolio.SelectedValue.ToString
        sCodigoCustodio = dlCustodio.SelectedValue.ToString
        Dim oInfCusDS As New DataTable
        Dim objInformacionCustodio As New CustodioArchivoBM
        If Not (String.IsNullOrEmpty(tbFechaOperacion.Text)) Then
            oInfCusDS = objInformacionCustodio.InstrumentosPorConciliar(sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2), sPortafolioCodigo, sCodigoCustodio, DatosRequest).Tables(0)
        Else
            sFechaOperacion = UIUtility.ObtenerFechaMaximaNegocio()
            oInfCusDS = objInformacionCustodio.InstrumentosPorConciliar(sFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest).Tables(0)
        End If
        dgArchivo.DataSource = oInfCusDS
        dgArchivo.DataBind()
    End Sub
    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        Dim objportafolio As New PortafolioBM
        drlista.DataSource = objportafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoPortafolio"
        drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista)
        objportafolio = Nothing
    End Sub
    Private Sub CargaCustodios()
        Dim objCustodio As New CustodioBM
        dlCustodio.DataTextField = "Descripcion"
        dlCustodio.DataValueField = "CodigoCustodio"
        dlCustodio.DataSource = objCustodio.Listar(DatosRequest)
        dlCustodio.DataBind()
        UIUtility.InsertarElementoSeleccion(dlCustodio, "0", "--SELECCIONE--")
        objCustodio = Nothing
    End Sub
    Private Function Validar() As Boolean
        Dim sFechaOperacion As String = tbFechaOperacion.Text.Trim
        If sFechaOperacion.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT49"))
            Return False
            Exit Function
        ElseIf sFechaOperacion.Length < 10 Then
            AlertaJS(ObtenerMensaje("ALERT50"))
            Return False
            Exit Function
        End If
        If sFechaOperacion.Trim <> "" Then
            If Not IsDate(sFechaOperacion) Then
                AlertaJS(ObtenerMensaje("ALERT51"))
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
#End Region
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
    Function CargarRuta() As String
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("012", MyBase.DatosRequest())
        Return (oArchivoPlanoBE.Tables(0).Rows(0).Item(4))
    End Function
    Private Sub btnImprimirTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimirTodo.Click
        Dim strCodigoPortafolio As String, strNombrePortafolio As String

        If ddlPortafolio.SelectedValue.Trim() <> String.Empty Then
            strCodigoPortafolio = ddlPortafolio.SelectedValue
            strNombrePortafolio = ddlPortafolio.SelectedItem.Text

            ImprimirTodo(strCodigoPortafolio, strNombrePortafolio, False)
        Else
            lstReportes = New List()

            Dim arrayPortafolios As New ArrayList
            arrayPortafolios = New CustodioBM().ListarPortafoliosBBH()

            For index = 0 To ddlPortafolio.Items.Count - 1
                strCodigoPortafolio = ddlPortafolio.Items(index).Value
                strNombrePortafolio = ddlPortafolio.Items(index).Text

                If dlCustodio.SelectedValue = "CAVALI" Then
                    If strCodigoPortafolio.Trim() <> String.Empty Then
                        ImprimirTodo(strCodigoPortafolio, strNombrePortafolio, True)
                    End If
                Else
                    If strCodigoPortafolio.Trim() <> String.Empty Then
                        If arrayPortafolios.Contains(strCodigoPortafolio.Trim()) Then
                            ImprimirTodo(strCodigoPortafolio, strNombrePortafolio, True)
                        End If
                    End If
                End If
            Next

            Dim sb As New StringBuilder()
            For index = 0 To lstReportes.Items.Count - 1
                sb.Append(lstReportes.Chunks(index).ToString())
                sb.Append("!")
            Next

            Dim Extension As String = "xls"
            If ddlTipoImpresion.SelectedValue = "1" Then Extension = "pdf"

            Dim archivoSalida As String
            Dim ruta As String = CargarRuta()
            archivoSalida = ruta + "InstrumentosUnificadosTODOS" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Extension

            If ddlTipoImpresion.SelectedValue = "1" Then
                MergePDF(sb.ToString(), archivoSalida)
            Else
                MergeXLS(sb.ToString(), archivoSalida)
            End If

            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/" + Extension
            Response.AddHeader("Content-Disposition", "attachment; filename=" + archivoSalida)
            Response.WriteFile(archivoSalida)
            Response.End()

            lstReportes.Items.Clear()
            lstReportes = Nothing
        End If
    End Sub
    Private Sub btnGenerarReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarReporte.Click
        Try
            If Not (RbtnFiltro.SelectedValue.Equals("DiferenciasCartera_Custodia") Or (RbtnFiltro.SelectedValue.Equals("Conciliados") And dlCustodio.SelectedValue.Equals("BCRPLP"))) Then 'RGF 20081118
                If Not VerificaCustodioInformacion() Then
                    AlertaJS(ObtenerMensaje("ALERT98"))
                    Exit Sub
                End If
            End If
            Dim sFechaOperacion As String = tbFechaOperacion.Text
            sPortafolioCodigo = ddlPortafolio.SelectedValue.ToString
            sPortafolioDescripcion = ddlPortafolio.SelectedItem.Text.Trim
            sCodigoCustodio = dlCustodio.SelectedValue.ToString
            Select Case RbtnFiltro.SelectedValue.ToString
                Case "No Registrados"
                    Dim strurl As String = "Reportes/frmInstrumentosNoRegistrados.aspx?nFechaOperacion=" & sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2) & "&sPortafolioCodigo=" & sPortafolioCodigo & "&sCodigoCustodio=" & sCodigoCustodio & "&sNombreCustodio=" & dlCustodio.SelectedItem.Text
                    EjecutarJS("showWindow('" & strurl & "', '800', '600');")
                Case "No Reportado"
                    Dim strurl As String = "Reportes/frmInstrumentosNoReportado.aspx?nFechaOperacion=" & sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2) & "&sPortafolioCodigo=" & sPortafolioCodigo & "&sCodigoCustodio=" & sCodigoCustodio & "&sNombreCustodio=" & dlCustodio.SelectedItem.Text
                    EjecutarJS("showWindow('" & strurl & "', '800', '600');")
                Case "Conciliados"
                    Dim strurl As String = "Reportes/frmInstrumentosConciliados.aspx?nFechaOperacion=" & sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2) & "&sPortafolioCodigo=" & sPortafolioCodigo & "&sCodigoCustodio=" & sCodigoCustodio & "&sNombreCustodio=" & dlCustodio.SelectedItem.Text
                    EjecutarJS("showWindow('" & strurl & "', '800', '600');")
                Case "Diferencias"
                    Dim strurl As String = "Reportes/frmDiferenciaInstrumentos.aspx?nFechaOperacion=" & sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2) & "&sPortafolioCodigo=" & sPortafolioCodigo & "&sPortafolioDescripcion=" & sPortafolioDescripcion & "&sCodigoCustodio=" & sCodigoCustodio & "&sRep=" & RbtnFiltro.SelectedValue & "&sNombreCustodio=" & dlCustodio.SelectedItem.Text
                    EjecutarJS("showWindow('" & strurl & "', '800', '600');")
                Case "DiferenciasDet"
                    Dim strurl As String = "Reportes/frmDiferenciaInstrumentosCDet.aspx?nFechaOperacion=" & sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2) & "&sPortafolioCodigo=" & sPortafolioCodigo & "&sPortafolioDescripcion=" & sPortafolioDescripcion & "&sCodigoCustodio=" & sCodigoCustodio & "&sNombreCustodio=" & dlCustodio.SelectedItem.Text
                    EjecutarJS("showWindow('" & strurl & "', '800', '600');")
                Case "DiferenciasCartera_Custodia"
                    Dim strurl As String = "Reportes/frmDiferenciaInstrumentos.aspx?nFechaOperacion=" & sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2) & "&sPortafolioCodigo=" & sPortafolioCodigo & "&sPortafolioDescripcion=" & sPortafolioDescripcion & "&sCodigoCustodio=" & sCodigoCustodio & "&sRep=" & RbtnFiltro.SelectedValue
                    EjecutarJS("showWindow('" & strurl & "', '800', '600');")
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Generar el Reporte")
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub
    Private Sub btnConciliar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConciliar.Click
        Try
            If VerificaCustodioInformacion() Then
                Call CargarGrilla()
                btnProcesar.Enabled = True
            Else
                AlertaJS(ObtenerMensaje("ALERT98"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Conciliación")
        End Try
    End Sub
    Private Function VerificaCustodioInformacion() As Boolean
        Dim sFechaCorte As String
        sFechaCorte = tbFechaOperacion.Text
        sFechaCorte = sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2)
        Dim oCustodioInformacion As DataSet = oCustodioArchivoBM.VerificaCustodioInformacion(CLng(sFechaCorte), DatosRequest)
        If oCustodioInformacion.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            If dgArchivo.Rows.Count <= 0 Then
                AlertaJS(ObtenerMensaje("ALERT99"))
                Exit Sub
            Else
                If dgArchivo.Rows(0).Cells(1).Text = "&nbsp;" Then
                    AlertaJS(ObtenerMensaje("ALERT99"))
                    Exit Sub
                Else
                    If dgArchivo.Rows.Count <= 0 Then
                        AlertaJS(ObtenerMensaje("ALERT99"))
                        Exit Sub
                    End If
                End If
            End If
            'Verifica seleccionados
            If VerificaSeleccionados() Then
                AlertaJS(ObtenerMensaje("ALERT92"))
            Else
                AlertaJS(ObtenerMensaje("ALERT93"))
            End If
            Call CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al procesar los datos")
        End Try
    End Sub
    Private Function VerificaSeleccionados() As Boolean
        Dim i As Integer
        Dim sCodigoMnemonico As String
        Dim sCodigoISIN As String
        Dim sCodigoPortafolioSBS As String
        Dim sCodigoCustodio As String
        Dim oCheck As CheckBox
        Dim bSeleccionado As Boolean
        bSeleccionado = False
        For i = 0 To dgArchivo.Rows.Count - 1
            oCheck = New CheckBox
            oCheck = CType(dgArchivo.Rows(i).FindControl("chkNominal"), CheckBox)
            If oCheck.Checked = True Then
                sCodigoMnemonico = dgArchivo.Rows(i).Cells(0).Text
                sCodigoISIN = dgArchivo.Rows(i).Cells(6).Text
                sCodigoPortafolioSBS = dgArchivo.Rows(i).Cells(7).Text
                sCodigoCustodio = dgArchivo.Rows(i).Cells(8).Text
                'Actualiza Saldos en Inf. Custodio
                If Not ActualizaSaldosInfCustodio(sCodigoMnemonico, sCodigoISIN, sCodigoPortafolioSBS, sCodigoCustodio) Then
                    AlertaJS(ObtenerMensaje("ALERT94"))
                End If
                bSeleccionado = True
            End If
        Next
        Return bSeleccionado
    End Function
    Private Function ActualizaSaldosInfCustodio(ByVal sCodigoMnemonico As String, ByVal sCodigoISIN As String, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoCustodio As String) As Boolean
        Dim sFechaOperacion As String = tbFechaOperacion.Text
        sFechaOperacion = sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2)
        Dim oInfCusDS As New DataTable
        Dim objInformacionCustodio As New CustodioArchivoBM

        Return objInformacionCustodio.ActualizaSaldosInfCustodio(sFechaOperacion, sCodigoMnemonico, sCodigoISIN, sCodigoPortafolioSBS, sCodigoCustodio, DatosRequest)
    End Function
    Private Sub tbFechaOperacion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If VerificaExistenDiferencias() Then
                _Validacion.Value = "1"
            Else
                _Validacion.Value = "0"
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Fecha")
        End Try
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            If VerificaExistenDiferencias() Then
                _Validacion.Value = "1"
            Else
                _Validacion.Value = "0"
            End If
            EstablecerFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Portafolio")
        End Try
    End Sub
    Private Sub dlCustodio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dlCustodio.SelectedIndexChanged
        Try
            If VerificaExistenDiferencias() Then
                _Validacion.Value = "1"
            Else
                _Validacion.Value = "0"
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Custodio")
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                Call CargaPortafolio(ddlPortafolio)
                Call CargaCustodios()
                Call CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema")
        End Try
    End Sub

    Private Sub EstablecerFecha()
        If ddlPortafolio.SelectedIndex > 0 Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
        Else
            tbFechaOperacion.Text = Now.ToString("dd/MM/yyyy")
        End If
    End Sub
    Protected Sub dgArchivo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgArchivo.PageIndexChanging
        Try
            dgArchivo.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub
    Private Sub MergePDF(ByVal archivosEntrada As String, ByVal archivoSalida As String)
        Dim archivoEntrada() = archivosEntrada.Split("!")
        Dim i As Byte
        Dim MemStream As New System.IO.MemoryStream
        Dim doc As New iTextSharp.text.Document(PageSize.A4, 33, 0, 5, 0)
        Dim reader As iTextSharp.text.pdf.PdfReader
        Dim numberOfPages As Integer
        Dim currentPageNumber As Integer
        Dim PdfPath As String = archivoSalida
        Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, New FileStream(PdfPath, FileMode.Create))
        doc.Open()
        Dim cb As iTextSharp.text.pdf.PdfContentByte = writer.DirectContent
        Dim page As iTextSharp.text.pdf.PdfImportedPage
        Dim rotation As Integer
        Dim filename As String
        For i = 0 To archivoEntrada.Length - 2
            filename = archivoEntrada(i)
            reader = New PdfReader(filename)
            numberOfPages = reader.NumberOfPages
            currentPageNumber = 0
            Do While (currentPageNumber < numberOfPages)
                currentPageNumber += 1
                doc.SetPageSize(reader.GetPageSizeWithRotation(currentPageNumber))
                doc.NewPage()
                page = writer.GetImportedPage(reader, currentPageNumber)
                rotation = reader.GetPageRotation(currentPageNumber)
                If (rotation = 90) Or (rotation = 270) Then
                    cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(currentPageNumber).Height)
                Else
                    cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                End If
            Loop
        Next
        doc.Close()
    End Sub
    Private Sub MergeXLS(ByVal archivosEntrada As String, ByVal archivoSalida As String)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application
        Dim wbReporteBook As Excel.Workbook = Nothing
        Dim wbAgruparBook As Excel.Workbook = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            xlApp.Visible = False : xlApp.DisplayAlerts = False
            Dim sFile As String, sTemplate As String
            sFile = archivoSalida
            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaTenencias.xls"
            wbAgruparBook = xlApp.Workbooks.Open(sTemplate)
            Dim NombreLibro As String
            Dim oSheetsList() As Object = {"Sheet1"}
            Dim archivoEntrada() = archivosEntrada.Split("!")
            For i = 0 To archivoEntrada.Length - 2
                wbReporteBook = xlApp.Workbooks.Open(archivoEntrada(i))
                wbReporteBook.Sheets(oSheetsList).Copy(After:=wbAgruparBook.Worksheets(wbAgruparBook.Worksheets.Count))

                NombreLibro = Replace(wbReporteBook.Name, ".xls", "")
                wbAgruparBook.Worksheets(2 + i).Name = NombreLibro
                wbReporteBook.Close()
            Next
            wbAgruparBook.Worksheets(1).Delete()
            wbAgruparBook.SaveAs(Filename:=sFile)
            wbAgruparBook.Close()
        Catch ex As Exception
            Throw ex
        Finally
            xlApp.Quit()
            ReleaseComObject(xlApp)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
    Protected Sub Modulos_Valorizacion_y_Custodia_Custodia_frmCargarConciliacionCustodios_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oCustodioArchivoBM = Nothing
        rep.Close()
        rep.Dispose()
        rep2.Close()
        rep2.Dispose()
        rep3.Close()
        rep3.Dispose()
        rep4.Close()
        rep4.Dispose()
        rep5.Close()
        rep5.Dispose()
        rep6.Close()
        rep6.Dispose()
    End Sub

    Private Sub ImprimirTodo(ByVal codigoPortafolio As String, ByVal nombrePortafolio As String, ByVal multiplesArchivos As Boolean)
        Try
            Dim sFechaOperacion As String = tbFechaOperacion.Text
            sPortafolioCodigo = codigoPortafolio 'ddlPortafolio.SelectedValue.ToString 
            sCodigoCustodio = dlCustodio.SelectedValue.ToString
            Dim nFechaOperacion As Long = sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2)
            Dim sNombreCustodio As String = Request.QueryString("sNombreCustodio")
            Dim BoolDetalle As Boolean = True
            Dim BoolInstrumentos As Boolean = True
            Dim BoolDiferencia As Boolean = True
            Dim BoolDiferenciaVarios As Boolean = True
            Dim ruta As String
            ruta = CargarRuta()
            Dim archivosEntrada As String = String.Empty
            Dim archivoSalida As String
            sCodigoCustodio = dlCustodio.SelectedValue
            Dim archivo As DataSet = New ArchivoPlanoBM().Seleccionar("025", Nothing)
            Dim rutaArchivo As String = archivo.Tables(0).Rows(0)("ArchivoUbicacion").ToString.Trim() & archivo.Tables(0).Rows(0)("ArchivoNombre").ToString.Trim() & "." & archivo.Tables(0).Rows(0)("ArchivoExtension").ToString.Trim()
            Dim portafolio As New PortafolioBM
            Dim dtPortafolio As DataTable = portafolio.PortafolioCodigoListar(ParametrosSIT.PORTAFOLIO_MULTIFONDOS)
            Dim Extension As String = String.Empty
            If ddlTipoImpresion.SelectedValue = "1" Then
                Extension = "pdf"
            Else
                Extension = "xls"
            End If
            Dim i As Byte
            rep.Load(Server.MapPath("Reportes/InstrumentosNoRegistradosUnif.rpt"))
            rep2.Load(Server.MapPath("Reportes/InstrumentosConciliadosUnif.rpt"))
            rep3.Load(Server.MapPath("Reportes/DiferenciasUnif.rpt"))
            rep4.Load(Server.MapPath("Reportes/DiferenciasCarteraCustodio.rpt"))
            rep6.Load(Server.MapPath("Reportes/InstrumentosNoReportados.rpt"))
            Dim oInstNoreg As New InstNoreg
            Dim tableNoRegis As DataTable
            Dim oInstNoregTMP As DataSet
            sCodigoCustodio = dlCustodio.SelectedValue '#ERROR#
            oInstNoregTMP = New CustodioArchivoBM().InstrumentosNoRegistrados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
            tableNoRegis = oInstNoregTMP.Tables(0)
            CopiarTabla(tableNoRegis, oInstNoreg.InstrumentosNoRegistrados)
            BoolInstrumentos = False
            rep.SetDataSource(oInstNoreg)
            rep.SetParameterValue("Usuario", Usuario)
            rep.SetParameterValue("FechaOperacion", sFechaOperacion)
            'rep.SetParameterValue("Portafolio", ddlPortafolio.SelectedItem.Text)
            rep.SetParameterValue("Portafolio", nombrePortafolio)
            rep.SetParameterValue("Custodio", sCodigoCustodio & " - " & dlCustodio.SelectedItem.Text)
            rep.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
            rutaArchivo = ruta + sPortafolioCodigo + "noreg" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Extension
            archivosEntrada = archivosEntrada & rutaArchivo & "!"
            If ddlTipoImpresion.SelectedValue = "1" Then
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
            Else
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutaArchivo)
            End If
            Dim oInstCons As New InstCons
            Dim tableCons As DataTable
            Dim oInstConsTMP As DataSet
            sCodigoCustodio = dlCustodio.SelectedValue
            oInstConsTMP = New CustodioArchivoBM().InstrumentosConciliados(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
            tableCons = oInstConsTMP.Tables(0)
            CopiarTabla(tableCons, oInstCons.InstrumentosConciliados)
            rep2.SetDataSource(oInstCons)
            rep2.SetParameterValue("Usuario", MyBase.Usuario)
            rep2.SetParameterValue("FechaOperacion", sFechaOperacion)
            rep2.SetParameterValue("Portafolio", sPortafolioCodigo)
            rep2.SetParameterValue("Custodio", sCodigoCustodio & " - " & sNombreCustodio)
            rep2.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
            rutaArchivo = ruta + sPortafolioCodigo + "concil" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Extension
            archivosEntrada = archivosEntrada & rutaArchivo & "!"
            If ddlTipoImpresion.SelectedValue = "1" Then
                rep2.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
            Else
                rep2.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutaArchivo)
            End If
            Dim tableDife As DataTable
            Dim oInstDifeTMP As DataSet
            Dim oInstDife As New InstDife
            sCodigoCustodio = dlCustodio.SelectedValue
            'Este reporte es diferente al que se imprime desde el boton de impresion individual
            oInstDifeTMP = New CustodioArchivoBM().DIFerenciasCustodioResumen_Listar(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio)
            tableDife = oInstDifeTMP.Tables(0)
            CopiarTabla(tableDife, oInstDife.InstrumentosDiferencias)
            BoolDiferencia = False
            rep3.SetDataSource(oInstDife)
            rep3.SetParameterValue("Usuario", MyBase.Usuario)
            rep3.SetParameterValue("FechaOperacion", sFechaOperacion)
            'rep3.SetParameterValue("Portafolio", ddlPortafolio.SelectedItem.Text)
            rep3.SetParameterValue("Portafolio", nombrePortafolio)
            rep3.SetParameterValue("Custodio", sCodigoCustodio & " - " & dlCustodio.SelectedItem.Text)
            rep3.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
            rutaArchivo = ruta + sPortafolioCodigo + "difere" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Extension
            archivosEntrada = archivosEntrada & rutaArchivo & "!"
            If ddlTipoImpresion.SelectedValue = "1" Then
                rep3.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
            Else
                rep3.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutaArchivo)
            End If
            Dim oInstDifeDet As New InstDifeCDet
            Dim oInstDifeDetTMP As DataSet = New CustodioArchivoBM().InstrumentosDiferenciasCDet(nFechaOperacion, sPortafolioCodigo, sCodigoCustodio, DatosRequest)
            Dim dtCustodios As New DataTable
            Dim dtCabeceraDiferencias As New DataTable
            Dim dtDetalleDiferencias As New DataTable
            'rep5.Load(Server.MapPath("Reportes/DiferenciasCDet.rpt"))
            rep5.Load(Server.MapPath("Reportes/DiferenciasCDetNew.rpt"))
            CopiarTabla(oInstDifeDetTMP.Tables(0), oInstDifeDet.Custodios)
            CopiarTabla(oInstDifeDetTMP.Tables(1), oInstDifeDet.CabeceraDiferencias)
            CopiarTabla(oInstDifeDetTMP.Tables(2), oInstDifeDet.DetalleDiferencias)
            rep5.SetDataSource(oInstDifeDet)
            rep5.SetParameterValue("Usuario", MyBase.Usuario)
            rep5.SetParameterValue("FechaOperacion", sFechaOperacion)
            'rep5.SetParameterValue("Portafolio", ddlPortafolio.SelectedItem.Text)
            rep5.SetParameterValue("Portafolio", nombrePortafolio)
            rep5.SetParameterValue("Custodio", dlCustodio.SelectedItem.Text)
            rep5.SetParameterValue("CodigoCustodio", sCodigoCustodio)
            rep5.SetParameterValue("RutaLogo", Server.MapPath("~/App_Themes/img/logo.jpg"))
            rutaArchivo = ruta + dtPortafolio.Rows(i)("CodigoPortafolio").ToString.Trim + "difeVarDet" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Extension
            archivosEntrada = archivosEntrada & rutaArchivo & "!"
            If ddlTipoImpresion.SelectedValue = "1" Then
                rep5.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaArchivo)
            Else
                rep5.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutaArchivo)
            End If
            archivoSalida = ruta + "InstrumentosUnificados" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "." + Extension
            If ddlTipoImpresion.SelectedValue = "1" Then
                MergePDF(archivosEntrada, archivoSalida)
            Else
                MergeXLS(archivosEntrada, archivoSalida)
            End If

            If Not multiplesArchivos Then
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/" + Extension
                Response.AddHeader("Content-Disposition", "attachment; filename=" + archivoSalida)
                Response.WriteFile(archivoSalida)
                Response.End()
            Else
                lstReportes.Add(archivoSalida)
            End If

        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
End Class