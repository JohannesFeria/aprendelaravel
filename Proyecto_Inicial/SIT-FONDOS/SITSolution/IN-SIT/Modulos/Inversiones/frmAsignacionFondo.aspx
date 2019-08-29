<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAsignacionFondo.aspx.vb" Inherits="Modulos_Inversiones_frmAsignacionFondo" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Ingreso de Capital Comprometido</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Asignación de Fondos</h2>
                </div>
            </div>
        </header>
        <br />
        <div class="grilla">
            <asp:UpdatePanel runat="server" ID="updGrilla">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="GridFooter" ID="dgLista">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPrevOrden") %>' />
                                    <asp:ImageButton ID="ibdelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("CodigoPrevOrden") %>'
                                        SkinID="imgDelete" AlternateText="Eliminar" />
                                    <asp:Label ID="lbCodigoPrevOrden" CssClass="stlPaginaTexto" runat="server" Text='<%# Eval("CodigoPrevOrden") %>'
                                        Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ibagregar" runat="server" CommandName="Add" AlternateText="Agregar" SkinID="imgAdd" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Portafolio" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPortafolio" runat="server" Width="200px" Enabled="false" />
                                    <asp:Label ID="lblportafolio" runat="server" Text='<%# Eval("CodigoPortafolio") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlPortafolioF" runat="server" Width="200px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Monto Asignado" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMontoAsignado" CssClass="Numbox-7" Width="250px" runat="server" Text='<%# Eval("Asignacion") %>' Enabled="true" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtMontoAsignadoF" Width="250px"  runat="server" Enabled="true" CssClass="Numbox-7" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
    </div>
    </form>
</body>
</html>
