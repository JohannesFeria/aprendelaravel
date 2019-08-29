Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT
Imports System.Globalization

Partial Class Modulos_Parametria_frmAperturaPortafolio
    Inherits BasePage
    Dim oPortafolio As New PortafolioBM
    Dim oCustodioBM As New CustodioArchivoBM
    Dim DiaPosterior As String = ""

#Region "/* Métodos de la Página */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                lblFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(dlPortafolio.SelectedValue))
                ObtenerFechaActual(lblFecha.Text)
                Dim oParagen As New ParametrosGeneralesBM
                Dim dt As DataTable = oParagen.SeleccionarPorFiltro("DiaPos", "", "", "", DatosRequest)
                If dt.Rows.Count > 0 Then
                    DiaPosterior = dt.Rows(0)("Valor")
                    If DiaPosterior = "S" Then
                        tbFechaInicio.Enabled = True
                        EjecutarJS("$('#Div_fn').addClass('date');")
                    Else
                        tbFechaInicio.Enabled = False
                        EjecutarJS("$('#Div_fn').removeClass('date');")
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Página")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub
    Private Sub dlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dlPortafolio.SelectedIndexChanged
        Try
            lblFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(dlPortafolio.SelectedValue))
            ObtenerFechaActual(lblFecha.Text)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Private Function ValidarIndicadores() As Boolean
        Dim dt As DataTable = New PortafolioBM().ValidaIndicadores(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
        If dt.Rows.Count = 0 Then
            Return True
        Else
            Dim strMensaje As New StringBuilder
            strMensaje.Append("<table style=""width: 100%; text-align: left; font-weight: normal;"">")
            strMensaje.Append("<tr><td style=""width: 100%;"">Existen indicadores vencidos :</td></tr>")
            strMensaje.Append("</table>")
            strMensaje.Append("<hr>")
            strMensaje.Append("<table style=""width: 100%; text-align: left; font-weight: normal;"" border=1>")
            'OT10902 - 23/10/2017 - Jorge Benitez
            'Mostrar mensaje de validación de tasas vencidas
            strMensaje.Append("<tr><td width=25%><strong>Código Indicador</strong></td><td width=25%><strong>Código Nemónico</strong></td><td width=25%><strong>Código ISIN</strong></td><td width=25%><strong>Fecha Vencimiento<strong></td></tr>")
            For Each dr As DataRow In dt.Rows
                strMensaje.Append("<tr><td>" + dr(0).ToString + "</td><td>" + dr(1).ToString + "</td><td>" + dr(2).ToString + "</td><td>" + dr(3).ToString + "</td></tr>")
            Next
            strMensaje.Append("</table>")
            AlertaJS(strMensaje.ToString())
            Return False
        End If
    End Function
    Private Function GeneraSaldosCarteraTitulo() As Boolean
        Dim oInfCarteraTitulo As New CarteraBM
        Dim strNewFechaOperacion As String
        Dim strOldFechaOperacion As String
        strNewFechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        strOldFechaOperacion = UIUtility.ConvertirFechaaDecimal(lblFecha.Text)
        If oInfCarteraTitulo.GeneraSaldosCarteraTitulo(strOldFechaOperacion, strNewFechaOperacion, dlPortafolio.SelectedValue.Trim, DatosRequest) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim Mensaje As String = ""
            Mensaje = VerificarFecha(tbFechaInicio.Text.Trim(), VALIDAFERIADO)

            If chkreproceso.Checked Then
                Dim fecini As Decimal = UIUtility.ConvertirFechaaDecimal(lblFecha.Text)
                Dim fecfin As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
                'OT 10238 - 07/04/2017 - Carlos Espejo
                'Descripcion: Si el fondo valorizado no se puede realizar la reversion
                Dim cantidadreg As Integer = New ValoresBM().ExisteValoracion(dlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
                If cantidadreg > 0 Then
                    AlertaJS("Ya existe una valorización para esta fecha, debe extornarla.")
                    Exit Sub
                End If
                'OT 10238 Fin
                If fecini <= fecfin Then
                    AlertaJS("La fecha nueva debe ser menor a la fecha inicial.")
                Else
                    EjecutarJS("MostrarConfirmacion('" & Mensaje & "','','');")
                End If
            Else
                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20  
                Dim saldos As DataTable = VerificarSaldosBancarios()

                Dim cuentasConcat As String = String.Empty
                For Each dr As DataRow In saldos.Rows
                    'cuentasConcat = cuentasConcat & IIf(cuentasConcat.Length = 0, "", ", ") & dr("NumeroCuenta")

                    EjecutarJS(String.Format("agregarFilaCuentas('{0}', '{1}', '{2}');", dr("DescripcionTercero"), dr("NumeroCuenta"), Format(CDec(dr("SaldoDisponible")), "##,##0.00")))
                Next

                If saldos.Rows.Count = 0 Then
                    EjecutarJS("MostrarConfirmacion('" & Mensaje & "', 0,'" & cuentasConcat & "');")
                Else
                    EjecutarJS("mostrarCuentasConIncidentes();")
                End If
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20  
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Private Sub btnEjecutar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEjecutar.Click
        Try
            '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20        
            Dim formatoFecha As String = "yyyyMMdd",
                formatoProv As CultureInfo = CultureInfo.InvariantCulture,
                fechaAux As DateTime

            Dim oPortafolioBM As New PortafolioBM
            Dim oPortafolioBE As PortafolioBE = oPortafolioBM.Seleccionar(dlPortafolio.SelectedValue, Me.DatosRequest)
            Dim detallePotafolio As DataRow = oPortafolioBE.Tables(0).Rows(0)
            '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20

            If Not chkreproceso.Checked Then
                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20
                'Validamos cuando el Portafolio tengas Fechas NULL o VACIO. No se permitirá continuar hasta que se revise
                If Not DateTime.TryParseExact(detallePotafolio("FechaConstitucion").ToString(), formatoFecha, formatoProv, DateTimeStyles.None, fechaAux) Or
                    Not DateTime.TryParseExact(detallePotafolio("FechaAperturaContable").ToString(), formatoFecha, formatoProv, DateTimeStyles.None, fechaAux) Then

                    AlertaJS("El Portafolio tiene Fechas Nulas o en Vacío, revise el fondo antes de continuar.")
                    Exit Sub
                End If
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20


                '================================================================================================================
                'OT-10979 - 24/11/2017 - Hanz Cocchi. Se invocan a los métodos para realizar la liquidación automática de cuentas
                '                                     por cobrar y cuentas por pagar
                'Dim dtParam As DataTable
                'dtParam = New ParametrosGeneralesBM().Listar("LIQAUTOM", DatosRequest)
                'Dim codigoIntermediario As String = String.Empty
                'Dim codigoBancoLiquida As String = String.Empty
                'If Not dtParam Is Nothing Then
                '    For Each fila As DataRow In dtParam.Rows
                '        codigoIntermediario = fila("Valor").ToString()
                '        codigoBancoLiquida = fila("Valor2").ToString()
                '        RealizarLiquidacionAutomatica(codigoIntermediario, codigoBancoLiquida)
                '    Next
                'End If
                Dim dtIntermediarioSAB As DataTable
                dtIntermediarioSAB = New TercerosBM().ListarTercerPorTipoEntidad("SAB")
                'Dim dtBancoLiquida As DataTable
                Dim codigoIntermediario As String = String.Empty
                Dim descripcionIntermediario As String = String.Empty

                Dim liqMessage As New StringBuilder
                Dim swLiqMessage As Integer = 0
                'Dim codigoBancoLiquida As String = String.Empty
                'Dim codigoMonedaLiquida As String = String.Empty
                If dtIntermediarioSAB IsNot Nothing Then
                    For Each dtSABRow As DataRow In dtIntermediarioSAB.Rows
                        codigoIntermediario = dtSABRow("CodigoTercero").ToString()
                        descripcionIntermediario = dtSABRow("Descripcion").ToString()
                        'dtBancoLiquida = Nothing
                        'dtBancoLiquida = New CuentaTercerosBM().SeleccionarCuentaTerceros(codigoIntermediario, "S").Tables(0)
                        'For Each dtBancoLiqRow As DataRow In dtBancoLiquida.Rows
                        '    codigoBancoLiquida = dtBancoLiqRow("CodigoTerceroLiquida").ToString()
                        '    codigoMonedaLiquida = dtBancoLiqRow("CodigoMoneda").ToString()
                        '    RealizarLiquidacionAutomatica(codigoIntermediario, codigoBancoLiquida, codigoMonedaLiquida)
                        'Next
                        RealizarLiquidacionAutomatica(codigoIntermediario, descripcionIntermediario, liqMessage, swLiqMessage)
                    Next
                End If
                If swLiqMessage = 1 Then
                    liqMessage.Append("</table>")
                End If
                '================================================================================================================

                'Cierre Idi
                If Not New PortafolioBM().ValidarCierre(dlPortafolio.SelectedValue) Then
                    AlertaJS(ObtenerMensaje("ALERT67"), "MostrarPopup();")
                    Exit Sub
                End If

                'If ValidarIndicadores() = False Then
                '    Exit Sub
                'End If
                Dim Mensaje As String = String.Empty
                If Not New PortafolioBM().ValidarApertura(dlPortafolio.SelectedValue) Then
                    AlertaJS("Existen Ordenes pendientes de Aprobación por exceso de broker o por exceso de límites y/o por Ejecutar o Liquidar.")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, New Guid().ToString(), UIUtility.MostrarPopUp("frmVisorOrdenesFaltantes.aspx?pfondo=" + dlPortafolio.SelectedValue + "&pFecha=" + lblFecha.Text + "&tipoOperacion=A" + "&pFechaNueva=" + tbFechaInicio.Text, "no", 800, 600, 40, 150, "no", "yes", "yes", "yes"), False)
                    Exit Sub
                End If

                'Dim dt As DataTable = VerificarSaldosBancarios()

                'If dt.Rows.Count > 0 And hdValidarApertura.Value = "1" Then
                '    EjecutarJS(UIUtility.MostrarPopUp("../Tesoreria/Reportes/frmReporte.aspx?ClaseReporte=ReporteSaldoBancariosNegativos&FechaOperacion=" & UIUtility.ConvertirFechaaDecimal(lblFecha.Text) & "&CodigoPortafolioSBS=" & dlPortafolio.SelectedValue, "", 1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                'End If

                Dim dtCuponesLibor As New DataSet
                Dim ValidaFechaLibor() As String
                Dim fechaBuscarLibor As Decimal = UIUtility.ConvertirFechaaDecimal(lblFecha.Text)
                While (fechaBuscarLibor < UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
                    fechaBuscarLibor = UIUtility.ConvertirFechaaDecimal(Convert.ToDateTime(UIUtility.ConvertirFechaaString(fechaBuscarLibor)).AddDays(1))
                    ValidaFechaLibor = oPortafolio.ValidarFechasLibor(fechaBuscarLibor).Split(",")
                    If ValidaFechaLibor(0).ToString() <> "" Then
                        Mensaje = "Falta asignar Tasa Libor para la siguientes fechas: <br><p align=left>"
                        For i As Integer = 0 To ValidaFechaLibor.Length - 1
                            Mensaje = Mensaje & "-> " & ValidaFechaLibor(i) & "<br>"
                        Next
                        AlertaJS(Mensaje + "</p>")
                        Exit Sub
                    End If
                End While
                If UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text) <= UIUtility.ObtenerFechaNegocio(dlPortafolio.SelectedValue) Then
                    AlertaJS(ObtenerMensaje("ALERT64"))
                    Exit Sub
                End If
                'Crea y actualiza SaldosCarteraTitulo, que es un consolidado de saldos custodio
                If Not GeneraSaldosCarteraTitulo() Then
                    AlertaJS(ObtenerMensaje("ALERT156"))
                    Exit Sub
                End If
                'Actualiza la fecha de constitucion y la fecha de termino del portafolio
                oPortafolio.Cerrar(dlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), DatosRequest)
                Dim dFechaApertura As Decimal = UIUtility.ObtenerFechaApertura(dlPortafolio.SelectedValue)
                Dim sHoraApertura As String = UIUtility.ObtenerDatosPortafolio(dlPortafolio.SelectedValue)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                oPrevOrdenInversionBM.TruncarProcesoMasivo()
                If Not PortafolioAperturar() Then
                    AlertaJS(ObtenerMensaje("ALERT88"))
                    Exit Sub
                End If
                If swLiqMessage = 0 Then
                    AlertaJS(ObtenerMensaje("ALERT65"))
                Else
                    AlertaJS(liqMessage.ToString())
                End If
            Else
                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20 
                'Solo si la FechaCajaOperaciones es válida exigiremos la reversa del cierre de caja de inversiones
                'If DateTime.TryParseExact(detallePotafolio("FechaCajaOperaciones").ToString(), formatoFecha, formatoProv, DateTimeStyles.None, fechaAux) Then
                '    Dim fechaExtorno As DateTime = UIUtility.ConvertirStringaFecha(tbFechaInicio.Text)
                '    If fechaAux > fechaExtorno Then
                '        AlertaJS("Debe reversar el Cierre de Caja de Inversiones.")
                '        Exit Sub
                '    End If
                'End If
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | 2018-06-20


                Dim fechaExtorno As DateTime = UIUtility.ConvertirStringaFecha(tbFechaInicio.Text)

                Dim oPortafolioCajaBM As New PortafolioCajaBM
                Dim dtPortafolioCaja As DataTable = oPortafolioCajaBM.ObtenerFechaCajaOperaciones(dlPortafolio.SelectedValue, "")
                Dim mensajeCaja As String = "<p align=left>"
                For Each drCaja As DataRow In dtPortafolioCaja.Rows

                    If UIUtility.ConvertirStringaFecha(drCaja("FechaCajaCadena")) > fechaExtorno And Not drCaja.ItemArray(9).ToString.Contains("10") Then
                        mensajeCaja = mensajeCaja + "- " & drCaja("ClaseCuenta") & " | Fecha Actual: " & drCaja("FechaCajaCadena") & "<br>"
                    End If

                Next
                If mensajeCaja <> "<p align=left>" Then
                    mensajeCaja = "No se puede realizar la reversión del portafolio porque las siguientes cajas se encuentran en las siguientes fechas:" + mensajeCaja + "</p>"
                    AlertaJS(mensajeCaja)
                    Exit Sub
                End If


                '**************************************************************************************************************
                'OT-10902 - 02/11/2017 - Ian Pastor. Sólo se debe de reversar la fecha de negocio del portafolio y no
                'inicializar los saldos bancarios a cero.
                '**************************************************************************************************************
                ''OT 10238 - 21/04/2017 - Carlos Espejo
                ''Descripcion: Cuando se realiza la reversa crea los registros para las dos cajas I,R
                'oCustodioBM.GeneraSaldoBanco(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), 0, dlPortafolio.SelectedValue, "20", DatosRequest)
                'oCustodioBM.GeneraSaldoBanco(UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), 0, dlPortafolio.SelectedValue, "10", DatosRequest)
                ''OT 10238 Fin
                '**************************************************************************************************************
                oPortafolio.Actualiza_FechaNegocio(dlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))
                AlertaJS("La fecha ha sido reversada.")
            End If
            lblFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(dlPortafolio.SelectedValue))
            ObtenerFechaActual(lblFecha.Text)
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub chkreproceso_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkreproceso.CheckedChanged
        If chkreproceso.Checked = False Then
            If DiaPosterior = "S" Then
                tbFechaInicio.Enabled = True
                EjecutarJS("$('#Div_fn').addClass('date');")
            Else
                tbFechaInicio.Enabled = False
                EjecutarJS("$('#Div_fn').removeClass('date');")
            End If
        Else
            tbFechaInicio.Enabled = True
            EjecutarJS("$('#Div_fn').addClass('date');")
        End If
    End Sub
#End Region

#Region "/* Métodos Personalizados */"
    Private Function PortafolioAperturar() As Boolean
        Dim oInfCusDS As New DataTable
        Dim strNewFechaOperacion As String
        Dim strOldFechaOperacion As String
        strNewFechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        strOldFechaOperacion = UIUtility.ConvertirFechaaDecimal(lblFecha.Text)
        If oCustodioBM.GeneraSaldos(strNewFechaOperacion, strOldFechaOperacion, dlPortafolio.SelectedValue.Trim, DatosRequest) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            dlPortafolio.Items.Clear()
            dlPortafolio.DataSource = dsPortafolio
            dlPortafolio.DataValueField = "CodigoPortafolio"
            dlPortafolio.DataTextField = "Descripcion"
            dlPortafolio.DataBind()
        Else
            dlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(dlPortafolio)
        End If
        dlPortafolio.Enabled = enabled
    End Sub
    Private Sub ObtenerFechaActual(ByVal fechaAnt As String)
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        Dim EsFeriado As Boolean
        fechaAnterior = Convert.ToDateTime(fechaAnt)
        fechaNueva = fechaAnterior.AddDays(1)
        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        If fechaNueva.DayOfWeek = DayOfWeek.Saturday Then
            While fechaNueva.DayOfWeek = DayOfWeek.Saturday
                fechaNueva = fechaNueva.AddDays(2)
                Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
                    fechaNueva = fechaNueva.AddDays(1)
                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                End While
            End While
        Else
            EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
            If EsFeriado = True Then
                While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
                    fechaNueva = fechaNueva.AddDays(1)
                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                    While fechaNueva.DayOfWeek = DayOfWeek.Saturday
                        fechaNueva = fechaNueva.AddDays(2)
                        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                    End While
                End While
            End If
        End If
        tbFechaInicio.Text = fechaNueva.ToShortDateString
    End Sub
    Private Function VerificarSaldosBancarios() As DataTable
        Dim SaldosBancarios As New SaldosBancariosBM
        Dim dsSaldosVancarios As DataSet
        dsSaldosVancarios = SaldosBancarios.ValidarSaldosBancariosNegativos(dlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(lblFecha.Text))
        Return dsSaldosVancarios.Tables(0)
    End Function

    Private Sub RealizarLiquidacionAutomatica(ByVal strIntermediario As String, ByVal strDescripcionIntermediario As String, ByRef liqMessage As StringBuilder, ByRef sw As Integer) ', ByVal strBancoLiquida As String, ByVal strCodigoMonedaLiquida As String)
        Dim objCuentaPCP As New CuentasPorCobrarPagarBE
        Dim objCuentaPCP_row As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = objCuentaPCP.CuentasPorCobrarPagar.NewCuentasPorCobrarPagarRow

        objCuentaPCP_row.CodigoMercado = ""
        objCuentaPCP_row.CodigoPortafolioSBS = dlPortafolio.SelectedValue
        objCuentaPCP_row.CodigoMoneda = ""
        objCuentaPCP_row.CodigoTercero = strIntermediario
        objCuentaPCP_row.CodigoOperacion = ""
        objCuentaPCP_row.FechaOperacion = UIUtility.ConvertirFechaaDecimal(lblFecha.Text)
        objCuentaPCP_row.FechaIngreso = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        objCuentaPCP_row.Egreso = "S"

        Dim fechaIni As Decimal = IIf(lblFecha.Text = "", 0, UIUtility.ConvertirFechaaDecimal(lblFecha.Text))
        Dim fechaFin As Decimal = IIf(tbFechaInicio.Text = "", 0, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text))

        objCuentaPCP.CuentasPorCobrarPagar.AddCuentasPorCobrarPagarRow(objCuentaPCP_row)

        Dim swLiquidacion As Integer = 0
        Dim lstBancoClaseCuenta As New List(Of String)
        Dim eventoAuto As New EventoAutomaticoBM()

        '//==== Listar las Cuentas por Cobrar
        Dim dsCuentasPorCobrar() As DataRow = New CuentasPorCobrarBM().SeleccionarPorFiltro(objCuentaPCP, fechaIni, fechaFin, DatosRequest, String.Empty).Tables(0).Select("Categoria='AC'")
        If Not dsCuentasPorCobrar Is Nothing Then
            If dsCuentasPorCobrar.Length > 0 Then
                For Each fila As DataRow In dsCuentasPorCobrar
                    lstBancoClaseCuenta = CargarBanco(fila("CodigoMercado").ToString(), dlPortafolio.SelectedValue, fila("CodigoMoneda").ToString(), strIntermediario)

                    If lstBancoClaseCuenta.Count > 0 Then
                        Dim oCuentasPorCobrar As New CuentasPorCobrarBM
                        Dim dsOpCaja As New OperacionCajaBE
                        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                        Dim CodigoContacto As String

                        opCaja.BancoMatrizDestino = ""
                        opCaja.BancoMatrizOrigen = ""
                        opCaja.ObservacionCartaDestino = ""
                        opCaja.BancoGlosaDestino = ""
                        opCaja.NumeroOperacion = fila("NroOperacion").ToString() 'row.Cells(1).Text
                        opCaja.NumeroCuenta = lstBancoClaseCuenta.Item(2) 'fila("NumeroCuenta").ToString() 'ddlNroCuenta.SelectedValue.Split(",")(1)
                        opCaja.CodigoPortafolioSBS = dlPortafolio.SelectedValue 'ddlNroCuenta.SelectedValue.Split(",")(0)
                        opCaja.FechaPago = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text) 'UIUtility.ConvertirFechaaDecimal(txtPago.Text)
                        opCaja.CodigoOperacionCaja = "1"
                        opCaja.CodigoModalidadPago = "CCOB" 'ddlTipoPago.SelectedValue
                        opCaja.CodigoModelo = IIf(opCaja.CodigoPortafolioSBS = "57" Or opCaja.CodigoPortafolioSBS = "7" Or opCaja.CodigoPortafolioSBS = "111", "OIAC", "SC01") 'ddlModeloCarta.SelectedValue
                        opCaja.CodigoMercado = fila("CodigoMercado").ToString()
                        opCaja.CodigoClaseCuenta = lstBancoClaseCuenta.Item(1)
                        opCaja.CodigoTerceroDestino = String.Empty 'ddlBancoRenovacion.SelectedValue
                        opCaja.Importe = Decimal.Parse(fila("Importe").ToString().Replace(".", UIUtility.DecimalSeparator())) 'Decimal.Parse(row.Cells(7).Text.Replace(".", UIUtility.DecimalSeparator()))
                        opCaja.TasaImpuesto = Nothing
                        opCaja.CodigoContactoIntermediario = String.Empty 'ddlContactoIntermediario.SelectedValue
                        CodigoContacto = String.Empty
                        dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                        oCuentasPorCobrar.Liquidar(dsOpCaja, String.Empty, CodigoContacto, DatosRequest, String.Empty, lstBancoClaseCuenta.Item(0), lstBancoClaseCuenta.Item(2), "A", String.Empty, "")
                        Dim objEvAuto = New EventosAutomaticosBE()
                        objEvAuto.CodigoPortafolioSBS = opCaja.CodigoPortafolioSBS
                        objEvAuto.NumeroOperacion = opCaja.NumeroOperacion
                        objEvAuto.CodigoOperacion = fila("CodigoOperacion").ToString()
                        objEvAuto.Egreso = "S"
                        eventoAuto.Insertar(objEvAuto, DatosRequest)
                    Else
                        If sw = 0 Then
                            liqMessage.Append("<table style=""width: 100%; text-align: center; font-weight: bold;"">")
                            liqMessage.Append("<tr><td>El portafolio se ha aperturado satisfactoriamente</td></tr>")
                            liqMessage.Append("</table><br/>")
                            liqMessage.Append("<table style=""width: 100%; border: 1px solid black;"">")
                            liqMessage.Append("<tr style=""width: 100%; border: 1px solid black; text-align: center; font-weight: bold;""><td>Cuenta Intermediario Sin Configurar</td></tr>")
                            sw = 1
                        End If
                        If swLiquidacion = 0 Then
                            liqMessage.Append("<tr style=""width: 100%;border: 1px solid black;""><td>" & strDescripcionIntermediario & " - " & fila("CodigoMoneda").ToString() & "</td></tr>")
                            swLiquidacion = 1
                        End If
                    End If
                Next
            End If
        End If

        '//==== Listar las Cuentas por Pagar
        objCuentaPCP.CuentasPorCobrarPagar(0).Egreso = "N"
        Dim dsCuentasPorPagar() As DataRow = New CuentasPorCobrarBM().SeleccionarPorFiltro(objCuentaPCP, fechaIni, fechaFin, DatosRequest, String.Empty).Tables(0).Select("Categoria='AC'")
        If Not dsCuentasPorPagar Is Nothing Then
            If dsCuentasPorPagar.Length > 0 Then
                For Each fila As DataRow In dsCuentasPorPagar
                    lstBancoClaseCuenta = CargarBanco(fila("CodigoMercado").ToString(), dlPortafolio.SelectedValue, fila("CodigoMoneda").ToString(), strIntermediario)

                    If lstBancoClaseCuenta.Count > 0 Then
                        Dim oCuentasPorCobrar As New CuentasPorCobrarBM
                        Dim dsOpCaja As New OperacionCajaBE
                        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()
                        Dim CodigoContacto As String
                        Dim dt As DataTable
                        Dim ordenInverscionBM As New OrdenInversionWorkFlowBM

                        opCaja.BancoMatrizDestino = ""
                        opCaja.BancoMatrizOrigen = ""
                        opCaja.ObservacionCartaDestino = ""
                        opCaja.BancoGlosaDestino = ""
                        opCaja.NumeroOperacion = fila("NroOperacion").ToString() 'row.Cells(1).Text
                        opCaja.NumeroCuenta = lstBancoClaseCuenta.Item(2) 'ddlNroCuenta.SelectedValue.Split(",")(1)
                        opCaja.CodigoPortafolioSBS = dlPortafolio.SelectedValue 'ddlNroCuenta.SelectedValue.Split(",")(0)
                        opCaja.CodigoMercado = fila("CodigoMercado").ToString() '""
                        opCaja.CodigoClaseCuenta = lstBancoClaseCuenta.Item(1) 'ddlClase.SelectedValue
                        opCaja.FechaPago = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text) 'UIUtility.ConvertirFechaaDecimal(txtPago.Text)
                        opCaja.CodigoOperacionCaja = "1"
                        opCaja.CodigoModelo = "OIAC" 'ddlModeloCarta.SelectedValue
                        opCaja.CodigoModalidadPago = "CCOB" 'ddlTipoPago.SelectedValue
                        opCaja.TasaImpuesto = 0
                        opCaja.CodigoTerceroDestino = String.Empty 'ddlBancoRenovacion.SelectedValue
                        dt = ordenInverscionBM.GetOrdenInversionDivisas(dlPortafolio.SelectedValue, opCaja.NumeroOperacion)
                        opCaja.CodigoContactoIntermediario = String.Empty 'ddlContactoIntermediario.SelectedValue
                        dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                        CodigoContacto = String.Empty 'Me.ddlContacto.SelectedValue

                        'If row.Cells(19).Text = "93" Or row.Cells(19).Text = "94" Then
                        If fila(19).ToString() = "93" Or fila(19).ToString() = "94" Then
                            opCaja.Importe = Convert.ToDecimal(dt.Rows(0)("MontoCancelar"))
                        Else
                            If dt.Rows(0)("MontoOrigen").ToString.Trim <> "" Then
                                opCaja.Importe = Convert.ToDecimal(dt.Rows(0)("MontoOrigen"))
                            Else
                                opCaja.Importe = 0
                            End If
                        End If
                        oCuentasPorCobrar.Liquidar(dsOpCaja, String.Empty, CodigoContacto, DatosRequest, String.Empty, lstBancoClaseCuenta.Item(0), lstBancoClaseCuenta.Item(2), "A", String.Empty, String.Empty)
                        Dim objEvAuto = New EventosAutomaticosBE()
                        objEvAuto.CodigoPortafolioSBS = opCaja.CodigoPortafolioSBS
                        objEvAuto.NumeroOperacion = opCaja.NumeroOperacion
                        objEvAuto.CodigoOperacion = fila("CodigoOperacion").ToString()
                        objEvAuto.Egreso = "N"
                        eventoAuto.Insertar(objEvAuto, DatosRequest)
                    Else
                        If sw = 0 Then
                            liqMessage.Append("<table style=""width: 100%; text-align: center; font-weight: bold;"">")
                            liqMessage.Append("<tr><td>El portafolio se ha aperturado satisfactoriamente</td></tr>")
                            liqMessage.Append("</table><br/>")
                            liqMessage.Append("<table style=""width: 100%; border: 1px solid black;"">")
                            liqMessage.Append("<tr style=""width: 100%; border: 1px solid black; text-align: center; font-weight: bold;""><td>Cuenta Intermediario Sin Configurar</td></tr>")
                            sw = 1
                        End If
                        If swLiquidacion = 0 Then
                            liqMessage.Append("<tr style=""width: 100%;border: 1px solid black;""><td>" & strDescripcionIntermediario & " - " & fila("CodigoMoneda").ToString() & "</td></tr>")
                            swLiquidacion = 1
                        End If
                    End If
                Next
            End If
        End If

    End Sub
    Private Function CargarBanco(ByVal codigoMercado As String, ByVal codigoPortafolioSBS As String, ByVal codigoMoneda As String, _
                                 ByVal codigoTercero As String) As List(Of String)
        Dim lstBancoClaseCuenta As New List(Of String)
        CargarBanco = lstBancoClaseCuenta
        Dim dtBancoLiquida As DataTable
        dtBancoLiquida = New CuentaTercerosBM().SeleccionarCuentaTerceros(codigoTercero, "S").Tables(0)

        If dtBancoLiquida IsNot Nothing Then
            If dtBancoLiquida.Rows.Count > 0 Then
                For Each dtBancoLiqRows As DataRow In dtBancoLiquida.Rows
                    If dtBancoLiqRows("CodigoMoneda").ToString() = codigoMoneda Then
                        Dim oBanco As New TercerosBM
                        'Dim dtBancos As DataTable
                        Dim dsBanco As DataSet = oBanco.SeleccionarBancoPorMercPortMone(codigoMercado, codigoPortafolioSBS, codigoMoneda)
                        'dtBancos = Distinct(dsBanco.Tables(0), "Descripcion")
                        'Dim dtv As DataView = New DataView(dtBancos, "descripcion like '%SCOTIABANK%'", "", DataViewRowState.OriginalRows)
                        'Dim dtv As DataView = New DataView(dtBancos, "CodigoTerceroRuc='" & codigoTercero & "'", "", DataViewRowState.OriginalRows)
                        'Dim drBanco() As DataRow = dsBanco.Tables(0).Select("CodigoTerceroRuc='" & codigoTercero & "' AND CodigoClaseCuenta='20'")
                        Dim drBanco() As DataRow = dsBanco.Tables(0).Select("CodigoTercero='" & dtBancoLiqRows("EntidadFinanciera").ToString() & "' AND CodigoClaseCuenta='20'")

                        'If dtv.Count = 1 Then
                        '    lstBancoClaseCuenta.Add(dtv(0)("CodigoTercero").ToString())
                        '    lstBancoClaseCuenta.Add(dtv(0)("CodigoClaseCuenta").ToString())
                        '    lstBancoClaseCuenta.Add(dtv(0)("NumeroCuenta").ToString())
                        'End If
                        If (drBanco IsNot Nothing) Then
                            If drBanco.Length = 1 Then
                                lstBancoClaseCuenta.Add(drBanco(0)("CodigoTercero").ToString())
                                lstBancoClaseCuenta.Add(drBanco(0)("CodigoClaseCuenta").ToString())
                                lstBancoClaseCuenta.Add(drBanco(0)("NumeroCuenta").ToString())
                            End If
                        End If
                        'Return lstBancoClaseCuenta
                        CargarBanco = lstBancoClaseCuenta
                    End If
                Next
            End If
        End If
    End Function
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
#End Region

End Class