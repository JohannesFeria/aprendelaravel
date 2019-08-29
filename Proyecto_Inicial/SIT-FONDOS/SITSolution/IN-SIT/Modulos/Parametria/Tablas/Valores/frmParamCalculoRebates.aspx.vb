Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmParamCalculoRebates
    Inherits BasePage

    Dim CodigoNemonicoReq As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
            CargarCombos()
            DeshabilitarDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            If ValidarDetalle() = False Then

            Else
                InsertarFila()
                LimpiarDetalle()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al agregar")
        End Try
    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Try
            ActualizarDetalleGrillaBase()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modifiar")
        End Try
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CodigoNemonicoReq = Request.QueryString("CodigoNemonico")
            btnModificar.Visible = False
            If CodigoNemonicoReq = "0" Then
                CargarEstructuraGrilla()
            Else
                Me.tbNemonico.Text = CodigoNemonicoReq
                CargarRegistro(CodigoNemonicoReq)
                CargarEstructuraGrilla()
            End If
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            tbNemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
            Session.Remove("SS_DatosModal")
        End If
    End Sub

    Private Sub CargarCombos()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dt As New DataTable
        Dim DatosRequest As DataSet = Nothing

        dt = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, dt, "Valor", "Nombre", False)

        HelpCombo.LlenarComboBox(Me.ddlSituacionDet, dt, "Valor", "Nombre", False)
    End Sub

    Private Sub Ingresar()

        Dim blnExisteGrupoLimite As String

        blnExisteGrupoLimite = ExisteRebate()
        CargarEstructuraGrilla()
        If blnExisteGrupoLimite = False Then
            DeshabilitarCabecera()
        Else
            HabilitarCabecera()
            AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
        End If

        If chkRango.Checked = True Then
            HabilitarDetalle()
        Else
            DeshabilitarDetalle()
            LimpiarDetalle()
        End If

    End Sub

    Private Function ExisteRebate() As Boolean
        Dim dt As DataTable
        dt = New DividendosRebatesLiberadasBM().ExisteRebate(Val(Me.hd.Value.Trim))
        Return dt.Rows.Count > 0
    End Function

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub

    Private Sub Aceptar()
        Dim blnExisteGrupoLimite As String
        If Me.hd.Value.Equals(String.Empty) Then
            blnExisteGrupoLimite = ExisteRebate()
            If blnExisteGrupoLimite = False Then
                Insertar()
                HabilitarCabecera()
                InsertarDetalleGrillaBase()
                Me.chkRango.Checked = False
                LimpiarDetalle()
                DeshabilitarDetalle()
                LimpiarCabecera()
                Dim dtTabla As Data.DataTable
                Dim oRebatesBM As New DividendosRebatesLiberadasBM
                dtTabla = oRebatesBM.ObtenerCodigoRebateDetalle("")
                ViewState("Detalle") = dtTabla
                dgLista.DataSource = Nothing
                dgLista.DataBind()
                btnAgregar.Visible = True
                btnModificar.Visible = False
            Else
            End If
        End If
    End Sub

    Private Sub Insertar()
        Dim oRebatesBM As New DividendosRebatesLiberadasBM
        Dim Usuario As String = "mayarzat"
        Dim dataRequest As DataSet = Nothing
        Dim Rango As String = ""
        If chkRango.Checked = True Then
            Rango = "D"
        Else
            Rango = "G"
        End If
        'JZAVALA  - OT - 67090 Rebates. - SE AGREGA EL PARAMETRO DE ENTRADA @p_SumatoriaFondos.
        oRebatesBM.InsertarRebate(Me.tbNemonico.Text, Me.tbDias.Text, Me.tbRebate.Text, Rango, "A", Usuario, dataRequest, IIf(chkSumatoriafondos.Checked, "S", "N"))
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
    End Sub

    Private Sub DeshabilitarCabecera()
        tbNemonico.Enabled = False
        tbDias.Enabled = False
        tbRebate.Enabled = False
        ddlSituacion.Enabled = False
    End Sub

    Private Sub HabilitarCabecera()
        tbNemonico.Enabled = True
        tbDias.Enabled = True
        tbRebate.Enabled = True
        ddlSituacion.Enabled = True
    End Sub

    Private Sub DeshabilitarDetalle()
        If chkRango.Checked = False Then
            tbImpInicial.Enabled = False
            tbImpFinal.Enabled = False
            tbPorRebate.Enabled = False
            ddlSituacionDet.Enabled = False
        End If
    End Sub

    Private Sub HabilitarDetalle()
        tbImpInicial.Enabled = True
        tbImpFinal.Enabled = True
        tbPorRebate.Enabled = True
        ddlSituacionDet.Enabled = True
    End Sub

    Private Sub LimpiarCabecera()
        tbNemonico.Text = ""
        tbDias.Text = ""
        tbRebate.Text = ""
        ddlSituacion.SelectedIndex = 0
    End Sub

    Private Sub LimpiarDetalle()
        tbImpInicial.Text = ""
        tbImpFinal.Text = ""
        tbPorRebate.Text = ""
        ddlSituacionDet.SelectedIndex = 0
    End Sub

    Private Function ValidarDetalle() As Boolean
        Dim dtTabla As New Data.DataTable
        dtTabla = CType(ViewState("Detalle"), Data.DataTable)
        Dim intFila As Integer

        For intFila = 0 To dtTabla.Rows.Count - 1
            If (dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("ImporteIni")) = tbImpInicial.Text.Trim And _
            dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("ImporteFin")) = tbImpFinal.Text.Trim) Then
                AlertaJS("Los valores ingresados ya se encuentran registrados")
                LimpiarDetalle()
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub InsertarFila()
        Dim dtTabla As New Data.DataTable
        dtTabla = CType(ViewState("Detalle"), Data.DataTable)
        Dim fila As Data.DataRow
        fila = dtTabla.NewRow()
        fila("ImporteIni") = tbImpInicial.Text.ToString
        fila("ImporteFin") = tbImpFinal.Text.ToString
        fila("PorcRebate") = tbPorRebate.Text.ToString
        fila("Situacion") = ddlSituacionDet.SelectedValue

        dtTabla.Rows.Add(fila)
        ViewState("Detalle") = dtTabla
        dgLista.DataSource = dtTabla
        dgLista.DataBind()
    End Sub

    Private Sub CargarEstructuraGrilla()
        Dim dtTabla As Data.DataTable
        Dim oRebatesBM As New DividendosRebatesLiberadasBM
        dtTabla = oRebatesBM.ObtenerCodigoRebateDetalle(Me.tbNemonico.Text)
        ViewState("Detalle") = dtTabla
        dgLista.DataSource = dtTabla
        dgLista.DataBind()
    End Sub

    Private Sub InsertarDetalleGrillaBase()
        Dim dtTabla As Data.DataTable
        dtTabla = CType(ViewState("Detalle"), Data.DataTable)
        Dim oRebatesBM As New DividendosRebatesLiberadasBM
        Dim Usuario As String = "mayarzat"
        Dim dataRequest As DataSet = Nothing

        Dim intFila As Integer
        For intFila = 0 To dtTabla.Rows.Count - 1
            Dim Valor As String = ""
            Try
                Valor = dtTabla.Rows(intFila)("CodigoCalculoRebateDet")
            Catch ex As Exception
                Valor = "0"
            End Try

            oRebatesBM.InsertarRebateDetalle(Me.tbNemonico.Text, CDec(dtTabla.Rows(intFila)("ImporteIni")), CDec(dtTabla.Rows(intFila)("ImporteFin")), CDec(dtTabla.Rows(intFila)("PorcRebate")), dtTabla.Rows(intFila)("Situacion"), Usuario, CInt(Valor), dataRequest)
        Next
    End Sub

    Private Sub ActualizarDetalleGrillaBase()
        Dim oRebatesBM As New DividendosRebatesLiberadasBM
        Dim Usuario As String = "mayarzat"
        Dim dataRequest As DataSet = Nothing
        oRebatesBM.InsertarRebateDetalle(Me.tbNemonico.Text, tbImpInicial.Text.Trim, tbImpFinal.Text.Trim, tbPorRebate.Text.Trim, ddlSituacionDet.SelectedValue.Trim, Usuario, IIf(Label1.Text.Trim = "0", 0, CInt(Label1.Text.Trim)), dataRequest)
        AlertaJS("Se modifico el registro correctamente")
        CargarEstructuraGrilla()
    End Sub

    Private Sub CargarRegistro(ByVal CodigoNemonico As String)
        Dim dtTabla As Data.DataTable
        Dim oRebatesBM As New DividendosRebatesLiberadasBM
        dtTabla = oRebatesBM.ObtenerCodigoRebateDetalleCabecera(CodigoNemonico)
        Me.tbDias.Text = dtTabla.Rows(0)("DiasCalculo")
        Me.tbRebate.Text = dtTabla.Rows(0)("PorcRebate")
        Me.ddlSituacion.SelectedValue = dtTabla.Rows(0)("Situacion")

        'JZAVALA 25/03/2013 - OT - 67090 Rebates. - SE ASIGNA EL VALOR DEL CAMPO CAMPO SumatoriaFondos AL CONTROL.
        Me.chkSumatoriafondos.Checked = IIf(dtTabla.Rows(0)("SumatoriaFondos").ToString() = "S", True, False)

        If dtTabla.Rows(0)("IndRango") = "G" Then
            Me.chkRango.Checked = False
        Else
            Me.chkRango.Checked = True
        End If
    End Sub

    Private Sub btnretornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedasParamCalculoRebates.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibnEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('Desea eliminar registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Modificar" Then
                Dim strDatos() As String = e.CommandArgument.ToString.Split(",")
                Me.tbImpInicial.Text = strDatos(0)
                Me.tbImpFinal.Text = strDatos(1)
                Me.tbPorRebate.Text = Convert.ToDecimal(strDatos(2))
                Me.ddlSituacionDet.SelectedValue = strDatos(3)
                Me.Label1.Text = strDatos(4)
                HabilitarDetalle()
                btnAgregar.Visible = False
                btnModificar.Visible = True
            End If
            If e.CommandName = "Eliminar" Then
                Dim strDatos() As String = e.CommandArgument.ToString.Split(",")
                Dim CodigoEliminar As String = strDatos(4)

                Dim oRebatesBM As New DividendosRebatesLiberadasBM
                Dim Usuario As String = "mayarzat"
                Dim dataRequest As DataSet = Nothing

                oRebatesBM.EliminarRebateDetalle(CInt(CodigoEliminar), dataRequest)
                Me.AlertaJS("Se Elimino el registro correctamente")

                CargarEstructuraGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarEstructuraGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

End Class
