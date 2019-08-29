<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRating.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmRating" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Rating</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Rating</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Rating</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRating" Width="150px">
                                <asp:ListItem Selected="True">--Seleccione--</asp:ListItem>
                                <asp:ListItem Value="LOC">Nacional</asp:ListItem>
                                <asp:ListItem Value="EXT">Internacional</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="Tipo de Rating" ControlToValidate="ddlTipoRating"
                                runat="server" />
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
                            <asp:TextBox runat="server" ID="tbDescripcion" /><asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n"
                                ControlToValidate="tbDescripcion" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Factor</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbFactor" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="Factor" ControlToValidate="tbFactor" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
    </div>
    <input type="hidden" name="hddAction" id="hddAction" runat="server" />
    <input type="hidden" name="hddValor" id="hddValor" runat="server" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
