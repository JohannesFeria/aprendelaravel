Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT

Partial Class Modulos_Valorizacion_y_Custodia_Custodia_frmTransferenciaCustodios
    Inherits BasePage

#Region "Variables"
    Dim objPortafolio As New PortafolioBM
    '------------------------------------------------------------
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
    Dim oCustodioAjusteBE As New CustodioAjusteBE
    Dim oCustodioAjusteBM As New CustodioAjusteBM

    Dim oCustodioKardexBE As New CustodioKardexBE
    Dim oValoresBM As New ValoresBM

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Function RecuperaSaldoCustodioOrigen(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal sFechaOperacion As String) As Boolean
        Dim oValoresBE As DataSet = oValoresBM.RecuperaSaldoTransferencia(CodigoSBS, CodigoPortafolioSBS, CodigoCustodio, UIUtility.ConvertirFechaaDecimal(sFechaOperacion), DatosRequest)

        If oValoresBE.Tables(0).Rows.Count > 0 Then
            txtSaldoCustodio.Text = String.Format("{0:0.0000000}", oValoresBE.Tables(0).Rows(0).Item("SaldoInicialUnidades"))
            ViewState("vsSaldoCustodioOrigen") = oValoresBE.Tables(0).Rows(0).Item("SaldoInicialUnidades")
            Return True
            Exit Function
        Else
            txtSaldoCustodio.Text = "0"
        End If

        oValoresBE = Nothing
        Return False
    End Function

    Private Function RecuperaSaldoCustodioDestino(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal sFechaOperacion As String) As Boolean
        Dim oValoresBE As DataSet = oValoresBM.RecuperaSaldoTransferencia(CodigoSBS, CodigoPortafolioSBS, CodigoCustodio, UIUtility.ConvertirFechaaDecimal(sFechaOperacion), DatosRequest)

        If oValoresBE.Tables(0).Rows.Count > 0 Then
            txtSaldoCustodioFinal.Text = String.Format("{0:0.0000000}", oValoresBE.Tables(0).Rows(0).Item("SaldoInicialUnidades"))
            ViewState("vsSaldoCustodioDestino") = oValoresBE.Tables(0).Rows(0).Item("SaldoInicialUnidades")
            Return True
            Exit Function
        Else
            txtSaldoCustodioFinal.Text = "0"
        End If

        oValoresBE = Nothing
        Return False
    End Function

    Private Function VerificaRelacionInstrumentoCustodio(ByVal CodigoMnemonico As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String) As Boolean
        Dim oValoresBE As DataSet = oValoresBM.VerificaRelacionInstrumentoCustodio(CodigoMnemonico, CodigoPortafolioSBS, CodigoCustodio, DatosRequest)

        viewstate("vsCuentaDepositaria") = ""
        If oValoresBE.Tables(0).Rows.Count > 0 Then
            viewstate("vsCuentaDepositaria") = oValoresBE.Tables(0).Rows(0).Item("CuentaDepositaria")
            Return True
            Exit Function
        End If

        oValoresBE = Nothing
        Return False
    End Function

    Private Function CarteraTituloVerifica(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String) As Boolean
        Dim oValoresBE As DataSet = oValoresBM.CarteraTituloVerifica(CodigoSBS, CodigoPortafolioSBS, CodigoCustodio, DatosRequest)

        If oValoresBE.Tables(0).Rows.Count > 0 Then
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
        dlFondo.SelectedIndex = 0
        ddlCustodio.SelectedIndex = 0
        ddlCustodioFinal.SelectedIndex = 0
        txtMnemonico.Text = ""
        txtISIN.Text = ""
        txtSBS.Text = ""
        txtDescripcion.Text = ""
        txtCantidadUnidades.Text = ""
        txtSaldoCustodio.Text = ""
        txtSaldoCustodioFinal.Text = ""
    End Sub
    Private Sub CargaPortafolio()
        UIUtility.CargarPortafoliosOI(Me.dlFondo)
        UIUtility.InsertarElementoSeleccion(dlFondo)
    End Sub

    Private Sub CargaCustodios(ByVal ddList As DropDownList)
        Dim objCustodio As New CustodioBM
        ddList.DataTextField = "Descripcion"
        ddList.DataValueField = "CodigoCustodio"
        ddList.DataSource = objCustodio.Listar(DatosRequest)
        ddList.DataBind()
        UIUtility.InsertarElementoSeleccion(ddList)
    End Sub

    Private Sub EstablecerFecha()
        If dlFondo.SelectedIndex > 0 Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(dlFondo.SelectedValue))
        Else
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(PORTAFOLIO_MULTIFONDOS))
        End If
    End Sub

    Private Function ValidarConsulta() As Boolean
        Dim oValoresBE As DataSet = oValoresBM.SeleccionarInstrumento(txtISIN.Text, txtMnemonico.Text, DatosRequest)

        If oValoresBE.Tables(0).Rows.Count > 0 Then
            txtISIN.Text = oValoresBE.Tables(0).Rows(0).ItemArray(0)
            txtMnemonico.Text = oValoresBE.Tables(0).Rows(0).ItemArray(1)
            txtSBS.Text = oValoresBE.Tables(0).Rows(0).ItemArray(2)
            txtDescripcion.Text = oValoresBE.Tables(0).Rows(0).ItemArray(3)
        End If
        If dlFondo.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT3"))
            Return False
            Exit Function
        ElseIf ddlCustodio.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT4"))
            Return False
            Exit Function
        ElseIf ddlCustodioFinal.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT4"))
            Return False
            Exit Function
        ElseIf ddlCustodio.SelectedValue.Trim = ddlCustodioFinal.SelectedValue.Trim Then
            AlertaJS(ObtenerMensaje("ALERT11"))
            Return False
            Exit Function
        ElseIf txtMnemonico.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT113"))
            Return False
            Exit Function
        End If
        oValoresBE = Nothing
        Return True
    End Function

    Private Function Validar() As Boolean
        Dim Mensaje As String = "El valor " + txtMnemonico.Text + " NO tiene como Custodio "
        If Not ValidarConsulta() Then
            Return False
            Exit Function
        ElseIf txtSBS.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT5"))
            Return False
            Exit Function
        ElseIf Not VerificaRelacionInstrumentoCustodio(txtMnemonico.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim) Then
            Mensaje += ddlCustodio.SelectedValue
            AlertaJS(Mensaje)
            Return False
            Exit Function
        ElseIf Not VerificaRelacionInstrumentoCustodio(txtMnemonico.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodioFinal.SelectedValue.Trim) Then
            Mensaje += ddlCustodioFinal.SelectedValue
            AlertaJS(Mensaje)
            Return False
            Exit Function
        ElseIf Not CarteraTituloVerifica(txtSBS.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim) Then
            AlertaJS(ObtenerMensaje("ALERT69"))
            Return False
            Exit Function
        ElseIf txtCantidadUnidades.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT8"))
            Return False
            Exit Function
        ElseIf System.Convert.ToDecimal("0" & txtSaldoCustodio.Text.Trim) = 0 Then
            AlertaJS(ObtenerMensaje("ALERT116"))
            Return False
            Exit Function
        ElseIf System.Convert.ToDecimal("0" & txtSaldoCustodio.Text.Trim) < System.Convert.ToDecimal(txtCantidadUnidades.Text.Trim) Then
            AlertaJS(ObtenerMensaje("ALERT117"))
            Return False
            Exit Function
        End If

        Return True
    End Function

    Private Function InsertarAjusteKardex() As Boolean
        Dim dHoraSistema As DateTime = oUtil.RetornarFechaHoraSistema
        Dim sHoraSistema As String = Right("0" & dHoraSistema.Hour.ToString.Trim, 2) & Right("0" & dHoraSistema.Minute.ToString.Trim, 2) & Right("0" & dHoraSistema.Minute.ToString.Trim, 2) & Right("0" & dHoraSistema.Second.ToString.Trim, 2)

        If Validar() Then
            oCustodioAjusteBE = CrearObjetoCustodioAjuste( _
                                                        dlFondo.SelectedValue.Trim, _
                                                        0, _
                                                        txtISIN.Text.Trim, _
                                                        txtMnemonico.Text.Trim, _
                                                        0, _
                                                        UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim), _
                                                        sHoraSistema, _
                                                        txtCantidadUnidades.Text.Trim, _
                                                        0, _
                                                         0, _
                                                        "T", _
                                                        0)

            oCustodioKardexBE = CrearObjetoCustodioKardex( _
                                                        dlFondo.SelectedValue.ToString.Trim, _
                                                        0, _
                                                        txtISIN.Text.Trim, _
                                                        txtMnemonico.Text.Trim, _
                                                        0, _
                                                        0, _
                                                        UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim), _
                                                        sHoraSistema, _
                                                        0, _
                                                        ddlCustodio.SelectedValue.Trim, _
                                                        ddlCustodioFinal.SelectedValue.Trim, _
                                                         "", _
                                                        txtCantidadUnidades.Text.Trim, _
                                                        0, _
                                                        0, _
                                                        "E", _
                                                        0)

            Return oCustodioAjusteBM.InsertarTransferencia(oCustodioAjusteBE, oCustodioKardexBE, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim), DatosRequest)

        End If

    End Function

    Private Sub Limpiar_SaldosCustodios()
        txtSaldoCustodio.Text = 0
        txtSaldoCustodioFinal.Text = 0
        txtCantidadUnidades.Text = 0
    End Sub

#End Region

#Region " /* Métodos de la Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargaPortafolio()
                CargaCustodios(ddlCustodio)
                CargaCustodios(ddlCustodioFinal)
                tbFechaOperacion.Text = oUtil.RetornarFechaSistema
            Else
                ViewState("vsPortafolioCodigo") = dlFondo.SelectedValue.ToString
                ViewState("vsCodigoCustodio") = ddlCustodio.SelectedValue.ToString
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datos As String()
                datos = CType(Session("SS_DatosModal"), String())
                txtISIN.Text = HttpUtility.HtmlDecode(datos(0))
                txtMnemonico.Text = HttpUtility.HtmlDecode(datos(1))
                txtSBS.Text = HttpUtility.HtmlDecode(datos(2))
                txtDescripcion.Text = HttpUtility.HtmlDecode(datos(3))
                txtMoneda.Text = HttpUtility.HtmlDecode(datos(4))
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        oUtil = Nothing
        oUIUtil = Nothing
    End Sub

    Private Sub dlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dlFondo.SelectedIndexChanged
        Try
            If Page.IsPostBack Then
                CargaCustodios(ddlCustodio)
                CargaCustodios(ddlCustodioFinal)
            End If
            EstablecerFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Fondo")
        End Try
    End Sub

    Private Sub ddlCustodio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCustodio.SelectedIndexChanged
        Try
            If Page.IsPostBack Then
                'CargaCodigoMnemonico(viewstate("vsPortafolioCodigo"), viewstate("vsCodigoCustodio"))
                'CargaCodigoCodigoISIN(viewstate("vsPortafolioCodigo"), viewstate("vsCodigoCustodio"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Custodio Inicial")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If InsertarAjusteKardex() Then
                AlertaJS(ObtenerMensaje("ALERT1"))
                Call Limpiadatos()
            Else
                AlertaJS(ObtenerMensaje("ALERT68"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Function CrearObjetoCustodioAjuste(ByVal CodigoPortafolioSBS As String, _
                                                ByVal NumeroTitulo As Decimal, _
                                                ByVal CodigoISIN As String, _
                                                ByVal CodigoMnemonico As String, _
                                                ByVal NumeroOperacion As Decimal, _
                                                ByVal FechaAjuste As Decimal, _
                                                ByVal HoraAjuste As String, _
                                                ByVal CantidadUnidades As Decimal, _
                                                ByVal ValorOrigen As Double, _
                                                ByVal ValorLocal As Double, _
                                                ByVal TipoMovimiento As String, _
                                                ByVal NumeroLamina As String) As CustodioAjusteBE

        Dim oCustodioAjusteBE As New CustodioAjusteBE
        Dim oRow As CustodioAjusteBE.CustodioAjusteRow

        oRow = CType(oCustodioAjusteBE.CustodioAjuste.NewRow(), CustodioAjusteBE.CustodioAjusteRow)

        oRow.CodigoPortafolioSBS() = CodigoPortafolioSBS
        oRow.NumeroTitulo() = NumeroTitulo
        oRow.CodigoISIN() = CodigoISIN
        oRow.CodigoMnemonico() = CodigoMnemonico
        oRow.NumeroOperacion() = NumeroOperacion
        oRow.FechaAjuste() = FechaAjuste
        oRow.HoraAjuste() = HoraAjuste
        oRow.CantidadUnidades() = CantidadUnidades
        oRow.ValorOrigen() = ValorOrigen
        oRow.ValorLocal() = ValorLocal
        oRow.TipoMovimiento() = TipoMovimiento

        oCustodioAjusteBE.CustodioAjuste.AddCustodioAjusteRow(oRow)
        oCustodioAjusteBE.CustodioAjuste.AcceptChanges()

        Return oCustodioAjusteBE

    End Function

    Private Function CrearObjetoCustodioKardex(ByVal CodigoPortafolioSBS As String, _
                                                ByVal NumeroTitulo As Decimal, _
                                                ByVal CodigoISIN As String, _
                                                ByVal CodigoMnemonico As String, _
                                                ByVal NumeroOperacion As Decimal, _
                                                ByVal NumeroMovimiento As Decimal, _
                                                ByVal FechaMovimiento As Decimal, _
                                                ByVal HoraMovimiento As String, _
                                                ByVal TituloNumeroLocalizacion As String, _
                                                ByVal CodigoCustodio As String, _
                                                ByVal CodigoCustodioFinal As String, _
                                                ByVal CtaDepositaria As String, _
                                                ByVal CantidadUnidades As Decimal, _
                                                ByVal ValorOrigenMovimiento As Decimal, _
                                                ByVal ValorLocalMovimiento As Decimal, _
                                                ByVal TipoMovimiento As String, _
                                                ByVal NumeroLamina As String) As CustodioKardexBE

        Dim oCustodioKardexBE As New CustodioKardexBE
        Dim oRow As CustodioKardexBE.CustodioKardexRow

        oRow = CType(oCustodioKardexBE.CustodioKardex.NewRow(), CustodioKardexBE.CustodioKardexRow)

        oRow.CodigoPortafolioSBS() = CodigoPortafolioSBS
        oRow.NumeroTitulo() = NumeroTitulo
        oRow.CodigoISIN() = CodigoISIN
        oRow.CodigoMnemonico() = CodigoMnemonico
        oRow.NumeroOperacion() = NumeroOperacion
        oRow.NumeroMovimiento() = NumeroMovimiento
        oRow.FechaMovimiento() = FechaMovimiento
        oRow.HoraMovimiento() = HoraMovimiento
        oRow.TituloNumeroLocalizacion() = TituloNumeroLocalizacion
        oRow.CodigoCustodio() = CodigoCustodio
        oRow.CodigoCustodioFinal() = CodigoCustodioFinal
        oRow.CtaDepositaria() = CtaDepositaria
        oRow.CantidadUnidades() = CantidadUnidades
        oRow.ValorOrigenMovimiento() = ValorOrigenMovimiento
        oRow.ValorLocalMovimiento() = ValorLocalMovimiento
        oRow.TipoMovimiento() = TipoMovimiento
        oRow.NumeroLamina() = NumeroLamina

        oCustodioKardexBE.CustodioKardex.AddCustodioKardexRow(oRow)
        oCustodioKardexBE.CustodioKardex.AcceptChanges()

        Return oCustodioKardexBE

    End Function

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de página")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim Mensaje As String = "El valor " + txtMnemonico.Text + " NO tiene como Custodio "

            If Not ValidarConsulta() Then
                Exit Sub
            End If

            If Not VerificaRelacionInstrumentoCustodio(txtMnemonico.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim) Then
                Mensaje += ddlCustodio.SelectedValue
                AlertaJS(Mensaje)
                Exit Sub
            Else
                If Not RecuperaSaldoCustodioOrigen(txtSBS.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim, tbFechaOperacion.Text.Trim) Then

                End If
            End If

            If Not VerificaRelacionInstrumentoCustodio(txtMnemonico.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodioFinal.SelectedValue.Trim) Then
                Mensaje += ddlCustodioFinal.SelectedValue
                AlertaJS(Mensaje)
                Exit Sub
            Else
                If RecuperaSaldoCustodioDestino(txtSBS.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodioFinal.SelectedValue.Trim, tbFechaOperacion.Text.Trim) Then

                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

#End Region

End Class
