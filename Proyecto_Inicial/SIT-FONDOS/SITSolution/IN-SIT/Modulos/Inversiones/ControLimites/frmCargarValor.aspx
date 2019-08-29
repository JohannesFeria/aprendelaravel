<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCargarValor.aspx.vb"
    Inherits="Modulos_Inversiones_ControLimites_frmCargarValor" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <base target="_self" />
    <script type="text/javascript">
        function CloseWindow() { window.close(); }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-sm-6">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Búsqueda Valores</asp:Label></h2>
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <div class="grilla">
            <asp:GridView ID="dgLista" runat="server" SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" OnCommand="SeleccionarISIN" SkinID="imgCheck"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Código ISIN")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Código Mnemónico")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Código SBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Descripción")&amp;","&amp;DataBinder.Eval(Container, "DataItem.moneda") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="C&#243;digo ISIN" HeaderText="C&#243;digo ISIN">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="C&#243;digo Mnem&#243;nico" HeaderText="C&#243;digo Mnem&#243;nico">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="C&#243;digo SBS" HeaderText="C&#243;digo SBS">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripci&#243;n" HeaderText="Descripci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="row" style="text-align: right;">
            <asp:Button runat="server" ID="ibCancelar" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>
