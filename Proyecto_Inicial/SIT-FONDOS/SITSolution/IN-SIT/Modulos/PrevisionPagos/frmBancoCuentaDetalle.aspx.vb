Imports Sit.BusinessEntities
Imports SIT.BusinessLayer
Public Class Modulos_PrevisionPagos_frmBancoCuentaDetalle
    Inherits BasePage
    'Dim IdUsuario As String = Usuario

    Private Sub LimpiarControles()
        ddlFondo.SelectedIndex = 0
        ddlBanco.SelectedIndex = 0
        ddlTipoCuenta.SelectedIndex = 0
        ddlTipoMoneda.SelectedIndex = 0
        ddlEstado.SelectedIndex = 0
        tbCtaCte.Text = String.Empty
    End Sub
    Private Function Validar() As Boolean
        Dim val As Boolean = True
        If ddlFondo.SelectedIndex = 0 Then
            Return False
        End If
        If ddlBanco.SelectedIndex = 0 Then
            Return False
        End If
        If ddlEstado.SelectedIndex = 0 Then
            Return False
        End If
        If ddlTipoMoneda.SelectedIndex = 0 Then
            Return False
        End If
        If ddlTipoCuenta.SelectedIndex = 0 Then
            Return False
        End If
        If tbCtaCte.Text.Trim.Equals(String.Empty) Then
            Return False
        End If
        Return val
    End Function

    Private Sub PosicionarCombosTextBox()
        For i As Integer = 1 To ddlFondo.Items.Count - 1
            If ddlFondo.Items(i).Text = Session("sEntidad").ToString Then
                ddlFondo.SelectedIndex = i
            End If
        Next
        For i As Integer = 1 To ddlBanco.Items.Count - 1
            If ddlBanco.Items(i).Text = Session("sBanco").ToString Then
                ddlBanco.SelectedIndex = i
            End If
        Next
        For i As Integer = 1 To ddlEstado.Items.Count - 1
            If ddlEstado.Items(i).Text = Session("sEstado").ToString Then
                ddlEstado.SelectedIndex = i
            End If
        Next
        For i As Integer = 1 To ddlTipoMoneda.Items.Count - 1
            If ddlTipoMoneda.Items(i).Text = Session("sTipoMoneda").ToString Then
                ddlTipoMoneda.SelectedIndex = i
            End If
        Next
        For i As Integer = 1 To ddlTipoCuenta.Items.Count - 1
            If ddlTipoCuenta.Items(i).Text = Session("sTipoCuenta").ToString Then
                ddlTipoCuenta.SelectedIndex = i
            End If
        Next

        tbCtaCte.Text = Session("sCta").ToString
    End Sub

    Private Sub CargarCombos(ByVal ddl As DropDownList, ByVal Parametro As Int32)
        Dim tablaListaParametria As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaListaParametria = PrevisionParametriaBM.ListarParametria(Parametro).Tables(0)
        HelpCombo.LlenarComboBox(ddl, tablaListaParametria, "Valor", "Descripcion", True)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CargarCombos(ddlEstado, 1)
            CargarCombos(ddlFondo, 2)
            'Se filtra Afiliado porque no requiere banco
            'ddlFondo.Items.Remove(6)
            Try
                Dim dt As New Data.DataTable
                dt = PrevisionParametriaBM.ListarParametria("15").Tables(0)

                If dt.Rows.Count > 0 Then
                    Dim Valor As String = dt.Rows(0)("Valor").ToString()
                    ddlFondo.Items.RemoveAt(CInt(Valor))
                End If
            Catch ex As Exception

            End Try

            CargarCombos(ddlBanco, 3)
            CargarCombos(ddlTipoMoneda, 6)
            CargarCombos(ddlTipoCuenta, 9)
            If Session("sCodigo") <> Nothing Then
                PosicionarCombosTextBox()
            End If
        End If
    End Sub

    Protected Sub bSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bSalir.Click
        Session("sCodigo") = Nothing
        Response.Redirect("frmBancoCuenta.aspx")
    End Sub

    Protected Sub bAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bAceptar.Click
        If Validar() = False Then
            AlertaJS("Ingrese todos los campos.")
        Else
            Try
                Dim oCta As New PrevisionCuentasCorrientes
                Dim Estado As Int32
                oCta.IdEntidad = ddlFondo.SelectedValue.ToString
                oCta.IdBanco = ddlBanco.SelectedValue.ToString
                oCta.IdTipoCuenta = ddlTipoCuenta.SelectedValue.ToString
                oCta.IdMoneda = ddlTipoMoneda.SelectedValue.ToString
                oCta.Situacion = ddlEstado.SelectedValue.ToString
                oCta.IdCuentaCorriente = tbCtaCte.Text.Trim
                oCta.CodUsuario = Usuario
                If Session("sCodigo") = Nothing Then
                    Estado = PrevisionParametriaBM.InsertarCuentaCorriente(oCta)
                    If Estado = 2 Then
                        AlertaJS("Error al grabar.")
                    ElseIf Estado = 3 Then
                        AlertaJS("Ya existe un registro con la cuenta corriente registrada.")
                    Else
                        LimpiarControles()
                        AlertaJS("Datos Grabados.")
                    End If
                Else
                    oCta.Codigo = Session("sCodigo").ToString
                    Estado = PrevisionParametriaBM.ActualizarCuentaCorriente(oCta)
                    If Estado = 2 Then
                        AlertaJS("Error al grabar.")
                    ElseIf Estado = 3 Then
                        AlertaJS("Ya existe un registro con la cuenta corriente registrada.")
                    Else
                        AlertaJS("Datos Grabados.")
                        ddlFondo.SelectedIndex = 0
                        ddlBanco.SelectedIndex = 0
                        ddlEstado.SelectedIndex = 0
                        ddlTipoCuenta.SelectedIndex = 0
                        ddlTipoMoneda.SelectedIndex = 0
                        tbCtaCte.Text = String.Empty
                        Session("sCodigo") = Nothing
                    End If
                End If

            Catch ex As Exception
                AlertaJS(ex.Message)
            End Try
        End If
    End Sub
End Class