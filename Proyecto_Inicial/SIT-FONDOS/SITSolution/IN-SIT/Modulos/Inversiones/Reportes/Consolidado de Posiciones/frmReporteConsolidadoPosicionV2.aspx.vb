Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.IO
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports iTextSharp.text 'HDG OT 61566 Nro3-B 20101103
Imports iTextSharp.text.pdf 'HDG OT 61566 Nro3-B 20101103
Imports Microsoft.Office.Interop
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Inversiones_Reportes_Consolidado_de_Posiciones_frmReporteConsolidadoPosicionV2
    Inherits BasePage
    Dim pCodLimite As String
    Dim pRutas As String
    Dim oLimiteBM As New LimiteBM
    Dim report As CrystalDecisions.CrystalReports.Engine.ReportDocument
#Region " /* Eventos de la Página */ "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
                CargarPortafolio()
                ValidarGeneraTodoFondos()
                Session("Multifondo") = New ParametrosGeneralesBM().SeleccionarPorFiltro(GRUPO_FONDO, MULTIFONDO, String.Empty, String.Empty, Nothing).Rows(0)("Valor").ToString.Trim
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error alcargar la Página")
        End Try        
    End Sub
    Private Sub btnGenerarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarExcel.Click
        Try
            Dim bResultado As Boolean = New ReporteLimitesBM().ValidarValorizacion(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), ddlPortafolio.SelectedValue, rblEscenario.SelectedValue)
            If Not bResultado Then
                AlertaJS("Falta procesar la Valoricación del Fondo o de\nalgún Fondo para el generación por MultiFondo.")
                Exit Sub
            End If
            ConsolidaLimitesJob()
        Catch ex As Exception
            UIUtility.PublicarEvento("btnGenerarExcel_Click - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
            AlertaJS("Ocurrió un error al Generar Excel")
        End Try
    End Sub
    Private Sub tbFechaInicio_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaInicio.TextChanged
        Try
            ValidarGeneraTodoFondos()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try        
    End Sub
    Private Sub rblEscenario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblEscenario.SelectedIndexChanged
        Try
            ValidarGeneraTodoFondos()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try        
    End Sub
    Private Sub btnImprimirResumen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimirResumen.Click
        Try
            'Page.RegisterStartupScript("JScript", UtilLibrary.MostrarPopUp("VisorReporteConsolidadoPosiciones.aspx?pFecha=" & tbFechaInicio.Text & "&pPortafolio=&pEscenario=" + rblEscenario.SelectedValue, "no", 1024, 768, 10, 10, "no", "yes", "yes", "yes"))
            'EjecutarJS(UIUtility.MostrarPopUp("frmVisorReporteOperacionesEjecutadas.aspx?Finicio=" & Me.tbFechaInicio.Text.Trim & "&FFin=" & Me.tbFechaFin.Text.Trim & "&pPortafolio=" & ddlPortafolio.SelectedValue & "&nomPortafolio=" & Me.ddlPortafolio.SelectedItem.Text.Trim, "", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)

            EjecutarJS("ShowPopup('" + tbFechaInicio.Text.Trim + "','" + ddlPortafolio.SelectedValue.Trim + "','" + rblEscenario.SelectedValue.Trim + "');")
        Catch ex As Exception
            UIUtility.PublicarEvento("btnGenerarExcel_Click - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace) 'RGF 20100512
            AlertaJS("Ocurrió un error al Imprimir Resumen")
        End Try
    End Sub
    Private Sub btnImprimirPDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimirPDF.Click
        Try
            Dim bResultado As Boolean = New ReporteLimitesBM().ValidarGeneraTodoFondos(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), rblEscenario.SelectedValue, ddlPortafolio.SelectedValue) 'RGF 20101111
            If Not bResultado Then
                AlertaJS("Los limites del fondo " & ddlPortafolio.SelectedValue & " aun no han sido generados para la fecha indicada.")
                Exit Sub
            End If
            ConsolidaLimitesPDF()
        Catch ex As Exception
            UIUtility.PublicarEvento("btnGenerarExcel_Click  Limite-" & pCodLimite & " - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace) 'RGF 20100512
            AlertaJS("Ocurrió un error al Imprimir PDF")
            For Each savedDoc As String In pRutas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub
#End Region
#Region " /* Métodos Personalizados */ "
    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.Items.Clear()
        HelpCombo.LlenarComboBox(ddlPortafolio, dsPortafolio, "CodigoPortafolio", "Descripcion", True)
    End Sub
    Private Sub ConsolidaLimitesPDF()
        Session("PortafolioLimite") = ddlPortafolio.SelectedValue
        '1=Vuelve a generar los limites
        '0=No genera, solo lee de la tabla ReporteLimites
        Session("ProcesarLimite") = 0 'RGF 20101111
        Dim rutaArchivo As String
        Dim dtLimites As DataTable
        Dim dr As DataRow
        Dim rutas As New System.Text.StringBuilder
        Dim detalladoPorFondo As String
        UIUtility.PublicarEvento("Inicio Proceso")
        dtLimites = New ReporteLimitesBM().SeleccionarLimitesPorPortafolio(ddlPortafolio.SelectedValue, DatosRequest).Tables(0)
        Me.lblTime.Text = "Tiempo Total [" & Now.ToString(" hh:mm:ss")
        For Each dr In dtLimites.Rows
            detalladoPorFondo = "N"
            If ddlPortafolio.SelectedValue.Equals(Session("Multifondo").ToString()) And Not dr("codigoLimite").Equals("16") Then
                detalladoPorFondo = "S"
            End If
            Dim visor As New ReporteLimites
            pCodLimite = dr("codigoLimite")
            visor._CodLimite = dr("codigoLimite")
            visor._CodLimiteCaracteristica = dr("codigoLimiteCaracteristica")
            visor._FechaOperacion = tbFechaInicio.Text
            visor._DetalladoPorFondo = detalladoPorFondo
            visor._Escenario = rblEscenario.SelectedValue
            visor._ProcesarLimite = 0 'RGF 20101111
            visor._FolderReportes = "~/Modulos/Inversiones/Reportes/Limites/"
            visor._Portafolio = ddlPortafolio.SelectedValue
            visor._DescPortafolio = New PortafolioBM().Seleccionar(ddlPortafolio.SelectedValue, Nothing).Tables(0)(0)("Descripcion")
            If dr("codigoLimite") = "42" Then
                If (oLimiteBM.LimiteMaximoNegociacion_Validar(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)).Count <> 0) Then
                    GoTo [continue]
                End If
            End If
            If dr("codigoLimite") = "36" Or dr("codigoLimite") = "44" Then
                Dim oTipoCambio As New TipoCambioBM
                If (Not oTipoCambio.ExisteTipoCambio("Spot", "DOL", UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))) Then
                    GoTo [continue]
                End If
            End If
            report = visor.GeneraLimite(DatosRequest, Me.Usuario)

            If Not (report Is Nothing) Then
                rutaArchivo = "c:\temp\" & visor._CodLimite & " - " & UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text) & Now.ToString(" - yyyyMMdd hh.mm.ss") & ".pdf"
                report.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                rutas.Append("&" & rutaArchivo)
                pRutas = rutas.ToString()
            End If
[continue]:
        Next
        CrearMultiCartaPDF(rutas.ToString())
    End Sub
    Private Sub EliminaReferencias(ByRef Referencias As Object)
        Try
            Do Until _
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(Referencias) <= 0
            Loop
        Catch
        Finally
            Referencias = Nothing
        End Try
    End Sub
    Private Sub ValidarGeneraTodoFondos()
        Dim bResultado As Boolean
        bResultado = New ReporteLimitesBM().ValidarGeneraTodoFondos(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), rblEscenario.SelectedValue, "") 'RGF 20101111
        If bResultado Then
            btnImprimirResumen.Visible = True
        Else
            btnImprimirResumen.Visible = False
        End If
    End Sub
    Public Sub CrearMultiCartaPDF(ByVal cartas As String)
        Dim destinoPdf As String, nombreNuevoArchivo As String
        Dim PrefijoFolder As String = "ConsolidadoPDF_"
        Dim fechaActual As String = System.DateTime.Now.ToString("yyyyMMdd")
        Dim sRutaTemp As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")    'HDG OT 67554 duplicado
        Dim foldersAsesoria() As String = Directory.GetDirectories(sRutaTemp, PrefijoFolder & "*")  'HDG OT 67554 duplicado
        Dim folderActual As String = sRutaTemp & PrefijoFolder & fechaActual  'HDG OT 67554 duplicado
        Dim cont As Integer
        Try
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
            nombreNuevoArchivo = System.Guid.NewGuid().ToString() & ".pdf"
            destinoPdf = folderActual & "\" & nombreNuevoArchivo
            Dim sourceFiles() As String = cartas.Substring(1).Split("&")
            Dim f As Integer = 0
            Dim reader As PdfReader = New PdfReader(sourceFiles(f))
            Dim n As Integer = reader.NumberOfPages
            Dim document As Document = New Document(reader.GetPageSizeWithRotation(1))
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(destinoPdf, FileMode.Create))
            document.Open()
            Dim cb As PdfContentByte = writer.DirectContent
            Dim page As PdfImportedPage
            Dim rotation As Integer
            While (f < sourceFiles.Length)
                Dim i As Integer = 0
                While (i < n)
                    i += 1
                    document.SetPageSize(reader.GetPageSizeWithRotation(i))
                    document.NewPage()
                    page = writer.GetImportedPage(reader, i)
                    rotation = reader.GetPageRotation(i)
                    If rotation = 90 Or rotation = 270 Then
                        cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                    Else
                        cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                    End If
                End While
                f += 1
                If f < sourceFiles.Length Then
                    reader = New PdfReader(sourceFiles(f))
                    n = reader.NumberOfPages
                End If
            End While
            document.Close()
            Dim sfile As String
            sfile = folderActual & "\" & nombreNuevoArchivo
            Session("ArchivoPDF") = nombreNuevoArchivo
            Session("RutaFilePDF") = sfile
            EjecutarJS("ShowPopupPDF();")
            'RegisterStartupScript("abre", "<script>window.open('" & sfile.Replace("\", "\\") & "')</script>")

            For Each savedDoc As String In cartas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        Catch ex As Exception
            UIUtility.PublicarEvento("CrearMultiCartaPDF - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
            For Each savedDoc As String In cartas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        Finally
        End Try
    End Sub
    Private Sub ConsolidaLimitesJob()
        Dim sPortafolio As String = ddlPortafolio.SelectedValue
        Dim dFecha As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        Dim sEscenario As String = rblEscenario.SelectedValue
        Dim dt As DataTable = DatosRequest.Tables(0)
        Dim sUsuario As String = CType(dt.Select(dt.Columns(0).ColumnName & "='Usuario'")(0)(1), String)
        Dim sFondoHora As String = IIf(sPortafolio.Equals(Session("Multifondo").ToString()), "_MUL", "_F0" & sPortafolio.Substring(sPortafolio.Length - 1)) & System.DateTime.Now.ToString("_hhmmss")  'HDG OT 63128 20110511
        Dim Variable As String = "TmpPortafolio,TmpFecha,TmpEscenario,TmpUsuario,TmpFondoHora"  'HDG OT 63128 20110511
        Dim Parametros As String = sPortafolio + "," + dFecha.ToString + "," + sEscenario + "," + sUsuario + "," + sFondoHora   'HDG OT 63128 20110511
        Dim obj As New JobBM
        Dim bResultado As Boolean
        bResultado = New ReporteLimitesBM().LimiteValoresInicializa(ddlPortafolio.SelectedValue)    'HDG INC 64299	20111110
        Me.lblTime.Text = obj.EjecutarJob("DTS_SIT_ProcesarLimites" & DateTime.Today.ToString("_yyyyMMdd") & DateTime.Now.ToString("_hhmmss"), "Procesa los Limites por Fondo basandose en los parametros enviados", Variable, Parametros, "", "", ConfigurationManager.AppSettings(SERVIDORETL))
    End Sub
#End Region
    Protected Sub Modulos_Inversiones_Reportes_Consolidado_de_Posiciones_frmReporteConsolidadoPosicionV2_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Not report Is Nothing Then
            report.Close()
            report.Dispose()
        End If
    End Sub
End Class