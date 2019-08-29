<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresos.aspx.vb" Inherits="Modulos_Tesoreria_Cuentasxpagar_frmIngresos" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Ingresos Cuentas por Pagar </title>
    <script type="text/javascript">
        function ValidarDatos() {
            if (!document.getElementById("ddlMercado").selectedIndex > 0) {
                alertify.alert("Debe seleccionar el Mercado.");
                return false;
            }
            if (!document.getElementById("ddlIntermediario").selectedIndex > 0) {
                alertify.alert("Debe seleccionar un intermediario.");
                return false;
            }
            if (!document.getElementById("ddlOperacion").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una operación.");
                return false;
            }
            if (!document.getElementById("ddlMoneda").selectedIndex > 0) {
                alertify.alert("Debe seleccionar una moneda.");
                return false;
            }
            if (document.getElementById("txtReferencia").value == '') {
                alertify.alert("Debe ingresar la Descripción.");
                return false;
            }
            if (!document.getElementById("txtImporte").value > 0) {
                alertify.alert("Debe especificar el importe.");
                return false;
            }
            if (document.getElementById("txtFechaCobro").value == '') {
                alertify.alert("Debe seleccionar una fecha de vencimiento.");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />

    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Ingresos Cuentas por Pagar
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Intermediario</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlIntermediario" Width="280px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="280px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="280px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtReferencia" CssClass="mayusculas" Width="190px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Importe</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtImporte" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblFechaOperacion" SkinID="Date" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Vencimiento</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaCobro" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Salir" runat="server" ID="btnSalir" />
        </div>
    </div>
    </form>
</body>
</html>
