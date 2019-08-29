<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSectorEmpresarial.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmSectorEmpresarial" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid"><header><h2>Sector Empresarial</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Código</label>
                <div class="col-md-9">
                    <asp:TextBox ID="txtCodigo" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                    <strong><asp:RequiredFieldValidator runat="server" ID="rfvCodigo" 
                        ControlToValidate="txtCodigo" ErrorMessage="Código" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Descripción</label>
                <div class="col-md-9">
                    <asp:TextBox ID="txtNombre" CssClass="stlCajaTexto" runat="server" Width="226px" MaxLength="40"></asp:TextBox>
                    <strong><asp:RequiredFieldValidator runat="server" ID="rfvDescripcion" 
                        ControlToValidate="txtNombre" ErrorMessage="Descripción" Text="(*)" 
                        ForeColor="Red"></asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Situación</label>
                <div class="col-md-9">
                    <asp:DropDownList id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
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
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="false" />
        <asp:HiddenField ID="hd" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />        
    </form>
</body>
</html>
