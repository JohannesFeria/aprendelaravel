<%@ Page Language="VB" AutoEventWireup="True" CodeFile="frmCertificadosSuscripcion.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmCertificadosSuscripcion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Bonos</title>

    <script type="text/javascript">
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
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br>";
                return false;
            }
            {
                return true;
            }
        }

        function Numero() {
            tecla = window.event.keyCode
            if ((tecla >= 48 && tecla <= 57) || tecla == 32 || tecla == 46)
            { }
            else {
                window.event.keyCode = 0
            }
        }


        function formatCurrencyPrecio(num) {

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

                if (pos1 == -1) {
                    tmp2 = '0000000';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                frmInvocador.txtPrecio.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;

        }

        function formatCurrency(num) {

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

                if (pos1 == -1) {
                    tmp2 = '0000000';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                frmInvocador.txtNroAccOper.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;

        }

        function formatCurrencyAccionesOperacion(num) {

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

                if (pos1 == -1) {
                    tmp2 = '0000000';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                frmInvocador.txtNroAccOper.value = (((sign) ? '' : '-') + num + '.' + tmp2);
                frmInvocador.txtNroAccOrde.value = (((sign) ? '' : '-') + num + '.' + tmp2);

            }
            return false;

        }
        function formatCurrencyMontoOperacion(num) {

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

                if (pos1 == -1) {
                    tmp2 = '0000000';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                frmInvocador.txtMontoNominal.value = (((sign) ? '' : '-') + num + '.' + tmp2);


            }
            return false;

        }

        function buscar() {
            strISIN = document.getElementById("txtISIN").value;
            winSelProd = document.open("BuscarValor.aspx?ISIN=" + strISIN, "winSelProd", "top=30,left=20,height=400,width=450,menubar=no,toolbar=no", true);
            winSelProd.focus();
        }
        function itemSelected(source, params) {
            if (source == 'LISTADO') {
                frmInvocador.document.getElementById("txtISIN").value = params[0];
                frmInvocador.document.getElementById("txtMnemonico").value = params[1];
                frmInvocador.document.getElementById("txtSBS").value = params[2];
                frmInvocador.document.getElementById("btnBuscar").click();
            }
            if (source == 'LISTADOORDENES') {
                frmInvocador.document.getElementById("txtCodigoOrden").value = params[0];
                frmInvocador.document.getElementById("txtISIN").value = params[1];
                frmInvocador.document.getElementById("txtMnemonico").value = params[2];
                frmInvocador.document.getElementById("ddlfondo").value = params[3];
                frmInvocador.document.getElementById("lblMoneda").value = params[4];
                frmInvocador.document.getElementById("ddlOperacion").value = params[5];
                frmInvocador.document.getElementById("txtSBS").value = params[6];
                frmInvocador.document.getElementById("btnBuscar").click();

            }
        }

        function OpenWindow1(ventana) {
            CargarPopUp('ConsultaCuponeras.aspx?id=' + ventana);

        }
        function OpenWindow2(ventana) {
            CargarPopUp('ConsultaLimitesInstrumento.aspx');

        }
        function MostrarMensaje(mensaje) {
            if (Validar()) {
                return confirm(mensaje);
            }
            else
                return false;
        }
        function cambiaCantidad() {
            frmInvocador.txtNroAccOper.value = frmInvocador.txtNroAccOrde.value;
            return false;
        }
        function calcularMontoNominal() {
            if (frmInvocador.txtNroAccOper.value != "" && frmInvocador.txtPrecio.value != "") {
                var txtNroAccOper = "";
                var txtPrecio = "";
                var txtMontoNominal = "";
                var num = "";

                txtNroAccOper = frmInvocador.txtNroAccOper.value;
                txtPrecio = frmInvocador.txtPrecio.value;

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


                frmInvocador.txtMontoNominal.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;
        }

        function CalcularPrecio() {

            if (frmInvocador.txtNroAccOper.value != "" && frmInvocador.txtMontoNominal.value != "") {
                var txtNroAccOper = "";
                var txtNominal = "";
                var txtprecio = "";
                var num = "";

                txtNroAccOper = frmInvocador.txtNroAccOper.value;
                txtNominal = frmInvocador.txtMontoNominal.value;

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

                frmInvocador.txtPrecio.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;
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

        function Salida() {
            var strMensaje = "";
            var strAccion = "";

            strAccion = document.getElementById("hdMensaje").value
            var Pagina = document.getElementById("<%=hdPagina.ClientID %>").value

            if (strAccion != "") {
                if (document.getElementById("ddlFondo").value != 'MULTIFONDO') {
                    strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Certificados de Suscripción?"
                }
                else {
                    strMensaje = "¿Desea cancelar " + strAccion + " de Pre-Orden de Inversión de Certificados de Suscripción?"
                }

                if (strMensaje != "") {
                    confirmacion = confirm(strMensaje);
                    if (confirmacion == true) {
                        if (Pagina == "MODIFICA") {
                            window.close();
                        }
                        else {
                            location.href = "../../../Bienvenida.aspx";
                        }
                    }
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                location.href = "../../../Bienvenida.aspx";
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

        function ValidaCampos() {

            var strMsjCampOblig = "";
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "-Portafolio<br>"
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Operación<br>"
            if (document.getElementById("<%= txtISIN.ClientID %>").value == "")
                strMsjCampOblig += "-Código ISIN<br>"
            if (document.getElementById("<%= txtMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "-Código Mnemónico<br>"
            if (document.getElementById("<%= txtSBS.ClientID %>").value == "")
                strMsjCampOblig += "-Código SBS<br>"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "-Intermediario<br>"
            if (document.getElementById("<%= txtNroAccOrde.ClientID %>").value == "")
                strMsjCampOblig += "-Número Acciones Ordenado<br>"
            if (document.getElementById("<%= txtNroAccOper.ClientID %>").value == "")
                strMsjCampOblig += "-Número Acciones Operación<br>"
            if (document.getElementById("<%= txtPrecio.ClientID %>").value == "")
                strMsjCampOblig += "-Precio<br>"
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Operación<br>"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Vencimiento<br>"
            if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Hora Operación<br>"
            if (document.getElementById("<%= txtMontoNominal.ClientID %>").value == "")
                strMsjCampOblig += "-Monto Operación<br>"
            if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
            if (document.getElementById("<%= ddlGrupoInt.ClientID %>").value == "")
                strMsjCampOblig += "-Grupo de Intermediarios<br>"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br>";
                return false;
            }
            {
                return true;
            }
        }

        function mostrarPopup() {
            var isin = document.getElementById('txtISIN').value;
            var sbs = document.getElementById('txtSBS').value;
            var mnemonico = document.getElementById('txtMnemonico').value;
            var fondo = document.getElementById('ddlFondo').value;
            var operacion = document.getElementById('ddlOperacion').value;
            var categoria = "CS";
            var aux = "1";

            return showModalDialog('frmBuscarValor.aspx?vISIN=' + isin + '&vSBS=' + sbs + '&vMnemonico=' + mnemonico + '&vFondo=' + fondo + '&vOperacion=' + operacion + '&vCategoria=' + categoria , '950', '600', '');
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
<body>
    <form id="frmInvocador" runat="server" method="post" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-9">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Orden de Inversión - CERTIFICADOS DE SUSCRIPCIÓN</asp:Label></h2>
                </div>
                <div class="col-md-3" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <br />
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Moneda" que no se utilizará a sección oculta| 28/05/18 --%>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblFondo" runat="server">Portafolio</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlFondo" runat="server" Enabled="False" AutoPostBack="True"
                            Width="120px" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Operación</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlOperacion" runat="server" Enabled="False" Width="120px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-4" id="divMoneda" style="display:none">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Moneda</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="lblMoneda" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Moneda" que no se utilizará a sección oculta| 28/05/18 --%>
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta| 28/05/18 --%>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código Isin</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtISIN" runat="server" Width="120px" MaxLength="12"></asp:TextBox>
                        <asp:HiddenField ID="txtCodigoOrden" runat="server" />
                        <%--<asp:TextBox ID="txtCodigoOrden" runat="server" Width="1px" MaxLength="20"></asp:TextBox>--%>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código Mnemónico</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtMnemonico" runat="server" Width="120px" MaxLength="15"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta| 28/05/18 --%>
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 28/05/18 --%>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar" Style="height: 26px" />
                <%--OnClientClick="mostrarPopup();"--%>
            </div>
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button runat="server" ID="btnCaracteristicas" Text="Características" />
            </div>
        </div>
        <br />
        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 28/05/18 --%>
        <fieldset>
            <legend>Datos de Operación</legend>
            <asp:UpdatePanel runat="server" ID="updDatos" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 28/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Operación</label>
                                <div class="col-sm-7">
                                    <div class="input-append">
                                        <div id="imgFechaOperacion" runat="server" class="input-append date">
                                            <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Width="100px" Enabled="False"
                                                AutoPostBack="True" />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Vencimiento</label>
                                <div class="col-sm-6">
                                    <div class="input-append">
                                        <div id="imgFechaVcto" runat="server" class="input-append date">
                                            <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" Width="100px" />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 28/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Nro. Papeles Operación" que no se utilizará a sección oculta| 28/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Nro. Papeles Ordenadas</label>
                                <div class="col-sm-7">
                                    <%--<asp:TextBox runat="server" ID="txtMnomOrd" Width="100px" MaxLength="20" CssClass="Numbox-7"
                                onblur="Javascript:cambiaCantidad();" />--%>
                                    <asp:TextBox ID="txtNroAccOrde" runat="server" Width="120px" CssClass="Numbox-7"
                                        onblur="Javascript:cambiaCantidad();calcularMontoNominal();"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Precio</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtPrecio" runat="server" Width="120px" MaxLength="12" ReadOnly="True"
                                        CssClass="Numbox-7" onblur="Javascript: calcularMontoNominal();"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Nro. Papeles Operación" que no se utilizará a sección oculta| 28/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Bolsa" que no se utilizará a sección oculta| 28/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Operación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtMontoNominal" runat="server" Width="120px" MaxLength="12" CssClass="Numbox-7"
                                        onblur="Javascript: CalcularPrecio();"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Bolsa</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlPlaza" runat="server" Enabled="False" AutoPostBack="True"
                                        Width="150px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Bolsa" que no se utilizará a sección oculta| 28/05/18 --%>
                    <asp:UpdatePanel ID="uprow1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación", "No Liquida en Caja" y "Regularización SBS" que no se utilizará a sección oculta| 28/05/18 --%>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">
                                            Grupo de Intermediarios</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlGrupoInt" runat="server" AutoPostBack="True" Enabled="False"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Intermediario</label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlIntermediario" runat="server" Enabled="False" AutoPostBack="True"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="DivObservacion" runat="server">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label runat="server" id="Label1" class="col-sm-5 control-label">
                                            Observación Carta</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox runat="server" ID="txtObservacionCarta" TextMode="MultiLine" Rows="4" Width="200px" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4" runat="server" id="DivDatosCarta">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Datos Cartas</label>
                                        <div class="col-sm-5">
                                            <asp:LinkButton ID="lkbMuestraModalDatos" runat="server" OnClientClick="javascript:ShowDatosCarta();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                         <%--  <div class="row" runat="server" id="DivDatosCarta">                     
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">
                                            Datos Cartas</label>
                                        <div class="col-sm-7">
                                            <asp:LinkButton ID="lkbMuestraModalDatos" runat="server" OnClientClick="javascript:ShowDatosCarta();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación", "No Liquida en Caja" y "Regularización SBS" que no se utilizará a sección oculta| 28/05/18 --%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row" id="trMotivoCambio" style="display: none">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Motivo de cambio</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="150px" />
                                    <%--AutoPostBack="true"--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label runat="server" id="lblComentarios" class="col-sm-6 control-label">
                                    Comentarios</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="4" Width="250px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset>
            <legend>Comisiones y Gastos de Administradora</legend>
            <div class="grilla">
                <asp:UpdatePanel ID="upGrilla" runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server" SkinID="Grid" ID="dgLista" DataKeyNames="codigoComision2">
                            <Columns>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="codigoComision1"
                                    HeaderText="Código Impuesto/Comisión"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisión">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisión">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="strValorCalculadoComision1"
                                    HeaderText="Comisión"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision1" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorOcultoComision1"
                                    HeaderText="ValorOcultoComision1"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="codigoComision2"
                                    HeaderText="Código Impuesto/Comisión"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisión">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisión">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="strValorCalculadoComision2"
                                    HeaderText="Comisión"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision2" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorOcultoComision2"
                                    HeaderText="ValorOcultoComision2"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPlaza" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <asp:UpdatePanel ID="up3" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Total Comisiones</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txttotalComisionesC" runat="server" Width="230px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Monto Neto Operación</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtMontoNetoOpe" runat="server" CssClass="stlCajaBloqueadoNumero"
                                    Width="230px" ForeColor="Black" MaxLength="20" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Precio Promedio</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtPrecPromedio" runat="server" Width="230px" MaxLength="12" ReadOnly="true"></asp:TextBox>
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
            <div class="col-md-4">
            </div>
            <div class="col-md-5">
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <div class="col-sm-12 control-label">
                        <asp:Button runat="server" ID="btnAsignar" Text="Asignar" />
                        <asp:Button runat="server" ID="btnProcesar" Text="Procesar" OnClientClick="return Validar();" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tblDestino" runat="server" class="row" style="visibility: hidden">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Comisiones
                        <asp:Label ID="lblMDest" runat="server"></asp:Label></label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtComisionesDestino" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Monto Operación
                        <asp:Label ID="lblMDest2" runat="server"></asp:Label></label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtMontoOperacionDestino" runat="server" Width="200px"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <div class="col-sm-12">
                        <%--<asp:Button runat="server" ID="btnLimites" Text="Limites" Visible="false" />
                        <asp:Button runat="server" ID="ibConsultaCertificados" Text="Consulta Certificados" Visible="false" />
                        <asp:Button runat="server" ID="btnLimitesParametrizados" Text="Consultar Limites Parametrizados" Visible="False" />--%>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <div class="col-sm-12" style="text-align: right;">
                        <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" />
                    </div>
                </div>
            </div>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                    <asp:Button runat="server" ID="btnIngresar" Text="Ingresar" />
                    <asp:Button runat="server" ID="btnModificar" Text="Modificar" />
                    <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" />
                    <asp:Button runat="server" ID="btnConsultar" Text="Consultar" />
            </div>
            <div class="col-sm-6" style="text-align: right;">
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnRetornar" Text="Salir" Visible="False" />
                        <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" UseSubmitBehavior="False" />
                        <asp:Button runat="server" ID="btnSalir" Text="Salir" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdPagina" runat="server" />
    <asp:HiddenField ID="hdCustodio" runat="server" />
    <asp:HiddenField ID="hdSaldo" runat="server" />
    <asp:HiddenField ID="hdNumUnidades" runat="server" />
    <asp:HiddenField ID="hdMensaje" runat="server" />
    <asp:HiddenField ID="hfModal" runat="server" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación| 07/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <asp:Button ID="btnModal" runat="server" />
      <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 28/05/18 --%>
    <fieldset class="hidden">
        <legend>Campos Retirados</legend>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código SBS</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtSBS" runat="server" Width="120px" MaxLength="12"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblFondoDestino" runat="server" Visible="false">Portafolio Destino</asp:Label>
                    </label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlFondoDestino" runat="server" Enabled="False" Visible="false"
                            Width="120px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <fieldset>
            <legend>Caracter&iacute;sticas del Valor</legend>
            <div class="row">
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
            <div class="row">
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
            <div class="row">
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
                        <label class="col-sm-7 control-label">%Fondo</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtporcentaje" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row"><div class="col-sm-3"></div></div>
        </fieldset>
        <br />
        <div class="row">
          <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Hora Operación</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" Width="100px" />                            
                    </div>
                </div>
           </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Nro. Papeles Operación</label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtNroAccOper" runat="server" Width="120px" MaxLength="12" ReadOnly="True" CssClass="Numbox-7"></asp:TextBox>
                            <%--onblur="formatCurrency(frmInvocador.txtMnomOp.id);" />--%>
                    </div>
                </div>
             </div>
        </div>
        <div class="row">
         
             <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            <asp:Label ID="lNPoliza" runat="server" Visible="False">Nro.Poliza :</asp:Label></label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="tbNPoliza" runat="server" Width="120px" MaxLength="15" Visible="False"></asp:TextBox>
                        </div>
                    </div>
               </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Contacto</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlContacto" runat="server" Enabled="False" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
              </div>
        </div>
        <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Observación</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtObservacion" runat="server" Width="280px" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label"></label>
                        <div class="col-sm-6"></div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label" style="text-align: left;" >
                            <asp:CheckBox ID="chkFicticia" Style="margin-left: 1px;" runat="server" Enabled="False"
                                Text="No Liquida en Caja"></asp:CheckBox>
                        </label>
                        <div class="col-sm-6 control-label" style="text-align: left;" >
                            <asp:CheckBox ID="chkRegulaSBS" runat="server" Text="Regularización SBS" Enabled="False"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
     </fieldset>
      <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 28/05/18 --%>
    </form>
</body>
</html>

