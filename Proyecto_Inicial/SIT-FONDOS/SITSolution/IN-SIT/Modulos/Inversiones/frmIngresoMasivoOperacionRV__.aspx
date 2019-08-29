<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoMasivoOperacionRV__.aspx.vb"
    Inherits="Modulos_Inversiones_frmIngresoMasivoOperacionRV__" %>

<!DOCTYPE html >
<html>
<head id="Head1" runat="server">
    <title>Renta Variable</title>
    <%: Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <script type="text/javascript">
        function CalcularTotalOrden(myElem) {
            var elem = '#' + myElem.id.substring(0, myElem.id.lastIndexOf('_') + 1)
            var precio = $(elem + 'tbPrecio').NumBox('getRaw');
            var cantidad = $(elem + 'tbCantidad').NumBox('getRaw');

            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                $(elem + 'tbTotal').val(total);
                //$(elem + 'tbTotal').NumBox('setRaw', total);
            }
            else {
                $(elem + 'tbTotal').NumBox('setRaw', 0.00);
            }
        }
        function CalcularTotalEjecucion(myElem) {
            var elem = '#' + myElem.id.substring(0, myElem.id.lastIndexOf('_') + 1)
            var precio = $(elem + 'tbPrecioOperacion').NumBox('getRaw');
            var cantidad = $(elem + 'tbCantidadOperacion').NumBox('getRaw');

            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                $(elem + 'tbTotalOperacion').val(total);
                //$(elem + 'tbTotalOperacion').NumBox('setRaw', total);
            }
            else {
                $(elem + 'tbTotalOperacion').NumBox('setRaw', 0.00);
            }
        }
        function CalcularTotalEjecucionF(myElem) {
            var elem = '#' + myElem.id.substring(0, myElem.id.lastIndexOf('_') + 1)
            var precio = $(elem + 'tbPrecioOperacionF').NumBox('getRaw');
            var cantidad = $(elem + 'tbCantidadOperacionF').NumBox('getRaw');

            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                $(elem + 'tbTotalOperacionF').val(total);
                //$(elem + 'tbTotalOperacionF').NumBox('setRaw', total);
            }
            else {
                $(elem + 'tbTotalOperacionF').NumBox('setRaw', 0.00);
            }
        }
        function CalcularTotalOrdenF(myElem) {
            var elem = '#' + myElem.id.substring(0, myElem.id.lastIndexOf('_') + 1)
            var precio = $(elem + 'tbPrecioF').NumBox('getRaw');
            var cantidad = $(elem + 'tbCantidadF').NumBox('getRaw');

            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                //alert("total: " + total);
                $(elem + 'tbTotalF').val(total);
                $(elem + 'tbTotalOperacionF').val(total);
                $(elem + 'tbCantidadOperacionF').val(cantidad);
                $(elem + 'tbPrecioOperacionF').val(precio);
//                $(elem + 'tbTotalF').NumBox('setRaw', total);
//                $(elem + 'tbTotalOperacionF').NumBox('setRaw', total);
//                $(elem + 'tbCantidadOperacionF').NumBox('setRaw', cantidad);
//                $(elem + 'tbPrecioOperacionF').NumBox('setRaw', precio);
                $('#hdnTotalF').val(total);
            }
            else {
                $(elem + 'tbTotalF').NumBox('setRaw', 0.00);
                $(elem + 'tbTotalOperacionF').NumBox('setRaw', 0.00);
            }
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
        function BorrarHiddens() {
            document.getElementById('hdnTotalF').value = "";
            document.getElementById('hdnIntermediario').value = "";
            document.getElementById('hdnOperador').value = "";
        }
        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }
        //Popup
        function showPopupMnemonicoGrilla(myElem) {
            var index;
            index = myElem.id.split('_')[1].substring(3);
            $('#_RowIndex').val(index);
            $('#_PopUp').val('GM');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoAprob', '789', '500', '');
        }
        function showPopupMnemonicoGrillaF(myElem) {
            $('#_PopUp').val('FM');
            $('#_ControlID').val(myElem.id);
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoAprob', '789', '500', '');
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
        function ShowModalMnemonico() {
            $('#_PopUp').val('B');
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '789', '500', '')
        }

        function desactivarEjecutar() {
            $("#ibAprobar").click(function () {
                $("#ibAprobar").attr("disabled", true);
            });
        }

        function replaceAll(text, busca, reemplaza) {
            while (text.toString().indexOf(busca) != -1)
                text = text.toString().replace(busca, reemplaza);
            return text;
        }

        //Popup
        $(document).ready(function () {
            $("#ibExportar").click(function () {
                ShowProgress();
            });
        });    
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" runat="server">Registro Masivo de Ordenes - Renta Variable</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3><asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Operación</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Width="120px" />
                                    <span id="Img2" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo de Instrumento</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoInstrumento" runat="server" Width="200px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código Mnemónico</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" />
                                <asp:LinkButton runat="server" ID="lkbMnemonicoModal" OnClientClick="return ShowModalMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Operador</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlOperador" Width="120px" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Clase de Instrumento</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlClaseInstrumento" Width="250px" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Estado</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlEstado" Width="120px" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">
            <div class="form-group">
                <div class="col-md-6">
                    <asp:Button Text="Grabar" runat="server" ID="ibGrabar" />
                    <asp:Button Text="Validar" runat="server" ID="ibValidar" />
                    <asp:Button Text="Validar Exec. Trader" runat="server" ID="ibValidarTrader" />
                    <asp:Button Text="Ejecutar" runat="server" ID="ibAprobar" Style="height: 26px" OnClientClick="desactivarEjecutar();" />
                    <asp:Button Text="Exportar" runat="server" ID="ibExportar" />
                </div>
            </div>
        </div>
        <div class="grilla-footer">
            <asp:UpdatePanel ID="upnemonicoF" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="Datagrid1" runat="server" SkinID="GridFooter" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="N°">
                                <HeaderTemplate>
                                    <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                    <asp:ImageButton ID="ImageButton3" runat="server" CommandName="Select" CommandArgument='<%# Eval("CodigoPrevOrden") %>'
                                        SkinID="imgMenu" AlternateText="Agregar SubNivel" />
                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="_Delete" CommandArgument='<%# Eval("CodigoPrevOrden") %>'
                                        SkinID="imgDelete" AlternateText="Eliminar" />
                                    <asp:Label ID="lbCodigoPrevOrden" CssClass="stlPaginaTexto" runat="server" Text='<%# Eval("CodigoPrevOrden") %>'
                                        Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ImageButton2" runat="server" CommandName="Add" AlternateText="Agregar"
                                        SkinID="imgAdd" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Correlativo" HeaderText="N&#176;"></asp:BoundField>
                            <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                            <asp:TemplateField HeaderText="Hora Orden">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbHora" runat="server" Width="57px" SkinID="Hour" Text='<%# Eval("HoraOperacion") %>'
                                        MaxLength="8" />
                                    <input id="hdCambio" type="hidden" name="hdCambio" runat="server">
                                    <input id="hdCambioTraza" type="hidden" name="hdCambioTraza" runat="server">
                                    <input id="hdCambioTrazaFondo" type="hidden" name="hdCambioTrazaFondo" runat="server">
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbHoraF" SkinID="Hour" Width="57px" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbOperador" runat="server" Width="70px" Text='<%# Eval("UsuarioCreacion") %>'
                                        Enabled="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbOperadorF" Width="70px" runat="server" Enabled="False" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nem&#243;nico">
                                <ItemTemplate>
                                    <div class="input-append">
                                        <asp:TextBox runat="server" ID="tbNemonico" Text='<%# Eval("CodigoNemonico") %>'
                                            ReadOnly="True" />
                                        <asp:LinkButton  ID="lkbShowModal" CommandName="Item" runat="server" CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'> <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                    </div>
                                    <asp:ImageButton ID="ibBNemonico" runat="server" CssClass="hidden" />
                                    <input type="hidden" id="hdNemonico" runat="server" />
                                    <input type="hidden" id="hdNemonicoTrz" runat="server" value='<%# Eval("CodigoNemonico") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="input-append">
                                        <asp:TextBox runat="server" ID="tbNemonicoF" />
                                        <asp:LinkButton ID="lkbShowModal" runat="server" CommandName="Footer" OnClientClick="showPopupMnemonicoGrillaF(this)"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                    </div>
                                    <asp:ImageButton ID="ibBNemonicoF" runat="server" Visible="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Instrumento">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbInstrumento" Width="50px" runat="server" Text='<%# Eval("Instrumento") %>'
                                        Enabled="False" />
                                    <input id="hdClaseInstrumento" type="hidden" name="hdClaseInstrumento" value='<%# Eval("Categoria") %>'
                                        runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbInstrumentoF" Width="50px" CssClass="stlCajaTexto" runat="server"
                                        Enabled="False" />
                                    <input id="hdClaseInstrumentoF" type="hidden" name="hdClaseInstrumentoF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operaci&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOperacion" runat="server" Width="100px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbOperacion" runat="server" Text='<%# Eval("CodigoOperacion") %>'
                                        Visible="False" />
                                    <input type="hidden" id="hdOperacionTrz" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlOperacionF" runat="server" Width="100px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant. Instrumento">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCantidad" runat="server" Text='<%# Eval("Cantidad") %>' Style="text-align: right" CssClass="Numbox-7" onblur='javascript:CalcularTotalOrden(this);' />
                                    <%--onblur='javascript:CalcularTotalOrden(this);'--%>
                                    <input type="hidden" id="hdCantidadTrz" runat="server" value='<%# Eval("Cantidad") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCantidadF" runat="server" CssClass="Numbox-7" />
                                    <%--onblur='javascript:CalcularTotalOrdenF(this);'--%>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecio" runat="server" Text='<%# Eval("Precio") %>' CssClass="Numbox-7" />
                                    <%--onblur='javascript:CalcularTotalOrden(this);'--%>
                                    <input type="hidden" id="hdPrecioTrz" runat="server" value='<%# Eval("Precio") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioF" runat="server" onblur='javascript:CalcularTotalOrdenF(this);' CssClass="Numbox-7" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Orden">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotal" runat="server" CssClass="Numbox-7" ReadOnly="True" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTotalF" runat="server" CssClass="Numbox-7" ReadOnly="True" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Condici&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCondicion" runat="server" Width="120px">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ibCondicion" runat="server" CommandName="Condicion" CssClass="hidden" />
                                    <asp:Label ID="lbTipoCondicion2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoCondicion") %>'
                                        Visible="False" />
                                    <input id="hdTotal" type="hidden" name="hdTotal" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCondicionF" runat="server" Width="120px">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ibCondicionF" runat="server" CommandName="CondicionF" CssClass="hidden" />
                                    <input id="hdTotalF" type="hidden" name="hdTotalF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Intermediario">
                                <ItemTemplate>
                                    <div class="input-append">
                                        <asp:TextBox ID="tbIntermediario" runat="server" Width="250px" Text='<%# Eval("Intermediario") %>'
                                            ReadOnly="True" />
                                        <asp:LinkButton ID="ibBIntermediario" runat="server" 
                                            CommandName="Item" CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'>
                            <span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                    </div>
                                    <input type="hidden" id="hdIntermediario" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoTercero") %>'
                                        name="hdIntermediario" />
                                    <input type="hidden" id="hdDescTercero" runat="server" />
                                    <input type="hidden" id="hdIntermediarioTrz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Intermediario") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="input-append">
                                        <asp:TextBox ID="tbIntermediarioF" runat="server" Width="250px" />
                                        <asp:LinkButton ID="ibBIntermediarioF" runat="server" OnClientClick="ShowPopupTercerosGrillaF(this)"
                                            CommandName="Footer" CommandArgument='<%# "|" & CType(Container, GridViewRow).RowIndex %>'><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                    </div>
                                    <input type="hidden" id="hdIntermediarioF" runat="server" name="hdIntermediarioF" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contacto">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlContacto" runat="server" Width="150px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbContacto" runat="server" Text='<%# Eval("CodigoContacto") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlContactoF" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medio Transmisión">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMedioTrans" runat="server" Width="150px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbMedioTrans" runat="server" Text='<%# Eval("MedioNegociacion") %>'
                                        Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlMedioTransF" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Fondo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoFondo" runat="server" Width="100px" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ibTipoFondo" runat="server" CommandName="TipoFondo" CssClass="hidden" />
                                    <asp:Label ID="lbTipoFondo" runat="server" Text='<%# Eval("TipoFondo") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoFondoF" runat="server" Width="100px" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ibTipoFondoF" runat="server" CommandName="TipoFondoF" CssClass="hidden" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Tramo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoTramo" runat="server" Width="100px" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbTipoTramo" runat="server" Text='<%# Eval("TipoTramo") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoTramoF" runat="server" Width="100px" Enabled="False">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Plaza Neg.">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPlazaN" runat="server" Width="100px" Enabled="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbPlazaN" runat="server" CssClass="stlPaginaTexto" Text='<%# Eval("CodigoPlaza") %>'
                                        Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlPlazaNF" runat="server" Width="100px" Enabled="True">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Intervalo Precio Aut.">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbIntervaloPrecio" runat="server" Text='<%# Eval("IntervaloPrecio") %>'
                                        CssClass="Numbox-7" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbIntervaloPrecioF" runat="server" CssClass="Numbox-7" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hora Ejecución">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbHoraEje" runat="server" SkinID="Hour" Text='<%# Eval("HoraEjecucion") %>'
                                        MaxLength="8" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbHoraEjeF" SkinID="Hour" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant. Instrumento Ejecuci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCantidadOperacion" runat="server" Text='<%# Eval("CantidadOperacion")%>' CssClass="Numbox-7" Style="text-align: right" onblur='javascript:CalcularTotalEjecucion(this);' />
                                        <%--onblur='javascript:CalcularTotalEjecucion(this);'--%>
                                    <input type="hidden" id="hdCantidadOperacionTrz" runat="server" value='<%# Eval("CantidadOperacion") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCantidadOperacionF" runat="server" CssClass="Numbox-7" onblur='javascript:CalcularTotalEjecucionF(this);' />
                                    <%--onblur='javascript:CalcularTotalEjecucionF(this);'--%>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio Ejecuci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecioOperacion" runat="server" Text='<%# Eval("PrecioOperacion") %>' CssClass="Numbox-7" onblur='javascript:CalcularTotalEjecucion(this);'/>
                                        <%--onblur='javascript:CalcularTotalEjecucion(this);'--%>
                                    <input type="hidden" id="hdPrecioOperacionTrz" runat="server" value='<%# Eval("PrecioOperacion") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioOperacionF" runat="server" CssClass="Numbox-7" onblur='javascript:CalcularTotalEjecucionF(this);' />
                                    <%--onblur='javascript:CalcularTotalEjecucionF(this);'--%>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Ejecuci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotalOperacion" runat="server" Text='<%# Eval("TotalOperacionRV") %>'
                                        ReadOnly="True" CssClass="Numbox-7" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTotalOperacionF" runat="server" CssClass="Numbox-7" ReadOnly="True" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fondos">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlfondos" runat="server" />
                                    <input type="hidden" id="hdFondo1Trz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Fondos") %>' />
                                    <input type="hidden" id="hdPortafolioSel" runat="server" value='<%# Eval("CodigoPortafolioSBS")  %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlfondosF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hdnOperador" runat="server" />
                    <asp:HiddenField ID="hdnIntermediario" runat="server" />
                    <asp:HiddenField ID="hdnTotalF" runat="server" />
                    <asp:HiddenField ID="hdFechaNegocio" runat="server" />
                    <asp:HiddenField ID="hdPuedeNegociar" runat="server" />
                    <asp:HiddenField runat="server" ID="_PopUp" />
                    <asp:HiddenField runat="server" ID="_RowIndex" />
                    <asp:HiddenField runat="server" ID="_ControlID" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibGrabar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ibValidar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ibValidarTrader" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ibAprobar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
        </div>
    </div>
    </form>
</body>
</html>