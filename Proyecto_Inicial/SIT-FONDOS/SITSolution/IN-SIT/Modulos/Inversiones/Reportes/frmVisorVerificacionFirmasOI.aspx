<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorVerificacionFirmasOI.aspx.vb"
    Inherits="Modulos_Inversiones_Reportes_frmVisorVerificacionFirmasOI" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="cr1" %>
<!DOCTYPE html >
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr1:crystalreportviewer id="crVerificacionFirmas" runat="server" />
    </div>
    </form>
</body>
</html>
