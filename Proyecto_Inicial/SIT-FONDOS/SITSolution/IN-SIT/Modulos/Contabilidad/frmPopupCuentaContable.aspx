<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPopupCuentaContable.aspx.vb"
    Inherits="Modulos_Contabilidad_frmPopupCuentaContable" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>PopupCuentaContable</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField HeaderText="Seleccionar">
                                <HeaderStyle Width="100px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibCheck" runat="server" OnCommand="Seleccionar" SkinID="imgCheck"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CuentaContable")&amp;","&amp;DataBinder.Eval(Container, "DataItem.DescripcionCuenta")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Ano") %>'
                                        CausesValidation="False"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Ano" HeaderText="Año"></asp:BoundField>
                            <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta Contable"></asp:BoundField>
                            <asp:BoundField DataField="DescripcionCuenta" HeaderText="Descripción de Cuenta">
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
