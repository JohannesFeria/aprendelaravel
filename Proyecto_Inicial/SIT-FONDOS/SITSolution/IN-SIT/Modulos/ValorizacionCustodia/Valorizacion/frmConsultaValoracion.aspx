<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaValoracion.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmConsultaValoracion" %>

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
        <header><h2>Consulta de Valorización</h2></header>
        <br />
        <fieldset>
        <legend></legend>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 cnotrol-label">Portafolio</label>
                    <div class="col-sm-9">
                        <asp:dropdownlist id="ddlPortafolio" runat="server" AutoPostBack="True" Width="120px" ></asp:dropdownlist>
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 cnotrol-label">Fecha Operación</label>
                    <div class="col-sm-9">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                            <span class="add-on"><i class="awe-calendar"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Fecha Operación" ControlToValidate="tbFechaOperacion">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <asp:radiobuttonlist id="rbtlNormativa" runat="server" AutoPostBack="True" Width="584px" CssClass="stlCajaTexto" RepeatDirection="Horizontal">
					<asp:ListItem Value="N" Selected="True">Valorizaci&#243;n Normativa</asp:ListItem>
					<asp:ListItem Value="E">Valorizaci&#243;n Estimada</asp:ListItem>
					<asp:ListItem Value="C">CURVA CUPON CERO</asp:ListItem>
				</asp:radiobuttonlist>
            </div>
            <div class="col-md-6"></div>
        </div>        
        </fieldset>
        <br />
        <header></header>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" 
                CausesValidation="False" />
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
