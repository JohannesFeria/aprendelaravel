Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Partial Class Modulos_Tesoreria_OperacionesCaja_frmTransferencias
    Inherits BasePage
#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarMoneda(True)
            CargarPortafolio(True)
            CargarPortafolioDestino(True)
            ValidarDatosPortafolioOrigen()
            ValidarDatosPortafolioDestino()
            CargarCombos()
            EstablecerFecha()
            BindingModeloCarta(ddltipoTraspaso.SelectedValue)
            Me.ddlContacto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
            hdValSaldo.Value = "0"
        End If
        btnAceptar.Attributes.Add("onclick", "return ValidarCuentas();")
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub
    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        If ddlMoneda.SelectedValue <> "" Then
            lblMoneda.Text = ddlMoneda.SelectedItem.Text
        Else
            lblMoneda.Text = ""
        End If
        Me.lblSaldoDisponible.Text = ""
        Me.lblSaldoDisponible2.Text = ""
        CargarDetPortafolioOrigen(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, ddlMercado.SelectedValue)
        CargarDetPortafolioDestino(ddlBancoDestino.SelectedValue, ddlClaseCuentaDestino.SelectedValue, ddlPortafolioDestino.SelectedValue, ddlMoneda.SelectedValue, ddlMercadoDestino.SelectedValue)
    End Sub
    Private Sub CambiarEstadoDDL(ByVal sel As DropDownList, ByVal enabled As Boolean)
        If Not enabled Then
            sel.SelectedIndex = 0
        End If
        sel.Enabled = enabled
    End Sub
    Private Sub ConfirmaSaldosBancarios()
        Dim script As New StringBuilder
        Dim objUtil As New UtilDM
        Dim Mensaje As String = objUtil.RetornarMensajeConfirmacion("ALERT16") & ". ¿Desea continuar?"
        With script
            .Append("<script>")
            .Append("function ConfirmaSaldo()")
            .Append("{ ")
            .Append("   if (confirm(""" + Mensaje + """)) ")
            .Append("   { document.getElementById('" + btnAceptar.ClientID + "').onclick = ''; document.getElementById('" + btnAceptar.ClientID + "').click(); } ")
            .Append("   else ")
            .Append("   { document.getElementById('" + hdValSaldo.ClientID + "').value = '0'; return false; } ")
            .Append("} ")
            .Append("ConfirmaSaldo();")
            .Append("</script>")
        End With
        EjecutarJS(script.ToString(), False)
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try

            Dim mensajeValidacion As String = String.Empty
            If ddlClaseCuenta.SelectedValue <> ddlClaseCuentaDestino.SelectedValue Then
                mensajeValidacion = UIUtility.ValidarCajas(ddlPortafolio.SelectedValue, ddlClaseCuenta.SelectedValue)
                If mensajeValidacion <> String.Empty Then
                    AlertaJS(mensajeValidacion)
                    Exit Sub
                End If
                mensajeValidacion = UIUtility.ValidarCajas(ddlPortafolio.SelectedValue, ddlClaseCuentaDestino.SelectedValue)
                If mensajeValidacion <> String.Empty Then
                    AlertaJS(mensajeValidacion)
                    Exit Sub
                End If
            End If

                'Dim dtOrigen As DataTable = UIUtility.ObtenerFechaCaja(ddlPortafolio.SelectedValue, "")

                'If dtOrigen.Rows.Count = 0 Then
                '    AlertaJS("No existe cierre de caja para la clase origen" + ddlClaseCuenta.SelectedItem.Text)
                '    Exit Sub
                'End If

                'INICIO | ZOLUXIONES | RCE | OT12003 | 15/05/19 
                'Validacion Fecha Caja con clase Inversion para agregar movimientos
            Dim FechaCaja As Decimal = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClaseCuenta.SelectedValue)

            If UIUtility.ConvertirFechaaDecimal(txtFecha.Text) <> FechaCaja Then
                AlertaJS("Solo se puede modificar, agregar o eliminar operaciones en la fecha de Caja asignada. Fecha de Caja : " + UIUtility.ConvertirFechaaString(FechaCaja))
                Exit Sub
            End If



            'Validacion Fecha de Valoracion
                Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFecha.Text))
                If cantidadreg > 0 Then
                    AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                    txtFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                    Exit Sub
                End If
                'FIN  | ZOLUXIONES | RCE | OT12003 | 15/05/19 

                If hdValSaldo.Value = "0" Then
                    Dim dsSaldos As DataSet = New SaldosBancariosBM().SeleccionarSaldoPorNumeroCuenta(ddlPortafolio.SelectedValue, ddlNroCuenta.SelectedValue)
                    Dim sSaldo As String
                    If dsSaldos.Tables(0).Rows.Count > 0 Then
                        sSaldo = dsSaldos.Tables(0).Rows(0)("SaldoDisponibleInicial")
                    Else
                        sSaldo = "0"
                    End If
                    Dim saldo As Decimal = Decimal.Parse(sSaldo.Replace(".", UIUtility.DecimalSeparator()))
                    If saldo < _
                        Decimal.Parse(txtImporte.Text.Replace(".", UIUtility.DecimalSeparator())) Then
                        If ddlMercado.SelectedValue = MERCADO_EXTERIOR And ddlMercadoDestino.SelectedValue = MERCADO_LOCAL Then
                            hdValSaldo.Value = "1"
                            ConfirmaSaldosBancarios()
                        Else
                            AlertaJS(ObtenerMensaje("ALERT16"))
                        End If
                        Exit Sub
                    End If
                Else
                    hdValSaldo.Value = "0"
                End If


            Dim msjValidacion As String = ""
            If Not validarTipoOperacion(msjValidacion) Then
                AlertaJS(msjValidacion)
            Else
                Dim dsOpCaja As New OperacionCajaBE
                Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                opCaja.CodigoMercado = ddlMercado.SelectedValue
                opCaja.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                opCaja.CodigoPortafolioSBSDestino = ddlPortafolioDestino.SelectedValue
                opCaja.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue
                opCaja.CodigoMoneda = ddlMoneda.SelectedValue
                opCaja.NumeroCuenta = ddlNroCuenta.SelectedValue
                opCaja.CodigoTerceroOrigen = ddlBanco.SelectedValue
                opCaja.CodigoTerceroDestino = ddlBancoDestino.SelectedValue
                opCaja.CodigoClaseCuentaDestino = ddlClaseCuentaDestino.SelectedValue
                opCaja.NumeroCuentaDestino = ddlNroCuentaDestino.SelectedValue
                opCaja.CodigoOperacionCaja = ""
                opCaja.Importe = Decimal.Parse(txtImporte.Text.Replace(".", UIUtility.DecimalSeparator()))
                opCaja.CodigoModelo = ddlModeloCarta.SelectedValue
                opCaja.NumeroCarta = IIf(Me.ddlContacto.SelectedIndex = 0, "", Me.ddlContacto.SelectedValue) 'Pasa el Contacto en vez que el numero de la carta 21082008
                opCaja.FechaPago = ConvertirFechaaDecimal(txtFecha.Text)

                'OT12012
                opCaja.ObservacionCartaDestino = txtObservacionCarta.Text

                dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                Dim TranFictizia As String
                If ddlNroCuentaDestino.SelectedValue = "Matriz" Then
                    TranFictizia = "S"
                Else
                    TranFictizia = "N"
                End If
                Dim oOpCaja As New OperacionesCajaBM
                oOpCaja.InsertarTransferenciaInterna(dsOpCaja, ddltipoTraspaso.SelectedValue, TranFictizia, DatosRequest)
                AlertaJS(ObtenerMensaje("ALERT12"))
                LimpiarFormulario()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Function validarTipoOperacion(ByRef msjValidacion As String) As Boolean
        Dim strOrigen As String
        Dim dtOrigen As DataTable
        Dim strDestino As String
        Dim dtDestino As DataTable
        Dim oTercerosBM As New TercerosBM
        Dim intcontLocal As Integer = 0
        Dim intContExtranjero As Integer = 0

        dtOrigen = oTercerosBM.SeleccionarPorFiltro(ddlBanco.SelectedValue.ToString(), "", "", "", "", "", DatosRequest).Tables(0)
        dtDestino = oTercerosBM.SeleccionarPorFiltro(ddlBancoDestino.SelectedValue, "", "", "", "", "", DatosRequest).Tables(0)
        strOrigen = dtOrigen(0)("CodigoPais").ToString()
        strDestino = dtDestino(0)("CodigoPais").ToString()

        If (strOrigen = "604") Then
            intcontLocal = intcontLocal + 1
        Else
            intContExtranjero = intContExtranjero + 1
        End If

        If (strDestino = "604") Then
            intcontLocal = intcontLocal + 1
        Else
            intContExtranjero = intContExtranjero + 1
        End If

        If (ddltipoTraspaso.SelectedValue = "63") Then

            If (intContExtranjero >= 1 And intcontLocal <= 1) Then
                msjValidacion = ""
            Else
                msjValidacion = "Debe seleccionar 1 o menos entidades financieras nacionales"
            End If

        ElseIf (ddltipoTraspaso.SelectedValue = "63") Then

            If (intContExtranjero = 0 And intcontLocal = 2) Then
                msjValidacion = ""
            Else
                msjValidacion = "Debe seleccionar 2 entidades financieras nacionales"
            End If
        Else
            msjValidacion = ""
        End If


        If (String.IsNullOrEmpty(msjValidacion)) Then
            Return True
        Else
            Return False
        End If

    End Function
    Private Sub BindingModeloCarta(ByVal codigoOperacion As String) 'OT12012
        Dim operacion As New OperacionBM
        Try
            up_transferencia.Update()
            If codigoOperacion = "BCRI" Then
                codigoOperacion = "BCRE"
            End If
            Dim dt As DataTable = operacion.ListaModeloCartaOperacion(codigoOperacion, "")
            HelpCombo.LlenarComboBox(ddlModeloCarta, dt, "CodigoModelo", "Descripcion", False)
            ddlModeloCarta.Items.Insert(0, New ListItem("SIN CARTA", "SC01"))
            ddlModeloCarta.SelectedValue = "SC01"
            If ddlModeloCarta.Items.Count > 1 Then
                ddlModeloCarta.Items(0).Selected = False
                ddlModeloCarta.Items(1).Selected = True
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub LimpiarFormulario()
        MostrarSaldos()
        ddlBancoDestino.SelectedIndex = 0
        ddlClaseCuentaDestino.SelectedIndex = 0
        ddlNroCuentaDestino.SelectedIndex = 0
        Me.txtImporte.Text = ""
        lblSaldoDisponible1Destino.Text = ""
        lblSaldoDisponible2Destino.Text = ""
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        CargarDetPortafolioOrigen(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, ddlMercado.SelectedValue, True)
        Me.CargarBanco(ddlMercado.SelectedValue, True)
        MostrarSaldos()
        ModeloCartaSeleccionar()
    End Sub
    Private Sub ddlClaseCuenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClaseCuenta.SelectedIndexChanged
        CargarDetPortafolioOrigen(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, ddlMercado.SelectedValue, True)
        MostrarSaldos()
        Verificar_Traspaso()
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        txtFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
        ValidarDatosPortafolioOrigen()
        Me.CargarBanco(ddlMercado.SelectedValue, True)
        EstablecerFecha()
        MostrarSaldos()
        Verificar_Traspaso()
        ddlPortafolioDestino.SelectedValue = ddlPortafolio.SelectedValue
        ValidarDatosPortafolioDestino()
        Me.CargarBancoDestino(ddlMercadoDestino.SelectedValue, True)
        Verificar_Traspaso()
        MostrarSaldos2()
    End Sub
    Private Sub ddlPortafolioDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolioDestino.SelectedIndexChanged
        ValidarDatosPortafolioDestino()
        Me.CargarBancoDestino(ddlMercadoDestino.SelectedValue, True)
        Verificar_Traspaso()
        MostrarSaldos2()
    End Sub
    Private Sub ddlClaseCuentaDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClaseCuentaDestino.SelectedIndexChanged
        CargarDetPortafolioDestino(ddlBancoDestino.SelectedValue, ddlClaseCuentaDestino.SelectedValue, ddlPortafolioDestino.SelectedValue, ddlMoneda.SelectedValue, ddlMercadoDestino.SelectedValue, True)
        Verificar_Traspaso()
        MostrarSaldos2()
    End Sub
    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarDetPortafolioOrigen(ddlBanco.SelectedValue, ddlClaseCuenta.SelectedValue, ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, ddlMercado.SelectedValue, True)
        CargarContactos()
        MostrarSaldos()
        ModeloCartaSeleccionar()
    End Sub
    Private Sub ddlMercadoDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercadoDestino.SelectedIndexChanged
        Me.CargarBancoDestino(ddlMercadoDestino.SelectedValue, True)
        CargarDetPortafolioDestino(ddlBancoDestino.SelectedValue, ddlClaseCuentaDestino.SelectedValue, ddlPortafolioDestino.SelectedValue, ddlMoneda.SelectedValue, ddlMercadoDestino.SelectedValue, True)
        ModeloCartaSeleccionar()
        MostrarSaldos2()
    End Sub
    Private Sub ddlBancoDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBancoDestino.SelectedIndexChanged
        ddlClaseCuentaDestino.SelectedValue = "20"
        CargarDetPortafolioDestino(ddlBancoDestino.SelectedValue, ddlClaseCuentaDestino.SelectedValue, ddlPortafolioDestino.SelectedValue, ddlMoneda.SelectedValue, ddlMercadoDestino.SelectedValue, True)
        ModeloCartaSeleccionar()
        MostrarSaldos2()
    End Sub
    Private Sub ddlNroCuenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlNroCuenta.SelectedIndexChanged
        MostrarSaldos()
    End Sub
#End Region
#Region "CargarDatos"
    Private Sub CargarCombos()
        CargarMercado()
        CargarBanco("")
        CargarClaseCuenta()
        CargarMercadoDestino()
        CargarBancoDestino("")
        CargarClaseCuentaDestino()
    End Sub
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dt As DataTable = oMercado.ListarActivos(DatosRequest, ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMercado, dt, "CodigoMercado", "Descripcion", True)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
    End Sub
    Private Sub CargarMercadoDestino(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dt As DataTable = oMercado.ListarActivos(DatosRequest, ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMercadoDestino, dt, "CodigoMercado", "Descripcion", True)
        Else
            ddlMercadoDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercadoDestino)
        End If
        ddlMercadoDestino.Enabled = enabled
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", False)
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dt As DataTable = oClaseCuenta.Listar().Tables(0)
            HelpCombo.LlenarComboBox(ddlClaseCuenta, dt, "CodigoClaseCuenta", "Descripcion", True, "SELECCIONE")
            ddlClaseCuenta.SelectedValue = "20"
        Else
            ddlClaseCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuenta)
        End If
        ddlClaseCuenta.Enabled = enabled
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dt As DataTable = oMoneda.Listar(ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMoneda, dt, "CodigoMoneda", "Descripcion", True, "SELECCIONE")
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub
    Private Sub CargarDetPortafolioOrigen(ByVal codTercero As String, ByVal codClaseCuenta As String, ByVal codPortafolio As String, ByVal codMoneda As String, ByVal codMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dt As DataTable = oCuentaEconomica.SeleccionarPorFiltro(codPortafolio, codClaseCuenta, codTercero, codMoneda, codMercado, DatosRequest).Tables(1)
            HelpCombo.LlenarComboBox(ddlNroCuenta, dt, "NumeroCuenta", "NumeroCuenta", True, "SELECCIONE")
        Else
            ddlNroCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        End If
        If (ddlNroCuenta.Items.Count >= 2) Then
            ddlNroCuenta.SelectedIndex = 1
        End If
        ddlNroCuenta.Enabled = enabled
    End Sub
    Private Sub CargarDetPortafolioDestino(ByVal codTercero As String, ByVal codClaseCuenta As String, ByVal codPortafolio As String, ByVal codMoneda As String, ByVal codMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dt As DataTable
            If ddlMoneda.SelectedValue = "CAD" Then
                dt = oCuentaEconomica.SeleccionarPorFiltro_CAD(codPortafolio, codClaseCuenta, codTercero, codMercado).Tables(1)
            Else
                dt = oCuentaEconomica.SeleccionarPorFiltro(codPortafolio, codClaseCuenta, codTercero, codMoneda, codMercado, DatosRequest).Tables(1)
            End If
            HelpCombo.LlenarComboBox(ddlNroCuentaDestino, dt, "NumeroCuenta", "NumeroCuenta", True, "SELECCIONE")
            If ddlNroCuentaDestino.Items.Count = 1 And ddlBancoDestino.SelectedValue <> "" Then
                ddlNroCuentaDestino.Items(0).Text = "Cuenta Matriz"
                ddlNroCuentaDestino.Items(0).Value = "Matriz"
            End If
        Else
            ddlNroCuentaDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuentaDestino)
        End If
        If (ddlNroCuentaDestino.Items.Count = 2) Then
            ddlNroCuentaDestino.SelectedIndex = 1
        End If
        ddlNroCuentaDestino.Enabled = enabled
    End Sub
    Private Sub CargarClaseCuentaDestino(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dt As DataTable = oClaseCuenta.Listar().Tables(0)
            HelpCombo.LlenarComboBox(ddlClaseCuentaDestino, dt, "CodigoClaseCuenta", "Descripcion", True, "SELECCIONE")
        Else
            ddlClaseCuentaDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClaseCuentaDestino)
        End If
        ddlClaseCuentaDestino.Enabled = enabled
    End Sub
    Private Sub CargarPortafolioDestino(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            HelpCombo.LlenarComboBox(ddlPortafolioDestino, dt, "CodigoPortafolio", "Descripcion", False)
        Else
            ddlPortafolioDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolioDestino)
        End If
    End Sub
    Private Sub CargarBanco(ByVal codigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dt As DataTable = oBanco.SeleccionarBancoPorCodigoMercadoYPortafolio(codigoMercado, Me.ddlPortafolio.SelectedValue).Tables(0)
            HelpCombo.LlenarComboBox(ddlBanco, dt, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
    End Sub
    Private Sub CargarBancoDestino(ByVal codigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            ddlBancoDestino.Items.Clear()
            UIUtility.CargarIntermediariosOISoloBancos(ddlBancoDestino)
        Else
            ddlBancoDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBancoDestino)
        End If
        ddlBancoDestino.Enabled = enabled
    End Sub
#End Region
#Region "Metodos de Control"
    Private Sub ValidarDatosPortafolioOrigen()
        CargarDetPortafolioOrigen("", "", ddlPortafolio.SelectedValue, ddlMoneda.SelectedValue, ddlMercado.SelectedValue)
    End Sub
    Private Sub ValidarDatosPortafolioDestino()
        CargarDetPortafolioDestino("", "", ddlPortafolioDestino.SelectedValue, ddlMoneda.SelectedValue, ddlMercadoDestino.SelectedValue)
    End Sub
    Private Sub EstablecerFecha()
        txtFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
    End Sub
#End Region
    Private Sub MostrarSaldos()
        Dim sCodigoPortafolio As String = ddlPortafolio.SelectedValue
        Dim sNumeroCuenta As String = IIf(ddlNroCuenta.SelectedIndex = 0, "", ddlNroCuenta.SelectedItem.Text.Trim)
        Dim sBanco As String = ddlBanco.SelectedValue
        Dim sBancoNombre As String = ddlBanco.SelectedItem.Text.Trim
        Dim sFecha As String = Me.txtFecha.Text.Trim
        Dim strMensaje As String
        Dim strMensaje2 As String
        If sCodigoPortafolio.Trim = "" Then
            Me.lblSaldoDisponible.Text = ""
            Me.lblSaldoDisponible2.Text = ""
            Exit Sub
        ElseIf sNumeroCuenta.Trim = "" Then
            Me.lblSaldoDisponible.Text = ""
            Me.lblSaldoDisponible2.Text = ""
            Exit Sub
        ElseIf sBanco.Trim = "" Then
            Me.lblSaldoDisponible.Text = ""
            Me.lblSaldoDisponible2.Text = ""
            Exit Sub
        End If
        Dim oSaldoBancario As New SaldosBancariosBM
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        roCuentaEconomica.CodigoPortafolioSBS = sCodigoPortafolio
        roCuentaEconomica.NumeroCuenta = sNumeroCuenta
        roCuentaEconomica.CodigoMoneda = ""
        roCuentaEconomica.CodigoClaseCuenta = ""
        roCuentaEconomica.CodigoTercero = ddlBanco.SelectedValue
        roCuentaEconomica.FechaCreacion = 0
        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        Dim oSBDS As DataSet = oSaldoBancario.SeleccionarPorFiltro3(dsCuentaEconomica, DatosRequest)
        If oSBDS.Tables(0).Rows.Count <= 0 Then
            Me.lblSaldoDisponible.Text = ""
            Me.lblSaldoDisponible2.Text = ""
            Exit Sub
        End If
        If oSBDS.Tables(0).Rows.Count > 0 Then
            Dim sSaldoContable As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoContable")
            Dim sSaldoDisponible As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoDisponible")
            Dim sClaseCuenta As String = oSBDS.Tables(0).Rows(0).Item("ClaseDescripcion")
            Dim sMoneda As String = oSBDS.Tables(0).Rows(0).Item("DescripcionMoneda")
            strMensaje = sBancoNombre & " - " & "Cuenta: " & sNumeroCuenta & " - " & " Moneda:" & sMoneda
            strMensaje2 = "Saldo Contable: " & String.Format("{0:#,##0.00}", sSaldoContable) & " - " & "Saldo Disponible: " & String.Format("{0:#,##0.00}", sSaldoDisponible)
            Me.lblSaldoDisponible.Text = strMensaje
            Me.lblSaldoDisponible2.Text = strMensaje2
        End If
    End Sub
    Private Sub MostrarSaldos2()
        Dim sCodigoPortafolio As String = Me.ddlPortafolioDestino.SelectedValue
        Dim sNumeroCuenta As String = IIf(Me.ddlNroCuentaDestino.SelectedIndex = 0, "", ddlNroCuentaDestino.SelectedItem.Text.Trim)
        Dim sBanco As String = Me.ddlBancoDestino.SelectedValue
        Dim sBancoNombre As String = ddlBancoDestino.SelectedItem.Text.Trim
        Dim sFecha As String = Me.txtFecha.Text.Trim
        Dim strMensaje As String
        Dim strMensaje2 As String
        If sCodigoPortafolio.Trim = "" Then
            Me.lblSaldoDisponible1Destino.Text = ""
            Me.lblSaldoDisponible2Destino.Text = ""
            Exit Sub
        ElseIf sNumeroCuenta.Trim = "" Then
            Me.lblSaldoDisponible1Destino.Text = ""
            Me.lblSaldoDisponible2Destino.Text = ""
            Exit Sub
        ElseIf sBanco.Trim = "" Then
            Me.lblSaldoDisponible1Destino.Text = ""
            Me.lblSaldoDisponible2Destino.Text = ""
            Exit Sub
        End If
        Dim oSaldoBancario As New SaldosBancariosBM
        Dim dsCuentaEconomica As New CuentaEconomicaBE
        Dim roCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.NewCuentaEconomicaRow
        roCuentaEconomica.CodigoPortafolioSBS = sCodigoPortafolio
        roCuentaEconomica.NumeroCuenta = sNumeroCuenta
        roCuentaEconomica.CodigoMoneda = ""
        roCuentaEconomica.CodigoClaseCuenta = ""
        roCuentaEconomica.CodigoTercero = ddlBancoDestino.SelectedValue
        roCuentaEconomica.FechaCreacion = 0
        dsCuentaEconomica.CuentaEconomica.AddCuentaEconomicaRow(roCuentaEconomica)
        Dim oSBDS As DataSet = oSaldoBancario.SeleccionarPorFiltro3(dsCuentaEconomica, DatosRequest)
        If oSBDS.Tables(0).Rows.Count <= 0 Then
            Me.lblSaldoDisponible1Destino.Text = ""
            Me.lblSaldoDisponible2Destino.Text = ""
            Exit Sub
        End If
        If oSBDS.Tables(0).Rows.Count > 0 Then
            Dim sSaldoContable As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoContable")
            Dim sSaldoDisponible As Decimal = oSBDS.Tables(0).Rows(0).Item("SaldoDisponible")
            Dim sClaseCuenta As String = oSBDS.Tables(0).Rows(0).Item("ClaseDescripcion")
            Dim sMoneda As String = oSBDS.Tables(0).Rows(0).Item("DescripcionMoneda")
            strMensaje = sBancoNombre & " - " & "Cuenta: " & sNumeroCuenta & " - " & " Moneda:" & sMoneda
            strMensaje2 = "Saldo Contable: " & String.Format("{0:#,##0.00}", sSaldoContable) & " - " & "Saldo Disponible: " & String.Format("{0:#,##0.00}", sSaldoDisponible)
            Me.lblSaldoDisponible1Destino.Text = strMensaje
            Me.lblSaldoDisponible2Destino.Text = strMensaje2
        End If
    End Sub
    Private Sub CargarContactos()
        Dim objContacto As New ContactoBM
        Dim dt As DataTable = objContacto.ListarContactoPorTercerosTesoreria(ddlBanco.SelectedValue).Tables(0)
        HelpCombo.LlenarComboBox(ddlContacto, dt, "CodigoContacto", "DescripcionContacto", True, "SELECCIONE")
    End Sub
    Private Function Distinct(ByVal dt As DataTable, ByVal columName As String) As DataTable
        Dim dr As DataRow
        Dim value As String
        Dim dtResult As DataTable
        dtResult = dt.Clone()
        If (dt.Rows.Count > 0) Then
            value = Convert.ToString(dt.Rows(0)(columName))
            dtResult.LoadDataRow(dt.Rows(0).ItemArray(), True)
            For Each dr In dt.Rows
                If String.Equals(value, Convert.ToString(dr(columName))) = False Then
                    value = Convert.ToString(dr(columName))
                    dtResult.LoadDataRow(dr.ItemArray(), True)
                End If
            Next
        End If
        Return dtResult
    End Function
    Private Sub Verificar_Traspaso()
        Dim oOpCaja As New OperacionesCajaBM
        If oOpCaja.ValidarTransferenciaClaseCuenta(ddlPortafolio.SelectedValue, ddlPortafolioDestino.SelectedValue, ddlNroCuenta.SelectedValue, ddlNroCuentaDestino.SelectedValue) > 0 Then
            Me.hdTransferenciaValida.Value = "SI"
        Else
            Me.hdTransferenciaValida.Value = "NO"
        End If
    End Sub
    Public Sub ModeloCartaSeleccionar()
        Dim cadBusqueda As String
        Dim indBusqueda As Integer
        Dim i As Integer
        If (Me.ddlMercado.SelectedValue <> Me.ddlMercadoDestino.SelectedValue) Then
            For i = 0 To Me.ddlModeloCarta.Items.Count - 1
                cadBusqueda = ddlModeloCarta.Items(i).Text()
                indBusqueda = cadBusqueda.IndexOf("EXTERIOR DE")
                If indBusqueda > 0 Then
                    ddlModeloCarta.SelectedIndex = i
                    Exit For
                End If
            Next
            Exit Sub
        End If
        If (Me.ddlBanco.SelectedIndex <> 0 And Me.ddlBancoDestino.SelectedIndex <> 0) Then
            If (Me.ddlBanco.SelectedValue = Me.ddlBancoDestino.SelectedValue) Then

                For i = 0 To Me.ddlModeloCarta.Items.Count - 1
                    cadBusqueda = ddlModeloCarta.Items(i).Text()
                    indBusqueda = cadBusqueda.IndexOf("INTERNA DE")
                    If indBusqueda > 0 Then
                        ddlModeloCarta.SelectedIndex = i
                        Exit For
                    End If
                Next
                Exit Sub

            End If
        Else
            For i = 0 To Me.ddlModeloCarta.Items.Count - 1
                cadBusqueda = ddlModeloCarta.Items(i).Text()
                indBusqueda = cadBusqueda.IndexOf("BCR DE")
                If indBusqueda > 0 Then
                    ddlModeloCarta.SelectedIndex = i
                    Exit For
                End If
            Next
            Exit Sub
        End If
    End Sub
    Private Sub ddlNroCuentaDestino_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNroCuentaDestino.SelectedIndexChanged
        MostrarSaldos2()
    End Sub
    Protected Sub ddltipoTraspaso_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddltipoTraspaso.SelectedIndexChanged
        BindingModeloCarta(ddltipoTraspaso.SelectedValue) 'OT12012
    End Sub
End Class