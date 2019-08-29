<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportePortafolio.aspx.vb" Inherits="Modulos_Inversiones_Reportes_frmReportePortafolio" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Reportes de Portafolio</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">        
        <header><h2>Resumen de Portafolio</h2></header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="220px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Operacion</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate> 
            </asp:UpdatePanel> 
        </fieldset>
        <hr />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte" />
        </div>
    </div>
    </form>
</body>
</html>