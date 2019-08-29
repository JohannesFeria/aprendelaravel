<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmObligacionTecnica.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmObligacionTecnica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Obligación Técnica</title>
    <script type="text/javascript">

        function formatCurrency(cajatexto) {
            var num = "";
            num = txtMonto.value;
//            switch (cajatexto) {
//                case "txtPrecio":
//                    num = txtPrecio.value; break;
//                case "tbPrecio":
//                    num = tbPrecio.value; break;
//                case "tbCantidad":
//                    num = tbCantidad.value; break;
//                case "tbNumeroOperacion":
//                    num = tbNumeroOperacion.value; break;
//            }

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

                txtMonto.value = (((sign) ? '' : '-') + num + '.' + cents);
//                switch (cajatexto) {
//                    case "tbMontoEfectivo":
//                        tbMontoEfectivo.value = (((sign) ? '' : '-') + num + '.' + cents); break;
//                    case "tbPrecio":
//                        tbPrecio.value = (((sign) ? '' : '-') + num + '.' + cents); break;
//                    case "tbCantidad":
//                        tbCantidad.value = (((sign) ? '' : '-') + num + '.' + cents); break;
//                    case "tbNumeroOperacion":
//                        tbNumeroOperacion.value = (((sign) ? '' : '-') + num); break;
//                }
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header><h2>Obligación Técnica</h2></header>
        <br />  

        <fieldset>
            <legend>Ingreso de Datos</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Portafolio</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlPortafolio" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="Portafolio" ControlToValidate="ddlPortafolio" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha</label>
                        <div class="col-sm-9" >
                            <div class="input-append date" id="spanFecha"  runat ="server">
                                <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                <span id="Span1" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Fecha" ControlToValidate="txtFecha" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Precio</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtMonto" runat="server" MaxLength="50" Width="100px"  CssClass="Numbox-7" onblur="Javascript:formatCurrency(txtMonto.id); return false;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ErrorMessage="Monto" ControlToValidate="txtMonto" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
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

        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
                CausesValidation="False" />
                        <asp:HiddenField ID="hd" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
