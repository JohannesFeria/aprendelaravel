<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGeneracionDeAsientosContables.aspx.vb"
    Inherits="Modulos_Contabilidad_frmGeneracionDeAsientosContables" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Generación de Asientos Contables</title>
    <script type="text/javascript">
        function ValidarDatos() {
            if (document.getElementById("tbFechaOperacion").value == ""){
                alertify.alert("Debe seleccionar una fecha de proceso.");
                return false;
            }
            return true;
        }
        //OT10783 - Creación de mensaje de confirmación de generación de asientos contables
        function GenerarAsientos_Confirmacion(msje) {
            if (!confirm(msje)) {
                return false;
            }
            document.getElementById('hdProcesar').value = '1';
            document.getElementById("<%= ibProcesar.ClientID %>").click();
        }
        //OT10783 - Fin
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Generar Asientos Contables</h2></div></div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="115px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Contable</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divRuta" runat="server" class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Ruta Archivo Interfaz</label>
                        <div class="col-sm-8"><asp:TextBox runat="server" ID="tbRuta" Width="600px" /></div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Selección de Lote</legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <asp:RadioButtonList ID="RbReportes" runat="server" AutoPostBack="True" Width="100%" CssClass="stlCajaTexto">
                                <asp:ListItem Value="CVI">Compra Venta de Inversiones</asp:ListItem>
                                <asp:ListItem Value="VC">Valorizacion de la Cartera</asp:ListItem>
                                <asp:ListItem Value="COM">Comision SAF</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <%--<asp:Button Text="Revertir archivo" runat="server" ID="ibRevertir" />--%>
            <asp:Button Text="Procesar" runat="server" ID="ibProcesar" />
            <asp:Button Text="Cierre Contable" runat="server" ID="btnCierre" />
            <asp:Button Text="Reversión Contable" runat="server" ID="btnRevertirCierre" />
            <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
            <asp:Button Text="Salir" runat="server" ID="btnSalir" />
        </div>
        <asp:HiddenField ID="hdProcesar" runat="server" />
        <div class="col-md-6"> <asp:Label Text="" runat="server" ID="lblLog" /> </div>
    </div>
    </form>
</body>
</html>