<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportesGestion.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmReportesGestion" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reportes de Gesti&oacute;n</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-12">
                    <h2>
                        Reporte de
                        <asp:Label Text="" ID="lNombreReporte" runat="server" />
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div id="divPrimeraFila" runat="server" class="row">
                <div class="col-md-4" id="divMercado" runat="server">
                    <div class="form-group">
                        <label id="lMercado" runat="server" class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divPortafolio" runat="server">
                    <div class="form-group">
                        <label id="lblPortafolio" runat="server" class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divInstrumento" runat="server">
                    <div class="form-group">
                        <label id="lblInstrumento" runat="server" class="col-sm-4 control-label">
                            Instrumentos</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlInstrumento" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" id="divFechaInicio" runat="server">
                    <div class="form-group">
                        <label id="lFechaInicio" runat="server" class="col-sm-4 control-label">
                            Fecha de Inicio</label>
                        <div class="col-sm-8">
                            <div id="ImgINI" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" id="divFechaFin" runat="server">
                    <div class="form-group">
                        <label id="lFechaFin" runat="server" class="col-sm-4 control-label">
                            Fecha Fin</label>
                        <div class="col-sm-8">
                            <div id="ImgFIN" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
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
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Imprimir" runat="server" ID="btnVista" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdReporte" />
    </form>
</body>
</html>
