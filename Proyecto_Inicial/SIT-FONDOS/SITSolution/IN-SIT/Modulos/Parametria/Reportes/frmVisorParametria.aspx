<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorParametria.aspx.vb"
    Inherits="Modulos_Parametria_Reportes_frmVisorParametria" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <title>Visor Parametria</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnCancelar" runat="server" Text="Retornar" />
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
        
    </div>
    </form>
</body>
</html>
