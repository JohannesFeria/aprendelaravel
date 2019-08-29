<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSeriesPortafolio.aspx.vb"
    Inherits="Modulos_ValorizacionCustodia_Valorizacion_frmSeriesPortafolio" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<script src="../../App_Themes/js/html2canvas.js"></script>
<script src="../../App_Themes/js/jspdf.min.js"></script>
<head id="Head1" runat="server">
    <title>Valores Portafolio</title>
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
        
        #btnRefrescar, #btnRefrescar:hover
        {
            background-image: url(../../App_Themes/img/refresh01.png);
            background-size: contain;
            background-repeat: no-repeat;
            background-position-x: center;
            background-position-y: center;
            min-width: 50px !important;
            margin-right: 35px;
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
    //OT10927 - 22/11/2017 - Ian Pastor M. Modal detalle de CxC Venta de título
    $(function () {

        $('#tbFechaOperacion').change(function () {
            $('#listButtons').hide();
        });

        $('#ddlPortafolio').change(function () {
            $('#listButtons').hide();
        });

        $("#datosOtrasCXP").dialog({ resizable: false, autoOpen: false,
            buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
            hide: { effect: "fade", duration: 500 }
        });
        //Elimina el botón "X" del modal
        $("#datosOtrasCXP").dialog("option", "width", 750).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
        $("#imgDetalle").click(function () {
            $("#datosOtrasCXP").dialog('open');
            $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
        });

        //INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se replica para mostrar DIV datosOtrasCxc | 22/06/18
        $("#datosOtrasCxC").dialog({ resizable: false, autoOpen: false,
            buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
            hide: { effect: "fade", duration: 500 }
        });
        $("#datosOtrasCxC").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
        $("#imgDetalleCaja").click(function () {
            $("#datosOtrasCxC").dialog('open');
            $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
        });
        //FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se replica para mostrar DIV datosOtrasCxc | 22/06/18

        //INICIO | ZOLUXIONES | DACV | SPRINT III - RF017 | 22/06/18
        $("#datosDesagrado").dialog({ resizable: false, autoOpen: false,
            buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
            hide: { effect: "fade", duration: 500 }
        });

        $("#datosDesagrado").dialog("option", "width", 610).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
        $("#imgDetallePrecierre").click(function () {
            $("#datosDesagrado").dialog('open');
            $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
        });
        //FIN | ZOLUXIONES | DACV | SPRINT III - RF017 | 22/06/18

        //INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se replica para mostrar DIV datosOtrasCxc | 09/08/18
        $("#divTotalReporteVL").dialog({ resizable: false, autoOpen: false,
            buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
            hide: { effect: "fade", duration: 500 }
        });
        $("#divTotalReporteVL").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
        $("#imgtbInversionesT1").click(function () {
            $("#divTotalReporteVL").dialog('open');
            $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
        });
        //FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se replica para mostrar DIV datosOtrasCxc | 09/08/18

        //INICIO | ZOLUXIONES | rcolonia | Se replica para mostrar DIV divTotalInteresesAumentoCapital | 04/10/18
        $("#divTotalInteresesAumentoCapital").dialog({ resizable: false, autoOpen: false,
            buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
            hide: { effect: "fade", duration: 500 }
        });
        $("#divTotalInteresesAumentoCapital").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
        $("#imgDetalleTitulo").click(function () {
            $("#divTotalInteresesAumentoCapital").dialog('open');
            $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
        });
        //FIN | ZOLUXIONES | rcolonia | Se replica para mostrar DIV divTotalInteresesAumentoCapital | 04/10/18

        //INICIO | ZOLUXIONES | Ipastorm | Se replica para mostrar DIV divTotalInteresesAumentoCapital | 04/10/18
        $("#divPopupOtrasCxC").dialog({ resizable: false, autoOpen: false,
            buttons: {
                Guardar: function () {
                    $("#hdDevolucionComisionUnificada").val(parseFloat($("#txtDevolucionComisionUnificada").val().toString().replace(/$|,/g, '')));
                    $("#txtOtrasCxCPreCierre").val(parseFloat($("#txtMontoDividendosPrecierre").val().toString().replace(/$|,/g, '')) + parseFloat($("#txtDevolucionComisionUnificada").val().toString().replace(/$|,/g, '')));
                    $("#hdOtrasCxCPreCierre").val(parseFloat($("#txtMontoDividendosPrecierre").val().toString().replace(/$|,/g, '')) + parseFloat($("#txtDevolucionComisionUnificada").val().toString().replace(/$|,/g, '')));
                    $("#hdCambioOtrasCxC").val("1");
                    $("#divBackground").remove();
                    $(this).dialog("close");
                },
                Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); }
            },
            hide: { effect: "fade", duration: 500 }
        });
        //Elimina el botón "X" del modal
        $("#divPopupOtrasCxC").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
        $("#imgOtrasCxCPreCierre").click(function () {
            $("#divPopupOtrasCxC").dialog('open');
            $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
        });
        //FIN | ZOLUXIONES | Ipastorm | Se replica para mostrar DIV divTotalInteresesAumentoCapital | 04/10/18

        $("div>.ui-dialog-buttonset>:button").addClass("btn btn-integra");

    });

    function ExportarArchivo() {
        $("#btnExportar").click();
    }

    function GrabarDistribucionLib() {
        $("#btnGrabarDistribucionLib").click();
        alert("Los datos se guardaron correctamente.");
    }


    $(document).keyup(function () {
        if (event.which == 27) {
            $("#divBackground").remove();
        }
    });

    function ObtenerTipoCambio() {
        var _fecha = $("#<%=tbFechaOperacion.ClientID%>").val();
        var param = { fecha: _fecha };
        var _url = "../../MetodosExternos.aspx/getTipoCambioOperacion";

        $.ajax({
            url: "../../MetodosExternos.aspx/getTipoCambioOperacion",
            data: JSON.stringify(param),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8;",
            success: function (obj) {
                // Para manejar los datos obtenidos correctamente
                debugger;
                $("#<%=txtTipoCambioOperacion.ClientID%>").val(obj.d);
                //alert($("#<%=txtTipoCambioOperacion.ClientID%>").val());
                $("#<%=btnProcesarAuxiliar.ClientID%>").click();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) { $("#<%=txtTipoCambioOperacion.ClientID%>").val("") }
        });

        return false;
    }


    $(document).ready(function () {

        // INICIO | ZOLUXIONES | CRumiche | ProyFondosII | 2018-07-24 - Integración del Proceso de Cierre Operaciones en SIF-Fondos      
        $("#<%=btnEjecutarCierreOpe.ClientID%>").click(function () {
            IniciarPopup(false);
            return false;
        });
        $("#<%=btnSoloLecturaCierreOpe.ClientID%>").click(function () {
            IniciarPopup(true);
            return false;
        });
        $("#Popup01_btnEjecutar").click(function () {
            EjecutarPreorden();
            return false;
        });
        $("#Popup01_btnCerrar").click(function () { $("#popup01").hide(); });
        $("#Popup01_btnCerrar2").click(function () { $("#popup01").hide(); });

        $("#<%=btnRefrescar.ClientID%>").click(function () {
            if ($("#<%=ddlPortafolio.ClientID%>").val() != "0") {    /*Validamos que tenga portafolio*/
                $("#divBloqueadorFondo").show();
                return true;
            }
        });
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
            var _fecha = $("#<%=tbFechaOperacion.ClientID%>").val();
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
        var _comisionSAFM = parseFloat($("#<%=tbComiSAFM.ClientID%>").val().replace(",", "").replace(",", "")) / (1 + parseFloat($("#<%=txtigv.ClientID%>").val()));

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
                        $("#Popup01_btnCerrar").click(function () { document.getElementById("<%= btnRefrescar.ClientID %>").click(); });

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
        <div class="winBloqueador-inner" id="Div2">
            <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
        </div>
    </div>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><h2>Calculo Valor Fondo Manual</h2></header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha</label>
                        <div class="col-sm-5">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                        <label class="col-sm-3 control-label">
                            Portafolio</label>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <div class="col-sm-12" style="text-align: left;">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="170px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlPortafolio"
                                InitialValue="0" runat="server" ErrorMessage="Seleccione Portafolio"></asp:RequiredFieldValidator>
                            <asp:Button ID="btnRefrescar" runat="server" Text="" />
                            <div style="display:none;">
                                <asp:Button  ID="btnProcesarAuxiliar" style="display:none;" runat="server" text="ProcesarAuxiliar"></asp:Button>
                             </div>
                            <span ID="listButtons" style="display:none;" runat="server">
                                <asp:Button ID="btnProcesar" runat="server" Text="Procesar" Visible="false" OnClientClick="return ObtenerTipoCambio()" />
                                <asp:Button ID="btnEjecutarCierreOpe" runat="server" Text="Ejecutar Cierre en Ope."
                                    Visible="false" />
                                <asp:Button ID="btnSoloLecturaCierreOpe" runat="server" Text="Ver Cierre en Ope."
                                    Visible="false" />
                                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Visible="false" />
                                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Visible="false" />
                                <asp:Button ID="btnVerDistribucion" runat="server" Text="Ver Distribución" Visible="false" />
                                <span style="display: none;">
                                    <asp:Button runat="server" ID="btnExportar" Text="Exportar" />
                                    <asp:Button runat="server" ID="btnGrabarDistribucionLib" Text="Grabar Distribucion" />
                                </span>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset id="FormInversiones" runat="server" visible="false">
            <legend>Inversiones</legend>
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
                            Diferencia Valor Cuota (%)</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtVCDiferencia" ReadOnly="true" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio Comisión SAFM</label>
                        <div class="col-sm-4">
                            <div id="dvDiv321" runat="server" class="input-append date">
                                <asp:TextBox  runat="server" ID="txtComisionSAFMAnterior" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Inversiones [t-1]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbInversionesT1" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Compras [t]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbComprasT" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Ventas y Venci. [t]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbVentasVenciT" ReadOnly="true" CssClass="Numbox-7" runat="server" />
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
                            <asp:TextBox ID="tbRentabilidadT" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Val. Forwards [t]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValoriForwardT" ReadOnly="true" CssClass="Numbox-7" runat="server" />
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
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <b>Inversiones [T] Subtotal</b>
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbInversionesSubTotal" ReadOnly="true" CssClass="Numbox-7" runat="server"
                                Width="180px" />
                            <%--INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Botón para mostrar detalle de diferencia | 09/08/18 --%>
                            <img alt="Detalle" runat="server" id="imgtbInversionesT1" src="../../App_Themes/img/icons/tree_list.png"
                                style="cursor: pointer;" visible="false" />
                            <%--FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Botón para mostrar detalle de diferencia | 09/08/18 --%>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pncom" runat="server">
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
                                <asp:TextBox ID="txtporcom" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </fieldset>
        <br />
        <fieldset id="FormPrecierre" runat="server" visible="false">
            <legend>Valor Cuota Precierre</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Caja [Precierre]</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCajaPrecierre" CssClass="Numbox-7" Style="width: 180px" runat="server"
                                ReadOnly="true" />
                            <%--INICIO | ZOLUXIONES | DACV | SPRINT III --%>
                            <img alt="Detalle" id="imgDetallePrecierre" src="../../App_Themes/img/icons/tree_list.png"
                                style="cursor: pointer;" />
                            <%--FIN | ZOLUXIONES | DACV | SPRINT III --%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Otras CxC</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtOtrasCxCPreCierre" CssClass="Numbox-7" runat="server"
                                ReadOnly="true" Width="180px" />
                            <img alt="Detalle" runat="server" id="imgOtrasCxCPreCierre" src="../../App_Themes/img/icons/tree_list.png"
                                style="cursor: pointer;" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Ajustes CXC</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtAjustesCXC" CssClass="-Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <%--<div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            CxC Monto Liberada(Precierre)</label>
                        <div class="col-sm-8">
                            <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosPrecierre"
                            <%--<asp:TextBox ID="txtMontoDividendosPrecierre" CssClass="Numbox-7" runat="server"
                                ReadOnly="true" />
                        </div>
                    </div>
                </div>--%>
                <%--<div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                           Devolución de Comisión Unificada</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtDevolucionComisionUnificada" CssClass="Numbox-7" runat="server" AutoPostBack = "true" OnTextChanged = "txtDevolucionComisionUnificada_TextChanged"/>
                        </div>
                    </div>
                </div>--%>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            CXC Venta de título</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCtitulo" ReadOnly="true" CssClass="Numbox-7" runat="server"
                                Width="180px" />
                            <img alt="Detalle" runat="server" id="imgDetalleTitulo" src="../../App_Themes/img/icons/tree_list.png"
                                style="cursor: pointer;" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Suscripción de Fondos CXC</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCotras" CssClass="Numbox-7" runat="server" Width="180px" />
                            <img alt="Detalle" runat="server" id="imgDetalleCaja" src="../../App_Themes/img/icons/tree_list.png"
                                style="cursor: pointer;" />
                        </div>
                    </div> 
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            CXC(Precierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtCXCPrecierre" CssClass="Numbox-7" runat="server" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cheque Pendiente</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtChequePendiente" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Rescate Pendiente</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtRescatePendiente" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Ajustes CXP</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtAjustesCXP" CssClass="-Numbox-7" runat="server" Text="0" />
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
                            <asp:TextBox ID="tbCXPtitulo" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Otras CXP</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXPotras" CssClass="Numbox-7" runat="server" ReadOnly="true" Width="180px" />
                            <img alt="Detalle" id="imgDetalle" src="../../App_Themes/img/icons/tree_list.png"
                                style="cursor: pointer;" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            CXP(Precierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtCXPPrecierre" CssClass="Numbox-7" runat="server" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Otros Gastos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosGastos" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Otros Ingresos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosIngresos" CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            V. Pat. Precierre 1</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatriPrecierre" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="FilaPrecierre" runat="server">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Comisión SAFM</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbComiSAFM" ReadOnly="true" CssClass="Numbox-7"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            V. Pat. Precierre 2</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatriPrecierre2" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <b>V. Cuota Precierre(Cuotas) </b>
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValCuotaPrecierre" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" runat="server" id="cuotapre">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <b>V. Cuota Precierre(Valores) </b>
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValValoresPrecierre" Style="font-weight: bold; background-color: #e3e3e3;"
                                ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset id="FormCierre" runat="server" visible="false">
            <legend>Valor Cuota Cierre</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Caja</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCajaCierre" CssClass="Numbox-7" runat="server" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Aportes Cuotas</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbAporteCuota" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label" id="lblAportes" runat="server">
                            Aportes Valores</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbAporteValores" CssClass="Numbox-7" runat="server" ReadOnly="false" />
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
                            <asp:TextBox ID="tbRescatesCuota" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label" id="lblRescates" runat="server">
                            Rescates Valores</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbRescatesValores" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Comision Unificada Cuotas</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtComisionUnificadaCuota" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Comision Unificada Mandato</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtComisionUnificadaMandato" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            CXC Venta de título</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCtituloCierre" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Otras CxC</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCotrasCierre" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            CXC(Cierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXCCierre" CssClass="Numbox-7" runat="server" ReadOnly="true" />
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
                            <asp:TextBox ID="tbCXPtituloCierre" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Otras CXP</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXPotrasCierre" CssClass="Numbox-7" runat="server" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            CXP(Cierre)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCXPCierre" CssClass="Numbox-7" runat="server" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Otros Gastos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosGastosCierre" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Otros Ingresos</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbOtrosIngresosCierre" CssClass="Numbox-7"  runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            V. Pat. Cierre (Cuota)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatCierreCuota" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            V. Pat. Cierre (Valor)</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValPatCierreValores" ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <b>V. Cuota Cierre (Cuota) </b>
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValCuotaCierreCuota" Style="font-weight: bold; background-color: #e3e3e3;"
                                ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <b>V. Cuota Cierre (Valor) </b>
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbValCuotaCierreValores" Style="font-weight: bold; background-color: #e3e3e3;"
                                ReadOnly="true" CssClass="Numbox-7" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset id="FormSeries" runat="server" visible="false">
            <legend>Series</legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Cargar Porcentaje Series</label>
                        <div class="col-sm-9">
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".txt" class="filestyle"
                                data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" style="width: 300px;">
                            <asp:HiddenField ID="hfRutaDestino" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:CheckBox ID="cbHabilitarCarga" runat="server" Text="Habilitar carga automática"
                        Checked="true" AutoPostBack="true" />
                    <asp:Button ID="btnCarga" runat="server" Text="Cargar Porcentaje Series" />
                </div>
            </div>
            <br />
            <div class="grilla" style="height: 200px; width: 100%; overflow: auto;">
                <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgArchivo">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                    CommandArgument='<%# String.Format("{0}", DataBinder.Eval(Container.DataItem, "CodigoSerie")) %>'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CodigoSerie" HeaderText="Codigo Serie" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre Serie" />
                        <asp:BoundField DataField="CuotasPrecierre" HeaderText="Cuotas Precierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="ValoresPrecieree" HeaderText="Valores Precierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="CuotasCierre" HeaderText="Cuotas Cierre" DataFormatString="{0:0.0000000}" />
                        <asp:BoundField DataField="ValoresCieree" HeaderText="Valores Cierre" DataFormatString="{0:0.0000000}" />
                        <asp:TemplateField HeaderText="Porcentaje Serie" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPorcentaje" onkeypress="return soloNumeros(event)" Text='<%# DataBinder.Eval(Container.DataItem, "Porcentaje")%>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
        <div id="divFondo" style="width: 100%; height: 100%; position: absolute; top: 0;
            left: 0; display: none; background-color: #7D7D7D; opacity: 0.7">
        </div>
        <div id="datosOtrasCXP" title="Detalle del calculo de Otras CXP">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size: 12px;">
                                <label class="col-sm-6 control-label">
                                    <b>Comision Neta SAFM Acumulada</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtComisionSAFM" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>                    
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    <b>Devolución Neta de Comisión Unificada:</b></label>
                                <div class="col-sm-6">
                                    <%--<asp:TextBox ID="txtDevolucionComisionUnificada" CssClass="Numbox-7" runat="server" AutoPostBack = "true" OnTextChanged = "txtDevolucionComisionUnificada_TextChanged"/>--%>
                                    <asp:TextBox ID="txtDevolucionComisionUnificada" CssClass="Numbox-7" runat="server" Width="150px" style="background-color: #d5f4e6" ReadOnly="true" />(*)
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    <b>Caja de Recaudo:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtCajaRecaudo" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    <b id="lblSuscripcion" runat="server">Suscripción:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtSuscripcion" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    <b>Cheque Pendiente:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtChequeP" runat="server" ReadOnly="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    <b>Rescate Pendiente:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtRescateP" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    <b>Distribución Programada:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtDistribucionProgramada" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                    (*)No suma al total de CxP
                    </div>
                </div>
            </div>
        </div>
        <%--INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se crea DIV para mostrar listado de importes para campo Otras CxC | 22/06/18 --%>
        <div id="datosOtrasCxC" title="Detalle de cálculo de Suscripción de Fondos CxC">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="grilla" style="height: 200px; width: 100%; overflow: auto;">
                        <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgOtrasCxC">
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Concepto" />
                                <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.0000000}"
                                    ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <%--FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se crea DIV para mostrar listado de importes para campo Otras CxC | 22/06/18 --%>
        <%--INICIO | ZOLUXIONES | DACV | SPRINT III --%>
        <div id="datosDesagrado" title="Detalle de Caja Precierre">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="grilla" style="height: 300px; width: 100%; overflow: auto;">
                        <asp:GridView ID="gvDesagradoCaja" runat="server" SkinID="Grid_Paging_No" AutoGenerateColumns="False"
                            ShowHeader="False" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="Banco" HeaderText="Banco" />
                                <asp:BoundField DataField="SaldoInicial" HeaderText="SaldoInicial" DataFormatString="{0:#,##0.00}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SaldoLibro" HeaderText="SaldoLibro" DataFormatString="{0:#,##0.00}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <%--FIN | ZOLUXIONES | DACV | SPRINT III  --%>
        <%--INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se crea DIV para mostrar diferencia de reporteVL con Inversiones [t-1] | 09/08/18 --%>
        <div id="divTotalReporteVL" title="Diferencia Valorización en Composición Cartera">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size: 12px;">
                                <label class="col-sm-6 control-label">
                                    <b>Total Valorización Composición Cartera:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtTotalComposicionCartera" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    <b>Diferencia:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtDiferenciaComposicionCartera" runat="server" ReadOnly="true"
                                        CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se crea DIV para mostrar diferencia de reporteVL con Inversiones [t-1] | 09/08/18 --%>
        <%--INICIO | ZOLUXIONES | rcolonia | Se crea DIV para mostrar detalle intereses de aumento de capital | 04/10/18 --%>
        <div id="divTotalInteresesAumentoCapital" title="Aumento de Capital">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size: 12px;">
                                <br />
                                <label class="col-sm-6 control-label">
                                    <b>Intereses Aumento de Capital:</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtInteresesAumentoCapital" runat="server" ReadOnly="true" CssClass="Numbox-7"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--FIN | ZOLUXIONES | Ipastorm | Se crea DIV para mostrar detalle intereses de aumento de capital | 04/10/18 --%>
        <div id="divPopupOtrasCxC" title="Detalle de Otras CXC">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size: 12px;">
                                <label class="col-sm-6 control-label">
                                    <b>CxC Monto Liberada(Precierre):</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtMontoDividendosPrecierre" CssClass="Numbox-7" runat="server" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input id="txtComisionPortafolio" type="text" runat="server" class="hidden" />
    <input id="HdFechaCreacionFondo" type="text" runat="server" class="hidden" />
    <input id="hdSeriado" type="text" runat="server" class="hidden" />
    <input id="hdCajaPrecierre" type="text" runat="server" class="hidden" />
    <input id="hdCuotasLiberadas" type="text" runat="server" class="hidden" />
    <input id="hdCodigoPortafolioSisOpe" type="text" runat="server" class="hidden" />
    <input id="hdCheckOperaciones" type="text" runat="server" class="hidden" />
    <%--OT10927 - 22/11/2017 - Hanz Cocchi. Variable que permite guardar la cantidad de registros de operaciones liberadas--%>
    <input id="hdCantRegDistribLib" type="text" runat="server" class="hidden" value="0" />
    <%--OT10927 - Fin--%>
    <%--OT11237 - 15/02/2018 - Ian Pastor M. Variable que guarda el codigo de portafolio padre del Sistema de Operaciones--%>
    <input id="hdCPPadreSisOpe" type="hidden" runat="server" />
    <%--OT11237 - Fin--%>
    <input id="hdCambioOtrasCxC" type="hidden" runat="server" />
    <input id="hdOtrasCxCPreCierre" type="hidden" runat="server" />
    <input id="hdDevolucionComisionUnificada" type="hidden" runat="server" />
    <div style="display:none;">
        <asp:TextBox ID="txtTipoCambioOperacion" runat="server" ></asp:TextBox>
        <asp:TextBox ID="txtDevolucionComisionDiaria" runat="server" ></asp:TextBox>
    </div >
     
    <br />
    </form>
    <span id="spanMarca"></span>
</body>
</html>
