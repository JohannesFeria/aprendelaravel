<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorOperacionesNoContabilizadas.aspx.vb" Inherits="Modulos_Contabilidad_frmVisorOperacionesNoContabilizadas" %>
<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Operaciones no Contabilizadas</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Height="50px" Width="350px">
        </cr:CrystalReportViewer>
    </div>
    </form>
</body>
</html>