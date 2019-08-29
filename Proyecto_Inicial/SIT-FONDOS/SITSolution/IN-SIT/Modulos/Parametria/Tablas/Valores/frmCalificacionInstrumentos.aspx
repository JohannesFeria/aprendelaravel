<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCalificacionInstrumentos.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmCalificacionInstrumentos" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Calificaci&oacute;n de Instrumento</title>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#txtPrioridad').val() == '') {
                $('#txtPrioridad').val('0');
            }
        });
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
                       Mantenimiento de Calificaci&oacute;n de Instrumento</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            C&oacute;digo Calificaci&oacute;n de Instrumento</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="tbCodigo" Width="150px" MaxLength="5" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Calificaci&oacute;n de Instrumento"
                                ControlToValidate="tbCodigo" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="200px"  MaxLength="40" style="text-transform:uppercase" />
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">Prioridad</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="txtPrioridad" Width="170px" CssClass="Numbox-0_5"  />
                            <asp:RequiredFieldValidator ErrorMessage="Prioridad" ControlToValidate="txtPrioridad" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Plazo</label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="ddlPlazo" Width="170px" />
                            <asp:RequiredFieldValidator ErrorMessage="Plazo" ControlToValidate="ddlPlazo" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                            <asp:RequiredFieldValidator ErrorMessage="Situaci&oacute;n" ControlToValidate="ddlSituacion"
                                runat="server" />
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
                <asp:Button Text="Retornar" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
