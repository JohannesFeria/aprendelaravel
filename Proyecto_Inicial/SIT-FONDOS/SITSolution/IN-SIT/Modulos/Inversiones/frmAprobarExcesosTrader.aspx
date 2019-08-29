<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAprobarExcesosTrader.aspx.vb"
    Inherits="Modulos_Inversiones_frmAprobarExcesosTrader" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Aprobar Excesos por Trader</title>
    
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Label Text="text" runat="server" ID="lbEstado" />
            </div>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Salir" runat="server" ID="btnSalir" Visible ="false"/>
        </div>
    </div>
    </form>
</body>
</html>
