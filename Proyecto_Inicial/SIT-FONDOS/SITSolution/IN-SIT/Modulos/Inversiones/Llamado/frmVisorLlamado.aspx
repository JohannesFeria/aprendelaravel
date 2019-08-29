<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorLlamado.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_Llamado_frmVisorLlamado" %>

<%@ Register TagPrefix="cr1" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Visor Llamado</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr1:CrystalReportViewer ID="CrystalReportViewer1" runat="server" BestFitPage="False"
            HasCrystalLogo="False" Width="100%"></cr1:CrystalReportViewer>
    </div>
    <asp:HiddenField runat="server" ID="tbFondo" />
    </form>
</body>
</html>
