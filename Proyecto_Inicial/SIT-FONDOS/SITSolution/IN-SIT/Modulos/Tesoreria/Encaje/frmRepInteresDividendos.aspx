<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRepInteresDividendos.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmRepInteresDividendos" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Reporte de Intereses y Dividendos</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Reporte de Intereses y Dividendos</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="170px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Inicio</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Fecha Inicio" ControlToValidate="tbFechaInicio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"><asp:Label runat="server" ID="lFechaFin" Text="Fecha Inicio"></asp:Label></label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Fecha Fin" ControlToValidate="tbFechaFin">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
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
