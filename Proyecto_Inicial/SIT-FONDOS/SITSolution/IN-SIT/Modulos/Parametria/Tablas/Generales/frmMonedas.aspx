<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMonedas.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmMonedas" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Monedas</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
        <h2>Moneda</h2>
        </header>
        <br />
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Código</label>
                    <div class="col-md-9">
                        <asp:TextBox id="txtCodigo" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                        <strong>
                        <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" 
                            ErrorMessage="Código Moneda" ForeColor="Red" ControlToValidate="txtCodigo">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Código ISO</label>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtCodigoIso" Runat="server" MaxLength="3"></asp:TextBox>
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Código SBS</label>
                    <div class="col-md-9">
                        <asp:TextBox id="txtCodigoSBS" runat="server" MaxLength="1" Width="50px"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="rfvCodigoSBS" runat="server" 
                            ErrorMessage="Código Moneda según SBS" ForeColor="Red" 
                            ControlToValidate="txtCodigoSBS">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Sinónimo ISO</label>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtSinonimoIso" Runat="server" CssClass="stlCajaTexto" MaxLength="3"></asp:TextBox>
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Descripción</label>
                    <div class="col-md-9">
                        <asp:textbox id="txtDescripcion" runat="server" MaxLength="50" Width="360px"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="rfvDescripcionCodigo" runat="server" 
                            ErrorMessage="Descripción Moneda" ForeColor="Red" 
                            ControlToValidate="txtDescripcion">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Tipo de Cambio a:</label>
                    <div class="col-md-9">
                        <asp:DropDownList ID="ddlmoneda" runat="server" />
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Símbolo</label>
                    <div class="col-md-9">
                        <asp:textbox id="txtSimbolo" runat="server" MaxLength="5" Width="48px"></asp:textbox>
                        <strong>
                        <asp:RequiredFieldValidator ID="rfvSimbolo" runat="server" 
                            ErrorMessage="SImbolo" ForeColor="Red" ControlToValidate="txtSimbolo">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Tipo de Cálculo</label>
                    <div class="col-md-9">
                        <asp:dropdownlist id="ddlTipoCalculo" runat="server" Width="115px"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Tipo Cálculo" ForeColor="Red" 
                            ControlToValidate="ddlTipoCalculo">(*)</asp:RequiredFieldValidator>
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
                        <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
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
