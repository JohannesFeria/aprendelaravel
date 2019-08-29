<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorOrdenesFaltantes.aspx.vb" Inherits="Modulos_Parametria_frmVisorOrdenesFaltantes" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="cr1" %>

<!DOCTYPE html >

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Cerrar() {
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header></header>
    <div class="row">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClientClick="Cerrar();" />
        <br />
        <cr1:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Width="350px" Height="50px"/>
    </div>
    </div>
    </form>
</body>
</html>
