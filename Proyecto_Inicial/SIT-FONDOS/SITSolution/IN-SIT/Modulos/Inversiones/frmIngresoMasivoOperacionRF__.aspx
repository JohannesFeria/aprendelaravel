<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoMasivoOperacionRF__.aspx.vb" Inherits="Modulos_Inversiones_frmIngresoMasivoOperacionRF__" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Renta Fija</title>
    <script type="text/javascript">
        function ShowModalMnemonico() {
            $('#_PopUp').val('B');
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '800', '600', '');            
        }
        function showPopupMnemonicoGrilla(myElem) {
            var index;
            index = myElem.id.split('_')[1].substring(3);
            $('#_RowIndex').val(index);
            $('#_PopUp').val('GM');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoAprob', '800', '600', ''); 
        }
        function showPopupMnemonicoGrillaF(myElem) {
            $('#_PopUp').val('FM');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoAprob', '800', '600', ''); 
        }
        function ShowPopupTercerosGrilla(myElem) {
            var index;
            index = myElem.id.split('_')[1].substring(3);
            $('#_RowIndex').val(index);
            $('#_PopUp').val('GT');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '800', '600', ''); 
        }
        function ShowPopupTercerosGrillaF(myElem) {
            $('#_PopUp').val('FT');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '800', '600', ''); 
        }
        function BorrarHiddens() {
            document.getElementById('hdnIntermediario').value = ""
            document.getElementById('hdnOperador').value = ""
        }
        function cambio(myElemId) {
            var elem = myElemId.id.split('_');
            var elem2 = elem;
            var elem3 = elem;
            var elem4 = elem;
            elem = elem[0] + '_' + elem[1] + '_' + 'hdCambio';
            document.getElementById(elem).value = "1";
            elem2 = elem2[0] + '_' + elem2[1] + '_' + 'hdCambioTraza';
            elem4 = elem4[0] + '_' + elem4[1] + '_' + 'hdCambioTrazaFondo';
            if ((elem3[2] == "tbNemonico") || (elem3[2] == "ddlOperacion") || (elem3[2] == "tbCantidad") || (elem3[2] == "tbPrecio") || (elem3[2] == "tbIntermediario") ||
                                (elem3[2] == "tbCantidadOperacion") || (elem3[2] == "tbPrecioOperacion") || (elem3[2] == "tbFondo1") || (elem3[2] == "tbFondo3") || (elem3[2] == "tbFondo3"))
                document.getElementById(elem2).value = "1";
            if ((elem3[2] == "tbFondo1") || (elem3[2] == "tbFondo3") || (elem3[2] == "tbFondo3"))
                document.getElementById(elem4).value = "1";
        }
        function HabilitaCondicion(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibCondicion';

            document.getElementById(elem).click();
            cambio(myElem)
        }
        function HabilitaCondicionF(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibCondicionF';

            document.getElementById(elem).click();
        }
        function HabilitaTipoTramo(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibTipoFondo';

            cambio(myElem)
            document.getElementById(elem).click();
        }
        function HabilitaTipoTramoF(myElem) {
            var elem = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'ibTipoFondoF';

            document.getElementById(elem).click();
        }

        function Salir() {
            location.href = "../../frmDefault.aspx";
        }

        function cantidadF_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbCantidadOperacionF';

            var cantidad = document.getElementById(myElem.id).value;
            if (cantidad != "") {
                document.getElementById(resultado).value = cantidad;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
        }
        function precio_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbCantidad';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotal';

            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            //formatTotal(document.getElementById(resultado));
        }

        function precioF_CalcularMonto(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbPrecioOperacionF';

            var precio = document.getElementById(myElem.id).value;
            if (precio != "") {
                document.getElementById(resultado).value = precio;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
        }

        function cantidad_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbPrecioOperacion';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacion';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            //formatTotal(document.getElementById(resultado));
        }

        function cantidadF_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbPrecioOperacionF';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacionF';
            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            //formatTotal(document.getElementById(resultado));
        }

        function precio_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbCantidadOperacion';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacion';

            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            //formatTotal(document.getElementById(resultado));
        }

        function precioF_CalcularMontoOperacion(myElem) {
            var elem = myElem.id.split('_');
            var resultado = myElem.id.split('_');
            elem = elem[0] + '_' + elem[1] + '_' + 'tbCantidadOperacionF';
            resultado = resultado[0] + '_' + resultado[1] + '_' + 'tbTotalOperacionF';

            var precio = document.getElementById(myElem.id).value;
            var cantidad = document.getElementById(elem).value;
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                document.getElementById(resultado).value = total;
            }
            else {
                document.getElementById(resultado).value = 0;
            }
            //formatTotal(document.getElementById(resultado));
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

        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }

        $(document).ready(function () {
            $("#ibExportar").click(function () {
                ShowProgress();
            });
            $("#ibImprimir").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Renta Fija</h2>
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
                            Tipo de Instrumento</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="320px" />
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
                            C&oacute;digo Mnem&oacute;nico:</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" />
                                <asp:LinkButton runat="server" ID="lkbMnemonicoModal" OnClientClick="return ShowModalMnemonico()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Operador</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlOperador" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Instrumento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseInstrumento" Width="280px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Estado</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlEstado" Width="120px" />
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
            <asp:Button Text="Ejecutar" runat="server" ID="btnAprobar" />
            <asp:Button Text="Exportar" runat="server" ID="btnExportar" />
            <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
        </div>
        <br />
        <div class="grilla-footer">
            <asp:UpdatePanel ID="updGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="GridFooter" ID="Datagrid1">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>&nbsp;
                                    <asp:ImageButton ID="Imagebutton1" runat="server" SkinID="imgDelete" CommandName="_Delete"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden") & "|" & CType(Container, GridViewRow).RowIndex %>'>
                                    </asp:ImageButton>
                                    <asp:Label ID="lbCodigoPrevOrden" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPrevOrden") %>'
                                        Visible="False">
                                    </asp:Label>
                                    <asp:Label ID="lbClase" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Clase") %>'
                                        Visible="False"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ImageButton2" runat="server" SkinID="imgAdd" CommandName="Add"
                                        CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Correlativo" HeaderText="N&#176;" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                            <asp:TemplateField HeaderText="Hora">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbHora" runat="server" SkinID="Hour" Text='<%# DataBinder.Eval(Container, "DataItem.HoraOperacion") %>'>
                                    </asp:TextBox>
                                    <input id="hdCambio" type="hidden" name="hdCambio" runat="server" value ="1">
                                    <input id="hdCambioTraza" type="hidden" name="hdCambioTraza" runat="server">
                                    <input id="hdCambioTrazaFondo" type="hidden" name="hdCambioTrazaFondo" runat="server">
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbOperador" runat="server" Width="110px" Text='<%# DataBinder.Eval(Container, "DataItem.UsuarioCreacion") %>'
                                        ReadOnly="true">
                                    </asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbOperadorF" Width="110px" Enabled="False" runat="server"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nem&#243;nico">
                                <ItemTemplate>
                                    <div class="input-append">
                                        <asp:TextBox ID="tbNemonico" Width="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CodigoNemonico") %>'
                                            ReadOnly="True" />
                                        <asp:LinkButton ID="ibBNemonico" CommandName="Item" runat="server" CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'>
                                <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <input type="hidden" runat="server" id="hdNemonico" />
                            <input type="hidden" id="hdNemonicoTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoNemonico") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <div class="input-append">
                                <asp:TextBox ID="tbNemonicoF" Width="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CodigoNemonico") %>' ReadOnly="True" />
                                <asp:LinkButton OnClientClick='return showPopupMnemonicoGrillaF(this)' ID="ibBNemonicoF" CommandName="Footer"
                                    runat="server" CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'>
                                <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Instrumento">
                        <ItemTemplate>
                            <asp:TextBox ID="tbInstrumento" Width="60px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Instrumento") %>'
                                Enabled="False">
                            </asp:TextBox>
                            <input id="hdClaseInstrumento" type="hidden" name="hdClaseInstrumento" value='<%# DataBinder.Eval(Container, "DataItem.Categoria") %>'
                                runat="server">
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbInstrumentoF" Width="60px" runat="server" Enabled="False"></asp:TextBox>
                            <input id="hdClaseInstrumentoF" type="hidden" name="hdClaseInstrumentoF" runat="server">
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Operaci&#243;n">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlOperacion" runat="server" Width="160px">
                            </asp:DropDownList>
                            <asp:Label ID="lbOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacion") %>'
                                Visible="False">
                            </asp:Label>
                            <input type="hidden" id="hdOperacionTrz" runat="server" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlOperacionF" runat="server" Width="160px">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cant. Instrumento">
                        <ItemTemplate>
                                    <asp:TextBox ID="tbCantidad" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cantidad") %>'
                                        Width="120px" Style="text-align: right" CssClass="Numbox-2">
                                    </asp:TextBox>
                                    <input type="hidden" id="hdCantidadTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Cantidad") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCantidadF" runat="server" onblur='javascript:cantidadF_CalcularMonto(this);'
                                        Width="120px" Style="text-align: right" CssClass="Numbox-2"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlIndice" runat="server" Width="80px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbIndice" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IndPrecioTasa") %>'
                                        Visible="False">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlIndiceF" runat="server" Width="80px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecio" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Precio") %>'
                                        Width="140px" Style="text-align: right;" CssClass="Numbox-7">
                                    </asp:TextBox>
                                    <input type="hidden" id="hdPrecioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Precio") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioF" runat="server" onblur='javascript:precioF_CalcularMonto(this);'
                                        CssClass="Numbox-7" Width="140px" Style="text-align: right;"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Tasa">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoTasa" runat="server" Width="100px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbTipoTasa" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container,"DataItem.TipoTasa") %>'>
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
                                        Style="text-align: right;" CssClass="Numbox-7" Width="140px">
                                    </asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTasaF" runat="server" CssClass="Numbox-7" Width="140px" Style="text-align: right;"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Orden">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MontoNominal") %>'
                                        ReadOnly="true" Style="text-align: right" CssClass="Numbox-2" Width="140px">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Plaza Neg.">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPlazaN" runat="server" Width="120px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbPlazaN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoPlaza") %>'
                                        Visible="False">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlPlazaNF" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Condici&#243;n">
                                <ItemTemplate>
                                    <asp:Label ID="lbCondicion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoCondicion") %>'
                                        Visible="False">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlCondicion" runat="server" Width="115px">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ibCondicion" runat="server" CommandName="Condicion" CssClass="hidden"
                                        CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCondicionF" runat="server" Width="120px">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ibCondicionF" runat="server" CommandName="CondicionF" CssClass="hidden">
                                    </asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Intermediario">
                                <ItemTemplate>
                                    <div class="input-append">
                                        <asp:TextBox ID="tbIntermediario" runat="server" Width="250px" Text='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>'
                                            ReadOnly="True" />
                                        <asp:LinkButton ID="ibBIntermediario" runat="server" 
                                            CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'>
                                <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <input type="hidden" id="hdIntermediario" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoTercero") %>'
                                name="hdIntermediario" />
                            <input type="hidden" id="hdDescTercero" runat="server" />
                            <input type="hidden" id="hdIntermediarioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <div class="input-append">
                                <asp:TextBox ID="tbIntermediarioF" runat="server" Width="250px" Text='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>'
                                    ReadOnly="True">
                                </asp:TextBox>
                                <asp:LinkButton ID="ibBIntermediarioF" runat="server" OnClientClick="return ShowPopupTercerosGrillaF(this)"  CommandName="Footer"
                                    CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'>
                                <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <input type="hidden" id="hdIntermediarioF" runat="server" name="hdIntermediarioF">
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- INI CMB REQ 67089 20130319 --%>
                    <asp:TemplateField HeaderText="Fixing">
                        <ItemTemplate>
                            <asp:TextBox ID="tbFixing" runat="server" Width="100px" CssClass="Numbox-7" Text='<%# DataBinder.Eval(Container, "DataItem.Fixing") %>'
                                Style="text-align: right;"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbFixingF" runat="server" Width="100px" CssClass="Numbox-7" Style="text-align: right;"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- FIN CMB REQ 67089 20130319 --%>
                    <asp:TemplateField HeaderText="Medio de Negociaci&#243;n">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlMedioNeg" runat="server" Width="120px">
                            </asp:DropDownList>
                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MedioNegociacion") %>'
                                Visible="False" ID="lbMedioNeg">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlMedioNegF" runat="server" Width="120px">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Fondo">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlTipoFondo" runat="server" Width="100px" Enabled="False">
                            </asp:DropDownList>
                            <asp:ImageButton ID="ibTipoFondo" runat="server" CommandName="TipoFondo" CssClass="hidden"
                                CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                            <asp:Label ID="lbTipoFondo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoFondo") %>'
                                Visible="False">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlTipoFondoF" runat="server" Width="100px" />
                            <asp:ImageButton ID="ibTipoFondoF" runat="server" CommandName="TipoFondoF" CssClass="hidden"
                                CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Tramo">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlTipoTramo" runat="server" Width="100px" />
                            <asp:Label ID="lbTipoTramo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoTramo") %>'
                                Visible="False">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlTipoTramoF" runat="server" Width="100px" Enabled="False">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha de Liquidaci&#243;n">
                        <ItemTemplate>
                                    <div id="FechaVal" runat="server" class="input-append date">
                                        <asp:TextBox ID="tbFechaLiquidacion" runat="server" SkinID="Date" Text='<%# DataBinder.Eval(Container, "DataItem.FechaLiquidacion") %>' />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div class="input-append date">
                                <asp:TextBox ID="tbFechaLiquidacionF" runat="server" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cant. Instrumento Ejecuci&#243;n">
                        <ItemTemplate>
                                    <asp:TextBox ID="tbCantidadOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CantidadOperacion") %>'
                                        Style="text-align: right" Width="140px" CssClass="Numbox-2">
                            </asp:TextBox>
                            <input type="hidden" id="hdCantidadOperacionTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CantidadOperacion") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbCantidadOperacionF" runat="server" Style="text-align: right" Width="140px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio Ejecuci&#243;n">
                        <ItemTemplate>
                            <asp:TextBox ID="tbPrecioOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PrecioOperacion") %>'
                                CssClass="Numbox-7" Width="140px" Style="text-align: right;">
                            </asp:TextBox>
                            <input type="hidden" id="hdPrecioOperacionTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.PrecioOperacion") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbPrecioOperacionF" runat="server" CssClass="Numbox-7" Width="140px"
                                Style="text-align: right;"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Ejecuci&#243;n">
                        <ItemTemplate>
                            <asp:TextBox ID="tbTotalOperacion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MontoOperacion") %>'
                                Style="text-align: right" CssClass="Numbox-2" Width="140px">
                            </asp:TextBox>
                        </ItemTemplate>
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
                            <asp:ImageButton ID="ibasignarfondoF" runat="server" CommandName="asignarfondoF" CommandArgument='<%# Eval("CodigoPrevOrden") %>'
                                SkinID="imgMenu" AlternateText="Agregar SubNivel" />
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGrabar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnValidar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnValidarTrader" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAprobar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnOperador" />
    <asp:HiddenField runat="server" ID="hdnIntermediario" />
    <asp:HiddenField runat="server" ID="hdFechaNegocio" />
    <asp:HiddenField runat="server" ID="hdPuedeNegociar" />
    <asp:HiddenField runat="server" ID="_PopUp" />
    <asp:HiddenField runat="server" ID="_RowIndex" />
    <asp:HiddenField runat="server" ID="_ControlID" />
    </form>
</body>
</html>
