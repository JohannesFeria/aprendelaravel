<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOperacionesReporte.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmOperacionesReporte" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Operaciones de Reporte</title>

    <style type="text/css">
        .oculto {
            display: none;
        }
    </style>

    <script type="text/javascript">
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
            }
            { return true; }
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
        function MostrarMensaje(mensaje) {
            if (Validar()) {
                return alertify.confirm(mensaje);
            }
            else
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
                if (confirmacion == true) { window.close(); }
                return false;
            } { return true; }
        }
        function Salida() {
            var strMensaje = "";
            var strAccion = "";
            strAccion = document.getElementById("hdMensaje").value
            if (strAccion != "") {
                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Operaciones Reporte?"
                if (strMensaje != "") {
                    confirmacion = alertify.confirm(strMensaje);
                    if (confirmacion == true) {
                        location.href = "../../../frmDefault.aspx";
                    }
                    return false;
                }
                { return true; }
            }
            else { location.href = "../../../frmDefault.aspx"; }
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
            }
            else { return true; }
        }
        function Validar() {
            strMensajeError = "";
            if (ValidaCampos()) { return true; }
            else { alertify.alert(strMensajeError); return false; }
        }
        function ValidaCampos() {
            var strMsjCampOblig = "";
            if ($("#<%=tbCantidadCOMPRA.ClientID%>").val() == "") strMsjCampOblig += "-Cantidad Compra<br>"
            if ($("#<%=tbPlazoVenCOMPRA.ClientID%>").val() == "") strMsjCampOblig += "-Plazo Reporte<br>"
            if ($("#<%=tbMontoNetoCOMPRA.ClientID%>").val() == "") strMsjCampOblig += "-Monto Neto<br>"
            if ($("#<%=tbNroPoliza.ClientID%>").val() == "") strMsjCampOblig += "-Nro. Poliza<br>"
            if ($("#<%=tbMontoNetoVENTA.ClientID%>").val() == "") strMsjCampOblig += "-Monto Neto Vcto.<br>"
            if ($("#<%=ddlGrupoInt.ClientID%>").val() == "") strMsjCampOblig += "-Grupo Intermediario<br>"
            if ($("#<%=ddlIntermediario.ClientID%>").val() == "") strMsjCampOblig += "-Intermediario<br>"
            if ($("#<%=ddlBolsa.ClientID%>").val() == "") strMsjCampOblig += "-Bolsa<br>"
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            } { return true; }
        }
    </script>
</head>
<body>
    <form id="frmInvocador" runat="server" method="post" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" runat="server">Orden de Inversión - Operaciones de Reporte</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3><asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se habilita campo "Operación" cuando formulario es llamado de otra ventana| 05/06/18 --%>
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
                <div class="col-md-4" id="divOperacion" style="display:none">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlOperacion" runat="server" Enabled="False" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <asp:UpdatePanel ID="upMoneda" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlmoneda" runat="server" />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se habilita campo "Operación" cuando formulario es llamado de otra ventana| 05/06/18 --%>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Código SBS"  que no se utilizará a sección oculta y CodigoISIN se mostrará cuando formulario es invocado de otra ventana| 05/06/18 --%>
            <div class="row">
                <div class="col-md-4" id="divCodigoIsin">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código Mnemónico</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtMnemonico" runat="server" Width="120px" MaxLength="15" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Código Isin</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtISIN" runat="server" Width="120px" MaxLength="12" />
                            <asp:HiddenField ID="txtCodigoOrden" runat="server" />
                    </div>
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Código Isin", "Código SBS"  que no se utilizará a sección oculta| 05/06/18 --%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <asp:Label ID="lblFondoDestino" runat="server" Visible="false">Portafolio Destino</asp:Label>
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFondoDestino" runat="server" Enabled="False" Visible="false"
                                Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" Style="height: 26px" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos de la Operación COMPRA/CONTADO</legend>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Fecha Vencimiento"  que no se utilizará a sección oculta| 05/06/18 --%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-7">
                            <asp:UpdatePanel ID="upfechainicompra" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="tbFechaInicioCOMPRA" runat="server" Width="90px" Enabled="False" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Liquidación</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbFechaLiquiCOMPRA" runat="server" Width="90px" Enabled="False" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divFechaVcto">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Vencimiento</label>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="tbFechaVenCOMPRA" runat="server" Width="90px" Enabled="false" />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Fecha Vencimiento"  que no se utilizará a sección oculta| 05/06/18 --%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Plazo Vencimiento</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbPlazoVenCOMPRA" runat="server" Enabled="false" Width="90px" CssClass="Numbox-0"
                                AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cantidad</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbCantidadCOMPRA" runat="server" Enabled="false" Width="90px" CssClass="Numbox-2" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Monto Neto</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbMontoNetoCOMPRA" runat="server" Enabled="false" Width="90px"
                                CssClass="Numbox-2" />
                        </div>
                    </div>
                </div>
            </div>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Clasificación"  que no se utilizará a sección oculta| 05/06/18 --%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa (360)%</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbTasa360" runat="server" Width="90px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Bolsa</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlBolsa" Width="180px" Enabled="false" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Clasificación"  que no se utilizará a sección oculta| 05/06/18 --%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Grupo Intermediario</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlGrupoInt" runat="server" Enabled="False" AutoPostBack="True"
                                Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Intermediario</label>
                        <div class="col-sm-8">
                            <asp:UpdatePanel ID="upintermediario" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlIntermediario" runat="server" Enabled="False" AutoPostBack="True"
                                        Width="300px" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlGrupoInt" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <asp:UpdatePanel ID="updatos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend>Datos de la Operación VENTA/PLAZO</legend>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Plazo Vencimiento" , "Cantidad" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Neto Vcto.</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="tbMontoNetoVENTA" runat="server" Enabled="false" Width="90px" CssClass="Numbox-2" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Plazo Vencimiento" , "Cantidad" que no se utilizará a sección oculta| 05/06/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Observación" , "Rendimientos" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Tasa (360)%</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="tbTasa365" runat="server" Width="90px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Observación" , "Rendimientos" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row" id="trMotivoCambio" style="display: none">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Motivo de cambio</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="200px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label runat="server" id="lblComentarios" class="col-sm-6 control-label">
                                    Comentarios</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtComentarios" CssClass="mayusculas" TextMode="MultiLine"
                                        Rows="4" Width="250px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <div class="col-sm-12">
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4" style="text-align: right;">
                            <asp:Button runat="server" ID="btnProcesar" Text="Procesar" OnClientClick="return Validar();" />
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset class ="oculto" id="divComisiones">
                    <legend>Comisiones</legend>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Comisión Compra AI</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtcomisioncompra" runat="server" CssClass="Numbox-7" /></div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Comisión Venta AI</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtcomisionventa" runat="server" CssClass="Numbox-7" /></div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Resto Comisión Compra</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtrestocompra" runat="server" CssClass="Numbox-7" /></div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Resto Comisión Venta</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtrestoventa" runat="server" CssClass="Numbox-7" /></div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Impuesto Compra</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtimpuestocompra" runat="server" CssClass="Numbox-7" /></div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Impuesto Venta</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtimpuestoventa" runat="server" CssClass="Numbox-7" /></div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="tbPlazoVenCOMPRA" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset class="oculto" id="divTotalComisiones">
                    <legend>Total de Comisiones</legend>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Total Comisiones Compra</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbTotalComiCompra" runat="server" CssClass="Numbox-7" 
                                        Enabled="False" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Total Comisiones Venta</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="tbTotalComiVenta" runat="server" CssClass="Numbox-7" 
                                        Enabled="False" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
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
                        <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" />
                    </div>
                </div>
            </div>
        </div>
        <header></header>
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
                        <asp:UpdatePanel ID="upnl1" runat="server">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="btnRetornar" Text="Salir" Visible="False" />
                                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" UseSubmitBehavior="false" Style="height: 26px" />
                                <asp:Button runat="server" ID="btnSalir" Text="Salir" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
    <asp:HiddenField ID="hdHoraOperacion" runat="server" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladaron los campos que no se utilizará a sección oculta | 05/06/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS</legend>
        <div class="row">
           
            <div class="col-md-4">
                <div class="form-group" id="celda_poliza" runat="server">
                    <label class="col-sm-5 control-label">
                        Nro. Poliza</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="tbNroPoliza" runat="server" Width="120px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
<%--        <div class="row">
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código Isin</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtISIN" runat="server" Width="120px" MaxLength="12" />
                        <asp:HiddenField ID="txtCodigoOrden" runat="server" />
                    </div>
                </div>
            </div>--%>
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código SBS</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtSBS" runat="server" Width="120px" MaxLength="12" />
                    </div>
                </div>
            </div>
        </div>
<%--        <div class="row">
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Fecha Vencimiento</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="tbFechaVenCOMPRA" runat="server" Width="90px" />
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Clasificación</label>
                    <div class="col-sm-7">
                        <asp:DropDownList ID="ddlClasificacion" Width="180px" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Plazo Vencimiento</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="tbPlazoVENTA" runat="server" Enabled="false" Width="90px" CssClass="Numbox-0"
                            AutoPostBack="True" />
                    </div>
                </div>
            </div>
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Cantidad</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbCantidadVENTA" runat="server" Enabled="false" Width="90px" CssClass="Numbox-2" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Observación</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbObservacion" runat="server" Width="180px" Enabled="false" />
                    </div>
                </div>
            </div>
            <div class="col-md-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Rendimientos</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="tbRendimientos" runat="server" Width="90px" CssClass="Numbox-7"
                            Enabled="false" />
                    </div>
                </div>
            </div>
        </div>
        <br />
    </fieldset>
    </form>
</body>
</html>