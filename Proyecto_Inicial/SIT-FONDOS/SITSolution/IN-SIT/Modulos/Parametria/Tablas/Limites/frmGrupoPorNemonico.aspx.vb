Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Limites_frmGrupoPorNemonico
    Inherits BasePage
    Dim oLimParametria As New LimiteParametriaBM
    Sub Limpiar()
        txtcodigogrupo.Text = ""
        txtDescripcion.Text = ""
        ddlSituacion.SelectedValue = "A"
        lbxSeleccionValores.Items.Clear()
        Dim dt As DataTable = oLimParametria.GrupoPorNemonico_Grupo("", "", "0")
        HelpCombo.LlenarListBox(lbxValores, dt, "CodigoNemonico", "Descripcion", False)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("Codigo") Is Nothing Then
                Session("Codigo") = ""
            End If
            Dim dtD As DataTable = oLimParametria.GrupoPorNemonico_Seleccionar(Session("Codigo").ToString)
            If dtD.Rows.Count > 0 Then
                txtcodigogrupo.Text = dtD(0)("CodigoGrupoNemonico").ToString
                txtDescripcion.Text = dtD(0)("Descripcion").ToString
                ddlSituacion.SelectedValue = dtD(0)("Situacion").ToString
            End If
            Dim dt As DataTable = oLimParametria.GrupoPorNemonico_Grupo(Session("Codigo").ToString, txtDesVal.Text, "0")
            Dim dt2 As DataTable = oLimParametria.GrupoPorNemonico_Grupo(Session("Codigo").ToString, "", "1")
            HelpCombo.LlenarListBox(lbxValores, dt, "CodigoNemonico", "Descripcion", False)
            HelpCombo.LlenarListBox(lbxSeleccionValores, dt2, "CodigoNemonico", "Descripcion", False)
        End If
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If txtcodigogrupo.Text = "" Then
                AlertaJS("Ingrese el codigo del grupo", "txtcodigoclase.focus();")
            ElseIf txtDescripcion.Text = "" Then
                AlertaJS("Ingrese la Descripcion del grupo", "txtDescripcion.focus();")
            Else
                oLimParametria.GrupoPorNemonico_Borrar(txtcodigogrupo.Text)
                For Each it As ListItem In lbxSeleccionValores.Items
                    oLimParametria.GrupoPorNemonico_Insertar(txtcodigogrupo.Text, txtDescripcion.Text, it.Value, ddlSituacion.SelectedValue, DatosRequest)
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
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Dim dt As DataTable = oLimParametria.GrupoPorNemonico_Grupo(Session("Codigo").ToString, txtDesVal.Text, "0")
        HelpCombo.LlenarListBox(lbxValores, dt, "CodigoNemonico", "Descripcion", False)
    End Sub
End Class