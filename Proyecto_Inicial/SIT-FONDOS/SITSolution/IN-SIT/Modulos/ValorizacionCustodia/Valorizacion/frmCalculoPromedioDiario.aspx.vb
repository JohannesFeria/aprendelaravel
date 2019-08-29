Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data

Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmCalculoPromedioDiario
    Inherits BasePage

#Region " /* Métodos de la Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            If Not Page.IsPostBack Then
                CargarPortafolio()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                txtISIN.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oValores As New ValoresBM
            Dim PNPromedio As Decimal = oValores.SeleccionarValorizacionPromedio(ddlPortafolio.SelectedValue, txtMnemonico.Text, txtISIN.Text, txtDBase.Text, DatosRequest)
            If PNPromedio = 0 Then
                lblVPNPromedio.Text = "No se hallaron resultados"
            Else
                lblVPNPromedio.Text = Convert.ToString(PNPromedio)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub

#End Region

#Region " /* Métodos de la Página */ "

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dtPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", True)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

#End Region

End Class
