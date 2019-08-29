<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorReporteConsolidadoPosiciones.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Consolidado_de_Posiciones_frmVisorReporteConsolidadoPosiciones" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Width="350px" Height="50px"></CR:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
