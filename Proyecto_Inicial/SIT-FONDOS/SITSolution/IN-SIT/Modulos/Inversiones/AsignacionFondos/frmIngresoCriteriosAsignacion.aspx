<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoCriteriosAsignacion.aspx.vb"
    Inherits="Modulos_Inversiones_AsignacionFondos_frmIngresoCriteriosAsignacion" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:label text="Asiganción de Pre-Ordenes de Inversión" runat="server" /></h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo de Orden</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lbCodigoOrden" Width="100px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo ISIN</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lbCodigoISIN" Width="100px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lbMnemonico" Width="100px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Monto Nominal</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lbMontoNominal" Width="100px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Unidades (Compra/Venta)</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lbUnidades" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgPreOrdenes">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkOrden" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoPreOrden" HeaderText="Codigo Orden" />
                    <asp:BoundField Visible="False" DataField="CodigoISIN" HeaderText="Codigo ISIN" />
                    <asp:BoundField DataField="CodigoMnemonico" HeaderText="Codigo Mnemonico" />
                    <asp:BoundField DataField="CodigoSBS" HeaderText="Codigo SBS" />
                    <asp:BoundField DataField="CodigoTercero" HeaderText="Intermediario" />
                    <asp:BoundField DataField="CodigoEmisor" HeaderText="Emisor" />
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operacion" DataFormatString="{0:0,0.0000000}" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Operacion" HeaderText="Operacion" />
                    <asp:BoundField Visible="False" DataField="CodigoMoneda" HeaderText="CodigoMoneda" />
                    <asp:BoundField Visible="False" DataField="CodigoOperacion" HeaderText="CodigoOperacion" />
                    <asp:BoundField Visible="False" DataField="FechaOperacion" HeaderText="FechaOperacion" />
                    <asp:BoundField Visible="False" DataField="HoraOperacion" HeaderText="HoraOperacion" />
                    <asp:BoundField Visible="False" DataField="TipoValor" HeaderText="TipoValor" />
                    <asp:BoundField Visible="False" DataField="MontoNominalOrdenado" HeaderText="MontoNominalOrdenado" />
                    <asp:BoundField DataField="MontoNominalOperacion" HeaderText="MontoNominalOperacion" />
                    <asp:BoundField DataField="CantidadOperacion" HeaderText="Cantidad Operacion" />
                    <asp:BoundField Visible="False" DataField="CantidadOrdenado" HeaderText="Cantidad Ordenada" />
                    <asp:BoundField Visible="False" DataField="CodigoAsignacionPreOrden" HeaderText="CodigoAsignacionPreOrden" />
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblImporte" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidad" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <fieldset>
            <legend>
                <asp:Label Text="Asignaci&oacute;n de Fondos" runat ="server" ID="Label2"></asp:Label>(Pre-Ordenes
                seleccionadas)</legend>
            <asp:Label Text="" runat="server" ID="Label1" />
            <div class="row">
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Monto Nominal</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lbNominalTotal" Width="100px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <label runat="server" id="lbEtiqueta" class="col-sm-4 control-label">
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lbUnidadesTotal" Width="100px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoAsignacion" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Modo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlModo" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" />
                    <asp:BoundField DataField="PorcentajePropuesto" HeaderText="Asignaci&#243;n (%)" />
                    <asp:TemplateField HeaderText="Asignaci&#243;n (%)">
                        <ItemTemplate>
                            <asp:TextBox ID="tbAsignacion" runat="server" Width="88px" MaxLength="5"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UnidadesPropuesto" HeaderText="Unidades" />
                    <asp:TemplateField HeaderText="Unidades">
                        <ItemTemplate>
                            <asp:TextBox ID="tbUnidades" runat="server" Width="90px" MaxLength="15"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False" HeaderText="Impuestos">
                        <ItemTemplate>
                            <asp:TextBox ID="tbImpuestos" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False" HeaderText="Total">
                        <ItemTemplate>
                            <asp:TextBox ID="tbTotal" runat="server" Width="74px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField Visible="False" DataField="CodigoAsignacionPreOrden" HeaderText="CodigoAsignacionPreOrden" />
                    <asp:BoundField Visible="False" DataField="PorcentajeReal" HeaderText="PorcentajeReal" />
                    <asp:BoundField Visible="False" DataField="UnidadesReal" HeaderText="UnidadesReal" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div class="grilla-small">
            <asp:GridView runat="server" SkinID="GridSmall" ID="dgvDetalle">
                <Columns>
                    <asp:BoundField DataField="CodigoAsignacionPreOrden" HeaderText="C&oacute;digo Orden" />
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" />
                    <asp:BoundField DataField="UnidadesPropuesto" HeaderText="Unidades" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio/Tasa" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operacion" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField Visible="False" DataField="PorcentajePropuesto" HeaderText="Porcentaje" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
                <asp:Button Text="Modificar" runat="server" ID="btnModificar" />
                <asp:Button Text="Grabar" runat="server" ID="btnGuardar" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hdCodigoOperacion" type="hidden" runat="server" name="hdCodigoOperacion">
    </form>
</body>
</html>
