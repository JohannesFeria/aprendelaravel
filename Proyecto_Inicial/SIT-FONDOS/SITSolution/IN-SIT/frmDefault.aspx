<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDefault.aspx.vb" Inherits="frmDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html style="height: 100%;">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts2")%>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos2") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function ShowMessage(msje) {
            alertify.alert(msje, function () {
                $("#btnEjecutar").click();
            });
        }

        function GoToLogin() {
            this.parent.location = $("#hiddenUrlLogin").val();
        }
    </script>
</head>
<body style="position: relative; height: 100%; margin: 0px; padding: 0px; border-spacing: 0px;">
    <form id="frmDefault" runat="server" style="height: 100%; width: 364px; margin: auto; background-image: url('App_Themes/img/logo.jpg');
            background-position: center; background-repeat: no-repeat;">
        <%--<div style="height: 100%; width: 100%;">
            <div style="height: 100%; width: 364px; margin: auto; background-image: url('App_Themes/img/logo.jpg');
                background-position: center; background-repeat: no-repeat;">
            </div>
        </div>--%>
        <input id="hiddenUrlLogin" type="hidden" runat="server" value="emptyPage.html" />
        <input id="hiddenMsje" type="hidden" runat="server" value="" />
        <div style="display:none">
            <asp:Button Text="Ejecutar" runat="server" ID="btnEjecutar" />
        </div>
    </form>
</body>
</html>
