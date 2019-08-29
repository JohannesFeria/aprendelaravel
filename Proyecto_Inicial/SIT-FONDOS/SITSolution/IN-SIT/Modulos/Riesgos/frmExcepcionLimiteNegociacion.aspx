<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExcepcionLimiteNegociacion.aspx.vb" Inherits="Modulos_Riesgos_frmExcepcionLimiteNegociacion" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Excepciones Límites de Negociación</title>
    <script type="text/javascript">

        function IngresoNumeroDecimales() {
            if (window.event.keyCode != 45 &&
		        window.event.keyCode != 46 &&
		        (window.event.keyCode < 48 || window.event.keyCode > 57)) {
                window.event.keyCode = 0;
            }
        }

        function HabilitaCampoCantidad(myElem) {
            var elem = myElem.id.split('_');
            var elem2 = elem;
            elem = elem[0] + '__' + elem[2] + '_' + 'ddlExcepcionN';
            elem2 = elem2[0] + '__' + elem2[2] + '_' + 'tbCantidadN';
            if (document.getElementById(elem).value == "") {
                document.getElementById(elem2).disabled = 'disabled';
            }
            else {
                document.getElementById(elem2).disabled = '';
            }
        }

        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf('GridView1') > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf('GridView1') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
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





        function jsDecimals(e) {

            var evt = (e) ? e : window.event;
            var key = (evt.keyCode) ? evt.keyCode : evt.which;
            if (key != null) {
                key = parseInt(key, 10);
                if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
                    //Aca tenemos que reemplazar "Decimals" por "NoDecimals" si queremos que no se permitan decimales
                    if (!jsIsUserFriendlyChar(key, "Decimals")) {
                        return false;
                    }
                }
                else {
                    if (evt.shiftKey) {
                        return false;
                    }
                }
            }
            return true;
        }

        // Función para las teclas especiales
        //------------------------------------------
        function jsIsUserFriendlyChar(val, step) {
            // Backspace, Tab, Enter, Insert, y Delete
            if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
                return true;
            }
            // Ctrl, Alt, CapsLock, Home, End, y flechas
            if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
                return true;
            }
            if (step == "Decimals") {
                if (val == 190 || val == 110) {  //Check dot key code should be allowed
                    return true;
                }
            }
            // The rest
            return false;
        }





    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
    <header><h2>Excepciones Límites de Negociación</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de Renta</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoRenta" runat="server" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" ></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha de Inicio</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha de Fin</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de Operación</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoOperacion" Runat="server" Width="55%" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de Instrumento</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoInstrumento" Width="85%" Runat="server"></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Exclusión</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlExclusion" Runat="server"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9"></div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9"></div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
           <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    
        <div class="grilla">
            <asp:UpdatePanel ID="Grilla" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <HeaderTemplate>
                                    <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                                    <%--<asp:CheckBox ID="chk_Item" runat="server"></asp:CheckBox>--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>&nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoOrden" HeaderText="Código Orden"></asp:BoundField>
                            <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha"></asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                            <asp:BoundField DataField="CodigoMnemonico" HeaderText="Mnem&#243;nico"></asp:BoundField>
                            <asp:BoundField DataField="CodigoISIN" HeaderText="C&#243;digo ISIN"></asp:BoundField>
                            <asp:BoundField DataField="Precio" HeaderText="Precio"></asp:BoundField>
                            <asp:BoundField DataField="CantidadOperacion" HeaderText="Cantidad"></asp:BoundField>
                            <asp:BoundField DataField="MontoNetoOperacion" HeaderText="Monto"></asp:BoundField>
                            <asp:TemplateField HeaderText="Excepci&#243;n">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlExcepcionN" runat="server" Width="150px" 
                                        CommandName="Excepcion" AutoPostBack="true" onselectedindexchanged="ddlExcepcionN_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbExcepcionN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoExcepcion") %>'
                                        Visible="False">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad<br>Excepci&#243;n">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCantidadN" runat="server" onkeydown="return jsDecimals(event);" Text='<%# DataBinder.Eval(Container, "DataItem.CantidadExcepcion") %>' >
                                    </asp:TextBox>
                                    <%--CssClass="Numbox-0"--%>
                                     <%--onblur='formatCurrencyPrecio(this);'--%>
                                    <%--onkeypress="javascript:IngresoNumeroDecimales();"--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
