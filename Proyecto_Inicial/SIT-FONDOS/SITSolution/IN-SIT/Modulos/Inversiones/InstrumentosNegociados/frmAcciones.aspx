<%@ Page Language="VB" AutoEventWireup="true" CodeFile="frmAcciones.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmAcciones" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title></title>
    <style type="text/css">
        .oculto
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function MostrarTooltip(obj) {
            if (obj.options[obj.selectedIndex].title == "") {
                obj.title = obj.options[obj.selectedIndex].text;
                obj.options[obj.selectedIndex].title = obj.options[obj.selectedIndex].text;
                for (i = 0; i < obj.options.length; i++) {
                    obj.options[i].title = obj.options[i].text;
                }
            }
            else
                obj.title = obj.options[obj.selectedIndex].text;
        }
        function buscar() {
            strISIN = document.getElementById("txtISIN").value;
            winSelProd = document.open("BuscarValor.aspx?ISIN=" + strISIN, "winSelProd", "top=30,left=20,height=400,width=450,menubar=no,toolbar=no", true);
            winSelProd.focus();
        }
        function itemSelected(source, params) {
            if (source == 'LISTADO') {
                document.getElementById("txtISIN").value = params[0];
                document.getElementById("txtMnemonico").value = params[1];
                document.getElementById("txtSBS").value = params[2];
                document.getElementById("btnBuscar").click();
            }
            if (source == 'LISTADOORDENES') {
                document.getElementById("txtCodigoOrden").value = params[0];
                document.getElementById("txtISIN").value = params[1];
                document.getElementById("txtMnemonico").value = params[2];
                document.getElementById("ddlfondo").value = params[3];
                document.getElementById("lblMoneda").value = params[4];
                document.getElementById("ddlOperacion").value = params[5];
                document.getElementById("txtSBS").value = params[6];
                document.getElementById("btnBuscar").click();
            }
        }
        function OpenWindow1(ventana) {
            CargarPopUp('frmConsultaCuponeras.aspx?id=' + ventana);

        }
        function OpenWindow2(ventana) {
            CargarPopUp('frmConsultaLimitesInstrumento.aspx');

        }
        function MostrarMensaje(mensaje) {
            if (Validar()) {
                return confirm(mensaje);
            }
            else return false

        }
        function cambiaCantidad() {
            $('#txtNroAccOper').NumBox('setRaw', $('#txtNroAccOrde').val());
            return false;
        }
        function calcularMontoNominal() {
            if (txtNroAccOper.value != "" && txtPrecio.value != "") {
                var txtNroAccOper = "";
                var txtPrecio = "";
                var txtMontoNominal = "";
                var num = "";

                txtNroAccOper = txtNroAccOper.value;
                txtPrecio = txtPrecio.value;

                txtNroAccOper = txtNroAccOper.replace(/,/g, "");
                txtPrecio = txtPrecio.replace(/,/g, "");

                txtMontoNominal = (txtNroAccOper * txtPrecio);

                num = txtMontoNominal;

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

                if (pos1 == -1) {
                    tmp2 = '0000000';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                txtMontoNominal.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;
        }
        function calcularPrecio() {
            if (txtNroAccOper.value != "" && txtMontoNominal.value != "") {
                var txtNroAccOper = "";
                var txtNominal = "";
                var txtprecio = "";
                var num = "";

                txtNroAccOper = txtNroAccOper.value;
                txtNominal = txtMontoNominal.value;

                txtNroAccOper = txtNroAccOper.replace(/,/g, "");
                txtNominal = txtNominal.replace(/,/g, "");

                txtprecio = (txtNominal / txtNroAccOper);

                num = txtprecio;

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

                if (pos1 == -1) {
                    tmp2 = '0000000';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                txtPrecio.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;
        }
        function cambiaTitulo() {
            if (document.getElementById("ddlFondo").value == 'MULTIFONDO') {
                document.getElementById("lblTitulo").innerHTML = 'PreOrden de Inversión - ACCIONES';
            }
            else {
                document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - ACCIONES';
            }
            return false;
        }
        function Salida() {
            var strMensaje = "";
            var strAccion = "";

            strAccion = document.getElementById("hdMensaje").value
            var Pagina = document.getElementById("<%=hdPagina.ClientID %>").value

            if (strAccion != "") {
                if (document.getElementById("ddlFondo").value != 'MULTIFONDO') {
                    strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Acciones?"
                }
                else {
                    strMensaje = "¿Desea cancelar " + strAccion + " de Pre-Orden de Inversión de Acciones?"
                }

                if (strMensaje != "") {
                    confirmacion = confirm(strMensaje);
                    if (confirmacion == true) {
                        if (Pagina == "MODIFICA") {
                            window.close();
                        }
                        else {
                            location.href = "../../../frmDefault.aspx";
                        }
                    }
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                location.href = "../../../frmDefault.aspx";
            }
        }
        function Confirmar() {
            var strMensajeConfirmacion = "";
            var Pagina = "";
            var NroOrden = "";

            Pagina = document.getElementById("<%=hdPagina.ClientID %>").value
            NroOrden = document.getElementById("<%=txtCodigoOrden.ClientID %>").value

            switch (Pagina) {
                case "TI":
                    strMensajeConfirmacion = "¿Desea cancelar el Traspaso de Instrumento?";
                    break;
                case "EO":
                    strMensajeConfirmacion = "¿Desea cancelar la Ejecución de la Orden de Inversión Nro. " + NroOrden + "?";
                    break;
                case "CO":
                    strMensajeConfirmacion = "¿Desea cancelar la Confirmación de la Orden de Inversión Nro. " + NroOrden + "?";
                    break;
                case "XO":
                    strMensajeConfirmacion = "¿Desea cancelar el Extorno de la Orden de Inversión Nro. " + NroOrden + "?";
                    break;
                case "OE":
                    strMensajeConfirmacion = "¿Desea cancelar la Aprobacion de la Orden de Inversión Excedida Nro. " + NroOrden + "?";
                    break;
                case "DA":
                    strMensajeConfirmacion = "¿Desea cancelar la Negociación de la Orden de Inversión Nro. " + NroOrden + "?";
                    break;
            }

            if (strMensajeConfirmacion != "") {
                confirmacion = confirm(strMensajeConfirmacion);
                if (confirmacion == true) {
                    window.close();
                }
                return false;
            }
            else {
                if (Pagina == "CONSULTA" || Pagina == "MODIFICA") {
                    window.close();
                    return false;
                }
                else
                    return true;
            }
        }
        function Confirmacion() {
            var strMensajeConfirmacion = "";
            var Pagina = "";
            var NroOrden = "";

            Pagina = document.getElementById("<%=hdPagina.ClientID %>").value
            NroOrden = document.getElementById("<%=txtCodigoOrden.ClientID %>").value

            strMensajeConfirmacion = "";

            switch (Pagina) {
                case "CO":
                    strMensajeConfirmacion = "¿Realmente desea aceptar la Confirmación de la Orden de Inversión Nro. " + NroOrden + "?";
                    break;
            }

            if (strMensajeConfirmacion != "") {
                confirmacion = confirm(strMensajeConfirmacion);
                if (confirmacion == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return true;
            }
        }
        function Validar() {
            strMensajeError = "";
            if (ValidaCampos()) {
                return true;
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function Validar2() {
            strMensajeError = "";
            if (ValidaCampos()) {
                return true;
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function ValidaCampos() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "<br>-Portafolio<br>"
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Operación<br>"
            if (document.getElementById("<%= txtISIN.ClientID %>").value == "")
                strMsjCampOblig += "-Código ISIN<br>"
            if (document.getElementById("<%= txtMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "-Código Mnemónico<br>"
            if (document.getElementById("<%= txtSBS.ClientID %>").value == "")
                strMsjCampOblig += "-Código SBS<br>"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "-Código Intermediario<br>"
            if (document.getElementById("<%= txtNroAccOper.ClientID %>").value == "")
                strMsjCampOblig += "-Número Acciones Operación<br>"
            if (document.getElementById("<%= txtNroAccOrde.ClientID %>").value == "")
                strMsjCampOblig += "-Número Acciones Ordenadas<br>"
            if (document.getElementById("<%= txtPrecio.ClientID %>").value == "")
                strMsjCampOblig += "-Precio<br>"
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Operación<br>"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Vencimiento<br>"
            if (document.getElementById("<%= ddlPlaza.ClientID %>").value == "")
                strMsjCampOblig += "-Bolsa<br>"
            if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Hora Operación<br>"
            if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
            if (document.getElementById("<%= ddlGrupoInt.ClientID %>").value == "")
                strMsjCampOblig += "-Grupo Intermediario<br>"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            }
            {
                return true;
            }
        }
        function ValidarFondo() {
            strMensajeError = "";
            if (ValidaCamposFondo()) {
                return true;
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }
        function ValidaCamposFondo() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "-Portafolio<br>"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            }
            {
                return true;
            }
        }
        function EnabledFechaOperacion(element) {
            var elem = element.id;
            if (document.getElementById(elem).checked == true) {
                document.getElementById("<%= tbFechaOperacion.ClientID %>").disabled = false;
                if (document.getElementById("<%= hdFechaOperacion.ClientID %>").value == "") {
                    document.getElementById("<%= hdFechaOperacion.ClientID %>").value = document.getElementById("<%= tbFechaOperacion.ClientID %>").value;
                }
            }
            else {
                document.getElementById("<%= tbFechaOperacion.ClientID %>").disabled = true;
                if (document.getElementById("<%= hdFechaOperacion.ClientID %>").value != "") {
                    document.getElementById("<%= tbFechaOperacion.ClientID %>").innerText = document.getElementById("<%= hdFechaOperacion.ClientID %>").value;
                }
            }
        }

        function ShowDatosCarta() {
            var clase = $("#lkbMuestraModalDatos").attr('class');
            if (clase != "aspNetDisabled")
                window.open('../../Inversiones/InstrumentosNegociados/frmDatosCarta.aspx', "_blank", "toolbar=no,scrollbars=no,resizable=no,location=no,titlebar=no,top=500,left=500,width=600,height=300");
            else
                return;
        }


    </script>
</head>
<body onload="javascript:cambiaTitulo();">
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblTitulo" Text="Orden de Inversión - ACCIONES" runat="server" /></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h2>
                        <asp:Label ID="lblAccion" Text="" runat="server" />
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="up_busqueda" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lblFondo" class="col-sm-4 control-label">
                                    Portafolio</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlFondo" Width="120px" AutoPostBack="true"
                                        OnChange="javascript:cambiaTitulo();" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Operaci&oacute;n</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlOperacion" Width="120px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Moneda</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="lblMoneda" Width="150px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    C&oacute;digo ISIN</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" onkeypress="Javascript:Numero();" ID="txtISIN" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    C&oacute;digo Mnem&oacute;nico</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta | 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lblFondoDestino" class="col-sm-4 control-label" visible="false">
                                    Portafolio Destino</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlFondoDestino" Width="150px" Visible="False" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                        </div>
                        <div class="col-sm-4" style="text-align: right;">
                            <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                            <asp:Button runat="server" ID="btnCaracteristicas" Text="Características" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos de Operaci&oacute;n</legend>
            <asp:UpdatePanel ID="updDatosOperacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Hora Operación", "Hora Ejecución" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Operaci&oacute;n</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" Width="100px" ReadOnly="true" />
                                    <asp:CheckBox Text="" runat="server" ID="chkEmisionPrimaria" Visible="false" onclick="javascript:EnabledFechaOperacion(this)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Vencimiento
                                </label>
                                <div class="col-sm-7">
                                    <div id="imgFechaVcto" class="input-append date" runat="server">
                                        <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Hora Operación", "Hora Ejecución" que no se utilizará a sección oculta| 05/06/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo " Nro. Acciones Operación" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Nro. Acciones Ordenadas</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtNroAccOrde" Width="150px" CssClass="Numbox-7"
                                        onchange="cambiaCantidad()" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Precio</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtPrecio" Width="150px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo " Nro. Acciones Operación" que no se utilizará a sección oculta| 05/06/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Tipo Tramo" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Operaci&oacute;n</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtMontoNominal" Width="150px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Bolsa</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlPlaza" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Tipo Tramo" que no se utilizará a sección oculta| 05/06/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Contacto","Observación", "Medio de Transmisión", "No Liquida en Caja", "Regularización SBS", "Recalcular" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Grupo de Intermediarios</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlGrupoInt" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Intermediario</label>
                                <div class="col-sm-7">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlIntermediario" Width="270px" AutoPostBack="True" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlGrupoInt" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trMotivoCambio" style="display: none">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Motivo de cambio</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lblComentarios" class="col-sm-5 control-label">
                                    Comentarios</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="4" Width="270px"
                                        Style="text-transform: uppercase;" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <div class="col-sm-7">
                                <asp:CheckBox ID="chkFicticia" Text="No Liquida en Caja" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <label class="col-sm-5 control-label">
                                Datos Cartas</label>
                            <div class="col-md-7 text-left">
                                <asp:LinkButton ID="lkbMuestraModalDatos" runat="server" OnClientClick="javascript:ShowDatosCarta();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <div class="col-sm-7">
                                    <asp:CheckBox ID="chkRegulaSBS" Text="Regularizaci&oacute;n SBS" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <div class="col-sm-7">
                                    <asp:CheckBox ID="chkRecalcular" Text="Recalcular" runat="server" Checked="True" />
                                </div>
                            </div>
                        </div>
                    </div>
            <%--        <div class="row">
              
                    </div>--%>
                   
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Contacto","Observación", "Medio de Transmisión", "No Liquida en Caja", "Regularización SBS", "Recalcular" que no se utilizará a sección oculta| 05/06/18 --%>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                    <asp:PostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset>
            <legend>Comisiones y Gastos de Administradora</legend>
            <div class="grilla">
                <asp:UpdatePanel ID="id1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="dgLista" SkinID="Grid" DataKeyNames="codigoComision2">
                            <Columns>
                                <asp:BoundField DataField="codigoComision1" HeaderText="C&#243;digo Impuesto/Comisi&#243;n"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisi&#243;n" />
                                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisi&#243;n" />
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="strValorCalculadoComision1"
                                    HeaderText="Comisi&#243;n" />
                                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision1" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="ValorOcultoComision1"
                                    HeaderText="ValorOcultoComision1" />
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="codigoComision2"
                                    HeaderText="C&#243;digo Impuesto/Comisi&#243;n" />
                                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisi&#243;n" />
                                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisi&#243;n" />
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="strValorCalculadoComision2"
                                    HeaderText="Comisi&#243;n" />
                                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision2" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="ValorOcultoComision2"
                                    HeaderText="ValorOcultoComision2" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlIntermediario" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlPlaza" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="up3" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Total Comisiones</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txttotalComisionesC" Width="150px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Neto Operaci&oacute;n</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtMontoNetoOpe" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Precio Promedio</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtPrecPromedio" Width="150px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tblDestino" runat="server" class="row" style="visibility: hidden;">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Comisiones&nbsp;<span runat="server" id="lblMDest"></span>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtComisionesDestino" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Monto Operaci&oacute;n&nbsp;<span runat="server" id="lblMDest2"></span>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtMontoOperacionDestino" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="row">
                <div class="col-sm-6">
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <asp:Button Text="Asignar" runat="server" ID="btnAsignar" Visible="false" />
                    <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                    <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                </div>
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Modificar" runat="server" ID="btnModificar" Style="height: 26px" />
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" />
                <asp:Button Text="Consultar" runat="server" ID="btnConsultar" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <asp:Button Text="Salir" runat="server" ID="btnRetornar" Visible="false" />
                        <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" UseSubmitBehavior="False" />
                        <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <input runat="server" type="hidden" id="hdMensaje" name="hdMensaje" />
    <input runat="server" type="hidden" id="txtCodigoOrden" name="txtCodigoOrden" />
    <asp:HiddenField ID="hdPagina" runat="server" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <input runat="server" type="hidden" id="hdSaldo" name="hdSaldo" />
    <input runat="server" type="hidden" id="hdCustodio" name="hdCustodio" />
    <input runat="server" type="hidden" id="hdNumUnidades" name="hdNumUnidades" />
    <input runat="server" type="hidden" id="hdFechaOperacion" name="hdFechaOperacion" />
    <input runat="server" type="hidden" id="hdPopUp" name="hdPopUp" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladaron los campos que no se utilizará a sección oculta | 05/06/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS</legend>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        C&oacute;digo SBS</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtSBS" Width="150px" />
                    </div>
                </div>
            </div>
        </div>
        <fieldset class="oculto">
            <legend>Caracter&iacute;sticas del Valor</legend>
            <asp:UpdatePanel ID="up_valor" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row oculto">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Market Cap</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblMarketCap" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Saldo</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblSaldoValor" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Indicador 1</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblPriceEarnings" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row oculto">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Siguiente Dividendo - Fecha</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblSigDivFecha" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Monto Negociado Diario Promedio</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblMonNegDiaProm" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Indicador 2</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblValorDFC" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row oculto">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Siguiente Dividendo - Factor</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblSigDivFactor" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Nro Operaciones Diarias Promedio</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblNroOperDiaProm" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    %Fondo</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtporcentaje" runat="server" Width="104px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                        </div>
                        <div class="col-md-5">
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <div class="col-sm-11 control-label">
                                    <%--<asp:Button runat="server" ID="btnCaracteristicas" Text="Características" />--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <div class="row">
            <div class="col-sm-2 oculto">
                <div class="form-group">
                    <label class="col-sm-7 control-label" style="margin-left: -45px;">
                        Hora Operaci&oacute;n</label>
                    <div class="col-sm-5">
                        <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" />
                    </div>
                </div>
            </div>
            <div class="col-sm-2 oculto">
                <div class="form-group">
                    <label class="col-sm-7 control-label" style="margin-left: -15px;">
                        Hora Ejecuci&oacute;n
                    </label>
                    <div class="col-sm-5">
                        <asp:TextBox runat="server" ID="tbHoraEjecucion" SkinID="Hour" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Nro. Acciones Operaci&oacute;n</label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="txtNroAccOper" Width="150px" CssClass="Numbox-7" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label runat="server" id="lNPoliza" class="col-sm-5 control-label" visible="false">
                            Nro. Poliza</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbNPoliza" Width="150px" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Tipo Tramo</label>
                    <div class="col-sm-7">
                        <asp:DropDownList runat="server" ID="ddlTipoTramo" Width="150px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Contacto</label>
                    <div class="col-sm-7">
                        <asp:DropDownList runat="server" ID="ddlContacto" Width="150px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Observaci&oacute;n
                    </label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="txtObservacion" MaxLength="20" Width="150px" Style="text-transform: uppercase" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Medio Transmisi&oacute;n</label>
                    <div class="col-sm-7">
                        <asp:DropDownList runat="server" ID="ddlMedioTrans" Width="150px" />
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladaron los campos que no se utilizará a sección oculta | 05/06/18 --%>
    </form>
</body>
</html>
