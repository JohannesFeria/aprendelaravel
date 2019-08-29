<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLocacionBBH.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmLocacionBBH" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbCostoTrans":
                    num = tbCostoTrans.value; break;
                case "tbTasa":
                    num = tbTasa.value; break;
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
                    case "tbCostoTrans":
                        tbCostoTrans.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbTasa":
                        tbTasa.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                }
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Locaciones BBH</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Mercado</label>
                <div class="col-md-9">
                    <asp:textbox id="tbMercado" runat="server" Width="328px" MaxLength="50"></asp:textbox>
                    <strong><asp:RequiredFieldValidator ID="rfvMercado" runat="server" 
                        ErrorMessage="Mercado" ControlToValidate="tbMercado" ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Location (SOD Name)</label>
                <div class="col-md-9">
                    <asp:textbox id="tbLocacion" runat="server"  Width="328px" MaxLength="50"></asp:textbox>
                    <strong><asp:RequiredFieldValidator ID="rfvLocation" runat="server" 
                        ErrorMessage="Location SOD Name" ControlToValidate="tbLocacion" ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Location Custodio</label>
                <div class="col-md-9">
                    <asp:textbox id="tbLocacionCustodio" runat="server" Width="148px" MaxLength="5"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Set. Location Custodio</label>
                <div class="col-md-9">
                    <asp:textbox style="Z-INDEX: 0" id="tbSetLocCusatodio" runat="server" 
                        MaxLength="5" Width="148px"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Costo Trasacción</label>
                <div class="col-md-9">
                    <asp:textbox onblur="Javascript:formatCurrency(tbCostoTrans.id); return false;" 
                        id="tbCostoTrans" runat="server" Width="148px" MaxLength="30"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Costo Transacción" ControlToValidate="tbCostoTrans" 
                        ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Tasa Custodio</label>
                <div class="col-md-9">
                    <asp:TextBox onblur="Javascript:formatCurrency(tbTasa.id); return false;" 
                        style="Z-INDEX: 0" id="tbTasa" runat="server" Width="148px" MaxLength="30"></asp:textbox>
                        <strong>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Tasa Custodio" ControlToValidate="tbTasa" ForeColor="Red">(*)</asp:RequiredFieldValidator></strong>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Situación</label>
                <div class="col-md-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <header></header>
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="False" />
        <asp:HiddenField ID="hdCodigo" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>

