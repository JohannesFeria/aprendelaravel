<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaLimitesInstrumento.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_frmConsultaLimitesInstrumento" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>L&iacute;mites de Inversi&oacute;n</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        L&iacute;mites de Inversi&oacute;n - <asp:Label runat="server" ID="lblInstrumento"></asp:Label></h2>
                </div>
            </div>
        </header>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="NombreLimite" HeaderText="Limite" />
                    <asp:BoundField DataField="DescripcionNivel" HeaderText="Agrupaci&#242;n" />
                    <asp:BoundField DataField="ValorPorcentaje" HeaderText="%" />
                    <asp:BoundField DataField="Posicion" DataFormatString="{0:N0}" HeaderText="Posicion" />
                    <asp:BoundField DataField="Margen" DataFormatString="{0:N0}" HeaderText="Margen" />
                    <asp:BoundField DataField="Alerta" HeaderText="Alerta" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
