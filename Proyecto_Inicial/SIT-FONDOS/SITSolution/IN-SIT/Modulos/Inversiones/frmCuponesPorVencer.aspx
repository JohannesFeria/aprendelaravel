<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCuponesPorVencer.aspx.vb" Inherits="Modulos_Inversiones_frmCuponesPorVencer" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Notificaciones de Importancia</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Notificaciones de Importancia</h2></header>
    <br />
    <div class="grilla">
        <asp:GridView ID="dgCupones" runat="server" AutoGenerateColumns="False" SkinID="Grid" >            
            <Columns>
                <asp:BoundField DataField="CodigoNemonico" HeaderText="Nemónico"></asp:BoundField>
                <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio"></asp:BoundField>
                <asp:BoundField DataField="Hecho" HeaderText="Descripci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha Término"></asp:BoundField>
                <asp:BoundField DataField="Dias" HeaderText="Dias"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
    </div>
    </div>
    </form>
</body>
</html>
