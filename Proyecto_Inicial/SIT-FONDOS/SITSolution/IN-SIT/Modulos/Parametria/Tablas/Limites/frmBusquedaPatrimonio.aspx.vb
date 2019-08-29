Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Limites_frmBusquedaPatrimonio
    Inherits BasePage

    Private Const CLASIFICACION As String = "PATRI"
    Private Const VWS_PATRIMONIO As String = "patrimonio"
    Private Const VWS_PATRIMONIODETALLE As String = "patrimonioDetalle"


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                cargarCombos()
                cargarCabeceraPatrimonioLimite()
                validarBotones()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub validarBotones()
        Me.btnModificarCabecera.Visible = False
        Me.btnCancelarCabecera.Visible = False
        Me.btnModificarDetalle.Visible = False
        Me.btnCancelarDetalle.Visible = False
    End Sub

    Function SelectDataTable(ByVal dt As DataTable, ByVal filter As String, Optional ByVal sort As String = "") As DataTable
        Dim rows As DataRow()
        Dim dtNew As DataTable
        dtNew = dt.Clone()
        If sort.Length = 0 Then
            rows = dt.Select(filter)
        Else
            rows = dt.Select(filter, sort)
        End If
        For Each dr As DataRow In rows
            dtNew.ImportRow(dr)
        Next
        Return dtNew
    End Function

    Private Sub ibCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub cargarCombos()
        ddlFondo.DataSource = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondo.DataTextField = "Descripcion"
        ddlFondo.DataValueField = "CodigoPortafolio"
        ddlFondo.DataBind()
    End Sub

#Region "Patrimonio Cabecera"

    Private Sub cargarCabeceraPatrimonioLimite()
        Dim obm As New ParametrosGeneralesBM
        ViewState(VWS_PATRIMONIO) = obm.Listar(CLASIFICACION, Me.DatosRequest)
        dgCabecera.DataSource = CType(ViewState(VWS_PATRIMONIO), DataTable)
        dgCabecera.DataBind()
    End Sub

    Private Sub btnBuscarCabecera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarCabecera.Click
        Try
            Dim filtro As New StringBuilder
            If (tbDescripcion.Text.Trim.Length > 0) Then
                filtro.Append(String.Format("nombre like '{0}%' ", tbDescripcion.Text))
            End If
            Dim dt As DataTable = SelectDataTable(CType(ViewState(VWS_PATRIMONIO), DataTable), filtro.ToString)
            dgCabecera.DataSource = dt
            dgCabecera.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error de la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresarCabecera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresarCabecera.Click
        Try
            Dim dt As DataTable = SelectDataTable(CType(ViewState(VWS_PATRIMONIO), DataTable), "1=1", "valor asc")
            Dim valor As String
            If (dt.Rows.Count > 0) Then
                valor = dt.Rows(dt.Rows.Count - 1)("valor")
                valor = 1 + CInt(valor)
            Else
                valor = "1"
            End If
            Dim obm As New ParametrosGeneralesBM
            obm.Insertar(CLASIFICACION, tbDescripcion.Text.ToUpper, valor, "", Me.DatosRequest)
            LimpiarCabecera()
            LimpiarDetalle()
            cargarCabeceraPatrimonioLimite()
            dgCabecera.SelectedIndex = -1
            HabilitarDesabilitaDetalle(False)
            hdCodigoCabecera.Value = ""
            cargarDetallePatrimonioLimite("")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try
    End Sub

    Private Sub btnModificarCabecera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificarCabecera.Click
        Try
            Dim oPatrimonioValorBM As New PatrimonioValorBM
            oPatrimonioValorBM.ModificarIncrementoDecremento("PATRI", tbDescripcion.Text, ViewState("CodigoCabecera"), "")

            LimpiarCabecera()
            cargarCabeceraPatrimonioLimite()
            dgCabecera.SelectedIndex = -1
            HabilitarDesabilitaDetalle(False)
            hdCodigoCabecera.Value = ""
            cargarDetallePatrimonioLimite("")
            btnIngresarCabecera.Visible = True
            btnModificarCabecera.Visible = False
            btnCancelarCabecera.Visible = False
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar")
        End Try
        
    End Sub

    Private Sub btnCancelarCabecera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelarCabecera.Click
        Try
            LimpiarCabecera()
            btnIngresarCabecera.Visible = True
            btnModificarCabecera.Visible = False
            btnCancelarCabecera.Visible = False
            dgCabecera.SelectedIndex = -1
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cancelar")
        End Try
    End Sub

    Private Sub LimpiarCabecera()
        tbDescripcion.Text = ""
    End Sub

#End Region

#Region "Patrimonio Detalle"

    Private Sub HabilitarDesabilitaDetalle(ByVal resultado As Boolean)
        ddlFondo.Enabled = resultado
        txtValor.Enabled = resultado
        ddlTipoIngresoD.Enabled = resultado
    End Sub

    Private Sub LimpiarDetalle()
        ddlFondo.SelectedIndex = 0
        txtValor.Text = ""
        ddlTipoIngresoD.SelectedIndex = 0
    End Sub

    Private Sub btnIngresarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresarDetalle.Click
        Try
            Dim oLimiteBM As New LimiteBM
            If oLimiteBM.ValidarIncDecPatrimonio(Me.hdCodigoCabecera.Value, Me.ddlFondo.SelectedValue) Then
                AlertaJS("Ya existe un valor para el " & ddlFondo.SelectedValue)
                Exit Sub
            End If
            oLimiteBM.InsertarIncDecPatrimonio(hdCodigoCabecera.Value, ddlFondo.SelectedValue, CDec(txtValor.Text), ddlTipoIngresoD.SelectedValue)
            cargarDetallePatrimonioLimite(Me.hdCodigoCabecera.Value)
            LimpiarDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar Detalle")
        End Try
    End Sub

    Private Sub cargarDetallePatrimonioLimite(ByVal codigocategoriaincdec As String)
        Dim oLimiteBM As New LimiteBM
        ViewState(VWS_PATRIMONIODETALLE) = oLimiteBM.SeleccionarIncDecPatrimonio(codigocategoriaincdec)
        upCabecera.Update()
        dgDetalle.DataSource = CType(ViewState(VWS_PATRIMONIODETALLE), DataTable)
        dgDetalle.DataBind()
    End Sub

    Private Sub btnModificarDetalle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificarDetalle.Click
        Try
            Dim oLimiteBM As New LimiteBM
            oLimiteBM.ModificarIncDecPatrimonio(hdCodigoDetalle.Value, ddlFondo.SelectedValue, CDec(txtValor.Text), ddlTipoIngresoD.SelectedValue)
            hdCodigoDetalle.Value = ""
            dgDetalle.SelectedIndex = -1
            LimpiarDetalle()
            cargarDetallePatrimonioLimite(hdCodigoCabecera.Value)
            btnModificarDetalle.Visible = False
            btnCancelarDetalle.Visible = False
            btnIngresarDetalle.Visible = True
            ddlFondo.Enabled = True
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar Detalle")
        End Try
    End Sub

    Private Sub btnCancelarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelarDetalle.Click
        Try
            hdCodigoDetalle.Value = ""
            dgDetalle.SelectedIndex = -1
            LimpiarDetalle()
            btnModificarDetalle.Visible = False
            btnCancelarDetalle.Visible = False
            btnIngresarDetalle.Visible = True
            ddlFondo.Enabled = True
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cancelar Detalle")
        End Try        
    End Sub

#End Region

    Protected Sub dgCabecera_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCabecera.RowCommand
        Try
            Dim index As Integer = 0
            Dim codigocategoriaincdec As String

            Select Case e.CommandName
                Case "Modificar"
                    index = CInt(e.CommandArgument)
                    tbDescripcion.Text = dgCabecera.Rows(index).Cells(5).Text
                    ViewState("CodigoCabecera") = dgCabecera.DataKeys(index)("valor")
                    btnIngresarCabecera.Visible = False
                    btnModificarCabecera.Visible = True
                    btnCancelarCabecera.Visible = True
                    LimpiarDetalle()
                    HabilitarDesabilitaDetalle(False)
                    cargarDetallePatrimonioLimite("")
                    dgCabecera.SelectedIndex = index
                    hdCodigoCabecera.Value = ""
                Case "Eliminar"
                    index = CInt(e.CommandArgument)
                    codigocategoriaincdec = dgCabecera.DataKeys(index)("valor")
                    Dim oPatrimonioBM As New PatrimonioValorBM
                    oPatrimonioBM.EliminarIncrementoDecremento(CLASIFICACION, codigocategoriaincdec)
                    cargarCabeceraPatrimonioLimite()
                    cargarDetallePatrimonioLimite("")
                    dgCabecera.SelectedIndex = -1
                    LimpiarDetalle()
                    HabilitarDesabilitaDetalle(False)
                    hdCodigoCabecera.Value = ""
                Case "Select"
                    index = CInt(e.CommandArgument)
                    codigocategoriaincdec = dgCabecera.DataKeys(index)("valor")
                    hdCodigoCabecera.Value = codigocategoriaincdec
                    cargarDetallePatrimonioLimite(codigocategoriaincdec)
                    HabilitarDesabilitaDetalle(True)
                    btnIngresarCabecera.Visible = True
                    btnModificarCabecera.Visible = False
                    btnCancelarCabecera.Visible = False
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub

    Protected Sub dgCabecera_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgCabecera.RowDataBound
        Try
            Dim btn As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                btn = CType(e.Row.Cells(0).FindControl("ibEliminar"), ImageButton)
                btn.Attributes.Add("onclick", "return confirm_delete();")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub

    Protected Sub dgDetalle_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDetalle.RowCommand
        Try
            Dim index As Integer
            Select Case e.CommandName
                Case "Modificar"
                    index = CInt(e.CommandArgument)
                    Dim dt As DataTable = dgDetalle.DataSource()
                    hdCodigoDetalle.Value = dgDetalle.DataKeys(index)("CodigoIncDec")
                    ddlFondo.SelectedValue = dgDetalle.DataKeys(index)("CodigoPortafolioSBS").ToString.Trim 'dgDetalle.Rows.Item(index).Cells(4).Text
                    ddlFondo.Enabled = False
                    txtValor.Text = dgDetalle.Rows.Item(index).Cells(6).Text
                    ddlTipoIngresoD.SelectedValue = dgDetalle.Rows.Item(index).Cells(7).Text
                    dgDetalle.SelectedIndex = index
                    btnModificarDetalle.Visible = True
                    btnCancelarDetalle.Visible = True
                    btnIngresarDetalle.Visible = False
                Case "Eliminar"
                    index = CInt(e.CommandArgument)
                    Dim oLimiteBM As New LimiteBM
                    oLimiteBM.EliminarIncDecPatrimonio(dgDetalle.DataKeys(index)("CodigoIncDec"))
                    cargarDetallePatrimonioLimite(hdCodigoCabecera.Value)
                    hdCodigoDetalle.Value = ""
                    LimpiarDetalle()
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub

    Protected Sub dgDetalle_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDetalle.RowDataBound
        Try
            Dim btn As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                btn = CType(e.Row.FindControl("ibEliminarDetalle"), ImageButton)
                btn.Attributes.Add("onclick", "return confirm_delete();")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
End Class
