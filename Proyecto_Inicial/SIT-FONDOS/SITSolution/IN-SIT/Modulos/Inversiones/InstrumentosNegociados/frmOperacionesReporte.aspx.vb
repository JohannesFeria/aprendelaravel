Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
'OT 10090 - 26/07/2017 - Carlos Espejo
'Se quitaron los comentarios y se ordeno la clase
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmOperacionesReporte
    Inherits BasePage
#Region "Variables"
    Dim objPortafolio As New PortafolioBM
    Dim objTipoOperacion As New TipoOperacionBM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim objutil As New UtilDM
    Dim objtercero As New TercerosBM
    Dim objMoneda As New MonedaBM
    Dim objferiadoBM As New FeriadoBM
    Dim strTipoRenta As String
    Dim oValoresBM As New ValoresBM
#End Region
#Region "Rutinas"
    Dim txtTipoCambio As Object
    Private Sub CargarMoneda()
        Dim oMoneda As New MonedaBM
        Dim dt As DataTable = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO).Tables(0)
        HelpCombo.LlenarComboBox(ddlmoneda, dt, "CodigoMoneda", "Descripcion", True, "SELECCIONE")
    End Sub
    Sub CargaComisiones(ByVal CodigoOrden As String)
        Dim dtComi As DataTable = oOrdenInversionBM.Seleccionar_ComisionesOR(CodigoOrden)
        If Not dtComi.Rows.Count = 0 Then
            txtimpuestocompra.Text = dtComi.Rows(0)("ImpuestoCompra")
            txtimpuestoventa.Text = dtComi.Rows(0)("ImpuestoVenta")
            txtcomisionventa.Text = dtComi.Rows(0)("ComisionAIVenta")
            txtcomisioncompra.Text = dtComi.Rows(0)("ComisionAICompra")
            txtrestocompra.Text = dtComi.Rows(0)("RestoComisionCompra")
            txtrestoventa.Text = dtComi.Rows(0)("RestoComisionVenta")
        End If
    End Sub
    Sub ModificarComisiones(ByVal CodigoOrden As String)
        oOrdenInversionBM.Modificar_ComisionesOR(CodigoOrden, txtimpuestocompra.Text, txtimpuestoventa.Text, txtcomisionventa.Text, txtcomisioncompra.Text,
        txtrestocompra.Text, txtrestoventa.Text)
    End Sub
    Sub InsertarComisiones(ByVal CodigoOrden As String)
        oOrdenInversionBM.Insertar_ComisionesOR(CodigoOrden, txtimpuestocompra.Text, txtimpuestoventa.Text, txtcomisionventa.Text, txtcomisioncompra.Text,
        txtrestocompra.Text, txtrestoventa.Text)
    End Sub
    Private Sub CargarPaginaConsultar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacion(False)
        CargarPaginaProcesar()
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaEliminar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacion(False)
        CargarPaginaProcesar()
        Me.lblComentarios.InnerHtml = "Comentarios eliminación:"
        Me.txtComentarios.Text = ""
    End Sub
    Private Sub CargarPaginaModificar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacion(True)
        Me.lblComentarios.InnerHtml = "Comentarios modificación:"
        Me.txtComentarios.Text = ""
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal operacion As String,
    ByVal moneda As String, ByVal categoria As String, ByVal fecha As String, ByVal accion As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&cFondo=" + ddlFondo.SelectedValue +
        "&vFondo=" + ddlFondo.SelectedItem.Text + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=OR"
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='2'; ")
    End Sub
    Private Sub CargarValores()
        Dim dtValores As DataTable
        dtValores = oValoresBM.SeleccionarPorFiltro("", "", txtMnemonico.Text.Trim(), "", "", "", MyBase.DatosRequest).Tables(0)
        Session("datosValores") = dtValores
    End Sub
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal operacion As String, ByVal categoria As String,
    ByVal valor As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & ddlFondo.SelectedValue & "&vFondo=" &
        ddlFondo.SelectedItem.Text & "&vOperacion=" & operacion & "&vCategoria=" & categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='" & valor & "';")
    End Sub
    Private Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(Session("CodigoOrden"), Me.ddlFondo.SelectedValue, "", DatosRequest)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Session("CodigoOrden"), Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc,
        txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        btnAceptar.Enabled = True
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            strJS.AppendLine("$('#btnImprimir').show();")
            strJS.AppendLine("$('#btnImprimir').removeAttr('disabled');")
        End If
        EjecutarJS(strJS.ToString())
    End Sub
    Private Function ValidarFechaVencimiento() As Boolean
        Try
            Dim dia As String = ""
            Dim Fecha As Date = Convert.ToDateTime(Me.tbFechaInicioCOMPRA.Text).AddDays(ToNullDecimal(Me.tbPlazoVenCOMPRA.Text))
            If Fecha.DayOfWeek = DayOfWeek.Saturday Then
                dia = "Sábado"
            ElseIf Fecha.DayOfWeek = DayOfWeek.Sunday Then
                dia = "Domingo"
            ElseIf ddlIntermediario.SelectedIndex > 0 Then
                dia = New UtilDM().ValidarFechaHabil(Fecha.ToString("yyyyMMdd"), ddlIntermediario.SelectedValue)
            End If
            If dia.Equals("") Then
                EjecutarJS("document.getElementById('tbFechaVenCOMPRA').value= '" + Fecha.ToString("dd/MM/yyyy") + "'")

                '   tbFechaVenCOMPRA.Text = Fecha.ToString("dd/MM/yyyy")
                Return True
            Else
                AlertaJS("La fecha de la Operacion a Plazo cae " & dia & ". Debe cambiar los días de plazo Reporte.")
                Return False
            End If
        Catch ex As Exception
            AlertaJS("La fecha de la Operacion a Plazo cae. Debe cambiar los días de plazo Reporte.")
            Return False
        End Try
    End Function
    Private Sub CalcularTotales()
        Dim TotalComisionesCOMPRA As Decimal = 0
        Dim TotalComisionesVENTA As Decimal = 0
        txtimpuestocompra.Text = IIf(txtimpuestocompra.Text = "", "0", txtimpuestocompra.Text)
        txtcomisioncompra.Text = IIf(txtcomisioncompra.Text = "", "0", txtcomisioncompra.Text)
        txtrestocompra.Text = IIf(txtrestocompra.Text = "", "0", txtrestocompra.Text)
        txtimpuestoventa.Text = IIf(txtimpuestoventa.Text = "", "0", txtimpuestoventa.Text)
        txtcomisionventa.Text = IIf(txtcomisionventa.Text = "", "0", txtcomisionventa.Text)
        txtrestoventa.Text = IIf(txtrestoventa.Text = "", "0", txtrestoventa.Text)
        TotalComisionesCOMPRA = CDec(txtimpuestocompra.Text) + CDec(txtcomisioncompra.Text) + CDec(txtrestocompra.Text)
        TotalComisionesVENTA = CDec(txtimpuestoventa.Text) + CDec(txtcomisionventa.Text) + CDec(txtrestoventa.Text)
        tbTotalComiCompra.Text = TotalComisionesCOMPRA
        tbTotalComiVenta.Text = TotalComisionesVENTA
    End Sub
    Private Sub Procesar()
        If ValidarFechaVencimiento() = False Then
            Exit Sub
        End If
        Dim Tasa365 As Decimal = 0
        Dim Tasa360 As Decimal = 0
        Dim MontoVcto As Decimal = 0
        Dim MontoNeto As Decimal = 0
        Dim PlazoVencimiento As Decimal = 0
        MontoVcto = IIf(tbMontoNetoVENTA.Text = "", 0, tbMontoNetoVENTA.Text)
        MontoNeto = IIf(tbMontoNetoCOMPRA.Text = "", 0, tbMontoNetoCOMPRA.Text)
        PlazoVencimiento = IIf(tbPlazoVENTA.Text = "", 0, tbPlazoVENTA.Text)
        Tasa365 = (Math.Pow((MontoVcto / MontoNeto), (360 / PlazoVencimiento)) - 1) * 100
        tbTasa365.Text = Tasa365
        tbRendimientos.Text = MontoVcto - MontoNeto
        Tasa360 = (Math.Pow((1 + (Tasa365 / 100)), (360 / 360)) - 1) * 100
        EjecutarJS("document.getElementById('tbTasa360').value= " + Math.Round(Tasa360, 7).ToString())
        '   tbTasa360.Text = Tasa360
        tbCantidadVENTA.Text = tbCantidadCOMPRA.Text
        CalcularTotales()
        ViewState("estadoOI") = ""
        ViewState("GUID_Limites") = System.Guid.NewGuid.ToString()
        'Comisiones se ingresan manualmente para esta Instumento
        'Limite no desarrollado para Operaciones de Reporte
        'OrdenInversion.CalculaLimitesOnLine(Me, DatosRequest, ViewState("estadoOI"), ViewState("GUID_Limites").ToString())
        Session("Procesar") = 1
        CargarPaginaProcesar()
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String,
    ByVal mnemonico As String, ByVal mnemonicotemp As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&cportafolio=" + ddlFondo.SelectedValue + "&vportafolio=" +
        ddlFondo.SelectedItem.Text + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" +
        mnemonico + "&vnemonicotemp=" + mnemonicotemp, "10", 1000, 650, 0, 0, "No", "Yes", "Yes", "Yes"), False)
    End Sub
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11",
        "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Cantidad"
        drGrilla("v1") = tbCantidadCOMPRA.Text
        drGrilla("c2") = "Valor Nominal"
        drGrilla("v2") = tbMontoNetoCOMPRA.Text
        drGrilla("c3") = "Tasa (360) %"
        drGrilla("v3") = tbTasa360.Text
        drGrilla("c4") = "Fecha Inicio"
        drGrilla("v4") = tbFechaInicioCOMPRA.Text
        drGrilla("c5") = "Plazo Reporte"
        drGrilla("v5") = tbPlazoVenCOMPRA.Text
        drGrilla("c6") = "Tasa (360) %"
        drGrilla("v6") = tbTasa365.Text
        drGrilla("c7") = "Subyacente"
        drGrilla("v7") = txtMnemonico.Text
        drGrilla("c8") = "Rendimiento"
        drGrilla("v8") = tbRendimientos.Text
        drGrilla("c9") = "Monto Neto Vencimiento"
        drGrilla("v9") = tbMontoNetoVENTA.Text
        drGrilla("c10") = "Calificación"
        drGrilla("v10") = ddlClasificacion.SelectedItem
        drGrilla("c11") = ""
        drGrilla("v11") = ""
        drGrilla("c12") = ""
        drGrilla("v12") = ""
        drGrilla("c13") = ""
        drGrilla("v13") = ""
        drGrilla("c14") = ""
        drGrilla("v14") = ""
        drGrilla("c15") = ""
        drGrilla("v15") = ""
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        drGrilla("c21") = ""
        drGrilla("v21") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Function InsertarOrdenInversion() As String
        'Ojo con strCodigoReporte que no tiene valor pero se considera como parametro, dentro de la funcion se crea un viewstate con este valor vacio.
        Dim strCodigoOI, strCodigoOI_T As String
        Dim oValoresBM As New ValoresBM
        Dim strCodigoReporte As String = ""
        oOrdenInversionBE = crearObjetoOI(strCodigoReporte)
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        If Me.hdPagina.Value = "TI" Then
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoOperacion") = UIUtility.ObtenerCodigoOperacionTIngreso().ToString()
            Session("ValorCustodio") = UIUtility.ObtieneUnCustodio(Session("ValorCustodio"))
            strCodigoOI_T = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
            ViewState("CodigoOrden_T") = "-" + strCodigoOI_T
        Else
            ViewState("CodigoOrden_T") = ""
        End If
        Return strCodigoOI
    End Function
    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacion(False)
        If Session("EstadoPantalla") = "Ingresar" Then
            Me.btnImprimir.Visible = True
            Me.btnImprimir.Enabled = True
        End If
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub ReturnArgumentShowDialogPopup()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 06/06/18
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
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se modifica el retorno de mensaje de confirmación | 06/06/18
    End Sub
    Public Function crearObjetoOI(ByVal strCodigoReporte As String) As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        Dim objutil As New UtilDM
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = txtCodigoOrden.Value.Trim
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoMoneda = ddlmoneda.SelectedValue
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = txtSBS.Text
        oRow.CategoriaInstrumento = "OR"
        oRow.Situacion = "A"
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.Observacion = tbObservacion.Text
        oRow.CodigoTipoCupon = "2"
        '----------------------Operacion Contado---------------------------
        'Existen campos de BD que se ha usado sin necesariamente tener el mismp nombre que el campo
        'Caso del Monto sin interes que se almacena en MontoNominalOrdenado
        oRow.CantidadOperacion = CDec(tbCantidadCOMPRA.Text)
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaInicioCOMPRA.Text)
        oRow.MontoNominalOrdenado = CDec(tbMontoNetoCOMPRA.Text)
        oRow.MontoNominalOperacion = CDec(tbMontoNetoCOMPRA.Text)
        oRow.Plazo = CDec(tbPlazoVenCOMPRA.Text)
        oRow.TasaPorcentaje = CDec(Me.tbTasa360.Text)
        oRow.MontoOperacion = CDec(tbMontoNetoVENTA.Text)
        oRow.MontoNetoOperacion = CDec(tbMontoNetoVENTA.Text)
        oRow.NumeroPoliza = tbNroPoliza.Text
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaInicioCOMPRA.Text)
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.Plaza = ddlBolsa.SelectedValue
        oRow.TipoCondicion = ddlClasificacion.SelectedValue
        oRow.CodigoTipoTitulo = IIf(ddlmoneda.SelectedValue = "NSOL", "REPORTESOL", "REPORTEDOL")
        '-----------------------Operacion Plazo-----------------------
        oRow.TasaCastigo = CDec(Me.tbTasa365.Text)
        oRow.MontoCancelar = CDec(tbMontoNetoVENTA.Text)
        oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaVenCOMPRA.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquiCOMPRA.Text)
        oRow.HoraOperacion = hdHoraOperacion.Value
        If Session("EstadoPantalla") = "Ingresar" Then
            oRow.HoraOperacion = objutil.RetornarHoraSistema
        End If
        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
            If ddlMotivoCambio.SelectedIndex > 0 Then
                oRow.CodigoMotivoCambio = ddlMotivoCambio.SelectedValue
            End If
            If Session("EstadoPantalla") = "Modificar" Then
                oRow.IndicaCambio = "1"
            End If
        End If
        ViewState("CodigoReporte") = strCodigoReporte
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Private Sub ModificarOrdenInversion()
        Dim strAux As String = ""
        oOrdenInversionBE = crearObjetoOI(strAux)
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, Me.hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
    End Sub
    Protected Sub CargaPortafolioSegunUsuario(ByVal ddlFondo As DropDownList)
        Dim dtPortafolios As DataTable = New PortafolioBM().ListarPortafolioPorUsuario(Session("Login"))
        HelpCombo.LlenarComboBox(ddlFondo, dtPortafolios, "CodigoPortafolioSBS", "CodigoPortafolioSBS", True)
    End Sub
    Private Sub HabilitaBotones(ByVal bLimites As Boolean, ByVal bIngresar As Boolean, ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean,
    ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean,
    ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean)
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
    End Sub
    Private Sub LimpiarDatosOperacion()
        ddlmoneda.SelectedValue = ""
        ddlFondo.SelectedIndex = 0
        ddlOperacion.SelectedIndex = 0
        txtISIN.Text = ""
        txtSBS.Text = ""
        txtMnemonico.Text = ""
        ddlGrupoInt.SelectedIndex = 0
        ddlIntermediario.Items.Clear()
        'tbFechaLiquiCOMPRA.Text = ""
        tbFechaVenCOMPRA.Text = ""
        tbMontoNetoCOMPRA.Text = ""
        tbMontoNetoVENTA.Text = ""
        tbRendimientos.Text = ""
        tbTotalComiCompra.Text = ""
        tbTotalComiVenta.Text = ""
        tbPlazoVenCOMPRA.Text = ""
        tbPlazoVENTA.Text = ""
        tbCantidadCOMPRA.Text = ""
        tbCantidadVENTA.Text = ""
        tbTasa360.Text = ""
        tbTasa365.Text = ""
        ddlBolsa.SelectedIndex = 0
        ddlClasificacion.SelectedIndex = 0
        tbNroPoliza.Text = ""
        txtimpuestocompra.Text = "0"
        txtimpuestoventa.Text = "0"
        txtcomisionventa.Text = "0"
        txtcomisioncompra.Text = "0"
        txtrestocompra.Text = "0"
        txtrestoventa.Text = "0"
    End Sub
    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
        LimpiarDatosOperacion()
        HabilitaDeshabilitaCabecera(True)
        Me.btnBuscar.Visible = True
        Me.btnBuscar.Enabled = True
        Session("ValorCustodio") = ""
    End Sub
    Private Sub ConfiguraModoConsulta()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Consulta"
    End Sub
    Private Sub ControlarCamposOE()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Enabled = True
    End Sub
    Private Sub ControlarCamposEO_CO_XO()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Enabled = True
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            Me.btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacion(True)
            Me.btnAceptar.Enabled = True
            tbNroPoliza.Enabled = True
            If acceso = "EO" Then Session("EstadoPantalla") = "Modificar"
        End If
    End Sub
    Private Sub CargarContactos(Optional ByVal cambiaIntermediario As Boolean = False)
        Dim i As Integer
        Dim dtAux As DataTable
        dtAux = CType(Session("datosEntidad"), DataTable)
        If Not dtAux Is Nothing Then
            For i = 0 To dtAux.Rows.Count - 1
                If dtAux.Rows(i)("CodigoTercero") = ddlIntermediario.SelectedValue Then
                    If ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        hdCustodio.Value = dtAux.Rows(i)("codigoCustodio")
                    End If
                    Session("Mercado") = dtAux.Rows(i)("mercado")
                    ViewState("CodigoEntidad") = dtAux.Rows(i)("codigoentidad")
                    Exit For
                End If
            Next
        End If
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
    End Sub
    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlFondo.Enabled = estado
        ddlmoneda.Enabled = estado
        ddlOperacion.Enabled = estado
        btnBuscar.Enabled = estado
        txtSBS.ReadOnly = Not estado
        txtISIN.ReadOnly = Not estado
        txtMnemonico.ReadOnly = Not estado
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacion(ByVal estado As Boolean)
        ddlGrupoInt.Enabled = estado
        ddlIntermediario.Enabled = estado
        If Session("EstadoPantalla") = "Ingresar" Then
            ddlMotivoCambio.Enabled = Not estado
        Else
            ddlMotivoCambio.Enabled = estado
        End If
        tbPlazoVenCOMPRA.Enabled = estado
        tbCantidadCOMPRA.Enabled = estado
        tbMontoNetoCOMPRA.Enabled = estado
        ddlBolsa.Enabled = estado
        ddlClasificacion.Enabled = estado
        tbNroPoliza.Enabled = estado
        tbMontoNetoVENTA.Enabled = estado
        tbObservacion.Enabled = estado
        txtimpuestocompra.Enabled = estado
        txtimpuestoventa.Enabled = estado
        txtcomisionventa.Enabled = estado
        txtcomisioncompra.Enabled = estado
        txtrestocompra.Enabled = estado
        txtrestoventa.Enabled = estado
    End Sub
    Private Sub OcultarBotonesInicio()
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        Me.btnImprimir.Visible = False
    End Sub
    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacion(False)
        OcultarBotonesInicio()
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub MostrarOcultarBotonesAcciones(ByVal estado As Boolean)
        Me.btnIngresar.Visible = estado
        Me.btnModificar.Visible = estado
        Me.btnEliminar.Visible = estado
        Me.btnConsultar.Visible = estado
    End Sub
    Private Sub CargarFechaVencimiento()
        If (Me.hdPagina.Value <> "DA") Then
            Dim dtAux As DataTable = objPortafolio.SeleccionarPortafolioPorFiltro(Me.ddlFondo.SelectedValue, DatosRequest).Tables(0)
            If Not dtAux Is Nothing Then
                If dtAux.Rows.Count > 0 Then
                    tbFechaInicioCOMPRA.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                    tbFechaLiquiCOMPRA.Text = tbFechaInicioCOMPRA.Text
                    'ddlmoneda.SelectedValue = dtAux.Rows(0)("CodigoMoneda")
                End If
            End If
        Else
            'EjecutarJS("document.getElementById('tbFechaLiquiCOMPRA').value= " + tbFechaInicioCOMPRA.Text)
        End If
    End Sub
    Private Sub CargarPaginaBuscar()
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacion(True)
    End Sub
    Private Sub ControlarCamposTI()
        Me.lblFondo.Text = "Fondo Origen"
        MostrarOcultarBotonesAcciones(False)
        Session("ValorCustodio") = ""
        If hdPagina.Value <> "CO" Then Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        CargarFechaVencimiento()
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        CargarPaginaIngresar()
    End Sub
    Private Sub CargarDatosOrdenInversion(ByVal CodigoOrden As String)
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(CodigoOrden, Me.ddlFondo.SelectedValue, DatosRequest, PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            Session("CodigoMoneda") = oRow.CodigoMoneda
            txtISIN.Text = oRow.CodigoISIN
            txtMnemonico.Text = oRow.CodigoMnemonico
            txtSBS.Text = oRow.CodigoSBS
            txtCodigoOrden.Value = oRow.CodigoOrden
            'OT 10209 - 03/04/2017 - Carlos Espejo
            'Descripcion: El numero de poliza son los ultimos 5 digitos del codigo de Orden
            tbNroPoliza.Text = Right(oRow.CodigoOrden, 5)
            'OT 10209 Fin
            Session("CodigoOrden") = oRow.CodigoOrden
            ddlmoneda.SelectedValue = oRow.CodigoMoneda
            tbObservacion.Text = oRow.Observacion
            hdHoraOperacion.Value = oRow.HoraOperacion
            ViewState("TipoTasa") = oRow.CodigoTipoCupon
            If oRow.CodigoOperacion.ToString <> "" Then
                ddlOperacion.SelectedIndex = ddlOperacion.Items.IndexOf(ddlOperacion.Items.FindByValue(oRow.CodigoOperacion.ToString()))
            Else
                ddlOperacion.SelectedIndex = 0
            End If
            '----------------------Operacion Contado---------------------------
            'Existen campos de BD que se ha usado sin necesariamente tener el mismp nombre que el campo
            'Caso del Monto sin interes que se almacena en MontoNominalOrdenado
            tbCantidadCOMPRA.Text = oRow.CantidadOperacion
            tbMontoNetoCOMPRA.Text = oRow.MontoNominalOperacion
            tbPlazoVenCOMPRA.Text = oRow.Plazo
            tbTasa360.Text = oRow.TasaPorcentaje
            ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
            ddlIntermediario.SelectedValue = oRow.CodigoTercero
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, oRow.CodigoTercero)
            Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
            tbFechaInicioCOMPRA.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            tbFechaVenCOMPRA.Text = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
            tbFechaLiquiCOMPRA.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
            ddlBolsa.SelectedValue = oRow.Plaza
            ddlClasificacion.SelectedValue = oRow.TipoCondicion
            '-----------------------Operacion Plazo-----------------------
            tbTasa365.Text = oRow.TasaCastigo
            'OT 10090 - 31/07/2017 - Carlos Espejo
            'Descripcion: Se calcula los rendimientos con datos obtenidos del masivo
            tbMontoNetoVENTA.Text = oRow.MontoNetoOperacion
            tbRendimientos.Text = CDec(oRow.MontoNetoOperacion) - CDec(oRow.MontoNominalOperacion)
            'OT 10090 Fin
            tbCantidadVENTA.Text = tbCantidadCOMPRA.Text
            tbPlazoVENTA.Text = tbPlazoVenCOMPRA.Text
            CalcularTotales()
            ViewState("CodigoReporte") = oRow.CodigoMnemonicoReporte
            ViewState("MontoNeto") = oRow.MontoOperacion
            Session("CodigoOI") = Me.txtCodigoOrden.Value

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga data de motivo de cambio si fuera el caso | 07/06/18 
            If oRow.CodigoMotivoCambio <> String.Empty Then ddlMotivoCambio.SelectedValue = oRow.CodigoMotivoCambio
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga data de motivo de cambio si fuera el caso | 07/06/18 

        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF31"))
        End Try
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("SS_DatosModal") Is Nothing And hfModal.Value = "1" Then
            txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
            txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1)
            txtSBS.Text = CType(Session("SS_DatosModal"), String())(2)
            hdCustodio.Value = CType(Session("SS_DatosModal"), String())(3)
            hdSaldo.Value = CType(Session("SS_DatosModal"), String())(4)
            Session.Remove("SS_DatosModal")
        End If
        If Not Session("SS_DatosModal") Is Nothing And hfModal.Value = "2" Then
            txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
            txtSBS.Text = CType(Session("SS_DatosModal"), String())(1)
            txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(2)
            ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3)
            ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4)
            txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6)
            Session("CodigoOrden") = CType(Session("SS_DatosModal"), String())(6)
            Session.Remove("SS_DatosModal")
        End If
        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
            EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
            If ddlMotivoCambio.Items.Count = 0 Then HelpCombo.CargarMotivosCambio(Me)
        End If
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        '   Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If Not Page.IsPostBack Then
            CargarMoneda()
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 13/06/18 
            hdRptaConfirmar.Value = "NO"
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se inicializa campo oculto para respuesta de confirmación | 13/06/18 
            celda_poliza.Visible = False
            Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            LimpiarSesiones()
            'Se listan los portafolios dependiendo del area al cual corresponda el usuario
            UIUtility.CargarOperacionOI(ddlOperacion, "OperacionReporte")
            UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
            CargarPlaza()
            Dim oCalificacionSBS As New CalificacionInstrumentoBM
            Dim DtTablaCalificacionSBS As DataTable = oCalificacionSBS.Listar(DatosRequest).Tables(0)
            HelpCombo.LlenarComboBox(Me.ddlClasificacion, DtTablaCalificacionSBS, "CodigoCalificacion", "CodigoCalificacion", True)
            CargarPaginaInicio()
            Me.hdPagina.Value = ""
            HelpCombo.PortafolioCodigoListar(ddlFondo, PORTAFOLIO_MULTIFONDOS)
            If Not Request.QueryString("PTNeg") Is Nothing Then
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo cuando se llama de otra ventana | 28/06/18 
                'EjecutarJS("$('#divCodigoIsin').removeAttr('style');")
                EjecutarJS("$('#divOperacion').removeAttr('style');")
                EjecutarJS("$('#divComisiones').removeAttr('class');")
                EjecutarJS("$('#divTotalComisiones').removeAttr('class');")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo cuando se llama de otra ventana | 28/06/18 
                Me.hdPagina.Value = Request.QueryString("PTNeg")
                Me.txtMnemonico.Text = Request.QueryString("PTCMnemo")
                Me.ddlFondo.SelectedValue = Request.QueryString("PTFondoO")
                Me.ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                ControlarCamposTI()
                Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                Session("CodigoOrden") = Request.QueryString("PTNOrden")
                Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
           
                'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then
                    celda_poliza.Visible = True
                    CargarDatosOrdenInversion(txtCodigoOrden.Value)
                    CargaComisiones(Session("CodigoOrden"))
                    Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(Session("CodigoOrden"), ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                    CargarPaginaModificarEO_CO_XO(Me.hdPagina.Value)
                    ControlarCamposEO_CO_XO()
                    CalcularTotales()
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                    If hdPagina.Value = "CO" Then
                        btnAceptar.Text = "Grabar y Confirmar"
                        If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then CargarPaginaInicio()
                    End If
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18       Else
                    If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                        ControlarCamposOE()
                    Else
                        If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                            ViewState("ORDEN") = "OI-DA"
                            tbFechaInicioCOMPRA.Text = Request.QueryString("Fecha")
                            MostrarOcultarBotonesAcciones(True)
                        Else
                            If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                Call ConfiguraModoConsulta()
                                ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                txtMnemonico.Text = Request.QueryString("Mnemonico")
                                txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                Session("CodigoOrden") = Request.QueryString("CodigoOrden")
                                ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                CargarDatosOrdenInversion(txtCodigoOrden.Value)
                                CargaComisiones(Session("CodigoOrden"))
                                CalcularTotales()
                                Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, True, False)
                            End If
                            '-----------------------------------
                        End If
                    End If
                End If
                Me.btnSalir.Attributes.Remove("onClick")
                '  Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                'Me.btnAceptar.Attributes.Add("onClick", "if (Confirmacion()){this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');}")
                UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF49", txtCodigoOrden.Value + "?", "SI")
            End If
        Else
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo cuando se llama de otra ventana | 28/06/18 
            If Not Request.QueryString("PTNeg") Is Nothing Then
                'EjecutarJS("$('#divCodigoIsin').removeAttr('style');")
                EjecutarJS("$('#divOperacion').removeAttr('style');")
                EjecutarJS("$('#divComisiones').removeAttr('class');")
                EjecutarJS("$('#divTotalComisiones').removeAttr('class');")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se muestra campo cuando se llama de otra ventana | 28/06/18 
            End If
        End If
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
            Dim accionRpta As String = String.Empty
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Creación de Variable para controlar acción y poder enviar respuesta de las acciones | 11/06/18 
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
            If hdRptaConfirmar.Value.ToUpper = "SI" Then
                EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Evitar doble Postback | 07/06/18 
                If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" Then
                    If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                        'No se valida saldo disponible al confirmar
                        'el problema detectado fue que el saldo se puso en cero en la ejecucion
                        ModificarOrdenInversion()
                        ModificarComisiones(Session("CodigoOrden"))
                        Session("Modificar") = 0
                        CargarPaginaAceptar()
                    End If
                    If Me.hdPagina.Value = "XO" Then
                        oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(Me.Session("CodigoOrden"), Me.ddlFondo.SelectedValue, Me.DatosRequest)
                        ReturnArgumentShowDialogPopup()
                    Else
                        If Me.hdPagina.Value = "EO" Then
                            oOrdenInversionWorkFlowBM.EjecutarOI(Me.Session("CodigoOrden"), Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                            ReturnArgumentShowDialogPopup()
                        Else
                            If Me.hdPagina.Value = "CO" Then
                                oOrdenInversionWorkFlowBM.ConfirmarOI(Me.Session("CodigoOrden"), Me.ddlFondo.SelectedValue, tbNroPoliza.Text, Me.DatosRequest)
                                ReturnArgumentShowDialogPopup()
                            End If
                        End If
                    End If
                Else
                    If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
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
                    End If
                    If Session("EstadoPantalla") = "Ingresar" Then
                        If Session("Procesar") = 1 Then
                            '    Dim strcodigoOrden As String
                            '    strcodigoOrden = InsertarOrdenInversion()
                            '    'Modificar Estado = E-EJE (Solo para ordenes de Días Anteriores)
                            '    oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                            '    txtCodigoOrden.Value = strcodigoOrden
                            '    Session("CodigoOrden") = strcodigoOrden
                            '    If Me.hdPagina.Value <> "TI" Then
                            '        InsertarComisiones(strcodigoOrden)
                            '    End If
                            '    If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then AlertaJS(ObtenerMensaje("CONF18"))
                            '    Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            '    GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), "OPERACIONES DE REPORTE", Me.ddlOperacion.SelectedItem.Text, _
                            'Session("CodigoMoneda"), Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ViewState("CodigoReporte"))
                            '    Session("CodigoOI") = strcodigoOrden
                            '    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                            '    accionRpta = "Ingresó"
                            '    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18 
                            '    CargarPaginaAceptar()
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
                            GuardarPreOrden()
                            accionRpta = "Ingresó" 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Ingresar | 11/06/18                                     
                            CargarPaginaInicio()
                            'FIN | ZOLUXIONES | RCE | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion

                        End If
                    Else
                        If Session("EstadoPantalla") = "Modificar" Then
                            Procesar()
                            ModificarOrdenInversion()
                            FechaEliminarModificarOI("M")
                            Session("Modificar") = 0
                            ModificarComisiones(Session("CodigoOrden"))
                            CargarPaginaAceptar()
                            Session("dtdatosoperacion") = ObtenerDatosOperacion()
                            GenerarLlamado(Me.Session("CodigoOrden"), "OPERACIONES DE REPORTE", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), _
                            Me.txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ViewState("CodigoReporte"))
                            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
                            accionRpta = "Modificó"
                            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se carga variable con acción Modificar | 11/06/18 
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
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                    Select Case Session("EstadoPantalla")
                        Case "Eliminar", "Modificar"
                            retornarMensajeAccion(accionRpta)
                        Case "Ingresar"
                            retornarMensajeAccionPreOrden(accionRpta)
                    End Select
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - LLamado de procedimiento para enviar respuesta de acción | 11/06/18 
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
            EjecutarJS("document.getElementById('hdRptaConfirmar').value = 'NO'")
        End Try
    End Sub
    Public Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlBolsa.DataSource = oPlazaBM.Listar(Nothing)
        ddlBolsa.DataTextField = "Descripcion"
        ddlBolsa.DataValueField = "CodigoPlaza"
        ddlBolsa.DataBind()
        ddlBolsa.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If Session("EstadoPantalla") = "Ingresar" Then
            If Session("Busqueda") = 0 Then
                If Me.ddlFondo.SelectedValue = "" Then
                    If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
                        AlertaJS(ObtenerMensaje("CONF42"))
                        Exit Sub
                    ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
                        AlertaJS(ObtenerMensaje("CONF42"))
                        Exit Sub
                    End If
                End If
                ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlOperacion.SelectedValue, "OR", 1)
                Session("Busqueda") = 2
            Else
                If Session("Busqueda") = 1 Then
                    CargarFechaVencimiento()
                    If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                    Else
                        UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF15", "", "SI")
                    End If
                    CargarPaginaIngresar()
                    CargarValores()
                    If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = Me.ddlOperacion.SelectedValue Then
                        Me.ddlFondo.Enabled = True
                    End If
                    UIUtility.ResaltaCombo(ddlIntermediario, True)
                    UIUtility.ResaltaCombo(ddlBolsa, True)
                    UIUtility.ResaltaCombo(ddlGrupoInt, True)
                    UIUtility.ResaltaCajaTexto(tbPlazoVenCOMPRA, True)
                    UIUtility.ResaltaCajaTexto(tbCantidadCOMPRA, True)
                    UIUtility.ResaltaCajaTexto(tbMontoNetoCOMPRA, True)
                    UIUtility.ResaltaCajaTexto(tbTasa360, True)
                    UIUtility.ResaltaCajaTexto(tbMontoNetoVENTA, True)
                    'UIUtility.ResaltaCajaTexto(tbTasa365, True)
                    ddlBolsa.SelectedValue = "7"
                Else
                    Session("Busqueda") = 0
                End If
            End If
        Else
            If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Or Session("EstadoPantalla") = "Consultar" Then
                If Session("Busqueda") = 0 Then
                    ShowDialogPopupInversionesRealizadas(Me.txtISIN.Text.ToString.Trim, Me.txtSBS.Text.ToString.Trim, Me.txtMnemonico.Text.ToString.Trim, _
                    ddlOperacion.SelectedValue, ddlmoneda.SelectedValue.ToString, "OR", "", Left(Session("EstadoPantalla"), 1))
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarValores()
                        CargarDatosOrdenInversion(Session("CodigoOrden"))
                        CargaComisiones(Session("CodigoOrden"))
                        CalcularTotales()
                        btnAceptar.Enabled = True
                        If Session("EstadoPantalla") = "Modificar" Then
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF2", "Nro " + txtCodigoOrden.Value + "?", "SI")
                            CargarPaginaModificar()
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            UIUtility.AsignarMensajeBotonAceptar(btnAceptar, "CONF3", "Nro " + txtCodigoOrden.Value + "?", "SI")
                            CargarPaginaEliminar()
                            ddlMotivoCambio.Enabled = True
                        ElseIf Session("EstadoPantalla") = "Consultar" Then
                            CargarPaginaConsultar()
                        End If
                    Else
                        Session("Busqueda") = 0
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
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
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.Session("CodigoOrden"), "OPERACIONES DE REPORTE", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"), Me.txtISIN.Text.Trim,
        Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ViewState("CodigoReporte"))
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        LimpiarSesiones()
        'GUID: Identificador único para cuponetas temporales
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlOperacion)
        CargarPaginaAccion()
        Session("EstadoPantalla") = "Ingresar"
        Session("Procesar") = 0
        Session("Busqueda") = 0
        lblAccion.Text = "Ingresar"
        hdMensaje.Value = "el Ingreso"
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "Orden de Inversión - OPERACIONES DE REPORTE"
        HabilitarBotonGuardarPreOrden()
    End Sub
    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        LimpiarSesiones()
        'GUID: Identificador único para cuponetas temporales
        Dim GUID As Guid = System.Guid.NewGuid()
        ViewState("CuponeraTemporalGUID") = Convert.ToString(GUID.ToString())
        '------------------------------------------------------------------
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Modificar"
        Session("Busqueda") = 0
        Session("Procesar") = 0
        lblAccion.Text = "Modificar"
        CargarPaginaAccion()
        hdMensaje.Value = "la Modificación"
        EjecutarJS("$('#trMotivoCambio').removeAttr('style');")
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        If tbMontoNetoVENTA.Text = "" Or CInt(tbMontoNetoVENTA.Text) = 0 Then
            AlertaJS("Debe ingresar el valor para Monto Neto Vcto.", "tbMontoNetoVENTA.focus()")
        ElseIf tbMontoNetoCOMPRA.Text = "" Or CInt(tbMontoNetoCOMPRA.Text) = "0" Then
            AlertaJS("Debe ingresar el valor para Monto Neto.", "tbMontoNetoCOMPRA.focus()")
        Else
            Procesar()
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
            Case "CDP"
                strMensajeConfirmacion = "¿Desea cancelar la Corrección de la orden de inversión Nro. " + NroOrden + "?"
            Case Else
                If (strAccion <> String.Empty) Then
                    If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de la orden de inversión de Operaciones Reporte?"
                    Else
                        strMensajeConfirmacion = "¿Desea cancelar " + strAccion + " de pre-orden de inversión de Operaciones Reporte?"
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
    Protected Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If ddlFondo.SelectedValue <> "" Then
            CargarFechaVencimiento()
        End If
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicioCOMPRA.Text))
        If cantidadreg > 0 Then
            AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
            ddlFondo.SelectedIndex = 0
            Exit Sub
        End If
    End Sub
    Protected Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Protected Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos(True)
        CargarFechaVencimiento()
    End Sub
    Protected Sub tbPlazoVenCOMPRA_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPlazoVenCOMPRA.TextChanged
        If Not tbPlazoVenCOMPRA.Text = "0" Or tbPlazoVenCOMPRA.Text = "" Then
            EjecutarJS("document.getElementById('tbPlazoVENTA').value= " + tbPlazoVenCOMPRA.Text)
            If ValidarFechaVencimiento() = False Then
                Exit Sub
            End If
            tbCantidadCOMPRA.Focus()
        End If
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False, True)
    End Sub
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF008 - Se implementa las funciones para el guardado en PreOrden  | 17/08/18 
#Region "Registro En la Preorden"
    Sub GuardarPreOrden()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII | 2018-08-17 | Guardado en Pre Orden Inversion
        Dim entPreOrden As New PrevOrdenInversionBE
        Dim negPreOrden As New PrevOrdenInversionBM
        Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow = CType(entPreOrden.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
        Dim oValoresBM As New ValoresBM


        negPreOrden.InicializarPrevOrdenInversion(oRow)

        oRow.CodigoPrevOrden = 0
        oRow.CodigoOperacion = ddlOperacion.SelectedValue '101 -Constitución de OR
        oRow.CodigoNemonico = txtMnemonico.Text

        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaInicioCOMPRA.Text)
        oRow.HoraOperacion = Now.ToLongTimeString()
        oRow.ClaseInstrumentoFx = "OR"
        oRow.CodigoTipoTitulo = IIf(ddlmoneda.SelectedValue = "NSOL", "REPORTESOL", "REPORTEDOL")

        oRow.MontoNominal = CDec(tbMontoNetoCOMPRA.Text)
        oRow.Tasa = CType(tbTasa360.Text, Decimal)
        oRow.TipoTasa = "2"
        oRow.FechaContrato = UIUtility.ConvertirFechaaDecimal(tbFechaVenCOMPRA.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquiCOMPRA.Text)
        oRow.MontoOperacion = CDec(tbMontoNetoVENTA.Text)
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.Cantidad = CDec(tbCantidadCOMPRA.Text)
        oRow.CantidadOperacion = CDec(tbCantidadCOMPRA.Text)
        oRow.CodigoPlaza = ddlBolsa.SelectedValue

        'Valores por Defecto          
        oRow.MedioNegociacion = "E" 'Por defecto 'ELECTRONICO'


        'oRow.TipoFondo = "Normal" 'Por defecto 'NORMAL'
        ' oRow.TipoTramo = "AGENCIA" 'Por defecto 'AGENCIA'
        ' oRow.TipoCondicion = "PL" 'Por defecto 'PRECIO LÍMITE'
        oRow.Porcentaje = "N" 'N: No Porcentaje, solo Monto directo
        oRow.Fixing = 0 'Por defecto 
        oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
        oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO

        entPreOrden.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
        entPreOrden.PrevOrdenInversion.AcceptChanges()

        Dim dtAsignacion As New DataTable ' Asignacion Por Fondo
        dtAsignacion.Columns.Add("CodigoPortafolio")
        dtAsignacion.Columns.Add("Asignacion")
        'Solo necesitamos una Fila donde se indicará el 100% de unidades para el Fondo
        dtAsignacion.Rows.Add(ddlFondo.SelectedValue, oRow.MontoNominal)

        'Guardamos la Pre-Orden
        negPreOrden.Insertar(entPreOrden, ParametrosSIT.TR_DERIVADOS.ToString, DatosRequest, dtAsignacion)

        HabilitarBotonesAccion()

        'FIN | ZOLUXIONES | RCE | ProyFondosII | 2018-07-02 | Guardado en Pre Orden Inversion
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
#End Region
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF008 - Se implementa las funciones para el guardado en PreOrden  | 17/08/18 
End Class