<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmValorParametroEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmValorParametroEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Parametría Encaje</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Parametría Encaje</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Nombre Parámetro</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtnombre" runat="server" Width="360px" MaxLength="50" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Nombre Parámetro" ControlToValidate="txtnombre">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Valor</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtvalor" runat="server" Width="80px" MaxLength="50" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Valor" ControlToValidate="txtvalor">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <br />
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
            CausesValidation="False" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
