<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmHipervalorizador.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmHipervalorizador" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>HiperValorizador</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        HiperValorizador</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mnem&oacute;nico
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblCodigoNemonico" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    CARACTERISTICAS
                </div>
                <div class="col-md-5">
                    NEGOCIACION
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:BoundField DataField="Evento" HeaderText="Evento" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Cupon" HeaderText="Cup&#243;n" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="TIR" HeaderText="TIR(C)" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="VA" HeaderText="VA (C)" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="Dias" HeaderText="D&#237;as" DataFormatString="{0:#,##0}" />
                            <asp:BoundField DataField="Amortiza" HeaderText="Amortiza" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="AmortAcum" HeaderText="Amort. Acum" DataFormatString="{0:#,##0.00}" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valor Actual</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbValorActual" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            + Intereses</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbMasIntereses" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Valor Total</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbValorTotal" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button Text="Imprimir" runat="server" ID="btnExportar" />
                    <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
                </div>
            </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
