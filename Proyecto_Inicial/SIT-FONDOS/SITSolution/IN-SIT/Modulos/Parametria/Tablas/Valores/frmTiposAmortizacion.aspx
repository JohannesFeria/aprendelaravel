<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTiposAmortizacion.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmTiposAmortizacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Tipo de Amortización</title>
    <script type="text/javascript">
        function ValidaSoloNumeros() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                event.returnValue = false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Mantenimiento de Tipo de Amortización</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 control-label">Código Tipo Amortización</label>
                <div class="col-sm-8">
                    <asp:textbox id="tbCodigo" runat="server" Width="64px" MaxLength="4"></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Código Tipo Amortización" ControlToValidate="tbCodigo">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 control-label">Descripción</label>
                <div class="col-sm-8">
                    <asp:textbox id="tbDescripcion" runat="server" Width="350px" MaxLength="20" />
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
                <label class="col-sm-4 control-label">Número de Días</label>
                <div class="col-sm-8">
                    <asp:textbox id="tbNumeroDias" runat="server" MaxLength="4" Width="64px" onkeypress="ValidaSoloNumeros();"></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="Número de días" ControlToValidate="tbNumeroDias">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 control-label">Situación</label>
                <div class="col-sm-8">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" Style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
            CausesValidation="False" />
            <asp:HiddenField ID="hd" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
