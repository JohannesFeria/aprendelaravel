Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Imports System.Web.Configuration

Partial Class Modulos_Menu_frmEdicionRol
    Inherits BasePage

#Region "/* Metodos Personalizados */"

    Private Sub CargarPagina()

        CargarCombos()

        If Request.QueryString("type") IsNot Nothing Then
            tbCodAplicacion.Text = Request.QueryString("codApl").ToString
            If Request.QueryString("type") = "Add" Then
                lblEdicion.Text = "Agregar rol"
                tbCodRol.Enabled = False
            Else
                lblEdicion.Text = "Modificar rol"
                tbCodRol.Enabled = True
                tbCodRol.Text = Request.QueryString("codRol").ToString
                tbDescRol.Text = Request.QueryString("nomRol").ToString
                ddlSituacion.SelectedValue = Request.QueryString("est").ToString
            End If
        End If

    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub Grabar()
        Dim rolBM As New MnRolBM

        If rolBM.Insertar(RegistrarRol(True), DatosRequest) Then
            LimpiarControles()
            AlertaJS("Se registró el nuevo Rol correctamente")
        End If
    End Sub

    Private Sub Actualizar()
        Dim rolBM As New MnRolBM

        If rolBM.Actualizar(RegistrarRol(False), DatosRequest) Then
            AlertaJS("Se actualizó el Rol correctamente")
        End If
    End Sub

    Private Function RegistrarRol(isInsert As Boolean) As MnRolBE
        Dim rolBE As New MnRolBE

        rolBE.CODIGO_APLICATIVO = tbCodAplicacion.Text.Trim
        If Not isInsert Then
            rolBE.CODIGO_ROL = tbCodRol.Text.Trim
        End If
        rolBE.NOMBRE_ROL = tbDescRol.Text.Trim
        rolBE.ESTADO = ddlSituacion.SelectedValue

        Return rolBE
    End Function

    Private Sub LimpiarControles()
        tbCodAplicacion.Text = String.Empty
        tbCodRol.Text = String.Empty
        tbDescRol.Text = String.Empty
        ddlSituacion.SelectedValue = "A"
    End Sub

#End Region

#Region "/* Eventos de la Pagina */"

    Protected Sub Modulos_Menu_frmEdicionRol_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString())
        End Try
    End Sub


    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Request.QueryString("type") = "Add" Then
                Grabar()
            Else
                Actualizar()
            End If
        Catch ex As Exception
            AlertaJS(ex.ToString())
        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("frmRol.aspx")
    End Sub

#End Region

End Class
