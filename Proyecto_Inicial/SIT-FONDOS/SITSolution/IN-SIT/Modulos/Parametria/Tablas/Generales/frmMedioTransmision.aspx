<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMedioTransmision.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmMedioTransmision" %>

<!DOCTYPE html />

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Medio de Transmisión</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Medio de Transmisión</h2></header>
    <fieldset>
    <legend>Medio de Transmisión</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Código</label>
                <div class="col-md-9">
                    <asp:TextBox id="txtCodigo" runat="server" Width="32px" MaxLength="1"></asp:TextBox>
                    <strong><asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ErrorMessage="Código" 
                        ControlToValidate="txtCodigo" ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Nombre</label>
                <div class="col-md-9">
                    <asp:TextBox id="txtDescripcion" runat="server" Width="350px" ></asp:TextBox>
                    <strong>
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ErrorMessage="Nombre" 
                        ControlToValidate="txtDescripcion" ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Tipo Renta</label>
                <div class="col-md-9">
                    <asp:dropdownlist style="Z-INDEX: 0" id="ddlTipoRenta" runat="server" Width="160px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
            CausesValidation="False" />
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </div>
    </form>
</body>
</html>
