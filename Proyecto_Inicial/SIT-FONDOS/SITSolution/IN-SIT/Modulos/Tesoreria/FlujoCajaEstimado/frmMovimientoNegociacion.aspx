<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMovimientoNegociacion.aspx.vb" Inherits="Modulos_Tesoreria_FlujoCajaEstimado_frmMovimientoNegociacion" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Movimientos de Negociaci&oacute;n</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Movimientos de Negociaci&oacute;n
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha
                        </label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFecha" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <header style="text-align: center;">
            <h5>
                Disponible de Caja</h5>
        </header>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="15px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibMenu" runat="server" OnCommand="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Formula") %>'
                                Width="10px" Height="10px" ImageAlign="Middle"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Formula" HeaderText="Formula" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Clasificacion" HeaderText="Clasificacion" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Titulo" />
                    <asp:BoundField DataField="Tercero" HeaderText="Banco" />
                    <asp:BoundField DataField="Soles" HeaderText="Soles" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="Dolares" HeaderText="D&#243;lares" DataFormatString="{0:#,##0.00}" />
                    <asp:TemplateField HeaderText="Liq">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkLiquidado" Enabled="False" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imp">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkImpreso" Enabled="False" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
