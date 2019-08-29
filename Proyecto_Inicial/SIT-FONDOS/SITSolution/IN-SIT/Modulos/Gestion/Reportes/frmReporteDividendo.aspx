<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteDividendo.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmReporteDividendo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reporte de Dividendos</title>
      <script type="text/javascript">
          function validarPantalla() {

              if ($("#ddlPortafolio").val() == "") {
                  alert("Seleccionar Portafolio.");
                  return false;
              }
              else if ($("#tbFechaIni").val() == "") {
                  alert("Seleccionar fecha de inicio para la consulta.");
                  return false;
              }
              else if ($("#tbFechaFin").val() == "") {
                  alert("Seleccionar fecha de final para la consulta.");
                  return false;
              }
          }
    </script>
</head>
<body >
    <form id="form1" runat="server">
 <div class="container-fluid">
 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Reporte de Dividendos</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos del Reporte</legend>
             <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Portafolio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlPortafolio" Width="200px" runat="server" AutoPostBack ="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Inicio</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaIni" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Final</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                        &nbsp&nbsp
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClientClick="javascript: return validarPantalla();"/>
                    </div>
                   
                </div>  
         
      
            </div>
                  </ContentTemplate> 
            </asp:UpdatePanel> 
        </fieldset>


          <br />
        <div class="row">
            <div class="col-md-12">
                <div class="Grilla">
                    <asp:UpdatePanel ID="udpReporte" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="dgReporte" AutoGenerateColumns="false" SkinID="Grid">
                            <Columns>
                                <asp:BoundField DataField="CODIGOORDEN" HeaderText="C&#243;digo de Orden"/>
                                <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo de Operación"/>
                                <asp:BoundField DataField="CodigoNemonico" HeaderText="Nemonico"/>
                                <asp:BoundField DataField="TipoInstrumento" HeaderText="Tipo Instrumento"/>
                                <asp:BoundField DataField="FechaOperacionFormato" HeaderText="Fecha de Operaci&#243;n"/>
                                <asp:BoundField DataField="FechaLiquidacionFormato" HeaderText="Fecha de Liquidación"/>
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                                <asp:BoundField DataField="CantidadOrdenado" HeaderText="Cantidad" DataFormatString="{0:#,##0.0000000}" />
                                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:#,##0.0000000}" />
                                <asp:BoundField DataField="MontoNetoOperaciones" HeaderText="Monto Neto" DataFormatString="{0:#,##0.0000000}"/>
                                <asp:BoundField DataField="ValorPrimario" HeaderText="Tipo de Cambio"  DataFormatString="{0:#,##0.0000000}"/>
                                <asp:BoundField DataField="MontoSoles" HeaderText="Monto en Soles" DataFormatString="{0:#,##0.0000000}"/>
                            </Columns>
                        </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID ="btnBuscar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <br /><hr />




        <div class="row">
            <br />
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" OnClientClick="javascript: return validarPantalla();" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
