Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmGrupoInstrumento
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
#End Region

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
                dtCaracteristicaGrupoNivel = UIUtility.GetStructureTablebase(camposCaracteristicaGrupo, tiposCaracteristicaGrupo)
                ViewState("IndiceGrilla") = Nothing
                ViewState("CaracteristicaGrupoNivel") = dtCaracteristicaGrupoNivel
                If Not (Request.QueryString("cod") = Nothing) Then
                    hd.Value = Request.QueryString("cod")
                    CargarRegistro(hd.Value.Trim())
                Else
                    hd.Value = ""
                    Dim dtAux As DataTable = oGrupoInstrumentoBM.SeleccionarCaracteristicaGrupo("", "", DatosRequest).Tables(0)
                    ViewState("Caracteristicas") = dtAux
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaGrupoInstrumento.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub

    Protected Sub dgListaGrupoInstrumento_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgListaGrupoInstrumento.PageIndexChanging
        Try
            dgListaGrupoInstrumento.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

    Protected Sub dgListaGrupoInstrumento_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgListaGrupoInstrumento.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
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
            AlertaJS("Ocurrió un error al Agregar Elemento")
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
            AlertaJS("Ocurrió un error al Devolver Elemento")
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

    Private Sub ibAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            dtCaracteristicaGrupoNivel = ViewState("CaracteristicaGrupoNivel")
            If dtCaracteristicaGrupoNivel.Rows.Count <> 0 Then
                If (hd.Value = "") Then
                    Try
                        oGrupoInstrumentoBM.Insertar(tbDescripcion.Text, ddlSituacion.SelectedValue, dtCaracteristicaGrupoNivel, DatosRequest)
                        LimpiarCampos()
                        AlertaJS("Los Datos Fueron Guardados Correctamente")
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    End Try
                Else
                    Try
                        oGrupoInstrumentoBM.Modificar(hd.Value.Trim(), tbDescripcion.Text, ddlSituacion.SelectedValue, dtCaracteristicaGrupoNivel, DatosRequest)
                        LimpiarCampos()
                        AlertaJS("Los Datos Fueron Modificados Correctamente")
                    Catch ex As Exception
                        AlertaJS(ex.ToString())
                    End Try
                End If
            Else
                AlertaJS("Debe Ingresar Mínimo una Característica")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try        
    End Sub

    Private Sub ddlCaracteristica_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCaracteristica.SelectedIndexChanged
        Try
            If (ddlCaracteristica.SelectedIndex <> 0) Then
                Dim tablaValores As DataTable = oGrupoInstrumentoBM.SeleccionarValoresPorCaracteristica(ddlCaracteristica.SelectedValue.ToString, DatosRequest).Tables(0)
                HelpCombo.LlenarListBox(lbxValores, tablaValores, "Codigo", "Descripcion", False)
                Dim dt As DataTable = oGrupoInstrumentoBM.SeleccionarCodigoCaracteristicasGrupoNivel("GIXXX", "D", "", DatosRequest).Tables(0)
                dgListaGrupoInstrumento.DataSource = dt
                ViewState("CaracteristicaGrupoNivel") = dt
                dgListaGrupoInstrumento.DataBind()
                lbxSeleccionValores.Items.Clear()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            InsertarFila()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregar")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarGrilla()
        Dim dtDatos As New DataTable
        dtDatos = ViewState("CaracteristicaGrupoNivel")
        dgListaGrupoInstrumento.DataSource = dtDatos
        dgListaGrupoInstrumento.DataBind()
    End Sub

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        Dim tablaCaracteristica As New DataTable
        tablaCaracteristica = oGrupoInstrumentoBM.SeleccionarCaracteristicaGrupo("", "", DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlCaracteristica, tablaCaracteristica, "CodigoCaracteristica", "Descripcion", True)
    End Sub

    Private Sub limpiarListbox()
        lbxValores.Items.Clear()
        lbxSeleccionValores.Items.Clear()
    End Sub

    Private Sub LimpiarCampos()
        ddlSituacion.SelectedIndex = 0
        tbDescripcion.Text = ""
        limpiarListbox()
        ViewState("IndiceGrilla") = Nothing
        ViewState("CaracteristicasValores") = Nothing
        ViewState("CaracteristicaGrupoNivel") = Nothing
        CargarGrilla()
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
        HelpCombo.LlenarListBox(lbxValores, dtAux, "Codigo", "Descripcion", False)
    End Sub

    Public Sub CargarRegistro(ByVal codigoGrupoInstrumento As String)
        Dim dtGrupoInstrumento As New DataTable
        Dim estado As String
        dtGrupoInstrumento = oGrupoInstrumentoBM.SeleccionarPorFiltro(codigoGrupoInstrumento, "", "", DatosRequest).Tables(0)
        tbDescripcion.Text = dtGrupoInstrumento.Rows(0).Item("Descripcion")
        estado = dtGrupoInstrumento.Rows(0).Item("Situacion")
        If (estado.Length > 1) Then
            ddlSituacion.SelectedValue = estado.Substring(0, 1)
        End If
        'RGF 20080812

        ddlCaracteristica.SelectedValue = IIf(IsDBNull(dtGrupoInstrumento.Rows(0).Item("CodigoCaracteristica")), "", dtGrupoInstrumento.Rows(0).Item("CodigoCaracteristica").ToString())
        ddlCaracteristica.Enabled = False
        Dim tablaValores As DataTable = oGrupoInstrumentoBM.SeleccionarValoresPorCaracteristica(ddlCaracteristica.SelectedValue.ToString, DatosRequest).Tables(0)
        HelpCombo.LlenarListBox(lbxValores, tablaValores, "Codigo", "Descripcion", False)

        'Caracteristicas
        dtCaracteristicaGrupoNivel = oGrupoInstrumentoBM.SeleccionarCodigoCaracteristicasGrupoNivel(hd.Value.Trim(), "D", "", DatosRequest).Tables(0)
        For Each fila As DataRow In dtCaracteristicaGrupoNivel.Rows
            fila("DescripcionValorCaracteristica") = oGrupoInstrumentoBM.SeleccionarDescripcionValoresPorValorVista(fila("ValorCaracteristica").ToString, fila("Vista").ToString, DatosRequest)
        Next
        ViewState("CaracteristicaGrupoNivel") = dtCaracteristicaGrupoNivel
        dgListaGrupoInstrumento.DataSource = dtCaracteristicaGrupoNivel
        dgListaGrupoInstrumento.DataBind()
    End Sub

    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim dtCaracteristicaGrupoNivel As DataTable
        Dim dr As DataRow
        Dim index As Integer = 0
        Dim c As Integer = 0
        dtCaracteristicaGrupoNivel = ViewState("CaracteristicaGrupoNivel")

        For Each dr In dtCaracteristicaGrupoNivel.Rows
            'RGF 20080812 CodigoCaracteristica no es unico por registro
            'If CType(dr("CodigoCaracteristica"), String) = CType(e.CommandArgument, String) Then
            If CType(dr("ClaseNormativa"), String) = CType(e.CommandArgument, String) Then
                'c = index
                index = c 'RGF 20080709
                Exit For
            End If
            c = c + 1
        Next
        dtCaracteristicaGrupoNivel.Rows(index).Delete()
        dtCaracteristicaGrupoNivel.AcceptChanges()

        ViewState("CaracteristicaGrupoNivel") = dtCaracteristicaGrupoNivel
        dgListaGrupoInstrumento.DataSource = dtCaracteristicaGrupoNivel

        dgListaGrupoInstrumento.DataBind()
    End Sub

    Private Function ExisteFila(ByVal dtCaracteristicaGrupoNivel As DataTable) As String
        Dim Msg As String = String.Empty
        Dim cont As Integer = 0
        Dim CodigoCaracteristica, ValorCaracteristica, ClaseNormativa As String
        CodigoCaracteristica = ddlCaracteristica.SelectedValue.ToString
        For Each item As ListItem In lbxSeleccionValores.Items
            ValorCaracteristica = item.Value
            ClaseNormativa = item.Value
            For Each fila As DataRow In dtCaracteristicaGrupoNivel.Rows
                If Not fila.RowState = DataRowState.Deleted Then
                    If (fila("CodigoCaracteristica").ToString = CodigoCaracteristica And fila("ValorCaracteristica").ToString = ValorCaracteristica And fila("ClaseNormativa").ToString = ClaseNormativa) Then
                        'If (fila("CodigoCaracteristica").ToString = CodigoCaracteristica And fila("ValorCaracteristica").ToString = ValorCaracteristica) Then
                        Msg = "- El registro " + item.Text + " elegido ya existe\n"
                    End If
                End If
            Next
        Next
        Return Msg
    End Function

    Private Sub InsertarFila()
        Dim dtCaracteristicaGrupoNivel As New DataTable
        If Not ViewState("CaracteristicaGrupoNivel") Is Nothing Then
            dtCaracteristicaGrupoNivel = ViewState("CaracteristicaGrupoNivel")
        Else
            dtCaracteristicaGrupoNivel = UIUtility.GetStructureTablebase(camposCaracteristicaGrupo, tiposCaracteristicaGrupo)
        End If
        Dim Mensaje As String = String.Empty
        Mensaje = ExisteFila(dtCaracteristicaGrupoNivel)
        If (Mensaje <> "") Then
            AlertaJS(Mensaje)
        End If

        Dim row As DataRow
        For Each item As ListItem In lbxSeleccionValores.Items
            row = dtCaracteristicaGrupoNivel.NewRow
            row.Item("CodigoCaracteristica") = ddlCaracteristica.SelectedValue.ToString
            row.Item("ValorCaracteristica") = item.Value
            row.Item("DescripcionValorCaracteristica") = item.Text
            row.Item("ClaseNormativa") = item.Value
            row.Item("Situacion") = "A"
            dtCaracteristicaGrupoNivel.Rows.Add(row)
        Next
        dgListaGrupoInstrumento.DataSource = dtCaracteristicaGrupoNivel
        ViewState("CaracteristicaGrupoNivel") = dtCaracteristicaGrupoNivel
        dgListaGrupoInstrumento.DataBind()
    End Sub

#End Region

End Class
