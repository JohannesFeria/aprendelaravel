<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorReporteOrdenesInversion.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Orden_de_Inversion_frmfrmVisorReporteOrdenesInversion" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte Ordenes de Inversión</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>        
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"  Height="50px" Width="350px"/>
    </div>
    </form>
</body>
</html>
