Imports ParametrosSIT
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmPagares
    Inherits BasePage
#Region "Rutinas"
    Private Sub CargarCuponera(ByVal strCodigoMnemonico As String)
        Dim oCuponeraBM As New CuponeraBM
        Dim dtCuponera As DataTable = oCuponeraBM.SeleccionarPorOrdenInversion(strCodigoMnemonico, DatosRequest)
        Session("dtCuponera") = dtCuponera
    End Sub
    Private Sub GrabarCuponeraOI(ByVal IsTemporal As Boolean, Optional ByVal GUID As String = "")
        CargarCuponera(txtMnemonico.Text.Trim)
        Dim oCuponera As New CuponeraBM
        Dim dtCuponeraOI As DataTable = CType(Session("dtCuponera"), DataTable)
        Dim i As Integer
        Dim montoNominal As Decimal = Math.Round(Convert.ToDecimal(Me.txtMnomOp.Text.Replace(".", UIUtility.DecimalSeparator)), 2)
        Dim YTM As String = Math.Round(Convert.ToDecimal(Me.txtYTM.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        If Not dtCuponeraOI Is Nothing Then
            If dtCuponeraOI.Rows.Count > 0 Then
                oCuponera.EliminarCuponeraOI(GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest)
                For i = 0 To dtCuponeraOI.Rows.Count - 1
                    If i < dtCuponeraOI.Rows.Count - 1 Then
                        oCuponera.InsertarCuponeraOI(0, GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue.ToString,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaInicio")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaTermino")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Amortizacion")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("ValorNominal")),
                        String.Empty, String.Empty, "N", dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TasaCupon")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TotalVP")), montoNominal, Me.ddlTipoTasa.SelectedValue,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiferenciaDias")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiasPago")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Base")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("codigoTipoAmortizacion")),
                        txtMnemonico.Text.ToString(), DatosRequest)
                    Else
                        oCuponera.InsertarCuponeraOI(1, GUID, IsTemporal, txtCodigoOrden.Value, ddlFondo.SelectedValue.ToString,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaInicio")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("FechaTermino")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Amortizacion")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("ValorNominal")),
                        String.Empty, String.Empty, "N", dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TasaCupon")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("TotalVP")), montoNominal, Me.ddlTipoTasa.SelectedValue,
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiferenciaDias")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("DiasPago")),
                        dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("Base")), dtCuponeraOI.Rows(i).Item(dtCuponeraOI.Columns.IndexOf("codigoTipoAmortizacion")),
                        txtMnemonico.Text.ToString(), DatosRequest)
                    End If
                Next
            End If
        End If
    End Sub
    Public Sub CalcularComisiones()
        Dim dblTotalComisiones As Decimal = 0.0
        If Me.hdPagina.Value = "TI" Then
            dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoOperacional.Text.Replace(",", ""),
            txtNroPapeles.Text.Replace(",", ""))
        Else
            dblTotalComisiones = UIUtility.CalculaImpuestosComisiones(dgLista, Session("Mercado"), Me.txtMontoOperacional.Text.Replace(",", ""),
            txtNroPapeles.Text.Replace(",", ""))
        End If
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoOperacional.Text.Replace(",", ""), "##,##0.0000000")
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = txtMnomOp.Text.Replace(",", "")
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
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
            If dblFechaConstitucion > dblFechaOperacion Then
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
    Private Sub ShowDialogCuponera(ByVal mnemonico As String, Optional ByVal IsProcesar As Boolean = True)
        Dim sGuid As String = String.Empty
        Dim sCodigoOrden As String = String.Empty
        Dim sCodigoPortafolioSBS As String = String.Empty
        IsProcesar = CType(ViewState("IsIndica"), Boolean)
        If IsProcesar Then
            sGuid = ViewState("CuponeraTemporalGUID").ToString()
            sCodigoOrden = "0"
            sCodigoPortafolioSBS = "0"
        Else
            sGuid = "0"
            sCodigoOrden = Me.txtCodigoOrden.Value
            sCodigoPortafolioSBS = ddlFondo.SelectedValue
        End If
        Dim sCodigoMnemonico As String = mnemonico
        Dim sInteresCorrido As String = Convert.ToDecimal(Me.txtInteresCorNeg.Text.Replace(",", "")).ToString()
        Dim sMontoOperacion As String = Convert.ToDecimal(Me.txtMontoOperacional.Text.Replace(",", "")).ToString()
        Dim sPrecioCalculado As String = Convert.ToDecimal(Me.lblPrecioCal.Text.Replace(",", "")).ToString()
        Dim sFechaOperacion As String = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
        Dim strURL As String = "frmConsultaCuponeras.aspx?CodigoMnemonico=" & mnemonico & "&guid=" & sGuid & "&codigoOrden=" & sCodigoOrden & "&CodigoPortafolioSBS=" &
        sCodigoPortafolioSBS & "&InteresCorrido=" & sInteresCorrido & "&MontoOperacion=" & sMontoOperacion & "&PrecioCalculado=" & sPrecioCalculado & "&FechaOperacion=" &
        sFechaOperacion
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', ' '); ")
    End Sub
    Private Sub CargarPaginaProcesar()
        Dim strJS As New StringBuilder
        strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
        strJS.AppendLine("$('#btnCuponera').show();")
        strJS.AppendLine("$('#btnCuponera').removeAttr('disabled');")
        If Session("EstadoPantalla") <> "Ingresar" Then
            strJS.AppendLine("$('#btnImprimir').show();")
            strJS.AppendLine("$('#btnImprimir').removeAttr('disabled');")
            If ddlFondo.SelectedValue = "MULTIFONDO" Then
                btnAsignar.Visible = True
                strJS.AppendLine("$('#btnAsignar').show();")
            End If
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
        Me.lblComentarios.Text = "Comentarios eliminación:"
        Me.txtComentarios.Text = ""
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
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub CargarPaginaModificar()
        CargarPaginaBuscar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
        Me.lblComentarios.Text = "Comentarios modificación:"
        Me.txtComentarios.Text = ""
    End Sub
    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String,
    ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String, ByVal nomFondo As String)
        Dim strURL As String = "../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&vFondo=" + nomFondo + "&cFondo=" +
        fondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=PA"
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='1'; ")
    End Sub
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal operacion As String,
    ByVal categoria As String, ByVal valor As String, ByVal nomFondo As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&vFondo=" & nomFondo & "&cFondo=" & fondo &
        "&vOperacion=" & operacion & "&vCategoria=" & categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "'); document.getElementById('hfModal').value='2'; ")
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
    Public Sub EliminarOrdenInversion()
        oOrdenInversionBM.EliminarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "", DatosRequest)
        oImpComOP.Eliminar(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, DatosRequest)
    End Sub
    Public Function InsertarOrdenInversion() As String
        Dim strCodigoOI, strCodigoOI_T As String
        oOrdenInversionBE = crearObjetoOI()
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        If Me.hdPagina.Value = "TI" Then
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoPortafolioSBS") = Me.ddlFondoDestino.SelectedValue
            oOrdenInversionBE.OrdenPreOrdenInversion.Rows(0)("CodigoOperacion") = UIUtility.ObtenerCodigoOperacionTIngreso().ToString()
            Session("ValorCustodio") = UIUtility.ObtieneUnCustodio(Session("ValorCustodio"))
            strCodigoOI_T = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
            ViewState("CodigoOrden_T") = "-" + strCodigoOI_T
        Else
            ViewState("CodigoOrden_T") = ""
        End If
        Return strCodigoOI
    End Function
    Private Sub ReturnArgumentShowDialogPopup()
        Dim script As New StringBuilder
        If Me.hdPagina.Value = "CO" Then
            AlertaJS("Se Confirmó la orden correctamente", "window.close()")
        Else
            If Me.hdPagina.Value = "EO" Then
                AlertaJS("Se Ejecutó la orden correctamente", "window.close()")
            Else
                If Me.hdPagina.Value = "XO" Then
                    AlertaJS("Se Extornó la orden correctamente", "window.close()")
                Else
                    If Me.hdPagina.Value = "OE" Then
                        EjecutarJS("window.close()")
                    Else
                        If Me.hdPagina.Value = "MODIFICA" Then
                            AlertaJS("Se Modificó la orden correctamente", "window.close()")
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String,
    ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal nomPortafolio As String)
        EjecutarJS(UIUtility.MostrarPopUp("../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + nomPortafolio + "&cportafolio=" + portafolio +
        "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico, "10", 1000, 650, 0, 0,
        "No", "Yes", "Yes", "Yes"), False)
    End Sub
    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(Me.ddlFondo.SelectedValue, Me.txtCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc,
        txtComentarios.Text, DatosRequest)
        txtComentarios.Text = ""
    End Sub
    Public Function ObtenerDatosOperacion() As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11",
        "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = Me.tbFechaOperacion.Text
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = Me.tbFechaLiquidacion.Text
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = Me.tbHoraOperacion.Text
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = Me.txtMnomOrd.Text
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = Me.txtMnomOp.Text
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = Me.ddlTipoTasa.SelectedItem.Text
        drGrilla("c7") = "YTM%"
        drGrilla("v7") = Me.txtYTM.Text
        drGrilla("c8") = "Precio Negociación %"
        drGrilla("v8") = Me.txtPrecioNegoc.Text
        drGrilla("c9") = "Precio Calculado %"
        drGrilla("v9") = Me.lblPrecioCal.Text
        drGrilla("c10") = "Precio Negociación Sucio"
        drGrilla("v10") = Me.txtPrecioNegSucio.Text
        drGrilla("c11") = "Interés Corrido Negociado"
        drGrilla("v11") = Me.txtInteresCorNeg.Text
        drGrilla("c12") = "Interés Corrido"
        drGrilla("v12") = Me.lblInteresCorrido.Text
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = Me.txtMontoOperacional.Text
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = Me.txtNroPapeles.Text
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = Me.ddlIntermediario.SelectedItem.Text
        If Me.ddlContacto.SelectedIndex <> 0 Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = Me.ddlContacto.SelectedItem.Text
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        If Me.tbNPoliza.Visible = True Then
            drGrilla("c17") = "Número Poliza"
            drGrilla("v17") = Me.tbNPoliza.Text
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Observación"
        drGrilla("v18") = Me.txtObservacion.Text.ToUpper
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Me.txttotalComisionesC.Text
        drGrilla("c20") = "Total Operación"
        drGrilla("v20") = Me.txtMontoNetoOpe.Text
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        Me.btnProcesar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(False)
        If Session("EstadoPantalla") = "Ingresar" Then
            Me.btnImprimir.Visible = True
            Me.btnImprimir.Enabled = True
            If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
                Me.btnAsignar.Visible = True
            End If
        End If
        Me.btnAceptar.Enabled = False
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
        oRow.InteresCorrido = Math.Round(Convert.ToDecimal(Me.lblInteresCorrido.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioCalculado = Math.Round(Convert.ToDecimal(Me.lblPrecioCal.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioNegociacionLimpio = Math.Round(Convert.ToDecimal(Me.txtPrecioNegoc.Text.Replace(",", "")), Constantes.M_INT_NRO_DECIMALES)
        oRow.Situacion = "A"
        oRow.InteresCorridoNegociacion = Math.Round(Convert.ToDecimal(Me.txtInteresCorNeg.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.MontoOperacion = Math.Round(Convert.ToDecimal(Me.txtMontoOperacional.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.PrecioNegociacionSucio = Math.Round(Convert.ToDecimal(Me.txtPrecioNegSucio.Text.Replace(".", UIUtility.DecimalSeparator)), Constantes.M_INT_NRO_DECIMALES)
        oRow.CantidadOperacion = Math.Round(Convert.ToDecimal(Me.txtNroPapeles.Text), 0)
        oRow.Observacion = Me.txtObservacion.Text.ToUpper
        oRow.HoraOperacion = Me.tbHoraOperacion.Text
        oRow.TotalComisiones = Convert.ToDecimal(txttotalComisionesC.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.MontoNetoOperacion = Convert.ToDecimal(txtMontoNetoOpe.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.CategoriaInstrumento = "PA"    'UNICO POR TIPO
        oRow.Plaza = ddlPlaza.SelectedValue
        If (Me.hdPagina.Value <> "XO") Then
            oRow.NumeroPoliza = Me.tbNPoliza.Text.ToString().Trim
        End If
        If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
            If ddlMotivoCambio.SelectedIndex > 0 Then
                oRow.CodigoMotivoCambio = ddlMotivoCambio.SelectedValue
            End If
            If Session("EstadoPantalla") = "Modificar" Then
                oRow.IndicaCambio = "1"
            End If
        End If
        If (chkRegulaSBS.Checked) Then
            oRow.RegulaSBS = "S"
        Else
            oRow.RegulaSBS = "N"
        End If
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, Me.hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
    End Sub
    Private Sub actualizaMontos()
        Dim dblTotalComisiones As Decimal = 0.0
        dblTotalComisiones = UIUtility.ActualizaMontosFinales(dgLista)
        txttotalComisionesC.Text = Format(dblTotalComisiones, "##,##0.0000000")
        txtMontoNetoOpe.Text = Format(dblTotalComisiones + txtMontoOperacional.Text.Replace(",", ""), "##,##0.0000000")
        Dim strMontoAux As String = txtMontoNetoOpe.Text.Replace(",", "")
        strMontoAux = strMontoAux.Replace(".", UIUtility.DecimalSeparator)
        Dim strAccAux As String = txtMnomOp.Text.Replace(",", "")
        strAccAux = strAccAux.Replace(".", UIUtility.DecimalSeparator)
    End Sub
    Private Sub CargarPaginaIngresar()
        CargarPaginaBuscar()
        Me.btnAsignar.Visible = False
        HabilitaDeshabilitaCabecera(False)
        Me.btnBuscar.Visible = False
        HabilitaDeshabilitaDatosOperacionComision(True)
        Me.btnCaracteristicas.Visible = True
        Me.btnCaracteristicas.Enabled = True
    End Sub
    Public Sub CargarFechaVencimiento()
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
            If (txtMnemonico.Text.Trim <> "") Then
                tbFechaLiquidacion.Text = oOrdenInversionBM.RetornarFechaVencimiento(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text),
                txtMnemonico.Text, ddlFondo.SelectedValue, ddlIntermediario.SelectedValue)
            End If
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
        tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Traspaso"
        hdMensaje.Value = "el Ingreso"
        If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
            AlertaJS(ObtenerMensaje("CONF1"))
        Else
            AlertaJS(ObtenerMensaje("CONF15"))
        End If
        CargarPaginaIngresar()
        CargarIntermediario()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
    End Sub
    Private Sub HabilitaBotones(ByVal bCuponera As Boolean, ByVal bLimites As Boolean, ByVal bIngresar As Boolean, ByVal bModificar As Boolean, ByVal bEliminar As Boolean,
    ByVal bConsultar As Boolean, ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, ByVal bAceptar As Boolean, ByVal bBuscar As Boolean,
    ByVal bSalir As Boolean, ByVal bRetornar As Boolean, ByVal bCaracteristicas As Boolean, ByVal bLimitesParametrizados As Boolean)
        btnCuponera.Visible = bCuponera
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnConsultar.Visible = bConsultar
        btnAsignar.Visible = bAsignar
        btnProcesar.Visible = bProcesar
        btnImprimir.Visible = bImprimir
        btnAceptar.Visible = bAceptar
        btnBuscar.Visible = bBuscar
        btnSalir.Visible = bSalir
        btnRetornar.Visible = bRetornar
        btnCaracteristicas.Visible = bCaracteristicas
    End Sub
    Private Sub ControlarCamposOE()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Enabled = True
    End Sub
    Private Sub CargarPaginaBuscar()
        If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            Me.btnAsignar.Visible = True
            Me.btnAsignar.Enabled = True
        End If
        Me.btnProcesar.Visible = True
        Me.btnProcesar.Enabled = True
    End Sub
    Private Sub CargarPaginaModificarEO_CO_XO(ByVal acceso As String)
        If acceso = "EO" Or acceso = "CO" Then
            CargarPaginaBuscar()
            HabilitaDeshabilitaCabecera(False)
            Me.btnBuscar.Visible = False
            HabilitaDeshabilitaDatosOperacionComision(True)
            Me.btnCaracteristicas.Visible = True
            Me.btnCaracteristicas.Enabled = True
            Me.btnAceptar.Enabled = True
            Session("EstadoPantalla") = "Modificar"
            'GUID: Identificador único para cuponetas temporales
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
    Private Sub ControlarCamposEO_CO_XO()
        MostrarOcultarBotonesAcciones(False)
        Me.btnAceptar.Enabled = True
    End Sub
    Private Sub LimpiarDatosOperacion()
        Me.tbFechaOperacion.Text = ""
        Me.tbFechaLiquidacion.Text = ""
        Me.txtMnomOp.Text = ""
        Me.txtMnomOrd.Text = ""
        Me.ddlTipoTasa.SelectedIndex = 0
        Me.txtYTM.Text = ""
        Me.lblPrecioCal.Text = ""
        Me.txtPrecioNegoc.Text = ""
        Me.lblInteresCorrido.Text = ""
        Me.txtInteresCorNeg.Text = ""
        Me.ddlGrupoInt.SelectedIndex = 0
        If Me.ddlIntermediario.Items.Count > 0 Then Me.ddlIntermediario.SelectedIndex = 0
        CargarContactos()
        Me.ddlContacto.SelectedIndex = 0
        Me.txtMontoOperacional.Text = ""
        Me.txtPrecioNegSucio.Text = ""
        Me.txtNroPapeles.Text = ""
        Me.tbHoraOperacion.Text = ""
        Me.txtObservacion.Text = ""
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        Me.txttotalComisionesC.Text = ""
        Me.txtMontoNetoOpe.Text = ""
    End Sub
    Private Sub LimpiarCaracteristicasValor()
        Me.lblbasecupon.Text = ""
        Me.lblbasetir.Text = ""
        Me.lbldescripcion.Text = ""
        Me.lblduracion.Text = ""
        Me.lblemisor.Text = ""
        Me.lblnominales.Text = ""
        Me.lblunidades.Text = ""
        Me.lblfecfinbono.Text = ""
        Me.lblFecProxCupon.Text = ""
        Me.lblFecUltCupon.Text = ""
        Me.lblpreciovector.Text = ""
        Me.lblparticipacion.Text = ""
        Me.lblMoneda.Text = ""
        Me.ddlFondo.SelectedIndex = 0
        Me.ddlOperacion.SelectedIndex = 0
        Me.txtISIN.Text = ""
        Me.txtSBS.Text = ""
        Me.txtMnemonico.Text = ""
    End Sub
    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
        LimpiarCaracteristicasValor()
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
    Public Sub CargarIntermediario()
        If Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("COMPRA") Then
            UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        ElseIf Me.ddlOperacion.SelectedValue = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
            UIUtility.CargarIntermediariosCustodioXGrupoInterOI(ddlIntermediario, Me.hdCustodio.Value, ddlGrupoInt.SelectedValue)
        End If
        Session("datosEntidad") = CType(ddlIntermediario.DataSource, DataSet).Tables(0)
    End Sub
    Private Sub CargarDatosOrdenInversion()
        Try
            oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, DatosRequest,
            PORTAFOLIO_MULTIFONDOS)
            Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
            oRow = oOrdenInversionBE.Tables(0).Rows(0)
            Session("CodigoMoneda") = oRow.CodigoMoneda
            txtISIN.Text = oRow.CodigoISIN
            txtMnemonico.Text = oRow.CodigoMnemonico
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
            ddlTipoTasa.SelectedValue = oRow.CodigoTipoCupon
            txtYTM.Text = Format(oRow.YTM, "##,##0.0000000")
            lblInteresCorrido.Text = Format(oRow.InteresCorrido, "##,##0.0000000")
            lblPrecioCal.Text = Format(oRow.PrecioCalculado, "##,##0.0000000")
            txtInteresCorNeg.Text = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
            txtMontoOperacional.Text = Format(oRow.MontoOperacion, "##,##0.00")
            txtPrecioNegSucio.Text = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
            txtNroPapeles.Text = Format(oRow.CantidadOperacion, "##,##0.0000000")
            txtPrecioNegoc.Text = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
            hdNumUnidades.Value = Me.txtMontoOperacional.Text
            Me.tbHoraOperacion.Text = oRow.HoraOperacion
            txtObservacion.Text = oRow.Observacion
            lblpreciovector.Text = Format(oRow.Precio, "##,##0.0000000")
            txttotalComisionesC.Text = Format(oRow.TotalComisiones, "##,##0.0000000")
            txtMontoNetoOpe.Text = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
            tbNPoliza.Text = oRow.NumeroPoliza.ToString()
            ViewState("MontoNeto") = Convert.ToDecimal(txtMontoOperacional.Text)
            CargarPlaza()
            ddlPlaza.SelectedValue = oRow.Plaza
            Me.ddlGrupoInt.SelectedValue = oRow.GrupoIntermediario
            Session("CodigoOI") = Me.txtCodigoOrden.Value
            Dim dtAux As DataTable
            dtAux = (New TercerosBM().Seleccionar(oRow.CodigoTercero, DatosRequest)).Tables(0)
            If dtAux.Rows.Count > 0 Then
                Me.hdCustodio.Value = dtAux.Rows(0)("CodigoCustodio")
                CargarIntermediario()
                If ddlIntermediario.Items.Count > 1 Then
                    ddlIntermediario.SelectedValue = oRow.CodigoTercero
                    CargarContactos()
                    ddlContacto.SelectedValue = oRow.CodigoContacto
                Else
                    AlertaJS(ObtenerMensaje("CONF29"))
                End If
            Else
                AlertaJS(ObtenerMensaje("CONF29"))
            End If
        Catch ex As Exception
            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Protected Sub CargaPortafolioSegunUsuario(ByVal ddlFondo As DropDownList)
        Dim dtPortafolios As DataTable = New PortafolioBM().ListarPortafolioPorUsuario(Session("Login"))
        HelpCombo.LlenarComboBox(ddlFondo, dtPortafolios, "CodigoPortafolioSBS", "CodigoPortafolioSBS", True)
    End Sub
    Private Sub CargarCaracteristicasValor()
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM
        imgFechaOperacion.Attributes.Add("class", "input-append")
        Try
            dsValor = oOIFormulas.SeleccionarCaracValor_Pagares(Me.txtMnemonico.Text, DatosRequest)
            If dsValor.Tables(0).Rows.Count > 0 Then
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                Session("TipoRenta") = CType(drValor("val_TipoRenta"), String)
                Session("CodigoMoneda") = CType(drValor("val_CodigoMoneda"), String)
                If Not ((Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Or (Me.hdPagina.Value = "MODIFICA")) Then
                    Session("Mercado") = CType(drValor("val_Mercado"), String)
                End If
                Me.lblMoneda.Text = CType(drValor("val_CodigoMoneda"), String)
                Me.txtISIN.Text = CType(drValor("val_CodigoISIN"), String)
                Me.txtSBS.Text = CType(drValor("val_CodigoSBS"), String)
                Me.lblbasecupon.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseCupon")), Constantes.M_INT_NRO_DECIMALES), Integer)
                Me.lbldescripcion.Text = CType(drValor("val_Descripcion"), String)
                Me.lblbasetir.Text = CType(Math.Round(Convert.ToDecimal(drValor("val_BaseTir")), Constantes.M_INT_NRO_DECIMALES), Integer)
                Me.lblduracion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_Duracion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblemisor.Text = CType(drValor("val_Emisor"), String)
                Me.lblnominales.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesEmitidos")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblunidades.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_NominalesUnitarias")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblfecfinbono.Text = CType(drValor("val_FechaFinPagare"), String)
                Me.lblFecProxCupon.Text = CType(drValor("val_FechaProxCupon"), String)
                Me.lblFecUltCupon.Text = CType(drValor("val_FechaUltCupon"), String)
                Me.lblpreciovector.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_VectorPrecio")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.lblparticipacion.Text = Format(CType(Math.Round(Convert.ToDecimal(drValor("val_PorParticipacion")), Constantes.M_INT_NRO_DECIMALES), Decimal), "##,##0.0000000")
                Me.hdUnitarias.Value = CType(drValor("val_NominalesUnitarias"), String)
                If Session("EstadoPantalla") = "Ingresar" Or Me.hdPagina.Value = "TI" Then
                    Me.ddlTipoTasa.SelectedValue = CType(drValor("val_CodigoTipoCupon"), String)
                End If
                If drValor("val_Rescate").ToString = "N" Then
                    Me.lblRescate.Text = "NO"
                ElseIf drValor("val_Rescate").ToString = "S" Then
                    Me.lblRescate.Text = "SI"
                Else
                    Me.lblRescate.Text = ""
                End If
                ViewState("PrecioVector") = CType(drValor("val_VectorPrecio"), Decimal)
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
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
    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlFondo.Enabled = estado
        ddlOperacion.Enabled = estado
        btnBuscar.Enabled = estado
        txtSBS.ReadOnly = Not estado
        txtISIN.ReadOnly = Not estado
        txtMnemonico.ReadOnly = Not estado
    End Sub
    Private Sub HabilitaDeshabilitaDatosOperacionComision(ByVal estado As Boolean)
        Me.tbFechaOperacion.ReadOnly = Not estado
        Me.tbFechaLiquidacion.ReadOnly = Not estado
        Me.txtMnomOp.ReadOnly = Not estado
        Me.txtMnomOrd.ReadOnly = Not estado
        Me.ddlTipoTasa.Enabled = estado
        Me.txtYTM.ReadOnly = Not estado
        Me.txtPrecioNegoc.ReadOnly = Not estado
        Me.txtInteresCorNeg.ReadOnly = Not estado
        Me.ddlGrupoInt.Enabled = estado
        Me.ddlIntermediario.Enabled = estado
        Me.ddlContacto.Enabled = estado
        Me.txtMontoOperacional.ReadOnly = Not estado
        Me.txtPrecioNegSucio.ReadOnly = Not estado
        Me.tbHoraOperacion.ReadOnly = Not estado
        Me.txtNroPapeles.ReadOnly = Not estado
        Me.txtObservacion.ReadOnly = Not estado
        If estado Then
            imgFechaOperacion.Attributes.Add("class", "input-append date")
            imgFechaVcto.Attributes.Add("class", "input-append date")
        Else
            imgFechaOperacion.Attributes.Add("class", "input-append")
            imgFechaVcto.Attributes.Add("class", "input-append")
        End If
        If (Me.hdPagina.Value = "DA") Then
            Me.tbFechaOperacion.ReadOnly = True
            Me.imgFechaOperacion.Attributes.Add("class", "input-append")
        End If
        If Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS Then
            Me.chkRegulaSBS.Enabled = False
        Else
            Me.chkRegulaSBS.Enabled = estado
        End If
        dgLista.Enabled = estado
        Me.ddlPlaza.Enabled = estado
    End Sub
    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        HabilitaDeshabilitaDatosOperacionComision(False)
        OcultarBotonesInicio()
        Me.btnAceptar.Enabled = False
    End Sub
    Private Sub OcultarBotonesInicio()
        Me.btnBuscar.Visible = False
        Me.btnCaracteristicas.Visible = False
        Me.btnCuponera.Visible = False
        Me.btnAsignar.Visible = False
        Me.btnProcesar.Visible = False
        Me.btnImprimir.Visible = False
    End Sub
#End Region
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("SS_DatosModal") Is Nothing Then
            If hfModal.Value = "1" Then
                txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
                txtSBS.Text = CType(Session("SS_DatosModal"), String())(1)
                txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(2)
                ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3)
                ddlOperacion.SelectedValue = CType(Session("SS_DatosModal"), String())(4)
                lblMoneda.Text = CType(Session("SS_DatosModal"), String())(5)
                txtCodigoOrden.Value = CType(Session("SS_DatosModal"), String())(6)
            ElseIf hfModal.Value = "2" Then
                txtISIN.Text = CType(Session("SS_DatosModal"), String())(0).ToString.Trim()
                txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString.Trim()
                txtSBS.Text = CType(Session("SS_DatosModal"), String())(2).ToString.Trim()
            End If
            Session.Remove("SS_DatosModal")
            hfModal.Value = "0"
        End If
        Me.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Me.hdSaldo.Value = 0
        Me.btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If Not Page.IsPostBack Then
            Me.btnBuscar.Attributes.Add("onclick", "javascript:return ValidarFondo();")
            Me.btnProcesar.Attributes.Add("onclick", "javascript:return Validar();")
            btnRetornar.Attributes.Add("onClick", "javascript:history.back();return false;")
            LimpiarSesiones()
            If Not Request.QueryString("PTNeg") Is Nothing Then
                Me.hdPagina.Value = Request.QueryString("PTNeg")
            End If
            If (Me.hdPagina.Value = "TI") Then
                UIUtility.CargarOperacionOIParaTraspaso(ddlOperacion)
            Else
                UIUtility.CargarOperacionOI(ddlOperacion, "OperacionOI")
            End If
            CargarPlaza()
            UIUtility.CargarTipoCuponOI(ddlTipoTasa)
            UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
            CargarPaginaInicio()
            Me.hdPagina.Value = ""
            If Not Request.QueryString("PTNeg") Is Nothing Then
                ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                ddlFondo.DataValueField = "CodigoPortafolio"
                ddlFondo.DataTextField = "Descripcion"
                ddlFondo.DataBind()
                ddlFondo.Items.Insert(0, "--SELECCIONE--")
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
                    ControlarCamposTI()
                    txtPrecioNegSucio.Text = "0.0000000"
                    lblInteresCorrido.Text = "0.0000000"
                    txtNroPapeles.Text = "0.0000000"
                    txtInteresCorNeg.Text = "0.0000000"
                    Me.lblPrecioCal.Text = "0.0000000"
                    Me.txtPrecioNegoc.Text = "0.0000000"
                    txtMontoOperacional.Text = "0.00"
                    Me.txtYTM.Text = "0.0000000"
                    Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(Request.QueryString("fechaOperacion")))
                Else
                    Me.txtCodigoOrden.Value = Request.QueryString("PTNOrden")
                    Me.ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                    'Viene de la Pagina Confirmacion , Ejecucion  o Extorno de Ordenes de Inversion
                    If (Me.hdPagina.Value = "EO") Or (Me.hdPagina.Value = "CO") Or (Me.hdPagina.Value = "XO") Then
                        CargarDatosOrdenInversion()
                        CargarCaracteristicasValor()
                        tbNPoliza.Text = Right(txtCodigoOrden.Value, 4)
                        tbNPoliza.ReadOnly = True
                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        If (Me.hdPagina.Value <> "XO") Then
                            Me.lNPoliza.Visible = True
                            Me.tbNPoliza.ReadOnly = False
                            Me.tbNPoliza.Visible = True
                        End If
                        ControlarCamposEO_CO_XO()
                        CargarPaginaModificarEO_CO_XO(Me.hdPagina.Value)
                    Else
                        If (Me.hdPagina.Value = "OE") Then 'Viene de la Pagina Ordenes Excedidas
                            ControlarCamposOE()
                        Else
                            If (Me.hdPagina.Value = "DA") Then 'Viene de la Pagina Negociacion Dias Anteriores
                                ViewState("ORDEN") = "OI-DA"
                                Me.tbFechaOperacion.Text = Request.QueryString("Fecha")
                                Me.tbFechaOperacion.ReadOnly = True
                                Me.imgFechaOperacion.Attributes.Add("class", "input-append")
                            Else
                                If (Me.hdPagina.Value = "CP") Then 'Viene de la Pagina Liquidaciones Cuentas Por Pagar
                                    Call ConfiguraModoConsulta()
                                    ddlFondo.SelectedValue = Request.QueryString("PTFondo")
                                    txtMnemonico.Text = Request.QueryString("Mnemonico")
                                    txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                    ddlOperacion.SelectedValue = Request.QueryString("PTOperacion")
                                    Call CargarDatosOrdenInversion()
                                    Call CargarCaracteristicasValor()
                                    UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                    Call HabilitaBotones(False, False, False, False, False, False, False, False, False, False, False, False, True, False, False)
                                Else
                                    If (Me.hdPagina.Value = "CONSULTA") Then 'Viene de la Pagina CONSULTAR ORDEN PREORDEN
                                        ConfiguraModoConsulta()
                                        ddlFondo.SelectedValue = Request.QueryString("Portafolio")
                                        txtCodigoOrden.Value = Request.QueryString("CodigoOrden")
                                        Me.tbFechaOperacion.Text = Request.QueryString("FechaOperacion")
                                        CargarDatosOrdenInversion()
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
                                            CargarDatosOrdenInversion()
                                            CargarCaracteristicasValor()
                                            UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                                            HabilitaBotones(False, False, False, False, False, False, False, True, False, True, False, True, False, False, False)
                                            HabilitaDeshabilitaCabecera(False)
                                            HabilitaDeshabilitaDatosOperacionComision(True)
                                            Session("EstadoPantalla") = "Modificar"
                                            lblAccion.Text = "Modificar"
                                            hdMensaje.Value = "la Modificación"
                                            HelpCombo.CargarMotivosCambio(Me)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Me.btnSalir.Attributes.Remove("onClick")
                Me.btnSalir.Attributes.Add("onClick", "javascript:return Confirmar();")
                Me.btnAceptar.Attributes.Add("onClick", "javascript:return Confirmacion();")
            Else
                ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
                ddlFondo.DataValueField = "CodigoPortafolio"
                ddlFondo.DataTextField = "Descripcion"
                ddlFondo.DataBind()
                ddlFondo.Items.Insert(0, "--SELECCIONE--")
            End If
        End If
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        If Me.hdPagina.Value <> "" And Me.hdPagina.Value <> "DA" And Me.hdPagina.Value <> "TI" And Me.hdPagina.Value <> "MODIFICA" Then
            If Me.hdPagina.Value = "EO" Or Me.hdPagina.Value = "CO" Then
                ModificarOrdenInversion()
                UIUtility.InsertarModificarImpuestosComisiones("M", dgLista, txtCodigoOrden.Value, ddlPlaza.SelectedValue, Session("TipoRenta"),
                ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                Session("Modificar") = 0
                CargarPaginaAceptar()
            End If
            If Me.hdPagina.Value = "XO" Then
                oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.DatosRequest)
                ReturnArgumentShowDialogPopup()
            Else
                If Me.hdPagina.Value = "EO" Then
                    oOrdenInversionWorkFlowBM.EjecutarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.tbNPoliza.Text.Trim, Me.DatosRequest)
                    ReturnArgumentShowDialogPopup()
                Else
                    If Me.hdPagina.Value = "CO" Then
                        oOrdenInversionWorkFlowBM.ConfirmarOI(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, Me.tbNPoliza.Text.Trim, Me.DatosRequest)
                        ReturnArgumentShowDialogPopup()
                    End If
                End If
            End If
        Else
            If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Then
                Dim strAlerta As String = ""
                If ddlMotivoCambio.SelectedIndex <= 0 Then
                    strAlerta = "-Elija el motivo por el cual desea " & Session("EstadoPantalla") & " esta operación.\n"
                End If
                If txtComentarios.Text.Trim.Length <= 0 Then
                    strAlerta += "-Ingrese los comentarios por el cual desea " & Session("EstadoPantalla") & " esta operación."
                End If
                If strAlerta.Length > 0 Then
                    AlertaJS(strAlerta)
                    Exit Sub
                End If
            End If
            actualizaMontos()
            If Session("EstadoPantalla") = "Ingresar" Then
                If Session("Procesar") = 1 Then
                    Dim strcodigoOrden As String
                    strcodigoOrden = InsertarOrdenInversion()
                    If strcodigoOrden <> "" Then
                        If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                            Dim toUser As String = ""
                            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                            Session("dtValTrading") = ""
                        End If
                    End If
                    oOrdenInversionWorkFlowBM.EjecutarOI(strcodigoOrden, Me.ddlFondo.SelectedValue, "", Me.DatosRequest)
                    Me.txtCodigoOrden.Value = strcodigoOrden
                    If Me.hdPagina.Value <> "TI" Then
                        UIUtility.InsertarModificarImpuestosComisiones("I", dgLista, strcodigoOrden, ddlPlaza.SelectedValue, Session("TipoRenta"),
                        ddlFondo.SelectedValue.Trim, DatosRequest, ddlPlaza.SelectedValue)
                    End If
                    Session("dtdatosoperacion") = ObtenerDatosOperacion()
                    GenerarLlamado(strcodigoOrden + ViewState("CodigoOrden_T"), Me.ddlFondo.SelectedValue, "PAGARES", Me.ddlOperacion.SelectedItem.Text,
                    Session("CodigoMoneda"), Me.txtISIN.Text, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ddlFondo.SelectedItem.Text)
                    Session("CodigoOI") = strcodigoOrden
                    CargarPaginaAceptar()
                End If
            Else
                If Session("EstadoPantalla") = "Modificar" Then
                    actualizaMontos()
                    ModificarOrdenInversion()
                    FechaEliminarModificarOI("M")
                    Session("Modificar") = 0
                    CargarPaginaAceptar()
                    Session("dtdatosoperacion") = ObtenerDatosOperacion()
                    If Me.hdPagina.Value <> "MODIFICA" Then
                        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "PAGARES", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"),
                        txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ddlFondo.SelectedItem.Text)
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
                End If
            End If
            If Session("Procesar") = 0 And (Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Ingresar") Then
                If CType(ViewState("MontoNeto"), String) = "" Then
                    AlertaJS(ObtenerMensaje("CONF9"))
                End If
                EliminarCuponerasOITemporales()
            End If
        End If
    End Sub
    Protected Sub btnAsignar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
        Session("URL_Anterior") = Page.Request.Url.AbsolutePath.ToString
        If ddlFondo.SelectedValue.ToString = PORTAFOLIO_MULTIFONDOS Then
            Response.Redirect("../AsignacionFondos/frmIngresoCriteriosAsignacion.aspx?vISIN=" & Me.txtISIN.Text.ToString & "&vCantidad=" & Me.txtMnomOp.Text.ToString &
            "&vMnemonico=" & Me.txtMnemonico.Text.ToString & "&vFondo=" & ddlFondo.SelectedValue.ToString & "&vOperacion=" & ddlOperacion.SelectedItem.Text & "&vMoneda=" &
            Me.lblMoneda.Text & "&vImpuestosComisiones=" & Me.txttotalComisionesC.Text & "&vCodigoOrden=" & Me.txtCodigoOrden.Value & "&vCategoria=PA", False)
        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        txtPrecioNegSucio.Text = "0.0000000"
        lblInteresCorrido.Text = "0.0000000"
        txtNroPapeles.Text = "0.0000000"
        txtInteresCorNeg.Text = "0.0000000"
        Me.lblPrecioCal.Text = "0.0000000"
        Me.txtPrecioNegoc.Text = "0.0000000"
        txtMontoOperacional.Text = "0.00"
        Me.txtYTM.Text = "0.0000000"
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
                ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlFondo.SelectedValue,
                ddlOperacion.SelectedValue, "PA", 1, ddlFondo.SelectedItem.Text)
                Session("Busqueda") = 2
            Else
                If Session("Busqueda") = 1 Then
                    CargarFechaVencimiento()
                    If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                        UIUtility.AsignarMensajeBoton(btnAceptar, "CONF1")
                    Else
                        UIUtility.AsignarMensajeBoton(btnAceptar, "CONF15")
                    End If
                    CargarPaginaIngresar()
                    CargarCaracteristicasValor()
                    CargarIntermediario()
                    If UIUtility.ObtenerCodigoTipoOperacion("COMPRA") = Me.ddlOperacion.SelectedValue Then
                        Me.ddlFondo.Enabled = True
                    End If
                Else
                    Session("Busqueda") = 0
                End If
            End If
        Else
            If Session("EstadoPantalla") = "Modificar" Or Session("EstadoPantalla") = "Eliminar" Or Session("EstadoPantalla") = "Consultar" Then
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
                    ddlFondo.SelectedValue, ddlOperacion.SelectedValue, lblMoneda.Text.ToString, strAux, strAccion, ddlFondo.SelectedItem.Text)
                    Session("Busqueda") = 2
                Else
                    If Session("Busqueda") = 1 Then
                        CargarCaracteristicasValor()
                        CargarDatosOrdenInversion()
                        UIUtility.ObtieneImpuestosComisionesGuardado(Me.dgLista, txtCodigoOrden.Value, Me.ddlFondo.SelectedValue)
                        Me.btnAceptar.Enabled = True
                        Session("ValorCustodio") = UIUtility.ObtieneCustodiosOI(txtCodigoOrden.Value, ddlFondo.SelectedValue, DatosRequest, hdCustodio.Value, hdSaldo.Value)
                        If Session("EstadoPantalla") = "Modificar" Then
                            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF2", "Nro " + txtSBS.Text + "?")
                            Else
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF16", "Nro " + txtSBS.Text + "?")
                            End If
                            CargarPaginaModificar()
                        ElseIf Session("EstadoPantalla") = "Eliminar" Then
                            If Me.ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF3", "Nro " + txtSBS.Text + "?")
                            Else
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF17", "Nro " + txtSBS.Text + "?")
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
    End Sub
    Protected Sub btnCaracteristicas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaracteristicas.Click
        If Me.txtMnemonico.Text <> "" Then
            Session("accionValor") = "MODIFICAR"
            EjecutarJS(UIUtility.MostrarPopUp("../../Parametria/AdministracionValores/frmAdministracionValores.aspx?cod=" + Me.txtMnemonico.Text + "&vOI=T", "10",
            1000, 600, 0, 0, "No", "No", "Yes", "Yes"), False)
        Else
            AlertaJS(ObtenerMensaje("CONF23"))
        End If
    End Sub
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row hidden")
        LimpiarSesiones()
        Call ConfiguraModoConsulta()
    End Sub
    Protected Sub btnCuponera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCuponera.Click
        ShowDialogCuponera(txtMnemonico.Text.ToString)
    End Sub
    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        LimpiarSesiones()
        UIUtility.ExcluirOtroElementoSeleccion(Me.ddlFondo)
        UIUtility.InsertarOtroElementoSeleccion(Me.ddlOperacion, "")
        Session("EstadoPantalla") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        CargarPaginaAccion()
        hdMensaje.Value = "la Eliminación"
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Session("dtdatosoperacion") = ObtenerDatosOperacion()
        GenerarLlamado(Me.txtCodigoOrden.Value, Me.ddlFondo.SelectedValue, "PAGARES", Me.ddlOperacion.SelectedItem.Text, Session("CodigoMoneda"),
        txtISIN.Text.Trim, Me.txtSBS.Text.Trim, Me.txtMnemonico.Text, ddlFondo.SelectedItem.Text)
    End Sub
    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "hidden")
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
        If (Me.hdPagina.Value <> "DA") Then
            tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Else
            tbFechaOperacion.Text = Request.QueryString("Fecha")
        End If
        Me.tbHoraOperacion.Text = objutil.RetornarHoraSistema
        lblAccion.Text = "Ingresar"
        hdMensaje.Value = "el Ingreso"
        ViewState("IsIndica") = True
        hdNumUnidades.Value = 0
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            Me.ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "PreOrden de Inversión - PAGARES"
    End Sub
    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        trMotivoCambio.Attributes.Remove("class")
        trMotivoCambio.Attributes.Add("class", "row")
        LimpiarSesiones()
        'GUID: Identificador único para cuponetas temporales
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
        HelpCombo.CargarMotivosCambio(Me)
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Se quita parametro GUID y DataRequest
    'calcularTasanegociacion, calcularTasanegociacionPrecio, CalcularInteresesCorridos2, CalcularNumeroPapeles
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        If ValidarFechas() = True Then
            If UIUtility.ValidarHora(Me.tbHoraOperacion.Text) = False Then
                AlertaJS(ObtenerMensaje("CONF22"))
                Exit Sub
            End If
            Session("Procesar") = 1
            If Me.hdPagina.Value <> "TI" Then
                Dim oLimiteEvaluacion As New LimiteEvaluacionBM
                Dim GUID As String = System.Guid.NewGuid().ToString()
                Dim dsAux As New DataSet
                Dim codigoOperacion As String = ddlOperacion.SelectedValue.ToString()
                Dim codigoNemonico As String = txtMnemonico.Text.ToString()
                Dim cantidadValor As Decimal = 0
                Dim montoNominal As Decimal = Convert.ToDecimal(Me.txtMnomOp.Text.ToString())
                Dim codigoPortafolio As String = ddlFondo.SelectedValue.ToString()
                Dim fechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.ToString())
                dsAux = oLimiteEvaluacion.Evaluar(codigoOperacion, codigoNemonico, cantidadValor, montoNominal, codigoPortafolio, fechaOperacion, Me.DatosRequest)
                Session(GUID) = dsAux
                ViewState("GUID_Limites") = GUID
                If (dsAux.Tables(0).Rows.Count > 0) Then
                    Session("Instrumento") = "PAGARES"
                    EjecutarJS(UIUtility.MostrarPopUp("frmConsultaLimitesInstrumento.aspx?GUID=" + GUID, "no", 1000, 500, 50, 5, "no", "yes", "yes", "yes"))
                End If
            End If
            'Calculo de formulas
            Dim objFormulasOI As New OrdenInversionFormulasBM
            Dim vpnegociacion As Decimal, vpncupon As Decimal, Tir As Decimal, InteresCorrido As Decimal, numeropapeles As Decimal, preciolimpio As Decimal
            If (CDec(txtYTM.Text.Trim) = 0 And CDec(txtMontoOperacional.Text.Trim) <> 0) Then
                txtYTM.Text = Format(objFormulasOI.calcularTasanegociacion(txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                Convert.ToDecimal(txtMnomOp.Text), Me.ddlTipoTasa.SelectedValue, Convert.ToDecimal(txtMontoOperacional.Text)), "##,##0.0000000")
            End If
            If (txtYTM.Text.Trim = "0.0000000" And txtPrecioNegoc.Text.Trim <> "0.0000000") Then
                txtYTM.Text = Format(objFormulasOI.calcularTasanegociacionPrecio(txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                Convert.ToDecimal(txtMnomOp.Text), Me.ddlTipoTasa.SelectedValue, Convert.ToDecimal(txtPrecioNegoc.Text)), "##,##0.0000000")
            End If
            InteresCorrido = objFormulasOI.CalcularInteresesCorridos2(Convert.ToString(ViewState("CuponeraTemporalGUID")), txtMnemonico.Text, _
            UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), Convert.ToDecimal(txtMnomOp.Text), Convert.ToDecimal(txtYTM.Text), Me.ddlTipoTasa.SelectedValue)
            If InteresCorrido = -1 Then
                AlertaJS("Los Datos ingresados no son consistente para el calculo del interes corrido")
                Exit Sub
            End If
            lblInteresCorrido.Text = Format(Convert.ToDecimal(InteresCorrido), "##,##0.0000000")
            txtInteresCorNeg.Text = Format(Convert.ToDecimal(InteresCorrido), "##,##0.0000000")
            vpnegociacion = Convert.ToString(objFormulasOI.CalcularVPN2("N", txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
            Convert.ToString(ViewState("CuponeraTemporalGUID")), Convert.ToDecimal(txtMnomOp.Text), Convert.ToDecimal(txtYTM.Text), "N", DatosRequest))
            vpncupon = Convert.ToString(objFormulasOI.CalcularVPN2("N", txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
            Convert.ToString(ViewState("CuponeraTemporalGUID")), Convert.ToDecimal(txtMnomOp.Text), Convert.ToDecimal(txtYTM.Text), "C", DatosRequest))
            If Double.Parse(txtMontoOperacional.Text) = 0 Then
                txtMontoOperacional.Text = Format(Math.Round(Convert.ToDecimal(objFormulasOI.CalcularMontoOperacion2(Convert.ToString(ViewState("CuponeraTemporalGUID")), _
                txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), Convert.ToDecimal(txtMnomOp.Text), Convert.ToDecimal(txtYTM.Text), _
                Me.ddlTipoTasa.SelectedValue, DatosRequest)), 2), "##,##0.00")
            End If
            numeropapeles = objFormulasOI.CalcularNumeroPapeles(Convert.ToDecimal(txtMnomOp.Text), Convert.ToDecimal(lblunidades.Text))
            txtPrecioNegSucio.Text = Format(Convert.ToDecimal(objFormulasOI.CalcularPrecioSucio3(Convert.ToDecimal(txtMontoOperacional.Text), _
            Convert.ToDecimal(txtMnomOp.Text), DatosRequest)) * 100, "##,##0.0000000")
            txtNroPapeles.Text = Format(numeropapeles, "##,##0.0000000")
            If Double.Parse(txtPrecioNegoc.Text) = 0 Then
                preciolimpio = objFormulasOI.CalcularPrecioLimpio2(Convert.ToDecimal(txtMontoOperacional.Text), Convert.ToDecimal(txtMnomOp.Text), _
                InteresCorrido, DatosRequest) * 100
                Me.lblPrecioCal.Text = Format(Convert.ToDecimal(preciolimpio), "##,##0.0000000")
                Me.txtPrecioNegoc.Text = Format(Convert.ToDecimal(preciolimpio), "##,##0.0000000")
            End If
            Tir = objFormulasOI.CalcularTIR(txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
            Convert.ToString(ViewState("CuponeraTemporalGUID")), Convert.ToDecimal(txtMnomOp.Text), DatosRequest)
            lblduracion.Text = 0
            Dim ds As DataSet
            Dim Mensaje As String
            ds = New OrdenPreOrdenInversionBM().ValidacionPuntual_LimitesTrading(txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
            ddlFondo.SelectedValue, Convert.ToDecimal(txtNroPapeles.Text.Replace(",", "").Replace(".", UIUtility.DecimalSeparator)), Session("CodigoMoneda"), _
            Usuario.ToString.Trim, String.Empty)
            If ds.Tables(0).Rows.Count > 0 Then
                Mensaje = "El usuario no esta permitido grabar la operación, el monto de negociado excede el límite de autonomía por Trader:\n\n"
                For Each fila As DataRow In ds.Tables(0).Rows
                    Mensaje = Mensaje & "- Usuario (" & fila("TipoCargoExc") & ") excedió límite de autonomía \""" & fila("GrupoLimTrd") &
                    "\"", debe ser autorizado por un usuario " & fila("TipoCargoAut") & " (" & fila("TraderAut") & "). \n\n"
                Next
                Mensaje = Mensaje & "La operación debe ser grabada por el usuario autorizado haciendo clic en el botón Aceptar de la orden de inversión."
                AlertaJS(Mensaje)
                Session("dtValTrading") = ds.Tables(0)
            End If
            If oValoresBM.VerificarCalculoComisiones(Me.txtMnemonico.Text(), Me.ddlIntermediario.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text),
            Me.ddlOperacion.SelectedValue) = 0 Then
                CalcularComisiones()
            Else
                Me.txtMontoNetoOpe.Text = Format(Convert.ToDecimal(Me.txtMontoOperacional.Text), "##,##0.0000")
            End If
            CargarPaginaProcesar()
        End If
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Protected Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If ddlFondo.SelectedValue <> "" Then
            CargarFechaVencimiento()
        End If
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If cantidadreg > 0 Then
            AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
            ddlFondo.SelectedIndex = 0
            Exit Sub
        End If
    End Sub
    Protected Sub ddlGrupoInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
        CargarContactos()
    End Sub
    Protected Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        CargarContactos()
        CargarFechaVencimiento()
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se filtra las comisiones por tipo de intermediario cuando el mercado es extranjero | 11/06/18 
    End Sub
    Protected Sub ddlPlaza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaza.SelectedIndexChanged
        Session("Mercado") = ddlPlaza.SelectedValue
        Me.dgLista.Dispose()
        Me.dgLista.DataBind()
        OrdenInversion.ObtieneImpuestosComisiones(Me.dgLista, Session("Mercado"), Session("TipoRenta"), ddlIntermediario.SelectedValue)
        dgLista.Visible = True
    End Sub
    Public Sub CargarPlaza()
        Dim oPlazaBM As New PlazaBM
        ddlPlaza.DataSource = oPlazaBM.Listar(Nothing)
        ddlPlaza.DataTextField = "Descripcion"
        ddlPlaza.DataValueField = "CodigoPlaza"
        ddlPlaza.DataBind()
        ddlPlaza.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub
End Class