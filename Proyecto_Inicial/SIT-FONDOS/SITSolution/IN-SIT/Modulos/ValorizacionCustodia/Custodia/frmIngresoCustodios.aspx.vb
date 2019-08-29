Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports ParametrosSIT

Partial Class Modulos_Valorizacion_y_Custodia_Custodia_frmIngresoCustodios
    Inherits BasePage

#Region "Variables"

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

    Private campos() As String = {"Fecha Ajuste", "Código Mnemónico", "Descripción", "Unidades Ajustadas"}

#End Region

#Region "/* Métodos de la Página  */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Call DescargaGrillaLista()
                CargaPortafolio()
                Call CargaCustodios()
                tbFechaOperacion.Text = oUtil.RetornarFechaSistema
                btnAceptar.Attributes.Add("onclick", "return confirm('" & ObtenerMensaje("CONF10", "Faltantes") & "');")
                ViewState("vsPoseeSaldo") = 0
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
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Try
            oUtil = Nothing
            oUIUtil = Nothing
            oCustodioAjusteBE = Nothing
            oCustodioAjusteBM = Nothing
            oCustodioKardexBE = Nothing
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        End Try
    End Sub

    Private Sub dlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dlFondo.SelectedIndexChanged
        Try
            If Page.IsPostBack Then
                Call CargaCustodios()
            End If
            EstablecerFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
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
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsulta.Click
        Try
            Dim oValoresBE As DataSet = oValoresBM.SeleccionarInstrumento(txtISIN.Text, txtMnemonico.Text, DatosRequest)
            If oValoresBE.Tables(0).Rows.Count > 0 Then
                txtISIN.Text = oValoresBE.Tables(0).Rows(0).ItemArray(0)
                txtMnemonico.Text = oValoresBE.Tables(0).Rows(0).ItemArray(1)
                txtSBS.Text = oValoresBE.Tables(0).Rows(0).ItemArray(2)
                txtDescripcion.Text = oValoresBE.Tables(0).Rows(0).ItemArray(3)
            End If
            If ValidarConsulta() Then
                If Not RecuperaSaldosInstrumento(txtSBS.Text.Trim, dlFondo.SelectedValue.Trim, Me.ddlCustodio.SelectedValue.Trim, tbFechaOperacion.Text.Trim) Then
                    ViewState("vsPoseeSaldo") = 0
                    AlertaJS(ObtenerMensaje("ALERT95"))
                End If
                Call CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Consultar")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

    Protected Sub dgAjuste_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAjuste.PageIndexChanging
        Try
            dgAjuste.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

#End Region

#Region " /* Métodos Personalizadas*/"

    Private Sub DescargaGrillaLista()
        dgAjuste.DataSource = UIUtility.GetStructureTablebase(campos)
        dgAjuste.DataBind()
    End Sub

    Private Function RecuperaSaldosInstrumento(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal sFechaOperacion As String) As Boolean
        Dim oValores As New ValoresBM
        Dim oValoresBE As DataSet = oValoresBM.RecuperaSaldosInstrumento(CodigoSBS, CodigoPortafolioSBS, CodigoCustodio, oUIUtil.ConvertirFechaaDecimal(sFechaOperacion), DatosRequest)

        If oValoresBE.Tables(0).Rows.Count > 0 Then
            txtSaldoSistema.Text = String.Format("{0:0.0000000}", oValoresBE.Tables(0).Rows(0).Item("SaldoInicialUnidades"))
            txtSaldoCustodio.Text = String.Format("{0:0.0000000}", oValoresBE.Tables(0).Rows(0).Item("SaldoContable"))
            lblfechaSaldoSistema.Text = "" & oValoresBE.Tables(0).Rows(0).Item("FechaSaldo")
            lblFechaSaldoCustodio.Text = "" & oValoresBE.Tables(0).Rows(0).Item("FechaCorte")
            ViewState("vsPoseeSaldo") = oValoresBE.Tables(0).Rows(0).Item("TieneSaldo")
            If oValoresBE.Tables(0).Rows(0).Item("CodigoCustodioInf") <> "" Then
                ViewState("vsInformacionCustodio") = True
                ViewState("vsSaldoCustodio") = oValoresBE.Tables(0).Rows(0).Item("SaldoContable")
                txtCantidadUnidades.Text = String.Format("{0:0.0000000}", (oValoresBE.Tables(0).Rows(0).Item("SaldoContable") - oValoresBE.Tables(0).Rows(0).Item("SaldoInicialUnidades")))
            Else
                AlertaJS(ObtenerMensaje("ALERT96"))
                ViewState("vsInformacionCustodio") = False
                ViewState("vsSaldoCustodio") = 0
            End If

            Return True
            Exit Function
        Else
            ViewState("vsInformacionCustodio") = False
            txtSaldoSistema.Text = "0"
            txtSaldoCustodio.Text = "0"
        End If

        oValoresBE = Nothing
        Return False
    End Function

    Private Sub CargarGrilla()
        Dim oInfCusDS As New DataTable
        oInfCusDS = oCustodioAjusteBM.Listar(dlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim, txtSBS.Text, "I", DatosRequest).Tables(0)
        dgAjuste.DataSource = oInfCusDS
        dgAjuste.DataBind()
    End Sub

    Private Function VerificaRelacionInstrumentoCustodio(ByVal CodigoMnemonico As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String) As Boolean
        Dim oValores As New ValoresBM
        Dim oValoresBE As DataSet = oValoresBM.VerificaRelacionInstrumentoCustodio(CodigoMnemonico, CodigoPortafolioSBS, CodigoCustodio, DatosRequest)

        ViewState("vsCuentaDepositaria") = ""
        If oValoresBE.Tables(0).Rows.Count > 0 Then
            ViewState("vsCuentaDepositaria") = oValoresBE.Tables(0).Rows(0).Item("CuentaDepositaria")
            Return True
            Exit Function
        End If

        oValoresBE = Nothing
        Return False
    End Function

    Private Function CarteraTituloVerifica(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String) As Boolean
        Dim oValores As New ValoresBM
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
        txtMnemonico.Text = ""
        txtISIN.Text = ""
        txtSBS.Text = ""
        txtDescripcion.Text = ""
        txtCantidadUnidades.Text = ""
        txtSaldoSistema.Text = ""
        txtSaldoCustodio.Text = ""
        txtMoneda.Text = ""
        lblfechaSaldoSistema.Text = ""
        lblFechaSaldoCustodio.Text = ""
        Call DescargaGrillaLista()
    End Sub
    Private Sub CargaPortafolio()
        UIUtility.CargarPortafoliosOI(Me.dlFondo)
        'Dim objportafolio As New PortafolioBM
        'dlFondo.DataSource = objportafolio.Listar(Nothing, ESTADO_ACTIVO, CODIGO_NEGOCIO_FONDO)
        'dlFondo.DataTextField = "Descripcion"
        'dlFondo.DataValueField = "CodigoPortafolioSBS"
        'dlFondo.DataBind()
        UIUtility.InsertarElementoSeleccion(dlFondo)
        'objportafolio = Nothing
    End Sub

    Private Sub CargaCustodios()
        Dim objCustodio As New CustodioBM
        ddlCustodio.DataTextField = "Descripcion"
        ddlCustodio.DataValueField = "CodigoCustodio"
        ddlCustodio.DataSource = objCustodio.Listar(DatosRequest)
        ddlCustodio.DataBind()
        UIUtility.InsertarElementoSeleccion(ddlCustodio)
        objCustodio = Nothing
    End Sub

    Private Sub EstablecerFecha()
        If dlFondo.SelectedIndex > 0 Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(dlFondo.SelectedValue))
        Else
            tbFechaOperacion.Text = Now.ToString("dd/MM/yyyy")
        End If
    End Sub

    Private Function ValidarConsulta() As Boolean
        If dlFondo.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT3"))
            Return False
            Exit Function
        ElseIf ddlCustodio.SelectedValue.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT4"))
            Return False
            Exit Function
        ElseIf txtSBS.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT5"))
            Return False
            Exit Function
        ElseIf Not VerificaRelacionInstrumentoCustodio(txtMnemonico.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim) Then
            AlertaJS(ObtenerMensaje("ALERT112"))
            Return False
            Exit Function
        End If
        Return True
    End Function

    Private Function Validar() As Boolean
        If Not ValidarConsulta() Then
            Return False
            Exit Function
        ElseIf txtMnemonico.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT113"))
            Return False
            Exit Function
        ElseIf txtCantidadUnidades.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT8"))
            Return False
            Exit Function
        ElseIf ViewState("vsInformacionCustodio") = False And chkSaldoInicial.Checked = False Then
            AlertaJS(ObtenerMensaje("ALERT96"))
            Return False
            Exit Function
        ElseIf (System.Convert.ToDecimal(txtSaldoCustodio.Text.Trim & "0") - System.Convert.ToDecimal(txtSaldoSistema.Text.Trim & "0")) <> System.Convert.ToDecimal(txtCantidadUnidades.Text.Trim & "0") And (chkSaldoInicial.Checked = False) Then
            AlertaJS(ObtenerMensaje("ALERT97"))
            Return False
            Exit Function
        End If

        If chkSaldoInicial.Checked = False Then
            If Not CarteraTituloVerifica(txtSBS.Text.Trim, dlFondo.SelectedValue.Trim, ddlCustodio.SelectedValue.Trim) Then
                AlertaJS(ObtenerMensaje("ALERT69"))
                Return False
                Exit Function
            End If
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
                                                        "I", _
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
                                                        ViewState("vsCuentaDepositaria"), _
                                                        txtCantidadUnidades.Text.Trim, _
                                                        0, _
                                                        0, _
                                                        "I", _
                                                        0)

            Return oCustodioAjusteBM.Insertar(oCustodioAjusteBE, oCustodioKardexBE, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim), DatosRequest)

        End If
    End Function

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

#End Region

End Class
