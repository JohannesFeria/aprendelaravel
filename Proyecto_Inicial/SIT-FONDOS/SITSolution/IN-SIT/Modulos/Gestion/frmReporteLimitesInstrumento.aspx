<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteLimitesInstrumento.aspx.vb"
    Inherits="Modulos_Gestion_frmReporteLimitesInstrumento" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reporte de control de límites por instrumento</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="container-fluid">
                    <header>
                        <div class="row">
                            <div class="col-md-6">
                                <h2>
                                    Reporte de control de límites por instrumento 
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
                                        Código Isin</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="tbCodigoIsin" Width="120px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Código SBS</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="tbCodigoSBS" Width="120px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Código Mnemónico</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="tbMnemonico" Width="120px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Tipo Renta</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Sinónimo</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="150px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Derivados</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList runat="server" ID="ddlDerivados" Width="150px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Fecha de Operación</label>
                                    <div class="col-sm-8">
                                        <div class="input-append date">
                                            <asp:TextBox runat="server" ID="tbFechaValoracion" SkinID="Date" />
                                            <span class="add-on"><i class="awe-calendar"></i></span>
                                        </div>
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
                            <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                            <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
                        </div>
                    </div>
                    <asp:Label runat="server" ID="lblTime" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
