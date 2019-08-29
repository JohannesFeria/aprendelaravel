<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaAplicacion.aspx.vb"
    Inherits="Modulos_Contabilidad_frmBusquedaAplicacion" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Firma de Documentos</title>
</head>
<body> 
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Aplicación
                    </h2>
                </div>
            </div>
        </header>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <HeaderStyle Width="80px"></HeaderStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionar" runat="server" OnCommand="Seleccionar" SkinID="imgCheck"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Campo") %>' CausesValidation="False">
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Campo" HeaderText="Aplicación"></asp:BoundField>
                </Columns>
            </asp:GridView>
            </ContentTemplate>
            </asp:UpdatePanel>            
        </div>
    </div>
    <asp:HiddenField ID="hd" runat="server" />
    </form>
</body>
</html>
