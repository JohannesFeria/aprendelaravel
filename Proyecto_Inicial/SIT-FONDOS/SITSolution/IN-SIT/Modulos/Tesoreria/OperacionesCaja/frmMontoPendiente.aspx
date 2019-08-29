<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMontoPendiente.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmMontoPendiente" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Mantenimiento de Pendiente</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:UpdatePanel ID="UPCuerpo" runat="server" UpdateMode ="Conditional">
    <ContentTemplate>
    <div class="container-fluid">
        <header>
        <div class="row"><div class="col-md-6"><h2>Mantenimiento de Pendiente</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Banco</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlTercero" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Moneda</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Numero Cuenta</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlNumeroCuenta" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Inicio</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaOperacion" SkinID="Date" AutoPostBack="true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Importe</label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="TXTImporte" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" />
            </div>
        </fieldset>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>