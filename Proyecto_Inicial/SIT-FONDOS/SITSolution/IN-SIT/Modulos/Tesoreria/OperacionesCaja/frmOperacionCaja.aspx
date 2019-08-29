<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOperacionCaja.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmOperacionCaja" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Ingresos y Egresos Varios</title>
    <script type="text/javascript">             
        function SeleccionarOpcion(sel1, sel2) {
            sel1.disabled = false;
            sel2.selectedIndex = 0;
            sel2.disabled = true;
            document.getElementById("ddlNumeroCuentaDestino").selectedIndex = 0;
            document.getElementById("ddlBancoDestino").selectedIndex = 0;
            document.getElementById("ddlNumeroCuentaDestino").disabled = true;
            document.getElementById("ddlBancoDestino").disabled = true;
        }
        function ValidarCuentas() {
                        
            if (document.getElementById("lblFechaPago").value == "") {
                alertify.alert("Debe Ingresar Fecha de Pago.");
                return false;
            }
            if (!document.getElementById("ddlOperacion").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una operación.");
                return false;
            }
            if (!document.getElementById("ddlNroCuenta").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una cuenta origen.");
                return false;
            }
            if (document.getElementById("txtImporte").value == "" || document.getElementById("txtImporte").value * 1 == 0) {
                alertify.alert("El importe debe ser mayor a cero.");
                return false;
            }
            var valorBuscado = "CAPESTR";
            var e = document.getElementById("ddlPortafolio");
            var textoCombo = e.options[e.selectedIndex].text;
            if (document.getElementById("ddlOperacion").value == "OP0089" && document.getElementById("ddlTipoOperacion").value == "1" && document.getElementById("ddlClase").value == "20" && textoCombo.indexOf(valorBuscado) > -1 && document.getElementById("ddlTipoPago").value == "TRNS") {
                alertify.confirm('<b>¿Está seguro de registrar el egreso en caja y en Suscripción de Fondos CXC de Valor Cuota? </b>', function (e) {
                    if (e) {
                        document.getElementById("hdRpta").value = "SI";
                        __doPostBack('btnAceptar', '')
                        return true;
                    }
                    else {
                        document.getElementById("hdRpta").value = "NO";
                        return false;
                    }
                });
            }
            else 
            {
                document.getElementById("hdRpta").value = "SI";
                return true;
            }
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Ingresos y Egresos Varios</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <asp:UpdatePanel ID="updatos" runat="server" UpdateMode ="Conditional" >
            <ContentTemplate>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Pago</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="lblFechaPago" SkinID="Date" AutoPostBack="true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoOperacion" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Banco</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="280px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="200px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Clase Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClase" Width="200px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Nro. Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlNroCuenta" Width="200px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Modelo Carta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlModeloCarta" Width="350px" Enabled ="false" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo Pago</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoPago" Width="200px" Enabled ="false"  />
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdCodigoMoneda" />
           <%-- INICIO | RCE | ZOLUXIONES - Proy> FondosII | Campo Oculto que controla la respuesta de confirmación de Grabar Egreso de suscripción de fondo | 22/06/2018--%>
            <asp:HiddenField runat="server" ID="hdRpta" />
            <%-- FIN | RCE | ZOLUXIONES - Proy> FondosII | Campo Oculto que controla la respuesta de confirmación de Grabar Egreso de suscripción de fondo | 22/06/2018--%>
            </ContentTemplate>
            <Triggers >
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset id="pnlDatosEgreso" runat="server">
            <legend>Información Referencial</legend>
            <asp:UpdatePanel ID="updatosegreso" runat="server" UpdateMode ="Conditional" >
            <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            <asp:RadioButton ID="opcIntermediario" runat="server" GroupName="Referencial" Text="Intermediario" onclick="SeleccionarOpcion(ddlIntermediario,ddlTercero)" />                            
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlIntermediario" Width="280px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Banco</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlBancoDestino" Width="280px" AutoPostBack="True" Enabled="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Nro. Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlNumeroCuentaDestino" Width="200px" Enabled="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            <asp:RadioButton ID="opcTercero" runat="server" GroupName="Referencial" Text="Tercero" onclick="SeleccionarOpcion(ddlTercero,ddlIntermediario)" />
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTercero" Width="280px" />
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlNroCuenta" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlClase" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlMoneda" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlBanco" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlMercado" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upmoneda" runat="server" UpdateMode = "Conditional" >
            <ContentTemplate >
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtReferencia" Width="150px" MaxLength="20" CssClass="mayusculas" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Importe</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtImporte" Width="150px" onkeypress="return soloNumeros(event)" />
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlNroCuenta" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlClase" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlMoneda" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlBanco" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlMercado" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>        
    </div>    
    </form>
</body>
</html>