<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmValidacionExcesosPrevOI.aspx.vb"
    Inherits="Modulos_Inversiones_frmValidacionExcesosPrevOI" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Validaci&oacute;n de Excesos</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Validacion de Excesos -
                        <asp:Label Text="" runat="server" ID="lblTipo" />
                    </h2>
                </div>
            </div>
        </header>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbHeader1" runat="server" Visible="False">Codigo Orden</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbCodigoOrden" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOrden") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoNemonico" HeaderText="Nemonico" />
                    <asp:BoundField DataField="Operacion" HeaderText="Operacion" />
                    <asp:BoundField DataField="Intermediario" HeaderText="Intermediario" />
                    <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
                    <%--ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbHeader2" runat="server" Visible="False">Unidades</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbCantidad" runat="server" Text='<%# String.Format("{0:###,###}",DataBinder.Eval(Container, "DataItem.Cantidad")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbHeader3" runat="server" Visible="False">Precio Prom.</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbPrecioPromedio" runat="server" Text='<%# String.Format("{0:###,##0.0000}",DataBinder.Eval(Container, "DataItem.PrecioPromedio")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <HeaderTemplate>
                            <asp:Label ID="lbHeader4" runat="server">Monto Nominal</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbMontoNominal" runat="server" Text='<%# String.Format("{0:###,###.00}",DataBinder.Eval(Container, "DataItem.MontoNominal")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbHeader5" runat="server" Visible="False">Monto Operación</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbMontoOperacion" runat="server" Text='<%# String.Format("{0:###,###.00}",DataBinder.Eval(Container, "DataItem.MontoOperacion")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Alerta">
                        <ItemTemplate>
                            <asp:Label ID="lbEstadoLimites" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EstadoLimites") %>'
                                ForeColor="Red" Font-Bold="True">
                            </asp:Label>
                            <asp:Label ID="lbCodigoOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacion") %>'
                                Visible="False">
                            </asp:Label>
                            <asp:Label ID="lbCodigoTercero" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CodigoTercero") %>'
                                Visible="False">
                            </asp:Label>
                            <asp:Label ID="lbCodigoNemonico" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CodigoNemonico") %>'
                                Visible="False">
                            </asp:Label>
                            <asp:Label ID="lbPortafolio" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Fondo") %>'
                                Visible="False">
                            </asp:Label>
                            <asp:Label ID="lbInstrumento" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Instrumento") %>'
                                Visible="False">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EstadoBroker" HeaderText="Est. Broker">
                        <ItemStyle Font-Bold="True" ForeColor="Red"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <asp:Label Text="" runat="server" ID="lblMensaje" CssClass="validator" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnRetornar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
