<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteComisionInversiones.aspx.vb" Inherits="Modulos_Tesoreria_Reportes_frmReporteComisionInversiones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reporte Comisiones - Caja Recaudo</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header><h2>Reporte de Comisiones - Inversiones</h2></header>
        <fieldset>
            <legend>Datos de Reporte</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" />
                                <span class="add-on" id="Span1"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaFin" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
         </fieldset>
        <hr />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte" ToolTip ="Puede imprimir el reporte sin necesidad de buscar."  />
            <asp:Button ID="btnCancelar" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>
