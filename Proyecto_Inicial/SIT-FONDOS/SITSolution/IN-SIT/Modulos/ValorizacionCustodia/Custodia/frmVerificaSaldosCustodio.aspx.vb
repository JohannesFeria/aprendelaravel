'Creado por: HDG INC 64460	20120102
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT

Partial Class Modulos_Valorizacion_y_Custodia_Custodia_frmVerificaSaldosCustodio
    Inherits BasePage

#Region "Variables"

    Dim oUIUtil As New UIUtility
    Dim oUtil As New UtilDM
    '--------------------------------------------------------------------
    Dim sPortafolioCodigo As String
    Dim sCodigoCustodio As String
    Dim sCodigoISIN As String
    Dim sCodigoMnemonico As String
    Dim sCodigoMoneda As String
    '--------------------------------------------------------------------
    Dim oValores As New ValoresBM
    '--------------------------------------------------------------------
    Dim oCustodioBM As New CustodioBM

    Dim oValoresBM As New ValoresBM

#End Region

#Region "/* Métodos de la Página*/"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargaPortafolio()
                Call CargaCustodios()
                tbFechaOperacion.Text = oUtil.RetornarFechaSistema
            Else
                ViewState("vsPortafolioCodigo") = ddlFondo.SelectedValue.ToString
                ViewState("vsCodigoCustodio") = ddlCustodio.SelectedValue.ToString
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datos As String()
                datos = CType(Session("SS_DatosModal"), String())
                txtISIN.Text = datos(0)
                txtSBS.Text = datos(2)
                txtMnemonico.Text = datos(1)
                txtDescripcion.Text = datos(3)
                txtMoneda.Text = HttpUtility.HtmlDecode(datos(4)) 'HSP 20150615. Erro en tildes y ASCI
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Try
            oUtil = Nothing
            oUIUtil = Nothing
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        End Try
    End Sub

    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If Page.IsPostBack Then
                Call CargaCustodios()
            End If
            EstablecerFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub

    Private Sub btnConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsulta.Click
        Try
            Dim oValoresBE As DataSet = oValoresBM.SeleccionarInstrumento(txtISIN.Text, txtMnemonico.Text, DatosRequest)
            If oValoresBE.Tables.Count > 0 Then
                If oValoresBE.Tables(0).Rows.Count > 0 Then
                    txtISIN.Text = oValoresBE.Tables(0).Rows(0).ItemArray(0)
                    txtMnemonico.Text = oValoresBE.Tables(0).Rows(0).ItemArray(1)
                    txtSBS.Text = oValoresBE.Tables(0).Rows(0).ItemArray(2)
                    txtDescripcion.Text = oValoresBE.Tables(0).Rows(0).ItemArray(3)
                End If
            End If
            If ValidarConsulta() Then
                Call CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Consultar")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            oCustodioBM.ProcesarSaldosCustodioxMnemonico(ddlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim, txtMnemonico.Text, oUIUtil.ConvertirFechaaDecimal(tbFechaProceso.Text), DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Procesar")
        End Try
    End Sub

    Protected Sub dgAjuste_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAjuste.PageIndexChanging
        Try
            dgAjuste.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarGrilla()
        Dim oInfCusDS As New DataTable
        oInfCusDS = oCustodioBM.SeleccionaSaldosCustodioxNemonico(ddlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim, txtMnemonico.Text, oUIUtil.ConvertirFechaaDecimal(tbFechaOperacion.Text)).Tables(0)
        dgAjuste.DataSource = oInfCusDS
        dgAjuste.DataBind()
    End Sub

    Private Function VerificaRelacionInstrumentoCustodio(ByVal CodigoMnemonico As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String) As Boolean
        Dim oValores As New ValoresBM
        Dim oValoresBE As DataSet = oValoresBM.VerificaRelacionInstrumentoCustodio(CodigoMnemonico, CodigoPortafolioSBS, CodigoCustodio, DatosRequest)

        ViewState("vsCuentaDepositaria") = ""
        If oValoresBE.Tables(0).Rows.Count > 0 Then
            ViewState("vsCuentaDepositaria") = oValoresBE.Tables(0).Rows(0).Item("CuentaDepositaria")
            Return True
            Exit Function
        End If

        oValoresBE = Nothing
        Return False
    End Function

    Public Sub SeleccionarSBS(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String
        Dim strcodSBS As String

        strcadena = e.CommandArgument
        strcodSBS = strcadena.Split(",").GetValue(0)
        ViewState("vscodSBS") = strcodSBS
    End Sub

    Private Sub Limpiadatos()
        ddlFondo.SelectedIndex = 0
        ddlCustodio.SelectedIndex = 0
        txtMnemonico.Text = ""
        txtISIN.Text = ""
        txtSBS.Text = ""
        txtDescripcion.Text = ""
        txtMoneda.Text = ""
    End Sub

    Private Sub CargaPortafolio()
        Dim dsPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondo.Items.Clear()
        ddlFondo.DataSource = dsPortafolio
        ddlFondo.DataValueField = "CodigoPortafolio"
        ddlFondo.DataTextField = "Descripcion"
        ddlFondo.DataBind()
        UIUtility.InsertarElementoSeleccion(ddlFondo)
    End Sub

    Private Sub CargaCustodios()
        Dim objCustodio As New CustodioBM
        ddlCustodio.DataTextField = "Descripcion"
        ddlCustodio.DataValueField = "CodigoCustodio"
        ddlCustodio.DataSource = objCustodio.Listar(DatosRequest)
        ddlCustodio.DataBind()
        UIUtility.InsertarElementoSeleccion(ddlCustodio)
        objCustodio = Nothing
    End Sub

    Private Sub EstablecerFecha()
        If ddlFondo.SelectedIndex > 0 Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondo.SelectedValue))
        Else
            tbFechaOperacion.Text = Now.ToString("dd/MM/yyyy")
        End If
    End Sub

    Private Function ValidarConsulta() As Boolean

        If ddlFondo.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT3"))
            Return False
            Exit Function
        ElseIf ddlCustodio.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT4"))
            Return False
            Exit Function
        ElseIf txtSBS.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT5"))
            Return False
            Exit Function
        ElseIf Not VerificaRelacionInstrumentoCustodio(txtMnemonico.Text.Trim, ddlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim) Then
            AlertaJS(ObtenerMensaje("ALERT112"))
            Return False
            Exit Function
        End If
        Return True
    End Function

#End Region

End Class