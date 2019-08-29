<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOperacion.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmOperacion" %>
<!DOCTYPE html >
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Operaci&oacute;n</title>

    <script type="text/javascript">
        function soloLetras(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = " 0123456789abcdefghijklmnñopqrstuvwxyz";
            especiales = [8, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 37, 39, 46];
           if (letras.indexOf(tecla) == -1) return false;
        }
        function limpia() {
            var val = document.getElementById("miInput").value;
            var tam = val.length;
            for (i = 0; i < tam; i++) {
                if (!isNaN(val[i]))
                    document.getElementById("miInput").value = '';
            }
        }
</script>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Operaci&oacute;n</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" onkeypress="return soloLetras(event)" onblur="limpia()" MaxLength="6" style="text-transform:uppercase;" readonly="true" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Operaci&oacute;n" ControlToValidate="tbCodigo" runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo Op. Tesorer&iacute;a</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" Width="150px" ID="ddlTipoOperacion" />
                            <asp:RequiredFieldValidator ErrorMessage="Tipo Op. Tesorer&iacute;a" ControlToValidate="ddlTipoOperacion"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" style="text-transform:uppercase;" />
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" Width="120px" ID="ddlSituacion" />
                            <asp:RequiredFieldValidator ErrorMessage="Situaci&oacute;n" ControlToValidate="ddlSituacion"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Clase Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlClaseCuenta" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
        <input id="hdnCodigo" type="hidden" name="hdnCodigo" runat="server">
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
