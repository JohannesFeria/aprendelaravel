<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmFactor.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmFactor" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Factores por Emisor</title>
    <script type="text/javascript">
        function showPopup() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');                
        }

        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbFactor":
                    num = tbFactor.value; break;
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
                    case "tbFactor":
                        tbFactor.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                }
            }
            return false;
        }

        function ValidaFecha(control) {
            if (!CheckDateTimeFormat(control.value, 'dd/mm/yyyy')) {
                //control.value = "";
                return false;
            } else {
                return true;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Factores por Emisor</h2></header>
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-8">
            <div class="form-group">
                <label class="col-md-3 control-label">Codigo Entidad</label>
                <div class="col-md-9">
                    <div class="input-append">
                    <asp:textbox id="tbCodigoEntidad" runat="server" CssClass="stlCajaTexto" Width="137px" MaxLength="50"></asp:textbox>
                    <asp:LinkButton ID="lkbBuscar" runat="server" CausesValidation="False" OnClientClick="return showPopup();" ><span class="add-on"><i class="awe-search"></i>
                    </span></asp:LinkButton>
                    </div>
                    &nbsp;
                    <strong>
                        <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" 
                        ErrorMessage="Código Entidad" ControlToValidate="tbCodigoEntidad" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                    </strong>
                    &nbsp;
                    <asp:textbox id="tbDescEntidad" runat="server" Width="220px" MaxLength="50" ReadOnly="True"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-4"></div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="form-group">
                <label class="col-md-3 control-label">Tipo Factor</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlTipoFactor" runat="server" Width="137px"></asp:dropdownlist>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <strong>
                        <asp:RequiredFieldValidator ID="rfvTipoFactor" runat="server" 
                        ErrorMessage="Tipo Factor" ControlToValidate="ddlTipoFactor" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                    </strong>
                    &nbsp;                    
                    Factor &nbsp;
                    <asp:textbox onblur="Javascript:formatCurrency(tbFactor.id); return false;" id="tbFactor" runat="server" Width="170px" MaxLength="50"></asp:textbox>
                    <strong>
                        <asp:RequiredFieldValidator ID="rfvFactor" runat="server" 
                        ErrorMessage="Factor" ControlToValidate="tbFactor" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                    </strong>
                </div>
            </div>
        </div>
        <div class="col-md-4"></div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="form-group">
                <label class="col-md-3 control-label">Fecha de Vigencia</label>
                <div class="col-md-9">
                    <div class="input-append date">
                    <asp:TextBox runat="server" ID="tbFechaVigencia" SkinID="Date" CssClass="input-medium" />                    
                    <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <strong>
                        <asp:RequiredFieldValidator ID="rfvFecha" runat="server" 
                        ErrorMessage="Fecha de Vigencia" ControlToValidate="tbFechaVigencia" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                    </strong>
                </div>
            </div>
        </div>
        <div class="col-md-4"></div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="form-group">
                <label class="col-md-3 control-label">Situación</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="137px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-4"></div>
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
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
