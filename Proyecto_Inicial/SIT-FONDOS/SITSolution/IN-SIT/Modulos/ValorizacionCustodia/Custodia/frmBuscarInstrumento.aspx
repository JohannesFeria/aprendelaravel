<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBuscarInstrumento.aspx.vb"
    Inherits="Modulos_Valorizacion_y_Custodia_Custodia_BuscarInstrumento" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <base target="_self">
    <title>Buscar Instrumento</title>
    <script language="javascript" type="text/javascript">    	
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>
                B&uacute;squeda Valores</h2>
        </header>
        <fieldset>
            <legend>Resultados de la b&uacute;squeda</legend>
            <asp:Label ID="lblContador" runat="server"></asp:Label>
            <asp:UpdatePanel ID="UP1" runat="server">        
            <ContentTemplate>
            <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" GridLines="None"
                SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbSeleccionar" Text="Seleccionar" runat="server" CommandName="Seleccionar"
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />
                            <%--<asp:ImageButton ID="Imagebutton3" runat="server" SkinID="imgCheck" onrowcommand="dgLista_RowCommand" CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                        </asp:ImageButton>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Código ISIN" HeaderText="Código ISIN" />
                    <asp:BoundField DataField="Código Mnemónico" HeaderText="Código Mnemónico" />
                    <asp:BoundField DataField="Código SBS" HeaderText="Código SBS" />
                    <asp:BoundField DataField="Descripción" HeaderText="Descripción" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                </Columns>
            </asp:GridView>
            </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button ID="btnCancelar" runat="server" Text="Salir" OnClientClick="window.close();" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
