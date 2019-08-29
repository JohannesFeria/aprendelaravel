Imports Sit.BusinessLayer
Imports System.Data
Imports Sit.BusinessEntities

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmDatosCarta
    Inherits BasePage

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not (Page.IsPostBack) Then

            ddlValorTipo.Enabled = False
            ddlCuentasDatoCarta.Enabled = False
            ListarTipoDatoCartas()
            ListarBancosPorOrden()

        End If
    End Sub


#Region "Metodos"

    Sub ListarTipoDatoCartas()
        Dim objNegParam As New ParametrosGeneralesBM()
        Dim sTipo As String = "DATOSFONDOLIMITE"
        Dim Tabla As DataTable
        Tabla = objNegParam.Listar(sTipo, Nothing)
        HelpCombo.LlenarComboBox(ddlTipoDatoCarta, Tabla, "Valor", "Nombre", True)
    End Sub

    Sub ListarValoresPorTipoDato()
        Dim objNegOrdenDatos As New OrdenInversionDatosOperacionBM()
        Dim sCodigoOrden As String = IIf(Session("CodOrden") = Nothing, 0, Session("CodOrden").ToString())
        Dim sCodigoTipo As String = ddlTipoDatoCarta.SelectedValue.ToString()
        Dim Tabla As DataTable
        Tabla = objNegOrdenDatos.ListarValorTipoCarta(sCodigoOrden, sCodigoTipo).Tables(0)
        HelpCombo.LlenarComboBox(ddlValorTipo, Tabla, "CodigoDatosCarta", "Valor", True)
    End Sub

    Sub ListarBancosPorOrden()
        Dim objNegOrdenDatos As New OrdenInversionDatosOperacionBM()

        Dim sCodigoOrden As String = IIf(Session("CodOrden") = Nothing, 0, Session("CodOrden").ToString())
        Dim Tabla As DataTable
        Tabla = objNegOrdenDatos.ListarBancosConfirmacion(sCodigoOrden).Tables(0)
        HelpCombo.LlenarComboBox(ddlBancosDatoCarta, Tabla, "CodigoTercero", "Descripcion", True)
    End Sub

    Sub ListarCuentasBanco()
        Dim objNegOrdenDatos As New OrdenInversionDatosOperacionBM()
        Dim sCodigoTercero As String = ddlBancosDatoCarta.SelectedValue.ToString()
        Dim sCodigoOrden As String = IIf(Session("CodOrden") = Nothing, 0, Session("CodOrden").ToString())
        Dim Tabla As DataTable
        Tabla = objNegOrdenDatos.ListarCuentasPorBancoConfirmacion(sCodigoTercero, sCodigoOrden).Tables(0)
        HelpCombo.LlenarComboBox(ddlCuentasDatoCarta, Tabla, "Codigo", "Descripcion", True)
    End Sub

#End Region

#Region "Eventos"

    Protected Sub ddlTipoDatoCarta_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTipoDatoCarta.SelectedIndexChanged

        If (ddlTipoDatoCarta.SelectedValue.ToString.Equals("0")) Then
            ddlValorTipo.Enabled = False
        Else
            ddlValorTipo.Enabled = True
            ListarValoresPorTipoDato()
        End If
    End Sub

    Protected Sub ddlBancosDatoCarta_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlBancosDatoCarta.SelectedIndexChanged

        If (ddlBancosDatoCarta.SelectedValue.ToString.Equals("0")) Then
            ddlCuentasDatoCarta.Enabled = False
        Else
            ddlCuentasDatoCarta.Enabled = True
            ListarCuentasBanco()
        End If
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As System.EventArgs) Handles btnGrabar.Click

        Dim objEnt As New DatosCartasBE()
        objEnt.CodigoTipo = ddlTipoDatoCarta.SelectedValue.ToString()
        objEnt.ValorTipo = ddlValorTipo.SelectedValue.ToString()
        objEnt.CodigoTercero = ddlBancosDatoCarta.SelectedValue.ToString()
        objEnt.NumeroCuenta = ddlCuentasDatoCarta.SelectedValue.ToString()
        Session("ObjDatosCarta") = objEnt
        CerrarModal()

    End Sub


    Private Sub CerrarModal()
        EjecutarJS("window.close();")
    End Sub

#End Region

End Class
