<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMontoNegociadoBVL.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmMontoNegociadoBVL" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Monto Negociado BVL</title>
    <script type="text/javascript">
        function showPopupComprador() {
            $('#hdTipoBusqueda').val('C');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '1200', '600', '');    
        }

        function showPopupVendedor() {
            $('#hdTipoBusqueda').val('V');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '1200', '600', '');    
        }

        function showPopupMnemonico() {
            $('#hdTipoBusqueda').val('M');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');    
        }

        function formatCurrency(cajatexto) {
            var num = "";
            switch (cajatexto) {
                case "tbMontoEfectivo":
                    num = tbMontoEfectivo.value; break;
                case "tbPrecio":
                    num = tbPrecio.value; break;
                case "tbCantidad":
                    num = tbCantidad.value; break;
                case "tbNumeroOperacion":
                    num = tbNumeroOperacion.value; break;
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
                    case "tbMontoEfectivo":
                        tbMontoEfectivo.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbPrecio":
                        tbPrecio.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbCantidad":
                        tbCantidad.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "tbNumeroOperacion":
                        tbNumeroOperacion.value = (((sign) ? '' : '-') + num); break;
                }
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header><h2>Mantenimiento de Monto Negociado BVL</h2></header>
        <br />
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha Operación</label>
                        <div class="col-sm-9" >
                            <div class="input-append date" id="spanFecha"  runat ="server">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Enabled="false" />
                                <span runat="server" class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Fecha Operación" ControlToValidate="tbFechaOperacion" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Número Operacion</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbNumeroOperacion" runat="server" Width="72px" MaxLength="50" onblur="Javascript:formatCurrency(tbNumeroOperacion.id); return false;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="Número Operación" ControlToValidate="tbNumeroOperacion" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Mnemonico</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox ID="tbCodigoMnemonico" MaxLength="15" runat="server" Width="100"></asp:TextBox>
                                <asp:LinkButton ID="lkbBuscarMnemonico" runat="server"  CausesValidation="false" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ErrorMessage="Mnemonico" ControlToValidate="tbCodigoMnemonico" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                            &nbsp;
                            <asp:TextBox ID="tbDescMnemonico" runat="server" Width="200" MaxLength="100" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Comprador</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox ID="tbCodigoComprador" MaxLength="50" runat="server" Width="100"></asp:TextBox>
                                <asp:LinkButton ID="lkbBuscarComprador" runat="server"  CausesValidation="false" OnClientClick="return showPopupComprador();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                ErrorMessage="Comprador" ControlToValidate="tbCodigoComprador" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                            &nbsp;
                            <asp:TextBox ID="tbDescripcionComprador" runat="server" Width="200" MaxLength="100"
                                ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Vendedor</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox ID="tbCodigoVendedor" MaxLength="50" runat="server" Width="100"></asp:TextBox>
                                <asp:LinkButton ID="lkbBuscarVendedor" runat="server"  CausesValidation="false" OnClientClick="return showPopupVendedor();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ErrorMessage="Vendedor" ControlToValidate="tbCodigoVendedor" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                            &nbsp;
                            <asp:TextBox ID="tbDescripcionVendedor" runat="server" Width="200" MaxLength="100"
                                ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Precio</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbPrecio" runat="server" MaxLength="50" Width="100px"  CssClass="Numbox-7" onblur="Javascript:formatCurrency(tbPrecio.id); return false;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                ErrorMessage="Precio" ControlToValidate="tbPrecio" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Cantidad &nbsp;
                            <asp:TextBox ID="tbCantidad" onblur="Javascript:formatCurrency(tbCantidad.id); return false;" CssClass="Numbox-7"
                                runat="server" Width="135" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                ErrorMessage="Cantidad" ControlToValidate="tbCantidad" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Monto Efectivo</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbMontoEfectivo" onblur="Javascript:formatCurrency(tbMontoEfectivo.id); return false;" CssClass="Numbox-7"
                                runat="server" Width="148px" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                ErrorMessage="Monto Efectivo" ControlToValidate="tbMontoEfectivo" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>                        
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Situación</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="115px">
                            </asp:DropDownList>
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
        </div>
    </div>
    <asp:HiddenField ID="hd" runat="server" />
    <asp:HiddenField ID="hdTipoBusqueda" runat="server" />
    </form>
</body>
</html>
