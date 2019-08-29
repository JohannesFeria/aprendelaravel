<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLiquidaciones.aspx.vb" Inherits="Modulos_Tesoreria_Cuentasxpagar_frmLiquidaciones" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Liquidaci&oacute;n Cuentas por Pagar</title>
    <script type="text/javascript">
        function LiqAntFechaFondo() {
            if (document.getElementById("chkIndLiqAntFondo").checked == true) {
                document.getElementById("hdIndLiqAntFondo").value = "1";
            }
            else {
                document.getElementById("hdIndLiqAntFondo").value = "0";
            }
        }
        function AceptarVisible() {
            var valor;
            d = document.Form1;
            valor = document.getElementById("lblMoneda").innerText;

            var Fecha = d.txtFechaVcto.value;
            var fec_ini = Fecha.substring(6, 10);
            var mes_ini = Fecha.substring(3, 5);
            var dia_ini = Fecha.substring(0, 2);
            var FechaVcto = Fecha.substring(6, 10) + Fecha.substring(3, 5) + Fecha.substring(0, 2);
            var hidFecha = d.hidFechaVcto.value;

            if (document.getElementById("hdIndLiqAntFondo").value != '1') {
                if (valor != '') {
                    if (dia_ini > "31" || mes_ini > "12" || fec_ini > "2030") {
                        alertify.alert("Fecha no valida");
                        d.txtFechaVcto.value = hidFecha.substring(8, 6) + "/" + hidFecha.substring(6, 4) + "/" + hidFecha.substring(0, 4);
                        document.getElementById("tdaceptar").style.display = 'none';
                        return false;
                    }
                    else {
                        if (FechaVcto < hidFecha || d.txtFechaVcto.value.length < 10) {
                            alertify.alert("La fecha de Vencimiento debe ser mayor o igual a " + hidFecha.substring(8, 6) + "/" + hidFecha.substring(6, 4) + "/" + hidFecha.substring(0, 4));
                            document.getElementById("tdaceptar").style.display = 'none';
                            return false;
                        }
                        else {
                            document.getElementById("tdaceptar").style.display = '';
                            if (document.getElementById("btnLiquidar") != null) {
                                document.getElementById("btnLiquidar").style.visibility = 'hidden';
                            }
                        }
                    }
                }
            }
        }

        function ValidarDatos(mensaje) {
            if (!document.getElementById("ddlNroCuenta").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un número de cuenta.");
                return false;
            }
            if (document.getElementById("txtPago").value == '') {
                alertify.alert("Debe seleccionar una fecha de pago.");
                return false;
            }

            if (!document.getElementById("ddlTipoPago").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un tipo de pago.");
                return false;
            }
            return confirm(mensaje);
        }
        function ValidarCuenta() {
            if (!document.getElementById("ddlNroCuenta").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un número de cuenta.");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Operaciones Cuentas por Pagar - Liquidaci&oacute;n</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upcabecera" runat="server" UpdateMode ="Conditional" >
            <ContentTemplate>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Portafolio</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Intermediario</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlIntermediario" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Moneda</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMoneda" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Operaci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlOperacion" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Inicio Vencimiento</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Fin Vencimiento</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-1" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
          </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID ="btnSaldos" EventName ="click" />
                <asp:AsyncPostBackTrigger ControlID ="btnOperaciones" EventName ="click" />
                <asp:AsyncPostBackTrigger ControlID ="btnPagoParcial" EventName ="click" />
                <asp:AsyncPostBackTrigger ControlID ="btnAceptarFecha" EventName ="click" />
                <asp:AsyncPostBackTrigger ControlID ="btnLiquidar" EventName ="click" />
                <asp:AsyncPostBackTrigger ControlID ="btnSalir" EventName ="click" />
            </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <asp:UpdatePanel ID="upcuuerpo" runat="server" UpdateMode ="Conditional" >
        <ContentTemplate>
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
            <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:ButtonField Text="Select" CommandName="Select" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="NroOperacion" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField>
                        <ItemTemplate><asp:CheckBox ID="chbConfirmar" GroupName="Confirmar" runat="server" AutoPostBack="True" OnCheckedChanged="chbConfirmar_CheckedChanged" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FechaNegociacion" HeaderText="Fecha Operación" />
                    <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vcto" />
                    <asp:BoundField DataField="NroOperacion" HeaderText="Nro. Operación" HtmlEncode="false" HtmlEncodeFormatString="true" />
                    <asp:BoundField DataField="Referencia" HeaderText="Descripción" />
                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}"  />
                    <asp:BoundField DataField="DescripcionMercado" HeaderText="Mercado" />
                    <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="DescripcionMoneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Intermediario" />
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operación" />
                    <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo Movimiento" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                    <asp:BoundField DataField="CodigoPortafolioSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoMoneda" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Categoria" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoOrden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoMnemonico" HeaderText="Mnemonico" />
                    <asp:BoundField DataField="CodigoOperacion" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoRenta" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoMercado" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoTercero" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoEntidad" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                     <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se agrega nuevo campo para vlidar suscripción de fondos Capestr | 01/08/18 --%>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoTipoInstrumentoSBS" />
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se agrega nuevo campo para vlidar suscripción de fondos Capestr | 01/08/18 --%>
                     <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - OT12012 - Se agrego el campo clase instrumento para obtener el modelo de carta | 10/06/19 --%>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoClaseInstrumento" />
                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - OT12012 - Se agrego el campo clase instrumento para obtener el modelo de carta | 10/06/19 --%>
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Cuenta Egreso</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Banco</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlBanco" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tipo de Pago</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoPago" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Pago</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtPago" ReadOnly="true" Width="150px" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Moneda</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="lblMoneda" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Clase de Cuenta</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlClase" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Nro. Cuenta</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlNroCuenta" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtDescripcion" ReadOnly ="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Importe</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtImporte" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label" >Fecha Vencimiento</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaVcto" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Contacto Intermediario</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlContactoIntermediario" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label runat="server" id="lblContacto" class="col-sm-5 control-label">Contacto</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlContacto" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Modelo Carta</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlModeloCarta" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnBancoRenovacion" runat="server" >
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 control-label">Banco Glosa</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlBancoRenovacion" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row" style="display:none;">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Banco Matriz Origen</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlBancoMatrizOrigen" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Observaciones</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtObservacionCarta" runat="server" Width="500px" MaxLength ="300" TextMode="MultiLine" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
            <br />
            <fieldset id="pnlDestinoDivisas" runat="server">
            <legend>Cuenta Ingreso</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Banco</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlBancoDestino" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Clase de Cuenta</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlClaseCuentaDestino" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Nro. Cuenta</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlCuentaDestino" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Moneda</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="lblMonedaDestino" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Importe</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtImporteDestino" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" style="display:none;">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Banco Matriz Destino</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlBancoMatrizDestino" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Banco Glosa</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlBancoGlosaIngreso" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                        <label class="col-sm-5 control-label">Observaciones</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtObservacionDestino" runat="server" Width="650px" MaxLength ="1000" TextMode="MultiLine" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
            <br />
            <asp:Panel ID="pnRenovacion" Visible="false" runat="server">
                <fieldset>
                    <legend>Datos de la Renovación</legend>
                    <div class="grilla">
                        <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="gvRenovacion">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                        <asp:Label ID="lbCodigo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOrden") %>'></asp:Label>
                                        <asp:Label ID="lbCodigoConstitucion" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodDatatec") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CodigoOrden" HeaderText="Orden" />
                                <asp:BoundField DataField="Nemonico" HeaderText="Nemonico" />
                                <asp:BoundField DataField="CodigoTipoCupon" HeaderText="Tipo Cupon" />
                                <asp:BoundField DataField="TasaPorcentaje" HeaderText="Tasa %" />
                                <asp:BoundField DataField="MontoNominalOperacion" HeaderText="Nominal" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="MontoOperacion" HeaderText="Monto" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnBCR" Visible="false" runat="server">
                    <fieldset>
                        <legend>Datos de la transferencia BCR</legend>
                        <div class="grilla">
                            <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="gvBCR">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                            <asp:Label ID="lbCodigo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.CodigoOperacionCaja") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CodigoOperacionCaja" HeaderText="Orden" />
                                    <asp:BoundField DataField="TerceroOrigen" HeaderText="Origen" />
                                    <asp:BoundField DataField="TerceroDestino" HeaderText="Destino" />
                                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </asp:Panel>
            <input runat="server" id="hdCodigoCuenta" name="hdCodigoCuenta" type="hidden" />
            <input runat="server" id="hidFechaVcto" name="hidFechaVcto" type="hidden" />
            <input runat="server" id="hdIndLiqAntFondo" type="hidden" name="hdIndLiqAntFondo" />
            <input runat="server" id="hdCodigoOrden" name="hdCodigoOrden" type="hidden" />
            <input runat="server" id="hdIntermediario" name=" hdOperacion" type="hidden" />
            <input runat="server" id="hdMercado" name="hdOperacion" type="hidden" />
            <input runat="server" id="hdCantIntContacto" name="hdOperacion" type="hidden" />
            <input runat="server" id="hdnSinCuenta" name="hdOperacion" type="hidden" />
            <input runat="server" id="hdSAB" name="hdOperacion" type="hidden" />
            <input runat="server" id="hdOperacion" name="hdOperacion" type="hidden" />
            <input runat="server" id="hdValSaldo" name="hdValSaldo" type="hidden" />
            <input runat="server" id="hidCodigoMonedaOrigen" name="hidCodigoMonedaOrigen" type="hidden" />
            <input runat="server" id="hidCodigoMonedaDestino" name="hidCodigoMonedaDestino" type="hidden" />
            <input runat="server" id="hdCodigoTrecero" name="hdCodigoTrecero" type="hidden" />
        <header></header>
        <div class="row">
            <div class="col-md-6">
                <asp:Button ID="btnSaldos" Text="Saldos" runat="server" />
                <asp:Button ID="btnOperaciones" Text="Datos de la Operacion" runat="server" />
                <asp:Button ID="btnPagoParcial" Text="Pago Parcial" runat="server" Visible="false" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button ID="btnAceptarFecha" Text="Aceptar" runat="server" />
                <asp:Button ID="btnLiquidar" Text="Liquidar" runat="server" />
                <asp:Button ID="btnImprimir" Text="Imprimir" runat="server" />
                <asp:Button ID="btnSalir" Text="Salir" runat="server" CausesValidation="false" />
            </div>
        </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID ="btnBuscar" EventName ="click" />
            <asp:PostBackTrigger ControlID ="btnImprimir" />
            <asp:AsyncPostBackTrigger ControlID="ddlPortafolio"  EventName="SelectedIndexChanged" />
        </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>