<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCalculoRebates.aspx.vb" Inherits="Modulos_ValorizacionCustodia_DividendosRebatesyLiberadas_frmCalculoRebates" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

<head runat="server">
    <title>Reporte Cálculo Rebates</title>
    <script type="text/javascript">
            function showModal() {
                var mnemonico = $('#txtMnemonico').val();
                return showModalDialog('../Custodia/frmBuscarInstrumento.aspx?vMnemonico=' + mnemonico, '800', '600', '');             
            }

            function limpiar() {
                $('#txtMnemonico').val('');
                return false;
            }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Reporte de C&aacutelculo de Rebates</h2></header>
    <fieldset>
    <legend>Filtros</legend>

         <div class="row">
               <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            C&oacutedigo Mnem&oacutenico</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModal()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lbkLimpiarControl" OnClientClick="return limpiar()"><span class="add-on"><i class="awe-remove"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Mnem&oacute;nico"
                                ControlToValidate="txtMnemonico" runat="server" Text="(*)" />
                                <%--<asp:TextBox runat="server" ID="tbCodigoMnemonico" CssClass="input-medium" />
                                <asp:LinkButton ID="btnbuscar" runat="server"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>--%>
                                <asp:DropDownList ID="dllMoneda" runat="server" Visible="False">
                                </asp:DropDownList>                            
                        </div>
                    </div>
               </div>
         </div>
         <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Inicio</label>
                        <div class="col-sm-9">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Fecha Inicio"
                                ControlToValidate="tbFechaInicio" runat="server" Text="(*)" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Fecha Fin</label>
                            <div class="col-sm-9">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Fecha Fin"
                                ControlToValidate="tbFechaFin" runat="server" Text="(*)" />
                            </div>
                    </div>
               </div>
            </div>
    </fieldset> 

    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </div>
    </form>
</body>
</html>
