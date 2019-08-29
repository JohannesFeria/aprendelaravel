<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaDivRebatesLib.aspx.vb" Inherits="Modulos_ValorizacionCustodia_DividendosRebatesyLiberadas_frmConsultaDividendos" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

<head runat="server">
    <title>Consulta de Dividendos Rebates y Liberadas</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Consulta de Dividendos, Rebates y Liberadas</h2></header>
    <fieldset>
    <legend></legend>
           <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Portafolio</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlFondo" runat="server" Width="120px">
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
                            </div>
                    </div>
               </div>
           </div>
    </fieldset> 
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnGenerarReporte" runat="server" Text="Imprimir" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>

    </div>
    </form>
</body>
</html>
