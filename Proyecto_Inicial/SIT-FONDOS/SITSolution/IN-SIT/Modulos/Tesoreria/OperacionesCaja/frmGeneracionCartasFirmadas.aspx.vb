Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Text
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Tesoreria_OperacionesCaja_frmGeneracionCartasFirmadas
    Inherits BasePage
    Dim oOperacionesCaja As New OperacionesCajaBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

#Region "Funciones Personalizadas"
    Public Sub CargarPagina()
        btnAnularImp.Attributes.Add("onclick", "javascript:return ValidarSeleccion();")
        btnPerdidaImp.Attributes.Add("onclick", "javascript:return ValidarSeleccion();")
        'btnImprimir.Attributes.Add("onclick", "javascript:return ValidarSeleccion();")
        btnAceptarImp.Attributes.Add("onclick", "javascript:return ValidarImpresion();")
        CargarPortafolio()
        CargarMercado()
        CargarBanco(ddlMercado.SelectedValue, True)
        CargarIntermediario(True)
        CargarEstadoCartaImpresion(True)
        EstablecerFecha()
        divRangoImpresion.Visible = False
        ViewState("codigoMercado") = ddlMercado.SelectedValue
        ViewState("codigoPortafolio") = ddlPortafolio.SelectedValue
        ViewState("CodigoTercero") = ddlIntermediario.SelectedValue
        ViewState("codigoTerceroBanco") = ddlBanco.SelectedValue
        ViewState("fechaLiquidacion") = UIUtility.ConvertirFechaaDecimal(IIf(tbFecha.Text = "", "0", tbFecha.Text))
        ViewState("estadoCarta") = ddlEstado.SelectedValue
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            'Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
            ddlPortafolio.Items.Insert(0, New ListItem("TODOS", "")) 'CMB 20110425 Nro 27
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
            HelpCombo.LlenarComboBox(ddlMercado, dsMercado.Tables(0), "CodigoMercado", "Descripcion", True)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
        If (ddlMercado.Items.Count > 1) Then
            ddlMercado.SelectedIndex = 2
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
    Private Sub CargarEstadoCartaImpresion(ByVal enabled As Boolean)
        If enabled Then
            Dim dtEstados As New DataTable
            dtEstados = New ParametrosGeneralesBM().Listar(ParametrosSIT.ESTADO_CARTA_IMPRESION, DatosRequest)
            HelpCombo.LlenarComboBox(ddlEstado, dtEstados, "Valor", "Comentario", True)
            ddlEstado.SelectedValue = ParametrosSIT.ESTADO_CARTA_NIMP
        Else
            ddlEstado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlEstado)
        End If
        ddlEstado.Enabled = enabled
    End Sub
    Private Sub EstablecerFecha()
        tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
    End Sub

    Private Sub CargarGrilla(Optional ByVal fechaLiquidacion As Decimal = 0, _
            Optional ByVal codigoMercado As String = "", _
            Optional ByVal codigoPortafolio As String = "", _
            Optional ByVal codigoTercero As String = "", _
            Optional ByVal codigoTerceroBanco As String = "", _
            Optional ByVal estadoCarta As String = "", _
            Optional ByVal codigoOperacionCaja As String = "")

        Dim dsOperaciones As DataSet = New OperacionesCajaBM().SeleccionarCartasFirmadas(codigoMercado, _
         codigoPortafolio, codigoTercero, codigoTerceroBanco, fechaLiquidacion, False, estadoCarta, codigoOperacionCaja, DatosRequest)

        dgLista.DataSource = dsOperaciones
        dgLista.DataBind()
        lbContador.Text = UIUtility.MostrarResultadoBusqueda(dsOperaciones.Tables(0))
    End Sub

    Private Sub HabilitaBotones(ByVal visible As Boolean)
        btnBuscar.Visible = visible
        btnImprimir.Visible = visible
        btnAnularImp.Visible = visible
        btnPerdidaImp.Visible = visible
        btnVista.Visible = visible
    End Sub

    Private Function SeleccionaCartasImpresion(ByVal enabled As Boolean, Optional ByRef codigoOperacionCaja As ArrayList = Nothing) As ArrayList
        Dim chkSelect As System.Web.UI.WebControls.CheckBox
        Dim lbEstado As System.Web.UI.WebControls.Label
        Dim lbCodigo As System.Web.UI.WebControls.Label
        Dim lbCodigoOp As System.Web.UI.WebControls.Label
        Dim codigoImpresion As New ArrayList
        codigoOperacionCaja = New ArrayList
        For Each fila As GridViewRow In dgLista.Rows
            If fila.RowType = DataControlRowType.DataRow Then
                chkSelect = CType(fila.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                lbEstado = CType(fila.FindControl("lbEstado"), System.Web.UI.WebControls.Label)
                lbCodigo = CType(fila.FindControl("lbCodigo"), System.Web.UI.WebControls.Label)
                lbCodigoOp = CType(fila.FindControl("lbCodigoOp"), System.Web.UI.WebControls.Label)
                chkSelect.Enabled = enabled
                If chkSelect.Checked Then
                    If lbEstado.Text = ParametrosSIT.ESTADO_CARTA_NIMP Then
                        chkSelect.Checked = True
                        codigoImpresion.Add(CType(lbCodigo.Text, Decimal))
                        codigoOperacionCaja.Add(lbCodigoOp.Text)
                    Else
                        chkSelect.Checked = False
                    End If
                End If
            End If
        Next
        Return codigoImpresion
    End Function
    Private Sub CalcularRangosInicialFinal(ByVal cantidad As Decimal)
        If cantidad > 0 Then
            Dim dsRangos As DataSet
            dsRangos = oOperacionesCaja.CalcularRangosInicialFinalCartas(cantidad, DatosRequest)
            tbRangoInicial.Text = CType(dsRangos.Tables(0).Rows(0)("RangoInicial"), String)
            tbRangoFinal.Text = CType(dsRangos.Tables(0).Rows(0)("RangoFinal"), String)
        End If
    End Sub

#End Region

#Region "Eventos de la Pagina"
    Private Sub btnAceptarImp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarImp.Click
        Try
            Dim codigoImpresion As New ArrayList
            Dim codigoOperacionCaja As New ArrayList
            Dim codigoRango As Decimal = 0
            Dim cont As Decimal = 0
            codigoImpresion = ViewState("codigoImpresion")
            codigoOperacionCaja = ViewState("codigoOperacionCaja")
            If Val(tbRangoInicial.Text) <= Val(tbRangoFinal.Text) Then
                oOperacionesCaja.InsertarRangoImpresionCartas(Val(tbRangoInicial.Text), Val(tbRangoFinal.Text), codigoImpresion.Count, codigoRango, DatosRequest)
                For codigoCarta As Decimal = Val(tbRangoInicial.Text) To Val(tbRangoFinal.Text)
                    oOperacionesCaja.ImprimirCarta(CType(codigoImpresion(cont), Decimal), codigoCarta, codigoRango, DatosRequest, ESTADO_CARTA_NIMP)    'HDG 20120120
                    cont = cont + 1
                Next
                If cont > 0 Then
                    ImprimirCartasPDF(ParametrosSIT.IMPRESION_CARTAS, codigoOperacionCaja)
                    CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
                    divRangoImpresion.Visible = False
                    HabilitaBotones(True)
                End If
                'HDG 20120120
                cont = 0
                For codigoCarta As Decimal = Val(tbRangoInicial.Text) To Val(tbRangoFinal.Text)
                    oOperacionesCaja.ImprimirCarta(CType(codigoImpresion(cont), Decimal), codigoCarta, codigoRango, DatosRequest, ESTADO_CARTA_IMP)
                    cont = cont + 1
                Next
                'HDG 20120120
            Else
                AlertaJS("Ingrese correctamente los rangos de impresión! ")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnAnularImp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnularImp.Click
        Try
            Dim chkSelect As System.Web.UI.WebControls.CheckBox
            Dim lbCodigo As System.Web.UI.WebControls.Label
            Dim lbEstado As System.Web.UI.WebControls.Label
            Dim oOperacionesCaja As New OperacionesCajaBM
            Dim cont As Integer = 0
            For Each fila As GridViewRow In dgLista.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                    lbCodigo = CType(fila.FindControl("lbCodigo"), Label)
                    lbEstado = CType(fila.FindControl("lbEstado"), Label)
                    If chkSelect.Checked Then
                        If lbEstado.Text = ParametrosSIT.ESTADO_CARTA_IMP Then
                            oOperacionesCaja.ActualizarEstadoImpresion(CType(lbCodigo.Text, Decimal), ParametrosSIT.ESTADO_CARTA_AIMP, DatosRequest)
                            cont = cont + 1
                        End If
                    End If
                End If
            Next
            If cont > 0 Then
                CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
                AlertaJS("Se ha realizado la anulación de impresión de " & cont & " Carta(s)")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
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
            dgLista.SelectedIndex = -1
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnCancelarImp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelarImp.Click
        Try
            HabilitaBotones(True)
            SeleccionaCartasImpresion(True)
            ViewState("codigoImpresion") = Nothing
            ViewState("codigoOperacionCaja") = Nothing
            tbRangoInicial.Text = ""
            tbRangoFinal.Text = ""
            divRangoImpresion.Visible = False
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try


            Dim codigoImpresion As New ArrayList
            Dim codigoOperacionCaja As New ArrayList
            HabilitaBotones(False)
            codigoImpresion = SeleccionaCartasImpresion(False, codigoOperacionCaja)
            If codigoImpresion.Count > 0 Then
                ViewState("codigoImpresion") = codigoImpresion
                ViewState("codigoOperacionCaja") = codigoOperacionCaja
                CalcularRangosInicialFinal(codigoImpresion.Count)
                divRangoImpresion.Visible = True
            Else
                SeleccionaCartasImpresion(True)
                HabilitaBotones(True)
                AlertaJS("No hay registros disponibles para imprimir!")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnPerdidaImp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPerdidaImp.Click
        Try
            Dim chkSelect As System.Web.UI.WebControls.CheckBox
            Dim lbCodigo As System.Web.UI.WebControls.Label
            Dim lbEstado As System.Web.UI.WebControls.Label
            Dim cont As Integer = 0
            For Each fila As GridViewRow In dgLista.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                    lbCodigo = CType(fila.FindControl("lbCodigo"), Label)
                    lbEstado = CType(fila.FindControl("lbEstado"), Label)
                    If chkSelect.Checked Then
                        If lbEstado.Text = ParametrosSIT.ESTADO_CARTA_IMP Then
                            oOperacionesCaja.ActualizarEstadoImpresion(CType(lbCodigo.Text, Decimal), ParametrosSIT.ESTADO_CARTA_PIMP, DatosRequest)
                            cont = cont + 1
                        End If
                    End If
                End If
            Next
            If cont > 0 Then
                CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
                AlertaJS("Se ha realizado la perdida de impresión de " & cont & " Carta(s)")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
#End Region

    Private Sub ImprimirCartasPDF(Optional ByVal opcion As Decimal = 0, Optional ByVal codigoOperacionCaja As ArrayList = Nothing)
        Dim strTempCarta As String
        Dim ruta As String = New ParametrosGeneralesBM().ListarRutaGeneracionCartas(DatosRequest) + "\"
        Dim path As String
        Dim chk As System.Web.UI.WebControls.CheckBox
        Dim windowsCartas As New System.Text.StringBuilder
        Dim rutaCartas As String
        Dim veces As Integer
        Dim lbCodigoOp As System.Web.UI.WebControls.Label
        Dim dsOperaciones As DataSet
        veces = 0

        Session("RutaCarta") = ""

        If opcion = ParametrosSIT.IMPRESION_CARTAS Then
            For i As Integer = 0 To codigoOperacionCaja.Count - 1
                strTempCarta = Usuario.ToString.Trim() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:hhmmss}", DateTime.Now) & veces.ToString
                path = ruta & strTempCarta & ".pdf"
                dsOperaciones = oOperacionesCaja.SeleccionarCartasFirmadas(ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), _
                  ViewState("codigoTerceroBanco"), ViewState("fechaLiquidacion"), False, ParametrosSIT.ESTADO_CARTA_NIMP, CType(codigoOperacionCaja(i), String), DatosRequest)   'HDG 20120120
                'CrearCartaPDF(dsOperaciones.Tables(0).Rows(0), path)
                HelpCarta.CrearCartaPDF(dsOperaciones.Tables(0).Rows(0), DatosRequest, path)
                windowsCartas.Append(path & "&")
                veces = veces + 1
            Next
        Else
            For Each item As GridViewRow In dgLista.Rows
                If item.RowType = DataControlRowType.DataRow Then
                    If (item.FindControl("chkSelect") Is Nothing) = False Then
                        chk = CType(item.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox)
                        If (chk.Checked = True) Then
                            strTempCarta = Usuario.ToString.Trim() & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:hhmmss}", DateTime.Now) & veces.ToString
                            path = ruta & strTempCarta & ".pdf"
                            lbCodigoOp = CType(item.FindControl("lbCodigoOp"), System.Web.UI.WebControls.Label)
                            dsOperaciones = oOperacionesCaja.SeleccionarCartasFirmadas(ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), _
                              ViewState("codigoTerceroBanco"), ViewState("fechaLiquidacion"), False, ViewState("estadoCarta"), lbCodigoOp.Text, DatosRequest)
                            'CrearCartaPDF(dsOperaciones.Tables(0).Rows(0), path)
                            HelpCarta.CrearCartaPDF(dsOperaciones.Tables(0).Rows(0), DatosRequest, path)
                            windowsCartas.Append(path & "&")
                            veces = veces + 1
                        End If
                    End If
                End If
            Next
        End If
        rutaCartas = windowsCartas.ToString()
        If (rutaCartas.Length > 0) Then

            rutaCartas = rutaCartas.Remove(rutaCartas.Length - 1, 1)

            If (veces = 1) Then
                Session("RutaCarta") = rutaCartas
            Else
                Session("RutaCarta") = CrearMultiCartaPDF(rutaCartas)
            End If

            If IO.File.Exists(Session("RutaCarta")) Then
                EjecutarJS("window.open('frmVisorCarta.aspx?');")
            Else
                AlertaJS(ObtenerMensaje("ALERT126"))
            End If
        End If
    End Sub

    Private Sub btnVista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Try
            ImprimirCartasPDF()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Function CrearTablaValores(ByVal drDocumento As DataRow) As DataTable
        Dim dtValores As New DataTable
        Dim drValor As DataRow
        dtValores.Columns.Add("Find", GetType(String))
        dtValores.Columns.Add("New", GetType(String))

        Dim dsEstructuraCarta As DataTable = New ModeloCartaBM().SeleccionarCartaEstructuraPorModelo(drDocumento("CodigoModelo")).Tables(0)

        For Each drEtiqueta As DataRow In dsEstructuraCarta.Rows
            drValor = dtValores.NewRow
            drValor("Find") = drEtiqueta("NombreCampo")
            drValor("New") = drDocumento(drEtiqueta("OrigenCampo"))
            dtValores.Rows.Add(drValor)
        Next

        Return dtValores
    End Function

    Private Function CrearCartaPDF(ByVal drValores As DataRow, ByVal archivoPDF As String) As Boolean
        If drValores("ArchivoPlantilla") = "" Then Return False
        Try
            Dim strRutaHTML = drValores("ArchivoPlantilla").Replace("dot", "html")
            'Dim strRutaPDF = _strcartas & "\" & nombreArchivo & ".pdf"
            Dim strRutaPDF = archivoPDF

            If Not File.Exists(strRutaHTML) Then Return False
            If File.Exists(strRutaPDF) Then File.Delete(strRutaPDF)

            UIUtility.CrearArchivoPDF(CrearTablaValores(drValores), strRutaHTML, strRutaPDF)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Private Function CrearMultiCartaPDF(ByVal strCartas As String)
        Dim strDirecctorio As String
        Dim strNombreArchivo As String
        Dim arrCartas As String()
        Dim oPdfReader As PdfReader
        Dim oPdfWriter As PdfWriter
        Dim oDocument As New iTextSharp.text.Document(iTextSharp.text.PageSize.A5)
        Dim oPdfImportedPage As PdfImportedPage

        Try
            arrCartas = strCartas.Split(New Char() {"&"})
            If arrCartas.Length > 0 Then strDirecctorio = System.IO.Path.GetDirectoryName(arrCartas(0))

            strNombreArchivo = strDirecctorio & "\" & System.Guid.NewGuid().ToString() & ".pdf"
            oPdfWriter = PdfWriter.GetInstance(oDocument, New FileStream(strNombreArchivo, FileMode.Create))
            oDocument.Open()

            For Each strCartaPDF As String In arrCartas

                If File.Exists(strCartaPDF) Then
                    oPdfReader = New PdfReader(strCartaPDF)

                    For x As Integer = 1 To oPdfReader.NumberOfPages
                        oDocument.NewPage()
                        oPdfImportedPage = oPdfWriter.GetImportedPage(oPdfReader, x)

                        If oPdfReader.GetPageRotation(x) = 90 Or oPdfReader.GetPageRotation(x) = 270 Then
                            oPdfWriter.DirectContent.AddTemplate(oPdfImportedPage, 0, -1.0F, 1.0F, 0, 0, oPdfReader.GetPageSizeWithRotation(x).Height)
                        Else
                            oPdfWriter.DirectContent.AddTemplate(oPdfImportedPage, 1.0F, 0, 0, 1.0F, 0, 0)
                        End If
                    Next
                    oPdfReader.Close()
                End If
            Next
            oDocument.Close()
            oPdfWriter.Close()

            Return strNombreArchivo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        ViewState("codigoMercado") = ddlMercado.SelectedValue
        ViewState("codigoPortafolio") = ddlPortafolio.SelectedValue
        ViewState("codigoTercero") = ddlIntermediario.SelectedValue
        ViewState("codigoTerceroBanco") = ddlBanco.SelectedValue
        ViewState("fechaLiquidacion") = UIUtility.ConvertirFechaaDecimal(IIf(tbFecha.Text = "", "0", tbFecha.Text))
        ViewState("estadoCarta") = ddlEstado.SelectedValue
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla(ViewState("fechaLiquidacion"), ViewState("codigoMercado"), ViewState("codigoPortafolio"), ViewState("codigoTercero"), ViewState("codigoTerceroBanco"), ViewState("estadoCarta"))
        dgLista.SelectedIndex = -1
    End Sub
End Class
