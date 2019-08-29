<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMatriz.aspx.vb" Inherits="Modulos_Parametria_Tablas_Contabilidad_frmMatriz" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Matriz</title>
    <script type="text/javascript">
        function ValidaSoloNumeros() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                event.returnValue = false;
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
                        Matriz</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo
                        </label>
                        <div class="col-sm-8"> 
                            <asp:TextBox runat="server" ID="tbCodigo" Width="120px" onkeypress="ValidaSoloNumeros();" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo" ControlToValidate="tbCodigo"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="250px" />
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tabla
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTabla" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clave Interfaz
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbClaveInterfaz" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" />
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
            <asp:Button Text="Retornar" runat="server" ID="btnSalir" CausesValidation="false" />
        </div>
    </div>
    <input runat="server" type="hidden" name="hd" id="hd" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
