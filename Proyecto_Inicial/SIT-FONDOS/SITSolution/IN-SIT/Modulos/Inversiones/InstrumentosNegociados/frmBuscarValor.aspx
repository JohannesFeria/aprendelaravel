<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBuscarValor.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmBuscarValor" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Inversiones Realizadas</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Búsqueda Valores</h2>
                </div>
            </div>
        </header>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Tipo de Instrumento</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddlTipoIntrumento" runat="server" Width="230px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="upGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnModificar" runat="server" OnCommand="SeleccionarISIN" SkinID="imgCheck"
                                        CausesValidation="False" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoISIN")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoNemonico")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoSBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoCustodio")&amp;","&amp;DataBinder.Eval(Container, "DataItem.SaldoDisponible")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoPortafolioSBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoMoneda") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Tipo Instrumento" />
                            <asp:BoundField DataField="CodigoISIN" HeaderText="Codigo ISIN" />
                            <asp:BoundField DataField="CodigoNemonico" HeaderText="Codigo Mnemonico" />
                            <asp:BoundField DataField="CodigoSBS" HeaderText="Codigo SBS" />
                            <asp:BoundField DataField="CodigoEmisor" HeaderText="Emisor" />
                            <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="CodigoCustodio" HeaderText="Codigo Custodio" />
                            <asp:BoundField DataField="SaldoDisponible" HeaderText="Cantidad Disponible">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        <br />
        <header></header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:Button Text="Salir" runat="server" ID="ibCancelar" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
