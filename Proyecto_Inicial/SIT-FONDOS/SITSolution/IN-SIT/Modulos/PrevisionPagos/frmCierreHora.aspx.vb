Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Globalization
Imports System.Web
Imports System.Web.UI.Control

Public Class Modulos_PrevisionPagos_frmCierreHora
    Inherits BasePage

    'Dim IdUsuario As String = Usuario.ToString

    Protected Sub Alerta(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), New Guid().ToString(), "<script language='javascript'>alert('" & mensaje & "');</script>", False)
        'Me.RegisterStartupScript("Mensaje", "<script language='javascript'>alert('" & mensaje & "');</script>")
    End Sub

    Private Function ValidarFormato(ByVal texto As String) As Boolean
        Dim dato As DateTime
        Dim estado As Boolean
        estado = DateTime.TryParseExact(texto, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, dato)
        Return estado
    End Function

    Private Sub CargarDatos()
        Dim ObtenerCierre As New DataSet
        ObtenerCierre = PrevisionCierreBM.ObtenerPrevisionCierre()
        txtHoraCierreReg.Text = ObtenerCierre.Tables(0).Rows(0)(0)
        Session("sTipoCierre1") = ObtenerCierre.Tables(0).Rows(0)(1)
        txtHoraCierreApro.Text = ObtenerCierre.Tables(0).Rows(1)(0)
        Session("sTipoCierre2") = ObtenerCierre.Tables(0).Rows(1)(1)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CargarDatos()
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        If txtHoraCierreApro.Text = String.Empty Or txtHoraCierreReg.Text = String.Empty Then
            AlertaJS("Ingresar todos los datos")
            Return
        Else
            If (ValidarFormato(txtHoraCierreReg.Text.Trim) = False Or ValidarFormato(txtHoraCierreApro.Text.Trim) = False) Then
                AlertaJS("Ingresar en formato hh:mm")
            Else
                Try
                    If PrevisionCierreBM.ActualizarPrevisionCierre(txtHoraCierreReg.Text.Trim, Session("sTipoCierre1").ToString, txtHoraCierreApro.Text.Trim, Session("sTipoCierre2"), Usuario.ToString) = True Then
                        AlertaJS("Actualizacion correcta")
                    Else
                        AlertaJS("Error al actualizar")
                    End If

                Catch ex As Exception
                    AlertaJS(ex.Message)
                End Try
            End If
        End If
    End Sub

End Class
