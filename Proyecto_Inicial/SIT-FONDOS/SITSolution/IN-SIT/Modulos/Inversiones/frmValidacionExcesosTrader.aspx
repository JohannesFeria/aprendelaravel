<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmValidacionExcesosTrader.aspx.vb" Inherits="Modulos_Inversiones_frmValidacionExcesosTrader" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Validaci&oacute;n de Excesos por Trader</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Validaci&oacute;n de Excesos por Trader
                    </h2>
                </div>
            </div>
        </header>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="GrupoInstrumento" HeaderText="Grupo Instrumento" />
                    <asp:BoundField DataField="PosicionNegociado" HeaderText="Posici&oacute;n Negociado"
                        DataFormatString="{0:###,###,##0.0000000}" />
                    <asp:BoundField DataField="PosicionLimite" HeaderText="Posici&oacute;n Limite" DataFormatString="{0:###,###,##0.0000000}" />
                    <asp:BoundField DataField="PorcentajeNegociado" HeaderText="Porcentaje Negociado" />
                    <asp:BoundField DataField="PorcentajeLimite" HeaderText="Porcentaje Limite" />
                    <%--<asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" />--%>
                    <asp:BoundField DataField="Descripcion" HeaderText="Portafolio" />
                    <asp:BoundField DataField="TipoCargo" HeaderText="Aprobador" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-ForeColor="Red" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Enviar Aprobaci&oacute;n" runat="server" ID="btnAprobar" />
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnRetornar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
