<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAdministracionValores.aspx.vb" Inherits="Modulos_Parametria_AdministracionValores_frmAdministracionValores" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Administración de Valores</title>

    <script type="text/javascript">
        function CerrarLoading() {
            $("#divBackground").hide();
            $("#divLoading").hide();
        }
        function SeleccionarTR() {
            if (document.getElementById("<%= ddlTipoRenta.ClientID %>").value == "1") {
                $("#divRowSubordinario").show();
                $("#divRowPrecioDevengado").show();       
            } else {
                $("#divRowSubordinario").hide();
                $("#divRowPrecioDevengado").hide();
            }
        }

        function actualizarValorBaseMensual(idx) {
            if (idx == 0) {
                idx = 1;
            } else {
                if (idx == 1) idx = 0;
            }
            $('#ddlCuponNDias').prop('selectedIndex', idx); 
            return false;
        }
        function actualizarValorBaseAnual(idx) {
            if (idx == 2) idx = 1;
            $('#ddlCuponBase').prop('selectedIndex', idx); 
            return false;
        }
        function showModalInstrumento() {
            $('#_Modal').val('INS');
            return showModalDialog('frmBusquedaTipoInstrumento.aspx?tipoRenta=' + $('#ddlTipoRenta').val(), '1200', '600', '');               
        }
        function showModalEmisor() {
            $('#_Modal').val('EMI');
            return showModalDialog('../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');   
        }
        var ERR_CAMPO_OBLIGATORIO = "Los siguientes campos son obligatorios:<br />";
        function ValidaCamposObligatorios() {
            if (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "06") { return true; }
            var strMsjCampOblig = "";
            var tipoTitulo = "";
            if ((document.getElementById("<%= ddlTipoRenta.ClientID %>").value == "1")
		        && (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value != "52"
		        && document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value != "51")) { //renta FIJA
                if (document.getElementById("<%= tbMnemonico.ClientID %>").value == "")
                    strMsjCampOblig += "-Código Mnemónico<br />"
                if (document.getElementById("<%= ddlMercado.ClientID %>").value == "")
                    strMsjCampOblig += "-Mercado<br />"
                if (document.getElementById("<%= tbDescripcion.ClientID %>").value == "")
                    strMsjCampOblig += "-Descripción<br />"
                if (document.getElementById("<%= ddlAgrupacion.ClientID %>").value == "")
                    strMsjCampOblig += "-Agrupacion<br />"
                if (document.getElementById("<%= tbEmisor.ClientID %>").value == "")
                    strMsjCampOblig += "-Emisor<br />"
                if (document.getElementById("<%= ddlMoneda.ClientID %>").value == "")
                    strMsjCampOblig += "-Moneda<br />"
                if (document.getElementById("<%= tbNumUnidades.ClientID %>").value == "")
                    strMsjCampOblig += "-Número de Unidades<br />"
                if (document.getElementById("<%= tbValorUnitario.ClientID %>").value == "")
                    strMsjCampOblig += "-Valor Unitario<br />"
                if (document.getElementById("<%= tbValorNominal.ClientID %>").value == "")
                    strMsjCampOblig += "-Valor Nominal<br />"
                if (document.getElementById("<%= tbValorEfecColocado.ClientID %>").value == "")
                    strMsjCampOblig += "-Valor efectivo colocado<br />"
                if (document.getElementById("<%=tbCodigoIsin.ClientID %>").value == "")
                    strMsjCampOblig += "-Código ISIN<br />"
                if (document.getElementById("<%= tbCodigoSBS.ClientID %>").value == "")
                    strMsjCampOblig += "-Código SBS<br />"
                // 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se comenta para quitar campos obligatorios en Rating Interno| 18/05/18 
//                if (document.getElementById("<%= ddlCalificacion.ClientID %>").value == "")
                //                    strMsjCampOblig += "-Calificación<br />"
                // 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se comenta para quitar campos obligatorios en Rating Interno| 18/05/18 
                if (document.getElementById("<%= hdnCodigoClaseInstrumento.ClientID %>").value == '1' ||
                    document.getElementById("<%= hdnCodigoClaseInstrumento.ClientID %>").value == '3' ||
                    document.getElementById("<%= hdnCodigoClaseInstrumento.ClientID %>").value == '7') {
                    if (document.getElementById("<%= ddlTipoCupon.ClientID %>").value == "") { strMsjCampOblig += "-Tipo Cupón<br />" }
                }
                if ((document.getElementById("<%= hdnCodigoClaseInstrumento.ClientID %>").value == "1") ||
		            (document.getElementById("<%= hdnCodigoClaseInstrumento.ClientID %>").value == "7")) {
                    if ((document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "19") ||
			            (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "09")) {
                        if (document.getElementById("<%= tbTasaCupon.ClientID %>").value == "")
                            strMsjCampOblig += "-Tasa Cupón<br />"
                        if (document.getElementById("<%= ddlPeriodicidad.ClientID %>").value == "")
                            strMsjCampOblig += "-Periodicidad<br />"
                    }
                    if (document.getElementById("<%= tbFechaEmision.ClientID %>").value == "")
                        strMsjCampOblig += "-Fecha de Emisión<br />"
                    if (document.getElementById("<%= tbFechaVencimiento.ClientID %>").value == "")
                        strMsjCampOblig += "-Fecha de Vencimiento<br />"

                    tipoTitulo = document.getElementById("<%= ddlTipoTitulo.ClientID %>").value;

                    if (tipoTitulo.substring(0, 5) == "NRBCR" && document.getElementById("<%= tbFechaVencimiento.ClientID %>").value == "" && document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value == "" || (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "52" && document.getElementById("<%= tbFechaVencimiento.ClientID %>").value == "" && document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value == ""))
                        strMsjCampOblig += "-Fecha de Primer Cupón<br />";

                    if (tipoTitulo.substring(0, 5) == "NRBCR" && document.getElementById("<%= tbFechaVencimiento.ClientID %>").value != "" && document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value == "" || (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "52" && document.getElementById("<%= tbFechaVencimiento.ClientID %>").value != "" && document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value == ""))
                        document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value = document.getElementById("<%= tbFechaVencimiento.ClientID %>").value;

                    if (tipoTitulo.substring(0, 5) != "NRBCR" || document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "52" || tipoTitulo.substring(0, 6) != "CLPBCR") {
                        if (document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value == "" && document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value != "100" && document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value != "101")
                            strMsjCampOblig += "-Fecha de Primer Cupón<br />"
                    }
                    if (document.getElementById("<%= tbTasaCupon.ClientID %>").value == "")
                        strMsjCampOblig += "-Tasa Cupón<br />"
                    if (document.getElementById("<%= ddlPeriodicidad.ClientID %>").value == ""  && document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value != "100" && document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value != "101")
                        strMsjCampOblig += "-Periodicidad<br />"
                    if (document.getElementById("<%= ddlTipoAmortizacion.ClientID %>").value == "")
                        strMsjCampOblig += "-Tipo Amortización<br />"
                    if (document.getElementById("<%= ddlCuponBase.ClientID %>").value == "")
                        strMsjCampOblig += "-Cupón Base<br />"
                }
                if (document.getElementById("<%= ddlRating.ClientID %>").value == "")
                    strMsjCampOblig += "-Rating Fondo<br />"
            } else {	//renta VARIABLE
                if ((document.getElementById("<%= ddlTipoRenta.ClientID %>").value == "2") ||
	                ((document.getElementById("<%= ddlTipoRenta.ClientID %>").value == "1") && (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "52"))) {
                    if (document.getElementById("<%= tbMnemonico.ClientID %>").value == "")
                        strMsjCampOblig += "-Código Mnemónico<br />"
                    if (document.getElementById("<%= tbDescripcion.ClientID %>").value == "")
                        strMsjCampOblig += "-Descripción<br />"
                    if (document.getElementById("<%= ddlMercado.ClientID %>").value == "")
                        strMsjCampOblig += "-Mercado<br />"
                    if (document.getElementById("<%= ddlAgrupacion.ClientID %>").value == "")
                        strMsjCampOblig += "-Agrupacion<br />"
                    if (document.getElementById("<%= tbEmisor.ClientID %>").value == "")
                        strMsjCampOblig += "-Emisor<br />"
                    if (document.getElementById("<%= ddlMoneda.ClientID %>").value == "")
                        strMsjCampOblig += "-Moneda<br />"
                    if (document.getElementById("<%= tbNumUnidades.ClientID %>").value == "")
                        strMsjCampOblig += "-Número de Unidades<br />"
                    if (document.getElementById("<%= tbValorUnitario.ClientID %>").value == "")
                        strMsjCampOblig += "-Valor Unitario<br />"
                    if (document.getElementById("<%= tbValorEfecColocado.ClientID %>").value == "")
                        strMsjCampOblig += "-Valor efectivo colocado<br />"
                    if (document.getElementById("<%=tbCodigoIsin.ClientID %>").value == "")
                        strMsjCampOblig += "-Código ISIN<br />"
                    if (document.getElementById("<%= tbCodigoSBS.ClientID %>").value == "")
                        strMsjCampOblig += "-Código SBS<br />"

                    // 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se comenta para quitar campos obligatorios en Rating Interno y Bursatibilidad | 18/05/18 
                    //                    if (document.getElementById("<%= ddlCalificacion.ClientID %>").value == "")
                    //                        strMsjCampOblig += "-Rating Interno<br />"
                    //                    if (document.getElementById("<%= ddlBursatilidad.ClientID %>").value == "")
                    //                        strMsjCampOblig += "-Bursatilidad<br />"
                    //                    if (document.getElementById("<%= ddlTipoLiquidez.ClientID %>") != null) {
                    //                        if (document.getElementById("<%= ddlTipoLiquidez.ClientID %>").value == "")
                    //                            strMsjCampOblig += "-Tipo Liquidez<br />"
                    // }
                    // 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se comenta para quitar campos obligatorios en Rating Interno y Bursatibilidad | 18/05/18


                } else {	//renta derivados
                    if (document.getElementById("<%= ddlTipoRenta.ClientID %>").value == "3") {
                        if (document.getElementById("<%= tbMnemonico.ClientID %>").value == "")
                            strMsjCampOblig += "-Código Mnemónico<br />"
                        if (document.getElementById("<%= tbDescripcion.ClientID %>").value == "")
                            strMsjCampOblig += "-Descripción<br />"
                        if (document.getElementById("<%= ddlMercado.ClientID %>").value == "")
                            strMsjCampOblig += "-Mercado<br />"
                        if (document.getElementById("<%= ddlMoneda.ClientID %>").value == "")
                            strMsjCampOblig += "-Moneda<br />"
                        if (document.getElementById("<%= tbEmisor.ClientID %>").value == "")
                            strMsjCampOblig += "-Emisor<br />"
                        if (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "")
                            strMsjCampOblig += "-Tipo Instrumento<br />"
                        if (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value = "86") {
                            strMsjCampOblig = "";
                            if (document.getElementById("<%= tbMnemonico.ClientID %>").value == "")
                                strMsjCampOblig += "-Código Mnemónico<br />"
                            if (document.getElementById("<%= tbDescripcion.ClientID %>").value == "")
                                strMsjCampOblig += "-Descripción<br />"
                            if (document.getElementById("<%= ddlMoneda.ClientID %>").value == "")
                                strMsjCampOblig += "-Moneda<br />"
                            if (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "")
                                strMsjCampOblig += "-Tipo Instrumento<br />"
                            if (document.getElementById("<%= tbMargenInicial.ClientID %>").value == "")
                                strMsjCampOblig += "-Margen Inicial<br />"
                            if (document.getElementById("<%= tbMargenMnto.ClientID %>").value == "")
                                strMsjCampOblig += "-Margen Mantenimiento<br />"
                            if (document.getElementById("<%= tbContractSize.ClientID %>").value == "")
                                strMsjCampOblig += "-Contract Size<br />"
                        }
                    }
                }
            }
            if (document.getElementById("<%= ddlTipoCodigoValor.ClientID %>").value == "0")
                strMsjCampOblig += "-Tipo Código Valor<br />"
            //}
            if ((document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "51") || (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "53")) {
                if (document.getElementById("<%= ddlCategoria.ClientID %>").value == "")
                    strMsjCampOblig += "-Categoria<br />"
            }
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br />";
                return false;
            } { return true; }
        }
        function ValidarCasoRF_TipoInsturmento52() {
            if (document.getElementById("<%= tbCodigoSBSinst.ClientID %>").value == "52") {
                document.getElementById("<%= ddlBursatilidad.ClientID %>").disabled = false;
            }
        }
        function Validar() {
            strMensajeError = "";
            if (ValidaCamposObligatorios()) {
                return true;
            } else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function ValidarCuponNormal() {
            strMensajeError = "";
            if (ValidaCamposCuponNormal()) { return true; }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function ValidaCamposCuponNormal() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= tbMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "-Código Mnemónico<br />"
            if (document.getElementById("<%=tbCodigoIsin.ClientID %>").value == "")
                strMsjCampOblig += "-Código ISIN<br />"
            if (document.getElementById("<%= tbFechaEmision.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha de Emisión<br />"
            if (document.getElementById("<%= tbFechaVencimiento.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha de Vencimiento<br />"
            if (document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha de Primer Cupón<br />"
            if (document.getElementById("<%= ddlTipoAmortizacion.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Amortización<br />"
            if (document.getElementById("<%= ddlCuponBase.ClientID %>").value == "")
                strMsjCampOblig += "-Base Cupón<br />"
            if (document.getElementById("<%= tbTasaCupon.ClientID %>").value == "")
                strMsjCampOblig += "-Tasa Cupón<br />"
            if (document.getElementById("<%= ddlPeriodicidad.ClientID %>").value == "")
                strMsjCampOblig += "-Periodicidad<br />"
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br />";
                return false;
            } { return true; }
        }
        function ValidarCuponEspecial() {
            strMensajeError = "";
            if (ValidaCamposCuponEspecial()) { return true; }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function ValidaCamposCuponEspecial() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= tbMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "-Código Mnemónico<br />"
            if (document.getElementById("<%=tbCodigoIsin.ClientID %>").value == "")
                strMsjCampOblig += "-Código ISIN<br />"
            if (document.getElementById("<%= tbFechaEmision.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha de Emisión<br />"
            if (document.getElementById("<%= tbFechaVencimiento.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha de Vencimiento<br />"
            if (document.getElementById("<%= tbFechaPrimerCupon.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha de Primer Cupón<br />"
            if (document.getElementById("<%= ddlTipoAmortizacion.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Amortización<br />"
            if (document.getElementById("<%= ddlCuponBase.ClientID %>").value == "")
                strMsjCampOblig += "-Base Cupón<br />"
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br />";
                return false;
            } { return true; }
        }
        function ValidarInstrumentosCompuestosEstructurados() {
            strMensajeError = "";
            if (ValidaInstrumentos()) { return true; }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function ValidaInstrumentos() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= tbMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "-Código Mnemónico<br />"
            if (document.getElementById("<%=tbCodigoIsin.ClientID %>").value == "")
                strMsjCampOblig += "-Código ISIN<br />"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br />";
                return false;
            } { return true; }
        }
        function calcularMontos() {
            if (frmInvocador.tbValorUnitario.value != "" && frmInvocador.tbNumUnidades.value != "") {
                frmInvocador.tbValorEfecColocado.value = frmInvocador.tbValorUnitario.value * frmInvocador.tbNumUnidades.value;
                frmInvocador.tbValorNominal.value = frmInvocador.tbValorUnitario.value * frmInvocador.tbNumUnidades.value;
            }
            return false;
        }
        function calcularSBS() {
            $("#tbCodigoSBS").val(
                 $("#tbCodigoSBSinst").val() + $("#hdEmisor").val() + $("#hdCodigoSBSMoneda").val()
            );
            return false;
        }
        function Calcular() {
            if (frmInvocador.tbValorUnitario.value != "" && frmInvocador.tbNumUnidades.value != "") {
                total = frmInvocador.tbValorUnitario.value.toString().replace(/,/g, '') * frmInvocador.tbNumUnidades.value.toString().replace(/,/g, '');
                num = total;
                if (num != "") {
                    var pos1 = num.toString().lastIndexOf('.');
                    var pos2 = num.toString().substring(pos1 + 1);
                    var tmp1 = pos2 + '0000000'
                    var tmp2 = tmp1.substr(0, 7);
                    num = num.toString().replace(/$|,/g, '');
                    if (isNaN(num))
                        num = "0";
                    sign = (num == (num = Math.abs(num)));
                    num = Math.floor(num * 100 + 0.50000000001);
                    cents = num % 100;
                    num = Math.floor(num / 100).toString();
                    if (cents < 10) {
                        cents = "0" + cents + '0000000';
                        cents = cents.substr(0, 7);
                    }
                    else
                    { cents = tmp2; }
                    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
									num.substring(num.length - (4 * i + 3));
                    frmInvocador.tbValorEfecColocado.value = (((sign) ? '' : '-') + num + '.' + cents);
                    frmInvocador.tbValorNominal.value = (((sign) ? '' : '-') + num + '.' + cents);
                }
            }
            return false;
        }
        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbNumUnidades":
                    num = frmInvocador.tbNumUnidades.value; break;
                case "tbValorUnitario":
                    num = frmInvocador.tbValorUnitario.value; break;
                case "tbValorNominal":
                    num = frmInvocador.tbValorNominal.value; break;
                case "tbValorEfecColocado":
                    num = frmInvocador.tbValorEfecColocado.value; break;
                case "tbCapitalCompro":
                    num = frmInvocador.tbCapitalCompro.value; break;
                case "tbCapitalComproF2":
                    num = frmInvocador.tbCapitalComproF2.value; break;
                case "tbCapitalComproF3":
                    num = frmInvocador.tbCapitalComproF3.value; break;
                case "tbPosicionAct":
                    num = frmInvocador.tbPosicionAct.value; break;
                case "tbPorcPosicion":
                    num = frmInvocador.tbPorcPosicion.value; break;
                case "tbMargenInicial":
                    num = frmInvocador.tbMargenInicial.value; break;
                case "tbMargenMnto":
                    num = frmInvocador.tbMargenMnto.value; break;
                case "tbContractSize":
                    num = frmInvocador.tbContractSize.value; break;
            }

            num = num.toString().replace(/$|,/g, '');
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '0000000'
                var tmp2 = tmp1.substr(0, 7);
                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);
                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '0000000';
                    cents = cents.substr(0, 7);
                }
                else
                { cents = tmp2; }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
									num.substring(num.length - (4 * i + 3));
                switch (cajatexto) {
                    case "tbNumUnidades":
                        frmInvocador.tbNumUnidades.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbValorUnitario":
                        frmInvocador.tbValorUnitario.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbValorNominal":
                        frmInvocador.tbValorNominal.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbValorEfecColocado":
                        frmInvocador.tbValorEfecColocado.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbCapitalCompro":
                        frmInvocador.tbCapitalCompro.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbCapitalComproF2":
                        frmInvocador.tbCapitalComproF2.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbCapitalComproF3":
                        frmInvocador.tbCapitalComproF3.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbPosicionAct":
                        frmInvocador.tbPosicionAct.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbPorcPosicion":
                        frmInvocador.tbPorcPosicion.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbMargenInicial":
                        frmInvocador.tbMargenInicial.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbMargenMnto":
                        frmInvocador.tbMargenMnto.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbContractSize":
                        frmInvocador.tbContractSize.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                }
            }
            return false;
        }
        function llamarAceptar() {
            document.getElementById("imbAceptar").click();
        }

        $(function () {
            $("#datosRating").dialog({ resizable: false, autoOpen: false,
                buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
                hide: { effect: "fade", duration: 500 }
            });
            $("#datosRating").dialog("option", "width", 500).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
            $("#imgDetalleCR").click(function () {
                $("#datosRating").dialog('open');
                $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            });
        });
        $('form').live("submit", function () {
            $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            ShowProgress();
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="frmInvocador" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><div class="row"><div class="col-md-6"><h2>Mantenimiento de Administración de Valores</h2></div></div></header>
        
        <fieldset id="fdSeccionSuperior" runat="server">
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Renta
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" Width="150px" ID="ddlTipoRenta" onchange="SeleccionarTR()" AutoPostBack="True" />
                            <asp:Label Text="" runat="server" ID="lbAlerta" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Factor</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ReadOnly="true" ID="tbTipoFactor" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label runat="server" id="lblTemporal" class="col-sm-4 control-label">
                            Portafolio Asociado</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="200px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>

        <br />

        <fieldset id="fdSeccionCentral" runat="server">
            <legend></legend>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladó campo "Tipo Título" que no se utilizará a sección oculta| 25/05/18 --%>
            <div class="row">
                <div class="col-md-4" id="divCodigoMnemonico" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código Mnemonico</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbMnemonico" Width="150px" onBlur="Javascript:Calcular(); formatCurrency(frmInvocador.tbNumUnidades.id);" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divMercado" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMercado" runat="server" AutoPostBack="true" Width="200px" />
                        </div>
                    </div>
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladó campo "Tipo Título" que no se utilizará a sección oculta| 25/05/18 --%>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladó campo "Factor" que no se utilizará a sección oculta| 25/05/18 --%>
            <div class="row">
                <div class="col-md-4" id="divDescripcion" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripción</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="180px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divRating" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Rating Fondo</label><div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlRating" Width="150px" AutoPostBack="True" />
                            </div>
                    </div>
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladó campo "Factor" que no se utilizará a sección oculta| 25/05/18 --%>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladó campo "Agrupación","Bursatilidad", "Margen Inicial", "Margen Mant.", "Calificación", "Tasa de Encaje", "Indicadores","Tasa Spread","Grupo RegAux","Tipo Renta Fija", "Fecha de Liberación", "Piso", "Techo", "Garante", "Subyacente", "Precio Ejecicio", "Tamaño Emisión", "Situación"  que no se utilizará a sección oculta| 25/05/18 --%>
            <div class="row">
                <div class="col-md-4" id="divNumeroUnidades" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            N&uacute;mero de Unidades</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbNumUnidades" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="div1" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Rating Aseguradora</label><div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlRatingM" Width="150px"/>
                            </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" id="divValorUnitario" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Valor Unitario</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbValorUnitario" Width="150px" CssClass="Numbox-7"
                                onblur="Javascript:Calcular();" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" id="divTipoInstrumento" runat="server">
                    <div class="form-group" style="padding-left: 5px;">
                        <label class="col-sm-2 control-label">
                            Tipo Instrumento</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoSBSinst" Width="80px" ReadOnly="true" onblur="Javascript:calcularSBS(); return false;" />
                                <asp:LinkButton ID="lbkModalInstrumento" OnClientClick="return showModalInstrumento();"
                                    runat="server">
                                    <span runat="server" id="imbTipoInstrumento" class="add-on"><i class="awe-search"></i>
                                    </span>
                                </asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbSinTipoInst" Width="220px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" id="divValorNominal" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Valor Nomimal</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbValorNominal" Width="150px" CssClass="Numbox-7" /></div>
                    </div>
                </div>
                <div class="col-md-8" id="divEmisor" runat="server">
                    <div class="form-group" style="padding-left: 5px;">
                        <label id="Label1" runat="server" class="col-sm-2 control-label">
                            Emisor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbEmisor" Width="80px" ReadOnly="true" onblur="Javascript:calcularSBS(); return false;" />
                                <asp:LinkButton ID="lbkModalEmisor" OnClientClick="return showModalEmisor()" runat="server">
                                    <span runat="server" id="imbEmisor" class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbEmisorDesc" Width="220px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" id="divValorEfectivoColocado" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Valor Efectivo Colocado</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbValorEfecColocado" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divMoneda" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" id="divTipoCodigoValor" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo Código Valor</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoCodigoValor" runat="server" Width="150px">
                                <asp:ListItem Value="0">--SELECCIONE--</asp:ListItem>
                                <asp:ListItem Value="1">Código Valor</asp:ListItem>
                                <asp:ListItem Value="2">Código Isin</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divCodigoIsin" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código ISIN
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigoISIN" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divRowFechaEmisionTipoCupon" runat="server">
                <div class="col-md-4">
                    <div class="form-group" id="divFechaEmision" runat="server">
                        <label id="dvDiv111" runat="server" class="col-sm-5 control-label">
                            Fecha Emisión</label>
                        <label id="dvDiv112" runat="server" class="hidden">
                            Capital Comprometido</label><%--no se muestra--%>
                        <div class="col-sm-7">
                            <div id="dvDiv121" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaEmision" SkinID="Date" />
                                <span runat="server" id="imgFechaEmision" class="add-on"><i class="awe-calendar"></i>
                                </span>
                            </div>
                            <div id="dvDiv122" class="hidden" runat="server">
                                <asp:Button Text="Registro por Fondo" ID="btnCapComp" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divCodigoSBS" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigoSBS" Width="150px" onblur="Javascript:calcularSBS(); return false;"
                                onfocus="Javascript:calcularSBS(); return false;" MaxLength="9" />
                            <asp:TextBox runat="server" ID="txtCodigoSBSCorrelativo" Width="70px" MaxLength="5" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divRowFechaVencimientoTasaCupon" runat="server">
                <div class="col-md-4" id="divFechaVencimiento" runat="server">
                    <div class="form-group">
                        <label id="dvDiv211" runat="server" class="col-sm-5 control-label">
                            Fecha de Vencimiento</label>
                        <label id="dvDiv212" runat="server" class="hidden">
                            Posición Actual</label>
                        <div class="col-sm-7">
                            <div id="dvDiv221" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVencimiento" SkinID="Date" />
                                <span runat="server" id="imgFechaVcto" class="add-on"><i class="awe-calendar"></i>
                                </span>
                            </div>
                            <div id="dvDiv222" class="hidden" runat="server">
                                <asp:TextBox runat="server" ID="tbPosicionAct" Width="70px" CssClass="Numbox-7" />%
                                <asp:TextBox runat="server" ID="tbPorcPosicion" Width="50px" CssClass="Numbox-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divTipoCupon" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Cupón</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoCupon" Width="150px" AutoPostBack="True" />
                            <img id= "imgInfoTipoCupon" runat="server" src="../../../App_Themes/img/info_01.png" style="height: 24px" visible="false"
                                    title="Para los instrumentos con tipo cupón [A DESCUENTO] la Tasa Cupón No es obligatoria">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divRowPrimerVctoCuponPeriocidad" runat="server">
                <div class="col-md-4" id="divPrimerVctoCupon" runat="server">
                    <div class="form-group">
                        <label id="dvDiv311" runat="server" class="col-sm-5 control-label">
                            1er Vencimiento Cupón</label>
                        <div class="col-sm-7">
                            <div id="dvDiv321" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaPrimerCupon" SkinID="Date" />
                                <span runat="server" id="imgFechaPriCupon" class="add-on"><i class="awe-calendar"></i>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divVacio" runat="server">
                <div class="form-group">
                </div>
                </div>
                <div class="col-md-4" id="divTasaCupon" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tasa Cupón</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbTasaCupon" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divRowTipoAmortizacionIndicadores" runat="server">
                <div class="col-md-4" id="divTipoAmortizacion" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo Amortización</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoAmortizacion" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divPeriocidad" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Periodicidad</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPeriodicidad" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divRowGeneraInteres" runat="server">
                <div class="col-md-4" id="divGeneraInteres" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label" style="margin-top: -8px">
                            Genera Intereses</label>
                        <div class="col-sm-7">
                            <asp:CheckBox ID="chkGeneraIntereses" runat="server"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divIndicadores" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Indicadores</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlCotizacionVAC" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="divRowSubordinario" runat="server">
                <div class="col-md-4" id="divSubordinario" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label" style="margin-top: -8px">
                            Subordinario</label>
                        <div class="col-sm-7">
                            <asp:CheckBox ID="chkSubordinario" runat="server"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divRowPrecioDevengado" runat="server">
                <div class="col-md-4" id="divPrecioDevengado" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label" style="margin-top: -8px">
                            Precio Devengado</label>
                        <div class="col-sm-7">
                            <asp:CheckBox ID="chkPrecioDevengado" runat="server"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <div id="trCategoria" runat="server" class="row hidden">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Categoría</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlCategoria" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Estilo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlEstilo" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladó campo "Agrupación","Bursatilidad", "Margen Inicial", "Margen Mant.", "Calificación", "Tasa de Encaje", "Indicadores","Tasa Spread","Grupo RegAux","Tipo Renta Fija", "Fecha de Liberación", "Piso", "Techo", "Garante", "Subyacente", "Precio Ejecicio", "Tamaño Emisión", "Situación"  que no se utilizará a sección oculta| 25/05/18 --%>
            <%-- 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se añade nuevo campo (combo) Tipo Renta Riesgo | 17/05/18 --%>
            <hr />
            <div class="row">
                <div class="col-md-4" id="divTipoRentaRiesgo" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo Renta Riesgo</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoRentaRiesgo" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divClasificacionRating" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Clasif. Rating</label>
                        <div class="col-sm-7">
                            <img alt="Detalle Rating" id="imgDetalleCR" src="../../../App_Themes/img/icons/tree_list.png" style="cursor:pointer;" />
                        </div>
                    </div>
                </div>
            </div>
            <%-- 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se añade nuevo campo (combo) Tipo Renta Riesgo | 17/05/18 --%>
            <%-- 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se añade nuevo campo (checkbox) Paga Dividendo | 18/05/18 --%>
            <div class="row">
                <div class="col-md-4" id="divPagaDividendo" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label" style="margin-top: -8px">
                            Paga Dividendo</label>
                        <div class="col-sm-7">
                            <asp:CheckBox ID="chkPagaDividendo" runat="server" AutoPostBack="true"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <%-- 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se añade nuevo campo (checkbox) Paga Dividendo | 18/05/18 --%>
            <%-- 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se añade nuevo campo (combo) Frecuencia de Pago | 18/05/18 --%>
            <div class="row" id="divRowFrecuenciaPago" runat="server">
                <div class="col-md-4" id="divFrecuenciaPago" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Frecuencia de Pago</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlFrecuenciaPago" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <%-- 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se añade nuevo campo (combo) Frecuencia de Pago | 18/05/18 --%>
        </fieldset>
        <br />
        <fieldset id="fdSeccionInferior" runat="server">
            <legend>Base de Cálculo Cupón</legend>
            <div class="row" style="text-align: center">
                <div class="col-md-4 hidden" id="divTituloBaseCupon" runat="server" >
                    <label class="col-sm-9 control-label">
                        Base Cupón</label>
                </div>
                <div class="col-md-4 hidden" id="divChkBaseIC" runat="server" style="margin-top: 8px;">
                    <asp:CheckBox ID="chkBaseIC" runat="server" TextAlign="Right" AutoPostBack="true" 
                        Text="Base Interés Corrido"></asp:CheckBox>
                </div>
            </div>
            <div class="row">
                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se oculta: Campo será reemplazado por ddlIntCorrBase | 15/06/18 --%>
                <div class="col-md-4 hidden" id="divDiasAño" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            N Días
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlCuponNDias" Width="150px" />
                        </div>
                    </div>
                </div>
                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se oculta: Campo será reemplazado por ddlIntCorrBase | 15/06/18 --%>
                <div class="col-md-3" id="divBaseNDiasIC" runat="server">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base</label>
                        <div class="col-sm-3" style="width:90px">
                            <asp:DropDownList runat="server" ID="ddlIntCorrNDias" Width="75px" onchange="actualizarValorBaseMensual(this.selectedIndex)" />
                        </div>
                        <div class="col-md-2" id="divBaseIC" runat="server">
                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:DropDownList runat="server" ID="ddlIntCorrBase" Width="75px" onchange="actualizarValorBaseAnual(this.selectedIndex)" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se oculta: Campo será reemplazado por ddlIntCorrBase | 15/06/18 --%>
                <div class="col-md-4 hidden" id="divBaseCupon" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Base</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlCuponBase" Width="150px" />
                        </div>
                    </div>
                </div>
                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se oculta: Campo será reemplazado por ddlIntCorrBase | 15/06/18 --%>
    
            </div>
        </fieldset>
        <br />
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladaron los campos que no se utilizará a sección oculta | 16/05/18 --%>
        <fieldset class="hidden">
            <legend>Campos Retirados </legend>
            <div class="row">
          
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Título</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoTitulo" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Factor</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbFactor" Width="150px" CssClass="Numbox-7" /></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Agrupación</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlAgrupacion" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
         
            <div runat="server" id="trMargenes" class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Margen Inicial</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbMargenInicial" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Margen Mant.</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbMargenMnto" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div runat="server" id="trContractSize" class="row" visible="false">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Contract Size</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbContractSize" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Bursatibilidad</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlBursatilidad" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Calificacion</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCalificacion" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tasa Encaje</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbTasaEncaje" CssClass="Numbox-7" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Liberación</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbFechaLiberada" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tasa Spread</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbTasaSpread" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Grupo RegAux</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlGrupoContable" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label runat="server" id="lblTipoRentaFija" class="col-sm-4 control-label">
                            Tipo Renta Fija</label>
                        <label runat="server" id="lblTipoLiquidez" class="col-sm-4 control-label">
                            Tipo Liquidez</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlTipoRentaFija" runat="server" />
                            <asp:DropDownList ID="ddlTipoLiquidez" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Piso</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtPiso" runat="server" CssClass="Numbox-7" Width="150px" Enabled="false"
                                value="0" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Techo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtTecho" Width="150px" CssClass="Numbox-7" Enabled="false"
                                value="0" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Garante</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtGarante" runat="server" Width="230px" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Subyacente</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSubyacente" Width="250px" Enabled="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Precio Ejercicio</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtPrecioEjercicio" runat="server" CssClass="Numbox-7" Width="150px"
                                Enabled="false" value="0" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tamaño Emsión</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtTamanoEmision" Width="150px" CssClass="Numbox-7"
                                Enabled="false" value="0" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-10">
                    <div class="form-group" style="padding-left: 5px;">
                        <label class="col-sm-2 control-label">
                            Observaciones</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="tbObservaciones" Width="350px" /></div>
                    </div>
                </div>
            </div>
        </fieldset>
        <%--<%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se trasladaron los campos que no se utilizará a sección oculta | 16/05/18 --%>
        <div class="row">
            <div class="col-md-12" style="text-align: right">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button Text="Custodios" ID="btnCustodios" runat="server" />
                        <asp:Button Text="Cuponera" ID="btnCuponNormal" runat="server" Visible="False" />
                        <asp:Button Text="Aceptar" ID="btnAceptar" runat="server" />
                        <asp:Button Text="Retornar" ID="btnSalir" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <br />

        <%--INICIO | ZOLUXIONES | Ian Pastor M. | Proy. Límites Mandatos - Se crea DIV para mostrar los rating asociados al instrumento | 13/10/2018 --%>
        <div id="divFondo" style="width:100%; height:100%; position:absolute; top:0; left:0; display:none; background-color:#7D7D7D; opacity:0.7"></div>
        <div id="datosRating" title ="Clasificación Rating">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group" style="font-size:12px;">
                                <label class="col-sm-6 control-label"><b>Rating Interno</b></label>
                                <div class="col-sm-6">
                                    <%--<asp:DropDownList ID="ddlRatingInterno" runat="server" />--%>
                                    <asp:TextBox ID="txtRatingInterno" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group" style="font-size:12px;">
                                <label class="col-sm-6 control-label"><b>Rating Externo</b></label>
                                <div class="col-sm-6">
                                    <%--<asp:DropDownList ID="ddlRatingExterno" runat="server" />--%>
                                    <asp:TextBox ID="txtRatingExterno" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group" style="font-size:12px;">
                                <label class="col-sm-6 control-label"><b>Fortaleza Financiera</b></label>
                                <div class="col-sm-6">
                                    <%--<asp:DropDownList ID="ddlFortalezaFinanciera" runat="server" />--%>
                                    <asp:TextBox ID="txtFortalezaFinanciera" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--FIN | ZOLUXIONES | Ian Pastor M. | Proy. Límites Mandatos - Se crea DIV para mostrar los rating asociados al instrumento | 13/10/2018 --%>

    </div>
    <input id="hdnTipoRenta" type="hidden" runat="server" />
    <input id="hdnNemonico" type="hidden" runat="server" />
    <input id="hdnCodigoClaseInstrumento" type="hidden" runat="server" />
    <input id="hdEmisor" type="hidden" runat="server" />
    <input id="hdCantidadIE" type="hidden" runat="server" />
    <input id="hdRentaFijaIE" type="hidden" runat="server" />
    <input id="hdRentaVarIE" type="hidden" runat="server" />
    <input id="hdRentaFijaIC" type="hidden" runat="server" />
    <input id="hdRentaVarIC" type="hidden" runat="server" />
    <input id="hdCodigoTipoDerivado" type="hidden" runat="server" />
    <input id="hdEmisorIE" type="hidden" runat="server" />
    <asp:HiddenField ID="hdCodigoSBSinst" runat="server" />
    <asp:HiddenField ID="hdSinTipoInst" runat="server" />
    <asp:HiddenField ID="hdEmisorVal" runat="server" />
    <asp:HiddenField ID="hdEmisorDesc" runat="server" />
    <asp:HiddenField ID="hdTipoTasaVariable" runat="server" />
    <asp:HiddenField ID="hdDiasTTasaVariable" runat="server" />
    <asp:HiddenField ID="hdTasaVariable" runat="server" />
    <asp:HiddenField runat="server" ID="_Modal" />
    <asp:HiddenField runat="server" ID="hdCodigoSBSMoneda" />
    <div class="loading" align="center" id="divLoading" >
        <img src="../../../App_Themes/img/icons/loading.gif" alt="" />
    </div>
    </form>
</body>
</html>