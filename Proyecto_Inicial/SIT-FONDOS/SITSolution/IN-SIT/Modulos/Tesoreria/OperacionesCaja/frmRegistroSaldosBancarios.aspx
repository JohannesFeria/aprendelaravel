<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRegistroSaldosBancarios.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmRegistroSaldosBancarios" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro de Saldos Bancarios</title>
    <script type="text/javascript">
        function ValidarCuenta() {
            if (!document.getElementById("ddlNroCuenta").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un número de cuenta.");
                return false;
            }
            if (document.getElementById("txtSaldoDisponible").value == "") {
                alertify.alert("Debe Ingresar un Saldo Disponible.");
                return false;
            }
            return true;
        }
        function ValidarSaldos() {
            if (!document.getElementById("ddlNroCuenta").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un número de cuenta.");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Registro de Saldos Bancarios</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="lblFechaOper" runat="server" SkinID="Date" />
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
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="220px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="220px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase de Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseCuenta" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Número de Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlNroCuenta" Width="180px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
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
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Select" />
                    <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelection" runat="server" AutoPostBack="True"></asp:CheckBox>
                            <asp:HiddenField runat="server" ID="_NroCuenta" Value='<%# Bind("NumeroCuenta") %>' />
                            <asp:HiddenField runat="server" ID="_CodPortafolioSBS" Value='<%# Bind("CodigoPortafolioSBS") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DescripcionTercero" HeaderText="Banco" />
                    <asp:BoundField DataField="DescripcionMoneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="ClaseDescripcion" HeaderText="Clase de Cuenta" />
                    <asp:BoundField DataField="CodigoPortafolioSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="NumeroCuenta" HeaderText="Número de Cuenta" />
                    <asp:TemplateField HeaderText="Saldo Inicial del Dia">
                        <ItemTemplate>
                            <asp:Label ID="lblSaldoDisponible" runat="server"></asp:Label>
                            <asp:TextBox ID="txtSaldoDisponible" runat="server" Width="140px" 
                                CssClass="Numbox-7" ></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Saldo Contable del Dia">
                        <ItemTemplate>
                            <asp:Label ID="lblSaldoContable" runat="server" Text='<%# Bind("SaldoContable", "{0:#,0.0000000}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField Visible="False" DataField="SaldoDisponibleInicial" HeaderText="Saldo Inicial" />
                    <asp:BoundField Visible="False" DataField="SaldoContableInicial" HeaderText="Saldo Disponible" />                    
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
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>