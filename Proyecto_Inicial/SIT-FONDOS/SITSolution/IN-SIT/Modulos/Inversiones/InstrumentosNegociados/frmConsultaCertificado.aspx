<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaCertificado.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_frmConsultaCertificado" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Consulta de Certificados</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lbTitulo" runat="server"></asp:Label></h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <asp:Label ID="lbContador" runat="server"></asp:Label>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="upGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio">
                                <ItemStyle HorizontalAlign="Center" Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CodigoNemonico" HeaderText="Mnem&#243;nico">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaEmision" HeaderText="Fecha Emisi&#243;n">
                                <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Expiraci&#243;n">
                                <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NumeroUnidades" HeaderText="Nro. Certificados" DataFormatString="{0:0,0.0000000}" HtmlEncodeFormatString="false">
                                <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ValorUnitario" HeaderText="Precio" DataFormatString="{0:0,0.0000000}" HtmlEncodeFormatString="false">
                                <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ValorNominal" HeaderText="Monto Nominal" DataFormatString="{0:0,0.0000000}" HtmlEncodeFormatString="false">
                                <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="ibImprimir" />
                <asp:Button Text="Retornar" runat="server" ID="ibCancelar" OnClientClick="javascript:window.print();" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
