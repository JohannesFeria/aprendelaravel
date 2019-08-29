<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInstrumentosEstructurados.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmInstrumentosEstructurados" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Instrumentos Estructurados</title>
        <style type="text/css">
        .oculto {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function ValidarFondo() {
            strMensajeError = "";
            if (ValidaCamposFondo()) {
                return true;
            }
            else {
                alert(strMensajeError);
                return false;
            }
        }

        function ValidaCamposFondo() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "\t-Portafolio\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
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


        function formatCurrencyOperacion(num) {

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

                frmInvocador.txtUnidadesOrd.value = (((sign) ? '' : '-') + num + '.' + tmp2);
                frmInvocador.txtUnidadesOp.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;
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




        function formatCurrencyMontoNominal(num) {

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

                frmInvocador.txtUnidadesOp.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;

        }

        function CalcularMontoNominal() {
            if (frmInvocador.txtUnidadesOp.value != "" && frmInvocador.txtPrecio.value != "") {
                var txtUnidadesOp = "";
                var txtPrecio = "";
                var txtMontoNominal = "";
                var num = "";

                txtUnidadesOp = frmInvocador.txtUnidadesOp.value;
                txtPrecio = frmInvocador.txtPrecio.value;

                txtUnidadesOp = txtUnidadesOp.replace(/,/g, "");
                txtPrecio = txtPrecio.replace(/,/g, "");

                txtMontoNominal = (txtUnidadesOp * txtPrecio);

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


        function cambiaTitulo() {
            frmInvocador.document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - INSTRUMENTOS ESTRUCTURADOS';
//            if (document.getElementById("ddlFondo").value == 'MULTIFONDO') {
//                frmInvocador.document.getElementById("lblTitulo").innerHTML = 'PreOrden de Inversión - INSTRUMENTOS ESTRUCTURADOS';
//            }
//            else {
//                frmInvocador.document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - INSTRUMENTOS ESTRUCTURADOS';
//            }

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
                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Instrumentos Estructurados?"

//                if (document.getElementById("ddlFondo").value != 'MULTIFONDO') {
//                    strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Instrumentos Estructurados?"
//                }
//                else {
//                    strMensaje = "¿Desea cancelar " + strAccion + " de Pre-Orden de Inversión de Instrumentos Estructurados?"
//                }

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
                alert(strMensajeError);
                return false;
            }
        }

        function ValidaCampos() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "\t-Portafolio\n"
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Operación\n"
            if (document.getElementById("<%= txtISIN.ClientID %>").value == "")
                strMsjCampOblig += "\t-Código ISIN\n"
            if (document.getElementById("<%= txtMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "\t-Código Mnemónico\n"
            if (document.getElementById("<%= txtSBS.ClientID %>").value == "")
                strMsjCampOblig += "\t-Código SBS\n"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "\t-Intermediario\n"
            if (document.getElementById("<%= txtUnidadesOrd.ClientID %>").value == "")
                strMsjCampOblig += "\t-Número Unidades Ordenado\n"
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Operación\n"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Vencimiento\n"
            if (document.getElementById("<%= txtUnidadesOp.ClientID %>").value == "")
                strMsjCampOblig += "\t-Número Unidades Operación\n"
            if (document.getElementById("<%= txtPrecio.ClientID %>").value == "")
                strMsjCampOblig += "\t-Precio\n"
            if (document.getElementById("<%= txtMontoNominal.ClientID %>").value == "")
                strMsjCampOblig += "\t-Monto Operación\n"
            if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Hora Operación\n"
            if (document.getElementById("<%= tbMontoPrima.ClientID %>").value == "") //RGF 20090330
                strMsjCampOblig += "\t-Monto Prima\n"
            if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "\t-Formato de Hora Incorrecto\n"
            if (document.getElementById("<%= ddlGrupoInt.ClientID %>").value == "")
                strMsjCampOblig += "\t-Grupo de Intermediarios\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
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
                        <asp:Label ID="lblTitulo" runat="server">Orden de Inversión - INSTRUMENTOS ESTRUCTURADOS</asp:Label></h2>
                </div>
                <div class="col-md-3" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <br />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:label id="lblFondo" runat="server">Portafolio</asp:label></label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlFondo" runat="server" Enabled="False" OnChange="javascript:cambiaTitulo();"
                            AutoPostBack="True" Width="120px">
                        </asp:DropDownList>
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
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Moneda</label>
                    <div class="col-sm-8">                        
                        <asp:TextBox ID="lblMoneda" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código Isin</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtISIN" runat="server" Width="120px" MaxLength="12"></asp:TextBox>
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
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código SBS</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtSBS" runat="server" Width="120px" MaxLength="12"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
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
            <div class="col-md-8" style="text-align: right;">
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar"/>
            </div>
        </div>
        <br />
        <fieldset class="oculto">
            <legend>Características del Valor</legend>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">Mnemónico y Descripción del 1er Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblnemo1" runat="server" Width="95px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            Mnemónico y Descripción del 2do Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblnemo2" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            Mnemónico y Descripción del 3er Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblnemo3" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            % Composición del 1er Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblporc1" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            % Composición del 2do Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblporc2" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            % Composición del 3er Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblporc3" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            Precio Composición del 1er Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblprecio1" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            Precio Composición del 2do Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblprecio2" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            Precio Composición del 3er Instrumento</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblprecio3" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-8 control-label">
                            % Participación</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="lblparticipacion" runat="server" Width="95px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <div class="col-sm-12 control-label">
                            <asp:Button runat="server" ID="btnCaracteristicas" Text="Características" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos de Operación</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Fecha Operación</label>
                        <div class="col-sm-6">
                            <div class="input-append">
                                <div id="imgFechaOperacion" runat="server" class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Width="100px" Enabled="False" AutoPostBack="True" />
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
                                    <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" Width="120px" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 oculto">
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
                            Unidades Ordenados</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtUnidadesOrd" onblur="javascript: CalcularMontoNominal();"
                                runat="server" CssClass="Numbox-7" Width="112px" MaxLength="12"
                                ></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 oculto">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Unidades Operación</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtUnidadesOp" runat="server" CssClass="Numbox-7" Width="112px"
                                MaxLength="12"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Precio</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtPrecio" onblur="javascript: CalcularMontoNominal();" runat="server"
                                CssClass="Numbox-7" Width="96px" MaxLength="12"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">Monto Operación</label>
                        <div class="col-sm-6"><asp:TextBox ID="txtMontoNominal" runat="server" CssClass="Numbox-7" Width="114px" MaxLength="12" ReadOnly="True" /></div>
                        <%--CssClass="Numbox-7_12"--%>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                        </label>
                        <div class="col-sm-6"></div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            <asp:Label ID="lNPoliza" runat="server" Visible="False">Nro.Poliza</asp:Label></label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="tbNPoliza" runat="server" Width="100px" MaxLength="15" Visible="False"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Grupo de Intermediarios</label>
                        <div class="col-sm-6">
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
                <div class="col-md-4 oculto">
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
                <div class="col-md-4 oculto">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Observación</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtObservacion" runat="server" Width="224px" MaxLength="20" style="text-transform:uppercase" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label"></label>
                        <div class="col-sm-6"></div>
                    </div>
                </div>
                <div class="col-md-4 oculto">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Monto Prima</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="tbMontoPrima" CssClass="Numbox-0_15" runat="server" Width="120px" MaxLength="22"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label" style="margin-left: -4px;">
                            <asp:CheckBox ID="chkFicticia" Style="margin-left: 1px;" runat="server" Enabled="False"
                                Text="No Liquida en Caja"></asp:CheckBox>
                        </label>
                        <div class="col-sm-6 control-label" style="text-align: left;">
                            <asp:CheckBox ID="chkRegulaSBS" runat="server" Text="Regularización SBS"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row" id="trMotivoCambio" style="DISPLAY: none">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Motivo de cambio</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label runat="server" id="lblComentarios" class="col-sm-6 control-label">Comentarios</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="4" Width="250px" />
                        </div>
                    </div>
                </div>
            </div>

        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="col-sm-12 control-label">
                        <asp:Button runat="server" ID="btnProcesar" Text="Procesar" OnClientClick="return Validar();" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
<%--                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnLimites" Text="Limites" />                        
                    </div>--%>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <div class="col-sm-12 control-label">
                        <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" />
                    </div>
                </div>
            </div>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnIngresar" Text="Ingresar" />
                        <asp:Button runat="server" ID="btnModificar" Text="Modificar" />
                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" />
                        <asp:Button runat="server" ID="btnConsultar" Text="Consultar" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <div class="col-sm-12" style="text-align: right;">
                        <asp:Button runat="server" ID="btnRetornar" Text="Salir" Visible="False" />
                        <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" />
                        <asp:Button runat="server" ID="btnSalir" Text="Salir" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="txtCodigoOrden" runat="server" />
    <asp:HiddenField ID="hdPagina" runat="server" />
    <asp:HiddenField ID="hdCustodio" runat="server" />
    <asp:HiddenField ID="hdSaldo" runat="server" />
    <asp:HiddenField ID="hdNumUnidades" runat="server" />
    <asp:HiddenField ID="hdMensaje" runat="server" />
    <asp:HiddenField ID="hfModal" runat="server" />
    </form>
</body>
</html>
