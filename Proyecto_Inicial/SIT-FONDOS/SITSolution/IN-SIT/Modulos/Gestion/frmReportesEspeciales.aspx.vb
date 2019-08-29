Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Runtime.InteropServices.Marshal
Imports System.Text
Imports System.Data
Imports Microsoft.Office.Core
Imports System.Globalization
Imports System.Threading
Imports UIUtility
Partial Class Modulos_Gestion_frmReportesEspeciales
    Inherits BasePage
    Private Const S_POSMON_CAJA As String = "PosicionMonedaCaja"
    Private Const VS_Moneda As String = "Moneda"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oVectorPrecio As New VectorPrecioBM
        Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
        Dim fecha As String
        Dim ds As DataTable
        Select Case Request.QueryString("RPT").ToString
            Case "PRE"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Vector de Precios"
                lblFechaDsc1.InnerText = "Fecha"
                divRadioButtons.Attributes.Remove("class")
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
                divFechaValoracion2.Attributes.Add("class", "hidden")
            Case "VECVAR"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Vector Variación"
                lblFechaDsc1.InnerText = "Fecha"
                lbPeriodoDias.InnerText = "Periodo Dias"
                divRadioButtons.Attributes.Remove("class")
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
                divPeriodoDias.Attributes.Add("class", "col-md-3")
                divFechaValoracion2.Attributes.Add("class", "hidden")
            Case "UNI"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Unidades Negociadas"
                lblFechaDsc1.InnerText = "Fecha"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
            Case "UNIVAL"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Unidades Valoradas"
                lblFechaDsc1.InnerText = "Fecha"
                Dim oMercado As New MercadoBM
                Dim oTipoRentaBM As New TipoRentaBM
                HelpCombo.LlenarComboBox(ddlMercado, oMercado.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO).Tables(0), "CodigoMercado", "Descripcion", True)
                HelpCombo.LlenarComboBox(ddlTipoRenta, oTipoRentaBM.Listar(DatosRequest).Tables(0), "CodigoRenta", "Descripcion", True)
                rblEscenario.Visible = False
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divTipoMercado.Attributes.Add("class", "col-md-3")
                divTipoRenta.Attributes.Add("class", "col-md-3")
            Case "REPEXT"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Reporte Exterior"
                lblFechaDsc1.InnerText = "Fecha"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
            Case "MON"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Posición Cartera por Moneda"
                lblFechaDsc1.InnerText = "Fecha"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
            Case "CAR"
                lblTitulo.InnerText = "Composición de Cartera"
                rblEscenario.Visible = True
                lblFechaDsc1.InnerText = "Fecha"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divTipoMercado.Attributes.Add("class", "col-md-3")
                ddlMercado.Visible = False
                Label2.Visible = False
            Case "FON3"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Cartera por VPN Local"
                lblFechaDsc1.InnerText = "Fecha Inicio"
                lblFechaDsc2.InnerText = "Fecha Fin"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
            Case "OPE"
                PNportafolio.Attributes.Add("class", "hidden")
                Dim oBM As New UtilDM
                lblTitulo.InnerText = "Ordenes de Inversión"
                lblFechaDsc1.InnerText = "Fecha"

                divFechaDsc1.Attributes.Add("class", "col-md-3")
            Case "ENG"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Engrapado"
                lblFechaDsc1.InnerText = "Fecha Inicio"
                lblFechaDsc2.InnerText = "Fecha Fin"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
            Case "ACCCV"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Compra Venta"
                lblFechaDsc2.Visible = True
                tbFechaValoracion2.Visible = True
                lblFechaDsc1.InnerText = "Fecha Inicio"
                lblFechaDsc2.InnerText = "Fecha Fin"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
                divInstrumento.Attributes.Add("class", "col-md-6")
                lblTipoInstrumento.InnerText = "Tipo de Instrumento"
                CargarCombo()
            Case "UNIXFECHA"
                If (Not IsPostBack) Then
                    PNportafolio.Attributes.Add("class", "hidden")
                    lblTitulo.InnerText = "Reporte Unidades por Fecha"
                    lblFechaDsc1.InnerText = "Fecha Inicio"
                    lblFechaDsc2.InnerText = "Fecha Fin"
                    divFechaDsc1.Attributes.Add("class", "col-md-3")
                    divFechaDsc2.Attributes.Add("class", "col-md-3")
                    divInstrumento.Attributes.Add("class", "col-md-6")
                    lblTipoInstrumento.InnerText = "Portafolio"
                    CargarPortafolio()
                End If
            Case "VECSER"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Vector Serie"
                lblFechaDsc2.Visible = True
                tbFechaValoracion2.Visible = True
                lblFechaDsc1.InnerText = "Fecha Inicio"
                lblFechaDsc2.InnerText = "Fecha Fin"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
                divInstrumento.Attributes.Add("class", "col-md-6")
                lblTipoInstrumento.InnerText = "Tipo de Instrumento"
                CargarCombo()
            Case "AUXMEN"
                If (Not IsPostBack) Then
                    PNportafolio.Attributes.Add("class", "hidden")
                    lblTitulo.InnerText = "Auxiliar Mensual de Inversiones"
                    lblFechaDsc1.InnerText = "Fecha Inicio"
                    lblFechaDsc2.InnerText = "Fecha Fin"
                    divFechaDsc1.Attributes.Add("class", "col-md-3")
                    divFechaDsc2.Attributes.Add("class", "col-md-3")
                    divInstrumento.Attributes.Add("class", "col-md-6")
                    lblTipoInstrumento.InnerText = "Portafolio"
                    CargarPortafolio()
                End If
            Case "DIVREBLIB"
                If (Not IsPostBack) Then
                    PNportafolio.Attributes.Add("class", "hidden")
                    lblTitulo.InnerText = "Dividendos - Rebates -Liberadas Decretadas"
                    lblFechaDsc1.InnerText = "Fecha Inicio"
                    lblFechaDsc2.InnerText = "Fecha Fin"
                    divFechaDsc1.Attributes.Add("class", "col-md-3")
                    divFechaDsc2.Attributes.Add("class", "col-md-3")
                    divInstrumento.Attributes.Add("class", "col-md-6")
                    lblTipoInstrumento.InnerText = "Portafolio"
                    ddlTipoInstrumento.Width = 150
                    CargarPortafolio()
                End If
            Case "VEN"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "RV/NFD"
                lblFechaDsc1.InnerText = "Fecha Inicio"
                lblFechaDsc2.InnerText = "Fecha Fin"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
                divInstrumento.Attributes.Add("class", "col-md-6")
                lblTipoInstrumento.InnerText = "Portafolio"
                ddlTipoInstrumento.Width = 150
                If (Not IsPostBack) Then
                    CargarPortafolio2()
                End If
            Case "LIBCONVER"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Conversión Acciones"
                lblFechaDsc1.InnerText = "Fecha"
                Label3.InnerText = "Intermediario"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divTipoMercado.Attributes.Add("class", "col-md-3")
                divLabel3.Attributes.Add("class", "col-md-3")
                rblEscenario.Attributes.Add("class", "hidden")
                If (Not IsPostBack) Then
                    CargarPortafolio3()
                    UIUtility.CargarIntermediariosOI(Dropdownlist1)
                    Dropdownlist1.Items.RemoveAt(0)
                End If
            Case "REPOPE"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Reporte de Operaciones"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
                lblFechaDsc1.InnerText = "Fecha Inicio"
                lblFechaDsc2.InnerText = "Fecha Fin"
            Case "LCRE"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Líneas de Crédito por Emisor"
                lblFechaDsc1.InnerText = "Fecha"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
            Case "LCNT"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Líneas por Contraparte"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                lblFechaDsc1.InnerText = "Fecha"
            Case "LSTL"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Líneas Settlement"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                lblFechaDsc1.InnerText = "Fecha"
            Case "TRZOI"
                PNportafolio.Attributes.Add("class", "hidden")
                lblTitulo.InnerText = "Trazabilidad de Operaciones"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divFechaDsc2.Attributes.Add("class", "col-md-3")
                lblFechaDsc1.InnerText = "Fecha Inicio"
                lblFechaDsc2.InnerText = "Fecha Fin"
            Case "RENTABILIDAD"
                lblTitulo.InnerText = "Reporte de Rentabilidad"
                PNportafolio.Attributes.Add("class", "hidden")
                lblFechaDsc1.InnerText = "Fecha"
                divFechaDsc1.Attributes.Add("class", "col-md-3")
                divLabel3.Attributes.Add("class", "col-md-3")
                Label3.InnerText = "Portafolio"
                'OT 10328 - 28/04/2017 - Carlos Espejo
                'Descripcion: Se musestran los nuevos controles cuando se presenta el reporte de rentabilidad
                pnGrillaRentabilidad.Visible = True
                btnBuscar.Visible = True
                'OT 10328 Fin
        End Select
        If (Not IsPostBack) Then
            tbPeriodoDias.Text = "5"
            rbtTodos.Checked = True
            tbFechaValoracion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            Select Case Request.QueryString("RPT").ToString
                Case "PRE", "VECVAR", "TRZOI"
                    fecha = oVectorPrecio.SeleccionarUltimoVectorPrecio(DatosRequest)
                    tbFechaValoracion.Text = UIUtility.ConvertirFechaaString(Convert.ToInt32(fecha))
                Case "OPE"
                    Dim oBM As New UtilDM
                    tbFechaValoracion.Text = oBM.RetornarFechaNegocio()
                Case "CAR"
                    Dim oPortafolioBM As New PortafolioBM
                    PNportafolio.Visible = True
                    HelpCombo.LlenarComboBox(ddlportafolio, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "--Todos--")
                Case "RENTABILIDAD"
                    CargarPortafolio4()
                Case Else
                    ds = oCarteraTituloValoracion.UltimaValoracion(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), DatosRequest).Tables(0)
                    fecha = ds.Rows(0).Item(0)
                    tbFechaValoracion.Text = UIUtility.ConvertirFechaaString(Convert.ToInt32(fecha))
            End Select
            tbFechaValoracion2.Text = tbFechaValoracion.Text
        End If
    End Sub
    Private Sub CargarCombo()
        Dim tablaTipoInstrumento As New DataTable
        Dim oTipoInstrumento As New TipoInstrumentoBM
        tablaTipoInstrumento = oTipoInstrumento.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlTipoInstrumento, tablaTipoInstrumento, "CodigoTipoInstrumento", "Descripcion", True)
    End Sub
    Private Sub ReporteConsolidadoVectorVariacion(ByVal fecha As String, ByVal tipoRpt As String, ByVal periodoDias As String)
        Dim Variable As String = "TmpCodigoUsuario,TmpFecha,TmpTipoRpt,TmpPeriodoDias"
        Dim parametros As String = Usuario & "," & fecha & "," & tipoRpt & "," & periodoDias
        Dim obj As New JobBM
        Dim mensaje As String = obj.EjecutarJob("DTS_SIT_ReporteVectorVariacion" & DateTime.Today.ToString("_yyyyMMdd") & System.DateTime.Now.ToString("_hhmmss"), _
        "Genera el reporte vector variacion", Variable, parametros, "", "", ConfigurationManager.AppSettings("SERVIDORETL"))
        AlertaJS(mensaje)
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim ds As DataSet
        Try
            Select Case Request.QueryString("RPT").ToString
                Case "PRE"
                    ds = New ReporteGestionBM().ListaPrecios2(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), IIf(rbtTodos.Checked = True, "TODOS", "AFP"), _
                    DatosRequest)
                    Copia("VectorPrecio", ds.Tables(0))
                Case "VECVAR"
                    Dim periodoDias As Int32 = Convert.ToInt32(Val(tbPeriodoDias.Text))
                    ReporteConsolidadoVectorVariacion(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text).ToString(), IIf(rbtTodos.Checked = True, "TODOS", "AFP"), _
                    periodoDias.ToString())
                Case "CAR"
                    ds = New ReporteGestionBM().ComposicionCartera(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), rblEscenario.SelectedValue, _
                    ddlportafolio.SelectedValue, DatosRequest)
                    If ddlportafolio.SelectedValue = "" Then
                        Copia_Fondo("ComposicionCartera" & rblEscenario.SelectedValue, ds.Tables(0))
                    Else
                        Copia_Fondo("ComposicionCartera" & rblEscenario.SelectedValue, ds.Tables(0))
                    End If
                Case "MON"
                    ds = New ReporteGestionBM().MonedaGestion(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), DatosRequest)
                    Copia("Moneda", ds.Tables(0))
                Case "UNI"
                    ds = New ReporteGestionBM().CarteraUnidades(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), "UNI", DatosRequest, "", "")
                    Copia("CarteraUnidades", ds.Tables(0))
                Case "UNIVAL"
                    ds = New ReporteGestionBM().CarteraUnidades(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), "UNIVAL", DatosRequest, ddlMercado.SelectedValue, _
                    ddlTipoRenta.SelectedValue)
                    Copia("CarteraUnidades", ds.Tables(0))
                Case "FON3"
                    If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text) Then
                        AlertaJS(ObtenerMensaje("ALERT48"))
                        Exit Sub
                    End If
                    ds = New ReporteGestionBM().CarteraFondo3(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text), DatosRequest)
                    Copia("VPNLocal", ds.Tables(0))
                Case "OPE"
                    ds = New ReporteGestionBM().CarteraOperacion(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), DatosRequest)
                    Copia("CarteraOperacion", ds.Tables(0))
                Case "ENG"
                    If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text) Then
                        AlertaJS(ObtenerMensaje("ALERT48"))
                        Exit Sub
                    End If
                    ds = New ReporteGestionBM().CarteraEngrapado(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text), DatosRequest)
                    Copia("VectorHistorico", ds.Tables(0))
                Case "ACCCV"
                    If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text) Then
                        AlertaJS(ObtenerMensaje("ALERT48"))
                        Exit Sub
                    End If
                    ds = New ReporteGestionBM().CompraVenta(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text), ddlTipoInstrumento.SelectedValue, DatosRequest)
                    Copia("CompraVenta", ds.Tables(0))
                Case "VECSER"
                    If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text) Then
                        AlertaJS(ObtenerMensaje("ALERT48"))
                        Exit Sub
                    End If
                    ds = New ReporteGestionBM().VectorSerie(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text), ddlTipoInstrumento.SelectedValue, DatosRequest)
                    Copia("VectorSerie", ds.Tables(0))
                Case "UNIXFECHA"
                    If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text) Then
                        AlertaJS(ObtenerMensaje("ALERT48"))
                        Exit Sub
                    End If
                    ds = New ReporteGestionBM().UnidadesxFecha(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text), ddlTipoInstrumento.SelectedValue, DatosRequest)
                    Copia(ddlTipoInstrumento.SelectedItem.Text, ds.Tables(0))
                Case "AUXMEN"
                    If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text) Then
                        AlertaJS(ObtenerMensaje("ALERT48"))
                        Exit Sub
                    End If
                    ds = New ReporteGestionBM().GenerarReporteUtilidad(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text), ddlTipoInstrumento.SelectedValue, DatosRequest)
                    Copia(ddlTipoInstrumento.SelectedItem.Text, ds.Tables(0))
                Case "DIVREBLIB"
                    If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) > UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text) Then
                        AlertaJS(ObtenerMensaje("ALERT48"))
                        Exit Sub
                    End If
                    ds = New ReporteGestionBM().DividendosRebatesLiberadas(ddlTipoInstrumento.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text))
                    Copia(ddlTipoInstrumento.SelectedItem.Text, ds.Tables(0))
                Case "REPEXT"
                    ds = New ReporteGestionBM().ReporteExterior(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), DatosRequest)
                    Copia("ReporteExterior", ds.Tables(0))
                Case "VEN"
                    Dim dsCuentasPorCobrar As New CuentasPorCobrarPagarBE
                    Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow
                    cuentaPorCobrar.CodigoMercado = ""
                    cuentaPorCobrar.CodigoMoneda = ""
                    cuentaPorCobrar.CodigoTercero = ""
                    cuentaPorCobrar.CodigoOperacion = ""
                    cuentaPorCobrar.CodigoPortafolioSBS = ddlTipoInstrumento.SelectedValue
                    dsCuentasPorCobrar.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(cuentaPorCobrar)
                    ds = New CuentasPorCobrarBM().SeleccionarVencimientos(dsCuentasPorCobrar, "", UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text), DatosRequest)
                    Copia("ReporteVencimiento", ds.Tables(0))
                Case "LIBCONVER"
                    ds = New ReporteGestionBM().ConversionAcciones(ddlMercado.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    Dropdownlist1.SelectedValue)
                    Copia("ConversionAccion", ds.Tables(0))
                Case "POSMON"
                    ds = New ReporteGestionBM().PosicionMoneda(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text))
                    Session(S_POSMON_CAJA) = ds.Tables(1)
                    Copia("PosicionMoneda", ds.Tables(0))
                Case "REPOPE"
                    ds = New ReporteGestionBM().OperacionInversion(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text))
                    Copia("OperacionInversion", ds.Tables(0), ds.Tables(1))
                Case "LCRE"
                    ds = New ReporteGestionBM().LineasCreditoxEmisor(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text))
                    If ds.Tables(0).Rows.Count <= 0 Then
                        AlertaJS("No hay líneas de crédito cargadas, favor de verificar.")
                    Else
                        Copia("LineasCredito", ds.Tables(0), ds.Tables(1))
                    End If
                Case "LCNT"
                    ds = New ReporteGestionBM().LineasContraparte(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text))
                    If ds.Tables(0).Rows.Count <= 0 Then
                        AlertaJS("No hay líneas de contraparte cargadas, favor de verificar.")
                    Else
                        Copia("LineasContraparte", ds.Tables(0))
                    End If
                Case "LSTL"
                    ds = New ReporteGestionBM().LineasSettlement(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text))
                    If ds.Tables(0).Rows.Count <= 0 Then
                        AlertaJS("No hay líneas de settlement cargadas, favor de verificar.")
                    Else
                        Copia("LineasSettlement", ds.Tables(0))
                    End If
                Case "TRZOI"
                    ds = New PrevOrdenInversionBM().TrazabilidadOperaciones(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text))
                    If ds.Tables(0).Rows.Count <= 0 Then
                        AlertaJS("No hay Trazabilidad en el rango de fechas, favor de verificar.")
                    Else
                        Copia("TrazabilidadOperaciones", ds.Tables(0), ds.Tables(1))
                    End If
                Case "RENTABILIDAD"
                    Dim dt As DataTable
                    dt = New ReporteGestionBM().reporteRentabilidad(Dropdownlist1.SelectedValue, ConvertirFechaaDecimal(Me.tbFechaValoracion.Text), "A")
                    copiaNotepad("Rentabilidad", dt)
            End Select
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Public Sub Copia_Fondo(ByVal Archivo As String, ByVal dt As DataTable, Optional ByVal dtPortafolio As DataTable = Nothing)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim sRutaArchivo As String, sTemplate As String
        Dim nombreArchivo As String
        Dim sufijo As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            nombreArchivo = Archivo & "_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".xls"
            sRutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
            sufijo = Request.QueryString("RPT")
            sTemplate = RutaPlantillas() & "\Plantilla" & sufijo & ".xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            Dim i As Integer
            If ddlportafolio.SelectedValue = "" Then
                For Each lt As ListItem In ddlportafolio.Items
                    If lt.Value <> "" Then
                        oBook.Sheets(1).Copy(after:=oBook.Sheets(i))
                        oSheet = oBook.Sheets(i + 1)
                        oSheet.Name = lt.Text
                        oCells = oSheet.Cells
                        DumpData_Fondos(dt, 3, oSheet, oCells, lt.Text)
                        oCells.EntireColumn.AutoFit()
                    End If
                    i += 1
                Next
                oBook.Worksheets(1).Delete()
            Else
                oBook.Sheets(1).Copy(after:=oBook.Sheets(1))
                oSheet = oBook.Sheets(1)
                oSheet.Name = ddlportafolio.SelectedItem.Text
                oCells = oSheet.Cells
                DumpData_Fondos(dt, 3, oSheet, oCells, ddlportafolio.SelectedItem.Text)
                oCells.EntireColumn.AutoFit()
                oBook.Worksheets(2).Delete()
            End If
            oSheet.SaveAs(sRutaArchivo)
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & nombreArchivo)
            Response.WriteFile(sRutaArchivo)
            Response.End()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
    Public Sub Copia(ByVal Archivo As String, ByVal dt As DataTable, Optional ByVal dtPortafolio As DataTable = Nothing)
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oldCulture As CultureInfo = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Dim sRutaArchivo As String, sTemplate As String
        Dim nombreArchivo As String
        Dim sufijo As String
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            oldCulture = Thread.CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
            nombreArchivo = Archivo & "_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".xls"
            sRutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
            sufijo = Request.QueryString("RPT")
            If sufijo.Equals("UNIVAL") Or sufijo.Equals("REPEXT") Then
                sufijo = "UNI"
            End If
            sTemplate = RutaPlantillas() & "\Plantilla" & sufijo & ".xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oSheet.Name = Archivo
            oCells = oSheet.Cells
            DumpData(dt, oSheet, oCells, dtPortafolio)

            oSheet.SaveAs(sRutaArchivo)
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & nombreArchivo)
            Response.WriteFile(sRutaArchivo)
            Response.End()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
    Public Sub copiaNotepad(ByVal Archivo As String, ByVal dt As DataTable, Optional ByVal dtPortafolio As DataTable = Nothing)
        Dim sRutaArchivo As String
        Dim nombreArchivo As String
        Dim sufijo As String
        nombreArchivo = Archivo & "_" & Dropdownlist1.SelectedItem.Text & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & System.DateTime.Now.ToString("_hhmmss") & ".txt"
        sRutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & nombreArchivo
        sufijo = Request.QueryString("RPT")
        escribirArchivo(sufijo, sRutaArchivo, dt)
        System.GC.Collect()
        Response.Clear()
        Response.ContentType = "application/txt"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & nombreArchivo)
        Response.WriteFile(sRutaArchivo)
        Response.End()
    End Sub
    Sub DumpData_Fondos(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range, Portafolio As String)
        Dim rows As DataRow()
        rows = dt.Select("Portafolio = '" + Portafolio + "'")
        For Each dr As DataRow In rows
            Dim i As Integer = 0
            Do While i <= dt.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                i = i + 1
            Loop
            FilaInicial += 1
        Next
    End Sub
    Private Sub DumpData(ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByRef oCells As Excel.Range, Optional ByVal dtPortafolio As DataTable = Nothing)
        Dim dr As DataRow, ary() As Object
        Dim iRow As Integer, iCol As Integer
        Select Case Request.QueryString("RPT").ToString
            Case "FON3"
                For iCol = 0 To dt.Columns.Count - 1
                    oCells(1, iCol + 1) = dt.Columns(iCol).ToString
                Next
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        oCells(iRow + 2, iCol + 1) = ary(iCol).ToString
                    Next
                Next
            Case "ENG"
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        oCells(iRow + 1, iCol + 1) = ary(iCol).ToString
                    Next
                Next
            Case "UNIXFECHA"
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        If (iCol = 256) Then
                            Exit For
                        End If
                        If (ary(iCol).ToString().Contains("_")) Then
                            oCells(iRow + 1, iCol + 1) = ary(iCol).ToString().Substring(0, ary(iCol).ToString().IndexOf("_"))
                        Else
                            oCells(iRow + 1, iCol + 1) = ary(iCol).ToString()
                        End If
                    Next
                Next
            Case "CAR"
                For iCol = 0 To dt.Columns.Count - 1
                    oCells(1, iCol + 1) = dt.Columns(iCol).ToString
                Next
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        oCells(iRow + 2, iCol + 1) = ary(iCol).ToString
                    Next
                Next
                oCells.EntireColumn.AutoFit()
            Case "ACCCV"
                For iCol = 0 To dt.Columns.Count - 1
                    oCells(1, iCol + 1) = dt.Columns(iCol).ToString
                Next
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        oCells(iRow + 2, iCol + 1) = ary(iCol).ToString
                    Next
                Next
                oSheet.Columns().AutoFit()
            Case "AUXMEN"
                For iRow = 0 To dt.Rows.Count - 1
                    oCells(iRow + 3, 1) = dt.Rows(iRow)("CodigoPortafolioSBS")
                    oCells(iRow + 3, 2) = dt.Rows(iRow)("TipoInstrumento")
                    oCells(iRow + 3, 3) = dt.Rows(iRow)("CodigoNemonico")
                    oCells(iRow + 3, 4) = dt.Rows(iRow)("Saldo")
                    oCells(iRow + 3, 5) = dt.Rows(iRow)("Compra")
                    oCells(iRow + 3, 6) = dt.Rows(iRow)("Venta")
                    oCells(iRow + 3, 7) = dt.Rows(iRow)("Vencimiento")
                    oCells(iRow + 3, 8) = dt.Rows(iRow)("Cuponera")
                    oCells(iRow + 3, 9) = CDec(-CDec(dt.Rows(iRow)("Saldo")) - CDec(dt.Rows(iRow)("Compra")) + dt.Rows(iRow)("Venta") + _
                    CDec(dt.Rows(iRow)("Vencimiento")) + CDec(dt.Rows(iRow)("Cuponera") + dt.Rows(iRow)("VPN")))
                    oCells(iRow + 3, 10) = dt.Rows(iRow)("VPN")
                Next
            Case "REPEXT"
                oCells(1, 1) = tbFechaValoracion.Text.ToString
                For iCol = 0 To dt.Columns.Count - 1
                    oCells(2, iCol + 1) = dt.Columns(iCol).ToString
                Next
                For iRow = 0 To dt.Rows.Count - 1
                    For iCol = 0 To dt.Columns.Count - 1
                        oCells(iRow + 3, iCol + 1) = dt.Rows(iRow)(iCol)
                    Next
                Next
            Case "DIVREBLIB"
                Dim EsLiberada As Boolean = False
                Dim iRowExcel As Integer = 3
                Dim iRowNemonicoInicio As Integer = 3
                Dim iRowMenonicoFin As Integer = 3
                Dim SubTotales As New ArrayList
                For iRow = 0 To dt.Rows.Count - 1
                    If iRow > 0 Then
                        If (dt.Rows(iRow)("CodigoNemonico") <> dt.Rows(iRow - 1)("CodigoNemonico")) Then
                            iRowMenonicoFin = iRowExcel - 1
                            oCells(iRowExcel, 2) = "Total " & dt.Rows(iRow - 1)("CodigoNemonico")
                            oCells(iRowExcel, 2).Font.Bold = True

                            If EsLiberada Then
                                oCells(iRowExcel, 11).Formula = String.Format("=SUM(K{0}:K{1})", iRowNemonicoInicio, iRowMenonicoFin)
                                SubTotales.Add("K" & iRowExcel)
                                oCells(iRowExcel, 11).Font.Bold = True
                            Else
                                oCells(iRowExcel, 9).Formula = String.Format("=SUM(I{0}:I{1})", iRowNemonicoInicio, iRowMenonicoFin)
                                SubTotales.Add("I" & iRowExcel)
                                oCells(iRowExcel, 9).Font.Bold = True
                            End If
                            iRowExcel = iRowExcel + 1
                            iRowNemonicoInicio = iRowExcel
                        End If
                        If dt.Rows(iRow)("Tipo") <> dt.Rows(iRow - 1)("Tipo") Then
                            iRowNemonicoInicio = iRowNemonicoInicio + 1
                            Dim RangoSubTotales As String = String.Empty
                            Dim i As Integer
                            For i = 0 To SubTotales.Count - 1
                                RangoSubTotales = String.Concat(RangoSubTotales, "+", Convert.ToString(SubTotales(i)))
                            Next
                            oCells(iRowExcel, 1) = "Total " & dt.Rows(iRow - 1)("Tipo")
                            oCells(iRowExcel, 1).Font.Bold = True
                            If EsLiberada Then
                                oCells(iRowExcel, 11).Formula = String.Format("=SUM({0})", RangoSubTotales)
                                oCells(iRowExcel, 11).Font.Bold = True
                            Else
                                oCells(iRowExcel, 9).Formula = String.Format("=SUM({0})", RangoSubTotales)
                                oCells(iRowExcel, 9).Font.Bold = True
                            End If
                            iRowExcel = iRowExcel + 1
                            SubTotales.Clear()
                        End If
                    End If
                    oCells(iRowExcel, 1) = dt.Rows(iRow)("Tipo")
                    oCells(iRowExcel, 2) = dt.Rows(iRow)("CodigoNemonico")
                    oCells(iRowExcel, 3) = dt.Rows(iRow)("FechaCorte")
                    oCells(iRowExcel, 4) = dt.Rows(iRow)("FechaIDI")
                    oCells(iRowExcel, 5) = dt.Rows(iRow)("FechaPago")
                    oCells(iRowExcel, 6) = dt.Rows(iRow)("Unidades")
                    oCells(iRowExcel, 7) = dt.Rows(iRow)("Factor")
                    oCells(iRowExcel, 8) = dt.Rows(iRow)("CodigoMoneda")
                    If (dt.Rows(iRow)("Tipo") = "Liberadas") Then
                        oCells(iRowExcel, 11) = dt.Rows(iRow)("Importe")
                        EsLiberada = True
                    Else
                        oCells(iRowExcel, 9) = dt.Rows(iRow)("Importe")
                        EsLiberada = False
                    End If
                    oCells(iRowExcel, 10) = dt.Rows(iRow)("CodigoCustodio")
                    iRowExcel = iRowExcel + 1
                    If iRow > 0 Then
                        If (iRow = dt.Rows.Count - 1) Then
                            iRowMenonicoFin = iRowExcel - 1
                            oCells(iRowExcel, 2) = "Total " & dt.Rows(iRow)("CodigoNemonico")
                            oCells(iRowExcel, 9).Formula = String.Format("=SUM(I{0}:I{1})", iRowNemonicoInicio, iRowMenonicoFin)
                            SubTotales.Add("I" & iRowExcel)

                            oCells(iRowExcel, 9).Formula = String.Format("=SUM(I{0}:I{1})", iRowNemonicoInicio, iRowMenonicoFin)
                            oCells(iRowExcel, 2).Font.Bold = True
                            oCells(iRowExcel, 9).Font.Bold = True
                            iRowExcel = iRowExcel + 1
                            iRowNemonicoInicio = iRowExcel
                        End If
                        If iRow = dt.Rows.Count - 1 Then
                            Dim RangoSubTotales As String = String.Empty
                            Dim i As Integer
                            For i = 0 To SubTotales.Count - 1
                                RangoSubTotales = String.Concat(RangoSubTotales, "+", Convert.ToString(SubTotales(i)))
                            Next
                            oCells(iRowExcel, 1) = "Total " & dt.Rows(iRow)("Tipo")
                            oCells(iRowExcel, 9).Formula = String.Format("=SUM({0})", RangoSubTotales)
                            oCells(iRowExcel, 1).Font.Bold = True
                            oCells(iRowExcel, 9).Font.Bold = True
                            iRowExcel = iRowExcel + 1
                            SubTotales.Clear()
                        End If
                    End If
                Next
            Case "VEN"
                Dim rows() As DataRow = dt.Select("DescripcionPortafolio<>'ADMINISTRA'", "DescripcionPortafolio asc ,DescripcionMoneda asc")
                Dim i As Integer = 0
                For Each row As DataRow In rows
                    If (row("Referencia") <> "DIVISA" And row("Referencia") <> "FORWARD") Then
                        oCells(i + 3, 1) = row("DescripcionPortafolio")
                        oCells(i + 3, 2) = row("DescripcionMoneda")
                        oCells(i + 3, 3) = row("FechaOperacion")
                        oCells(i + 3, 4) = row("FechaVencimiento")
                        oCells(i + 3, 5) = row("NroOperacion")
                        oCells(i + 3, 6) = row("DescripcionOperacion")
                        oCells(i + 3, 7) = row("Referencia")
                        oCells(i + 3, 8) = row("Importe")
                        oCells(i + 3, 9) = row("DescripcionIntermediario")
                        oCells(i + 3, 10) = row("NumeroPoliza")
                        i = i + 1
                    End If
                Next
            Case "LIBCONVER"
                Dim FORMATO_DOLLAR As String = "[$$-409]#,##0.00"
                Dim FORMATO_LIBRA As String = "[$£-809]#,##0.0000000"
                Dim codigooperacion As String = String.Empty
                Dim numeroPoliza As String = String.Empty
                Dim oTipoCambio As New VectorTipoCambioBM
                oCells(1, 2) = tbFechaValoracion.Text
                oCells(2, 2) = Dropdownlist1.SelectedItem.Text
                oCells(3, 2) = ddlMercado.SelectedItem.Text
                oCells(6, 4) = oTipoCambio.SeleccionarTipoCambio(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), "REAL", "DOL")
                oCells(7, 4) = oTipoCambio.SeleccionarTipoCambio(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), "REAL", "GBP")
                iCol = 2
                iRow = 18
                If Not dt.Rows.Count > 0 Then
                    Exit Select
                End If
                'primera cabecera de operacion Compra o Venta con su Poliza
                oCells(iRow, iCol) = IIf(dt.Rows(0)("codigooperacion") = 1, "COMPRA", "VENTA")
                oCells(iRow + 2, iCol) = dt.Rows(0)("numeroPoliza")
                codigooperacion = dt.Rows(0)("codigooperacion")
                numeroPoliza = dt.Rows(0)("numeroPoliza")
                iRow = 21
                Dim flag As Boolean = True
                Dim iRowEstaticaMonto As Integer = 21
                For Each row As DataRow In dt.Rows
                    If codigooperacion <> row("codigooperacion") Then
                        numeroPoliza = row("numeroPoliza")
                        codigooperacion = row("codigooperacion")
                        iRow = iRow + 15
                        iCol = 2
                        oCells(iRow - 2, iCol - 1) = "Operaciones Maple"
                        oCells(iRow - 2, iCol) = IIf(row("codigooperacion") = 1, "COMPRA", "VENTA")
                        iRow = iRow + 1
                        oCells(iRow - 1, iCol) = row("numeroPoliza")
                        oCells(iRow - 1, iCol - 1) = "Nro Poliza"
                        oCells(iRow, iCol - 1) = "Acciones"
                        oCells(iRow + 1, iCol - 1) = "Precio"
                        oCells(iRow + 2, iCol - 1) = "Precio en Libras"
                        oCells(iRow + 3, iCol - 1) = "Total en Dolares"
                        oCells(iRow + 4, iCol - 1) = "Total en Libras"
                        oCells(iRow + 6, iCol - 1) = "Total General Dolares"
                        oCells(iRow + 7, iCol - 1) = "Total General Libras"
                        iRowEstaticaMonto = iRow
                        oCells(iRow, iCol) = row("MontoOperacion")
                        oCells(iRow + 1, iCol) = row("Precio")
                        oCells(iRow + 2, iCol) = String.Format("=TRUNCAR({0}*$C$14,7)", HelpExcel.LetraColumna(iCol) & iRow + 1)
                        oCells(iRow + 3, iCol) = String.Format("={0}*{1}", HelpExcel.LetraColumna(iCol) & iRow, HelpExcel.LetraColumna(iCol) & iRow + 1)
                        oCells(iRow + 4, iCol) = row("MontoDestino")
                        oCells(iRow + 6, iCol) = String.Format("=Suma({0}:{1})", "B" & iRow + 3, "Z" & iRow + 3)
                        oCells(iRow + 7, iCol) = String.Format("=Suma({0}:{1})", "B" & iRow + 4, "Z" & iRow + 4)
                        oCells(iRow + 6, iCol + 1) = String.Format("={0}*D6", "B" & iRow + 6)
                        oCells(iRow + 7, iCol + 1) = String.Format("={0}*D7", "B" & iRow + 7)
                        oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 1)).NumberFormat = FORMATO_DOLLAR
                        oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 2)).NumberFormat = FORMATO_LIBRA
                        oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 3)).NumberFormat = FORMATO_DOLLAR
                        oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 4)).NumberFormat = FORMATO_LIBRA
                        oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 6)).NumberFormat = FORMATO_DOLLAR
                        oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 7)).NumberFormat = FORMATO_LIBRA
                        iRow = iRowEstaticaMonto
                        iCol = iCol + 1
                    Else
                        If numeroPoliza <> row("numeroPoliza") Then
                            numeroPoliza = row("numeroPoliza")
                            iRow = iRow + 10
                            iCol = 2
                            iRowEstaticaMonto = iRow
                            oCells(iRow - 1, iCol) = row("numeroPoliza")
                            oCells(iRow - 1, iCol - 1) = "Nro Poliza"
                            oCells(iRow, iCol - 1) = "Acciones"
                            oCells(iRow + 1, iCol - 1) = "Precio"
                            oCells(iRow + 2, iCol - 1) = "Precio en Libras"
                            oCells(iRow + 3, iCol - 1) = "Total en Dolares"
                            oCells(iRow + 4, iCol - 1) = "Total en Libras"
                            oCells(iRow + 6, iCol - 1) = "Total General Dolares"
                            oCells(iRow + 7, iCol - 1) = "Total General Libras"
                            oCells(iRow + 6, iCol) = String.Format("=Suma({0}:{1})", "B" & iRow + 3, "Z" & iRow + 3)
                            oCells(iRow + 7, iCol) = String.Format("=Suma({0}:{1})", "B" & iRow + 4, "Z" & iRow + 4)
                            oCells(iRow + 6, iCol + 1) = String.Format("={0}*D6", "B" & iRow + 6)
                            oCells(iRow + 7, iCol + 1) = String.Format("={0}*D7", "B" & iRow + 7)
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 6)).NumberFormat = FORMATO_DOLLAR
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 7)).NumberFormat = FORMATO_LIBRA
                            oCells(iRow, iCol) = row("MontoOperacion")
                            oCells(iRow + 1, iCol) = row("Precio")
                            oCells(iRow + 2, iCol) = String.Format("=TRUNCAR({0}*$C$14,7)", HelpExcel.LetraColumna(iCol) & iRow + 1)
                            oCells(iRow + 3, iCol) = String.Format("={0}*{1}", HelpExcel.LetraColumna(iCol) & iRow, HelpExcel.LetraColumna(iCol) & iRow + 1)
                            oCells(iRow + 4, iCol) = row("MontoDestino")
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 1)).NumberFormat = FORMATO_DOLLAR
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 2)).NumberFormat = FORMATO_LIBRA
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 3)).NumberFormat = FORMATO_DOLLAR
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 4)).NumberFormat = FORMATO_LIBRA
                            iRow = iRowEstaticaMonto
                            iCol = iCol + 1
                        Else
                            If flag Then
                                oCells(iRow - 1, iCol) = row("numeroPoliza")
                                oCells(iRow - 1, iCol - 1) = "Nro Poliza"
                                oCells(iRow, iCol - 1) = "Acciones"
                                oCells(iRow + 1, iCol - 1) = "Precio"
                                oCells(iRow + 2, iCol - 1) = "Precio en Libras"
                                oCells(iRow + 3, iCol - 1) = "Total en Dolares"
                                oCells(iRow + 4, iCol - 1) = "Total en Libras"
                                oCells(iRow + 6, iCol - 1) = "Total General Dolares"
                                oCells(iRow + 7, iCol - 1) = "Total General Libras"
                                oCells(iRow + 6, iCol) = String.Format("=Suma({0}:{1})", "B" & iRow + 3, "Z" & iRow + 3)
                                oCells(iRow + 7, iCol) = String.Format("=Suma({0}:{1})", "B" & iRow + 4, "Z" & iRow + 4)
                                oCells(iRow + 6, iCol + 1) = String.Format("={0}*D6", "B" & iRow + 6)
                                oCells(iRow + 7, iCol + 1) = String.Format("={0}*D7", "B" & iRow + 7)
                                oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 6)).NumberFormat = FORMATO_DOLLAR
                                oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 7)).NumberFormat = FORMATO_LIBRA
                                flag = False
                            End If
                            oCells(iRow, iCol) = row("MontoOperacion")
                            oCells(iRow + 1, iCol) = row("Precio")
                            oCells(iRow + 2, iCol) = String.Format("=TRUNCAR({0}*$C$14,7)", HelpExcel.LetraColumna(iCol) & iRow + 1)
                            oCells(iRow + 3, iCol) = String.Format("={0}*{1}", HelpExcel.LetraColumna(iCol) & iRow, HelpExcel.LetraColumna(iCol) & iRow + 1)
                            oCells(iRow + 4, iCol) = row("MontoDestino")
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 1)).NumberFormat = FORMATO_DOLLAR
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 2)).NumberFormat = FORMATO_LIBRA
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 3)).NumberFormat = FORMATO_DOLLAR
                            oCells.Range(String.Concat(HelpExcel.LetraColumna(iCol), iRow + 4)).NumberFormat = FORMATO_LIBRA
                            iRow = iRowEstaticaMonto
                            iCol = iCol + 1
                        End If
                    End If
                Next
            Case "POSMON"
                Dim oMonedaBM As New MonedaBM
                ViewState(VS_Moneda) = oMonedaBM.Listar("A").Tables(0)
                Dim col As Integer
                oCells(1, 1) = tbFechaValoracion.Text.ToString
                For iCol = 0 To dt.Columns.Count - 1
                    oCells(4, iCol + 1) = dt.Columns(iCol).ToString
                    col = col + 1
                Next
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        oCells(iRow + 5, iCol + 1) = ary(iCol).ToString
                    Next
                Next
                If Session(S_POSMON_CAJA) Is Nothing Then
                    Exit Select
                End If
                Dim dtPosMonCaja As DataTable
                dtPosMonCaja = CType(Session(S_POSMON_CAJA), DataTable)
                Dim oTipoCambioBM As New TipoCambioBM
                For iCol = 0 To dtPosMonCaja.Columns.Count - 1
                    oCells(4, col + 5 + iCol) = dtPosMonCaja.Columns(iCol).ToString
                    If iCol > 0 Then
                        oCells(3, col + 5 + iCol) = oTipoCambioBM.SeleccionarValorTCOrigen_TCDestinoXEntidad(dtPosMonCaja.Columns(iCol).ToString, "DOL", _
                        UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), "REAL")
                    End If
                Next
                Dim CajaExtranjeroDolar As String = String.Empty
                For iRow = 0 To dtPosMonCaja.Rows.Count - 1
                    dr = dtPosMonCaja.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        oCells(iRow + 5, col + 5 + iCol) = ary(iCol).ToString
                        If iCol > 0 Then
                            CajaExtranjeroDolar = CajaExtranjeroDolar & String.Format("({0}{1}{2})", HelpExcel.LetraColumna(col + 5 + iCol) & (iRow + 5), _
                            TipoCalculo(dtPosMonCaja.Columns(iCol).ToString), HelpExcel.LetraColumna(col + 5 + iCol) & "3") & IIf(iCol < ary.Length - 1, "+", "")
                        End If
                    Next
                    oCells(iRow + 5, col + 4) = "=" & CajaExtranjeroDolar
                    CajaExtranjeroDolar = ""
                Next
                Dim iColCabBBH As Integer
                Dim iColBBH As Integer
                iColBBH = col + 5
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 1 To UBound(ary)
                        For iColCabBBH = 1 To dtPosMonCaja.Columns.Count - 1
                            If dt.Columns(iCol).ToString = dtPosMonCaja.Columns(iColCabBBH).ToString Then
                                oCells(iRow + 5, iCol + 1) = String.Format("= {0}+ ({1}{2}{3})*{4}", ary(iCol).ToString, HelpExcel.LetraColumna(iColBBH + iColCabBBH) & _
                                (iRow + 5), TipoCalculo(dt.Columns(iCol).ToString), HelpExcel.LetraColumna(iColBBH + iColCabBBH) & "3", "$P$12")
                                Exit For
                            End If
                        Next
                    Next
                Next
                Dim oVectorTipoCambioBM As New VectorTipoCambioBM
                oCells(12, 16) = oVectorTipoCambioBM.SeleccionarTipoCambio(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), "REAL", "DOL")
                'CAJA , Local + Extranjera
                Dim oReporteGestionBM As New ReporteGestionBM
                Dim dtPosMonCajaLocal As DataTable
                dtPosMonCajaLocal = oReporteGestionBM.PosicionMonedaCajaLocal(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text))
                For iCol = 0 To dtPosMonCajaLocal.Columns.Count - 1
                    oCells(11, iCol + 1) = dtPosMonCajaLocal.Columns(iCol).ToString
                Next
                For iRow = 0 To dtPosMonCajaLocal.Rows.Count - 1
                    dr = dtPosMonCajaLocal.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        If dtPosMonCajaLocal.Columns(iCol).ToString = "DOL (PEN)" Then
                            oCells(iRow + 12, iCol + 1) = "=" & ary(iCol).ToString & "*$P$12 +" & HelpExcel.LetraColumna(iColBBH - 1) & (iRow + 5).ToString & "*$P$12"
                        Else
                            oCells(iRow + 12, iCol + 1) = ary(iCol).ToString
                        End If
                    Next
                Next
                'FORWARD
                Dim dtForward As DataTable
                dtForward = oReporteGestionBM.PosicionMonedaForward(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text))
                For iCol = 0 To dtForward.Columns.Count - 1
                    oCells(18, iCol + 1) = dtForward.Columns(iCol).ToString
                Next
                For iRow = 0 To dtForward.Rows.Count - 1
                    dr = dtForward.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        If dtForward.Columns(iCol).ToString = "NSOL" Then
                            oCells(iRow + 19, iCol + 1) = "=" & HelpExcel.LetraColumna(iCol) & (iRow + 19).ToString & "*$P$12"
                        Else
                            oCells(iRow + 19, iCol + 1) = ary(iCol).ToString
                        End If
                    Next
                Next
                Session(S_POSMON_CAJA) = Nothing
                ViewState(VS_Moneda) = Nothing
            Case "REPOPE"
                Dim _fila As Integer
                Dim _filaTotal As Integer
                Dim _columna As Integer
                Dim _totPortafolio As Integer
                Dim _instrumento As String
                Dim _instrumentoAnt As String = ""
                Dim _letra As String = ""
                Dim _letra2 As String
                Dim _rango As String = ""
                Dim _filaAnt As Integer = 1
                Dim htPortafolio As New Hashtable
                Dim _cultureInfo As CultureInfo = Thread.CurrentThread.CurrentCulture
                Dim dt2 As DataTable = New ReporteGestionBM().OperacionInversionPatrimonio(UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text), _
                UIUtility.ConvertirFechaaDecimal(tbFechaValoracion2.Text))
                _filaTotal = dt.Rows.Count + SelectCountDistinct(dt, "Descripcion") + 12
                'Generamos la cabecera
                oCells.Range(oCells(1, 1), oCells(1, 3 * (dtPortafolio.Rows.Count + 1))).Merge()
                If tbFechaValoracion.Text = tbFechaValoracion2.Text Then
                    oCells(2, 1) = "Fecha"
                    oCells(2, 2) = tbFechaValoracion.Text
                Else
                    oCells(2, 1) = "Fecha Inicio"
                    oCells(2, 2) = tbFechaValoracion.Text
                    oCells(3, 1) = "Fecha Fin"
                    oCells(3, 2) = tbFechaValoracion2.Text
                End If
                With dtPortafolio
                    _totPortafolio = .Rows.Count
                    oCells(5, 3).value = "Cantidades"
                    oCells.Range(oCells(5, 3), oCells(5, 2 + _totPortafolio)).Merge()
                    For _fila = 0 To _totPortafolio - 1
                        oCells(6, 3 + _fila).value = .Rows(_fila)("Descripcion").ToString
                        oCells(5, 4 + _totPortafolio + (2 * _fila)).value = .Rows(_fila)("Descripcion").ToString
                        oCells(6, 4 + _totPortafolio + (2 * _fila)).value = "S/."
                        oCells(6, 5 + _totPortafolio + (2 * _fila)).value = "%"
                        oCells.Range(oCells(5, 4 + _totPortafolio + (2 * _fila)), oCells(5, 5 + _totPortafolio + (2 * _fila))).Merge()
                        oCells(_filaTotal, 4 + _totPortafolio + (2 * _fila)).value = ObtenerPatrimonio(.Rows(_fila)("CodigoPortafolioSBS").ToString, dt2)
                        oCells(_filaTotal, 4 + _totPortafolio + (2 * _fila)).NumberFormat = String.Format("#{0}##0", _cultureInfo.NumberFormat.CurrencyGroupSeparator)
                        'Guardamos la posicion de los fondos
                        htPortafolio.Add(.Rows(_fila)("CodigoPortafolioSBS").ToString & "FON", 3 + _fila)
                        htPortafolio.Add(.Rows(_fila)("CodigoPortafolioSBS").ToString & "SOL", 4 + _totPortafolio + (2 * _fila))
                        htPortafolio.Add(.Rows(_fila)("CodigoPortafolioSBS").ToString & "POR", 5 + _totPortafolio + (2 * _fila))
                    Next
                    _letra = ObtenerLetra(4 + _totPortafolio)
                    _letra2 = ObtenerLetra(3 * (dtPortafolio.Rows.Count + 1))
                    FormatoCelda(oCells, _letra & (_filaTotal - 3).ToString, _letra2 & (_filaTotal - 3).ToString, 3)
                End With
                'Dando Formato a la cabecera
                oCells(6, 3 + _totPortafolio) = "Precio S/."
                _letra = ObtenerLetra(2 + _totPortafolio)
                FormatoCelda(oCells, "C5", _letra & "5", 3)
                _letra = ObtenerLetra(4 + _totPortafolio)
                _letra2 = ObtenerLetra(3 * (dtPortafolio.Rows.Count + 1))
                FormatoCelda(oCells, _letra & "5", _letra2 & "5", 3)
                FormatoCelda(oCells, "B6", _letra2 & "6", 3)
                'Generamos el detalle
                _fila = 7
                Dim j As Integer = 0
                For Each dr In dt.Rows
                    j += 1
                    _instrumento = dr("Descripcion").ToString
                    If _instrumento <> _instrumentoAnt Or j = dt.Rows.Count Then
                        _letra = ObtenerLetra(3 * (dtPortafolio.Rows.Count + 1))
                        If j <> dt.Rows.Count Then
                            oCells.Range("A" & _fila.ToString, _letra & _fila.ToString).EntireColumn.AutoFit()
                            oCells.Range("A" & _fila.ToString, _letra & _fila.ToString).Interior.Color = System.Drawing.Color.LightGray
                            oCells(_fila, 1).value = "CÓDIGO SBS"
                            oCells(_fila, 1).Font.Bold = True
                            oCells(_fila, 2).value = _instrumento
                            oCells(_fila, 2).Font.Bold = True
                            _instrumentoAnt = _instrumento
                        Else
                            _fila = _fila + 1
                        End If
                        If _fila > 7 Then
                            With dtPortafolio
                                For i = 0 To _totPortafolio - 1
                                    _columna = htPortafolio(.Rows(i)("CodigoPortafolioSBS").ToString & "SOL")
                                    _letra = ObtenerLetra(_columna)
                                    _rango = String.Format("=SUM({0}{1}:{0}{2})", _letra, (_filaAnt + 1).ToString, (_fila - 1).ToString)
                                    oCells(_filaAnt, _columna).Formula = _rango
                                    oCells(_filaAnt, _columna).NumberFormat = "_(* #,##0_);_(* (#,##0);_(* ""-""_);_(@_)"
                                    If j = dt.Rows.Count Then
                                        oCells(_filaTotal - 3, _columna).formula = "=" & oCells(_filaTotal - 3, _columna).formula & "+" & _letra & _filaAnt.ToString
                                    Else
                                        oCells(_filaTotal - 3, _columna).value += "+" & _letra & _filaAnt.ToString
                                    End If
                                    oCells(_filaTotal - 3, _columna).NumberFormat = "_(* #,##0_);_(* (#,##0);_(* ""-""_);_(@_)"

                                    _columna = htPortafolio(.Rows(i)("CodigoPortafolioSBS").ToString & "POR")
                                    _letra = ObtenerLetra(_columna)
                                    _rango = String.Format("=SUM({0}{1}:{0}{2})", _letra, (_filaAnt + 1).ToString, (_fila - 1).ToString)
                                    oCells(_filaAnt, _columna).Formula = _rango
                                    oCells(_filaAnt, _columna).NumberFormat = String.Format("0{0}00%", _cultureInfo.NumberFormat.CurrencyDecimalSeparator)
                                    If j = dt.Rows.Count Then
                                        oCells(_filaTotal - 3, _columna).formula = "=" & oCells(_filaTotal - 3, _columna).formula & "+" & _letra & _filaAnt.ToString
                                    Else
                                        oCells(_filaTotal - 3, _columna).value += "+" & _letra & _filaAnt.ToString
                                    End If
                                    oCells(_filaTotal - 3, _columna).NumberFormat = String.Format("0{0}00%", _cultureInfo.NumberFormat.CurrencyDecimalSeparator)
                                Next
                            End With
                        End If
                        If j <> dt.Rows.Count Then
                            _filaAnt = _fila
                            _fila += 1
                        Else
                            _fila = _fila - 1
                        End If
                    End If
                    oCells(_fila, 1).value = dr("CodigoSBS").ToString
                    oCells(_fila, 1).NumberFormat = "0"
                    oCells(_fila, 2).value = dr("CodigoMnemonico").ToString
                    oCells(_fila, 3 + _totPortafolio).value = dr("TC").ToString
                    oCells(_fila, 3 + _totPortafolio).NumberFormat = String.Format("#{1}##0{0}00", _cultureInfo.NumberFormat.CurrencyDecimalSeparator, _
                    _cultureInfo.NumberFormat.CurrencyGroupSeparator)
                    With dtPortafolio
                        For i = 0 To _totPortafolio - 1
                            _columna = htPortafolio(.Rows(i)("CodigoPortafolioSBS").ToString & "FON")
                            oCells(_fila, _columna).value = dr(.Rows(i)("Descripcion").ToString).ToString
                            oCells(_fila, _columna).NumberFormat = "_(* #,##0_);_(* (#,##0);_(* ""-""_);_(@_)"
                            _letra = ObtenerLetra(_columna)
                            _columna = htPortafolio(.Rows(i)("CodigoPortafolioSBS").ToString & "SOL")
                            oCells(_fila, _columna).value = "=" & _letra & _fila.ToString & " * " & ObtenerLetra(3 + _totPortafolio) & _fila.ToString
                            oCells(_fila, _columna).NumberFormat = "_(* #,##0_);_(* (#,##0);_(* ""-""_);_(@_)"
                            _letra = ObtenerLetra(_columna)
                            iCol = htPortafolio(.Rows(i)("CodigoPortafolioSBS").ToString & "POR")
                            If oCells(_filaTotal, _columna).value <> 0 Then
                                oCells(_fila, iCol).value = "=" & _letra & _fila.ToString & " / " & _letra & _filaTotal.ToString
                            Else
                                oCells(_fila, iCol).value = 0
                            End If
                            oCells(_fila, iCol).NumberFormat = String.Format("0{0}00%", _cultureInfo.NumberFormat.CurrencyDecimalSeparator)
                        Next
                    End With
                    _fila += 1
                Next
                For iCol = 1 To 3 * (dtPortafolio.Rows.Count + 1) Step 2
                    _letra = ObtenerLetra(iCol)
                    FormatoCelda(oCells, _letra & "7", _letra & (_filaTotal - 6).ToString, 3, True)
                Next
                _letra = ObtenerLetra(3 * (dtPortafolio.Rows.Count + 1))
                FormatoCelda(oCells, _letra & "7", _letra & (_filaTotal - 6).ToString, 3, True)
                FormatoCelda(oCells, "A7", _letra & (_filaTotal - 6).ToString, 3, True)
                oCells(_filaTotal - 3, 2).value = "TOTAL"
                FormatoCelda(oCells, "B" & (_filaTotal - 3).ToString, "B" & (_filaTotal - 3).ToString, 3)
                oCells(_filaTotal, 2).value = "Patrimonio"
                FormatoCelda(oCells, "B" & _filaTotal.ToString, "B" & _filaTotal.ToString, 3, True)
                oCells.Range("B" & (_filaTotal - 3).ToString, _letra & _filaTotal.ToString).Font.Size = 11
                oCells.Range("B" & (_filaTotal - 3).ToString, _letra & _filaTotal.ToString).Font.Bold = True
                With dtPortafolio
                    For _fila = 0 To _totPortafolio - 1
                        _letra = ObtenerLetra(Convert.ToInt32(htPortafolio(.Rows(_fila)("CodigoPortafolioSBS").ToString & "SOL")))
                        _letra2 = ObtenerLetra(Convert.ToInt32(htPortafolio(.Rows(_fila)("CodigoPortafolioSBS").ToString & "POR")))
                        oCells.Range(_letra & _filaTotal.ToString, _letra2 & _filaTotal.ToString).Merge()
                        FormatoCelda(oCells, _letra & _filaTotal, _letra2 & _filaTotal.ToString, 3, True)
                    Next
                End With
            Case "LCRE"
                Dim dtLim As DataTable
                Dim _fechaEmision As String = ""
                Dim _emisor As String = ""
                Dim _codigoMnemonico As String = ""
                Dim _clasificacion As String = ""
                Dim _fila As ULong = 4
                Dim _filaAnt As ULong
                Dim _filaTemp As ULong
                Dim _contador As ULong = 0
                Dim primerFondo As Boolean = True
                Dim _col As Integer = 4
                Dim _letra As String = ""
                Dim _letra2 As String = ""
                Dim _totalFondos As Integer = dtPortafolio.Rows.Count
                Dim htPortafolio As New Hashtable
                For Each drPortafolio As DataRow In dtPortafolio.Rows
                    If primerFondo Then
                        primerFondo = False
                    Else
                        _letra = UIUtility.ObtenerLetra(_col - 1)
                        oSheet.Columns(_letra & ":" & _letra).Copy()
                        _letra = UIUtility.ObtenerLetra(_col)
                        oSheet.Columns(_letra & ":" & _letra).Insert(Excel.XlInsertShiftDirection.xlShiftToRight, Nothing)
                        _letra = UIUtility.ObtenerLetra((2 * _col))
                        oSheet.Columns(_letra & ":" & _letra).Copy()
                        _letra = UIUtility.ObtenerLetra((2 * _col) + 1)
                        oSheet.Columns(_letra & ":" & _letra).Insert(Excel.XlInsertShiftDirection.xlShiftToRight, Nothing)
                    End If
                    htPortafolio.Add(drPortafolio("CodigoPortafolioSBS").ToString & "P", _col)
                    htPortafolio.Add(drPortafolio("CodigoPortafolioSBS").ToString & "M", _col + _totalFondos + 4)
                    _col += 1
                Next
                dtLim = New LimiteBM().ObtenerPorcentajeLimiteGen("12", "1")
                For Each drLimite As DataRow In dtLim.Rows
                    If Not htPortafolio(drLimite("CodigoPortafolio").ToString & "P") Is Nothing Then
                        _letra = oCells(4, htPortafolio(drLimite("CodigoPortafolio").ToString & "P")).value
                        oCells(4, htPortafolio(drLimite("CodigoPortafolio").ToString & "P")).value = _letra.Replace("{Fondo}", drLimite("Descripcion").ToString)
                        _letra = oCells(4, htPortafolio(drLimite("CodigoPortafolio").ToString & "M")).value
                        oCells(4, htPortafolio(drLimite("CodigoPortafolio").ToString & "M")).value = _
                        _letra.Replace("{Fondo}", drLimite("Descripcion").ToString).Replace("x", CLng(drLimite("PorcentajeFondo")))
                    End If
                Next
                _filaAnt = _fila + 1
                For Each dr In dt.Rows
                    _contador += 1
                    If _fechaEmision <> dr("FechaEmision") Or _emisor <> dr("Emisor").ToString Or _codigoMnemonico <> dr("CodigoMnemonico") Or _
                        _clasificacion <> dr("Clasificacion") Or _contador = dt.Rows.Count Then
                        _fila += 1
                        If _contador = dt.Rows.Count Then
                            _emisor = ""
                        End If
                        If (_emisor = dr("Emisor").ToString And _clasificacion <> dr("Clasificacion")) Or (_emisor <> dr("Emisor").ToString) Then
                            If _fila > 5 Then
                                _letra = UIUtility.ObtenerLetra(9 + (2 * _totalFondos))
                                oCells.Range(String.Format("{0}{1}:{0}{2}", _letra, _filaTemp.ToString, (_fila - 1).ToString)).Merge()
                                _letra = UIUtility.ObtenerLetra(10 + (2 * _totalFondos))
                                oCells.Range(String.Format("{0}{1}:{0}{2}", _letra, _filaTemp.ToString, (_fila - 1).ToString)).Merge()
                                _letra = UIUtility.ObtenerLetra(11 + (2 * _totalFondos))
                                oCells.Range(String.Format("{0}{1}:{0}{2}", _letra, _filaTemp.ToString, (_fila - 1).ToString)).Merge()
                            End If
                            _filaTemp = _fila
                        End If
                        If _emisor <> dr("Emisor").ToString Then
                            If _fila > 5 Then
                                For Each drPortafolio As DataRow In dtPortafolio.Rows
                                    _letra = UIUtility.ObtenerLetra(htPortafolio(drPortafolio("CodigoPortafolioSBS").ToString & "M"))
                                    oCells.Range(String.Format("{0}{1}:{0}{2}", _letra, _filaAnt.ToString, (_fila - 1).ToString)).Merge()
                                    oCells(_fila, htPortafolio(drPortafolio("CodigoPortafolioSBS").ToString & "M")).Formula = String.Format("=SUM({0}{1}:{0}{2})", _
                                    _letra, _filaAnt.ToString, (_fila - 1).ToString)
                                Next
                                oCells(_fila, 1).value = "SUB TOTAL POR EMISOR"
                                oCells(_fila, 1).HorizontalAlignment = Excel.Constants.xlLeft
                                _letra = UIUtility.ObtenerLetra(8 + (2 * _totalFondos))
                                oCells.Range(String.Format("{0}{1}:{0}{2}", _letra, _filaAnt.ToString, (_fila - 1).ToString)).Merge()
                                oCells(_fila, 8 + (2 * _totalFondos)).Formula = String.Format("=SUM({0}{1}:{0}{2})", _letra, _filaAnt.ToString, (_fila - 1).ToString)
                                _letra = UIUtility.ObtenerLetra(9 + (2 * _totalFondos))
                                oCells(_fila, 9 + (2 * _totalFondos)).Formula = String.Format("=SUM({0}{1}:{0}{2})", _letra, _filaAnt.ToString, (_fila - 1).ToString)
                                _letra = UIUtility.ObtenerLetra(10 + (2 * _totalFondos))
                                oCells(_fila, 10 + (2 * _totalFondos)).Formula = String.Format("=SUM({0}{1}:{0}{2})", _letra, _filaAnt.ToString, (_fila - 1).ToString)
                                _letra2 = UIUtility.ObtenerLetra(9 + (2 * _totalFondos))
                                oCells(_fila, 11 + (2 * _totalFondos)).Formula = String.Format("={0}{2}-{1}{2}", _letra, _letra2, _fila.ToString)
                                _letra = UIUtility.ObtenerLetra(11 + (2 * _totalFondos))
                                _letra2 = UIUtility.ObtenerLetra(8 + _totalFondos)
                                FormatoCelda(oCells, "A" & _filaAnt.ToString, _letra & (_fila - 1).ToString, 2)
                                FormatoCelda(oCells, _letra2 & _fila.ToString, _letra & _fila.ToString, 2)
                                If _contador = dt.Rows.Count Then
                                    _fila -= 1
                                Else
                                    _fila += 2
                                End If
                            End If
                            _filaAnt = _fila
                            _filaTemp = _fila
                        End If
                        oCells(_fila, 1).value = dr("FechaEmision")
                        oCells(_fila, 2).value = dr("Emisor")
                        oCells(_fila, 3).value = dr("CodigoMnemonico")
                        If dr("PosicionF").ToString() = "" Then
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "P")).value = 0
                        Else
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "P")).value = dr("PosicionF").ToString
                        End If
                        oCells(_fila, 4 + _totalFondos).value = dr("CodigoISIN").ToString
                        oCells(_fila, 5 + _totalFondos).value = dr("CodigoSBS").ToString
                        oCells(_fila, 6 + _totalFondos).value = dr("Sector").ToString
                        oCells(_fila, 7 + _totalFondos).value = dr("Clasificacion").ToString
                        If dr("MargenF").ToString() = "" Then
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "M")).value = 0
                        Else
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "M")).value = dr("MargenF").ToString
                        End If
                        oCells(_fila, 9 + (2 * _totalFondos)).value = dr("Posicion").ToString
                        oCells(_fila, 10 + (2 * _totalFondos)).value = dr("Linea").ToString
                        oCells(_fila, 11 + (2 * _totalFondos)).value = dr("Holgura").ToString
                        _emisor = dr("Emisor").ToString
                        _fechaEmision = dr("FechaEmision").ToString
                        _codigoMnemonico = dr("CodigoMnemonico").ToString
                        _clasificacion = dr("Clasificacion").ToString
                    Else
                        If dr("PosicionF").ToString() = "" Then
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "P")).value = 0
                        Else
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "P")).value = dr("PosicionF").ToString
                        End If
                        If Convert.ToString(oCells(_fila, 5 + _totalFondos).value) = "" Then
                            oCells(_fila, 5 + _totalFondos).value = dr("CodigoSBS").ToString
                        End If
                        If Convert.ToString(oCells(_fila, 6 + _totalFondos).value) = "" Then
                            oCells(_fila, 6 + _totalFondos).value = dr("Sector").ToString
                        End If
                        If dr("MargenF").ToString() = "" Then
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "M")).value = 0
                        Else
                            oCells(_fila, htPortafolio(dr("CodigoPortafolioSBS") & "M")).value = dr("MargenF").ToString
                        End If
                    End If
                Next
                oCells.Range("A1").Select()
            Case "LCNT"
                Dim nCol As Long, nFil As Long
                Dim pRow As Long, rRow As Long
                nFil = 5
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    pRow = iRow + nFil
                    rRow = iRow - 1 + nFil
                    oCells.Range("A" + pRow.ToString + ":G" + pRow.ToString).Copy()
                    For iCol = 0 To UBound(ary)
                        nCol = iCol + 1
                        oCells(pRow, nCol) = ary(iCol).ToString
                        If iCol > 1 And iCol <= 6 Then
                            If IsNumeric(ary(iCol)) Then
                                If iCol = 6 Then
                                    oCells(pRow, nCol) = IIf(ary(iCol) = 0, "-", String.Format("{0:###,###}", ary(iCol)))
                                Else
                                    oCells(pRow, nCol) = IIf(ary(iCol) = 0, "-", String.Format("{0:###,###.00}", ary(iCol)))
                                End If
                            End If
                        End If
                    Next
                    If iRow < dt.Rows.Count - 1 Then
                        oCells.Range(String.Format("A{0}:G{1}", pRow + 1, pRow + 1)).PasteSpecial(Excel.XlPasteType.xlPasteAll)
                    End If
                Next
            Case "LSTL"
                Dim nCol As Long, nFil As Long
                Dim pRow As Long, rRow As Long
                Dim sEmi1 As String, sEmi2 As String
                Dim nFilGini As Long, nFilCini As Long
                nFil = 5
                sEmi1 = dt.Rows(0)(1)
                nFilGini = nFil
                nFilCini = nFil
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    pRow = iRow + nFil
                    rRow = iRow - 1 + nFil
                    sEmi2 = ary(1).ToString
                    If iRow < dt.Rows.Count - 1 Then
                        oCells.Range("A" + (nFil + 1).ToString + ":H" + (nFil + 1).ToString).Copy()
                        oCells.Range(String.Format("A{0}:H{1}", pRow + 2, pRow + 2)).PasteSpecial(Excel.XlPasteType.xlPasteAll)
                    End If
                    If (sEmi1 <> sEmi2) Then
                        oCells.Range("A" & nFilGini.ToString & ":A" & rRow.ToString).Merge()
                        oCells.Range("B" & nFilGini.ToString & ":B" & rRow.ToString).Merge()
                        sEmi1 = ary(1).ToString
                        nFilGini = pRow
                    End If
                    For iCol = 0 To UBound(ary)
                        nCol = iCol + 1

                        If iCol >= 0 And iCol <= 1 Then
                            If iRow = (nFilGini - nFil) Then
                                oCells(pRow, nCol) = ary(iCol).ToString
                            Else
                                If sEmi1 <> sEmi2 Then
                                    oCells(pRow, nCol) = ary(iCol).ToString
                                End If
                            End If
                        Else
                            oCells(pRow, nCol) = ary(iCol).ToString
                        End If

                        If iCol > 2 And iCol <= 8 Then
                            If IsNumeric(ary(iCol)) Then
                                If iCol = 6 Then
                                    oCells(pRow, nCol) = IIf(ary(iCol) = 0, "-", String.Format("{0:###,###}", ary(iCol)))
                                Else
                                    oCells(pRow, nCol) = IIf(ary(iCol) = 0, "-", String.Format("{0:###,###.00}", ary(iCol)))
                                End If
                            End If
                        End If
                    Next
                Next
                If sEmi1 = sEmi2 Then
                    oCells.Range("A" & nFilGini.ToString & ":A" & pRow.ToString).Merge()
                    oCells.Range("B" & nFilGini.ToString & ":B" & pRow.ToString).Merge()
                End If
                oCells.Range(String.Format("A{0}:H{1}", pRow + 1, pRow + 1)).Delete(Excel.XlDirection.xlUp)
            Case "TRZOI"
                Dim primerFondo As Boolean = True
                Dim _col As Integer = 14
                Dim _letra As String = ""
                Dim _totalFondos As Integer = dtPortafolio.Rows.Count
                For Each drPortafolio As DataRow In dtPortafolio.Rows
                    If primerFondo Then
                        primerFondo = False
                    Else
                        _letra = UIUtility.ObtenerLetra(_col - 1)
                        oSheet.Columns(_letra & ":" & _letra).Copy()
                        _letra = UIUtility.ObtenerLetra(_col)
                        oSheet.Columns(_letra & ":" & _letra).Insert(Excel.XlInsertShiftDirection.xlShiftToRight, Nothing)
                    End If
                    oCells(1, _col).value = drPortafolio("Descripcion")
                    _col += 1
                Next
                iRow = 2
                For Each dr In dt.Rows
                    For iCol = 0 To dt.Columns.Count - 1
                        oCells(iRow, iCol + 1) = dr(iCol).ToString
                    Next
                    iRow += 1
                Next
                iRow -= 1
                _letra = UIUtility.ObtenerLetra(dt.Columns.Count)
                FormatoCelda(oCells, "A1", _letra & iRow.ToString, 2)
                oCells.Range("A1").Select()
            Case Else
                oCells(1, 1) = tbFechaValoracion.Text.ToString
                For iCol = 0 To dt.Columns.Count - 1
                    oCells(2, iCol + 1) = dt.Columns(iCol).ToString
                Next
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        oCells(iRow + 3, iCol + 1) = ary(iCol).ToString
                    Next
                Next
        End Select
    End Sub
    Private Sub escribirArchivo(sufijo As String, rutaArchivo As String, dt As DataTable)
        Dim lineaArchivo As String
        Dim escritura As New IO.StreamWriter(rutaArchivo)
        escritura.Flush()
        Select Case sufijo
            Case "RENTABILIDAD"
                For i = 0 To dt.Rows.Count - 1
                    lineaArchivo = ""
                    For j = 0 To dt.Columns.Count - 1
                        'Remueve Acentos
                        lineaArchivo = UIUtility.RemoveDiacritics(lineaArchivo & dt.Rows(i)(j).ToString.Trim & ";")
                    Next
                    lineaArchivo = lineaArchivo.Remove(lineaArchivo.Length - 1, 1)
                    escritura.WriteLine(lineaArchivo)
                Next
        End Select
        escritura.Close()
    End Sub
    Private Sub FormatoCelda(ByRef oCells As Excel.Range, ByVal celda1 As String, ByVal celda2 As String, ByVal pesoBorde As Integer, Optional ByVal contorno As Boolean = False)
        With oCells.Range(celda1, celda2)
            If contorno Then
                .BorderAround(Excel.XlLineStyle.xlContinuous, pesoBorde, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black)
            Else
                .Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                .Borders.Color = System.Drawing.Color.Black
                .Borders.Weight = pesoBorde
            End If
        End With
    End Sub
    Function SelectCountDistinct(ByVal dt As DataTable, ByVal NombreColumna As String) As Integer
        Dim ht As New Hashtable
        Dim row As DataRow
        For Each row In dt.Rows
            If Not ht.ContainsKey(row(NombreColumna)) Then
                ht.Add(row(NombreColumna), String.Empty)
            End If
        Next
        Return ht.Count
    End Function
    Function TipoCalculo(ByVal codigoMoneda As String) As String
        Dim dt As DataTable
        Dim resultado As String = String.Empty
        dt = CType(ViewState(VS_Moneda), DataTable)
        dt = SelectDataTable(dt, "CodigoMoneda='" & codigoMoneda & "'")
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("TipoCalculo") = "D" Then
                resultado = "*"
            Else
                resultado = "/"
            End If
        End If
        Return resultado.ToString
    End Function
    Function SelectDataTable(ByVal dt As DataTable, ByVal filter As String) As DataTable
        Dim rows As DataRow()
        Dim dtNew As DataTable
        dtNew = dt.Clone()
        rows = dt.Select(filter)
        For Each dr As DataRow In rows
            dtNew.ImportRow(dr)
        Next
        Return dtNew
    End Function
    Function NumeroPolizas(ByVal dt As DataTable, ByVal filter As String) As Integer
        Dim rows As DataRow()
        Dim dtNew As DataTable
        dtNew = dt.Clone()
        rows = dt.Select(filter)
        For Each dr As DataRow In rows
            dtNew.ImportRow(dr)
        Next
        Return dtNew.Rows.Count
    End Function
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, dt, "CodigoPortafolio", "Descripcion", False)
    End Sub
    Private Sub CargarPortafolio2()
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, dt, "CodigoPortafolio", "Descripcion", True, "SELECCIONE")
    End Sub
    Private Sub CargarPortafolio3()
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(ddlMercado, dt, "CodigoPortafolio", "Descripcion", False)
    End Sub
    Private Sub CargarPortafolio4()
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Dropdownlist1, dt, "CodigoPortafolio", "Descripcion", True)
    End Sub
    Private Function ObtenerPatrimonio(ByVal filaSel As String, ByVal dtPatrimonio As DataTable) As String
        For Each dr As DataRow In dtPatrimonio.Rows
            If filaSel = dr("codigoportafoliosbs").ToString Then
                Return dr("Patrimonio").ToString
            End If
        Next
        Return 0
    End Function
    Protected Sub ddlportafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlportafolio.SelectedIndexChanged
        If ddlportafolio.SelectedValue <> "" Then
            Select Case Request.QueryString("RPT").ToString
                Case "CAR"
                    Dim oCarteraTituloValoracion As New CarteraTituloValoracionBM
                    tbFechaValoracion.Text = UIUtility.ConvertirFechaaString(oCarteraTituloValoracion.ObtenerFechaValoracion(ddlportafolio.SelectedValue.ToString))
            End Select
        End If
    End Sub
    'OT 10328 - 28/04/2017 - Carlos Espejo
    'Descripcion: Presenta la busqueda en la grilla
    Sub CargaGrilla()
        Dim oCarteraTituloValoracionBM As New CarteraTituloValoracionBM
        If UIUtility.ConvertirFechaaDecimal(tbFechaValoracion.Text) < Decimal.Parse(oCarteraTituloValoracionBM.ObtenerFechaValoracion(Dropdownlist1.SelectedValue.ToString)) Then
            AlertaJS("Búsqueda realizada satisfactoriamente.", "$('#btnImprimir').removeAttr('disabled')")
        Else
            AlertaJS("- Búsqueda realizada satisfactoriamente. <br>- Falta valorizar el portafolio en la fecha seleccionada por lo que no se podrá Imprimir.")
            EjecutarJS("$('#btnImprimir').attr('disabled','disabled')")
        End If
        Dim dt As DataTable = New ReporteGestionBM().reporteRentabilidad(Dropdownlist1.SelectedValue, ConvertirFechaaDecimal(tbFechaValoracion.Text), "A")
        dgLista.DataSource = dt
        dgLista.DataBind()
    End Sub
    'OT 10328 - 28/04/2017 - Carlos Espejo
    'Descripcion: Presenta la busqueda en la grilla
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            If (Dropdownlist1.SelectedValue.Trim = String.Empty) Then
                AlertaJS("Falta seleccionar el Portafolio")
            Else
                CargaGrilla()
            End If

        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    'OT 10328 - 28/04/2017 - Carlos Espejo
    'Descripcion: Presenta la busqueda en la grilla
    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargaGrilla()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub tbFechaValoracion_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaValoracion.TextChanged
        If Request.QueryString("RPT").ToString.ToUpper = "RENTABILIDAD" Then btnImprimir.Enabled = False
    End Sub
End Class