<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresos.aspx.vb" Inherits="Modulos_Tesoreria_Cuentasxcobrar_frmIngresos" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Ingresos</title>
    <script language="javascript">

        function ValidarDatos() {
            if (!document.getElementById("ddlMercado").selectedIndex > 0) {
                alert("Debe seleccionar el Mercado.");
                return false;
            }
            if (!document.getElementById("ddlIntermediario").selectedIndex > 0) {
                alert("Debe seleccionar un intermediario.");
                return false;
            }
            if (!document.getElementById("ddlOperacion").selectedIndex > 0) {
                alert("Debe seleccionar una operación.");
                return false;
            }
            if (!document.getElementById("ddlMoneda").selectedIndex > 0) {
                alert("Debe seleccionar una moneda.");
                return false;
            }
            if (document.getElementById("txtReferencia").value == '') {
                alert("Debe ingresar la Descripción.");
                return false;
            }
            if (!document.getElementById("txtImporte").value > 0) {
                alert("Debe especificar el importe.");
                return false;
            }
            if (document.getElementById("tbFechaOperac").value == '') {
                alert("Debe seleccionar una fecha de operación.");
                return false;
            }
            if (document.getElementById("txtFechaVencimiento").value == '') {
                alert("Debe seleccionar una fecha de vencimiento.");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header>
            <h2>
                Ingresos Cuentas por Cobrar
            </h2>
        </header>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMercado" runat="server" Width="220px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="220px" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Intermediario</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlIntermediario" runat="server" Width="250px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlMoneda" runat="server" Width="220px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlOperacion" runat="server" Width="250px" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripción</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtReferencia" runat="server" Width="220px" style="text-transform:uppercase"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Importe</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtImporte" runat="server" CssClass="Numbox-7"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Operación</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="tbFechaOperac" runat="server" ReadOnly="true" Width="90px"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Vencimiento</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaVencimiento" SkinID="Date" />
                                <span class="add-on" id="imgFecha"><i class="awe-calendar"></i></span>
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
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>
