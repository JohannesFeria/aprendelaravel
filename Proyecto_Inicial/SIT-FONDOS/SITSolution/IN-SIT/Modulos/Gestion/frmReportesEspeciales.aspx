<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportesEspeciales.aspx.vb" Inherits="Modulos_Gestion_frmReportesEspeciales" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reportes Generales</title>
</head>

<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2 runat="server" id="lblTitulo"></h2></div></div>
        </header>
        <fieldset>
            <legend>Datos Principales</legend>
            <div class="row">
            <asp:Panel ID="PNportafolio" runat="server" >
                    <div class="col-md-4">
                    <div class="form-group">
                        <label id="Label4" runat="server" class="col-sm-3 control-label" >Portafolio</label> 
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlportafolio" runat="server" Width = "200px"  AutoPostBack = "true" />
                            </div>
                        </div>
                    </div>                       
                    </div>
                </asp:Panel>
                <div id="divFechaDsc1" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="lblFechaDsc1" runat="server" class="col-sm-3 control-label" />
                        <div class="col-sm-7">
                            <div id="divFechaValoracion1" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaValoracion" SkinID="Date" Width="150px"  AutoPostBack="true"/>
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>                       
                </div>
                <div id="divFechaDsc2" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="lblFechaDsc2" runat="server" class="col-sm-5 control-label" />
                        <div class="col-sm-7">
                            <div id="divFechaValoracion2" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaValoracion2" SkinID="Date" Width="150px" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <div id="divRadioButtons" runat="server" class="hidden">
                                <asp:RadioButton ID="rbtTodos" Text="Todos" runat="server" GroupName="rbtPrecio" Checked="true" />
                                <asp:RadioButton ID="rbtNegociada" Text="Negociados" runat="server" GroupName="rbtPrecio" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divInstrumento" runat="server" class="col-md-6 hidden">
                    <div class="form-group">
                        <label id="lblTipoInstrumento" runat="server" class="col-sm-5 control-label" />
                        <div class="col-sm-7"><asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="190px" /></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="divTipoMercado" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="Label2" runat="server" class="col-sm-5 control-label">Tipo de Mercado</label>
                        <div class="col-sm-7">
                            <asp:RadioButtonList runat="server" ID="rblEscenario" RepeatDirection="Horizontal">
                                <asp:ListItem Value="REAL" Text="REAL" Selected="True" />
                                <asp:ListItem Value="ESTIMADO" Text="ESTIMADO" />
                            </asp:RadioButtonList>
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div id="divTipoRenta" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="Label1" runat="server" class="col-sm-5 control-label">Tipo Renta</label>
                        <div class="col-sm-7"><asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" /></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="divLabel3" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="Label3" runat="server" class="col-sm-3 control-label" />
                        <div class="col-sm-7"><asp:DropDownList runat="server" ID="Dropdownlist1" Width="150px" /></div>
                    </div>
                </div>
                <div id="divPeriodoDias" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="lbPeriodoDias" runat="server" class="col-sm-5 control-label" />
                        <div class="col-sm-7"><asp:TextBox runat="server" ID="tbPeriodoDias" Width="120px" /></div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
        <legend>Datos de Busqueda</legend>
            <asp:Panel ID="pnGrillaRentabilidad" runat="server" Visible ="true" >
                <div class="grilla">
                    <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode ="Conditional" >
                    <ContentTemplate>
                        <asp:GridView runat="server" SkinID="Grid" ID="dgLista"  >
                            <Columns>
                                <asp:BoundField DataField="Portafolio" HeaderText="Portafolio" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion"  />
                                <asp:BoundField DataField="Emision" HeaderText="Emision" />
                                <asp:BoundField DataField="Entidad" HeaderText="Entidad"  />
                                <asp:BoundField DataField="VPNHoy" HeaderText="VPNHoy" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="VPNAyer" HeaderText="VPNAyer"  DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="Compras" HeaderText="Compras" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="Ventas" HeaderText="Ventas" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="Vencimientos" HeaderText="Vencimientos" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="Amortizaciones" HeaderText="Amortizaciones" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="Intereses" HeaderText="Intereses" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="Dividendos" HeaderText="Dividendos" DataFormatString="{0:#,##0.00}" />
                                <asp:BoundField DataField="Rentabilidad" HeaderText="Rentabilidad" DataFormatString="{0:#,##0.00}" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    </Triggers>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar" Visible ="false" />
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hdborrar" type="hidden" />
    </form>
</body>
</html>