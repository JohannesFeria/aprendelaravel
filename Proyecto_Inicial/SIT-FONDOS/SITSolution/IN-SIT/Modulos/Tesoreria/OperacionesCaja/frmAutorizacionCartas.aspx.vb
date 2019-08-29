Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.html
Imports System.IO
Imports System.Text
Imports System.Collections
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Data
Partial Class Modulos_Tesoreria_OperacionesCaja_frmAutorizacionCartas
    Inherits BasePage
    Private rutaCartas As String = "C:\SIT\Modelos Cartas"
    Private _strcartas As String
    Dim arrayCodigos As New ArrayList
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
#Region "Eventos de la Pagina"
    Public Shared Sub CrearArchivoPDF(ByVal TablaText As DataTable, ByVal HtmlPath As String, ByVal PdfPath As String)
        Try
            Dim document As New Document(PageSize.A4, 55, 55, 20, 6)
            Dim oStreamReader As New StreamReader(HtmlPath, System.Text.Encoding.Default)
            Dim styles As New iTextSharp.text.html.simpleparser.StyleSheet
            Dim hw As New iTextSharp.text.html.simpleparser.HTMLWorker(document)
            Dim oIElement As IElement
            Dim oParagraph As Paragraph
            Dim oPdfPTable As PdfPTable
            Dim oPdfPCell As PdfPCell
            Dim objects As List(Of iTextSharp.text.IElement)
            Dim strContent As String
            PdfWriter.GetInstance(document, New FileStream(PdfPath, FileMode.Create))
            document.Open()
            document.NewPage()
            objects = hw.ParseToList(oStreamReader, styles)
            For k As Integer = 0 To objects.Count - 1
                oIElement = CType(objects(k), IElement)
                If objects(k).GetType().FullName = "iTextSharp.text.Paragraph" Then
                    oParagraph = New Paragraph
                    oParagraph.Alignment = CType(objects(k), Paragraph).Alignment
                    For z As Integer = 0 To oIElement.Chunks.Count - 1
                        strContent = ReplaceText(oIElement.Chunks(z).Content, TablaText)
                        oParagraph.Add(New Chunk(strContent, oIElement.Chunks(z).Font))
                        oParagraph.Leading = 11
                    Next
                    document.Add(oParagraph)
                ElseIf objects(k).GetType().FullName = "iTextSharp.text.pdf.PdfPTable" Then
                    oPdfPTable = CType(objects(k), PdfPTable)
                    Dim oNewPdfPTable As PdfPTable = New PdfPTable(oPdfPTable.NumberOfColumns)
                    Dim DimensionColumna(oPdfPTable.NumberOfColumns - 1) As Integer
                    Dim aux As Integer
                    oNewPdfPTable.WidthPercentage = 100
                    Dim imgFirma1 As String = ""
                    Dim imgFirma2 As String = ""
                    Dim jpg As iTextSharp.text.Image
                    For row As Integer = 0 To oPdfPTable.Rows.Count - 1
                        For cell As Integer = 0 To oPdfPTable.Rows(row).GetCells().Length - 1
                            oPdfPCell = oPdfPTable.Rows(row).GetCells()(cell)
                            oParagraph = New Paragraph
                            For paragraph As Integer = 0 To oPdfPTable.Rows(row).GetCells()(cell).CompositeElements.Count - 1
                                For chunk As Integer = 0 To oPdfPTable.Rows(row).GetCells()(cell).CompositeElements(paragraph).Chunks.Count - 1
                                    If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Entidad]" Or _
                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Direccion]" Or _
                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
                                        strContent = ""
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Entidad]") Then
                                            strContent = "Fondos Sura"
                                        End If
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Direccion]") Then
                                            strContent = "Canaval y Moreyra 522 San Isidro. Teléfono: 411-9191. Fax: 411-9192"
                                        End If
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
                                            strContent = "www.sura.pe"
                                        End If
                                        oParagraph.Add(New Chunk(strContent, oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Font))
                                        aux = Len(strContent)
                                        If aux > DimensionColumna(cell) Then
                                            DimensionColumna(cell) = aux
                                        End If
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
                                            oParagraph.Leading = 10
                                        Else
                                            oParagraph.Leading = 6
                                        End If
                                    ElseIf oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Firma1]" And _
                                    oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Firma2]" And _
                                    oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Logo]" Then
                                        strContent = ReplaceText(oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content, TablaText)
                                        oParagraph.Add(New Chunk(strContent, oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Font))
                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[NombreUsuarioF1]" Or
                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[NombreUsuarioF2]" Or
                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[CargoUsuarioF1]" Or
                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[CargoUsuarioF2]" Then
                                            oParagraph.Alignment = Element.ALIGN_CENTER
                                        End If
                                        aux = Len(strContent)
                                        If aux > DimensionColumna(cell) Then
                                            DimensionColumna(cell) = 1
                                        End If
                                        oParagraph.Leading = 11
                                    Else
                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Firma1]" Then
                                            For Each dr As DataRow In TablaText.Rows
                                                If dr("Find") = "[Firma1]" Then
                                                    imgFirma1 = CType(dr("New"), String)
                                                End If
                                            Next
                                            If imgFirma1 <> "" Then
                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma1)
                                                jpg.Alignment = Element.ALIGN_CENTER
                                            End If
                                        End If
                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Firma2]" Then
                                            For Each dr As DataRow In TablaText.Rows
                                                If dr("Find") = "[Firma2]" Then
                                                    imgFirma2 = CType(dr("New"), String)
                                                End If
                                            Next
                                            If imgFirma2 <> "" Then
                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma2)
                                                jpg.Alignment = Element.ALIGN_CENTER
                                            End If
                                        End If
                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Logo]" Then
                                            For Each dr As DataRow In TablaText.Rows
                                                If dr("Find") = "[Logo]" Then
                                                    imgFirma2 = New BasePage().Ruta_Logo()
                                                End If
                                            Next
                                            If imgFirma2 <> "" Then
                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma2)
                                                jpg.ScaleToFit(127.0F, 40.0F)
                                                jpg.SpacingBefore = 5.0F
                                                jpg.SpacingAfter = 1.0F
                                                jpg.Alignment = Element.ALIGN_RIGHT
                                            End If
                                        End If
                                    End If
                                Next
                                aux = 0
                            Next
                            oPdfPCell.CompositeElements.Clear()
                            oPdfPCell.AddElement(oParagraph)
                            If Not jpg Is Nothing Then
                                oPdfPCell.AddElement(jpg)
                                jpg = Nothing
                            End If
                            oNewPdfPTable.AddCell(oPdfPCell)
                        Next
                    Next
                    If DimensionColumna.Length = 2 Then
                        Dim widths As Integer() = New Integer() {50.0F, 50.0F}
                        oNewPdfPTable.SetWidths(widths)
                    ElseIf DimensionColumna.Length = 3 Then
                        Dim widths As Integer() = New Integer() {15.0F, 45.0F, 45.0F}
                        oNewPdfPTable.SetWidths(widths)
                    ElseIf DimensionColumna.Length = 7 Then
                        Dim widths As Integer() = New Integer() {20.0F, 20.0F, 20.0F, 20.0F, 20.0F, 10.0F, 20.0F}
                        oNewPdfPTable.SetWidths(widths)
                    Else
                        oNewPdfPTable.SetWidths(CalcularDimensiones(DimensionColumna))
                    End If
                    document.Add(oNewPdfPTable)
                End If
            Next
            document.Close()
            oStreamReader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Shared Function ReplaceText(ByVal TextBase As String, ByVal TableText As DataTable) As String
        Try
            For x As Integer = 0 To TableText.Rows.Count - 1
                If TextBase.IndexOf("[") = -1 And TextBase.IndexOf("]") = -1 Then Exit For
                If TextBase.IndexOf(TableText.Rows(x)("Find")) > -1 Then TextBase = TextBase.Replace(TableText.Rows(x)("Find"), TableText.Rows(x)("New"))
            Next
            Return TextBase
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Shared Function CalcularDimensiones(ByVal Columnas() As Integer) As Integer()
        Dim total As Integer
        Dim i As Integer
        For i = 0 To Columnas.Length - 1
            If Columnas(i) = 0 Then
                Columnas(i) = 10
            End If
            total = total + Columnas(i)
        Next
        For i = 0 To Columnas.Length - 1
            Columnas(i) = (Columnas(i) / total) * 100
        Next
        Return Columnas
    End Function
    Public Shared Function CrearCartaPDF(ByVal drValores As DataRow, ByVal request As DataSet, Optional ByVal path As String = "") As Boolean
        Dim _strcartas As String
        _strcartas = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(request)
        If drValores("ArchivoPlantilla") = "" Then Return False
        Try
            Dim strRutaHTML = drValores("ArchivoPlantilla").Replace("dot", "html")
            Dim strRutaPDF As String

            If Not String.IsNullOrEmpty(path) Then
                strRutaPDF = path
            Else
                strRutaPDF = _strcartas & "\" & drValores("NumeroCarta") & ".pdf"
            End If
            If Not File.Exists(strRutaHTML) Then Return False
            If File.Exists(strRutaPDF) Then File.Delete(strRutaPDF)
            CrearArchivoPDF(CrearTablaValores(drValores), strRutaHTML, strRutaPDF)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function
    Private Shared Function CrearTablaValores(ByRef drDocuemnto As DataRow) As DataTable
        Dim dtValores As New DataTable
        Dim drValor As DataRow
        dtValores.Columns.Add("Find", GetType(String))
        dtValores.Columns.Add("New", GetType(String))
        Dim dsEstructuraCarta As DataTable = New ModeloCartaBM().SeleccionarCartaEstructuraPorModelo(drDocuemnto("CodigoModelo")).Tables(0)
        For Each drEtiqueta As DataRow In dsEstructuraCarta.Rows
            drValor = dtValores.NewRow
            If (drDocuemnto.Table.Columns.Contains(drEtiqueta("OrigenCampo"))) Then
                If Not IsDBNull(drDocuemnto(Trim(drEtiqueta("OrigenCampo")))) Then
                    drValor("Find") = drEtiqueta("NombreCampo")
                    drValor("New") = drDocuemnto(drEtiqueta("OrigenCampo"))
                    dtValores.Rows.Add(drValor)
                End If
            Else
                If (Convert.ToString(drEtiqueta("OrigenCampo")).Trim().Equals("Logo")) Then
                    drValor("Find") = drEtiqueta("NombreCampo")
                    drValor("New") = ""
                    dtValores.Rows.Add(drValor)
                End If
            End If

        Next
        Return dtValores
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Sub btnVista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVista.Click
        GenerarVista()
        If IO.File.Exists(Session("RutaCarta")) Then
            EjecutarJS("window.open('frmVisorCarta.aspx');")
        Else
            AlertaJS(ObtenerMensaje("ALERT126"))
        End If
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            ViewState("codigoMercado") = ddlMercado.SelectedValue
            ViewState("codigoPortafolio") = ddlPortafolio.SelectedValue
            ViewState("codigoTercero") = ddlIntermediario.SelectedValue
            ViewState("codigoTerceroBanco") = ddlBanco.SelectedValue
            ViewState("fechaLiquidacion") = UIUtility.ConvertirFechaaDecimal(IIf(tbFecha.Text = "", "0", tbFecha.Text))
            ViewState("estadoCarta") = ddlEstado.SelectedValue
            CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Try
            Dim MENSAJE As String = "No se pudo enviar el correo."
            Dim oOperacionCaja As New OperacionesCajaBM
            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
            Dim dtFirmantes As DataTable = New AprobadorCartaBM().SeleccionarPorFiltro("", ParametrosSIT.FRM_CARTA, ParametrosSIT.ESTADO_ACTIVO, DatosRequest).Tables(0)
            Dim dtClaves As DataTable = New AprobadorCartaBM().GeneraClaves(6, True, DatosRequest).Tables(0)
            Dim i As Integer = 0
            oOperacionCaja.InicializarAprobacionOperacionesCaja(DatosRequest)
            For Each fila As GridViewRow In dgLista.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As System.Web.UI.WebControls.CheckBox = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                    Dim lbCodigo As System.Web.UI.WebControls.Label = CType(fila.FindControl("lbCodigo"), System.Web.UI.WebControls.Label)
                    If Not chkSelect Is Nothing Then
                        If chkSelect.Checked = True Then
                            If oOperacionCaja.AprobarOperacionCaja(lbCodigo.Text, DatosRequest, "1") Then
                                arrayCodigos.Add(lbCodigo.Text)
                                i = i + 1
                            End If
                        End If
                    End If
                End If
            Next
            If i > 0 Then
                Dim cont As Integer
                Dim codigoInterno As String
                Dim rutaArchivo As String = GenerarReporte()
                Dim clavefirma As String = String.Empty
                Dim dtClavesFirmante As DataTable
                GenerarVista(arrayCodigos)
                For k As Integer = 0 To arrayCodigos.Count - 1
                    cont = 0
                    For Each row As DataRow In dtFirmantes.Rows
                        codigoInterno = CType(row("CodigoInterno"), String)
                        clavefirma = dtClaves.Rows(cont)("Clave")
                        dtClavesFirmante = New OperacionesCajaBM().ObtenerClaveFirmantes(codigoInterno, DatosRequest).Tables(0)
                        If dtClavesFirmante.Rows(0)("Indicador") = "1" Then
                            clavefirma = dtClavesFirmante.Rows(0)("ClaveFirma")
                            oOperacionCaja.GenerarClaveFirmantesCarta(arrayCodigos.Item(k), codigoInterno, clavefirma, rutaArchivo, ParametrosSIT.VISTA_REP_APROB1, DatosRequest)
                        Else
                            oOperacionCaja.InsertarClaveFirmantes(codigoInterno, clavefirma, DatosRequest)
                            oOperacionCaja.GenerarClaveFirmantesCarta(arrayCodigos.Item(k), codigoInterno, clavefirma, rutaArchivo, ParametrosSIT.VISTA_REP_APROB1, DatosRequest) 'HDG OT 64016 20111021
                        End If
                        cont = cont + 1
                    Next
                Next
                EnviarClaveFirma(rutaArchivo, CType(Session("RutaCarta"), String))
                MENSAJE = ""
                For k As Integer = 0 To arrayCodigos.Count - 1
                    cont = 0
                    If oOperacionCaja.AprobarOperacionCaja(arrayCodigos.Item(k), DatosRequest, "2") Then
                        i = i + 1
                    End If
                Next
                CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
                AlertaJS("Se ha realizado la aprobacion de " & i & " Carta(s)")
            Else
                AlertaJS("Debe seleccionar algún registro! " + MENSAJE)
            End If
        Catch ex As Exception
            Dim oOperacionCaja2 As New OperacionesCajaBM
            For k As Integer = 0 To arrayCodigos.Count - 1
                oOperacionCaja2.EliminarClavesFirmantesCartas(arrayCodigos.Item(k))
            Next
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnFirmar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirmar.Click
        Try
            Dim cont As Integer = 0
            Dim oOperacionesCaja As New OperacionesCajaBM
            Dim chkSelect As System.Web.UI.WebControls.CheckBox
            Dim lbCodigo As System.Web.UI.WebControls.Label
            Dim codigoOrden As String = ""
            Dim mensaje As String = ""
            Dim usuarioAprobador As String = ""

            For Each fila As GridViewRow In dgLista.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                    lbCodigo = CType(fila.FindControl("lbCodigo"), System.Web.UI.WebControls.Label)
                    If chkSelect.Checked Then
                        Dim resultado As Boolean = oOperacionesCaja.FirmarCarta(lbCodigo.Text, tbCodAprob.Text.ToString().ToUpper, DatosRequest)
                        If resultado Then
                            cont = cont + 1
                            codigoOrden = codigoOrden & lbCodigo.Text.Trim & "|"
                        Else
                            If cont > 0 Then
                                AlertaJS("Se ha realizado la firma de " & cont & " Carta(s). <br />" & _
                                         "!&nbspVerifique en su correo, que la clave ingresada pertenezca a todas las cartas pendientes por firmar&nbsp!")
                            Else
                                AlertaJS("Ingrese la clave de firmante correcto. <br />" & _
                                         "!&nbspVerifique en su correo que la clave ingresada pertenezca a las cartas pendientes por firmar&nbsp!")
                            End If
                        End If
                    End If
                End If
            Next
            If cont > 0 Then
                CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
                AlertaJS("Se ha realizado la firma de " & cont & " Carta(s)")
                usuarioAprobador = obtenerUsuarioAprobador()
                Me.lblMensaje.Visible = True
                If usuarioAprobador <> "" Then
                    mensaje = mensajeCartasFirmadas(codigoOrden)
                    UIUtility.EnviarMail(usuarioAprobador, "", "SIT - Cartas Firmadas", mensaje, DatosRequest) 'Falta elegir los usuarios aprobadores.
                    Me.lblMensaje.Text = "Se ha enviado un correo de aviso a los Usuarios Aprobadores"
                Else
                    Me.lblMensaje.Text = "Ha ocurrido un error en el envío de correo a los Usuarios Aprobadores"
                End If
            Else
                AlertaJS("Debe seleccionar algún registro!")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub Modulos_Tesoreria_OperacionesCaja_frmAutorizacionCartas_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
#End Region
#Region "Funciones Personalizas"
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dsPortafolio, "CodigoPortafolio", "Descripcion", True, "Todos")
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dsMercado As DataSet = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            HelpCombo.LlenarComboBox(ddlMercado, dsMercado.Tables(0), "CodigoMercado", "Descripcion", True, "Todos")
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
        If (ddlMercado.Items.Count > 1) Then
            ddlMercado.SelectedIndex = 0
        End If
    End Sub
    Private Sub CargarBanco(ByVal CodigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio("", Me.ddlPortafolio.SelectedValue)
            HelpCombo.LlenarComboBox(ddlBanco, dsBanco.Tables(0), "CodigoTercero", "Descripcion", True)
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
    Private Sub CargarIntermediario(ByVal enabled As Boolean)
        If enabled Then
            Dim oIntermediario As New TercerosBM
            Dim dsIntermediario As TercerosBE = oIntermediario.SeleccionarPorFiltro(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "")
            HelpCombo.LlenarComboBox(ddlIntermediario, dsIntermediario.Tables(0), "CodigoTercero", "Descripcion", True)
        Else
            ddlIntermediario.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        End If
        ddlIntermediario.Enabled = enabled
    End Sub
    Private Sub CargarEstadoCarta(ByVal enabled As Boolean)
        If enabled Then
            Dim dtEstados As New DataTable
            dtEstados = New ParametrosGeneralesBM().Listar(ParametrosSIT.ESTADO_CARTA, DatosRequest)
            HelpCombo.LlenarComboBox(ddlEstado, dtEstados, "Valor", "Comentario", True)
        Else
            ddlEstado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlEstado)
        End If
        ddlEstado.Enabled = enabled
    End Sub
    Private Sub EstablecerFecha()
        tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
    End Sub
    Private Sub HabilitaControles(Optional ByVal tipo As String = "")
        CargarEstadoCarta(True)
        If tipo = ParametrosSIT.ADM_CARTA Then
            btnAprobar.Visible = True
            lbCodAprob.Visible = False
            tbCodAprob.Visible = False
            btnFirmar.Visible = False
            ddlEstado.SelectedValue = ParametrosSIT.ESTADO_CARTA_PND
        Else
            btnAprobar.Visible = False
            lbCodAprob.Visible = True
            tbCodAprob.Visible = True
            btnFirmar.Visible = True
            ddlEstado.Enabled = True
            ddlEstado.SelectedValue = ParametrosSIT.ESTADO_CARTA_APR
        End If
    End Sub
    Private Sub CargarPagina()
        btnFirmar.Attributes.Add("onclick", "javascript:return ValidarIngresoFirmas();")
        HabilitaControles()
        Dim codigoUsuario As String = ""
        Dim dtAdministrador As DataTable = New AprobadorCartaBM().SeleccionarPorFiltro("", ParametrosSIT.ADM_CARTA, ParametrosSIT.ESTADO_ACTIVO, DatosRequest).Tables(0)
        For Each dr As DataRow In dtAdministrador.Rows
            codigoUsuario = CType(dr("CodigoUsuario"), String)
            If codigoUsuario.ToString.ToUpper = Usuario.ToUpper Then
                HabilitaControles(ParametrosSIT.ADM_CARTA)
                Exit For
            End If
        Next
        CargarPortafolio()
        CargarMercado()
        CargarBanco(ddlMercado.SelectedValue, True)
        CargarIntermediario(True)
        tbFecha.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
        ViewState("codigoMercado") = ddlMercado.SelectedValue
        ViewState("codigoPortafolio") = ddlPortafolio.SelectedValue
        ViewState("CodigoTercero") = ddlIntermediario.SelectedValue
        ViewState("codigoTerceroBanco") = ddlBanco.SelectedValue
        ViewState("fechaLiquidacion") = UIUtility.ConvertirFechaaDecimal(IIf(tbFecha.Text = "", "0", tbFecha.Text))
        ViewState("estadoCarta") = ddlEstado.SelectedValue
    End Sub
    Private Sub CargarGrilla(Optional ByVal fechaLiquidacion As Decimal = 0, _
        Optional ByVal codigoMercado As String = "", _
        Optional ByVal codigoPortafolio As String = "", _
        Optional ByVal codigoTercero As String = "", _
        Optional ByVal codigoTerceroBanco As String = "", _
        Optional ByVal estadoCarta As String = "", _
        Optional ByVal codigoOperacionCaja As String = "")
        Dim dsOperaciones As DataTable = New OperacionesCajaBM().SeleccionCartas(codigoMercado, _
        codigoPortafolio, codigoTercero, codigoTerceroBanco, fechaLiquidacion, estadoCarta, codigoOperacionCaja)
        dgLista.DataSource = dsOperaciones
        dgLista.DataBind()
        NroCarta.Text = UIUtility.MostrarResultadoBusqueda(dsOperaciones)
    End Sub
    Private Sub EiminaReferencias(ByRef Referencias As Object)
        Try
            Do Until _
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(Referencias) <= 0
            Loop
        Catch
        Finally
            Referencias = Nothing
        End Try
    End Sub
    Public Function GenerateRandomString(ByRef len As Integer, ByRef upper As Boolean) As String
        Dim rand As New Random
        Dim allowableChars() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim final As String = String.Empty
        For i As Integer = 0 To len - 1
            final += allowableChars(rand.Next(allowableChars.Length - 1))
        Next
        Return IIf(upper, final.ToUpper(), final)
    End Function
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Next
    End Sub
    Private Sub GenerarVista(Optional ByVal arrayCodigos As ArrayList = Nothing)
        Dim objparametrosgenerales As New ParametrosGeneralesBM
        Dim oOperacionCaja As New OperacionesCajaBM
        Dim strTempCarta As String
        Dim ruta As String = objparametrosgenerales.ListarRutaGeneracionCartas(DatosRequest) + "\"
        Dim path As String
        Dim chk As System.Web.UI.WebControls.CheckBox
        Dim windowsCartas As New System.Text.StringBuilder
        Dim rutaCartas As String
        Dim veces As Integer
        Dim lbCodigo, lbCodigoOperacion As Label
        Dim dTOperaciones As DataTable
        veces = 0
        Try
            Session("RutaCarta") = ""
            'Actaliza algunos datos de las constitucuiones de DPZ a las cancelaciones para la impresion de cartas.
            oOperacionCaja.ActualizaDatosCancelacionesDPZ(UIUtility.ConvertirFechaaDecimal(tbFecha.Text))
            For Each item As GridViewRow In dgLista.Rows
                If (item.RowType = DataControlRowType.DataRow) Then
                    If (item.FindControl("chkSelect") Is Nothing) = False Then
                        chk = CType(item.FindControl("chkSelect"), WebControls.CheckBox)
                        If (chk.Checked = True) Then
                            strTempCarta = Usuario.ToString.Trim() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:hhmmss}", DateTime.Now) & veces.ToString
                            path = ruta & strTempCarta & ".pdf"
                            lbCodigo = CType(item.FindControl("lbCodigo"), Label)
                            lbCodigoOperacion = CType(item.FindControl("lblCodigoOperacion"), Label)
                            If lbCodigoOperacion.Text = "63" Or lbCodigoOperacion.Text = "BCRE" Then
                                'Transferencias
                                dTOperaciones = oOperacionCaja.ImpresionCarta_Transferencias(lbCodigo.Text)
                            ElseIf lbCodigoOperacion.Text = "3" Then
                                'Constituciones DPZ
                                dTOperaciones = oOperacionCaja.ImpresionCarta_ConstitucionDPZ(lbCodigo.Text)
                            ElseIf lbCodigoOperacion.Text = "4" Then
                                'Cancelaciones DPZ
                                dTOperaciones = oOperacionCaja.ImpresionCarta_CancelacionDPZ(lbCodigo.Text)
                            Else
                                'Antiguas
                                dTOperaciones = oOperacionCaja.SeleccionarAutorizacionCartas(ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), _
                                ViewState("codigoTerceroBanco"), ViewState("fechaLiquidacion"), False, ViewState("estadoCarta"), lbCodigo.Text, DatosRequest).Tables(0)
                            End If
                            If dTOperaciones.Rows.Count > 0 Then
                                CrearCartaPDF(dTOperaciones.Rows(0), DatosRequest, path)
                            End If
                            windowsCartas.Append(path & "&")
                            veces = veces + 1
                        End If
                    End If
                End If
            Next
            rutaCartas = windowsCartas.ToString()
            If (rutaCartas.Length > 0) Then
                rutaCartas = rutaCartas.Remove(rutaCartas.Length - 1, 1)
                If (veces = 1) Then
                    Session("RutaCarta") = rutaCartas
                Else
                    Session("RutaCarta") = HelpCarta.CrearMultiCartaPDF(rutaCartas)
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Function GenerarReporte(Optional ByVal proceso As String = "", _
        Optional ByVal fechaLiquidacion As Decimal = 0, _
        Optional ByVal codigoMercado As String = "", _
        Optional ByVal codigoPortafolio As String = "", _
        Optional ByVal codigoTercero As String = "", _
        Optional ByVal codigoTerceroBanco As String = "", _
        Optional ByVal estadoCarta As String = "", _
        Optional ByVal codigoOperacionCaja As String = "") As String
        Dim dsOrigen As New DataSet
        Dim dtAprobOperacionesCaja As DataTable
        Dim dtFirmantes As DataTable
        Dim dtFirmantesRes As New DataTable
        Dim nombreFirmante As String
        Dim i As Integer = 0
        Dim oOperacionesCajaBM As New OperacionesCajaBM
        Dim dsRepAprobOperacionesCaja As New RepAprobOperacionesCaja
        Dim folderActual As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
        Dim nombreNuevoArchivo = "AprobOperacionesCaja_" & Usuario.ToString() & System.DateTime.Now.ToString("yyyyMMdd") & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
        If Not Directory.Exists("c:\temp\") Then
            Directory.CreateDirectory("c:\temp\")
        End If
        Dim rutaArchivo = "c:\temp\" & nombreNuevoArchivo
        Dim strHora As String = ""
        strHora = DateAdd(DateInterval.Minute, 7, Now).ToString("yyyyMMddhhmm")
        While True
            If (Not File.Exists(rutaArchivo)) Or strHora <= Now.ToString("yyyyMMddhhmm") Then
                Exit While
            Else
                nombreNuevoArchivo = "AprobOperacionesCaja_" & Usuario.ToString() & System.DateTime.Now.ToString("yyyyMMdd") & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
                rutaArchivo = "c:\temp\" & nombreNuevoArchivo
            End If
        End While
        dtAprobOperacionesCaja = oOperacionesCajaBM.ReporteAprobacionOperacionesCaja(proceso, codigoMercado, _
            codigoPortafolio, codigoTercero, codigoTerceroBanco, fechaLiquidacion, False, estadoCarta, codigoOperacionCaja, DatosRequest).Tables(0)
        dtFirmantes = New AprobadorCartaBM().SeleccionarPorFiltro("", ParametrosSIT.FRM_CARTA, ParametrosSIT.ESTADO_ACTIVO, DatosRequest).Tables(0)
        Dim col As DataColumn
        col = New DataColumn("Nombre", System.Type.GetType("System.String"))
        dtFirmantesRes.Columns.Add(col)
        dtFirmantesRes.AcceptChanges()
        For Each dr As DataRow In dtFirmantes.Rows
            nombreFirmante = CType(dr("Nombre"), String)
            Dim row As DataRow
            row = dtFirmantesRes.NewRow()
            row.Item("Nombre") = nombreFirmante
            dtFirmantesRes.Rows.Add(row)
        Next
        CopiarTabla(dtAprobOperacionesCaja, dsRepAprobOperacionesCaja.AprobOperacionesCaja)
        CopiarTabla(dtFirmantesRes, dsRepAprobOperacionesCaja.Firmantes)
        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Tesoreria/Reportes/rptAprobOperacionesCaja.rpt"
        Dim strFechaLiquidacion As String = IIf(Not (ViewState("fechaLiquidacion") Is Nothing), ViewState("fechaLiquidacion").ToString, String.Empty)
        oReport.Load(fileReport)
        oReport.SetDataSource(dsRepAprobOperacionesCaja)
        oReport.SetParameterValue("@Usuario", Usuario)
        oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        oReport.SetParameterValue("@Fecha", IIf(strFechaLiquidacion <> String.Empty, UIUtility.ConvertirFechaaString(CDec(strFechaLiquidacion)), String.Empty))
        If Not (oReport Is Nothing) Then
            oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
            File.Copy(rutaArchivo, folderActual & nombreNuevoArchivo)
        End If
        EjecutarJS("window.open('frmVisorAprobOperacionesCaja.aspx?archivo=" & rutaArchivo.Replace("\", "\\") & "');")
        Return folderActual & nombreNuevoArchivo
    End Function
    Private Sub EnviarClaveFirma(ByVal rutaArchivo As String, ByVal rutaCartas As String)
        Dim dtClaveFirmantes As New DataTable
        Dim codigoUsuario As String
        Dim clave As String
        Dim mensaje As String
        Dim toUser As String
        dtClaveFirmantes = New ParametrosGeneralesBM().ListarClaveFirmantesCartas(DatosRequest).Tables(0)
        For Each dr As DataRow In dtClaveFirmantes.Rows
            codigoUsuario = CType(dr("CodigoUsuario"), String)
            clave = CType(dr("ClaveFirma"), String)
            mensaje = HelpCorreo.MensajeNotificacionClave(rutaArchivo, rutaCartas, clave)
            toUser = New PersonalBM().SeleccionarMail(codigoUsuario.ToString())
            UIUtility.EnviarMail(toUser, "", "Notificación de clave - Operaciones Caja", mensaje, DatosRequest, rutaArchivo)
        Next
    End Sub
    Private Sub Mostrar_Reporte_Aprobacion(ByVal dsOperaciones As DataSet)
        Dim CodigoOrden As String = ""
        Dim indUbica As String = ""
        Dim RutaReporte As String = ""
        If btnFirmar.Visible = True And ddlEstado.SelectedValue = ParametrosSIT.ESTADO_CARTA_APR Then
            For Each dr As DataRow In dsOperaciones.Tables(0).Rows
                CodigoOrden = CType(dr("CodigoOperacionCaja"), String)
                RutaReporte = New AprobadorCartaBM().ObtenerRutaReporteAprobacion(Usuario, CodigoOrden, ParametrosSIT.VISTA_REP_APROB1)
                If indUbica = "" And RutaReporte <> "" Then
                    EjecutarJS("window.open('frmVisorAprobOperacionesCaja.aspx?archivo=" & RutaReporte.Replace("\", "\\") & "');")
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Function mensajeCartasFirmadas(codigoOrden As String) As String
        Dim dt As DataTable
        Dim bmOperacionesCaja As New OperacionesCajaBM
        Dim bmPersonal As New PersonalBM
        Dim mensaje As New StringBuilder
        Dim arrayCodigo As Array
        codigoOrden = codigoOrden.Substring(0, codigoOrden.Length - 1)
        arrayCodigo = codigoOrden.Split("|")
        With mensaje
            .Append("<table>")
            .Append("<tr><td>")
            .Append("El usuario " & bmPersonal.SeleccionarPorCodigoInterno(ObtenerValorRequest(DatosRequest, "Usuario"), DatosRequest) & " ha firmado las siguientes cartas")
            .Append("</td></tr>")
            .Append("</table>")
            .Append("<br/>")
            'Cabecera Tabla
            .Append("<table  border='1' cellpadding='2'>")
            .Append("<tr align='center'>")
            .Append("<td>PORTAFOLIO</td>")
            .Append("<td>C&Oacute;DIGO OPERACI&Oacute;N CAJA</td>")
            .Append("<td>OPERACI&Oacute;N</td>")
            .Append("<td>IMPORTE</td>")
            .Append("<td>MONEDA</td>")
            .Append("<td>VBADMIN</td>")
            .Append("<td>VBGERF1</td>")
            .Append("<td>VBGERF2</td>")
            .Append("</tr>")
            'Contenido Tabla
            For i = 0 To arrayCodigo.Length - 1
                If arrayCodigo(i) <> "" Then
                    dt = bmOperacionesCaja.SeleccionarAutorizacionCartas(String.Empty, String.Empty, String.Empty, String.Empty, ViewState("fechaLiquidacion"), False, String.Empty, arrayCodigo(i), DatosRequest).Tables(0)
                    If dt.Rows.Count > 0 Then
                        .Append("<tr>")
                        .Append("<td>" & dt.Rows(0)("DescripcionPortafolio").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("CodigoOperacionCaja").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("DescripcionOperacion").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("Importe").ToString.Trim & "</td>")
                        .Append("<td>" & dt.Rows(0)("CodigoMoneda").ToString.Trim & "</td>")
                        .Append("<td>" & bmPersonal.SeleccionarPorCodigoInterno(dt.Rows(0)("VBADMINUSR").ToString.Trim, DatosRequest) & "</td>")
                        .Append("<td>" & bmPersonal.SeleccionarPorCodigoInterno(dt.Rows(0)("VBGERF1USR").ToString.Trim, DatosRequest) & "</td>")
                        .Append("<td>" & bmPersonal.SeleccionarPorCodigoInterno(dt.Rows(0)("VBGERF2USR").ToString.Trim, DatosRequest) & "</td>")
                        .Append("</tr>")
                    End If
                End If
            Next
            .Append("</table>")
            .Append("<br/>")
            'Fin Contenido Tabla
            .Append("<table>")
            .Append("<tr><td>AFP Integra</td></tr>")
            .Append("<tr><td>Grupo SURA</td></tr>")
            .Append("</table>")
        End With
        Return mensaje.ToString
    End Function
    Private Function obtenerUsuarioAprobador() As String
        Dim dt As DataTable
        Dim bmAprobadorCarta As New AprobadorCartaBM
        Dim usuario As String = ""
        dt = bmAprobadorCarta.SeleccionarPorFiltro("", "1", "A", DatosRequest).Tables(0)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                usuario = usuario & dt.Rows(i)("email_trabajo").ToString.Trim() & ";"
            Next
        End If
        Return usuario
    End Function
    Public Function ObtenerValorRequest(ByVal dataRequest As DataSet, ByVal nombre As String) As String
        Dim columnName As String = dataRequest.Tables(0).Columns(0).ColumnName
        Return CType(dataRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
    End Function
#End Region
End Class