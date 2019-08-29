<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTipoBursatilidad.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmTipoBursatilidad" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Tipos de Bursatilidad</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Tipos de Bursatilidad</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            C&oacute;digo Tipo Bursatibilidad</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="tbCodigo" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Tipo Bursatibilidad" ControlToValidate="tbCodigo"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="180px" />
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-6">
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
