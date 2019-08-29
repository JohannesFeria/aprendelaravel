<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmHelpControlParametria.aspx.vb" Inherits="Modulos_Parametria_frmHelpControlParametria" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <base target="_self">
    <title>HelpControlParametria</title>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" />
    <div class="container-fluid">
        <header>
            <h2>
                <asp:Label ID="lblTitulo" runat="server"></asp:Label></h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtCodigo" runat="server" Width="150px" CssClass="mayusculas"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="mayusculas"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="text-align: right;">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel runat="server" ID="updGrilla">
                <ContentTemplate>
                    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbSeleccionar" Text="Seleccionar" runat="server" CommandName="Seleccionar"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Codigointerno" ItemStyle-HorizontalAlign="Left" HeaderText="C&#243;digo">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Descripcion" ItemStyle-HorizontalAlign="Left" HeaderText="Descripci&#243;n">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreSituacion" ItemStyle-HorizontalAlign="Left" HeaderText="Situaci&#243;n">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CodigoSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" >
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Cerrar" runat="server" OnClientClick="window.close();" />
        </div>
    </div>
    </form>
</body>
</html>
