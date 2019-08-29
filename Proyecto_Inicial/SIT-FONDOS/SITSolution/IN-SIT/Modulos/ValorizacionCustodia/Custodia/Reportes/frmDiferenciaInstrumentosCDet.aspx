<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDiferenciaInstrumentosCDet.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Custodia_Reportes_frmDiferenciaInstrumentosCDet" %>

<%@ Register TagPrefix="cr1" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Conciliaci&oacute;n Inf. Custodios</title>
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
            <cr1:CrystalReportViewer ID="crDiferencias" runat="server"></cr1:CrystalReportViewer>
        </div>
    </div>
    </form>
</body>
</html>