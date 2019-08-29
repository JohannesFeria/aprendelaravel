Imports System.Web
Imports System.Security
Imports System.Reflection
Imports System.Diagnostics
Imports System.Configuration
Imports System.Text
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Gestion_frmConsultaDuraciones
    Inherits BasePage


#Region "/* Variables */"
    Dim oCarteraTituloValoracionBM As New CarteraTituloValoracionBM
    Dim oCarteraTituloValoracionBE As New DataSet
    '----------------------------------------------------------------
    Dim strMensajeObli As String = ""
#End Region

#Region "/* Metodo Personalizados*/"
#Region "/*Cargar*/"
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            Dim dtPortafolio As DataTable
            Dim oPortafolioBM As New PortafolioBM
            'dtPortafolio = oPortafolioBM.Listar(Me.DatosRequest).Portafolio
            dtPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(Me.ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", True)
        End If
    End Sub
#End Region
    Public Sub SeleccionarSBS(ByVal sender As Object, ByVal e As CommandEventArgs)

        Dim cadena, codSBS As String
        cadena = e.CommandArgument
        codSBS = cadena.Split(",").GetValue(0) : viewstate("vscodSBS") = codSBS

    End Sub

    Private Function GetISIN() As String
        Return tbCodigoIsin.Text
    End Function

    Private Function GetSBS() As String
        Return tbCodigoSBS.Text
    End Function

    Private Function GetMNEMONICO() As String
        Return tbCodigoMnemonico.Text
    End Function

    Private Sub Listar()
        Dim fechaValoracion As Decimal
        fechaValoracion = UIUtility.ConvertirFechaaDecimal(Me.tbFechaValoracion.Text)
        ViewState("Parametros") = Me.tbFechaValoracion.Text + "," + ddlPortafolio.SelectedValue.ToString() + "," + tbCodigoMnemonico.Text
        oCarteraTituloValoracionBE = oCarteraTituloValoracionBM.SeleccionarDuraciones(ddlPortafolio.SelectedValue.ToString(), tbCodigoMnemonico.Text, fechaValoracion, DatosRequest)
        ViewState("CarteraTituloValoracion") = oCarteraTituloValoracionBE
        Session("dsConsultaDuraciones") = oCarteraTituloValoracionBE
        CargarGrilla()
    End Sub
    Private Sub CargarGrilla()
        Dim dsCarteraTituloValoracion As New DataSet
        dsCarteraTituloValoracion = ViewState("CarteraTituloValoracion")
        dgLista.DataSource = dsCarteraTituloValoracion.Tables(0)
        dgLista.DataBind()
        If (dsCarteraTituloValoracion.Tables(0).Rows.Count = 0) Then
            AlertaJS("No hay resultado para la busqueda solicitada")
        End If
        Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dsCarteraTituloValoracion.Tables(0))
    End Sub

    Private Function ValidarBusqueda() As Boolean
        Dim ok As Boolean = True
        If (ddlPortafolio.SelectedItem.Text = "--Seleccione--") Then
            strMensajeObli = strMensajeObli + "<br /> - Portafolio "
            ok = False
        End If
        If (tbFechaValoracion.Text = "") Then
            strMensajeObli = strMensajeObli + "<br /> - Fecha "
            ok = False
        End If
        Return ok
    End Function
#End Region

#Region "/* Metodos de la pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
            If Not Session("SS_DatosModal") Is Nothing Then

                tbCodigoIsin.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                tbCodigoSBS.Text = CType(Session("SS_DatosModal"), String())(2).ToString()
                tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                tbDescripcionInstrumento.Text = CType(Session("SS_DatosModal"), String())(4).ToString()

                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub imbBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lkbMnemonico.Click
        Dim StrURL As String = "../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=" + tbCodigoIsin.Text.Trim + "&vSbs=" + tbCodigoSBS.Text.Trim + "&vMnemonico=" + tbCodigoMnemonico.Text.Trim
        EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', '" & btnpopup.ClientID & "');")
    End Sub
    Private Sub imbListar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListar.Click
        Try
            Dim okBusqueda As Boolean
            strMensajeObli = "<script language='JavaScript'> alertify.alert('Los siguientes campos son obligatorios:"
            okBusqueda = ValidarBusqueda()
            If (okBusqueda = True) Then
                dgLista.PageIndex = 0
                Listar()
            Else
                strMensajeObli = strMensajeObli + "')</script>"
                EjecutarJS(strMensajeObli, False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim parametros() As String
            parametros = Split(ViewState("Parametros"), ",")
            If (parametros(0) <> "") Then

                Dim fechaValoracion As String = parametros(0)
                Dim fondo As String = parametros(1)
                Dim codigoMnemonico As String = parametros(2)
                EjecutarJS(UIUtility.MostrarPopUp("frmVisorConsultaDuraciones.aspx?vfechaValoracion=" + parametros(0) + "&vfondo=" + parametros(1) + "&vcodigomnemonico=" + parametros(2), "no", 1100, 850, 40, 150, "no", "yes", "yes", "yes"), False)
            Else
                AlertaJS("Realice primero la consulta")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al imprimir")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el paginado")
        End Try
    End Sub
End Class
