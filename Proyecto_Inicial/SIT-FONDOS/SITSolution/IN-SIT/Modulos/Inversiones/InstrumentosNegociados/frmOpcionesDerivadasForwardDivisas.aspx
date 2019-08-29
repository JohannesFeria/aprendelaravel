<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOpcionesDerivadasForwardDivisas.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmOpcionesDerivadasForwardDivisas" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Forward de Divisas</title>
    <style type="text/css">
        .oculto
        {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ValidarFondo() {
            strMensajeError = "";
            if (ValidaCamposFondo()) { return true; }
            else { alertify.alert(strMensajeError); return false; }
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
        function DiasPlazo() {
            tecla = window.event.keyCode
            if ((tecla >= 48 && tecla <= 57) || tecla == 32) { }
            else { window.event.keyCode = 0 }
        }
        function Numero() {
            tecla = window.event.keyCode
            if ((tecla >= 48 && tecla <= 57) || tecla == 32 || tecla == 46) { }
            else {
                window.event.keyCode = 0
            }
        }
        function buscar() {
            strISIN = document.getElementById("txtISIN").value;
            winSelProd = document.open("frmBuscarValor.aspx?ISIN=" + strISIN, "winSelProd", "top=30,left=20,height=400,width=450,menubar=no,toolbar=no", true);
            winSelProd.focus();
        }
        function itemSelected(source, params) {
            if (source == 'LISTADO') {
                document.getElementById("btnBuscar").click();
            }
            if (source == 'LISTADOORDENES') {
                document.getElementById("txtCodigoOrden").value = params[0];
                document.getElementById("ddlfondo").value = params[3];
                document.getElementById("ddlMonedaOrigen").value = params[4];
                document.getElementById("ddlOperacion").value = params[5];
                document.getElementById("btnBuscar").click();
            }
        }
        function OpenWindow1(ventana) {
            CargarPopUp('fmConsultaCuponeras.aspx?id=' + ventana);
        }
        function OpenWindow2(ventana) {
            CargarPopUp('frmConsultaLimitesInstrumento.aspx');
        }
        function MostrarMensaje(mensaje) {
            if (Validar()) { return alertify.confirm(mensaje); }
            else return false;
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
                if (confirmacion == true) { window.close(); }
                return false;
            } { return true; }
        }
        function Salida() {
            fullQs = window.location.search.substring(1);
            qsParamsArray = fullQs.split("&");
            strKey = qsParamsArray[0].split("=");
            var strMensaje = "";
            var strAccion = "";
            strAccion = document.getElementById("hdMensaje").value
            if (strAccion != "") {
                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Opciones Derivadas/Forward Divisas?";
                if (strMensaje != "") {
                    confirmacion = alertify.confirm(strMensaje);
                    if (confirmacion == true) {
                        if (strKey[1] == "C" || strKey[1] == "M") { window.close(); }
                        else { location.href = "../../../frmDefault.aspx"; }
                    }
                    return false;
                } { return true; }
            }
            else {
                if (strKey[1] == "C" || strKey[1] == "M") { window.close(); }
                else { location.href = "../../../frmDefault.aspx"; }
            }
        }
        function Salida2() {
            var strMensaje = "", strAccion = "";
            strAccion = document.getElementById("hdMensaje").value
            if (strAccion != "") {
                confirmacion = alertify.confirm("¿Desea cancelar " + strAccion + " de la Orden de Inversión de Opciones Derivadas/Forward Divisas?");
                if (confirmacion == true) { window.close(); }
                return false;
            } else { window.close(); }
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
                } else { return false; }
            } else { return true; }
        }
        function Validar() {
            strMensajeError = "";
            if (ValidaCampos()) { return true; }
            else { alertify.alert(strMensajeError); return false; }
        }
        function ValidaCampos() {
            var strMsjCampOblig = "";
            var src = window.event.srcElement;
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "-Portafolio<BR>"
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Operación<BR>"
            if (document.getElementById("<%= ddlMonedaOrigen.ClientID %>").value == "")
                strMsjCampOblig += "-Moneda Origen<BR>"
            if (document.getElementById("<%= ddlMonedaDestino.ClientID %>").value == "")
                strMsjCampOblig += "-Moneda<BR>"
            if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Hora Operación<BR>"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "-Intermediario<br>"
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Operación<br>"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "-Fecha Vencimiento<br>"
            if (document.getElementById("<%= txtTcFuturo.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Cambio Futuro<br>"
            if (document.getElementById("<%= txtPlazo.ClientID %>").value == "")
                strMsjCampOblig += "-Plazo<br>"
            if (document.getElementById("<%= tbTcSpot.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Cambio Spot<br>"
            //RGF 20081105 Se debe garantizar que el monto negociado sea mayor que cero
            if (src.id == "btnAceptar") {
                if (document.getElementById("<%= tbMontoOrigen.ClientID %>").value <= 0)
                    strMsjCampOblig += "-Monto Negociada<br>"
                if (document.getElementById("<%= txtMontoFuturo.ClientID %>").value <= 0)
                    strMsjCampOblig += "-Monto Futuro<br>"
                if (document.getElementById("<%= ddlMotivo.ClientID %>").value == "")
                    strMsjCampOblig += "-Motivo<br>"
                //if (document.getElementById("<%= ddlTipoMoneda.ClientID %>").value == "")
                 //   strMsjCampOblig += "-Cobertura<br>"
            }
            else if (document.getElementById("<%= tbMontoOrigen.ClientID %>").value <= 0 &&
				document.getElementById("<%= txtMontoFuturo.ClientID %>").value <= 0) {
                strMsjCampOblig += "-Monto Negociada<br>"
                strMsjCampOblig += "-Monto Futuro<br>"
            }
            if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            } { return true; }
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-sm-9">
                    <h2><asp:label text="Forward de Divisas" runat="server" ID="lblTitulo" /></h2>
                </div> 
                <div class="col-sm-3" style=" text-align:right;">
                    <h2><asp:label text="" runat="server" ID="lblAccion" /></h2>
                </div>
            </div>
        </header>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label runat="server" id="lblFondo" class="col-sm-4 control-label">
                                    Portafolio</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true"
                                        Enabled="False" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Operaci&oacute;n</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlOperacion" Width="210px" Enabled="False" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Moneda Negociada</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlMonedaOrigen" Width="180px" Enabled="False" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnISIN" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label runat="server" id="Label1" class="col-sm-4 control-label">
                                        CodigoISIN
                                    </label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="TXTisin" MaxLength="12" width="150px"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label runat="server" id="lblFondoDestino" class="col-sm-4 control-label" visible="False">
                                    Portafolio Destino</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlFondoDestino" runat="server" Enabled="False" Visible="False" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-9" style="text-align: right;">
                            <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Datos de Operaci&oacute;n</legend>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Renovación" que no se utilizará a sección oculta | 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label" style="top:-7px">
                                    Modalidad Compra</label>
                                <div class="col-sm-8">
                                    <asp:RadioButtonList runat="server" ID="rbnDelivery" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="S" Text="Delivery" />
                                        <asp:ListItem Value="N" Text="Non-Delivery" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Renovación" que no se utilizará a sección oculta | 05/06/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta | 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fecha Operaci&oacute;n</label>
                                <div class="col-sm-8">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="tbFechaOperacion" Width="100px" ReadOnly="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Liquidaci&oacute;n</label>
                                <div class="col-sm-7">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <div runat="server" id="imgFechaVcto" class="input-append date">
                                                <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" />
                                                <span class="add-on"><i class="awe-calendar"></i></span>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtPlazo" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta | 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Plazo</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtPlazo" onkeypress="javascript: DiasPlazo();" MaxLength="20"
                                        CssClass="Numbox-0" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha de Vencimiento</label>
                                <div class="col-sm-7">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <div runat="server" id="imgFechaContrato" class="input-append date">
                                                <asp:TextBox runat="server" ID="tbFechaFinContrato" SkinID="Date" />
                                                <span class="add-on"><i class="awe-calendar"></i></span>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtPlazo" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lNPoliza" class="col-sm-4 control-label" visible="False">
                                    Nro. Poliza</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="tbNPoliza" Width="140px" Visible="False" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Cambio Futuro</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtTcFuturo" Width="140px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Negociada</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="tbMontoOrigen" Width="140px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Moneda</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlMonedaDestino" Width="190px" Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Cambio Spot</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="tbTcSpot" Width="140px" CssClass="Numbox-7" onkeypress="javascript: DiasPlazo();" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Futuro</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtMontoFuturo" Width="140px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Contacto", "Observación","No Liquida en Caja", "Regularización SBS" que no se utilizará a sección oculta | 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Intermediario</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlIntermediario" Width="210px" AutoPostBack="true"
                                        Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Contacto", "Observación","No Liquida en Caja", "Regularización SBS" que no se utilizará a sección oculta | 05/06/18 --%>
                    <div class="row hidden" runat="server" id="trMotivoCambio">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Motivo de cambio</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="210px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lblComentarios" class="col-sm-5 control-label">
                                    Comentarios</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="2" Width="300px"
                                        Style="text-transform: uppercase;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Motivo</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlMotivo" Width="360px" AutoPostBack="true"
                                        Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Cobertura</label>
                                <div class="col-sm-8">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlTipoMoneda" Width="120px" Enabled="false" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlMotivo" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-8" style="text-align: right;">
                            <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                        </div>
                    </div>
                </fieldset>
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnbuscar" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <div class="col-sm-12">
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <div class="col-sm-12" style="text-align: right;">
                        <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                    </div>
                </div>
            </div>
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
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" UseSubmitBehavior="false"/>
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <input runat="server" type="hidden" id="hdSaldo" name="hdSaldo" />
    <input runat="server" type="hidden" id="hdPagina" name="hdPagina" />
    <input runat="server" type="hidden" id="hdCustodio" name="hdCustodio" />
    <input runat="server" type="hidden" id="hdNemonico" name="hdNemonico" />
    <input runat="server" type="hidden" id="hdNumUnidades" name="hdNumUnidades" />
    <input runat="server" type="hidden" id="hddLoad" name="hddLoad" />
    <input runat="server" type="hidden" id="txtCodigoOrden" name="txtCodigoOrden" />
    <input runat="server" type="hidden" id="hdMensaje" name="hdMensaje" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladaron los campos que no se utilizará a sección oculta | 05/06/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS</legend>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Hora Operaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Contacto</label>
                    <div class="col-sm-8">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlContacto" Width="210px" Enabled="false" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlIntermediario" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div class="row oculto">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Observaciones</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtObservacion" Width="360px" MaxLength="20" Style="text-transform: uppercase" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row oculto">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                    </label>
                    <div class="col-sm-8">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkFicticia" Text="No Liquida en Caja" runat="server" Enabled="false" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                    </label>
                    <div class="col-sm-8">
                        <asp:CheckBox ID="chkRegulaSBS" Text="Regularización SBS" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lblRenovacion" class="col-sm-4 control-label">
                        Renovaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:RadioButtonList runat="server" ID="rbnRenovacion" RepeatDirection="Horizontal">
                            <asp:ListItem Value="S" Text="SI" />
                            <asp:ListItem Value="N" Text="NO" />
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladaron los campos que no se utilizará a sección oculta | 05/06/18 --%>
    </form>
</body>
</html>