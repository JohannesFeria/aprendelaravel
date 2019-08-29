<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRegistroDivRebLibImportar.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Custodia_frmRegistroDivRebLibImportar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Dividendos - Importar</title>
</head>
<body>
    <form id="form1" runat="server" class="forn-horizontal">
        <asp:ScriptManager ID="SMLocal" runat="server"></asp:ScriptManager>    
        <div class="container-fluid">
            <h2>Dividendos - Importar</h2>
            <fieldset>
                <legend></legend>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-1 control-label">Ruta</label>
                            <div class="col-sm-5">
                                <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx"
                                    class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar"
                                    data-size="sm">
                                <asp:HiddenField ID="hfRutaDestino" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>  
                <div class="row" style="text-align: center;">
                    <div id="divProgress" align="center" style="display: none;">
                        Procesando...<br />
                        <br />
                        <img src="../../../../App_Themes/img/icons/ajax-loader.gif" />
                    </div>
                </div>                
            </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" ID="dgLista" SkinID="Grid" 
                AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Identificador" HeaderText="Nro Fila" />
                    <asp:BoundField DataField="TipoDistribucion" HeaderText="Tipo de distribución" />
                    <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Codigo Portafolio" />
                    <asp:BoundField DataField="CodigoNemonico" HeaderText="Codigo Nemonico" />
                    <asp:BoundField DataField="CodigoISIN" HeaderText="Codigo ISIN" />
                    <asp:BoundField DataField="CodigoSBS" HeaderText="Codigo SBS" />
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Codigo Moneda" />
                    <asp:BoundField DataField="Unidades" HeaderText="Total Unidades" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="Factor" HeaderText="Factor" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="FechaCorte" HeaderText="Fecha de Corte" />
                    <asp:BoundField DataField="FechaIDI" HeaderText="Fecha IDI" />
                    <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha de Entrega" />
                    <asp:BoundField DataField="Situacion" HeaderText="Estado Carga" />
                    <asp:BoundField DataField="Mensaje" HeaderText="Mensaje Error" />
                </Columns>
            </asp:GridView>
        </div>
            <br />
            <header>
            </header>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
                <asp:Button ID="btnRetornar" runat="server" Text="Retornar" />
            </div>                              
        </div>
    </form>
</body>
</html>
