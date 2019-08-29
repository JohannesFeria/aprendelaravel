<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmParametriaEncaje.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Encaje_frmParametriaEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Parametría Encaje</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Parametría Encaje</h2></header>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <asp:Label ID="lbContador" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-md-6"></div>
            </div>            
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UP1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Secuencial") %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                            <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:#,##0.00}">
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header></header>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />            
        </div>
    </div>
    </form>
</body>
</html>
