<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSaldoNoAdministradoImportar.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmSaldoNoAdministradoImportar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Saldo no Administrado - Importar</title>
</head>
<body>
    <form id="form1" runat="server" class="forn-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <h2>Saldos no administrados - Importar</h2>    
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-1 control-label"> Mandato</label>
                        <div class="col-sm-5" >
                            <asp:DropDownList ID="ddlMandato" runat="server"></asp:DropDownList>
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
