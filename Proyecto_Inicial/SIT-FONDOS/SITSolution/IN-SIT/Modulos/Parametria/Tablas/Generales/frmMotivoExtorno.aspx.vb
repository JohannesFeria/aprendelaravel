Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmMotivoExtorno
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Not Request.QueryString("cod") Is Nothing Then
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                oParametrosGeneralesBM.Actualizar(ParametrosSIT.MOTIVO_EXTORNO, Request.QueryString("cod"), tbNombre.Text.ToUpper(), tbDescripcion.Text.ToUpper(), DatosRequest)
                AlertaJS("Los cambios se han registrado satisfactoriamente!")
            Else
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                oParametrosGeneralesBM.InsertarMotivoExtorno(tbNombre.Text.ToUpper(), tbDescripcion.Text.ToUpper(), DatosRequest)
                AlertaJS("Los cambios se han registrado satisfactoriamente!")
            End If
            LimpiarControles()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos en la Grilla")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaMotivoExtorno.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

    Private Sub CargarPagina()
        If Not Request.QueryString("cod") Is Nothing Then
            Dim dt As DataTable
            dt = New ParametrosGeneralesBM().SeleccionarMotivoExtorno("", Request.QueryString("cod"), DatosRequest).Tables(0)
            tbNombre.Text = CType(dt.Rows(0)("Nombre"), String)
            tbDescripcion.Text = CType(dt.Rows(0)("Comentario"), String)
        End If
    End Sub

    Private Sub LimpiarControles()
        tbNombre.Text = ""
        tbDescripcion.Text = ""
    End Sub

End Class
