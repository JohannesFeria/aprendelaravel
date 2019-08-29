Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmGrupoTipoRenta
    Inherits BasePage

#Region "/* Variables*/"
    Dim oGrupoInstrumentoBM As New GrupoInstrumentoBM
    '----------------------------------------------
    Private campos() As String = {"CodigoGrupoInstrumento", "CodigoCaracteristica", "Codigo", "Descripcion"}
    Private tipos() As String = {"System.String", "System.String", "System.String", "System.String"}
    '----------------------------------------------
    Private camposValores() As String = {"Codigo", "Descripcion"}
    Private tiposValores() As String = {"System.String", "System.String"}
    '----------------------------------------------
    Private camposCaracteristicaGrupo() As String = {"CodigoGrupoInstrumento", "CodigoCaracteristica", "ValorCaracteristica", "DescripcionValorCaracteristica", "ClaseNormativa", "Situacion", "Vista"}
    Private tiposCaracteristicaGrupo() As String = {"System.String", "System.String", "System.String", "System.String", "System.String", "System.String", "System.String"}
    '----------------------------------------------

    Dim dtCaracteristicaGrupoNivel As New DataTable

    Private oGrupoTipoRentaBE As GrupoTipoRentaBE
    Private operacion As String
    Private GrupoInstrumento As String

#End Region

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            operacion = Request.QueryString("ope")
            If (operacion = "mod") Then
                GrupoInstrumento = Request.QueryString("codGru")
            End If

            If Not Page.IsPostBack Then
                Me.CargarCombos()
                Me.cargarListBox()
                Me.hd.Value = ""
                If (operacion = "mod") Then
                    Me.cargarRegistro()
                Else
                    Session("dsDetalleGrupo") = Nothing
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnAgregarTodosCaracteristica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarTodosCaracteristica.Click
        Try
            Dim i As Integer = 0
            While (i < lbxValores.Items.Count)
                If (lbxSeleccionValores.Items.Contains(lbxValores.Items.Item(i)) = False) Then
                    lbxSeleccionValores.Items.Add(lbxValores.Items.Item(i))
                End If
                i = i + 1
            End While
            lbxValores.Items.Clear()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregar Todos")
        End Try        
    End Sub

    Private Sub btnAgregarCaracteristica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarCaracteristica.Click
        Try
            Dim row As DataRow
            Dim i As Integer
            i = 0

            If (lbxSeleccionValores.Items.Contains(lbxValores.SelectedItem) = False) And (lbxValores.SelectedValue <> "") Then
                lbxSeleccionValores.Items.Add(lbxValores.SelectedItem)
                lbxValores.Items.Remove(lbxValores.SelectedItem)
                lbxSeleccionValores.SelectedIndex = -1
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregar")
        End Try        
    End Sub

    Private Sub btnDevolverCaracteristica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDevolverCaracteristica.Click
        Try
            If (lbxValores.Items.Contains(lbxSeleccionValores.SelectedItem) = False) And (lbxSeleccionValores.SelectedValue <> "") Then
                lbxValores.Items.Add(lbxSeleccionValores.SelectedItem)
                lbxSeleccionValores.Items.Remove(lbxSeleccionValores.SelectedItem)
                lbxValores.SelectedIndex = -1
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Devolver")
        End Try        
    End Sub

    Private Sub btnDevolverTodosCaracteristica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDevolverTodosCaracteristica.Click
        Try
            Dim i As Integer
            i = 0
            While (i < lbxSeleccionValores.Items.Count)
                If (lbxValores.Items.Contains(lbxSeleccionValores.Items.Item(i)) = False) Then
                    lbxValores.Items.Add(lbxSeleccionValores.Items.Item(i))
                End If
                i = i + 1
            End While
            lbxSeleccionValores.Items.Clear()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Devolver Todos")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaGrupoTipoRenta.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            InsertarFila()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregar")
        End Try        
    End Sub

    Protected Sub dgListaTipoInstrumento_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgListaTipoInstrumento.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(0).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                If (e.Row.Cells(3).Text = "A") Then
                    e.Row.Cells(3).Text = "ACTIVO"
                ElseIf (e.Row.Cells(3).Text = "I") Then
                    e.Row.Cells(3).Text = "INACTIVO"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Protected Sub dgListaTipoInstrumento_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaTipoInstrumento.RowCommand
        Try
            Select Case e.CommandName
                Case "Eliminar"
                    Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                    Dim Index As Integer = Row.RowIndex
                    Me.agregarItemValores("" + e.CommandArgument)
                    oGrupoTipoRentaBE = Session("dsDetalleGrupo")
                    oGrupoTipoRentaBE.GrupoTipoRenta.Rows(Index).Delete()
                    oGrupoTipoRentaBE.AcceptChanges()
                    Me.dgListaTipoInstrumento.DataSource = oGrupoTipoRentaBE.GrupoTipoRenta
                    Me.dgListaTipoInstrumento.DataBind()
                    Session("dsDetalleGrupo") = oGrupoTipoRentaBE
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgListaTipoInstrumento_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaTipoInstrumento.PageIndexChanging
        Try
            dgListaTipoInstrumento.PageIndex = e.NewPageIndex
            oGrupoTipoRentaBE = Session("dsDetalleGrupo")
            Me.dgListaTipoInstrumento.DataSource = oGrupoTipoRentaBE.GrupoTipoRenta
            Me.dgListaTipoInstrumento.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click        
        Try
            Dim resultado As Boolean
            If Me.dgListaTipoInstrumento.Rows.Count <= 0 Then
                Me.AlertaJS("Debe seleccionar al menos un Tipo de Instrumento!!.")
                Exit Sub
            End If

            Me.oGrupoTipoRentaBE = Me.obtenerInstancia()
            If Me.operacion = "reg" Then
                If Not (exiteGrupo(Me.tbGrupoInstrumento.Text)) Then
                    resultado = New GrupoTipoRentaBM().Insertar(Me.oGrupoTipoRentaBE, Me.DatosRequest)
                    If (resultado) Then
                        Me.AlertaJS("Se registro el Grupo Renta satisfactoriamente.")
                        Me.limpiarCampos()
                    End If
                Else
                    Me.AlertaJS("El Codigo Grupo ingresado ya existe!!.")
                    Exit Sub
                End If
            Else
                resultado = New GrupoTipoRentaBM().Modificar(Me.oGrupoTipoRentaBE, Me.DatosRequest)
                Me.AlertaJS("Se modifico el Grupo Renta satisfactoriamente.")
            End If
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub limpiarListbox()
        lbxSeleccionValores.Items.Clear()
    End Sub

    Private Sub ActualizarValores()
        Dim i As Integer
        Dim j As Integer
        Dim dtAux As New DataTable
        Dim row As DataRow
        dtAux = UIUtility.GetStructureTablebase(camposValores, tiposValores)
        j = 0
        While (j < lbxValores.Items.Count)
            If (lbxSeleccionValores.Items.Contains(lbxValores.Items.Item(j)) = False) Then
                row = dtAux.NewRow
                row.Item("Codigo") = lbxValores.Items.Item(j).Value
                row.Item("Descripcion") = lbxValores.Items.Item(j).Text
                dtAux.Rows.Add(row)
            End If
            j = j + 1
        End While
        lbxValores.Items.Clear()
        HelpCombo.LlenarListBox(Me.lbxValores, dtAux, "Codigo", "Descripcion", False)
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)

    End Sub

    Private Function ExisteFila() As String
        Dim Msg As String = String.Empty
        Dim cont As Integer = 0
        Dim strGrupoInstrumento, strDescripcion, strCodigoTipoInstrumento As String

        For Each item As ListItem In lbxSeleccionValores.Items
            strCodigoTipoInstrumento = item.Value

            For cont = 0 To Me.oGrupoTipoRentaBE.GrupoTipoRenta.Rows.Count - 1
                If (Me.oGrupoTipoRentaBE.GrupoTipoRenta.Rows(cont).Item("CodigoTipoInstrumento") = strCodigoTipoInstrumento) Then
                    Msg = " - " + item.Text + ", ya esta seleccionado!!.\n"
                End If
            Next
        Next

        Return Msg
    End Function

    Private Sub InsertarFila()

        If Not Session("dsDetalleGrupo") Is Nothing Then
            Me.oGrupoTipoRentaBE = Session("dsDetalleGrupo")
        Else
            Me.oGrupoTipoRentaBE = New GrupoTipoRentaBE
        End If
        Dim Mensaje As String = String.Empty
        Mensaje = ExisteFila()
        If (Mensaje <> "") Then
            AlertaJS(Mensaje)
            Exit Sub
        End If

        Dim row As GrupoTipoRentaBE.GrupoTipoRentaRow

        For Each item As ListItem In lbxSeleccionValores.Items
            row = oGrupoTipoRentaBE.GrupoTipoRenta.NewRow
            row.Item("CodigoTipoInstrumento") = item.Value
            row.Item("DescripcionTipoInstrumento") = item.Text
            row.Item("Situacion") = "A"
            oGrupoTipoRentaBE.GrupoTipoRenta.Rows.Add(row)
        Next

        Session("dsDetalleGrupo") = oGrupoTipoRentaBE
        Me.dgListaTipoInstrumento.DataSource = oGrupoTipoRentaBE.GrupoTipoRenta
        Me.dgListaTipoInstrumento.DataBind()
        limpiarListbox()
    End Sub

    Private Sub cargarListBox()
        Dim dt As New DataTable
        dt = New TipoInstrumentoBM().SeleccionarPorCodigoyDescripcion("", "", Me.DatosRequest).Tables(0)
        Session("dtLista") = dt
        HelpCombo.LlenarListBox(Me.lbxValores, dt, "CodigoTipoInstrumento", "Descripcion", False)
        Me.lbxSeleccionValores.Items.Clear()
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As GrupoTipoRentaBE.GrupoTipoRentaRow

        oGrupoTipoRentaBE = New GrupoTipoRentaBM().SeleccionarPorFiltro(GrupoInstrumento, "", "", "Detalle", Me.DatosRequest)
        oRow = DirectCast(oGrupoTipoRentaBE.GrupoTipoRenta.Rows(0), GrupoTipoRentaBE.GrupoTipoRentaRow)

        Me.ddlSituacion.SelectedValue = oRow.Situacion
        Me.tbDescripcion.Text = oRow.Descripcion
        Me.tbGrupoInstrumento.Text = oRow.GrupoInstrumento

        'cargar detalle de registro
        Me.dgListaTipoInstrumento.DataSource = oGrupoTipoRentaBE.GrupoTipoRenta
        Me.dgListaTipoInstrumento.DataBind()
        'guardar detalle
        Session("dsDetalleGrupo") = oGrupoTipoRentaBE
        habilitarControles(False)
    End Sub

    Private Sub habilitarControles(ByVal flag As Boolean)
        Me.tbGrupoInstrumento.Enabled = flag
    End Sub

    Private Function obtenerInstancia() As GrupoTipoRentaBE

        Dim oAuxGrupoTipoRentaBE As New GrupoTipoRentaBE
        Dim fila As GrupoTipoRentaBE.GrupoTipoRentaRow

        Me.oGrupoTipoRentaBE = CType(Session("dsDetalleGrupo"), GrupoTipoRentaBE)

        For Each drv As GrupoTipoRentaBE.GrupoTipoRentaRow In Me.oGrupoTipoRentaBE.GrupoTipoRenta.Rows
            fila = oAuxGrupoTipoRentaBE.GrupoTipoRenta.NewRow

            'datos de cabecera
            fila("GrupoInstrumento") = Me.tbGrupoInstrumento.Text.ToUpper
            fila("Descripcion") = Me.tbDescripcion.Text.ToUpper
            fila("Situacion") = Me.ddlSituacion.SelectedValue

            'datos de detalle
            fila("CodigoTipoInstrumento") = drv("CodigoTipoInstrumento")

            oAuxGrupoTipoRentaBE.GrupoTipoRenta.AddGrupoTipoRentaRow(fila)
        Next

        oAuxGrupoTipoRentaBE.GrupoTipoRenta.AcceptChanges()
        Return oAuxGrupoTipoRentaBE
    End Function

    Private Function exiteGrupo(ByVal codigoGrupo As String) As Boolean
        Dim obj As GrupoTipoRentaBE

        obj = New GrupoTipoRentaBM().SeleccionarPorFiltro(codigoGrupo, "", "", "Cabecera", Me.DatosRequest)
        If (obj.GrupoTipoRenta.Rows.Count > 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub limpiarCampos()
        Me.ddlSituacion.SelectedIndex = 0
        Me.hd.Value = ""
        Me.tbDescripcion.Text = ""
        Me.tbGrupoInstrumento.Text = ""
        Me.lbxSeleccionValores.Items.Clear()
        Me.lbxValores.Items.Clear()
        Me.cargarListBox()
        Session("dsDetalleGrupo") = Nothing
        Me.dgListaTipoInstrumento.DataSource = Nothing
        Me.dgListaTipoInstrumento.DataBind()
    End Sub

    Private Sub agregarItemValores(ByVal strCodigoTipoInstrumento As String)
        Dim dt As DataTable = CType(Session("dtLista"), DataTable)
        For Each fila As DataRow In dt.Rows
            If (fila("CodigoTipoInstrumento") = strCodigoTipoInstrumento) Then
                Me.lbxValores.Items.Add(New ListItem(fila("Descripcion"), strCodigoTipoInstrumento))
                Exit Sub
            End If
        Next
    End Sub

#End Region

End Class
