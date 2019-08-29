<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVencimientoForwardNoDelivery.aspx.vb"
    Inherits="Modulos_Inversiones_frmVencimientoForwardNoDelivery" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>VencimientoForwardNoDelivery</title>
    <script type="text/javascript">
        function Confirmar() {
            if (confirm("¿Desea Generar el Vencimiento seleccionado?")) return true;
            else return false;
        }
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
            } else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
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
            $("#ibBuscar").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>Pre-liquidación FW Nom-Delivery</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fondo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFondo" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMercado" runat="server" AutoPostBack="True" Width="200px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-1" style="text-align: right;">
                    <asp:Button ID="ibBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda Negociada</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMonedaNegociada" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda destino</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMonedaDestino" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Vencimiento</label>
                        <div class="col-sm-8 control-label" style="text-align: left;">
                            Desde&nbsp;
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVencimientoDesde" SkinID="Date" />
                                <span class="add-on" id="imgFechaDesde"><i class="awe-calendar"></i></span>
                            </div>
                            &nbsp;&nbsp;Hasta&nbsp;
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVencimientoHasta" SkinID="Date" />
                                <span class="add-on" id="imgFechaHasta"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <div class="col-sm-12 control-label">
                            <asp:RadioButtonList ID="rbnCalculo" runat="server" RepeatDirection="Horizontal"
                                Width="360px">
                                <asp:ListItem Value="1">Calculado</asp:ListItem>
                                <asp:ListItem Value="2" Selected="True">No Calculado</asp:ListItem>
                                <asp:ListItem Value="3">Todas</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row" style="text-align: center;">
            Vencimientos
            <asp:Label ID="lblCantidad" runat="server">(0)</asp:Label>
        </div>
        <div id="divProgress" class="loading" style="text-align: center;">
            Procesando...<br />
            <br />
            <img src="../../App_Themes/img/icons/ajax-loader.gif" alt="" />
        </div>
        <div class="grilla">
            <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                <Columns>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Fondo">
                        <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operación">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaContrato" HeaderText="Fecha Vencimiento">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Liquidación">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Operacion" HeaderText="Operación">
                        <ItemStyle HorizontalAlign="Left" Width="60px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda">
                        <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoMonedaDestino" HeaderText="Moneda Destino">
                        <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MontoCancelar" HeaderText="Monto pactado" DataFormatString="{0:#,##0.00}"
                        HtmlEncodeFormatString="false">
                        <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoCambioFuturo" HeaderText="TC(pactado)" DataFormatString="{0:#,##0.0000}"
                        HtmlEncodeFormatString="false">
                        <ItemStyle HorizontalAlign="Right" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Intermediario" HeaderText="Intermediario">
                        <ItemStyle HorizontalAlign="Left" Width="140px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="hidden"
                        ItemStyle-CssClass="hidden">
                        <ItemStyle HorizontalAlign="Left" Width="140px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoOrden" HeaderText="Cod Orden">
                        <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="hidden"
                        ItemStyle-CssClass="hidden"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSeleccion" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                        <ItemStyle Width="30px"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fixing</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtFixing" runat="server" Width="122px" CssClass="Numbox-7" />
                    </div>
                </div>
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button ID="ibProcesar" runat="server" Text="Procesar" />
                <asp:Button ID="ibSalir" runat="server" Text="Salir" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
