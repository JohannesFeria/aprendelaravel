<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfaseReporteVLMidas.aspx.vb" Inherits="Modulos_Gestion_Archivos_Planos_frmInterfaseReporteVLMidas" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title></title>
</head>
<body>    
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Importa Reporte VL Midas</h2></header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Ruta</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="Myfile" runat="server" Width="577px" style="display:none;" />
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx"
                                class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar"
                                data-size="sm" style="width:300px;">
                            <asp:HiddenField ID="hfRutaDestino" runat="server" />
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
        <br />
        <div class="row">
            <asp:Label ID="msgError" runat="server" CssClass="stlPaginaTexto" Visible="False"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
