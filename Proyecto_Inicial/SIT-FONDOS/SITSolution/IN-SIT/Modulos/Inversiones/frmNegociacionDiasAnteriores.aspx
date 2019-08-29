<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmNegociacionDiasAnteriores.aspx.vb" Inherits="Modulos_Inversiones_frmNegociacionDiasAnteriores" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Negociación Días Anteriores</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server">Negociación Días Anteriores</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Operación</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                    <span id="img1" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Clase de Instrumento</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlOrdenInversion" runat="server" Width="260px" />                            
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Salir" runat="server" ID="btnsalir" />
        </div>
    </div>
    </form>
</body>
</html>