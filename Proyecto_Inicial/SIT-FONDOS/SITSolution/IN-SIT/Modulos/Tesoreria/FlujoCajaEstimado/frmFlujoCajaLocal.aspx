<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmFlujoCajaLocal.aspx.vb"
    Inherits="Modulos_Tesoreria_FlujoCajaEstimado_frmFlujoCajaLocal" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Flujo de Caja Local</title>
    <script type="text/javascript">
        function ValidarFormulario() {
            if (!document.getElementById("ddlMoneda").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una moneda.");
                return false;
            }
            if (document.getElementById("tbFechaVctoIni").value == '') {
                alertify.alert("Debe seleccionar una fecha de inicio.");
                return false;
            }
            if (document.getElementById("tbFechaVctoFin").value == '') {
                alertify.alert("Debe seleccionar una fecha de fin.");
                return false;
            }
            return true
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
                        Flujo de Caja Local</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Portafolio</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" AutoPostBack="True"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Moneda</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="200px" AutoPostBack="True"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            <%--Banco--%></label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="150px" Visible = "False" AutoPostBack="True"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            <%--Clase Cuenta--%></label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlClaseCuenta" Width="150px" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Vencimiento Inicio</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVctoIni" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Vencimiento Fin</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVctoFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Periodo</label>
                        <div class="col-sm-7">
                            <asp:RadioButton Text="Días" runat="server" ID="rbtDias" GroupName="rbtPeriodo" />
                            <asp:RadioButton Text="Meses" runat="server" ID="rbtMeses" GroupName="rbtPeriodo" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                        Tipo de Flujo</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoFlujo" runat="server" Width="130px">
                                <asp:ListItem Value="N" Selected="True">POR OPERACIONES</asp:ListItem>
                                <asp:ListItem Value="S">POR DIVISAS</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div id="divProgress" class="loading" style="text-align: center;">
            Procesando...<br />
            <br />
            <img alt="" src="../../../App_Themes/img/icons/ajax-loader.gif" width="45px" height="45px" />
        </div>
        <br />
        <div style="BORDER-BOTTOM: #999999 0px solid; BORDER-LEFT: #999999 0px solid; WIDTH: 100%; BORDER-COLLAPSE: collapse; HEIGHT: 410px; OVERFLOW: auto; BORDER-TOP: #999999 0px solid; BORDER-RIGHT: #999999 0px solid">
            <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="15px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibMenu" runat="server" OnCommand="Modificar" ImageAlign="Middle"
                                Height="10px" Width="10px" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Formula") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Formula" HeaderText="Formula" ItemStyle-CssClass="hidden"
                        HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Clasificacion" HeaderText="Clasificacion" ItemStyle-CssClass="hidden"
                        HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="ValorOperacion" HeaderText="ValorOperacion" ItemStyle-CssClass="hidden"
                        HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha" />
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Descripci&#243;n" />
                    <asp:BoundField DataField="Operacion" HeaderText="Operaci&#243;n" />
                    <asp:BoundField DataField="SaldoAnterior" HeaderText="Saldo" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="IngresoValor" HeaderText="Ingreso" DataFormatString="{0:###,###,##0.00}"
                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="EgresoValor" HeaderText="Egreso" DataFormatString="{0:###,###,##0.00}"
                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Egreso" HeaderText="EgresoFlag" ItemStyle-CssClass="hidden"
                        HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="SaldoPosterior" HeaderText="Saldo Final" DataFormatString="{0:#,##0.00}"
                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
