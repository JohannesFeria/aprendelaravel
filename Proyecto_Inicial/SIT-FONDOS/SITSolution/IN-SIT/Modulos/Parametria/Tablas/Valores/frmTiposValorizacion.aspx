<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTiposValorizacion.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmTiposValorizacion" %>

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
    <header><h2><asp:label id="lblTitulo" runat="server"></asp:label></h2></header>
    <br />
    <div id="TablaBCRSeriado" runat="server">
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Código Mnemonico</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbCodigo" runat="server" Enabled="False" Width="120px" MaxLength="15"></asp:TextBox>                            
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Cuenta Contable</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbDescripcion" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Nombre Cuenta</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbNombreCuenta" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
    <div id="TablaBCRUnico" runat="server">
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Código SBS</label>
                        <div class="col-sm-9">
                            <asp:textbox id="tbBCRUnico_CodigoSBS" runat="server" Width="120px" MaxLength="12"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Código Mnemonico" ValidationGroup="btnAceptar2" 
                                ControlToValidate="tbBCRUnico_CodigoSBS">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Código Entidad</label>
                        <div class="col-sm-9">
                            <asp:textbox id="tbBCRUnico_CodigoEntidad" runat="server" Width="120px" MaxLength="4"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="Código Entidad" ValidationGroup="btnAceptar2" 
                                ControlToValidate="tbBCRUnico_CodigoEntidad">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Código Moneda</label>
                        <div class="col-sm-9">
                            <asp:textbox id="tbBCRUnico_CodigoMoneda" runat="server" Width="120px" MaxLength="6"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ErrorMessage="Código Moneda" ValidationGroup="btnAceptar2" 
                                ControlToValidate="tbBCRUnico_CodigoMoneda">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Código Tipo Titulo</label>
                        <div class="col-sm-9">
                            <asp:textbox id="tbBCRUnico_CodigoTitulo" runat="server" Width="120px" MaxLength="12"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                ErrorMessage="Código Tipo Titulo" ValidationGroup="btnAceptar2" 
                                ControlToValidate="tbBCRUnico_CodigoTitulo">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Cuenta Contable</label>
                        <div class="col-sm-9">
                            <asp:textbox id="tbBCRUnico_CuentaContable" runat="server" Width="192px" MaxLength="16"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ErrorMessage="Cuenta Contable" ValidationGroup="btnAceptar2" 
                                ControlToValidate="tbBCRUnico_CuentaContable">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Nombre Cuenta</label>
                        <div class="col-sm-9">
                            <asp:textbox id="tbBCRUnico_NombreCuenta" runat="server" Width="400px" MaxLength="100"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                ErrorMessage="Nombre Cuenta" ValidationGroup="btnAceptar2" 
                                ControlToValidate="tbBCRUnico_NombreCuenta">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
    <header></header>
    <div class="row" style="text-align:right;">
        <div id="TablaBCRAceptar" runat="server">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
                CausesValidation="False" />
        </div>
        <div id="TablaBCRUnicoAceptar" runat="server">
            <asp:Button ID="btnAceptar2" runat="server" Text="Aceptar" 
                ValidationGroup="btnAceptar2" />
            <asp:Button ID="btnRetornar2" runat="server" Text="Retornar" 
                CausesValidation="False" />
        </div>
        <asp:HiddenField ID="hdCodigoUnico" runat="server" />
        <asp:HiddenField ID="hd" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="btnAceptar2" />
    </form>
</body>
</html>
