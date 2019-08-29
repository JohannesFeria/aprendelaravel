﻿Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Web
Imports System.Diagnostics
Imports System.Configuration
Imports System.Data
Imports ParametrosSIT

Imports Sit.BusinessLayer.MotorInversiones

Partial Class Modulos_Inversiones_InstrumentosNegociados_frmCertificadoDeposito
    Inherits BasePage
#Region "Variables"
    Dim objutil As New UtilDM
    Dim oImpComOP As New ImpuestosComisionesOrdenPreOrdenBM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oPortafolioBM As New PortafolioBM
    Dim oValoresBM As New ValoresBM
    Dim objcomisiones As New ImpuestosComisionesBM
    Dim strTipoRenta As String
    Dim objImpuestosComisionesOPBM As New ImpuestosComisionesOrdenPreOrdenBM
    Dim objferiadoBM As New FeriadoBM
#End Region
#Region "Funciones y Metodos"
    Private Sub ControlarCamposOE()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Visible = True
    End Sub
    Private Sub ControlarCamposEO_CO_XO()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Visible = True
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            Me.btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacionComision(True)
            Me.btnCaracteristicas.Visible = True
            Me.btnCaracteristicas.Enabled = True
            Me.btnAceptar.Visible = True
            Session("EstadoPantalla") = "Modificar"
            Dim GUID As Guid = System.Guid.NewGuid()
            ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        End If
    End Sub
    Private Sub MostrarOcultarBotonesAcciones(ByVal estado As Boolean)
        Me.btnIngresar.Visible = estado
        Me.btnModificar.Visible = estado
        Me.btnEliminar.Visible = estado
        Me.btnConsultar.Visible = estado
    End Sub
    Private Sub ObtieneImpuestosComisiones()
        If Not Session("Mercado") Is Nothing And Not Session("TipoRenta") Is Nothing Then
            'OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"))
        End If
    End Sub
    Private Sub ControlarCamposTI()
        UIUtility.CargarPortafoliosOI(Me.ddlFondoDestino)
        Me.lblFondo.Text = "Fondo Origen"
        Me.lblFondoDestino.Visible = True
        Me.ddlFondoDestino.Visible = True
        MostrarOcultarBotonesAcciones(False)
        Session("ValorCustodio") = ""
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        CargarFechaVencimiento()
        txtHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
        Else
            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
        End If
        CargarPaginaIngresar()
        CargarIntermediario()
        ObtieneImpuestosComisiones()
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String,
    ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal vdescripcionPortafolio As String, ByVal cportafolio As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + cportafolio + "&vdescripcionPortafolio=" +
        vdescripcionPortafolio + "&vportafolio=" + portafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin +
        "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub
    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        If Session("EstadoPantalla") = "Ingresar" Then
            Me.btnImprimir.Visible = True
            Me.btnImprimir.Enabled = True
        End If
        Me.btnAceptar.Visible = False
    End Sub
    Private Sub CargarPaginaConsultar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
        CargarPaginaProcesar()
        Me.btnAceptar.Visible = False
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        btnAceptar.Visible = True
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            strJS.AppendLine("$('#btnImprimir').show();")
            strJS.AppendLine("$('#btnImprimir').removeAttr('disabled');")
            btnImprimir.Visible = True
            btnImprimir.Enabled = True
        End If
        EjecutarJS(strJS.ToString())
    End Sub
    Private Sub CargarPaginaEliminar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
        CargarPaginaProcesar()
        Me.txtComentarios.Text = ""
    End Sub
    Private Sub CargarPaginaModificar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
        Me.txtComentarios.Text = ""
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        Me.tbFechaLiquidacion.ReadOnly = Not estado
        'Me.txtMnomOp.ReadOnly = Not estado
        'Me.txtMnomOrd.ReadOnly = Not estado
        Me.txtYTM.ReadOnly = Not estado
        'Me.txtPrecioNegoc.ReadOnly = Not estado
        Me.ddlIntermediario.Enabled = estado
        Me.ddlTipoTasa.Enabled = estado
        Me.ddlContacto.Enabled = estado
        Me.txtMontoOperacional.ReadOnly = Not estado
        Me.txtPrecioNegSucio.ReadOnly = Not estado
        Me.txtNroPapeles.ReadOnly = Not estado
        Me.txtObservacion.ReadOnly = Not estado
        Me.txtMontoNetoOpe.ReadOnly = Not estado
        If estado Then
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        If (Me.hdPagina.Value = "DA") Then
            Me.tbFechaOperacion.ReadOnly = True
        End If
        chkFicticia.Enabled = False
        If (Not Session("EstadoPantalla") Is Nothing And Not Session("Procesar") Is Nothing) Then
            If Session("EstadoPantalla") = "Ingresar" And Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS And Session("Procesar") = "0" Then
                chkFicticia.Enabled = True
            End If
        End If
        If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            Me.chkRegulaSBS.Enabled = False
        Else
            Me.chkRegulaSBS.Enabled = estado
        End If
    End Sub
    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlFondo.Enabled = estado
        ddlOperacion.Enabled = estado
        btnBuscar.Enabled = estado
        txtSBS.ReadOnly = Not estado
        txtISIN.ReadOnly = Not estado
        txtMnemonico.ReadOnly = Not estado
    End Sub
    Private Sub CargarPaginaBuscar()
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
    End Sub
    Private Sub CargarCuponera(ByVal strCodigoMnemonico As String)
        Dim oCuponeraBM As New CuponeraBM
        Dim dtCuponera As DataTable = oCuponeraBM.SeleccionarPorOrdenInversion(strCodigoMnemonico, DatosRequest)
        Session("dtCuponera") = dtCuponera
    End Sub
    Private Sub EliminarCuponerasOITemporales()
        Dim oCuponera As New CuponeraBM
        Dim dtCuponeraOI As DataTable = CType(Session("dtCuponera"), DataTable)
        If Not dtCuponeraOI Is Nothing Then
            If dtCuponeraOI.Rows.Count > 0 Then
                oCuponera.EliminarCuponeraOI(ViewState("CuponeraTemporalGUID").ToString(), True, txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
            End If
        End If
    End Sub
    Public Sub CargarIntermediario()
        UIUtility.CargarIntermediariosOI(ddlIntermediario)
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Private Function ValidarFechas() As Boolean
        Dim dsFechas As PortafolioBE
        Dim drFechas As DataRow
        Dim blnResultado As Boolean = True
        If UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text) > UIUtility.ConvertirFechaaDecimal(Me.tbFechaLiquidacion.Text) Then
            blnResultado = False
            AlertaJS(ObtenerMensaje("CONF7"))
        End If
        If (Me.hdPagina.Value = "DA") Then
            Return True
        End If
        dsFechas = oPortafolioBM.Seleccionar(Me.ddlFondo.SelectedValue, DatosRequest)
        If dsFechas.Tables(0).Rows.Count > 0 Then
            drFechas = dsFechas.Tables(0).NewRow
            drFechas = dsFechas.Tables(0).Rows(0)
            Dim dblFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
            Dim dblFechaConstitucion As Decimal = CType(drFechas("FechaConstitucion"), Decimal)
            Dim dblFechaTermino As Decimal = CType(drFechas("FechaTermino"), Decimal)
            Dim dblFechaValoracion As Decimal = CType(drFechas("FechaValoracion"), Decimal)
            If (dblFechaConstitucion > dblFechaOperacion) And (dblFechaValoracion > dblFechaOperacion) Then
                blnResultado = False
                AlertaJS(ObtenerMensaje("CONF4"))
            End If
        End If
        If (objferiadoBM.BuscarPorFecha(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))) = True Then
            blnResultado = False
            AlertaJS(ObtenerMensaje("CONF8"))
        End If
        Return blnResultado
    End Function
    Private Sub OcultarBotonesInicio()
        Me.btnBuscar.Visible = False
        Me.btnCuponera.Visible = False
        Me.btnCaracteristicas.Visible = False
        Me.btnLimites.Visible = False
        Me.btnProcesar.Visible = False
        Me.btnImprimir.Visible = False
    End Sub
    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        'OT 9856 - 24/01/2017 - Carlos Espejo
        'Descripcion: Resalta los controles necesarios para la negociación
        UIUtility.ResaltaCajaTexto(txtMnomOrd, False)
        UIUtility.ResaltaCajaTexto(txtMnomOp, False)
        UIUtility.ResaltaCajaTexto(tbFechaLiquidacion, False)
        UIUtility.ResaltaCajaTexto(txtYTM, False)
        UIUtility.ResaltaCombo(ddlTipoTasa, False)
        UIUtility.ResaltaCombo(ddlIntermediario, False)
        UIUtility.ResaltaCombo(ddlContacto, False)
        UIUtility.ResaltaBotones(btnProcesar, False)
        UIUtility.ResaltaBotones(btnAceptar, False)
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se deshabilita campo "Intermediario" y "Tipo Tasa" en carga de formulario | 08/06/18
        ddlIntermediario.Enabled = False
        ddlTipoTasa.Enabled = False
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se deshabilita campo "Intermediario" y "Tipo Tasa" en carga de formulario | 08/06/18

        'Fin OT 9856
        btnAceptar.Visible = False
    End Sub
    Private Sub LimpiarCaracteristicasValor()
        Me.lbldescripcion.Text = ""
        Me.lblfecfinbono.Text = ""
        Me.lblnominales.Text = ""
        Me.lblEmisor.Text = ""
        Me.lblParticipacion.Text = ""
        Me.lblBaseCupon.Text = ""
        Me.lblPrecioVector.Text = ""
        Me.lblBaseTir.Text = ""
        Me.lblUnidades.Text = ""
        Me.lblduracion.Text = ""
        Me.lblMoneda.Text = ""
        Me.ddlFondo.SelectedIndex = 0
        Me.ddlOperacion.SelectedIndex = 0
        Me.txtISIN.Text = ""
        Me.txtSBS.Text = ""
        Me.txtMnemonico.Text = ""
    End Sub
    Private Sub LimpiarDatosOperacion()
        tbFechaOperacion.Text = ""
        tbFechaLiquidacion.Text = ""
        txtMnomOp.Text = ""
        txtMnomOrd.Text = ""
        txtYTM.Text = ""
        lblPrecioCal.Text = ""
        txtPrecioNegoc.Text = ""
        If ddlIntermediario.Items.Count > 0 Then ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        ddlContacto.SelectedIndex = 0
        If ddlTipoTasa.Items.Count > 0 Then ddlTipoTasa.SelectedIndex = 0
        txtMontoOperacional.Text = ""
        'OT 9936 - 06/02/2017 - Carlos Espejo
        'Descripcion: El campo es numerico por lo que debe ir 0 como valor incial para evitar errores al guardar
        txtCorrido.Text = "0"
        txtMontoNetoOpe.Text = "0"
        'OT 9939 Fin
        txtPrecioNegSucio.Text = ""
        txtHoraOperacion.Text = ""
        txtNroPapeles.Text = ""
        txtObservacion.Text = ""
        txtHoraOperacion.Text = ""
    End Sub
    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
        LimpiarCaracteristicasValor()
        LimpiarDatosOperacion()
        HabilitaDeshabilitaCabecera(True)
        btnBuscar.Visible = True
        btnBuscar.Enabled = True
        Session("ValorCustodio") = ""
    End Sub
    Private Sub ConfiguraModoConsulta()
        UIUtility.ExcluirOtroElementoSeleccion(ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
    End Sub
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11",
        "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha de Operacion"
        drGrilla("v1") = tbFechaOperacion.Text
        drGrilla("c2") = "Fecha de Vencimiento"
        drGrilla("v2") = tbFechaLiquidacion.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = txtHoraOperacion.Text
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = txtMnomOrd.Text
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = txtMnomOp.Text
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = "EFECTIVA"
        drGrilla("c7") = "YTM %"
        drGrilla("v7") = txtYTM.Text
        drGrilla("c8") = "Precio Negociación %"
        drGrilla("v8") = txtPrecioNegoc.Text
        drGrilla("c9") = "Precio Calculado"
        drGrilla("v9") = lblPrecioCal.Text
        drGrilla("c10") = "Precio Negociación Sucio"
        drGrilla("v10") = txtPrecioNegSucio.Text
        drGrilla("c11") = ""
        drGrilla("v11") = ""
        drGrilla("c12") = ""
        drGrilla("v12") = ""
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = txtMontoOperacional.Text
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = txtNroPapeles.Text
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = ddlIntermediario.SelectedItem.Text
        If ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = ddlContacto.SelectedItem.Text
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = "Observación"
        drGrilla("v18") = txtObservacion.Text.ToUpper
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Private Sub ReturnArgumentShowDialogPopup()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 12/06/18
        Select Case hdPagina.Value
            Case "CO"
                AlertaJS("Se Confirmó la orden correctamente", "window.close()")
            Case "EO"
                AlertaJS("Se Ejecutó la orden correctamente", "window.close()")
            Case "XO"
                AlertaJS("Se Extornó la orden correctamente", "window.close()")
            Case "OE"
                EjecutarJS("window.close()")
            Case "MODIFICA"
                AlertaJS("Se Modificó la orden correctamente", "window.close()")
        End Select
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 12/06/18
    End Sub
    Public Function InsertarOrdenInversion() As String
        Dim strCodigoOI, strCodigoOI_T As String
        oOrdenInversionBE = crearObjetoOI()
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        If hdPagina.Value = "TI" Then
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoPortafolioSBS") = ddlFondoDestino.SelectedValue
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoOperacion") = UIUtility.ObtenerCodigoOperacionTIngreso().ToString()
            Session("ValorCustodio") = UIUtility.ObtieneUnCustodio(Session("ValorCustodio"))
            strCodigoOI_T = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
            ViewState("CodigoOrden_T") = "-" + strCodigoOI_T
        Else
            ViewState("CodigoOrden_T") = ""
        End If
        Return strCodigoOI
    End Function
    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
    End Sub
    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, ddlMotivoCambio.SelectedValue, DatosRequest)
        oImpComOP.Eliminar(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc,
        txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = Me.txtCodigoOrden.Value
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoMoneda = Session("CodigoMoneda")
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = Me.txtSBS.Text
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.MontoNominalOrdenado = Math.Round(Convert.ToDecimal(Me.txtMnomOrd.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.MontoNominalOperacion = Math.Round(Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.CodigoTipoCupon = Me.ddlTipoTasa.SelectedValue
        oRow.YTM = Math.Round(Convert.ToDecimal(Me.txtYTM.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.InteresCorrido = Math.Round(Convert.ToDecimal(Me.txtCorrido.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)

        oRow.PrecioNegociacionLimpio = Math.Round(Convert.ToDecimal(Me.txtPrecioLimpio.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioNegociacionSucio = Math.Round(Convert.ToDecimal(Me.txtPrecioNegSucio.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioCalculado = Math.Round(Convert.ToDecimal(Me.lblPrecioCal.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)

        oRow.Situacion = "A"
        oRow.InteresCorridoNegociacion = Math.Round(Convert.ToDecimal(Me.txtCorrido.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.MontoOperacion = Math.Round(Convert.ToDecimal(Me.txtMontoOperacional.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)

        oRow.CantidadOperacion = Math.Round(Convert.ToDecimal(Me.txtNroPapeles.Text.Replace(".", UIUtility.DecimalSeparator)), 0)

        oRow.Observacion = Me.txtObservacion.Text.ToUpper
        oRow.ObservacionCarta = Me.txtObservacionCarta.Text

        oRow.HoraOperacion = Me.txtHoraOperacion.Text
        oRow.Precio = Math.Round(Convert.ToDecimal(lblPrecioVector.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.TotalComisiones = 0
        oRow.MontoNetoOperacion = Math.Round(Convert.ToDecimal(Me.txtMontoNetoOpe.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.CategoriaInstrumento = "CD"
        If Not ViewState("estadoOI") Is Nothing Then
            If ViewState("estadoOI").Equals("E-EXC") Then
                oRow.Estado = ViewState("estadoOI")
            End If
        End If
        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
            If ddlMotivoCambio.SelectedIndex > 0 Then
                oRow.CodigoMotivoCambio = ddlMotivoCambio.SelectedValue
            End If
            If Session("EstadoPantalla") = "Modificar" Then
                oRow.IndicaCambio = "1"
            End If
        End If
        If (chkFicticia.Checked) Then
            oRow.Ficticia = "S"
        Else
            oRow.Ficticia = "N"
        End If
        If (chkRegulaSBS.Checked) Then
            oRow.RegulaSBS = "S"
        Else
            oRow.RegulaSBS = "N"
        End If
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se agrega campo Bolsa para el registro de la OI | 27/06/18 
        If ddlPlaza.SelectedIndex > 0 Then oRow.Plaza = ddlPlaza.SelectedValue
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se agrega campo Bolsa para el registro de la OI | 27/06/18

        ' INICIO | Proyecto SIT Fondos - Mandato | Ian Pastor M. - CRumiche | 2018-09-21 | Categoría Contable
        oRow.TipoValorizacion = ddlCategoriaContable.SelectedValue
        oRow.TirNeta = Math.Round(Convert.ToDecimal(Me.txtTIRNeto.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        ' FIN | Proyecto SIT Fondos - Mandato | Ian Pastor M. - CRumiche | 2018-09-21 | Categoría Contable

        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Private Function ObtieneCustodiosSaldos() As Boolean
        Dim decAux As Decimal
        Dim strCodigoOperacion As String = String.Empty
        If Session("EstadoPantalla") = "Ingresar" Or Session("EstadoPantalla") = "Modificar" Then
            If VerificarSaldosCustodios(decAux) = False Then
                If Me.hdPagina.Value = "TI" Then
                    Select Case ddlOperacion.SelectedValue
                        Case UIUtility.ObtenerCodigoOperacionTIngreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionCompra()
                        Case UIUtility.ObtenerCodigoOperacionTEgreso() : strCodigoOperacion = UIUtility.ObtenerCodigoOperacionVenta()
                    End Select
                Else
                    strCodigoOperacion = ddlOperacion.SelectedValue.ToString()
                End If
                Return False
            End If
        End If
        Return True
    End Function
    Private Function VerificarSaldosCustodios(ByRef decAux As Decimal) As Boolean
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim decMontoAux As Decimal = 0.0
        Dim cantCustodios As Integer = 0
        Try
            Dim decUnidades As Decimal
            If Me.lblUnidades.Text = "0" Then
                decUnidades = 1
            Else
                decUnidades = Convert.ToDecimal(Me.lblUnidades.Text.Replace(".", UIUtility.DecimalSeparator))
            End If
            Dim decValorLocal As Decimal = Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator)) / decUnidades
            decAux = Math.Round(decValorLocal, Constantes.M_INT_NRO_DECIMALES)
            If hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".") Then
                Return True
            End If
            '********COMPRA*********
            If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                Return True
            End If
            '********VENTA*********
            If Session("ValorCustodio") Is Nothing Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + lblSaldoValor.Text
            ElseIf Session("ValorCustodio") = "" Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + lblSaldoValor.Text
            End If
            decMontoAux = UIUtility.ObtenerSumatoriaSaldosSeleccionados(CType(Session("ValorCustodio"), String), cantCustodios)
            If decMontoAux = decAux Then
                hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                Return True
            ElseIf decMontoAux > decAux Then
                If cantCustodios = 1 Then
                    Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                    hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                    Return True
                Else
                    Session("ValorCustodio") = UIUtility.AjustarMontosCustodios(CType(Session("ValorCustodio"), String),
                    Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, "."))
                    hdNumUnidades.Value = Convert.ToString(decAux).Replace(UIUtility.DecimalSeparator, ".")
                    Return True
                End If
                Return False
            ElseIf decMontoAux < decAux Then
                Session("ValorCustodio") = Me.hdCustodio.Value + strSeparador + Me.hdSaldo.Value
                Return False
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal operacion As String, ByVal categoria As String,
    ByVal valor As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & ddlFondo.SelectedValue & "&vFondo=" &
        ddlFondo.SelectedItem.Text & "&vOperacion=" & operacion & "&vCategoria=" & categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '500', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "'; ")
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal cfondo As String, ByVal fondo As String,
    ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal valor As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & cfondo & "&vFondo=" & fondo &
        "&vOperacion=" & operacion & "&vFechaOperacion=" & fecha & "&vAccion=" & accion & "&vCategoria=CD"
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "'; ")
    End Sub
    Private Sub ShowDialogCuponera(ByVal mnemonico As String, Optional ByVal IsProcesar As Boolean = True)
        Dim strGuid As String = String.Empty
        Dim strCodigoOrden As String = String.Empty
        Dim strCodigoPortafolioSBS As String = String.Empty
        IsProcesar = CType(ViewState("IsIndica"), Boolean)
        If IsProcesar Then
            strGuid = ViewState("CuponeraTemporalGUID").ToString()
            strCodigoOrden = "0"
            strCodigoPortafolioSBS = "0"
        Else
            strGuid = "0"
            strCodigoOrden = Me.txtCodigoOrden.Value
            strCodigoPortafolioSBS = ddlFondo.SelectedValue
        End If
        Dim strCodigoMnemonico As String = mnemonico
        Dim strInteresCorrido As String = "0"
        Dim strMontoOperacion As String = Convert.ToDecimal(Me.txtMontoOperacional.Text.Replace(",", "")).ToString()
        Dim strPrecioCalculado As String = Convert.ToDecimal(Me.lblPrecioCal.Text.Replace(",", "")).ToString()
        Dim strFechaOperacion As String = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
        Dim strURL As String = "frmConsultaCuponeras.aspx?CodigoMnemonico=" & mnemonico & "&guid=" + strGuid & "&codigoOrden=" & strCodigoOrden &
        "&CodigoPortafolioSBS=" & strCodigoPortafolioSBS & "&InteresCorrido=" & strInteresCorrido & "&MontoOperacion=" & strMontoOperacion & "&PrecioCalculado=" &
        strPrecioCalculado & "&FechaOperacion=" & strFechaOperacion
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', ' '); ")
    End Sub
    Private Sub HabilitaBotones(ByVal bCuponera As Boolean, ByVal bLimites As Boolean, ByVal bIngresar As Boolean, ByVal bModificar As Boolean,
    ByVal bEliminar As Boolean, ByVal bConsultar As Boolean, ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean,
    ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean, ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean,
    ByVal bLimitesParametrizados As Boolean)
        btnCuponera.Visible = False
        btnLimites.Visible = False
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnConsultar.Visible = bConsultar
        btnProcesar.Visible = bProcesar
        btnImprimir.Visible = bImprimir
        btnAceptar.Visible = bAceptar
        btnBuscar.Visible = bBuscar
        btnSalir.Visible = bSalir
        btnRetornar.Visible = bRetornar
        btnCaracteristicas.Visible = bCaracteristicas
    End Sub
    Private Sub LimpiarSesiones()
        Session("Procesar") = Nothing
        Session("ValorCustodio") = Nothing
        Session("Mercado") = Nothing
        Session("TipoRenta") = Nothing
        Session("Instrumento") = Nothing
        Session("EstadoPantalla") = Nothing
        Session("Busqueda") = Nothing
        Session("CodigoMoneda") = Nothing
        Session("datosEntidad") = Nothing
        Session("ReporteLimitesEvaluados") = Nothing
        Session("dtdatosoperacion") = Nothing
        Session("accionValor") = Nothing
        Session("dtCuponera") = Nothing
    End Sub
    Public Sub CargarFechaVencimiento(Optional ByVal flag As Boolean = False)
        If (Me.hdPagina.Value <> "CO") Then
            If (Me.hdPagina.Value <> "DA") Then
                Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
                If Not dtAux Is Nothing Then
                    If dtAux.Rows.Count > 0 Then
                        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                    End If
                End If
            Else
                tbFechaOperacion.Text = Request.QueryString("Fecha")
            End If
            If flag Then
                tbFechaLiquidacion.Text = tbFechaOperacion.Text
            End If
        End If
    End Sub
    Private Sub CargarContactos()
        Dim objContacto As New ContactoBM
        Me.ddlContacto.DataTextField = "DescripcionContacto"
        Me.ddlContacto.DataValueField = "CodigoContacto"
        Me.ddlContacto.DataSource = objContacto.ListarContactoPorTerceros(Me.ddlIntermediario.SelectedValue)
        Me.ddlContacto.DataBind()
        Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = CType(Session("datosEntidad"), DataTable)
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = ddlIntermediario.SelectedValue Then
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        Me.hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If

                    Exit For
                End If
            Next
        End If
    End Sub
    'Public Sub CalcularComisiones()
    '    Dim dblTotalComisiones As Decimal = 0.0
    '    txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
    'End Sub
    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
    End Sub
    Private Sub CargarTipoTasa()
        If (txtSBS.Text.Trim.Length > 0) Then
            ddlTipoTasa.Items.Clear()
            Dim codigoSBS As String = txtSBS.Text.Substring(0, 2)
            If (codigoSBS = "64" Or codigoSBS = "78") Then
                ddlTipoTasa.Items.Insert(0, New ListItem("Efectiva", "2"))
                If Not ViewState("CodigoTipoCupon") = "2" Then ddlTipoTasa.SelectedValue = "2"
            Else
                UIUtility.CargarTipoCuponOI(ddlTipoTasa)
                If Not ViewState("CodigoTipoCupon") Is Nothing Then ddlTipoTasa.SelectedValue = ViewState("CodigoTipoCupon")
            End If
        End If
    End Sub
    Private Sub CargarDatosOrdenInversion(ByVal CodigoOrden As String)
        '    Try
        'OT 10019 - 22/02/2017 - Carlos Espejo
        'Descripcion: Se agrega el parametro CodigoOrden
        oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, Me.ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
        'OT 10019 Fin
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = oOrdenInversionBE.Tables(0).Rows(0)
        Session("CodigoMoneda") = oRow.CodigoMoneda
        txtISIN.Text = oRow.CodigoISIN
        txtMnemonico.Text = oRow.CodigoMnemonico
        Session("Nemonico") = oRow.CodigoMnemonico
        txtCodigoOrden.Value = oRow.CodigoOrden
        If oRow.CodigoOperacion.ToString <> "" Then
            ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
        Else
            ddlOperacion.SelectedIndex = 0
        End If
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        txtMnomOrd.Text = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
        txtMnomOp.Text = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
        txtYTM.Text = Format(oRow.YTM, "##,##0.0000000")

        txtPrecioLimpio.Text = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
        txtPrecioNegSucio.Text = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
        lblPrecioCal.Text = Format(oRow.PrecioCalculado, "##,##0.0000000")
        txtPrecioNegoc.Text = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")

        txtMontoOperacional.Text = Format(oRow.MontoOperacion, "##,##0.00")
        ViewState("MontoNeto") = Convert.ToDecimal(txtMontoOperacional.Text)
        txtNroPapeles.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")

        txtObservacion.Text = oRow.Observacion
        txtObservacionCarta.Text = oRow.ObservacionCarta

        hdNumUnidades.Value = Format(Me.txtMontoOperacional.Text, "##,##0.0000000")
        lblPrecioVector.Text = Format(oRow.Precio, "##,##0.0000000")
        ViewState("CodigoTipoCupon") = Nothing
        If oRow.CodigoTipoCupon <> String.Empty Then
            ViewState("CodigoTipoCupon") = oRow.CodigoTipoCupon
            ' ddlTipoTasa.SelectedValue = oRow.CodigoTipoCupon
        End If
        txtHoraOperacion.Text = oRow.HoraOperacion
        Session("CodigoOI") = Me.txtCodigoOrden.Value
        Dim dtAux As DataTable
        dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
        If dtAux.Rows.Count > 0 Then
            Me.hdCustodio.Value = dtAux.Rows(0)("CodigoCustodio")
            CargarIntermediario()
            If ddlIntermediario.Items.Count > 1 Then
                ddlIntermediario.SelectedValue = oRow.CodigoTercero
                CargarContactos()
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se valida campo vacío "Contacto" debido a que ahora está oculto | 05/06/18
                If oRow.CodigoContacto <> String.Empty Then ddlContacto.SelectedValue = oRow.CodigoContacto
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se valida campo vacío "Contacto" debido a que ahora está oculto | 05/06/18
            Else
                AlertaJS(ObtenerMensaje("CONF29"))
            End If
        Else
            AlertaJS(ObtenerMensaje("CONF29"))
        End If
        If oRow.Ficticia = "S" Then
            chkFicticia.Checked = True
        Else
            chkFicticia.Checked = False
        End If
        'OT 10019 - 22/02/2017 - Carlos Espejo
        'Descripcion: Se agregan los campos faltantes
        txtMontoNetoOpe.Text = oRow.MontoNetoOperacion
        txttotalComisionesC.Text = oRow.TotalComisiones
        'OT 10019 Fin
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) el motivo de cambio en combo dllMotivoCambio| 05/06/18
        If ddlMotivoCambio.Items.Count > 0 And oRow.CodigoMotivoCambio <> String.Empty Then
            ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) el motivo de cambio en combo dllMotivoCambio| 05/06/18

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) la bolsa registrada por masivos | 25/06/18
        If ddlPlaza.Items.Count > 0 And oRow.Plaza <> String.Empty Then
            ddlPlaza.SelectedValue = oRow.Plaza
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se recupera (si fuera el caso) la bolsa registrada por masivos | 25/06/18

        ' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion
        If oRow.TipoValorizacion.ToString <> "" Then
            ddlCategoriaContable.SelectedIndex = ddlCategoriaContable.Items.IndexOf(ddlCategoriaContable.Items.FindByValue(oRow.TipoValorizacion.ToString()))
        Else
            ddlCategoriaContable.SelectedIndex = 0
        End If
        ddlCategoriaContable.Enabled = IIf(ObtenerTipoFondo() = "MANDA", True, False)

        txtTIRNeto.Text = Format(oRow.TirNeta, "##,##0.0000000")
        If oRow.TirNeta = 0 Then txtTIRNeto.Text = Format(oRow.YTM, "##,##0.0000000")
        ' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Tipo Valorizacion

        'Catch ex As Exception
        '    If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
        '        AlertaJS(ObtenerMensaje("CONF31"))
        '    Else
        '        AlertaJS(ObtenerMensaje("CONF32"))
        '    End If
        'End Try
    End Sub
    Private Sub CargarCaracteristicasValor()
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM
        'OT 9968 - 14/02/2017 - Carlos Espejo
        'Descripcion: Se usa la session Nemonico
        'OT 10090 - 26/07/2017 - Carlos Espejo
        'Se quitaron los parametros innecesarios
        dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoDeposito(Session("Nemonico"), Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value)
        'OT 10090 Fin
        'OT 9968 Fin
        Try
            If dsValor.Tables(0).Rows.Count > 0 Then
                drValor = dsValor.Tables(0).Rows(0)
                Session("TipoRenta") = CType(drValor("val_TipoRenta"), String)
                Session("CodigoMoneda") = CType(drValor("val_CodigoMoneda"), String)
                If Not ((Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Or (Me.hdPagina.Value = "MODIFICA")) Then
                    Session("Mercado") = CType(drValor("val_Mercado"), String)
                End If
                Me.lblMoneda.Text = CType(drValor("val_CodigoMoneda"), String)
                Me.txtISIN.Text = CType(drValor("val_CodigoISIN"), String)
                Me.txtSBS.Text = CType(drValor("val_CodigoSBS"), String)
                Me.lblSaldoValor.Text = CType(drValor("SaldoValor"), String)
                Me.lblBaseCupon.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseCupon")), Constantes.M_INT_NRO_DECIMALES), Integer)
                Me.lblBaseTir.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseTir")), Constantes.M_INT_NRO_DECIMALES), Integer)
                Me.lbldescripcion.Text = CType(drValor("val_Descripcion"), String)
                Me.lblduracion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_Duracion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblEmisor.Text = CType(drValor("val_Emisor"), String)
                Me.lblnominales.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesEmitidos")), Constantes.M_INT_NRO_DECIMALES), Decimal),
                "##,##0.0000000")
                Me.lblUnidades.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesUnitarias")), Constantes.M_INT_NRO_DECIMALES), Decimal),
                "##,##0.0000000")
                Me.lblfecfinbono.Text = CType(drValor("val_FechaFinBono"), String)
                Me.lblPrecioVector.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_VectorPrecio")), Constantes.M_INT_NRO_DECIMALES), Decimal),
                "##,##0.0000000")
                Me.lblParticipacion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_PorParticipacion")), Constantes.M_INT_NRO_DECIMALES), Decimal),
                "##,##0.0000000")
                hdCodigoTipoCupon.Value = drValor("val_CodigoTipoCupon")

                If drValor("val_Rescate") = "N" Then
                    Me.lblRescate.Text = "NO"
                ElseIf drValor("val_Rescate") = "S" Then
                    Me.lblRescate.Text = "SI"
                Else
                    Me.lblRescate.Text = ""
                End If

                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 11/07/2018 
                If Session("EstadoPantalla") = "Ingresar" Or Me.hdPagina.Value = "TI" Then
                    ddlTipoTasa.SelectedValue = "2"
                    ''Si MERCADO (Del Valor) = Extranjero
                    'If drValor("val_Mercado").ToString.Equals("2") Then ddlTipoTasa.SelectedValue = "2"
                End If
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 11/07/2018  

                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 29/05/2018
                If dsValor.Tables.Count < 2 Then Throw New Exception("Error: No se ha podido obtener el detalle de cupones del Bono")
                ViewState("DatosValor_DetalleCupones") = dsValor.Tables(1)

                ViewState("DatosValor_TasaCupon") = drValor("TasaCupon")
                ViewState("DatosValor_ValorNominalUnitario") = drValor("val_NominalesUnitarias")
                ViewState("DatosValor_EsCuponADescuento") = drValor("val_CodigoTipoCupon").ToString.Equals("3") ' Es cupón a descuento solo si CodigoTipoCupon = 3 
                ViewState("DatosValor_EsMercadoExtrangero") = drValor("val_Mercado").ToString.Equals("2") 'Mercado = 2 : Es equivalente a MERCADO EXTRANJERO

                ViewState("DatosValor_BaseCuponMensual") = drValor("BaseCuponMensual")
                ViewState("DatosValor_BaseCuponAnual") = drValor("BaseCuponAnual")
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 29/05/2018
                If Me.hdPagina.Value <> "CO" Then
                    ddlCategoriaContable.SelectedValue = CType(IIf(ObtenerTipoFondo() = "MANDA", "DIS_VENTA", "VAL_RAZO"), String)
                    ddlCategoriaContable.Enabled = IIf(ObtenerTipoFondo() = "MANDA", True, False)
                End If
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
    End Sub
#End Region
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se centraliza el Try ... Catch  en uno solo por evento | 11/06/18 
        Try
            txtPrecioNegSucio.Text = "0.0000"
            txtNroPapeles.Text = "0.0000000"
            Me.lblPrecioCal.Text = "0.0000"
            Me.txtPrecioNegoc.Text = "0.0000"
            txtMontoOperacional.Text = "0.00"
            Me.txtYTM.Text = "0.0000000"
            'Nuevo 
            If Session("EstadoPantalla") = "Ingresar" Then
                If Session("Busqueda") = 0 Then
                    If Me.ddlFondo.SelectedValue = "" Then
                        If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                            AlertaJS(ObtenerMensaje("CONF42"))
                        ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                            AlertaJS(ObtenerMensaje("CONF43"))
                            Exit Sub
                        End If
                    End If
                    ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlOperacion.SelectedValue, "CD", "1")
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF1", "", "SI")
                        Else
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                        End If
                        If Me.ddlFondo.SelectedValue <> "" Then
                            CargarCaracteristicasValor()
                            UIUtility.ResaltaCajaTexto(txtNroPapeles, True)
                            UIUtility.ResaltaCajaTexto(txtMnomOp, True)
                            UIUtility.ResaltaCajaTexto(tbFechaLiquidacion, True)
                            UIUtility.ResaltaCajaTexto(txtYTM, True)
                            UIUtility.ResaltaCombo(ddlTipoTasa, True)
                            UIUtility.ResaltaCombo(ddlIntermediario, True)
                            UIUtility.ResaltaCombo(ddlContacto, True)
                            UIUtility.ResaltaBotones(btnProcesar, True)
                            UIUtility.ResaltaBotones(btnAceptar, True)
                        End If
                        If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = Me.ddlOperacion.SelectedValue Then
                            Me.ddlFondo.Enabled = True
                        End If
                        CargarPaginaIngresar()
                        CargarFechaVencimiento()
                        CargarTipoTasa()
                        CargarIntermediario()
                    Else
                        Session("Busqueda") = 0
                    End If
                End If
            Else
                If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Or Session("EstadoPantalla") = "Consultar" Then
                    If Not Session("SS_DatosModal") Is Nothing Then
                        txtISIN.Text = CType(Session("SS_DatosModal"), String())(0).ToString.Trim()
                        txtSBS.Text = CType(Session("SS_DatosModal"), String())(1).ToString.Trim()
                        txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(2).ToString.Trim()
                        ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3).ToString.Trim()
                        ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4).ToString.Trim()
                        lblMoneda.Text = CType(Session("SS_DatosModal"), String())(4).ToString.Trim()
                        txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6).ToString.Trim()
                        Session.Remove("SS_DatosModal")
                    End If

                    If Session("Busqueda") = 0 Then
                        txtCodigoOrden.Value = ""
                        Dim strAux As String = String.Empty
                        If (Me.hdPagina.Value = "DA") Then
                            tbFechaOperacion.Text = Request.QueryString("Fecha")
                            strAux = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text).ToString()
                        End If
                        Dim strAccion As String = ""
                        If Session("EstadoPantalla") = "Modificar" Then
                            strAccion = "M"
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            strAccion = "E"
                        ElseIf Session("EstadoPantalla") = "Consultar" Then
                            strAccion = "C"
                        End If
                        ShowDialogPopupInversionesRealizadas(Me.txtISIN.Text.ToString.Trim, Me.txtSBS.Text.ToString.Trim, Me.txtMnemonico.Text.ToString.Trim,
                        ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, ddlOperacion.SelectedValue, lblMoneda.Text.ToString, strAux, strAccion, "2")
                        Session("Busqueda") = 2
                    Else
                        If Session("Busqueda") = 1 Then
                            CargarCaracteristicasValor()
                            CargarDatosOrdenInversion(Session("Orden"))
                            CargarTipoTasa()
                            Me.btnAceptar.Visible = True
                            Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                            If Session("EstadoPantalla") = "Modificar" Then
                                If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + txtCodigoOrden.Value + "?", "SI")
                                Else
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF16", "Nro " + txtCodigoOrden.Value + "?", "SI")
                                End If
                                CargarPaginaModificar()
                            ElseIf Session("EstadoPantalla") = "Eliminar" Then
                                If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF3", "Nro " + txtCodigoOrden.Value + "?", "SI")
                                Else
                                    UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF17", "Nro " + txtCodigoOrden.Value + "?", "SI")
                                End If
                                CargarPaginaEliminar()
                            ElseIf Session("EstadoPantalla") = "Consultar" Then
                                CargarPaginaConsultar()
                            End If
                        Else
                            Session("Busqueda") = 0
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se centraliza el Try ... Catch  en uno solo por evento | 11/06/18 
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        LimpiarSesiones()
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        If (Me.hdPagina.Value <> "DA") Then
            tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Else
            tbFechaOperacion.Text = Request.QueryString("Fecha")
        End If
        Me.txtHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        hdMensaje.Value = "el Ingreso"
        ViewState("IsIndica") = True
        hdNumUnidades.Value = 0
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - CERTIFICADO DE DEPÓSITO"
        chkRecalcular.Checked = True
        HabilitarBotonGuardarPreOrden()
    End Sub
    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        ViewState("IsIndica") = False
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        Dim accionRpta As String = String.Empty
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
        Try
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And hdPagina.Value <> "TI" And Me.hdPagina.Value <> "MODIFICA" Then
                    If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                        ModificarOrdenInversion()
                        UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlFondo.SelectedValue.Trim,
                   DatosRequest, ddlPlaza.SelectedValue)
                        Session("Modificar") = 0
                        CargarPaginaAceptar()
                    End If
                    If Me.hdPagina.Value = "XO" Then
                        oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(txtCodigoOrden.Value, ddlFondo.SelectedValue, Me.DatosRequest)
                        ReturnArgumentShowDialogPopup()
                    Else
                        If Me.hdPagina.Value = "EO" Then
                            oOrdenInversionWorkFlowBM.EjecutarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, txtCodigoOrden.Value, Me.DatosRequest)
                            ReturnArgumentShowDialogPopup()
                        Else
                            If Me.hdPagina.Value = "CO" Then
                                oOrdenInversionWorkFlowBM.ConfirmarOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, txtCodigoOrden.Value, Me.DatosRequest)
                                ActualizaDatosCarta()
                                ReturnArgumentShowDialogPopup()
                            End If
                        End If
                    End If
                Else
                    If hdPagina.Value = "" Or hdPagina.Value = "TI" Or hdPagina.Value = "DA" Or hdPagina.Value = "MODIFICA" Then
                        If ObtieneCustodiosSaldos() = False Then
                            Exit Sub
                        End If
                        If Session("EstadoPantalla") = "Ingresar" Then
                            If Session("NegociacionRentaFija") IsNot Nothing Then
                                If Session("Procesar") = 1 Then
                                    'Dim strcodigoOrden As String
                                    'strcodigoOrden = InsertarOrdenInversion()
                                    'oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                                    'If UIUtility.ObtenerFechaNegocio(Me.ddlFondo.SelectedValue) = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text) Then
                                    '    oOrdenInversionWorkFlowBM.EnviarCXPCDPH(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                                    'End If
                                    'Me.txtCodigoOrden.Value = strcodigoOrden
                                    'Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                    'GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), Me.ddlFondo.SelectedValue, "CERTIFICADO DE DEPOSITO", ddlOperacion.SelectedItem.Text,
                                    'Session("CodigoMoneda"), txtISIN.Text.Trim, txtSBS.Text.Trim, txtMnemonico.Text, ddlFondo.SelectedItem.Text, ddlFondo.SelectedItem.Value)
                                    'Session("CodigoOI") = strcodigoOrden

                                    'INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                                    GuardarPreOrden()
                                    accionRpta = "Ingresó"
                                    CargarPaginaInicio()
                                    'FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                                End If
                            Else
                                AlertaJS("De click en el botón Procesar para generar los datos de la negociación")
                            End If
                        Else
                            Dim strAlerta As String = ""
                            If ddlMotivoCambio.SelectedIndex <= 0 Then
                                strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.<br>"
                            End If
                            If txtComentarios.Text.Trim.Length <= 0 Then
                                strAlerta += "-Ingrese los comentarios por el cual desea " & Session("EstadoPantalla") & " esta operación."
                            End If
                            If strAlerta.Length > 0 Then
                                AlertaJS(strAlerta)
                                Exit Sub
                            End If
                            If Session("EstadoPantalla") = "Modificar" Then
                                ModificarOrdenInversion()
                                FechaEliminarModificarOI("M")
                                CargarPaginaAceptar()
                                Session("dtdatosoperacion") = ObtenerDatosOperacion()
                                If Me.hdPagina.Value <> "MODIFICA" Then
                                    GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "CERTIFICADO DE DEPOSITO", Me.ddlOperacion.SelectedItem.Text,
                                    Session("CodigoMoneda"), txtISIN.Text, txtSBS.Text.Trim, txtMnemonico.Text.Trim, ddlFondo.SelectedItem.Text, ddlFondo.SelectedItem.Value)
                                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                    accionRpta = "Modificó"
                                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                                Else
                                    ReturnArgumentShowDialogPopup()
                                End If
                            ElseIf Session("EstadoPantalla") = "Eliminar" Then
                                EliminarOrdenInversion()
                                FechaEliminarModificarOI("E")
                                CargarPaginaAceptar()
                                CargarPaginaAccion()
                                HabilitaDeshabilitaCabecera(False)
                                Me.lblAccion.Text = ""
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Eliminar | 11/06/18 
                                accionRpta = "Eliminó"
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Eliminar | 11/06/18 
                            End If
                        End If
                        If Session("Procesar") = 0 And (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar") Then
                            If CType(ViewState("MontoNeto"), String) = "" Then
                                AlertaJS(ObtenerMensaje("CONF9"))
                            Else
                                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                                retornarMensajeAccion(accionRpta)
                                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                            End If
                        Else
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                            If (Session("EstadoPantalla") = "Eliminar" Or (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar")) Then retornarMensajeAccion(accionRpta)
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                        End If
                        EliminarCuponerasOITemporales()
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedItem.Value, "CERTIFICADO DE DEPOSITO", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"),
        Me.txtISIN.Text, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text.Trim, ddlFondo.SelectedItem.Text, ddlFondo.SelectedItem.Value)
    End Sub
    Protected Sub btnCuponera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCuponera.Click
        ShowDialogCuponera(txtMnemonico.Text.ToString)
    End Sub
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            If ValidarFechas() = True Then
                If UIUtility.ValidarHora(Me.txtHoraOperacion.Text) = False Then
                    AlertaJS(ObtenerMensaje("CONF22"))
                    Exit Sub
                End If
                Session("Procesar") = 1
                Dim GUID As String = System.Guid.NewGuid().ToString()
                If hdPagina.Value = "TI" Then
                    ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
                End If

                '==== INICIO | PROYECTO FONDOS-II | CRumiche (ZOLUXIONES) | 2018-07-06 
                'Calculamos el Monto Nominal Total 
                txtMnomOrd.Text = Format(Convert.ToDecimal(txtNroPapeles.Text) * CDec(ViewState("DatosValor_ValorNominalUnitario")), "##,##0.0000000")
                txtMnomOp.Text = txtMnomOrd.Text

                Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(ViewState("DatosValor_BaseCuponAnual"))
                Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(ViewState("DatosValor_BaseCuponMensual"))
                Dim aplicacionTasa As TipoAplicacionTasa = UIUtility.ObtenerTipoAplicacionTasaDesdeCodTipoTasa(ddlTipoTasa.SelectedValue)
                Dim diasPeriodicidad As Integer = 360 'Periodicidad Anual (Un CUPON x Año)                

                'Pasamos a calcular la negociacion (El motor hará todo el trabajo en tiempo real y en la capa de Negocio)
                Dim neg As NegociacionRentaFija = UIUtility.CalcularNegociacionRentaFija(ViewState("DatosValor_DetalleCupones"),
                                                                    CDec(ViewState("DatosValor_TasaCupon")),
                                                                    diasPeriodicidad,
                                                                    CDec(ViewState("DatosValor_ValorNominalUnitario")),
                                                                    Convert.ToDecimal(txtNroPapeles.Text),
                                                                    Convert.ToDateTime(tbFechaLiquidacion.Text),
                                                                    Convert.ToDecimal(txtYTM.Text),
                                                                    baseMensual,
                                                                    baseAnual,
                                                                    CBool(ViewState("DatosValor_EsCuponADescuento")),
                                                                    CBool(ViewState("DatosValor_EsMercadoExtrangero")),
                                                                    aplicacionTasa)

                'Mostramos los resultados
                txtMontoOperacional.Text = Format(neg.ValorActual, "##,##0.00")

                txtPrecioLimpio.Text = Format(neg.PrecioLimpio * 100, "##,##0.0000000") '%: Es un Porcentaje
                txtPrecioNegSucio.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje
                lblPrecioCal.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje
                txtPrecioNegoc.Text = Format(neg.PrecioSucio * 100, "##,##0.0000000") '%: Es un Porcentaje

                txtCorrido.Text = Format(neg.InteresCorrido, "##,##0.0000")

                Session("NegociacionRentaFija") = neg
                '==== FIN | PROYECTO FONDOS-II | CRumiche (ZOLUXIONES) | 2018-07-06

                CalcularComisiones()

                '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto
                Me.txtTIRNeto.Text = Me.txtYTM.Text
                If Me.pnlTirNeto.Visible Then
                    Dim negCopia As NegociacionRentaFija = neg.Clone() 'CRumiche: Copia de la Negociación para que no afecte la original
                    If IsNumeric(Me.txtMontoNetoOpe.Text.Replace(",", "")) Then
                        If Math.Round(Convert.ToDecimal(Me.txtMontoNetoOpe.Text.Replace(",", "")), 2) <> Math.Round(negCopia.ValorActual, 2) Then
                            negCopia.PrecioSucio = Convert.ToDecimal(Me.txtMontoNetoOpe.Text.Replace(",", "")) / negCopia.CuponVigente.SaldoNominalInicial ' Se determina el nuevo Precio Sucio
                            negCopia.CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio()
                        End If
                    End If
                    Me.txtTIRNeto.Text = Format(negCopia.YTM * 100, "##,##0.0000000") '%: Es un Porcentaje 
                End If
                '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto

                CargarPaginaProcesar()
            End If

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If Me.txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text + "&vOI=T", "10", 1038,
            600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '  Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        '  Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If Not Page.IsPostBack Then
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
            hdRptaConfirmar.Value = "NO"
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 07/06/18 
            Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            LimpiarSesiones()
            If Not Request.QueryString("PTNeg") Is Nothing Then
                Me.hdPagina.Value = Request.QueryString("PTNeg")
            End If
            'If (Me.hdPagina.Value = "TI") Then
            '    UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
            'Else
            '    UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
            'End If
            'UIUtility.CargarIntermediariosOI(ddlIntermediario)
            CargarCombos()
            CargarPaginaInicio()
            Me.hdPagina.Value = ""
            DivDatosCarta.Visible = False
            DivObservacion.Visible = False
            'HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
            If Not Request.QueryString("PTNeg") Is Nothing Then
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo Mercado cuando se llama de otra ventana | 25/06/18 
                EjecutarJS("$('#divMercado').removeAttr('style');")
                CargarPlaza()
                Me.chkRecalcular.Enabled = True
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo Mercado cuando se llama de otra ventana | 25/06/18 
                Me.hdPagina.Value = Request.QueryString("PTNeg")
                If Me.hdPagina.Value = "TI" Then  'Viene de la Pagina Traspaso de Instrumentos

                    Me.txtMnemonico.Text = Request.QueryString("PTCMnemo")
                    Me.ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                    Me.ddlFondoDestino.SelectedValue = Request.QueryString("PTFondoD")
                    Me.txtISIN.Text = Request.QueryString("PTISIN")
                    Me.txtSBS.Text = Request.QueryString("PTSBS")
                    Me.lblMoneda.Text = Request.QueryString("PTMon")
                    Me.ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                    Me.hdCustodio.Value = Request.QueryString("PTCustodio")
                    Me.hdSaldo.Value = Request.QueryString("PTSaldo")
                    CargarCaracteristicasValor()
                    OrdenInversion.ObtieneImpuestosComisiones(dgLista, ddlPlaza.SelectedValue, Session("TipoRenta"), ddlIntermediario.SelectedValue)
                    ControlarCamposTI()
                    txtPrecioNegSucio.Text = "0.0000"
                    txtNroPapeles.Text = "0.0000000"
                    Me.lblPrecioCal.Text = "0.0000"
                    Me.txtPrecioNegoc.Text = "0.0000"
                    txtMontoOperacional.Text = "0.00"
                    Me.txtYTM.Text = "0.0000000"
                    Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                Else
                    Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                    Session("CodUsuario") = txtCodigoOrden.Value
                    Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                    Session("CodOrden") = txtCodigoOrden.Value
                    'ValidaOrigen()
                    If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then
                        CargarDatosOrdenInversion(Request.QueryString("PTNOrden"))
                        CargarCaracteristicasValor()
                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        ControlarCamposEO_CO_XO()
                        CargarPaginaModificarEO_CO_XO(Me.hdPagina.Value)
                        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                        If hdPagina.Value = "CO" Then
                            btnAceptar.Text = "Grabar y Confirmar"
                            DivObservacion.Visible = True
                            DivDatosCarta.Visible = True
                            If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then
                                CargarPaginaInicio()
                                btnCaracteristicas.Visible = True
                            End If
                        End If
                        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                    Else
                        If (Me.hdPagina.Value = "OE") Then
                            ControlarCamposOE()
                        Else
                            If (Me.hdPagina.Value = "DA") Then
                                ViewState("ORDEN") = "OI-DA"
                                Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                                Me.tbFechaOperacion.ReadOnly = True
                            Else
                                If (Me.hdPagina.Value = "CP") Then
                                    Call ConfiguraModoConsulta()
                                    ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                    txtMnemonico.Text = Request.QueryString("Mnemonico")
                                    txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                    Call CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                    Call CargarCaracteristicasValor()
                                    UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                    Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, False, True, False, False)
                                Else
                                    If (Me.hdPagina.Value = "CONSULTA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                        ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                        CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                        CargarCaracteristicasValor()
                                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                        HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False, False, False)
                                        HabilitaDeshabilitaCabecera(False)
                                    Else
                                        If (Me.hdPagina.Value = "MODIFICA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                            ConfiguraModoConsulta()
                                            ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                            txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                            Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                            CargarDatosOrdenInversion(Request.QueryString("CodigoOrden"))
                                            CargarCaracteristicasValor()
                                            UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                            HabilitaBotones(False, False, False, False, False, False, False, True, False, True, False, True, False, False, False)
                                            HabilitaDeshabilitaCabecera(False)
                                            HabilitaDeshabilitaDatosOperacionComision(True)
                                            Session("EstadoPantalla") = "Modificar"
                                            lblAccion.Text = "Modificar"
                                            hdMensaje.Value = "la Modificación"
                                            EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                                            HelpCombo.CargarMotivosCambio(Me)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                CargarTipoTasa()

                '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto
                Me.pnlTirNeto.Visible = (Me.hdPagina.Value <> "") 'Solo será visible si hdPagina.Value tiene un valor
                '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-21 | Calculo del TIR Neto

                Me.btnSalir.Attributes.Remove("onClick")
                '   Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                ' Me.btnAceptar.Attributes.Add("onClick", "if (Confirmacion()){this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');}")
                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
            End If
        Else
            If Session("SS_DatosModal") IsNot Nothing Then ObtenerValoresDesdePopup()
            If Not Session("EstadoPantalla") Is Nothing Then
                If Session("EstadoPantalla").ToString() = "Modificar" Or Session("EstadoPantalla").ToString() = "Eliminar" Then
                    EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
                    If ddlMotivoCambio.Items.Count = 0 Then HelpCombo.CargarMotivosCambio(Me)
                End If
            End If
            If Not Request.QueryString("PTNeg") Is Nothing Then
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo Mercado cuando se llama de otra ventana | 25/06/18 
                EjecutarJS("$('#divMercado').removeAttr('style');")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo Mercado cuando se llama de otra ventana | 25/06/18 
            End If
        End If
    End Sub
    Sub ObtenerValoresDesdePopup()
        Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
        If hfModal.Value = "1" Then
            txtISIN.Text = datosModal(0)
            txtMnemonico.Text = datosModal(1)
            'OT 9968 - 14/02/2017 - Carlos Espejo
            'Descripcion: Se agrega la sesion Nemonico
            Session("Nemonico") = datosModal(1)
            'OT 9968 Fin
            txtSBS.Text = datosModal(2)
            hdCustodio.Value = datosModal(3)
            hdSaldo.Value = datosModal(4)
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga valor devuelto en campo Moneda  | 08/06/18 
            lblMoneda.Text = datosModal(5)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga valor devuelto en campo Moneda  | 08/06/18 
        ElseIf hfModal.Value = "2" Then
            txtISIN.Text = datosModal(0)
            txtSBS.Text = datosModal(1)
            txtMnemonico.Text = datosModal(2)
            'OT 9968 - 14/02/2017 - Carlos Espejo
            'Descripcion: Se agrega la sesion Nemonico
            Session("Nemonico") = datosModal(2)
            'OT 9968 Fin
            ddlFondo.SelectedValue = datosModal(3)
            ddlOperacion.SelectedValue = datosModal(4)
            lblMoneda.Text = datosModal(5)
            txtCodigoOrden.Value = datosModal(6)
            'OT 10019 - 22/02/2017 - Carlos Espejo
            'Descripcion: Se gurda la orden en una session
            Session("Orden") = datosModal(6)
            'OT 10019 Fin
        End If
        Session.Remove("SS_DatosModal")
    End Sub
    Protected Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
        CargarFechaVencimiento()
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If ddlFondo.SelectedValue <> "" Then
            CargarFechaVencimiento()
        End If
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If cantidadreg > 0 Then
            AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
            ddlFondo.SelectedIndex = 0
            Exit Sub
        End If

        Dim portafolioBM As New PortafolioBM()
        Dim dt As DataTable = portafolioBM.PortafolioSelectById(ddlFondo.SelectedValue)
        Dim fecha As Decimal = 0

        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                fecha = CDec(dt.Rows(0)("FechaNegocio").ToString())

                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(fecha)
                tbFechaLiquidacion.Text = tbFechaOperacion.Text
            End If
        End If
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa evento Click de botón Salir para mostrar Confirmación de acuerdo a Ventana de llamado  | 07/06/18 
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Dim strMensajeConfirmacion As String = ""
        Dim Pagina As String = hdPagina.Value
        Dim NroOrden As String = txtCodigoOrden.Value
        Dim strAccion As String = hdMensaje.Value

        Select Case Pagina
            Case "TI"
                strMensajeConfirmacion = "¿Desea cancelar el Traspaso de Instrumento?"
            Case "EO"
                strMensajeConfirmacion = "¿Desea cancelar la Ejecución de la orden de inversión Nro. " + NroOrden + "?"
            Case "CO"
                strMensajeConfirmacion = "¿Está seguro de salir de la Confirmación de la orden de inversión Nro. " + NroOrden + "?"
            Case "XO"
                strMensajeConfirmacion = "¿Desea cancelar el Extorno de la orden de inversión Nro. " + NroOrden + "?"
            Case "OE"
                strMensajeConfirmacion = "¿Desea cancelar la Aprobacion de la orden de inversión Excedida Nro. " + NroOrden + "?"
            Case "DA"
                strMensajeConfirmacion = "¿Desea cancelar la Negociación de la orden de inversión Nro. " + NroOrden + "?"
            Case Else
                If (strAccion <> String.Empty) Then
                    If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la orden de inversión de Certificado de Depósito?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Certificado de Depósito?"
                    End If
                End If
        End Select

        If (strMensajeConfirmacion <> String.Empty) Then
            If hdRptaConfirmar.Value.ToUpper = "NO" Then
                ConfirmarJS(strMensajeConfirmacion, "document.getElementById('hdRptaConfirmar').value = 'SI'; document.getElementById('btnSalir').click(); ")
            Else
                hdRptaConfirmar.Value = "NO"
                If strAccion <> String.Empty Then
                    Response.Redirect("~/frmDefault.aspx")
                Else
                    EjecutarJS("window.close()")
                End If
            End If
        Else
            If (Pagina = "CONSULTA" Or Pagina = "MODIFICA") Then EjecutarJS("window.close()")
        End If
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa evento Click de botón Salir para mostrar Confirmación de acuerdo a Ventana de llamado  | 07/06/18 

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Procedimiento para cargar los valores de Plaza (Mercado) | 25/06/18 
    Public Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlPlaza.DataSource = oPlazaBM.Listar(Nothing)
        ddlPlaza.DataTextField = "Descripcion"
        ddlPlaza.DataValueField = "CodigoPlaza"
        ddlPlaza.DataBind()
        ddlPlaza.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
    Protected Sub ddlPlaza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaza.SelectedIndexChanged
        Session("Mercado") = ddlPlaza.SelectedValue
        dgLista.Dispose()
        dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Procedimiento para cargar los valores de Plaza (Mercado) | 25/06/18 

    Public Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If Me.hdPagina.Value = "TI" Then
            If chkRecalcular.Checked Then
                dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoOperacional.Text.Replace(",", ""), Me.txtNroPapeles.Text.Replace(",", ""))
            Else
                dblTotalComisiones = UIUtility.CalculaImpuestosComisionesNoRecalculo(dgLista, Session("Mercado"), txtMontoOperacional.Text.Replace(",", ""),
           txtNroPapeles.Text.Replace(",", ""), ddlIntermediario.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_DEPOSITOPLAZO)
            End If
        Else
            If chkRecalcular.Checked Then
                dblTotalComisiones = UIUtility.CalcularComisionesYLlenarGridView(dgLista, String.Empty, txtMontoOperacional.Text.Replace(",", ""), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text, ddlIntermediario.SelectedValue)
            Else
                dblTotalComisiones = UIUtility.CalculaImpuestosComisionesNoRecalculo(dgLista, Session("Mercado"), txtMontoOperacional.Text.Replace(",", ""),
           txtNroPapeles.Text.Replace(",", ""), ddlIntermediario.SelectedValue, ddlOperacion.SelectedValue, CLASE_INSTRUMENTO_DEPOSITOPLAZO)
            End If
            '    dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoOperacional.Text.Replace(",", ""), Me.txtNroPapeles.Text.Replace(",", ""))
        End If
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        If (ddlOperacion.SelectedValue = "2") Then
            txtMontoNetoOpe.Text = Format(txtMontoOperacional.Text.Replace(",", "") - dblTotalComisiones, "##,##0.0000000")
        Else
            txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoOperacional.Text.Replace(",", ""), "##,##0.0000000")
        End If
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        'Dim strAccAux As String = txtNroPapeles.Text.Replace(",", "")
        'strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
        'txtPrecPromedio.Text = Format(Convert.ToDecimal(strMontoAux) / Convert.ToDecimal(strAccAux), "##,##0.0000000")
    End Sub

    Private Sub CargarCombos()
        CargarComboCategoriaContable()
        CargarComboOperacionOIParaTraspaso()
        UIUtility.CargarIntermediariosOI(ddlIntermediario)
        HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
    End Sub

    Private Sub CargarComboCategoriaContable()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtParams As DataTable = oParametrosGenerales.Listar(ParametrosSIT.TIPO_VALORIZACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlCategoriaContable, dtParams, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarComboOperacionOIParaTraspaso()
        If (Me.hdPagina.Value = "TI") Then
            UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
        Else
            UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
        End If
    End Sub

    Private Function ObtenerTipoFondo() As String
        ObtenerTipoFondo = String.Empty
        Dim portafolioBM As New PortafolioBM()
        Dim dt As DataTable = portafolioBM.PortafolioSelectById(ddlFondo.SelectedValue)
        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                ObtenerTipoFondo = dt.Rows(0)("TipoNegocio").ToString()
            End If
        End If
    End Function

#Region "Registro En la Preorden"

    Sub GuardarPreOrden()
        'INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
        Dim neg As NegociacionRentaFija = Session("NegociacionRentaFija")

        Dim entPreOrden As New PrevOrdenInversionBE
        Dim negPreOrden As New PrevOrdenInversionBM
        Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(entPreOrden.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)

        negPreOrden.InicializarPrevOrdenInversion(oRow)

        oRow.CodigoPrevOrden = 0
        oRow.CodigoOperacion = ddlOperacion.SelectedValue 'Compra/Venta/Etc.
        oRow.CodigoNemonico = txtMnemonico.Text
        oRow.IndPrecioTasa = "T" 'ddlModoNegociacion.SelectedValue 'T: Tasa YTM % , P: Precio
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)

        oRow.FechaLiquidacion = neg.FechaEvaluacion.ToString("yyyyMMdd")
        oRow.Cantidad = neg.CantidadUnidadesNegociadas
        oRow.CantidadOperacion = neg.CantidadUnidadesNegociadas
        oRow.TipoTasa = ddlTipoTasa.SelectedValue
        oRow.Tasa = neg.YTM * 100 'Es un Porcentaje
        oRow.Precio = neg.PrecioSucio * 100 'Es un Porcentaje
        oRow.PrecioOperacion = neg.PrecioSucio * 100 'Es un Porcentaje
        oRow.MontoNominal = neg.ValorNominal
        oRow.MontoOperacion = neg.ValorActual
        oRow.InteresCorrido = neg.InteresCorrido

        oRow.CodigoPlaza = "7" 'Por defecto 7:'LIMA' ---- ddlPlaza.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.HoraOperacion = Now.ToLongTimeString()

        'Valores por Defecto          
        oRow.MedioNegociacion = "E" 'Por defecto 'ELECTRONICO'
        oRow.TipoFondo = "Normal" 'Por defecto 'NORMAL'
        oRow.TipoTramo = "AGENCIA" 'Por defecto 'AGENCIA'
        oRow.TipoCondicion = "AM" 'Por defecto 'A MERCADO'
        oRow.Porcentaje = "N" 'N: No Porcentaje, solo Monto directo
        oRow.Fixing = 0 'Por defecto 

        oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
        oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO

        ' INICIO | Proyecto SIT Fondos - Mandato | Ian Pastor M. | 2018-09-21 | Categoría Contable
        oRow.TipoValorizacion = ddlCategoriaContable.SelectedValue
        ' FIN | Proyecto SIT Fondos - Mandato | Ian Pastor M. | 2018-09-21 | Categoría Contable

        entPreOrden.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
        entPreOrden.PrevOrdenInversion.AcceptChanges()

        Dim dtAsignacion As New DataTable ' Asignacion Por Fondo
        dtAsignacion.Columns.Add("CodigoPortafolio")
        dtAsignacion.Columns.Add("Asignacion")
        'Solo necesitamos una Fila donde se indicará el 100% de unidades para el Fondo
        dtAsignacion.Rows.Add(ddlFondo.SelectedValue, neg.CantidadUnidadesNegociadas)

        'Guardamos la Pre-Orden
        negPreOrden.Insertar(entPreOrden, ParametrosSIT.TR_RENTA_FIJA.ToString(), DatosRequest, dtAsignacion)

        HabilitarBotonesAccion()
        'retornarMensajeAccion("Ingresó") 'Notificamos

        'FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
    End Sub

    Sub HabilitarBotonGuardarPreOrden()
        Me.btnIngresar.Visible = False
        Me.btnModificar.Visible = False
        Me.btnEliminar.Visible = False
        Me.btnConsultar.Visible = False
        Me.btnAceptar.Visible = True
    End Sub

    Sub HabilitarBotonesAccion()
        Me.btnIngresar.Visible = True
        Me.btnModificar.Visible = True
        Me.btnEliminar.Visible = True
        Me.btnConsultar.Visible = True

        Me.btnAceptar.Visible = False
    End Sub

    Sub ValidaOrigen()
        Dim objNeg As New OrdenInversionWorkFlowBM()
        If (objNeg.OrdenInversion_ValidaExterior(txtCodigoOrden.Value.ToString()).Equals("EXT")) Then
            lkbMuestraModalDatos.Enabled = True
        Else
            lkbMuestraModalDatos.Enabled = False
        End If
    End Sub

    Sub ActualizaDatosCarta()
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM()
        If (lkbMuestraModalDatos.Enabled = True) Then

            If Not IsDBNull(Session("ObjDatosCarta")) Then
                Dim objEnt As New DatosCartasBE()
                objEnt = CType(Session("ObjDatosCarta"), DatosCartasBE)
                oOrdenInversionWorkFlowBM.ActualizaDatosCarta(txtCodigoOrden.Value, objEnt.NumeroCuenta, objEnt.ValorTipo)
            End If

        End If
    End Sub
#End Region

End Class