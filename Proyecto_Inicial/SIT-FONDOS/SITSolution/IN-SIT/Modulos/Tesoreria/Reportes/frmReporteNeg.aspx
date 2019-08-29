<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteNeg.aspx.vb" Inherits="Modulos_Tesoreria_Reportes_frmReporteNeg" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="cr1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr1:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Height="50px" Width="350px" />
    </div>
    </form>
</body>
</html>
