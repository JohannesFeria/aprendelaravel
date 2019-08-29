<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmValorizacionMensual.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Valorizacion_frmValorizacionMensual" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Valorización SBS</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Valorización Mensual de Portafolios</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fondo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" AutoPostBack="true" />
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Inicio</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Fecha Inicio" ControlToValidate="txtFechaInicio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Fin</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtFechaFin" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Fecha Fin" ControlToValidate="txtFechaFin">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" 
            CausesValidation="False" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
