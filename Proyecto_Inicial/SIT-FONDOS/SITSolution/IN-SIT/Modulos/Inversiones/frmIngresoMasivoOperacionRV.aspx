<%@ Page Language="VB" AutoEventWireup="false" EnableEventValidation="false" CodeFile="frmIngresoMasivoOperacionRV.aspx.vb" Inherits="Modulos_Inversiones_frmIngresoMasivoOperacionRV" %>
<!DOCTYPE html >
<html>
<head id="Head1" runat="server">
    <title>Renta Variable</title>
    <%: Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <script type="text/javascript">

        //INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 21/05/2018
        $(document).ready(function () {
            var control = '';
            var controlfecha = '';
            // ==== INICIO | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Actualizar Combo Portafolios cuando cambia un Nemonico
            $("[id$='tbNemonicoF']").change(function () {

                var nombreTextboxNemonico = $(this).attr('id').toString();
                var nombreEnArray = nombreTextboxNemonico.split('_');
                var prefijoControlEnGrilla = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';

                actualizarComboPortafolioCuandoCambioNemonico(nombreTextboxNemonico, prefijoControlEnGrilla + "ddlPortafolioF");
            });

            $("[id$='tbNemonico']").change(function () {

                var nombreTextboxNemonico = $(this).attr('id').toString();
                var nombreEnArray = nombreTextboxNemonico.split('_');
                var prefijoControlEnGrilla = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';

                actualizarComboPortafolioCuandoCambioNemonico(nombreTextboxNemonico, prefijoControlEnGrilla + "ddlfondos");
            });
            // ==== FIN | PROYECTO FONDOS-II | ZOLUXIONES | CRumiche | 2018-08-07 | Actualizar Combo Portafolios cuando cambia un Nemonico


            $("[id$='ddlPortafolioF']").change(function () {

                $("[id$='HdPortafolioF']").val($(this).val());
                var separador = $(this).attr('Id').split('_');
                control = '#Datagrid1_' + separador[1] + '_tFechaOperacionF';
                controlfecha = '#Datagrid1_' + separador[1] + '_hdFechaOperacionF';
                var fecha = CargarFechaNegocio($(this).val());
                $(control).val(fecha);
                $(controlfecha).val(fecha);
                //alert($(fechaControl).val());

            });

            $("[id$='ddlPortafolioF']").click(function () {
                var separador = $(this).attr('Id').split('_');
                var nemonico = '#Datagrid1_' + separador[1] + '_tbNemonicoF';
                if ($(nemonico).val().length <= 0) {
                    alertify.alert('<b>Ingrese el Nemónico</b>');
                }
            });

            $("[id$='ddlfondos']").change(function () {
                var separador = $(this).attr('Id').split('_');
                var HdfondosHtml = '#Datagrid1_' + separador[1] + '_Hdfondos';
                $(HdfondosHtml).val($(this).val());
                control = '#Datagrid1_' + separador[1] + '_tFechaOperacion';
                controlfecha = '#Datagrid1_' + separador[1] + '_hdFechaOperacion';
                var fecha = CargarFechaNegocio($(this).val());
                $(control).val(fecha);
                $(controlfecha).val(fecha);
                //alert($(fechaControl).val());

            });

            $("[id$='ddlfondos']").click(function () {
                var separador = $(this).attr('Id').split('_');
                var nemonico = '#Datagrid1_' + separador[1] + '_tbNemonico';
                if ($(nemonico).val().length <= 0) {
                    alertify.alert('<b>Ingrese el Nemónico</b>');
                }
            });

            function CargarFechaNegocio(Idportafolio) {
                var param = { Idportafolio: Idportafolio };
                $.ajax({
                    url: "../../MetodosWeb.aspx/GetPortafolioSelectById",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8;",
                    success: function (data) {
                        $(control).val(data.d);
                        $(controlfecha).val(data.d);
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
                });
            }
        });
        //FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 21/05/2018

        function CalcularTotalOrden(myElem) {
            var elem = '#' + myElem.id.substring(0, myElem.id.lastIndexOf('_') + 1)
            var precio = $(elem + 'tbPrecio').NumBox('getRaw');
            var cantidad = $(elem + 'tbCantidad').NumBox('getRaw');
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                $(elem + 'tbTotal').val(total);
            } else {
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
            } else {
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
            } else {
                $(elem + 'tbTotalOperacionF').NumBox('setRaw', 0.00);
            }
        }
        function CalcularTotalOrdenF(myElem) {
            var elem = '#' + myElem.id.substring(0, myElem.id.lastIndexOf('_') + 1)
            var precio = $(elem + 'tbPrecioF').NumBox('getRaw');
            var cantidad = $(elem + 'tbCantidadF').NumBox('getRaw');
            if (cantidad != "" && precio != "") {
                var total = precio * cantidad;
                $(elem + 'tbTotalF').val(total);
                $(elem + 'tbTotalOperacionF').val(total);
                $(elem + 'tbCantidadOperacionF').val(cantidad);
                $(elem + 'tbPrecioOperacionF').val(precio);
                $('#hdnTotalF').val(total);
            } else {
                $(elem + 'tbTotalF').NumBox('setRaw', 0.00);
                $(elem + 'tbTotalOperacionF').NumBox('setRaw', 0.00);
            }
        }
        function SelectAll(CheckBoxControl) {
            var i;
            if (CheckBoxControl.checked == true) {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                                document.forms[0].elements[i].checked = true;
                            }
                        }
                    }
                }
            } else {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('Datagrid1') > -1)) {
                        if (document.forms[0].elements[i].name.toString().slice(16, 29) != 'chkPorcentaje') {
                            document.forms[0].elements[i].checked = false;
                        }
                    }
                }
            }
        }
        $('form').live("submit", function () {
            //$('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            ShowProgress();
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
                        <label class="col-sm-5 control-label">Fecha Operación</label>
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
                        <label class="col-sm-5 control-label">Tipo de Instrumento</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoInstrumento" runat="server" Width="200px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Código Mnemónico</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigoMnemonico" Width="200px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Operador</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlOperador" Width="120px" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Clase de Instrumento</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlClaseInstrumento" Width="250px" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Estado</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlEstado" Width="120px" runat="server" />
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
                    <%--<asp:UpdatePanel ID="upBotones" runat="server" UpdateMode ="Conditional" >
                    <ContentTemplate>--%>
                        <asp:Button Text="Grabar" runat="server" ID="ibGrabar" />
                        <asp:Button Text="Validar" runat="server" ID="ibValidar" Visible ="false" />
                        <asp:Button Text="Validar Exec. Trader" runat="server" ID="ibValidarTrader" />
                        <asp:Button Text="Ejecutar" runat="server" ID="ibAprobar" Style= "height: 26px" />
                        <asp:Button Text="Exportar" runat="server" ID="ibExportar" />
                    <%--</ContentTemplate>
                        <Triggers >
                            <asp:AsyncPostBackTrigger ControlID="ibAprobar" EventName="Click" />
                            <asp:PostBackTrigger ControlID="ibExportar"  />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
        <div class="grilla-footer">
            <%--<asp:UpdatePanel ID="upnemonicoF" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <asp:GridView ID="Datagrid1" runat="server" SkinID="GridFooter" 
                AutoGenerateColumns="False" style="margin-bottom: 0px">
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
                                    <asp:Label ID="lbCodigoPrevOrden" CssClass="stlPaginaTexto" runat="server" Text='<%# Eval("CodigoPrevOrden") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="ImageButton2" runat="server" CommandName="Add" AlternateText="Agregar" SkinID="imgAdd" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Correlativo" HeaderText="N&#176;"></asp:BoundField>
                            <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                            <asp:TemplateField HeaderText="Hora">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbHora" runat="server" Width="40px" SkinID="Hour" Text='<%# Eval("HoraOperacion") %>' ReadOnly="true"  MaxLength="8" />
                                    <input id="hdCambio" type="hidden" name="hdCambio" runat="server" />
                                    <input id="hdCambioTraza" type="hidden" name="hdCambioTraza" runat="server" />
                                    <input id="hdCambioTrazaFondo" type="hidden" name="hdCambioTrazaFondo" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbHoraF" SkinID="Hour" Width="40px" runat="server" ReadOnly="true"  />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbOperador" runat="server" Width="70px" Text='<%# Eval("UsuarioCreacion") %>' Enabled="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbOperadorF" Width="70px" runat="server" Enabled="False" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nem&#243;nico">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="tbNemonico" Text='<%# Eval("CodigoNemonico") %>' Width="130px" />
                                    <input id="hdClaseInstrumento" type="hidden" name="hdClaseInstrumento" value='<%# Eval("Categoria") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="tbNemonicoF" Width="130px" />
                                    <input id="hdClaseInstrumentoF" type="hidden" name="hdClaseInstrumentoF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 22/05/2018--%>
                           <asp:TemplateField HeaderText="Portafolio">
                              <ItemTemplate>
                                     <asp:HiddenField ID="HdCodigoOrden" runat="server" Value='<%# Eval("CodigoPrevOrden") %>' />
                                       <asp:HiddenField ID="Hdfondos" runat="server"  Value='<%# Eval("CodigoPortafolioSelec") %>' />
                                    <asp:DropDownList ID="ddlfondos" runat="server" Width="130px" />
                                    <input type="hidden" id="hdFondo1Trz" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.Fondos") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlPortafolioF" runat="server" Width="200px" />
                                     <asp:HiddenField ID="HdPortafolioF" runat="server"  Value='0' />
                                    <asp:HiddenField ID="HdCodigoOrdenF" runat="server" 
                                        Value='<%# Eval("CodigoPrevOrden") %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Operación">
                                <ItemTemplate>
                                    <asp:TextBox ID="tFechaOperacion" runat="server" SkinID="Date" ReadOnly="True" 
                                        Text='<%# UIUtility.ConvertirFechaaString(Eval("FechaOperacion")) %>' ></asp:TextBox>
                                          <asp:HiddenField ID="hdFechaOperacion" runat="server"   Value='<%# UIUtility.ConvertirFechaaString(Eval("FechaOperacion")) %>'/>
                                </ItemTemplate>
                                 <FooterTemplate>
                                    <asp:TextBox ID="tFechaOperacionF" ReadOnly="true" runat="server" SkinID="Date" Width="100px"/>
                                    <asp:HiddenField ID="hdFechaOperacionF" runat="server" />

                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF009 - AGREGADO | 22/05/2018--%>
                            <asp:TemplateField HeaderText="Operaci&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOperacion" runat="server" Width="80px" />
                                    <asp:Label ID="lbOperacion" runat="server" Text='<%# Eval("CodigoOperacion") %>' Visible="False" />
                                    <input type="hidden" id="hdOperacionTrz" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlOperacionF" runat="server" Width="80px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant. Instrumento" >
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCantidad" runat="server" Text='<%# Eval("Cantidad") %>' CssClass="Numbox-7" Width ="100px"   
                                    onblur='javascript:CalcularTotalOrden(this);' />
                                    <input type="hidden" id="hdCantidadTrz" runat="server" value='<%# Eval("Cantidad") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCantidadF" runat="server" CssClass="Numbox-7" Width ="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecio" runat="server" Text='<%# Eval("Precio") %>' CssClass="Numbox-7" Width ="100px" />
                                    <input type="hidden" id="hdPrecioTrz" runat="server" value='<%# Eval("Precio") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioF" runat="server" onblur='javascript:CalcularTotalOrdenF(this);' CssClass="Numbox-7" Width ="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Orden">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotal" runat="server" CssClass="Numbox-7" ReadOnly="True" Width ="100px" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTotalF" runat="server" CssClass="Numbox-7" ReadOnly="True" Width ="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Condici&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCondicion" runat="server" Width="120px">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="ibCondicion" runat="server" CommandName="Condicion" CssClass="hidden" />
                                    <asp:Label ID="lbTipoCondicion2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoCondicion") %>' Visible="False" />
                                    <input id="hdTotal" type="hidden" name="hdTotal" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCondicionF" runat="server" Width="120px" />
                                    <asp:ImageButton ID="ibCondicionF" runat="server" CommandName="CondicionF" CssClass="hidden" />
                                    <input id="hdTotalF" type="hidden" name="hdTotalF" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Intermediario">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbIntermediario" runat="server" Width="170px" Text='<%# Eval("Intermediario") %>' />
                                    <input type="hidden" id="hdIntermediario" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoTercero") %>' name="hdIntermediario" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbIntermediarioF" runat="server" Width="180px" />
                                    <input type="hidden" id="hdIntermediarioF" runat="server" name="hdIntermediarioF" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contacto">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlContacto" runat="server" Width="120px" />
                                    <asp:Label ID="lbContacto" runat="server" Text='<%# Eval("CodigoContacto") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlContactoF" runat="server" Width="120px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medio Transmisión">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMedioTrans" runat="server" Width="130px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbMedioTrans" runat="server" Text='<%# Eval("MedioNegociacion") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlMedioTransF" runat="server" Width="130px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Fondo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoFondo" runat="server" Width="100px" Enabled="False" />
                                    <asp:ImageButton ID="ibTipoFondo" runat="server" CommandName="TipoFondo" CssClass="hidden" />
                                    <asp:Label ID="lbTipoFondo" runat="server" Text='<%# Eval("TipoFondo") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoFondoF" runat="server" Width="100px" />
                                    <asp:ImageButton ID="ibTipoFondoF" runat="server" CommandName="TipoFondoF" CssClass="hidden" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Tramo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoTramo" runat="server" Width="100px" Enabled="False" />
                                    <asp:Label ID="lbTipoTramo" runat="server" Text='<%# Eval("TipoTramo") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTipoTramoF" runat="server" Width="100px"  />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Plaza Neg.">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPlazaN" runat="server" Width="100px" Enabled="True" />
                                    <asp:Label ID="lbPlazaN" runat="server" CssClass="stlPaginaTexto" Text='<%# Eval("CodigoPlaza") %>' Visible="False" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlPlazaNF" runat="server" Width="100px" Enabled="True" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant. Instrumento Ejecuci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCantidadOperacion" runat="server" Text='<%# Eval("CantidadOperacion")%>' CssClass="Numbox-7" 
                                    Style="text-align: right" onblur='javascript:CalcularTotalEjecucion(this);' Width="100px" />
                                    <input type="hidden" id="hdCantidadOperacionTrz" runat="server" value='<%# Eval("CantidadOperacion") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbCantidadOperacionF" runat="server" CssClass="Numbox-7" onblur='javascript:CalcularTotalEjecucionF(this);' Width="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio Ejecuci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPrecioOperacion" runat="server" Text='<%# Eval("PrecioOperacion") %>' CssClass="Numbox-7" 
                                    onblur='javascript:CalcularTotalEjecucion(this);' Width="100px" />
                                    <input type="hidden" id="hdPrecioOperacionTrz" runat="server" value='<%# Eval("PrecioOperacion") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbPrecioOperacionF" runat="server" CssClass="Numbox-7" onblur='javascript:CalcularTotalEjecucionF(this);' Width="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Ejecuci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotalOperacion" runat="server" Text='<%# Eval("TotalOperacionRV") %>' ReadOnly="True" 
                                    CssClass="Numbox-7" Width="100px" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="tbTotalOperacionF" runat="server" CssClass="Numbox-7" ReadOnly="True" Width="100px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" % " Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPorcentaje" runat="server" />
                                    <input type="hidden" id="hdPorcentaje" runat="server" value='<%# Eval("Porcentaje") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkPorcentajeF" runat="server" Checked = "true"  />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hdNemonicoBusqueda" runat="server" />
                    <asp:HiddenField ID="hdIntermediarioBusqueda" runat="server" />
                    <asp:HiddenField ID="hdTipoRenta" runat="server" Value ="2" />
                    <asp:HiddenField ID="hdGrillaRegistros" runat="server"  />
                    <asp:HiddenField ID="hdFechaNegocio" runat="server" />
                    <asp:HiddenField ID="hdPuedeNegociar" runat="server" />
            <%--    </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibGrabar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ibValidar" EventName="Click"  />
                    <asp:AsyncPostBackTrigger ControlID="ibValidarTrader" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ibAprobar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
        <br />
        <div class="loading" align="center">
            <%--Loading. Please wait.<br /><br />--%>
            <img src="../../App_Themes/img/icons/loading.gif" />
        </div>
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
        </div>
    </div>
    </form>
</body>
</html>