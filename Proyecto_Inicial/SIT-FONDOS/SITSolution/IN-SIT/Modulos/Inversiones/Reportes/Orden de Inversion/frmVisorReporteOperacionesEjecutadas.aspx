<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorReporteOperacionesEjecutadas.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Orden_de_Inversion_frmVisorReporteOperacionesEjecutadas" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<head runat="server">
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
