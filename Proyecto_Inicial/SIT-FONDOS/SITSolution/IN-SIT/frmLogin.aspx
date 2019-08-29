<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLogin.aspx.vb" Inherits="frmLogin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Login</title>
    <style type="text/css">
        .button
        {}
    </style>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManagerLocal" />

    <div class="container-fluid" style="width:680px; min-width:200px;">
        <header>
            <div class="row">
                <div style="margin: 10px 10px 0px 10xp; padding-left:10px; padding-top: 10px; height: 70px; width: 100%;">
                    <img alt="" src="App_Themes/img/integra.png" style="margin: auto; width: 187px;" />
                </div>
                <div class="col-md-6">
                    <h2></h2>
                </div>
            </div>
        </header>

        <fieldset style="padding-bottom:10px;">
            <legend style="text-align:center;">Inicio de Sesión</legend>

                <table align="center" border="0" width="50%">
                    <tr>
                        <td valign="middle" style="text-align:right;">
                            <br />
                            <label >Usuarios&nbsp;&nbsp;&nbsp;&nbsp; </label>&nbsp;<br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtUsuario" runat="server" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" style="text-align:right;">
                            <br />
                            <label>Contraseña&nbsp;&nbsp;&nbsp;&nbsp;  </label>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                </table>         

        </fieldset>

        <br />

        <%--<div class="grilla">    
        </div>--%>
        
        <div style="text-align: right;" align="right">                
                <asp:UpdatePanel runat="server" ID="updExtraer">
                    <ContentTemplate>
                        <asp:Button ID="btnIngresar" runat="server" Width="2px" Text="Ingresar"
                            CausesValidation="False" CssClass="button" ></asp:Button>
                        <%--<asp:Button ID="bSalir" runat="server" Width="72px" Text="Salir" 
                            CausesValidation="False" CssClass="button" Visible="false">
                        </asp:Button>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
         </div>

    </div>
    </form>
</body>
</html>
