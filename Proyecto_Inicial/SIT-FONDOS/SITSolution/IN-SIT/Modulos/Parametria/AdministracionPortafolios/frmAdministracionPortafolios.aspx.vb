Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT
Imports Constantes
Partial Class Modulos_Parametria_AdministracionPortafolios_frmAdministracionPortafolios
    Inherits BasePage
    Private Const VWS_FECHACONTABLE As String = "Fecha Contable"
#Region " /* Metodos de Página */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Dim oPortafolioBM As New PortafolioBM
                'tbCodigoPortafolioSBS.Text = oPortafolioBM.PortafolioCodAuto()
                CargarFiltros()

                If Not Request.QueryString("codigo") Is Nothing Then
                    Me.hdnCodigo.Value = Request.QueryString("codigo")
                    Me.CargarControles()
                Else
                    Me.hdnCodigo.Value = String.Empty
                    If Me.chkFondoCliente.Checked = True Then
                        Me.divFondoCliente.Attributes.Add("style", "display:block")
                    Else
                        Me.divFondoCliente.Attributes.Add("style", "display:none")
                    End If
                End If
            End If

            If Not Session("SS_DatosModal") Is Nothing Then
                ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24 | Agregado del campo CLIENTE
                If hdModal.Value = "_MODAL_TERCERO_" Then
                    txtCodCliente.Value = CType(Session("SS_DatosModal"), String())(0).ToString()
                    txtDescCliente.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                    Session("SS_DatosModal") = Nothing
                End If
                ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24 | Agregado del campo CLIENTE
            End If
           
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página.")
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaPortafolio.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar.")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar.")
        End Try
    End Sub
    Protected Sub ddlMultifondo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMultifondo.SelectedIndexChanged
        tblDetalleGrpFondo.Visible = IIf(ddlMultifondo.SelectedValue = M_STR_CONDICIONAL_NO, False, True)
    End Sub
    Protected Sub btAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btAgregar.Click
        Dim dtDetalle As New Data.DataTable
        Dim fila As Data.DataRow

        If ddlPortafolio.SelectedValue = String.Empty Then
            AlertaJS("Seleccione un portafolio.")
            Exit Sub
        End If
        If ValidarDetalle(ddlPortafolio.SelectedValue) Then
            AlertaJS("El portafolio ya ha sido agregado.")
            Exit Sub
        End If
        dtDetalle = CType(ViewState("TablaDetalleGrupoPortafolio"), Data.DataTable)
        fila = dtDetalle.NewRow()
        fila("CodigoPortafolioSBS") = ddlPortafolio.SelectedValue
        fila("Descripcion") = ddlPortafolio.SelectedItem.Text
        dtDetalle.Rows.Add(fila)
        ViewState("TablaDetalleGrupoPortafolio") = dtDetalle
        dgLista.DataSource = dtDetalle
        dgLista.DataBind()
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim dtDetalle As New Data.DataTable
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                dtDetalle = CType(ViewState("TablaDetalleGrupoPortafolio"), Data.DataTable)
                dtDetalle.Rows(gvr.RowIndex).Delete()
                dtDetalle.AcceptChanges()
                ViewState("TablaDetalleGrupoPortafolio") = dtDetalle
                dgLista.DataSource = dtDetalle
                dgLista.DataBind()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla.")
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibnEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('Desea eliminar registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla.")
        End Try
    End Sub

    Protected Sub chkFondoCliente_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFondoCliente.CheckedChanged
        If chkFondoCliente.Checked Then
            divFondoCliente.Attributes.Add("style", "display:block")
        Else
            divFondoCliente.Attributes.Add("style", "display:none")
        End If
    End Sub

#End Region
#Region " /* Funciones Personalizadas */"
    Private Sub CargarFiltros()
        Dim dtNegocio As DataTable
        Dim dtParametrosGenarales As DataTable
        Dim DtPortafolio As DataTable
        Dim DtValoresSerie As DataTable
        Dim oNegocioBM As New NegocioBM
        Dim oTipoValorizacionBM As New TipoValorizacionBM
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oCustodio As New CustodioBM
        Dim oPortafolio As New PortafolioBM
        Dim codigoPortafolio As String = ""

        HelpCombo.LlenarComboBox(ddlcustodio, oCustodio.Listar(DatosRequest).Tables(0), "CodigoCustodio", "Descripcion", True)
        dtParametrosGenarales = oParametrosGeneralesBM.ListarSituacion(Me.DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, dtParametrosGenarales, "Valor", "Nombre", False)
        dtNegocio = oNegocioBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlNegocio, dtNegocio, "CodigoNegocio", "Descripcion", True)

        Dim tablaMoneda As Data.DataTable
        Dim oMoneda As New MonedaBM
        tablaMoneda = oMoneda.Listar("A").Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMoneda, tablaMoneda, "CodigoMoneda", "CodigoMoneda", True)
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(TIPOFONDO, Me.DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlMultifondo, dtParametrosGenarales, "Valor", "Nombre", False)
        ddlMultifondo.SelectedValue = M_STR_CONDICIONAL_NO
        tblDetalleGrpFondo.Visible = False
        DtPortafolio = oPortafolio.PortafolioCodigoListar(codigoPortafolio)
        HelpCombo.LlenarComboBox(ddlPortafolio, DtPortafolio, "CodigoPortafolio", "Descripcion", True)
        DtValoresSerie = oPortafolio.PortafolioCodigoListar_ValoresSerie("0")
        dgGrillaVCouta.DataSource = DtValoresSerie
        dgGrillaVCouta.DataBind()
        Dim dtindicador = oParametrosGeneralesBM.SeleccionarPorFiltro("Desicion", "", "", "", MyBase.DatosRequest())
        HelpCombo.LlenarComboBox(ddlIndicador, dtindicador, "Valor", "Nombre", True)
        tbCodigoFondo.Enabled = False
        FilaSeries.Visible = False

        ''''' INI MPENAL - 09/09/2016
        Dim oTipoRentaBM As New TipoRentaBM
        Dim DtTablaTipoRenta As DataTable = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoRenta, DtTablaTipoRenta, "CodigoRenta", "Descripcion", True)
        ''''' FIN MPENAL - 09/09/2016
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(FONDOFIR, Me.DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoCalculoValorCuota, dtParametrosGenarales, "Valor", "Nombre", True)
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(VALORACION_MENSUAL, DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoValorizacion, dtParametrosGenarales, "Valor", "Nombre", True)

        ' INICIO --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(TIPO_NEGOCIO, Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlTipoNegocio, dtParametrosGenarales, "Valor", "Nombre", True)

        ' INICIO --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(TIPO_COMISION, Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlTipoComision, dtParametrosGenarales, "Valor", "Nombre", True)

        'dtParametrosGenarales = oParametrosGeneralesBM.Listar(ORDEN_VECTOR, Me.DatosRequest)
        'HelpCombo.LlenarComboBox(Me.ddlOrdenVectorPrecio, dtParametrosGenarales, "Valor", "Nombre", True)

        ' FIN --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
    End Sub
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            Me.CargarFiltros()
            If Not Request.QueryString("codigo") Is Nothing Then
                Me.hdnCodigo.Value = Request.QueryString("codigo")
                Me.CargarControles()
            Else
                Me.hdnCodigo.Value = String.Empty
            End If
        End If
    End Sub
    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        If Me.hdnCodigo.Value.Equals(String.Empty) Then
            blnExisteEntidad = Me.ExisteEntidad()
            Dim ddlValue As String = ddlTipoRenta.SelectedValue
            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            ElseIf String.IsNullOrEmpty(ddlValue) Then
                AlertaJS("Debe seleccionar un Tipo de Renta")
            Else
                Insertar()
            End If
        Else
            Modificar()
        End If
    End Sub
    'OT 10883 20/10/2017 Hanz Cocchi
    'Se agrega código para eliminar en la BD de forma correcta los registros eliminados en memoria en la página 
    Sub CustodioCuentaDep()
        Dim oCustodio As New CustodioBM
        Dim dt As DataTable = ViewState("Custodio")
        If Not dt Is Nothing Then
            For Each dr As DataRow In dt.Rows
                'HCOCCHI 20171020: Los registros eliminados no se tratan en esta sección.
                If dr.RowState <> DataRowState.Deleted Then
                    oCustodio.Insertar_Cuentadepositaria_Portafolio(tbCodigoPortafolioSBS.Text, dr(0), dr(2), DatosRequest)
                End If
            Next

            'HCOCCHI 20171020: En esta sección se eliminan de la BD aquellos registros que fueron eliminados en la grilla
            Dim dtv = New DataView(dt, String.Empty, String.Empty, DataRowState.Deleted)
            For index = 0 To dtv.Count - 1
                'ELIMINAR LA FILA EN LA BD
                oCustodio.Del_Cuentadepositaria_Portafolio(tbCodigoPortafolioSBS.Text, dtv(index)(0), dtv(index)(2))
            Next
        End If
    End Sub
    'OT 10883 20/10/2017 FIN
    Private Function ValidarMontoSeries() As Boolean
        Dim Total As Decimal = 0
        For Each filagrilla As GridViewRow In dgGrillaVCouta.Rows
            Total = Total + CType(filagrilla.FindControl("tbPorcentaje"), TextBox).Text
        Next
        If Total = 100 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub InsertarSeries(ByVal CodigoPortafolioSBS As String)
        Dim oPortafolioBM As New PortafolioBM
        Dim dtSeries As New DataTable
        dtSeries = RecuperarGrillaSeries()
        'dtSeriesEliminar = Session("EliminarSeries")
        'If Not dtSeriesEliminar Is Nothing Then
        '    oPortafolioBM.InsertarSeries(CodigoPortafolioSBS, dtSeriesEliminar)
        'End If

        oPortafolioBM.InsertarSeries(CodigoPortafolioSBS, dtSeries)
    End Sub
    Private Sub Insertar()
        Dim oPortafolioBE As PortafolioBE
        Dim oPortafolioBM As New PortafolioBM
        oPortafolioBE = Me.ObtenerInstancia()
        oPortafolioBM.Insertar(oPortafolioBE, Me.DatosRequest)
        If chkSerie.Checked Then
            InsertarSeries(CType(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow).CodigoPortafolioSBS)
        End If
        InsertarDetalle()
        CustodioCuentaDep()
        Me.AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        Me.LimpiarControles()
        'tbCodigoPortafolioSBS.Text = oPortafolioBM.PortafolioCodAuto()
    End Sub
    Private Sub Modificar()
        If (ValidarDatos()) Then
            Dim ddlTipoRentaValor As String = ddlTipoRenta.SelectedValue    ' MPENAL - 13/09/16
            If String.IsNullOrEmpty(ddlTipoRentaValor) Then                 ' MPENAL - 13/09/16
                Me.AlertaJS("Debe seleccionar un Tipo de Renta")            ' MPENAL - 13/09/16
            Else                                                            ' MPENAL - 13/09/16
                Dim oPortafolioBE As PortafolioBE
                Dim oPortafolioBM As New PortafolioBM
                oPortafolioBE = Me.ObtenerInstancia()
                oPortafolioBM.Modificar(oPortafolioBE, Me.DatosRequest)
                If chkSerie.Checked Then
                    InsertarSeries(CType(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow).CodigoPortafolioSBS)
                End If
                InsertarDetalle()
                CustodioCuentaDep()
                Me.AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
            End If
        Else
            Me.AlertaJS("La valor de la Fecha Contable no es valida.")
        End If
    End Sub
    Private Sub InsertarDetalle()
        Dim dtDetalle As Data.DataTable
        Dim oPortafolioBM As New PortafolioBM
        Dim intFila As Integer

        If ddlMultifondo.SelectedValue = M_STR_CONDICIONAL_SI Then
            oPortafolioBM.EliminarDetalleGrupoPortafolio(tbCodigoPortafolioSBS.Text)
            dtDetalle = CType(ViewState("TablaDetalleGrupoPortafolio"), Data.DataTable)

            For intFila = 0 To dtDetalle.Rows.Count - 1
                oPortafolioBM.InsertarDetalleGrupoPortafolio(tbCodigoPortafolioSBS.Text, dtDetalle.Rows(intFila)("CodigoPortafolioSBS").ToString())
            Next
        End If

        'Guardar ORDEN de aplicacion del VECTOR PRECIO Y DEL TIPO CAMBIO
        oPortafolioBM.EliminarSecuenciaVector(tbCodigoPortafolioSBS.Text, "TC")

        Dim dt As DataTable = GenerarItemsSecuenciaVectorDesdeString(tbCodigoPortafolioSBS.Text, "TC", Me.hdnOrdenVectorTC.Value)
        For Each row In dt.Rows
            oPortafolioBM.InsertarSecuenciaVector(row("CodigoPortafolioSBS"), row("TipoVector"), row("FuenteVector"), row("Secuencia"))
        Next

        oPortafolioBM.EliminarSecuenciaVector(tbCodigoPortafolioSBS.Text, "PRECIO")
        dt = GenerarItemsSecuenciaVectorDesdeString(tbCodigoPortafolioSBS.Text, "PRECIO", Me.hdnOrdenVectorPrecio.Value)
        For Each row In dt.Rows
            oPortafolioBM.InsertarSecuenciaVector(row("CodigoPortafolioSBS"), row("TipoVector"), row("FuenteVector"), row("Secuencia"))
        Next

    End Sub

    '-- INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
    Function GenerarStringDesdeItemsSecuenciaVector(ByVal dtItems As DataTable) As String
        Dim str As String = ""
        For Each row As DataRow In dtItems.Rows
            If str.Length > 0 Then str = str & ","
            str = str & row("FuenteVector")
        Next
        Return str
    End Function
    Function GenerarItemsSecuenciaVectorDesdeString(ByVal codigoPortafoilio As String, ByVal tipoVetor As String, ByVal vectoresEnSecuencia As String) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("CodigoPortafolioSBS")
        dt.Columns.Add("TipoVector")
        dt.Columns.Add("FuenteVector") 'SBS, PIP, MANUAL
        dt.Columns.Add("Secuencia")

        Dim codVectores As String() = Split(vectoresEnSecuencia, ",")
        Dim sec As Integer = 0

        For Each codVec As String In codVectores
            If codVec.Trim().Length > 0 Then
                sec = sec + 1
                dt.Rows.Add(New String() {codigoPortafoilio, tipoVetor, codVec.Trim(), sec})
            End If
        Next

        Return dt
    End Function
    '-- FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio

    Private Function ValidarDatos() As Boolean
        Dim FechaContable As Decimal = UIUtility.ConvertirFechaaDecimal(Me.tbFechaContable.Text)
        Dim FechaContableValida As Boolean = True
        Dim FechaContableDATE As Date = CDate(Me.tbFechaContable.Text)
        If (FechaContableDATE.DayOfWeek = DayOfWeek.Saturday Or FechaContableDATE.DayOfWeek = DayOfWeek.Sunday Or FechaContableDATE.ToString("MMdd").Equals("1225") Or FechaContableDATE.ToString("MMdd").Equals("0101")) Then
            FechaContableValida = False
        End If
        Return FechaContableValida
    End Function
    Private Sub CargarControles()
        Dim oPortafolioBE As PortafolioBE
        Dim oPortafolioBM As New PortafolioBM
        Dim oRow As PortafolioBE.PortafolioRow
        Dim oCustodio As New CustodioBM
        oPortafolioBE = oPortafolioBM.Seleccionar(Request.QueryString("codigo"), Me.DatosRequest)
        Me.tbCodigoPortafolioSBS.Enabled = False

        oRow = DirectCast(oPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
        Me.hdnCodigo.Value = oRow.CodigoPortafolioSBS
        Me.tbCodigoPortafolioSBS.Text = oRow.CodigoPortafolioSBS
        Me.tbFechaConstitucion.Text = UIUtility.ConvertirFechaaString(oRow.FechaConstitucion)
        Me.txtDescripcion.Text = oRow.Descripcion
        Me.ddlNegocio.SelectedValue = oRow.CodigoNegocio
        Me.ddlSituacion.SelectedValue = oRow.Situacion
        Me.ddlMultifondo.SelectedValue = oRow.EsMultiPortafolio
        Me.ddlMultifondo.Enabled = False
        '''''' INI MPENAL - 09/09/16
        ddlTipoRenta.SelectedValue = oRow.CodigoRenta
        '''''' FIN MPENAL - 09/09/16

        If oRow.IsFechaAperturaContableNull Then
            Me.tbFechaContable.Text = String.Empty
            ViewState(VWS_FECHACONTABLE) = String.Empty
        Else
            Me.tbFechaContable.Text = UIUtility.ConvertirFechaaString(oRow.FechaAperturaContable)
            ViewState(VWS_FECHACONTABLE) = oRow.FechaAperturaContable
        End If
        If oRow.IsCodigoMonedaNull() Then
            Me.ddlMoneda.SelectedIndex = 0
        Else
            Me.ddlMoneda.SelectedValue = oRow.CodigoMoneda
        End If
        If oRow.EsMultiPortafolio = M_STR_CONDICIONAL_SI Then
            tblDetalleGrpFondo.Visible = True
            CargarDetalleGrupoPortafolio(oRow.CodigoPortafolioSBS)
        Else
            tblDetalleGrpFondo.Visible = False
        End If
        Dim oPortafolio As New PortafolioBM
        Dim DtValoresSerie As DataTable = oPortafolio.PortafolioCodigoListar_ValoresSerie(oRow.CodigoPortafolioSBS)
        dgGrillaVCouta.DataSource = DtValoresSerie
        dgGrillaVCouta.DataBind()
        If oRow.InterfazContable.Equals("S") Then
            chkIContable.Checked = True
            tbCodigoFondo.Text = oRow.CodContabilidadFondo
            tbCodigoFondo.Enabled = True
        End If
        If oRow.PorSerie.Equals("S") Then
            chkSerie.Checked = True
            FilaSeries.Visible = True
        End If
        If chkSerie.Checked Then
            PNComisionVariable.Visible = False
        End If
        ddlIndicador.SelectedValue = oRow.IndicadorFondo
        tbCodFondoMutuo.Text = oRow.CodigoFondosMutuos
        tbValorINIFondo.Text = oRow.ValorInicialFondo
        tbNroCuotaPreCierre.Text = oRow.NumeroCuotaPreCierre
        tbNombreCompleto.Text = oRow.NombreCompleto
        chkComisionVariable.Checked = CType(oRow.FlagComisionVariable, Boolean)
        txtMontoSuscripcionInicial.Text = oRow.MontoSuscripcionInicial
        txtTopeValorCuota.Text = oRow.TopeValorCuota
        chkComisionSusIni.Checked = CType(oRow.FlagComisionSuscripInicial, Boolean)
        chkCuotasLiberadas.Checked = CType(oRow.CuotasLiberadas, Boolean)
        txtBaseContable.Text = oRow.BDConta
        txtCodSO.Text = oRow.CodigoPortafolioSisOpe
        txtRuc.Text = oRow.RUCPortafolio
        If Not oRow.IsTipoCalculoValorCuotaNull Then ddlTipoCalculoValorCuota.SelectedValue = oRow.TipoCalculoValorCuota
        If Not oRow.IsValoracionMensualNull Then ddlTipoValorizacion.SelectedValue = oRow.ValoracionMensual
        ViewState("Custodio") = oCustodio.Cuentadepositaria_Portafolio(tbCodigoPortafolioSBS.Text)
        ' INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
        chkAumentoCapital.Checked = CType(oRow.FlagAumentoCapital, Boolean)
        ' FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
        txtCPPadreSO.Text = oRow.CPPadreSisOpe

        ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24
        txtCodCliente.Value = oRow.CodigoTerceroCliente
        txtDescCliente.Text = oRow.DescTerceroCliente
        ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24

        'ddlOrdenVectorPrecio.SelectedValue = oRow.VectorPrecioVal 'No aplicaria pues el combo se llena y selecciona de otra manera (CRumiche)
        Me.ddlTipoNegocio.SelectedValue = oRow.TipoNegocio
        'INICIO | rcolonia | Zoluxiones | Cuando el tipo de negocio es Mandatos se bloquea el indicadorVL y se setea "No" por defecto | 31-01-2019    
        ddlIndicador.Enabled = (ddlTipoNegocio.SelectedValue.ToUpper.Trim <> "MANDA")
        'FIN | rcolonia | Zoluxiones | Cuando el tipo de negocio es Mandatos se bloquea el indicadorVL y se setea "No" por defecto | 31-01-2019    
        If oRow.FondoCliente = "S" Then
            Me.chkFondoCliente.Checked = True
            Me.divFondoCliente.Attributes.Add("style", "display:block")
        Else
            Me.chkFondoCliente.Checked = False
            Me.divFondoCliente.Attributes.Add("style", "display:none")
        End If

        If oRow.Consolidado = 1 Then
            Me.chkConsolidado.Checked = True
        Else
            Me.chkConsolidado.Checked = False
        End If
        'Me.chkFondoCliente.Checked = IIf(oRow.FondoCliente = "S", True, False)
        ' INICIO --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
        'Llenando los SELECTED VALUES
        Dim dt As DataTable = oPortafolio.ListarSecuenciaVector(tbCodigoPortafolioSBS.Text, "TC")
        Me.hdnOrdenVectorTC.Value = GenerarStringDesdeItemsSecuenciaVector(dt)

        dt = oPortafolio.ListarSecuenciaVector(tbCodigoPortafolioSBS.Text, "PRECIO")
        Me.hdnOrdenVectorPrecio.Value = GenerarStringDesdeItemsSecuenciaVector(dt)
        ' FIN --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
        Me.ddlTipoComision.SelectedValue = oRow.TipoComision
        GVcustodio.DataSource = ViewState("Custodio")
        GVcustodio.DataBind()
        CargarGrillaPorcentajeComision()
    End Sub
    Public Sub CargarDetalleGrupoPortafolio(ByVal CodigoPortafolio As String)
        Dim oPortafolioBM As New PortafolioBM
        Dim dsDetalle As DataSet
        dsDetalle = oPortafolioBM.SeleccionarDetalleGrupoPortafolio(CodigoPortafolio)
        If dsDetalle.Tables(0).Rows.Count > 0 Then
            ViewState("TablaDetalleGrupoPortafolio") = dsDetalle.Tables(0)
            dgLista.DataSource = dsDetalle
            dgLista.DataBind()
        End If
    End Sub
    Private Function ObtenerInstancia() As PortafolioBE
        Dim oPortafolioBE As New PortafolioBE
        Dim oRow As PortafolioBE.PortafolioRow
        oRow = oPortafolioBE.Portafolio.NewPortafolioRow()
        If (chkConsolidado.Checked) Then
            oRow.Consolidado = 1
        Else
            oRow.Consolidado = 0
        End If
        oRow.CodigoPortafolioSBS = Me.tbCodigoPortafolioSBS.Text
        oRow.FechaConstitucion = UIUtility.ConvertirFechaaDecimal(Me.tbFechaConstitucion.Text)
        oRow.Descripcion = Me.txtDescripcion.Text
        oRow.CodigoNegocio = Me.ddlNegocio.SelectedValue
        oRow.CodigoTipoValorizacion = ""
        oRow.EsMultiPortafolio = Me.ddlMultifondo.SelectedValue
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.FechaTermino = UIUtility.ConvertirFechaaDecimal(Me.tbFechaConstitucion.Text)
        If Me.ddlMoneda.SelectedIndex = 0 Then
            oRow.CodigoMoneda = ""
        Else
            oRow.CodigoMoneda = Me.ddlMoneda.SelectedValue
        End If
        If chkIContable.Checked Then
            oRow.InterfazContable = "S"
            oRow.CodContabilidadFondo = tbCodigoFondo.Text
        Else
            oRow.InterfazContable = "N"
            oRow.CodContabilidadFondo = ""
        End If
        If chkSerie.Checked Then
            oRow.PorSerie = "S"
            oRow.PorcentajeComision = 0
        Else
            oRow.PorSerie = "N"
        End If
        '''' INI MPENAL - 09/09/2016
        If Me.ddlTipoRenta.SelectedIndex = 0 Then
            oRow.CodigoRenta = ""
        Else
            oRow.CodigoRenta = Me.ddlTipoRenta.SelectedValue
        End If
        '''' FIN MPENAL - 09/09/2016
        oRow.TipoCartera = ""
        oRow.PorcentajeComision = 0
        oRow.IndicadorFondo = ddlIndicador.SelectedValue
        oRow.CodigoFondosMutuos = tbCodFondoMutuo.Text
        oRow.ValorInicialFondo = tbValorINIFondo.Text
        oRow.NumeroCuotaPreCierre = tbNroCuotaPreCierre.Text
        oRow.NombreCompleto = tbNombreCompleto.Text
        oRow.FlagComisionVariable = CType(chkComisionVariable.Checked, Int32)
        If txtMontoSuscripcionInicial.Text = "" Then
            txtMontoSuscripcionInicial.Text = "0"
        End If
        oRow.MontoSuscripcionInicial = Convert.ToDecimal(txtMontoSuscripcionInicial.Text.Replace(".", UIUtility.DecimalSeparator))
        If txtTopeValorCuota.Text = "" Then
            txtTopeValorCuota.Text = "0"
        End If
        oRow.TopeValorCuota = Convert.ToDecimal(txtTopeValorCuota.Text.Replace(".", UIUtility.DecimalSeparator))
        oRow.FlagComisionSuscripInicial = CType(chkComisionSusIni.Checked, Int32)
        oRow.BDConta = txtBaseContable.Text
        oPortafolioBE.Portafolio.AddPortafolioRow(oRow)
        oPortafolioBE.Portafolio.AcceptChanges()
        oRow.FechaAperturaContable = UIUtility.ConvertirFechaaDecimal(Me.tbFechaContable.Text)
        oRow.CodigoPortafolioSisOpe = txtCodSO.Text
        oRow.RUCPortafolio = txtRuc.Text
        oRow.TipoCalculoValorCuota = ddlTipoCalculoValorCuota.SelectedValue
        oRow.FechaCajaOperaciones = UIUtility.ConvertirFechaaDecimal(Me.tbFechaConstitucion.Text)
        oRow.ValoracionMensual = ddlTipoValorizacion.SelectedValue
        If chkCuotasLiberadas.Checked Then
            oRow.CuotasLiberadas = "1"
        Else
            oRow.CuotasLiberadas = "0"
        End If
        oRow.CPPadreSisOpe = txtCPPadreSO.Text
        oRow.VectorPrecioVal = Me.hdnOrdenVectorPrecio.Value.Split(",")(0).Trim() 'Compatibilidad con campo creado por IAN anteriormente (CRumiche)
        oRow.TipoNegocio = ddlTipoNegocio.SelectedValue
        oRow.TipoComision = ddlTipoComision.SelectedValue
        '  INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
        oRow.FlagAumentoCapital = IIf(chkAumentoCapital.Checked, 1, 0)
        '  FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
        oRow.FondoCliente = IIf(chkFondoCliente.Checked = True, "S", "N")
        oRow.CodigoTerceroCliente = txtCodCliente.Value ' Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-24

        Return (oPortafolioBE)
    End Function
    Private Sub LimpiarControles()
        Me.tbCodigoPortafolioSBS.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbFechaConstitucion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlNegocio.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0
        Me.ddlMoneda.SelectedIndex = 0
        ViewState("Custodio") = Nothing
        GVcustodio.DataSource = Nothing
        GVcustodio.DataBind()
    End Sub
    Private Function ExisteEntidad() As Boolean
        Dim oPortafolioBE As PortafolioBE
        Dim oPortafolioBM As New PortafolioBM
        oPortafolioBE = oPortafolioBM.Seleccionar(Me.tbCodigoPortafolioSBS.Text, Me.DatosRequest)
        Return oPortafolioBE.Portafolio.Rows.Count > 0
    End Function
    Private Function ValidarDetalle(ByVal codigoPortafolio As String) As Boolean
        Dim dtDetalle As New Data.DataTable
        Dim intFila As Integer
        dtDetalle = CType(ViewState("TablaDetalleGrupoPortafolio"), Data.DataTable)
        If dtDetalle Is Nothing Then
            dtDetalle = New DataTable()
            dtDetalle.Columns.Add("CodigoPortafolioSBS")
            dtDetalle.Columns.Add("Descripcion")
            ViewState("TablaDetalleGrupoPortafolio") = dtDetalle
        Else
            For intFila = 0 To dtDetalle.Rows.Count - 1
                If (dtDetalle.Rows(intFila)("CodigoPortafolioSBS") = codigoPortafolio) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function
#End Region
    Protected Sub chkIContable_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIContable.CheckedChanged
        If chkIContable.Checked Then
            tbCodigoFondo.Enabled = True
        Else
            tbCodigoFondo.Enabled = False
            tbCodigoFondo.Text = ""
        End If
    End Sub
    Protected Sub chkSerie_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSerie.CheckedChanged
        If chkSerie.Checked Then
            FilaSeries.Visible = True
            PNComisionVariable.Visible = False
        Else
            FilaSeries.Visible = False
            PNComisionVariable.Visible = True
        End If
    End Sub
    Protected Sub btnaddcustodio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddcustodio.Click
        Try
            If ddlcustodio.SelectedValue = "" Then
                AlertaJS("Seleccione algun custodio de la lista.", "ddlcustodio.focus();")
            ElseIf txtcuentadepositaria.Text = "" Then
                AlertaJS("Ingrese la cuenta depositaria del custodio para este portafolio.", "txtcuentadepositaria.focus();")
            Else
                If ViewState("Custodio") Is Nothing Then
                    Dim oCustodio As New CustodioBM
                    ViewState("Custodio") = oCustodio.Cuentadepositaria_Portafolio(tbCodigoPortafolioSBS.Text)
                End If
                Dim dt As DataTable = ViewState("Custodio")
                For Each dradd As DataRow In dt.Rows
                    If ddlcustodio.SelectedValue = dradd(0) Then
                        AlertaJS("El custodio seleccionado ya esta en la lista")
                        Exit Sub
                    End If
                Next
                Dim dr As DataRow = dt.NewRow
                dr(0) = ddlcustodio.SelectedValue
                dr(1) = ddlcustodio.SelectedItem.Text
                dr(2) = txtcuentadepositaria.Text
                dt.Rows.Add(dr)
                ViewState("Custodio") = dt
                GVcustodio.DataSource = dt
                GVcustodio.DataBind()
                txtcuentadepositaria.Text = ""
            End If
        Catch ex As Exception
            EjecutarJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub GVcustodio_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVcustodio.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim dtDetalle As New Data.DataTable
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim dt As DataTable = ViewState("Custodio")
            dt.Rows(gvr.RowIndex).Delete()

            ViewState("TablaDetalleGrupoPortafolio") = dt
            GVcustodio.DataSource = dt
            GVcustodio.DataBind()
        End If
    End Sub
    Public Sub EliminarPC(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim objPortafolioPCBE As New PortafolioPorcentajeComisionBE
        Dim objPortafolioPCBM As New PortafolioPorcentajeComisionBM
        Dim resultado As Boolean = False
        Dim secuencia = e.CommandArgument
        Try
            objPortafolioPCBE.CodigoPortafolio = tbCodigoPortafolioSBS.Text.Trim()
            objPortafolioPCBE.Secuencia = secuencia
            resultado = objPortafolioPCBM.Eliminar(objPortafolioPCBE)
            If (resultado) Then
                CargarGrillaPorcentajeComision()
            End If
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; un error al eliminar Porcentaje Comisi&oacute;n.")
        End Try
    End Sub
    Protected Sub btnAgregarPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarPC.Click
        Dim objPortafolioPCBE As New PortafolioPorcentajeComisionBE
        Dim objPortafolioPCBM As New PortafolioPorcentajeComisionBM
        Dim resultado As Boolean = False
        Try
            objPortafolioPCBE.CodigoPortafolio = tbCodigoPortafolioSBS.Text.Trim()
            objPortafolioPCBE.ValorMargenMaximo = txtMargenMax.Text
            objPortafolioPCBE.ValorMargenMinimo = txtMargenMin.Text
            objPortafolioPCBE.ValorPorcentajeComision = txtPorcentaje.Text
            objPortafolioPCBE.UsuarioCreacion = Usuario
            objPortafolioPCBE.FechaCreacion = Convert.ToDecimal(Date.Now.ToString("yyyMMdd"))
            objPortafolioPCBE.HoraCreacion = Date.Now.ToString("HH:mm:ss")
            objPortafolioPCBE.Host = CType(DatosRequest.Tables(0).Select(DatosRequest.Tables(0).Columns(0).ColumnName & "='" & "Host" & "'")(0)(1), String)
            resultado = objPortafolioPCBM.Insertar(objPortafolioPCBE)
            If (resultado) Then
                CargarGrillaPorcentajeComision()
            End If
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; un error al ingresar Porcentaje Comisi&oacute;n.")
        End Try
    End Sub
    Private Sub CargarGrillaPorcentajeComision()
        Dim objPortafolioPCBM As New PortafolioPorcentajeComisionBM
        Dim listPortafolioPCBE As List(Of PortafolioPorcentajeComisionBE)
        Try
            listPortafolioPCBE = objPortafolioPCBM.Listar(tbCodigoPortafolioSBS.Text.Trim())
            dgPorcentajeComision.DataSource = listPortafolioPCBE
            dgPorcentajeComision.DataBind()
            txtMargenMax.Text = ""
            txtMargenMin.Text = ""
            txtPorcentaje.Text = ""
            Session("Grilla_PorcentajeComision") = listPortafolioPCBE
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; un error al cargar Porcentaje Comisi&oacute;n.")
        End Try
    End Sub
    Protected Sub btnAgregarSeries_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarSeries.Click
        Try
            Dim dtSeries As New DataTable
            dtSeries = RecuperarGrillaSeries()
            dtSeries.Rows.Add(0, txtCodigoSerie.Text.Trim(), txtNombreSerie.Text.Trim(), txtPorcentajeSerie.Text, txtCodigoPortafolioSO.Text.Trim())
            dgGrillaVCouta.DataSource = dtSeries
            dgGrillaVCouta.DataBind()
            txtCodigoSerie.Text = String.Empty
            txtNombreSerie.Text = String.Empty
            txtPorcentajeSerie.Text = String.Empty
            txtCodigoPortafolioSO.Text = String.Empty
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; un error al Agregar la serie.")
        End Try
    End Sub
    Private Function RecuperarGrillaSeries() As DataTable
        Dim dtSeries As New DataTable
        dtSeries.Columns.Add("IdPortafolioSerie")
        dtSeries.Columns.Add("CodigoSerie")
        dtSeries.Columns.Add("NombreSerie")
        dtSeries.Columns.Add("Porcentaje")
        dtSeries.Columns.Add("CodigoPortafolioSO")
        For Each gvRow As GridViewRow In dgGrillaVCouta.Rows
            Dim drSerie As DataRow = dtSeries.NewRow
            drSerie("IdPortafolioSerie") = 0
            drSerie("CodigoSerie") = gvRow.Cells(1).Text.Trim()
            drSerie("NombreSerie") = gvRow.Cells(2).Text.Trim()
            drSerie("Porcentaje") = gvRow.Cells(3).Text.Trim()
            drSerie("CodigoPortafolioSO") = gvRow.Cells(4).Text.Trim()
            dtSeries.Rows.Add(drSerie)
        Next
        Return dtSeries
    End Function
    Protected Sub dgGrillaVCouta_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgGrillaVCouta.RowCommand
        Try
            Select Case e.CommandName
                Case "EliminarSerie"
                    Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                    Dim index As Int32 = row.RowIndex
                    'Dim dt, dtEliminar As DataTable
                    Dim dt As DataTable
                    Dim oRow As DataRow
                    dt = RecuperarGrillaSeries()
                    oRow = dt.Rows(index)
                    'dtEliminar = RecuperarGrillaSeries()
                    'dtEliminar.Clear()
                    'dtEliminar = Session("EliminarSeries")
                    'If dtEliminar Is Nothing Then
                    '    dtEliminar = New DataTable
                    '    dtEliminar.Columns.Add("IdPortafolioSerie")
                    '    dtEliminar.Columns.Add("CodigoSerie")
                    '    dtEliminar.Columns.Add("NombreSerie")
                    '    dtEliminar.Columns.Add("Porcentaje")
                    '    dtEliminar.Columns.Add("CodigoPortafolioSO")
                    'End If
                    'dtEliminar.Rows.Add(oRow(0), oRow(1), oRow(2), 0)
                    'Session("EliminarSeries") = dtEliminar
                    dt.Rows(index).Delete()
                    dgGrillaVCouta.DataSource = dt
                    dgGrillaVCouta.DataBind()
            End Select
        Catch ex As Exception
            AlertaJS("Ocurri&oacute; un error al eliminar la serie.")
        End Try
    End Sub

    '''' INI MPENAL - 09/09/2016
    'Protected Sub ddlTipoRenta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoRenta.SelectedIndexChanged
    '    Try
    '        If ddlTipoRenta.SelectedIndex <> 0 Then
    '            CargaTipoTituloDeTipoRenta()
    '        End If
    '    Catch ex As Exception
    '        AlertaJS("Ocurrió un error al Seleccionar")
    '    End Try
    'End Sub

    Private Sub CargaTipoTituloDeTipoRenta()
        'Dim dtTipoTitulo As DataTable
        'Dim dtAux As New DataTable
        'Dim i As Integer
        'dtTipoTitulo = Session("tipoTitulo")
        'dtAux = dtTipoTitulo.Clone
        'For i = 0 To dtTipoTitulo.Rows.Count - 1
        '    If CType(dtTipoTitulo.Rows(i)("CodigoTipoRenta"), String) = ddlTipoRenta.SelectedValue Then
        '        dtAux.ImportRow(dtTipoTitulo.Rows(i))
        '    End If
        'Next
        'HelpCombo.LlenarComboBox(ddlTipoTitulo, dtAux, "CodigoTipoTitulo", "CodigoTipoTitulo", False)
    End Sub
    '''' FIN MPENAL - 09/09/2016

End Class