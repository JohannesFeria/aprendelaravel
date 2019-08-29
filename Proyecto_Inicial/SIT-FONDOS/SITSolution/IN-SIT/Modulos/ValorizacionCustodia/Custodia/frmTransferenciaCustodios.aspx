<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTransferenciaCustodios.aspx.vb"
    Inherits="Modulos_Valorizacion_y_Custodia_Custodia_frmTransferenciaCustodios" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Transferencias entre Custodios</title>
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
                        Transferencias entre Custodios</h2>
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
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" 
                                    ReadOnly="True" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Fecha Operaci&oacute;n"
                                ControlToValidate="tbFechaOperacion" runat="server" />
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
                </div>
            </div>
        </fieldset>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Custodio Inicial</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCustodio" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Custodio Final</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlCustodioFinal" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Saldo Custodio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSaldoCustodio" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Saldo Custodio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSaldoCustodioFinal" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
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
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <header></header>
        <div class="row">
            <div class="col-md-12" style="text-align: right">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
