<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisorErrorValoracion.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVisorErrorValoracion" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
    <header></header>
    <br />
    <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClientClick="Cerrar();" />
    <br />
        <CR:CrystalReportViewer ID="CrNemonico" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>
