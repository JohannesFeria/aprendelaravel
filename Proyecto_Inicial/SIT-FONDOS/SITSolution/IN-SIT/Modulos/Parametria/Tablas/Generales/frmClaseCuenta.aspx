﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmClaseCuenta.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmClaseCuentas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Clase de Cuenta</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Clase de Cuenta</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtCodigo" /><asp:RequiredFieldValidator ErrorMessage="C&oacute;digo"
                                ControlToValidate="txtCodigo" runat="server" Text="(*)" CssClass="validator" />
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
                            <asp:TextBox runat="server" ID="txtDescripcion" Width="250px" /><asp:RequiredFieldValidator
                                ErrorMessage="Descripci&oacute;n" ControlToValidate="txtDescripcion" runat="server"
                                Text="(*)" CssClass="validator" />
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
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
        <input id="hdnCodigoClaseCuenta" type="hidden" name="hdnCodigoClaseCuenta" runat="server">
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
