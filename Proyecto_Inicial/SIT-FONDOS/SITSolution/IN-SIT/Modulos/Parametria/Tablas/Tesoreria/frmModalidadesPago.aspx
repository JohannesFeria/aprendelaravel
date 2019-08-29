<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmModalidadesPago.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Tesoreria_frmModalidadesPago" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Modalidad de Pago</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            C&oacute;digo Modalidad de Pago</label>
                        <div class="col-md-6">
                            <asp:TextBox ID="txtCodigo" runat="server" MaxLength="4" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Modalidad de Pago" Text="(*)"
                                ControlToValidate="txtCodigo" CssClass="validator" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-md-6">
                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="25" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" Text="(*)" ControlToValidate="txtDescripcion"
                                CssClass="validator" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Situación</label>
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="136px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <asp:HiddenField ID="hd" runat="server" />
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <div class="col-md-8">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
