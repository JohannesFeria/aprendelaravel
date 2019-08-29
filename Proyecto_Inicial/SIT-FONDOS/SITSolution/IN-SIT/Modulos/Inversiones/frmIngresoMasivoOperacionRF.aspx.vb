Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports CrystalDecisions.Shared
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Microsoft.Office
Imports System.Collections.Generic
Imports ParametrosSIT
Imports System.Globalization
Imports System.Threading
Imports Sit.BusinessLayer.MotorInversiones

Partial Class Modulos_Inversiones_frmIngresoMasivoOperacionRF
    Inherits BasePage
    Dim pRutas As String
    Dim rutas As New System.Text.StringBuilder

    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se ordenes los controles
    Dim lbCodigoPrevOrden As Label, tbHora As TextBox, ddlOperacion As DropDownList, ddlMedioNeg As DropDownList, tbNemonico As TextBox, ddlCondicion As DropDownList,
    ddlTipoTasa As DropDownList, tbFechaLiquidacion As TextBox, tbCantidad As TextBox, tbPrecio As TextBox, tbCantidadOperacion As TextBox, tbPrecioOperacion As TextBox,
    hdIntermediario As HtmlInputHidden, tbTasa As TextBox, ddlPlazaN As DropDownList, ddlIndice As DropDownList, hdClaseInstrumento As HtmlInputHidden,
    ddlTipoFondo As DropDownList, ddlTipoTramo As DropDownList, lbClase As Label, tbNemonicoF As TextBox, ddlOperacionF As DropDownList, ddlIndiceF As DropDownList,
    ddlPlazaNF As DropDownList, ddlMedioNegF As DropDownList, ddlCondicionF As DropDownList, ddlTipoTasaF As DropDownList, tbFechaLiquidacionF As TextBox,
    hdIntermediarioF As HtmlInputHidden, tbCantidadF As TextBox, tbPrecioF As TextBox, tbCantidadOperacionF As TextBox, tbPrecioOperacionF As TextBox, tbTasaF As TextBox,
    chkPorcentajeF As CheckBox, tbIntermediario As TextBox, tbIntermediarioF As TextBox, ddlFondos As DropDownList, lbOperacion As Label, lbPlazaN As Label,
    lbMedioNeg As Label, lbCondicion As Label, lbTipoTasa As Label, lbIndice As Label, lbTipoFondo As Label, lbTipoTramo As Label, hdOperacionTrz As HtmlInputHidden,
    tbTotal As TextBox, chkSelect As CheckBox, tbHora2 As TextBox, tbNemonico2 As TextBox, ddlOperacion2 As DropDownList, tbCantidad2 As TextBox, ddlIndice2 As DropDownList,
    tbPrecio2 As TextBox, ddlTipoTasa2 As DropDownList, tbTasa2 As TextBox, ddlPlazaN2 As DropDownList, ddlCondicion2 As DropDownList, tbIntermediario2 As TextBox,
    ddlMedioNeg2 As DropDownList, tbFechaLiquidacion2 As TextBox, tbCantidadOperacion2 As TextBox, tbPrecioOperacion2 As TextBox, ddlTipoTramo2 As DropDownList,
    chkPorcentaje2 As CheckBox, hdPorcentaje2 As HtmlInputHidden, ddlTipoFondoF As DropDownList, ddlTipoTramoF As DropDownList, tbOperadorF As TextBox,
    hdCambio As HtmlInputHidden, hdCambioTraza As HtmlInputHidden, hdCambioTrazaFondo As HtmlInputHidden, hdClaseInstrumentoF As HtmlInputHidden, chkPorcentaje As CheckBox, tFechaOperacion As TextBox, HdFondos As HiddenField,
    ddlTipoValorizacion As DropDownList
    'OT 10090 Fin
    Dim strMensaje As String = "", InteresCorrido As Decimal, ValorActual As Decimal
    Dim oFeriado As New FeriadoBM
    Dim objParamGeneral As New ParametrosGeneralesBM
    Public Function instanciarTablaPrevOrdenInversionDetalle() As DataTable
        Dim tabla As New DataTable
        tabla.Columns.Add("CodigoPrevOrden")
        tabla.Columns.Add("CodigoPortafolio")
        tabla.Columns.Add("Asignacion")
        tabla.Columns.Add("Cambio")
        Return tabla
    End Function
    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        'EJECUCION DE ORDENES DE INVERSION
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim strCodPrevOrden As String = ""
        Dim ds As New DataSet
        Dim strEstadoPrevOrden As String = ""
        Dim decNProceso As Decimal = 0
        Dim arrCodPrevOrden As Array
        Try
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            Dim count As Decimal = 0, contFechaDiferenteNegocio As Integer = 0, strMensajeDiferenteFechaNegocio As String = "<p align=left>"
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                    Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    Dim fondos As String = CType(fila.FindControl("ddlfondos"), DropDownList).SelectedItem.ToString
                    tFechaOperacion = CType(fila.FindControl("tFechaOperacion"), TextBox)
                    HdFondos = CType(fila.FindControl("Hdfondos"), HiddenField)
                    Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio(HdFondos.Value.Trim)
                    Dim decCodigoPrevOrden As Decimal
                    strEstadoPrevOrden = oPrevOrdenInversionBM.ObtenerEstadoPrevOrdenInversion(CType(lbCodigoPrevOrden.Text, Decimal), ds) 'agregado por JH , para no generar duplicados
                    If chkSelect.Checked = True Then
                        If fila.Cells(2).Text = "APR" Then
                            If UIUtility.ConvertirFechaaDecimal(tFechaOperacion.Text.Trim) <> fechaNegocio Then
                                contFechaDiferenteNegocio += 1
                                strMensajeDiferenteFechaNegocio += "-> N°: " + fila.Cells(1).Text + " / Portafolio: " + fondos + _
                                                                  "<br> F. Operación: " + tFechaOperacion.Text.Trim + _
                                                                  " / F. Portafolio: " + UIUtility.ConvertirFechaaString(fechaNegocio) + "<br><br>"
                            Else
                                decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                                count = count + 1
                                strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                            End If
                        End If
                    End If
                End If
            Next


            If contFechaDiferenteNegocio > 0 Then
                AlertaJS("Verificar las siguiente negociaciones, las fecha de operación no coincide con la fecha negocio del portafolio: " + _
                         strMensajeDiferenteFechaNegocio + "</p>")
            ElseIf count > 0 Then
                strCodPrevOrden = strCodPrevOrden.Substring(0, strCodPrevOrden.Length - 1)
                EjecutarOrdenInversion(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), strCodPrevOrden, , , decNProceso)
                CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
            Else
                AlertaJS("Seleccione el registro a ejecutar ó el registro ya se encontraba ejecutado!")
                CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                ''OT-10784 Inicio
                'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'Se elimina todo los procesos
                ''Se setea el valor a cero de las ordenes afectadas en el proceso
                'arrCodPrevOrden = strCodPrevOrden.Split("|")
                'For i = 0 To arrCodPrevOrden.Length - 1
                '    If arrCodPrevOrden(i) <> "" Then
                '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                '    End If
                'Next
                'OT-10784 Fin
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            'OT-10784 Inicio
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
            btnAprobar.Text = "Ejecutar"
            btnAprobar.Enabled = True
        End Try
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            ViewState("decFechaOperacion") = decFechaOperacion
            Dim strCodigoClaseInstrumento As String = ddlClaseInstrumento.SelectedValue
            Dim strCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue
            Dim strOperador As String = ddlOperador.SelectedValue
            Dim strCodigoNemonico As String = tbCodigoMnemonico.Text
            Dim strEstado As String = ddlEstado.SelectedValue
            ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
            ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
            ViewState("strOperador") = ddlOperador.SelectedValue
            ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
            ViewState("strEstado") = ddlEstado.SelectedValue
            CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, _
            strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Sub CalcularTirNetaNegociacionRF(ByVal codigoOrden As String, ByVal codigoPortafolio As String)
        '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución
        Dim datosOrdenInv As OrdenPreOrdenInversionBE = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(codigoOrden, codigoPortafolio, DatosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim orden As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow = datosOrdenInv.Tables(0).Rows(0)
        Dim tirNeta As Decimal = orden.YTM

        If (orden.CategoriaInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_BONO Or
            orden.CategoriaInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_CERTIFICADO_DEPOSITO Or
            orden.CategoriaInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_PAPELES_COMERCIALES Or
            orden.CategoriaInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_LETRAS_HIPOTECARIAS) And
            Math.Round(orden.MontoNetoOperacion, 2) <> Math.Round(orden.MontoOperacion, 2) Then

            Dim fechaLiquidacion_Date As Date = DateTime.ParseExact(orden.FechaLiquidacion, "yyyyMMdd", CultureInfo.InvariantCulture)

            Dim dsValor As DataSet = New PrevOrdenInversionBM().SeleccionarCaracValor(orden.CodigoMnemonico, orden.FechaOperacion) 'CRumiche: El Parametro fecha es obsoleto para este método
            If dsValor.Tables.Count < 2 Then Throw New Exception("Error: No se ha podido obtener el detalle de cupones del Valor")

            Dim dtCaracValor As DataTable = dsValor.Tables(0)
            Dim dtDetalleCupones As DataTable = dsValor.Tables(1)

            Dim rowValor As DataRow = dtCaracValor.Rows(0)

            Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(rowValor("BaseCuponMensual"))
            Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(rowValor("BaseCuponAnual"))
            Dim aplicacionTasa As TipoAplicacionTasa = UIUtility.ObtenerTipoAplicacionTasaDesdeCodTipoTasa(orden.CodigoTipoCupon)

            Dim EsCuponADescuento As Boolean = rowValor("codigoTipoCupon").ToString.Equals("3") ' Es cupón A DESCUENTO solo si CodigoTipoCupon = 3 
            Dim esValorExtranjero As Boolean = rowValor("CodigoMercado").ToString.Equals("2")

            'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
            Dim esCalculoTBill As Boolean = rowValor("CodigoTipoInstrumentoSBS").ToString.Equals("100")

            Dim neg As NegociacionRentaFija

            'Pasamos a calcular la negociacion (El motor hará todo el trabajo en tiempo real y en la capa de Negocio)
            neg = UIUtility.CalcularNegociacionRentaFija(dtDetalleCupones,
                                                                CDec(rowValor("TasaCupon")),
                                                                CInt(rowValor("DiasPeriodicidad")),
                                                                CDec(rowValor("ValorUnitario")),
                                                                orden.CantidadOperacion,
                                                                fechaLiquidacion_Date,
                                                                orden.YTM,
                                                                baseMensual,
                                                                baseAnual,
                                                                EsCuponADescuento,
                                                                esValorExtranjero,
                                                                aplicacionTasa,
                                                                ,
                                                                ,
                                                                esCalculoTBill)

            neg.PrecioSucio = orden.MontoNetoOperacion / neg.CuponVigente.SaldoNominalInicial 'CRumiche: Se determina el nuevo Precio Sucio
            neg.CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio() 'Esto permitirá que el valor de neg.YTM se recalcule

            tirNeta = neg.YTM * 100 '%: Es un Porcentaje 
        End If

        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        oPrevOrdenInversionBM.ActualizarTirNetaEnOrdenInversion(codigoOrden, codigoPortafolio, tirNeta)
        '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución
    End Sub


    Private Sub btnGrabar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM, oPrevOrdenInversionBE As New PrevOrdenInversionBE, objFormulasOI As New OrdenInversionFormulasBM,
            precioOperacion As Decimal, tasa As Decimal, totalOperacionRF As Decimal, precioEjecutado As Decimal, totalEjecutadoRF As Decimal,
            bolValidaCampos As Boolean = False, fixing As Decimal = 0, validaFixing As Boolean = True, oTrazabilidadOperacionBE As New TrazabilidadOperacionBE,
            _filaTraz As TrazabilidadOperacionBE.TrazabilidadOperacionRow
            Dim porcentaje As String
            Dim dtDistribucionPorFondo As DataTable = instanciarTablaPrevOrdenInversionDetalle()
            Dim dtDistribucionPorFondoTotal As DataTable = instanciarTablaPrevOrdenInversionDetalle()
            Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow
            Dim chkSelect As CheckBox

            For Each fila As GridViewRow In Datagrid1.Rows
                chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                If chkSelect.Checked Then

                    chkPorcentaje = CType(fila.FindControl("chkPorcentaje"), CheckBox)

                    If fila.RowType = DataControlRowType.DataRow Then

                        lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                        hdCambio = CType(fila.FindControl("hdCambio"), HtmlInputHidden)
                        hdCambioTraza = CType(fila.FindControl("hdCambioTraza"), HtmlInputHidden)
                        hdCambioTrazaFondo = CType(fila.FindControl("hdCambioTrazaFondo"), HtmlInputHidden)

                        If fila.Cells(2).Text = PREV_OI_INGRESADO OrElse dtDistribucionPorFondo.Rows.Count > 0 Then
                            If (Not lbCodigoPrevOrden Is Nothing) OrElse dtDistribucionPorFondo.Rows.Count > 0 Then

                                tbHora = CType(fila.FindControl("tbHora"), TextBox)
                                tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                                hdIntermediario = CType(fila.FindControl("hdIntermediario"), HtmlInputHidden)
                                ddlOperacion = CType(fila.FindControl("ddlOperacion"), DropDownList)
                                'tbFechaOperacion = CType(fila.FindControl("tFechaOperacion"), TextBox)
                                ddlMedioNeg = CType(fila.FindControl("ddlMedioNeg"), DropDownList)
                                ddlCondicion = CType(fila.FindControl("ddlCondicion"), DropDownList)
                                ddlTipoTasa = CType(fila.FindControl("ddlTipoTasa"), DropDownList)
                                tbFechaLiquidacion = CType(fila.FindControl("tbFechaLiquidacion"), TextBox)
                                tbCantidad = CType(fila.FindControl("tbCantidad"), TextBox)
                                tbPrecio = CType(fila.FindControl("tbPrecio"), TextBox)
                                tbCantidadOperacion = CType(fila.FindControl("tbCantidadOperacion"), TextBox)
                                tbPrecioOperacion = CType(fila.FindControl("tbPrecioOperacion"), TextBox)
                                tbTasa = CType(fila.FindControl("tbTasa"), TextBox)
                                ddlPlazaN = CType(fila.FindControl("ddlPlazaN"), DropDownList)
                                ddlIndice = CType(fila.FindControl("ddlIndice"), DropDownList)
                                hdClaseInstrumento = CType(fila.FindControl("hdClaseInstrumento"), HtmlInputHidden)
                                ddlTipoFondo = CType(fila.FindControl("ddlTipoFondo"), DropDownList)
                                ddlTipoTramo = CType(fila.FindControl("ddlTipoTramo"), DropDownList)
                                '    ddlFondos = CType(fila.FindControl("ddlfondos"), DropDownList)
                                HdFondos = CType(fila.FindControl("Hdfondos"), HiddenField)
                                Dim hdTipoValorizacion As HiddenField
                                hdTipoValorizacion = CType(fila.FindControl("hdTipoValorizacion"), HiddenField)
                                hdFechaNegocio = CType(fila.FindControl("hdFechaOperacion"), HiddenField)

                                Dim strFechaOperacion As String = hdFechaNegocio.Value
                                Dim strFechaLiquidacion As String = tbFechaLiquidacion.Text

                                If strFechaOperacion.Trim.Length > 0 Then ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(strFechaOperacion)

                                bolValidaCampos = False

                                If tbCantidad.Text <> "" And _
                                    tbNemonico.Text <> "" And _
                                    hdIntermediario.Value <> "" And _
                                    Not ((hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And
                                        ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) And
                                    Not (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And
                                         ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And
                                         ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) Then
                                    validaFixing = ValidarFixing(hdClaseInstrumento.Value, tbNemonico.Text, fixing)

                                    If ValidarNemonico(tbNemonico.Text) And _
                                        ValidarIntermediario(hdIntermediario.Value.ToString) And _
                                        ValidarFechaVencimiento(strFechaOperacion, strFechaLiquidacion, tbNemonico.Text) And _
                                        validaFixing = True Then
                                        If fila.Cells(2).Text <> ParametrosSIT.PREV_OI_EJECUTADO And _
                                            fila.Cells(2).Text <> ParametrosSIT.PREV_OI_APROBADO And _
                                            fila.Cells(2).Text <> ParametrosSIT.PREV_OI_PENDIENTE Then
                                            bolValidaCampos = True
                                        End If
                                    Else
                                        ' strMensaje = strMensaje + "- Validar Nemonico o Intermediario. \n"
                                        Throw New UserInfoException("Validar Nemonico o Intermediario") 'CRumiche: No se estaba mostrando esta validación
                                    End If
                                    If Not ValidarSaldo(UIUtility.ConvertirFechaaDecimal(strFechaOperacion), tbNemonico.Text, HdFondos.Value.ToString, hdClaseInstrumento.Value.ToString, ddlOperacion.SelectedValue, IIf(IsNumeric(tbCantidad.Text), CDec(tbCantidad.Text), 0), 2, CType(lbCodigoPrevOrden.Text, Decimal)) Then
                                        bolValidaCampos = False
                                        Throw New UserInfoException("No existe el saldo suficiente para generar la pre-orden de inversión")

                                    End If
                                Else
                                    If (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or _
                                        hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And _
                                    ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                                        strMensaje = strMensaje + "- Seleccione Tipo. <br />"
                                    End If
                                    If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And
                                        ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                                        strMensaje = strMensaje + "- Seleccione Tipo Tramo. <br />"
                                    End If
                                End If
                                If bolValidaCampos = True Then

                                    'INICIO | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-08-06 | Corrección de Distribución x Fondo
                                    porcentaje = "N"
                                    If porcentaje.Equals("N") Then
                                        Dim rowPorFondo As DataRow = dtDistribucionPorFondo.NewRow
                                        rowPorFondo("CodigoPrevOrden") = lbCodigoPrevOrden.Text
                                        rowPorFondo("CodigoPortafolio") = HdFondos.Value.ToString
                                        '    rowPorFondo("Asignacion") = tbCantidadOperacion.Text
                                        rowPorFondo("Asignacion") = tbCantidad.Text

                                        dtDistribucionPorFondo.Rows.Clear() 'Pues ahora solo consideramos distribuciones individuales
                                        dtDistribucionPorFondo.Rows.Add(rowPorFondo)

                                        dtDistribucionPorFondoTotal.Rows.Add(rowPorFondo.ItemArray) 'Unificamos todo en una sola lista
                                    End If
                                    'FIN | PROYECTO FONDOS II - ZOLUXIONES | CRumiche | 2018-08-06 | Corrección de Distribución x Fondo


                                    Dim Mensaje As String = ""
                                    If (hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or _
                                        hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) Then
                                        precioOperacion = Val(tbPrecio.Text)
                                        totalOperacionRF = precioOperacion * CType(tbCantidad.Text, Decimal)
                                        precioEjecutado = Val(tbPrecioOperacion.Text)
                                        totalEjecutadoRF = precioEjecutado * CType(tbCantidadOperacion.Text, Decimal)
                                    Else
                                        tasa = CType(tbTasa.Text, Decimal)
                                        'OT 10090 - 26/07/2017 - Carlos Espejo
                                        'Descripcion: En base al tipo de calculo se usa la formula
                                        If ddlIndice.SelectedValue = "P" Then
                                            precioOperacion = Val(tbPrecio.Text)
                                            totalOperacionRF = OrdenInversion.CalculaMontoOperacion(tbNemonico.Text, tbCantidad.Text, ddlIndice.SelectedValue, _
                                            strFechaLiquidacion, ddlTipoTasa.SelectedValue, strFechaOperacion, precioOperacion, tasa, Mensaje, DatosRequest).ToString()
                                            'precioEjecutado = Val(tbPrecioOperacion.Text)
                                            precioEjecutado = Val(tbPrecio.Text) 'OT10795
                                            totalEjecutadoRF = OrdenInversion.CalculaMontoOperacion(tbNemonico.Text, tbCantidadOperacion.Text, ddlIndice.SelectedValue, _
                                            strFechaLiquidacion, ddlTipoTasa.SelectedValue, strFechaOperacion, precioEjecutado, tasa, Mensaje, DatosRequest).ToString()
                                        Else
                                            '/*INICIO: Proy SIT- FONDOS II - CRumiche: 2018-05-30 */
                                            Dim dsValor As DataSet = New PrevOrdenInversionBM().SeleccionarCaracValor(tbNemonico.Text, UIUtility.ConvertirFechaaDecimal(strFechaOperacion))
                                            If dsValor.Tables.Count < 2 Then Throw New Exception("Error: No se ha podido obtener el detalle de cupones del Valor")

                                            Dim dtCaracValor As DataTable = dsValor.Tables(0)
                                            Dim dtDetalleCupones As DataTable = dsValor.Tables(1)

                                            Dim rowValor As DataRow = dtCaracValor.Rows(0)
                                            Dim claseInstrumento As String = rowValor("CategoriaClaseInstrumento")

                                            If claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_BONO Or
                                                claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_CERTIFICADO_DEPOSITO Or
                                                claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_PAPELES_COMERCIALES Or
                                                claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_LETRAS_HIPOTECARIAS Then 'Versión nueva del Cálculo de Renta Fija (CRumiche)

                                                Dim vacEmision As Decimal = 1, vacEvaluacion As Decimal = 1

                                                If rowValor("CodigoMoneda").ToString.Equals("VAC") Then
                                                    ' Obtencion de valores VAC para los Bonos que aplique
                                                    UIUtility.ObtenerValoresVAC(rowValor("FechaEmision"),
                                                                      UIUtility.ConvertirFechaaDecimal(strFechaOperacion),
                                                                      vacEmision,
                                                                      vacEvaluacion,
                                                                      DatosRequest)
                                                End If

                                                Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(rowValor("BaseCuponMensual"))
                                                Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(rowValor("BaseCuponAnual"))
                                                Dim aplicacionTasa As TipoAplicacionTasa = UIUtility.ObtenerTipoAplicacionTasaDesdeCodTipoTasa(ddlTipoTasa.SelectedValue)

                                                Dim EsCuponADescuento As Boolean = rowValor("codigoTipoCupon").ToString.Equals("3") ' Es cupón A DESCUENTO solo si CodigoTipoCupon = 3 
                                                Dim esValorExtranjero As Boolean = rowValor("CodigoMercado").ToString.Equals("2")
                                                'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
                                                Dim esCalculoTBill As Boolean = rowValor("CodigoTipoInstrumentoSBS").ToString.Equals("100")

                                                Dim tasaYTM As Decimal = Val(tbTasa.Text)
                                                totalOperacionRF = Convert.ToDecimal(tbCantidad.Text) * Convert.ToDecimal(rowValor("ValorUnitario"))

                                                Dim neg As NegociacionRentaFija

                                                'Pasamos a calcular la negociacion (El motor hará todo el trabajo en tiempo real y en la capa de Negocio)
                                                neg = UIUtility.CalcularNegociacionRentaFija(dtDetalleCupones,
                                                                                                    CDec(rowValor("TasaCupon")),
                                                                                                    CInt(rowValor("DiasPeriodicidad")),
                                                                                                    CDec(rowValor("ValorUnitario")),
                                                                                                    CDec(tbCantidad.Text),
                                                                                                    Convert.ToDateTime(strFechaLiquidacion),
                                                                                                    tasaYTM,
                                                                                                    baseMensual,
                                                                                                    baseAnual,
                                                                                                    EsCuponADescuento,
                                                                                                    esValorExtranjero,
                                                                                                    aplicacionTasa,
                                                                                                    vacEmision,
                                                                                                    vacEvaluacion,
                                                                                                    esCalculoTBill
                                                                                                    )

                                                'Mostramos los resultados
                                                ValorActual = neg.ValorActual
                                                InteresCorrido = neg.InteresCorrido
                                                totalEjecutadoRF = neg.ValorActual 'Flujo
                                                precioOperacion = neg.PrecioSucio * 100 '% (Es un Porcentaje)
                                                precioEjecutado = precioOperacion
                                                '/*FIN: Proy SIT- FONDOS II - CRumiche: 2018-05-30 */

                                            Else 'VERSION ANTERIOR AL PROYETO SIT-FONDOS-II
                                                'OT10795 06/10/2017
                                                'totalOperacionRF = CDec(tbCantidad.Text) * objFormulasOI.ValorMercado(tbNemonico.Text)
                                                totalOperacionRF = Convert.ToDecimal(tbCantidad.Text) * Convert.ToDecimal(dtCaracValor.Rows(0)("ValorUnitario")) * Convert.ToDecimal(dtCaracValor.Rows(0)("AmortizacionPendiente"))
                                                'OT10795 - Fin
                                                Dim DTPrecio As DataTable = objFormulasOI.CalcularPrecioBono("", tbNemonico.Text,
                                                UIUtility.ConvertirFechaaDecimal(strFechaOperacion), UIUtility.ConvertirFechaaDecimal(strFechaLiquidacion), tasa,
                                                totalOperacionRF, ddlTipoTasa.SelectedValue, ddlOperacion.SelectedValue)
                                                'Precio de Negociacion Limpio
                                                precioOperacion = Convert.ToDecimal(DTPrecio(0)(0).ToString())
                                                precioEjecutado = precioOperacion
                                                'Interes Corridos
                                                InteresCorrido = Convert.ToDecimal(DTPrecio(0)(2).ToString())
                                                'Valor actual
                                                ValorActual = Convert.ToDecimal(DTPrecio(0)(3).ToString())
                                                If ValorActual > totalOperacionRF Then
                                                    totalEjecutadoRF = precioOperacion / 100 * totalOperacionRF + InteresCorrido
                                                Else
                                                    totalEjecutadoRF = ValorActual - InteresCorrido
                                                End If
                                            End If
                                        End If
                                        'OT 10090 Fin
                                        If Mensaje.Length > 0 Then
                                            AlertaJS(Mensaje)
                                            Exit Sub
                                        End If
                                    End If

                                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                                    oRow.CodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                    oRow.HoraOperacion = Now.ToLongTimeString()
                                    oRow.CodigoNemonico = tbNemonico.Text
                                    oRow.CodigoOperacion = ddlOperacion.SelectedValue
                                    oRow.CodigoTercero = hdIntermediario.Value
                                    oRow.CodigoPlaza = ddlPlazaN.SelectedValue
                                    oRow.MedioNegociacion = ddlMedioNeg.SelectedValue
                                    oRow.TipoCondicion = ddlCondicion.SelectedValue

                                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(strFechaLiquidacion)

                                    oRow.TipoTasa = ddlTipoTasa.SelectedValue
                                    oRow.Tasa = tasa
                                    oRow.Cantidad = CType(tbCantidad.Text, Decimal)
                                    oRow.Precio = precioOperacion
                                    oRow.MontoNominal = totalOperacionRF
                                    oRow.CantidadOperacion = CType(tbCantidadOperacion.Text, Decimal)
                                    oRow.PrecioOperacion = precioEjecutado
                                    oRow.MontoOperacion = totalEjecutadoRF
                                    oRow.InteresCorrido = InteresCorrido
                                    oRow.IndPrecioTasa = ddlIndice.SelectedValue
                                    oRow.TipoFondo = IIf(ddlTipoFondo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoFondo.SelectedValue)
                                    oRow.TipoTramo = IIf(ddlTipoTramo.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoTramo.SelectedValue)
                                    oRow.Porcentaje = porcentaje
                                    oRow.Fixing = fixing
                                    oRow.TipoValorizacion = hdTipoValorizacion.Value
                                    oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(strFechaOperacion)

                                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()

                                    If hdCambioTraza.Value = "1" Then
                                        hdCambioTraza.Value = ""
                                        If dtDistribucionPorFondo.Rows.Count > 0 Then
                                            For Each dr As DataRow In dtDistribucionPorFondo.Rows
                                                _filaTraz = oTrazabilidadOperacionBE.TrazabilidadOperacion.NewTrazabilidadOperacionRow()
                                                AgregarFilaTrazabilidad(_filaTraz, fila, lbCodigoPrevOrden.Text, tbNemonico.Text, ddlOperacion.SelectedValue, _
                                                tbCantidad.Text, tbPrecio.Text, hdIntermediario.Value, tbCantidadOperacion.Text, precioEjecutado, _
                                                dr("CodigoPortafolio"), dr("Asignacion").ToString.Trim)
                                                oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(_filaTraz)
                                            Next
                                        Else
                                            _filaTraz = oTrazabilidadOperacionBE.TrazabilidadOperacion.NewTrazabilidadOperacionRow()
                                            AgregarFilaTrazabilidad(_filaTraz, fila, lbCodigoPrevOrden.Text, tbNemonico.Text, ddlOperacion.SelectedValue, tbCantidad.Text, _
                                            tbPrecio.Text, hdIntermediario.Value, tbCantidadOperacion.Text, precioEjecutado, "", 0)
                                            oTrazabilidadOperacionBE.TrazabilidadOperacion.AddTrazabilidadOperacionRow(_filaTraz)
                                        End If
                                        oTrazabilidadOperacionBE.TrazabilidadOperacion.AcceptChanges()
                                        oPrevOrdenInversionBM.InsertarTrazabilidad_sura(oTrazabilidadOperacionBE, PROCESO_TRAZA1, DatosRequest)
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If
            Next

            If oPrevOrdenInversionBE.PrevOrdenInversion.Rows.Count > 0 Then
                oPrevOrdenInversionBM.Modificar(oPrevOrdenInversionBE, DatosRequest)

                '1: La Elminacion es necsaria por bucle FOR SEPARADO pues podría haber distribución por Fondo en cada transacción
                For Each dr As DataRow In dtDistribucionPorFondoTotal.Rows
                    oPrevOrdenInversionBM.eliminarDetalle(Integer.Parse(dr("CodigoPrevOrden").ToString.Trim), dr("CodigoPortafolio").ToString.Trim)
                Next
                '2: Ahora ACTUALIZAMOS la distribución x Fondos
                For Each dr As DataRow In dtDistribucionPorFondoTotal.Rows
                    If Decimal.Parse(dr("Asignacion").ToString.Trim) <> 0 Then
                        oPrevOrdenInversionBM.insertarDetalle(Integer.Parse(dr("CodigoPrevOrden").ToString.Trim),
                                                                dr("CodigoPortafolio").ToString.Trim,
                                                                Decimal.Parse(dr("Asignacion").ToString.Trim))
                    End If
                Next
            End If

            CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), _
                         ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))

            AlertaJS("Se guardó correctamente.")

        Catch uex As UserInfoException
            AlertaJS(Replace(uex.Message.ToString(), "'", ""))
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            btnGrabar.Text = "Grabar"
            btnGrabar.Enabled = True
        End Try
    End Sub
    Private Sub TotalizarDetalleInversion(ByRef dtTotal As DataTable, ByVal dt As DataTable)
        If dtTotal.Rows.Count = 0 Then
            dtTotal = dt.Copy
        Else
            For Each dr As DataRow In dt.Rows
                dtTotal.ImportRow(dr)
            Next
        End If
    End Sub
    Private Sub AgregarFilaTrazabilidad(ByRef _fila As TrazabilidadOperacionBE.TrazabilidadOperacionRow, ByVal fila As GridViewRow, ByVal CodigoPrevOrden As String, _
    ByVal Nemonico As String, ByVal Operacion As String, ByVal Cantidad As String, ByVal Precio As String, ByVal Intermediario As String, _
    ByVal CantidadOperacion As String, ByVal precioEjecutado As String, ByVal CodigoPortafolio As String, ByVal Asignacion As String)
        _fila.FechaOperacion = ViewState("decFechaOperacion").ToString()
        _fila.Correlativo = fila.Cells(1).Text
        _fila.Estado = fila.Cells(2).Text
        _fila.TipoOperacion = TIPO_OPER_PREVORDEN
        _fila.CodigoPrevOrden = CType(CodigoPrevOrden, Decimal)
        _fila.CodigoOrden = String.Empty
        _fila.CodigoNemonico = Nemonico
        _fila.CodigoOperacion = Operacion
        _fila.Cantidad = CType(Cantidad, Decimal)
        _fila.Precio = CType(Precio, Decimal)
        _fila.CodigoTercero = Intermediario
        _fila.CantidadEjecucion = CType(CantidadOperacion, Decimal)
        _fila.PrecioEjecucion = precioEjecutado
        _fila.ModoIngreso = MODO_ING_MASIVO
        _fila.Proceso = _PROCESO_TRAZA.Grabar
        _fila.MotivoCambio = MOTIVO_MODIFICAR_TRAZA
        _fila.Comentarios = COMENTARIO_MODIFICA_TRAZA
        _fila.CodigoPortafolio = CodigoPortafolio
        _fila.Asignacion = CType(Asignacion, Decimal)
    End Sub
    Private Sub btnValidar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidar.Click
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
        Dim count As Integer = 0
        Dim decNProceso As Decimal = 0
        Dim strCodPrevOrden As String = ""
        Dim arrCodPrevOrden As Array
        Try
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    If fila.Cells(2).Text <> PREV_OI_EJECUTADO Then
                        If Not lbCodigoPrevOrden Is Nothing Then
                            'PROCESAR VALIDACION
                            Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                            Dim decCodigoPrevOrden As Decimal
                            If chkSelect.Checked = True Then
                                decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                                count = count + 1
                                strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text.Trim & "|"
                            End If
                        End If
                    End If
                End If
            Next
            'SE PROCEDE A VALIDAR
            If count > 0 Then
                Dim mensaje As String = Limites.VerificaExcesoLimites(Usuario, ParametrosSIT.TR_RENTA_FIJA.ToString(), decNProceso)
                Dim dt As New DataTable
                dt = oPrevOrdenInversionBM.SeleccionarValidacionExcesos(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), _
                DatosRequest, decNProceso).Tables(0)
                If dt.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dt
                    AlertaJS(mensaje, "window.showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" + ParametrosSIT.TR_RENTA_FIJA.ToString() + _
                    "','650','450','" & btnBuscar.ClientID & "');")
                Else
                    AlertaJS(mensaje)
                End If
            Else
                AlertaJS("Seleccione los registros a validar")
                'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'OT-10784
            End If
            'OT-10784 Inicio
            'arrCodPrevOrden = strCodPrevOrden.Split("|")
            'For i = 0 To arrCodPrevOrden.Length - 1
            '    If arrCodPrevOrden(i) <> "" Then
            '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
            '    End If
            'Next
            'OT-10784 Fin
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        Finally
            'OT-10784 Inicio
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
        End Try
    End Sub
    Private Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarReporteRentaFija()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub GenerarReporteRentaFija()
        'OT10689 - Inicio. Kill process excel
        Dim ObjCom As UIUtility.COMObjectAplication = Nothing
        Dim oldCulture As CultureInfo
        Dim oExcel As Excel.Application
        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet, oSheetSBS As Excel.Worksheet
        Dim oCells As Excel.Range

        Dim sFile As String, sTemplate As String
        Dim dtRentaFija As New DataTable
        Dim dtResumen As New DataTable
        Dim dtResumenSBS As New DataTable
        Dim oDs As New DataSet
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim decNProceso As Decimal = 0
        Try
            ObjCom = New UIUtility.COMObjectAplication("Excel.Application", "EXCEL")
            oExcel = CType(ObjCom.ObjetoAplication, Excel.Application)
            oldCulture = Thread.CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            oDs = oPrevOrdenInversionBM.GenerarReporte(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), DatosRequest, decNProceso)
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            dtRentaFija = oDs.Tables(0)
            dtResumenSBS = oDs.Tables(4)
            sFile = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RF_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", _
            DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".xls"
            Dim n As Integer
            Dim n2 As Long
            Dim dr As DataRow

            If File.Exists(sFile) Then File.Delete(sFile)
            sTemplate = RutaPlantillas() & "\" & "PlantillaPrevOrdenInversionRF.xls"
            oExcel.Visible = False : oExcel.DisplayAlerts = False
            oBooks = oExcel.Workbooks
            oBooks.Open(sTemplate)
            oBook = oBooks.Item(1)
            oSheets = oBook.Worksheets
            oSheet = CType(oSheets.Item(1), Excel.Worksheet)
            oCells = oSheet.Cells
            oCells(2, 2) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheet.SaveAs(sFile)
            n = 8
            Dim codpreorden As String = ""
            For Each dr In dtRentaFija.Rows
                n2 = n + 1
                oSheet.Rows(n & ":" & n).Copy()
                oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                oSheet.Application.CutCopyMode = False
                If Not codpreorden = dr("CodigoPrevOrden") Then
                    oCells(n, 1) = dr("Correlativo")
                    oCells(n, 2) = dr("HoraOperacion")
                    oCells(n, 3) = dr("UsuarioCreacion")
                    oCells(n, 4) = dr("CodigoNemonico")
                    oCells(n, 5) = dr("Instrumento")
                    oCells(n, 6) = dr("Operacion")
                    oCells(n, 7) = dr("Cantidad")
                    oCells(n, 8) = dr("Precio")
                    oCells(n, 9) = CType(dr("Tasa"), String)
                    oCells(n, 10) = dr("Condicion")
                    oCells(n, 11) = dr("Intermediario")
                    oCells(n, 12) = dr("MedioNegociacion")
                    oCells(n, 14) = dr("IntervaloPrecio")
                    oCells(n, 15) = dr("CantidadOperacion")
                    oCells(n, 16) = dr("PrecioOperacion")
                    oCells(n, 17) = CType(dr("Tasa"), String)
                    oCells(n, 21) = dr("Ejecutado")
                    oSheet.Range("A" + n.ToString() + ":U" + n.ToString()).Interior.Color = RGB(215, 211, 211)
                    n = n + 1
                    n2 = n + 1
                    oSheet.Rows(n & ":" & n).Copy()
                    oSheet.Rows(n2 & ":" & n2).Insert(Excel.XlDirection.xlDown)
                    oSheet.Application.CutCopyMode = False
                    oCells(n, 18) = dr("Portafolico")
                    If dr("Porcetaje") = "S" Then
                        oCells(n, 20) = CDec(dr("Asignacion")) / 100
                    Else
                        oCells(n, 20) = dr("Asignacion")
                    End If
                Else
                    oCells(n, 18) = dr("Portafolico")
                    oCells(n, 20) = CDec(dr("Asignacion")) / 100
                    If dr("Porcetaje") = "S" Then
                        oCells(n, 20) = CDec(dr("Asignacion")) / 100
                    Else
                        oCells(n, 20) = dr("Asignacion")
                    End If
                End If
                If dr("Porcetaje") = "N" Then
                    oCells(n, 18).NumberFormat = "###,###,##0.0000"
                    oCells(n, 20).NumberFormat = "###,###,##0.0000"
                End If
                codpreorden = dr("CodigoPrevOrden")
                n = n + 1
            Next
            oSheet.Rows(n & ":" & n).Delete(Excel.XlDirection.xlUp)
            oSheetSBS = CType(oSheets.Item(2), Excel.Worksheet)
            oCells = oSheetSBS.Cells
            oCells(2, 12) = UIUtility.ConvertirFechaaString(ViewState("decFechaOperacion"))
            oSheetSBS.SaveAs(sFile)
            n = 6
            For Each dr In dtResumenSBS.Rows
                oCells(n, 2) = dr("Afp")
                oCells(n, 3) = dr("Fondo")
                oCells(n, 4) = dr("Nemonico")
                oCells(n, 5) = dr("HoraOrden")
                oCells(n, 6) = dr("HoraOperacion")
                oCells(n, 7) = dr("Movimiento")
                oCells(n, 8) = dr("Cantidad")
                oCells(n, 8).NumberFormat = "###,###,##0"
                oCells(n, 9) = dr("Precio")
                oCells(n, 9).NumberFormat = "###,###,##0.0000"
                oCells(n, 10) = dr("Intermediario")
                oCells(n, 11) = dr("Resultado")
                oCells(n, 12) = dr("NumeroOperacion")
                oCells(n, 13) = dr("MedioTransmision")
                n = n + 1
            Next
            oBook.Save()
            oBook.Close()
            Response.Clear()
            Response.ContentType = "application/xls"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "FX_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", DateTime.Today) & _
            String.Format("{0:HHMMss}", DateTime.Now) & ".xls")
            Response.WriteFile(sFile)
            Response.End()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
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
    Private Sub CargarPagina()
        CargarCombos()
        hdFechaNegocio.Value = UIUtility.ObtenerFechaMaximaNegocio()
        tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(hdFechaNegocio.Value)
        ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
        ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
        ViewState("strOperador") = ddlOperador.SelectedValue
        ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
        ViewState("strEstado") = ddlEstado.SelectedValue
        Dim dtOperacion As DataTable
        Dim oOperacionBM As New OperacionBM
        dtOperacion = oOperacionBM.SeleccionarporClaseinstrumento("OperacionOI", ParametrosSIT.ESTADO_ACTIVO).Tables(0)
        Session("dtOperacion") = dtOperacion
        Dim dtIntermediario As DataTable
        Dim oTercerosBM As New TercerosBM
        dtIntermediario = oTercerosBM.ListarTerceroPorGrupoIntermediario(ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO, "").Tables(0)
        Session("dtIntermediario") = dtIntermediario
        Dim dtMedioNeg As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtMedioNeg = oParametrosGeneralesBM.ListarMedioNegociacionPrevOI(ParametrosSIT.TR_RENTA_FIJA).Tables(0)
        Session("dtMedioNeg") = dtMedioNeg
        Dim dtCondicion As DataTable
        dtCondicion = oParametrosGeneralesBM.ListarCondicionPrevOI().Tables(0)
        Session("dtCondicion") = dtCondicion
        Dim dtPlazaN As DataTable
        Dim oPlazaBM As New PlazaBM
        dtPlazaN = oPlazaBM.Listar(DatosRequest).Tables(0)
        Session("dtPlazaN") = dtPlazaN
        Dim dtTipoTasa As DataTable
        dtTipoTasa = oParametrosGeneralesBM.Listar("TipoTasaI", DatosRequest)
        Session("dtTipoTasa") = dtTipoTasa
        Dim dtIndice As DataTable
        dtIndice = oParametrosGeneralesBM.Listar(ParametrosSIT.INDICE_PRECIO_TASA, DatosRequest)
        Session("dtIndice") = dtIndice
        Dim dtTipoFondo As DataTable
        dtTipoFondo = New ParametrosGeneralesBM().ListarFondosInversion(DatosRequest, "M")
        Session("dtTipoFondo") = dtTipoFondo
        Dim dtTipoTramo As DataTable
        dtTipoTramo = New ParametrosGeneralesBM().Listar("TIPOTRAMO", DatosRequest)
        Session("dtTipoTramo") = dtTipoTramo
        CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"))
        hdPuedeNegociar.Value = New PersonalBM().VerificaPermisoNegociacion(Usuario)
        If hdPuedeNegociar.Value = "0" Then
            HabilitaControles(False)
        End If
    End Sub
    Private Sub CargarCombos()
        CargarOperadores()
        CargaClaseInstrumento()
        CargarTipoInstrumento("")
        CargarEstados()
    End Sub
    Private Sub CargarGrilla(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, _
        Optional ByVal strCodigoClaseInstrumento As String = "", _
        Optional ByVal strCodigoTipoInstrumentoSBS As String = "", _
        Optional ByVal strCodigoNemonico As String = "", _
        Optional ByVal strOperador As String = "", _
        Optional ByVal strEstado As String = "")
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As DataSet
        ds = oPrevOrdenInversionBM.SeleccionarPorFiltro(strTipoRenta, decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, _
        strCodigoNemonico, strOperador, strEstado, DatosRequest)
        hdGrillaRegistros.Value = ds.Tables(0).Rows.Count
        If ds.Tables(0).Rows.Count = 0 Then
            llenarFilaVacia(ds)
            Datagrid1.DataSource = ds
            Datagrid1.DataBind()
            Datagrid1.Rows(0).Visible = False
        Else
            Datagrid1.DataSource = ds
            Datagrid1.DataBind()
        End If
    End Sub
    Private Sub CargarEstados()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As New DataTable
        dt = oParametrosGeneralesBM.Listar(ParametrosSIT.ESTADO_PREV_OI, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, dt, "Valor", "Nombre", True)
        ddlEstado.SelectedIndex = 0
    End Sub
    Private Sub CargarOperadores()
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim dt As DataTable
        dt = oPrevOrdenInversionBM.SeleccionarOperadores(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlOperador, dt, "UsuarioCreacion", "UsuarioCreacion", True)
        ddlOperador.SelectedIndex = 0
    End Sub
    Private Sub CargaClaseInstrumento()
        'CLASE INSTRUMENTO
        Dim oClaseInstrumentoBM As New ClaseInstrumentoBM
        Dim dsClaseInstrumento As DataSet
        dsClaseInstrumento = oClaseInstrumentoBM.SeleccionarClaseInstrumentoPorTipoRenta(ParametrosSIT.TR_RENTA_FIJA, DatosRequest)
        HelpCombo.LlenarComboBox(ddlClaseInstrumento, dsClaseInstrumento.Tables(0), "Codigo", "Descripcion", True)
        ddlClaseInstrumento.SelectedIndex = 0
    End Sub
    Private Sub CargarTipoInstrumento(ByVal codigoClaseInstrumento As String)
        'TIPO INSTRUMENTO
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim dt As New DataTable
        dt = oTipoInstrumentoBM.SeleccionarPorFiltro(codigoClaseInstrumento, "", ParametrosSIT.TR_RENTA_FIJA, "A", DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoInstrumento, dt, "CodigoTipoInstrumentoSBS", "Descripcion", True)
        ddlTipoInstrumento.SelectedIndex = 0
    End Sub
    Private Function ValidarAsignaciones(ByVal decAsignacionF1 As Decimal, ByVal decAsignacionF2 As Decimal, ByVal decAsignacionF3 As Decimal, _
    ByVal decCantidadOperacion As Decimal) As Boolean
        Dim bolResult As Boolean = False
        Dim decSumAsignaciones As Decimal = 0
        decSumAsignaciones = decAsignacionF1 + decAsignacionF2 + decAsignacionF3
        If decSumAsignaciones <> 0 Then
            If decSumAsignaciones = 100 Then
                'ASIGNACION DE PORCENTAJE
                bolResult = True
            Else
                'ASIGNACION DE UNIDADES
                If decSumAsignaciones = decCantidadOperacion Then
                    bolResult = True
                End If
            End If
        End If
        Return bolResult
    End Function
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Private Sub HabilitaControles(ByVal habilita As Boolean, Optional ByVal AntesApertura As Boolean = False)
        '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | El contenido de este método pasa a ser obsoleto pues cada negociación estará HABILITADA o NO según su ESTADO

        'btnGrabar.Enabled = habilita
        'btnValidar.Enabled = habilita
        'btnValidarTrader.Enabled = habilita
        'btnAprobar.Enabled = habilita
        'Datagrid1.Enabled = habilita
        'If AntesApertura Then
        '    btnGrabar.Enabled = True
        '    Datagrid1.Enabled = True
        'End If

        '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | El contenido de este método pasa a ser obsoleto pues cada negociación estará HABILITADA o NO según su ESTADO
    End Sub

    Private Sub btnBuscar_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-09-25 | Los filtros de búsqueda no deben de alterarse

            Dim decFechaOperacion As Decimal = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
            Dim strCodigoClaseInstrumento As String = ddlClaseInstrumento.SelectedValue
            Dim strCodigoTipoInstrumentoSBS As String = ddlTipoInstrumento.SelectedValue
            Dim strOperador As String = ddlOperador.SelectedValue
            Dim strCodigoNemonico As String = tbCodigoMnemonico.Text
            Dim strEstado As String = ddlEstado.SelectedValue

            ViewState("decFechaOperacion") = decFechaOperacion
            ViewState("strClaseInstrumento") = ddlClaseInstrumento.SelectedValue
            ViewState("strTipoInstrumento") = ddlTipoInstrumento.SelectedValue
            ViewState("strOperador") = ddlOperador.SelectedValue
            ViewState("strCodigoNemonico") = tbCodigoMnemonico.Text
            ViewState("strEstado") = ddlEstado.SelectedValue

            ''Dim decFechaActual As Decimal = Convert.ToDecimal(DateTime.Now.ToString("yyyyMMdd"))
            'If decFechaOperacion = hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then
            '    HabilitaControles(True)
            'ElseIf decFechaOperacion > hdFechaNegocio.Value And hdPuedeNegociar.Value = "1" Then
            '    HabilitaControles(False)
            'Else
            '    HabilitaControles(False)
            'End If
            '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-09-25 | Los filtros de búsqueda no deben de alterarse

            CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), decFechaOperacion, strCodigoClaseInstrumento, strCodigoTipoInstrumentoSBS, strCodigoNemonico, _
            strOperador, strEstado)
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Function ValidarNemonico(ByVal strNemonico As String) As Boolean
        Dim oValoresBM As New ValoresBM
        Dim bolResult As Boolean = False
        Dim strTipoRenta As String = ""
        strTipoRenta = oValoresBM.SeleccionarTipoRentaPorCodigoNemonico(strNemonico)
        If strTipoRenta <> "" Then
            If ParametrosSIT.TIPO_RENTA_FIJA.ToString.Replace("_", " ") = strTipoRenta Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function

    'INICIO | ZOLUXIONES | RCOLONIA | OT11698 - OC10284 | Validar las unidades negociadas del día | 06/12/18 
    Private Function ValidarSaldo(ByVal FechaOperacion As Decimal, ByVal strNemonico As String, ByVal strCodigoPortafolio As String, ByVal strCategoriaInstrumento As String, ByVal strOperacion As String, ByVal cantidad As Decimal, ByVal tipoGuardado As Integer, ByVal codigoOrden As Decimal) As Boolean
        Dim bolResult As Boolean = True
        Dim objValores As New ValoresBM
        Dim oPrevOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtSumaUnidades As DataTable
        Dim DSresultado = objValores.ListarPorFiltro(DatosRequest, strCategoriaInstrumento, String.Empty, String.Empty, strNemonico, strCodigoPortafolio, String.Empty, strOperacion)
        If strOperacion = OPERACION_VENTA Then
            If DSresultado.Tables(0).Rows.Count > 0 Then
                dtSumaUnidades = oPrevOrdenInversionBM.ObtenerUnidadesNegociadasDiaT(strCodigoPortafolio, FechaOperacion, strNemonico).Tables(0)
                If dtSumaUnidades.Rows.Count > 0 Then
                    If tipoGuardado = 1 Then
                        cantidad += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", String.Empty))
                    ElseIf tipoGuardado = 2 Then
                        cantidad += Decimal.Parse(dtSumaUnidades.Compute("Sum(UNIDADES)", "ID <> " & codigoOrden))
                    End If
                End If
                If CDec(DSresultado.Tables(0).Rows(0)(5)) < cantidad Then bolResult = False
            Else
                bolResult = False
            End If
        End If
        Return bolResult
    End Function
    'INICIO | ZOLUXIONES | RCOLONIA | OT11698 - OC10284 | Validar las unidades negociadas del día | 06/12/18 
    Private Function ValidarIntermediario(ByVal strCodigoTercero As String) As Boolean
        Dim oTercerosBM As New TercerosBM
        Dim oTercerosBE As New TercerosBE
        Dim bolResult As Boolean = False
        oTercerosBE = oTercerosBM.Seleccionar(strCodigoTercero, DatosRequest)
        If oTercerosBE.Tables(0).Rows.Count > 0 Then
            If ParametrosSIT.CLASIFICACIONTERCERO_INTERMEDIARIO = CType(oTercerosBE.Tables(0).Rows(0)("ClasificacionTercero"), String) Then
                bolResult = True
            End If
        End If
        Return bolResult
    End Function
    Private Function ValidarFechaVencimiento(ByVal fechaOperacion As String, ByVal fechaLiquidacion As String, ByVal strNemonico As String) As Boolean
        Dim feriado As New FeriadoBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim bolResult As Boolean = False
        Dim dtValor As DataTable = oPrevOrdenInversionBM.SeleccionarCaracValor(strNemonico, DatosRequest).Tables(0)

        If dtValor.Rows.Count > 0 Then
            If (feriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(fechaLiquidacion), dtValor.Rows(0)("CodigoMercado").ToString()) = True) Then
                Dim decFechaVenc As Decimal = UIUtility.ConvertirFechaaDecimal(fechaLiquidacion)
                'CRumiche: Corrección de la validación. Fecha Operación debe ser menor a la Fecha Liquidación : 2018-09-21
                Return (UIUtility.ConvertirFechaaDecimal(fechaOperacion) <= decFechaVenc)
            End If
        End If
        Return bolResult
    End Function
    Private Function ValidarFixing(ByVal claseInstrumento As String, ByVal nemonico As String, ByVal fixing As Decimal) As Boolean
        Dim bolValidaFixing As Boolean = True
        Dim oOrdenInversionFormulas As New OrdenInversionFormulasBM()
        Dim dtCaracValorBono As DataTable
        Dim monedaPago As String = ""
        Dim valCodigoMoneda As String = ""
        If claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_BONO Then
            dtCaracValorBono = oOrdenInversionFormulas.SeleccionarCaracValor_Bonos(nemonico, ParametrosSIT.PORTAFOLIO_FONDO1, DatosRequest).Tables(0)
            valCodigoMoneda = CType(dtCaracValorBono.Rows(0)("val_CodigoMoneda"), String)
            monedaPago = CType(dtCaracValorBono.Rows(0)("MonedaPago"), String)
            If valCodigoMoneda <> monedaPago Then
                If fixing <> -1 Then
                    bolValidaFixing = True
                Else
                    bolValidaFixing = False
                End If
            End If
        End If
        Return bolValidaFixing
    End Function
    Private Sub btnValidarTrader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidarTrader.Click
        'VALIDACION DE EXCESOS POR TRADER
        Dim dtValidaTrader As New DataTable
        Dim ds As New DataSet
        Dim oLimiteTradingBM As New LimiteTradingBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim decNProceso As Decimal = 0
        Dim strCodPrevOrden As String = ""
        Dim arrCodPrevOrden As Array
        Dim strEstadoPrevOrden As String = ""
        Try
            decNProceso = oPrevOrdenInversionBM.InsertarProcesoMasivo(Usuario)
            Dim count As Decimal = 0, contFechaDiferenteNegocio As Integer = 0, strMensajeDiferenteFechaNegocio As String = "<p align=left>"
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    Dim chkSelect As CheckBox = CType(fila.FindControl("chkSelect"), CheckBox)
                    Dim lbCodigoPrevOrden As Label = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                    Dim fondos As String = CType(fila.FindControl("ddlfondos"), DropDownList).SelectedItem.ToString
                    tFechaOperacion = CType(fila.FindControl("tFechaOperacion"), TextBox)
                    HdFondos = CType(fila.FindControl("Hdfondos"), HiddenField)
                    Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio(HdFondos.Value.Trim)
                    Dim decCodigoPrevOrden As Decimal
                    strEstadoPrevOrden = oPrevOrdenInversionBM.ObtenerEstadoPrevOrdenInversion(CType(lbCodigoPrevOrden.Text, Decimal), ds) 'agregado por JH , para no generar duplicados
                    If chkSelect.Checked = True Then
                        If fila.Cells(2).Text = ParametrosSIT.PREV_OI_INGRESADO Then
                            If UIUtility.ConvertirFechaaDecimal(tFechaOperacion.Text.Trim) <> fechaNegocio Then
                                contFechaDiferenteNegocio += 1
                                strMensajeDiferenteFechaNegocio += "-> N°: " + fila.Cells(1).Text + " / Portafolio: " + fondos + _
                                                                  "<br> F. Operación: " + tFechaOperacion.Text.Trim + _
                                                                  " / F. Portafolio: " + UIUtility.ConvertirFechaaString(fechaNegocio) + "<br><br>"
                            Else
                                decCodigoPrevOrden = CType(lbCodigoPrevOrden.Text, Decimal)
                                oPrevOrdenInversionBM.ProcesarEjecucion(decCodigoPrevOrden, DatosRequest, decNProceso)
                                count = count + 1
                                strCodPrevOrden = strCodPrevOrden & lbCodigoPrevOrden.Text & "|"
                            End If
                        End If
                    End If
                End If
            Next
            If contFechaDiferenteNegocio > 0 Then
                AlertaJS("Verificar las siguiente negociaciones, las fecha de operación no coincide con la fecha negocio del portafolio: " + _
                         strMensajeDiferenteFechaNegocio + "</p>")
            ElseIf count > 0 Then
                dtValidaTrader = oLimiteTradingBM.SeleccionarValidacionExcesosTrader_Sura(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), _
                Usuario, DatosRequest, , decNProceso).Tables(0)
                If dtValidaTrader.Rows.Count > 0 Then
                    Session("dtValidaTrader") = dtValidaTrader
                    EjecutarJS("window.showModalDialog('frmValidacionExcesosTrader.aspx?TipoRenta=" & ParametrosSIT.TR_RENTA_FIJA.ToString() & "&nProc=" & _
                    decNProceso.ToString() + "','650','450','" & btnBuscar.ClientID & "');")
                Else
                    AlertaJS("No se han podido evaluar los limites trader.")
                    'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'OT-10784
                End If
            Else
                AlertaJS("Seleccione el registro a validar ó el registro ya se encontraba aprobado!")
                CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                'oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso) 'OT-10784
            End If
            'OT-10784 Inicio
            'arrCodPrevOrden = strCodPrevOrden.Split("|")
            'For i = 0 To arrCodPrevOrden.Length - 1
            '    If arrCodPrevOrden(i) <> "" Then
            '        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
            '    End If
            'Next
            'OT-10784 Fin
        Catch ex As Exception
            AlertaJS(ex.Message.ToString().Replace(Environment.NewLine, "").Replace("'", ""))
        Finally
            'OT-10784 Inicio
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            arrCodPrevOrden = strCodPrevOrden.Split("|")
            For i = 0 To arrCodPrevOrden.Length - 1
                If arrCodPrevOrden(i) <> "" Then
                    oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
                End If
            Next
            'OT-10784 Fin
            btnValidarTrader.Text = "Validar Exc. Trader"
            btnValidarTrader.Enabled = True
        End Try
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM
        Dim ds As DataSet
        Dim dtOI As DataTable
        Dim strCodigoOrden As String = ""
        Dim strPortafolioSBS As String = ""
        Dim PortafolioSBS As String = ""
        Dim strMoneda As String = ""
        Dim strOperacion As String = ""
        Dim strCodigoISIN As String = ""
        Dim strCodigoSBS As String = ""
        Dim chkSelect As CheckBox
        Try
            For Each fila As GridViewRow In Datagrid1.Rows
                If fila.RowType = DataControlRowType.DataRow Then
                    chkSelect = CType(fila.FindControl("chkSelect"), CheckBox)
                    If fila.Cells(2).Text = PREV_OI_EJECUTADO And chkSelect.Checked = True Then
                        lbCodigoPrevOrden = CType(fila.FindControl("lbCodigoPrevOrden"), Label)
                        lbClase = CType(fila.FindControl("lbClase"), Label)
                        tbNemonico = CType(fila.FindControl("tbNemonico"), TextBox)
                        ds = oPrevOrdenInversion.SeleccionarImprimir_PrevOrdenInversion(lbCodigoPrevOrden.Text, DatosRequest)
                        dtOI = ds.Tables(0)
                        For Each fila2 As DataRow In dtOI.Rows
                            strCodigoOrden = fila2("CodigoOrden")
                            strPortafolioSBS = fila2("PortafolioSBS")
                            PortafolioSBS = fila2("CodigoPortafolioSBS")
                            strMoneda = fila2("Moneda")
                            strOperacion = fila2("Operacion")
                            strCodigoISIN = fila2("CodigoISIN")
                            strCodigoSBS = fila2("CodigoSBS")
                            Session("dtdatosoperacion") = Nothing
                            GenerarLlamado(strCodigoOrden, PortafolioSBS, strPortafolioSBS, lbClase.Text.ToUpper, strOperacion, strMoneda, _
                            strCodigoISIN, strCodigoSBS, tbNemonico.Text)
                        Next
                    End If
                End If
            Next
            CrearMultiCartaPDF(rutas.ToString())
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Public Sub GenerarLlamado(ByVal codigo As String, ByVal portafolio As String, ByVal dscportafolio As String, ByVal clase As String, ByVal operacion As String, _
    ByVal moneda As String, ByVal isin As String, ByVal sbs As String, ByVal mnemonico As String)
        Dim strtitulo, strnemonico, strisin, strsbs, strsubtitulo, strcodigo1, strcodigo, strportafolio, strclase, strmoneda, stroperacion, strmnemonicotemp As String
        Dim dscaract As New dscaracteristicas
        Dim dttempoperacion As DataTable
        Dim drcar As DataRow
        strclase = clase
        strnemonico = mnemonico
        strisin = isin
        strsbs = sbs
        strmnemonicotemp = mnemonico
        strportafolio = portafolio
        strcodigo1 = codigo
        Select Case clase.ToUpper
            Case "FONDOS MUTUOS EN EL EXTERIOR"
                strclase = "ORDENES DE FONDO"
            Case "FONDOS DE INVERSIÓN"
                strclase = "ORDENES DE FONDO"
        End Select

        Dim dsValor As New DataSet
        Dim oOIFormulas As New OrdenInversionFormulasBM
        If strclase = "BONOS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Bonos(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Bonos(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = ""
                drcar("Val3") = ""
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val5") = CType(drValor("val_NominalesUnitarias"), Integer)
                drcar("Car6") = ""
                drcar("Val6") = ""
                drcar("Car7") = ""
                drcar("Val7") = ""
                drcar("Car8") = ""
                drcar("Val8") = ""
                drcar("Car9") = ""
                drcar("Val9") = ""
                drcar("Car10") = ""
                drcar("Val10") = ""
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val13") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val14") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car15") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val15") = CType(drValor("val_TipoRenta"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "ACCIONES" Then
            dsValor = oOIFormulas.SeleccionarCaracValor_Acciones(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_sigDivFecha"), String)
                drcar("Val3") = CType(drValor("val_sigDivFecha"), String)
                drcar("Car4") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_sigDivFactor"), String)
                drcar("Val5") = CType(drValor("val_sigDivFactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = CType(drValor("lbl_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "CERTIFICADO DE DEPÓSITO" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_CertificadoDeposito(strcodigo1, _
            strportafolio, DatosRequest)
            'OT 10090 - 21/03/2017 - Carlos Espejo
            'Descripcion: Se elimian el parametro obsoleto
            dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoDeposito(strnemonico, strportafolio, strcodigo1)
            'OT 10090 Fin
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = ""
                drcar("Val7") = ""
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = ""
                drcar("Val10") = ""
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val15") = CType(drValor("val_CodigoSBS"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "CERTIFICADO DE SUSCRIPCION" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_CertificadoSuscripcion(strcodigo1, _
            strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_CertificadoSuscripcion(portafolio, strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_sigdivfecha"), String)
                drcar("Val3") = CType(drValor("val_sigdivfecha"), String)
                drcar("Car4") = CType(drValor("lbl_valorDFC"), String)
                drcar("Val4") = CType(drValor("val_valorDFC"), String)
                drcar("Car5") = CType(drValor("lbl_sigdivfactor"), String)
                drcar("Val5") = CType(drValor("val_sigdivfactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = CType(drValor("lbl_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_Priceearnings"), String)
                drcar("Val8") = CType(drValor("val_Priceearnings"), String)
                drcar("Car9") = CType(drValor("lbl_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = ""
                drcar("Val13") = ""
                drcar("Car14") = ""
                drcar("Val14") = ""
                drcar("Car15") = ""
                drcar("Val15") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "PAPELES COMERCIALES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_PapelesComerciales(strcodigo1, strportafolio, _
            DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_PapelesComerciales(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val15") = CType(drValor("val_CodigoISIN"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "PAGARES" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_Pagares(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_Pagares(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Emisor"), String)
                drcar("Val1") = CType(drValor("val_Emisor"), String)
                drcar("Car2") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_Serie"), String)
                drcar("Val3") = CType(drValor("val_Serie"), String)
                drcar("Car4") = CType(drValor("lbl_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_TasaCupon"), String)
                drcar("Val5") = CType(drValor("val_TasaCupon"), String)
                drcar("Car6") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val6") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car7") = CType(drValor("lbl_TipoCupon"), String)
                drcar("Val7") = CType(drValor("val_TipoCupon"), String)
                drcar("Car8") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val8") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car9") = CType(drValor("lbl_PeriodoPago"), String)
                drcar("Val9") = CType(drValor("val_PeriodoPago"), String)
                drcar("Car10") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val10") = CType(drValor("val_FechaVcto"), String)
                drcar("Car11") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_fc"), String)
                drcar("Val13") = CType(drValor("val_fc"), String)
                drcar("Car14") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val14") = CType(drValor("val_TipoRenta"), String)
                drcar("Car15") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val15") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "ORDENES DE FONDO" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_OrdenesFondo(strcodigo1, strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_OrdenesFondo(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinBono"), String)
                drcar("Val2") = CType(drValor("val_FechaFinBono"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car12") = CType(drValor("lbl_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_Duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val13") = CType(drValor("val_TipoRenta"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val14") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car15") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val15") = CType(drValor("val_CodigoISIN"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "DEPOSITOS A PLAZO" Then
            If Session("dtdatosoperacion") Is Nothing Then
                Dim dtOrden As DataTable = New OrdenPreOrdenInversionBM().ListarOrdenesInversionPorCodigoOrden(strcodigo1, strportafolio, DatosRequest, _
                PORTAFOLIO_MULTIFONDOS).Tables(0)
                Session("dtdatosoperacion") = New DepositoPlazos().ObtenerDatosOperacion(DatosRequest, dtOrden.Rows(0))
            End If

        End If
        If strclase = "OPERACIONES DE REPORTE" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_OperacionesReporte(strcodigo1, strportafolio, _
            DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_OperacionesReporte(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_MarketCap"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val2") = CType(drValor("val_porcFondo3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car3") = CType(drValor("lbl_sigDivFecha"), String)
                drcar("Val3") = CType(drValor("val_sigDivFecha"), String)
                drcar("Car4") = CType(drValor("lbl_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val4") = CType(drValor("val_MontoNegDiarProm"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car5") = CType(drValor("lbl_sigDivFactor"), String)
                drcar("Val5") = CType(drValor("val_sigDivFactor"), String)
                drcar("Car6") = CType(drValor("lbl_NroOperDiarProm"), String)
                drcar("Val6") = CType(drValor("val_NroOperDiarProm"), String)
                drcar("Car7") = CType(drValor("lbl_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_porcFondo1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_PriceEarnings"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_porcFondo2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val10") = CType(drValor("val_ValorDFC"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val13") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car14") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val14") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car15") = CType(drValor("lbl_Mercado"), String)
                drcar("Val15") = CType(drValor("val_Mercado"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "INSTRUMENTOS COBERTURADOS" Then
            dsValor = oOIFormulas.SeleccionarCaracValor_InstCoberturados(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_TipoRenta"), String)
                drcar("Val1") = CType(drValor("val_TipoRenta"), String)
                drcar("Car2") = CType(drValor("lbl_CodigoMoneda"), String)
                drcar("Val2") = CType(drValor("val_CodigoMoneda"), String)
                drcar("Car3") = CType(drValor("lbl_CodigoISIN"), String)
                drcar("Val3") = CType(drValor("val_CodigoISIN"), String)
                drcar("Car4") = CType(drValor("lbl_CodigoSBS"), String)
                drcar("Val4") = CType(drValor("val_CodigoSBS"), String)
                drcar("Car5") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val5") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car6") = CType(drValor("lbl_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_categoria"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_Emisor"), String)
                drcar("Val10") = CType(drValor("val_Emisor"), String)
                drcar("Car11") = CType(drValor("lbl_fc"), String)
                drcar("Val11") = CType(drValor("val_fc"), String)
                drcar("Car12") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val12") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car13") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val13") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car14") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val14") = CType(drValor("val_FechaVcto"), String)
                drcar("Car15") = CType(drValor("lbl_observacion"), String)
                drcar("Val15") = CType(drValor("val_observacion"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String).Replace(UIUtility.DecimalSeparator, ".")
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "LETRAS HIPOTECARIAS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_LetrasHipotecarias(strcodigo1, _
                strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_LetrasHipotecarias(strnemonico, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val1") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car2") = CType(drValor("lbl_FechaFinLetra"), String)
                drcar("Val2") = CType(drValor("val_FechaFinLetra"), String)
                drcar("Car3") = CType(drValor("lbl_NominalesEmitidos"), String)
                drcar("Val3") = CType(drValor("val_NominalesEmitidos"), String)
                drcar("Car4") = CType(drValor("lbl_Emisor"), String)
                drcar("Val4") = CType(drValor("val_Emisor"), String)
                drcar("Car5") = CType(drValor("lbl_PorParticipacion"), String)
                drcar("Val5") = CType(drValor("val_PorParticipacion"), String)
                drcar("Car6") = CType(drValor("lbl_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_BaseCupon"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_FechaUltCupon"), String)
                drcar("Val7") = CType(drValor("val_FechaUltCupon"), String)
                drcar("Car8") = CType(drValor("lbl_VectorPrecio"), String)
                drcar("Val8") = CType(drValor("val_VectorPrecio"), String)
                drcar("Car9") = CType(drValor("lbl_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val9") = CType(drValor("val_BaseTir"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car10") = CType(drValor("lbl_FechaProxCupon"), String)
                drcar("Val10") = CType(drValor("val_FechaProxCupon"), String)
                drcar("Car11") = CType(drValor("lbl_NominalesUnitarias"), String)
                drcar("Val11") = CType(drValor("val_NominalesUnitarias"), String)
                drcar("Car12") = CType(drValor("lbl_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val12") = CType(drValor("val_duracion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car13") = CType(drValor("lbl_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val13") = CType(drValor("val_emision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car14") = CType(drValor("lbl_fc"), String)
                drcar("Val14") = CType(drValor("val_fc"), String)
                drcar("Car15") = CType(drValor("lbl_FechaVcto"), String)
                drcar("Val15") = CType(drValor("val_FechaVcto"), String)
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        If strclase = "INSTRUMENTOS ESTRUCTURADOS" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = UIUtility.ObtenerDatosOperacion_InstrumentosEstructurados(strcodigo1, _
            strportafolio, DatosRequest)
            dsValor = oOIFormulas.SeleccionarCaracValor_InstEstructurados(strnemonico, strportafolio, DatosRequest)
            For Each drValor As DataRow In dsValor.Tables(0).Rows
                drcar = dscaract.Tables(0).NewRow
                drcar("Car1") = CType(drValor("lbl_nemo1"), String)
                drcar("Val1") = CType(drValor("val_nemo1"), String)
                drcar("Car2") = CType(drValor("lbl_nemo2"), String)
                drcar("Val2") = CType(drValor("val_nemo2"), String)
                drcar("Car3") = CType(drValor("lbl_nemo3"), String)
                drcar("Val3") = CType(drValor("val_nemo3"), String)
                drcar("Car4") = CType(drValor("lbl_porc1"), String)
                drcar("Val4") = CType(drValor("val_porc1"), String)
                drcar("Car5") = CType(drValor("lbl_porc2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val5") = CType(drValor("val_porc2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car6") = CType(drValor("lbl_porc3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val6") = CType(drValor("val_porc3"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car7") = CType(drValor("lbl_precio1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val7") = CType(drValor("val_precio1"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car8") = CType(drValor("lbl_precio2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val8") = CType(drValor("val_precio2"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car9") = CType(drValor("lbl_precio3"), String)
                drcar("Val9") = CType(drValor("val_precio3"), String)
                drcar("Car10") = CType(drValor("lbl_porcParticip"), String)
                drcar("Val10") = CType(drValor("val_porcParticip"), String)
                drcar("Car11") = ""
                drcar("Val11") = ""
                drcar("Car12") = ""
                drcar("Val12") = ""
                drcar("Car13") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val13") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car14") = CType(drValor("lbl_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Val14") = CType(drValor("val_Descripcion"), String).Replace(UIUtility.DecimalSeparator, ".")
                drcar("Car15") = ""
                drcar("Val15") = ""
                dscaract.Tables(0).Rows.Add(drcar)
            Next
        End If
        Dim sNNeg As String = ""
        If strclase.Substring(1) = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Or strclase.Substring(1) = "COMPRA/VENTA MONEDA EXTRANJERA" Then
            If Session("dtdatosoperacion") Is Nothing Then Session("dtdatosoperacion") = Session("dtdatosoperacionSW" & strclase.Substring(0, 1))
            sNNeg = strclase.Substring(0, 1)
            strclase = strclase.Substring(1)
        End If
        stroperacion = operacion
        Dim dsoper As New dsDatosOperacion
        Dim droper As DataRow
        dttempoperacion = CType(Session("dtdatosoperacion"), DataTable)
        For Each dr As DataRow In dttempoperacion.Rows
            droper = dsoper.Tables(0).NewRow
            droper("c1") = dr("c1")
            droper("v1") = dr("v1")
            droper("c2") = dr("c2")
            droper("v2") = dr("v2")
            droper("c3") = dr("c3")
            droper("v3") = dr("v3")
            droper("c4") = dr("c4")
            'si se realiza una compra de divisas (EUROS), debe aparecer: de Dólares a EUROS. 
            'Si se realiza una venta de EUROS (Venta de Divisas), debe aparecer “de: EUROS a: Dólares”
            If strclase.Equals("COMPRA/VENTA MONEDA EXTRANJERA") And stroperacion.ToLower.IndexOf("compra") >= 0 Then
                droper("v4") = dr("v6")
                droper("v6") = dr("v4")
            Else
                droper("v4") = dr("v4")
                droper("v6") = dr("v6")
            End If
            droper("c5") = dr("c5")
            droper("v5") = dr("v5")
            droper("c6") = dr("c6")
            droper("c7") = dr("c7")
            droper("v7") = dr("v7")
            droper("c8") = dr("c8")
            droper("v8") = dr("v8")
            droper("c9") = dr("c9")
            droper("v9") = dr("v9")
            droper("c10") = dr("c10")
            droper("v10") = dr("v10")
            droper("c11") = dr("c11")
            droper("v11") = dr("v11")
            droper("c12") = dr("c12")
            droper("v12") = dr("v12")
            droper("c13") = dr("c13")
            droper("v13") = dr("v13")
            droper("c14") = dr("c14")
            droper("v14") = dr("v14")
            droper("c15") = dr("c15")
            droper("v15") = dr("v15")
            droper("c16") = dr("c16")
            droper("v16") = dr("v16")
            droper("c17") = dr("c17")
            droper("v17") = dr("v17")
            droper("c18") = dr("c18")
            droper("v18") = dr("v18")
            droper("c19") = dr("c19")
            droper("v19") = dr("v19")
            droper("c20") = dr("c20")
            droper("v20") = dr("v20")
            droper("c21") = dr("c21")
            droper("v21") = dr("v21")
            If strclase = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Or strclase.Substring(1) = "OPERACIONES DERIVADAS - FORWARD DIVISAS" Then
                droper("c22") = dr("c22")
                droper("v22") = dr("v22")
                droper("v18") = dr("v18").ToString.Substring(0, IIf(dr("v18").ToString.Length >= 42, 42, dr("v18").ToString.Length))
            Else
                droper("c22") = ""
                droper("v22") = ""
            End If
            dsoper.Tables(0).Rows.Add(droper)
        Next
        Dim oStream As New System.IO.MemoryStream
        Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos\Inversiones\Llamado\RptLlamado.rpt"
        strcodigo = strcodigo1
        strmoneda = moneda
        '-----------------------------------------------------------------------------------------------
        'Validar si se trata de una Traspaso de Instrumento entre Fondos
        '-----------------------------------------------------------------------------------------------
        Dim ordenes As String() = strcodigo.Split("-")
        Dim ordenOrigen As String = String.Empty
        Dim ordenDestino As String = String.Empty
        Dim esTraspaso As Boolean = False
        If ordenes.Length > 1 Then
            esTraspaso = True
            ordenOrigen = ordenes(0).ToString()
            ordenDestino = ordenes(1).ToString()
        End If
        '-----------------------------------------------------------------------------------------------
        Try
            Dim StrNombre As String = "Usuario"
            Dim strusuario As String
            If esTraspaso Then
                strtitulo = "Orden de Inversion: (Origen) Nro - " + ordenOrigen + " , (Destino) Nro - " + ordenDestino
            Else
                strtitulo = "Orden de Inversion Nro - " + strcodigo
            End If
            strsubtitulo = strclase + " - IN-" + stroperacion
            Dim dscomisiones As New DataSet
            Dim dscomi As New dsdatoscomisiones
            Dim drcomi As DataRow
            dscomisiones = UIUtility.ObtenerTablaimpuestosComisionesGuardado(strcodigo, strportafolio)
            For Each drcomisiones As DataRow In dscomisiones.Tables(0).Rows
                drcomi = dscomi.Tables(0).NewRow
                drcomi("Descripcion1") = drcomisiones("Descripcion1")
                drcomi("ValorOcultoComision1") = drcomisiones("ValorOcultoComision1")
                drcomi("PorcentajeComision1") = drcomisiones("PorcentajeComision1") + " :"
                drcomi("Descripcion2") = drcomisiones("Descripcion2")
                drcomi("ValorOcultoComision2") = drcomisiones("ValorOcultoComision2")
                drcomi("PorcentajeComision2") = drcomisiones("PorcentajeComision2") + " :"
                dscomi.Tables(0).Rows.Add(drcomi)
            Next
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            strusuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)

            Using Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                Cro.Load(Archivo)
                Dim oOrdenInverion As New OrdenPreOrdenInversionBM()
                Dim dsFirma As New DsFirma
                Dim drFirma As DsFirma.FirmaRow
                Dim dr2 As DataRow
                Dim dtFirmas As New DataTable
                dtFirmas = oOrdenInverion.ObtenerFirmasLlamadoOI(ordenes(0).ToString(), UIUtility.ObtenerFechaMaximaNegocio(), DatosRequest).Tables(0)
                dr2 = dtFirmas.Rows(0)
                drFirma = CType(dsFirma.Firma.NewFirmaRow(), DsFirma.FirmaRow)
                drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(0), String)))
                drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(1), String)))
                drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(2), String)))
                drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(3), String)))
                drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(4), String)))
                drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(5), String)))
                drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(6), String)))
                drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(7), String)))
                drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(8), String)))
                drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(9), String)))
                drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(10), String)))
                drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(11), String)))
                drFirma.Firma13 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(12), String)))
                drFirma.Firma14 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(dr2(13), String)))
                dsFirma.Firma.AddFirmaRow(drFirma)
                dsFirma.Firma.AcceptChanges()
                Cro.SetDataSource(dsFirma)
                Cro.OpenSubreport("RptCaracteristicas").SetDataSource(dscaract)
                Cro.OpenSubreport("RptDatosOperacion").SetDataSource(dsoper)
                Cro.OpenSubreport("RptDatosComisiones").SetDataSource(dscomi)
                dsoper = New dsDatosOperacion
                Dim dttemptotal As DataTable = CType(Session("dtdatosoperacion"), DataTable)
                For Each dr As DataRow In dttemptotal.Rows
                    If CType(dr("c19"), String) = "" Then
                        Exit For
                    End If
                    droper = dsoper.Tables(0).NewRow
                    droper("c19") = dr("c19")
                    droper("v19") = dr("v19")
                    droper("c20") = dr("c20")
                    droper("v20") = dr("v20")
                    droper("c21") = dr("c21")
                    droper("v21") = dr("v21")
                    dsoper.Tables(0).Rows.Add(droper)
                Next
                Cro.OpenSubreport("RptTotalOperacion").SetDataSource(dsoper)
                Cro.SetParameterValue("@Ruta_Logo", System.Configuration.ConfigurationManager.AppSettings("RUTA_LOGO"))
                Cro.SetParameterValue("@Titulo", strtitulo)
                Cro.SetParameterValue("@Subtitulo", strsubtitulo)
                Cro.SetParameterValue("@Fondo", "Portafolio: " & dscportafolio)
                Cro.SetParameterValue("@Moneda", "Moneda: " & strmoneda)
                If esTraspaso Then
                    Cro.SetParameterValue("@Operacion", "Operación: " & stroperacion & "-" & Constantes.M_TRASPASO_INGRESO_)
                Else
                    Cro.SetParameterValue("@Operacion", "Operación: " & stroperacion)
                End If
                If strmnemonicotemp <> "" Or strmnemonicotemp <> Nothing Then
                    Cro.SetParameterValue("@MnemonicoReporte", "Mnemónico Temporal: " & strmnemonicotemp)
                Else
                    Cro.SetParameterValue("@MnemonicoReporte", "")
                End If
                If strisin <> "" Or strisin <> Nothing Then
                    Cro.SetParameterValue("@CodigoIsin", "Codigo Isin: " & strisin)
                Else
                    Cro.SetParameterValue("@CodigoIsin", "")
                End If
                If strsbs <> "" Or strsbs <> Nothing Then
                    Cro.SetParameterValue("@CodigoSBS", "Codigo SBS: " & strsbs)
                Else
                    Cro.SetParameterValue("@CodigoSBS", "")
                End If
                If strnemonico <> "" Or strnemonico <> Nothing Then
                    Cro.SetParameterValue("@CodigoNemonico", "Codigo Mnemónico: " & strnemonico)
                Else
                    Cro.SetParameterValue("@CodigoNemonico", "")
                End If
                Dim rutaArchivo As String
                If Not (Cro Is Nothing) Then
                    rutaArchivo = "c:\temp\" & strcodigo1 & " - " & Now.ToString(" - yyyyMMdd hhmmss") & ".pdf"
                    Cro.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                    rutas.Append("&" & rutaArchivo)
                    pRutas = rutas.ToString()
                End If

                Cro.Close()
            End Using

        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Public Sub CrearMultiCartaPDF(ByVal cartas As String)
        Dim destinoPdf As String, nombreNuevoArchivo As String
        Dim PrefijoFolder As String = "Llamado_"
        Dim fechaActual As String = System.DateTime.Now.ToString("yyyyMMdd")
        Dim sRutaTemp As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
        Dim foldersAsesoria() As String = Directory.GetDirectories(sRutaTemp, PrefijoFolder & "*")
        Dim folderActual As String = sRutaTemp & PrefijoFolder & fechaActual
        Dim cont As Integer
        Try
            For cont = 0 To foldersAsesoria.Length - 1
                If Not foldersAsesoria(cont).Equals(folderActual) Then
                    Try
                        Directory.Delete(foldersAsesoria(cont), True)
                    Catch ex As Exception
                    End Try
                End If
            Next
            If Not Directory.Exists(folderActual) Then
                Directory.CreateDirectory(folderActual)
            End If
            nombreNuevoArchivo = System.Guid.NewGuid().ToString() & ".pdf"
            destinoPdf = folderActual & "\" & nombreNuevoArchivo
            Dim sourceFiles() As String = cartas.Substring(1).Split("&")
            Dim f As Integer = 0
            Dim reader As PdfReader = New PdfReader(sourceFiles(f))
            Dim n As Integer = reader.NumberOfPages
            Dim document As Document = New Document(reader.GetPageSizeWithRotation(1))
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(destinoPdf, FileMode.Create))
            document.Open()
            Dim cb As PdfContentByte = writer.DirectContent
            Dim page As PdfImportedPage
            Dim rotation As Integer
            While (f < sourceFiles.Length)
                Dim i As Integer = 0
                While (i < n)
                    i += 1
                    document.SetPageSize(reader.GetPageSizeWithRotation(i))
                    document.NewPage()
                    page = writer.GetImportedPage(reader, i)
                    rotation = reader.GetPageRotation(i)
                    If rotation = 90 Or rotation = 270 Then
                        cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                    Else
                        cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                    End If
                End While
                f += 1
                If f < sourceFiles.Length Then
                    reader = New PdfReader(sourceFiles(f))
                    n = reader.NumberOfPages
                End If
            End While
            document.Close()
            Dim sfile As String
            sfile = folderActual & "\" & nombreNuevoArchivo
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nombreNuevoArchivo)
            Response.WriteFile(sfile)
            Response.End()
            For Each savedDoc As String In cartas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        Catch ex As Exception
            UIUtility.PublicarEvento("CrearMultiCartaPDF - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
            For Each savedDoc As String In cartas.Split(New Char() {"&"})
                If File.Exists(savedDoc) Then
                    File.Delete(savedDoc)
                End If
            Next
        Finally
        End Try
    End Sub

    'Private Sub GenerarReporteRentaFijaPDF()

    '    Using oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    '        Dim fechaNegocio As Decimal = UIUtility.ObtenerFechaNegocio("MULTIFONDO")
    '        Dim oPrevOrdenInversion As New PrevOrdenInversionBM()
    '        Dim fileReport As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/Reportes/rptReporteOperacionesMasivasRF.rpt"
    '        oReport.Load(fileReport)
    '        Dim dsAux As DataSet
    '        Dim dsReporteOperacionesMasivasRF As New DsReporteOperacionesMasivasRF
    '        dsAux = oPrevOrdenInversion.GenerarReporteConFirmas(ParametrosSIT.TR_RENTA_FIJA, fechaNegocio, DatosRequest)
    '        CopiarTabla(dsAux.Tables(0), dsReporteOperacionesMasivasRF.RegistroPrevio)
    '        'Firmas
    '        Dim drFirma As DsReporteOperacionesMasivasRF.FirmaRow
    '        Dim drRutaFirma As DataRow
    '        drRutaFirma = dsAux.Tables(1).Rows(0)
    '        drFirma = CType(dsReporteOperacionesMasivasRF.Firma.NewFirmaRow(), DsReporteOperacionesMasivasRF.FirmaRow)
    '        drFirma.Firma1 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(0), String)))
    '        drFirma.Firma2 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(1), String)))
    '        drFirma.Firma3 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(2), String)))
    '        drFirma.Firma4 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(3), String)))
    '        drFirma.Firma5 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(4), String)))
    '        drFirma.Firma6 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(5), String)))
    '        drFirma.Firma7 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(6), String)))
    '        drFirma.Firma8 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(7), String)))
    '        drFirma.Firma9 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(8), String)))
    '        drFirma.Firma10 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(9), String)))
    '        drFirma.Firma11 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(10), String)))
    '        drFirma.Firma12 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(11), String)))
    '        drFirma.Firma13 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(12), String)))
    '        drFirma.Firma14 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(13), String)))
    '        drFirma.Firma15 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(14), String)))
    '        drFirma.Firma16 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(15), String)))
    '        drFirma.Firma17 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(16), String)))
    '        drFirma.Firma18 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(17), String)))
    '        drFirma.Firma19 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(18), String)))
    '        drFirma.Firma20 = HelpImage.ImageToByteArray(HelpImage.ObtenerImagen(CType(drRutaFirma(19), String)))
    '        dsReporteOperacionesMasivasRF.Firma.AddFirmaRow(drFirma)
    '        dsReporteOperacionesMasivasRF.Firma.AcceptChanges()
    '        dsReporteOperacionesMasivasRF.Merge(dsReporteOperacionesMasivasRF, False, System.Data.MissingSchemaAction.Ignore)
    '        oReport.SetDataSource(dsReporteOperacionesMasivasRF)
    '        oReport.SetParameterValue("@Usuario", Usuario)
    '        oReport.SetParameterValue("@FechaOperacion", UIUtility.ConvertirFechaaString(fechaNegocio))
    '        Dim rutaArchivo As String = ""
    '        If Not (oReport Is Nothing) Then
    '            rutaArchivo = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor") & "RF_" & Usuario.ToString() & String.Format("{0:yyyyMMdd}", _
    '            DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
    '            oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
    '        End If
    '        ViewState("RutaReporte") = rutaArchivo

    '        oReport.Close()
    '    End Using
    'End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            Try
                dtDestino.LoadDataRow(dr.ItemArray, False)
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        Next
    End Sub
    Protected Sub Datagrid1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Datagrid1.PageIndexChanging
        Datagrid1.PageIndex = e.NewPageIndex
        CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), ViewState("strTipoInstrumento"), _
        ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
    End Sub
    Protected Sub Datagrid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Datagrid1.RowCommand
        Try
            Dim gvr As GridViewRow = Nothing
            Select Case e.CommandName
                Case "Footer", "Item", "Add", "Footer"
                    gvr = Datagrid1.FooterRow
                Case "SelectValorizador"
                    gvr = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow)
                Case Else
                    Try
                        gvr = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                    Catch ex As Exception
                    End Try
            End Select
            If e.CommandName = "asignarfondo" Then
                'Dim codigoPrevOrden As String = e.CommandArgument.trim
                'Dim chkPorcentaje As CheckBox
                'Dim porcentaje As String
                'chkPorcentaje = CType(gvr.FindControl("chkPorcentaje"), CheckBox)
                'If chkPorcentaje.Checked Then
                '    porcentaje = "S"
                'Else
                '    porcentaje = "N"
                'End If

                'If gvr.Cells(2).Text = PREV_OI_INGRESADO Then
                '    EjecutarJS("showModalDialog('frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentaje + "', '650', '450','');")
                'End If
            ElseIf e.CommandName = "Add" Then
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim oPrevOrdenInversionBE As New PrevOrdenInversionBE
                Dim oRow As PrevOrdenInversionBE.PrevOrdenInversionRow

                'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 16/05/2018
                'Dim dtDetalleInversiones As DataTable
                'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 16/05/2018

                Dim tFechaOperacionF As HiddenField

                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                ddlOperacionF = CType(Datagrid1.FooterRow.FindControl("ddlOperacionF"), DropDownList)
                tFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("hdFechaOperacionF"), HiddenField)
                ddlIndiceF = CType(Datagrid1.FooterRow.FindControl("ddlIndiceF"), DropDownList)
                ddlPlazaNF = CType(Datagrid1.FooterRow.FindControl("ddlPlazaNF"), DropDownList)
                ddlMedioNegF = CType(Datagrid1.FooterRow.FindControl("ddlMedioNegF"), DropDownList)
                ddlCondicionF = CType(Datagrid1.FooterRow.FindControl("ddlCondicionF"), DropDownList)
                ddlTipoTasaF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTasaF"), DropDownList)
                tbFechaLiquidacionF = CType(Datagrid1.FooterRow.FindControl("tbFechaLiquidacionF"), TextBox)
                hdIntermediarioF = CType(Datagrid1.FooterRow.FindControl("hdIntermediarioF"), HtmlInputHidden)
                tbCantidadF = CType(Datagrid1.FooterRow.FindControl("tbCantidadF"), TextBox)
                tbPrecioF = CType(Datagrid1.FooterRow.FindControl("tbPrecioF"), TextBox)
                tbCantidadOperacionF = CType(Datagrid1.FooterRow.FindControl("tbCantidadOperacionF"), TextBox)
                tbPrecioOperacionF = CType(Datagrid1.FooterRow.FindControl("tbPrecioOperacionF"), TextBox)
                tbTasaF = CType(Datagrid1.FooterRow.FindControl("tbTasaF"), TextBox)
                chkPorcentajeF = CType(Datagrid1.FooterRow.FindControl("chkPorcentajeF"), CheckBox)
                hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden)
                ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)

                'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 16/05/2018
                'Session.Remove("dtDetalleInversiones")
                Dim porcentajeF As String = "N" 'Por Default // Solicitado por el usuario 'Consecuencia de la Linea Anterior
                Dim sumaAsignacion As Decimal = Decimal.Parse(tbCantidadOperacionF.Text.Trim)

                Dim portafolioF As DropDownList
                portafolioF = CType(Datagrid1.FooterRow.FindControl("ddlPortafolioF"), DropDownList)
                Dim portafolioIdF As HiddenField
                portafolioIdF = CType(Datagrid1.FooterRow.FindControl("HdPortafolioF"), HiddenField)
                Dim codigoPrevOrden As HiddenField
                codigoPrevOrden = CType(Datagrid1.FooterRow.FindControl("HdCodigoOrdenF"), HiddenField)
                If codigoPrevOrden.Value.Trim.Length <= 0 Then
                    codigoPrevOrden.Value = "0"
                End If

                Dim HdTipoValorizacionF As HiddenField
                HdTipoValorizacionF = CType(Datagrid1.FooterRow.FindControl("HdTipoValorizacionF"), HiddenField)

                Dim dtDetalleInversiones As DataTable = instanciarTablaPrevOrdenInversionDetalle()
                dtDetalleInversiones.Rows.Add(CType(codigoPrevOrden.Value, Decimal), portafolioIdF.Value, CType(tbCantidadF.Text, Decimal), "N")
                'Session("dtDetalleInversiones") = dtDetalleInversiones
                'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 16/05/2018

                Dim objFormulasOI As New OrdenInversionFormulasBM
                Dim precioOperacion As Decimal, tasa As Decimal, totalOperacionRF As Decimal, precioEjecutado As Decimal, totalEjecutadoRF As Decimal, strMensaje As String = "",
                fixing As Decimal = 0, validaFixing As Boolean = True, validaFechaVenc As Boolean, validaIntermediario As Boolean, validaNemonico As Boolean, validaSaldoVenta As Boolean

                Dim bolValidaCampos As Boolean = False

                If tbCantidadF.Text <> "" And tbNemonicoF.Text <> "" And hdIntermediarioF.Value <> "" And tbFechaLiquidacionF.Text <> "" And
                Not ((hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or
                    hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And
                    ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) And
                Not (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And
                     ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And
                    ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE) Then

                    If IsNumeric(tbCantidadF.Text) And IsDate(tbFechaLiquidacionF.Text) Then

                        validaNemonico = ValidarNemonico(tbNemonicoF.Text)
                        validaIntermediario = ValidarIntermediario(hdIntermediarioF.Value.ToString)
                        validaFechaVenc = ValidarFechaVencimiento(tFechaOperacionF.Value, tbFechaLiquidacionF.Text, tbNemonicoF.Text)
                        validaFixing = ValidarFixing(hdClaseInstrumentoF.Value, tbNemonicoF.Text, fixing)
                        validaSaldoVenta = ValidarSaldo(UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value.ToString), tbNemonicoF.Text, portafolioIdF.Value.ToString, hdClaseInstrumentoF.Value.ToString, ddlOperacionF.SelectedValue, IIf(IsNumeric(tbCantidadF.Text), CDec(tbCantidadF.Text), 0), 1, 0)

                        If validaNemonico = True And
                            validaIntermediario = True And
                            validaFechaVenc = True And
                            validaSaldoVenta And
                            validaFixing = True Then
                            bolValidaCampos = True
                        Else
                            If validaNemonico = False Then
                                strMensaje = strMensaje + "- El nemonico no pertenece a renta fija. \n"
                            End If
                            If validaIntermediario = False Then
                                strMensaje = strMensaje + "- El intermediario es incorrecto"
                            End If
                            If validaFechaVenc = False Then
                                strMensaje = strMensaje + "- La Fecha de liquidación es incorrecta."
                            End If
                            If validaFixing = False Then
                                strMensaje = strMensaje + "- Debe ingresar el Fixing. \n"
                            End If
                            If Not validaSaldoVenta Then strMensaje += "No existe el saldo suficiente para generar la pre-orden de inversión \n"
                        End If
                    End If
                Else
                    If tbNemonicoF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Nemónico. \n"
                    End If
                    If tbCantidadF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Cant. Instrumento. \n"
                    End If
                    If hdIntermediarioF.Value.ToString() = "" Then
                        strMensaje = strMensaje + "- Ingrese Intermediario. \n"
                    End If
                    If tbFechaLiquidacionF.Text = "" Then
                        strMensaje = strMensaje + "- Ingrese Fecha de liquidación. \n"
                    End If
                    If (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or _
                        hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) And _
                    ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                        strMensaje = strMensaje + "- Seleccione Tipo. \n"
                    End If
                    If hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And _
                        ddlTipoFondoF.SelectedValue = ParametrosSIT.TIPOFONDO_ETF And _
                        ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE Then
                        strMensaje = strMensaje + "- Seleccione Tipo Tramo. \n"
                    End If
                End If

                If bolValidaCampos = True Then
                    Dim Mensaje As String = ""
                    If (hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or _
                        hdClaseInstrumentoF.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION) Then

                        precioOperacion = Val(tbPrecioF.Text)
                        totalOperacionRF = precioOperacion * CType(tbCantidadF.Text, Decimal)
                        precioEjecutado = Val(tbPrecioOperacionF.Text)
                        totalEjecutadoRF = precioEjecutado * CType(tbCantidadOperacionF.Text, Decimal)
                    Else
                        tasa = Val(tbTasaF.Text)
                        'OT 10090 - 26/07/2017 - Carlos Espejo
                        'Descripcion: En base al tipo de calculo se usa la formula
                        If ddlIndiceF.SelectedValue = "P" Then
                            precioOperacion = Val(tbPrecioF.Text)
                            totalOperacionRF = OrdenInversion.CalculaMontoOperacion(tbNemonicoF.Text, tbCantidadF.Text, ddlIndiceF.SelectedValue, _
                            tbFechaLiquidacionF.Text, ddlTipoTasaF.SelectedValue, tbFechaOperacion.Text, precioOperacion, tasa, Mensaje, DatosRequest).ToString()
                            'precioEjecutado = Val(tbPrecioOperacionF.Text)
                            precioEjecutado = Val(tbPrecioF.Text) 'OT10795
                            totalEjecutadoRF = OrdenInversion.CalculaMontoOperacion(tbNemonicoF.Text, tbCantidadOperacionF.Text, ddlIndiceF.SelectedValue, _
                            tbFechaLiquidacionF.Text, ddlTipoTasaF.SelectedValue, tbFechaOperacion.Text, precioEjecutado, tasa, Mensaje, DatosRequest).ToString()
                        Else
                            '/*INICIO: Proy SIT- FONDOS II - CRumiche: 2018-05-30 */
                            Dim dsValor As DataSet = New PrevOrdenInversionBM().SeleccionarCaracValor(tbNemonicoF.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
                            If dsValor.Tables.Count < 2 Then Throw New Exception("Error: No se ha podido obtener el detalle de cupones del Bono")

                            Dim dtCaracValor As DataTable = dsValor.Tables(0)
                            Dim dtDetalleCupones As DataTable = dsValor.Tables(1)

                            Dim rowValor As DataRow = dtCaracValor.Rows(0)
                            Dim claseInstrumento As String = rowValor("CategoriaClaseInstrumento")

                            If claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_BONO Or
                                claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_CERTIFICADO_DEPOSITO Or
                                claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_PAPELES_COMERCIALES Or
                                claseInstrumento = ParametrosSIT.CLASE_INSTRUMENTO_LETRAS_HIPOTECARIAS Then 'Versión nueva del Cálculo de Renta Fija (CRumiche)

                                Dim vacEmision As Decimal = 1, vacEvaluacion As Decimal = 1

                                If rowValor("CodigoMoneda").ToString.Equals("VAC") Then
                                    ' Obtencion de valores VAC para los Bonos que aplique
                                    UIUtility.ObtenerValoresVAC(rowValor("FechaEmision"),
                                                      UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value),
                                                      vacEmision,
                                                      vacEvaluacion,
                                                      DatosRequest)
                                End If

                                Dim baseMensual As BaseMensualCupon = UIUtility.ObtenerBaseMensualDesdeTexto(rowValor("BaseCuponMensual"))
                                Dim baseAnual As BaseAnualCupon = UIUtility.ObtenerBaseAnualDesdeTexto(rowValor("BaseCuponAnual"))
                                Dim aplicacionTasa As TipoAplicacionTasa = UIUtility.ObtenerTipoAplicacionTasaDesdeCodTipoTasa(ddlTipoTasaF.SelectedValue)

                                Dim EsCuponADescuento As Boolean = rowValor("codigoTipoCupon").ToString.Equals("3") ' Es cupón A DESCUENTO solo si CodigoTipoCupon = 3 
                                Dim esValorExtranjero As Boolean = rowValor("CodigoMercado").ToString.Equals("2")

                                'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
                                Dim esCalculoTBill As Boolean = rowValor("CodigoTipoInstrumentoSBS").ToString.Equals("100")

                                Dim tasaYTM As Decimal = Val(tbTasaF.Text)
                                totalOperacionRF = Convert.ToDecimal(tbCantidadF.Text) * Convert.ToDecimal(rowValor("ValorUnitario"))

                                Dim neg As NegociacionRentaFija

                                'Pasamos a calcular la negociacion (El motor hará todo el trabajo en tiempo real y en la capa de Negocio)
                                neg = UIUtility.CalcularNegociacionRentaFija(dtDetalleCupones,
                                                                                    CDec(rowValor("TasaCupon")),
                                                                                    CInt(rowValor("DiasPeriodicidad")),
                                                                                    CDec(rowValor("ValorUnitario")),
                                                                                    CDec(tbCantidadF.Text),
                                                                                    Convert.ToDateTime(tbFechaLiquidacionF.Text),
                                                                                    tasaYTM,
                                                                                    baseMensual,
                                                                                    baseAnual,
                                                                                    EsCuponADescuento,
                                                                                    esValorExtranjero,
                                                                                    aplicacionTasa,
                                                                                    vacEmision,
                                                                                    vacEvaluacion,
                                                                                    esCalculoTBill)
                                'Mostramos los resultados
                                ValorActual = neg.ValorActual
                                InteresCorrido = neg.InteresCorrido
                                totalEjecutadoRF = neg.ValorActual 'Flujo
                                precioOperacion = neg.PrecioSucio * 100 '% (Es un Porcentaje)
                                precioEjecutado = precioOperacion
                                '/*FIN: Proy SIT- FONDOS II - CRumiche: 2018-05-30 */

                            Else 'VERSION ANTERIOR AL PROYETO SIT-FONDOS-II
                                'OT10795 26/10/2017 - Cálculo Monto nominal
                                'totalOperacionRF = CDec(tbCantidadF.Text) * objFormulasOI.ValorMercado(tbNemonicoF.Text)
                                totalOperacionRF = Convert.ToDecimal(tbCantidadF.Text) * Convert.ToDecimal(dtCaracValor.Rows(0)("ValorUnitario")) * Convert.ToDecimal(dtCaracValor.Rows(0)("AmortizacionPendiente"))
                                'OT10795 - Fin
                                Dim DTPrecio As DataTable = objFormulasOI.CalcularPrecioBono("", tbNemonicoF.Text, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                                 UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text), tasa, totalOperacionRF, ddlTipoTasaF.SelectedValue, ddlOperacionF.SelectedValue)
                                'Precio de Negociacion Limpio
                                precioOperacion = CDec(DTPrecio(0)(0).ToString())
                                precioEjecutado = precioOperacion
                                'Interes Corridos
                                InteresCorrido = Convert.ToDecimal(DTPrecio(0)(2).ToString())
                                'Valor actual
                                ValorActual = Convert.ToDecimal(DTPrecio(0)(3).ToString())
                                If ValorActual > totalOperacionRF Then
                                    totalEjecutadoRF = precioOperacion / 100 * totalOperacionRF + InteresCorrido
                                Else
                                    totalEjecutadoRF = ValorActual - InteresCorrido
                                End If
                            End If

                        End If
                        'OT 10090 Fin
                        If Mensaje.Length > 0 Then
                            AlertaJS(Mensaje)
                            Exit Sub
                        End If
                    End If
                    oRow = CType(oPrevOrdenInversionBE.PrevOrdenInversion.NewRow(), PrevOrdenInversionBE.PrevOrdenInversionRow)
                    oPrevOrdenInversionBM.InicializarPrevOrdenInversion(oRow)
                    'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 18/05/2018
                    'oRow.FechaOperacion = ViewState("decFechaOperacion")
                    'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 18/05/2018

                    'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018
                    'Dim tFechaOperacionF As TextBox
                    'tFechaOperacionF = CType(Datagrid1.FooterRow.FindControl("tFechaOperacionF"), TextBox)
                    'ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Text)
                    'oRow.FechaOperacion = ViewState("decFechaOperacion")

                    ViewState("decFechaOperacion") = UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value)
                    oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value)

                    'hdFechaOperacionF
                    'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018

                    oRow.HoraOperacion = Now.ToLongTimeString()
                    oRow.CodigoNemonico = tbNemonicoF.Text
                    oRow.CodigoOperacion = ddlOperacionF.SelectedValue
                    oRow.CodigoTercero = hdIntermediarioF.Value
                    oRow.CodigoPlaza = ddlPlazaNF.SelectedValue
                    oRow.Situacion = ParametrosSIT.ESTADO_ACTIVO
                    oRow.Estado = ParametrosSIT.PREV_OI_INGRESADO
                    oRow.MedioNegociacion = ddlMedioNegF.SelectedValue
                    oRow.TipoCondicion = ddlCondicionF.SelectedValue
                    'OT11008 - 23/01/2018 - Ian Pastor M.
                    'Descripción: Se obtiene la fecha de liquidación parametrizada en el sistema para los instrumentos de renta fija
                    oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(tbFechaLiquidacionF.Text)

                    'Dim objOrdenPreInversion As New OrdenPreOrdenInversionBM
                    'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 18/05/2018
                    'oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(objOrdenPreInversion.RetornarFechaVencimiento( _
                    '                                                         UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text) _
                    '                                                         , tbNemonicoF.Text, "", ""))
                    'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - COMENTADO | 18/05/2018

                    ''INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018
                    'oRow.FechaLiquidacion = UIUtility.ConvertirFechaaDecimal(objOrdenPreInversion.RetornarFechaVencimiento( _
                    '                                                    UIUtility.ConvertirFechaaDecimal(tFechaOperacionF.Value) _
                    '                                                    , tbNemonicoF.Text, "", ""))
                    ''FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018

                    'OT11008 - Fin
                    oRow.TipoTasa = ddlTipoTasaF.SelectedValue
                    oRow.Tasa = tasa
                    oRow.Cantidad = CType(tbCantidadF.Text, Decimal)
                    oRow.Precio = precioOperacion
                    oRow.MontoNominal = totalOperacionRF
                    oRow.CantidadOperacion = CType(tbCantidadOperacionF.Text, Decimal)
                    oRow.PrecioOperacion = precioEjecutado
                    oRow.MontoOperacion = totalEjecutadoRF
                    oRow.InteresCorrido = InteresCorrido
                    oRow.IndPrecioTasa = ddlIndiceF.SelectedValue
                    oRow.TipoFondo = IIf(ddlTipoFondoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoFondoF.SelectedValue)
                    oRow.TipoTramo = IIf(ddlTipoTramoF.SelectedValue = ParametrosSIT.DDL_ITEM_SELECCIONE, "", ddlTipoTramoF.SelectedValue)
                    oRow.Porcentaje = porcentajeF
                    oRow.Fixing = fixing
                    oRow.TipoValorizacion = HdTipoValorizacionF.Value

                    oPrevOrdenInversionBE.PrevOrdenInversion.AddPrevOrdenInversionRow(oRow)
                    oPrevOrdenInversionBE.PrevOrdenInversion.AcceptChanges()
                    oPrevOrdenInversionBM.Insertar(oPrevOrdenInversionBE, ParametrosSIT.TR_RENTA_FIJA.ToString(), DatosRequest, dtDetalleInversiones)

                    CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                    ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
                    tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(CType(ViewState("decFechaOperacion"), String))
                    'Session.Remove("dtDetalleInversiones")
                Else
                    If strMensaje <> "" Then
                        AlertaJS(strMensaje)
                    Else
                        AlertaJS("Ingrese correctamente el registro")
                    End If
                    Exit Sub
                End If
                EjecutarJS("BorrarHiddens();")
            End If
            If e.CommandName = "TipoFondo" Then
                ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                ddlTipoTramo = CType(gvr.FindControl("ddlTipoTramo"), DropDownList)
                hdClaseInstrumento = CType(gvr.FindControl("hdClaseInstrumento"), HtmlInputHidden)
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                    If ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                        ddlTipoTramo.Enabled = True
                    Else
                        ddlTipoTramo.Enabled = False
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "_Delete" Then
                gvr = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim decCodigoPrevOrden As Decimal = CType(e.CommandArgument.ToString().Split("|")(0), Decimal)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                Dim script As String = ""
                If gvr.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Then
                    Dim dtValidaExtorno As New DataTable
                    dtValidaExtorno = oPrevOrdenInversionBM.ValidaExtorno(decCodigoPrevOrden, DatosRequest).Tables(0)
                    If dtValidaExtorno.Rows.Count > 0 Then
                        script = "Para realizar el extorno, debe revertir las siguientes operaciones: \n"
                        For Each fila As DataRow In dtValidaExtorno.Rows
                            script = script & " - " & fila("CodigoOrden").ToString() & ", " & fila("CodigoPortafolioSBS") & ", " & fila("Estado").ToString() & "\n"
                        Next
                        AlertaJS(script)
                    Else
                        script = UIUtility.MostrarPopUp("frmExtornoIngresoMasivoOperacion.aspx?tipoRenta=" & _
                        ParametrosSIT.TR_RENTA_FIJA.ToString() & "&codigo=" & _
                        decCodigoPrevOrden.ToString(), "DetalleRegistroRV", 770, 480, 110, 55, "no", "no", "yes", "yes")
                        EjecutarJS(script, False)
                    End If
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                ElseIf gvr.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                    oPrevOrdenInversionBM.DesAprobarNegociacion(decCodigoPrevOrden, DatosRequest)
                Else
                    oPrevOrdenInversionBM.Eliminar(decCodigoPrevOrden, DatosRequest)
                End If
                CargarGrilla(ParametrosSIT.TR_RENTA_FIJA.ToString(), ViewState("decFechaOperacion"), ViewState("strClaseInstrumento"), _
                ViewState("strTipoInstrumento"), ViewState("strCodigoNemonico"), ViewState("strOperador"), ViewState("strEstado"))
            End If
            If e.CommandName = "Item" Then
                tbNemonico = CType(row.FindControl("tbNemonico"), TextBox)
                tbIntermediario = CType(row.FindControl("tbIntermediario"), TextBox)
                tbNemonico = CType(row.FindControl("tbNemonico"), TextBox)
                If tbNemonico.Text.Trim <> "" Then
                    hdClaseInstrumento = CType(row.FindControl("hdClaseInstrumento"), HtmlInputHidden)
                    ddlTipoFondo = CType(row.FindControl("ddlTipoFondo"), DropDownList)
                    ddlTipoTramo = CType(row.FindControl("ddlTipoTramo"), DropDownList)
                    If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Or _
                        hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Then
                        ddlTipoFondo.Enabled = True
                        If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                            ddlTipoTramo.Enabled = True
                        Else
                            ddlTipoTramo.Enabled = False
                            ddlTipoTramo.SelectedIndex = 0
                        End If
                    Else
                        ddlTipoFondo.Enabled = False
                        ddlTipoTramo.Enabled = False
                        ddlTipoFondo.SelectedIndex = 0
                        ddlTipoTramo.SelectedIndex = 0
                    End If
                End If
            End If
            If e.CommandName = "Footer" Then

                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbOperadorF"), TextBox)
                ddlTipoTasaF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTasaF"), DropDownList)
                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                tbIntermediarioF = CType(Datagrid1.FooterRow.FindControl("tbIntermediarioF"), TextBox)

                '==== INICIO | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 2018-07-24
                'Dim oValoresBM As New ValoresBM
                'Dim oValorBE As ValoresBE
                'oValorBE = oValoresBM.Seleccionar(tbNemonicoF.Text, DatosRequest)
                'Dim strTipoCupon As String
                'If oValorBE.Tables(0).Rows.Count > 0 Then
                '    strTipoCupon = oValorBE.Tables(0).Rows(0)("CodigoTipoCupon")

                '    If strTipoCupon = "1" Or strTipoCupon = "2" Then
                '        ddlTipoTasaF.SelectedValue = "1"
                '    ElseIf strTipoCupon = "4" Or strTipoCupon = "5" Then
                '        ddlTipoTasaF.SelectedValue = "2"
                '    End If
                'End If
                '==== FIN | PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 2018-07-24

                tbNemonicoF = CType(Datagrid1.FooterRow.FindControl("tbNemonicoF"), TextBox)
                If tbNemonicoF.Text.Trim <> "" Then
                    hdClaseInstrumentoF = CType(Datagrid1.FooterRow.FindControl("hdClaseInstrumentoF"), HtmlInputHidden)
                    ddlTipoFondoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoFondoF"), DropDownList)
                    ddlTipoTramoF = CType(Datagrid1.FooterRow.FindControl("ddlTipoTramoF"), DropDownList)
                End If
            End If
            If e.CommandName = "asignarfondoF" Then
                Dim codigoPrevOrden As String = e.CommandArgument.trim
                Dim chkPorcentajeF As CheckBox
                Dim porcentajeF As String
                chkPorcentajeF = CType(gvr.FindControl("chkPorcentajeF"), CheckBox)
                If chkPorcentajeF.Checked Then
                    porcentajeF = "S"
                Else
                    porcentajeF = "N"
                End If
                EjecutarJS("showModalDialog('frmAsignacionFondo.aspx?codigoprevorden=" & codigoPrevOrden & "&porcentaje=" & porcentajeF + "', '650', '450','');")
            End If

            'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018
            If e.CommandName = "SelectValorizador" Then
                Dim oDatosHiper As New ValoresBM
                Dim dsDatosHiper As DataSet

                tbNemonico = CType(gvr.FindControl("tbNemonico"), TextBox)
                ddlTipoFondo = CType(gvr.FindControl("ddlTipoFondo"), DropDownList)
                dsDatosHiper = oDatosHiper.ObtenerDatosHipervalorizador(tbNemonico.Text, DatosRequest)
                Dim hdSBS As HiddenField
                hdSBS = CType(gvr.FindControl("hdSBS"), HiddenField)

                If dsDatosHiper.Tables.Count > 0 Then
                    If dsDatosHiper.Tables(0).Rows.Count > 0 Then
                        With dsDatosHiper.Tables(0)
                            EjecutarJS(UIUtility.MostrarPopUp("../Parametria/AdministracionValores/frmHipervalorizador.aspx?nemonico=" + tbNemonico.Text +
                            "&valorUnitario=" + .Rows(0)("valorUnitario").ToString + "&tasaCupon=" + .Rows(0)("tasaCupon").ToString + "&baseTIR=" + .Rows(0)("baseTir").ToString +
                            "&baseDias=" + .Rows(0)("baseCupon").ToString + "&fechaEmision=" + UIUtility.ConvertirFechaaString(.Rows(0)("fechaEmision")) +
                            "&tipoCuponera=" + .Rows(0)("tipoCuponera").ToString + "&codigoSBS=" + hdSBS.Value + "&fondo=" + ddlTipoFondo.SelectedValue, "no", 800,
                            650, 30, 50, "No", "No", "Yes", "Yes"), False)
                        End With
                    End If
                End If
            End If
            'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub Datagrid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Datagrid1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            hdIntermediario = CType(e.Row.FindControl("hdIntermediario"), HtmlInputHidden)
            lbOperacion = CType(e.Row.FindControl("lbOperacion"), Label)
            ddlOperacion = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacion, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
            ddlOperacion.SelectedValue = lbOperacion.Text
            hdOperacionTrz = CType(e.Row.FindControl("hdOperacionTrz"), HtmlInputHidden)
            hdOperacionTrz.Value = ddlOperacion.SelectedItem.Text
            lbMedioNeg = CType(e.Row.FindControl("lbMedioNeg"), Label)
            ddlMedioNeg = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
            HelpCombo.LlenarComboBox(ddlMedioNeg, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
            ddlMedioNeg.SelectedValue = lbMedioNeg.Text
            lbCondicion = CType(e.Row.FindControl("lbCondicion"), Label)
            ddlCondicion = CType(e.Row.FindControl("ddlCondicion"), DropDownList)
            HelpCombo.LlenarComboBox(ddlCondicion, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
            ddlCondicion.SelectedValue = lbCondicion.Text
            ddlCondicion.Attributes.Add("onchange", "javascript:HabilitaCondicion(this);")
            lbTipoTasa = CType(e.Row.FindControl("lbTipoTasa"), Label)
            ddlTipoTasa = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTasa, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", False)
            ddlTipoTasa.SelectedValue = IIf(lbTipoTasa.Text = "", "2", lbTipoTasa.Text)
            lbPlazaN = CType(e.Row.FindControl("lbPlazaN"), Label)
            ddlPlazaN = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
            HelpCombo.LlenarComboBox(ddlPlazaN, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
            ddlPlazaN.SelectedValue = lbPlazaN.Text
            tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
            tbTasa = CType(e.Row.FindControl("tbTasa"), TextBox)
            lbIndice = CType(e.Row.FindControl("lbIndice"), Label)
            ddlIndice = CType(e.Row.FindControl("ddlIndice"), DropDownList)
            HelpCombo.LlenarComboBox(ddlIndice, CType(Session("dtIndice"), DataTable), "Valor", "Nombre", False)
            ddlIndice.SelectedValue = lbIndice.Text
            lbTipoFondo = CType(e.Row.FindControl("lbTipoFondo"), Label)
            ddlTipoFondo = CType(e.Row.FindControl("ddlTipoFondo"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoFondo, CType(Session("dtTipoFondo"), DataTable), "Valor", "Nombre", False)
            ddlTipoFondo.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
            ddlTipoFondo.SelectedValue = IIf(lbTipoFondo.Text.Trim = "", ParametrosSIT.DDL_ITEM_SELECCIONE, lbTipoFondo.Text)
            lbTipoTramo = CType(e.Row.FindControl("lbTipoTramo"), Label)
            ddlTipoTramo = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTramo, CType(Session("dtTipoTramo"), DataTable), "Valor", "Nombre", False)
            ddlTipoTramo.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, ParametrosSIT.DDL_ITEM_SELECCIONE))
            ddlTipoTramo.SelectedValue = IIf(lbTipoTramo.Text.Trim = "", ParametrosSIT.DDL_ITEM_SELECCIONE, lbTipoTramo.Text)

            '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Mejor forma de utilizar los datos de la grilla con "DataItem", en vez de Hiddens por todos lados
            ddlFondos = CType(e.Row.FindControl("ddlfondos"), DropDownList)
            ddlFondos.Items.Add(New WebControls.ListItem(e.Row.DataItem("PortafolioSelec"), e.Row.DataItem("CodigoPortafolioSelec")))
            '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Mejor forma de utilizar los datos de la grilla con "DataItem", en vez de Hiddens por todos lados

            Dim hdTipoValorizacion As HiddenField
            hdTipoValorizacion = CType(e.Row.FindControl("hdTipoValorizacion"), HiddenField)
            Dim ddlTipoValorizacion As DropDownList
            ddlTipoValorizacion = CType(e.Row.FindControl("ddlTipoValorizacion"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoValorizacion, objParamGeneral.Listar("TipoValorizacion", DatosRequest), "Valor", "Nombre", True)
            ddlTipoValorizacion.SelectedValue = hdTipoValorizacion.Value
            'ddlTipoValorizacion.Enabled = False

            ddlTipoFondo.Attributes.Add("onchange", "javascript:HabilitaTipoTramo(this);")
            hdClaseInstrumento = CType(e.Row.FindControl("hdClaseInstrumento"), HtmlInputHidden)
            If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOINVERSION Or hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO Then
                ddlTipoFondo.Enabled = True
                If hdClaseInstrumento.Value = ParametrosSIT.CLASE_INSTRUMENTO_FONDOMUTUO And ddlTipoFondo.SelectedValue = ParametrosSIT.TIPOFONDO_ETF Then
                    ddlTipoTramo.Enabled = True
                End If
            End If
            If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO Then
                ddlCondicion.Enabled = False
                ddlOperacion.Enabled = False
                ddlPlazaN.Enabled = False
                ddlTipoTasa.Enabled = False
                ddlMedioNeg.Enabled = False
                ddlIndice.Enabled = False
                ddlTipoFondo.Enabled = False
                ddlTipoTramo.Enabled = False
                tbHora = CType(e.Row.FindControl("tbHora"), TextBox)
                tbCantidad = CType(e.Row.FindControl("tbCantidad"), TextBox)
                tbTotal = CType(e.Row.FindControl("tbTotal"), TextBox)
                tbFechaLiquidacion = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
                tbPrecio = CType(e.Row.FindControl("tbPrecio"), TextBox)
                tbIntermediario = CType(e.Row.FindControl("tbIntermediario"), TextBox)
                tbNemonico = CType(e.Row.FindControl("tbNemonico"), TextBox)
                chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
                tbCantidad.Enabled = False
                tbPrecio.Enabled = False
                tbTasa.Enabled = False
                tbTotal.Enabled = False
                tbFechaLiquidacion.Enabled = False
                tbIntermediario.Enabled = False
                tbNemonico.Enabled = False
                chkSelect.Enabled = False
                If e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_EJECUTADO Or e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_APROBADO Or
                e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_PENDIENTE Then
                    chkSelect.Enabled = True
                End If
                Dim FechaVal As HtmlGenericControl
                FechaVal = CType(e.Row.FindControl("FechaVal"), HtmlGenericControl)
                FechaVal.Attributes.Add("class", "input-append")
            End If
            tbHora2 = CType(e.Row.FindControl("tbHora"), TextBox)
            tbNemonico2 = CType(e.Row.FindControl("tbNemonico"), TextBox)
            ddlOperacion2 = CType(e.Row.FindControl("ddlOperacion"), DropDownList)
            tbCantidad2 = CType(e.Row.FindControl("tbCantidad"), TextBox)
            ddlIndice2 = CType(e.Row.FindControl("ddlIndice"), DropDownList)
            tbPrecio2 = CType(e.Row.FindControl("tbPrecio"), TextBox)
            ddlTipoTasa2 = CType(e.Row.FindControl("ddlTipoTasa"), DropDownList)
            tbTasa2 = CType(e.Row.FindControl("tbTasa"), TextBox)
            ddlPlazaN2 = CType(e.Row.FindControl("ddlPlazaN"), DropDownList)
            ddlCondicion2 = CType(e.Row.FindControl("ddlCondicion"), DropDownList)
            tbIntermediario2 = CType(e.Row.FindControl("tbIntermediario"), TextBox)
            ddlMedioNeg2 = CType(e.Row.FindControl("ddlMedioNeg"), DropDownList)
            tbFechaLiquidacion2 = CType(e.Row.FindControl("tbFechaLiquidacion"), TextBox)
            tbCantidadOperacion2 = CType(e.Row.FindControl("tbCantidadOperacion"), TextBox)
            tbPrecioOperacion2 = CType(e.Row.FindControl("tbPrecioOperacion"), TextBox)
            ddlTipoTramo2 = CType(e.Row.FindControl("ddlTipoTramo"), DropDownList)
            hdPorcentaje2 = CType(e.Row.FindControl("hdPorcentaje"), HtmlInputHidden)
            chkPorcentaje2 = CType(e.Row.FindControl("chkPorcentaje"), CheckBox)
            If hdPorcentaje2.Value = "S" Then
                chkPorcentaje2.Checked = True
            Else
                chkPorcentaje2.Checked = False
            End If
            tbHora2.Attributes.Add("onchange", "cambio(this);")
            tbNemonico2.Attributes.Add("onpropertychange", "cambio(this);")
            ddlOperacion2.Attributes.Add("onchange", "cambio(this);")
            tbCantidad2.Attributes.Add("onchange", "cambio(this);")
            ddlIndice2.Attributes.Add("onchange", "cambio(this);")
            tbPrecio2.Attributes.Add("onchange", "cambio(this);")
            ddlTipoTasa2.Attributes.Add("onchange", "cambio(this);")
            tbTasa2.Attributes.Add("onchange", "cambio(this);")
            ddlPlazaN2.Attributes.Add("onchange", "cambio(this);")
            ddlCondicion2.Attributes.Add("onchange", "cambio(this);")
            tbIntermediario2.Attributes.Add("onchange", "cambio(this);")
            ddlMedioNeg2.Attributes.Add("onchange", "cambio(this);")
            tbFechaLiquidacion2.Attributes.Add("onchange", "cambio(this);")
            tbCantidadOperacion2.Attributes.Add("onchange", "cambio(this);")
            tbPrecioOperacion2.Attributes.Add("onchange", "cambio(this);")
            ddlTipoTramo2.Attributes.Add("onchange", "cambio(this);")
            chkPorcentaje2.Attributes.Add("onchange", "cambio(this);")

            '==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | Para cada fila que llena, solo estarán habilitados sus controles si el ESTADO  ELIMINADO
            e.Row.Enabled = Not (e.Row.Cells(2).Text = ParametrosSIT.PREV_OI_ELIMINADO)
            '==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-13 | Para cada fila que llena, solo estarán habilitados sus controles si el ESTADO ELIMINADO
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim esDiaHabil As Boolean = False, DiaFeriado As Integer, FechaLiquidacion As String = String.Empty
            tbOperadorF = CType(e.Row.FindControl("tbOperadorF"), TextBox)
            tbOperadorF.Text = Usuario.ToString.Trim
            ddlOperacionF = CType(e.Row.FindControl("ddlOperacionF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlOperacionF, CType(Session("dtOperacion"), DataTable), "codigoOperacion", "Descripcion", False)
            ddlOperacionF.SelectedIndex = 0
            ddlMedioNegF = CType(e.Row.FindControl("ddlMedioNegF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlMedioNegF, CType(Session("dtMedioNeg"), DataTable), "Valor", "Nombre", False)
            ddlMedioNegF.SelectedIndex = 0
            ddlCondicionF = CType(e.Row.FindControl("ddlCondicionF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlCondicionF, CType(Session("dtCondicion"), DataTable), "Valor", "Nombre", False)
            ddlCondicionF.SelectedValue = "AM"
            ddlCondicionF.Attributes.Add("onchange", "javascript:HabilitaCondicionF(this);")
            ddlPlazaNF = CType(e.Row.FindControl("ddlPlazaNF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlPlazaNF, CType(Session("dtPlazaN"), DataTable), "CodigoPlaza", "Descripcion", False)
            ddlPlazaNF.SelectedValue = "4"
            ddlTipoTasaF = CType(e.Row.FindControl("ddlTipoTasaF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTasaF, CType(Session("dtTipoTasa"), DataTable), "Valor", "Nombre", False)
            ddlTipoTasaF.SelectedIndex = 2 ' PROYECTO FONDOS-II - ZOLUXIONES | CRumiche | RF007 | 2018-07-24
            ddlIndiceF = CType(e.Row.FindControl("ddlIndiceF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlIndiceF, CType(Session("dtIndice"), DataTable), "Valor", "Nombre", False)
            ddlIndiceF.SelectedIndex = 1
            ddlTipoFondoF = CType(e.Row.FindControl("ddlTipoFondoF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoFondoF, CType(Session("dtTipoFondo"), DataTable), "Valor", "Nombre", True, ParametrosSIT.DDL_ITEM_SELECCIONE)
            ddlTipoFondoF.SelectedValue = "Normal"
            ddlTipoTramoF = CType(e.Row.FindControl("ddlTipoTramoF"), DropDownList)
            HelpCombo.LlenarComboBox(ddlTipoTramoF, CType(Session("dtTipoTramo"), DataTable), "Valor", "Nombre", True, ParametrosSIT.DDL_ITEM_SELECCIONE)
            ddlTipoTramoF.SelectedIndex = 0
            ddlTipoFondoF.Attributes.Add("onchange", "javascript:HabilitaTipoTramoF(this);")
            tbFechaLiquidacionF = CType(e.Row.FindControl("tbFechaLiquidacionF"), TextBox)

            'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 24/05/2018
            Dim ddlPortafolioF As DropDownList
            ddlPortafolioF = CType(e.Row.FindControl("ddlPortafolioF"), DropDownList)
            ddlPortafolioF.Items.Insert(0, New System.Web.UI.WebControls.ListItem(ParametrosSIT.DDL_ITEM_SELECCIONE, "0"))
            ddlPortafolioF.SelectedValue = "0"
            'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 24/05/2018

            'OT11008 - 23/01/2018 - Ian Pastor M.
            'Descripción: Evalúa día hábil T+2
            DiaFeriado = 2
            If Not tbFechaOperacion.Text = "" Then
                While esDiaHabil = False
                    FechaLiquidacion = CDate(tbFechaOperacion.Text).AddDays(DiaFeriado).ToString("dd/MM/yyyy")
                    esDiaHabil = oFeriado.VerificaDia(UIUtility.ConvertirFechaaDecimal(FechaLiquidacion), "")
                    DiaFeriado += 1
                End While
                tbFechaLiquidacionF.Text = FechaLiquidacion
            End If
            'OT11008 - Fin
        End If
    End Sub
    Public Sub EjecutarOrdenInversion(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, Optional ByVal strCodPrevOrden As String = "", _
    Optional ByRef bolUpdGrilla As Boolean = False, Optional ByVal claseInstrumento As String = "", Optional ByVal decNProceso As Decimal = 0)
        Dim objBM As New LimiteBM
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim ds As New DataSet
        Dim dtOrdenInversion As New DataTable
        Dim bolGeneraOrden As Boolean = False
        Dim i As Long
        Dim arrCodPrevOrden As Array
        objBM.RegistrarOrdenesPreviasSeleccionadas(strTipoRenta, decNProceso)
        ds = oPrevOrdenInversionBM.GenerarOrdenInversion_Sura(strTipoRenta, decFechaOperacion, DatosRequest, claseInstrumento, decNProceso)
        If ds.Tables(0).Rows.Count <= 0 Then
            bolUpdGrilla = True
        Else
            If ds.Tables(0).Rows(0)(0) = LIQUIDADO Then
                AlertaJS("Alguna operación ejecutada similar para una agrupación esta liquidada.\nDebe extornar la operación liquidada antes de ejecutar una agrupación similar.')")
            Else
                dtOrdenInversion = ds.Tables(0)
                Try
                    Try
                        bolGeneraOrden = CType(ds.Tables(1).Rows(0)("GeneraOrden"), Boolean)
                    Catch ex As Exception
                        bolGeneraOrden = CType(ds.Tables(2).Rows(0)("GeneraOrden"), Boolean)
                    End Try
                Catch ex As Exception
                    bolGeneraOrden = CType(ds.Tables(3).Rows(0)("GeneraOrden"), Boolean)
                End Try
                arrCodPrevOrden = strCodPrevOrden.Split("|")
                For i = 0 To arrCodPrevOrden.Length - 1
                    If arrCodPrevOrden(i) <> "" Then
                        oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "1")
                    End If
                Next
                'OT-10784 Inicio
                If dtOrdenInversion.Rows.Count > 0 Then
                    Session("dtListaExcesos") = dtOrdenInversion
                    EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + _
                    "', '1000', '500','" & btnBuscar.ClientID & "');")
                End If
                'If bolGeneraOrden Then
                '    If dtOrdenInversion.Rows.Count > 0 Then
                '        Dim Variable As String = "TmpCodigoUsuario,TmpEscenario,TmpNProceso"
                '        Dim Parametros As String = Usuario + "," + ParametrosSIT.EJECUCION_PREVOI + "," + decNProceso.ToString
                '        Dim obj As New JobBM
                '        Dim mensaje As String = obj.EjecutarJob("DTS_SIT_VerificaExcesoLimitesEnLinea" & DateTime.Today.ToString("_yyyyMMdd") & _
                '        System.DateTime.Now.ToString("_hhmmss"), "Verifica exceso de limites en linea, considerando el neteo de operaciones", Variable, _
                '        Parametros, "", "", ConfigurationManager.AppSettings("SERVIDORETL"))
                '        AlertaJS(mensaje)
                '        Session("dtOrdenInversion") = dtOrdenInversion
                '        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=OI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + _
                '        "', '1000', '500','" & btnBuscar.ClientID & "');")
                '    End If
                'Else
                '    If dtOrdenInversion.Rows.Count > 0 Then
                '        Session("dtListaExcesos") = dtOrdenInversion
                '        EjecutarJS("showModalDialog('frmValidacionExcesosPrevOI.aspx?Tipo=PREVOI&TipoRenta=" & strTipoRenta & "&Instrumento=" & claseInstrumento + _
                '        "', '1000', '500','" & btnBuscar.ClientID & "');")
                '    End If
                'End If
            End If

            '==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución
            ' Debido a que las comisiones se generan en BD y despues de ejecutada Orden de Inversion no vemos en la necesidad
            ' de calcularla la Tir Neta al terminar este proceso
            For Each rowOrden In ds.Tables(0).Rows
                Try
                    CalcularTirNetaNegociacionRF(rowOrden("CodigoOrden"), rowOrden("CodigoPortafolioSBS"))
                Catch ex As Exception
                    AlertaJS("No se pudo calcular la TIR Neta de la orden [" & rowOrden("CodigoOrden") & "], sin embargo puede volver a intentarlo en la Confirmación")
                End Try
            Next
            '==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-10-01 | Calculo del TIR Neto Post Ejecución

        End If
        'If bolGeneraOrden = False Then
        '    arrCodPrevOrden = strCodPrevOrden.Split("|")
        '    For i = 0 To arrCodPrevOrden.Length - 1
        '        If arrCodPrevOrden(i) <> "" Then
        '            oPrevOrdenInversionBM.ActualizaSeleccionPrevOrden(arrCodPrevOrden(i), "0")
        '        End If
        '    Next
        '    oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
        'End If
        'OT-10784 Fin
    End Sub
    Private Sub llenarFilaVacia(ByRef table As DataSet)
        Dim row As DataRow = table.Tables(0).NewRow()
        For Each item As DataColumn In table.Tables(0).Columns
            Select Case item.DataType
                Case GetType(String)
                    row(item.ColumnName) = String.Empty
                Case GetType(Decimal)
                    row(item.ColumnName) = 0.0
            End Select
        Next
        table.Tables(0).Rows.Add(row)
    End Sub
    Private Function row() As Object
        Throw New NotImplementedException
    End Function
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        If Not Page.IsPostBack Then
            CargarLoading("btnBuscar")
            'btnGrabar.Attributes.Add("onClick", "this.disabled = true; this.value = 'En proceso...';")
            'btnValidarTrader.Attributes.Add("onClick", "this.disabled = true; this.value = 'En proceso...';")
            'btnAprobar.Attributes.Add("onClick", "this.disabled = true; this.value = 'En proceso...';")
            CargarPagina()
        End If
    End Sub

    'Protected Sub Modulos_Inversiones_frmIngresoMasivoOperacionRF_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
    '    Cro.Close()
    '    Cro.Dispose()
    'End Sub

End Class