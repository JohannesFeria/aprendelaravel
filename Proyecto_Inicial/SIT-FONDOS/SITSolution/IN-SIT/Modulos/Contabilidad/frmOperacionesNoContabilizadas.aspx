<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOperacionesNoContabilizadas.aspx.vb"
    Inherits="Modulos_Contabilidad_frmOperacionesNoContabilizadas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Operaciones No Contabilizadas</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Operaciones No Contabilizadas
                    </h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" AutoPostBack="true"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Proceso</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaProceso" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha Proceso" ControlToValidate="tbFechaProceso"
                                runat="server" />
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
                <asp:Button Text="Salir" runat="server" ID="btnTerminar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:ValidationSummary ID="vsResumenError" runat="server" ShowMessageBox="True" ShowSummary="False" HeaderText="Los siguientes campos son obligatorios:">
    </asp:ValidationSummary>
    </form>
</body>
</html>
