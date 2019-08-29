<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGeneracionRistraContable.aspx.vb"
    Inherits="Modulos_Contabilidad_frmGeneracionRistraContable" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Cierre Contable</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Generación de Ristra Contable
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
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Operaci&oacute;n Desde
                        </label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacionDesde" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Fecha Proceso Desde"
                                ControlToValidate="tbFechaOperacionDesde" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Operaci&oacute;n Hasta</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacionHasta" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Fecha Proceso Hasta"
                                ControlToValidate="tbFechaOperacionHasta" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="divRuta" runat="server" class="row hidden">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Ruta Ristra
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbRuta" Width="280px" />
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
                <asp:Label Text="" runat="server" ID="lblError" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Consolidar VAX" runat="server" ID="btnConsolidarVAX" />
                <asp:Button Text="Generar Ristra" runat="server" ID="btnGeneraRistra" Visible="false" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <br />
    <asp:Literal Text="" runat="server" ID="ltrLog" />
    <asp:ValidationSummary ID="vsResumenError" runat="server" ShowMessageBox="True" ShowSummary="False"
        HeaderText="Los siguientes campos son obligatorios:"></asp:ValidationSummary>
    </form>
</body>
</html>
