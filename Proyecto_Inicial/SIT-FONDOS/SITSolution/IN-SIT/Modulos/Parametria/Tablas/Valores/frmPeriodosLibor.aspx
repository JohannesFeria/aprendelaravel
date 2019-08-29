<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPeriodosLibor.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmPeriodosLibor" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Períodos Libor</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Períodos Libor</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Período Libor</label>
                <div class="col-sm-9">
                    <asp:TextBox id="tbCodigo" runat="server" MaxLength="4" Width="55px" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Código Período Libor" ControlToValidate="tbCodigo">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Descripción</label>
                <div class="col-sm-9">
                    <asp:TextBox id="tbDescripcion" runat="server" MaxLength="15" Width="178px" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Descripción" ControlToValidate="tbDescripcion">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situación</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="false" />
        <asp:HiddenField ID="hd" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
