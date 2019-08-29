<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCierrePortafolio.aspx.vb" Inherits="Modulos_Parametria_frmCierrePortafolio" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Cierre de IDI</title>
    <script type="text/javascript">
        function MostrarPopup() {
            var vPorta = $('#dlPortafolio').val();
            var tFecha = $('#lblFecha').text();
            //alert(vPorta); alert(tFecha);
            return showModalDialog('frmVisorOrdenesFaltantes.aspx?pfondo=' + vPorta + '&pFecha=' + tFecha, '1200', '600', '');            
        }
        function MostrarConfirmacion(msj) {
            var fecha = ""
            fecha = $('#tbFechaInicio').val();
            if (fecha != "") {
                if (!confirm('Desea abrir el portafolio con la  fecha : ' + fecha))
                    return false;
                else {
                    if (msj == 'Feriado') {
                        if (!confirm('La Fecha Ingresada es ' + msj + '.¿Desea cerrar?'))
                            return false;
                    }
                    document.getElementById("<%= btnEjecutar.ClientID %>").click();
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header><h2>Cierre de IDI</h2></header>
        <br />
        <fieldset>
        <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Portafolio</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="dlPortafolio" runat="server" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Actual</label>
                        <asp:Label ID="lblFecha" runat="server" Width="70px" CssClass="col-sm-9 control-label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Nueva</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"></label>
                        <div class="col-sm-9" style="text-align:left;">
                            <asp:CheckBox ID="chkReproceso" runat="server" Text="Reproceso" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
        </fieldset>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <div id="divBtnEjecutar" runat="server">
                <asp:Button ID="btnEjecutar" runat="server" Text="Ejecutar" />
            </div>
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>

