<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTipoDeCambio.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Valorizacion_frmTipoDeCambio" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html >
<%--PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <cr:CrystalReportViewer ID="crTipoDeCambio" runat="server">
        </cr:CrystalReportViewer>
    </form>
</body>
</html>
