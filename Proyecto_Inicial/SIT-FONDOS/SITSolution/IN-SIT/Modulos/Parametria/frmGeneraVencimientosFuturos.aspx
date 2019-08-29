<%@ Page Language = "VB" AutoEventWireup = "false" CodeFile = "frmGeneraVencimientosFuturos.aspx.vb" Inherits = "Modulos_Parametria_frmGeneraVencimientosFuturos" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server"> <title> Genera Vencimientos Futuros </title> </head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Generar Vencimientos</h2></header>
        <fieldset>
            <legend>Busqueda</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fondo</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlFondo" runat="server" Width="120px" AutoPostBack ="true"  />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Vencimiento (Busqueda)</label>
                        <div class="col-sm-4">
                            <div class="input-append date" style="text-align: left;">
                                <asp:TextBox runat="server" ID="tbFechaVencimiento" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        </br>
        <fieldset>
            <legend>Vencimientos</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Vencimiento(Nueva)</label>
                        <div class="col-sm-4">
                            <div class="input-append date" style="text-align: left;">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Calculo del Vencimiento</label>
                        <div class="col-sm-4">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlcalculovencimiento" runat="server">
                                    <asp:ListItem Value="S">Fecha de vencimiento Nueva</asp:ListItem>
                                    <asp:ListItem Value="N">Fecha de vencimiento Original</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <b><asp:Label ID="lblVencimiento" runat="server">Vencimientos</asp:Label><asp:Label runat="server" ID="lblCantidad">(0)</asp:Label></b>
            </div>
            <br />
            <div class="grilla">
                <asp:GridView ID="dgVencimientos" runat="server" SkinID="Grid" DataKeyNames="Fondo">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="Imagebutton3" runat="server" SkinID="imgCheck" onrowcommand="dgVencimientos_RowCommand"
                                    CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                </asp:ImageButton>
                            </ItemTemplate>
                            <ItemStyle Width="25px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operaci&oacute;n" />
                        <asp:BoundField DataField="Fondo" HeaderText="Codigo de Fondo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="DescripcionFondo" HeaderText="Fondo" />
                        <asp:BoundField DataField="NumeroTransaccion" HeaderText="Numero Transacci&oacute;n" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operaci&oacute;n"  />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripci&oacute;n" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                        <asp:BoundField DataField="MontoOperacion" DataFormatString="{0:#,##0.00}" HeaderText="Monto Operaci&oacute;n" />
                        <asp:BoundField DataField="ISIN" HeaderText="ISIN" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="TipoOrden" HeaderText="Tipo Orden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="Categoria" HeaderText="Categoria" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vencimiento" />
                        <asp:BoundField DataField="ValorNominaLocalCupon" HeaderText="Valor Nomina Local Cup&oacute;n"
                            ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="TipoConfirmacion" HeaderText="Tipo Confirmaci&oacute;n" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoNemonico" HeaderText="CodigoNemonico" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="Secuencial" HeaderText="Secuencial" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoSBS" HeaderText="CodigoSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoOperacion" HeaderText="CodigoOperacion" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoTipoCupon" HeaderText="CodigoTipoCupon" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoTercero" HeaderText="CodigoTercero" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoTipoTitulo" HeaderText="CodigoTipoTitulo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="TasaCupon" HeaderText="Tasa Cup&oacute;n"  DataFormatString="{0:#,##0.00}" />
                        <asp:BoundField DataField="MontoNominalOperacion" DataFormatString="{0:#,##0.00}" HeaderText="Unidades" />
                        <asp:BoundField DataField="FechaOperacionGenera" HeaderText="Fecha Operaci&oacute;n Genera" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoOrden" HeaderText="Codigo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div class="row">
                <div class="col-md-13" style="text-align: right;">
                    <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                    <asp:Button Text="Salir" runat="server" ID="btnSalir" />
                </div>
            </div>
        </fieldset>
        </div>
    </form>
</body>
</html>