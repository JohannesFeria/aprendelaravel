<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSwapDivisas.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmSwapDivisas" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>SWAP DIVISAS</title>
    <script type="text/javascript">
        function Salida(){
            var strMensaje = "";
            var strAccion = "";
            strAccion = document.getElementById("hdMensaje").value
            if (strAccion != "") {
                if (document.getElementById("ddlFondo").value != 'MULTIFONDO') {strMensaje = "¿Desea cancelar " + strAccion + " de la Orden de SWAP Divisas?"}
                else { strMensaje = "¿Desea cancelar " + strAccion + " de Orden de SWAP Divisas?" }
                    if (strMensaje != "") {
                        confirmacion = confirm(strMensaje);
                        if (confirmacion == true) {location.href = "../../../frmDefault.aspx";}
                        return false;
                    }{ return true; }
            }else {location.href = "../../../frmDefault.aspx";}
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
        <asp:UpdatePanel ID="updatos" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <header>
                    <div class="row">
                        <div class="col-md-6"><h2><asp:label id="lblTitulo" text="SWAP DIVISAS" runat="server" /></h2></div>
                        <div class="col-md-6" style="text-align:right;"><h2><asp:label id="lblAccion" runat="server"></asp:label></h2></div>
                    </div>
                </header>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Portafolio</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Portafolio" ControlToValidate="ddlFondo" validationGroup="vgCampos" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Portafolio" ControlToValidate="ddlFondo" ValidationGroup="vgPortafolio" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8" style="text-align: right;">
                        <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                    </div>
                </div>
                <fieldset>
                    <legend>Seleccionar Operaciones</legend>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    <asp:ImageButton SkinID="imgCheck" ID="btnOper1" runat="server" />
                                    &nbsp;Primera Orden de Inversi&oacute;n
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlOperacion1" Width="220px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Primera Orden de Inversi&oacute;n" ControlToValidate="ddlOperacion1"
                                        ValidationGroup="vgCampos" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label runat="server" id="lblOrden1" class="col-sm-4 control-label"  Visible="False">Nro. Orden</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCodigoOrden1" Width="100px" Enabled="false" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">
                                    <asp:ImageButton SkinID="imgCheck" ID="btnOper2" runat="server" />
                                    Segunda Orden de Inversi&oacute;n</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlOperacion2" Width="220px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Segunda Orden de Inversi&oacute;n" ControlToValidate="ddlOperacion2"
                                        ValidationGroup="vgCampos" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4" >
                            <div class="form-group">
                                <label runat="server" id="lblOrden2"  Visible="False" class="col-sm-4 control-label">Nro. Orden</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCodigoOrden2" Width="100px" Enabled="false" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <br />
                <header>
                </header>
                <div class="row">
                    <div class="col-md-6">
                        <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" CausesValidation="false" />
                        <asp:Button Text="Modificar" runat="server" ID="btnModificar" CausesValidation="false" />
                        <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" CausesValidation="false" />
                        <asp:Button Text="Consultar" runat="server" ID="btnConsultar" CausesValidation="false" />
                    </div>
                    <div class="col-md-6" style="text-align: right;">
                        <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                        <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                        <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
                    </div>
                </div>
            </div>
            <input id="hddLoad" type="hidden" value="0" name="hddLoad" runat="server" />
            <input id="hdFechaOperacion" type="hidden" name="hdFechaOperacion" runat="server" />
            <input id="txtCodigoOrdenH1" type="hidden" name="txtCodigoOrdenH1" runat="server" />
            <input id="txtCodigoOrdenH2" type="hidden" name="txtCodigoOrdenH2" runat="server" />
            <input id="Hidden1" type="hidden" name="Hidden1" runat="server" />
            <input id="hdMensaje" type="hidden" name="hdMensaje" runat="server" />
            <input id="hdOper1" type="hidden" name="hdOper1" runat="server" />
            <input id="hdOper2" type="hidden" name="hdOper2" runat="server" />
            <asp:ValidationSummary ID="vsResumenError" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgPortafolio"></asp:ValidationSummary>
            <asp:ValidationSummary ID="vsResumenError2" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgCampos"></asp:ValidationSummary>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
