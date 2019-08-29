<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOrdenesFondo.aspx.vb"
    EnableEventValidation="true" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmOrdenesFondo" %>

<!DOCTYPE html />
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head id="Head1" runat="server">
    <base target="_self">
    <title>Orden de Inversión - FONDOS DE INVERSIÓN</title>
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
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            }
            {
                return true;
            }
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
                return alertify.confirm(mensaje);
            }
            else
                return false;
        }
        function calcularMontoOperacion() {
            if (document.getElementById("<%= txtNroFondoOrd.ClientID %>").value != "" && document.getElementById("<%= txtPrecio.ClientID %>").value != "") {
                var txtNroFondoOrd = "";
                var txtPrecio = "";
                var num = "";
                txtNroFondoOrd = document.getElementById("<%= txtNroFondoOrd.ClientID %>").value; //1
                txtPrecio = document.getElementById("<%= txtPrecio.ClientID %>").value; //2
                //txtNroFondoOrd = txtNroFondoOrd.replace(",", "");
                txtNroFondoOrd = txtNroFondoOrd.replace(/$|,/g, ""); //OT10965 - 01/12/2017 - Ian Pastor M.
                //txtPrecio = txtPrecio.replace(",", "");
                txtPrecio = txtPrecio.replace(/$|,/g, ""); //'OT10965 - 01/12/2017 - Ian Pastor M.
                txtNroFondoOrd = parseFloat(txtNroFondoOrd);
                txtPrecio = parseFloat(txtPrecio);
                document.getElementById("<%= txtMontoNominal.ClientID %>").value = Math.round((txtNroFondoOrd * txtPrecio) * 100) / 100;
            }
            return false;
        }
        function cambiaTitulo() {
            if (document.getElementById("ddlOrdenFondo").value == 'M')
                document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - FONDOS MUTUOS';
            else
                document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - FONDOS DE INVERSIÓN';
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
            var strOI = "";
            strOI = document.getElementById("<%=ddlOrdenFondo.ClientID %>").value;

            if (strOI == "I") {
                strOI = "Fondo de Inversión"
            }
            else {
                strOI = "Fondos Mutuos"
            }

            strAccion = document.getElementById("hdMensaje").value
            var Pagina = document.getElementById("<%=hdPagina.ClientID %>").value

            if (strAccion != "") {
                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de " + strOI + "?"
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
            var esCashCall = document.getElementById("<%= hdCashCall.ClientID %>").value;
            var strMsjCampOblig = "";
            if (esCashCall == 0 || (esCashCall == 1 && document.getElementById("<%= ddlTipoFondo.ClientID %>").value == "CC_CNC")) {
                //INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se quita opción de dato obligatorio porque se oculta campo Tipo de Fondo | 15/06/18 
                //if (document.getElementById("<%= ddlTipoFondo.ClientID %>").value == "")
                //                    strMsjCampOblig += "-Tipo<br>" 
                //FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se quita opción de dato obligatorio porque se oculta campo Tipo de Fondo | 15/06/18 
                if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                    strMsjCampOblig += "-Portafolio<br>"
                if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                    strMsjCampOblig += "-Operación<br>"
                if (document.getElementById("<%= ddlOrdenFondo.ClientID %>").value == "")
                    strMsjCampOblig += "-Orden Fondo<br>"
                if (document.getElementById("<%= txtISIN.ClientID %>").value == "")
                    strMsjCampOblig += "-Código ISIN<br>"
                if (document.getElementById("<%= txtMnemonico.ClientID %>").value == "")
                    strMsjCampOblig += "-Código Mnemónico<br>"
                if (document.getElementById("<%= txtSBS.ClientID %>").value == "")
                    strMsjCampOblig += "-Código SBS<br>"
                if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                    strMsjCampOblig += "-Código Intermediario<br>"
                if (document.getElementById("<%= txtNroFondoOp.ClientID %>").value == "")
                    strMsjCampOblig += "-Número Cuotas Operación<br>"
                if (document.getElementById("<%= txtNroFondoOrd.ClientID %>").value == "")
                    strMsjCampOblig += "-Número Cuotas Ordenadas<br>"
                if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                    strMsjCampOblig += "-Fecha Operación<br>"
                if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                    strMsjCampOblig += "-Fecha Vencimiento<br>"
                if (document.getElementById("<%= tbFechaTrato.ClientID %>").value == "")
                    strMsjCampOblig += "-Fecha Trato<br>"
                if (document.getElementById("<%= txtPrecio.ClientID %>").value == "")
                    strMsjCampOblig += "-Precio<br>"
                if (document.getElementById("<%= txtMontoNominal.ClientID %>").value == "")
                    strMsjCampOblig += "-Monto Operación<br>"
                if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                    strMsjCampOblig += "-Hora Operación<br>"
                if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                    strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
                if (document.getElementById("<%= ddlGrupoInt.ClientID %>").value == "")
                    strMsjCampOblig += "-Grupo de Intermediarios<br>"
            }
            else {
                if (document.getElementById("<%= ddlTipoFondo.ClientID %>").value == "CC_SNC") {
                    if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                        strMsjCampOblig += "-Portafolio<br>"
                    if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                        strMsjCampOblig += "-Operación<br>"
                    if (document.getElementById("<%= ddlOrdenFondo.ClientID %>").value == "")
                        strMsjCampOblig += "-Orden Fondo<br>"
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
                    if (document.getElementById("<%= tbFechaTrato.ClientID %>").value == "")
                        strMsjCampOblig += "-Fecha Trato<br>"
                    if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                        strMsjCampOblig += "-Hora Operación<br>"
                    if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                        strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
                    if (document.getElementById("<%= txtMontoNominal.ClientID %>").value == "")
                        strMsjCampOblig += "-Monto Operación<br>"
                }
            }
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
                document.getElementById("<%= tbFechaOperacion.ClientID %>").readonly = "";
                if (document.getElementById("<%= hdFechaOperacion.ClientID %>").value == "")
                    document.getElementById("<%= hdFechaOperacion.ClientID %>").value = document.getElementById("<%= tbFechaOperacion.ClientID %>").value;
            }
            else {
                document.getElementById("<%= tbFechaOperacion.ClientID %>").disabled = true;
                document.getElementById("<%= tbFechaOperacion.ClientID %>").readonly = "readonly";
                if (document.getElementById("<%= hdFechaOperacion.ClientID %>").value != "")
                    document.getElementById("<%= tbFechaOperacion.ClientID %>").innerText = document.getElementById("<%= hdFechaOperacion.ClientID %>").value;
            }
        }
        function cambiaCantidad(num) {
            if (num != "") {
                document.getElementById("<%= txtNroFondoOp.ClientID %>").value = num;
            }
            return false;
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
                        <asp:Label ID="lblTitulo" Text="Orden de Inversión - FONDOS DE INVERSIÓN"
                            runat="server" /></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h2>
                        <asp:Label ID="lblAccion" Text="" runat="server" />
                    </h2>
                </div>
            </div>
        </header>
        <asp:UpdatePanel ID="uppagina" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Orden de Fondo</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlOrdenFondo" Width="150px" AutoPostBack="true"
                                        OnChange="javascript:cambiaTitulo();" Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lblFondo" class="col-sm-4 control-label">
                                    Portafolio</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true"
                                        OnChange="javascript:cambiaTitulo();" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Operación</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlOperacion" Width="150px" />
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
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta| 25/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Código ISIN</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtISIN" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Código Mnemónico</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS" que no se utilizará a sección oculta| 25/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 25/05/18 --%>
                    <div class="row">
                        <div class="col-sm-12" style="text-align: right;">
                            <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                        </div>
                        <div class="col-sm-12" style="text-align: right;">
                            <asp:Button Text="Características" runat="server" ID="btnCaracteristicas" />
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 25/05/18 --%>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Datos de Operación</legend>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 25/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Operación</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" ReadOnly="true" Width="100px" />
                                    <asp:CheckBox Text="" runat="server" ID="chkEmisionPrimaria" Visible="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Vencimiento</label>
                                <div class="col-sm-4">
                                    <div runat="server" id="imgFechaVcto" class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación" que no se utilizará a sección oculta| 25/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Nro. Cuotas Op." que no se utilizará a sección oculta| 25/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Trato</label>
                                <div class="col-sm-4">
                                    <div runat="server" id="imgFechaTrato" class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaTrato" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label runat="server" id="lbNroCuotasOrdenado" class="col-sm-5 control-label" style="width: 40.45%">
                                    Nro. Cuotas Ordenado</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtNroFondoOrd" Width="146px" CssClass="Numbox-7_22"
                                        onblur="calcularMontoOperacion(); cambiaCantidad(this.value)" />
                                </div>
                                <div id="divSaldoDispo" runat="server" class="col-sm-3" visible="false">
                                    <asp:TextBox runat="server" ID="txtSaldoDispo" CssClass="Numbox-7_22" Enabled="false"
                                        Width="130px" ToolTip="Cuotas Disponibles" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Nro. Cuotas Op." que no se utilizará a sección oculta| 25/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Nro. Poliza." que no se utilizará a sección oculta| 25/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lbPrecio" class="col-sm-6 control-label">
                                    Precio</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtPrecio" Width="150px" CssClass="Numbox-7_12" onblur="calcularMontoOperacion()" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Monto Operación</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtMontoNominal" Width="150px" CssClass="Numbox-7_22" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Nro. Poliza." que no se utilizará a sección oculta| 25/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Medio Transmisión","Mercado Caja", "Bolsa","Observación","No Liquida en Caja" y "Regularización SBS" que no se utilizará a sección oculta| 25/05/18 --%>
                    <div class="row" runat="server" id="tr5">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Grupo de Intermediarios</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList runat="server" ID="ddlGrupoInt" Width="150px" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Intermediario</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList runat="server" ID="ddlintermediario" Width="150px" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="tr7">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Bolsa</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlPlaza" runat="server" Enabled="False" AutoPostBack="True"
                                        Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto", "Medio Transmisión","Mercado Caja", "Bolsa","Observación","No Liquida en Caja" y "Regularización SBS" que no se utilizará a sección oculta| 25/05/18 --%>
                    <div class="row hidden" id="trMotivoCambio" runat="server">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Motivo de cambio</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList runat="server" ID="ddlMotivoCambio" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label runat="server" id="lblComentarios" class="col-sm-6 control-label">
                                    Comentarios</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="2" Width="300px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="tr8">
                        <div class="col-sm-2">
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox Text="No Liquida en Caja" runat="server" ID="chkFicticia" /></div>
                        <div class="col-sm-2">
                            <asp:CheckBox Text="Regularización SBS" runat="server" ID="chkRegulaSBS" />
                        </div>
                    </div>
                </fieldset>
                <br />
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <div class="col-sm-6">
                                <asp:CheckBox ID="chkreprocesar" runat="server" Checked="true" Text="Recalcular" />
                            </div>
                        </div>
                    </div>
                </div>
                <fieldset>
                    <legend>Comisiones y Gastos de Administradora</legend>
                    <div class="grilla">
                        <asp:GridView runat="server" ID="dgLista" SkinID="Grid">
                            <Columns>
                                <asp:BoundField DataField="codigoComision1" HeaderText="C&#243;digo Impuesto/Comisi&#243;n"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisi&#243;n" />
                                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisi&#243;n" />
                                <asp:BoundField DataField="strValorCalculadoComision1" HeaderText="Comisi&#243;n"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision1" runat="server" Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ValorOcultoComision1" HeaderText="ValorOcultoComision1"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="codigoComision2" HeaderText="C&#243;digo Impuesto/Comisi&#243;n"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisi&#243;n" />
                                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisi&#243;n" />
                                <asp:BoundField DataField="strValorCalculadoComision2" HeaderText="Comisi&#243;n"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:TemplateField HeaderText="Valor Comisi&#243;n">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision2" runat="server" Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ValorOcultoComision2" HeaderText="ValorOcultoComision2"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label runat="server" id="Label1" class="col-sm-4 control-label">
                                    Total Comisiones</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txttotalComisionesC" Width="150px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label" id="Label2" runat="server">
                                    Monto Neto Operación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtMontoNetoOpe" Width="150px" CssClass="stlCajaBloqueadoNumero"
                                        ForeColor="Black" MaxLength="20" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4" runat="server" id="PrecioPromedio">
                            <div class="form-group">
                                <label runat="server" id="Label3" class="col-sm-4 control-label">
                                    Precio Promedio</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtPrecPromedio" Width="150px" ReadOnly="true" Style="margin-bottom: 0px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div class="row" style="text-align: right;">
                    <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                    <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                </div>
                <asp:HiddenField ID="hdPagina" runat="server" Value="" />
                <input id="hdCashCall" value="0" type="hidden" runat="server" />
                <input id="hdSaldo" type="hidden" name="hdSaldo" runat="server" />
                <input id="txtCodigoOrden" type="hidden" name="txtCodigoOrden" runat="server" />
                <input id="hdNumUnidades" type="hidden" name="hdNumUnidades" runat="server" />
                <input id="hdMensaje" type="hidden" name="hdMensaje" runat="server" />
                <input id="hdCustodio" type="hidden" name="hdCustodio" runat="server" />
                <input id="hdFechaOperacion" type="hidden" name="hdFechaOperacion" runat="server" />
                <input id="hdPopUp" type="hidden" name="hdPopUp" runat="server" />
                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
                <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
                <header></header>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnBuscar" />
                <%--      <asp:PostBackTrigger ControlID="btnAceptar" />--%>
            </Triggers>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-6">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Modificar" runat="server" ID="btnModificar" />
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" />
                <asp:Button Text="Consultar" runat="server" ID="btnConsultar" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <asp:Button Text="Salir" runat="server" ID="btnRetornar" Visible="false" />
                        <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" UseSubmitBehavior="false" />
                        <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 25/05/18 --%>
    <fieldset class="hidden">
        <legend>Campos Retirados</legend>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Tipo</label>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlTipoFondo" Width="150px" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Código SBS</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtSBS" Width="150px" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lblFondoDestino" class="col-sm-4 control-label" visible="false">
                        Portafolio Destino</label>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlFondoDestino" Width="150px" Visible="False" />
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
                            <asp:TextBox runat="server" ID="lbldescripcion" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Fecha Fin Bono</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblfecfinbono" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Nominales Emitidos</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblnominales" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Saldo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblSaldoValor" Width="100px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-7 control-label">
                            Emisor</label>
                        <div class="col-sm-5">
                            <asp:TextBox runat="server" ID="lblemisor" Width="125px" ReadOnly="true" Height="22px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            % Participación</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblparticipacion" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Base Cupón</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblbasecupon" Width="125px" ReadOnly="true" />
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
                            <asp:TextBox runat="server" ID="lblFecUltCupon" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Precio Vector</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblpreciovector" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Base Tir</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblbasetir" Width="125px" ReadOnly="true" />
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
                            <asp:TextBox runat="server" ID="lblFecProxCupon" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Nominales por Unidad</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblUnidades" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Duración</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblduracion" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                </div>
                <div class="col-sm-4">
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Rescate</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="lblRescate" Width="125px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Hora Operación</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lbNroCuotasOp" class="col-sm-6 control-label">
                        Nro. Cuotas Op.</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtNroFondoOp" Width="150px" CssClass="Numbox-7_22" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lNPoliza" class="col-sm-6 control-label">
                        Nro. Poliza</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="tbNPoliza" Width="150px" CssClass="Numbox-7_22" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Contacto</label>
                    <div class="col-sm-4">
                        <asp:DropDownList runat="server" ID="ddlContacto" Width="150px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lblTipoTramo" class="col-sm-6 control-label">
                        Tipo Tramo</label>
                    <div class="col-sm-4">
                        <asp:DropDownList runat="server" ID="ddlTipoTramo" Width="150px" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lbMedioTrans" class="col-sm-6 control-label">
                        Medio Transmisión</label>
                    <div class="col-sm-4">
                        <asp:DropDownList runat="server" ID="ddlMedioTrans" Width="150px" AutoPostBack="true" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lbMercado" class="col-sm-6 control-label">
                        Mercado Caja</label>
                    <div class="col-sm-4">
                        <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" Enabled="False" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row" runat="server" id="trx">
            <%--                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">Bolsa</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlPlaza" runat="server" Enabled="False" AutoPostBack="True" Width="150px" />
                        </div>
                    </div>
                </div>--%>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Observación</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtObservacion" Width="350px" Style="text-transform: uppercase;" />
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 25/05/18 --%>
    </form>
</body>
</html>
