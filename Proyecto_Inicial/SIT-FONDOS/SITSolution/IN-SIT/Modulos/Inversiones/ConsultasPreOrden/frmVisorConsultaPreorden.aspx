<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorConsultaPreorden.aspx.vb" Inherits="Modulos_Inversiones_ConsultasPreOrden_frmVisorConsultaPreorden" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Consulta de Pre Ordenes y Ordenes de Inversión</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
        <br />
		<CR:CrystalReportViewer id="crvisor" runat="server" Width="350px" Height="50px"></CR:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
