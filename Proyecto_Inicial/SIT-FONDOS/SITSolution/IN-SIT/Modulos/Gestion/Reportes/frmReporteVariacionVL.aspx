<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteVariacionVL.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmReporteVariacionVL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title></title>
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
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Reporte  de variación VL </h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos del Reporte</legend>
             <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Fecha</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaIni" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
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
        <div class="row">
            <div class="col-md-12">
                <div class="Grilla">
                    <asp:UpdatePanel ID="udpReporte" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="dgReporte" AutoGenerateColumns="false" SkinID="Grid">
                            <Columns>
                                <asp:BoundField DataField="FondoIndador" HeaderText="Tipo"/>
                                <asp:BoundField DataField="Fondo" HeaderText="Fondo"/>
                                <asp:BoundField DataField="FechaFormato" HeaderText="Fecha"/>
                                <asp:BoundField DataField="CodigoValor" HeaderText="C&#243;digo Valor"/>
                                <asp:BoundField DataField="MontoNominal" HeaderText="Monto Nominal" DataFormatString="{0:#,##0.0000000}" />
                                 <asp:BoundField DataField="DescripcionTipoInstrumento" HeaderText="Descripci&#243;n TD"/>
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                                <asp:BoundField DataField="Signo" HeaderText="Signo"/>
                                <asp:BoundField DataField="Diferencia" HeaderText="Diferencia" DataFormatString="{0:#,##0.0000000}"/>
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
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>