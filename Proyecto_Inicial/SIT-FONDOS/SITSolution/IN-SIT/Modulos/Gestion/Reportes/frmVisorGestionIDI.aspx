<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorGestionIDI.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmVisorGestionIDI" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:crystalreportviewer id="crGestion" runat="server" width="350px" height="50px"
            bestfitpage="True"></cr:crystalreportviewer>
    </div>
    </form>
</body>
</html>
