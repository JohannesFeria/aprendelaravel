Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_ValorizacionCustodia_Valorizacion_frmValorizacionMensual
    Inherits BasePage

#Region "Eventos de la página"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace("'", ""))
        End Try
    End Sub

    Protected Sub ddlPortafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            CargarFechas()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace("'", ""))
        End Try
    End Sub

    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click
        Try
            If ProcesarValoracion() Then
                AlertaJS("El proceso de la valorización se ejecutó correctamente")
            Else
                AlertaJS("El proceso de la valorización no se pudo ejecutar")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace("'", ""))
        End Try
    End Sub

#End Region

#Region "Métodos de la página"

    Private Sub CargarPagina()
        CargarPortafolios()
        CargarFechas()
    End Sub

    Private Sub CargarPortafolios()
        Dim dt As DataTable = New PortafolioBM().Portafolio_ListarPortafolioMensual("", "S", "A")
        HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub CargarFechas()
        Dim objCarteraTituloValoracionBM As New CarteraTituloValoracionBM
        If ddlPortafolio.SelectedValue <> "" Then
            txtFechaInicio.Text = UIUtility.ConvertirFechaaString(objCarteraTituloValoracionBM.ObtenerFechaValoracion(ddlPortafolio.SelectedValue.ToString))
            txtFechaFin.Text = UIUtility.ConvertirFechaaString(objCarteraTituloValoracionBM.ObtenerFechaValoracion(ddlPortafolio.SelectedValue.ToString))
        Else
            txtFechaInicio.Text = String.Empty
            txtFechaFin.Text = String.Empty
        End If
    End Sub

    Private Function ProcesarValoracion() As Boolean
        ProcesarValoracion = New CarteraTituloValoracionBM().CarteraTituloValoracion_GenerarValoracionMensual(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaInicio.Text), UIUtility.ConvertirFechaaDecimal(txtFechaFin.Text), DatosRequest)
    End Function

#End Region

End Class
