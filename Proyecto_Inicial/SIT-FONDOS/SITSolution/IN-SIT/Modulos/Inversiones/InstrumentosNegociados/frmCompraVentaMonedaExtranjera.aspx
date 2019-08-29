<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCompraVentaMonedaExtranjera.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmCompraVentaMonedaExtranjera" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Forward de Divisas</title>
    <script type="text/javascript">
        function ValidarFondo() {
            strMensajeError = "";
            if (ValidaCamposFondo()) {
                return true;
            }
            else {
                alertify.alert('<b>' + strMensajeError + '</b>');
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

        function itemSelected(source, params) {
            if (source == 'LISTADOORDENES') {
                document.getElementById("txtCodigoOrden").value = params[0];
                document.getElementById("ddlfondo").value = params[3];
                document.getElementById("ddlOperacion").value = params[5];
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
            return alertify.confirm(mensaje);
        }

        function cambiaTitulo() {
            document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - COMPRA / VENTA DE MONEDA EXTRANJERA';
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
                confirmacion = alertyify.confirm(strMensajeConfirmacion);
                if (confirmacion == true) {
                    window.close();
                }
                return false;
            }
            {
                return true;
            }
        }

        function Salida() {
            var strMensaje = "";
            var strAccion = "";
            var strOI = "";

            strAccion = document.getElementById("hdMensaje").value

            if (strAccion != "") {
                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Compra/Venta de Moneda Extranjera?";

                //                if (document.getElementById("ddlFondo").value != 'MULTIFONDO') {
                //                    strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Compra/Venta de Moneda Extranjera?"
                //                }
                //                else {
                //                    strMensaje = "¿Desea cancelar " + strAccion + " de Pre-Orden de Inversión de Compra/Venta de Moneda Extranjera?"
                //                }

                if (strMensaje != "") {
                    confirmacion = alertyify.confirm(strMensaje);
                    if (confirmacion == true) {
                        location.href = "../../../frmDefault.aspx";
                    }
                    return false;
                }
                {
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
                confirmacion = alertyify.confirm(strMensajeConfirmacion);
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
                alertify.alert('<b>' + strMensajeError + '</b>');
                return false;
            }
        }

        function ValidaCampos() {
            var strMsjCampOblig = "";
            var src = window.event.srcElement;
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "-Portafolio<br>"
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Operación<br>"
            if (document.getElementById("<%= ddlMoneda.ClientID %>").value == "")
                strMsjCampOblig += "-Moneda<br>"
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Operación<br>"
            if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Hora Operación<br>"
            if (document.getElementById("<%= txtTipoCambio.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Cambio<br>"
            else if (document.getElementById("<%= txtTipoCambio.ClientID %>").value <= 0)
                strMsjCampOblig += "-Tipo Cambio<br>"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Liquidación<br>"
            if (document.getElementById("<%= ddlMonedaDestino.ClientID %>").value == "")
                strMsjCampOblig += "-Moneda Destino<br>"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "-Código Intermediario<br>"
            if (document.getElementById("<%= ddlAfectaFlujoCaja.ClientID %>").value == "--SELECCIONE--")
                strMsjCampOblig += "-Flujo de caja<br>"
            if (src.id == "btnAceptar") {
                if (document.getElementById("<%= txtMontoOrigen.ClientID %>").value <= 0)
                    strMsjCampOblig += "-Monto Divisa Negociada<br>"
                if (document.getElementById("<%= txtMontoDestino.ClientID %>").value <= 0)
                    strMsjCampOblig += "-Monto Destino<br>"
            }
            else if (document.getElementById("<%= txtMontoOrigen.ClientID %>").value <= 0 &&
				document.getElementById("<%= txtMontoDestino.ClientID %>").value <= 0) {
                strMsjCampOblig += "-Monto Divisa Negociada<br>"
                strMsjCampOblig += "-Monto Destino<br>"
            }
            if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "-Formato de Hora Incorrecto<br>"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</head>
<body onload="javascript:cambiaTitulo();">
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-sm-9">
                    <h2>
                        <asp:Label Text="Orden de Inversión - COMPRA / VENTA DE MONEDA EXTRANJERA" runat="server"
                            ID="lblTitulo"></asp:Label></h2>
                </div>
                <div class="col-sm-3" style="text-align: right;">
                    <h2>
                        <asp:Label Text="" runat="server" ID="lblAccion" /></h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label runat="server" id="lblFondo" class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="120px" OnChange="javascript:cambiaTitulo();"
                                AutoPostBack="true" Enabled="False" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operación
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="210px" Enabled="False" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Divisa Negociada</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="180px" Enabled="False" />
                        </div>
                    </div>
                </div>
            </div>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se deja solo botón Buscar| 29/05/18 --%>
            <div class="row">
                <div class="col-sm-12" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se deja solo botón Buscar| 29/05/18 --%>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos de Operación</legend>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Operación</label>
                                <div class="col-sm-7">
                                    <div runat="server" id="imgFechaOperacion" class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Enabled="false" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fecha Liquidación</label>
                                <div class="col-sm-8">
                                    <div runat="server" id="imgFechaVcto" class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 29/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "SPOT PH" con radio button "SI" y "NO"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Divisa Negociada</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtMontoOrigen" Width="150px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Cambio
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtTipoCambio" Width="150px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "SPOT PH" con radio button "SI" y "NO"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Moneda</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlMonedaDestino" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Monto</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtMontoDestino" Width="150px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                        </div>
                    </div>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación", "Flujo de Caja","No Liquida en Caja" y "Regulariación SBS"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Intermediario</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlIntermediario" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>            
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Observación", "Flujo de Caja","No Liquida en Caja" y "Regulariación SBS"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row hidden" runat="server" id="trMotivoCambio">
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
                                <label runat="server" id="lblComentarios" class="col-sm-4 control-label">
                                    Comentarios</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="2" Width="300px"
                                        Style="text-transform: uppercase;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlIntermediario" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
            <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-sm-6">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Modificar" runat="server" ID="btnModificar" />
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" />
                <asp:Button Text="Consultar" runat="server" ID="btnConsultar" />
            </div>
            <div class="col-sm-6" style="text-align: right;">
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <asp:Button Text="Salir" runat="server" ID="btnRetornar" Visible="False" />
                        <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" UseSubmitBehavior="false" />
                        <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <input runat="server" type="hidden" id="hdMensaje" name="hdMensaje" />
    <input runat="server" type="hidden" id="txtCodigoOrden" name="txtCodigoOrden" />
    <input runat="server" type="hidden" id="hdPagina" name="hdPagina" />
    <input runat="server" type="hidden" id="hdSaldo" name="hdSaldo" />
    <input runat="server" type="hidden" id="hdCustodio" name="hdCustodio" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS </legend>
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <label runat="server" id="lblFondoDestino" class="col-sm-4 control-label" visible="False">
                        Portafolio Destino
                    </label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlFondoDestino" runat="server" OnChange="javascript:cambiaTitulo();"
                            Enabled="False" Visible="False">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Hora Operación</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Spot PH
                    </label>
                    <div class="col-sm-8">
                        <asp:RadioButtonList runat="server" ID="rblspot" RepeatDirection="Horizontal">
                            <asp:ListItem Value="S" Text="SI" />
                            <asp:ListItem Value="N" Text="NO" />
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Contacto</label>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlContacto" Width="150px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Observación</label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="txtObservacion" Width="280px" Style="text-transform: uppercase" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Flujo de Caja</label>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlAfectaFlujoCaja" Width="150" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <div class="col-sm-4">
                    </div>
                    <div class="col-sm-8">
                        <asp:CheckBox Text="No Liquida en Caja" runat="server" ID="chkFicticia" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <div class="col-sm-4">
                    </div>
                    <div class="col-sm-8">
                        <asp:CheckBox Text="Regulariación SBS" runat="server" ID="chkRegulaSBS" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label hidden" id="lNPoliza" runat="server">
                        Nro. Poliza</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="tbNPoliza" Width="150px" Visible="False" />
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    </form>
</body>
</html>