<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoMasivoOperacionFX__.aspx.vb" Inherits="Modulos_Inversiones_frmIngresoMasivoOperacionFX__" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Operaci&oacute;n FX</title>
    <script type="text/javascript">
        function cambio(myElem) {
            var elem = myElem.id.split('_');
            var elem2 = elem;
            var elem3 = elem;
            var elem4 = elem;
            elem = elem[0] + '_' + elem[1] + '_' + 'hdCambio';
            elem2 = elem2[0] + '_' + elem2[1] + '_' + 'hdCambioTraza';
            elem4 = elem4[0] + '_' + elem4[1] + '_' + 'hdCambioTrazaFondo';
            document.getElementById(elem).value = "1";
            if ((elem3[2] == "ddlClaseInstrumentofx") || (elem3[2] == "ddlTipoTitulo") || (elem3[2] == "ddlOperacion") || (elem3[2] == "tbTotalOrden") || (elem3[2] == "tbPrecio") || (elem3[2] == "tbIntermediario") ||
            (elem3[2] == "tbTotalOperacion") || (elem3[2] == "tbPrecioFuturo") || (elem3[2] == "tbFondo1") || (elem3[2] == "tbFondo3") || (elem3[2] == "tbFondo3")) {
                document.getElementById(elem2).value = "1";
            }
            if ((elem3[2] == "tbFondo1") || (elem3[2] == "tbFondo3") || (elem3[2] == "tbFondo3")) {
                document.getElementById(elem4).value = "1";
            }
        }
        function ShowPopupTercerosGrilla(myElem) {
            var index;
            index = myElem.id.split('_')[1].substring(3);
            $('#_RowIndex').val(index);
            $('#_PopUp').val('GT');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '789', '500', '');
        }
        function ShowPopupTercerosGrillaF(myElem) {
            $('#_PopUp').val('FT');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '789', '500', '');
        }
        function formatCurrencyMontoOperacion(myElem) {
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
                else { cents = tmp2; }
                if (pos1 == -1) { tmp2 = '00'; }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
					num.substring(num.length - (4 * i + 3));
                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + num + '.' + tmp2);
                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + num + '.' + tmp2);
            }
            return false;
        }
        function formatCurrencyAsignacionFondos(myElem) {
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
                else
                { numoriginal = num }

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

                for (var i = 0; i < Math.floor((numoriginal.length - (1 + i)) / 3); i++)
                    numoriginal = numoriginal.substring(0, numoriginal.length - (4 * i + 3)) + ',' +
					numoriginal.substring(numoriginal.length - (4 * i + 3));

                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + numoriginal + '.' + tmp2);
            }
            return false;

        }
        function formatCurrencyTasa(myElem) {
            var num = document.getElementById(myElem.id).value;
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '00000'
                var tmp2 = tmp1.substr(0, 5);
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
                else
                { numoriginal = num }

                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();

                if (cents < 10) {
                    cents = "0" + cents + '00000';
                    cents = cents.substr(0, 5);
                }
                else
                { cents = tmp2; }

                if (pos1 == -1) {
                    tmp2 = '00000';
                }

                for (var i = 0; i < Math.floor((numoriginal.length - (1 + i)) / 3); i++)
                    numoriginal = numoriginal.substring(0, numoriginal.length - (4 * i + 3)) + ',' +
					numoriginal.substring(numoriginal.length - (4 * i + 3));

                document.getElementById(myElem.id).value = (((sign) ? '' : '-') + numoriginal + '.' + tmp2);
            }
            return false;

        }

        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
		(document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                                document.forms[0].elements[i].checked = true;
                            }
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
		(document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                            document.forms[0].elements[i].checked = false;
                        }
                    }
                }
            }
        }

        function confirmSwapDivisa() {
            var i;
            var count;
            count = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
		(document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                    if (document.forms[0].elements[i].disabled != true) {
                        if (document.forms[0].elements[i].checked) {
                            count = count + 1;
                        }
                    }
                }
            }
            if (count == 2) {
                return confirm('Desea vincular / desvincular las siguientes operaciones? ');
            }
            else {
                alertify.alert('Debe seleccionar dos registros para realizar la operación! ');
                return false;
            }
        }

        function ibSwapDivisa_PreCallBack(button) {



            return confirmSwapDivisa()
        }

        function BorrarHiddens() {
            document.getElementById('hdnIntermediario').value = "";
        }

        function desactivarEjecutar() {
            $("#btnAprobar").click(function () {
                $("#btnAprobar").attr("disabled", true);
            });
        }

    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Registro Masivo de Operaciones Fx</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Operador</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlOperador" Width="150px  " />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Instrumento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseInstrumento" Width="320px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Estado</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlEstado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">
            <asp:Button Text="Grabar" runat="server" ID="btnGrabar" />
            <asp:Button Text="Validar" runat="server" ID="btnValidar" />
            <asp:Button Text="Validar Exc. Trader" runat="server" ID="btnValidarTrader" />
            <asp:Button Text="SWAP Divisas" runat="server" ID="btnSwapDivisa" />
            <asp:Button Text="Ejecutar" runat="server" ID="btnAprobar" OnClientClick="desactivarEjecutar();" />
            <asp:Button Text="Exportar" runat="server" ID="btnExportar" />
            <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
        </div>
        <br />
        <div class="grilla-footer">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="GridFooter" ID="Datagrid1">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <HeaderTemplate>
                                    <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>&nbsp;
                                    <asp:ImageButton ID="Imagebutton1" runat="server" SkinID="imgDelete" CommandName="_Delete"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden")  %>'>
                                    </asp:ImageButton>
                                    <asp:Label ID="lbCodigoPrevOrden" CssClass="hidden" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden") %>'>
                                    </asp:Label>
                                    <asp:Label ID="lbClase" CssClass="hidden" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Clase") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ImageButton2" runat="server" SkinID="imgAdd" CommandName="Add">
                                    </asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Correlativo" HeaderText="N&#176;" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                            <asp:TemplateField HeaderText="Hora">
                                <ItemTemplate>
                                    <asp:TextBox Style="z-index: 0" ID="tbHora" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container, "DataItem.HoraOperacion") %>'>
                                    </asp:TextBox>
                                    <input id="hdCambio" type="hidden" name="hdCambio" runat="server" value ="1" />
                                    <input id="hdCambioTraza" type="hidden" name="hdCambioTraza" runat="server" />
                                    <input id="hdCambioTrazaFondo" type="hidden" name="hdCambioTrazaFondo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbOperador" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.UsuarioCreacion") %>'
                                        ReadOnly="true">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlClaseInstrumentofx" runat="server" Width="100px" />
                                    <asp:Label ID="lbClaseInstrumentofx" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ClaseInstrumentoFx") %>'
                                        CssClass="hidden" />
                                    <input type="hidden" id="hdClaseInstrumentofxTrz" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlClaseInstrumentofxF" runat="server" Width="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Moneda Negociada">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMonedaNeg" runat="server" Width="250px" />
                                    <asp:Label ID="lbMonedaNeg" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.MonedaNegociada") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlMonedaNegF" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Moneda">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMoneda" runat="server" Width="250px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbMoneda" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Moneda") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlMonedaF" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operaci&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOperacion" runat="server" Width="220px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbOperacion" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacion") %>'>
                                    </asp:Label>
                                    <input type="hidden" id="hdOperacionTrz" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlOperacionF" runat="server" Width="220px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Titulo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoTitulo" runat="server" Width="240px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbTipoTitulo" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoTipoTitulo") %>'>
                                    </asp:Label>
                                    <input type="hidden" id="hdTipoTituloTrz" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoTituloF" runat="server" Width="240px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Monto Orden">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotalOrden" runat="server" CssClass="Numbox-7" Text='<%# DataBinder.Eval(Container, "DataItem.MontoNominal") %>' />
                                    <input type="hidden" id="hdTotalOrdenTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.MontoNominal") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTotalOrdenF" runat="server" CssClass="Numbox-7" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio / T.C">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecio" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Precio") %>'
                                        CssClass="Numbox-7" />
                                    <input type="hidden" id="hdPrecioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Precio") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioF" runat="server" CssClass="Numbox-7" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Modalidad Forward">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlModalidad" runat="server" Width="150px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbModalidad" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.ModalidadForward") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlModalidadF" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="T.C. Futuro">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecioFuturo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PrecioFuturo") %>'
                                        CssClass="Numbox-7" />
                                    <input type="hidden" id="hdPrecioFuturoTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.PrecioFuturo") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioFuturoF" runat="server" CssClass="Numbox-7" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Tasa">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoTasa" runat="server" Width="100px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbTipoTasa" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.TipoTasa") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoTasaF" runat="server" Width="100px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="YTM %">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTasa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Tasa") %>'
                                        CssClass="Numbox-7" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTasaF" runat="server" CssClass="Numbox-7" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Fin Contrato">
                                <ItemTemplate>
                                    <div id="FechaContrato" runat="server" class="input-append date">
                                        <asp:TextBox ID="tbFechaContrato" runat="server" Width="70px" SkinID="Date" Text='<%# DataBinder.Eval(Container, "DataItem.FechaContrato") %>' />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="input-append date">
                                        <asp:TextBox ID="tbFechaContratoF" runat="server" Width="70px" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Vencimiento">
                                <ItemTemplate>
                                    <div id="FechaLiqui" runat="server" class="input-append date">
                                        <asp:TextBox ID="tbFechaLiquidacion" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.FechaLiquidacion") %>'
                                            SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="input-append date">
                                        <asp:TextBox ID="tbFechaLiquidacionF" runat="server" Width="70px" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Monto Operaci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotalOperacion" runat="server" CssClass="Numbox-7" Text='<%# DataBinder.Eval(Container, "DataItem.MontoOperacion") %>'
                                        Enabled="False">
                                    </asp:TextBox>
                                    <input type="hidden" id="hdTotalOperacionTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.MontoOperacion") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Intermediario">
                                <ItemTemplate>
                                    <div class="input-append">
                                        <asp:TextBox ID="tbIntermediario" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>' ReadOnly="True">
                                        </asp:TextBox>
                                        <div class="add-on">
                                            <asp:ImageButton ID="ibBIntermediario" runat="server" SkinID="imgSearch"></asp:ImageButton>
                                        </div>
                                    </div>
                                    <input type="hidden" id="hdIntermediario" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoTercero") %>' name="hdIntermediario" />
                                    <input type="hidden" runat="server" id="hdDescTercero" />
                                    <input type="hidden" runat="server" id="hdCodigoPais" />
                                    <input type="hidden" id="hdIntermediarioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="input-append">
                                        <asp:TextBox ID="tbIntermediarioF" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                        <span class="add-on">
                                            <asp:ImageButton ID="ibBIntermediarioF" runat="server" SkinID="imgSearch" CommandName="Item">
                                            </asp:ImageButton>
                                        </span>
                                    </div>
                                    <input type="hidden" id="hdIntermediarioF" runat="server" name="hdIntermediarioF" />
                                    <input type="hidden" id="hdCodigoPaisF" runat="server" name="hdCodigoPaisF" /><%--CMB OT 66768 20130530 --%>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medio Negociaci&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMedioNeg" runat="server" Width="110px">
                                    </asp:DropDownList>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MedioNegociacion") %>'
                                        Visible="False" ID="lbMedioNeg">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlMedioNegF" runat="server" Width="110px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fondos">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlfondos" runat="server" />
                                    <input type="hidden" id="hdFondo1Trz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Fondos") %>' />
                                    <input type="hidden" id="hdPortafolioSel" runat="server" value='<%# Eval("CodigoPortafolioSBS") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlfondosF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Motivo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMotivo" runat="server" Width="350px" />
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoMotivo") %>'
                                        Visible="False" ID="lbMotivo" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlMotivoF" runat="server" Width="350px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vinculaci&#243;n Swap">
                                <ItemTemplate>
                                    <asp:Label ID="lbSwapDivisa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SwapDivisa")%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" % " Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPorcentaje" runat="server" />
                                    <input type="hidden" id="hdPorcentaje" runat="server" value='<%# Eval("Porcentaje") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkPorcentajeF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibasignarfondo" runat="server" CommandName="asignarfondo" CommandArgument='<%# Eval("CodigoPrevOrden") %>'
                                        SkinID="imgMenu" AlternateText="Agregar SubNivel" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ibasignarfondoF" runat="server" CommandName="asignarfondoF"
                                        CommandArgument='<%# Eval("CodigoPrevOrden") %>' SkinID="imgMenu" AlternateText="Agregar SubNivel" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField runat="server" ID="hdFechaNegocio" />
                    <asp:HiddenField runat="server" ID="hdPuedeNegociar" />
                    <asp:HiddenField runat="server" ID="hdnIntermediario" />
                    <asp:HiddenField runat="server" ID="hdnOperador" />
                    <asp:HiddenField runat="server" ID="_PopUp" />
                    <asp:HiddenField runat="server" ID="_RowIndex" />
                    <asp:HiddenField runat="server" ID="_ControlID" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGrabar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnValidar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnValidarTrader" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSwapDivisa" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAprobar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Salir" runat="server" ID="btnSalir" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
