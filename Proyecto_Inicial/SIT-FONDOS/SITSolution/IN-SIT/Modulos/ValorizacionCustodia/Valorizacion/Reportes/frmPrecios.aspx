<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPrecios.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Valorizacion_Reportes_frmPrecios" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:CrystalReportViewer ID="crPrecios" runat="server"></cr:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
