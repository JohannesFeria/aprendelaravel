<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmContacto.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmContacto" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Contactos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Contactos</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                C&oacute;digo de Contacto</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="tbCodigo" style="text-transform:uppercase;" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Descripci&oacute;n</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="tbDescripcion" Width="290px" style="text-transform:uppercase;" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Situaci&oacute;n"
                                    ControlToValidate="ddlSituacion" runat="server" Text="(*)" CssClass="validator" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tipo Contacto</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlTipoContacto" Width="170px" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Situaci&oacute;n</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Situaci&oacute;n"
                                    ControlToValidate="ddlSituacion" runat="server" Text="(*)" CssClass="validator" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Observaciones</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Rows="5" Width="320px" />
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
        <input id="hdnCodigo" type="hidden" name="hdnCodigo" runat="server">
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
