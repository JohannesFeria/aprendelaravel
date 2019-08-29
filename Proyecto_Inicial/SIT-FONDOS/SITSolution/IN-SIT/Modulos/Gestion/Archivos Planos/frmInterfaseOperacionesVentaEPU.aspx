<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfaseOperacionesVentaEPU.aspx.vb"
    Inherits="Modulos_Gestion_Archivos_Planos_frmInterfaseOperacionesVentaEPU" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>frmInterfaseOperacionesVentaEPU</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Importar Operaciones de Venta - Consitución EPU US
            </h2>
        </header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-10">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Portafolio</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="120px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-10">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Ruta</label>
                        <div class="col-sm-7">
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx"
                                class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar"
                                data-size="sm" size="150" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button ID="btnImportar" runat="server" Text="Importar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo"></asp:BoundField>
                    <asp:TemplateField HeaderText="Portafolio">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlPortafolioF" runat="server" Width="100px">
                            </asp:DropDownList>
                            <asp:HiddenField ID="lbPortafolioF" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.CodigoPortafolioSBS") %>' />
                            <asp:HiddenField ID="hdIntermediario" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.CodigoTercero") %>' />
                            <asp:HiddenField ID="hdCodigo" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoNemonico" HeaderText="Nem&#243;nico"></asp:BoundField>
                    <asp:BoundField DataField="CodigoSBS" HeaderText="Codigo SBS"></asp:BoundField>
                    <asp:BoundField DataField="CodigoISIN" HeaderText="Codigo ISIN"></asp:BoundField>
                    <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Liq"></asp:BoundField>
                    <asp:TemplateField HeaderText="Intermediario">
                        <ItemTemplate>
                            <asp:TextBox ID="tbIntermediario" runat="server" Width="150px" ReadOnly="True" Text='<%# DataBinder.Eval(Container, "DataItem.DescripcionTercero")%>'></asp:TextBox>
                            <asp:ImageButton ID="ibBIntermediario" runat="server" SkinID="imgSearch"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda Neg"></asp:BoundField>
                    <asp:BoundField DataField="CantidadOperacion" HeaderText="Unidades"></asp:BoundField>
                    <asp:BoundField DataField="Precio" HeaderText="Precio"></asp:BoundField>
                    <asp:TemplateField HeaderText="Importe">
                        <ItemTemplate>
                            <asp:TextBox ID="tbImporteOrigen" runat="server" Width="100px" Enabled="False" Text='<%# String.Format("{0:###,###.00}",DataBinder.Eval(Container, "DataItem.MontoOrigen")) %>'>
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="T.C. ($)">
                        <ItemTemplate>
                            <asp:TextBox ID="tbTipoCambio" runat="server" Width="80px" Text='<%# String.Format("{0:##0.0000000}", DataBinder.Eval(Container, "DataItem.TipoCambio")) %>'
                                onblur='javascript:tipoCambio_CalcularImporte(this); formatCurrencyTipoCambio(this);'>
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe ($)">
                        <ItemTemplate>
                            <asp:TextBox ID="tbImporteDestino" runat="server" Width="100px" Enabled="False" Text='<%# String.Format("{0:###,###.00}", DataBinder.Eval(Container, "DataItem.MontoDestino")) %>'>
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgListaOrdenes">
                <Columns>
                    <asp:TemplateField HeaderText="Selec.">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSelec" runat="server" SkinID="imgEdit" CommandName="Select"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoOrden")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoMnemonico")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoISIN")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoSBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoPortafolioSBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoMoneda") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoOrden" HeaderText="Codigo Orden"></asp:BoundField>
                    <asp:BoundField DataField="CodigoMnemonico" HeaderText="Nemónico"></asp:BoundField>
                    <asp:BoundField DataField="CodigoISIN" HeaderText="Codigo ISIN"></asp:BoundField>
                    <asp:BoundField DataField="CodigoSBS" HeaderText="Codigo SBS"></asp:BoundField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio"></asp:BoundField>
                    <asp:BoundField DataField="DescripcionTercero" HeaderText="Intermediario"></asp:BoundField>
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operación" DataFormatString="{0:###,###.00}"
                        HtmlEncodeFormatString="false"></asp:BoundField>
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda"></asp:BoundField>
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operación"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" />
        </div>
    </div>
    </form>
</body>
</html>
