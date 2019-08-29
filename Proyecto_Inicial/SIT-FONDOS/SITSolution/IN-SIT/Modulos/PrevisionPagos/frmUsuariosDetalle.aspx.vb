Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Text
Imports System.Data

Public Class Modulos_PrevisionPagos_frmUsuariosDetalle
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                CargarTipoOperacion()
                If Not Request.QueryString("cod") Is Nothing Then
                    Dim objBM As New PrevisionUsuarioBM
                    Dim ds As DataSet
                    ds = objBM.ListarOperacionesUsuario(Request.QueryString("cod"))

                    lbCodigoUsuario.Text = ds.Tables(0).Rows(0)("CodUsuario").ToString()
                    tbUsuario.Text = ds.Tables(0).Rows(0)("NombreUsuario").ToString()
                    ddlEstado.SelectedValue = ds.Tables(0).Rows(0)("Situacion").ToString()
                    tbArea.Text = ds.Tables(0).Rows(0)("Area").ToString()
                    'ibBuscar.Enabled = False
                    Me.lkbBuscar.Enabled = False

                    _CodigoUsuario.Value = ds.Tables(0).Rows(0)("CodUsuario").ToString()
                    _NomUsuario.Value = ds.Tables(0).Rows(0)("NombreUsuario").ToString()

                    ViewState("gvDetalleUsuarios") = CType(ds.Tables(1), DataTable)


                    gvPagos.DataSource = CType(ds.Tables(1), DataTable)
                    gvPagos.DataBind()
                End If

                If Not Request.QueryString("user") Is Nothing Then
                    ViewState("user") = Request.QueryString("user")
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSalir.Click
        ViewState.Remove("gvDetalleUsuarios")
        Response.Redirect("frmUsuarios.aspx")
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click
        If ddlTipoOperacion.SelectedItem.Text = "--Seleccione--" Then
            AlertaJS("- Seleccione un tipo de operación a agregar")
        Else
            CargarGrilla()
        End If
        tbUsuario.Text = _NomUsuario.Value
        lbCodigoUsuario.Text = _CodigoUsuario.Value
    End Sub

    Private Sub CargarTipoOperacion()
        Dim objBM As New PrevisionParametroBM()
        Dim ds As New DataSet()
        ds = objBM.SeleccionarPorCodigo("4")
        HelpCombo.LlenarComboBox(ddlTipoOperacion, ds.Tables(0), "Valor", "descripcion", True)
    End Sub

    Private Sub gvPagos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPagos.RowCommand
        Select Case e.CommandName
            Case "Eliminar"
                EliminarRegistro(CInt(e.CommandArgument.ToString()))
        End Select
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim objBM As New PrevisionUsuarioBM()
        Dim objUsuarioBE As New PrevisionUsuario
        Dim objDetalleBE As New PrevisionDetalleUsuario
        Dim result As Integer

        objUsuarioBE.CodUsuario = _CodigoUsuario.Value
        objUsuarioBE.Area = tbArea.Text
        objUsuarioBE.NombreUsuario = _NomUsuario.Value
        objUsuarioBE.Situacion = ddlEstado.SelectedValue
        objUsuarioBE.UsuarioCreacion = ViewState("user").ToString() 'Usuario
        Try
            If Request.QueryString("cod") Is Nothing Then
                result = objBM.RegistrarUsuario(objUsuarioBE)
            Else
                result = objBM.ActualizarUsuario(objUsuarioBE)
            End If
            If result = 1 Then
                AlertaJS("Datos registrados correctamente.")
                objBM.LimpiarUsuarioDetalle(objUsuarioBE)
                Dim dt As DataTable
                dt = ViewState("gvDetalleUsuarios")
                For i = 0 To dt.Rows.Count - 1
                    objDetalleBE = New PrevisionDetalleUsuario()
                    objDetalleBE.CodUsuario = _CodigoUsuario.Value
                    objDetalleBE.IdTipoOperacion = dt.Rows(i)("IdTipoOperacion").ToString.Trim
                    objDetalleBE.Situacion = dt.Rows(i)("Situacion").ToString.Trim.Substring(0, 1)
                    objDetalleBE.UsuarioCreacion = ViewState("user").ToString() 'Usuario
                    objBM.RegistrarUsuarioDetalle(objDetalleBE)
                Next
                AlertaJS("Datos registrados correctamente.")
            Else
                AlertaJS("Por favor verifique la Información Ingresada.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Private Sub gvPagos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPagos.RowDataBound
        Try
            Dim strfila As String = e.Row.RowIndex.ToString()
            Dim ibeliminar As ImageButton

            If e.Row.RowType = DataControlRowType.DataRow Then

                ibeliminar = CType(e.Row.FindControl("ibeliminar"), ImageButton)
                ibeliminar.CommandArgument = strfila
            End If
        Catch ex As Exception
            AlertaJS("ocurrió un error en el sistema.")
        End Try
    End Sub

    Private Sub EliminarRegistro(ByVal indice As Integer)
        Dim dt As New DataTable()
        If Not ViewState("gvDetalleUsuarios") Is Nothing Then
            dt = CType(ViewState("gvDetalleUsuarios"), DataTable)
            ViewState.Remove("gvDetalleUsuarios")
        End If
        dt.Rows.RemoveAt(indice)
        ViewState("gvDetalleUsuarios") = dt

        gvPagos.DataSource = dt
        gvPagos.DataBind()
    End Sub

    Private Sub CargarGrilla()
        Dim dt As New DataTable()
        Dim dr As DataRow
        Dim dts As DataColumn()

        dt.Columns.Add("IdTipoOperacion")
        dt.Columns.Add("TipoOperacion")
        dt.Columns.Add("Situacion")

        Dim tipoOperacion As String
        tipoOperacion = ddlTipoOperacion.SelectedValue.ToString()

        If Not ViewState("gvDetalleUsuarios") Is Nothing Then
            dt = CType(ViewState("gvDetalleUsuarios"), DataTable)
            For Each row As DataRow In dt.Rows
                If row("IdTipoOperacion").ToString().Trim() = tipoOperacion.Trim() Then
                    AlertaJS("Tipo de operación repetido")
                    Exit Sub
                End If
            Next
            ViewState.Remove("gvDetalleUsuarios")
        End If

        dr = dt.NewRow()
        dr("IdTipoOperacion") = tipoOperacion
        dr("TipoOperacion") = ddlTipoOperacion.SelectedItem.Text.ToString()
        dr("Situacion") = ddlSituacion.SelectedItem.Text.ToString()
        dt.Rows.Add(dr)
        dt.AcceptChanges()

        ViewState("gvDetalleUsuarios") = dt
        gvPagos.DataSource = dt
        gvPagos.DataBind()
    End Sub

    Private Sub CargarPaginado()
        gvPagos.DataSource = ViewState("gvDetalleUsuarios")
        gvPagos.DataBind()
    End Sub

    'Protected Sub ibBuscar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibBuscar.Click
    '    Try
    '        BuscarPersonal()
    '    Catch ex As Exception
    '        AlertaJS("Ocurrió un error en el sistema.")
    '    End Try
    'End Sub

    Private Sub BuscarPersonal()
        Dim script As New StringBuilder
        With script
            .Append("<script>")
            .Append("function PopupBuscador()")
            .Append("{")
            .Append("hidden = open('frmPopupUsuario.aspx','NewWindow','top=0,left=0,width=995,height=500,status=no,resizable=no,scrollbars=no');")
            .Append("}")
            .Append("PopupBuscador();")
            .Append("</script>")
        End With

        ClientScript.RegisterStartupScript(GetType(String), New Guid().ToString, script.ToString())
    End Sub

    Protected Sub lkbBuscar_Click(sender As Object, e As System.EventArgs) Handles lkbBuscar.Click
        Try
            BuscarPersonal()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Private Sub Alerta(mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), New Guid().ToString(), "<script language='javascript'>alert('" & mensaje & "');</script>", False)
    End Sub

    Protected Sub gvPagos_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPagos.PageIndexChanging
        gvPagos.PageIndex = e.NewPageIndex
        CargarPaginado()
    End Sub
End Class

