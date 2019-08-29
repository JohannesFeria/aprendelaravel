<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDepositoPlazos.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmDepositoPlazos" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Deposito Plazos</title>
    <style type="text/css">
        .oculto {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function actualizarMonto(num) {
            $('#txtMnomOp').NumBox('setRaw', num);
            return false;
        }
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
            if (strMsjCampOblig != "") { strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>"; ; return false; }
            { return true; }
        }
        function buscar() {
            strISIN = document.getElementById("txtISIN").value;
            winSelProd = document.open("BuscarValor.aspx?ISIN=" + strISIN, "winSelProd", "top=30,left=20,height=400,width=450,menubar=no,toolbar=no", true);
            winSelProd.focus();
        }
        function itemSelected(source, params) {
            if (source == 'LISTADO') {
                frmInvocador.document.getElementById("btnBuscar").click();
            }
            if (source == 'LISTADOORDENES') {
                frmInvocador.document.getElementById("txtCodigoOrden").value = params[0];
                frmInvocador.document.getElementById("ddlfondo").value = params[3];
                frmInvocador.document.getElementById("ddlOperacion").value = params[5];
                frmInvocador.document.getElementById("btnBuscar").click();
            }
        }
        function OpenWindow1(ventana) { CargarPopUp('frmConsultaCuponeras.aspx?id=' + ventana); }
        function OpenWindow2(ventana) { CargarPopUp('frmConsultaLimitesInstrumento.aspx'); }
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
                case "CDP":
                    strMensajeConfirmacion = "¿Desea cancelar la Corrección de la Orden de Inversión Nro. " + NroOrden + "?";
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
            strAccion = document.getElementById("hdMensaje").value;
            if (strAccion != "") {
                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Depósito a Plazos?";
                if (strMensaje != "") {
                    confirmacion = confirm(strMensaje);
                    if (confirmacion == true) { location.href = "../../../frmDefault.aspx"; }
                    return false;
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
                confirmacion = confirm(strMensajeConfirmacion);
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
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "") {
                strMsjCampOblig += "-Portafolio<br>"
            }
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "") {
                strMsjCampOblig += "-Operación<br>"
            }
            if (document.getElementById("<%= ddlTipoTitulo.ClientID %>").value == "") {
                strMsjCampOblig += "-Tipo Título<br>"
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            } else {
                if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                    strMsjCampOblig += "-Fecha Operación<br>"
                if (document.getElementById("<%= tbFechaPago.ClientID %>").value == "")
                    strMsjCampOblig += "-Fecha Vencimiento<br>"
                if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                    strMsjCampOblig += "-Hora Operación<br>"
                if (document.getElementById("<%= tbFechaContrato.ClientID %>").value == "")
                    strMsjCampOblig += "-Fecha Contrato<br>"
                if (document.getElementById("<%= txtMnomOp.ClientID %>").value == "")
                    strMsjCampOblig += "-Monto Nominal Operación<br>"
                if (document.getElementById("<%= ddlTipoTasa.ClientID %>").value == "")
                    strMsjCampOblig += "-Tipo Tasa<br>"
                if (document.getElementById("<%= txtTasa.ClientID %>").value == "")
                    strMsjCampOblig += "-Tasa<br>"
                if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                    strMsjCampOblig += "-Intermediario<br>"
                if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                    strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
                if (strMsjCampOblig != "") {
                    strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                    return false;
                }{
                    return true; 
                }
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
                <div class="col-sm-9">
                    <h2>
                        <asp:Label ID="lblTitulo" name="lblTitulo" runat="server" Text="Orden de Inversión - DEPÓSITO A PLAZO" />
                    </h2>
                </div>
                <div class="col-sm-3" style="text-align: right;">
                    <h3><asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <br />
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblFondo" runat="server">Portafolio</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlFondo" runat="server" Enabled="False" AutoPostBack="True"
                            Width="120px" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-3 control-label">
                        Operación</label>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlOperacion" runat="server" Enabled="False" />
                    </div>
                </div>
            </div>
            <div class="col-sm-5">
                <div class="form-group">
                    <label class="col-sm-3 control-label">
                        Tipo Título</label>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlTipoTitulo" runat="server" Enabled="False" AutoPostBack="True"
                            Width="280px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
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
            <div class="col-sm-8 control-label">
                <asp:Button Text="Buscar" runat="server" Visible="false" ID="btnBuscar" />
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="UPcuerpo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend>Datos de Operación</legend>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Fecha Depósito"  que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <%--OT10795 Agregar calendario--%>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Operación</label>
                                <div class="col-sm-7">
                                    <div class="input-append">
                                        <div id="Div2" runat="server" class="input-append date">
                                            <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Width="100px" AutoPostBack="true" />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--OT10795 Fin--%>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fecha Vencimiento</label>
                                <div class="col-sm-6">
                                    <div class="input-append">
                                        <div id="imgFechaContrato" runat="server" class="input-append date">
                                            <asp:TextBox runat="server" ID="tbFechaContrato" SkinID="Date" Width="100px" AutoPostBack="true" />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Fecha Depósito" que no se utilizará a sección oculta| 05/06/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Días Plazo</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPlazo" runat="server" CssClass="Numbox-0_3" AutoPostBack="True"
                                        Width="96px" MaxLength="3" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Moneda</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtMoneda" runat="server" Width="70px" MaxLength="10" ReadOnly="True" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    M. Nominal Operacion</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtMnomOp" runat="server" CssClass="Numbox-7" Width="96px" MaxLength="20"
                                        onchange="actualizarMonto(this.value)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tipo Tasa</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlTipoTasa" runat="server" Enabled="False" Width="96px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Tasa %</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtTasa" runat="server" Width="96px" MaxLength="12" CssClass="-Numbox-7" /></div>
                            </div>
                        </div>
                    </div>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Contacto" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Operación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtMontoOperacion" runat="server" Width="96px" ReadOnly="True" CssClass="Numbox-2" /></div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Intermediario</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlIntermediario" runat="server" Enabled="False" AutoPostBack="True"
                                        Width="200px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "Contacto" que no se utilizará a sección oculta| 05/06/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    <asp:Label ID="lblCodigoSBS" runat="server" Visible="False">Código SBS:</asp:Label></label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="tbCodigoSBS" runat="server" Width="96px" Visible="False" MaxLength="12"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "No Liquida en Caja", "Observación"  que no se utilizará a sección oculta| 05/06/18 --%>
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
                                <label runat="server" id="lblComentarios" class="col-sm-4 control-label">
                                    Comentarios</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="4" Width="250px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladó campo "No Liquida en Caja", "Observación"  que no se utilizará a sección oculta| 05/06/18 --%>
                </fieldset>
                 <br />
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-5 control-label">
                                <asp:Label ID="lNPoliza" runat="server" Visible="False">Nro.Poliza</asp:Label>
                            </label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="tbNPoliza" runat="server" Width="96px" MaxLength="15" Visible="False" />
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnRenovacion" Visible="false" runat="server">
                    <fieldset>
                        <legend>Datos de la Renovación</legend>
                        <div class="grilla">
                            <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="gvDatos">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox"></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                            <asp:Label ID="lbCodigo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOrden") %>'></asp:Label>
                                            <asp:Label ID="lbCodigoConstitucion" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodDatatec") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CodigoOrden" HeaderText="Orden" />
                                    <asp:BoundField DataField="Nemonico" HeaderText="Nemonico" />
                                    <asp:BoundField DataField="CodigoTipoCupon" HeaderText="Tipo Cupon" />
                                    <asp:BoundField DataField="TasaPorcentaje" HeaderText="Tasa %" />
                                    <asp:BoundField DataField="MontoNominalOperacion" HeaderText="Nominal" DataFormatString="{0:#,##0.00}" />
                                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto" DataFormatString="{0:#,##0.00}" />
                                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnBCR" Visible="false" runat="server">
                    <fieldset>
                        <legend>Datos de la transferencia BCR</legend>
                        <div class="grilla">
                            <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="gvBCR">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox"></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                            <asp:Label ID="lbCodigo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacionCaja") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CodigoOperacionCaja" HeaderText="Orden" />
                                    <asp:BoundField DataField="TerceroOrigen" HeaderText="Origen" />
                                    <asp:BoundField DataField="TerceroDestino" HeaderText="Destino" />
                                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </asp:Panel>
                <br />
                <div class="row" style="text-align: right;">
                    <asp:Button runat="server" ID="btnProcesar" Text="Procesar" OnClientClick="return Validar();" />
                </div>
                <div class="row" style="text-align: right;">
                    <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <header>
                </header>
        <div class="row">
            <div class="col-sm-7">
                <div class="form-group">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnIngresar" Text="Ingresar" />
                        <asp:Button runat="server" ID="btnModificar" Text="Modificar" />
                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" />
                        <asp:Button runat="server" ID="btnConsultar" Text="Consultar" />
                    </div>
                </div>
            </div>
            <div class="col-sm-5" style="text-align: right;">
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnRetornar" Text="Salir" Visible="False" />
                        <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" UseSubmitBehavior="false" />
                        <asp:Button runat="server" ID="btnSalir" Text="Salir" />
                    </ContentTemplate>
                </asp:UpdatePanel>
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
    <asp:HiddenField ID="hdMnemonico" runat="server" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 12/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 12/06/18 --%>
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladaron los campos que no se utilizará a sección oculta | 05/06/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS</legend>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Depósito</label>
                    <div class="col-sm-6">
                        <div class="input-append">
                            <div id="Div1" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaPago" SkinID="Date" Width="96px" AutoPostBack="true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label">
                        Hora Operación</label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" Width="100px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Contacto</label>
                    <div class="col-sm-6">
                        <asp:DropDownList ID="ddlContacto" runat="server" Enabled="False" Width="200px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-5 control-label" style="margin-left: -4px;">
                        <asp:CheckBox ID="chkFicticia" Style="margin-left: 1px;" runat="server" Enabled="False"
                            Text="No Liquida en Caja" />
                    </label>
                </div>
            </div>
            <div class="col-sm-4 oculto">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Observación</label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtObservacion" runat="server" Width="280px" MaxLength="20" Style="text-transform: uppercase" />
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se trasladaron los campos que no se utilizará a sección oculta | 05/06/18 --%>
    </form>
</body>
</html>