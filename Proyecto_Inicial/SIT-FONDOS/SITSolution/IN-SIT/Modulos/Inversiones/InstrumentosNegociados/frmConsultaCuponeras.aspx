<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaCuponeras.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_frmConsultaCuponeras" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Consulta de Cuponeras</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Consulta de Cuponera</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Estimación de Precios</legend>
            <div class="grilla">
                <asp:UpdatePanel ID="upGrilla" runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                            <Columns>
                                <asp:BoundField DataField="FechaTermino1" HeaderText="Fecha Vencimiento">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Amortizacion" HeaderText="Amortización">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DiferenciaDias" HeaderText="Dias Trans.">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TasaCupon" HeaderText="Cupón">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Flujo" HeaderText="Flujo">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DiasAcumulados" HeaderText="Días Acum.">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalVP" HeaderText="Total VP">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <br />
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Precio Calculado (%)</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lblPrecioCalculado" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Monto Operación</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lblMontoOperación" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Interés Corrido</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="lblInteresCorrido" Width="150px" ReadOnly="true" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <header></header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Retornar" runat="server" ID="btnRetornar" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
