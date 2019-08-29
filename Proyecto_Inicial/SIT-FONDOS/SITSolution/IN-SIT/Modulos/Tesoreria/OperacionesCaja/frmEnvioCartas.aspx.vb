Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Tesoreria_OperacionesCaja_frmEnvioCartas
    Inherits BasePage


#Region "Cargar Datos"
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolioSBS"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO)
            ddlMoneda.Items.Clear()
            ddlMoneda.DataSource = dsMoneda
            ddlMoneda.DataValueField = "CodigoMoneda"
            ddlMoneda.DataTextField = "Descripcion"
            ddlMoneda.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub

    Private Sub CargarBanco(ByVal codigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio(codigoMercado, Me.ddlPortafolio.SelectedValue)
            ddlBanco.Items.Clear()
            ddlBanco.DataSource = dsBanco
            ddlBanco.DataValueField = "CodigoTercero"
            ddlBanco.DataTextField = "Descripcion"
            ddlBanco.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
#End Region

#Region "Metodos de Control"

    Private Sub CargarGrilla()
        Dim dsOpCaja As New OperacionCajaBE
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
        Dim fecha As Decimal = 0
        opCaja.CodigoMercado = ListaMercados()
        opCaja.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
        opCaja.EstadoCarta = ""
        opCaja.TipoEmisionCarta = ""
        opCaja.NumeroCuenta = ""
        opCaja.CodigoOperacionCaja = ""
        opCaja.CodigoClaseCuenta = ""
        opCaja.CodigoTerceroOrigen = ddlBanco.SelectedValue
        opCaja.CodigoMoneda = ddlMoneda.SelectedValue
        opCaja.FechaCreacion = UIUtility.ConvertirFechaaDecimal(tbFecha.Text)
        dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
        Dim dsOperaciones As DataSet = New OperacionesCajaBM().SeleccionarCartas(dsOpCaja, False, DatosRequest)
        Session("ReporteEnvioCartas") = dsOperaciones

        If dsOperaciones.Tables(0).Rows.Count <> 0 Then
            dgLista.DataSource = dsOperaciones
            dgLista.DataBind()

            lbContador.Text = UIUtility.MostrarResultadoBusqueda(dsOperaciones.Tables(0))
        Else
            Dim dstemp As New DataTable
            dgLista.DataSource = dstemp
            dgLista.DataBind()
            lbContador.Text = UIUtility.MostrarResultadoBusqueda(dstemp)
        End If
    End Sub

    Private Sub EstablecerFecha()
        tbFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
    End Sub
#End Region

#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPortafolio()
            CargarMoneda()
            CargarBanco(String.Empty, True)
            EstablecerFecha()
            Dim dstemp As New DataTable
            dgLista.DataSource = dstemp
            dgLista.DataBind()
            Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dstemp)
            btnVista.Attributes.Add("onclick", "return ValidarSeleccion(true);")
        End If
    End Sub
#End Region

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        CargarGrilla()
        hdNumeroCarta.Value = ""
        dgLista.SelectedIndex = -1
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        For Each row As DataGridItem In dgLista.Rows
            Dim NumeroCarta As String = row.Cells(2).Text
            Dim chkEnviar As CheckBox = row.Cells(8).Controls(1)
            If chkEnviar.Checked And chkEnviar.Enabled Then
                Dim oOperacionCaja As New OperacionesCajaBM
                oOperacionCaja.ModificarEstadoCarta(NumeroCarta, ParametrosSIT.ESTADOCARTA_ENVIADA, "", False, DatosRequest)
            End If
        Next
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        EjecutarJS("window.open('../Reportes/frmReporte.aspx?ClaseReporte=ReporteEnvioCartas', '', 'width=800, height=600, top=50, left=50, menubar=no, resizable=yes');")
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        EstablecerFecha()
        CargarBanco(String.Empty, True)
    End Sub

    Private Sub dgLista_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        hdNumeroCarta.Value = dgLista.Rows(dgLista.SelectedIndex).Cells(2).Text
    End Sub

    Public Sub dgListaItemCommand(ByVal sender As Object, ByVal e As CommandEventArgs)
        If e.CommandName = "Seleccionar" Then
            hdNumeroCarta.Value = e.CommandArgument
        End If
    End Sub

    Private Sub btnVista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVista.Click
        Dim objparametrosgenerales As New ParametrosGeneralesBM
        Dim ruta As String = objparametrosgenerales.ListarRutaGeneracionCartas(DatosRequest)

        Try
            If IO.File.Exists(ruta & "\" & hdNumeroCarta.Value & ".doc") Then
                Session("RutaCarta") = ruta & "\" & hdNumeroCarta.Value & ".doc"
                EjecutarJS("window.open('frmVisorCarta.aspx?');")
            Else
                AlertaJS(ObtenerMensaje("ALERT126"))
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
        hdNumeroCarta.Value = ""
        dgLista.SelectedIndex = -1
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Static ind = 1
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand'")
            e.Row.Attributes.Add("onclick", "document.getElementById('hdNumeroCarta').value='" & e.Row.Cells(2).Text & "';__doPostBack" & _
                   "('_ctl0$dgLista$_ctl" & _
                   ((Convert.ToInt32(e.Row.RowIndex.ToString())) + 2) & _
                   "$_ctl0','')")
            Dim chkEnviar As CheckBox = e.Row.Cells(8).Controls(1)
            chkEnviar.Enabled = (e.Row.Cells(9).Text = ParametrosSIT.ESTADOCARTA_APROBADA)
        End If
        ind = ind + 1
    End Sub

    Private Function ListaMercados() As String
        Dim cadena As String = ""
        For Each cad As String In ConfigurationManager.AppSettings("Mercados").Split(",") '#ERROR#
            cadena = cadena & cad & ","
        Next
        Return IIf(cadena.Length > 0, cadena.Substring(0, cadena.Length - 1), cadena)
    End Function

    Protected Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
End Class
