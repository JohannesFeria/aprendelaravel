<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPagoParcial.aspx.vb"
    Inherits="Modulos_Tesoreria_Cuentasxpagar_frmPagoParcial" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Pago Parcial</title>
    <script type="text/javascript">
        function ValidarDatos() {
            if (document.getElementById("txtFechaLiq").value == '') {
                alertify.alert("Debe seleccionar una fecha de liquidación.");
                return false;
            }
            return true;
        }
        function ActualizarEstadoControles(chbSeleccion, txImporte, txPorcentaje, dllFormaPago, ddlClaseCuenta, ddlBanco, ddlNroCuenta) {
            var estado;
            if (chbSeleccion.checked) {
                estado = true;
            }
            else {
                txImporte.value = '';
                txPorcentaje.value = '';
                estado = false;
            }

            txImporte.disabled = !estado;
            txPorcentaje.disabled = !estado;
            dllFormaPago.disabled = !estado;
            ddlClaseCuenta.disabled = !estado;
            ddlBanco.disabled = !estado;
            ddlNroCuenta.disabled = !estado;
        }

        function ActualizarImporte(txImporte, txPorcentaje) {
            var ImporteTotal = document.getElementById("txtImporteTotal").value.replace(",", "");

            if (txPorcentaje.value == '') {
                txImporte.value = '';
                return false;
            }
            if (txPorcentaje.value.replace(",", "") * 1 > 100) {
                alertify.alert('El porcentaje ingresado no es válido');
                txPorcentaje.focus();
                return false;
            }
            txImporte.value = ImporteTotal * txPorcentaje.value.replace(",", "") / 100;
            txImporte.value = Math.round(txImporte.value.replace(",", "") * 100) / 100;
        }

        function ActualizarPorcentaje(txPorcentaje, txImporte) {
            var ImporteTotal = document.getElementById("txtImporteTotal").value.replace(",", "");
            if (txImporte.value == '') {
                txPorcentaje.value = '';
                return false;
            }
            if (txImporte.value.replace(",", "") * 1 > ImporteTotal * 1) {
                alertify.alert('El importe ingresado no es válido');
                txImporte.focus();
                return false;
            }
            txPorcentaje.value = (txImporte.value.replace(",", "") / ImporteTotal) * 100;
            txPorcentaje.value = Math.round(txPorcentaje.value.replace(",", "") * 100) / 100;
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
                        Pago Parcial</h2>
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
                            <asp:TextBox runat="server" ID="txtPortafolio" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtMoneda" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Pago</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaLiq" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Importe Total</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtImporteTotal" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="Indice" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:CheckBox ID="chbSeleccion" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Forma de Pago ">
                        <ItemTemplate>
                            <asp:DropDownList ID="dllFormaPago" runat="server" Width="160px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Porcentaje">
                        <ItemTemplate>
                            <asp:TextBox ID="txPorcentaje" runat="server" Width="60px" CssClass="Numbox-2"></asp:TextBox>
                            <asp:Label ID="Label1" runat="server">%</asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe">
                        <ItemTemplate>
                            <asp:TextBox ID="txtImporte" runat="server" Width="80px" CssClass="Numbox-2" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Clase Cuenta">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlClaseCuenta" runat="server" Width="130px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_SelectedIndexChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Banco">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlBanco" runat="server" Width="130px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_SelectedIndexChanged1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nro. Cuenta">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlNroCuenta" runat="server" Width="130px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
