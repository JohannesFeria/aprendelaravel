<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPeriodicidad.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmPeriodicidad" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Periodicidad</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                     Mantenimiento de Periodicidad</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Periodicidad</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" Width="150px"  MaxLength="5" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="C&oacute;digo" ControlToValidate="tbCodigo" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="200px" MaxLength="40" style="text-transform:uppercase" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            D&iacute;as de Periodo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtDias" Width="170px" CssClass="Numbox-0_5" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="D&iacute;as de Periodo" ControlToValidate="txtDias"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Situaci&oacute;n" ControlToValidate="ddlSituacion"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        EnableClientScript="true" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
