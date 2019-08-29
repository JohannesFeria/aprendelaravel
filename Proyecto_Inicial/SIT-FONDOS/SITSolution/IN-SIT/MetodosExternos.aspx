<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MetodosExternos.aspx.cs" Inherits="MetodosExternos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">

    <title></title>

  <style type="text/css"> 
        .win01 {
            border: solid 1px gray;
            background-color: #80808052;
            position: absolute;
            z-index: 10;
            width: 100%;
            height: 100%;
            text-align: center;     
        }        
        .cont01 {
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
        
        .tab01 {
            width: 100%;
        }
        .tab01>tbody>tr>td {
            padding: 2px 5px;
            border: Solid 1px gray;
        }        
        .span01 {
            font-size: 17px;
        }

        .tooltip>div{
            /*background-color: gray;*/
        }

    </style>

    <script type="text/javascript">

        $(document).ready(function () {
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
        });       

        /*CRumiche: idFondoOpe=ID Fondo en BD Operaciones*/
        function IniciarPopup(soloLectura) {
            $("#popup01").show();
            $("#popup01_loading").show();
            $("#pop1_frm").hide();
            $("#pop1_Errores").hide();
            
            var _idFondoOpe = 2;
            var _codUsuario = "P500625"; // Probando con el usuario LISETH

            var param = { idFondoOpe: _idFondoOpe, codUsuario: _codUsuario };
            var _url = "MetodosExternos.aspx/Get_VerificarPreviewPrecierre";

            if(soloLectura){
                // Mostramos el Histórico
                var _fecha = "08/08/2018";
                param = { idFondoOpe: _idFondoOpe, fecha: _fecha };
                _url = "MetodosExternos.aspx/Get_InfoPrecierreHistorico";
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
                        
                        if(!resul.ProcesoErrado){
                            $("#pop1_frm").show();
                            MapearCampos(resul);        

                            if(soloLectura){
                                $("#Popup01_btnEjecutar").hide();
                                $("#Popup01_btnCerrar").val("Cerrar");
                                $("#pop1_divEnviarCorreos").hide();
                            }else{
                                $("#Popup01_btnEjecutar").show();
                                $("#Popup01_btnCerrar").val("Cancelar");    
                                $("#pop1_divEnviarCorreos").show();                        
                            }

                        }else{
                            $("#pop1_Errores").show();
                            $("#pop1_areaErrores").val(resul.Notificaciones);
                        } 
                    }
                    else
                    {
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

            var _idFondoOpe = 2;
            var _enviarCorreos = $("#pop1_chkEnviarCorreos").is(":checked") ? 1 : 0;
            var _codUsuario = "P500625"; // Probando con el usuario LISETH

            var param = { idFondoOpe: _idFondoOpe, enviarCorreos: _enviarCorreos, codUsuario: _codUsuario };
            $.ajax({
                url: "MetodosExternos.aspx/Get_EjecutarPrecierre",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                success: function (response) {
                    // Para manejar los datos obtenidos correctamente
                    if (typeof response.d == "object") {
                        $("#popup01_loading").hide();

                        var resul = response.d;
                        
                        if(!resul.ProcesoErrado){
                            $("#pop1_frm").show();
                            MapearCampos(resul);

                            $("#pop1_divEnviarCorreos").hide();
                            $("#Popup01_btnEjecutar").hide();
                            $("#Popup01_btnCerrar").val("Cerrar");
                            $("#Popup01_btnCerrar").click(function () { location.reload(); });

                            alertify.alert('El CIERRE se realizó correctamente', function () {}); 
                        }else{
                            $("#pop1_Errores").show();
                            $("#pop1_areaErrores").val(resul.Notificaciones);
                        } 
                    }
                    else
                    {
                        $("#popup01").hide();
                        alert("No se pudieron cargar los datos correctamente");
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { $("#popup01").hide(); alert(textStatus); }
            });
        }

        function MapearCampos(resul){
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

            if($("#pop1_txtConversionCuotas").val() === "-") $("#pop1_grupoConversionCuotas").hide();
            if($("#pop1_txtPagoFlujoMonto").val() === "-") $("#pop1_grupoPagoFlujoMonto").hide();
            if($("#pop1_txtRetencionFlujo").val() === "-") $("#pop1_grupoRetencionFlujo").hide();                        

            $("#pop1_areaNotificaciones").val(resul.Notificaciones);

            if(resul.Notificaciones.length > 0)
                $("#pop1_divNotificaciones").show();
            else
                $("#pop1_divNotificaciones").hide();

            if(resul.TipoProceso === "V")
                $("#pop1_lblNotificaciones").text("Los siguientes rescates serían rechazados");
            if(resul.TipoProceso === "E")
                $("#pop1_lblNotificaciones").text("Proceso Finalizado, pero se tienen las siguientes alertas:");
        
            // Seccion para mostrar las diferencias
            habilitarTooltipDiferencias(101
                                    , $("#pop1_txtValorCuota").val()
                                    , "pop1_txtValorCuota"
                                    , "Valor Cuota");
            habilitarTooltipDiferencias(102
                                    , $("#pop1_txtSaldoAyer").val()
                                    , "pop1_txtSaldoAyer"
                                    , "Saldo al Día de Ayer");
            habilitarTooltipDiferencias(103
                                    , $("#pop1_txtSaldoDia").val()
                                    , "pop1_txtSaldoDia"
                                    , "Saldo del Día");
            habilitarTooltipDiferencias(104
                                    , $("#pop1_txtSaldoCuotasAyer").val()
                                    , "pop1_txtSaldoCuotasAyer"
                                    , "Saldo Cuotas al Día de Ayer");
            habilitarTooltipDiferencias(105
                                    , $("#pop1_txtSaldoCuotasDia").val()
                                    , "pop1_txtSaldoCuotasDia"
                                    , "Saldo Cuotas del Día");
            habilitarTooltipDiferencias(106
                                    , $("#pop1_txtTotalEgresoCuotas").val()
                                    , "pop1_txtTotalEgresoCuotas"
                                    , "Total Egreso Cuotas");            
        }

        function habilitarTooltipDiferencias(valA, valB, controlConTooltip, tituloControl) {
            var auxValA = ifNoNumber(parseFloat(valA.replace(/,/g, "")), 0);
            var auxValB = ifNoNumber(parseFloat(valB.replace(/,/g, "")), 0);

            var diff = auxValA - auxValB;
            if (diff != 0) {
                $("#" + controlConTooltip).attr("title", "Diferencias en '" + tituloControl + "' (SIT - OPE) => "
                            + auxValA.toString() + " - " + auxValB.toString() + " => " + formatNumber(diff));

                $("#" + controlConTooltip).css("background-color", "rgb(241, 192, 138)");
                //$( "#" + controlConTooltip ).tooltip();
            } else
                $("#" + controlConTooltip).css("border", "Solid 2px #4bb54b");
        }

        function ifNoNumber(val, defaultVal){
            if (!$.isNumeric(val)) return defaultVal;
            return val;
        }

    </script>




</head>
<body style="background-image: url('fondo1.png')" >

    <div id="popup01" class="win01" style="display:none">
        <div class="winBloqueador-inner" id="popup01_loading">
            <img src="App_Themes/img/icons/loading.gif" alt="Cargando..."  style="height: 50px;"/>
        </div>

        <div class="cont01" id="pop1_frm" style="display:none">
            <fieldset>
                <legend>CIERRE - OPERACIONES</legend>

                <div class="row">
                    <div class="col-sm-9">
                        <div class="form-group">
                            <label class="col-sm-4 control-label" style="text-align: right;" >Fondo</label>
                            <div class="col-sm-8">
                                <input id="pop1_txtFondo" type="text" readonly="readonly" style="width:100%; "  />
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
                            <label class="col-sm-6 control-label" style="text-align: right;">Fecha</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtFecha" type="text" readonly="readonly" style="width:150px; "/>
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
                            <label class="col-sm-6 control-label" style="text-align: right;">Valor Cuota</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtValorCuota" type="text" readonly="readonly" class="Numbox-7_12 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7"/>
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
                            <label class="col-sm-6 control-label" style="text-align: right;">Saldo día de ayer</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtSaldoAyer" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="100000000000000000" step="0.01"/>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Saldo del día</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtSaldoDia" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7"/>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Saldo cuotas al día de ayer</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtSaldoCuotasAyer" type="text" readonly="readonly" class="Numbox-7_12 NumBox" style="width:150px; text-align: right;" min="0" max="100000000000000000" step="0.01"/>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Saldo cuotas del día</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtSaldoCuotasDia" type="text" readonly="readonly" class="Numbox-7_12 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7"/>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Total Ingreso Bruto</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtTotalIngresoBruto" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="100000000000000000" step="0.01"/>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Total Egreso Bruto</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtTotalEgresoBruto" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7"/>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Total Ingreso Neto</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtTotalIngresoNeto" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="100000000000000000" step="0.01"/>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Total Egreso Neto</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtTotalEgresoNeto" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7"/>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Total Ingreso Cuotas</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtTotalIngresoCuotas" type="text" readonly="readonly" class="Numbox-7_12 NumBox" style="width:150px; text-align: right;" min="0" max="100000000000000000" step="0.01"/>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Total Egreso Cuotas</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtTotalEgresoCuotas" type="text" readonly="readonly" class="Numbox-7_12 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7"/>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group" id="pop1_grupoConversionCuotas">
                            <label class="col-sm-6 control-label" style="text-align: right;">Conversión de Cuotas</label>
                            <div class="col-sm-5">
                                <input id="pop1_txtConversionCuotas" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="100000000000000000" step="0.01"/>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label" style="text-align: right;">Egreso Cuotas Retención Traspasos</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtEgresoCuotasRT" type="text" readonly="readonly" class="Numbox-7_12 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7">
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
                            <label class="col-sm-6 control-label" style="text-align: right;">Egreso Monto Retención Traspasos</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtEgresoMontoRT" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7">
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
                        <div class="form-group" >
                            <label class="col-sm-6 control-label" style="text-align: right;">Pago de Flujo Monto</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtPagoFlujoMonto" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7">
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
                            <label class="col-sm-6 control-label" style="text-align: right;">Retención por Flujo</label>
                            <div class="col-sm-6">
                                <input id="pop1_txtRetencionFlujo" type="text" readonly="readonly" class="Numbox-2 NumBox" style="width:150px; text-align: right;" min="0" max="1000000000000" step="1e-7" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" id="pop1_divNotificaciones" style="display:none">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <br/>
                            <label class="col-sm-12 control-label" id="pop1_lblNotificaciones"></label>
                            <div class="col-sm-12">                                
                                <textarea id="pop1_areaNotificaciones" style="width:100%; height: 70px;" readonly="readonly"  ></textarea>
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
                            <label class="col-sm-8 control-label" style="text-align: right;">Enviar Correos al Ejecutar</label>
                            <div class="col-sm-4" style="text-align: left; margin-top: 5px">
                                <input id="pop1_chkEnviarCorreos" type="checkbox" >
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>

            <br />
            <input type="submit" value="Cancelar" id="Popup01_btnCerrar" class="btn btn-integra" style="min-width: 80px; text-align: center; width:auto;" />
            <input type="submit" value="Ejecutar Precierre" id="Popup01_btnEjecutar" class="btn btn-integra" style="min-width: 80px; text-align: center; width:auto; margin-left: 10px;" />
        </div>

        <div class="cont01" id="pop1_Errores" style="display:none">
            <fieldset>
                <legend>CIERRE - OPERACIONES</legend>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">                            
                            <label class="col-sm-12 control-label" >Se han encontrado problemas durante el procesamiento</label>
                            <div class="col-sm-12">                                
                                <textarea id="pop1_areaErrores" style="width:100%; height: 70px;" readonly="readonly"  ></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>

            <br />
            <input type="submit" value="Cancelar" id="Popup01_btnCerrar2" class="btn btn-integra" style="min-width: 80px; text-align: center; width:auto;" />
        </div>
    </div>

    <form id="form1" runat="server" >
    <div>
    
        <asp:Button ID="btnEjecutarCierreOpe" runat="server" Text="Ejecutar" />
        <asp:Button ID="btnSoloLecturaCierreOpe" runat="server" Text="Solo Lectura" />
    

    </div>
    </form>
</body>
</html>
