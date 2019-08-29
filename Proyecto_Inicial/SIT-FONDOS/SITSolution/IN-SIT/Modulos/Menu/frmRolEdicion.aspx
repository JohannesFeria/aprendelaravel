<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRolEdicion.aspx.vb" Inherits="Modulos_Menu_frmEdicionRol" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Usuarios</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblEdicion" runat="server"></asp:Label></h2>
                </div>
            </div>
        </header>

        <div class="Contenedor">
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código Aplicación</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox ID="tbCodAplicacion" runat="server" Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <div class="col-sm-7">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código Rol</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbCodRol" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <div class="col-sm-7">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripción Rol</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbDescRol" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <div class="col-sm-7">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Situación</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <div class="col-sm-7">
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        </div>

        <br />
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CausesValidation="False">
                </asp:Button>&nbsp;
                <asp:Button ID="btnSalir" runat="server" Text="Retornar" CausesValidation="False">
                </asp:Button>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
