<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorConsultaCuponeras.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Cuponera_frmVisorConsultaCuponeras" %>
<%@ Register TagPrefix="cr1" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Visor Pre Ordenes</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cr1:crystalreportviewer id="crPreOrden" runat="server" height="50px" width="350px"></cr1:crystalreportviewer>
    </div>
    </form>
</body>
</html>
