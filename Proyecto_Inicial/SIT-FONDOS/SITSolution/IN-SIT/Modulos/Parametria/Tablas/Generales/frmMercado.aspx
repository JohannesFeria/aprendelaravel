<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMercado.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmMercado" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Mercados</title>    
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header>
    <h2>Mercados</h2>
    </header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
            <label class="col-md-3 control-label">Código</label>
            <div class="col-md-9">
                <asp:TextBox id="txtCodigo" runat="server" Width="32px"  MaxLength="3"></asp:TextBox>
                <strong><asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ErrorMessage="Código" ControlToValidate="txtCodigo" ForeColor="Red" Text="(*)"></asp:RequiredFieldValidator></strong>                
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
                <asp:TextBox id="tbDescripcion" runat="server" Width="152px" MaxLength="20"></asp:TextBox>
                <strong><asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ErrorMessage="Descripción" ControlToValidate="tbDescripcion" ForeColor="Red" Text="(*)"></asp:RequiredFieldValidator></strong>
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
                <asp:DropDownList id="ddlSituacion" runat="server" Width="115px" ></asp:DropDownList>
            </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:HiddenField ID="hd" runat="server" />
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar"/>
        <asp:Button ID="btnSalir" runat="server" Text="Retornar" CausesValidation="false" />        
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
