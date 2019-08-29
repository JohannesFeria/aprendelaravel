<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmHiperValorizador_rep.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmHiperValorizador_rep" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>HiperValorizador</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <asp:Button Text="Salir" runat="server" OnClientClick="window.close()" />
            </div>
        </div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"></CR:CrystalReportViewer>
    </div>
    </form>
</body>
</html>
