<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmNegocio.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmNegocio" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Negocio</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Negocio</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Código</label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="4" Width="40px" />
                            <asp:RequiredFieldValidator ID="rfvCodigo" ErrorMessage="Fecha" ControlToValidate="tbCodigo"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Descripción</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="tbDescripcion" runat="server" MaxLength="30" Width="224px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDescripcion" ErrorMessage="Descripción" ControlToValidate="tbDescripcion"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Situación</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="152px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvSituacion" ErrorMessage="Situación" ControlToValidate="ddlSituacion"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <label class="col-md-3 control-label">Moneda</label>
                    <div class="col-md-9">
                        <asp:dropdownlist id="ddlMoneda" runat="server" Width="180px" ></asp:dropdownlist>
                        <strong>
                        <asp:RequiredFieldValidator ID="rfvMonedaDestino" runat="server" 
                            ErrorMessage="Moneda Destino" ControlToValidate="ddlMoneda"  ValidationGroup="vgDetalle"
                            ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>            
        </fieldset>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button runat="server" ID="ibAceptar" Text="Aceptar" ValidationGroup="vgDetalle" />
            <asp:Button runat="server" ID="ibCancelar" Text="Retornar" CausesValidation="false" />
        </div>
        <asp:HiddenField ID="hd" runat="server" />
        <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
            HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="vgDetalle" />
    </div>
    </form>
</body>
</html>
