<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorValorizacion.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVisorValorizacion" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Cerrar() {
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <div class="row">
    <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClientClick="Cerrar();" />
    <br />
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    </div>
    </div>
    </form>
</body>
</html>
