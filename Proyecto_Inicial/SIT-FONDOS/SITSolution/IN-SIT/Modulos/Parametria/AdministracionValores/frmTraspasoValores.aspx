<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTraspasoValores.aspx.vb" Inherits="Modulos_Parametria_AdministracionValores_frmTraspasoValores" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Traspaso de Valores</title>
        <script type="text/javascript">
            function showModalNemOri() {
                $('#hdBusqueda').val("Origen");
                var isin = $('#txtCodigoIsinOrigen').val().trim(); var sbs = '';
                var mnemonico = $('#txtCodigoNemonicoOrigen').val().trim();
                return showModalDialog('../../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', '');
            }
            function showModalNemDes() {
                $('#hdBusqueda').val("Destino");
                var isin = $('#txtCodigoIsinDestino').val().trim(); var sbs = '';
                var mnemonico = $('#txtCodigoNemonicoDestino').val().trim();
                return showModalDialog('../../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', '');
            }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><div class="row"><div class="col-md-6"><h2>Traspaso de Valores</h2></div></div></header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-4">
                     <div class="form-group">
                        <label class="col-sm-4 control-label">Código Nemonico Origen</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtCodigoNemonicoOrigen"  MaxLength ="12" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonicoOri" OnClientClick="return showModalNemOri()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                     <div class="form-group">
                        <label class="col-sm-4 control-label">Código Isin Origen</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtCodigoIsinOrigen"  MaxLength ="12" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código Nemonico Destino</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtCodigoNemonicoDestino"  MaxLength ="12" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="LinkButton3" OnClientClick="return showModalNemDes()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                     <div class="form-group">
                        <label class="col-sm-4 control-label">Código Isin Destino</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtCodigoIsinDestino"  MaxLength ="12" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Proceso</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaProceso" SkinID="Date" MaxLength ="12" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="grilla">
                <asp:GridView runat="server" ID="dgLista" SkinID="Grid">
                    <Columns>
                        <asp:BoundField DataField="Descripcion" HeaderText="Portafolio" />
                        <asp:BoundField DataField="CodigoIsinDestino" HeaderText="Instrumento Traspasado" />
                        <asp:BoundField DataField="SaldoTraspaso" HeaderText="Saldo de Unidades" />
                        <asp:BoundField DataField="MontoInversionTraspaso" HeaderText="Monto de Inversión" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="row">
                <div class="col-md-6" style="text-align: right">
                    <asp:Button Text="Aceptar" ID="btnAceptar" runat="server" />
                    <asp:Button Text="Salir" ID="btnSalir" runat="server" />
                </div>
            </div>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdBusqueda" runat="server" />
    </form>
</body>
</html>
