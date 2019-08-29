<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorReporteLimitesV2.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Limites_frmVisorReporteLimitesV2" %>
<%@ Register TagPrefix="cr1" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title></title>
</head>
<body> 
    <form id="form1" runat="server"> 
    <div>
        <cr1:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            BestFitPage="False" HasCrystalLogo="False" Width="100%" EnableDrillDown="False" 
            ToolPanelView="None"></cr1:CrystalReportViewer>
    </div>
    </form>
</body>
</html>