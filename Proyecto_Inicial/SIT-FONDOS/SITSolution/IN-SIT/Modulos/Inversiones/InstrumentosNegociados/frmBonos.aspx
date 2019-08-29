<%@ Page Language="VB" AutoEventWireup="True" CodeFile="frmBonos.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmBonos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Bonos</title>
    <style type="text/css">
        .oculto
        {
            display: none;
        }
    </style>
    <script type="text/javascript">

        // -'INICIO | ZOLUXIONES | CRumiche | ProyFondosII - Calculos a ddlPlaza de Cantidad de Instrumentos

        $(document).ready(function () {
            $("#<%=txtNroPapeles.ClientID %>").change(function () {
                var cantidad = parseFloat($("#<%= txtNroPapeles.ClientID %>").val().replace(/,/g, ""));
                var valorUnit = parseFloat($("#<%= lblUnidades.ClientID %>").val().replace(/,/g, ""));

                $("#<%= txtMnomOrd.ClientID %>").val(cantidad * valorUnit);
                $("#<%= txtMnomOrd.ClientID %>").blur();
            });

        });

        // -'FIN | ZOLUXIONES | CRumiche | ProyFondosII - Calculos a nivel de Cantidad de Instrumentos

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

        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "txtPrecioNegSucio":
                    num = frmInvocador.txtPrecioNegSucio.value; break;
                case "txtInteresCorNeg":
                    num = frmInvocador.txtInteresCorNeg.value; break;
                case "txtNroPapeles":
                    num = frmInvocador.txtNroPapeles.value; break;
                case "txtMnomOp":
                    num = frmInvocador.txtMnomOp.value; break;
            }

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

                switch (cajatexto) {
                    case "txtPrecioNegSucio":
                        frmInvocador.txtPrecioNegSucio.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtInteresCorNeg":
                        frmInvocador.txtInteresCorNeg.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtNroPapeles":
                        frmInvocador.txtNroPapeles.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtMnomOp":
                        frmInvocador.txtMnomOp.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                }

            }
            return false;
        }


        function formatCurrencyPorcentajes(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "txtYTM":
                    num = frmInvocador.txtYTM.value; break;
                case "txtPrecioNegoc":
                    num = frmInvocador.txtPrecioNegoc.value; break;
            }

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

                switch (cajatexto) {
                    case "txtYTM":
                        frmInvocador.txtYTM.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtPrecioNegoc":
                        frmInvocador.txtPrecioNegoc.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                }
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


                frmInvocador.txtMontoOperacional.value = (((sign) ? '' : '-') + num + '.' + tmp2);
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


                frmInvocador.txtMnomOrd.value = (((sign) ? '' : '-') + num + '.' + tmp2);
                frmInvocador.txtMnomOp.value = (((sign) ? '' : '-') + num + '.' + tmp2);
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
                return alertify.confirm(mensaje);
            }
            else
                return false;
        }
        function cambiaCantidad(num) {
            if (num != "") {
                var txtMnomOrd = "";
                txtMnomOrd = num;
                //txtMnomOrd = replaceAll(txtMnomOrd, ",", "");
                txtMnomOrd = txtMnomOrd.replace(/,/g, "");  //'OT10965 - 01/12/2017 - Ian Pastor M.
                txtMnomOrd = parseFloat(txtMnomOrd);

                $('#txtMnomOp').val(txtMnomOrd);
            }
            //frmInvocador.txtMnomOp.value = frmInvocador.txtMnomOrd.value;
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
                confirmacion = alertify.confirm(strMensajeConfirmacion);
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
                    strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Bonos?"
                }
                else {
                    strMensaje = "¿Desea cancelar " + strAccion + " de Pre-Orden de Inversión de Bonos?"
                }

                if (strMensaje != "") {
                    confirmacion = alertify.confirm(strMensaje);
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
                confirmacion = alertify.confirm(strMensajeConfirmacion);
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
                strMsjCampOblig += "-Portafolio<br>";
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Operación<br>";
            if (document.getElementById("<%= txtISIN.ClientID %>").value == "")
                strMsjCampOblig += "-Código ISIN<br>";
            if (document.getElementById("<%= txtMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "-Código Mnemónico<br>";
            if (document.getElementById("<%= txtSBS.ClientID %>").value == "")
                strMsjCampOblig += "-Código SBS<br>";
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Operación<br>";
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Vencimiento<br>";
            if (document.getElementById("<%= txtMnomOrd.ClientID %>").value == "")
                strMsjCampOblig += "-Monto Nominal Ordenado<br>";
            if (document.getElementById("<%= txtMnomOp.ClientID %>").value == "")
                strMsjCampOblig += "-Monto Nominal Operación<br>";
            if (document.getElementById("<%= ddlTipoTasa.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Tasa<br>";
            if (document.getElementById("<%= txtPrecioNegoc.ClientID %>").value == "")
                strMsjCampOblig += "-Precio Negociación<br>";
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "-Intermediario<br>";
            if (document.getElementById("<%= txtPrecioNegSucio.ClientID %>").value == "")
                strMsjCampOblig += "-Precio Negociación Sucio<br>";
            if (document.getElementById("<%= txtNroPapeles.ClientID %>").value == "")
                strMsjCampOblig += "-Número Papeles<br>"
            if (document.getElementById("<%= txtHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Hora Operación<br>";
            if (!EsHoraValida(document.getElementById("<%= txtHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "-Formato de Hora Incorrecto<br>";
            if (document.getElementById("<%= ddlPlaza.ClientID %>").value == "")
                strMsjCampOblig += "-Bolsa<br>";
            if (document.getElementById("<%= ddlGrupoInt.ClientID %>").value == "")
                strMsjCampOblig += "-Grupo de Intermediarios<br>";
            if ((document.getElementById("<%= lblMoneda.ClientID %>").innerText != "" & document.getElementById("<%= lblMonedaDestino.ClientID %>").innerText != "") &
                (document.getElementById("<%= lblMoneda.ClientID %>").innerText != document.getElementById("<%= lblMonedaDestino.ClientID %>").innerText))
                if (document.getElementById("<%= tbFixing.ClientID %>").value == "")
                    strMsjCampOblig += "-Fixing";

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            }
            {
                return true;
            }
        }

        function replaceAll(text, busca, reemplaza) {
            while (text.toString().indexOf(busca) != -1)
                text = text.toString().replace(busca, reemplaza);
            return text;
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
                <div class="col-sm-6">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Orden de Inversión - BONOS</asp:Label></h2>
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <br />
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblFondo" runat="server">Portafolio</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:UpdatePanel ID="UPFondo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlFondo" runat="server" Enabled="False" AutoPostBack="True"
                                    Width="120px" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Operación</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlOperacion" runat="server" Enabled="False" Width="120px" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Moneda</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="lblMoneda" runat="server" Width="120px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" y se coloca campo "Emisor" que no se utilizará a sección oculta| 31/05/18 --%>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código Isin</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtISIN" runat="server" Width="120px" MaxLength="12" CssClass="mayusculas" />
                        <asp:HiddenField ID="txtCodigoOrden" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código Mnemónico</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtMnemonico" runat="server" Width="120px" MaxLength="15" CssClass="mayusculas" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Emisor</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="lblEmisor" runat="server" Width="120px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" y se coloca campo "Emisor" que no se utilizará a sección oculta| 31/05/18 --%>
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 31/05/18 --%>
        <div class="row">
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
            </div>
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button runat="server" ID="btnCaracteristicas" Text="Características" />
            </div>
        </div>
        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 31/05/18 --%>
        <br />
        <fieldset>
            <legend>Datos de Operación</legend>
            <asp:UpdatePanel runat="server" ID="updDatosOperaciones">
                <ContentTemplate>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 31/05/18 --%>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Operación</label>
                                <div class="col-sm-7">
                                    <div class="input-append">
                                        <asp:TextBox runat="server" ID="tbFechaOperacion" Width="100px" ReadOnly="True" />
                                    </div>
                                    <asp:CheckBox ID="chkEmisionPrimaria" runat="server" CssClass="stlPaginaTexto" Visible="False"
                                        Text="" onclick="javascript:EnabledFechaOperacion(this)"></asp:CheckBox>
                                    <asp:HiddenField ID="hdFechaOperacion" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Liquidación</label>
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
                    </div>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Modo Negociación</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlModoNegociacion" runat="server" Width="150px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <%--<label class="col-sm-5 control-label">YTM % <h class="requeridob">*</h> </label>--%>
                                <label class="col-sm-6 control-label">
                                    YTM %</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtYTM" runat="server" CssClass="Numbox-7_12" Width="100px" /></div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 31/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Monto Nominal Operación" que no se utilizará a sección oculta| 31/05/18 --%>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Nro. Papeles</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtNroPapeles" Width="150px" ReadOnly="true" MaxLength="12"
                                        CssClass="Numbox-7_12" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Valor Nominal Unidad</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="lblUnidades" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Tipo Tasa</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlTipoTasa" runat="server" Enabled="False" Width="150px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Monto Nominal Total</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtMnomOrd" Width="150px" MaxLength="200" CssClass="Numbox-7_22"
                                        onblur="Javascript:cambiaCantidad(this.value);" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group" id="pnlTirNeto" runat="server" visible="false">
                                <label class="col-sm-5 control-label">
                                    TIR Neto</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtTIRNeto" Width="150px" MaxLength="200" CssClass="Numbox-7_22"
                                        ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Saldo Nominal Vigente</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtMontoNominalVigente" Width="150px" MaxLength="200"
                                        CssClass="Numbox-7_22" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="pnlValoresVAC" visible="false">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    VAC Emisión</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtVACEmision" Width="150px" MaxLength="200" CssClass="Numbox-7_22"
                                        ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    VAC Fecha Operación</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtVACActual" Width="150px" MaxLength="200" CssClass="Numbox-7_22"
                                        ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Monto Nominal Operación" que no se utilizará a sección oculta| 31/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Calculado %" que no se utilizará a sección oculta| 31/05/18 --%>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Precio (Sucio) %</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtPrecioNegoc" Width="150px" CssClass="Numbox-7_12" /></div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Precio (Limpio) %</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtPrecioLimpio" Width="150px" CssClass="Numbox-7_12"
                                        ReadOnly="True" /></div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Calculado %" que no se utilizará a sección oculta| 31/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Negociación Sucio %" y "Interés Corrido" que no se utilizará a sección oculta| 31/05/18 --%>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Operación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtMontoOperacional" runat="server" Width="150px" MaxLength="20"
                                        CssClass="Numbox-7" /></div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Interés Corrido</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtInteresCorNeg" runat="server" Width="150px" MaxLength="12" CssClass="Numbox-7_12" /></div>
                            </div>
                        </div>
                        <br />
                    </div>
                    <div class="row" runat="server" id="pnlInteresAjustado" visible="false">
                        <div class="col-sm-5">
                            <div class="form-group">
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Interés Corrido Ajustado</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtInteresAjustado" runat="server" Width="150px" MaxLength="12"
                                        CssClass="Numbox-7_12" ReadOnly="true" /></div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó botón "Ejecución" que no se utilizará a sección oculta| 31/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Fixing" y "Nro.Poliza" que no se utilizará a sección oculta| 31/05/18 --%>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Bolsa</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlPlaza" runat="server" Enabled="False" AutoPostBack="True"
                                        Width="150px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Categoría Contable</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlTipoValorizacion" runat="server" Enabled="False" AutoPostBack="True"
                                        Width="150px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Fixing" y "Nro.Poliza" que no se utilizará a sección oculta| 31/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación","No Liquida en Caja" y "Regularización SBS" que no se utilizará a sección oculta| 31/05/18 --%>
                    <div class="row">
                        <div class="col-sm-5">
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
                        <div class="col-sm-4">
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
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación","No Liquida en Caja" y "Regularización SBS" que no se utilizará a sección oculta| 31/05/18 --%>
                    <div class="row" id="trMotivoCambio" style="display: none">
                        <div class="col-sm-5">
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
                                <label runat="server" id="lblComentarios" class="col-sm-6 control-label">
                                    Comentarios</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="4" Width="250px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="DivObservacion" runat="server">
                        <div class="col-sm-5">
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
                                <label class="col-sm-5 control-label">
                                    Datos Cartas</label>
                                <div class="col-sm-7">
                                    <asp:LinkButton ID="lkbMuestraModalDatos" runat="server" OnClientClick="javascript:ShowDatosCarta();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:CheckBox ID="chkRecalcular" Text="Recalcular" runat="server" Checked="True" />
                                </div>
                            </div>
                        </div>
                       <%-- <div class="col-sm-4" runat="server" id="DivDatosCarta">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Datos Cartas</label>
                                <div class="col-sm-7">
                                    <asp:LinkButton ID="lkbMuestraModalDatos" runat="server" OnClientClick="javascript:ShowDatosCarta();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>--%>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset>
            <legend>Comisiones y gastos de Administradora</legend>
            <div class="grilla">
                <asp:UpdatePanel ID="upGrilla" runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server" SkinID="Grid" ID="dgLista" DataKeyNames="codigoComision2">
                            <Columns>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="codigoComision1"
                                    HeaderText="Código Impuesto/Comisión"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisión">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisión">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="strValorCalculadoComision1"
                                    HeaderText="Comisión"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision1" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="ValorOcultoComision1"
                                    HeaderText="ValorOcultoComision1"></asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="codigoComision2"
                                    HeaderText="Código Impuesto/Comisión"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisión">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisión">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="strValorCalculadoComision2"
                                    HeaderText="Comisión"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision2" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="ValorOcultoComision2"
                                    HeaderText="ValorOcultoComision2"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <asp:UpdatePanel ID="updMontos" runat="server">
                <ContentTemplate>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Total Comisiones</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txttotalComisionesC" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-5">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Monto Neto Operación</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtMontoNetoOpe" runat="server" CssClass="stlCajaBloqueadoNumero"
                                    Width="200px" ForeColor="Black" MaxLength="20" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <div class="col-sm-12" style="text-align: right;">
                                <asp:Button runat="server" ID="btnProcesar" Text="Procesar" OnClientClick="return Validar();" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="tblDestino" runat="server" class="row" style="visibility: hidden">
            <asp:UpdatePanel runat="server" ID="updDestino">
                <ContentTemplate>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Comisiones
                                <asp:Label ID="lblMDest" runat="server"></asp:Label></label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtComisionesDestino" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-5">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Monto Operación
                                <asp:Label ID="lblMDest2" runat="server"></asp:Label></label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtMontoOperacionDestino" runat="server" Width="200px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-sm-8">
                <div class="form-group">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnCuponera" Text="Cuponera" Visible="False" />
                        <asp:Button runat="server" ID="btnLimites" Text="Limites" Visible="False" />
                        <asp:Button runat="server" ID="btnLimitesParametrizados" Text="Consultar Limites Parametrizados"
                            Visible="False" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
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
            <div class="col-sm-8">
                <div class="form-group">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnIngresar" Text="Ingresar" />
                        <asp:Button runat="server" ID="btnModificar" Text="Modificar" />
                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" />
                        <asp:Button runat="server" ID="btnConsultar" Text="Consultar" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <div class="col-sm-12" style="text-align: right;">
                                <asp:Button runat="server" ID="btnRetornar" Text="Salir" Visible="False" />
                                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" UseSubmitBehavior="false" />
                                <asp:Button runat="server" ID="btnSalir" Text="Salir" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <asp:HiddenField ID="hdTipoFondo" runat="server" />
    <asp:Button ID="btnModal" runat="server" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 12/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 12/06/18 --%>
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 31/05/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS</legend>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código SBS</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtSBS" runat="server" Width="120px" MaxLength="12" CssClass="mayusculas"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
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
            <legend>Características del Valor</legend>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Descripción Instrumento</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lbldescripcion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Fecha Fin Bono</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblfecfinbono" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Nominales emitidos</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblnominales" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Saldo</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblSaldoValor" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Base calculo Dif. Dias</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="LBLBase" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Base Cupón</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblBaseCupon" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Rescate</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblRescate" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Fecha Ult. Cupón</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblFecUltCupon" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Precio Vector</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblPrecioVector" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Base Tir</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblBaseTir" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Moneda Destino</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblMonedaDestino" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Fecha Prox. Cupón</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblFecProxCupon" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Duración</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblDuracion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Hora Operación</label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="txtHoraOperacion" SkinID="Hour" Width="100px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Monto Nominal Operación</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtMnomOp" Width="100px" MaxLength="20" CssClass="Numbox-7_22" /></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Precio Calculado %</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="lblPrecioCal" Width="100px" ReadOnly="true" /></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Precio Negociación Sucio %</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtPrecioNegSucio" runat="server" Width="100px" MaxLength="12" CssClass="Numbox-7"></asp:TextBox>
                        <%--onblur="formatCurrency(frmInvocador.txtPrecioNegSucio.id);"--%>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Interés Corrido Negociado</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="lblInteresCorrido" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                    </label>
                    <div class="col-sm-7">
                        <asp:Button runat="server" ID="imbEjecucion" Text="Ejecución" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Contacto</label>
                    <div class="col-sm-7">
                        <asp:DropDownList ID="ddlContacto" runat="server" Enabled="False" Width="150px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Observación</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtObservacion" runat="server" Style="text-transform: uppercase;"
                            Width="150px" MaxLength="20" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <div class="form-group">
                    <label class="col-sm-5 control-label" style="margin-left: -4px;">
                        <asp:CheckBox ID="chkFicticia" Style="margin-left: 1px;" runat="server" Enabled="False"
                            Text="No Liquida en Caja"></asp:CheckBox>
                    </label>
                    <div class="col-sm-7 control-label" style="text-align: left;">
                        <asp:CheckBox ID="chkRegulaSBS" runat="server" Text="Regularización SBS"></asp:CheckBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        <asp:Label ID="lbFixing" runat="server">Fixing</asp:Label>
                    </label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="tbFixing" runat="server" Width="100px" MaxLength="20" onblur="javascript: FormateaDecimal(this,4);"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        <asp:Label ID="lNPoliza" runat="server" Visible="False">Nro.Poliza</asp:Label></label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="tbNPoliza" runat="server" Width="100px" MaxLength="15" Visible="False"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 31/05/18 --%>
    </form>
</body>
</html>
