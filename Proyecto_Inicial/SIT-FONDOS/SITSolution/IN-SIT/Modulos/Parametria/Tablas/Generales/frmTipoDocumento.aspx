<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTipoDocumento.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmTipoDocumento" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Tipo de Documento</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Tipo de Documento</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" Width="120px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="C&oacute;digo"
                                ControlToValidate="tbCodigo" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Persona</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoPersona" Width="100px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Tipo de Persona"
                                ControlToValidate="ddlTipoPersona" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="220px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Descripci&oacute;n"
                                ControlToValidate="tbDescripcion" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Longitud M&aacute;xima</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbLongitudMax" Width="120px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Longitud M&aacute;xima"
                                ControlToValidate="tbLongitudMax" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            D&iacute;gito Chequeo</label>
                        <div class="col-sm-8">
                            <asp:RadioButtonList ID="rdbChekeo" runat="server">
                                <asp:ListItem Value="S" Text="SI" />
                                <asp:ListItem Value="N" Text="NO" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header></header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button ID="btnAceptar" Text="Aceptar" runat="server" />
                <asp:Button ID="btnCancelar" Text="Retornar" runat="server" CausesValidation="false" />
            </div>
        </div>
        <input runat="server" id="hd" type="hidden" name="hd" />
        <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
            HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
