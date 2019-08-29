Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT
'Dividendos
Partial Class Modulos_ValorizacionCustodia_Custodia_frmRegistroDivRebLib
    Inherits BasePage
#Region "Variables"
    Dim oDividendosRebatesLiberadasBM As New DividendosRebatesLiberadasBM
    Dim oDividendosRebatesLiberadasBE As New DividendosRebatesLiberadasBE
    Dim oValoresBM As New ValoresBM
    Dim oUtil As New UtilDM
#End Region
#Region " /* Métodos de la Página */ "
    Private Function ValidaFecha() As Boolean
        Dim dsFechas As PortafolioBE, objferiadoBM As New FeriadoBM, objPortafolio As New PortafolioBM
        Dim drFechas As DataRow
        Dim blnResultado As Boolean = True
        dsFechas = objPortafolio.Seleccionar(ddlPortafolio.SelectedValue, DatosRequest)
        If dsFechas.Tables(0).Rows.Count > 0 Then
            drFechas = dsFechas.Tables(0).NewRow
            drFechas = dsFechas.Tables(0).Rows(0)
            Dim dblFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaIDI.Text)
            Dim dblFechaEntrega As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaEntrega.Text)
            Dim dblFechaConstitucion As Decimal = CType(drFechas("FechaConstitucion"), Decimal)
            Dim dblFechaValoracion As Decimal = CType(drFechas("FechaValoracion"), Decimal)
            If (dblFechaConstitucion < dblFechaOperacion) Then
                blnResultado = False
                AlertaJS("La fecha de operacion no debe ser mayor a la fecha de proceso del portafolio.")
            ElseIf (dblFechaValoracion >= dblFechaEntrega) Then
                blnResultado = False
                AlertaJS("Este portafolio ya es encuentra valorizado para esta fecha.")
            End If
        End If
        If (objferiadoBM.Feriado_ValidarFecha(UIUtility.ConvertirFechaaDecimal(tbFechaIDI.Text), ViewState("sMercado"))) = True Then
            blnResultado = False
            AlertaJS("La fecha de operación no puede ser día feriado.")
        End If
        Return blnResultado
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            btnEliminar.Attributes.Add("onclick", "return confirm('Esta seguro de eliminar el registro seleccionado.?');")

            If Not Page.IsPostBack Then
                ViewState("vsFechaSistema") = oUtil.RetornarFechaSistema
                Call VisibildadBotonesAccion(True, True, True, False, True)
                Call HabilitaControlesBusqueda(True, True, True, True)
                Call HabilitaControlesIngreso(False, False, False, False, False, False, False, False, False, False)
                Call VisibilidadBotones(False)
                UIUtility.CargarMonedaOI(dlMoneda)
                Call CargaPortafolio(ddlPortafolio)
                Call LimpiaDatos()                
            Else
                ViewState("vsDescripcion") = txtDescripcion.Text
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datos As String()
                datos = CType(Session("SS_DatosModal"), String())
                txtISIN.Text = datos(0)
                txtMnemonico.Text = datos(1)
                txtSBS.Text = datos(2)
                txtDescripcion.Text = HttpUtility.HtmlDecode(datos(3))
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            If txtMnemonico.Text.Trim <> "" Then
                If SeleccionarInstrumento("", txtMnemonico.Text.Trim) Then
                    AlertaJS(ObtenerMensaje("ALERT40"))
                    Exit Sub
                End If
            End If
            If txtSBS.Text.Trim = "" Then
                AlertaJS(ObtenerMensaje("ALERT32"))
                Exit Sub
            ElseIf Not SeleccionarInstrumentoPorCodigoSBS(txtSBS.Text.Trim) Then
                AlertaJS(ObtenerMensaje("ALERT41"))
                Exit Sub
            End If
            txtDescripcion.Text = HttpUtility.HtmlDecode(ViewState("vsDescripcion"))
            ViewState("vsModo") = "I"
            Call VisibildadBotonesAccion(False, False, False, False, False)
            Call VisibilidadBotones(True)
            Call VisibilidadBotonesBusqueda(False)
            Call HabilitaControlesIngreso(True, True, True, True, True, True, True, True, True, True)
            Call LimpiaDatos()
            If Session("sCodigoMoneda") <> Nothing And rdbOpt.SelectedValue = "Liberada" Then
                dlMoneda.SelectedValue = Session("sCodigoMoneda").ToString
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Try
            If ViewState("vscodSBS") <> "" And ViewState("vsIdentificador") <> 0 Then
                ViewState("vsModo") = "M"
                Call VisibildadBotonesAccion(False, False, False, False, False)
                Call VisibilidadBotones(True)
                Call VisibilidadBotonesBusqueda(False)
                Call HabilitaControlesIngreso(True, True, True, True, True, True, True, True, True, True)
                RecuperaTipoDistribucion()
            Else
                ViewState("vsModo") = ""
                ViewState("vsIdentificador") = 0
                AlertaJS(ObtenerMensaje("ALERT33"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un erro al Modificar")
        End Try
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            If ViewState("vscodSBS") <> "" And ViewState("vsIdentificador") <> 0 Then
                ViewState("vsModo") = "E"
                If EliminarDividendosLiberadasRebates() Then
                    ViewState("vsIdentificador") = 0
                    ViewState("vsModo") = ""
                    dgLista.SelectedIndex = -1
                    Call DescargaGrillaLista()
                    Call CargaInfoDividendosLiberadasRebates()
                    AlertaJS(ObtenerMensaje("ALERT36"))
                Else
                    AlertaJS("No se puede eliminar, el tipo de distribución seleccionado ya está confirmado")
                    Exit Sub
                End If
            Else
                ViewState("vsModo") = ""
                ViewState("vsIdentificador") = 0
                AlertaJS(ObtenerMensaje("ALERT33"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If ValidaFecha() Then
                Dim Multi As String
                Select Case ViewState("vsModo")
                    Case "I"
                        If InsertarDividendosLiberadasRebates() Then
                            If (ddlPortafolio.SelectedValue = ParametrosSIT.PORTAFOLIO_MULTIFONDOS) Then
                                Multi = "Y"
                            Else
                                Multi = "N"
                            End If
                            ImprimeDividendosLiberadasRebates(ViewState("vscodSBS"), ViewState("vsGrupoIdentificador"), ViewState("vsTipoDistribucion"), Multi)
                            Call LimpiaDatos()
                        Else
                            Exit Sub
                        End If
                    Case "M"
                        If ModificarDividendosLiberadasRebates() Then
                            dgLista.SelectedIndex = -1
                            Multi = "N"
                            ImprimeDividendosLiberadasRebates(ViewState("vscodSBS"), IIf(ViewState("vsGrupoIdentificador") Is DBNull.Value, 0, ViewState("vsGrupoIdentificador")), ViewState("vsTipoDistribucion"), Multi)
                            Call LimpiaDatos()
                        Else
                            Exit Sub
                        End If
                    Case "E"
                        If EliminarDividendosLiberadasRebates() Then
                            AlertaJS(ObtenerMensaje("ALERT36"))
                            Call LimpiaDatos()
                        Else
                            Exit Sub
                        End If
                End Select
                Call DescargaGrillaLista()
                Call CargaInfoDividendosLiberadasRebates()
                Call VisibildadBotonesAccion(True, True, True, False, True)
                Call VisibilidadBotones(False)
                Call VisibilidadBotonesBusqueda(True)
                Call HabilitaControlesIngreso(False, False, False, False, False, False, False, False, False, False)
                ViewState("vscodSBS") = ""
                ViewState("vsIdentificador") = 0
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            ViewState("vsModo") = ""
            Call VisibildadBotonesAccion(True, True, True, False, True)
            Call VisibilidadBotones(False)
            Call VisibilidadBotonesBusqueda(True)
            Call HabilitaControlesBusqueda(True, True, True, True)
            Call HabilitaControlesIngreso(False, False, False, False, False, False, False, False, False, False)
            ViewState("vscodSBS") = ""
            Call LimpiarCamposInstrumento()
            Call LimpiaDatos()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cancelar")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        Try
            SeleccionarInstrumento(txtISIN.Text.ToUpper.Trim, txtMnemonico.Text.ToUpper.Trim)
            ViewState("vsDescripcion") = txtDescripcion.Text.Trim
            Call CargaInfoDividendosLiberadasRebates()
            Call LimpiaDatos()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Consultar")
        End Try
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Try
            oDividendosRebatesLiberadasBM = Nothing
            oDividendosRebatesLiberadasBE = Nothing
            oValoresBM = Nothing
            oUtil = Nothing
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        End Try
    End Sub
#End Region
#Region " /* Funciones Personalizadas*/"
    Private Sub LimpiarCamposInstrumento()
        txtISIN.Text = ""
        txtSBS.Text = ""
        txtMnemonico.Text = ""
        txtDescripcion.Text = ""
    End Sub
    Private Function ImprimeDividendosLiberadasRebates(ByVal sCodigoSBS As String, ByVal nGrupoIdentificador As Decimal, ByVal sTipoDistribucion As String, ByVal sMulti As String) As Boolean
        Dim strurl As String = "Reportes/frmDivRebLibImpCR.aspx?CodigoSBS=" & sCodigoSBS & "&GrupoIdentificador=" & nGrupoIdentificador & "&TipoDistribucion=" & sTipoDistribucion & "&Multifondo=" & sMulti
        EjecutarJS("showWindow('" & strurl & "', '800', '600');")
        Return Nothing
    End Function
    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        drlista.DataSource = New PortafolioBM().Listar(Nothing, ESTADO_ACTIVO)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoPortafolioSBS" : drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista)
    End Sub
    Public Sub SeleccionarSBS(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String
        Dim strcodSBS As String
        Dim strIdentificador As Decimal
        Dim index As Integer
        strcadena = e.CommandArgument
        strcodSBS = strcadena.Split(",").GetValue(0)
        strIdentificador = strcadena.Split(",").GetValue(1)
        index = CInt(strcadena.Split(",").GetValue(2))
        ViewState("vscodSBS") = strcodSBS
        ViewState("vsIdentificador") = strIdentificador
        If ViewState("vscodSBS") <> "" And ViewState("vsIdentificador") <> 0 Then
            RecuperaTipoDistribucion()
            dgLista.SelectedIndex = index
        End If
    End Sub
    Private Sub LimpiaDatos()
        rdbOpt.SelectedIndex = 0
        txtFactor.Text = ""
        tbFechaCorte.Text = ViewState("vsFechaSistema")
        tbFechaEntrega.Text = ViewState("vsFechaSistema")
        tbFechaIDI.Text = ViewState("vsFechaSistema")
        dlMoneda.SelectedIndex = 0
        ddlPortafolio.SelectedIndex = 0
    End Sub
    Private Sub HabilitaControlesBusqueda(ByVal bISIN As Boolean, ByVal bSBS As Boolean, ByVal bMnemonico As Boolean, ByVal bDescripcion As Boolean)
        txtISIN.Enabled = bISIN
        txtSBS.Enabled = bSBS
        txtMnemonico.Enabled = bMnemonico
    End Sub
    Private Sub HabilitaControlesIngreso(ByVal bOpt As Boolean, ByVal bFactor As Boolean, ByVal bFechaCorte As Boolean, ByVal bFechaEntrega As Boolean, _
    ByVal bFechaIDI As Boolean, ByVal bdate1 As Boolean, ByVal bdate2 As Boolean, ByVal bdate3 As Boolean, ByVal bMoneda As Boolean, ByVal bPortafolio As Boolean)
        rdbOpt.Enabled = bOpt
        txtFactor.Enabled = bFactor
        tbFechaCorte.Enabled = bFechaCorte
        tbFechaEntrega.Enabled = bFechaEntrega
        tbFechaIDI.Enabled = bFechaIDI
        dlMoneda.Enabled = bMoneda
        ddlPortafolio.Enabled = bPortafolio
    End Sub
    Private Sub VisibildadBotonesAccion(ByVal bIngresar As Boolean, ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bImprimir As Boolean, ByVal bImportar As Boolean)
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnImprimir.Visible = bImprimir
        btnImportar.Visible = bImportar
    End Sub
    Private Sub VisibilidadBotonesBusqueda(ByVal bValor As Boolean)
        btnConsultar.Visible = bValor
    End Sub
    Private Sub VisibilidadBotones(ByVal bValor As Boolean)
        btnAceptar.Visible = bValor
        btnCancelar.Visible = bValor
        btnSalir.Visible = Not bValor
    End Sub
    Private Sub CargaInfoDividendosLiberadasRebates()
        Dim oDivLibTable As DataTable = oDividendosRebatesLiberadasBM.Listar(txtSBS.Text.Trim, txtMnemonico.Text.Trim, DatosRequest)
        dgLista.DataSource = oDivLibTable
        dgLista.DataBind()
    End Sub
    Private Function SeleccionarInstrumentoPorCodigoSBS(ByVal sCodigoSBS As String) As Boolean
        Dim oValoresBE As DataSet = oValoresBM.SeleccionarInstrumentoPorCodigoSBS(sCodigoSBS, DatosRequest)
        If oValoresBE.Tables(0).Rows.Count > 0 Then
            Return True
            Exit Function
        End If
        oValoresBE = Nothing
        Return False
    End Function
    Private Function SeleccionarInstrumento(ByVal sCodigoISIN As String, ByVal sCodigoMnemonico As String) As Boolean
        Dim oValoresBE As DataSet = oValoresBM.SeleccionarInstrumento(sCodigoISIN, sCodigoMnemonico, DatosRequest)
        Dim oValoresMonedaBE As ValoresBE = New ValoresBM().Seleccionar(sCodigoMnemonico, DatosRequest)
        If oValoresBE.Tables(0).Rows.Count > 0 Then
            txtISIN.Text = oValoresBE.Tables(0).Rows(0).ItemArray(0)
            txtMnemonico.Text = oValoresBE.Tables(0).Rows(0).ItemArray(1)
            txtSBS.Text = oValoresBE.Tables(0).Rows(0).ItemArray(2)
            txtDescripcion.Text = HttpUtility.HtmlDecode(oValoresBE.Tables(0).Rows(0).ItemArray(3))
            ViewState("sMercado") = "0"
            If oValoresMonedaBE.Valor.Rows.Count > 0 Then
                Session("sCodigoMoneda") = oValoresMonedaBE.Valor.Rows(0)("CodigoMoneda")
                ViewState("sMercado") = oValoresMonedaBE.Valor.Rows(0)("CodigoMercado")
            End If
            Return False
            Exit Function
        End If
        oValoresBE = Nothing
        Session("sCodigoMoneda") = Nothing
        ViewState("sMercado") = "0"
        Return True
    End Function
    Private Sub RecuperaTipoDistribucion()
        Dim strTipoDistribucion As String
        Dim oDivRebLibBE As DataSet = oDividendosRebatesLiberadasBM.Seleccionar(ViewState("vscodSBS"), ViewState("vsIdentificador"), "A", DatosRequest)
        If oDivRebLibBE.Tables(0).Rows.Count <= 0 Then
            AlertaJS(ObtenerMensaje("ALERT34"))
            Exit Sub
        End If
        Dim sFechaCorte As String = oDivRebLibBE.Tables(0).Rows(0).Item("FechaCorte")
        Dim sFechaEntrega As String = oDivRebLibBE.Tables(0).Rows(0).Item("FechaEntrega")
        Dim sFechaIDI As String = oDivRebLibBE.Tables(0).Rows(0).Item("FechaIDI")
        txtISIN.Text = oDivRebLibBE.Tables(0).Rows(0).Item("CodigoISIN")
        txtSBS.Text = ViewState("vscodSBS")
        txtMnemonico.Text = oDivRebLibBE.Tables(0).Rows(0).Item("CodigoNemonico")
        txtDescripcion.Text = HttpUtility.HtmlDecode(oDivRebLibBE.Tables(0).Rows(0).Item("Descripcion"))
        strTipoDistribucion = oDivRebLibBE.Tables(0).Rows(0).Item("TipoDistribucion")
        Select Case strTipoDistribucion.ToUpper
            Case "D"
                rdbOpt.SelectedIndex = 0
            Case "R"
                rdbOpt.SelectedIndex = 1
            Case "L"
                rdbOpt.SelectedIndex = 2
        End Select
        txtFactor.Text = oDivRebLibBE.Tables(0).Rows(0).Item("Factor")
        txtFactor.Text = txtFactor.Text.Replace(",", "")
        tbFechaCorte.Text = sFechaCorte.Substring(6, 2) & "/" & sFechaCorte.Substring(4, 2) & "/" & sFechaCorte.Substring(0, 4)
        tbFechaEntrega.Text = sFechaEntrega.Substring(6, 2) & "/" & sFechaEntrega.Substring(4, 2) & "/" & sFechaEntrega.Substring(0, 4)
        tbFechaIDI.Text = sFechaIDI.Substring(6, 2) & "/" & sFechaIDI.Substring(4, 2) & "/" & sFechaIDI.Substring(0, 4)
        dlMoneda.SelectedValue = oDivRebLibBE.Tables(0).Rows(0).Item("CodigoMoneda")
        Try
            ddlPortafolio.SelectedValue = oDivRebLibBE.Tables(0).Rows(0).Item("CodigoPortafolioSBS")
        Catch ex As Exception
            ddlPortafolio.SelectedIndex = 0
        End Try        
        'para lanzar la impresion necesito el grupo indentificador
        ViewState("vsGrupoIdentificador") = oDivRebLibBE.Tables(0).Rows(0).Item("GrupoIdentificador")
        oDivRebLibBE = Nothing
    End Sub
    Private Sub DescargaGrillaLista()
        Dim dtTabla As New DataTable
        dgLista.DataSource = Nothing
        dtTabla.Columns.Add("Acciones")
        dtTabla.Columns.Add("Factor")
        dtTabla.Columns.Add("Fecha de Corte")
        dtTabla.Columns.Add("Fecha de Entrega")
        dtTabla.Columns.Add("Beneficio")
        dtTabla.Columns.Add("CodigoSBS")
        dtTabla.Columns.Add("Identificador")
        dtTabla.Columns.Add("Moneda")
        dtTabla.Columns.Add("Fondo")
        dtTabla.Columns.Add("CodigoMoneda")
        dtTabla.Columns.Add("CodigoPortafolioSBS")
        Dim row As DataRow = dtTabla.NewRow()
        dtTabla.Rows.Add(row)
        dgLista.DataSource = dtTabla
        dgLista.DataBind()
    End Sub
    Private Function Validar() As Boolean
        Dim sFechaCorte As String = tbFechaCorte.Text.Trim
        Dim sFechaEntrega As String = tbFechaEntrega.Text.Trim
        Dim sFechaIDI As String = tbFechaIDI.Text.Trim
        Dim sTipoDistribucion As String = ""
        Select Case rdbOpt.SelectedIndex
            Case 0
                sTipoDistribucion = "D"
            Case 1
                sTipoDistribucion = "R"
            Case 2
                sTipoDistribucion = "L"
        End Select
        If txtISIN.Text.Trim <> "" And ViewState("vsModo") = "I" Then
            If SeleccionarInstrumento(txtISIN.Text.Trim, "") Then
                AlertaJS(ObtenerMensaje("ALERT39"))
                Return False
                Exit Function
            End If
        End If
        If txtMnemonico.Text.Trim <> "" And ViewState("vsModo") = "I" Then
            If SeleccionarInstrumento("", txtMnemonico.Text.Trim) Then
                AlertaJS(ObtenerMensaje("ALERT40"))
                Return False
                Exit Function
            End If
        End If
        If txtSBS.Text.Trim = "" And ViewState("vsModo") = "I" Then
            AlertaJS(ObtenerMensaje("ALERT32"))
            Return False
            Exit Function
        ElseIf txtMnemonico.Text.Trim = "" And txtISIN.Text.Trim = "" And ViewState("vsModo") = "I" Then
            AlertaJS(ObtenerMensaje("ALERT23"))
            Return False
            Exit Function
        ElseIf Not SeleccionarInstrumentoPorCodigoSBS(txtSBS.Text.Trim) Then
            AlertaJS(ObtenerMensaje("ALERT41"))
            Return False
            Exit Function
        ElseIf ddlPortafolio.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT106"))
            Return False
            Exit Function
        ElseIf dlMoneda.SelectedValue.Trim = "" And sTipoDistribucion <> "L" Then
            AlertaJS(ObtenerMensaje("ALERT105"))
            Return False
            Exit Function
        ElseIf sFechaCorte.Trim = "" And sFechaCorte.Trim <> "" Then
            AlertaJS(ObtenerMensaje("ALERT24"))
            Return False
            Exit Function
        ElseIf sFechaEntrega.Trim <> "" And sFechaEntrega.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT25"))
            Return False
            Exit Function
        ElseIf sFechaIDI.Trim <> "" And sFechaIDI.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT27"))
            Return False
            Exit Function
        ElseIf sFechaEntrega.Substring(6, 4) & sFechaEntrega.Substring(3, 2) & sFechaEntrega.Substring(0, 2) < _
                sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2) Then
            AlertaJS(ObtenerMensaje("ALERT28"))
            Return False
            Exit Function
        ElseIf sFechaIDI.Substring(6, 4) & sFechaIDI.Substring(3, 2) & sFechaIDI.Substring(0, 2) < _
                sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2) Then
            AlertaJS(ObtenerMensaje("ALERT29"))
            Return False
            Exit Function
        ElseIf txtFactor.Text.Trim = "" Or txtFactor.Text.Trim = "." Or txtFactor.Text.Trim = "," Then
            AlertaJS(ObtenerMensaje("ALERT30"))
            Return False
            Exit Function
        ElseIf System.Convert.ToDecimal(txtFactor.Text.Trim) <= 0 Then
            AlertaJS(ObtenerMensaje("ALERT124"))
            Return False
            Exit Function
        ElseIf sTipoDistribucion.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT26"))
            Return False
            Exit Function
        Else
            If sTipoDistribucion = "L" Then
                Select Case ViewState("vsModo")
                    Case "I"
                        If oDividendosRebatesLiberadasBM.NemonicoFechaidi_Contar(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) <> "0" Then
                            If Convert.ToDouble(oDividendosRebatesLiberadasBM.NemonicoFechaidi_Factor(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2))) <> Convert.ToDouble(txtFactor.Text) Then
                                If oDividendosRebatesLiberadasBM.NemonicoFechaidi_VerificaActual(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) = "1" Then
                                    If oDividendosRebatesLiberadasBM.NemonicoFechaidi_NumeroUnidades(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) <> "" Then
                                        AlertaJS("Se recalculara el (Número de Unidades) en Valores")
                                        Return True
                                        Exit Function
                                    Else
                                        AlertaJS("Falta información para recalcular de (Número de Unidades) en Valores")
                                        Return False
                                        Exit Function
                                    End If
                                Else
                                    AlertaJS("No se puede recalcular el (Número de Unidades) en Valores, no es es Mnemonico Actual en Trabajo")
                                    Return False
                                    Exit Function
                                End If
                            End If
                        End If
                    Case "M"
                        If Convert.ToDouble(oDividendosRebatesLiberadasBM.NemonicoFechaidi_Factor(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2))) <> Convert.ToDouble(txtFactor.Text) Then
                            If oDividendosRebatesLiberadasBM.NemonicoFechaidi_VerificaActual(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) = "1" Then
                                If oDividendosRebatesLiberadasBM.NemonicoFechaidi_NumeroUnidades(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) <> "" Then
                                    AlertaJS("Se recalculara el (Número de Unidades) en Valores")
                                    Return True
                                    Exit Function
                                Else
                                    AlertaJS("Falta información para recalcular de (Número de Unidades) en Valores")
                                    Return False
                                    Exit Function
                                End If
                            Else
                                AlertaJS("No se puede recalcular el (Número de Unidades) en Valores, no es Mnemonico Actual en Trabajo")
                                Return False
                                Exit Function
                            End If
                        End If
                    Case "E"
                        If oDividendosRebatesLiberadasBM.NemonicoFechaidi_Contar(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) = "1" Then
                            If oDividendosRebatesLiberadasBM.NemonicoFechaidi_VerificaActual(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) = "1" Then
                                If oDividendosRebatesLiberadasBM.NemonicoFechaidi_NumeroUnidades(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) <> "" Then
                                    AlertaJS("Se recalculara el (Número de Unidades) en Valores")
                                    Return True
                                    Exit Function
                                Else
                                    AlertaJS("Falta información para recalcular de (Número de Unidades) en Valores")
                                    Return False
                                    Exit Function
                                End If
                            Else
                                AlertaJS("No se puede Eliminar, porque no se puede recalcular el (Número de Unidades) en Valores, no es Mnemonico Actual en Trabajo")
                                Return False
                                Exit Function
                            End If
                        End If
                End Select
            End If
        End If
        Return True
    End Function
    Private Function InsertarDividendosLiberadasRebates() As Boolean
        Dim sFechaCorte As String = tbFechaCorte.Text.Trim
        Dim sFechaEntrega As String = tbFechaEntrega.Text.Trim
        Dim sFechaIDI As String = tbFechaIDI.Text.Trim
        Dim sTipoDistribucion As String
        Dim nIdentificador As Decimal = 0
        Dim nGrupoIdentificador As Decimal = 0
        Dim bTransaccion As Boolean = False
        Select Case rdbOpt.SelectedIndex
            Case 0
                sTipoDistribucion = "D"
            Case 1
                sTipoDistribucion = "R"
            Case 2
                sTipoDistribucion = "L"
            Case Else
                sTipoDistribucion = ""
        End Select
        If sTipoDistribucion = "L" Then
            tbFechaIDI.Text = tbFechaEntrega.Text
        End If
        If Validar() Then
            oDividendosRebatesLiberadasBE = CrearObjetoDividendosLiberadasRebates(txtSBS.Text.Trim, 0, txtISIN.Text.Trim, txtMnemonico.Text.Trim, _
            sFechaIDI.Substring(6, 4) & sFechaIDI.Substring(3, 2) & sFechaIDI.Substring(0, 2), txtFactor.Text.Trim, _
            sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2), _
            sFechaEntrega.Substring(6, 4) & sFechaEntrega.Substring(3, 2) & sFechaEntrega.Substring(0, 2), _
            sTipoDistribucion, dlMoneda.SelectedValue.Trim, ddlPortafolio.SelectedValue.Trim)

            bTransaccion = oDividendosRebatesLiberadasBM.Insertar(oDividendosRebatesLiberadasBE, nIdentificador, nGrupoIdentificador, DatosRequest)

            ViewState("vsIdentificador") = nIdentificador
            If ddlPortafolio.SelectedValue <> "MULTIFONDO" Then
                ViewState("vsGrupoIdentificador") = nIdentificador
            Else
                ViewState("vsGrupoIdentificador") = nGrupoIdentificador
            End If
            ViewState("vscodSBS") = txtSBS.Text.Trim
            ViewState("vsTipoDistribucion") = sTipoDistribucion
            Return bTransaccion
        Else
            ViewState("vsIdentificador") = 0
            ViewState("vsGrupoIdentificador") = 0
            ViewState("vsTipoDistribucion") = ""
            Return bTransaccion
        End If
    End Function
    Private Function ModificarDividendosLiberadasRebates() As Boolean
        Dim sFechaCorte As String = tbFechaCorte.Text.Trim
        Dim sFechaEntrega As String = tbFechaEntrega.Text.Trim
        Dim sFechaIDI As String = tbFechaIDI.Text.Trim
        Dim sTipoDistribucion As String
        Dim nGrupoIdentificador As Decimal = 0
        Select Case rdbOpt.SelectedIndex
            Case 0
                sTipoDistribucion = "D"
            Case 1
                sTipoDistribucion = "R"
            Case 2
                sTipoDistribucion = "L"
            Case Else
                sTipoDistribucion = ""
        End Select
        If Validar() Then
            oDividendosRebatesLiberadasBE = CrearObjetoDividendosLiberadasRebates(txtSBS.Text.Trim, ViewState("vsIdentificador"), txtISIN.Text.Trim, _
            txtMnemonico.Text.Trim, sFechaIDI.Substring(6, 4) & sFechaIDI.Substring(3, 2) & sFechaIDI.Substring(0, 2), txtFactor.Text.Trim, _
            sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2), _
            sFechaEntrega.Substring(6, 4) & sFechaEntrega.Substring(3, 2) & sFechaEntrega.Substring(0, 2), _
            sTipoDistribucion, dlMoneda.SelectedValue.Trim, ddlPortafolio.SelectedValue.Trim)
            ViewState("vscodSBS") = txtSBS.Text.Trim
            ViewState("vsTipoDistribucion") = sTipoDistribucion
            Return oDividendosRebatesLiberadasBM.Modificar(oDividendosRebatesLiberadasBE, DatosRequest)
        Else
            Return False
        End If
    End Function
    Private Function EliminarDividendosLiberadasRebates() As Boolean
        If oDividendosRebatesLiberadasBM.NemonicoFechaidi_Contar(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) = "1" Then
            If oDividendosRebatesLiberadasBM.NemonicoFechaidi_VerificaActual(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) = "1" Then
                If oDividendosRebatesLiberadasBM.NemonicoFechaidi_NumeroUnidades(txtMnemonico.Text.Trim(), tbFechaIDI.Text.Trim.Substring(6, 4) & tbFechaIDI.Text.Trim.Substring(3, 2) & tbFechaIDI.Text.Trim.Substring(0, 2)) <> "" Then
                    AlertaJS("Se recalculara el (Número de Unidades) en Valores")
                Else
                    AlertaJS("Falta información para recalcular el (Número de Unidades) en Valores")
                    Return False
                    Exit Function
                End If
            Else
                AlertaJS("No se puede Eliminar, porque no se puede recalcular el (Número de Unidades) en Valores, no es Mnemonico Actual en Trabajo")
                Return False
                Exit Function
            End If
        End If
        Return oDividendosRebatesLiberadasBM.Eliminar(ViewState("vscodSBS"), ViewState("vsIdentificador"), DatosRequest)
    End Function
    Private Function CrearObjetoDividendosLiberadasRebates(ByVal CodigoSBS As String, ByVal Identificador As Decimal, ByVal CodigoISIN As String, _
    ByVal CodigoMnemonico As String, ByVal FechaIDI As Decimal, ByVal Factor As Decimal, ByVal FechaCorte As Decimal, ByVal FechaEntrega As Decimal, _
    ByVal TipoDistribucion As String, ByVal CodigoMoneda As String, ByVal CodigoPortafolioSBS As String) As DividendosRebatesLiberadasBE
        Dim oDividendosRebatesLiberadasBE As New DividendosRebatesLiberadasBE
        Dim oRow As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow
        oRow = CType(oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.NewRow(), DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow)
        oRow.CodigoSBS() = CodigoSBS
        oRow.Identificador() = Identificador
        oRow.CodigoISIN() = CodigoISIN
        oRow.CodigoNemonico() = CodigoMnemonico
        oRow.CodigoISIN() = CodigoISIN
        oRow.FechaIDI() = FechaIDI
        oRow.Factor() = Factor
        oRow.FechaCorte() = FechaCorte
        oRow.FechaEntrega() = FechaEntrega
        oRow.TipoDistribucion() = TipoDistribucion
        oRow.CodigoMoneda() = CodigoMoneda
        oRow.CodigoPortafolioSBS() = CodigoPortafolioSBS
        oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.AddDividendosRebatesLiberadasRow(oRow)
        oDividendosRebatesLiberadasBE.DividendosRebatesLiberadas.AcceptChanges()
        Return oDividendosRebatesLiberadasBE
    End Function
#End Region
    Protected Sub ddlPortafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        tbFechaIDI.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
    End Sub
    Protected Sub rdbOpt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbOpt.SelectedIndexChanged
        If Session("sCodigoMoneda") <> Nothing And rdbOpt.SelectedValue = "Liberada" Then
            dlMoneda.SelectedValue = Session("sCodigoMoneda").ToString
        End If
    End Sub

    Protected Sub btnImportar_Click(sender As Object, e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmRegistroDivRebLibImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try
    End Sub
End Class