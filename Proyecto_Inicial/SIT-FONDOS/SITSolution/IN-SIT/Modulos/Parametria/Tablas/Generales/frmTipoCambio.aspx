<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTipoCambio.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmTipoCambio" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Tipo de Cambio</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Tipo de Cambio</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Divisa</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlMonedaOrigen" runat="server" Width="180px"></asp:dropdownlist>
                    <strong><asp:RequiredFieldValidator ID="rfvDevisa" runat="server" 
                        ErrorMessage="Moneda Origen (Devisa)" ControlToValidate="ddlMonedaOrigen" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Moneda</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlMonedaDestino" runat="server" Width="180px" ></asp:dropdownlist>
                    <strong>
                    <asp:RequiredFieldValidator ID="rfvMonedaDestino" runat="server" 
                        ErrorMessage="Moneda Destino" ControlToValidate="ddlMonedaDestino" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Tipo</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlTipo" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Situación</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Descripción</label>
                <div class="col-md-9">
                    <asp:TextBox ID="txtDescripcion" runat="server" Width="488px" MaxLength="100"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="Text-Align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
            CausesValidation="False" />
        <asp:HiddenField ID="hd" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
