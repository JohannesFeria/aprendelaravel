<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorAsientoContable.aspx.vb" Inherits="Modulos_Contabilidad_frmVisorAsientoContable" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>VisorAsientoContable</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr:crystalreportviewer id="crAsientoContable" runat="server" width="350px" height="50px" />
    </div>
    </form>
</body>
</html>