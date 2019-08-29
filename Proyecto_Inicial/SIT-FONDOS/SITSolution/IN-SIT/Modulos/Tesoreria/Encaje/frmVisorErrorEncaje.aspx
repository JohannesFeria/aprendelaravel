<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorErrorEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmVisorErrorEncaje" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
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
    <div class="row">
    <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClientClick="Cerrar();" />
    <br />
        <CR:CrystalReportViewer ID="CrNemonico" runat="server" AutoDataBind="true" />
    </div>
    </div>
    </form>
</body>
</html>
