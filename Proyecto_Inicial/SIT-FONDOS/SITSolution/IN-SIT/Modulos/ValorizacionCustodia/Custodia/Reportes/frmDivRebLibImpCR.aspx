<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDivRebLibImpCR.aspx.vb"
    Inherits="Modulos_ValorizacionCustodia_Custodia_Reportes_frmDivRebLibImpCR" %>

<!DOCTYPE html>
<%@ Register TagPrefix="cr1" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr1:CrystalReportViewer ID="crDivRebLibImp" runat="server"></cr1:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
