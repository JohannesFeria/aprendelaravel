Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Limites_frmGrupoPorCalificacion
    Inherits BasePage
    Sub Limpiar()
        Dim oLimParametria As New LimiteParametriaBM
        txtcodigogrupo.Text = ""
        txtDescripcion.Text = ""
        ddlSituacion.SelectedValue = "A"
        lbxSeleccionValores.Items.Clear()
        chklOpciones.Items(0).Selected = False
        chklOpciones.Items(1).Selected = False
        Dim dt As DataTable = oLimParametria.GrupoPorCalificacion_Grupo("", "0")
        lbxValores.DataSource = dt
        lbxValores.DataTextField = "Nombre"
        lbxValores.DataValueField = "Valor"
        lbxValores.DataBind()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("Codigo") Is Nothing Then
                Session("Codigo") = ""
            End If
            Dim oLimParametria As New LimiteParametriaBM
            Dim dtD As DataTable = oLimParametria.GrupoPorCalificacion_Seleccionar(Session("Codigo").ToString)
            If dtD.Rows.Count > 0 Then
                txtcodigogrupo.Text = dtD(0)("CodigoGrupoClasificacion").ToString
                txtDescripcion.Text = dtD(0)("Descripcion").ToString
                ddlSituacion.SelectedValue = dtD(0)("Situacion").ToString
                chklOpciones.Items(0).Selected = IIf(dtD(0)("Local").ToString = "S", True, False)
                chklOpciones.Items(1).Selected = IIf(dtD(0)("Dpz").ToString = "S", True, False)
            End If
            Dim dt As DataTable = oLimParametria.GrupoPorCalificacion_Grupo(Session("Codigo").ToString, "0")
            Dim dt2 As DataTable = oLimParametria.GrupoPorCalificacion_Grupo(Session("Codigo").ToString, "1")
            lbxValores.DataSource = dt
            lbxValores.DataTextField = "Nombre"
            lbxValores.DataValueField = "Valor"
            lbxValores.DataBind()
            lbxSeleccionValores.DataSource = dt2
            lbxSeleccionValores.DataTextField = "Nombre"
            lbxSeleccionValores.DataValueField = "Valor"
            lbxSeleccionValores.DataBind()
        End If
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If txtcodigogrupo.Text = "" Then
                AlertaJS("Ingrese el codigo del grupo", "txtcodigoclase.focus();")
            ElseIf txtDescripcion.Text = "" Then
                AlertaJS("Ingrese la Descripcion del grupo", "txtDescripcion.focus();")
            Else
                Dim oLimParametria As New LimiteParametriaBM
                oLimParametria.GrupoPorCalificacion_Borrar(txtcodigogrupo.Text)
                For Each it As ListItem In lbxSeleccionValores.Items
                    oLimParametria.GrupoPorCalificacion_Insertar(txtcodigogrupo.Text,
                    IIf(chklOpciones.Items(0).Selected, "S", "N"), IIf(chklOpciones.Items(1).Selected, "S", "N"), txtDescripcion.Text,
                    it.Value, ddlSituacion.SelectedValue, DatosRequest)
                Next
                If Session("Codigo") = "" Then
                    Limpiar()
                End If
                AlertaJS("Datos guardados correctamente.")
            End If
        Catch ex As Exception
            AlertaJS("Error: " + ex.Message)
        End Try
    End Sub
    Protected Sub btnAgregarTodosCaracteristica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarTodosCaracteristica.Click
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
    Protected Sub btnAgregarCaracteristica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarCaracteristica.Click
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
    Protected Sub btnDevolverCaracteristica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDevolverCaracteristica.Click
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
    Protected Sub btnDevolverTodosCaracteristica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDevolverTodosCaracteristica.Click
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
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("frmBusquedaLimiteParametria.aspx")
    End Sub
End Class