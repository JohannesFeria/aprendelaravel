<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfaseLineasSettlement.aspx.vb" Inherits="Modulos_Gestion_Archivos_Planos_frmInterfaseLineasSettlement" %>

<!DOCTYPE html >

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

<head runat="server">
    <title>Interfase Settlement</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Importar para Líneas Settlement
            </h2>
        </header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Ruta</label>
                        <div class="col-sm-7">
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx" class="filestyle"
                                data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" size="150">
                        </div>
                    </div>
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
