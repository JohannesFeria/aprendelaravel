<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOperacionesFuturas.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_frmOperacionesFuturas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Bonos</title>
    <script type="text/javascript">
        function Numero() {
            tecla = window.event.keyCode
            if ((tecla >= 48 && tecla <= 57) || tecla == 32 || tecla == 46)
            { }
            else {
                window.event.keyCode = 0
            }
        }

        function cambiaCantidad() {
            txtNumConOperacion.value = txtNumConOrdenados.value;
            return false;
        }

        function buscar() {
            strISIN = document.getElementById("txtISIN").value;
            winSelProd = document.open("frmBuscarValor.aspx?ISIN=" + strISIN, "winSelProd", "top=30,left=20,height=400,width=450,menubar=no,toolbar=no", true);
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
                document.getElementById("lblTitulo").innerHTML = 'PreOrden de Inversión - Operaciones Futuras';
            }
            else {
                document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - Operaciones Futuras';
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
                            location.href = "~/frmDefault.aspx";
                        }
                    }
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                location.href = "~/frmDefault.aspx";
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
                alert(strMensajeError);
                return false;
            }
        }

        function Validar2() {
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
                strMsjCampOblig += "\t-Código Intermediario\n"
            if (document.getElementById("<%= txtNumConOrdenados.ClientID %>").value == "")
                strMsjCampOblig += "\t-Número Acciones Operación\n"
            if (document.getElementById("<%= txtNumConOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Número Acciones Ordenadas\n"
            if (document.getElementById("<%= txtPrecio.ClientID %>").value == "")
                strMsjCampOblig += "\t-Precio\n"
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Operación\n"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Vencimiento\n"
            if (document.getElementById("<%= ddlPlaza.ClientID %>").value == "")
                strMsjCampOblig += "\t-Mercado\n"
            if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Hora Operación\n"
            if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "\t-Formato de Hora Incorrecto\n"
            if (document.getElementById("<%= ddlVencimientoMes.ClientID %>").value == "")
                strMsjCampOblig += "\t-Vencimiento Mes\n"
            if (document.getElementById("<%= txtVencimientoAno.ClientID %>").value == "")
                strMsjCampOblig += "\t-Vencimiento Año\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
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

        function EnabledFechaOperacion(element) {
            var elem = element.id;
            if (document.getElementById(elem).checked == true) {
                document.getElementById("<%= tbFechaOperacion.ClientID %>").disabled = false;
                if (document.getElementById("<%= hdFechaOperacion.ClientID %>").value == "")
                    document.getElementById("<%= hdFechaOperacion.ClientID %>").value = document.getElementById("<%= tbFechaOperacion.ClientID %>").value;
            }
            else {
                document.getElementById("<%= tbFechaOperacion.ClientID %>").disabled = true;
                if (document.getElementById("<%= hdFechaOperacion.ClientID %>").value != "")
                    document.getElementById("<%= tbFechaOperacion.ClientID %>").innerText = document.getElementById("<%= hdFechaOperacion.ClientID %>").value;
            }
        }
    </script>
</head>
<body>
    <form id="frmInvocador" runat="server" method="post" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>--%>
            <div class="container-fluid">
                <header>
                    <div class="row">
                        <div class="col-md-9">
                            <h2>
                                <asp:Label ID="lblTitulo" runat="server">Orden de Inversión - OPERACIÓN FUTUROS</asp:Label></h2>
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
                                <asp:Label ID="lblFondo" runat="server">Portafolio</asp:Label></label>
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
                    <div class="col-md-4">
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
                                <asp:HiddenField ID="txtCodigoOrden" runat="server" />
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
                    <div class="col-md-4">
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
                        <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                    </div>
                </div>
                <br />
                <fieldset>
                    <legend>Características del Valor</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Número de Unidades</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblNumUnidades" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Margen Inicial</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblMargenInicial" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Fecha de Emisión</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblFechaEmision" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Margen Mantenimiento</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblMargenMantenimiento" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Fecha Vencimiento</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblFechaVencimiento" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Contract Zise</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblContractZise" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <asp:Button Text="Características" runat="server" ID="btnCaracteristicas" />
                        </div>
                    </div>
                </fieldset>
                <br />

                <fieldset>
                    <legend>Datos de Operación</legend>
                    <asp:UpdatePanel ID="up2" runat="server">
                        <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
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
                            <div class="col-md-3">
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
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label">
                                        Hora Operación</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" Width="100px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label">
                                        Hora Ejecución</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="tbHoraEjecucion" SkinID="Hour" Width="100px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Nro. Contratos Ordenados</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtNumConOrdenados" runat="server" Width="120px" MaxLength="12"
                                                CssClass="Numbox-7_12"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Nro. Contratos Operación</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtNumConOperacion" runat="server" Width="120px" MaxLength="12"
                                                CssClass="Numbox-7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Precio</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtPrecio" runat="server" Width="120px" MaxLength="12" ReadOnly="True"
                                                CssClass="Numbox-7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Monto Operación</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtMontoNominal" runat="server" Width="120px" MaxLength="12" CssClass="Numbox-7"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Mercado</label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlPlaza" runat="server" Width="120px" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Condición
                                        </label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlCondicion" runat="server" Width="120px" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
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
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label runat="server" id="lNPoliza" class="col-sm-3 control-label">
                                            Nro. Poliza</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="tbNPoliza" runat="server" Width="120px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Vencimiento</label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlVencimientoMes" runat="server" Enabled="False" Width="150px">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">
                                            Año</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtVencimientoAno" runat="server" Width="120px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Observación</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtObservacion" runat="server" Width="250px" TextMode="MultiLine"
                                                Rows="5" Style="text-transform: uppercase;"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row hidden" id="trMotivoCambio" runat="server">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">
                                            Motivo de cambio</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlMotivoCambio" runat="server" AutoPostBack="True" Width="280px">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            <asp:Label ID="lblComentarios" runat="server">Comentarios</asp:Label></label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtComentarios" runat="server" Width="350px" MaxLength="150" Height="42px"
                                                TextMode="MultiLine" Style="text-transform: uppercase;"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlIntermediario" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    
                </fieldset>
                <br />

                <fieldset>
                    <legend>Comisiones y Gastos de Administradora</legend>
                    <div class="grilla">
                        <asp:UpdatePanel ID="upGrilla" runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                                    <Columns>
                                        <asp:BoundField Visible="False" DataField="codigoComision1" HeaderText="C&#243;digo Impuesto/Comisi&#243;n" />
                                        <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisi&#243;n" />
                                        <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisi&#243;n" />
                                        <asp:BoundField Visible="False" DataField="strValorCalculadoComision1" HeaderText="Comisi&#243;n" />
                                        <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValorComision1" runat="server" CssClass="stlCajaTextoNumero"
                                                    Width="200px" MaxLength="23"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField Visible="False" DataField="ValorOcultoComision1" HeaderText="ValorOcultoComision1" />
                                        <asp:BoundField Visible="False" DataField="codigoComision2" HeaderText="C&#243;digo Impuesto/Comisi&#243;n" />
                                        <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisi&#243;n" />
                                        <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisi&#243;n" />
                                        <asp:BoundField Visible="False" DataField="strValorCalculadoComision2" HeaderText="Comisi&#243;n" />
                                        <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValorComision2" runat="server" CssClass="stlCajaTextoNumero"
                                                    Width="200px" MaxLength="23"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField Visible="False" DataField="ValorOcultoComision2" HeaderText="ValorOcultoComision2" />
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </fieldset>

                <br />
                <header>
                </header>
                <asp:UpdatePanel ID="up1" runat="server">
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
                                        Width="230px" ForeColor="Black" MaxLength="20" ReadOnly="True"></asp:TextBox>
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
                                <asp:Button runat="server" ID="btnLimites" Text="Limites" Visible="false" />
                                <asp:Button runat="server" ID="ibConsultaCertificados" Text="Consulta Certificados" Visible="false" />
                                <asp:Button runat="server" ID="btnLimitesParametrizados" Text="Consultar Limites Parametrizados"
                                    Visible="False" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-sm-12" style="text-align: right;">
                                <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" Visible="false" />
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

            <asp:HiddenField ID="hdPagina" runat="server" />
            <asp:HiddenField ID="hdCustodio" runat="server" />
            <asp:HiddenField ID="hdSaldo" runat="server" />
            <asp:HiddenField ID="hdNumUnidades" runat="server" />
            <asp:HiddenField ID="hdMensaje" runat="server" />
            <asp:HiddenField ID="hfModal" runat="server" />

      <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
    
    <input type="hidden" id="hdFechaOperacion" runat="server" name="hdFechaOperacion" />
    </form>
</body>
</html>
