<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaMovimientosBancarios.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmConsultaMovimientosBancarios" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>ConsultaMovimientosBancarios</title>
    <script type="text/javascript">
        function PopUpReportes(strPaginaBuscar) {
            window.open(strPaginaBuscar, 'blank', 'width=800, height=600, top=50, left=50, menubar=no, resizable=yes');
        }
        function ValidarOperaciones() {
            if (document.getElementById("hdCodigoOperacionCaja").value == '') {
                alertify.alert("Debe seleccionar una operación.");
                return false;
            }
            return true;
        }
        function Retornar() {
            history.back();
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="container-fluid">
        <header>
            <h2>
                Movimiento Caja
            </h2>
        </header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMercado" runat="server" Width="170px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="170px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Banco</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlBanco" runat="server" Width="280px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMoneda" runat="server" Width="170px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlClaseCuenta" runat="server" Width="170px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlNroCuenta" runat="server" Width="170px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlOperacion" runat="server" Width="170px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlTipoOperacion" runat="server" Width="170px" AutoPostBack ="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on" id="imgFechaFin"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                <asp:Button ID="btnSaldos" runat="server" Text="Saldos" />
            </div>
        </fieldset>
        <div class="row" id="divExtornoOperacionesCaja" runat="server">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label" style="text-align: left;">
                            Ingrese los siguientes campos:</label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Motivo</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlMotivo" Width="250px" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Observación</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbComentario" Width="300px" runat="server" TextMode="MultiLine"
                                Height="50px" style="text-transform:uppercase;"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Liquida antes Apertura</label>
                        <div class="col-sm-9 control-label" style="text-align: left;">
                            <asp:CheckBox ID="chkLiqAntFon" runat="server"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnAceptarExt" runat="server" Text="Aceptar" />
                <asp:Button ID="btnCancelarExt" runat="server" Text="Cancelar" />
            </div>
        </div>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <asp:Label ID="lbContador" runat="server"></asp:Label>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="upGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibSeleccionarPA" runat="server" SkinID="imgCheck" CommandName="Seleccionar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoOperacionCaja") %>' />
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoOperacionCaja"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="FechaLiquidado"></asp:BoundField>
                            <asp:BoundField DataField="FechaLiquidado" HeaderText="Fec. Liquidado"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="DescripcionMercado" HeaderText="Mercado"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" ></asp:BoundField>
                            <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="IntermediarioDescripcion" HeaderText="Intermediario"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" >
                                <ItemStyle HorizontalAlign="Right" Width="40px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operación"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="Referencia" HeaderText="Descripción"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operación"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="NumeroOperacion" HeaderText="Nro. Operación"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="NumeroCuenta" HeaderText="Nro.Cuenta"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo Movimiento"><HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS"></asp:BoundField>
                            <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCodigoOperacionCaja" runat="server" Value='<%# Eval("CodigoOperacionCaja")%>' />
                                    <asp:HiddenField ID="hdCodigoPortafolioSBS" runat="server" Value='<%# Eval("CodigoPortafolioSBS")%>' />
                                    <asp:HiddenField ID="hdFechaLiquidacion" runat="server" Value='<%# Eval("FechaLiquidado")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnExtornar" runat="server" Text="Extornar" Visible="false" />
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
        <br />
    </div>
    <asp:HiddenField ID="hdCodigoOperacionCaja" runat="server" />
    <asp:HiddenField ID="hdCodigoPortafolioSBS" runat="server" />
    <asp:HiddenField ID="hfFilaSeleccionada" runat="server" Value="-1" />
    <asp:HiddenField ID="hdFechaNegocio" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
