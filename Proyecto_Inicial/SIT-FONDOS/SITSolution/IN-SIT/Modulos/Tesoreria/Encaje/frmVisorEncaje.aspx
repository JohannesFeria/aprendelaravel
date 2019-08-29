<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmVisorEncaje" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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
    <asp:Button ID="btnRetornar" runat="server" Text="Retornar" OnClientClick="Cerrar();" Visible="false" />
    <br />
        <CR:CrystalReportViewer ID="crEncaje" runat="server" Width="350px" Height="50px" />
    </div>
    </form>
</body>
</html>
