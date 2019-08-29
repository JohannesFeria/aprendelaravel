Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmValorParametroEncaje
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Dim vsecuencial = Request.QueryString("cod")
                Dim objEncajeBM As New EncajeBM
                Dim dtencaje As DataTable
                dtencaje = objEncajeBM.SeleccionarParametro(vsecuencial, DatosRequest).Tables(0)
                For Each dr As DataRow In dtencaje.Rows
                    txtnombre.Text = dr("Nombre")
                    txtvalor.Text = dr("Valor")
                Next
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim vsecuencial = Request.QueryString("cod")
            Dim objEncajeBM As New EncajeBM
            objEncajeBM.ModificarParametro(vsecuencial, Me.txtnombre.Text, Me.txtvalor.Text, DatosRequest)            
            AlertaJS(ObtenerMensaje("CONF6"))            
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los Datos")
        End Try        
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmParametriaEncaje.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

End Class

