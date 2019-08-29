<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInventarioForward.aspx.vb" Inherits="Modulos_Inversiones_frmInventarioForward" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>VencimientoForwardNoDelivery</title>
    <script type="text/javascript">
        function Confirmar() {
            if (confirm("¿Desea Generar el Vencimiento seleccionado?")) return true;
            else return false;
        }
        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }
        $(document).ready(function () {
            $("#ibBuscar").click(function () {
                ShowProgress();
            });
            $("#ibExportaExcel").click(function () {
                ShowProgress();
            });
            $("#ibProcesar").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<div class="container-fluid">

        <header>
            <h2>
                Inventario Forward</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fondo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFondo" runat="server" AutoPostBack="True" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Estado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlEstado" runat="server" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMercado" runat="server" CssClass="stlCajaTexto" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda Negociada</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMonedaNegociada" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda destino</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMonedaDestino" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Vencimiento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFechas" runat="server" Width="120px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <div class="col-sm-12 control-label" style="text-align: left;" >
                            &nbsp;Desde&nbsp;
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVencimientoDesde" SkinID="Date" />
                                <span class="add-on" id="imgFechaDesde"><i class="awe-calendar"></i></span>
                            </div>
                            &nbsp;Hasta&nbsp;
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVencimientoHasta" SkinID="Date" />
                                <span class="add-on" id="imgFechaHasta"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3" style="text-align: right;">
                    <asp:Button ID="Button2" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row" style="text-align: center;">
            Vencimientos
            <asp:Label ID="lblCantidad" runat="server">(0)</asp:Label>
        </div>
        <div id="divProgress" class="loading" style="text-align: center;">
            Procesando...<br />
            <br />
            <img src="../../App_Themes/img/icons/ajax-loader.gif" alt="" />
        </div>
        <div class="grilla">
            <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="Sel.">
                        <HeaderStyle Width="40px"></HeaderStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgSeleccionar" runat="server" SkinID="imgCheck" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>">
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ref" HeaderText="REF"></asp:BoundField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden" ></asp:BoundField>
                    <asp:BoundField DataField="Portafolio" HeaderText="Fondo"><ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operación">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaContrato" HeaderText="Fecha Vencimiento">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Liquidación">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Operacion" HeaderText="Operación">
                        <ItemStyle HorizontalAlign="Left" Width="60px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                        <ItemStyle HorizontalAlign="Left" Width="140px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda">
                        <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoMonedaDestino" HeaderText="Moneda Destino">
                        <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MontoCancelar" HeaderText="Monto pactado" DataFormatString="{0:#,##0.00}"
                        HtmlEncodeFormatString="false">
                        <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoCambioFuturo" HeaderText="TC(pactado)" DataFormatString="{0:#,##0.0000000}"
                        HtmlEncodeFormatString="false">
                        <ItemStyle HorizontalAlign="Right" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Intermediario" HeaderText="Intermediario">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NumeroContrato" HeaderText="Num Contrato"></asp:BoundField>
                    <asp:BoundField DataField="Mtm" HeaderText="MTM (USD)" HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden"></asp:BoundField>
                    <asp:BoundField DataField="MtmDestino" HeaderText="MTM (PEN)" HeaderStyle-CssClass ="hidden" ItemStyle-CssClass ="hidden"></asp:BoundField>
                    <asp:BoundField DataField="PrecioVector" HeaderText="P.Vector">
                        <ItemStyle HorizontalAlign="Right" Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoOrden" HeaderText="Cod Orden">
                        <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Número Contrato</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtNumeroContrato" runat="server" Width="123"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblRef" runat="server">REF</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtRef" runat="server" Width="123" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblMtmNac" runat="server">Mtm Destino</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtMtmDestino" runat="server" Width="123" CssClass="Numbox-2"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblMtmUSD" runat="server">Mtm</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtMtm" runat="server" Width="123" CssClass="Numbox-2"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        <asp:Label ID="lblPVector" runat="server">P.Vector:</asp:Label></label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtPrecioVector" runat="server" Width="123" CssClass="Numbox-7"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>        
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row" style="text-align: right;">
            <asp:Button ID="ibProcesar" runat="server" Text="Procesar" />
            <asp:Button ID="Button1" runat="server" Text="Exportar" />
            <asp:Button ID="ibImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="ibSalir" runat="server" Text="Salir" />
        </div>
    </form>
</body>
</html>
<%--<Triggers>
        <asp:AsyncPostBackTrigger ControlID="ibProcesar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ibExportaExcel" EventName="Click" />
    </Triggers>--%>