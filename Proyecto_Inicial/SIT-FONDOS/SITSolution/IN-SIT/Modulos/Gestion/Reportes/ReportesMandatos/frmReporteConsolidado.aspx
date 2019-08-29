<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteConsolidado.aspx.vb" Inherits="Modulos_Gestion_Reportes_ReportesMandatos_frmReporteConsolidado" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <link href="../../../../App_Themes/css/bootstrap-multiselect.css" rel="stylesheet"
        type="text/css" />
    <script src="../../../../App_Themes/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lbPortafolio]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Reporte Consolidado</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos del Reporte</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Portafolio</label>
                        <div class="col-sm-10">
                            <%--<asp:DropDownList ID="ddlPortafolio" runat="server" />--%>
                            <asp:ListBox ID="lbPortafolio" runat="server" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Fecha Proceso</label>
                        <div class="col-sm-10">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaProceso" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <header>
        </header>
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
