Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data

Public Class Modulos_PrevisionPagos_frmParametriaDetalle
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            Agregar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub dgDetalle_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

        Try
            If e.CommandName = "Modificar" Or e.CommandName = "Eliminar" Then
                Dim indiceFila As String = e.CommandArgument

                Dim pkFila As String = dgDetalle.DataKeys(indiceFila)("Valor")
                If pkFila.Length > 0 Then

                    Dim dtTabla As Data.DataTable = CType(ViewState("Detalle"), Data.DataTable) ' tabla en memoria
                    Dim result As DataRow() = dtTabla.Select("Valor = '" & pkFila & "'") ' Búsqueda por Primary Key

                    If result.Length > 0 Then

                        If e.CommandName = "Modificar" Then
                            tbDescripcion.Text = result(0)("Descripcion")
                            tbValor.Text = result(0)("Valor")
                            btnAgregar.Text = "Modificar"
                            Session("filaSelec") = result(0)

                            For intNum As Integer = 0 To dgDetalle.Rows.Count - 1
                                If intNum Mod 2 = 0 Then
                                    dgDetalle.Rows(intNum).BackColor = Drawing.Color.WhiteSmoke
                                Else
                                    dgDetalle.Rows(intNum).BackColor = Drawing.Color.White
                                End If
                            Next
                            dgDetalle.Rows(indiceFila).BackColor = Drawing.Color.LemonChiffon
                        End If

                        If e.CommandName = "Eliminar" Then
                            dtTabla.Rows.Remove(result(0))
                            ViewState("Detalle") = dtTabla ' Actualizamos la tabla en memoria

                            dgDetalle.DataSource = dtTabla
                            dgDetalle.DataBind()

                            LimpiarDet()
                            btnAgregar.Text = "Agregar"
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub dgDetalle_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            Dim strFila As String = e.Row.RowIndex.ToString()
            Dim ibModificar As ImageButton
            Dim ibEliminar As ImageButton

            If e.Row.RowType = DataControlRowType.DataRow Then
                ibModificar = CType(e.Row.FindControl("ibModificar"), ImageButton)
                ibModificar.CommandArgument = strFila

                ibEliminar = CType(e.Row.FindControl("ibEliminar"), ImageButton)
                ibEliminar.CommandArgument = strFila
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Protected Sub bSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bSalir.Click
        Try
            Response.Redirect("frmParametriaGeneral.aspx", False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

#Region "/* Funciones Personalizadas */"
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            hdCodigo.Value = Request.QueryString("cod").ToString()
            lblCabeceraParam.Text = Request.QueryString("desc").ToString()
            CargarRegistro()
        End If
    End Sub

    Private Function ValidarDetalle() As Boolean
        Dim dtTabla As New Data.DataTable
        dtTabla = CType(ViewState("Detalle"), Data.DataTable)
        Dim intFila As Integer


        For intFila = 0 To dtTabla.Rows.Count - 1
            Dim fila As String = dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("Descripcion")).ToString.ToUpper
            Dim txt As String = tbDescripcion.Text.Trim.ToUpper
            Dim fila2 As String = dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("Valor")).ToString.ToUpper
            Dim txt2 As String = tbValor.Text.Trim.ToUpper

            If (dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("Descripcion")).ToString.ToUpper = tbDescripcion.Text.Trim.ToUpper And _
            dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("Valor")).ToString.ToUpper = tbValor.Text.Trim.ToUpper) Then
                'Or _
                '(dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("Descripcion")) = tbDescripcion.Text.Trim.ToUpper) Or _
                'dtTabla.Rows(intFila).Item(dtTabla.Columns.IndexOf("Valor")) = tbValor.Text.Trim.ToUpper 
                AlertaJS("El registro ya se encuentra en el detalle")
                Return False
            End If
        Next
        Return True
    End Function
    Private Function Validar() As Boolean
        Dim dtTabla As New Data.DataTable
        dtTabla = New PrevisionParametroBM().SeleccionarPorCodigo(tbDescripcion.Text.Trim).Tables(0)
        If dtTabla.Rows.Count > 0 Then
            AlertaJS("El registro ya se encuentra en el detalle")
            Return False
        End If
        Return True
    End Function
    Private Sub Aceptar()
        Dim dtTabla As Data.DataTable = CType(ViewState("Detalle"), Data.DataTable)
        Dim strCodigo As String = Me.hdCodigo.Value
        Dim oParametroBM As New PrevisionParametroBM
        Dim i As Integer
        Dim IdParametro As String = "0"

        'ActualizaArchivos(dtTabla)

        If strCodigo <> String.Empty Then
            oParametroBM.Eliminar(strCodigo)
            For i = 0 To dtTabla.Rows.Count - 1
                oParametroBM.Insertar(Convert.ToInt32(strCodigo), dtTabla.Rows(i).Item(dtTabla.Columns.IndexOf("Descripcion")), dtTabla.Rows(i).Item(dtTabla.Columns.IndexOf("Valor")))
            Next
            AlertaJS("Se modificó satisfactoriamente")
            btnAgregar.Text = "Agregar"
            LimpiarDet()
        End If
    End Sub
    Private Sub InicializarTabla()
        Dim dtTabla As New Data.DataTable
        dtTabla.Columns.Add("Descripcion", GetType(String))
        dtTabla.Columns.Add("Valor", GetType(String))

        ViewState("Detalle") = dtTabla
    End Sub
    Private Sub InsertarFila()
        Dim dtTabla As New Data.DataTable
        dtTabla = CType(ViewState("Detalle"), Data.DataTable)
        Dim fila As Data.DataRow
        fila = dtTabla.NewRow()
        fila("Descripcion") = tbDescripcion.Text.ToString.ToUpper
        fila("Valor") = tbValor.Text.ToString.ToUpper

        dtTabla.Rows.Add(fila)
        ViewState("Detalle") = dtTabla
        dgDetalle.DataSource = dtTabla
        dgDetalle.DataBind()
    End Sub
    Private Sub CargarRegistro()
        CargarGrilla()
        CargarParametro()
    End Sub
    Private Sub Agregar()
        If btnAgregar.Text = "Modificar" Then
            hdValor.Value = tbValor.Text

            ModificarFila()
        Else
            If ValidarDetalle() = False Then
                Exit Sub
            End If
            InsertarFila()
            LimpiarDet()
        End If
    End Sub

    Public Sub ModificarFila()
        If Session("filaSelec") IsNot Nothing Then
            Dim pkFila As DataRow = Session("filaSelec")
            Dim dtTabla As Data.DataTable = CType(ViewState("Detalle"), Data.DataTable)

            pkFila("Descripcion") = tbDescripcion.Text.Trim.ToUpper
            pkFila("Valor") = hdValor.Value
            hdValor.Value = ""
            btnAgregar.Text = "Agregar"
            Me.tbValor.Enabled = True

            Session("filaSelec") = Nothing
            ViewState("Detalle") = pkFila.Table

            dgDetalle.DataSource = pkFila.Table
            dgDetalle.DataBind()

            LimpiarDet()
        End If
    End Sub

    'Public Sub EliminarFila(pkFila As String)
    '    Dim dtTabla As Data.DataTable = CType(ViewState("Detalle"), Data.DataTable) ' tabla en memoria
    '    Dim result As DataRow() = dtTabla.Select("Valor = '" & pkFila & "'") ' Búsqueda por Primary Key

    '    If result.Length > 0 Then
    '        dtTabla.Rows.Remove(result(0))

    '        dgDetalle.DataSource = dtTabla
    '        dgDetalle.DataBind()
    '    End If

    '    ViewState("Detalle") = dtTabla ' Actualizamos la tabla en memoria
    'End Sub

    Private Sub LimpiarCab()
        tbDescripcion.Text = ""
    End Sub
    Private Sub LimpiarDet()
        tbDescripcion.Text = ""
        tbValor.Text = ""
    End Sub
    Private Sub CargarCombos()
        'UtilDM.LlenarComboBox(ddlSituacion, New ParametroBM().SeleccionarObjeto(Statics.Constantes.ParamSituacion).Tables(0), "Valor", "Descripcion", False)
    End Sub
    Private Sub CargarParametro()
        Dim titulo As String = Request.QueryString("desc").ToString()
        'lbTitulo.Text = "Mantenimiento de Parámetros Generales - " + titulo
    End Sub

    Private Sub CargarGrilla()
        If Request.QueryString("cod").ToString <> String.Empty Then
            Dim dtTabla As Data.DataTable
            dtTabla = New PrevisionParametroBM().SeleccionarPorCodigo(hdCodigo.Value).Tables(0)
            EjecutarJS("$('#" + lbContador.ClientID + "').text('Registros Encontrados : " + dtTabla.Rows.Count.ToString + "')")

            ViewState("Detalle") = dtTabla
            dgDetalle.DataSource = dtTabla
            dgDetalle.DataBind()
        End If
    End Sub

    Private Sub cargarGrillaAgregar()
        Dim dtTabla As Data.DataTable
        dtTabla = ViewState("Detalle")
        EjecutarJS("$('#" + lbContador.ClientID + "').text('Registros Encontrados : " + dtTabla.Rows.Count.ToString + "')")

        ViewState("Detalle") = dtTabla
        dgDetalle.DataSource = dtTabla
        dgDetalle.DataBind()
    End Sub

    Private Sub Alerta(mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), New Guid().ToString(), "<script language='javascript'>alert('" & mensaje & "');</script>", False)
    End Sub

#End Region

    Protected Sub dgDetalle_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDetalle.PageIndexChanging
        If ViewState("Detalle") Is Nothing Then
            dgDetalle.PageIndex = e.NewPageIndex
            CargarGrilla()
        Else
            'Dim dt As New DataTable
            'dt = Session("Tabla")
            dgDetalle.PageIndex = e.NewPageIndex
            cargarGrillaAgregar()
        End If
        
    End Sub
End Class
