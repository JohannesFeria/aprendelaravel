<%@ Page Language="VB" AutoEventWireup="false" EnableEventValidation="false" CodeFile="frmIngresoMasivoOperacionRF.aspx.vb" Inherits="Modulos_Inversiones_frmIngresoMasivoOperacionRF" %>
<!DOCTYPE html>
<html>
<%: Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Renta Fija</title>
    <script type="text/javascript">

        function ObtenerPrefijoControlEnGrilla(controlEnGrilla) {            
            var idControl = $(controlEnGrilla).attr('id').toString();
            var nombreEnArray = idControl.split('_');
            var prefijoControl = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';            

            return prefijoControl.toString();
        }

        $(document).ready(function () {

            //INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 18/05/2018
            var control = '';
            var controlfecha = '';
            var controlfechaLiq = '';

            // ==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Actualizar Combo Portafolios cuando cambia un Nemonico
            $("[id$='tbNemonicoF']").change(function () {
                var nombreTextboxNemonico = $(this).attr('id').toString();
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
                actualizarComboPortafolioCuandoCambioNemonico(nombreTextboxNemonico, prefijoControlEnGrilla + "ddlPortafolioF");
            });

            $("[id$='tbNemonico']").change(function () {
                var nombreTextboxNemonico = $(this).attr('id').toString();
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
                actualizarComboPortafolioCuandoCambioNemonico(nombreTextboxNemonico, prefijoControlEnGrilla + "ddlfondos");
            });
            // ==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Actualizar Combo Portafolios cuando cambia un Nemonico

            // ==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-14 | Comportamiento del campo Tipo Valorizacion         
            $("[id$='ddlTipoValorizacion']").change(function () {
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
                $("#" + prefijoControlEnGrilla + "hdTipoValorizacion").val($(this).val());
            });

            $("[id$='ddlTipoValorizacionF']").change(function () {
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
                $("#" + prefijoControlEnGrilla + "HdTipoValorizacionF").val($(this).val());
            });

            $("[id$='ddlPortafolioF']").change(function () {
                // ==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-14 | Comportamiento del campo Tipo Valorizacion
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
                $("#" + prefijoControlEnGrilla + "HdPortafolioF").val($(this).val()); // Actualizamos el Hidden Fondos

                CargarTipoValorizacion("TipoValorizacion"
                                        , $(this).val()
                                        , prefijoControlEnGrilla + "ddlTipoValorizacionF"
                                        , prefijoControlEnGrilla + "HdTipoValorizacionF");

                CargarFechaNegocio($(this).val()
                                        , prefijoControlEnGrilla + "tFechaOperacionF"
                                        , prefijoControlEnGrilla + "hdFechaOperacionF"
                                        , prefijoControlEnGrilla + "tbFechaLiquidacionF");
            });

            $("[id$='ddlfondos']").change(function () {
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
                $("#" + prefijoControlEnGrilla + "Hdfondos").val($(this).val()); // Actualizamos el Hidden Fondos

                CargarTipoValorizacion("TipoValorizacion"
                                        , $(this).val()
                                        , prefijoControlEnGrilla + "ddlTipoValorizacion"
                                        , prefijoControlEnGrilla + "hdTipoValorizacion");

                CargarFechaNegocio($(this).val()
                                        , prefijoControlEnGrilla + "tFechaOperacion"
                                        , prefijoControlEnGrilla + "hdFechaOperacion"
                                        , prefijoControlEnGrilla + "tbFechaLiquidacion");
            });

            // Inicializamos los bloqueos del "TipoValorizacion"
            $("[id$='ddlfondos']").each(function () {
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);                

                ConfigTipoValorizacionPorPortafolio($(this).val()
                                        , prefijoControlEnGrilla + "ddlTipoValorizacion"
                                        , prefijoControlEnGrilla + "hdTipoValorizacion"
                                        , false);
            });
            // $("[id$='ddlfondos']").trigger("change"); // Lanzamos el evento change para inicializar 

            // ==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche | 2018-09-14 | Comportamiento del campo Tipo Valorizacion


            $("[id$='ddlPortafolioF']").click(function () {
                var separador = $(this).attr('Id').split('_');
                var nemonico = '#Datagrid1_' + separador[1] + '_tbNemonicoF';
                if ($(nemonico).val().length <= 0) {
                    alertify.alert('<b>Ingrese el Nemónico</b>');
                }
            });

            $("[id$='ddlfondos']").click(function () {
                var separador = $(this).attr('Id').split('_');
                var nemonico = '#Datagrid1_' + separador[1] + '_tbNemonico';
                if ($(nemonico).val().length <= 0) {
                    alertify.alert('<b>Ingrese el Nemónico</b>');
                }
            });

            function CargarFechaNegocio(Idportafolio, txtFechaOpe, hdFechaOpe, txtFechaLiq) {
                var param = { Idportafolio: Idportafolio };
                $.ajax({
                    url: "../../MetodosWeb.aspx/GetPortafolioSelectById",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8;",
                    success: function (data) {
                        var fechaOperacion = data.d;
                        $("#" + txtFechaOpe).val(fechaOperacion);
                        $("#" + hdFechaOpe).val(fechaOperacion);
                        CargarFechaLiquidacion(fechaOperacion, txtFechaLiq);
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) { /*alert(textStatus);*/ }
                });
            }

            function CargarFechaLiquidacion(fechaOperacion, txtFechaLiq) {
                var param = { fechaOperacion: fechaOperacion };
                $.ajax({
                    url: "../../MetodosWeb.aspx/GetFechaLiquidacionbyFechaOp",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8;",
                    success: function (data) {
                        var fechaLiquidacion = data.d;
                        $("#" + txtFechaLiq).val(fechaLiquidacion);
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) { /*alert(textStatus);*/ }
                });
            }

            // ==== INICIO | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche e IPastor | 2018-09-14 | Comportamiento del campo Tipo Valorizacion
            function CargarTipoValorizacion(clasificacion, portafolio, ddlTipoValorizacion, hdTipoValorizacion) {
                var param = { clasificacion: clasificacion };
                $.ajax({
                    url: "../../MetodosWeb.aspx/CargarTipoValorizacion",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8;",
                    success: function (data) {
                        $("#" + ddlTipoValorizacion).html('');
                        $("#" + ddlTipoValorizacion).append('<option value="">--SELECCIONE--</option>');
                        for (var i = 0; i < data.d.length; i++) {
                            $("#" + ddlTipoValorizacion).append('<option value="' + data.d[i].id + '">' + data.d[i].descripcion + '</option>');
                        }

                        ConfigTipoValorizacionPorPortafolio(portafolio, ddlTipoValorizacion, hdTipoValorizacion, true);
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) { /*alert(textStatus);*/ }
                });
            }

            function ConfigTipoValorizacionPorPortafolio(idPortafolio, ddlTipoValorizacion, hdTipoValorizacion, setDefault) {
                var param = { idPortafolio: idPortafolio };
                $.ajax({
                    url: "../../MetodosWeb.aspx/CargarTipoValorizacionPortafolio",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8;",
                    success: function (data) {
                        var valDefault = data.d;

                        if (valDefault == "VAL_RAZO")
                            $("#" + ddlTipoValorizacion).prop('disabled', 'disabled');
                        else 
                            $("#" + ddlTipoValorizacion).removeAttr("disabled");
                        
                        if (setDefault) {
                            $("#" + ddlTipoValorizacion).val(valDefault);
                            $("#" + hdTipoValorizacion).val(valDefault);                       
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) { /*alert(textStatus);*/ }
                });
            }

            // ==== FIN | PROYECTO FONDOS-MANDATOS | ZOLUXIONES | CRumiche e IPastor | 2018-09-14 | Comportamiento del campo Tipo Valorizacion

            //Funcionalidad Activar desactivar Tipo modo negociación
            var Fila = $("#Datagrid1 tr").length;
            var IndiceCaja = 0
            var IndiceBlucle = 0
            for (i = 2; i < Fila; i++) {
                if (i.toString().length == 1) { IndiceBlucle = "0" + i.toString(); } else { IndiceBlucle = i; }
                //Como no se puede indentificar el indice en el evento autocomplete, se usa un control comodin cuando se digita algo nuevo
                $("#Datagrid1_ctl" + IndiceBlucle + "_ddlIndice").change(function () {
                    IndiceCaja = $(this).parents("tr").index() + 1
                    if (IndiceCaja.toString().length == 1) { IndiceCaja = "0" + IndiceCaja.toString(); }
                    if ($(this).val() == "P") {
                        $("#Datagrid1_ctl" + IndiceCaja + "_tbTasa").attr('readonly', 'readonly');
                        $("#Datagrid1_ctl" + IndiceCaja + "_tbPrecio").removeAttr('readonly');
                    } else {
                        $("#Datagrid1_ctl" + IndiceCaja + "_tbPrecio").attr('readonly', 'readonly');
                        $("#Datagrid1_ctl" + IndiceCaja + "_tbTasa").removeAttr('readonly');
                    }
                });
            }
            //OT10795 - Se habilita los campos Precio o Tasa según la selección del combo ddlIndiceF (YTM o Precio)
            if ($("#hdGrillaRegistros").val() == "0") {
                Fila = "03"
                if ($("#Datagrid1_ctl" + Fila + "_ddlIndiceF").val() == "T") {
                    $("#Datagrid1_ctl" + Fila + "_tbPrecioF").attr('readonly', 'readonly');
                    $("#Datagrid1_ctl" + Fila + "_tbTasaF").removeAttr('readonly');
                } else {
                    $("#Datagrid1_ctl" + Fila + "_tbPrecioF").removeAttr('readonly');
                    $("#Datagrid1_ctl" + Fila + "_tbTasaF").attr('readonly', 'readonly');
                }
            }
            else {
                if (Fila.toString().length == 1) {
                    Fila = "0" + Fila.toString();
                    if ($("#Datagrid1_ctl" + Fila + "_ddlIndiceF").val() == "T") {
                        $("#Datagrid1_ctl" + Fila + "_tbPrecioF").attr('readonly', 'readonly');
                        $("#Datagrid1_ctl" + Fila + "_tbTasaF").removeAttr('readonly');
                    } else {
                        $("#Datagrid1_ctl" + Fila + "_tbPrecioF").removeAttr('readonly');
                        $("#Datagrid1_ctl" + Fila + "_tbTasaF").attr('readonly', 'readonly');
                    }
                }
            }
            //OT10795 - Fin
            $("#Datagrid1_ctl" + Fila + "_ddlIndiceF").change(function () {
                if ($(this).val() == "T") {
                    $("#Datagrid1_ctl" + Fila + "_tbPrecioF").attr('readonly', 'readonly');
                    $("#Datagrid1_ctl" + Fila + "_tbTasaF").removeAttr('readonly');
                } else {
                    $("#Datagrid1_ctl" + Fila + "_tbPrecioF").removeAttr('readonly');
                    $("#Datagrid1_ctl" + Fila + "_tbTasaF").attr('readonly', 'readonly');
                }
            });
        })
        function cambio(myElemId) {
            var elem = myElemId.id.split('_');
            var elem2 = elem;
            var elem3 = elem;
            var elem4 = elem;
            elem = elem[0] + '_' + elem[1] + '_' + 'hdCambio';
            document.getElementById(elem).value = "1";
            elem2 = elem2[0] + '_' + elem2[1] + '_' + 'hdCambioTraza';
            elem4 = elem4[0] + '_' + elem4[1] + '_' + 'hdCambioTrazaFondo';

            if ((elem3[2] == "tbNemonico") || (elem3[2] == "ddlOperacion") || (elem3[2] == "tbCantidad") || (elem3[2] == "tbPrecio") || (elem3[2] == "tbIntermediario") ||
                                (elem3[2] == "tbCantidadOperacion") || (elem3[2] == "tbPrecioOperacion") || (elem3[2] == "tbFondo1") || (elem3[2] == "tbFondo3") || (elem3[2] == "tbFondo3"))
                document.getElementById(elem2).value = "1";

            if ((elem3[2] == "tbFondo1") || (elem3[2] == "tbFondo3") || (elem3[2] == "tbFondo3"))
                document.getElementById(elem4).value = "1";
        }
        function HabilitaCondicion(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibCondicion';
            document.getElementById(elem).click();
            cambio(myElem)
        }
        function HabilitaCondicionF(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibCondicionF';
            document.getElementById(elem).click();
        }
        function HabilitaTipoTramo(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibTipoFondo';
            cambio(myElem)
            document.getElementById(elem).click();
        }
        function HabilitaTipoTramoF(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibTipoFondoF';
            document.getElementById(elem).click();
        }
        function cantidadF_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbCantidadOperacionF';
            var cantidad = document.getElementById(myElem.id).value;
            if (cantidad != "") {
                document.getElementById(resultado).value = cantidad;
            } else {
                document.getElementById(resultado).value = 0;
            }
        }
        function precio_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbCantidad';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotal';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            } else {
                document.getElementById(resultado).value = 0;
            }
        }
        function precioF_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbPrecioOperacionF';
            var precio = document.getElementById(myElem.id).value;
            if (precio != "") {
                document.getElementById(resultado).value = precio;
            } else {
                document.getElementById(resultado).value = 0;
            }
        }
        function cantidad_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbPrecioOperacion';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacion';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            } else {
                document.getElementById(resultado).value = 0;
            }
        }
        function cantidadF_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbPrecioOperacionF';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacionF';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            } else {
                document.getElementById(resultado).value = 0;
            }
        }
        function precio_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbCantidadOperacion';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacion';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            } else {
                document.getElementById(resultado).value = 0;
            }
        }
        function precioF_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbCantidadOperacionF';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacionF';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            } else {
                document.getElementById(resultado).value = 0;
            }
        }
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                                document.forms[0].elements[i].checked = true;
                            }
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                            document.forms[0].elements[i].checked = false;
                        }
                    }
                }
            }
        }
        $('form').live("submit", function () {
            //$('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            ShowProgress();
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Renta Fija</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tipo de Instrumento</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="320px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo Mnem&oacute;nico:</label>
                        <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" Width ="200px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Operador</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlOperador" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Clase de Instrumento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseInstrumento" Width="280px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Estado</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlEstado" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3" >
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" Height="26px" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">
             <%--<asp:UpdatePanel ID="upBotones" runat="server" UpdateMode ="Conditional" >
                <ContentTemplate>--%>
                    <asp:Button Text="Grabar" runat="server" ID="btnGrabar" UseSubmitBehavior="false" />
                    <asp:Button Text="Validar" runat="server" ID="btnValidar" Visible ="false" />
                    <asp:Button Text="Validar Exc. Trader" runat="server" ID="btnValidarTrader" UseSubmitBehavior="false" />
                    <asp:Button Text="Ejecutar" runat="server" ID="btnAprobar" UseSubmitBehavior="false" />
                    <asp:Button Text="Exportar" runat="server" ID="btnExportar" />
                    <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
<%--                </ContentTemplate>
                <Triggers >
                    <asp:AsyncPostBackTrigger ControlID="btnGrabar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnValidar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAprobar" EventName="Click" />
                    <asp:PostBackTrigger ControlID="btnExportar"  />
                    <asp:PostBackTrigger ControlID="btnImprimir"  />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
        <br />
        <div class="grilla-footer">
<%--            <asp:UpdatePanel ID="updGrilla" runat="server">
                <ContentTemplate>--%>
                    <asp:GridView runat="server" SkinID="GridFooter" ID="Datagrid1">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>&nbsp;
                                    <asp:ImageButton ID="Imagebutton1" runat="server" SkinID="imgDelete" CommandName="_Delete"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden") & "|" & CType(Container, GridViewRow).RowIndex %>'>
                                    </asp:ImageButton>
                                    <asp:Label ID="lbCodigoPrevOrden" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden") %>' Visible="False" />
                                    <asp:Label ID="lbClase" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Clase") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ImageButton2" runat="server" SkinID="imgAdd" CommandName="Add"
                                    CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Correlativo" HeaderText="N&#176;" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                            <asp:TemplateField HeaderText="Hora">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbHora" runat="server" Width="40px" SkinID="Hour" Text='<%# DataBinder.Eval(Container,"DataItem.HoraOperacion") %>'/>
                                    <input id="hdCambio" type="hidden" name="hdCambio" runat="server" />
                                    <input id="hdCambioTraza" type="hidden" name="hdCambioTraza" runat="server" />
                                    <input id="hdCambioTrazaFondo" type="hidden" name="hdCambioTrazaFondo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbOperador" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.UsuarioCreacion") %>'
                                    ReadOnly="true" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbOperadorF" runat="server" Width="70px" Enabled="False" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nem&#243;nico">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbNemonico" Width="150px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CodigoNemonico") %>' />
                                    <input id="hdClaseInstrumento" type="hidden" name="hdClaseInstrumento" value='<%# DataBinder.Eval(Container, "DataItem.Categoria") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:TextBox ID="tbNemonicoF" Width="150px" runat="server" />
                                    </div>
                                    <input id="hdClaseInstrumentoF" type="hidden" name="hdClaseInstrumentoF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 22/05/2018--%>
                           <asp:TemplateField HeaderText="Portafolio">
                              <ItemTemplate>
                                    <asp:HiddenField ID="HdCodigoOrden" runat="server" Value='<%# Eval("CodigoPrevOrden") %>' />
                                    <asp:HiddenField ID="Hdfondos" runat="server" Value='<%# Eval("CodigoPortafolioSelec") %>' />
                                    <asp:DropDownList ID="ddlfondos" runat="server" Width="200px" />
                                    <input type="hidden" id="hdFondo1Trz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Fondos") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlPortafolioF" runat="server" Width="200px" />
                                    <asp:HiddenField ID="HdPortafolioF" runat="server" Value='0' />
                                    <asp:HiddenField ID="HdCodigoOrdenF" runat="server" Value='<%# Eval("CodigoPrevOrden") %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Categoría Contable">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoValorizacion" runat="server" />
                                    <asp:HiddenField ID="hdTipoValorizacion" runat="server" Value='<%# Eval("TipoValorizacion") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoValorizacionF" runat="server" />
                                    <asp:HiddenField ID="HdTipoValorizacionF" runat="server" Value='0' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Operación">
                                <ItemTemplate>
                                    <asp:TextBox ID="tFechaOperacion" runat="server" SkinID="Date" ReadOnly="True" 
                                        Text='<%# UIUtility.ConvertirFechaaString(Eval("FechaOperacion")) %>' ></asp:TextBox>
                                          <asp:HiddenField ID="hdFechaOperacion" runat="server" Value='<%# UIUtility.ConvertirFechaaString(Eval("FechaOperacion")) %>' />
                                </ItemTemplate>
                                 <FooterTemplate>
                                    <asp:TextBox ID="tFechaOperacionF" ReadOnly="true" runat="server" SkinID="Date" Width="100px" />
                                    <asp:HiddenField ID="hdFechaOperacionF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 22/05/2018--%>
                            <asp:TemplateField HeaderText="Operaci&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOperacion" runat="server" Width="80px" />
                                    <asp:Label ID="lbOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacion") %>'
                                    Visible="False" />
                                    <input type="hidden" id="hdOperacionTrz" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlOperacionF" runat="server" Width="80px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant. Instrumento">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCantidad" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cantidad") %>'
                                    Width="90px"  CssClass="Numbox-2" />
                                    <input type="hidden" id="hdCantidadTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Cantidad") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCantidadF" runat="server" onblur='javascript:cantidadF_CalcularMonto(this);'
                                    Width="90px"  CssClass="Numbox-2" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText ="Modo Neg.">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlIndice" runat="server" Width="80px" />
                                    <asp:Label ID="lbIndice" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IndPrecioTasa") %>'
                                    Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlIndiceF" runat="server" Width="80px" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTM %">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTasa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Tasa") %>' CssClass="Numbox-7" Width="90px" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTasaF" runat="server" CssClass="Numbox-7" Width="90px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecio" runat="server" ReadOnly="true"  Text='<%# DataBinder.Eval(Container, "DataItem.Precio") %>' Width="90px"  CssClass="Numbox-7" />
                                    <input type="hidden" id="hdPrecioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Precio") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioF" runat="server" CssClass="Numbox-7" Width="90px" onblur='javascript:precioF_CalcularMonto(this);' Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Tasa">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoTasa" runat="server" Width="80px" />
                                    <asp:Label ID="lbTipoTasa" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container,"DataItem.TipoTasa") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoTasaF" runat="server" Width="80px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Orden">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MontoNominal") %>'
                                    ReadOnly="true"  CssClass="Numbox-2" Width="90px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Plaza Neg.">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPlazaN" runat="server" Width="90px" />
                                    <asp:Label ID="lbPlazaN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPlaza") %>'
                                    Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlPlazaNF" runat="server" Width="90px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Condici&#243;n">
                                <ItemTemplate>
                                    <asp:Label ID="lbCondicion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoCondicion") %>' Visible="False" />
                                    <asp:DropDownList ID="ddlCondicion" runat="server" Width="90px" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCondicionF" runat="server" Width="90px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Intermediario">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbIntermediario" runat="server" Width="170px" Text='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>' />
                                    <input type="hidden" id="hdIntermediario" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoTercero") %>' name="hdIntermediario" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbIntermediarioF" runat="server" Width="170px" />
                                    <input type="hidden" id="hdIntermediarioF" runat="server" name="hdIntermediarioF" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medio de Negociaci&#243;n" >
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMedioNeg" runat="server" Width="120px" />
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MedioNegociacion") %>'
                                    Visible="False" ID="lbMedioNeg" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlMedioNegF" runat="server" Width="120px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Fondo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoFondo" runat="server" Width="90px" Enabled="False"  />
                                    <asp:Label ID="lbTipoFondo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoFondo") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoFondoF" runat="server" Width="90px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Tramo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoTramo" runat="server" Width="90px" />
                                    <asp:Label ID="lbTipoTramo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoTramo") %>'
                                    Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoTramoF" runat="server" Width="90px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha de Liquidaci&#243;n">
                                <ItemTemplate>
                                    <div id="FechaVal" runat="server" class="input-append date">
                                        <asp:TextBox ID="tbFechaLiquidacion" runat="server" SkinID="Date" Text='<%# DataBinder.Eval(Container, "DataItem.FechaLiquidacion") %>' />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="input-append date">
                                        <asp:TextBox ID="tbFechaLiquidacionF" runat="server" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant. Instrumento Ejecuci&#243;n" >
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCantidadOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CantidadOperacion") %>'
                                     Width="100px" CssClass="Numbox-2" />
                                    <input type="hidden" id="hdCantidadOperacionTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CantidadOperacion") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCantidadOperacionF" runat="server"  Width="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio Ejecuci&#243;n (Sucio)">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecioOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PrecioOperacion") %>'
                                    CssClass="Numbox-7" Width="100px"  ReadOnly="true"  />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioOperacionF" runat="server" CssClass="Numbox-7" Width="100px"  ReadOnly="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Ejecuci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotalOperacion" ReadOlny="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MontoOperacion") %>'
                                    CssClass="Numbox-7" Width="100px"  />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Interes Corrido">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbInteresCorrido" ReadOnly="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.InteresCorrido") %>'
                                    CssClass="Numbox-7" Width="100px"   />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText=" % " Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPorcentaje" runat="server" />
                                    <input type="hidden" id="hdPorcentaje" runat="server" value='<%# Eval("Porcentaje") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkPorcentajeF" runat="server" Checked ="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ver">
                                <ItemTemplate>
                                <asp:Button ID="btnVer" runat = "server" Text="Ver más" CommandName="SelectValorizador" 
                                CssClass="btn btn-integra" />
<%--                                <asp:ImageButton id="ibVer" Text="Ver" 
                                        runat="server" CommandName="SelectValorizador" 
                                        ImageUrl="~/App_Themes/img/icons/ver_mas.png" Width="89px" />--%>
                                    <asp:HiddenField ID="hdSBS" runat="server" 
                                        Value='<%# Eval("codigoSBS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hdNemonicoBusqueda" runat="server" />
                    <asp:HiddenField ID="hdIntermediarioBusqueda" runat="server" />
                    <asp:HiddenField ID="hdTipoRenta" runat="server" Value ="1" />
                    <asp:HiddenField ID="hdGrillaRegistros" runat="server" />
                    <asp:HiddenField runat="server" ID="hdFechaNegocio" />
                    <asp:HiddenField runat="server" ID="hdPuedeNegociar" />
<%--                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGrabar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnValidar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnValidarTrader" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAprobar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
        <br />
        <div class="loading" align="center">
            <%--Loading. Please wait.<br /><br />--%>
            <img src="../../App_Themes/img/icons/loading.gif" />
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6" >
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>