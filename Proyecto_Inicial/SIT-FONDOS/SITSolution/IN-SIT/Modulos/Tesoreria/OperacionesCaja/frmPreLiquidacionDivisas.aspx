<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPreLiquidacionDivisas.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmPreLiquidacionDivisas" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>PreLiquidacionDivisas</title>
    <script type="text/javascript">
        function AceptarVisible() {
            var valor;
            valor = document.getElementById("txtImporte").value;

            if (valor != '') {
                document.getElementById("tdaceptar").style.display = '';


                if (document.getElementById("btnLiquidar") != null) {
                    document.getElementById("btnLiquidar").style.visibility = 'hidden';
                }
            }
        }
        function ValidarBuscar() {
            if (!document.getElementById("ddlMoneda").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una moneda.");
                return false;
            }
            return true;
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


                if (document.getElementById("hdnSinCuenta").value == "YES") {

                }

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

        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }

        $(document).ready(function () {
            $("#ibImprimir").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

    <div class="container-fluid">
        <header>
            <h2>
                Pre-Liquidación de Divisas - No PH
            </h2>
        </header>
        <br />
        <asp:UpdatePanel ID="up1" runat="server" >
        <ContentTemplate>
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Mercado</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlMercado" runat="server" Width="200px" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Portafolio</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlPortafolio" runat="server" Width="200px" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Intermediario</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlIntermediario" runat="server" Width="200px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Moneda</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlMoneda" runat="server" Width="200px" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Operación</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlOperacion" runat="server" Width="200px" AutoPostBack="True">
                            <asp:ListItem>--SELECCIONE--</asp:ListItem>
                            <asp:ListItem Value="65">COMPRA DIVISAS</asp:ListItem>
                            <asp:ListItem Value="66">VENTA DIVISAS</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Inicio Operación</label>
                    <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" Enabled="false" />
                    </div>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Fin Operación</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="txtFechaFin" SkinID="Date" />
                            <span class="add-on" id="imgFechaFin"><i class="awe-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-2" style="text-align: right;">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
            </div>
        </div>
                <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:ButtonField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" Text="Select" CommandName="Select"></asp:ButtonField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="NroOperacion"></asp:BoundField>
                    <asp:TemplateField HeaderText="Seleccionar"><ItemTemplate><asp:ImageButton ID="ibSeleccionarDivisas" runat="server" SkinID="imgCheck" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton></ItemTemplate></asp:TemplateField>
                    <asp:BoundField DataField="FechaNegociacion" HeaderText="Fecha Operación"></asp:BoundField>
                    <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vcto"></asp:BoundField>
                    <asp:BoundField DataField="NroOperacion" HeaderText="Nro. Operación"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="Referencia" HeaderText="Descripción"><%--<ItemStyle HorizontalAlign="Left"></ItemStyle>--%></asp:BoundField>
                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}"><ItemStyle HorizontalAlign="Right"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="DescripcionMercado" HeaderText="Mercado"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Portafolio"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="PortafolioSBS" HeaderText="Portafolio"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="DescripcionMoneda" HeaderText="Moneda"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Intermediario"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operación"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo Movimiento"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="CodigoMnemonico" HeaderText="Mnemonico"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMoneda"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Categoria"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoOrden"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoOperacion"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoRenta"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMercado"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoTercero"></asp:BoundField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoEntidad"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>

        

        <br />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Banco</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlBanco" runat="server" Width="280px" AutoPostBack="True" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Tipo de Pago</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlTipoPago" runat="server" Width="280px" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Pago</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="txtPago" SkinID="Date" />
                            <span class="add-on" id="imgPago"><i class="awe-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Moneda</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lblMoneda" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Clase de Cuenta</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlClase" runat="server" Width="150px" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Nro. Cuenta</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlNroCuenta" runat="server" Width="150px">
                        </asp:DropDownList>
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
                        <asp:TextBox ID="txtDescripcion" runat="server" Width="140px" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Importe</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtImporte" runat="server" CssClass="Numbox-7" Width="120px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Vencimiento</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="txtFechaVcto" SkinID="Date" />
                            <span class="add-on" id="imgFechaVcto"><i class="awe-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <label class="col-sm-2 control-label">
                        Modelo Carta</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlModeloCarta" runat="server" Width="320px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblContacto" runat="server">Contacto:</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlContacto" runat="server" Width="180px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Contacto Intermediario</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlContactoIntermediario" runat="server" Width="180px" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <fieldset id="pnlDestinoDivisas" runat="server">
            <legend>Cuenta Egreso</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Banco</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlBancoDestino" runat="server" AutoPostBack="True" Width="150px" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlClaseCuentaDestino" runat="server" AutoPostBack="True" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlCuentaDestino" runat="server" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblMonedaDestino" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Importe</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtImporteDestino" runat="server" CssClass="Numbox-7" Width="120px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <asp:Label ID="lblContactoEgreso" runat="server">Contacto:</asp:Label>
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlContactoDivisa" runat="server" Width="180px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
            <asp:HiddenField ID="hdCodigoOrden" runat="server" />
            <asp:HiddenField ID="hidCodigoMonedaOrigen" runat="server" />
            <asp:HiddenField ID="hidCodigoMonedaDestino" runat="server" />
            <asp:HiddenField ID="hdLiqBanco" runat="server" />
            <asp:HiddenField ID="hdLiqOperacion" runat="server" />
            <asp:HiddenField ID="hdLiqCategoria" runat="server" />
            <asp:HiddenField ID="hiddenCodigoOrden" runat="server" />
       </ContentTemplate>
        </asp:UpdatePanel>
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <%--Agregado por Carlos Hernández Ledesma--%>
            <%--se agreaga el UpdatePanel1 para que solo actualize el div de los botones y se muestre el button aceptar al cambiar la propiedad visible--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                    <asp:Button ID="ibImprimir" runat="server" Text="Imprimir" />
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
    </div>

    <div id="divProgress" class="loading">
        Procesando...<br />
        <br />
        <img src="../../../App_Themes/img/icons/ajax-loader.gif" alt="" />
    </div>
 
    </form>
</body>
</html>
