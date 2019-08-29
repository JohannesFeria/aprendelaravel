<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaInstrumentos.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmBusquedaInstrumentos" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Búsqueda Valores</title>
    <script type="text/javascript">
        function Cerrar() {
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Búsqueda Valores</h2></header>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <asp:label id="lbContador" runat="server"></asp:label>
        </fieldset>
        <br />
        <div class="grilla">
        <asp:UpdatePanel ID="UP1" runat="server">        
        <ContentTemplate>
            <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">                
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnSalir" runat="server" OnCommand="SeleccionarISIN" SkinID="imgCheck"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Código ISIN")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Código Mnemónico")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Código SBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Descripción") %>'
                                CausesValidation="False"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="C&#243;digo ISIN" HeaderText="C&#243;digo ISIN"></asp:BoundField>
                    <asp:BoundField DataField="C&#243;digo Mnem&#243;nico" HeaderText="C&#243;digo Mnem&#243;nico"></asp:BoundField>
                    <asp:BoundField DataField="C&#243;digo SBS" HeaderText="C&#243;digo SBS"></asp:BoundField>
                    <asp:BoundField DataField="Descripci&#243;n" HeaderText="Descripci&#243;n"></asp:BoundField>
                    <asp:BoundField DataField="TipoInstrumento" HeaderText="Tipo de Instrumento"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>        
        </asp:UpdatePanel>
        </div>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClientClick="Cerrar();" />
        </div>
    </div>
    </form>
</body>
</html>
