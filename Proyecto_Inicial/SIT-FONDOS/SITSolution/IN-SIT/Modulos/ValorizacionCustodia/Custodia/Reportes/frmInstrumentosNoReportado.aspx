<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInstrumentosNoReportado.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Custodia_Reportes_frmInstrumentosNoReportado" %>
<%@ Register TagPrefix="cr1" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-2">
                <asp:Button Text="Retornar" runat="server" OnClientClick="window.close()" ID="btnCancelar" />
            </div>
        </div>
        <div class="row">
            <cr1:CrystalReportViewer ID="crNoRegistrados" runat="server"></cr1:CrystalReportViewer>
        </div>
    </div>
    </form>
</body>
</html>
