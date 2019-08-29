<%@ Page Language="VB" AutoEventWireup="True" CodeFile="frmPapelesComerciales.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_frmPapelesComerciales" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title></title>
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
            else return false;
        }


        function cambiaTitulo() {

            document.getElementById("lblTitulo").innerHTML = 'Orden de Inversión - PAPELES COMERCIALES';

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

                strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Inversión de Papeles Comerciales?"

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
                strMsjCampOblig += "-Fecha Liquidación<br>"
            if (document.getElementById("<%= txtMnomOrd.ClientID %>").value == "")
                strMsjCampOblig += "-Monto Nominal Ordenado<br>"
            if (document.getElementById("<%= txtMnomOp.ClientID %>").value == "")
                strMsjCampOblig += "-Monto Nominal Operación<br>"
            if (document.getElementById("<%= ddlTipoTasa.ClientID %>").value == "")
                strMsjCampOblig += "-Tipo Tasa<br>"
            if (document.getElementById("<%= txtYTM.ClientID %>").value == "")
                strMsjCampOblig += "-YTM<br>"
            if (document.getElementById("<%= txtPrecioNegoc.ClientID %>").value == "")
                strMsjCampOblig += "-Precio Negociación<br>"
            if (document.getElementById("<%= txtInteresCorNeg.ClientID %>").value == "")
                strMsjCampOblig += "-Interés Corrido Negociado<br>"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "-Intermediario<br>"
            if (document.getElementById("<%= txtPrecioNegSucio.ClientID %>").value == "")
                strMsjCampOblig += "-Precio Negociación Sucio<br>"
            if (document.getElementById("<%= txtNroPapeles.ClientID %>").value == "")
                strMsjCampOblig += "-Número Papeles<br>"
            if (document.getElementById("<%= txtHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "-Hora Operación<br>"
            if (!EsHoraValida(document.getElementById("<%= txtHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "-Formato de Hora Incorrecto<br>"
            if (document.getElementById("<%= ddlGrupoInt.ClientID %>").value == "")
                strMsjCampOblig += "-Grupo de Intermediarios<br>"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + "<p align='left'>" + strMsjCampOblig + "</p><br>";
                return false;
            }
            {
                return true;
            }
        }
        function cambiaCantidad(num) {
            if (num != "") {
                document.getElementById("<%= txtMnomOp.ClientID %>").value = num;
            }
            return false;
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
<body onload="javascript:cambiaTitulo();">
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblTitulo" Text="Orden de Inversión - PAPELES COMERCIALES" runat="server" /></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h2>
                        <asp:Label ID="lblAccion" Text="" runat="server" />
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label runat="server" id="lblFondo" class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="120px" AutoPostBack="true"
                                OnChange="javascript:cambiaTitulo();" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="120px" />
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
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS"  que no se utilizará a sección oculta| 29/05/18 --%>
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
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Código SBS"  que no se utilizará a sección oculta| 29/05/18 --%>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 29/05/18 --%>
            <div class="row">
                <div class="col-sm-12" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
                <div class="col-sm-12" style="text-align: right;">
                    <asp:Button Text="Caracter&iacute;sticas" runat="server" ID="btnCaracteristicas" />
                </div>
            </div>
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta y se deja botón 'Características' | 29/05/18 --%>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos de Operación</legend>
            <asp:UpdatePanel ID="up2" runat="server">
                <ContentTemplate>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Operación</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" Width="100px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Fecha Liquidación</label>
                                <div class="col-sm-6">
                                    <div runat="server" id="imgFechaVcto" class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" style="border-color:Red;" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Hora Operación"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Monto Nominal Operación"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">

                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Monto Nominal Operación"  que no se utilizará a sección oculta| 29/05/18 --%>
                    
                     <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">

                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    YTM %</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtYTM" Width="150px" CssClass="Numbox-7_12" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Nro Papeles</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtNroPapeles" Width="150px" CssClass="Numbox-7_12" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">Valor Nominal Unidad</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="lblUnidades" Width="125px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Tipo Tasa</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList runat="server" ID="ddlTipoTasa" Width="150px"  />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Monto Nominal Total</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtMnomOrd" Width="150px" CssClass="Numbox-7_12" onblur="cambiaCantidad(this.value);" ReadOnly="True" />
                                </div>

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

                        <div class="col-sm-4">
                            <div class="form-group">

                            </div>
                        </div>
                    </div>

                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Calculado %"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">Precio (Sucio) %</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtPrecioNegoc" Width="150px" CssClass="Numbox-7_12" ReadOnly="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Precio (Limpio) %</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtPrecioLimpio" Width="150px" CssClass="Numbox-7_12" ReadOnly="True" /></div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Calculado %"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Negociación Sucio %", "Interés Corrido"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">Monto Operación</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtMontoOperacional" Width="150px" CssClass="Numbox-2" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">Inter&eacute;s Corrido</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtInteresCorNeg" Width="150px" CssClass="Numbox-7_12" ReadOnly="True" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Precio Negociación Sucio %" , "Interés Corrido"  que no se utilizará a sección oculta| 29/05/18 --%>

                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Grupo de Intermediarios</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList runat="server" ID="ddlGrupoInt" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Intermediario</label>
                                <div class="col-sm-6">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlIntermediario" Width="150px" AutoPostBack="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlGrupoInt" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Contacto"  que no se utilizará a sección oculta| 29/05/18 --%>
                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Observación" que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row">
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
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Categoría Contable</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList runat="server" ID="ddlCategoriaContable" Enabled="false" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladó campo "Observación" que no se utilizará a sección oculta| 29/05/18 --%>
                    <div class="row" style="display: none">
                        <div class="col-sm-2">
                        </div>
                        <asp:CheckBox Text="Regularización SBS" runat="server" ID="chkRegulaSBS" />
                    </div>
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
                                <label class="col-sm-6 control-label">
                                    Comentarios</label>
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="2" Width="300px" />
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
                        
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlGrupoInt" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlIntermediario" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
<%--        <div class="row">
            <asp:CheckBox ID="chkRecalcular" Text="Recalcular" runat="server" Checked="True" />
        </div>--%>
        <fieldset>
            <legend>Comisiones y Gastos de Administradora</legend>
            <div class="grilla">
                <asp:UpdatePanel ID="id1" runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="dgLista" SkinID="Grid" DataKeyNames="codigoComision2">
                            <Columns>
                                <asp:BoundField DataField="codigoComision1" HeaderText="Código Impuesto/Comisión"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisión" />
                                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisión" />
                                <asp:BoundField DataField="strValorCalculadoComision1" HeaderText="Comisión" ItemStyle-CssClass="hidden"
                                    HeaderStyle-CssClass="hidden" />
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision1" runat="server" Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ValorOcultoComision1" HeaderText="ValorOcultoComision1"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="codigoComision2" HeaderText="Código Impuesto/Comisión"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisión" />
                                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisión" />
                                <asp:BoundField DataField="strValorCalculadoComision2" HeaderText="Comisión" ItemStyle-CssClass="hidden"
                                    HeaderStyle-CssClass="hidden" />
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision2" runat="server" Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ValorOcultoComision2" HeaderText="ValorOcultoComision2"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPlaza" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Total Comisiones</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txttotalComisionesC" Width="150px" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Monto Neto Operación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtMontoNetoOpe" Width="150px" CssClass="stlCajaBloqueadoNumero"
                                        ForeColor="Black" MaxLength="20" />
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
                <div class="col-sm-6">
                    <asp:Button Text="Cuponera" runat="server" ID="btnCuponera" CausesValidation="false" />
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                    <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                </div>
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" CausesValidation="false" />
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
    <asp:HiddenField runat="server" ID="hdMensaje" />
    <asp:HiddenField runat="server" ID="txtCodigoOrden" />
    <asp:HiddenField runat="server" ID="hdPagina" />
    <asp:HiddenField runat="server" ID="hdSaldo" />
    <asp:HiddenField runat="server" ID="hdCustodio" />
    <asp:HiddenField runat="server" ID="hdNumUnidades" />
    <asp:HiddenField runat="server" ID="hdFechaOperacion" />
    <asp:HiddenField runat="server" ID="hdPopUp" />
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de confirmación | 07/06/18 --%>
   
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 25/05/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS </legend>
        <div class="row">
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
            <div class="col-sm-4">
            </div>
        </div>
        <br />
        <fieldset>
            <legend>Caracter&iacute;sticas del Valor</legend>
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
                            <asp:TextBox runat="server" ID="lblemisor" Width="125px" ReadOnly="true" />
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
        <br />
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Hora Operación
                    </label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtHoraOperacion" SkinID="Hour" />
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
                        <asp:TextBox runat="server" ID="txtMnomOp" Width="150px" CssClass="Numbox-7_12" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Precio Calculado %</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="lblPrecioCal" Width="150px" ReadOnly="true" CssClass="Numbox-4" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Precio Negociación Sucio %</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtPrecioNegSucio" Width="150px" CssClass="Numbox-4" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Inter&eacute;s Corrido</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="lblInteresCorrido" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="lNPoliza" class="col-sm-6 control-label hidden">
                        Nro. Poliza</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="tbNPoliza" Width="150px" Visible="False" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label runat="server" id="Label1" class="col-sm-6 control-label">
                        Contacto</label>
                    <div class="col-sm-6">
                        <asp:DropDownList runat="server" ID="ddlContacto" Width="150px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-6 control-label">
                        Observación</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtObservacion" Width="350px" ReadOnly="true" Style="text-transform: uppercase;"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF009 - Se trasladaron los campos que no se utilizará a sección oculta | 25/05/18 --%>
    </form>
</body>
</html>

