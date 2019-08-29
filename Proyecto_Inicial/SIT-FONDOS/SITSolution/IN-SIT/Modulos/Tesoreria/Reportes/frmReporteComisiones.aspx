<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteComisiones.aspx.vb" Inherits="Modulos_Tesoreria_Reportes_frmReporteComisiones" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte Comisiones - Caja Recaudo</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header><h2>Reporte Comisiones - Caja Recaudo</h2></header>
        <fieldset>
            <legend>Datos de Reporte</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="220px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on" id="Span1"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
         </fieldset>
          <br />
        <fieldset>
        <legend>Datos de Busqueda</legend>
            <div class="grilla">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode ="Conditional" >
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista"  >
                        <Columns>
                            <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha"  />
                            <asp:BoundField DataField="Banco" HeaderText="Banco" />
                            <asp:BoundField DataField="Operacion" HeaderText="Operacion"  />
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda"  />
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="NumeroCuenta" HeaderText="NumeroCuenta"  />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnVista" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <br /><hr />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnVista" runat="server" Text="Buscar" ToolTip ="Puede imprimir el reporte sin necesidad de buscar." />
            <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte" ToolTip ="Puede imprimir el reporte sin necesidad de buscar."  />
            <asp:Button ID="btnCancelar" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>