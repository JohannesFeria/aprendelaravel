<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCuentaEconomica.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmCuentaEconomica" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Cuentas Econ&oacute;micas</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Cuentas Econ&oacute;micas</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Portafolio</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" />
                            <asp:RequiredFieldValidator ErrorMessage="Portafolio" ControlToValidate="ddlPortafolio"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portes
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbPortes" Width="120px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Portes" ControlToValidate="tbPortes" runat="server"
                                Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
             <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">                          
                        Banco</label>
                        <div class="col-sm-7">                                                       
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlENtidadFinanciera" Width="200px" AutoPostBack="true" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Banco" ControlToValidate="ddlENtidadFinanciera"
                                    runat="server" Text="(*)" CssClass="validator" />
                            </ContentTemplate>
                            </asp:UpdatePanel>    
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                       
                        <label class="col-sm-4 control-label">
                            Comisi&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbComision" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Comisi&oacute;n" ControlToValidate="tbComision"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Moneda
                        </label>
                        <div class="col-sm-7">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlMoneda" Width="200px" AutoPostBack="true" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage="Moneda" ControlToValidate="ddlMoneda" runat="server"
                                    Text="(*)" CssClass="validator" />
                            </ContentTemplate> 
                            </asp:UpdatePanel> 
                        </div>
                    </div>
                </div>                
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. de Documento
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbNroDoc" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Nro. de Documento" ControlToValidate="tbNroDoc"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>           
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Cuenta Contable</label>
                        <div class="col-sm-7">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox runat="server" ID="txtCuentaContable" Width="200px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ErrorMessage="Cuenta Contable" ControlToValidate="txtCuentaContable"
                                    runat="server" Text="(*)" CssClass="validator" />
                            </ContentTemplate> 
                            </asp:UpdatePanel> 
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Cuenta
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseCuenta" Width="150px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="Clase de Cuenta" ControlToValidate="ddlClaseCuenta"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Nro. Cuenta
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbNumeroCuenta" Width="200px" />
                            <asp:RequiredFieldValidator ErrorMessage="Nro. Cuenta" ControlToValidate="tbNumeroCuenta"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Mercado" ControlToValidate="ddlMercado"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Nro. Cuenta Interbancaria
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbNumeroCuentaInterbancaria" Width="220px" />
                            <asp:RequiredFieldValidator ErrorMessage="Nro. Cuenta Interbancaria" ControlToValidate="tbNumeroCuentaInterbancaria"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="150px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="Situaci&oacute;n" ControlToValidate="ddlSituacion"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbTasa" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Tasa" ControlToValidate="tbTasa" runat="server"
                                Text="(*)" CssClass="validator" />
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
