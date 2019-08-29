<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCarteraIndirectaImportar.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmCarteraIndirectaImportar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Cartera Indirecta - Importar</title>
</head>
<body>
    <form id="form1" runat="server" class="forn-horizontal">
        <asp:ScriptManager ID="SMLocal" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <h2>Cartera Indirecta - Importar</h2>
            <fieldset>
                <legend></legend>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-1 control-label">Fecha</label>
                            <div class="col-sm-5">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>        
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-1 control-label">Ruta</label>
                            <div class="col-sm-5">
                                <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx"
                                    class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar"
                                    data-size="sm">
                                <asp:HiddenField ID="hfRutaDestino" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>        
                <div class="row" style="text-align: center;">
                    <div id="divProgress" align="center" style="display: none;">
                        Procesando...<br />
                        <br />
                        <img src="../../../../App_Themes/img/icons/ajax-loader.gif" />
                    </div>
                </div>                
            </fieldset>
            <br />
            <header>
            </header>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
                <asp:Button ID="btnRetornar" runat="server" Text="Retornar" />
            </div>
        </div>
    </form>
</body>
</html>
