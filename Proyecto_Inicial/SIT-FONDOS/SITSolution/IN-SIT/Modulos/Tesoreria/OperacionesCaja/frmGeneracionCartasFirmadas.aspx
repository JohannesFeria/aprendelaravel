<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGeneracionCartasFirmadas.aspx.vb"
    Inherits="Modulos_Tesoreria_OperacionesCaja_frmGeneracionCartasFirmadas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Impresi&oacute;n</title>
    <script type="text/javascript">
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
				(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
				(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
        function SeleccionarCartas() {
            var i;
            var count;
            count = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
			(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                    if (document.forms[0].elements[i].disabled != true) {
                        if (document.forms[0].elements[i].checked) {
                            count = count + 1;
                        }
                    }
                }
            }
            if (count > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%= tbRangoInicial.ClientID %>").value == "")
                strMsjCampOblig += "\t-De (Rango Inicial)\n"
            if (document.getElementById("<%= tbRangoFinal.ClientID %>").value == "")
                strMsjCampOblig += "\t-a (Rango Final)\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }

        function ValidarImpresion() {
            strMensajeError = "";
            if (ValidaCamposObligatorios()) {
                return true;
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }

        function ValidarSeleccion() {
            if (SeleccionarCartas())
            { return true; }
            else {
                alertify.alert('Debe seleccionar algún registro! ');
                return false;
            }
        }

        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }

        $(document).ready(function () {
            $("#btnBuscar").click(function () {
                ShowProgress();
            });
            $("#btnAceptarImp").click(function () {
                ShowProgress();
            });
            $("#btnVista").click(function () {
                ShowProgress();
            });
            $("#btnImprimir").click(function () {
                ShowProgress();
            });

        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Impresi&oacute;n
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Banco</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Liquidaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFecha" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Intermediario</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlIntermediario" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Estado Impresi&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlEstado" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <div id="divRangoImpresion" runat="server" class="row">
            <br />
            <div class="row">
                <div class="col-md-6">
                    De&nbsp;:&nbsp;<asp:TextBox runat="server" ID="tbRangoInicial" Width="70px" />&nbsp;&nbsp;&nbsp;A&nbsp;:&nbsp;<asp:TextBox
                        runat="server" ID="tbRangoFinal" Width="70px" />&nbsp;&nbsp;<asp:Button Text="Aceptar"
                            runat="server" ID="btnAceptarImp" />&nbsp;<asp:Button Text="Cancelar" ID="btnCancelarImp"
                                runat="server" />
                </div>
            </div>
            <div class="row">
                <span class="validator">Ingrese el rango de numero de cartas de impresión</span>
            </div>
        </div>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox" id="chkSelectAll">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            <asp:Label ID="lbEstado" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Estado") %>'>
                            </asp:Label>
                            <asp:Label ID="lbCodigo" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoImpresion") %>'>
                            </asp:Label>
                            <asp:Label ID="lbCodigoOp" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacionCaja") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NumeroCarta" HeaderText="Nro. Carta" />
                    <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" />
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operaci&#243;n" />
                    <asp:BoundField DataField="DescripcionBanco" HeaderText="Banco" />
                    <asp:BoundField DataField="DescripcionPago" HeaderText="Forma de Pago" />
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Importe" HeaderText="Monto" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="NumeroOrden" HeaderText="Codigo Orden" />
                    <asp:BoundField DataField="DescripcionEstado" HeaderText="Estado" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-2">
            </div>
            <div class="col-md-10" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Vista" runat="server" ID="btnVista" />
                <asp:Button Text="Anular Impresi&oacute;n" runat="server" ID="btnAnularImp" />
                <asp:Button Text="Perdida Impresi&oacute;n" runat="server" ID="btnPerdidaImp" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
