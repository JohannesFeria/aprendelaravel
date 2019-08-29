<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTitulosAsociadosCustodios.aspx.vb"
    Inherits="Modulos_ValorizacionCustodia_Custodia_Reportes_frmTitulosAsociadosCustodios" %>

<%@ Register TagPrefix="cr1" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<head id="Head1" runat="server">
    <title>Conciliaci&oacute;n Inf. Custodios</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr1:CrystalReportViewer ID="crTitulosAsociados" runat="server" BestFitPage="False"
            HasCrystalLogo="False" Width="100%"></cr1:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
