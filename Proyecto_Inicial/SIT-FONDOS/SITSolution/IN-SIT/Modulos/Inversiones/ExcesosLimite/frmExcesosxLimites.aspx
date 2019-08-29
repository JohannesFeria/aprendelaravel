<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExcesosxLimites.aspx.vb"
    Inherits="Modulos_Inversiones_ExcesosLimite_frmExcesosxLimites" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Aprobar Excesos por L&iacute;mite</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Ordenes Excedidas por L&iacute;mites de Inversión</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <asp:Label ID="lblFondo" runat="server">Fondo</asp:Label></label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFondoOE" runat="server" Width="120px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Orden</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtNroOrdenOE" runat="server" CssClass="stlCajaTexto" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button ID="ibBuscarOE" runat="server" Text="Buscar" Height="26px" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView ID="dgListaCE" runat="server" SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <HeaderStyle Width="80px"></HeaderStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionarPE" runat="server" SkinID="imgCheck" OnCommand="Seleccionar"
                                CommandName="Seleccionar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NumeroTransaccion")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoPortafolio")&amp;","&amp;DataBinder.Eval(Container.DataItem, "Fondo")&amp;","&amp;DataBinder.Eval(Container.DataItem, "ISIN")&amp;","&amp;DataBinder.Eval(Container.DataItem, "TipoOrden")&amp;","&amp;DataBinder.Eval(Container.DataItem, "TipoOperacion")&amp;","&amp;DataBinder.Eval(Container.DataItem, "Categoria")&amp;","&amp; DataBinder.Eval(Container.DataItem, "Descripcion") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolio" HeaderText="CodigoPortafolio">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Fondo" HeaderText="Fondo">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NumeroTransaccion" HeaderText="N&#250;mero Transacci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operaci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operaci&#243;n" DataFormatString="{0:#,##0.0000000}">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField ControlStyle-CssClass="hidden" DataField="ISIN" HeaderText="ISIN">
                    </asp:BoundField>
                    <asp:BoundField ControlStyle-CssClass="hidden" DataField="TipoOrden" HeaderText="TipoOrden">
                    </asp:BoundField>
                    <asp:BoundField ControlStyle-CssClass="hidden" DataField="Categoria" HeaderText="Categoria">
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
            <h5 style="text-align: center;">
                Ordenes Aprobadas</h5>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <asp:Label ID="Label1" runat="server">Fondo</asp:Label></label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlFondoOA" runat="server" Width="120px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Orden</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtNroOrdenOC" runat="server" CssClass="stlCajaTexto" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button ID="ibBuscarOA" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView ID="dgListaOA" runat="server" SkinID="Grid">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <HeaderStyle Width="80px"></HeaderStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionar" runat="server" SkinID="imgCheck" CommandName="Seleccionar"
                                OnCommand="Seleccionar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NumeroTransaccion")&amp;","&amp;DataBinder.Eval(Container.DataItem, "CodigoPortafolio")&amp;","&amp;DataBinder.Eval(Container.DataItem, "Fondo")&amp;","&amp;DataBinder.Eval(Container.DataItem, "ISIN")&amp;","&amp;DataBinder.Eval(Container.DataItem, "TipoOrden")&amp;","&amp;DataBinder.Eval(Container.DataItem, "TipoOperacion")&amp;","&amp;DataBinder.Eval(Container.DataItem, "Categoria")&amp;","&amp; DataBinder.Eval(Container.DataItem, "Descripcion") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolio" HeaderText="CodigoPortafolio">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Fondo" HeaderText="Fondo">
                        <HeaderStyle Width="80px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NumeroTransaccion" HeaderText="N&#250;mero Transacci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operaci&#243;n">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operaci&#243;n" DataFormatString="{0:#,##0.0000000}">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField ControlStyle-CssClass="hidden" DataField="ISIN" HeaderText="ISIN">
                    </asp:BoundField>
                    <asp:BoundField ControlStyle-CssClass="hidden" DataField="TipoOrden" HeaderText="TipoOrden">
                    </asp:BoundField>
                    <asp:BoundField ControlStyle-CssClass="hidden" DataField="Categoria" HeaderText="Categoria">
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <legend>Detalle de Ordenes Excedidas por L&iacute;mite de Inversi&oacute;n</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <asp:Label ID="Label2" runat="server">Código ISIN</asp:Label></label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblCodigoISIN" runat="server" Width="120px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Transacción</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblNroTransaccion" runat="server" Width="96px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <asp:Label ID="Label3" runat="server">Tipo Orden</asp:Label></label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblTipoOrden" runat="server" Width="250px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                        </label>
                        <div class="col-sm-8">
                            <div style="display: none">
                                <asp:TextBox ID="lCategoria" runat="server" Width="96px" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            <asp:Label ID="Label4" runat="server">Tipo Operación</asp:Label></label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblTipoOperacion" runat="server" Width="250px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                        </label>
                        <div class="col-sm-8">
                            <div style="display: none">
                                <asp:TextBox ID="lFondo" runat="server" Width="96px" ReadOnly="true" />
                                <asp:TextBox ID="lDescripcion" runat="server" Width="96px" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button runat="server" ID="ibAprobar" Text="Aprobar" />
                <asp:Button runat="server" ID="ibSalir" Text="Salir" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
