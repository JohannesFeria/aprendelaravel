<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaTitulosCustodios.aspx.vb"
    Inherits="Modulos_Valorizacion_y_Custodia_Custodia_frmConsultaTitulosCustodios" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>T&iacute;tulos asociados a Custodios</title>
    <script type="text/javascript">
        function showModal(pFecha, sPorta, pIsin, pCCust, pTIns, pCMnemo, pTRenta, pCMoneda) {
            return showModalDialog('Reportes/frmTitulosAsociadosCustodios.aspx?FechaSaldo=' + pFecha + '&PortafolioCodigo=' + sPorta + '&CodigoISIN=' + pIsin + '&CodigoCustodio=' + pCCust + '&TipoInstrumento=' + pTIns + '&CodigoMnemonico=' + pCMnemo + '&TipoRenta=' + pTRenta + '&CodigoMoneda=' + pCMoneda, '800', '600', '');            
        }

        function showModal2() {
            var isin = $('#txtISIN').val();
            var sbs = $('#txtSBS').val();
            var mnemonico = $('#txtMnemonico').val();
            return showModalDialog('frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', '');                     
        }

        function limpiar() {
            $('#txtISIN').val('');
            $('#txtSBS').val('');
            $('#txtMnemonico').val('');
            $('#txtDescripcion').val('');
            return false;
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
                        T&iacute;tulos asociados a Custodios</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Saldo</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaSaldo" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Custodio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="dlCustodio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Instrumento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="dlInstrumento" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="dlMoneda" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo ISIN</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtISIN" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSBS" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" ReadOnly="true" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModal2()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lbkLimpiarControl" OnClientClick="return limpiar()"><span class="add-on"><i class="awe-remove"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-11">
                    <div class="form-group" style="padding-left: 45px;">
                        <label class="col-sm-1 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-11">
                            <asp:TextBox runat="server" ID="txtDescripcion" Width="350px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-1" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnConsulta" />
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
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:BoundField DataField="Descripcion" HeaderText="Fondo" />
                            <asp:BoundField DataField="Custodio" HeaderText="Custodio" />
                            <asp:BoundField DataField="C&#243;digo ISIN" HeaderText="C&#243;digo ISIN" />
                            <asp:BoundField DataField="C&#243;digo Mnemonico" HeaderText="C&#243;digo Mnemonico" />
                            <asp:BoundField DataField="Tipo Titulo" HeaderText="Tipo Titulo" />
                            <asp:BoundField DataField="Nombre Emisor" HeaderText="Nombre Emisor" />
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="Fecha Emisi&#243;n" HeaderText="Fecha Emisi&#243;n" />
                            <asp:BoundField DataField="Fecha Vencimiento" HeaderText="Fecha Vencimiento" />
                            <asp:BoundField DataField="N&#250;mero Unidades" HeaderText="N&#250;mero Unidades"
                                DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="Valor Unitario" HeaderText="Valor Unitario" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="Valor Nominal" HeaderText="Valor Nominal" DataFormatString="{0:#,##0.0000000}" />
                            <asp:BoundField DataField="Valorizado" HeaderText="Valorizado" DataFormatString="{0:#,##0.0000000}" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnConsulta" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnGenerarReporte" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
