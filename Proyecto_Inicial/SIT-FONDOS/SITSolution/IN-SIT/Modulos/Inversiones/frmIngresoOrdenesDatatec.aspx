<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoOrdenesDatatec.aspx.vb"
    Inherits="Modulos_Inversiones_frmIngresoOrdenesDatatec" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title>Ingreso de Ordenes DATATEC</title>
    <script type="text/javascript">
        function KeyDownHandler() {
            if (event.keyCode != 13) {
                return false;
            }
        }
        function ValidaRuta() {
            d = document.Form1;
            var Longitud;

            if (d.iptRuta.value == '') {
                alertify.alert('No se ha especificado un archivo a cargar.');
                return false;
            }
            else {
                Longitud = d.iptRuta.value.length
                if (d.iptRuta.value.substring(Longitud - 3) == 'txt' || d.iptRuta.value.substring(Longitud - 3) == 'TXT') {
                    if (d.iptRuta.value.substring(1, 2) == '\\') {
                        if (window.confirm('Esta Seguro de Cargar el Archivo?')) {
                            d.hidRuta.value = d.iptRuta.value;
                            return true;
                        }
                        else
                            return false;
                    }
                    else {
                        alertify.alert('El Archivo Cargado tiene que tener una ruta de Red.');
                        return false;
                    }
                }
                else {
                    alertify.alert('El Archivo Cargado tiene que tener una Extension TXT.');
                    return false;
                }
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Ingreso de Ordenes DATATEC</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Ruta</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtRuta" runat="server" Width="624px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultado de la carga</legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <div class="col-sm-4">
                            <asp:Label ID="lbContador" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="grilla">
            <asp:GridView ID="dgLista" runat="server" SkinID="Grid">
                <Columns>
                    <asp:BoundField DataField="CodigoPreOrden" HeaderText="Nro Orden"></asp:BoundField>
                    <asp:BoundField DataField="CodigoISIN" HeaderText="C&#243;digo ISIN"></asp:BoundField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio"></asp:BoundField>
                    <asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operacion"></asp:BoundField>
                    <asp:BoundField DataField="MontoNominalOperacion" HeaderText="Monto Nominal"></asp:BoundField>
                    <asp:BoundField DataField="MontoNominalOrdenado" HeaderText="Valor a Negociar"></asp:BoundField>
                    <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Vencimiento"></asp:BoundField>
                    <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operaci&#243;n"></asp:BoundField>
                    <asp:BoundField DataField="HoraOperacion" HeaderText="Hora Operaci&#243;n"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button Text="Procesar" runat="server" ID="ibProcesar" />
                <asp:Button Text="Salir" runat="server" ID="ibSalir" />
            </div>
        </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
