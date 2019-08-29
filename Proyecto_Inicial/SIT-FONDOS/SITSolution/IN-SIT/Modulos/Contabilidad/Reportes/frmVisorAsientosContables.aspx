<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorAsientosContables.aspx.vb"
    Inherits="Modulos_Contabilidad_Reportes_frmVisorAsientosContables" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
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
