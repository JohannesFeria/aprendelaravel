<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLimiteTrading.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmLimiteTrading" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function formatCurrencyPorcentaje(myElem) {
            var num = document.getElementById(myElem.id).value;
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '00'
                var tmp2 = tmp1.substr(0, 2);

                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '00';
                    cents = cents.substr(0, 2);
                }
                else
                { cents = tmp2; }

                if (pos1 == -1) {
                    tmp2 = '00';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
							num.substring(num.length - (4 * i + 3));

                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + num + '.' + tmp2);
                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + num + '.' + tmp2);

            }
            return false;

        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Mantenimiento de Limites Trading</h2></header>
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Renta</label>
                <div class="col-sm-9">
                     <asp:dropdownlist id="ddlTipoRenta" runat="server" AutoPostBack="True" width="120px"></asp:dropdownlist>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                         ErrorMessage="Tipo Renta" ControlToValidate="ddlTipoRenta">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" Width="120px" Runat="server"></asp:dropdownlist>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                         ErrorMessage="Portafolio" ControlToValidate="ddlPortafolio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Grupo Límite</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlGrupoLimite" runat="server" AutoPostBack="True" width="220px" Enabled="False"></asp:dropdownlist>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                         ErrorMessage="Grupo Límite" ControlToValidate="ddlGrupoLimite">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Porcentaje</label>
                <div class="col-sm-9">
                    <asp:textbox onblur="formatCurrencyPorcentaje(this)" CssClass="Numbox-2" id="tbPorcentaje" runat="server" Width="120px" MaxLength="6"></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                         ErrorMessage="Porcentaje" ControlToValidate="tbPorcentaje">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Cargo</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoCargo" runat="server" Width="220px"></asp:dropdownlist>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                         ErrorMessage="Tipo Cargo" ControlToValidate="ddlTipoCargo">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9"></div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
            CausesValidation="False" />
    </div>
    </div>
    </form>
</body>
</html>
