<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorPlanCuentas.aspx.vb"
    Inherits="Modulos_Contabilidad_frmVisorPlanCuentas" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Visor Plan Cuentas</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:crystalreportviewer id="CrystalReportViewer1" runat="server" width="350px" height="50px" />
    </div>
    </form>
</body>
</html>
