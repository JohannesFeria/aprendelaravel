<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaSaldosBancarios.aspx.vb"
    Inherits="Modulos_Tesoreria_OperacionesCaja_frmConsultaSaldosBancarios" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Consulta de Saldos Bancarios</title>
    <script type="text/javascript">
        function ValidarDatos() {
            if (document.getElementById("tbFechaOperacion").value == "") {
                alertify.alert("Debe seleccionar una fecha de operación.");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:UpdatePanel ID="upcuerpo" runat="server" UpdateMode ="Conditional" >
    <ContentTemplate>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Consulta de Saldos Bancarios</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="170px" />
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
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="170px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Clase Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseCuenta" Width="170px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox ID="tbFechaOperacion" runat="server" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button ID="btnBuscar" Text="Buscar" runat="server" />
                    <asp:Button ID="btnExportar" Text="Exportar" runat="server" />
                    <asp:Button ID="btnMovimientos" Text="Movimientos" runat="server" />
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
                    <asp:TemplateField HeaderText="Sel.">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibSeleccionarPE" runat="server" SkinID="imgCheck" CommandName="Seleccionar"
                                CommandArgument='<%# Eval("CodigoPortafolioSBS")&amp;","&amp;Eval("NumeroCuenta")&amp;","&amp;Eval("CodigoMercado")&amp;","&amp;CType(Container, GridViewRow).RowIndex %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" />
                    <asp:BoundField DataField="DescripcionTercero" HeaderText="Banco" />
                    <asp:BoundField DataField="DescripcionMoneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="ClaseDescripcion" HeaderText="Clase de Cuenta" />
                    <asp:BoundField DataField="NumeroCuenta" HeaderText="N&#250;mero de Cuenta" />
                    <asp:BoundField DataField="SaldoDisponibleInicial" HeaderText="Saldo Disponible Inicial" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="SaldoContableInicial" HeaderText="Saldo Contable Inicial" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="IngresosEstimados" HeaderText="Total Ingresos" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="EgresosEstimados" HeaderText="Total Egresos" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="SaldoDisponible" HeaderText="Saldo Disponible" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="SaldoContable" HeaderText="Saldo Contable" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoPortafolioSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoMercado" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CodigoClaseCuenta" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button ID="btnexcel" runat="server" Text="Generar Excel" />
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdNroCuenta" />
    <asp:HiddenField runat="server" ID="hdGrilla" />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger  ControlID="btnexcel"/>
        <asp:PostBackTrigger  ControlID="btnExportar"/>
        <asp:PostBackTrigger  ControlID="btnMovimientos"/>
    </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
