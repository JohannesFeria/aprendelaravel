<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBuscaValor.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmBuscaValor" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>B&uacute;squeda Valores</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        B&uacute;squeda Valores</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="_seleccionar" ID="lkbSeleccionar" runat="server">Seleccionar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="C&#243;digo ISIN" HeaderText="C&#243;digo ISIN" />
                    <asp:BoundField DataField="C&#243;digo Mnem&#243;nico" HeaderText="C&#243;digo Mnem&#243;nico" />
                    <asp:BoundField DataField="C&#243;digo SBS" HeaderText="C&#243;digo SBS" />
                    <asp:BoundField DataField="Descripci&#243;n" HeaderText="Descripci&#243;n" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
