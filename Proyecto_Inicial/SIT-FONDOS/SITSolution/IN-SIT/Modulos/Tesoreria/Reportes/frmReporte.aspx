<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporte.aspx.vb" Inherits="Modulos_Tesoreria_Reportes_frmReporte" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:100%;">
        <cr:CrystalReportViewer ID="CrystalReportViewer1" runat="server" BestFitPage="True" Height="100%" Width="100%" style="width: 100%; height: 100%;" > 
        </cr:crystalreportviewer>
    </div>
    </form>
</body>
</html>
