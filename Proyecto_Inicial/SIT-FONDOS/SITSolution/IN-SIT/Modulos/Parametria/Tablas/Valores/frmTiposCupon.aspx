<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTiposCupon.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmTiposCupon" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Tipos de Cup&oacute;n</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Tipos de Cup&oacute;n</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Tipo Cup&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Tipo Cup&oacute;n" ControlToValidate="tbCodigo"
                                runat="server" />
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
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="180px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Descripci&oacute;n"
                                ControlToValidate="tbDescripcion" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Observaciones</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbObservaciones" TextMode="MultiLine" Rows="5" Width="240px" />
                            <asp:RequiredFieldValidator ErrorMessage="Observaciones" ControlToValidate="tbObservaciones"
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
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
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
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hd" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        EnableClientScript="true" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
