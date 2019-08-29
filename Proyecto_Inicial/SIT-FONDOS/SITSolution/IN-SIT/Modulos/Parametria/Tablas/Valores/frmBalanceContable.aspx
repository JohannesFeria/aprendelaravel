<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBalanceContable.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBalanceContable" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Balance Contable</title>
    <script type="text/javascript">
        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbTotalActivo":
                    num = tbTotalActivo.value; break;
                case "tbTotalPasivo":
                    num = tbTotalPasivo.value; break;
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
                    case "tbTotalActivo":
                        tbTotalActivo.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbTotalPasivo":
                        tbTotalPasivo.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbPatrimonio":
                        tbPatrimonio.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                }
            }
            return false;
        }
        function showPopup() {
            if ($('#hdTipoOperacion').val() == 'ins') {
                return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '1200', '600', '');         
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Balance Contable</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-3 control-label">Emisor</label>
            <div class="col-sm-9">
                <div class="input-append">
                    <asp:TextBox runat="server" ID="tbCodigoEmisor" Width="160px" />
                    <asp:LinkButton runat="server" ID="lkbBuscar" OnClientClick="return showPopup();" CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>                     
                </div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Emisor" ControlToValidate="tbCodigoEmisor" 
                    ForeColor="Red">(*)</asp:RequiredFieldValidator>
                <asp:TextBox ID="tbDescEmisor" runat="server" MaxLength="50" Width="135px" 
                    ReadOnly="True"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="col-md-6"></div>
    </div>
    <div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-3 control-label">Total Activo</label>
            <div class="col-sm-9">
                <asp:TextBox ID="tbTotalActivo" runat="server" Width="148px" MaxLength="50" onblur="Javascript:formatCurrency(tbTotalActivo.id); return false;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Total Activo" ControlToValidate="tbTotalActivo" 
                    ForeColor="Red">(*)</asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="col-md-6"></div>
    </div>
    <div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-3 control-label">Total Pasivo</label>
            <div class="col-sm-9">
                <asp:textbox id="tbTotalPasivo" runat="server" Width="148px" MaxLength="50" onblur="Javascript:formatCurrency(tbTotalPasivo.id); return false;"></asp:textbox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="Total Pasivo" ControlToValidate="tbTotalPasivo" 
                    ForeColor="Red">(*)</asp:RequiredFieldValidator>
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
                <asp:TextBox ID="tbPatrimonio" runat="server" Width="148px" MaxLength="50" onblur="Javascript:formatCurrency(tbPatrimonio.id); return false;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
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
                <asp:dropdownlist id="ddlSituacion" runat="server" Width="145px" ></asp:dropdownlist>
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
    </div>
    </div>
    <asp:HiddenField ID="hd" runat="server" />
    <asp:HiddenField ID="hdTipoOperacion" runat="server" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>    
</body>
</html>

