<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCuentasPorTipoInst.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmCuentasPorTipoInst" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Cuentas por Tipo de Instrumento</title>
    <script type="text/javascript">
        function validarNumeros(e) { // 1
            tecla = (document.all) ? e.keyCode : e.which; // 2
            if (tecla == 8) return true; // backspace
            if (tecla == 109) return true; // menos
            if (tecla == 110) return true; // punto
            if (tecla == 189) return true; // guion
            if (e.ctrlKey && tecla == 86) { return true }; //Ctrl v
            if (e.ctrlKey && tecla == 67) { return true }; //Ctrl c
            if (e.ctrlKey && tecla == 88) { return true }; //Ctrl x
            if (tecla >= 96 && tecla <= 105) { return true; } //numpad

            patron = /[0-9]/; // patron

            te = String.fromCharCode(tecla);
            return patron.test(te); // prueba
        }
</script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Cuentas por Tipo de Instrumento</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Instrumento
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="220px" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ErrorMessage="Tipo Instrumento" ControlToValidate="ddlTipoInstrumento"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="220px"  />
                            <asp:RequiredFieldValidator ErrorMessage="Moneda" ControlToValidate="ddlMoneda" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Grupo Contable
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlGrupoContable" Width="220px" />
                            <asp:RequiredFieldValidator ErrorMessage="Grupo Contable" ControlToValidate="ddlGrupoContable"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cuenta Contable Fondo
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCuentaContableFondo1" onkeydown="return validarNumeros(event)" />
                            <asp:RequiredFieldValidator ErrorMessage="Cuenta Contable Fondo 1" ControlToValidate="tbCuentaContableFondo1"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <%--<div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cuenta Contable Fondo 2
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCuentaContableFondo2" />
                            <asp:RequiredFieldValidator ErrorMessage="Cuenta Contable Fondo 2" ControlToValidate="tbCuentaContableFondo2"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>--%>

            <%--<div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cuenta Contable Fondo 3
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCuentaContableFondo3" />
                            <asp:RequiredFieldValidator ErrorMessage="Cuenta Contable Fondo 3" ControlToValidate="tbCuentaContableFondo3"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>--%>

            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hd" />
    <asp:ValidationSummary runat="server" ID="vs" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
