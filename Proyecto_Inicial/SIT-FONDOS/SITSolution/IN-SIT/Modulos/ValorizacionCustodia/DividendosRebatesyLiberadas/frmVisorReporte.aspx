<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorReporte.aspx.vb" Inherits="Modulos_ValorizacionCustodia_DividendosRebatesyLiberadas_frmVisorReporte" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Visor Reporte</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" Width="350px" Height="50px" />
    </div>
    </form>
</body>
</html>
