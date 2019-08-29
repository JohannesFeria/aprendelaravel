<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmError.aspx.vb" Inherits="frmError" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head>
    <title>Error Detalle</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <script type="text/javascript">
        function GoToLogin() {
            this.parent.location = $("#hiddenUrlLogin").val();
        }
    </script>
    <style type="text/css">
        .SizeImage
        {
            height: 32px;
            width: 138px;
        }
        .GoLogin
        {
            height: 45px;
            width: 120px;
        }
        .container-fluid{
            width:50%;
        }
        </style>
      
</head>
<body>
   <%--  <form id="frmError" runat="server">
    <p>&nbsp;</p>
    <p><br><br>&nbsp;</p>
    <table class="error" id="tblError" cellspacing="0" cellpadding="10" width="50%" align="center" runat="server">
        <tr>
            <td align="center" colspan="2">
                <hr width="100%" size="1" class="LineError">
            </td>
        </tr>
        <tr>
            <th align="center">
                &nbsp;<img src="App_Themes/img/logo.jpg" class="SizeImage" /></th>
            <th width="91%">
                <div align="left" class="Warnning" dir="ltr">
                    W A R N I N G !
                </div>
            </th>
        </tr>
        <tr>
            <td align="center">
                <img src="App_Themes/img/Error2.png" class="SizeImage" /></td>
            <td align="left" width="91%">
                <p class="MessageError">El sistema ha encontrado una situaci&oacute;n anormal.</p>
                <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="MessageError"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <hr width="100%" size="1" class="LineError">
                &nbsp;
            </td>
        </tr>
    </table>
    <div id="ocultar">
        <table class="error" id="tblOcultar" cellspacing="0" cellpadding="10" width="50%"
            align="center" runat="server">
            <tr>
                <td width="91%" align="right">
                    <input id="hiddenUrlLogin" type="hidden" runat="server" value="emptyPage.html" />
                    <img src="App_Themes/img/Login2.png" class="GoLogin" title="Volver ingresar al Sistema" onclick="GoToLogin();" />
                </td>
            </tr>
        </table>
    </div>
    </form>--%>
    <form id="form1" runat="server" class="form-horizontal">
     <p>&nbsp;</p>
    <p><br/><br/>&nbsp;</p>
    <div class="container-fluid">
        <h2>
            Error:</h2>
        <p>
        </p>
        <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" Style="color: red"></asp:Label>
        <hr />
        <div id="DetailedErrorPanel" runat="server" style="display: none">
            <fieldset>
                <legend>Detalle de Error</legend>
                <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" />
            </fieldset>
            <br />
            <fieldset>
                <legend>Error Handler</legend>
                <asp:Label ID="lblErrorHandler" runat="server" Font-Size="Small" />
            </fieldset>
            <br />
            <fieldset>
                <legend>Mensaje de Detalle de Error</legend>
                <asp:Label ID="InnerMessage" runat="server" Font-Size="Small" /><br />
                <br />
                <p>
                    <asp:Label ID="InnerTrace" runat="server" Font-Size="XX-Small" />
                </p>
            </fieldset>
        </div>
        <br />
        <div class="col-sm-12" style="text-align: right;">
            <input id="hiddenUrlLogin" type="hidden" runat="server" value="emptyPage.html" />
            <asp:Button runat="server" ID="btnSalir" Text="Salir" OnClientClick ="GoToLogin();" />
        </div>
    </div>
    </form>
</body>
</html>
