<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmTransferencias.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmTransferencias" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>VencimientosDiarios</title>
    <script type="text/javascript">
        function ValidarCuentas() {
            if (!document.getElementById("ddlMoneda").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una moneda.");
                return false;
            }
            if (!document.getElementById("ddlBanco").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un banco origen.");
                return false;
            }
            if (!document.getElementById("ddlBancoDestino").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un banco destino.");
                return false;
            }
            if (!document.getElementById("ddlClaseCuenta").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una clase de cuenta origen");
                return false;
            }
          if (!document.getElementById("ddlClaseCuentaDestino").selectedIndex > 0) {
              alertify.alert("Debe seleccionar una clase de cuenta destino");
              return false;
          }
//            if (!document.getElementById("ddlNroCuenta").selectedIndex > 0 || !document.getElementById("ddlNroCuentaDestino").selectedIndex > 0) {
//                alertify.alert("Debe seleccionar una cuenta origen y una destino.");
//                return false;
//            }
            if (document.getElementById("ddlNroCuenta").value == document.getElementById("ddlNroCuentaDestino").value) {
                alertify.alert("Las cuentas de origen y destino son las mismas.");
                return false;
            }
            if (document.getElementById("txtImporte").value == "" || document.getElementById("txtImporte").value * 1 == 0) {
                alertify.alert("El importe debe ser mayor a cero.");
                return false;
            }
            if (document.getElementById("hdTransferenciaValida").value == "NO") {
                if (!confirm('Transferencia no permitida. ¿Desea continuar?.')) {
                    return false;
                }
                else {
                    $('#hdTransferenciaValida').val("SI");
                    document.getElementById("<%= btnAceptar.ClientID %>").onclick() = '';
                    document.getElementById("<%= btnAceptar.ClientID %>").click();
                }
            }
            return true;
        }			
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>
                Transferencias de Cuentas Propias
            </h2>
        </header>
        <br />
        <asp:UpdatePanel ID="up_Origen" runat="server" UpdateMode ="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMoneda" runat="server" AutoPostBack="True" Width="200px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo de Traspaso</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddltipoTraspaso" runat="server" AutoPostBack = "true">
                                    <asp:ListItem Text ="Transferecia Exterior" Value = "63" ></asp:ListItem>
                                    <asp:ListItem Text ="Transferecia Interna" Value = "64" ></asp:ListItem>
                                    <asp:ListItem Text ="Transferecia BCR" Value ="BCRI"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                <span class="add-on" id="imgFecha"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <br />
            <fieldset>
                <legend>Origen</legend>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Mercado</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlMercado" runat="server" AutoPostBack="True" Width="170px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Portafolio</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="170px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Banco</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlBanco" runat="server" AutoPostBack="True" Width="280px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Clase de Cuenta</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlClaseCuenta" runat="server" AutoPostBack="True" Width="170px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Numero de Cuenta</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlNroCuenta" runat="server" Width="170px" AutoPostBack="True" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Contacto</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlContacto" runat="server" Width="216px">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-1">
                            </label>
                            <div class="col-sm-11">
                                <asp:Label ID="lblSaldoDisponible" runat="server"></asp:Label><br>
                                <asp:Label ID="lblSaldoDisponible2" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlMoneda" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="up_destino" runat="server" UpdateMode ="Conditional">
        <ContentTemplate>
            <fieldset>
                <legend>Destino</legend>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Mercado</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlMercadoDestino" runat="server" AutoPostBack="True" Width="170px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Portafolio</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlPortafolioDestino" runat="server" AutoPostBack="True" Width="170px" Enabled ="false" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Banco</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlBancoDestino" runat="server" AutoPostBack="True" Width="280px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Clase de Cuenta</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlClaseCuentaDestino" runat="server" AutoPostBack="True" Width="170px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Numero de Cuenta</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlNroCuentaDestino" runat="server" Width="170px" AutoPostBack="True" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                    </div>
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-sm-1">
                            </label>
                            <div class="col-sm-11">
                                <asp:Label ID="lblSaldoDisponible1Destino" runat="server"></asp:Label><br>
                                <asp:Label ID="lblSaldoDisponible2Destino" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlMoneda" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPortafolio" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>        
        <br />
        <asp:UpdatePanel ID="up_transferencia" runat="server" UpdateMode ="Conditional">
        <ContentTemplate>
            <fieldset>
                <legend>Transferencia</legend>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Importe</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtImporte" Width="100px" CssClass="Numbox-7" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Moneda:</label>
                            <div class="col-sm-8 control-label" style="text-align: left;">
                                <asp:Label ID="lblMoneda" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Modelo Carta</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlModeloCarta" runat="server" Width="300px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Observaciones</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtObservacionCarta" runat="server" Width="650px" MaxLength ="1000" TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
            </div>
            </fieldset>
        </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlMoneda" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddltipoTraspaso" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
        <br />
    </div>
    <asp:UpdatePanel ID="uphd" runat="server" UpdateMode ="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdCodigoTerceroOrigen" runat="server" />
        <asp:HiddenField ID="hdCodigoTerceroDestino" runat="server" />
        <asp:HiddenField ID="hdTransferenciaValida" runat="server" />
        <asp:HiddenField ID="hdValSaldo" runat="server" />
    </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlClaseCuenta" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPortafolio" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPortafolioDestino" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlClaseCuentaDestino" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>