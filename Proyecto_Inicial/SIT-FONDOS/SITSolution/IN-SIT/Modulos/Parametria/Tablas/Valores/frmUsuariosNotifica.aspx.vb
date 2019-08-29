Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Text
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmUsuariosNotifica
    Inherits BasePage

    Protected oLimiteBM As New LimiteBM
    Private CodigoInterno As String
    Private Descripcion As String

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Me.CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbNombre.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                tbCodigoUsuario.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                tbUnidad.Text = CType(Session("SS_DatosModal"), String())(2).ToString()
                hdCodCentroCosto.Value = CType(Session("SS_DatosModal"), String())(3).ToString()
                hdCodInterno.Value = CType(Session("SS_DatosModal"), String())(4).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Try
            Agregar()
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al Agregar")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim CodigoInterno As String = e.CommandArgument.ToString()
                        oLimiteBM.EliminarUsuarioNotifica(CodigoInterno, Me.DatosRequest)
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    End Try
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            Me.dgLista.DataSource = CType(Session("dtUsuarioNotifica"), DataTable)
            Me.dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Agregar()
        Dim resultado As Boolean
        Dim oUsuario As New UsuariosNotificaBE
        Dim oRow As UsuariosNotificaBE.UsuariosNotificaRow

        oRow = CType(oUsuario.UsuariosNotifica.NewRow(), UsuariosNotificaBE.UsuariosNotificaRow)
        oRow.CodigoInterno = hdCodInterno.Value.Trim
        oRow.CodigoUsuario = tbCodigoUsuario.Text.Trim
        oRow.CodigoCentroCosto = hdCodCentroCosto.Value.Trim
        oRow.Email = New PersonalBM().SeleccionarMail(tbCodigoUsuario.Text)

        oUsuario.UsuariosNotifica.AddUsuariosNotificaRow(oRow)
        oUsuario.UsuariosNotifica.AcceptChanges()

        resultado = oLimiteBM.InsertarUsuarioNotifica(oUsuario, Me.DatosRequest)
        If resultado Then
            Me.AlertaJS("Se registro el Usuario Notificado satisfactoriamente.")
            tbNombre.Text = String.Empty
        Else
            Me.AlertaJS("El registro ya existe!!.")
            Exit Sub
        End If

        Me.dgLista.PageIndex = 0
        CargarGrilla()        
    End Sub

    Private Sub CargarGrilla()
        Dim dt As DataTable
        dt = oLimiteBM.SeleccionarUsuarioNotifica
        Session("dtUsuarioNotifica") = dt
        Me.dgLista.DataSource = dt
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('Registros Encontrados: " + dgLista.Rows.Count.ToString + "')")
    End Sub

#End Region

End Class
