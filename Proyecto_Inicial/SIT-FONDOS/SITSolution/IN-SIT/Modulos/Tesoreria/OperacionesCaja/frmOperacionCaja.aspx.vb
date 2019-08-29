Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Tesoreria_OperacionesCaja_frmOperacionCaja
    Inherits BasePage
    Private codigoMercado As String = ""
    Private codigoPortafolio As String = ""
    Private codigoMoneda As String = ""
    Private descripcionMoneda As String = ""
#Region "Eventos de la Pagina"
    Sub SelTipoPago()
        If ddlTipoOperacion.SelectedValue = "1" Then
            ddlTipoPago.SelectedValue = "CPAG"
        ElseIf ddlTipoOperacion.SelectedValue = "2" Then
            ddlTipoPago.SelectedValue = "CCOB"
        End If
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se inicializa campo oculto para respuesta de confirmación | 22/06/18 
                hdRpta.Value = "NO"
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se inicializa campo oculto para respuesta de confirmación | 22/06/18 
                pnlDatosEgreso.Attributes.Add("class", "hidden")
                ddlTipoPago.Enabled = False
                CargarPortafolio(True)
                CargarMercado(True)
                CargarBanco(ddlMercado.SelectedValue, True)
                ddlMoneda.Items.Add("--Seleccione Banco--")
                'CargarMoneda(True)
                CargarTipoOperacion()
                ValidarDatosPortafolioyMercado()
                EstablecerFecha()
                SelTipoPago()
                CargarOperacion()
                CargarTipoPago()
            End If
            codigoPortafolio = ddlPortafolio.SelectedValue
            codigoMercado = ddlMercado.SelectedValue
            btnAceptar.Attributes.Add("onclick", "return ValidarCuentas();")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Evitar doble Postback | 22/06/18 
            If hdRpta.Value.ToUpper = "SI" Then

                'INICIO | ZOLUXIONES | RCE | OT12003 | 15/05/19 
                'Validacion Fecha Caja con clase Inversion para agregar movimientos
                Dim FechaPortafolioCaja As Decimal = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClase.SelectedValue)

                If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) <> FechaPortafolioCaja And ddlClase.SelectedValue = "10" Then
                    AlertaJS("Solo se puede modificar en la fecha actual del portafolio.")
                    Exit Sub
                End If

                If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) <> FechaPortafolioCaja And ddlClase.SelectedValue = "20" Then
                    AlertaJS("Solo se puede modificar, agregar o eliminar operaciones en la fecha de Caja asignada. Fecha de Caja : " + UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue, ddlClase.SelectedValue)))
                    lblFechaPago.Text = UIUtility.ConvertirFechaaString(FechaPortafolioCaja)
                    Exit Sub
                End If

                'Validaicon Fecha de Valoracion
                Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text))
                If cantidadreg > 0 Then
                    AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                    lblFechaPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                    Exit Sub
                End If
                'FIN  | ZOLUXIONES | RCE | OT12003 | 15/05/19 


                EjecutarJS("document.getElementById('hdRpta').value = 'NO'")
                'FIN  | ZOLUXIONES | RCE | ProyFondosII - RF019 - Evitar doble Postback | 22/06/18 
                Dim dsOpCaja As New OperacionCajaBE
                Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                If ddlOperacion.SelectedValue = "" Then
                    AlertaJS("Seleccione la operación correspondiente.", "ddlOperacion.focus();")
                    Exit Sub
                End If
                If ddlMoneda.SelectedValue = "" Then
                    AlertaJS("Seleccione la moneda correspondiente.", "ddlMoneda.focus();")
                    Exit Sub
                End If
                If ddlNroCuenta.SelectedValue = "" Then
                    AlertaJS("Seleccione el numero de cuenta correspondiente.", "ddlNroCuenta.focus();")
                    Exit Sub
                End If
                If ddlTipoPago.SelectedValue = "" Then
                    AlertaJS("Seleccione el tipo de pago correspondiente.", "ddlTipoPago.focus();")
                    Exit Sub
                End If
                If opcIntermediario.Checked Then
                    EjecutarJS("SeleccionarOpcion('" & ddlIntermediario.ClientID & "','" & ddlTercero.ClientID & "')")
                    Exit Sub
                ElseIf opcTercero.Checked Then
                    EjecutarJS("SeleccionarOpcion('" & ddlTercero.ClientID & "','" & ddlIntermediario.ClientID & "')")
                    Exit Sub
                End If

                opCaja.CodigoMercado = ddlMercado.SelectedValue
                opCaja.CodigoPortafolioSBS = ddlPortafolio.SelectedValue
                opCaja.CodigoClaseCuenta = ddlClase.SelectedValue
                opCaja.NumeroCuenta = ddlNroCuenta.SelectedValue
                opCaja.CodigoModalidadPago = ddlTipoPago.SelectedValue
                opCaja.CodigoTerceroOrigen = ddlBanco.SelectedValue
                If opcTercero.Checked = True Then
                    opCaja.CodigoTerceroDestino = ddlTercero.SelectedValue
                ElseIf opcIntermediario.Checked = True Then
                    opCaja.CodigoTerceroDestino = ddlIntermediario.SelectedValue
                ElseIf opcTercero.Checked = False And opcIntermediario.Checked = False Then
                    opCaja.CodigoTerceroDestino = ""
                End If
                opCaja.NumeroCuentaDestino = ddlNumeroCuentaDestino.SelectedValue
                opCaja.CodigoOperacion = ddlOperacion.SelectedValue
                opCaja.Referencia = txtReferencia.Text
                opCaja.CodigoMoneda = hdCodigoMoneda.Value
                opCaja.Importe = Replace(IIf(txtImporte.Text.Trim = "", "0", txtImporte.Text.Trim), ",", "")
                opCaja.CodigoModelo = ddlModeloCarta.SelectedValue
                opCaja.CodigoOperacionCaja = ""
                opCaja.FechaPago = UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text)
                dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)

                Dim ejecutar As Integer
                ejecutar = InsertarOperacionCaja_FechaOperacion(dsOpCaja)
                If ejecutar <> 0 Then
                    AlertaJS("Hubo un Error en el guardado del movimiento de caja / Code Error: " + ejecutar.ToString)
                    Exit Sub
                Else
                    AlertaJS("Se guardó correctamente el movimiento de caja .")
                End If
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Validar si viene con el nuevo concepto para otras Cxc | 22/06/18 
                If ddlOperacion.SelectedValue = "OP0089" And ddlPortafolio.SelectedItem.ToString.Contains("CAPESTR") And ddlClase.SelectedValue = "20" And ddlTipoPago.SelectedValue = "TRNS" And ddlTipoOperacion.SelectedValue = "1" Then
                    AlertaJS(ObtenerMensaje("ALERT175") + ddlPortafolio.Items(ddlPortafolio.SelectedIndex).Text)
                Else
                    AlertaJS(ObtenerMensaje("ALERT12"))
                End If
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Validar si viene con el nuevo concepto para otras Cxc | 22/06/18 
                LimpiarFormulario()
            End If

        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Sub LimpiarFormulario()

        ddlPortafolio.SelectedIndex = 0
        ddlMercado.SelectedIndex = 0
        txtImporte.Text = ""
        txtReferencia.Text = ""
        ddlBancoDestino.SelectedIndex = 0
        opcIntermediario.Checked = False
        opcTercero.Checked = False
    End Sub
    Private Sub ddlTipoOperacion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoOperacion.SelectedIndexChanged
        ValidarDatosPortafolioyMercado()
        CargarOperacion()
        SelTipoPago()
    End Sub
    Private Sub ddlNroCuenta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlNroCuenta.SelectedIndexChanged
        If ddlNroCuenta.SelectedIndex <> 0 Then
            ObtenerMoneda(ddlNroCuenta.SelectedItem.Text)
            ValidarDatosEgreso()
        End If
        Me.ddlBancoDestino.SelectedIndex = 0
    End Sub
    Private Sub ddlMercado_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMercado.SelectedIndexChanged
        codigoMercado = ddlMercado.SelectedValue
        codigoPortafolio = ddlPortafolio.SelectedValue
        ValidarDatosPortafolioyMercado()
        CargaMonedaBanco()
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        codigoMercado = ddlMercado.SelectedValue
        codigoPortafolio = ddlPortafolio.SelectedValue
        EstablecerFecha()
    End Sub
    Private Sub ddlClase_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlClase.SelectedIndexChanged
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, codigoPortafolio, True)
        CargarOperacion(True)
    End Sub
    Sub CargaMonedaBanco()
        If Not ddlBanco.SelectedValue = "" Then
            Dim oOperaCaja As New OperacionesCajaBM
            HelpCombo.LlenarComboBox(ddlMoneda, oOperaCaja.Listar_MonedaBanco(ddlPortafolio.SelectedValue, ddlBanco.SelectedValue), "CodigoMoneda", "Descripcion", True)
        End If
    End Sub
    Private Sub ddlBanco_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBanco.SelectedIndexChanged
        CargarClaseCuenta(True)
        CargaMonedaBanco()
        CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, codigoPortafolio, True)
    End Sub
    Private Sub ddlIntermediario_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlIntermediario.SelectedIndexChanged
        Me.ddlTercero.SelectedIndex = 0
        Me.ddlTercero.Enabled = False
        Me.ddlNumeroCuentaDestino.Enabled = False
        If ddlIntermediario.SelectedIndex <> 0 Then
            Dim oTerceros As New TercerosBM
            Dim dt As DataTable = oTerceros.SeleccionarBanco(ddlIntermediario.SelectedValue, hdCodigoMoneda.Value).Tables(0)
            HelpCombo.LlenarComboBox(Me.ddlBancoDestino, dt, "EntidadFinanciera", "Descripcion", True)
            Me.ddlBancoDestino.Enabled = True
            Me.ddlBancoDestino.SelectedIndex = 0
        Else
            ddlBancoDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBancoDestino)
        End If
    End Sub
    Private Sub ddlTercero_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTercero.SelectedIndexChanged
        Me.ddlIntermediario.SelectedIndex = 0
        Me.ddlNumeroCuentaDestino.Enabled = False
        If ddlTercero.SelectedIndex <> 0 Then
            Dim oTerceros As New TercerosBM
            Dim dt As DataTable = oTerceros.SeleccionarBanco(ddlTercero.SelectedValue, hdCodigoMoneda.Value).Tables(0)
            HelpCombo.LlenarComboBox(ddlBancoDestino, dt, "EntidadFinanciera", "Descripcion", True)
            Me.ddlBancoDestino.Enabled = True
        Else
            ddlBancoDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBancoDestino)
        End If
    End Sub
#End Region
#Region "Metodos de Control"
    Private Sub InsertarOperacionCaja(ByVal dsOperacionesCaja As OperacionCajaBE)
        Dim oOperacionCaja As New OperacionesCajaBM
        oOperacionCaja.Insertar(dsOperacionesCaja, DatosRequest)
    End Sub
    Private Function InsertarOperacionCaja_FechaOperacion(ByVal dsOperacionesCaja As OperacionCajaBE) As Integer
        Dim oOperacionCaja As New OperacionesCajaBM
        Return oOperacionCaja.Insertar_FechaOperacion(dsOperacionesCaja, DatosRequest)
    End Function
    Private Sub ValidarDatosEgreso()
        CargarIntermediario(codigoMercado, (ddlNroCuenta.SelectedIndex <> 0))
        CargarTercero(codigoMercado, (ddlNroCuenta.SelectedIndex <> 0))
        CargarNumeroCuentaDestino("", codigoMercado, (ddlNroCuenta.SelectedIndex <> 0))
    End Sub
    Private Sub EstablecerFecha()
        lblFechaPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))        
    End Sub
    Private Sub ValidarDatosPortafolioyMercado()
        CargarClaseCuenta()
        CargarBanco(codigoMercado)
        CargaMonedaBanco()
        CargarDetallePortafolio("", "", ddlPortafolio.SelectedValue)
        CargarIntermediario(codigoMercado)
        CargarTercero(codigoMercado)
        CargarNumeroCuentaDestino("", "", codigoMercado)
        CargarBancoDestino("", codigoMercado)
        If ObtenerTipoOperacionEgreso(ddlTipoOperacion.SelectedValue) = "S" Then
            pnlDatosEgreso.Attributes.Remove("class")
        Else
            pnlDatosEgreso.Attributes.Add("class", "hidden")
        End If
        ValidarDatosEgreso()
    End Sub
    Private Sub CambiarEstadoDDL(ByVal sel As DropDownList, ByVal enabled As Boolean)
        If Not enabled Then
            sel.SelectedIndex = 0
        End If
        sel.Enabled = enabled
    End Sub
    Private Sub InsertarElementoSeleccion(ByVal sel As DropDownList, Optional ByVal valor As String = "")
        sel.Items.Insert(0, New ListItem("--Seleccione--", valor))
    End Sub
#End Region
#Region "CargarDatos"
    Private Sub CargarMercado(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMercado As New MercadoBM
            Dim dt As DataTable = oMercado.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlMercado, dt, "CodigoMercado", "Descripcion", False)
        Else
            ddlMercado.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMercado)
        End If
        ddlMercado.Enabled = enabled
        If (ddlMercado.Items.Count > 1) Then
            ddlMercado.SelectedIndex = 1
        End If
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
    Private Sub CargarTipoOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim parametrosGenerales As New ParametrosGeneralesBM
            Dim dt As DataTable = parametrosGenerales.SeleccionarPorFiltro("OPC", String.Empty, String.Empty, String.Empty, Me.DatosRequest)
            HelpCombo.LlenarComboBox(ddlTipoOperacion, dt, "Valor", "Nombre", False)
        Else
            ddlTipoOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTipoOperacion)
        End If
        ddlTipoOperacion.Enabled = enabled
    End Sub
    Private Sub CargarOperacion(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oOperacion As New OperacionBM
            Dim dt As DataTable = oOperacion.SeleccionarPorCodigoTipoOperacion(ddlTipoOperacion.SelectedValue, "", ddlClase.SelectedValue).Tables(0)
            Dim dtfiltrado As DataTable = dt.Select("CODIGOOPERACION <> 'OP0092'", "Descripcion").CopyToDataTable
            HelpCombo.LlenarComboBox(ddlOperacion, dtfiltrado, "CodigoOperacion", "Descripcion", True, "SELECCIONE")
        Else
            ddlOperacion.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlOperacion)
        End If
        ddlOperacion.Enabled = enabled
    End Sub
    Private Sub CargarClaseCuenta(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oClaseCuenta As New ClaseCuentaBM
            Dim dt As DataTable = oClaseCuenta.Listar().Tables(0)
            HelpCombo.LlenarComboBox(ddlClase, dt, "CodigoClaseCuenta", "Descripcion", True, "SELECCIONE")
            ddlClase.SelectedValue = "20"
        Else
            ddlClase.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlClase)
        End If
        ddlClase.Enabled = enabled
    End Sub
    Private Sub CargarBanco(ByVal codMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(codMercado, ddlPortafolio.SelectedValue, "")
            Dim dtBancos As DataTable = Distinct(dsBanco.Tables(0), "Descripcion")
            Me.Session().Add("Bancos", dtBancos)
            HelpCombo.LlenarComboBox(ddlBanco, dtBancos, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlBanco.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBanco)
        End If
        ddlBanco.Enabled = enabled
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
    Private Sub CargarDetallePortafolio(ByVal codTercero As String, ByVal CodigoClaseCuenta As String, ByVal CodigoPortafolio As String, Optional ByVal enabled As Boolean = True)
        ddlNroCuenta.Enabled = enabled
        If enabled Then
            Dim oBanco As New TercerosBM
            Dim dtBancos As DataTable = oBanco.SeleccionarBancoPorMercPortMone(Me.ddlMercado.SelectedValue, CodigoPortafolio, ddlMoneda.SelectedValue).Tables(0)
            If ddlClase.SelectedValue = "" Then
                dtBancos.DefaultView.RowFilter = "CodigoTercero='" + ddlBanco.SelectedValue + "'"
            Else
                dtBancos.DefaultView.RowFilter = "CodigoClaseCuenta='" + ddlClase.SelectedValue + "' AND CodigoTercero='" + ddlBanco.SelectedValue + "'"
            End If
            dtBancos.DefaultView.RowStateFilter = DataViewRowState.CurrentRows
            HelpCombo.LlenarComboBox(ddlNroCuenta, dtBancos, "NumeroCuenta", "NumeroCuenta", True, "SELECCIONE")
        Else
            ddlNroCuenta.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNroCuenta)
        End If
        ValidarDatosEgreso()
        ObtenerMoneda(ddlNroCuenta.SelectedItem.Text)
    End Sub
    Private Sub ObtenerMoneda(ByVal numeroCuenta As String)
        If Not (numeroCuenta = "" Or numeroCuenta = "--SELECCIONE--") Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim row As DataRow = oCuentaEconomica.SeleccionarPorFiltro(codigoPortafolio, "", "", "", "", DatosRequest).Tables(1).Select("NumeroCuenta='" & numeroCuenta & "'")(0)
            codigoMoneda = row("CodigoMoneda")
            descripcionMoneda = ObtenerNombreMoneda(row("CodigoMoneda"))
            hdCodigoMoneda.Value = codigoMoneda
        End If
    End Sub
    Private Function ObtenerNombreMoneda(ByVal codigoMoneda As String) As String
        Dim dsMoneda As DataSet = New MonedaBM().Listar("")
        Return dsMoneda.Tables(0).Select("CodigoMoneda='" & codigoMoneda & "'")(0)("Descripcion")
    End Function
    Private Function ObtenerTipoOperacionEgreso(ByVal CodigoTipoOperacion As String) As String
        Dim dsTipoOperacion As TipoOperacionBE = New TipoOperacionBM().Listar("A")
        Try
            Return dsTipoOperacion.TipoOperacion.Select("CodigoTipoOperacion='" & CodigoTipoOperacion & "'")(0)("Egreso")
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Sub CargarIntermediario(ByVal CodigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oIntermediario As New TercerosBM
            Dim dt As DataTable = oIntermediario.SeleccionarPorFiltro(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "").Tables(0)
            HelpCombo.LlenarComboBox(ddlIntermediario, dt, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlIntermediario.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlIntermediario)
        End If
        ddlIntermediario.Enabled = enabled
        opcIntermediario.Enabled = enabled
    End Sub
    Private Sub CargarTercero(ByVal CodigoMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oTercero As New TercerosBM
            Dim dt As DataTable = oTercero.SeleccionarPorFiltro(ParametrosSIT.CLASIFICACIONTERCERO_TERCERO, "").Tables(0)
            HelpCombo.LlenarComboBox(ddlTercero, dt, "CodigoTercero", "Descripcion", True, "SELECCIONE")
        Else
            ddlTercero.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTercero)
        End If
        ddlTercero.Enabled = enabled
        opcTercero.Enabled = enabled
    End Sub
    Private Sub CargarBancoDestino(ByVal codTercero As String, ByVal codMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oTerceros As New TercerosBM
            Dim dt As DataTable = oTerceros.SeleccionarBanco(codTercero, codigoMoneda).Tables(0)
            HelpCombo.LlenarComboBox(ddlBancoDestino, dt, "EntidadFinanciera", "Descripcion", True, "SELECCIONE")
        Else
            ddlBancoDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlBancoDestino)
        End If
        ddlBancoDestino.Enabled = enabled
    End Sub
    Private Sub CargarNumeroCuentaDestino(ByVal codTercero As String, ByVal codBanco As String, ByVal codMercado As String, Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oCuentaEconomica As New CuentaEconomicaBM
            Dim dt As DataTable = oCuentaEconomica.SeleccionarPorCodigoTercero(codTercero, codBanco, codMercado, hdCodigoMoneda.Value).Tables(0)
            HelpCombo.LlenarComboBox(ddlNumeroCuentaDestino, dt, "NumeroCuenta", "NumeroCuenta", True, "SELECCIONE")
        Else
            ddlNumeroCuentaDestino.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlNumeroCuentaDestino)
        End If
        ddlNumeroCuentaDestino.Enabled = enabled
    End Sub

    Private Sub CargarTipoPago(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oModalidadPago As New ModalidadPagoBM
            Dim dt As DataTable = oModalidadPago.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO).Tables(0)
            HelpCombo.LlenarComboBox(ddlTipoPago, dt, "codigoModalidadPago", "Descripcion", True, "SELECCIONE")
        Else
            ddlTipoPago.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlTipoPago)
        End If
        ddlTipoPago.Enabled = enabled
    End Sub
#End Region
    Private Sub ddlBancoDestino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBancoDestino.SelectedIndexChanged
        If Me.ddlIntermediario.SelectedIndex <> 0 Then
            If ddlBancoDestino.SelectedIndex <> 0 Then
                Me.ddlNumeroCuentaDestino.Enabled = True
                CargarNumeroCuentaDestino(Me.ddlIntermediario.SelectedValue, Me.ddlBancoDestino.SelectedValue, codigoMercado, True)
            Else
                Me.ddlNumeroCuentaDestino.Items.Clear()
                UIUtility.InsertarElementoSeleccion(ddlNumeroCuentaDestino)
            End If
        Else
            If Me.ddlTercero.SelectedIndex <> 0 Then
                If ddlBancoDestino.SelectedIndex <> 0 Then
                    Me.ddlNumeroCuentaDestino.Enabled = True
                    CargarNumeroCuentaDestino(Me.ddlTercero.SelectedValue, Me.ddlBancoDestino.SelectedValue, codigoMercado, True)
                Else
                    Me.ddlNumeroCuentaDestino.Items.Clear()
                    UIUtility.InsertarElementoSeleccion(ddlNumeroCuentaDestino)
                End If
            End If
        End If
    End Sub
    Private Sub CargarMoneda(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oMoneda As New MonedaBM
            Dim dsMoneda As DataSet = oMoneda.GetMonedaMercadoPortafolio(ddlMercado.SelectedValue, ddlPortafolio.SelectedValue)
            ddlMoneda.Items.Clear()
            ddlMoneda.DataSource = dsMoneda
            ddlMoneda.DataValueField = "CodigoMoneda"
            ddlMoneda.DataTextField = "Descripcion"
            ddlMoneda.DataBind()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        Else
            ddlMoneda.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlMoneda)
        End If
        ddlMoneda.Enabled = enabled
    End Sub
    Private Sub BindingModeloCarta(ByVal codigoOperacion As String)
        Dim operacion As New OperacionBM
        Try
            ddlModeloCarta.DataTextField = "Descripcion"
            ddlModeloCarta.DataValueField = "CodigoModelo"
            ddlModeloCarta.DataSource = operacion.ListaModeloCartaOperacion(codigoOperacion, "")
            ddlModeloCarta.DataBind()
            ddlModeloCarta.Items.Insert(0, New ListItem("SIN CARTA", "SC01"))
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ddlOperacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperacion.SelectedIndexChanged
        BindingModeloCarta(ddlOperacion.SelectedValue)
    End Sub
    Private Sub ddlMoneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        Me.CargarClaseCuenta(True)
        Me.CargarDetallePortafolio(ddlBanco.SelectedValue, ddlClase.SelectedValue, ddlPortafolio.SelectedValue, True)
    End Sub
    Private Function fnFechaNueva(ByVal fechaAnt As String) As Date
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        Dim EsFeriado As Boolean
        fechaAnterior = Convert.ToDateTime(fechaAnt)
        FechaNueva = fechaAnterior.AddDays(1)
        Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
        If FechaNueva.DayOfWeek = DayOfWeek.Saturday Then
            While FechaNueva.DayOfWeek = DayOfWeek.Saturday
                FechaNueva = FechaNueva.AddDays(2)
                Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                While oFeriadoBM.BuscarPorFecha(Fecha, "A")
                    FechaNueva = FechaNueva.AddDays(1)
                    Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                End While
            End While
        Else
            EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, "A")
            If EsFeriado = True Then
                While oFeriadoBM.BuscarPorFecha(Fecha, "A")
                    FechaNueva = FechaNueva.AddDays(1)
                    Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                    While FechaNueva.DayOfWeek = DayOfWeek.Saturday
                        FechaNueva = FechaNueva.AddDays(2)
                        Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                    End While
                End While
            End If
        End If
        Return fechaNueva
    End Function
    Protected Sub lblFechaPago_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblFechaPago.TextChanged
        Try
            If lblFechaPago.Text = "" Then
                Exit Sub
            End If
            Dim FechaPortafolio As Date = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
            Dim FechaPago As Date = Date.Parse(lblFechaPago.Text)
            Dim fechaHabilSiguiente As Date = fnFechaNueva(FechaPortafolio.ToShortDateString)
            If FechaPago > FechaPortafolio Then
                If FechaPago <> FechaPortafolio And FechaPago <> fechaHabilSiguiente Then
                    lblFechaPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                    AlertaJS("La fecha ingresada no puede ser mayor a la fecha de apertura del portafolio o feriados.")
                End If
                'Else
                '    'Agregado JH - Validacion Fecha Caja con clase Inversion para agregar movimientos
                '    Dim FechaPortafolioCaja As Decimal = UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue)
                '    If UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text) <> FechaPortafolioCaja And ddlClase.SelectedValue = "20" Then
                '        AlertaJS("Solo se puede modificar, agregar o eliminar operaciones en la fecha de Caja asignada. Fecha de Caja : " + UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaCajaOperaciones(ddlPortafolio.SelectedValue)))
                '        lblFechaPago.Text = UIUtility.ConvertirFechaaString(FechaPortafolioCaja)
                '        Exit Sub
                '    End If

                '    Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFechaPago.Text))
                '    If cantidadreg > 0 Then
                '        AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                '        lblFechaPago.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlPortafolio.SelectedValue))
                '        Exit Sub
                '    End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
End Class