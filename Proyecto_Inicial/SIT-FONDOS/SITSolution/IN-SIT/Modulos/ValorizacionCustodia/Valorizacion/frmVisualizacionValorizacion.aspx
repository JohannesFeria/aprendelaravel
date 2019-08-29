<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVisualizacionValorizacion.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Valorizacion_frmVisualizacionValorizacion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Visualizacion Valorizacion</title>
    <link href="../../../App_Themes/css/jquery/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../../App_Themes/js/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="../../../App_Themes/js/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">
     var currentTab = 0;
     $(document).ready(function () {

           $("#tabs").tabs({
             select: function (e, i) {
                 currentTab = i.index;
             }
         });
     });

     function Cerrar() {
         window.close();
     }
    </script>
</head>
<body>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdContadoPxq" runat="server" />
<div id="tabs">
  <ul>
    <li runat="server" id="li01"><a href="#tabs01">Diferencia PXQ</a></li>
    <li runat="server" id="li02"><a href="#tabs02">Interés Negativo</a></li>
    <li runat="server" id="li03"><a href="#tabs03">Variación G_P</a></li>
     <li runat="server" id="li04"><a href="#tabs04">Inversión Nula</a></li>
      <li runat="server" id="li05"><a href="#tabs05">Valorización Nula</a></li>
  </ul>


  <div id="tabs01" runat="server">
  <strong>Diferencia PXQ</strong>
  <br />
      <asp:GridView ID="gvPxq" runat="server"  SkinID="Grid" AutoGenerateColumns="False" OnPageIndexChanging="gvPxq_OnPaging">
        <Columns>
            <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
            <asp:BoundField DataField="TipoCodigoValor" HeaderText="Tipo código valor" />
            <asp:BoundField DataField="CodigoValor" HeaderText="Código valor " />
            <asp:BoundField DataField="Validador" HeaderText="Validador" />
            <asp:BoundField DataField="Diferencia PXQ" HeaderText="Diferencia PXQ" DataFormatString="{0:#,##0.00}" />
        </Columns>
      </asp:GridView>
      
 </div>


  <div id="tabs02" runat="server">
    <strong>Interés Negativo</strong>
     <br />

    <asp:GridView ID="gvInteresNegativo" runat="server" SkinID="Grid" AutoGenerateColumns="False" OnPageIndexChanging="gvInteresNegativo_OnPaging">
        <Columns>
            <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
            <asp:BoundField DataField="CodigoValor" HeaderText="Código valor " />
            <asp:BoundField DataField="InteresesGanado" HeaderText="Interes ganado" DataFormatString="{0:#,##0.00}" />
        </Columns>
      </asp:GridView>
  </div>


  <div id="tabs03" runat="server">
      <strong>Variación G_P</strong>
     <br />
<asp:GridView ID="gvVariacion" runat="server" SkinID="Grid" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
        <asp:BoundField DataField="Codigovalor" HeaderText="Código valor" />
        <asp:BoundField DataField="Ganancia_Perdida" HeaderText="Ganacia o perdida " DataFormatString="{0:#,##0.00}"/>
    </Columns>
      </asp:GridView>
  </div>


  <div id="tabs04" runat="server">
      <strong>Inversión Nula</strong>
     <br />
   <asp:GridView ID="gvInversionNula" runat="server" SkinID="Grid" AutoGenerateColumns="False">
       <Columns>
           <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
           <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
           <asp:BoundField DataField="Codigovalor" HeaderText="Código valor" />
           <asp:BoundField DataField="MontoInversion" HeaderText="Monto inversión" DataFormatString="{0:#,##0.00}" />
       </Columns>
      </asp:GridView>
  </div>


  <div id="tabs05" runat="server">
      <strong>Valorización Nula</strong>
     <br />
 <asp:GridView ID="gvValorizacionNula" runat="server" SkinID="Grid" AutoGenerateColumns="False">
     <Columns>
         <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
         <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
         <asp:BoundField DataField="CodigoValor" HeaderText="Código valor" />
         <asp:BoundField DataField="Valorizacion" HeaderText="Valorización" DataFormatString="{0:#,##0.00}" />
     </Columns>
      </asp:GridView>

  </div>

   <asp:Button ID="btnExportar" Text = "Exportar" CssClass="btn btn-integra" 
        runat="server" />

            <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClientClick="Cerrar();" />
</div>
    </form>
</body>
</html>
