Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
Imports Ionic.Zip
Imports System.Object
Imports System.IO.Compression
Imports System.Runtime.InteropServices.Marshal
Imports System.Net.Mail
Imports Sit.BusinessEntities

Partial Class Modulos_Inversiones_Reportes_Limites_ReporteLimitesConsolidado
    Inherits BasePage
    Dim oUtil As New UtilDM
    Private RutaDestino As String = String.Empty
    Dim Limites As New LimiteBM
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Private Sub CargarGrilla(ByVal FechaOperacion As String)
        Try
            Dim dtTabla As New DataTable
            dtTabla = Limites.ConsolidadoLimites(UIUtility.ConvertirFechaaDecimal(FechaOperacion))
            dtTabla.TableName = "Limites"
            dgLimite.DataSource = Nothing
            dgLimite.DataBind()

            If dtTabla.Rows.Count > 0 And FechaOperacion <> "" Then
                dgLimite.DataSource = dtTabla
                dgLimite.DataBind()
                'btnImprimirTodo.Enabled = True
                'btnProcesarTodo.Enabled = True
            Else
                'btnImprimirTodo.Enabled = False
                'btnProcesarTodo.Enabled = False
                AlertaJS("No existe información a reportar.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub


    'Protected Sub btnImprimirTodo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimirTodo.Click
    '    Try
    '        Dim dtTabla As New DataTable
    '        dtTabla = Limites.ReporteLimites(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text)
    '        dtTabla.TableName = "DsReporteReporteLimites"
    '        If dtTabla.Rows.Count > 0 Then
    '            oReport.Load(Server.MapPath("Reportes/rptReporteLimites.rpt"))
    '            oReport.SetDataSource(dtTabla)
    '            oReport.SetParameterValue("@Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
    '            oReport.SetParameterValue("Usuario", Usuario)
    '            Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
    '            If chbExcel.Checked Then
    '                Dim excelOpts As New CrystalDecisions.Shared.ExcelFormatOptions
    '                exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel
    '                exportOpts.ExportFormatOptions = excelOpts
    '            Else
    '                Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '                exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '                exportOpts.ExportFormatOptions = pdfOpts
    '            End If
    '            oReport.ExportToHttpResponse(exportOpts, Response, True, "ReporteLimites" + Convert.ToString(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)))
    '        Else
    '            AlertaJS("No existe información a reportar.")
    '        End If
    '    Catch ex As Exception
    '        AlertaJS(Replace(ex.Message, "'", ""))
    '    End Try
    'End Sub

    'Protected Sub btnProcesarTodo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesarTodo.Click
    '    Try
    '        Limites.ProcesarLimites(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
    '        CargarGrilla(tbFechaOperacion.Text)
    '        AlertaJS("Se proceso correctamente!")
    '    Catch ex As Exception
    '        AlertaJS("Ocurrió un error en el sistema")
    '    End Try
    'End Sub

    Protected Sub Procesar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Dim Portafolio As String
            Portafolio = Limites.ProcesarLimites(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), codigo)
            CargarGrilla(tbFechaOperacion.Text)
            AlertaJS(Portafolio & " se proceso correctamente!")
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema")
        End Try
    End Sub

#Region "Mejoras al Procesar e Imprimir de la Grilla"


    ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Metodos de Impresión
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click, btnEnviarCorreo.Click
        Try
            'Dim folderArchivos As String = System.Web.HttpContext.Current.Server.MapPath("~\\temp_files\\")
            Dim objParamGenerales As ParametrosGeneralesBM = New ParametrosGeneralesBM()
            Dim dt As DataTable = objParamGenerales.SeleccionarPorFiltro(Constantes.ParametrosGenerales.RUTA_TEMP, "", "", "", DatosRequest)
            Dim folderArchivos As String = dt.Rows(0)("Valor").ToString()

            Dim secuenciaProceso As String = Now.ToString("yyyy-MM-dd") & System.Guid.NewGuid().ToString().Substring(0, 8)
            Dim listaArchivos As New List(Of String), listaPortafolios As New List(Of String), listaClienteMandato As New List(Of ClienteMandatoBE)
            Dim fechaPortafolioYYYYMMDD As String = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString()
            Dim sMandato As String = ""

            For Each gridRow As GridViewRow In dgLimite.Rows
                If gridRow.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As CheckBox = CType(gridRow.FindControl("chkSelect"), CheckBox)
                    ' Solo los Portafolios seleccionados
                    If chkSelect.Checked Then
                        Dim idPortafolio As String = CType(gridRow.FindControl("hdIdPortafolio"), HtmlInputHidden).Value
                        Dim nombrePortafolio As String = CType(gridRow.Controls(1), DataControlFieldCell).Text
                        Dim nombreArchivo As String = String.Format("ReporteLimites-{0}-{1}-{2}", nombrePortafolio, fechaPortafolioYYYYMMDD, secuenciaProceso)
                        sMandato = "N"
                        Try
                            nombreArchivo += IIf(chbExcel.Checked, ".xls", ".pdf")
                            ExportarHaciaPDFoExcel(idPortafolio, Not chbExcel.Checked, folderArchivos & nombreArchivo, sMandato)
                            listaArchivos.Add(folderArchivos & nombreArchivo) 'Agregamos el archivo PDF que se va a comprimir
                            listaPortafolios.Add(nombrePortafolio)

                            'Valida si tiene el check consolidado de Cliente Mandato
                            Dim objPortafolio As New PortafolioBM
                            Dim dtPortafolio As DataTable
                            dtPortafolio = objPortafolio.Seleccionar(idPortafolio, Nothing).Portafolio

                            'Creamos una lista de los clientes mandatos
                            If (dtPortafolio(0)("Consolidado").ToString().Equals("1") And dtPortafolio(0)("CodigoTerceroCliente").ToString() <> "") Then
                                Dim objClienteMandato As New ClienteMandatoBE()
                                objClienteMandato.CodigoClienteMandato = dtPortafolio(0)("CodigoTerceroCliente").ToString()
                                objClienteMandato.CodigoPortafolio = idPortafolio
                                objClienteMandato.ClienteMandato = dtPortafolio(0)("DescTerceroCliente").ToString()
                                listaClienteMandato.Add(objClienteMandato)
                            End If

                        Catch ex As Exception
                            AlertaJS(Replace(String.Format("Problemas al Imprimir o Exportar el Portafolio {0} [ID {1}]: {2}", nombrePortafolio, idPortafolio, ex.Message), "'", ""))
                        End Try
                    End If
                End If
            Next

            If listaClienteMandato.Count > 0 Then 'Validamos si hay clientes mandatos
                Dim ClienteMandatoList = listaClienteMandato.GroupBy(Function(x) x.CodigoClienteMandato).Select(Function(x) x.First).ToList
                'Recorro la lista
                For Each cliente As ClienteMandatoBE In ClienteMandatoList
                    Dim PortafolioList As String = "", ClienteMandato As String = ""
                    For Each vPortafolio As ClienteMandatoBE In listaClienteMandato
                        ClienteMandato = vPortafolio.ClienteMandato
                        If (vPortafolio.CodigoClienteMandato = cliente.CodigoClienteMandato) Then
                            PortafolioList = PortafolioList + "," + vPortafolio.CodigoPortafolio
                        End If
                    Next

                    sMandato = "S"
                    Dim nombrePortafolioMandato As String = ClienteMandato
                    Dim nombreArchivoMandato As String = String.Format("ReporteLimites-{0}-{1}-{2}", nombrePortafolioMandato, fechaPortafolioYYYYMMDD, secuenciaProceso)
                    nombreArchivoMandato += IIf(chbExcel.Checked, ".xls", ".pdf")
                    ExportarHaciaPDFoExcel(PortafolioList, Not chbExcel.Checked, folderArchivos & nombreArchivoMandato, sMandato)
                    listaArchivos.Add(folderArchivos & nombreArchivoMandato)
                    listaPortafolios.Add(nombrePortafolioMandato)

                Next
            End If
            sMandato = "N"
            If listaArchivos.Count > 0 Then ' Si hay archivos PDF generados pasamos a ZIPEARLOS

                If chbExcel.Checked Then
                    Dim routeFileExcCon As String = String.Empty
                    routeFileExcCon = GenerarExcelExcesoConsolidado(listaPortafolios, sMandato)
                    listaArchivos.Add(routeFileExcCon)
                    listaPortafolios.Add("Consolidado Excesos Limites")
                End If

                Dim multiplesArchivos As Boolean = (listaArchivos.Count > 1)
                Dim pathArchivoFinal As String, tipoContenido As String

                multiplesArchivos = (listaArchivos.Count > 1)
                If multiplesArchivos Then
                    tipoContenido = IIf(chbExcel.Checked, "xls", "zip")
                    Dim pathArchivoUnificador As String = folderArchivos & String.Format("{0}-{1}-{2}.{3}",
                                                                    IIf(chbExcel.Checked, "LimitesConsolidados", "ReportesLimites"),
                                                                    fechaPortafolioYYYYMMDD,
                                                                    secuenciaProceso,
                                                                    tipoContenido)

                    If chbExcel.Checked Then 'Solo se genera un excel UNIFICADO
                        MergeExcel(listaArchivos, listaPortafolios, pathArchivoUnificador)
                    Else 'Se UNIFICA todo en un ZIP
                        Dim bufferZip As Byte() = ZipearArchivos(listaArchivos, "-" & secuenciaProceso)
                        File.WriteAllBytes(pathArchivoUnificador, bufferZip)
                        'Response.BinaryWrite(bufferZip)
                    End If

                    pathArchivoFinal = pathArchivoUnificador
                Else 'Un solo archivo
                    tipoContenido = IIf(chbExcel.Checked, "xls", "pdf")
                    pathArchivoFinal = listaArchivos(0)
                End If

                Dim errorEnCorreo As Exception = Nothing
                Dim botonSender As Button = sender

                If botonSender.ID = btnImprimir.ID Then
                    Response.Clear()
                    Response.ContentType = "application/" & tipoContenido '"application/zip" O "application/pdf"
                    Response.AddHeader("Content-Disposition", "attachment;filename=" & Path.GetFileName(pathArchivoFinal).Replace("-" & secuenciaProceso, ""))
                    Response.WriteFile(pathArchivoFinal)
                    'Dim bufferFinal As Byte() = File.ReadAllBytes(pathArchivoFinal)
                    'Response.BinaryWrite(bufferFinal)
                    Response.Flush()
                Else 'btnEnviarCorreo.ID 'Envio de Correos
                    Try
                        EnviarCorreoConReporteAdjunto(pathArchivoFinal)
                        AlertaJS("Se ha enviado correo a los destinatarios configurados")
                    Catch exMail As Exception
                        errorEnCorreo = New Exception("Error en el envío de correo. " & exMail.Message, exMail)
                    End Try
                End If

                'CRumiche: Eliminamos los archivos residuales (tanto PDF ZIP)
                Try
                    If listaArchivos.Count > 0 Then
                        For Each pathArchivo As String In listaArchivos
                            If File.Exists(pathArchivo) Then File.Delete(pathArchivo)
                        Next
                    End If
                    If File.Exists(pathArchivoFinal) Then File.Delete(pathArchivoFinal)
                Catch ex01 As Exception 'CRumiche: Si obtenemos una Excepción por motivo de eliminación de archivos asumimos el no eliminarlo
                End Try

                If botonSender.ID = btnImprimir.ID Then Response.End()
                If errorEnCorreo IsNot Nothing Then Throw errorEnCorreo
            Else
                AlertaJS("No se han generado ninguna impresión")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", "").Replace(Environment.NewLine, ""))
        End Try
    End Sub
    ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Metodos de Impresión

    ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Enviar correo reporte consolidado
    Sub EnviarCorreoConReporteAdjunto(ByVal pathArchivoReporte As String)
        'CRumiche: Primero obtenemos la configuración para el envío de correo
        Dim toUser As New List(Of String), ccUser As New List(Of String), asunto As String = "", msg As String = ""
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dtInfoCorreos As DataTable = oParametrosGeneralesBM.SeleccionarPorFiltro(EMAIL_REP_CONSOLIDADO, "", "", "", DatosRequest)

        If dtInfoCorreos.Rows.Count = 0 Then Throw New Exception("No se encontró datos en la configuración de parametros del sistema EMAIL_REP_CONSOLIDADO")

        Dim resulSelecCorreo() As DataRow = dtInfoCorreos.Select("Nombre = 'MAIL_BODY'")
        If resulSelecCorreo.Length > 0 Then msg = resulSelecCorreo(0)("Valor").ToString().Trim()

        resulSelecCorreo = dtInfoCorreos.Select("Nombre = 'MAIL_ASUNTO'")
        If resulSelecCorreo.Length > 0 Then asunto = resulSelecCorreo(0)("Valor").ToString().Trim()

        For Each rowMail As DataRow In dtInfoCorreos.Select("Nombre like 'MAIL_TO_%'")
            If rowMail("Valor").ToString().Trim().Length > 0 Then
                toUser.Add(rowMail("Valor").ToString().Trim())
            End If
        Next

        If toUser.Count = 0 Then Throw New Exception("No se encontró Correos Destinatarios en la configuración de parametros del sistema EMAIL_REP_CONSOLIDADO")

        For Each rowMail As DataRow In dtInfoCorreos.Select("Nombre like 'MAIL_CC_%'")
            If rowMail("Valor").ToString().Trim().Length > 0 Then
                ccUser.Add(rowMail("Valor").ToString().Trim())
            End If
        Next

        Using unAttach As New Attachment(pathArchivoReporte)
            UIUtility.EnviarMail_v2(toUser.ToArray(), ccUser.ToArray(), asunto, msg, DatosRequest, unAttach)
        End Using
    End Sub
    ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Enviar correo reporte consolidado

    ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Exportación hacia PDF o Excel
    ''' <summary>
    ''' Exporta el Reporte de Limites a PDF o hacia Excel
    ''' </summary>
    ''' <param name="idPortafolio"></param>
    ''' <param name="HaciaPDF">True: Exporta a PDF, False: Exporta a Excel</param>
    ''' <param name="pathArchivo"></param>
    Protected Sub ExportarHaciaPDFoExcel(ByVal idPortafolio As String, ByVal HaciaPDF As Boolean, ByVal pathArchivo As String, ByVal Mandato As String)
        If HaciaPDF Then
            Dim dtTabla As New DataTable
            dtTabla = Limites.ReporteLimites(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text, Mandato, idPortafolio)
            If dtTabla.Rows.Count = 0 Then Throw New Exception("No existe información a reportar.")

            dtTabla.TableName = "DsReporteReporteLimites"

            oReport.Load(Server.MapPath("Reportes/rptReporteLimites.rpt"))
            oReport.SetDataSource(dtTabla)
            oReport.SetParameterValue("@Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            oReport.SetParameterValue("Usuario", Usuario)
            Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
            Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
            exportOpts.ExportFormatOptions = pdfOpts

            oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pathArchivo)
        Else
            GenerarExcelLimitePortafolio(idPortafolio, pathArchivo, Mandato)
            'GenerarExcelExcesoConsolidado()
            'oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, pathArchivo)
        End If
    End Sub
    ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Exportación hacia PDF o Excel

    ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Zipear Archivos
    Public Shared Function ZipearArchivos(ByVal listaArchivos As List(Of String), Optional ByVal reemplazarTextoEnNombresArchivos As String = "") As Byte()
        Using ms = New MemoryStream()
            Using zipout = New ZipOutputStream(ms)
                For Each pathArchivo As String In listaArchivos
                    Dim entryName As String = Path.GetFileName(pathArchivo)
                    If reemplazarTextoEnNombresArchivos.Length > 0 Then entryName = entryName.Replace(reemplazarTextoEnNombresArchivos, "")
                    zipout.PutNextEntry(entryName)

                    Using streamArchivo As FileStream = File.OpenRead(pathArchivo)
                        Dim buffer(streamArchivo.Length) As Byte
                        streamArchivo.Read(buffer, 0, buffer.Length)
                        zipout.Write(buffer, 0, buffer.Length)
                    End Using
                Next
            End Using

            Return ms.ToArray()
        End Using
    End Function
    ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Zipear Archivos

    ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | Ian Pastor | 2018-10-17 | Merge Excel 
    Protected Sub MergeExcel(ByVal listaArchivos As List(Of String), ByVal listaPortafolios As List(Of String), ByVal pathArchivoResultante As String)
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim xlApp As Excel.Application = Nothing
        Dim wbReporteBook As Excel.Workbook = Nothing
        Dim wbAgruparBook As Excel.Workbook = Nothing
        Dim sTemplate As String = String.Empty
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            xlApp = CType(ObjCom.ObjetoAplication, Excel.Application)
            xlApp.Visible = False : xlApp.DisplayAlerts = False
            sTemplate = RutaPlantillas() & "\" & "PlantillaLimitesConsolidado.xlsx"

            wbAgruparBook = xlApp.Workbooks.Open(sTemplate, [ReadOnly]:=True)
            Dim oSheetsList() As Object = {"Hoja1"}
            Dim iSheet As Integer = -1

            For i = 0 To listaArchivos.Count - 1
                If File.Exists(listaArchivos(i)) Then
                    wbReporteBook = xlApp.Workbooks.Open(listaArchivos(i), [ReadOnly]:=True)
                    wbReporteBook.Sheets(oSheetsList).Copy(After:=wbAgruparBook.Worksheets(wbAgruparBook.Worksheets.Count))
                    wbAgruparBook.Worksheets(wbAgruparBook.Worksheets.Count).Name = listaPortafolios(i)
                    wbReporteBook.Close()
                    If iSheet = -1 Then
                        iSheet = i
                    End If
                End If
            Next
            wbAgruparBook.Worksheets(1).Delete()
            wbAgruparBook.Worksheets(listaPortafolios(listaPortafolios.Count - 1)).Move(Before:=wbAgruparBook.Worksheets(listaPortafolios(iSheet)))
            wbAgruparBook.SaveAs(Filename:=pathArchivoResultante)
            wbAgruparBook.Close()
        Catch ex As Exception
            Throw ex
        Finally
            If xlApp IsNot Nothing Then
                xlApp.Quit()
                ReleaseComObject(xlApp)
            End If

            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
    End Sub
    ' FIN | Proyecto SIT Fondos - Limites | Sprint I | Ian Pastor | 2018-10-17 | Merge Excel 

#End Region

#Region "Eventos de la Página"

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not Page.IsPostBack Then
                'btnProcesarTodo.Attributes.Add("onClick", "return confirm('¿Desea procesar todo?')")
                'btnImprimirTodo.Attributes.Add("onClick", "return confirm('¿Desea imprimir todo?')")
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                CargarGrilla(tbFechaOperacion.Text)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            For Each gridRow As GridViewRow In dgLimite.Rows
                If gridRow.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As CheckBox = CType(gridRow.FindControl("chkSelect"), CheckBox)
                    ' Solo los Portafolios seleccionados
                    If chkSelect.Checked Then
                        Dim idPortafolio As String = CType(gridRow.FindControl("hdIdPortafolio"), HtmlInputHidden).Value
                        Dim nombrePortafolio As String = CType(gridRow.Controls(1), DataControlFieldCell).Text
                        Try
                            Limites.ProcesarLimites(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), idPortafolio)
                        Catch ex As Exception
                            AlertaJS(Replace(String.Format("Problemas al procesar el Portafolio {0} [ID {1}]: {2}", nombrePortafolio, idPortafolio, ex.Message), "'", ""))
                        End Try

                        CargarGrilla(tbFechaOperacion.Text)
                        AlertaJS("Proceso Correcto")
                    End If
                End If
            Next
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub Imprimir(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Dim dtTabla As New DataTable
            dtTabla = Limites.ReporteLimites(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text, codigo)
            dtTabla.TableName = "DsReporteReporteLimites"
            If dtTabla.Rows.Count > 0 Then
                oReport.Load(Server.MapPath("Reportes/rptReporteLimites.rpt"))
                oReport.SetDataSource(dtTabla)
                oReport.SetParameterValue("@Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                oReport.SetParameterValue("Usuario", Usuario)
                Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
                If chbExcel.Checked Then
                    Dim excelOpts As New CrystalDecisions.Shared.ExcelFormatOptions
                    exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel
                    exportOpts.ExportFormatOptions = excelOpts
                Else
                    Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
                    exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
                    exportOpts.ExportFormatOptions = pdfOpts
                End If
                oReport.ExportToHttpResponse(exportOpts, Response, True, "ReporteLimites" + Convert.ToString(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)) + codigo)
            Else
                AlertaJS("No existe información a reportar.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try

    End Sub

    Protected Sub tbFechaOperacion_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaOperacion.TextChanged
        Try
            CargarGrilla(tbFechaOperacion.Text)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema")
        End Try
    End Sub

    Protected Sub dgLimite_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLimite.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Estado As Literal = e.Row.FindControl("ltlEstado")
            If Estado.Text = "OK" Then
                e.Row.Cells(2).BackColor = Drawing.Color.Blue
                e.Row.Cells(2).ForeColor = Drawing.Color.White
            End If
            If Estado.Text = "Inconsistencias" Then
                e.Row.Cells(2).BackColor = Drawing.Color.Red
                e.Row.Cells(2).ForeColor = Drawing.Color.White
            End If
        End If
    End Sub

    Protected Sub form1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub

    Private Sub GenerarExcelLimitePortafolio(ByVal p_CodigoPortafolio As String, ByVal routeFile As String, ByVal Mandato As String)
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets
        Dim oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtReporte As DataTable = Nothing
            Dim objReporteLimite As New ReporteLimitesBM
            Dim NombrePlantilla As String = ""

            If (Mandato.Equals("S")) Then
                NombrePlantilla = "PlantillaLimitesMandato.xlsx"
            Else
                NombrePlantilla = "PlantillaLimites.xlsx"
            End If

            dtReporte = objReporteLimite.ObtenerReporteLimite_PorPortafolio(p_CodigoPortafolio, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text, Mandato)
            If dtReporte IsNot Nothing Then
                If dtReporte.Rows.Count > 0 Then
                    Dim sFile As String, sTemplate As String
                    'sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RptLim_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                    sFile = routeFile
                    If File.Exists(sFile) Then File.Delete(sFile)
                    sTemplate = RutaPlantillas() & "\" & NombrePlantilla
                    oExcel.Visible = False : oExcel.DisplayAlerts = False
                    oBooks = oExcel.Workbooks
                    oBooks.Open(sTemplate)
                    oBook = oBooks.Item(1)
                    oSheets = oBook.Worksheets
                    oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                    oCells = oSheet.Cells
                    oSheet.SaveAs(sFile)

                    FillExcelLimite(dtReporte, oSheet, oCells)
                    'oExcel.Cells.EntireColumn.AutoFit()
                    oBook.Save()
                    oBook.Close()
                    'Response.Clear()
                    'Response.ContentType = "application/xls"
                    'Response.AddHeader("Content-Disposition", "attachment; filename=" + "RptLim_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
                    'Response.WriteFile(sFile)
                    'Response.End()
                    'Else
                    '    Throw New Exception("No existen datos a la fecha para la generación del reporte")
                End If
                'Else
                '    Throw New Exception("No existen datos a la fecha para la generación del reporte")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
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

    Private Sub FillExcelLimite(ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        Dim nfila As Integer = 2
        For Each drLimite As DataRow In dt.Rows
            If Not IsDBNull(drLimite("FechaString")) Then oCells(nfila, 1) = drLimite("FechaString") Else oCells(nfila, 1) = ""
            If Not IsDBNull(drLimite("DescripcionPortafolio")) Then oCells(nfila, 2) = drLimite("DescripcionPortafolio") Else oCells(nfila, 2) = ""
            If Not IsDBNull(drLimite("CodigoMonedaSBS")) Then oCells(nfila, 3) = drLimite("CodigoMonedaSBS") Else oCells(nfila, 3) = ""
            If Not IsDBNull(drLimite("PatrimonioCierre")) Then oCells(nfila, 4) = drLimite("PatrimonioCierre") Else oCells(nfila, 4) = ""
            If Not IsDBNull(drLimite("CarteraFondo")) Then oCells(nfila, 5) = drLimite("CarteraFondo") Else oCells(nfila, 5) = ""
            If Not IsDBNull(drLimite("TipoLimite")) Then oCells(nfila, 6) = drLimite("TipoLimite") Else oCells(nfila, 6) = ""
            If Not IsDBNull(drLimite("Emisor")) Then oCells(nfila, 7) = drLimite("Emisor") Else oCells(nfila, 7) = ""
            If Not IsDBNull(drLimite("CodigoEntidad")) Then oCells(nfila, 8) = drLimite("CodigoEntidad") Else oCells(nfila, 8) = ""
            If Not IsDBNull(drLimite("DescripcionGruEco")) Then oCells(nfila, 9) = drLimite("DescripcionGruEco") Else oCells(nfila, 9) = ""
            If Not IsDBNull(drLimite("PatrimonioEmpresa")) Then oCells(nfila, 10) = drLimite("PatrimonioEmpresa") Else oCells(nfila, 10) = ""
            If Not IsDBNull(drLimite("PasivoEmpresa")) Then oCells(nfila, 11) = drLimite("PasivoEmpresa") Else oCells(nfila, 11) = ""
            If Not IsDBNull(drLimite("RatingFF")) Then oCells(nfila, 12) = drLimite("RatingFF") Else oCells(nfila, 12) = ""
            If Not IsDBNull(drLimite("RatingInterno")) Then oCells(nfila, 13) = drLimite("RatingInterno") Else oCells(nfila, 13) = ""
            If Not IsDBNull(drLimite("Rating")) Then oCells(nfila, 14) = drLimite("Rating") Else oCells(nfila, 14) = ""
            If Not IsDBNull(drLimite("LineaPlazo")) Then oCells(nfila, 15) = drLimite("LineaPlazo") Else oCells(nfila, 15) = ""
            If Not IsDBNull(drLimite("Participacion_Por")) Then oCells(nfila, 16) = drLimite("Participacion_Por") / 100
            If Not IsDBNull(drLimite("LimiteMinInt_Por")) Then oCells(nfila, 17) = IIf(Decimal.Parse(drLimite("LimiteMinInt_Por")) > 0, drLimite("LimiteMinInt_Por") / 100, "") Else oCells(nfila, 17) = ""
            If Not IsDBNull(drLimite("LimiteMaxInt_Por")) Then oCells(nfila, 18) = IIf(Decimal.Parse(drLimite("LimiteMaxInt_Por")) > 0, drLimite("LimiteMaxInt_Por") / 100, "") Else oCells(nfila, 18) = ""
            If Not IsDBNull(drLimite("LimiteMinLeg_Por")) Then oCells(nfila, 19) = IIf(Decimal.Parse(drLimite("LimiteMinLeg_Por")) > 0, drLimite("LimiteMinLeg_Por") / 100, "") Else oCells(nfila, 19) = ""
            If Not IsDBNull(drLimite("LimiteMaxLeg_Por")) Then oCells(nfila, 20) = IIf(Decimal.Parse(drLimite("LimiteMaxLeg_Por")) > 0, drLimite("LimiteMaxLeg_Por") / 100, "") Else oCells(nfila, 20) = ""
            'If Not IsDBNull(drLimite("Participacion_Por")) Then oCells(nfila, 16) = IIf(Decimal.Parse(drLimite("Participacion_Por")) > 0, drLimite("Participacion_Por") / 100, "") Else oCells(nfila, 16) = ""

            ''oCells(nfila, 21) = drLimite("ExcesoInterno_Por") / 100 - CALCULADOS EN LA PLANTILLA
            ''oCells(nfila, 22) = drLimite("ExcesoInterno") - CALCULADOS EN LA PLANTILLA
            ''oCells(nfila, 23) = drLimite("ExcesoLegal_Por") / 100 - CALCULADOS EN LA PLANTILLA
            ''oCells(nfila, 24) = drLimite("ExcesoLegal") - CALCULADOS EN LA PLANTILLA
            ''oCells(nfila, 25) = drLimite("Alerta_Por")
            If Not IsDBNull(drLimite("ValorBase")) Then oCells(nfila, 26) = drLimite("ValorBase") Else oCells(nfila, 26) = ""

            'Dim Valor As Decimal, Catera As Decimal, Consumo As Decimal

            'If (Not IsDBNull(drLimite("ValorBase"))) Then

            '    Valor = drLimite("ValorBase")

            '    If (Not IsDBNull(drLimite("CarteraFondo"))) Then
            '        Catera = drLimite("CarteraFondo")
            '        If (Valor <> 0) Then
            '            Consumo = Catera / Valor
            '        Else
            '            Consumo = 0
            '        End If
            '    Else
            '        Consumo = 0
            '    End If

            'Else
            '    Consumo = 0
            'End If
            If Not IsDBNull(drLimite("Consumo")) Then oCells(nfila, 27) = drLimite("Consumo") Else oCells(nfila, 27) = 0

            nfila += 1
        Next
    End Sub

    Private Function GenerarExcelExcesoConsolidado(ByVal p_Portafolio As List(Of String), ByVal p_Mandato As String) As String
        GenerarExcelExcesoConsolidado = ""
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks
        Dim oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets
        Dim oSheet As Excel.Worksheet
        Dim oCells As Excel.Range
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim dtReporte As DataTable = Nothing
            Dim objReporteLimite As New ReporteLimitesBM
            dtReporte = objReporteLimite.ObtenerReporteLimite_PorPortafolio("", UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), tbFechaOperacion.Text, p_Mandato)
            If dtReporte IsNot Nothing Then
                Dim sFile As String, sTemplate As String
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RptLim_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "PlantillaLimites.xlsx"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)

                FillExcelLimiteExcesoConsolidado(p_Portafolio, dtReporte, oSheet, oCells)
                'oExcel.Cells.EntireColumn.AutoFit()
                oBook.Save()
                oBook.Close()
                GenerarExcelExcesoConsolidado = sFile
                'Response.Clear()
                'Response.ContentType = "application/xls"
                'Response.AddHeader("Content-Disposition", "attachment; filename=" + "RptLim_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
                'Response.WriteFile(sFile)
                'Response.End()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
    End Function

    Private Sub FillExcelLimiteExcesoConsolidado(ByVal p_Portafolio As List(Of String), ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        Dim dtCabecera() As DataRow = Nothing
        Dim dtDetalle() As DataRow = Nothing
        Dim swFillCab As Boolean = False
        Dim nFila As Integer = 2
        For Each port As String In p_Portafolio
            dtCabecera = dt.Select("DescripcionPortafolio='" & port & "' AND CabeceraLimite='S'")
            dtDetalle = dt.Select("DescripcionPortafolio='" & port & "' AND FlagExceso='S'")
            For Each drCab As DataRow In dtCabecera
                For Each drDet As DataRow In dtDetalle
                    If (drCab("CodigoLimite") = drDet("CodigoLimite")) Then
                        If (swFillCab = False) Then
                            FillExcelRange(drCab, oSheet, oCells, nFila)
                            swFillCab = True
                        End If
                        FillExcelRange(drDet, oSheet, oCells, nFila)
                    End If
                Next
                swFillCab = False
            Next
        Next
    End Sub

    Private Sub FillExcelRange(ByVal dr As DataRow, ByRef oSheet As Excel.Worksheet, ByVal oCells As Excel.Range, ByRef nFila As Integer)
        If Not IsDBNull(dr("FechaString")) Then oCells(nFila, 1) = dr("FechaString") Else oCells(nFila, 1) = ""
        If Not IsDBNull(dr("DescripcionPortafolio")) Then oCells(nFila, 2) = dr("DescripcionPortafolio") Else oCells(nFila, 2) = ""
        If Not IsDBNull(dr("CodigoMonedaSBS")) Then oCells(nFila, 3) = dr("CodigoMonedaSBS") Else oCells(nFila, 3) = ""
        If Not IsDBNull(dr("PatrimonioCierre")) Then oCells(nFila, 4) = dr("PatrimonioCierre") Else oCells(nFila, 4) = ""
        If Not IsDBNull(dr("CarteraFondo")) Then oCells(nFila, 5) = dr("CarteraFondo") Else oCells(nFila, 5) = ""
        If Not IsDBNull(dr("TipoLimite")) Then oCells(nFila, 6) = dr("TipoLimite") Else oCells(nFila, 6) = ""
        If Not IsDBNull(dr("Emisor")) Then oCells(nFila, 7) = dr("Emisor") Else oCells(nFila, 7) = ""
        If Not IsDBNull(dr("CodigoEntidad")) Then oCells(nFila, 8) = dr("CodigoEntidad") Else oCells(nFila, 8) = ""
        If Not IsDBNull(dr("DescripcionGruEco")) Then oCells(nFila, 9) = dr("DescripcionGruEco") Else oCells(nFila, 9) = ""
        If Not IsDBNull(dr("PatrimonioEmpresa")) Then oCells(nFila, 10) = dr("PatrimonioEmpresa") Else oCells(nFila, 10) = ""
        If Not IsDBNull(dr("PasivoEmpresa")) Then oCells(nFila, 11) = dr("PasivoEmpresa") Else oCells(nFila, 11) = ""
        If Not IsDBNull(dr("RatingFF")) Then oCells(nFila, 12) = dr("RatingFF") Else oCells(nFila, 12) = ""
        If Not IsDBNull(dr("RatingInterno")) Then oCells(nFila, 13) = dr("RatingInterno") Else oCells(nFila, 13) = ""
        If Not IsDBNull(dr("Rating")) Then oCells(nFila, 14) = dr("Rating") Else oCells(nFila, 14) = ""
        If Not IsDBNull(dr("LineaPlazo")) Then oCells(nFila, 15) = dr("LineaPlazo") Else oCells(nFila, 15) = ""
        If Not IsDBNull(dr("Participacion_Por")) Then oCells(nFila, 16) = IIf(Decimal.Parse(dr("Participacion_Por")) > 0, dr("Participacion_Por") / 100, "") Else oCells(nFila, 16) = ""
        If Not IsDBNull(dr("LimiteMinInt_Por")) Then oCells(nFila, 17) = IIf(Decimal.Parse(dr("LimiteMinInt_Por")) > 0, dr("LimiteMinInt_Por") / 100, "") Else oCells(nFila, 17) = ""
        If Not IsDBNull(dr("LimiteMaxInt_Por")) Then oCells(nFila, 18) = IIf(Decimal.Parse(dr("LimiteMaxInt_Por")) > 0, dr("LimiteMaxInt_Por") / 100, "") Else oCells(nFila, 18) = ""
        If Not IsDBNull(dr("LimiteMinLeg_Por")) Then oCells(nFila, 19) = IIf(Decimal.Parse(dr("LimiteMinLeg_Por")) > 0, dr("LimiteMinLeg_Por") / 100, "") Else oCells(nFila, 19) = ""
        If Not IsDBNull(dr("LimiteMaxLeg_Por")) Then oCells(nFila, 20) = IIf(Decimal.Parse(dr("LimiteMaxLeg_Por")) > 0, dr("LimiteMaxLeg_Por") / 100, "") Else oCells(nFila, 20) = ""
        'oCells(nfila, 21) = drLimite("ExcesoInterno_Por") / 100 - CALCULADOS EN LA PLANTILLA
        'oCells(nfila, 22) = drLimite("ExcesoInterno") - CALCULADOS EN LA PLANTILLA
        'oCells(nfila, 23) = drLimite("ExcesoLegal_Por") / 100 - CALCULADOS EN LA PLANTILLA
        'oCells(nfila, 24) = drLimite("ExcesoLegal") - CALCULADOS EN LA PLANTILLA
        'oCells(nFila, 25) = dr("Alerta_Por")
        nFila += 1
    End Sub

#End Region


End Class
