<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmHelpControlCuentaContable.aspx.vb"
    Inherits="Modulos_Contabilidad_frmHelpControlCuentaContable" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>HelpControlCuentaContable</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <fieldset>
            <legend>
                <asp:Label ID="lblTitulo" runat="server">Cuentas Contables</asp:Label></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Cuenta Contable</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtCuentaContable" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Descripción</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDescripcion" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="ibBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista_Modal">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionar" runat="server" SkinID="imgCheck" CommandName="Seleccionar"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ano" HeaderText="Año"></asp:BoundField>
                    <asp:BoundField DataField="CuentaContable" ItemStyle-HorizontalAlign="Left" HeaderText="Cuenta Contable">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescripcionCuenta" ItemStyle-HorizontalAlign="Left" HeaderText="Descripción de Cuenta">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ibBuscar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>            
        </div>
    </div>
    </form>
</body>
</html>
