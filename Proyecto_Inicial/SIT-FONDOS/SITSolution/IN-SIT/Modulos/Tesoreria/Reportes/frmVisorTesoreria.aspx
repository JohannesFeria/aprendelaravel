﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorTesoreria.aspx.vb"
    Inherits="Modulos_Tesoreria_Reportes_frmVisorTesoreria" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Width="350px" Height="50px">
        </cr:CrystalReportViewer>
    </div>
    </form>
</body>
</html>