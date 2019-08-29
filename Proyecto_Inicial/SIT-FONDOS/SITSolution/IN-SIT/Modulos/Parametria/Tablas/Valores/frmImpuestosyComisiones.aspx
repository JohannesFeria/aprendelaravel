<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmImpuestosyComisiones.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmImpuestosyComisiones" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Impuestos y Comisiones</title>
    <script type="text/javascript" src="~/App_Themes/js/jquery.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            inicializar();
        });

        function inicializar() {
            $("#ddlTipoTarifa").change(function () {
                if (this.value.toString() == "F") {
                    $("#rowMonedaVF").show();
                } else {
                    $("#ddlMonedaValorFijo").val("");
                    $("#rowMonedaVF").hide();
                }
            });

            $("#ddlTipoTarifa").trigger("change");
        }

        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbValor":
                    num = tbValor.value; break;               
            }

            num = num.toString().replace(/$|,/g, '');
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '0000000'
                var tmp2 = tmp1.substr(0, 7);

                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '0000000';
                    cents = cents.substr(0, 7);
                }
                else
                { cents = tmp2; }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
									num.substring(num.length - (4 * i + 3));

                switch (cajatexto) {
                    case "tbValor":
                        tbValor.value = (((sign) ? '' : '-') + num + '.' + cents); break;                    
                }
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Impuestos y Comisiones</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Comisión</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbCodigo" runat="server" MaxLength="20" Width="208px" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Código Comisión" ControlToValidate="tbCodigo">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Descripción</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbDescripcion" runat="server" MaxLength="50" Width="300px" Height="32px" TextMode="MultiLine"></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Descripción" ControlToValidate="tbDescripcion">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <%--<div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mercado</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlMercado" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>--%>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Bolsa</label>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlBolsa" Width="180px" Enabled="false" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de Renta</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoRenta" runat="server" Width="248px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Indicador de Cálculo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlIndicador" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Base Cálculo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoTarifa" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Valor</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="tbValor" runat="server" MaxLength="15" Width="290px" CssClass="Numbox-7_31" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="Valor" ControlToValidate="tbValor">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>

    <div id="rowMonedaVF" class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Moneda del Valor Fijo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlMonedaValorFijo" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Genera Impuestos</label>
                <div class="col-sm-9" style="margin-top: 8px;">
                    <asp:CheckBox ID="chkGeneraImpuestos" runat="server"> </asp:CheckBox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>


    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situación</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
            CausesValidation="False" />
        <asp:HiddenField ID="hd" runat="server" />
        <asp:HiddenField ID="hdMercado" runat="server" />
        <asp:HiddenField ID="hdRenta" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
