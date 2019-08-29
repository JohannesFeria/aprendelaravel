<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresosFlujoCaja.aspx.vb"
    Inherits="Modulos_Tesoreria_FlujoCajaEstimado_frmIngresosFlujoCaja" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Ingreso de Flujo</title>
    <script language="javascript">
        function ValidarDatos() {
            if (document.getElementById("txtDescripcion").value == "") {
                alertify.alert("Debe Ingresar una Descripción.");
                return false;
            }
            if (document.getElementById("txtImporte").value == "") {
                alertify.alert("Debe Ingresar un Importe.");
                return false;
            }
            if (document.getElementById("txtFechaDesc").value == "") {
                alertify.alert("Debe Ingresar una fecha de vencimiento.");
                return false;
            }
            if (!document.getElementById("ddlMonedai").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una moneda.");
                return false;
            }

            if (document.getElementById("hdCantidadOperaciones").value >= 2) {
                if (!document.getElementById("ddlOperacioni").selectedIndex > 0) {
                    alertify.alert("Debe seleccionar una operación.");
                    return false;
                }
            }
            return true;
        }

        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
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
						(document.forms[0].elements[i].name.indexOf('dgLista') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
        function Confirmar() {
            var strMensajeConfirmacion = "";
            strMensajeConfirmacion = "¿Desea eliminar la Operación estimada?";

            return confirm(strMensajeConfirmacion);
        }
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
                        Ingreso de Flujo</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoOperacion" Width="150px" AutoPostBack = "true"  />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Cuenta
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseCuenta" Width="150px" AutoPostBack = "true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Banco</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            N&uacute;mero de Cuenta
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlNroCuenta" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Fin
                        </label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="80px"></HeaderStyle>
                        <HeaderTemplate>
                            <input onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSeleccion" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoMercado" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoMoneda" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="FechaFlujo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="NumeroSecuencial" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Portafolio" HeaderText="Portafolio" />
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Descripci&#243;n" />
                    <asp:BoundField DataField="Mercado" HeaderText="Mercado" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="Operacion" HeaderText="Operaci&#243;n" />
                    <asp:BoundField DataField="TEgreso" HeaderText="Tipo" />
                    <asp:BoundField DataField="FechaInstruccion" HeaderText="Fecha Instrucción" />
                    <asp:BoundField DataField="FechaDescargo" HeaderText="Fecha Vencimiento" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercadoi" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolioi" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoOperacioni" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMonedai" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacioni" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Vencimiento
                        </label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaDesc" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripción</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtDescripcion" Width="150px" style="text-transform: uppercase;" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Importe
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtImporte" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Instrucción</label>
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="txtFechaInst" SkinID="Date" />
                            <span class="add-on"><i class="awe-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdCantidadOperaciones" />
    </form>
</body>
</html>
