<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTiposTitulos.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmTiposTitulos" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Mantenimiento de Tipo de T&iacute;tulo</title>
    <script type="text/javascript">
        function validarBaseTIR(source, arguments) {
            var TipoRenta = $('#hdTipoRenta').val();
            if ($('#hdTipoRenta').val() == 'SI' && ($('#ddlBaseTir option:selected').text() == '--Seleccione--' || $('#ddlBaseTir option:selected').text() == '')) {
                arguments.IsValid = false;
            }
            return;
        }

        function validarBaseTIRDias(source, arguments) {
            var TipoRenta = $('#hdTipoRenta').val();
            if ($('#hdTipoRenta').val() == 'SI' && ($('#ddlDiasBaseTir option:selected').text() == '--Seleccione--' || $('#ddlDiasBaseTir option:selected').text() == '')) {
                arguments.IsValid = false;
            }
            return;
        }

        function validarBaseCupon(source, arguments) {
            var TipoRenta = $('#hdTipoRenta').val();
            if ($('#hdTipoRenta').val() == 'SI' && ($('#ddlBaseCupon option:selected').text() == '--Seleccione--' || $('#ddlBaseCupon option:selected').text() == '')) {
                arguments.IsValid = false;
            }
            return;
        }

        function validarBaseCuponDias(source, arguments) {
            var TipoRenta = $('#hdTipoRenta').val();
            if ($('#hdTipoRenta').val() == 'SI' && ($('#ddlDiasBaseCupon option:selected').text() == '--Seleccione--' || $('#ddlDiasBaseCupon option:selected').text() == '')) {
                arguments.IsValid = false;
            }
            return;
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
                        Tipo de T&iacute;tulo</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Renta
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ErrorMessage="Tipo Renta" ControlToValidate="ddlTipoRenta"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Cup&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCupon" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo" ControlToValidate="tbCodigo"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="220px" />
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código de Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="250px" />
                            <asp:RequiredFieldValidator ErrorMessage="Moneda" ControlToValidate="ddlMoneda" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Instrumento
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="220px" />
                            <asp:RequiredFieldValidator ErrorMessage="Tipo de Instrumento" ControlToValidate="ddlTipoInstrumento"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base TIR
                        </label>
                        <div class="col-sm-8">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">
                                            Base
                                        </label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList runat="server" ID="ddlBaseTir" Width="120px" />
                                            <asp:CustomValidator ID="cvBaseTir" ErrorMessage="Base Tir" ClientValidationFunction="validarBaseTIR"
                                                Text="(*)" CssClass="validator" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">
                                            Nro Dias
                                        </label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList runat="server" ID="ddlDiasBaseTir" Width="120px" />
                                            <asp:CustomValidator ID="cvDiasTir" ErrorMessage="Base Tir Dias" ClientValidationFunction="validarBaseTIRDias"
                                                Text="(*)" CssClass="validator" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Base Cup&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">
                                            Base
                                        </label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList runat="server" ID="ddlBaseCupon" Width="120px" />
                                            <asp:CustomValidator ID="cvBaseCupon" ErrorMessage="Base Cup&oacute;n" ClientValidationFunction="validarBaseCupon"
                                                Text="(*)" CssClass="validator" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">
                                            Nro Dias
                                        </label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList runat="server" ID="ddlDiasBaseCupon" Width="120px" />
                                            <asp:CustomValidator ID="cvDiasCupon" ErrorMessage="Base Cup&oacute;n Dias" ClientValidationFunction="validarBaseCuponDias"
                                                Text="(*)" CssClass="validator" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Periodicidad</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPeriodicidad" Width="180px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Indicador</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlIndicadorVAC" Width="180px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Amortizaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlAmortizacion" Width="180px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tasa Spread</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtTasaSpread" Width="180px" CssClass="Numbox-0_20" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Observaciones</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" TextMode="MultiLine" Rows="5" Width="250px" style="text-transform:uppercase" ID="txtObservaciones" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
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
    <asp:HiddenField runat="server" ID="hd" />
    <asp:HiddenField runat="server" ID="hdTipoRenta" />
    <asp:ValidationSummary runat="server" ID="vs" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>