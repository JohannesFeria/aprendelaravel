<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExcesosxBroker.aspx.vb"
    Inherits="Modulos_Inversiones_ExcesosLimite_frmExcesosxBroker" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Ordenes Excedidas por Limite de Brokers</title>
    <script type="text/javascript">
        function Confirmar() {
            return confirm("¿Desea eliminar la Orden de Inversión?");
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
                        Ordenes Excedidas por Limite de Brokers</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fondo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondoOE" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Orden</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtNroOrdenOE" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscarOE" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgListaCE">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionarPE" runat="server" SkinID="imgCheck" OnCommand="Seleccionar"
                                CommandName="Seleccionar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NumeroTransaccion")&amp;","&amp;DataBinder.Eval(Container.DataItem, "Fondo")&amp;","&amp;DataBinder.Eval(Container.DataItem, "ISIN")&amp;","&amp;DataBinder.Eval(Container.DataItem, "TipoOrden")&amp;","&amp;DataBinder.Eval(Container.DataItem, "TipoOperacion")&amp;","&amp;DataBinder.Eval(Container.DataItem, "Categoria")&amp;","&amp;DataBinder.Eval(Container.DataItem, "Estado")&amp;","&amp;CType(Container, GridViewRow).RowIndex %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NombreFondo" HeaderText="Fondo" />
                    <asp:BoundField DataField="NumeroTransaccion" HeaderText="Número Transacción" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operación" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operación" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="ISIN" HeaderText="ISIN" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="TipoOrden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Categoria" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Estado" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Fondo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
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
                            Fondo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondoOA" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Orden</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtNroOrdenOC" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscarOA" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgListaOA">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionar" runat="server" SkinID="imgCheck" CommandName="Seleccionar"
                                CommandArgument='<%# Eval("NumeroTransaccion")&amp;","&amp;Eval("TipoOperacion")&amp;","&amp;Eval("ISIN")&amp;","&amp;Eval("TipoOrden")&amp;","&CType(Container, GridViewRow).RowIndex %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NombreFondo" HeaderText="Fondo" />
                    <asp:BoundField DataField="NumeroTransaccion" HeaderText="Número Transacción" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operación" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operación" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="ISIN" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="TipoOrden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Categoria" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Fondo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <legend>Detalle de Ordenes Excedidas por L&iacute;mite de Broker</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo ISIN</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblCodigoISIN" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nro. Transacci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblNroTransaccion" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Orden</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblTipoOrden" Width="250px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lCategoria" Width="250px" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblTipoOperacion" Width="250px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lFondo" Width="250px" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Consultar" runat="server" ID="btnConsulta" Visible="False" />
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" />
                <asp:Button Text="Aprobar" runat="server" ID="btnAprobar" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="txtEstado" />
    </form>
</body>
</html>
