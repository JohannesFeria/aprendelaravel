<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmFactorEmision.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmFactorEmision" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Factores por Emisi&oacute;n</title>
    <script type="text/javascript">
        function showMnemonicoModal() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');              
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Mantenimiento de Factores por Emisi&oacute;n</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" Width="100px" />
                                <asp:LinkButton ID="lbkModal" OnClientClick="return showMnemonicoModal()" runat="server"
                                    CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbDescMnemonico" Width="250px" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Mnem&oacute;nico" ControlToValidate="tbCodigoMnemonico"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo Factor</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoFactor" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Tipo Factor" ControlToValidate="ddlTipoFactor"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Factor</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbFactor" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Factor" ControlToValidate="tbFactor" runat="server"
                                Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha de Vigencia</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVigencia" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha Vigencia" ControlToValidate="tbFechaVigencia"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
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
    </div>
    <input id="hd" type="hidden" name="hd" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
