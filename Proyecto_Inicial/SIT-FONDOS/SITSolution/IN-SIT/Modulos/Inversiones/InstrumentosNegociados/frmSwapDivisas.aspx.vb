Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_InstrumentosNegociados_frmSwapDivisas
    Inherits BasePage
#Region "Variables"
    Dim objutil As New UtilDM
    Dim oImpComOP As New ImpuestosComisionesOrdenPreOrdenBM
    Dim oOrdenInversionBE1 As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBE2 As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
    Dim oPortafolioBM As New PortafolioBM
    Dim strTipoRenta As String
    Dim oMonedaBM As New MonedaBM
    Dim oTercerosBM As New TercerosBM
    Dim oContactoBM As New ContactoBM
    Dim oMotivoBM As New MotivoBM
    Dim DECIMAL_NULO As Decimal = CDec(0.0)
    Dim strAccion As String
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        btnSalir.Attributes.Add("onClick", "javascript:return Salida();")
        If Not Page.IsPostBack Then
            LimpiarSesiones()
            ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlFondo.DataValueField = "CodigoPortafolio"
            ddlFondo.DataTextField = "Descripcion"
            ddlFondo.DataBind()
            ddlFondo.Items.Insert(0, New ListItem("--Seleccione--", ""))
            CargarOperacionOI(ddlOperacion1, "")
            CargarOperacionOI(ddlOperacion2, ddlOperacion1.SelectedValue.ToString)
            CargarPaginaInicio()
        End If
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Call HabilitaBotones(True, True, True, True, True, True, True, True, True, True, True, False)
    End Sub
    Private Sub HabilitaBotones(ByVal bLimites As Boolean, ByVal bIngresar As Boolean, _
                                ByVal bModificar As Boolean, ByVal bEliminar As Boolean, ByVal bConsultar As Boolean, _
                                ByVal bAsignar As Boolean, ByVal bProcesar As Boolean, ByVal bImprimir As Boolean, _
                                ByVal bAceptar As Boolean, ByVal bBuscar As Boolean, ByVal bSalir As Boolean, _
                                ByVal bRetornar As Boolean)
        btnIngresar.Visible = bIngresar
        btnModificar.Visible = bModificar
        btnEliminar.Visible = bEliminar
        btnConsultar.Visible = bConsultar
        btnAceptar.Visible = bAceptar
        btnBuscar.Visible = bBuscar
        btnSalir.Visible = bSalir
        btnRetornar.Visible = bRetornar
    End Sub
    Private Sub LimpiarSesiones()
        Session("EstadoPantallaSW") = Nothing
        Session("Busqueda") = Nothing
        Session("OrdenInversionBE1") = Nothing
        Session("Comentarios1") = Nothing
        Session("Oper1") = Nothing
        Session("OrdenInversionBE2") = Nothing
        Session("Comentarios2") = Nothing
        Session("Oper2") = Nothing
        Session("MontoOperacion") = Nothing
        Session("Intermediario") = Nothing
        Session("Contacto") = Nothing
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If ddlFondo.SelectedValue = "" Then
            AlertaJS("Seleccione un portafolio")
        Else
        If Not Session("SS_DatosModal") Is Nothing Then
            ddlFondo.SelectedValue = CType(Session("SS_DatosModal"), String())(3).ToString.Trim()
            txtCodigoOrdenH1.Value = CType(Session("SS_DatosModal"), String())(6).ToString.Trim()
            Session.Remove("SS_DatosModal")
        End If
        If Session("EstadoPantallaSW") = "Modificar" Or Session("EstadoPantallaSW") = "Eliminar" Or Session("EstadoPantallaSW") = "Consultar" Then
            If Session("Busqueda") = 0 Then
                txtCodigoOrden1.Text = ""
                txtCodigoOrden2.Text = ""
                Dim strAccion As String
                If Session("EstadoPantallaSW") = "Modificar" Then
                    strAccion = "M"
                ElseIf Session("EstadoPantallaSW") = "Eliminar" Then
                    strAccion = "E"
                ElseIf Session("EstadoPantallaSW") = "Consultar" Then
                    strAccion = "C"
                End If
                    ShowDialogPopupInversionesRealizadas(String.Empty, String.Empty, String.Empty, ddlFondo.SelectedValue, ddlFondo.SelectedItem.Text, ddlOperacion1.SelectedValue, "", "", strAccion)
                Session("Busqueda") = 2
            Else
                If Session("Busqueda") = 1 Then
                    CargarDatosOrdenInversion()
                    If Session("EstadoPantallaSW") = "Modificar" Then
                        CargarPaginaModificar()
                    ElseIf Session("EstadoPantallaSW") = "Eliminar" Then
                            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF3")
                            Else
                                UIUtility.AsignarMensajeBoton(btnAceptar, "CONF17")
                            End If
                        CargarPaginaEliminar()
                    ElseIf Session("EstadoPantallaSW") = "Consultar" Then
                        CargarPaginaConsultar()
                        End If

                        Session("Busqueda") = 2
                Else
                    Session("Busqueda") = 0
                End If
            End If
            End If
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../Bienvenida.aspx", False)
    End Sub
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        UIUtility.ExcluirOtroElementoSeleccion(ddlOperacion1)
        UIUtility.ExcluirOtroElementoSeleccion(ddlOperacion2)
        CargarPaginaAccion()
        Session("EstadoPantallaSW") = "Ingresar"
        Session("Busqueda") = 0
        lblAccion.Text = "Ingresar"
        CargarPaginaIngresar()
        hdMensaje.Value = "el Ingreso"
        If Not ddlFondo.Items.FindByValue(PORTAFOLIO_MULTIFONDOS) Is Nothing Then
            ddlFondo.SelectedValue = PORTAFOLIO_MULTIFONDOS
        End If
        lblTitulo.Text = "Orden de Inversión - SWAP DIVISAS"
        CargarOperacionOI(ddlOperacion2, ddlOperacion1.SelectedValue.ToString)
        Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(ddlFondo.SelectedValue, DatosRequest).Tables(0)
        If Not dtAux Is Nothing Then
            If dtAux.Rows.Count > 0 Then
                hdFechaOperacion.Value = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
            End If
        End If
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion1, "")
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion2, "")
        CargarPaginaAccion()
        lblAccion.Text = "Modificar"
        Session("EstadoPantallaSW") = "Modificar"
        Session("Busqueda") = 0
        hdMensaje.Value = "la Modificación"
        ddlOperacion1.Enabled = False
        ddlOperacion2.Enabled = False
        btnOper1.Enabled = False
        btnOper2.Enabled = False
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion1, "")
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion2, "")
        CargarPaginaAccion()
        Session("EstadoPantallaSW") = "Eliminar"
        lblAccion.Text = "Eliminar"
        Session("Busqueda") = 0
        hdMensaje.Value = "la Eliminación"
        ddlOperacion1.Enabled = False
        ddlOperacion2.Enabled = False
        btnOper1.Enabled = False
        btnOper2.Enabled = False
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion1, "")
        UIUtility.InsertarOtroElementoSeleccion(ddlOperacion2, "")
        CargarPaginaAccion()
        Session("EstadoPantallaSW") = "Consultar"
        lblAccion.Text = "Consultar"
        Session("Busqueda") = 0
        hdMensaje.Value = "la Consulta"
        ddlOperacion1.Enabled = False
        ddlOperacion2.Enabled = False
        btnOper1.Enabled = False
        btnOper2.Enabled = False
    End Sub
    Private Sub CargarDatosOrdenInversion()
        Try
            Dim dt As DataTable
            dt = oOrdenInversionBM.ListarOrdenesInversionSwap(txtCodigoOrdenH1.Value, DatosRequest)
            ddlFondo.SelectedValue = dt.Rows(0)("CodigoPortafolioSBS")
            hdFechaOperacion.Value = dt.Rows(0)("FechaOperacion")
            CargarOperacionOI(ddlOperacion1, "")
            CargarOperacionOI(ddlOperacion2, "")
            If dt.Rows(0)("PriorSimultaneo") = "1" Then
                ddlOperacion1.SelectedValue = dt.Rows(0)("CodigoOperacion")
                txtCodigoOrden1.Text = dt.Rows(0)("CodigoOrden")
                ddlOperacion2.SelectedValue = dt.Rows(1)("CodigoOperacion")
                txtCodigoOrden2.Text = dt.Rows(1)("CodigoOrden")
            End If
            If dt.Rows(1)("PriorSimultaneo") = "1" Then
                ddlOperacion1.SelectedValue = dt.Rows(1)("CodigoOperacion")
                txtCodigoOrden1.Text = dt.Rows(1)("CodigoOrden")
                ddlOperacion2.SelectedValue = dt.Rows(0)("CodigoOperacion")
                txtCodigoOrden2.Text = dt.Rows(0)("CodigoOrden")
            End If

        Catch ex As Exception
            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                AlertaJS(ObtenerMensaje("CONF31"))
            Else
                AlertaJS(ObtenerMensaje("CONF32"))
            End If
        End Try
    End Sub
    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If Session("EstadoPantallaSW") = "Ingresar" Then
            Dim dtAux As DataTable = oPortafolioBM.SeleccionarPortafolioPorFiltro(ddlFondo.SelectedValue, DatosRequest).Tables(0)
            If Not dtAux Is Nothing Then
                If dtAux.Rows.Count > 0 Then
                    hdFechaOperacion.Value = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaNegocio")))
                End If
            End If
        End If
    End Sub
    Private Sub btnOper1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnOper1.Click
        If ddlFondo.SelectedValue = "" Then
            AlertaJS("Seleccione el Portafolio ")
        Else
            strAccion = Session("EstadoPantallaSW").ToString.Substring(0, 1)
            Session("Oper1") = "1"
            hdOper1.Value = ddlOperacion1.SelectedValue
            If ddlOperacion1.SelectedValue = "93" Or ddlOperacion1.SelectedValue = "94" Then
                ShowDialogPopup("frmOpcionesDerivadasForwardDivisas.aspx?PTNegSim=" + strAccion + "&OI=1&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + txtCodigoOrden1.Text + "&Portafolio=" + ddlFondo.SelectedValue + "&Operacion=" + ddlOperacion1.SelectedValue, "")
            Else
                ShowDialogPopup("frmCompraVentaMonedaExtranjera.aspx?PTNegSim=" + strAccion + "&OI=1&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + txtCodigoOrden1.Text + "&Portafolio=" + ddlFondo.SelectedValue + "&Operacion=" + ddlOperacion1.SelectedValue, "")
            End If
            If Session("EstadoPantallaSW").ToString() <> "Consultar" Then
                If (Session("Oper1") = "1") Then
                    btnOper1.Enabled = False
                    ddlOperacion1.Enabled = False
                End If
                If (Session("Oper2") = "1") Then
                    btnOper2.Enabled = False
                    ddlOperacion2.Enabled = False
                End If
                If (Session("Oper1") = "1") And (Session("Oper2") = "1") Then
                    btnAceptar.Enabled = True
                End If
            End If
        End If
    End Sub
    Private Sub btnOper2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnOper2.Click
        If ddlFondo.SelectedValue = "" Then
            AlertaJS("Seleccione el Portafolio ")
        Else
            'If Session("OrdenInversionBE1") Is Nothing Then
            '    AlertaJS("Debe ingresar la primera Orden de Inversión.")
            'Else
            Dim dt1 As DataTable
            strAccion = Session("EstadoPantallaSW").ToString.Substring(0, 1)
            Session("Oper2") = "1"
            hdOper2.Value = ddlOperacion2.SelectedValue
            If strAccion = "I" Then
                If Session("OrdenInversionBE1") Is Nothing Then
                    AlertaJS("Debe ingresar la primera Orden de Inversión.")
                    Exit Sub
                Else
                    dt1 = CType(Session("OrdenInversionBE1"), DataTable)
                    Session("Intermediario") = dt1.Rows(0)("CodigoTercero")
                    Session("Contacto") = dt1.Rows(0)("CodigoContacto")
                End If
            End If
                If ddlOperacion2.SelectedValue = "93" Or ddlOperacion2.SelectedValue = "94" Then
                    If strAccion = "I" Then Session("MontoOperacion") = dt1.Rows(0)("MontoOrigen")
                    ShowDialogPopup("frmOpcionesDerivadasForwardDivisas.aspx?PTNegSim=" + strAccion + "&OI=2&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + txtCodigoOrden2.Text + "&Portafolio=" + ddlFondo.SelectedValue + "&Operacion=" + ddlOperacion2.SelectedValue, "")
                Else
                    If strAccion = "I" Then Session("MontoOperacion") = dt1.Rows(0)("MontoCancelar")
                    ShowDialogPopup("frmCompraVentaMonedaExtranjera.aspx?PTNegSim=" + strAccion + "&OI=2&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + txtCodigoOrden2.Text + "&Portafolio=" + ddlFondo.SelectedValue + "&Operacion=" + ddlOperacion2.SelectedValue, "")
                End If
                If Session("EstadoPantallaSW").ToString() <> "Consultar" Then
                    If (Session("Oper1") = "1") Then
                        btnOper1.Enabled = False
                        ddlOperacion1.Enabled = False
                    End If
                    If (Session("Oper2") = "1") Then
                        btnOper2.Enabled = False
                        ddlOperacion2.Enabled = False
                    End If
                    If (Session("Oper1") = "1") And (Session("Oper2") = "1") Then
                        btnAceptar.Enabled = True
                    End If
                End If
            End If
            '   End If
    End Sub
    Private Sub ddlOperacion1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperacion1.SelectedIndexChanged
        CargarOperacionOI(ddlOperacion2, ddlOperacion1.SelectedValue.ToString)
    End Sub
    Public Function ObtenerDatosOperacion(ByVal pOper As String, ByVal oOrdenInversionBE As OrdenPreOrdenInversionBE) As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21", "c22", "v22"}   'HDG OT 62325 20110323
        Dim dtOI As DataTable = oOrdenInversionBE.OrdenPreOrdenInversion
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        If pOper = "93" Or pOper = "94" Then
            drGrilla("c1") = "Fecha Operación"
            drGrilla("v1") = dtOI.Rows(0)("FechaOperacion")
            drGrilla("c2") = "Fecha Contrato"
            drGrilla("v2") = dtOI.Rows(0)("FechaContrato")
            drGrilla("c3") = "Hora Operación"
            drGrilla("v3") = dtOI.Rows(0)("HoraOperacion")
            drGrilla("c4") = "Fecha Liquidación"
            drGrilla("v4") = dtOI.Rows(0)("FechaLiquidacion")
            drGrilla("c5") = "Tipo Cambio Spot"
            drGrilla("v5") = dtOI.Rows(0)("TipoCambioSpot")
            drGrilla("c6") = "Tipo Cambio Futuro"
            drGrilla("v6") = dtOI.Rows(0)("TipoCambioFuturo")
            drGrilla("c7") = "De"
            drGrilla("v7") = oMonedaBM.SeleccionarPorFiltro(dtOI.Rows(0)("CodigoMoneda"), "", "", "", "", DatosRequest).Moneda.Rows(0)("Descripcion")
            drGrilla("c8") = "Monto Origen"
            drGrilla("v8") = dtOI.Rows(0)("MontoCancelar")
            drGrilla("c9") = "A"
            drGrilla("v9") = oMonedaBM.SeleccionarPorFiltro(dtOI.Rows(0)("CodigoMonedaDestino"), "", "", "", "", DatosRequest).Moneda.Rows(0)("Descripcion")
            drGrilla("c10") = "Monto Futuro"
            drGrilla("v10") = dtOI.Rows(0)("MontoOperacion")
            drGrilla("c11") = "Plazo"
            drGrilla("v11") = dtOI.Rows(0)("Plazo")
            drGrilla("c12") = "Diferencial"
            drGrilla("v12") = dtOI.Rows(0)("Diferencial")
            If dtOI.Rows(0)("Delibery") = "S" Then
                drGrilla("c13") = "Modalidad Compra"
                drGrilla("v13") = "Delivery"
            Else
                drGrilla("c13") = "Modalidad Compra"
                drGrilla("v13") = "Non-Delivery"
            End If
            drGrilla("c14") = "Intermediario"
            drGrilla("v14") = oTercerosBM.Seleccionar(dtOI.Rows(0)("CodigoTercero"), DatosRequest).Terceros.Rows(0)("Descripcion")
            If dtOI.Rows(0)("CodigoContacto").ToString.Trim <> "" Then
                drGrilla("c15") = "Contacto"
                drGrilla("v15") = oContactoBM.Seleccionar(dtOI.Rows(0)("CodigoContacto"), DatosRequest).Contacto.Rows(0)("Descripcion")
            Else
                drGrilla("c15") = ""
                drGrilla("v15") = ""
            End If
            drGrilla("c16") = "Observación"
            drGrilla("v16") = dtOI.Rows(0)("Observacion")
            If dtOI.Rows(0)("NumeroPoliza").ToString.Trim <> "" Then
                drGrilla("c17") = "Número Poliza"
                drGrilla("v17") = dtOI.Rows(0)("NumeroPoliza")
            Else
                drGrilla("c17") = ""
                drGrilla("v17") = ""
            End If
            drGrilla("c18") = "Motivo"
            drGrilla("v18") = oMotivoBM.Seleccionar(dtOI.Rows(0)("CodigoMotivo"), DatosRequest).Motivo.Rows(0)("Descripcion")
            drGrilla("c19") = ""
            drGrilla("v19") = ""
            drGrilla("c20") = ""
            drGrilla("v20") = ""
            drGrilla("c21") = ""
            drGrilla("v21") = ""
            drGrilla("c22") = "Cobertura"   'HDG OT 62325 20110323
            drGrilla("v22") = oOrdenInversionBM.SeleccionarTipoMonedaxMotivoForw(dtOI.Rows(0)("CodigoMotivo"), dtOI.Rows(0)("TipoMonedaForw")).Rows(0)("Descripcion")   'HDG OT 62325 20110323
        Else
            drGrilla("c1") = "Fecha Operacion"
            drGrilla("v1") = dtOI.Rows(0)("FechaOperacion")
            drGrilla("c2") = "Fecha Liquidacion"
            drGrilla("v2") = dtOI.Rows(0)("FechaLiquidacion")
            drGrilla("c3") = "Hora Operación"
            drGrilla("v3") = dtOI.Rows(0)("HoraOperacion")
            drGrilla("c4") = "De :"
            drGrilla("v4") = oMonedaBM.SeleccionarPorFiltro(dtOI.Rows(0)("CodigoMoneda"), "", "", "", "", DatosRequest).Moneda.Rows(0)("Descripcion")
            drGrilla("c5") = "Monto Divisa Negociada"
            drGrilla("v5") = dtOI.Rows(0)("MontoOperacion")
            drGrilla("c6") = "A :"
            drGrilla("v6") = oMonedaBM.SeleccionarPorFiltro(dtOI.Rows(0)("CodigoMonedaDestino"), "", "", "", "", DatosRequest).Moneda.Rows(0)("Descripcion")
            drGrilla("c7") = "Monto"
            drGrilla("v7") = dtOI.Rows(0)("MontoDestino")
            drGrilla("c8") = "Tipo Cambio"
            drGrilla("v8") = dtOI.Rows(0)("TipoCambio")
            drGrilla("c9") = "Intermediario"
            drGrilla("v9") = oTercerosBM.Seleccionar(dtOI.Rows(0)("CodigoTercero"), DatosRequest).Terceros.Rows(0)("Descripcion")
            If dtOI.Rows(0)("CodigoContacto").ToString.Trim <> "" Then
                drGrilla("c10") = "Contacto"
                drGrilla("v10") = oContactoBM.Seleccionar(dtOI.Rows(0)("CodigoContacto"), DatosRequest).Contacto.Rows(0)("Descripcion")
            Else
                drGrilla("c10") = ""
                drGrilla("v10") = ""
            End If
            If dtOI.Rows(0)("NumeroPoliza").ToString.Trim <> "" Then
                drGrilla("c11") = "Número Poliza"
                drGrilla("v11") = dtOI.Rows(0)("NumeroPoliza")
            Else
                drGrilla("c11") = ""
                drGrilla("v11") = ""
            End If
            drGrilla("c12") = "Observación"
            drGrilla("v12") = dtOI.Rows(0)("Observacion")
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
        End If
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Sub GenerarLlamado(ByVal strCodigoOI1 As String, ByVal strCodigoOI2 As String, ByVal clase1 As String, ByVal clase2 As String, ByVal moneda1 As String, ByVal moneda2 As String, ByVal mnemonico1 As String, ByVal mnemonico2 As String)
        Dim dir1 As String = "../Llamado/FrmVisorLlamado.aspx?vcodigo=" + strCodigoOI1 + "&cportafolio=" + ddlFondo.SelectedValue + "&vportafolio=" + ddlFondo.SelectedItem.Text + "&vclase=" + clase1 + "&voperacion=" + ddlOperacion1.SelectedItem.Text + "&vmoneda=" + moneda1 + "&vnemonico=" + mnemonico1
        Dim dir2 As String = "../Llamado/FrmVisorLlamado.aspx?vcodigo=" + strCodigoOI2 + "&cportafolio=" + ddlFondo.SelectedValue + "&vportafolio=" + ddlFondo.SelectedItem.Text + "&vclase=" + clase2 + "&voperacion=" + ddlOperacion2.SelectedItem.Text + "&vmoneda=" + moneda2 + "&vnemonico=" + mnemonico2
        Dim Cadena As String = "<script language='javascript'>" & _
        "window.open('" + dir1 + "','SW110','width=100,height=650,top=0,left=0,menubar=no,resizable=yes,status=yes,scrollbars=yes');" & _
        "window.open('" + dir2 + "','SW210','width=100,height=650,top=0,left=0,menubar=no,resizable=yes,status=yes,scrollbars=yes');" & _
        "</script>"
        EjecutarJS(Cadena, False)
    End Sub
    Public Sub InsertarOrdenInversion()
        Dim strCodigoOI1, strCodigoOI2 As String

        oOrdenInversionBE1 = crearObjetoOI(CType(Session("OrdenInversionBE1"), DataTable))
        strCodigoOI1 = oOrdenInversionBM.InsertarOI(oOrdenInversionBE1, "SW", "", DatosRequest)
        oOrdenInversionBE2 = crearObjetoOI(CType(Session("OrdenInversionBE2"), DataTable))
        strCodigoOI2 = oOrdenInversionBM.InsertarOI(oOrdenInversionBE2, "SW", "", DatosRequest)
        oOrdenInversionBM.OrdenSimultaneoSwapOI(strCodigoOI1, strCodigoOI2, "1", ddlFondo.SelectedValue)
        oOrdenInversionBM.OrdenSimultaneoSwapOI(strCodigoOI2, strCodigoOI1, "2", ddlFondo.SelectedValue)

        lblOrden1.Visible = True
        lblOrden2.Visible = True
        txtCodigoOrden1.Visible = True
        txtCodigoOrden2.Visible = True
        txtCodigoOrden1.Text = strCodigoOI1
        txtCodigoOrden2.Text = strCodigoOI2

        Session("dtdatosoperacionSW1") = ObtenerDatosOperacion(hdOper1.Value, oOrdenInversionBE1)
        Session("dtdatosoperacionSW2") = ObtenerDatosOperacion(hdOper2.Value, oOrdenInversionBE2)

        Dim s_Moneda As String = Convert.ToString(oOrdenInversionBE1.OrdenPreOrdenInversion.Rows(0)("CodigoMoneda")) 'IIf(hdOper1.Value = "93" Or hdOper1.Value = "94", oOrdenInversionBE1.OrdenPreOrdenInversion.Rows(0)("CodigoMoneda"), Session("CodigoMoneda"))
        Dim s_Moneda_2 As String = Convert.ToString(oOrdenInversionBE2.OrdenPreOrdenInversion.Rows(0)("CodigoMoneda"))
        GenerarLlamado(strCodigoOI1, strCodigoOI2, _
        IIf(hdOper1.Value = "93" Or hdOper1.Value = "94", "1OPERACIONES DERIVADAS - FORWARD DIVISAS", "1COMPRA/VENTA MONEDA EXTRANJERA"), _
        IIf(hdOper2.Value = "93" Or hdOper2.Value = "94", "2OPERACIONES DERIVADAS - FORWARD DIVISAS", "2COMPRA/VENTA MONEDA EXTRANJERA"), _
        s_Moneda, _
        s_Moneda_2, _
        IIf(hdOper1.Value = "93" Or hdOper1.Value = "94", oOrdenInversionBE1.OrdenPreOrdenInversion.Rows(0)("CodigoMnemonico"), ""),
        IIf(hdOper2.Value = "93" Or hdOper2.Value = "94", oOrdenInversionBE2.OrdenPreOrdenInversion.Rows(0)("CodigoMnemonico"), ""))
    End Sub

    Public Sub ModificarOrdenInversion()
        oOrdenInversionBE1 = crearObjetoOI(CType(Session("OrdenInversionBE1"), DataTable))
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE1, "", "", DatosRequest)
        oOrdenInversionBE2 = crearObjetoOI(CType(Session("OrdenInversionBE2"), DataTable))
        oOrdenInversionBM.ModificarOI(oOrdenInversionBE2, "", "", DatosRequest)
    End Sub

    Public Sub EliminarOrdenInversion()
        oOrdenInversionBE1 = crearObjetoOI(CType(Session("OrdenInversionBE1"), DataTable))
        oOrdenInversionBM.EliminarOI(txtCodigoOrden1.Text, ddlFondo.SelectedValue, oOrdenInversionBE1.OrdenPreOrdenInversion.Rows(0)("CodigoMotivoCambio"), DatosRequest)
        oOrdenInversionBE2 = crearObjetoOI(CType(Session("OrdenInversionBE2"), DataTable))
        oOrdenInversionBM.EliminarOI(txtCodigoOrden2.Text, ddlFondo.SelectedValue, oOrdenInversionBE2.OrdenPreOrdenInversion.Rows(0)("CodigoMotivoCambio"), DatosRequest)

        If ddlOperacion1.SelectedValue = "93" Or ddlOperacion1.SelectedValue = "94" Then
            oImpComOP.Eliminar(txtCodigoOrden1.Text, ddlFondo.SelectedValue, DatosRequest)
        End If
        If ddlOperacion2.SelectedValue = "93" Or ddlOperacion2.SelectedValue = "94" Then
            oImpComOP.Eliminar(txtCodigoOrden2.Text, ddlFondo.SelectedValue, DatosRequest)
        End If
    End Sub

    Public Sub FechaEliminarModificarOI(ByVal tProc As String)
        oOrdenInversionBM.FechaModificarEliminarOI(ddlFondo.SelectedValue, txtCodigoOrden1.Text, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, Session("Comentarios1"), DatosRequest)
        oOrdenInversionBM.FechaModificarEliminarOI(ddlFondo.SelectedValue, txtCodigoOrden2.Text, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), tProc, Session("Comentarios2"), DatosRequest)
    End Sub

    Public Sub CargarOperacionOI(ByVal drlista As DropDownList, ByVal codoper As String)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoOperacion"
        drlista.DataSource = New OperacionBM().SeleccionarOperacionesSwap(codoper)
        drlista.DataBind()
    End Sub

    Public Function crearObjetoOI(ByVal dtOI As DataTable) As OrdenPreOrdenInversionBE
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        For Each dr As DataRow In dtOI.Rows
            oRow = CType(oOrdenInversionBE.OrdenPreOrdenInversion.NewRow(), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
            oOrdenInversionBM.InicializarOrdenInversion(oRow)
            If ddlFondo.SelectedValue <> PORTAFOLIO_MULTIFONDOS Then
                oRow.CodigoOrden = IIf(dr("CodigoOrden") Is DBNull.Value, "", dr("CodigoOrden"))
            Else
                oRow.CodigoOrden = IIf(dr("CodigoPreOrden") Is DBNull.Value, "", dr("CodigoPreOrden"))
            End If
            oRow.CodigoPortafolioSBS = IIf(dr("CodigoPortafolioSBS") Is DBNull.Value, "", dr("CodigoPortafolioSBS"))
            oRow.CodigoOperacion = IIf(dr("CodigoOperacion") Is DBNull.Value, "", dr("CodigoOperacion"))
            oRow.CodigoMoneda = IIf(dr("CodigoMoneda") Is DBNull.Value, "", dr("CodigoMoneda"))
            oRow.HoraOperacion = IIf(dr("HoraOperacion") Is DBNull.Value, "", dr("HoraOperacion"))
            oRow.MontoCancelar = IIf(dr("MontoCancelar") Is DBNull.Value, 0, dr("MontoCancelar"))
            oRow.CodigoTercero = IIf(dr("CodigoTercero") Is DBNull.Value, "", dr("CodigoTercero"))
            oRow.CodigoContacto = IIf(dr("CodigoContacto") Is DBNull.Value, "", dr("CodigoContacto"))
            oRow.FechaOperacion = IIf(dr("FechaOperacion") Is DBNull.Value, 0, dr("FechaOperacion"))
            oRow.FechaLiquidacion = IIf(dr("FechaLiquidacion") Is DBNull.Value, 0, dr("FechaLiquidacion"))
            oRow.FechaContrato = IIf(dr("FechaContrato") Is DBNull.Value, 0, dr("FechaContrato"))
            oRow.TipoCambioFuturo = IIf(dr("TipoCambioFuturo") Is DBNull.Value, 0, dr("TipoCambioFuturo"))
            oRow.Plazo = IIf(dr("Plazo") Is DBNull.Value, 0, dr("Plazo"))
            oRow.Situacion = "A"
            oRow.MontoPlazo = IIf(dr("MontoPlazo") Is DBNull.Value, 0, dr("MontoPlazo"))
            oRow.CodigoMonedaOrigen = IIf(dr("CodigoMonedaOrigen") Is DBNull.Value, "", dr("CodigoMonedaOrigen"))
            oRow.CodigoMonedaDestino = IIf(dr("CodigoMonedaDestino") Is DBNull.Value, "", dr("CodigoMonedaDestino"))
            oRow.Delibery = IIf(dr("Delibery") Is DBNull.Value, "", dr("Delibery"))
            oRow.MontoOperacion = IIf(dr("MontoOperacion") Is DBNull.Value, 0, dr("MontoOperacion"))
            oRow.Observacion = IIf(dr("Observacion") Is DBNull.Value, "", dr("Observacion"))
            oRow.TipoCambioSpot = IIf(dr("TipoCambioSpot") Is DBNull.Value, 0, dr("TipoCambioSpot"))
            oRow.Diferencial = IIf(dr("Diferencial") Is DBNull.Value, 0, dr("Diferencial"))
            oRow.CodigoMotivo = IIf(dr("CodigoMotivo") Is DBNull.Value, "", dr("CodigoMotivo"))
            oRow.CategoriaInstrumento = IIf(dr("CategoriaInstrumento") Is DBNull.Value, "", dr("CategoriaInstrumento"))
            oRow.NumeroPoliza = IIf(dr("NumeroPoliza") Is DBNull.Value, "", dr("NumeroPoliza"))
            If Session("EstadoPantallaSW") = "Modificar" Or Session("EstadoPantallaSW") = "Eliminar" Then
                oRow.CodigoMotivoCambio = IIf(dr("CodigoMotivoCambio") Is DBNull.Value, "", dr("CodigoMotivoCambio"))
                If Session("EstadoPantallaSW") = "Modificar" Then
                    oRow.IndicaCambio = "1"
                End If
            End If
            oRow.Ficticia = IIf(dr("Ficticia") Is DBNull.Value, "", dr("Ficticia"))
            oRow.Renovacion = IIf(dr("Renovacion") Is DBNull.Value, "", dr("Renovacion"))
            oRow.TipoMonedaForw = IIf(dr("TipoMonedaForw") Is DBNull.Value, "", dr("TipoMonedaForw")) 'HDG OT 61573 20101125

            oRow.TipoCambio = IIf(dr("TipoCambio") Is DBNull.Value, 0, dr("TipoCambio"))
            oRow.MontoOrigen = IIf(dr("MontoOrigen") Is DBNull.Value, 0, dr("MontoOrigen"))
            oRow.MontoDestino = IIf(dr("MontoDestino") Is DBNull.Value, 0, dr("MontoDestino"))
            oRow.MontoNetoOperacion = IIf(dr("MontoNetoOperacion") Is DBNull.Value, 0, dr("MontoNetoOperacion"))
            oRow.AfectaFlujoCaja = IIf(dr("AfectaFlujoCaja") Is DBNull.Value, "", dr("AfectaFlujoCaja"))
            oRow.CodigoMnemonico = IIf(dr("CodigoMnemonico") Is DBNull.Value, "", dr("CodigoMnemonico"))

            oOrdenInversionBE.OrdenPreOrdenInversion.AddOrdenPreOrdenInversionRow(oRow)
            oOrdenInversionBE.OrdenPreOrdenInversion.AcceptChanges()
        Next
        Return oOrdenInversionBE
    End Function
    Private Sub ShowDialogPopup(ByVal StrURL As String, ByVal imgButton As String, Optional ByVal width As String = "1000", Optional ByVal heigth As String = "600", Optional ByVal left As String = "125")
        If imgButton = "" Then           
            EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', ''); ")
        Else
            EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', '" & imgButton & "'); ")
        End If
    End Sub

    Private Sub Mensaje(ByVal Cadena As String)
        AlertaJS(Cadena)
    End Sub

#Region " /* Métodos Personalizados (Popups Dialogs) */ "

    Private Sub ShowDialogPopupInversionesRealizadas(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, ByVal cfondo As String, ByVal fondo As String, ByVal operacion As String, ByVal moneda As String, ByVal fecha As String, ByVal accion As String)
        EjecutarJS("showModalDialog('../frmInversionesRealizadas.aspx?vISIN=" + isin + "&vSBS=" + sbs + "&vMnemonico=" + mnemonico + "&cFondo=" + cfondo + "&vFondo=" + fondo + "&vOperacion=" + operacion + "&vFechaOperacion=" + fecha + "&vAccion=" + accion + "&vCategoria=SW', '950','700','btnBuscar');")
    End Sub

#End Region

#Region " /* Métodos Controla Habilitar/Deshabilitar Campos */ "

    Private Sub CargarPaginaInicio()
        HabilitaDeshabilitaCabecera(False)
        OcultarBotonesInicio()
        btnAceptar.Enabled = False
    End Sub

    Private Sub CargarPaginaIngresar()
        btnBuscar.Visible = False
        ddlOperacion2.Enabled = False
    End Sub

    Private Sub CargarPaginaModificar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnOper1.Enabled = True
        btnOper2.Enabled = True
        txtCodigoOrden1.Visible = True
        txtCodigoOrden2.Visible = True
        lblOrden1.Visible = True
        lblOrden2.Visible = True
    End Sub

    Private Sub CargarPaginaEliminar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnOper1.Enabled = True
        btnOper2.Enabled = True
        txtCodigoOrden1.Visible = True
        txtCodigoOrden2.Visible = True
        lblOrden1.Visible = True
        lblOrden2.Visible = True
    End Sub

    Private Sub CargarPaginaConsultar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnOper1.Enabled = True
        btnOper2.Enabled = True
        txtCodigoOrden1.Visible = True
        txtCodigoOrden2.Visible = True
        lblOrden1.Visible = True
        lblOrden2.Visible = True
    End Sub

    Private Sub CargarPaginaProcesar()
        EjecutarJS("$('#btnAceptar').removeAttr('disabled');")
    End Sub

    Private Sub CargarPaginaAceptar()
        HabilitaDeshabilitaCabecera(False)
        btnBuscar.Visible = False
        btnAceptar.Enabled = False
    End Sub

    Private Sub CargarPaginaAccion()
        CargarPaginaInicio()
        LimpiarDatosOperacion()
        HabilitaDeshabilitaCabecera(True)
        LimpiarSesiones()
        btnBuscar.Visible = True
        btnBuscar.Enabled = True
        txtCodigoOrden1.Visible = False
        txtCodigoOrden2.Visible = False
        lblOrden1.Visible = False
        lblOrden2.Visible = False
    End Sub

    Private Sub HabilitaDeshabilitaCabecera(ByVal estado As Boolean)
        ddlFondo.Enabled = estado
        ddlOperacion1.Enabled = estado
        ddlOperacion2.Enabled = estado
        btnBuscar.Enabled = estado
        btnOper1.Enabled = estado
        btnOper2.Enabled = estado
    End Sub

    Private Sub OcultarBotonesInicio()
        btnBuscar.Visible = False
    End Sub

    Private Sub MostrarOcultarBotonesAcciones(ByVal estado As Boolean)
        btnIngresar.Visible = estado
        btnModificar.Visible = estado
        btnEliminar.Visible = estado
        btnConsultar.Visible = estado
    End Sub

    Private Sub LimpiarDatosOperacion()
        ddlFondo.SelectedIndex = 0
        ddlOperacion1.SelectedIndex = 0
        ddlOperacion1.SelectedIndex = 0
        txtCodigoOrden1.Text = ""
        txtCodigoOrden2.Text = ""
        hdFechaOperacion.Value = ""
        txtCodigoOrdenH1.Value = ""
        txtCodigoOrdenH2.Value = ""
    End Sub
#End Region
    Protected Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        If Session("EstadoPantallaSW") = "Ingresar" Then
            'insertar las ordenes de inversion
            InsertarOrdenInversion()
            CargarPaginaAceptar()
            Mensaje("Las ordenes de inversión se ingresaron satisfactoriamente.")
        Else
            If Session("EstadoPantallaSW") = "Modificar" Then
                ModificarOrdenInversion()
                FechaEliminarModificarOI("M")
                Session("Modificar") = 0
                CargarPaginaAceptar()
                Mensaje("Las ordenes de inversión se modificaron satisfactoriamente.")
            ElseIf Session("EstadoPantallaSW") = "Eliminar" Then
                EliminarOrdenInversion()
                FechaEliminarModificarOI("E")
                CargarPaginaAceptar()
                CargarPaginaAccion()
                HabilitaDeshabilitaCabecera(False)
                lblAccion.Text = ""
                Mensaje("Las ordenes de inversión se eliminaron satisfactoriamente.")
            End If
        End If
        ddlOperacion1.SelectedValue = hdOper1.Value
        ddlOperacion2.SelectedValue = hdOper2.Value
    End Sub
End Class
