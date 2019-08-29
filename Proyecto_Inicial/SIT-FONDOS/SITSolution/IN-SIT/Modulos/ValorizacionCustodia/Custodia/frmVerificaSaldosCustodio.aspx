<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVerificaSaldosCustodio.aspx.vb"
    Inherits="Modulos_Valorizacion_y_Custodia_Custodia_frmVerificaSaldosCustodio" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Saldos Custodio por Mnemonico</title>
    <script type="text/javascript">
        function showModal() {
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
    <br />
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Saldos Custodio por Mnemonico</h2>
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
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="Portafolio" ControlToValidate="ddlFondo"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha Operaci&oacute;n" ControlToValidate="tbFechaOperacion"
                                runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Custodio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCustodio" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="Custodio" ControlToValidate="ddlCustodio"
                                runat="server" />
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
                            <asp:TextBox runat="server" ID="txtISIN" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSBS" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModal()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lbkLimpiarControl" OnClientClick="return limpiar()"><span class="add-on"><i class="awe-remove"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Mnem&oacute;nico" ControlToValidate="txtMnemonico"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group" style="padding-left: 40px;">
                        <label class="col-sm-1 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-11">
                            <asp:TextBox runat="server" ID="txtDescripcion" Width="350px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtMoneda" Width="250px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnConsulta" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgAjuste">
                <Columns>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha Saldo" />
                    <asp:BoundField DataField="UnidadesCustodia" HeaderText="Unidades Custodio SIT" />
                    <asp:BoundField DataField="SaldoInicialUnidades" HeaderText="Saldo Custodio" />
                    <asp:BoundField DataField="IngresoUnidades" HeaderText="Ingresos" />
                    <asp:BoundField DataField="EgresoUnidades" HeaderText="Egresos" />
                    <asp:BoundField DataField="UnidadesCartera" HeaderText="Saldo Cartera" />
                    <asp:BoundField DataField="CantidadOperacion" HeaderText="Unidades Negociadas" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Proceso</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="tbFechaProceso" SkinID="Date" />
                            <span class="add-on"><i class="awe-calendar"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ErrorMessage="Fecha Proceso" ControlToValidate="tbFechaProceso"
                            ValidationGroup="vgFechaProceso" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-md-2" style="text-align: right;">
                <asp:Button Text="Procesar" runat="server" ID="btnProcesar" ValidationGroup="vgFechaProceso" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    <asp:ValidationSummary runat="server" ID="vsFechaProceso" ValidationGroup="vgFechaProceso"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
