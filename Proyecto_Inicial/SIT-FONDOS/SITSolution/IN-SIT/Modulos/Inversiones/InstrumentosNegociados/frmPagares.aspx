<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPagares.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmPagares" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pagaré</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

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
                strMsjCampOblig += "\t-Portafolio<br />"
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br />";
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
                if (document.getElementById("ddlFondo").value != 'MULTIFONDO') {
                    strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de Fondo del Pagaré?"
                }
                else {
                    strMensaje = "¿Desea cancelar " + strAccion + " de la Pre-Orden de Fondo del Pagaré?"
                }

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
                alertify.alert(strMensajeError);
                //alert(strMensajeError);
                return false;
            }
        }
        function ValidaCampos() {
            var strMsjCampOblig = "";
            if (document.getElementById("<%= ddlFondo.ClientID %>").value == "")
                strMsjCampOblig += "\t-Portafolio<br />"
            if (document.getElementById("<%= ddlOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Operación<br />"
            if (document.getElementById("<%= txtISIN.ClientID %>").value == "")
                strMsjCampOblig += "\t-Código ISIN<br />"
            if (document.getElementById("<%= txtMnemonico.ClientID %>").value == "")
                strMsjCampOblig += "\t-Código Mnemónico<br />"
            if (document.getElementById("<%= txtSBS.ClientID %>").value == "")
                strMsjCampOblig += "\t-Código SBS<br />"
            if (document.getElementById("<%= tbFechaOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Operación<br />"
            if (document.getElementById("<%= tbFechaLiquidacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Fecha Vencimiento<br />"
            if (document.getElementById("<%= txtMnomOrd.ClientID %>").value == "")
                strMsjCampOblig += "\t-Monto Nominal Ordenado<br />"
            if (document.getElementById("<%= txtMnomOp.ClientID %>").value == "")
                strMsjCampOblig += "\t-Monto Nominal Operación<br />"
            if (document.getElementById("<%= ddlTipoTasa.ClientID %>").value == "")
                strMsjCampOblig += "\t-Tipo Tasa<br />"
            if (document.getElementById("<%= txtYTM.ClientID %>").value == "")
                strMsjCampOblig += "\t-YTM<br />"
            if (document.getElementById("<%= txtPrecioNegoc.ClientID %>").value == "")
                strMsjCampOblig += "\t-Precio Negociación<br />"
            if (document.getElementById("<%= txtInteresCorNeg.ClientID %>").value == "")
                strMsjCampOblig += "\t-Interés Corrido Negociado<br />"
            if (document.getElementById("<%= ddlIntermediario.ClientID %>").value == "")
                strMsjCampOblig += "\t-Intermediario<br />"
            if (document.getElementById("<%= txtPrecioNegSucio.ClientID %>").value == "")
                strMsjCampOblig += "\t-Precio Negociación Sucio<br />"
            if (document.getElementById("<%= txtNroPapeles.ClientID %>").value == "")
                strMsjCampOblig += "\t-Número Papeles<br />"
            if (document.getElementById("<%= tbHoraOperacion.ClientID %>").value == "")
                strMsjCampOblig += "\t-Hora Operación<br />"
            if (!EsHoraValida(document.getElementById("<%= tbHoraOperacion.ClientID %>").value))
                strMsjCampOblig += "\t-Formato de Hora Incorrecto<br />"
            if (document.getElementById("<%= ddlGrupoInt.ClientID %>").value == "")
                strMsjCampOblig += "\t-Grupo de Intermediarios<br />"
            if (strMsjCampOblig != "") { strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "<br />"; return false; }
            { return true; }
        }

        function cambiaCantidad(num) {
            if (num != "") {
                document.getElementById("<%= txtMnomOp.ClientID %>").value = num;
            }
            return false;
        }

    </script>
</head>
<body>
    <form id="frmInvocador" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Ordenes de Inversión - Pagaré</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
        </header>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblFondo" runat="server">Portafolio</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlFondo" runat="server" Enabled="False" AutoPostBack="True"
                            Width="120px">
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
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
            </div>
        </div>
        <br />
        <fieldset>
            <legend>Características del Valor</legend>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Descripción Instrumento</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lbldescripcion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Fecha Fin Bono</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblfecfinbono" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Nominales emitidos</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblduracion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Emisor</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblemisor" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            % Participación</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblparticipacion" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Base Cupón</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblbasetir" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Fecha Ult. Cupón</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblFecUltCupon" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Precio Vector</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblpreciovector" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Base Tir</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblbasecupon" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Fecha Prox. Cupón</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblFecProxCupon" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Nominales por Unidad</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblunidades" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Duración</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblnominales" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button runat="server" ID="btnCaracteristicas" Text="Características" />
                </div>
            </div>
            <div class="row oculto">
                <div class="col-md-4">
                </div>
                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Rescate</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblRescate" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
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
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Width="120px" />
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
                        <label class="col-sm-4 control-label">
                            Hora Operación</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbHoraOperacion" runat="server" SkinID="Hour" Width="70px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Monto Nominal Ordenado
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtMnomOrd" CssClass="Numbox-7" runat="server" Width="120px" MaxLength="22"
                                ReadOnly="true" onblur="cambiaCantidad(this.value);" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4 oculto">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Monto Nominal Operación
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtMnomOp" CssClass="Numbox-7" runat="server" Width="120px" MaxLength="22"
                                ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Tipo Tasa</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlTipoTasa" runat="server" CssClass="stlCajaTexto" Width="120px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            YTM %</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtYTM" CssClass="Numbox-7_12" runat="server" Width="120px" MaxLength="22"
                                ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Precio Negociación Limpio %</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtPrecioNegoc" CssClass="Numbox-7_32" runat="server" Width="120px"
                                ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4 oculto">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Precio Calculado %</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblPrecioCal" CssClass="Numbox-7" runat="server" Width="96px" MaxLength="22"
                                ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Precio Negociación Sucio %</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtPrecioNegSucio" CssClass="Numbox-7_31" runat="server" Width="120px"
                                MaxLength="22" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Interés Corrido Negociado</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtInteresCorNeg" CssClass="Numbox-7_31" runat="server" Width="120px"
                                MaxLength="22" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4 oculto">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Interés Corrido</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="lblInteresCorrido" runat="server" Width="96px" ReadOnly="true" />
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
                            <asp:TextBox ID="txtMontoOperacional" CssClass="Numbox-7_32" runat="server" Width="120px"
                                MaxLength="22" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Nro Papeles</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtNroPapeles" CssClass="Numbox-0_12" runat="server" Width="120px"
                                MaxLength="22" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label id="lNPoliza" runat="server" visible="false" class="col-sm-4 control-label">
                            Nro.Poliza</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="tbNPoliza" runat="server" Width="96px" ReadOnly="true" Visible="false"
                                MaxLength="15" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Grupo de Intermediarios</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlGrupoInt" runat="server" AutoPostBack="True" Enabled="False"
                                        Width="160px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-6 control-label">
                                    Intermediario</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlIntermediario" runat="server" AutoPostBack="True" Enabled="False"
                                        Width="160px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 oculto">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Contacto</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlContacto" runat="server" Enabled="False" Width="160px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row oculto">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Bolsa</label>
                        <div class="col-sm-5">
                            <asp:DropDownList ID="ddlPlaza" runat="server" Enabled="False" AutoPostBack="True"
                                Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Observación</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtObservacion" runat="server" Width="500px" MaxLength="20" Style="text-transform: uppercase" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row hidden">
                <div class="col-md-4">
                    <div class="form-group">
                        <div class="col-sm-6">
                            <asp:CheckBox ID="chkRegulaSBS" runat="server" Text="Regularización SBS" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row oculto" id="trMotivoCambio" runat="server">
                <div class="col-md-4 oculto">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Motivo de cambio</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlMotivoCambio" runat="server" AutoPostBack="True" Width="160px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            <asp:Label ID="lblComentarios" runat="server">Comentarios</asp:Label></label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtComentarios" runat="server" Width="350px" MaxLength="150" Rows="2"
                                TextMode="MultiLine" CssClass="mayusculas"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <fieldset>
            <legend>Comisiones y Gastos de Administradora</legend>
            <div class="grilla">
                <asp:UpdatePanel ID="up1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="dgLista" runat="server" SkinID="Grid" DataKeyNames="codigoComision2">
                            <Columns>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="codigoComision1"
                                    HeaderText="Código Impuesto/Comisión"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion1" HeaderText="Impuesto/Comisión">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision1" HeaderText="Porcentaje Comisión">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="strValorCalculadoComision1"
                                    HeaderText="Comisión"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision1" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorOcultoComision1"
                                    HeaderText="ValorOcultoComision1"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="codigoComision2"
                                    HeaderText="Código Impuesto/Comisión"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion2" HeaderText="Impuesto/Comisión">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="porcentajeComision2" HeaderText="Porcentaje Comisión">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="strValorCalculadoComision2"
                                    HeaderText="Comisión"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor Comisión">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorComision2" runat="server" CssClass="stlCajaTextoNumero"
                                            Width="200px" MaxLength="23"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="ValorOcultoComision2"
                                    HeaderText="ValorOcultoComision2"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPlaza" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <header>
            </header>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row oculto">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Total Comisiones</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txttotalComisionesC" runat="server" Width="230px" MaxLength="22"
                                        ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Monto Neto Operación</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtMontoNetoOpe" runat="server" Width="230px" MaxLength="22" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblPrecioPromedio" runat="server" visible="false" class="col-sm-4 control-label">
                                    Precio Promedio</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPrecPromedio" Visible="False" runat="server" Width="230px" MaxLength="22"
                                        ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="row" style="text-align: right;">
                <asp:Button runat="server" ID="btnAsignar" Text="Asignar" />
                <asp:Button runat="server" ID="btnProcesar" Text="Procesar" OnClientClick="return Validar();" />
            </div>
            <div class="row">
                <div class="col-md-6">
                    <asp:Button runat="server" ID="btnCuponera" Text="Cuponera" />
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" />
                </div>
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <div class="col-md-8">
                <asp:Button runat="server" ID="btnIngresar" Text="Ingresar" />
                <asp:Button runat="server" ID="btnModificar" Text="Modificar" />
                <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" />
                <asp:Button runat="server" ID="btnConsultar" Text="Consultar" />
            </div>
            <div class="col-md-4" style="text-align: right;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnRetornar" Text="Salir" Visible="False" />
                        <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" />
                        <asp:Button runat="server" ID="btnSalir" Text="Salir" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    </Triggers>
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
    <asp:HiddenField ID="hdUnitarias" runat="server" />
    <asp:HiddenField ID="hfModal" runat="server" />
    </form>
</body>
</html>
