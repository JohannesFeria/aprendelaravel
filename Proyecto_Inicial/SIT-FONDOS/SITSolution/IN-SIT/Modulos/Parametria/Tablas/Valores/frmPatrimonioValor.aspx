<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPatrimonioValor.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmPatrimonioValor" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Patrimonio Valor Fondo</title>
    <script type="text/javascript">
        function Load() {

        }
        function showPopupInstrumento() {
            if ($('#hdTipoOperacion').val() == 'ins') {
                $('#hdTipoBusqueda').val('I');
                return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=TipoInstrumento', '1200', '600', '');                
            }
        }
        function showPopupMnemonico() {
            if ($('#hdTipoOperacion').val() == 'ins') {
                $('#hdTipoBusqueda').val('M');
                return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');
            }
        }
        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbPatrimonio":
                    num = tbPatrimonio.value; break;
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
                    case "tbPatrimonio":
                        tbPatrimonio.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                }
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Mantenimiento de Patrimonio Valor Fondo</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Instrumento</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbCodigoTipoInstrumento" Width="150px" />
                        <asp:LinkButton ID="lkbBuscarInstrumento" runat="server" CausesValidation="false" OnClientClick="return showPopupInstrumento();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Código Instrumento" ControlToValidate="tbCodigoTipoInstrumento" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        &nbsp;
                    <asp:textbox id="tbDescTipoInstrumento" runat="server" Width="160px" MaxLength="50" ReadOnly="True"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mnemonico</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbCodigoMnemonico" Width="150px" />
                        <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" CausesValidation="false" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Código Mnemonico" ControlToValidate="tbCodigoMnemonico" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        &nbsp;
                    <asp:textbox id="tbDescMnemonico" runat="server" Width="160px" MaxLength="50" ReadOnly="True"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Patrimonio</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="tbPatrimonio" runat="server" MaxLength="50" Width="148px" CssClass="Numbox-7" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="Patrimonio" ControlToValidate="tbPatrimonio" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>                        
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Vigencia</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaVigencia" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ErrorMessage="Fecha Vigencia" ControlToValidate="tbFechaVigencia" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
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
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="145px"></asp:dropdownlist>
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
        <asp:HiddenField ID="hdTipoBusqueda" runat="server" />
        <asp:HiddenField ID="hdTipoOperacion" runat="server" />
    </div>
    </div>
    </form>
</body>
</html>