<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorGestion.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmVisorGestion" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:CrystalReportViewer ID="crGestion" runat="server" BestFitPage="True" Height="50px"
            Width="350px"></cr:CrystalReportViewer>
        <br />
        <asp:Label ID="lblrutaArchivo" runat="server" CssClass="stlCajaTexto" Visible="False"></asp:Label>
    </div>
    </form>
</body>
</html>
