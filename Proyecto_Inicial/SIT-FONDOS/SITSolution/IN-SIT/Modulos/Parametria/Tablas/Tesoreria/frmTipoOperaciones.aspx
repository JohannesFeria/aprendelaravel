    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTipoOperaciones.aspx.vb" Inherits="Modulos_Parametria_Tablas_Tesoreria_frmTipoOperaciones" %>

    <!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Tipo Operacion</title>

    <script type="text/javascript">
        function soloLetras(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = " 0123456789abcdefghijklmnñopqrstuvwxyz";
            especiales = [8, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 37, 39, 46];

            //            tecla_especial = false
            //            for (var i in especiales) {
            //                if (key == especiales[i]) {
            //                    tecla_especial = true;
            //                    break;
            //                }
            //            }

            if (letras.indexOf(tecla) == -1)
                return false;
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
                        <h2>
                           Mantenimiento de Tipo Operaciones</h2>
                    </div>
                </div>
            </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Tipo Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="2" onkeypress="return soloLetras(event)" onblur="limpia()" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Codigo Operacion"
                                ControlToValidate="tbCodigo" runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n:
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" MaxLength="30" Width="300px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Descripcion"
                                ControlToValidate="tbDescripcion" runat="server" Text="(*)" CssClass="validator"
                                ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row" style="text-align: right;">
         <asp:Button Text="Aceptar" runat="server" ID="ibAceptar" ValidationGroup="vgDetalle" />
                        <asp:Button Text="Retornar" runat="server" ID="ibSalir" CausesValidation="false" />            
        </div>
    </div>
    <asp:HiddenField ID="hd" runat="server" />
    <asp:ValidationSummary runat="server" ID="vgDetalle" ValidationGroup="vgDetalle"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>

