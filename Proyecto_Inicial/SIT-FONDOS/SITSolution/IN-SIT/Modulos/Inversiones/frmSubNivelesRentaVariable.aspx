<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSubNivelesRentaVariable.aspx.vb" Inherits="Modulos_Inversiones_frmSubNivelesRentaVariable" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Renta Variable Local</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <script language="javascript" type="text/javascript">
        function CalcularMontoEF(myElem) {
            var elem = myElem.id.split('_');
            _cantidad = elem[0] + '_' + elem[1] + '_' + 'tbCantidadEF';
            _precio = elem[0] + '_' + elem[1] + '_' + 'tbPrecioEF';
            _resultado = elem[0] + '_' + elem[1] + '_' + 'tbTotalEF';
            var precio = parseFloat(document.getElementById(_precio).value);
            var cantidad = parseFloat(document.getElementById(_cantidad).value.replace(",", ""));
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(_resultado).value = total;
            }else {
                document.getElementById(_resultado).value = 0;
            }
            formatTotal(document.getElementById(_resultado));
        }
        function CalcularMontoE(myElem) {
            var elem = myElem.id.split('_');
            _cantidad = elem[0] + '_' + elem[1] + '_' + 'tbCantidadE';
            _precio = elem[0] + '_' + elem[1] + '_' + 'tbPrecioE';
            _resultado = elem[0] + '_' + elem[1] + '_' + 'tbTotalE';
            var precio = parseFloat(document.getElementById(_precio).value);
            var cantidad = parseFloat(document.getElementById(_cantidad).value.replace(",", ""));
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(_resultado).value = total;
            } else {
                document.getElementById(_resultado).value = 0;
            }
            formatTotal(document.getElementById(_resultado));
        }
        function formatTotal(myElem) {
            var num = myElem.value;
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
                }else{ cents = tmp2; }
                if (pos1 == -1) { tmp2 = '00'; }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
						num.substring(num.length - (4 * i + 3));
                myElem.value = (((sign) ? '' : '-') + num + '.' + tmp2);
                myElem.value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;
        }
        function formatCurrencyPrecio(myElem) {
            var num = document.getElementById(myElem.id).value;
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '0000000'
                var tmp2 = tmp1.substr(0, 7);
                var numoriginal = 0;
                var texto = '';
                var pos = 0;
                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                texto = num + '';
                pos = texto.lastIndexOf('.');
                if (pos > 0)
                { numoriginal = texto.substring(0, pos) }
                else { numoriginal = num }
                num = Math.floor(num * 100 + 0.50000000001);
                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '0000000';
                    cents = cents.substr(0, 7);
                } else { cents = tmp2; }
                if (pos1 == -1) { tmp2 = '0000000'; }
                for (var i = 0; i < Math.floor((numoriginal.length - (1 + i)) / 3); i++)
                    numoriginal = numoriginal.substring(0, numoriginal.length - (4 * i + 3)) + ',' +
					numoriginal.substring(numoriginal.length - (4 * i + 3));

                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + numoriginal + '.' + tmp2);
            }
            return false;
        }
        function formatCurrencyAccionesOperacion(myElem) {
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
                } else { cents = tmp2; }
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
        function Salir() {
            window.opener.location.href = "frmIngresoMasivoOperacionRV.aspx";
            window.close();
        }
		</script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
        <asp:ScriptManager runat="server" ID="SMLocal" />
        <div class="container-fluid">
            <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" runat="server">Sub Niveles - Renta Variable Local</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3><asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
            </header>
            <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-sm-4">
                            <asp:Button Text="Grabar" runat="server" ID="ibGrabar" Visible ="false"  /> 
                            <asp:Button Text="Retornar" runat="server" ID="ibRetornar" />
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label runat="server" id="Label11" class="col-sm-4 control-label">Correlativo</label>
                        <div class="col-sm-2">
                            <asp:TextBox runat="server" ID="txtCorrelativo" Width="50px" ReadOnly="true"  />
                        </div>
                    </div>
                </div>                 
                <div class="col-sm-3">
                    <div class="form-group">
                        <label runat="server" id="lblFondo" class="col-sm-4 control-label">Cantidad</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtCantidad" Width="100px" ReadOnly="true" CssClass ="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label runat="server" id="Label9" class="col-sm-4 control-label">Precio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtPrecio" Width="100px" ReadOnly="true" CssClass ="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label runat="server" id="Label10" class="col-sm-4 control-label">Total</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtTotal" Width="100px" ReadOnly="true" CssClass ="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode ="Conditional" >
            <ContentTemplate>
                <asp:GridView ID="dgNivel2" runat="server" SkinID="GridFooter" AutoGenerateColumns="False">
                <Columns>
					<asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign ="Center" />
						<ItemTemplate>
                            <asp:ImageButton id="ImageButton1" runat="server" SkinID="imgDelete" CommandArgument='<%# Eval("CodigoPrevOrdenDet") %>'
                            CommandName="_Delete" />
							<asp:Label id="lbCodigoPrevOrdenDet" runat="server" Visible="False" Text='<%# Eval("CodigoPrevOrdenDet") %>' />
						</ItemTemplate>
						<FooterTemplate>
							<asp:ImageButton id="ImageButton2" runat="server" SkinID="imgAdd" CommandName="Add" />
						</FooterTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cant. Instrumento Ejecuci&#243;n">
                        <ItemStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign ="Center" />
						<ItemTemplate>
							<asp:TextBox onblur="javascript:CalcularMontoE(this);" CssClass="Numbox-7" id="tbCantidadE" runat="server" ReadOnly="true"
                            Text='<%#Eval("CantidadOperacion")%>' Width="150px" />
						</ItemTemplate>
						<FooterTemplate>
                            <asp:TextBox onblur="javascript:CalcularMontoEF(this);" CssClass="Numbox-7" id="tbCantidadEF" runat="server" Width="150px" />
						</FooterTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Precio Ejecuci&#243;n">
                        <ItemStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign ="Center" />
						<ItemTemplate>
							<asp:TextBox id="tbPrecioE" runat="server" Width="150px" CssClass="Numbox-7" ReadOnly="true" Text='<%#Eval("PrecioOperacion")%>' />
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="tbPrecioEF" runat="server" Width="150px" CssClass="Numbox-7" ReadOnly="true" />
						</FooterTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Total Ejecuci&#243;n">
                        <ItemStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign ="Center" />
						<ItemTemplate>
							<asp:TextBox id="tbTotalE" runat="server" Width="150px" CssClass="Numbox-7" Text='<%#Eval("TotalOperacion")%>' ReadOnly="true" />
						</ItemTemplate>
						<FooterTemplate>
							<asp:TextBox id="tbTotalEF" runat="server" Width="150px" CssClass="Numbox-7" ReadOnly="true" />
						</FooterTemplate>
					</asp:TemplateField>
				</Columns>
                </asp:GridView> 
            </ContentTemplate>
            <Triggers >
                <asp:AsyncPostBackTrigger ControlID ="ibGrabar" EventName ="Click" />             
            </Triggers>
            </asp:UpdatePanel>
            </fieldset>
        </div>
        <asp:HiddenField ID="hdCodigoOperacion" runat="server" />
    </form>
</body>
</html>