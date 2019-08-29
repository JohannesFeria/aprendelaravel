<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoCustodios.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Custodia_frmIngresoCustodios" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro de Faltantes</title>
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
            $('#txtMoneda').val('');
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
                        Registro de Faltantes</h2>
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
                            <asp:DropDownList runat="server" ID="dlFondo" Width="150px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Portafolio"
                                ControlToValidate="dlFondo" runat="server" />
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Fecha Operaci&oacute;n"
                                ControlToValidate="tbFechaOperacion" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Custodio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCustodio" Width="150px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Custodio"
                                ControlToValidate="ddlCustodio" runat="server" />
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Mnem&oacute;nico"
                                ControlToValidate="txtMnemonico" runat="server" />
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
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Saldo Sistema</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSaldoSistema" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Saldo Custodio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSaldoCustodio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label runat="server" id="lblSaldoFecha" class="col-sm-4 control-label">
                            Saldo Fecha</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblfechaSaldoSistema" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label runat="server" id="lblSaldoFechaCust" class="col-sm-4 control-label">
                            Saldo Fecha</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblFechaSaldoCustodio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Unidades</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtCantidadUnidades" Width="150px" />
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                    </label>
                    <div class="col-sm-8">
                        <asp:CheckBox ID="chkSaldoInicial" Text="Ingresar como Saldo Inicial" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgAjuste">
                <Columns>
                    <asp:BoundField DataField="Fecha Ajuste" HeaderText="Fecha Ajuste" />
                    <asp:BoundField DataField="C&#243;digo Mnem&#243;nico" HeaderText="C&#243;digo Mnem&#243;nico" />
                    <asp:BoundField DataField="Descripci&#243;n" HeaderText="Descripci&#243;n" />
                    <asp:BoundField DataField="Unidades Ajustadas" HeaderText="Unidades Ajustadas" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera"  ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
