<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMantenimientoTasasEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmMantenimientoTasasEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Tasas de Encaje</title>
    <script type="text/javascript">
        function ShowPopup() {
            $('#hdTipoBusqueda').val('E');
            return showModalDialog('../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '800', '600', '');                  
        }
        function showPopupMnemonico() {
            $('#hdTipoBusqueda').val('M');
            return showModalDialog('../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '800', '600', '');            
        }

        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbValorTasaEncaje":
                    num = tbValorTasaEncaje.value; break;
                case "tbValorUnitario":
                    num = tbValorUnitario.value; break;
                case "tbValorNominal":
                    num = tbValorNominal.value; break;
                case "tbValorEfecColocado":
                    num = tbValorEfecColocado.value; break;
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

                if (pos1 == -1) {
                    tmp2 = '0000000';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));

                switch (cajatexto) {
                    case "tbValorTasaEncaje":
                        tbValorTasaEncaje.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "tbValorUnitario":
                        tbValorUnitario.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "tbValorNominal":
                        tbValorNominal.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                    case "tbValorEfecColocado":
                        tbValorEfecColocado.value = (((sign) ? '' : '-') + num + '.' + tmp2); break;
                }
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Tasas de Encaje</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Calificación</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlCalificacion" runat="server" Width="160px" ></asp:dropdownlist>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Calificación " ControlToValidate="ddlCalificacion">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Mnemónico</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbNemonico" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" CausesValidation="false" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Mnemónico " ControlToValidate="tbNemonico">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Emisor</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbemisor" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarEmisor" runat="server" CausesValidation="false" OnClientClick="return ShowPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="Emisor " ControlToValidate="tbemisor">(*)</asp:RequiredFieldValidator>
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
                        ErrorMessage="Fecha Vigencia" ControlToValidate="tbFechaVigencia">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Valor Tasa Encaje</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbValorTasaEncaje" runat="server" Width="144px" onkeypress="javascript:Numero();" onblur="Javascript:formatCurrency(tbValorTasaEncaje.id); return false;"></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                        ErrorMessage="Valor Tasa Encaje" ControlToValidate="tbValorTasaEncaje">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Observaciones</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="tbObservaciones" runat="server" Width="250px" ></asp:TextBox>                    
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="False" />
    </div>
    </div>
    <asp:HiddenField ID="hdTipoBusqueda" runat="server" />
    <asp:HiddenField ID="hd" runat="server" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
