Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmWarrants
    Inherits BasePage
    Dim oCuponera As New CuponeraBM
    Dim oPortafolioBM As New PortafolioBM
    Dim oPeriodicidadBM As New PeriodicidadBM
    Dim oTipoAmortizacionBM As New TipoAmortizacionBM
    Dim oValoresBM As New ValoresBM
    Dim objutil As New UtilDM
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oOIFormulas As New OrdenInversionFormulasBM
    Private Sub ReturnArgumentShowDialogPopup()
        Dim script As New StringBuilder
        If Me.hdPagina.Value = "CO" Then
            With script
                .Append("<script>")
                .Append("   alertify.alert('Se Confirmó la orden correctamente');")
                .Append("   window.close()")
                .Append("</script>")
            End With
        End If
        EjecutarJS(script.ToString(), False)
    End Sub
    Sub FechaPortafolio()
        If ddlFondo.SelectedValue = "" Then
            tbFechaOperacion.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
        Else
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue))
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session("SS_DatosModal") IsNot Nothing Then ObtenerValoresDesdePopup()
        If Not Page.IsPostBack Then
            ViewState("Load") = "0"
            HelpCombo.LlenarComboBox(ddlFondo, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "--Seleccione--")
            UIUtility.CargarGrupoIntermediarioOI(ddlGrupoInt)
            CargarIntermediario()
            btnProcesar.Enabled = True
            btnRetornar.Enabled = True
            hdPagina.Value = Request.QueryString("PTNeg")
            If Request.QueryString("cod") = "" Then
                Ingresar()
            Else
                Modificar(Request.QueryString("cod"))
                ddlFondo.Enabled = False
                If Request.QueryString("estado") = "E-ELI" Or Request.QueryString("estado") = "E-CON" Then
                    btnProcesar.Enabled = False
                    btnAceptar.Enabled = False
                Else
                    btnAceptar.Enabled = True
                End If
            End If
        End If
    End Sub
    Private Sub CargarPaginaAccion(Estado As Boolean)
        'tbFechaOperacion.Enabled = Estado
        ddlFondo.Enabled = Estado
        btnBuscar.Enabled = Estado
        tbFechaLiquidacion.Enabled = Estado
        txtISIN.Enabled = Estado
        txtMnemonico.Enabled = Estado
        txtSBS.Enabled = Estado
        tbHoraOperacion.Enabled = Estado
        txtUnidades.Enabled = Estado
        txtUnidadesOperacion.Enabled = Estado
        txtPrima.Enabled = Estado
        ddlGrupoInt.Enabled = Estado
        ddlIntermediario.Enabled = Estado
        ddlContacto.Enabled = Estado
        txtObservacion.Enabled = Estado
    End Sub
    Sub Ingresar()
        CargarPaginaAccion(True)
        ViewState("EstadoPantalla") = "Ingresar"
        If (hdPagina.Value <> "DA") Then
            tbFechaOperacion.Text = objutil.RetornarFechaNegocio
        Else
            tbFechaOperacion.Text = Request.QueryString("Fecha")
        End If
        lblAccion.Text = "Ingresar"
        btnRetornar.Enabled = True
        btnAceptar.Enabled = False
    End Sub
    Sub Modificar(CodigoOrden As String)
        Try
            Dim dtOrde As DataTable = oOrdenInversionBM.ConsultaOrden(ddlFondo.SelectedValue, 0, CodigoOrden, "WA")
            If dtOrde.Rows.Count > 0 Then
                ViewState("EstadoPantalla") = "Modificar"
                lblMoneda.Text = dtOrde(0)("CodigoMoneda")
                ddlOperacion.SelectedValue = dtOrde(0)("CodigoOperacion")
                txtISIN.Text = dtOrde(0)("CodigoISIN")
                txtMnemonico.Text = dtOrde(0)("CodigoMnemonico")
                CargarCaracteristicasValor(txtMnemonico.Text)
                txtSBS.Text = dtOrde(0)("CodigoSBS")
                CargarPaginaAccion(True)
                hdCodigoOrden.Value = CodigoOrden
                ddlFondo.SelectedValue = dtOrde(0)("CodigoPortafolioSBS")
                txtValorNominal.Text = dtOrde(0)("MontoNominalOperacion")
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CDec(dtOrde(0)("FechaOperacion")))
                tbFechaLiquidacion.Text = UIUtility.ConvertirFechaaString(CDec(dtOrde(0)("FechaLiquidacion")))
                tbHoraOperacion.Text = dtOrde(0)("HoraOperacion")
                txtUnidades.Text = dtOrde(0)("CantidadOperacion")
                txtUnidadesOperacion.Text = dtOrde(0)("CantidadOrdenado")
                txtMontoOperacion.Text = dtOrde(0)("MontoNetoOperacion")
                txtNocional.Text = dtOrde(0)("MontoOperacion")
                txtPrima.Text = dtOrde(0)("Tasa")
                txtMontoOperacion.Text = dtOrde(0)("MontoNetoOperacion")
                ddlGrupoInt.SelectedValue = dtOrde(0)("GrupoIntermediario")
                ddlIntermediario.SelectedValue = dtOrde(0)("CodigoTercero")
                ddlContacto.SelectedValue = dtOrde(0)("CodigoContacto")
                txtObservacion.Text = dtOrde(0)("Observacion")
                ddlOpcion.SelectedValue = dtOrde(0)("TipoFondo")
            Else
                AlertaJS("No hay información.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Sub BotonesCancelar()
        btnRetornar.Enabled = False
        btnAceptar.Enabled = False
    End Sub
    Sub Limpiar()
        CargarPaginaAccion(False)
        ViewState("EstadoPantalla") = ""
        BotonesCancelar()
    End Sub
    Public Function crearObjetoOI() As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        oOrdenInversionBM.InicializarOrdenInversion(oRow)
        oRow.CodigoOrden = hdCodigoOrden.Value.Trim
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoOperacion = ddlOperacion.SelectedValue
        oRow.CodigoISIN = txtISIN.Text
        oRow.CodigoMnemonico = txtMnemonico.Text
        oRow.CodigoSBS = txtSBS.Text
        oRow.GrupoIntermediario = ddlGrupoInt.SelectedValue
        oRow.CodigoTercero = ddlIntermediario.SelectedValue
        oRow.CodigoContacto = ddlContacto.SelectedValue
        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacion.Text)
        oRow.CantidadOrdenado = txtUnidades.Text
        oRow.CantidadOperacion = txtUnidadesOperacion.Text
        oRow.MontoOperacion = CDec(txtNocional.Text)
        oRow.MontoNetoOperacion = CDec(txtMontoOperacion.Text)
        oRow.Situacion = "A"
        oRow.Observacion = txtObservacion.Text
        oRow.HoraOperacion = tbHoraOperacion.Text
        oRow.CategoriaInstrumento = "WA"
        oRow.CodigoMoneda = lblMoneda.Text
        oRow.Estado = "E-EJE"
        oRow.IndicaCambio = "1"
        oRow.EventoFuturo = 1
        oRow.Ficticia = "N"
        oRow.MontoNominalOperacion = CDec(txtValorNominal.Text)
        oRow.RegulaSBS = "N"
        oRow.TasaPorcentaje = txtPrima.Text
        oRow.TipoFondo = ddlOpcion.SelectedValue
        oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
        oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Return oOrdenInversionBE
    End Function
    Sub LimpiaIsin()
        txtISIN.Text = ""
        txtMnemonico.Text = ""
        txtSBS.Text = ""
    End Sub
    Private Sub ShowDialogPopupValores(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal fondo As String, ByVal cfondo As String, ByVal operacion As String, ByVal categoria As String, ByVal valor As String)
        Dim strURL As String = "frmBuscarValor.aspx?vISIN=" & isin & "&vSBS=" & sbs & "&vMnemonico=" & mnemonico & "&cFondo=" & cfondo & "&vFondo=" & fondo & "&vOperacion=" & operacion & "&vCategoria=" & categoria
        EjecutarJS("showModalDialog('" & strURL & "', '950', '600', '" & btnBuscar.ClientID & "');")
    End Sub
    Private Sub CargarCaracteristicasValor(Nemonico As String)
        Dim dsValor As New DataSet
        Dim drValor As DataRow
        Dim oOIFormulas As New OrdenInversionFormulasBM
        Dim sPortafolio As String = ""
        Try
            dsValor = oOIFormulas.SeleccionarCaracValor_Acciones(ddlFondo.SelectedValue, Nemonico, DatosRequest)
            If dsValor.Tables(0).Rows.Count > 0 Then
                drValor = dsValor.Tables(0).NewRow
                drValor = dsValor.Tables(0).Rows(0)
                lblDescripcioIns.Text = drValor("val_Descripcion")
                txtMoneda.Text = drValor("val_CodigoMoneda").ToString
                LBLTamaño.Text = drValor("TamanoEmision").ToString
                TXTEmisor.Text = drValor("Tercero").ToString
                txtPrecioEjercicio.Text = drValor("PrecioEjercicio").ToString
                lblFechaEmision.Text = UIUtility.ConvertirFechaaString(CDec(drValor("FechaEmision")))
                txtGarante.Text = drValor("Garante").ToString
                txtRangoBajo.Text = Math.Round(CDec(drValor("MontoPiso")), 2).ToString + " %"
                txtFechaVencimiento.Text = UIUtility.ConvertirFechaaString(CDec(drValor("FechaVencimiento")))
                txtSubyacente.Text = drValor("Subyacente").ToString
                txtRangoAlto.Text = Math.Round(CDec(drValor("MontoTecho")), 2).ToString + " %"
                txtValorNominal.Text = drValor("ValorNominal").ToString
            End If
        Catch ex As Exception
            AlertaJS(ObtenerMensaje("CONF21"))
        End Try
    End Sub
    Sub ObtenerValoresDesdePopup()
        Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
        txtISIN.Text = datosModal(0)
        txtMnemonico.Text = datosModal(1)
        txtSBS.Text = datosModal(2)
        Session.Remove("SS_DatosModal")
        CargarCaracteristicasValor(txtMnemonico.Text)
    End Sub
    Public Sub CargarIntermediario()
        UIUtility.CargarIntermediariosXGrupoOI(ddlIntermediario, ddlGrupoInt.SelectedValue)
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        If Not ViewState("Load") = "1" Then
            If Me.ddlFondo.SelectedValue = "" Then
                AlertaJS("Seleccione un portafolio para continuar.")
                Exit Sub
            End If
            ShowDialogPopupValores(txtISIN.Text.Trim.ToUpper, txtSBS.Text.Trim.ToUpper, txtMnemonico.Text.Trim.ToUpper, ddlFondo.SelectedItem.Text, ddlFondo.SelectedValue, "2", "WA", 1)
            ViewState("Load") = "1"
            btnAceptar.Attributes.Add("onclick", "this.disabled = true; this.value = 'en proceso...'; __doPostBack('btnAceptar','');")
        Else
            ViewState("Load") = "0"
        End If
    End Sub
    Sub CargarFechaVencimiento()
        If Not ddlFondo.SelectedValue = "" Then
            Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(ddlFondo.SelectedValue, DatosRequest).Tables(0)
            lblMoneda.Text = dtAux.Rows(0)("CodigoMoneda")
            If dtAux.Rows.Count > 0 Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
            End If
        End If
    End Sub
    Protected Sub ddlFondo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If Not ddlFondo.SelectedValue = "" Then
            CargarFechaVencimiento()
        End If
        Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If cantidadreg > 0 Then
            AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
            ddlFondo.SelectedIndex = 0
            Exit Sub
        End If
    End Sub
    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmWarrantsListar.aspx")
    End Sub
    Public Function InsertarOrdenInversion() As String
        Dim strCodigoOI As String
        oOrdenInversionBE = crearObjetoOI()
        strCodigoOI = oOrdenInversionBM.InsertarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
        Return strCodigoOI
    End Function
    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE = crearObjetoOI()
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE, hdPagina.Value, CType(Session("ValorCustodio"), String), DatosRequest)
    End Sub
    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Request.QueryString("PTNeg") = "" Then
                Dim CodigoOrden As String
                If ViewState("EstadoPantalla") = "Ingresar" Then
                    CodigoOrden = InsertarOrdenInversion()
                Else
                    CodigoOrden = hdCodigoOrden.Value
                    ModificarOrdenInversion()
                End If
                Limpiar()
                Response.Redirect("frmWarrantsListar.aspx")
            Else
                If Trim(txtISIN.Text) = "" Then
                    AlertaJS("Debe ingresar el codigo ISIN de la operación.", "('#txtISINOperacion').focus();")
                Else
                    oOrdenInversionBM.ConfirmaOrdenInversion(hdCodigoOrden.Value, ddlFondo.SelectedValue, txtISIN.Text, DatosRequest)
                    ReturnArgumentShowDialogPopup()
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ddlGrupoInt_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlGrupoInt.SelectedIndexChanged
        CargarIntermediario()
    End Sub
    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click
        Try
            If ddlFondo.SelectedValue = "" Then
                AlertaJS("Seleccione un portafolio para continuar.", "$('#ddlFondo').focus();")
            ElseIf CDec(txtValorNominal.Text) = 0 Then
                AlertaJS("Seleccione una emisión para continuar.", "$('#btnBuscar').focus();")
            ElseIf CDec(txtPrima.Text) = 0 Then
                AlertaJS("Ingresa la Prima para continuar.", "$('#txtPrima').focus();")
            ElseIf CDec(txtUnidades.Text) = 0 Then
                AlertaJS("Ingresa las Unidades Ordenadas.", "$('#txtUnidades').focus();")
            Else
                txtNocional.Text = Math.Round((CDec(txtValorNominal.Text) * CDec(txtUnidades.Text)), 7).ToString
                txtMontoOperacion.Text = Math.Round((CDec(txtNocional.Text) * (CDec(txtPrima.Text) / 100)), 7).ToString

                Dim strJS As New StringBuilder
                btnAceptar.Enabled = True
                strJS.AppendLine("$('#btnAceptar').removeAttr('disabled');")
                EjecutarJS(strJS.ToString())
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
            btnAceptar.Enabled = False
        End Try
    End Sub
    Protected Sub txtUnidades_TextChanged(sender As Object, e As System.EventArgs) Handles txtUnidades.TextChanged
        txtUnidadesOperacion.Text = txtUnidades.Text
    End Sub
    Protected Sub chkajuste_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkajuste.CheckedChanged
        If chkajuste.Checked Then
            txtMontoOperacion.Enabled = True
        Else
            txtMontoOperacion.Enabled = False
        End If
    End Sub
End Class