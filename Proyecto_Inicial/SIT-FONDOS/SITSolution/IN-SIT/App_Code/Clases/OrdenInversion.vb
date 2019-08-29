Imports Microsoft.VisualBasic
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Public Class OrdenInversion
    Public Shared Function MensajeExcesosOI(ByVal numeroOrden As String, ByVal portafolio As String, DatosRequest As DataSet) As String
        Dim oOrdenPreOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim oOperacionBM As New OperacionBM
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim dsExcesoLimites As New DataSet
        Dim dsExcesoBroker As New DataSet
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oOrdenPreOrdenInversionBE = oOrdenPreOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(numeroOrden, portafolio, Nothing, PORTAFOLIO_MULTIFONDOS)
        oRow = CType(oOrdenPreOrdenInversionBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        Dim mensaje As New StringBuilder
        Dim nroOrden As String = numeroOrden
        Dim fondo As String = portafolio
        Dim operacion As String = CType(oOperacionBM.Seleccionar(oRow.CodigoOperacion, Nothing).Tables(0).Rows(0)("Descripcion"), String)
        Dim orden As String = CType(oClaseInstrumentoBM.SeleccionarPorCategoria(oRow.CategoriaInstrumento, Nothing).Tables(0).Rows(0)("Descripcion"), String)
        Dim codISIN As String = oRow.CodigoISIN
        Dim codNem As String = oRow.CodigoMnemonico
        Dim fecha As String = DateTime.Today.ToString("dd/MM/yyyy")
        Dim session As HttpSessionState = HttpContext.Current.Session
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='550' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td colspan='3'>La siguiente orden emitido el " & fecha & ", se encuentra pendiente para su aprobación:</td></tr>")
            .Append("<tr><td height='5' colSpan='3'></td></tr>")
            .Append("<tr><td width='35%'>Numero de Orden:</td>")
            .Append("<td colspan='2' width='65%'>" & nroOrden & "</td></tr>")
            .Append("<tr><td width='35%'>Fondo:</td><td colspan='2' width='65%'>" & fondo & "</td></tr>")
            .Append("<tr><td width='35%'>Tipo de Operación:</td><td colspan='2' width='65%'>" & operacion & "</td></tr>")
            .Append("<tr><td width='35%'>Clase de Instrumento: </td><td colspan='2' width='65%'>" & orden & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo ISIN:</td><td colspan='2' width='65%'>" & codISIN & "</td></tr>")
            .Append("<tr><td width='35%'>C&oacute;digo Nem&oacute;nico:</td><td colspan='2' width='65%'>" & codNem & "</td></tr>")
            .Append("<tr height='8'><td colspan='3'></td></tr></table>")
            If oRow.Estado = EXCESO_LIM_BRK Then
                dsExcesoLimites = CType(session("dsExcesoLimites"), DataSet)
                .Append("<table cellspacing='0' cellpadding='1' border='0' width='900' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none' id='tblExcesoLimite'>")
                .Append("<tr><td><b>Detalle exceso por Limites</b></td></tr>")
                .Append("<tr height='8'><td></td></tr>")
                .Append("<tr><td><table cellspacing='0' cellpadding='1' border='1' width='900' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
                .Append("<tr style='TEXT-ALIGN: left'>")
                .Append("<th>Limite</th>" _
                    & "<th>Agrupación</th>" _
                    & "<th>%</th>" _
                    & "<th>Posición</th>" _
                    & "<th>Margen</th>" _
                    & "<th>Alerta</th></tr>")
                For Each fila As DataRow In dsExcesoLimites.Tables(0).Rows
                    .Append("<tr>" _
                        & "<td>" & fila("NombreLimite") & "</td>" _
                        & "<td>" & fila("DescripcionNivel") & "</td>" _
                        & "<td>" & String.Format("{0:###.00}", fila("ValorPorcentaje")) & "</td>" _
                        & "<td>" & String.Format("{0:###,###}", fila("Posicion")) & "</td>" _
                        & "<td>" & String.Format("{0:###,###}", fila("Margen")) & "</td>" _
                        & "<td>" & fila("Alerta") & "</td></tr>")
                Next
                .Append("</table></td></tr></table><tr height='8'><td colspan='6'></td></tr>")

                .Append("<table cellspacing='1' cellpadding='0' border='0' width='750' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
                .Append("<tr><td colspan='6'><b>Detalle exceso por Broker</b></td></tr>")
                .Append("<tr height='8'><td colspan='6'></td></tr>")
                .Append("<tr><td><table cellspacing='0' cellpadding='1' border='1' width='750' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none' ID='tblExcesoBroker'>")
                .Append("<tr style='TEXT-ALIGN: left'>" _
                    & "<th>Intermediario</th>" _
                    & "<th>Monto Operación</th>" _
                    & "<th>Monto Limite</th>" _
                    & "<th>Exceso por Broker</th>" _
                    & "</tr>")
                dsExcesoBroker = oOrdenPreOrdenInversionBM.ListarExcesoPorBroker(numeroOrden, DatosRequest)
                Dim fila2 As DataRow = dsExcesoBroker.Tables(0).Rows(0)
                .Append("<tr>" _
                    & "<td>" & fila2("Intermediario") & "</td>" _
                    & "<td>" & String.Format("{0: ###,###.00}", fila2("MontoOperacion")) & "</td>" _
                    & "<td>" & String.Format("{0: ###,###.00}", fila2("MontoRestriccion")) & "</td>" _
                    & "<td>" & String.Format("{0: ###,###.00}", fila2("ExcesoPorBroker")) & "</td>" _
                    & "</tr>")
                .Append("</table></td></tr>")
                .Append("<tr height='8'><td colspan='6'></td></tr></table>")
            Else
                If oRow.Estado = EXCESO_LIMITES Then
                    dsExcesoLimites = CType(session("dsExcesoLimites"), DataSet)
                    .Append("<table cellspacing='0' cellpadding='1' border='0' width='900' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none' id='tblExcesoLimite'>")
                    .Append("<tr><td><b>Detalle exceso por Limites</b></td></tr>")
                    .Append("<tr height='8'><td></td></tr>")
                    .Append("<tr><td><table cellspacing='0' cellpadding='1' border='1' width='900' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
                    .Append("<tr style='TEXT-ALIGN: left'>")
                    .Append("<th>Limite</th>" _
                        & "<th>Agrupación</th>" _
                        & "<th>%</th>" _
                        & "<th>Posición</th>" _
                        & "<th>Margen</th>" _
                        & "<th>Alerta</th></tr>")
                    For Each fila As DataRow In dsExcesoLimites.Tables(0).Rows
                        .Append("<tr>" _
                            & "<td>" & fila("NombreLimite") & "</td>" _
                            & "<td>" & fila("DescripcionNivel") & "</td>" _
                            & "<td>" & String.Format("{0:###.00}", fila("ValorPorcentaje")) & "</td>" _
                            & "<td>" & String.Format("{0:###,###}", fila("Posicion")) & "</td>" _
                            & "<td>" & String.Format("{0:###,###}", fila("Margen")) & "</td>" _
                            & "<td>" & fila("Alerta") & "</td></tr>")
                    Next
                    .Append("</table></td></tr><tr height='8'><td colspan='6'></td></tr></table>")
                End If
                If oRow.Estado = EXCESO_BROKER Then
                    .Append("<table cellspacing='1' cellpadding='0' border='0' width='750' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
                    .Append("<tr><td colspan='6'><b>Detalle exceso por Broker</b></td></tr>")
                    .Append("<tr height='8'><td colspan='6'></td></tr>")
                    .Append("<tr><td><table cellspacing='0' cellpadding='1' border='1' width='750' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none' ID='tblExcesoBroker'>")
                    .Append("<tr style='TEXT-ALIGN: left'>" _
                        & "<th>Intermediario</th>" _
                        & "<th>Monto Operación</th>" _
                        & "<th>Monto Limite</th>" _
                        & "<th>Exceso por Broker</th>" _
                        & "</tr>")
                    dsExcesoBroker = oOrdenPreOrdenInversionBM.ListarExcesoPorBroker(numeroOrden, DatosRequest)
                    Dim fila2 As DataRow = dsExcesoBroker.Tables(0).Rows(0)
                    .Append("<tr>" _
                        & "<td>" & fila2("Intermediario") & "</td>" _
                        & "<td>" & String.Format("{0: ###,###.00}", fila2("MontoOperacion")) & "</td>" _
                        & "<td>" & String.Format("{0: ###,###.00}", fila2("MontoRestriccion")) & "</td>" _
                        & "<td>" & String.Format("{0: ###,###.00}", fila2("ExcesoPorBroker")) & "</td>" _
                        & "</tr>")
                    .Append("</table></td></tr>")
                    .Append("<tr height='8'><td colspan='6'></td></tr></table>")
                End If
            End If
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='750' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>" _
                & "<tr><td colspan='3'><strong>AFP Integra</strong></td></tr>" _
                & "<tr><td colspan='3'><strong>Grupo Integra</strong></td></tr>" _
                & "</table>")
        End With
        Return mensaje.ToString
    End Function
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade un dato de entrada para identificar que tipo de intermediario va generar la comisión cuando es mercado extranjero| 13/06/18 
    Public Shared Sub ObtieneImpuestosComisiones(ByVal dgLista As System.Web.UI.WebControls.GridView, ByVal Mercado As String, ByVal tiporenta As String, ByVal Intermediario As String)
        Dim objcomisiones As New ImpuestosComisionesBM
        Dim i As Integer
        If Intermediario Is Nothing Then Intermediario = String.Empty
        Dim dtComisiones As DataTable = objcomisiones.SeleccionarFiltroDinamico(tiporenta, Mercado).Tables(0)
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se comprueba los intermediarios: 46213 (SCOTIA CAPITAL) y 11111115 (INTL FC STONE) si es mercado extranjero (diferente 7 (Lima) para resetear los valores de la comisiones | 13/06/18 
        If Mercado <> "7" And Intermediario.Trim.ToUpper <> "46213" And Intermediario.Trim.ToUpper <> "11111115" Then
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se comprueba los intermediarios: 46213 (SCOTIA CAPITAL) y 11111115 (INTL FC STONE) si es mercado extranjero (diferente 7 (Lima) para resetear los valores de la comisiones | 13/06/18 
            For i = 0 To dtComisiones.Rows.Count - 1
                dtComisiones.Rows(i).Item("porcentajeComision") = "(0.0000000)"
            Next
        End If
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade un dato de entrada para identificar que tipo de intermediario va generar la comisión cuando es mercado extranjero| 13/06/18 
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        dtGrilla.Columns.Add("codigoComision1")
        dtGrilla.Columns.Add("Descripcion1")
        dtGrilla.Columns.Add("porcentajeComision1")
        dtGrilla.Columns.Add("strValorCalculadoComision1")
        dtGrilla.Columns.Add("ValorOcultoComision1")
        dtGrilla.Columns.Add("codigoComision2")
        dtGrilla.Columns.Add("Descripcion2")
        dtGrilla.Columns.Add("porcentajeComision2")
        dtGrilla.Columns.Add("strValorCalculadoComision2")
        dtGrilla.Columns.Add("ValorOcultoComision2")
        For i = 0 To dtComisiones.Rows.Count - 1
            If i Mod 2 = 0 Then
                'PAR, izq
                drGrilla = dtGrilla.NewRow
                drGrilla("codigoComision1") = dtComisiones.Rows(i)("codigoComision")
                drGrilla("Descripcion1") = dtComisiones.Rows(i)("Descripcion")
                drGrilla("porcentajeComision1") = CType(dtComisiones.Rows(i)("porcentajeComision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drGrilla("strValorCalculadoComision1") = dtComisiones.Rows(i)("strValorCalculadoComision")
                If i = dtComisiones.Rows.Count - 1 Then
                    dtGrilla.Rows.Add(drGrilla)
                    blnEsImpar = True
                End If
            Else
                'IMPAR, der
                drGrilla("codigoComision2") = dtComisiones.Rows(i)("codigoComision")
                drGrilla("Descripcion2") = dtComisiones.Rows(i)("Descripcion")
                drGrilla("porcentajeComision2") = CType(dtComisiones.Rows(i)("porcentajeComision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drGrilla("strValorCalculadoComision2") = dtComisiones.Rows(i)("strValorCalculadoComision")
                dtGrilla.Rows.Add(drGrilla)
            End If
        Next
        dgLista.DataSource = dtGrilla
        dgLista.DataBind()
        If blnEsImpar = True Then
            CType(dgLista.Rows(dgLista.Rows.Count - 1).FindControl("txtValorComision2"), TextBox).Visible = False
        End If
    End Sub
    ''' <summary>
    ''' Calcula los limites online
    ''' </summary>
    ''' <param name="p"></param>
    ''' <param name="estadoOI">El viewstate ya creado recibe el valor indiciado mas abajo </param>
    ''' <param name="GUID_Limites">Espera que se envie en valor aleatorio para nombrar a la sesion que se creara con los limites</param>
    ''' <remarks></remarks>
    Public Shared Sub CalculaLimitesOnLine(ByVal p As Page, ByVal DatosRequest As DataSet, ByRef estadoOI As Object, ByRef GUID_Limites As String)
        Dim txtCantidad As New TextBox
        Dim instrumento As String = ""
        Dim session As HttpSessionState = HttpContext.Current.Session
        If p.ToString.ToLower.IndexOf("acciones") > 0 Then
            instrumento = "ACCIONES"
            txtCantidad = p.FindControl("txtNroAccOper")
        ElseIf p.ToString.ToLower.IndexOf("bonos") > 0 Then
            instrumento = "BONOS"
            txtCantidad = p.FindControl("txtNroPapeles")
        ElseIf p.ToString.ToLower.IndexOf("certificadodeposito") > 0 Then
            instrumento = "CERTIFICADO DEPOSITO"
            txtCantidad = p.FindControl("txtMnomOp")
        ElseIf p.ToString.ToLower.IndexOf("certificadossuscripcion") > 0 Then
            instrumento = "CERTIFICADO SUSCRIPCIÓN"
            txtCantidad = p.FindControl("txtNroAccOper")
        ElseIf p.ToString.ToLower.IndexOf("papelescomerciales") > 0 Then
            instrumento = "PAPELES COMERCIALES"
            txtCantidad = p.FindControl("txtMnomOp")
        ElseIf p.ToString.ToLower.IndexOf("ordenesfondo") > 0 Then
            instrumento = "FONDOS DE INVERSIÓN"
            txtCantidad = p.FindControl("txtNroFondoOp")
        ElseIf p.ToString.ToLower.IndexOf("pagares") > 0 Then
            instrumento = "PAGARES"
            txtCantidad = p.FindControl("txtMnomOp")
        ElseIf p.ToString.ToLower.IndexOf("instrumentosestructurados") > 0 Then
            instrumento = "INSTRUMENTOS ESTRUCTURADOS"
            txtCantidad = p.FindControl("txtUnidadesOp")
        ElseIf p.ToString.ToLower.IndexOf("letrashipotecarias") > 0 Then
            instrumento = "LETRAS HIPOTECARIAS"
            txtCantidad = p.FindControl("txtMnomOp")
        ElseIf p.ToString.ToLower.IndexOf("depositoplazos") > 0 Then
            instrumento = "DEPOSITO A PLAZO"
            txtCantidad = p.FindControl("txtMnomOp")
        ElseIf p.ToString.ToLower.IndexOf("operacionesfuturas") > 0 Then
            instrumento = "FUTUROS"
            txtCantidad = p.FindControl("txtNumConOperacion")
        End If
        Dim hdPagina As HiddenField = p.FindControl("hdPagina")
        Dim ddlOperacion As DropDownList = p.FindControl("ddlOperacion")
        Dim txtMnemonico As TextBox = p.FindControl("txtMnemonico")
        Dim ddlTipoTitulo As DropDownList = p.FindControl("ddlTipoTitulo")
        Dim ddlFondo As DropDownList = p.FindControl("ddlFondo")
        Dim ddlIntermediario As DropDownList = p.FindControl("ddlIntermediario")
        If hdPagina.Value <> "TI" Then
            Dim oLimiteEvaluacion As New LimiteEvaluacionBM
            Dim dsAux As New DataSet
            Dim codigoOperacion As String = ddlOperacion.SelectedValue.ToString()
            Dim codigoNemonico As String = ""
            If p.ToString.ToLower.IndexOf("depositoplazos") > 0 Then
                codigoNemonico = ddlTipoTitulo.SelectedValue.ToString()
            Else
                codigoNemonico = txtMnemonico.Text.ToString()
            End If
            Dim cantidadOperacion As Decimal = Convert.ToDecimal(txtCantidad.Text)
            Dim codigoPortafolio As String = ddlFondo.SelectedValue.ToString()
            Dim codigoTercero As String = ddlIntermediario.SelectedValue.ToString()
            dsAux = oLimiteEvaluacion.ListarExcesosLimitesOnLine(codigoNemonico, cantidadOperacion, codigoPortafolio, codigoOperacion, DatosRequest, codigoTercero)
            estadoOI = ""
            session(GUID_Limites) = dsAux
            If Not (dsAux Is Nothing) Then
                If (dsAux.Tables.Count > 0) Then
                    If (dsAux.Tables(0).Rows.Count > 0) Then
                        session("Instrumento") = instrumento
                        estadoOI = "E-EXC"
                        session("dsExcesoLimites") = dsAux
                        Dim bp As New BasePage
                        bp.EjecutarJS(UIUtility.MostrarPopUp("~/Modulos/Inversiones/InstrumentosNegociados/frmConsultaLimitesInstrumento.aspx?GUID=" + GUID_Limites, "no",
                        1000, 500, 50, 5, "no", "yes", "yes", "yes"), False)
                    End If
                End If
            End If
        End If
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Se quita el parametro GUID y DataRequest de las funciones
    'calcularTasanegociacionPrecio, CalcularPrecioLimpioN, CalcularInteresesCorridos2, CalcularVPN, CalcularMontoNominalVAC, CalularValorOperacionDPCDPrecio
    Public Shared Function CalculaMontoOperacion(ByVal CodigoNemonico As String, ByVal Cantidad As Decimal, ByVal Indice As String, ByVal strFechaLiquidacion As String,
    ByVal TipoTasa As String, ByVal strFechaOperacion As String, ByRef PrecioOperacion As Decimal, ByRef Tasa As Decimal, ByRef Mensaje As String,
    ByVal request As DataSet) As Decimal
        Dim MontoOperacion As Decimal, Categoria As String, ValorUnitario As Decimal, MontoNominal As Decimal, FechaLiquidacion As Decimal, vpnegociacion As Decimal,
        InteresCorrido As Decimal, preciolimpio As Decimal, preciolimpio2 As Decimal, preciosucio As Decimal, ImporteNominal As Decimal, fechaFinBono As Decimal,
        FechaOperacion As Decimal, AmortizacionPendiente As Decimal
        FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(strFechaLiquidacion)
        FechaOperacion = UIUtility.ConvertirFechaaDecimal(strFechaOperacion)
        'Dim dtCaracValor As DataTable = New PrevOrdenInversionBM().SeleccionarCaracValor(CodigoNemonico, request).Tables(0)
        Dim dtCaracValor As DataTable = New PrevOrdenInversionBM().SeleccionarCaracValor(CodigoNemonico, FechaOperacion).Tables(0) 'OT10795
        Dim objFormulasOI As New OrdenInversionFormulasBM
        If dtCaracValor.Rows.Count > 0 Then
            Categoria = dtCaracValor.Rows(0)("Categoria").ToString()
            ValorUnitario = dtCaracValor.Rows(0)("ValorUnitario")
            AmortizacionPendiente = dtCaracValor.Rows(0)("AmortizacionPendiente")
            MontoNominal = Cantidad * ValorUnitario * AmortizacionPendiente 'OT10795
            Select Case Categoria
                Case "BO"
                    If Indice = "P" Then
                        If PrecioOperacion = 0 Then
                            Mensaje = "Debe ingresar el precio de la orden!"
                            Return 0
                        Else
                            Tasa = objFormulasOI.calcularTasanegociacionPrecio(CodigoNemonico, FechaLiquidacion, MontoNominal, TipoTasa, PrecioOperacion)
                        End If
                    ElseIf Indice = "T" Then
                        If Tasa = 0 Then
                            Mensaje = "Debe ingresar la Tasa de la orden!"
                            Return 0
                        Else
                            PrecioOperacion = objFormulasOI.CalcularPrecioLimpioN(CodigoNemonico, FechaLiquidacion, MontoNominal, Tasa, TipoTasa)
                        End If
                    End If
                    InteresCorrido = objFormulasOI.CalcularInteresesCorridos2(CodigoNemonico, FechaLiquidacion, MontoNominal, Tasa, TipoTasa)
                    If InteresCorrido = -1 Then
                        Mensaje = "Los Datos ingresados no son consistente para el calculo del interes corrido"
                        Return 0
                    End If
                    vpnegociacion = objFormulasOI.CalcularVPN("", CodigoNemonico, FechaLiquidacion, "", MontoNominal, Tasa, TipoTasa)
                    preciolimpio = 0
                    preciolimpio2 = 0
                    If Indice = "T" Then
                        preciolimpio2 = (vpnegociacion - InteresCorrido) / MontoNominal * 100
                        preciolimpio = objFormulasOI.CalcularPrecioLimpioN(CodigoNemonico, FechaLiquidacion, MontoNominal, Tasa, TipoTasa)
                    End If
                    If Math.Abs(preciolimpio2 - preciolimpio) > 5 Then
                        MontoOperacion = Format(vpnegociacion, "##,##0.0000000")
                    Else
                        ImporteNominal = objFormulasOI.CalcularMontoNominalVAC(CodigoNemonico, FechaLiquidacion, MontoNominal)
                        MontoOperacion = Format(PrecioOperacion / 100 * ImporteNominal + InteresCorrido, "##,##0.0000000")
                    End If
                Case "CD"
                    Dim oValoresBM As New ValoresBM
                    Dim oValorBE As ValoresBE
                    Dim dValorUnitario As Decimal
                    oValorBE = oValoresBM.Seleccionar(CodigoNemonico, request)
                    If oValorBE.Tables(0).Rows.Count > 0 Then
                        dValorUnitario = Val(oValorBE.Tables(0).Rows(0)("ValorUnitario").ToString)
                    End If
                    fechaFinBono = CType(dtCaracValor.Rows(0)("FechaVencimiento").ToString(), Decimal)
                    If Indice = "P" Then
                        If PrecioOperacion = 0 Then
                            Mensaje = "Debe ingresar el precio de la orden!"
                            Return 0
                        Else
                            preciolimpio = PrecioOperacion
                            Tasa = objFormulasOI.CalularValorOperacionDPCDPrecio(FechaLiquidacion, fechaFinBono, MontoNominal, PrecioOperacion, CodigoNemonico)
                        End If
                    ElseIf Indice = "T" Then
                        If Tasa = 0 Then
                            Mensaje = "Debe ingresar la Tasa de la orden!"
                            Return 0
                        Else
                            preciosucio = Math.Round(Convert.ToDecimal(objFormulasOI.CalularValorOperacionDPCD(FechaLiquidacion, fechaFinBono, MontoNominal, Tasa,
                            CodigoNemonico)) * 100, 4)
                            preciolimpio = preciosucio
                        End If
                    End If
                    MontoOperacion = ((preciolimpio / 100) * Convert.ToDecimal(Cantidad)) * dValorUnitario
            End Select
        End If
        Return MontoOperacion
    End Function
End Class
