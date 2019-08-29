<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLiquidaciones.aspx.vb"
    Inherits="Modulos_Tesoreria_Cuentasxcobrar_frmLiquidaciones" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Operaciones Cuentas por Cobrar - Liquidación</title>
    <script type="text/javascript">
        function AceptarVisible() {
            var valor;
            d = document.Form1;
            valor = document.getElementById("txtImporte").value;
            var Fecha = d.txtFechaVcto.value;
            var fec_ini = Fecha.substring(6, 10);
            var mes_ini = Fecha.substring(3, 5);
            var dia_ini = Fecha.substring(0, 2);
            var FechaVcto = Fecha.substring(6, 10) + Fecha.substring(3, 5) + Fecha.substring(0, 2);
            var hidFecha = d.hidFechaVcto.value;
            if (document.getElementById("hdIndLiqAntFondo").value != '1') {
                if (valor != '') {
                    if (dia_ini > "31" || mes_ini > "12" || fec_ini > "2020") {
                        alert("Fecha no valida");
                        d.txtFechaVcto.value = hidFecha.substring(8, 6) + "/" + hidFecha.substring(6, 4) + "/" + hidFecha.substring(0, 4);
                        document.getElementById("tdaceptar").style.display = 'none';
                        return false;
                    }
                    else {
                        if (FechaVcto < hidFecha || d.txtFechaVcto.value.length < 10) {
                            alert("La fecha de Vencimiento debe ser mayor o igual a " + hidFecha.substring(8, 6) + "/" + hidFecha.substring(6, 4) + "/" + hidFecha.substring(0, 4));
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
            var valor;
            valor = document.getElementById("hdOperacion").value;

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

            if (!document.getElementById("ddlClase").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una Clase de Cuenta.");
                return false;
            }

            if (document.getElementById("hdMercado").value == 1 && document.getElementById("hdOperacion").value != 35 && document.getElementById("hdOperacion").value != 38 && document.getElementById("hdOperacion").value != 39 && document.getElementById("hdOperacion").value != 67) {
                var BANCOLIQUIDAR = document.getElementById("ddlBanco").options[document.getElementById("ddlBanco").selectedIndex].text
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

//        function BusquedaOrdenInversionRelacion() {
//            var clase = $("#lkbRelacion").attr('class').toString();
//            if (clase != "aspNetDisabled")
//                window.open('../../Inversiones/InstrumentosNegociados/frmDatosCarta.aspx', "_blank", "toolbar=no,scrollbars=no,resizable=no,location=no,titlebar=no,top=500,left=500,width=600,height=300");
//            else
//                return;
//        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>Operaciones Cuentas por Cobrar - Liquidación</h2>
        </header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upcabecera" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Portafolio</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Intermediario</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlIntermediario" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Moneda</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlMoneda" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Operación</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlOperacion" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Inicio Vencimiento</label>
                                <div class="col-sm-7">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" />
                                        <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Fin Vencimiento</label>
                                <div class="col-sm-7">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="txtFechaFin" SkinID="Date" />
                                        <span class="add-on" id="imgFechaFin"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSaldos" EventName="click" />
                    <asp:AsyncPostBackTrigger ControlID="btnOperaciones" EventName="click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarFecha" EventName="click" />
                    <asp:AsyncPostBackTrigger ControlID="btnLiquidar" EventName="click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSalir" EventName="click" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <asp:UpdatePanel ID="upcuerpo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend>Resultados de la Búsqueda</legend>
                    <asp:Label runat="server" ID="lbContador" />
                    <div class="grilla">
                        <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                            <Columns>
                                <asp:ButtonField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" Text="Select"
                                    CommandName="Select" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="NroOperacion" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chbConfirmar" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FechaNegociacion" HeaderText="Fecha Operación" />
                                <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vcto" />
                                <asp:BoundField DataField="NroOperacion" HeaderText="Nro. Operación">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Referencia" HeaderText="Descripción">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DescripcionMercado" HeaderText="Mercado">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" ItemStyle-CssClass="hidden"
                                    HeaderStyle-CssClass="hidden">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DescripcionMoneda" HeaderText="Moneda">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Intermediario">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operación">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo Movimiento" ItemStyle-CssClass="hidden"
                                    HeaderStyle-CssClass="hidden">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMoneda" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Categoria" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoOrden" />
                                <asp:BoundField DataField="CodigoMnemonico" HeaderText="Mnemonico" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoOperacion" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoRenta" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMercado" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoTercero" />
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoEntidad" />
                                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se agrega nuevo campo para vlidar suscripción de fondos Capestr | 01/08/18 --%>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoTipoInstrumentoSBS" />
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se agrega nuevo campo para vlidar suscripción de fondos Capestr | 01/08/18 --%>
                                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - OT12012 Se agrega nuevo campo para obtener el modelo de carta | 10/06/19 --%>
                                <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoClaseInstrumento" />
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - OT12012 Se agrega nuevo campo para obtener el modelo de carta | 10/06/19 --%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Cuenta Ingreso</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Banco</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBanco" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Tipo de Pago</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlTipoPago" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Pago</label>
                                <div class="col-sm-7">
                                    <div class="input-append ">
                                        <asp:TextBox runat="server" ID="txtPago" ReadOnly="true" Width="150px" />
                                    </div>
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
                                    <asp:TextBox ID="lblMoneda" runat="server" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Clase de Cuenta</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlClase" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Nro. Cuenta</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlNroCuenta" runat="server" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Descripción</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDescripcion" runat="server" ReadOnly="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Importe</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtImporte" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Fecha Vencimiento</label>
                                <div class="col-sm-7">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="txtFechaVcto" SkinID="Date" />
                                        <span class="add-on" id="imgFechaVcto"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Contacto Intermediario</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlContactoIntermediario" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    <asp:Label ID="lblContacto" runat="server">Contacto</asp:Label>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlContacto" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Modelo Carta</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlModeloCarta" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnBancoRenovacion" runat="server">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">
                                        Banco Glosa</label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlBancoRenovacion" runat="server" AutoPostBack="True" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="row" style="display: none;">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Banco Matriz Origen</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBancoMatrizOrigen" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Observaciones</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtObservacionCarta" runat="server" Width="650px" MaxLength="1000"
                                        TextMode="MultiLine" />
                                </div>
                            </div>
                        </div>
                        <%--<div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Registro Relacion</label>
                                <div class="col-sm-7">
                                    <asp:LinkButton ID="lkbRelacion" runat="server" OnClientClick="javascript:BusquedaOrdenInversionRelacion();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>--%>
                    </div>
                </fieldset>
                <br />
                <fieldset id="pnlDestinoDivisas" runat="server">
                    <legend>Cuenta Egreso</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Banco</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBancoDestino" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Clase de Cuenta</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlClaseCuentaDestino" runat="server" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Nro. Cuenta</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlCuentaDestino" runat="server" Width="150px" />
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
                                    <asp:TextBox ID="lblMonedaDestino" runat="server" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Importe</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtImporteDestino" runat="server" CssClass="Numbox-7" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    <asp:Label ID="lblContactoDivisa" runat="server">Contacto:</asp:Label>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlContactoDivisa" runat="server" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="display: none;">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    <asp:Label ID="lblBancoGlosa" runat="server">Banco Matriz Intermediario:</asp:Label>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBancoMatrizDestino" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    <asp:Label ID="Label1" runat="server">Banco Glosa</asp:Label>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBancoGlosaEgreso" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    Observaciones</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtObservacionDestino" runat="server" Width="650px" MaxLength="1000"
                                        TextMode="MultiLine" />
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
                <asp:HiddenField ID="hidCodigoMonedaOrigen" runat="server" />
                <asp:HiddenField ID="hdOperacion" runat="server" />
                <asp:HiddenField ID="hidCodigoMonedaDestino" runat="server" />
                <asp:HiddenField ID="hdIndLiqAntFondo" runat="server" />
                <asp:HiddenField ID="hdIntermediario" runat="server" />
                <asp:HiddenField ID="hdMercado" runat="server" />
                <asp:HiddenField ID="hdSAB" runat="server" />
                <asp:HiddenField ID="hdnSinCuenta" runat="server" />
                <asp:HiddenField ID="hdCantIntContacto" runat="server" />
                <asp:HiddenField ID="hidFechaVcto" runat="server" />
                <input runat="server" id="hdCodigoTrecero" name="hdCodigoTrecero" type="hidden" />
                <div class="row">
                    <div class="col-md-3">
                        <asp:Button ID="btnSaldos" runat="server" Text="Saldos" />
                        <asp:Button ID="btnOperaciones" runat="server" Text="Datos de la Operacion" />
                    </div>
                    <div class="col-md-9" style="text-align: right;">
                        <asp:Button ID="btnAceptarFecha" runat="server" Text="Aceptar" />
                        <asp:Button ID="btnLiquidar" runat="server" Text="Liquidar" />
                        <asp:Button ID="ibImprimir" runat="server" Text="Imprimir" />
                        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="click" />
                <asp:PostBackTrigger ControlID="ibImprimir" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <br />
    </form>
</body>
</html>
