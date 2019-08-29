<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCalculoPromedioDiario.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmCalculoPromedioDiario" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Cálculo Promedio Diario por Instrumento</title>
    <script type="text/javascript">
        function showPopup() {
            var isin = $('#txtISIN').val();
            var sbs = '';
            var mnemonico = $('#txtMnemonico').val();
            return showModalDialog('frmBusquedaInstrumentos.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', ''); 
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Cálculo Promedio Diario por Instrumento</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fondo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="130px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mnemónico</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="txtMnemonico" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" CausesValidation="false" OnClientClick="return showPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Mnemónico " ControlToValidate="txtMnemonico">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código ISIN</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtISIN" runat="server" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Código ISIN" ControlToValidate="txtISIN">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Días base</label>
                <div class="col-sm-9">
                        <asp:TextBox runat="server" ID="txtDBase" CssClass="input-medium" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="Días base" ControlToValidate="txtDBase">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>            
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">VPN Promedio</label>
                <div class="col-sm-9">
                    <asp:TextBox id="lblVPNPromedio" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
       <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
       <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="False" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>

