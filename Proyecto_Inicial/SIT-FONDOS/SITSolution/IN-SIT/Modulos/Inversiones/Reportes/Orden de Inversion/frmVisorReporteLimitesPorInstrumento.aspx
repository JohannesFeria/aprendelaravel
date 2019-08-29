<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorReporteLimitesPorInstrumento.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Orden_de_Inversion_frmVisorReporteLimitesPorInstrumento" %>
<%@ Register TagPrefix="cr1" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr1:CrystalReportViewer id="crvInversion" runat="server" Height="50px" Width="350px"></cr1:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
