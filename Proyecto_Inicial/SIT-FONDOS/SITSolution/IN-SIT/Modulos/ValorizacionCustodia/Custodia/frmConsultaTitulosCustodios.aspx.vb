Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT

Partial Class Modulos_Valorizacion_y_Custodia_Custodia_frmConsultaTitulosCustodios
    Inherits BasePage

#Region " /* Declaración Variables */ "

    Dim oUIUtil As New UIUtility
    Dim sFechaSaldo As String = String.Empty
    Dim sPortafolioCodigo As String = String.Empty
    Dim sCodigoISIN As String = String.Empty
    Dim sCodigoCustodio As String = String.Empty
    Dim sTipoInstrumento As String = String.Empty
    Dim sTipoRenta As String = String.Empty
    Dim sCodigoMnemonico As String = String.Empty
    Dim sCodigoMoneda As String = String.Empty
    Dim oUtil As New UtilDM
    Private campos() As String = {"Fondo", "Código ISIN", "Código Mnemonico", "Tipo Titulo", "Nombre Emisor", "Moneda", "Fecha Emisiòn", "Fecha Vencimiento", "Numero Unidades", "Valor Unitario", "Valor Nominal", "Valorizado"}

#End Region

#Region " /* Eventos de Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Try
                    tbFechaSaldo.Text = oUtil.RetornarFechaSistema
                    UIUtility.CargarMonedaOI(dlMoneda)
                    DescargaGrillaLista()
                    CargaPortafolio(ddlPortafolio)
                    CargaTipoRenta(ddlTipoRenta)
                    CargaCustodios()
                    CargaTipoInstrumento()
                Catch ex As Exception
                    AlertaJS(ex.Message.ToString())
                End Try
            Else
                ViewState("vsDescripcion") = txtDescripcion.Text
                ViewState("vsFechaSaldo") = tbFechaSaldo.Text
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datos As String()

                datos = CType(Session("SS_DatosModal"), String())
                txtISIN.Text = datos(0)
                txtSBS.Text = datos(1)
                txtMnemonico.Text = datos(2)
                txtDescripcion.Text = datos(3)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsulta.Click
        Try
            Call CargaConsulta()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Consultar")
        End Try        
    End Sub

    Private Sub btnGenerarReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarReporte.Click
        Try
            sFechaSaldo = ViewState("vsFechaSaldo")
            sFechaSaldo = sFechaSaldo.Substring(6, 4) & sFechaSaldo.Substring(3, 2) & sFechaSaldo.Substring(0, 2)

            sPortafolioCodigo = ViewState("vsPortafolioCodigo")
            sCodigoISIN = ViewState("vsCodigoISIN")
            sCodigoCustodio = ViewState("vsCodigoCustodio")
            '--------------------------------------------------------------------
            sTipoInstrumento = ViewState("vsTipoInstrumento")
            sTipoRenta = ViewState("vsTipoRenta")
            sCodigoMnemonico = ViewState("vsCodigoMnemonico")
            sCodigoMoneda = ViewState("vsCodigoMoneda")

            EjecutarJS("showModal('" + sFechaSaldo + "','" + sPortafolioCodigo + "','" + sCodigoISIN + "','" + sCodigoCustodio + "','" + sTipoInstrumento + "','" + sCodigoMnemonico + "','" + sTipoRenta + "','" + sCodigoMoneda + "');")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Generar Reporte")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            EstablecerFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            Call CargaConsulta()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en al Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Métododos Personalizados */ "

    Private Sub CargaConsulta()
        sFechaSaldo = tbFechaSaldo.Text
        sFechaSaldo = sFechaSaldo.Substring(6, 4) & sFechaSaldo.Substring(3, 2) & sFechaSaldo.Substring(0, 2)

        sPortafolioCodigo = ddlPortafolio.SelectedValue.ToString
        sCodigoISIN = txtISIN.Text
        sCodigoCustodio = dlCustodio.SelectedValue.ToString

        sTipoInstrumento = dlInstrumento.SelectedValue.ToString
        sTipoRenta = ddlTipoRenta.SelectedValue.ToString
        sCodigoMnemonico = txtMnemonico.Text
        sCodigoMoneda = dlMoneda.SelectedValue.ToString

        ViewState("vsFechaSaldo") = sFechaSaldo
        ViewState("vsPortafolioCodigo") = sPortafolioCodigo
        ViewState("vsCodigoISIN") = sCodigoISIN
        ViewState("vsCodigoCustodio") = sCodigoCustodio
        ViewState("vsTipoInstrumento") = sTipoInstrumento
        ViewState("vsTipoRenta") = sTipoRenta

        ViewState("vsCodigoMnemonico") = sCodigoMnemonico
        ViewState("vsCodigoMoneda") = sCodigoMoneda
        ViewState("vsDescripcion") = txtDescripcion.Text

        CargaConsultaTitulosAsociadosCustodios(sFechaSaldo, sPortafolioCodigo, sCodigoISIN, sCodigoCustodio, sTipoInstrumento, sTipoRenta, sCodigoMnemonico, sCodigoMoneda)

    End Sub

    Private Sub EstablecerFecha()
        If ddlPortafolio.SelectedIndex > 0 Then
            tbFechaSaldo.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
        Else
            tbFechaSaldo.Text = Now.ToString("dd/MM/yyyy")
        End If
    End Sub

    Public Sub SeleccionarSBS(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim cadena, codSBS As String
        cadena = e.CommandArgument
        codSBS = cadena.Split(",").GetValue(0) : ViewState("vscodSBS") = codSBS
    End Sub

    Private Sub DescargaGrillaLista()
        dgLista.DataSource = UIUtility.GetStructureTablebase(campos)
        dgLista.DataBind()
    End Sub

    Private Sub CargaConsultaTitulosAsociadosCustodios(ByVal sFechaSaldo As Decimal, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoISIN As String, ByVal sCodigoCustodio As String, ByVal sCodigoTipoTitulo As String, ByVal sTipoRenta As String, ByVal sCodigoMnemonico As String, ByVal sCodigoMoneda As String)
        Dim dtblDatos As DataTable
        dtblDatos = New CustodioBM().ListarTitulosAsociadosCustodiosC1(sFechaSaldo, sCodigoPortafolioSBS, sCodigoISIN, sCodigoCustodio, sCodigoTipoTitulo, sTipoRenta, sCodigoMnemonico, sCodigoMoneda, DatosRequest).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos.Rows.Count) + "');")
    End Sub

    Private Sub CargaTipoRenta(ByVal drlista As DropDownList)
        drlista.DataSource = New TipoRentaBM().Listar(DatosRequest)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoRenta" : drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista)
        UIUtility.AgregarElementoFinal(drlista, "G", "Garantía")
        UIUtility.AgregarElementoFinal(drlista, "B", "Bloqueadas")
    End Sub

    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        drlista.DataSource = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoPortafolio" : drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista)
    End Sub

    Private Sub CargaCustodios()
        dlCustodio.DataSource = New CustodioBM().Listar(DatosRequest)
        dlCustodio.DataTextField = "Descripcion" : dlCustodio.DataValueField = "CodigoCustodio" : dlCustodio.DataBind()
        UIUtility.InsertarElementoSeleccion(dlCustodio)
    End Sub

    Private Sub CargaTipoInstrumento()
        dlInstrumento.DataSource = New TipoInstrumentoBM().Listar(DatosRequest)
        dlInstrumento.DataTextField = "Descripcion" : dlInstrumento.DataValueField = "CodigoTipoInstrumentoSBS" : dlInstrumento.DataBind()
        UIUtility.InsertarElementoSeleccion(dlInstrumento)
    End Sub

#End Region

End Class
