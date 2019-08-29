Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmGrupoTipoInstrumento
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

    Private oGrupoTipoInstrumentoBE As GrupoTipoInstrumentoBE
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
                CargarCombos()
                cargarListBox()
                hd.Value = ""
                If (operacion = "mod") Then
                    cargarRegistro()
                Else
                    Session("dsDetalleGrupo") = Nothing
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click        
        Try
            Dim resultado As Boolean
            If dgListaTipoInstrumento.Rows.Count <= 0 Then
                AlertaJS("Debe seleccionar al menos un Tipo de Instrumento!!.")
                Exit Sub
            End If
            oGrupoTipoInstrumentoBE = obtenerInstancia()
            If operacion = "reg" Then
                If Not (exiteGrupo(tbGrupoInstrumento.Text)) Then
                    resultado = New GrupoTipoInstrumentoBM().Insertar(oGrupoTipoInstrumentoBE, DatosRequest)
                    If (resultado) Then
                        AlertaJS("Se registro el Grupo Instrumento satisfactoriamente.")
                        limpiarCampos()
                    End If
                Else
                    AlertaJS("El Codigo Grupo ingresado ya existe!!.")
                    Exit Sub
                End If
            Else
                resultado = New GrupoTipoInstrumentoBM().Modificar(oGrupoTipoInstrumentoBE, DatosRequest)
                AlertaJS("Se modifico el Grupo Instrumento satisfactoriamente.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Session("dsDetalleGrupo") = Nothing
            Session("dtLista") = Nothing
            Response.Redirect("frmBusquedaGrupoTipoInstrumento.aspx")
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

    Protected Sub dgListaTipoInstrumento_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaTipoInstrumento.RowCommand
        Try
            Select Case e.CommandName
                Case "Eliminar"
                    agregarItemValores(e.CommandArgument)
                    oGrupoTipoInstrumentoBE = Session("dsDetalleGrupo")
                    oGrupoTipoInstrumentoBE.GrupoTipoInstrumento.Rows(CInt(e.CommandArgument)).Delete()
                    oGrupoTipoInstrumentoBE.AcceptChanges()
                    dgListaTipoInstrumento.DataSource = oGrupoTipoInstrumentoBE.GrupoTipoInstrumento
                    dgListaTipoInstrumento.DataBind()
                    Session("dsDetalleGrupo") = oGrupoTipoInstrumentoBE
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
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
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgListaTipoInstrumento_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaTipoInstrumento.PageIndexChanging
        Try
            dgListaTipoInstrumento.PageIndex = e.NewPageIndex
            dgListaTipoInstrumento.DataSource = Session("dsDetalleGrupo")
            dgListaTipoInstrumento.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub limpiarListbox()
        lbxSeleccionValores.Items.Clear()
    End Sub

    Private Sub ActualizarValores()
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
        HelpCombo.LlenarListBox(lbxValores, dtAux, "Codigo", "Descripcion", False)
    End Sub

    Private Function ExisteFila() As String
        Dim Msg As String = String.Empty
        Dim cont As Integer = 0
        Dim strCodigoTipoInstrumento As String

        For Each item As ListItem In lbxSeleccionValores.Items
            strCodigoTipoInstrumento = item.Value

            For cont = 0 To oGrupoTipoInstrumentoBE.GrupoTipoInstrumento.Rows.Count - 1
                If (oGrupoTipoInstrumentoBE.GrupoTipoInstrumento.Rows(cont).Item("CodigoTipoInstrumento") = strCodigoTipoInstrumento) Then
                    Msg = " - " + item.Text + ", ya esta seleccionado!!.\n"
                End If
            Next
        Next

        Return Msg
    End Function

    Private Sub InsertarFila()
        If Not Session("dsDetalleGrupo") Is Nothing Then
            oGrupoTipoInstrumentoBE = Session("dsDetalleGrupo")
        Else
            oGrupoTipoInstrumentoBE = New GrupoTipoInstrumentoBE
        End If
        Dim Mensaje As String = String.Empty
        Mensaje = ExisteFila()
        If (Mensaje <> "") Then
            AlertaJS(Mensaje)
            Exit Sub
        End If

        Dim row As GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow

        For Each item As ListItem In lbxSeleccionValores.Items
            row = oGrupoTipoInstrumentoBE.GrupoTipoInstrumento.NewRow
            row.Item("CodigoTipoInstrumento") = item.Value
            row.Item("DescripcionTipoInstrumento") = item.Text
            row.Item("Situacion") = "A"
            oGrupoTipoInstrumentoBE.GrupoTipoInstrumento.Rows.Add(row)
        Next

        Session("dsDetalleGrupo") = oGrupoTipoInstrumentoBE
        dgListaTipoInstrumento.DataSource = oGrupoTipoInstrumentoBE.GrupoTipoInstrumento
        dgListaTipoInstrumento.DataBind()
        limpiarListbox()
    End Sub

    Private Sub cargarListBox()
        Dim dt As New DataTable
        dt = New TipoInstrumentoBM().SeleccionarPorCodigoyDescripcion("", "", DatosRequest).Tables(0)
        Session("dtLista") = dt
        HelpCombo.LlenarListBox(lbxValores, dt, "CodigoTipoInstrumento", "Descripcion", False)
        lbxSeleccionValores.Items.Clear()
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow

        oGrupoTipoInstrumentoBE = New GrupoTipoInstrumentoBM().SeleccionarPorFiltro(GrupoInstrumento, "", "", "Detalle", DatosRequest)
        oRow = DirectCast(oGrupoTipoInstrumentoBE.GrupoTipoInstrumento.Rows(0), GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow)

        ddlSituacion.SelectedValue = oRow.Situacion
        tbDescripcion.Text = oRow.Descripcion
        tbGrupoInstrumento.Text = oRow.GrupoInstrumento

        'cargar detalle de registro
        dgListaTipoInstrumento.DataSource = oGrupoTipoInstrumentoBE.GrupoTipoInstrumento
        dgListaTipoInstrumento.DataBind()
        'guardar detalle
        Session("dsDetalleGrupo") = oGrupoTipoInstrumentoBE
        habilitarControles(False)
    End Sub

    Private Sub habilitarControles(ByVal flag As Boolean)
        tbGrupoInstrumento.Enabled = flag
    End Sub

    Private Function obtenerInstancia() As GrupoTipoInstrumentoBE

        Dim oAuxGrupoTipoInstrumentoBE As New GrupoTipoInstrumentoBE
        Dim fila As GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow

        oGrupoTipoInstrumentoBE = CType(Session("dsDetalleGrupo"), GrupoTipoInstrumentoBE)

        For Each drv As GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow In oGrupoTipoInstrumentoBE.GrupoTipoInstrumento.Rows
            fila = oAuxGrupoTipoInstrumentoBE.GrupoTipoInstrumento.NewRow

            'datos de cabecera
            fila("GrupoInstrumento") = tbGrupoInstrumento.Text.ToUpper
            fila("Descripcion") = tbDescripcion.Text.ToUpper
            fila("Situacion") = ddlSituacion.SelectedValue

            'datos de detalle
            fila("CodigoTipoInstrumento") = drv("CodigoTipoInstrumento")

            oAuxGrupoTipoInstrumentoBE.GrupoTipoInstrumento.AddGrupoTipoInstrumentoRow(fila)
        Next

        oAuxGrupoTipoInstrumentoBE.GrupoTipoInstrumento.AcceptChanges()
        Return oAuxGrupoTipoInstrumentoBE
    End Function

    Private Function exiteGrupo(ByVal codigoGrupo As String) As Boolean
        Dim obj As GrupoTipoInstrumentoBE

        obj = New GrupoTipoInstrumentoBM().SeleccionarPorFiltro(codigoGrupo, "", "", "Cabecera", DatosRequest)
        If (obj.GrupoTipoInstrumento.Rows.Count > 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub limpiarCampos()
        ddlSituacion.SelectedIndex = 0
        hd.Value = ""
        tbDescripcion.Text = ""
        tbGrupoInstrumento.Text = ""
        lbxSeleccionValores.Items.Clear()
        lbxValores.Items.Clear()
        cargarListBox()
        Session("dsDetalleGrupo") = Nothing
        dgListaTipoInstrumento.DataSource = Nothing
        dgListaTipoInstrumento.DataBind()
    End Sub

    Private Sub agregarItemValores(ByVal strCodigoTipoInstrumento As String)
        Dim dt As DataTable = CType(Session("dtLista"), DataTable)
        For Each fila As DataRow In dt.Rows
            If (fila("CodigoTipoInstrumento") = strCodigoTipoInstrumento) Then
                lbxValores.Items.Add(New ListItem(fila("Descripcion"), strCodigoTipoInstrumento))
                Exit Sub
            End If
        Next
    End Sub

#End Region

End Class
