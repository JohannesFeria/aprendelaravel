<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPrincipal.Old.aspx.vb" Inherits="frmPrincipalOld" %>

<!DOCTYPE html>
<html style="height: 100%;">
<head runat="server">
    <title>Sistema de Cartas - v2.2.1</title>
    <script type="text/javascript">
        function op() {
        }
    </script>
    <link rel="shortcut icon" href="~/App_Themes/img/icons/favicon.ico" />
</head>
<body style="position: relative; height: 100%; margin: 0px; padding: 0px; border-spacing: 0px;">
    <div style="clear: both; height: 100%;">
        <iframe style="float: left; width: 20%; height: 100%; border: 0; border-right-style: solid;
            border-right-width: 3px; border-right-color: #000;" id="frameMenu" name="frameMenu"
            scrolling="auto" src="MENU.html"></iframe>
        <iframe style="float: left; width: 79.5%; height: 100%; float: right; border: 0"
            id="basefrm" name="basefrm" src="frmDefault.aspx"></iframe>
    </div>
</body>
</html>
