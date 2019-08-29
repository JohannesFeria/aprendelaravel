<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteServiciosBBH.aspx.vb"
    Inherits="Modulos_Valorizacion_y_Custodia_Custodia_frmReporteServiciosBBH" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reporte de Servicios BBH</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Reporte de Servicios BBH</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="120px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox ID="tbFechaInicio" runat="server" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox ID="tbFechaFin" runat="server" SkinID="Date" />
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
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Imprimir" ID="btnVista" runat="server" />
                <asp:Button Text="Salir" ID="btnSalir" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
