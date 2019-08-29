<%@ Page Language="VB" AutoEventWireup="True" CodeFile="frmCertificadoDeposito.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_frmCertificadoDeposito" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Certificado de Depósito</title>
    <script type="text/javascript" language="javascript">
        function ValidarFondo() {
            strMensajeError = "";
            if (ValidaCamposFondo()) { return true; }
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
            } { return true; }
        }
        function Numeros() {
            tecla = window.event.keyCode
            if ((tecla >= 48 && tecla <= 57) || tecla == 32 || tecla == 46){ }
            else { window.event.keyCode = 0 }
        }
        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "txtPrecioNegSucio":
                    num = frmCertificadoDeposito.txtPrecioNegSucio.value; break;
                case "txtNroPapeles":
                    num = frmCertificadoDeposito.txtNroPapeles.value; break;
                case "txtMnomOp":
                    num = frmCertificadoDeposito.txtMnomOp.value; break;
                case "txtMontoOperacional":
                    num = frmCertificadoDeposito.txtMontoOperacional.value; break;
            }
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '0000'
                var tmp2 = tmp1.substr(0, 4);
                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '0000';
                    cents = cents.substr(0, 4);
                } else { cents = tmp2; }
                if (pos1 == -1) {
                    tmp2 = '0000';
                }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));
                switch (cajatexto) {
                    case "txtPrecioNegSucio":
                        frmCertificadoDeposito.txtPrecioNegSucio.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtNroPapeles":
                        frmCertificadoDeposito.txtNroPapeles.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtMnomOp":
                        frmCertificadoDeposito.txtMnomOp.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtMontoOperacional":
                        frmCertificadoDeposito.txtMontoOperacional.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                }
            }
            return false;
        }
        function formatCurrencyPorcentajesYMT(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "txtYTM":
                    num = frmCertificadoDeposito.txtYTM.value; break;
                case "txtPrecioNegoc":
                    num = frmCertificadoDeposito.txtPrecioNegoc.value; break;
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
                } else { cents = tmp2; }
                if (pos1 == -1) { tmp2 = '0000000'; }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));
                switch (cajatexto) {
                    case "txtYTM":
                        frmCertificadoDeposito.txtYTM.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtPrecioNegoc":
                        frmCertificadoDeposito.txtPrecioNegoc.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                }
            }
            return false;
        }
        function formatCurrencyPorcentajes(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "txtYTM":
                    num = frmCertificadoDeposito.txtYTM.value; break;
                case "txtPrecioNegoc":
                    num = frmCertificadoDeposito.txtPrecioNegoc.value; break;
            }
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '0000'
                var tmp2 = tmp1.substr(0, 4);
                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);
                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '0000';
                    cents = cents.substr(0, 4);
                }
                else { cents = tmp2; }
                if (pos1 == -1) {
                    tmp2 = '0000';
                }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));
                switch (cajatexto) {
                    case "txtYTM":
                        frmCertificadoDeposito.txtYTM.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "txtPrecioNegoc":
                        frmCertificadoDeposito.txtPrecioNegoc.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                }
            }
            return false;
        }
        function formatCurrencyAccionesOperacion(id) {
            formatearNumero(id);
            document.getElementById("<%=txtMnomOp.ClientID %>").value = document.getElementById("<%=txtMnomOrd.ClientID %>").value;
            return false;
        }
        function buscar() {
            strISIN = document.getElementById("txtISIN").value;
            winSelProd = document.open("BuscarValor.aspx?ISIN=" + strISIN, "winSelProd", "top=30,left=20,height=400,width=450,menubar=no,toolbar=no", true);
            winSelProd.focus();
        }
        function itemSelected(source, params) {
            if (source == 'LISTADO') {
                frmCertificadoDeposito.document.getElementById("txtISIN").value = params[0];
                frmCertificadoDeposito.document.getElementById("txtMnemonico").value = params[1];
                frmCertificadoDeposito.document.getElementById("txtSBS").value = params[2];
                frmCertificadoDeposito.document.getElementById("btnBuscar").click();
            }
            if (source == 'LISTADOORDENES') {
                frmCertificadoDeposito.document.getElementById("txtCodigoOrden").value = params[0];
                frmCertificadoDeposito.document.getElementById("txtISIN").value = params[1];
                frmCertificadoDeposito.document.getElementById("txtMnemonico").value = params[2];
                frmCertificadoDeposito.document.getElementById("ddlfondo").value = params[3];
                frmCertificadoDeposito.document.getElementById("lblMoneda").value = params[4];
                frmCertificadoDeposito.document.getElementById("ddlOperacion").value = params[5];
                frmCertificadoDeposito.document.getElementById("txtSBS").value = params[6];
                frmCertificadoDeposito.document.getElementById("btnBuscar").click();
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
            } else return false;
        }
        function cambiaCantidad() {
            frmCertificadoDeposito.txtMnomOp.value = frmCertificadoDeposito.txtMnomOrd.value;
            return false;
        }
        function cambiaTitulo() { return false; }
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
                if (confirmacion == true) { window.close(); } return false;
            }
            else {
                if (Pagina == "CONSULTA" || Pagina == "MODIFICA") {
                    window.close();
                    return false;
                } else return true;
            }
        }
        function Salida() {
            var strMensaje = "";
            var strAccion = "";
            strAccion = document.getElementById("hdMensaje").value
            var Pagina = document.getElementById("<%=hdPagina.ClientID %>").value

            if (strAccion != "") {
                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Certificados de Depósito?"
                if (strMensaje != "") {
                    confirmacion = alertify.confirm(strMensaje);
                    if (confirmacion == true) {
                        if (Pagina == "MODIFICA") { window.close(); } else { location.href = "../../../frmDefault.aspx"; }
                    } return false;
                } else { return true; }
            } else { location.href = "../../../frmDefault.aspx"; }
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
                if (confirmacion == true) { return true; }
                else { return false; }
            } else { return true; }
        }
        function Validar() {
            strMensajeError = "";
            if (ValidaCampos()) { return true; }
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
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Operación<br>"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Vencimiento<br>"
            if (document.getElementById("<%= txtMnomOrd.ClientID %>").value == "")
                strMsjCampOblig += "-Monto Nominal Ordenado<br>"
            if (document.getElementById("<%= txtMnomOp.ClientID %>").value == "")
                strMsjCampOblig += "-Monto Nominal Operación<br>"
            if (document.getElementById("<%= txtYTM.ClientID %>").value == "")
                strMsjCampOblig += "-YTM<br>"
            if (document.getElementById("<%= txtPrecioNegoc.ClientID %>").value == "")
                strMsjCampOblig += "-Precio Negociación<br>"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "-Intermediario<br>"
            if (document.getElementById("<%= ddlTipoTasa.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Tasa<br>"
            if (document.getElementById("<%= txtPrecioNegSucio.ClientID %>").value == "")
                strMsjCampOblig += "-Precio Negociación Sucio<br>"
            if (document.getElementById("<%= txtNroPapeles.ClientID %>").value == "")
                strMsjCampOblig += "-Número Papeles<br>"
            if (document.getElementById("<%= txtHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Hora Operación<br>"
            if (!EsHoraValida(document.getElementById("<%= txtHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            }{ return true; }
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
    <form id="frmCertificadoDeposito" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" runat="server">Orden de Inversión - Certificado de Depósito</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3><asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
        </header>
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta| 30/05/18 --%>
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
        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta| 30/05/18 --%>
        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 30/05/18 --%>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
            </div>
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button runat="server" ID="btnCaracteristicas" Text="Características" />
            </div>
        </div>
        <br />
        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 30/05/18 --%>
        <fieldset>
            <legend>Datos de Operación</legend>
            <asp:UpdatePanel runat="server" ID="updDatos">
                <ContentTemplate>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora" que no se utilizará a sección oculta | 30/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Operación</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="tbFechaOperacion" runat="server" Width="104px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Fecha Liquidación</label>
                                <div class="col-sm-5">
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
                        <div class="col-md-4">                        
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    YTM %</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtYTM" runat="server" CssClass="Numbox-7" Width="104px" ReadOnly="true" /></div>
                            </div>
                        </div>                    
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">Nro Papeles</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtNroPapeles" runat="server" CssClass="Numbox-7_12" Width="150px" ReadOnly="true" /></div>
                            </div>
                        </div>
                        
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">Valor Nominal Unidad</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="lblUnidades" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                                            
                    
                    </div>



                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora" que no se utilizará a sección oculta | 30/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Monto Nominal Operación" que no se utilizará a sección oculta | 30/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Tipo Tasa</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlTipoTasa" runat="server" Enabled="False" Width="150px" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">Monto Nominal Total</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtMnomOrd" CssClass="Numbox-7" runat="server" Width="150px" ReadOnly="true" onblur="cambiaCantidad();" /></div>
                            </div>
                        </div>
                    </div>


                    <div class="row" id="pnlTirNeto" runat="server" visible="false" >
                        <div class="col-sm-4">
                            <div class="form-group" >
                                <label class="col-sm-6 control-label">TIR Neto</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtTIRNeto" Width="150px" MaxLength="200" CssClass="Numbox-7_22" ReadOnly="true" />
                                </div>
                            </div>                                                 
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">

                            </div>
                        </div>
                    </div>

                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Monto Nominal Operación" que no se utilizará a sección oculta | 30/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Calculado %" que no se utilizará a sección oculta | 30/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">Precio (Sucio) %</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtPrecioNegoc" CssClass="Numbox-7" runat="server" Width="150px"
                                        ReadOnly="true" /></div>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Precio (Limpio) %</label>
                                <div class="col-sm-5">
                                    <asp:TextBox runat="server" ID="txtPrecioLimpio" Width="150px" CssClass="Numbox-7_12" ReadOnly="True" /></div>
                            </div>
                        </div>

                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Calculado %" que no se utilizará a sección oculta | 30/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Negociación Sucio" que no se utilizará a sección oculta | 30/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Monto Operación</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtMontoOperacional" CssClass="Numbox-7" runat="server" Width="104px"
                                        ReadOnly="true" /></div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">                                
                                <label class="col-sm-7 control-label">Interés Corrido</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtCorrido" Text="0" runat="server" Width="150px" CssClass="Numbox-7"
                                        ReadOnly="true" /></div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Negociación Sucio" que no se utilizará a sección oculta | 30/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación", "No Liquida en Caja", "Regularización SBS" y "I. Corrido" que no se utilizará a sección oculta | 30/05/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Intermediario</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlIntermediario" runat="server" Enabled="False" AutoPostBack="True"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Categoría Contable</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList runat="server" ID="ddlCategoriaContable" Enabled="false" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3" id="divMercado" style="display:none">
                            <div class="form-group">
                                <label class="col-sm-7 control-label">
                                    Bolsa</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList runat="server" ID="ddlPlaza" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación", "No Liquida en Caja", "Regularización SBS" y "I. Corrido" que no se utilizará a sección oculta | 30/05/18 --%>
                    <div class="row" id="trMotivoCambio" style="display: none">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Motivo de cambio</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label runat="server" id="lblComentarios" class="col-sm-7 control-label">
                                    Comentarios</label>
                                <div class="col-sm-5">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="4" Width="250px" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row" id="DivObservacion" runat="server">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="Label2" class="col-sm-6 control-label">
                                    Observación Carta</label>
                                <div class="col-sm-6">
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
<%--                        <div class="col-sm-4" runat="server" id="DivDatosCarta">
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
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
<%--        <div class="row">
            <asp:CheckBox ID="chkRecalcular" Text="Recalcular" runat="server" Checked="True" Enabled = "false" />
        </div>--%>
        <fieldset>
            <legend>Comisiones y Gastos de Administradora</legend>
            <asp:UpdatePanel runat="server" ID="updgrilla">
                <ContentTemplate>
                    <div class="row">
                        <asp:GridView ID="dgLista" SkinID="Grid" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="codigoComision2">
                            <Columns>
                                <asp:BoundField DataField="codigoComision1" HeaderText="C&#243;digo Impuesto/Comisi&#243;n">
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisi&#243;n">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisi&#243;n">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="strValorCalculadoComision1"
                                    HeaderText="Comisi&#243;n"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision1" runat="server" CssClass="Numbox-7" Width="200px"
                                            MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="ValorOcultoComision1"
                                    HeaderText="ValorOcultoComision1"></asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="codigoComision2"
                                    HeaderText="C&#243;digo Impuesto/Comisi&#243;n"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisi&#243;n">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisi&#243;n">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="strValorCalculadoComision2"
                                    HeaderText="Comisi&#243;n"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision2" runat="server" CssClass="Numbox-7" Width="200px"
                                            MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" DataField="ValorOcultoComision2"
                                    HeaderText="ValorOcultoComision2"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <asp:UpdatePanel runat="server" ID="updComisiones">
                <ContentTemplate>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Total Comisiones</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txttotalComisionesC" runat="server" Width="200px" ReadOnly="True"
                                    CssClass="Numbox-7" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-6 control-label">
                                Monto Neto Operación</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtMontoNetoOpe" runat="server" Width="200px" CssClass="Numbox-7" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="col-md-3">
                <div class="form-group">
                    <div class="col-sm-12" style="text-align: right;">
                        <asp:Button runat="server" ID="btnProcesar" Text="Procesar" OnClientClick="return Validar();" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnCuponera" Text="Cuponera" />
                        <!-- <asp:Button runat="server" ID="btnLimites" Text="Limites" />-->
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
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <div class="col-sm-12" style="text-align: right;">
                                <asp:Button runat="server" ID="btnRetornar" Text="Salir" Visible="False" />
                                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" UseSubmitBehavior="False" />
                                <asp:Button runat="server" ID="btnSalir" Text="Salir" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <asp:HiddenField ID="hdCodigoTipoCupon" runat="server" />
     <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 30/05/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS</legend>
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
        </div>
        <br />
        <fieldset>
            <legend>Características del Valor</legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Descripción Instrumento</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lbldescripcion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Fecha Fin Bono</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblfecfinbono" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Nominales emitidos</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblnominales" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
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
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Emisor</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblEmisor" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            % Participación</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblParticipacion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Base Cupón</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblBaseCupon" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Duración</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblduracion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Precio Vector</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblPrecioVector" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Base Tir</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblBaseTir" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Rescate</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblRescate" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Hora Operación</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtHoraOperacion" runat="server" SkinID="Hour" Width="104px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label class="col-sm-7 control-label">
                        Monto Nominal Operación</label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtMnomOp" CssClass="Numbox-7" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label class="col-sm-7 control-label">
                        Precio Calculado %</label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="lblPrecioCal" runat="server" CssClass="Numbox-4" Width="104px" ReadOnly="true" /></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Precio Negociación Sucio %</label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtPrecioNegSucio" CssClass="Numbox-7" runat="server" Width="104px"
                            ReadOnly="true" /></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label class="col-sm-7 control-label">
                        Contacto</label>
                    <div class="col-sm-5">
                        <asp:DropDownList ID="ddlContacto" runat="server" Enabled="False" Width="150px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <label class="col-sm-3 control-label">
                        Observación</label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtObservacion" Style="text-transform: uppercase" runat="server"
                            Width="656px" ReadOnly="true" /></div>
                </div>
            </div>
        </div>
      
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkFicticia" runat="server" Enabled="False" Text="No Liquida en Caja">
                        </asp:CheckBox></div>
                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkRegulaSBS" runat="server" Enabled="False" Text="Regularización SBS">
                        </asp:CheckBox></div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 30/05/18 --%>
    </form>
</body>
</html>
