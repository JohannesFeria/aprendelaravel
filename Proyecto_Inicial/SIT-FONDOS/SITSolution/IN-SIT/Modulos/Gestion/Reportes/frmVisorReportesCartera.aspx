<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorReportesCartera.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmVisorReportesCartera" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Height="50px" Width="350px"
            HasZoomFactorList="False"></cr:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
