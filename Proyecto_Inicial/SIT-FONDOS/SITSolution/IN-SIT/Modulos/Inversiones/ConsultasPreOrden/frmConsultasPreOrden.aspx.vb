Imports System.Data
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports UIUtility
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Partial Class Modulos_Inversiones_ConsultasPreOrden_frmConsultasPreOrden
    Inherits BasePage
#Region "rutinas"
    Private Sub ShowDialogPopup(ByVal StrURL As String, Optional ByVal width As String = "950", Optional ByVal heigth As String = "600", Optional ByVal left As String = "125")
        EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', '');")
    End Sub
    Sub DatosExcel(ByVal dt As DataTable, ByVal FilaInicial As Integer, ByVal oSheet As Excel.Worksheet, ByVal oCells As Excel.Range)
        oCells(2, 2) = tbFechaInicio.Text
        For Each dr In dt.Rows
            Dim i As Integer = 0
            Do While i <= dt.Columns.Count - 1
                oCells(FilaInicial, i + 1) = dr(i)
                i = i + 1
            Loop
            FilaInicial += 1
        Next
    End Sub
    Private Function verValorizadoExistencia(ByVal varCodigoPortafolio As String, ByVal varFechaOperacion As Decimal, ByVal varNumeroOrden As String, ByVal strMensaje As String) As Boolean
        Dim intCantPoliza As Integer
        Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim liquidado As Boolean
        Dim dsResulAux As DataSet = oOrdenInversionBM.verValorizadoExistencia(varCodigoPortafolio, varFechaOperacion, varNumeroOrden, liquidado, DatosRequest)
        If dsResulAux.Tables.Count > 0 Then
            If dsResulAux.Tables(0).Rows.Count > 0 Then
                intCantPoliza = Convert.ToInt32(dsResulAux.Tables(0).Rows(0)("CANT"))
                If intCantPoliza > 0 Then
                    AlertaJS(ObtenerMensaje("CONF47"))
                    Return False
                ElseIf liquidado Then
                    AlertaJS("No se puede " & strMensaje & ". Esta orden ya está liquidada.")
                    Return False
                Else
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Private Sub Accion_ModificarConsultar(ByVal strTipo As String)
        If hdCodigoOrden.Value = "" And hdPortafolio.Value = "" Then
            AlertaJS("Debe seleccionar un Registro")
            Exit Sub
        End If
        Dim strAccion As String = ""
        If strTipo = "CONSULTAR" Then
            strAccion = "CONSULTA"
        Else
            If Not verValorizadoExistencia(hdPortafolio.Value, UIUtility.ConvertirFechaaDecimal(hdFechaOperacion.Value), hdCodigoOrden.Value, "modificar") Then Exit Sub
            strAccion = "MODIFICA"
        End If
        If hdCategoriaInstrumento.Value = "BO" Then 'BONOS
            ShowDialogPopup("../InstrumentosNegociados/frmBonos.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "FI" Or _
        hdCategoriaInstrumento.Value = "FM" Then  'ORDENES FONDO
            ShowDialogPopup("../InstrumentosNegociados/frmOrdenesFondo.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "LH" Then 'LETRAS HIPOTECARIAS
            ShowDialogPopup("../InstrumentosNegociados/frmLetrasHipotecarias.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "IE" Then 'INSTRUMENTOS ESTRUCTURADOS
            ShowDialogPopup("../InstrumentosNegociados/frmInstrumentosEstructurados.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "CS" Then   'CERTIFICADO SUSCRIPCION
            ShowDialogPopup("../InstrumentosNegociados/frmCertificadosSuscripcion.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "CD" Then   'CERTIFICADO DEPOSITO
            ShowDialogPopup("../InstrumentosNegociados/frmCertificadoDeposito.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "PA" Then   'PAGARES
            ShowDialogPopup("../InstrumentosNegociados/frmPagares.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "AC" Then   'ACCIONES
            ShowDialogPopup("../InstrumentosNegociados/frmAcciones.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        ElseIf hdCategoriaInstrumento.Value = "PC" Then   'PAPELES COMERCIALES
            ShowDialogPopup("../InstrumentosNegociados/frmPapelesComerciales.aspx?PTNeg=" + strAccion + "&FechaOperacion=" + hdFechaOperacion.Value + "&CodigoOrden=" + hdCodigoOrden.Value + "&Portafolio=" + hdPortafolio.Value)
        Else
            AlertaJS("No se encuentra definido para esta clase de instrumento")
        End If
    End Sub
    Public Function ObtenerDatosOperacion(ByVal CategoriaInstrumento As String) As DataTable
        Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        Dim comisiones As Double
        Dim monedaO As String
        Dim monedaD As String
        Dim LlamadoForward As String = "N"
        If CategoriaInstrumento = "FD" Then
            LlamadoForward = "S"
        End If
        oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(Me.hdCodigoOrden.Value, Me.hdPortafolio.Value, Me.DatosRequest, PORTAFOLIO_MULTIFONDOS, LlamadoForward)
        oRow = CType(oOrdenInversionBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        Dim oTerceroBE As New TercerosBE
        Dim oTerceroBM As New TercerosBM
        Dim oRowTercero As TercerosBE.TercerosRow
        Dim oContactoBE As New ContactoBE
        Dim oContactoBM As New ContactoBM
        Dim oRowContacto As ContactoBE.ContactoRow
        Dim oMonedaBE_O As New MonedaBE
        Dim oMonedaBE_D As New MonedaBE
        Dim oMonedaBM As New MonedaBM
        Dim oRowMonedaO As MonedaBE.MonedaRow
        Dim oRowMonedaD As MonedaBE.MonedaRow
        oMonedaBE_O = oMonedaBM.SeleccionarPorFiltro(oRow.CodigoMoneda, "", "", "", "", Me.DatosRequest)
        oMonedaBE_D = oMonedaBM.SeleccionarPorFiltro(oRow.CodigoMonedaDestino, "", "", "", "", Me.DatosRequest)
        If (oMonedaBE_D.Tables.Count > 0) Then
            oRowMonedaD = oMonedaBE_D.Tables(0).Rows(0)
            monedaD = oRowMonedaD.Descripcion
        Else
            monedaD = ""
        End If
        oRowMonedaO = oMonedaBE_O.Tables(0).Rows(0)
        monedaO = oRowMonedaO.Descripcion
        Dim tipocupon As String
        Select Case oRow.CodigoTipoCupon
            Case 1
                tipocupon = "TASA FIJA NOMINAL"
            Case 2
                tipocupon = "TASA VARIABLE NOMINAL"
            Case 3
                tipocupon = "A DESCUENTO"
            Case Else
                tipocupon = ""
        End Select
        oTerceroBE = oTerceroBM.SeleccionarPorFiltro(oRow.CodigoTercero, "", "", "", "", "", Me.DatosRequest)
        oRowTercero = oTerceroBE.Tables(0).Rows(0)
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtComisiones As DataTable
        dtComisiones = oOrdenPreOrdenInversionBM.GetComisionesOrdenInversionByPoliza(DatosRequest, Me.hdPortafolio.Value, oRow.FechaOperacion, oRow.NumeroPoliza)
        If (dtComisiones.Rows.Count > 0) Then
            comisiones = dtComisiones.Compute("SUM(ValorCalculado)", Nothing)
        Else
            comisiones = 0
        End If
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21", "c22", "v22"}
        Select Case oRow.CategoriaInstrumento
            Case "AC"
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("v3") = "Hora Operación"
                drGrilla("c3") = oRow.HoraOperacion
                drGrilla("c4") = "Numero Acciones Ordenadas"
                drGrilla("v4") = oRow.CantidadOrdenado
                drGrilla("c5") = "Numero Acciones Operación"
                drGrilla("v5") = oRow.CantidadOperacion
                drGrilla("c6") = "Precio"
                drGrilla("v6") = oRow.Precio
                drGrilla("c7") = "Monto Operacion"
                drGrilla("v7") = oRow.MontoOperacion
                drGrilla("c8") = "Intermediario"
                drGrilla("v8") = oRowTercero.Descripcion 'Me.ddlIntermediario.SelectedItem.Text
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c9") = "Contacto"
                    drGrilla("v9") = oRowContacto.Descripcion
                Else
                    drGrilla("c9") = ""
                    drGrilla("v9") = ""
                End If
                drGrilla("c10") = "Observación"
                drGrilla("v10") = oRow.Observacion
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c11") = "Poliza"
                    drGrilla("v11") = oRow.NumeroPoliza
                Else
                    drGrilla("c11") = ""
                    drGrilla("v11") = ""
                End If
                drGrilla("c12") = ""
                drGrilla("v12") = ""
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
                drGrilla("c19") = "Total Comisiones"
                drGrilla("v19") = Format(comisiones, "##,##0.0000000")     'Me.txttotalComisionesC.Text
                drGrilla("c20") = "Precio Promedio"
                drGrilla("v20") = Format(oRow.PrecioPromedio, "##,##0.0000000")   'Me.txtPrecPromedio.Text
                drGrilla("c21") = "Monto Neto Operacion"
                drGrilla("v21") = Format(comisiones + oRow.MontoOperacion, "##,##0.0000000")  'Me.txtMontoNetoOpe.Text
                dtGrilla.Rows.Add(drGrilla)
            Case "BO"
                Dim oValoresBE As New ValoresBE
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "Monto Nominal Ordenado"
                drGrilla("v4") = oRow.MontoNominalOrdenado
                drGrilla("c5") = "Monto Nominal Operación"
                drGrilla("v5") = oRow.MontoNominalOperacion
                drGrilla("c6") = "Tipo Tasa"
                drGrilla("v6") = tipocupon
                drGrilla("c7") = "YTM%"
                drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
                drGrilla("c8") = "Precio Negociación %"
                drGrilla("v8") = oRow.Precio
                drGrilla("c9") = "Precio Calculado %"
                drGrilla("v9") = oRow.PrecioCalculado
                drGrilla("c10") = "Precio Negociación Sucio"
                drGrilla("v10") = oRow.PrecioNegociacionSucio
                drGrilla("c11") = "Interés Corrido Negociado"
                drGrilla("v11") = oRow.InteresCorridoNegociacion
                drGrilla("c12") = "Interés Corrido"
                drGrilla("v12") = oRow.InteresCorrido
                drGrilla("c13") = "Monto Operación"
                drGrilla("v13") = oRow.MontoOperacion
                drGrilla("c14") = "Número Papeles"
                drGrilla("v14") = Format(Math.Round(Convert.ToDecimal(IIf(oRow.CantidadOperacion = 0, 1, oRow.CantidadOperacion)), 7), "##,##0.0000000")
                drGrilla("c15") = "Intermediario"
                drGrilla("v15") = oRowTercero.Descripcion
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c16") = "Contacto"
                    drGrilla("v16") = oRowContacto.Descripcion
                Else
                    drGrilla("c16") = ""
                    drGrilla("v16") = ""
                End If
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c17") = "Poliza"
                    drGrilla("v17") = oRow.NumeroPoliza
                Else
                    drGrilla("c17") = ""
                    drGrilla("v17") = ""
                End If
                drGrilla("c18") = "Observación"
                drGrilla("v18") = oRow.Observacion
                drGrilla("c19") = "Total Comisiones"
                drGrilla("v19") = Format(comisiones, "##,##0.0000000")
                drGrilla("c20") = "Monto Neto Operación"
                drGrilla("v20") = Format(comisiones + oRow.MontoOperacion, "##,##0.0000000")
                dtGrilla.Rows.Add(drGrilla)
            Case "CD"
                Dim oValoresBE As New ValoresBE
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha de Operacion"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha de Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "Monto Nominal Ordenado"
                drGrilla("v4") = oRow.MontoNominalOrdenado
                drGrilla("c5") = "Monto Nominal Operación"
                drGrilla("v5") = oRow.MontoNominalOperacion
                drGrilla("c6") = "Tipo Tasa"
                drGrilla("v6") = "EFECTIVA"
                drGrilla("c7") = "YTM %"
                drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
                drGrilla("c8") = "Precio Negociación %"
                drGrilla("v8") = oRow.Precio
                drGrilla("c9") = "Precio Calculado"
                drGrilla("v9") = oRow.PrecioCalculado
                drGrilla("c10") = "Precio Negociación Sucio"
                drGrilla("v10") = oRow.PrecioNegociacionSucio
                drGrilla("c11") = ""
                drGrilla("v11") = ""
                drGrilla("c12") = ""
                drGrilla("v12") = ""
                drGrilla("c13") = "Monto Operación"
                drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c14") = "Número Papeles"
                drGrilla("v14") = Format(Math.Round(Convert.ToDecimal(IIf(oRow.CantidadOperacion = 0, 1, oRow.CantidadOperacion)), 7), "##,##0.0000000")
                drGrilla("c15") = "Intermediario"
                drGrilla("v15") = oRowTercero.Descripcion
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c16") = "Contacto"
                    drGrilla("v16") = oRowContacto.Descripcion
                Else
                    drGrilla("c16") = ""
                    drGrilla("v16") = ""
                End If
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c17") = "Poliza"
                    drGrilla("v17") = oRow.NumeroPoliza
                Else
                    drGrilla("c17") = ""
                    drGrilla("v17") = ""
                End If
                drGrilla("c18") = "Observación"
                drGrilla("v18") = oRow.Observacion
                drGrilla("c19") = ""
                drGrilla("v19") = ""
                drGrilla("c20") = ""
                drGrilla("v20") = ""
                dtGrilla.Rows.Add(drGrilla)
            Case "CV"
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operacion"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Liquidación"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "De :"
                drGrilla("v4") = monedaO
                drGrilla("c5") = "Monto Divisa Negociada"
                drGrilla("v5") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c6") = "A :"
                drGrilla("v6") = monedaD
                drGrilla("c7") = "Monto"
                drGrilla("v7") = Format(oRow.MontoDestino, "##,##0.0000000")
                drGrilla("c8") = "Tipo Cambio"
                drGrilla("v8") = Format(oRow.TipoCambio, "##,##0.0000000")
                drGrilla("c9") = "Intermediario"
                drGrilla("v9") = oRowTercero.Descripcion
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c10") = "Contacto"
                    drGrilla("v10") = oRowContacto.Descripcion
                Else
                    drGrilla("c10") = ""
                    drGrilla("v10") = ""
                End If
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c11") = "Poliza"
                    drGrilla("v11") = oRow.NumeroPoliza
                Else
                    drGrilla("c11") = ""
                    drGrilla("v11") = ""
                End If
                drGrilla("c12") = "Observación"
                drGrilla("v12") = oRow.Observacion
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
                dtGrilla.Rows.Add(drGrilla)
            Case "DP"
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Fecha Fin de Contrato"
                drGrilla("v3") = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
                drGrilla("c4") = "Hora Operación"
                drGrilla("v4") = oRow.HoraOperacion
                drGrilla("c5") = "Plazo"
                drGrilla("v5") = oRow.Plazo
                drGrilla("c6") = "Moneda"
                drGrilla("v6") = monedaO
                drGrilla("c7") = "Monto Nominal Ordenado"
                drGrilla("v7") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
                drGrilla("c8") = "Monto Nominal Operación"
                drGrilla("v8") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
                drGrilla("c9") = "Tipo Tasa"
                drGrilla("v9") = tipocupon
                drGrilla("c10") = "Tasa %"
                drGrilla("v10") = Format(oRow.TasaPorcentaje, "##,##0.00")
                drGrilla("c11") = "Monto Operación"
                drGrilla("v11") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c12") = "Intermediario"
                drGrilla("v12") = oRowTercero.Descripcion
                drGrilla("c13") = "Observación"
                drGrilla("v13") = oRow.Observacion
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c14") = "Poliza"
                    drGrilla("v14") = oRow.NumeroPoliza
                Else
                    drGrilla("c14") = ""
                    drGrilla("v14") = ""
                End If
                drGrilla("c15") = "Interés Acumulado"
                drGrilla("v15") = Format(oRow.InteresAcumulado, "##,##0.0000000")
                drGrilla("c16") = "Tasa Castigo"
                drGrilla("v16") = Format(oRow.TasaCastigo, "##,##0.0000000")
                drGrilla("c17") = "Interés Castigado"
                drGrilla("v17") = Format(oRow.InteresCastigado, "##,##0.0000000")
                drGrilla("c18") = "Monto a PreCancelar"
                drGrilla("v18") = Format(oRow.MontoPreCancelar, "##,##0.0000000")
                drGrilla("c19") = "Total Comisiones"
                drGrilla("v19") = Format(comisiones, "##,##0.0000000")
                drGrilla("c20") = "Monto Neto Operación"
                drGrilla("v20") = Format(comisiones + oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c21") = ""
                drGrilla("v21") = ""
                dtGrilla.Rows.Add(drGrilla)
            Case "FD"
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Contrato"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "Fecha Liquidación"
                drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c5") = "Tipo Cambio Spot"
                drGrilla("v5") = oRow.TipoCambioSpot
                drGrilla("c6") = "Tipo Cambio Futuro"
                drGrilla("v6") = oRow.TipoCambioFuturo
                drGrilla("c7") = "De"
                drGrilla("v7") = monedaO
                drGrilla("c8") = "Monto Origen"
                drGrilla("v8") = Format(oRow.MontoCancelar, "##,##0.0000000")
                drGrilla("c9") = "A"
                drGrilla("v9") = monedaD
                drGrilla("c10") = "Monto Futuro"
                drGrilla("v10") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c11") = "Plazo"
                drGrilla("v11") = oRow.Plazo
                drGrilla("c12") = "Diferencial"
                drGrilla("v12") = oRow.Diferencial
                If oRow.Delibery = "S" Then
                    drGrilla("c13") = "Modalidad Compra"
                    drGrilla("v13") = "Delivery"
                Else
                    drGrilla("c13") = "Modalidad Compra"
                    drGrilla("v13") = "Non-Delivery"
                End If
                drGrilla("c14") = "Intermediario"
                drGrilla("v14") = oRowTercero.Descripcion
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c15") = "Contacto"
                    drGrilla("v15") = oRowContacto.Descripcion
                Else
                    drGrilla("c15") = ""
                    drGrilla("v15") = ""
                End If
                drGrilla("c16") = "Observación"
                drGrilla("v16") = oRow.Observacion
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c17") = "Poliza"
                    drGrilla("v17") = oRow.NumeroPoliza
                Else
                    drGrilla("c17") = ""
                    drGrilla("v17") = ""
                End If
                If oRow.CodigoMotivo <> "" Then
                    Dim oMotivoBM As New MotivoBM
                    drGrilla("c18") = "Motivo"
                    drGrilla("v18") = oMotivoBM.Seleccionar(oRow.CodigoMotivo, Me.DatosRequest).Tables(0).Rows(0)("Descripcion")
                Else
                    drGrilla("c18") = ""
                    drGrilla("v18") = ""
                End If
                drGrilla("c19") = ""
                drGrilla("v19") = ""
                drGrilla("c20") = ""
                drGrilla("v20") = ""
                drGrilla("c21") = ""
                drGrilla("v21") = ""
                Dim objOI_BM As New OrdenPreOrdenInversionBM
                drGrilla("c22") = "Cobertura"
                drGrilla("v22") = objOI_BM.SeleccionarTipoMonedaxMotivoForw(oRow.CodigoMotivo, oRow.TipoMonedaForw).Rows(0)("Descripcion")
                dtGrilla.Rows.Add(drGrilla)
            Case "FI", "FM"
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "Fecha Trato"
                drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
                drGrilla("c5") = "Número Cuotas Ordenado"
                drGrilla("v5") = oRow.CantidadOrdenado
                drGrilla("c6") = "Número Cuotas Operación"
                drGrilla("v6") = oRow.CantidadOperacion
                drGrilla("c7") = "Monto Operación"
                drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c8") = "Precio"
                drGrilla("v8") = Format(oRow.Precio, "##,##0.0000000")
                drGrilla("c9") = "Intermediario"
                drGrilla("v9") = oRowTercero.Descripcion
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c10") = "Contacto"
                    drGrilla("v10") = oRowContacto.Descripcion
                Else
                    drGrilla("c10") = ""
                    drGrilla("v10") = ""
                End If
                drGrilla("c11") = "Observación"
                drGrilla("v11") = oRow.Observacion
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c12") = "Poliza"
                    drGrilla("v12") = oRow.NumeroPoliza
                Else
                    drGrilla("c12") = ""
                    drGrilla("v12") = ""
                End If
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
                drGrilla("c19") = "Total Comisiones"
                drGrilla("v19") = Format(comisiones, "##,##0.0000000")
                drGrilla("c20") = "Monto Neto Operación"
                drGrilla("v20") = Format(comisiones + oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c21") = "Precio Promedio"
                drGrilla("v21") = Format(oRow.PrecioPromedio, "##,##0.0000000")
                dtGrilla.Rows.Add(drGrilla)
            Case "IE"
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "Unidades Ordenadas"
                drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
                drGrilla("c5") = "Unidades Operación"
                drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
                drGrilla("c6") = "Precio"
                drGrilla("v6") = Format(oRow.Precio, "##,##0.0000000")
                drGrilla("c7") = "Monto Operación"
                drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c8") = "Intermediario"
                drGrilla("v8") = oRowTercero.Descripcion
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c9") = "Contacto"
                    drGrilla("v9") = oRowContacto.Descripcion
                Else
                    drGrilla("c9") = ""
                    drGrilla("v9") = ""
                End If
                drGrilla("c10") = "Observación"
                drGrilla("v10") = oRow.Observacion
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c11") = "Poliza"
                    drGrilla("v11") = oRow.NumeroPoliza
                Else
                    drGrilla("c11") = ""
                    drGrilla("v11") = ""
                End If
                drGrilla("c12") = ""
                drGrilla("v12") = ""
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
                drGrilla("c19") = "Total Comisiones"
                drGrilla("c20") = "Monto Neto Operación"
                drGrilla("c21") = "Precio Promedio"
                dtGrilla.Rows.Add(drGrilla)
            Case "PA"
                Dim oValoresBE As New ValoresBE
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "Monto Nominal Ordenado"
                drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
                drGrilla("c5") = "Monto Nominal Operación"
                drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
                drGrilla("c6") = "Tipo Tasa"
                drGrilla("v6") = tipocupon
                drGrilla("c7") = "YTM%"
                drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
                drGrilla("c8") = "Precio Negociación %"
                drGrilla("v8") = Format(oRow.Precio, "##,##0.0000000")
                drGrilla("c9") = "Precio Calculado %"
                drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
                drGrilla("c10") = "Precio Negociación Sucio"
                drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
                drGrilla("c11") = "Interés Corrido Negociado"
                drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
                drGrilla("c12") = "Interés Corrido"
                drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
                drGrilla("c13") = "Monto Operación"
                drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c14") = "Número Papeles"
                drGrilla("v14") = Format(Math.Round(Convert.ToDecimal(IIf(oRow.CantidadOperacion = 0, 1, oRow.CantidadOperacion)), 7), "##,##0.0000000")
                drGrilla("c15") = "Intermediario"
                drGrilla("v15") = oRowContacto.Descripcion
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c16") = "Contacto"
                    drGrilla("v16") = oRowContacto.Descripcion
                Else
                    drGrilla("c16") = ""
                    drGrilla("v16") = ""
                End If
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c17") = "Poliza"
                    drGrilla("v17") = oRow.NumeroPoliza
                Else
                    drGrilla("c17") = ""
                    drGrilla("v17") = ""
                End If
                drGrilla("c18") = "Observación"
                drGrilla("v18") = oRow.Observacion
                drGrilla("c19") = "Total Comisiones"
                drGrilla("v19") = Format(comisiones, "##,##0.0000000")
                drGrilla("c20") = "Monto Neto Operación"
                drGrilla("v20") = Format(comisiones + oRow.MontoOperacion, "##,##0.0000000")
                dtGrilla.Rows.Add(drGrilla)
            Case "PC"
                Dim oValoresBE As New ValoresBE
                dtGrilla = UIUtility.GetStructureTablebase(strTabla)
                drGrilla = dtGrilla.NewRow
                drGrilla("c1") = "Fecha Operación"
                drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
                drGrilla("c2") = "Fecha Vencimiento"
                drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
                drGrilla("c3") = "Hora Operación"
                drGrilla("v3") = oRow.HoraOperacion
                drGrilla("c4") = "Monto Nominal Ordenado"
                drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
                drGrilla("c5") = "Monto Nominal Operación"
                drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
                drGrilla("c6") = "Tipo Tasa"
                drGrilla("v6") = tipocupon
                drGrilla("c7") = "YTM%"
                drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
                drGrilla("c8") = "Precio Negociación %"
                drGrilla("v8") = Format(oRow.Precio, "##,##0.0000000")
                drGrilla("c9") = "Precio Calculado %"
                drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
                drGrilla("c10") = "Precio Negociación Sucio"
                drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
                drGrilla("c11") = "Interés Corrido Negociado"
                drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
                drGrilla("c12") = "Interés Corrido"
                drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
                drGrilla("c13") = "Monto Operación"
                drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.0000000")
                drGrilla("c14") = "Número Papeles"
                drGrilla("v14") = Format(Math.Round(Convert.ToDecimal(IIf(oRow.CantidadOperacion = 0, 1, oRow.CantidadOperacion)), 7), "##,##0.0000000")
                drGrilla("c15") = "Intermediario"
                If oRow.CodigoContacto <> "" Then
                    oContactoBE = oContactoBM.SeleccionarPorFiltro(oRow.CodigoContacto, "", "", Me.DatosRequest)
                    oRowContacto = oContactoBE.Tables(0).Rows(0)
                    drGrilla("c16") = "Contacto"
                    drGrilla("v16") = oRowContacto.Descripcion
                Else
                    drGrilla("c16") = ""
                    drGrilla("v16") = ""
                End If
                If oRow.NumeroPoliza <> "" Then
                    drGrilla("c17") = "Poliza"
                    drGrilla("v17") = oRow.NumeroPoliza
                Else
                    drGrilla("c17") = ""
                    drGrilla("v17") = ""
                End If
                drGrilla("c18") = "Observación"
                drGrilla("v18") = oRow.Observacion
                drGrilla("c19") = "Total Comisiones"
                drGrilla("v19") = Format(comisiones, "##,##0.0000000")
                drGrilla("c20") = "Monto Neto Operación"
                drGrilla("v20") = Format(comisiones + oRow.MontoOperacion, "##,##0.0000000")
                dtGrilla.Rows.Add(drGrilla)
            Case Else
                Return Nothing
        End Select
        Return dtGrilla
    End Function
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal clase As String, ByVal operacion As String, ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, nomPortafolio As String)
        Dim StrURL As String = "../Llamado/frmVisorLlamado.aspx?vcodigo=" + codigo + "&vportafolio=" + nomPortafolio + "&vclase=" + clase + "&voperacion=" + operacion + "&vmoneda=" + moneda + "&visin=" + isin + "&vsbs=" + sbs + "&vnemonico=" + mnemonico + "&cportafolio=" + portafolio
        EjecutarJS("showWindow('" & StrURL & "', '800', '600');")
    End Sub
    Private Sub ShowDialogPopup(ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String, _
                                ByVal StrFechaInicio As String, ByVal StrFechaFin As String, _
                                ByVal StrTipoOperacion As String, ByVal StrTipoInstrumento As String, ByVal StrTipoRenta As String)
        Dim StrURL As String = "frmBusquedaValoresInstrumento.aspx?vIsin=" + isin + "&vSbs=" + sbs + "&vMnemonico=" + mnemonico + "&vCorrelativo=&vFechaInicio=" + StrFechaInicio + "&vFechaFin=" + StrFechaFin + "&vTipoOperacion=" + StrTipoOperacion + "&vTipoInstrumento=" + StrTipoInstrumento + "&vTipoRenta=" + StrTipoRenta
        EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', '" & btnpopup.ClientID & "');")
    End Sub
    Private Sub CargarGrilla()
        Dim objOrdeninversionBM As New OrdenPreOrdenInversionBM
        Dim dFechaInicio, dFechaFin As Decimal
        Dim sTipoRenta, sTipoOperacion, sTipoInstrumento, sPortafolio As String
        dFechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
        dFechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
        If Me.ddlTipoRenta.SelectedIndex = 0 Then
            sTipoRenta = ""
        Else
            sTipoRenta = Me.ddlTipoRenta.SelectedValue
        End If
        If Me.ddlPortafolio.SelectedIndex = 0 Then
            sPortafolio = ""
        Else
            sPortafolio = Me.ddlPortafolio.SelectedValue
        End If
        If Me.ddltipoinstrumento.SelectedIndex = 0 Then
            sTipoInstrumento = ""
        Else
            sTipoInstrumento = Me.ddltipoinstrumento.SelectedValue
        End If
        If Me.ddlTipoOperacion.SelectedIndex = 0 Then
            sTipoOperacion = ""
        Else
            sTipoOperacion = Me.ddlTipoOperacion.SelectedValue
        End If
        Dim dsconsulta As DataSet = objOrdeninversionBM.ConsultaOrdenesPreOrdenes(dFechaInicio, dFechaFin, sTipoOperacion, sTipoInstrumento, Me.txtMnemonico.Text, Me.txtISIN.Text, Me.txtsbs.Text, sTipoRenta, sPortafolio, "", DatosRequest)
        If dsconsulta.Tables.Count <> 0 Then
            dgordenpreorden.DataSource = dsconsulta
            dgordenpreorden.DataBind()
            EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dsconsulta.Tables(0).Rows.Count) + "');")
        Else
            Dim dstemp As New DataTable
            dgordenpreorden.DataSource = dstemp
            dgordenpreorden.DataBind()
            EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(0) + "');")
        End If
    End Sub
    Private Sub ConsultarGrilla()
        CargarGrilla()
        dgordenpreorden.SelectedIndex = -1
        Me.hdCodigoOrden.Value = ""
        Me.hdPortafolio.Value = ""
    End Sub
    Public Sub CargarCombos()
        Dim dt As New DataTable
        Dim oOperacionBM As New OperacionBM
        Dim oPortafolioBM As New PortafolioBM
        Dim otipoInstrumentoBM As New TipoInstrumentoBM
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oTipoRentaBM As New TipoRentaBM
        dt = oOperacionBM.Listar_ClaseInstrumento(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoOperacion, dt, "CodigoOperacion", "Descripcion", True)
        dt = New DataTable
        dt = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoRenta, dt, "CodigoRenta", "Descripcion", True)
        dt = New DataTable
        dt = otipoInstrumentoBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddltipoinstrumento, dt, "CodigoTipoInstrumentoSBS", "Descripcion", True)
        dt = New DataTable
        dt = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, dt, "CodigoPortafolio", "Descripcion", True)
        dt = New DataTable
        dt = oParametrosGeneralesBM.Listar("MOTCAM", DatosRequest)
        HelpCombo.LlenarComboBox(ddlMotivoEliminar, dt, "valor", "nombre", True)
    End Sub
#End Region
    Dim oUtil As New UtilDM
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarCombos()
            Dim dstemp As New DataTable
            dgordenpreorden.DataSource = dstemp
            dgordenpreorden.DataBind()
            Me.lbContador.Text = UIUtility.MostrarResultadoBusqueda(dstemp)
            Me.tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            Me.tbFechaFin.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            imbEliminar.Attributes.Add("onClick", "javascript:return Confirmar();")
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            txtISIN.Text = CType(Session("SS_DatosModal"), String())(0)
            txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1)
            txtsbs.Text = CType(Session("SS_DatosModal"), String())(2)
            Session.Remove("SS_DatosModal")
        End If
        tblMotivoEliminar.Attributes.Item("Style") = "display : none"
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim sTipoRenta, sTipoOperacion, sTipoInstrumento As String
        If Me.ddlTipoOperacion.SelectedIndex = 0 Then
            sTipoOperacion = ""
        Else
            sTipoOperacion = Me.ddlTipoOperacion.SelectedValue
        End If
        If Me.ddltipoinstrumento.SelectedIndex = 0 Then
            sTipoInstrumento = ""
        Else
            sTipoInstrumento = Me.ddltipoinstrumento.SelectedValue
        End If
        If Me.ddlTipoRenta.SelectedIndex = 0 Then
            sTipoRenta = ""
        Else
            sTipoRenta = Me.ddlTipoRenta.SelectedValue
        End If
        ShowDialogPopup(txtISIN.Text.Trim, txtsbs.Text.Trim, txtMnemonico.Text.Trim, Me.tbFechaInicio.Text.Trim, Me.tbFechaFin.Text.Trim, sTipoOperacion, sTipoInstrumento, sTipoRenta)
    End Sub
    Protected Sub btnConsulta_Click(sender As Object, e As EventArgs) Handles btnConsulta.Click
        ConsultarGrilla()
    End Sub
    Private Sub GenerarReporte()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks = Nothing
        Dim oBook As Excel.Workbook = Nothing
        Dim oSheets As Excel.Sheets = Nothing
        Dim oSheet As Excel.Worksheet = Nothing
        Dim oCells As Excel.Range = Nothing
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
            Dim objOrdeninversionBM As New OrdenPreOrdenInversionBM
            Dim dFechaInicio, dFechaFin As Decimal
            Dim sTipoRenta, sTipoOperacion, sTipoInstrumento, sPortafolio As String
            dFechaInicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
            dFechaFin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
            If Me.ddlTipoRenta.SelectedIndex = 0 Then
                sTipoRenta = ""
            Else
                sTipoRenta = Me.ddlTipoRenta.SelectedValue
            End If
            If Me.ddlPortafolio.SelectedIndex = 0 Then
                sPortafolio = ""
            Else
                sPortafolio = Me.ddlPortafolio.SelectedValue
            End If
            If Me.ddltipoinstrumento.SelectedIndex = 0 Then
                sTipoInstrumento = ""
            Else
                sTipoInstrumento = Me.ddltipoinstrumento.SelectedValue
            End If
            If Me.ddlTipoOperacion.SelectedIndex = 0 Then
                sTipoOperacion = ""
            Else
                sTipoOperacion = Me.ddlTipoOperacion.SelectedValue
            End If
            Dim DT As DataTable = oOrdenInversionBM.ConsultaOrdenesPreOrdenes(dFechaInicio, dFechaFin, sTipoOperacion, sTipoInstrumento, Me.txtMnemonico.Text, Me.txtISIN.Text, Me.txtsbs.Text, sTipoRenta, sPortafolio, "", DatosRequest).Tables(0)
            If DT.Rows.Count > 0 Then
                DT.Columns.RemoveAt(20)
                DT.Columns.RemoveAt(19)
                DT.Columns.RemoveAt(18)
                Dim sFile As String, sTemplate As String, Nombre As String
                Nombre = "Con_Ord_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
                sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & Nombre
                If File.Exists(sFile) Then File.Delete(sFile)
                sTemplate = RutaPlantillas() & "\" & "ReporteConsultaOrden.xls"
                oExcel.Visible = False : oExcel.DisplayAlerts = False
                oBooks = oExcel.Workbooks
                oBooks.Open(sTemplate)
                oBook = oBooks.Item(1)
                oSheets = oBook.Worksheets
                oSheet = CType(oSheets.Item(1), Excel.Worksheet)
                oCells = oSheet.Cells
                oSheet.SaveAs(sFile)
                DatosExcel(DT, 4, oSheet, oCells)
                oExcel.Cells.EntireColumn.AutoFit()
                oBook.Save()
                oBook.Close()
                Response.Clear()
                Response.ContentType = "application/xls"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Nombre)
                Response.WriteFile(sFile)
                Response.End()
            Else
                AlertaJS("No existen registros que mostrar para esta fecha y portafolio.")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            oExcel.Quit()
            ReleaseComObject(oExcel)
            System.GC.Collect()
            System.GC.WaitForPendingFinalizers()
            If ObjCom IsNot Nothing Then
                ObjCom.terminarProceso()
            End If
        End Try
        'OT10689 - Fin.
    End Sub
    Protected Sub btnexportar_Click(sender As Object, e As EventArgs) Handles ibtnexportar.Click
        GenerarReporte()
        'Dim decfechainicio, decfechafin As Decimal
        'Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
        'Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
        'Dim oOperacionBE As New OperacionBE
        'Dim oOperacionBM As New OperacionBM
        'Dim oMonedaBE_O As New MonedaBE
        'Dim oMonedaBM As New MonedaBM
        'Dim oRowMonedaO As MonedaBE.MonedaRow
        'Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        'Dim Categoria_Instrumento As String
        'If (Me.hdPortafolio.Value = "" And Me.hdCodigoOrden.Value = "") Then
        '    decfechainicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
        '    decfechafin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
        '    Dim StrURL As String = "frmVisorConsultaPreorden.aspx?pIndica=2&pcorrelativo=&pcodigooperacion=" + Me.ddlTipoOperacion.SelectedValue + "&pcodigotipoinstrumento=" + Me.ddltipoinstrumento.SelectedValue + "&pmnemonico=" + Me.txtMnemonico.Text + "&pisin=" + Me.txtISIN.Text + "&psbs=" + Me.txtsbs.Text + "&pfechainicio=" + Convert.ToString(decfechainicio) + "&pfechafin=" + Convert.ToString(decfechafin) + "&pTipoRenta=" + Me.ddlTipoRenta.SelectedValue + "&pPortafolio=" + Me.ddlPortafolio.SelectedValue
        '    EjecutarJS("showModalDialog('" & StrURL & "', '950', '600', '');")
        '    Exit Sub
        'End If
        'oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(Me.hdCodigoOrden.Value, Me.hdPortafolio.Value, Me.DatosRequest, PORTAFOLIO_MULTIFONDOS)
        'oRow = CType(oOrdenInversionBE.Tables(0).Rows(0), OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow)
        'Dim oRowOpera As OperacionBE.OperacionRow
        'oOperacionBE = oOperacionBM.Seleccionar(oRow.CodigoOperacion, Me.DatosRequest)
        'oRowOpera = oOperacionBE.Tables(0).Rows(0)
        'Session("dtdatosoperacion") = ObtenerDatosOperacion(oRow.CategoriaInstrumento)
        'Select Case oRow.CategoriaInstrumento
        '    Case "AC"
        '        Categoria_Instrumento = "ACCIONES"
        '    Case "BO"
        '        Categoria_Instrumento = "BONOS"
        '    Case "CD"
        '        Categoria_Instrumento = "CERTIFICADO DE DEPOSITO"
        '    Case "CV"
        '        Categoria_Instrumento = "COMPRA/VENTA MONEDA EXTRANJERA"
        '    Case "DP"
        '        Categoria_Instrumento = "DEPOSITOS A PLAZO"
        '    Case "FD"
        '        Categoria_Instrumento = "OPERACIONES DERIVADAS - FORWARD DIVISAS"
        '    Case "FI"
        '        Categoria_Instrumento = "ORDENES DE FONDO"
        '    Case "FM"
        '        Categoria_Instrumento = "ORDENES DE FONDO"
        '    Case "IE"
        '        Categoria_Instrumento = "INSTRUMENTOS ESTRUCTURADOS"
        '    Case "PA"
        '        Categoria_Instrumento = "PAGARES"
        '    Case "PC"
        '        Categoria_Instrumento = "PAPELES COMERCIALES"
        '    Case Else
        '        Exit Sub
        'End Select
        'oMonedaBE_O = oMonedaBM.SeleccionarPorFiltro(oRow.CodigoMoneda, "", "", "", "", Me.DatosRequest)
        'oRowMonedaO = oMonedaBE_O.Tables(0).Rows(0)
        'GenerarLlamado(Me.hdCodigoOrden.Value, Me.hdPortafolio.Value, Categoria_Instrumento, oRowOpera.Descripcion, oRowMonedaO.Descripcion, oRow.CodigoISIN, oRow.CodigoSBS, oRow.CodigoMnemonico, Me.hdNomPortafolio.Value)
    End Sub
    Protected Sub imbModificar_Click(sender As Object, e As EventArgs) Handles imbModificar.Click
        Accion_ModificarConsultar("MODIFICAR")
        ConsultarGrilla()
    End Sub
    Protected Sub ibConsultar_Click(sender As Object, e As EventArgs) Handles ibConsultar.Click
        Accion_ModificarConsultar("CONSULTAR")
    End Sub
    Protected Sub imbEliminar_Click(sender As Object, e As EventArgs) Handles imbEliminar.Click
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        If hdCodigoOrden.Value = "" And hdPortafolio.Value = "" Then
            AlertaJS("Debe seleccionar un Registro")
            Exit Sub
        End If
        tblMotivoEliminar.Attributes.Item("Style") = "display="
        Dim strAlerta As String = ""
        If Me.ddlMotivoEliminar.SelectedValue = "" Then
            strAlerta = "-Debe de Seleccionar un Motivo de Eliminar.\n"
        End If
        If Me.txtComentarios.Text.Trim.Length <= 0 Then
            strAlerta += "-Ingrese comentarios por el cual desea eliminar la orden."
        End If
        If strAlerta.Length > 0 Then
            AlertaJS(strAlerta)
            Exit Sub
        End If
        Dim oOrdenInversionWorkFlowBM As New OrdenInversionWorkFlowBM
        If Not verValorizadoExistencia(hdPortafolio.Value, UIUtility.ConvertirFechaaDecimal(hdFechaOperacion.Value), hdCodigoOrden.Value, "eliminar") Then Exit Sub
        If ViewState("Estado") = "Ejecutada" Then
            oOrdenInversionWorkFlowBM.ExtornarOIEjecutadas(hdCodigoOrden.Value, hdPortafolio.Value, DatosRequest)
        End If
        oOrdenPreOrdenInversionBM.EliminarOI(hdCodigoOrden.Value, hdPortafolio.Value, ddlMotivoEliminar.SelectedValue, DatosRequest)
        oOrdenPreOrdenInversionBM.FechaModificarEliminarOI(hdPortafolio.Value, hdCodigoOrden.Value, Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd")), "E", txtComentarios.Text, DatosRequest)  'HDG INC 61422	20101016
        AlertaJS("El Registro ha sido eliminado satisfactoriamente")
        ConsultarGrilla()
        tblMotivoEliminar.Attributes.Item("Style") = "display :  none"
        ddlMotivoEliminar.SelectedValue = ""
    End Sub
    Protected Sub dgordenpreorden_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgordenpreorden.PageIndexChanging
        dgordenpreorden.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub
    Protected Sub dgordenpreorden_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgordenpreorden.RowCommand
        Dim i As Int32 = 0
        Dim valor As String = String.Empty
        If e.CommandName = "Seleccionar" Then
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            dgordenpreorden.SelectedIndex = gvr.RowIndex
            i = gvr.RowIndex
            Me.hdCodigoOrden.Value = gvr.Cells(15).Text()
            Me.hdPortafolio.Value = gvr.Cells(17).Text
            Me.hdNomPortafolio.Value = gvr.Cells(3).Text
            Me.hdFechaOperacion.Value = gvr.Cells(1).Text
            Me.hdCategoriaInstrumento.Value = gvr.Cells(16).Text
            ViewState("Estado") = gvr.Cells(9).Text
            If gvr.Cells(9).Text = "Ejecutada" Or gvr.Cells(9).Text = "Ingresada" Then
                imbEliminar.Visible = True
                imbModificar.Visible = True
                tblMotivoEliminar.Attributes.Item("Style") = "display="
            Else
                imbEliminar.Visible = False
                imbModificar.Visible = False
                tblMotivoEliminar.Attributes.Item("Style") = "display : none"
            End If
            EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dgordenpreorden.Rows.Count) + "');")
        End If
    End Sub
    Protected Sub ibSalir_Click(sender As Object, e As EventArgs) Handles ibSalir.Click
        Response.Redirect("~/frmDefault.aspx", False)
    End Sub
    Protected Sub ddlPortafolio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        tbFechaInicio.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue.ToString))
    End Sub
    Protected Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload

    End Sub
End Class