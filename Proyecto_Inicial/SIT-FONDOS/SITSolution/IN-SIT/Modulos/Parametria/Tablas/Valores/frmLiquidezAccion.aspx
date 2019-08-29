<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLiquidezAccion.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmLiquidezAccion" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Acciones seg&uacute;n Liquidez</title>
    <script type="text/javascript">
        function showPopup() {
            if ($('#tipoOperacion').val() != 'mod') {                
                return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Valores', '1200', '600', '');    
            }
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Mantenimeinto de Liquidez Acciones</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Mnem&oacute;nico</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" Width="100px" ReadOnly="true" />
                                <asp:LinkButton runat="server" ID="lkbModal" OnClientClick="return showPopup()" CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbDescMnemonico" Width="250px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Mnem&oacute;nico"
                                ControlToValidate="tbCodigoMnemonico" runat="server" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Criterio Liquidez</label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbLiquidez" style="text-transform:uppercase" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                ErrorMessage="Criterio Liquidez" ControlToValidate="tbLiquidez" runat="server"
                                CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
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
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server">
    <input id="tipoOperacion" type="hidden" name="tipoOperacion" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
