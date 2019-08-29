<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteCXC.aspx.vb" Inherits="Modulos_ValorCuota_frmReporteCXC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte de CXC Venta/Compra de Titulos</title>
    <script type="text/javascript">
        function validarPantalla() {
           if ($("#tbFechaInicio").val() == "") {
                alertify.alert("Seleccionar Fecha operacion.");
                return false;
            }
        }
    </script>
</head>
<body >
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header><h2>Reporte de CXC Venta/Compra de T&iacute;tulos</h2></header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="220px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddloperacion" runat="server" AutoPostBack="True" Width="220px" >
                                <asp:ListItem Text="Ambos" Value="0" />
                                <asp:ListItem Text ="Venta" Value ="1"  />
                                <asp:ListItem Text ="Compra" Value ="2"  />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5"></div>
                <div class="col-md-2">
                    <div style="text-align: right;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClientClick="javascript: return validarPantalla();"/>
                    </div>
                </div>
            </div>
            </ContentTemplate> 
            </asp:UpdatePanel> 
        </fieldset>
        <br />
        <asp:UpdatePanel ID="udpReporte" runat="server">
        <ContentTemplate>
            <div class="grilla">    
                    <asp:GridView runat="server" ID="dgReporteCxC" AutoGenerateColumns="false" SkinID="Grid">
                    <Columns>
                        <asp:BoundField DataField="Portafolio" HeaderText="Fondo"/>
                        <asp:BoundField DataField="CODIGOORDEN" HeaderText="C&#243;digo de Orden"/>
                        <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo de Operación"/>
                        <asp:BoundField DataField="CodigoNemonico" HeaderText="Nemonico"/>
                        <asp:BoundField DataField="TipoInstrumento" HeaderText="Tipo Instrumento"/>
                        <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha de Operaci&#243;n"/>
                        <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha de Liquidación"/>
                        <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                        <asp:BoundField DataField="CantidadOrdenado" HeaderText="Cantidad" DataFormatString="{0:#,##0.0000000}" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:#,##0.0000000}" />
                        <asp:BoundField DataField="MontoNetoOperaciones" HeaderText="Monto Neto" DataFormatString="{0:#,##0.0000000}"/>
                        <asp:BoundField DataField="ValorPrimario" HeaderText="Tipo de Cambio"  DataFormatString="{0:#,##0.0000000}"/>
                        <asp:BoundField DataField="MontoSoles" HeaderText="Monto en Soles" DataFormatString="{0:#,##0.0000000}"/>
                        <asp:BoundField DataField="MontoMonedaFondo" HeaderText="Monto en Fondo" DataFormatString="{0:#,##0.0000000}"/>
                    </Columns>
                </asp:GridView>            
            </div>
        </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID ="btnBuscar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br /><hr />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte"  Height="26px" OnClientClick="javascript: return validarPantalla();"/>
            <asp:Button ID="btnCancelar" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>