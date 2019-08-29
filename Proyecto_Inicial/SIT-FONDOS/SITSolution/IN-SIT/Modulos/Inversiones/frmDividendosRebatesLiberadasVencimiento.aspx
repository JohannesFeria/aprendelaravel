<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmDividendosRebatesLiberadasVencimiento.aspx.vb" Inherits="Modulos_Inversiones_frmDividendosRebatesLiberadasVencimiento" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Cerrar() { window.close(); }
        function Confirmar() {
            if (confirm("¿Desea Confirmar la operación.")) { return true;}
            else { return false; }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Confirmar Dividendos, Rebates</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-4 control-label">Fecha de vencimiento</label>
                <div class="col-sm-4">
                    <asp:TextBox ID="lFechaVencimiento" runat="server"  Enabled="false" Width="150px" />
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-3 control-label">Importe Local</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="lMontoNominalLocal" runat="server"  Enabled="false" Width="150px"/>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-3 control-label">Moneda</label>
                <div class="col-sm-9">
                    <asp:TextBox ID="lMoneda" runat="server"  Enabled="false" Width="150px"/>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Portafolio</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="lblPortafolio" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Identificador</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="lbIdentificador" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Tipo Distribución</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbTipoDistribucion" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Mnemónico</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="lblNemonico" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Factor</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbFactor" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Corte</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbFechaCorte" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Estado</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="lblEstado" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Entrega</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbFechaEntrega" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha IDI</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbFechaIDI" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Confirmar Monto</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtMontoConfirmar" runat="server" Text="" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Unidades</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="tbUnidades" runat="server"  Enabled="false" Width="150px"/>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" />
    </div>
    </div>
    </form>
</body>
</html>
