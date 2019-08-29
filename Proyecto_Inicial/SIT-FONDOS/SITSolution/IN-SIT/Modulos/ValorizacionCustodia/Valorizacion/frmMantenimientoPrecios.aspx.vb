Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmMantenimientoPrecios
    Inherits BasePage

#Region " /* Declaración Variables */ "
    Dim oUIUtil As New UIUtility
    Dim oVectorPrecioBM As New VectorPrecioBM
    Dim oVectorPrecioBE As VectorPrecioBE
    Dim oUtil As New UtilDM
    Private campos() As String = {"Descripción del Valor", "Precio Anterior", "Precio Actual"}
#End Region

#Region " /* Métodos de la Página */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                'ViewState("vsFuenteInformacion") = "Real"
                tbFechaOperacion.Text = oUtil.RetornarFechaSistema
                tbFechaOperacionFin.Text = oUtil.RetornarFechaSistema
                Call DescargaGrillaLista()
                Call CargaTipoInstrumento()
                Call CargaEntidadExterna()
                ViewState("vsConsulta") = 1
                'Call CargaVectorPrecio(ViewState("vsFuenteInformacion"))
                Call CargaVectorPrecio()
                txtFechaFin.ReadOnly = True
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                txtISIN.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                txtSBS.Text = CType(Session("SS_DatosModal"), String())(2).ToString()
                txtDescripcion.Text = HttpUtility.HtmlDecode(CType(Session("SS_DatosModal"), String())(3).ToString())
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Try
            'If UCase(ViewState("vsFuenteInformacion")) <> "REAL" Then
            '    AlertaJS(ObtenerMensaje("ALERT120"))
            '    Exit Sub
            'End If
            If ViewState("vsConsulta") = 0 Then
                AlertaJS(ObtenerMensaje("ALERT119"))
                Exit Sub
            End If
            If ViewState("vsCodigoMnemonico") <> "" And ViewState("vsFecha") <> "" And ViewState("vsEntidadExt") <> "" Then
                Call HabilitaBotones(False, True, True, False, False)
                Call HabilitaControlesEdicion(True)
                Call HabilitaControlesFiltro(False, False, False, False, False, False, False, False, False)
            Else
                ViewState("vsCodigoMnemonico") = ""
                ViewState("vsFecha") = ""
                ViewState("vsEntidadExt") = ""
                AlertaJS(ObtenerMensaje("ALERT77"))
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If ModificarPrecioSBS() Then
                AlertaJS(ObtenerMensaje("ALERT75"))
                Call LimpiarControlesEdicion()
                'Call CargaVectorPrecio(ViewState("vsFuenteInformacion"))
                Call CargaVectorPrecio()
            Else
                AlertaJS(ObtenerMensaje("ALERT76"))
                Exit Sub
            End If
            Call HabilitaBotones(True, False, False, True, True)
            Call HabilitaControlesEdicion(False)
            Call HabilitaControlesFiltro(True, True, True, True, True, True, True, True, True)
            ViewState("vsCodigoMnemonico") = ""
            ViewState("vsFecha") = ""
            ViewState("vsEntidadExt") = ""
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Call HabilitaBotones(True, False, False, True, True)
            Call HabilitaControlesEdicion(False)
            Call HabilitaControlesFiltro(True, True, True, True, True, True, True, True, True)
            Call LimpiarControlesEdicion()
            ViewState("vsCodigoMnemonico") = ""
            ViewState("vsFecha") = ""
            ViewState("vsEntidadExt") = ""
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub ddlEntidadExterna_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlEntidadExterna.SelectedIndexChanged
        Try
            ViewState("vsFuenteInformacion") = ddlEntidadExterna.SelectedValue.Trim
            ViewState("vsConsulta") = 0
            Call DescargaGrillaLista()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsulta.Click
        Try
            Dim fechainicio As String
            Dim fechafin As String
            If (Me.tbFechaOperacion.Text = "") Then
                AlertaJS("Debe seleccionar una fecha de busqueda inicio")
                Exit Sub
            End If
            If (Me.tbFechaOperacionFin.Text = "") Then
                AlertaJS("Debe seleccionar una fecha de busqueda fin")
                Exit Sub
            End If
            fechainicio = tbFechaOperacion.Text
            fechafin = tbFechaOperacionFin.Text
            fechainicio = fechainicio.Substring(6, 4) & fechainicio.Substring(3, 2) & fechainicio.Substring(0, 2)
            fechafin = fechafin.Substring(6, 4) & fechafin.Substring(3, 2) & fechafin.Substring(0, 2)
            If (Convert.ToInt64(fechafin) < Convert.ToInt64(fechainicio)) Then
                AlertaJS("La fecha de busqueda fin debe ser mayor o igual a la fecha de busqueda inicio")
                Exit Sub
            End If
            If ddlEntidadExterna.SelectedValue.Trim <> "" Then
                Call CargaVectorPrecio()
                ViewState("vsConsulta") = 1
            Else
                AlertaJS(ObtenerMensaje("ALERT118"))
                ViewState("vsConsulta") = 0
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Try
            oUIUtil = Nothing
            oVectorPrecioBM = Nothing
            oVectorPrecioBE = Nothing
            oUtil = Nothing
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim sFechaOperacion As String = tbFechaOperacion.Text
            Dim sFechaOperacionFin As String = tbFechaOperacionFin.Text
            Dim sCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue.Trim
            Dim sCodigoIsin As String = txtISIN.Text.Trim
            Dim sCodigoMnemonico As String = txtMnemonico.Text
            Dim sEntidadEx As String = ddlEntidadExterna.SelectedValue.Trim
            sFechaOperacion = sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2)
            sFechaOperacionFin = sFechaOperacionFin.Substring(6, 4) & sFechaOperacionFin.Substring(3, 2) & sFechaOperacionFin.Substring(0, 2)
            Dim strurl As String = "Reportes/frmPrecios.aspx?nFechaOperacion=" & sFechaOperacion & "&nFechaOperacionFin=" & sFechaOperacionFin & "&sTipoInstrumento=" & sCodigoTipoInstrumentoSBS & "&sCodigoIsin=" & sCodigoIsin & "&sCodigoMnemonico=" & sCodigoMnemonico & "&sEntidad=" & sEntidadEx
            EjecutarJS("showModalDialog('" & strurl & "', '800', '600', '');")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgPrecios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPrecios.RowCommand
        Try
            Dim index As Int32 = 0
            If e.CommandName = "Seleccionar" Then
                index = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = dgPrecios.Rows(index)
                Dim var = dgPrecios.DataKeys(index).Values
                dgPrecios.SelectedIndex = index
                ViewState("vsCodigoMnemonico") = var(0).ToString()
                ViewState("vsFecha") = row.Cells(2).Text
                ViewState("vsEntidadExt") = var(1).ToString()
                txtCotizacionT.Text = row.Cells(3).Text
                txtFechaFin.Text = row.Cells(2).Text
                txtPrecioSucio.Text = row.Cells(4).Text
                txtPorcPrecioLimpio.Text = row.Cells(5).Text
                txtPorcPrecioSucio.Text = row.Cells(6).Text
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub dgPrecios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPrecios.PageIndexChanging
        Try
            dgPrecios.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmIngresoPrecios.aspx")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"
    Private Sub CargaVectorPrecio()
        dgPrecios.PageIndex = 0
        Dim sFechaOperacion As String = tbFechaOperacion.Text
        If Not (String.IsNullOrEmpty(tbFechaOperacion.Text)) Then
            sFechaOperacion = tbFechaOperacion.Text
            sFechaOperacion = sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2)
        Else
            sFechaOperacion = UIUtility.ObtenerFechaMaximaNegocio()
        End If
        Dim sFechaOperacionFin As String = tbFechaOperacionFin.Text

        If Not (String.IsNullOrEmpty(tbFechaOperacionFin.Text)) Then
            sFechaOperacionFin = tbFechaOperacionFin.Text
            sFechaOperacionFin = sFechaOperacionFin.Substring(6, 4) & sFechaOperacionFin.Substring(3, 2) & sFechaOperacionFin.Substring(0, 2)
        Else
            sFechaOperacionFin = UIUtility.ObtenerFechaMaximaNegocio()
        End If
        Dim sCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue.Trim
        Dim sCodigoIsin As String = txtISIN.Text.Trim
        Dim sCodigoMnemonico As String = txtMnemonico.Text
        Dim sEntidadExt As String = ddlEntidadExterna.SelectedValue
        Dim dtblDatos As DataTable
        dtblDatos = New VectorPrecioBM().ListarRango(sFechaOperacion, sFechaOperacionFin, sCodigoMnemonico, sCodigoIsin, sEntidadExt, DatosRequest).Tables(0)
        Session("dtDatosPrecios") = dtblDatos
        CargarGrilla()
        Call LimpiarControlesEdicion()
        Call HabilitaBotones(True, False, False, True, True)
        Call HabilitaControlesEdicion(False)
        ViewState("vsCodigoMnemonico") = ""
        ViewState("vsFecha") = ""
        ViewState("vsEntidadExt") = ""
    End Sub
    Private Sub CargarGrilla()
        Dim dtblDatos As DataTable = Session("dtDatosPrecios")
        dgPrecios.DataSource = dtblDatos
        dgPrecios.DataBind()
    End Sub
    Private Sub CargaEntidadExterna()
        Dim dtParametrosGenarales As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(ParametrosSIT.VECTOR_PRECIO_VALORIZACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEntidadExterna, dtParametrosGenarales, "Valor", "Nombre", True)
    End Sub
    Private Sub HabilitaBotones(ByVal pModificar As Boolean, ByVal pAceptar As Boolean, ByVal pCancelar As Boolean, ByVal pBloomberg As Boolean, ByVal pBuscar As Boolean)
        btnModificar.Visible = pModificar
        btnAceptar.Visible = pAceptar
        btnCancelar.Visible = pCancelar
        btnbuscar.Enabled = pBuscar
    End Sub
    Private Sub HabilitaControlesFiltro(ByVal pISIN As Boolean, ByVal pSBS As Boolean, ByVal pMnemonico As Boolean, _
                                        ByVal pDescripcion As Boolean, ByVal pFechaOperacion As Boolean, ByVal pmg1 As Boolean, _
                                        ByVal pBorrar As Boolean, ByVal pTipoInstrumento As Boolean, ByVal pEntidadExterna As Boolean)
        txtISIN.Enabled = pISIN
        txtSBS.Enabled = pSBS
        txtMnemonico.Enabled = pMnemonico
        txtDescripcion.Enabled = pDescripcion
        tbFechaOperacion.Enabled = pFechaOperacion
        ddlTipoInstrumento.Enabled = pTipoInstrumento
        ddlEntidadExterna.Enabled = pEntidadExterna
    End Sub
    Private Sub HabilitaControlesEdicion(ByVal pCotizacionT As Boolean)
        txtCotizacionT.ReadOnly = Not pCotizacionT
        txtPrecioSucio.ReadOnly = Not pCotizacionT
        txtPorcPrecioLimpio.ReadOnly = Not pCotizacionT
        txtPorcPrecioSucio.ReadOnly = Not pCotizacionT
    End Sub
    Private Function ValidarPrecioSBS() As Boolean
        Dim sCotizacion As String = txtCotizacionT.Text.Trim
        If txtCotizacionT.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT73"))
            Return False
            Exit Function
        ElseIf txtCotizacionT.Text.Trim < 0 Then
            AlertaJS(ObtenerMensaje("ALERT74"))
            Return False
            Exit Function
        ElseIf CDec(sCotizacion) > 999999999999 Then
            AlertaJS(ObtenerMensaje("ALERT78"))
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Function ModificarPrecioSBS() As Boolean
        If ValidarPrecioSBS() Then
            oVectorPrecioBE = CrearObjetoPrecioSBS(ViewState("vsCodigoMnemonico"), UIUtility.ConvertirFechaaDecimal(ViewState("vsFecha")), _
            ddlEntidadExterna.SelectedValue, System.Convert.ToDecimal(txtCotizacionT.Text.Trim), Decimal.Parse(txtPrecioSucio.Text) _
            , Decimal.Parse(txtPorcPrecioLimpio.Text), Decimal.Parse(txtPorcPrecioSucio.Text))
            'OT 10238 - 18/04/2017 - Carlos Espejo
            'Descripcion: Se agrega el precio sucio
            Return oVectorPrecioBM.Modificar(oVectorPrecioBE, DatosRequest)
            'OT 10238 Fin
        Else
            Return False
        End If
    End Function
    Private Function CrearObjetoPrecioSBS(ByVal CodigoMnemonico As String, ByVal Fecha As Decimal, ByVal EntidadExt As String, ByVal Valor As Decimal _
                                          , ByVal PrecioSucio As Decimal, ByVal PorcPrecioLimpio As Decimal, ByVal PorcPrecioSucio As Decimal) As VectorPrecioBE
        Dim oVectorPrecioBE As New VectorPrecioBE
        Dim oRow As VectorPrecioBE.VectorPrecioRow
        oRow = CType(oVectorPrecioBE.VectorPrecio.NewRow(), VectorPrecioBE.VectorPrecioRow)
        oRow.CodigoMnemonico() = CodigoMnemonico
        oRow.Fecha() = Fecha
        oRow.EntidadExt() = EntidadExt
        oRow.Valor() = Valor
        oRow.Manual = "S"
        oRow.PrecioLimpio = Valor
        oRow.PrecioSucio = PrecioSucio
        oRow.PorcPrecioLimpio = PorcPrecioLimpio
        oRow.PorcPrecioSucio = PorcPrecioSucio
        oVectorPrecioBE.VectorPrecio.AddVectorPrecioRow(oRow)
        oVectorPrecioBE.VectorPrecio.AcceptChanges()
        Return oVectorPrecioBE
    End Function
    Private Sub LimpiarControlesEdicion()
        txtCotizacionT.Text = String.Empty
        txtFechaFin.Text = String.Empty
        txtPrecioSucio.Text = String.Empty
        txtPorcPrecioLimpio.Text = String.Empty
        txtPorcPrecioSucio.Text = String.Empty
    End Sub
    Private Sub DescargaGrillaLista()
        dgPrecios.DataSource = UIUtility.GetStructureTablebase(campos)
        dgPrecios.DataBind()
    End Sub
    Public Sub SeleccionarPrecio(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String
        Dim strCodigoMnemonico As String
        Dim strFecha As String
        Dim strEntidadExt As String
        strcadena = e.CommandArgument
        strCodigoMnemonico = strcadena.Split(",").GetValue(0)
        strFecha = strcadena.Split(",").GetValue(1)
        strEntidadExt = strcadena.Split(",").GetValue(2)
        ViewState("vsCodigoMnemonico") = strCodigoMnemonico
        ViewState("vsFecha") = strFecha.Substring(6, 4) & strFecha.Substring(3, 2) & strFecha.Substring(0, 2)
        ViewState("vsEntidadExt") = strEntidadExt
        Me.txtFechaFin.Text = strcadena.Split(",").GetValue(1)
    End Sub
    Private Sub CargaTipoInstrumento()
        ddlTipoInstrumento.DataSource = New TipoInstrumentoBM().Listar(DatosRequest)
        ddlTipoInstrumento.DataTextField = "Descripcion"
        ddlTipoInstrumento.DataValueField = "CodigoTipoInstrumentoSBS"
        ddlTipoInstrumento.DataBind()
        UIUtility.InsertarElementoSeleccion(ddlTipoInstrumento)
    End Sub
    Private Function GetISIN() As String
        Return txtISIN.Text
    End Function
    Private Function GetSBS() As String
        Return txtSBS.Text
    End Function
    Private Function GetMNEMONICO() As String
        Return txtMnemonico.Text
    End Function
    Public Sub SeleccionarSBS(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String
        Dim strcodSBS As String
        strcadena = e.CommandArgument
        strcodSBS = strcadena.Split(",").GetValue(0)
        ViewState("vscodSBS") = strcodSBS
    End Sub
#End Region

End Class