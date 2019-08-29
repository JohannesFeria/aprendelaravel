<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAgregarSeries.aspx.vb"
    Inherits="Modulos_ValorCuota_frmAgregarSeries" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<script src="../../App_Themes/js/html2canvas.js"></script>
<script src="../../App_Themes/js/jspdf.min.js"></script>
<head id="Head1" runat="server">
    <title>Calculo de Series</title>
    <style type="text/css">
        .caberaGrilla
        {
            color: #0039A6;
            background-color: #EFF4FA;
            font-family: Trebuchet MS;
            font-size: 11px;
            font-weight: bold;
            height: 23px;
        }
        
        
        .win01
        {
            border: solid 1px gray;
            background-color: #80808052;
            position: absolute;
            z-index: 10;
            width: 100%;
            height: 160%;
            text-align: center;
        }
        .cont01
        {
            border: solid 1px gray;
            background-color: white;
            display: inline-block;
            margin-top: 50px;
            padding: 10px 20px;
            border-radius: 5px;
            width: 700px;
        }
        
        .cont01 input[type=text]
        {
            background-color: White;
            cursor: default;
        }
        
        .cont01 div[class=row]
        {
            margin-top: 5px;
        }
        
        .tab01
        {
            width: 100%;
        }
        .tab01 > tbody > tr > td
        {
            padding: 2px 5px;
            border: Solid 1px gray;
        }
        .span01
        {
            font-size: 17px;
        }
        
        .tooltip > div
        {
            /*background-color: gray;*/
        }
        
        .pop1_frm_print input[type=text]
        {
            color: black !important;
            background-color: white;
            border-color: black !important;
            border-width: 1px !important;
            margin-top: 2px;
        }
        .pop1_frm_print label
        {
            color: black;
        }
    </style>
</head>
<script type="text/javascript">
        <%--OT10927 - 21/11/2017 - Ian Pastor M. Muestra una ventana con el detalle de las operaciones de ventas de títulos.--%>
        $(function () {
            $("#divDetalleVentaTitulo").dialog({ resizable: false, autoOpen: false,
                buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
                hide: { effect: "fade", duration: 500 }
            });
            //$("#divDetalleVentaTitulo").dialog("option", "width", 550);
            //Elimina el botón "X" del modal
            $("#divDetalleVentaTitulo").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
            $("#imgDetalleVT").click(function () {
                $("#divDetalleVentaTitulo").dialog('open');
                $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            });
        });

        function MostrarDetalleVentaTitulo() {
            $("#divDetalleVentaTitulo").dialog({ resizable: false, autoOpen: false,
                buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
                hide: { effect: "fade", duration: 500 }
            });
            $("#divDetalleVentaTitulo").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
            $("#imgDetalleVT").click(function () {
                $("#divDetalleVentaTitulo").dialog('open');
                $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            });

            $("div>.ui-dialog-buttonset>:button").addClass("btn btn-integra");
        }

        //OT10927 - Fin  

           $(document).ready(function () {

        // INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-24 - Integración del Proceso de Cierre Operaciones en SIF-Fondos      
        $("#<%=btnEjecutarCierreOpe.ClientID%>").click(function () {
            $('body,html').animate({scrollTop : 0}, 1500);
            IniciarPopup(false);
            return false;
        });
        $("#<%=btnSoloLecturaCierreOpe.ClientID%>").click(function () {
            $('body,html').animate({scrollTop : 0}, 1500);
            IniciarPopup(true);
            return false;
        });
        $("#Popup01_btnEjecutar").click(function () {

            EjecutarPreorden();
            return false;
        });
        $("#Popup01_btnCerrar").click(function () { $("#popup01").hide(); });
        $("#Popup01_btnCerrar2").click(function () { $("#popup01").hide(); });


        // FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-24 - Integración del Proceso de Cierre Operaciones en SIF-Fondos



        // INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-09-24 - Implementación del Botón Imprimir a PDF del detalle del Cierre
        var elemParaImprimir = $("#pop1_frm>fieldset"); // global variable
        var elemCanvas; // global variable

        $("#Popup01_btnImprimir").click(function () {
            $(elemParaImprimir).addClass("pop1_frm_print"); // Establecemos estilos para impresión

            html2canvas(elemParaImprimir, {
                onrendered: function (canvas) {
                    $(elemParaImprimir).removeClass("pop1_frm_print"); // Removemos los estilos de impresión

                    // $("#previewImage").append(canvas); // Mostrarlo como CANVAS dentro de un DIV
                    elemCanvas = canvas;

                    var nombreArchivo = 'Cierre de Fondo ' + $("#pop1_txtFondo").val() + ' ' + $("#pop1_txtFecha").val().replace(/\//g, "-");
                    descargarCanvasComoPDF(nombreArchivo);
                }
            });

        });
        function descargarCanvasComoPDF(nombreArchivo) {
            var imgData = elemCanvas.toDataURL("image/png", 1.0); // Imprimir como JPGE // var imgData = elemCanvas.toDataURL("image/jpeg", 1.0);
            //var options = { orientation: 'p', unit: 'mm', format: custom };
            var pdf = new jsPDF();

            pdf.addImage(imgData, 'PNG', 30, 40, 150, 86); // Coordenadas dentro del PDF => 18, 40
            pdf.save(nombreArchivo + ".pdf");
        }

        //          $("#btn-Convert-Html2Image").on('click', function () {
        //              var imgageData = elemCanvas.toDataURL("image/png");
        //              // Now browser starts downloading it instead of just showing it
        //              var newData = imgageData.replace(/^data:image\/png/, "data:application/octet-stream");
        //              $("#btn-Convert-Html2Image").attr("download", "your_pic_name.png").attr("href", newData);
        //          });

        // FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-09-24 - Implementación del Botón Imprimir a PDF del detalle del Cierre

    });

    // INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-24 - Integración del Proceso de Cierre Operaciones en SIF-Fondos
    /* CRumiche: idFondoOpe=ID Fondo en BD Operaciones */
    function IniciarPopup(soloLectura) {
        $("#popup01").show();
        $("#popup01_loading").show();
        $("#pop1_frm").hide();
        $("#pop1_Errores").hide();

        var _idFondoOpe = $("#<%=hdCodigoPortafolioSisOpe.ClientID%>").val();
        var _codUsuario = '<%=Session("UInfo_CodUsuario")%>'
        var _check = $("#<%=hdCheckOperaciones.ClientID%>").val();
        if (_check != '0') {$('#pop1_chkEnviarCorreos').prop('checked', false); } else {$('#pop1_chkEnviarCorreos').prop('checked', true); }
        var param = { idFondoOpe: _idFondoOpe, codUsuario: _codUsuario };
        var _url = "../../MetodosExternos.aspx/Get_VerificarPreviewPrecierre";

        if (soloLectura) { /* CRumiche: Mostramos el Histórico */
            var _fecha = $("#<%=tbFechaInforme.ClientID%>").val();
            param = { idFondoOpe: _idFondoOpe, fecha: _fecha };
            _url = "../../MetodosExternos.aspx/Get_InfoPrecierreHistorico";
        }

        $.ajax({
            url: _url,
            data: JSON.stringify(param),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8;",
            success: function (response) {
                // Para manejar los datos obtenidos correctamente
                if (typeof response.d == "object") {
                    $("#popup01_loading").hide();

                    var resul = response.d;

                    if (!resul.ProcesoErrado) {
                        $("#pop1_frm").show();
                        MapearCampos(resul);

                        if (soloLectura) {
                            $("#Popup01_btnEjecutar").hide();
                            $("#Popup01_btnCerrar").val("Cerrar");
                            $("#pop1_divEnviarCorreos").hide();
                        } else {
                            $("#Popup01_btnEjecutar").show();
                            $("#Popup01_btnCerrar").val("Cancelar");
                            $("#pop1_divEnviarCorreos").show();
                        }

                    } else {
                        $("#pop1_Errores").show();
                        $("#pop1_areaErrores").val(resul.Notificaciones);
                    }
                }
                else {
                    $("#popup01").hide();
                    alert("No se pudieron cargar los datos correctamente");
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) { $("#popup01").hide(); alert(textStatus); }
        });
    }

    /*CRumiche: idFondoOpe=ID Fondo en BD Operaciones*/
    function EjecutarPreorden() {
        $("#popup01").show();
        $("#popup01_loading").show();
        $("#pop1_frm").hide();
        $("#pop1_Errores").hide();

        var _idFondoOpe = $("#<%=hdCodigoPortafolioSisOpe.ClientID%>").val();
        var _enviarCorreos = $("#pop1_chkEnviarCorreos").is(":checked") ? 1 : 0;
        var _codUsuario = '<%=Session("UInfo_CodUsuario")%>'

        // Se captura el valor de comisionSAFM sin IGV
        var _comisionSAFM = parseFloat($("#<%=tbComiSAFM.ClientID%>").val().replace(",","").replace(",","")) / (1 + parseFloat($("#<%=txtigv.ClientID%>").val()));

        if (isNaN(_comisionSAFM)) _comisionSAFM = 0;
        var param = { comisionSAFM: _comisionSAFM, idFondoOpe: _idFondoOpe, enviarCorreos: _enviarCorreos, codUsuario: _codUsuario };
        //          var param = { idFondoOpe: _idFondoOpe, enviarCorreos: _enviarCorreos, codUsuario: _codUsuario };
        $.ajax({
            url: "../../MetodosExternos.aspx/Get_EjecutarPrecierre",
            data: JSON.stringify(param),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8;",
            success: function (response) {
                // Para manejar los datos obtenidos correctamente
                if (typeof response.d == "object") {
                    $("#popup01_loading").hide();

                    var resul = response.d;

                    if (!resul.ProcesoErrado) {
                        $("#pop1_frm").show();
                        MapearCampos(resul);

                        $("#pop1_divEnviarCorreos").hide();
                        $("#Popup01_btnEjecutar").hide();
                        $("#Popup01_btnCerrar").val("Cerrar");
                        $("#Popup01_btnCerrar").click(function () { location.reload(); });

                        alertify.alert('El CIERRE se realizó correctamente', function () { });
                    } else {
                        $("#pop1_Errores").show();
                        $("#pop1_areaErrores").val(resul.Notificaciones);
                    }
                }
                else {
                    $("#popup01").hide();
                    alert("No se pudieron cargar los datos correctamente");
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) { $("#popup01").hide(); alert(textStatus); }
        });
    }

    function MapearCampos(resul) {

        $("#pop1_txtFondo").val(resul.Fondo);
        $("#pop1_txtFecha").val(resul.Fecha);
        $("#pop1_txtValorCuota").val(resul.ValorCuota);

        $("#pop1_txtSaldoAyer").val(resul.SaldoAyer);
        $("#pop1_txtSaldoDia").val(resul.SaldoDia);

        $("#pop1_txtSaldoCuotasAyer").val(resul.SaldoCuotasAyer);
        $("#pop1_txtSaldoCuotasDia").val(resul.SaldoCuotasDia);

        $("#pop1_txtTotalIngresoBruto").val(resul.TotalIngresoBruto);
        $("#pop1_txtTotalEgresoBruto").val(resul.TotalEgresoBruto);

        $("#pop1_txtTotalIngresoNeto").val(resul.TotalIngresoNeto);
        $("#pop1_txtTotalEgresoNeto").val(resul.TotalEgresoNeto);

        $("#pop1_txtTotalIngresoCuotas").val(resul.TotalIngresoCuotas);
        $("#pop1_txtTotalEgresoCuotas").val(resul.TotalEgresoCuotas);

        $("#pop1_txtConversionCuotas").val(resul.ConversionCuotas);
        $("#pop1_txtEgresoCuotasRT").val(resul.EgresoCuotasRT);

        $("#pop1_txtEgresoMontoRT").val(resul.EgresoMontoRT);
        $("#pop1_txtPagoFlujoMonto").val(resul.PagoFlujoMonto);
        $("#pop1_txtRetencionFlujo").val(resul.RetencionFlujo);

        if ($("#pop1_txtConversionCuotas").val() === "-") $("#pop1_grupoConversionCuotas").hide();
        if ($("#pop1_txtPagoFlujoMonto").val() === "-") $("#pop1_grupoPagoFlujoMonto").hide();
        if ($("#pop1_txtRetencionFlujo").val() === "-") $("#pop1_grupoRetencionFlujo").hide();

        $("#pop1_areaNotificaciones").val(resul.Notificaciones);

        if (resul.TipoProceso === "V")
            $("#pop1_lblNotificaciones").text("Los siguientes rescates serían rechazados");
        if (resul.TipoProceso === "E")
            $("#pop1_lblNotificaciones").text("Proceso Finalizado, pero se tienen las siguientes alertas:");

        if (resul.Notificaciones.length > 0) {
            $("#pop1_lblNotificaciones").show();
            $("#pop1_divNotificaciones").show();
        }
        else {
            $("#pop1_lblNotificaciones").hide();
            $("#pop1_divNotificaciones").hide();
        }

        // Seccion para mostrar las diferencias en algunos valores del Valor Cuota
        habilitarTooltipDiferencias($("#tbValCuotaCierreValores").val()
                                    , $("#pop1_txtValorCuota").val()
                                    , "pop1_txtValorCuota"
                                    , "Valor Cuota");
        habilitarTooltipDiferencias($("#tbValPatriPrecierre2").val()
                                    , $("#pop1_txtSaldoAyer").val()
                                    , "pop1_txtSaldoAyer"
                                    , "Saldo al Día de Ayer");
        habilitarTooltipDiferencias($("#tbValPatCierreValores").val()
                                    , $("#pop1_txtSaldoDia").val()
                                    , "pop1_txtSaldoDia"
                                    , "Saldo del Día");
        habilitarTooltipDiferencias($("#tbValCuotaPrecierre").val()
                                    , $("#pop1_txtSaldoCuotasAyer").val()
                                    , "pop1_txtSaldoCuotasAyer"
                                    , "Saldo Cuotas al Día de Ayer");
        habilitarTooltipDiferencias($("#tbValCuotaCierreCuota").val()
                                    , $("#pop1_txtSaldoCuotasDia").val()
                                    , "pop1_txtSaldoCuotasDia"
                                    , "Saldo Cuotas del Día");
        habilitarTooltipDiferencias($("#tbRescatesValores").val()
                                    , $("#pop1_txtTotalEgresoBruto").val()
                                    , "pop1_txtTotalEgresoBruto"
                                    , "Total Egreso Cuotas");

    }

    /* CRumiche: Resaltamos en color Rojo los campos que tienen diferencias y en color verde los que son iguales (SIT vs Operaciones) */
    function habilitarTooltipDiferencias(valA, valB, controlConTooltip, tituloControl) {
        var auxValA = ifNoNumber(parseFloat(valA.replace(/,/g, "")), 0);
        var auxValB = ifNoNumber(parseFloat(valB.replace(/,/g, "")), 0);

        var diff = (auxValA - auxValB).toFixed(7);
        if (diff != 0) { // CRumiche: Hay diferencias
            $("#" + controlConTooltip).attr("title", "Diferencias en '" + tituloControl + "' (SIT - OPE) => "
                            + formatNumber(auxValA.toString()) + " - " + formatNumber(auxValB.toString()) + " => " + formatNumber(diff));

            $("#" + controlConTooltip).css("background-color", "rgb(241, 192, 138)");
            //$( "#" + controlConTooltip ).tooltip();
        } else // CRumiche: Todo Correcto
            $("#" + controlConTooltip).css("border", "Solid 2px #4bb54b");
    }

    function ifNoNumber(val, defaultVal) {
        if (!$.isNumeric(val)) return defaultVal;
        return val;
    }
    // FIN | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-24 - Integración del Proceso de Cierre Operaciones en SIF-Fondos
</script>
<body>
    <div id="popup01" class="win01" style="display: none">
        <div class="winBloqueador-inner" id="popup01_loading">
            <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
        </div>
        <div class="cont01" id="pop1_frm" style="display: none">
            <fieldset>
                <legend>CIERRE - OPERACIONES</legend>
                <div class="row">
                    <div class="col-sm-9">
                        <div class="form-group">
                            <label class="col-sm-4 control-label" style="text-align: right;">
                                Fondo</label>
                            <div class="col-sm-8">
                                <input id="pop1_txtFondo" type="text" readonly="readonly" style="width: 100%;" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Fecha</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtFecha" type="text" readonly="readonly" style="width: 150px;" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Valor Cuota</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtValorCuota" type="text" readonly="readonly" class="Numbox-7_12 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Saldo día de ayer</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtSaldoAyer" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="100000000000000000" step="0.01" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Saldo del día</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtSaldoDia" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Saldo cuotas al día de ayer</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtSaldoCuotasAyer" type="text" readonly="readonly" class="Numbox-7_12 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="100000000000000000" step="0.01" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Saldo cuotas del día</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtSaldoCuotasDia" type="text" readonly="readonly" class="Numbox-7_12 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Total Ingreso Bruto</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtTotalIngresoBruto" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="100000000000000000" step="0.01" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Total Egreso Bruto</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtTotalEgresoBruto" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Total Ingreso Neto</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtTotalIngresoNeto" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="100000000000000000" step="0.01" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Total Egreso Neto</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtTotalEgresoNeto" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Total Ingreso Cuotas</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtTotalIngresoCuotas" type="text" readonly="readonly" class="Numbox-7_12 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="100000000000000000" step="0.01" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Total Egreso Cuotas</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtTotalEgresoCuotas" type="text" readonly="readonly" class="Numbox-7_12 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group" id="pop1_grupoConversionCuotas">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Conversión de Cuotas</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtConversionCuotas" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="100000000000000000" step="0.01" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Egreso Cuotas Retención Traspasos</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtEgresoCuotasRT" type="text" readonly="readonly" class="Numbox-7_12 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Egreso Monto Retención Traspasos</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtEgresoMontoRT" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="pop1_grupoPagoFlujoMonto">
                    <div class="col-sm-6">
                        <div class="form-group">
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Pago de Flujo Monto</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtPagoFlujoMonto" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="pop1_grupoRetencionFlujo">
                    <div class="col-sm-6">
                        <div class="form-group">
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">
                                Retención por Flujo</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtRetencionFlujo" type="text" readonly="readonly" class="Numbox-2 NumBox"
                                    style="width: 150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="pop1_divNotificaciones" style="display: none">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <br />
                            <label class="col-sm-12 control-label" id="pop1_lblNotificaciones">
                            </label>
                            <div class="col-sm-12">
                                <textarea id="pop1_areaNotificaciones" style="width: 100%; height: 70px;" readonly="readonly"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 25px;" id="pop1_divEnviarCorreos">
                    <div class="col-sm-3">
                        <div class="form-group">
                        </div>
                    </div>
                    <div class="col-sm-9">
                        <div class="form-group">
                            <label class="col-sm-8 control-label" style="text-align: right;">
                                Enviar Correos al Ejecutar</label>
                            <div class="col-sm-4" style="text-align: left; margin-top: 5px">
                                <input id="pop1_chkEnviarCorreos" type="checkbox" checked="checked" />
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <input type="submit" value="Cancelar" id="Popup01_btnCerrar" class="btn btn-integra"
                style="min-width: 80px; text-align: center; width: auto;" />
            <input type="submit" value="Ejecutar Cierre" id="Popup01_btnEjecutar" class="btn btn-integra"
                style="min-width: 80px; text-align: center; width: auto;" />
            <input type="submit" value="Imprimir" id="Popup01_btnImprimir" class="btn btn-integra"
                style="min-width: 80px; text-align: center; width: auto;" />
            <%--Preview Image de Canvas--%>
            <%--<div id="previewImage"></div> --%>
        </div>
        <div class="cont01" id="pop1_Errores" style="display: none">
            <fieldset>
                <legend>CIERRE - OPERACIONES</legend>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label class="col-sm-12 control-label">
                                Se han encontrado problemas durante el procesamiento</label>
                            <div class="col-sm-12">
                                <textarea id="pop1_areaErrores" style="width: 100%; height: 70px;" readonly="readonly"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <input type="submit" value="Cancelar" id="Popup01_btnCerrar2" class="btn btn-integra"
                style="min-width: 80px; text-align: center; width: auto;" />
        </div>
    </div>
    <div id="divBloqueadorFondo" class="win01" style="display: none">
        <div class="winBloqueador-inner" id="Div6">
            <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
        </div>
    </div>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <br />
        <div class="col-md-3"> <font size="5">Calculo Valor Cuota Series</font></div>
        <asp:UpdatePanel ID="upControles" runat="server">
            <ContentTemplate>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Visible="false" />
                    <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
         <div class="col-md-3" style="text-align: left;">
            <asp:Button ID="btnEjecutarCierreOpe" runat="server" Text="Ejecutar Cierre en Ope."
                Visible="false" />
            <asp:Button ID="btnSoloLecturaCierreOpe" runat="server" Text="Ver Cierre en Ope."
                Visible="false" />
            <asp:Button ID="Button1" runat="server" Text="Cancelar" />
        </div>
        <br />
        <hr />
        <asp:UpdatePanel ID="upCalculoValorCuota" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Portafolio</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbPortafolio" ReadOnly="true" runat="server" Width="80px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fecha Informaci&oacute;n</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbFechaInforme" ReadOnly="true" runat="server" Width="80px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="col-sm-4">
                                <asp:TextBox ID="tbSerie" ReadOnly="true" Width="80px" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Valor Cuota Anterior</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtVCA" ReadOnly="true" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Diferencia Valor Cuota</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtVCDiferencia" ReadOnly="true" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upInversiones" runat="server">
            <ContentTemplate>
                <fieldset id="FormInversiones" runat="server">
                    <legend>Inversiones</legend>
                    <asp:Panel ID="Panel1" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Inversiones [t-1]</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="tbInversionesT1" ReadOnly="true" onkeypress="return soloNumeros(event)"
                                            runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Compras [t]</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="tbComprasT" ReadOnly="true" onkeypress="return soloNumeros(event)"
                                            runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Ventas y Venci. [t]</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="tbVentasVenciT" ReadOnly="true" onkeypress="return soloNumeros(event)"
                                            runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Rentabilidad [t]</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="tbRentabilidadT" ReadOnly="true" onkeypress="return soloNumeros(event)"
                                            runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Val. Forwards [t]</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="tbValoriForwardT" ReadOnly="true" onkeypress="return soloNumeros(event)"
                                            runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Val. Swap [t]</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="tbValoriSwapT" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <b>Inversiones [T] Subtotal</b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbInversionesSubTotal" ReadOnly="true" onkeypress="return soloNumeros(event)"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <b>I.G.V.</b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtigv" ReadOnly="true" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <b>Porcentaje de comisión</b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtporcom" ReadOnly="true" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upPrecierre" runat="server">
            <ContentTemplate>
                <fieldset id="FormPrecierre" runat="server">
                    <legend>Valor Cuota Precierre</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Caja (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCajaPrecierre" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CxC Monto Liberada(Precierre)</label>
                                <div class="col-sm-8">
                                    <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosPrecierre"--%>
                                    <asp:TextBox ID="txtMontoDividendosPrecierre" runat="server" CssClass="Numbox-7"
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <%--OT10986 - 13/12/2017 - Ian Pastor M. Agregar columna CxP Liberada--%>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CxP Liberada</label>
                                <div class="col-sm-8">
                                    <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosPrecierre"--%>
                                    <asp:TextBox ID="txtCxPLiberadaPreCierre" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <%--OT10986 - Fin--%>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXC Venta de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCVentaTitulo" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                    <%--<img alt="Detalle" id="imgDetalleVT" src="../../App_Themes/img/icons/tree_list.png" />--%>
                                    <%--OT10927 - 22/11/2017 - Ian Pastor M. Control imagen que permite mostrar una ventana con el detalle de las CxC Venta de título --%>
                                    <asp:ImageButton ID="imgDetalleVT" runat="server" SkinID="imgMenu" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otras CXC</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrasCXC" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se crea campo  oculto para Ajustes CXC | 16/07/18 --%>
                                    <input id="hdAjustesCxCPrecierre" type="hidden" runat="server" />
                                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se crea campo  oculto para Ajustes CXC | 16/07/18 --%>
                                    <input id="hdDevolucionComisionUnificada" type="hidden" runat="server" />
                                    <input id="hdOtrasCxCPreCierre" type="hidden" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXC (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCPreCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div1" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXC V. Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCExclusivos" runat="server" ReadOnly="true" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otras CXC exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbotrasCXCExclusivos" runat="server" ReadOnly="true" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXP Compra de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPtitulo" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otras CXP</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrasCXP" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                    <input id="hdAjustesCxPPrecierre" type="hidden" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXP (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPPreCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div2" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otras CXP Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPExclusivos" runat="server" ReadOnly="true" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CxP C. de Título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPCompraTitulo" runat="server" ReadOnly="true" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div id="Div3" runat="server" class="col-md-4" visible="false">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otros Gastos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastos" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otros Ingresos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresos" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    O. Gastos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastosExlusivos" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    O. Ingresos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresosExlusivos" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="FilaPrecierre" runat="server" class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    V. Pat. Precierre 1</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatriPrecierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Comisión SAFM</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbComiSAFM" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    V. Pat. Precierre 2</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatriPrecierre2" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <b>V. Cuota Precierre (Cuota) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValCuotaPrecierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <b>V. Cuota Precierre (Valor) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValValoresPrecierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upCierre" runat="server">
            <ContentTemplate>
                <fieldset id="FormCierre" runat="server">
                    <legend>Valor Cuota Cierre</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Aportes Cuotas</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbAporteCuota" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Aportes Valores</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbAporteValores" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Aportes Liberadas</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtAporteLiberadas" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Rescates Cuotas</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbRescatesCuota" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Rescates Valores</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbRescatesValores" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Retenciones pendientes</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtRetencionPendiente" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Caja</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCajaCierre" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CxC Monto Liberada(Cierre)</label>
                                <div class="col-sm-8">
                                    <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosCierre"--%>
                                    <asp:TextBox ID="txtMontoDividendosCierre" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXC Venta de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCVentaTituloCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otras CXC</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosCXCCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXC (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div4" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXC V. Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCExclusivosCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXP Compra de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPtituloCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otras CXP</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPotrasCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CXP (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrasCXPCierre" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div5" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otras CXP Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPExclusivosCierre" runat="server" ReadOnly="true" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    CxP C. de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPCompraTituloCierre" runat="server" ReadOnly="true" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otros Gastos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastosCierre" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Otros Ingresos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresosCierre" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    O. Gastos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastosExlusivosCierre" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    O. Ingresos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresosExlusivosCierre" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    V. Pat. Cierre(Cuota)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatCierreCuota" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    V. Pat. Cierre(Valor)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatCierreValores" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <b>V. Cuota Cierre(Cuota) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValCuotaCierreCuota" runat="server" ReadOnly="true" onkeypress="return soloNumeros(event)"
                                        Style="font-weight: bold; background-color: #e3e3e3;" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <b>V. Cuota Cierre(Valor) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValCuotaCierreValores" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <%--Modal: Detalle Venta de títulos--%>
        <div id="divDetalleVentaTitulo" title="Detalle Cuentas por Cobrar Venta de Títulos">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size: 12px;">
                                <label class="col-sm-6 control-label">
                                    <b>Venta de títulos acumulado</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtVentaTitulosDetalle" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size: 12px;">
                                <label class="col-sm-6 control-label">
                                    <b>Dividendos</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtDividendosDetalle" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>
    <input id="hdCodigoPortafolioSisOpe" type="text" runat="server" class="hidden" />
    <input id="hdCheckOperaciones" type="text" runat="server" class="hidden" />
    <div style="display:none;">
        <asp:TextBox ID="txtDevolucionComisionDiaria" runat="server"></asp:TextBox>
    </div>
    </form>
</body>
</html>