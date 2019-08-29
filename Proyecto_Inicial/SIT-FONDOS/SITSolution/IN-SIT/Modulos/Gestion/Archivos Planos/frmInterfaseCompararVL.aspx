<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfaseCompararVL.aspx.vb" Inherits="Modulos_Gestion_Archivos_Planos_frmInterfaseCompararVL" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Comparar Reportes VL</h2></header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Fecha</label>
                        <div class="col-sm-4">
                           <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                <span class="add-on" id="imgFecha"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                        <label class="col-sm-4 control-label"></label>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />            
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>        
        
    </div>
    </form>
</body>
</html>
