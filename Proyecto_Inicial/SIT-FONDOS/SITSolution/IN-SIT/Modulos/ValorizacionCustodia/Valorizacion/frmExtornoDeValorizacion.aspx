<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoDeValorizacion.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmExtornoDeValorizacion" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server"><title>Reversión de Valorización</title></head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Reversión de Valorización</h2></header>
    <br />
    <fieldset>
        <legend></legend>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Fondo</label>
                    <div class="col-sm-9">
                        <asp:dropdownlist id="ddlPortafolio" runat="server" Width="130px" AutoPostBack="True" />
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Fecha Operación</label>
                    <div class="col-sm-9">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" AutoPostBack="true" />
                            <span class="add-on"><i class="awe-calendar"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Fecha Operación" ControlToValidate="tbFechaOperacion">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
    </fieldset>
    <br />
    <header></header>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnExportar" runat="server" Text="Extornar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="False" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>