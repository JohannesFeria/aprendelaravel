<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteOI_Estados.aspx.vb"
    Inherits="Modulos_Inversiones_Reportes_Orden_de_Inversion_frmReporteOI_Estados" %>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte de Ordenes de Inversión</title>
</head>
<body>
    <form id="form1" runat="server" class="Form-Horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="Container-fluid">
        <header>
            <h2>
                Reporte de Ordenes de Inversión</h2>
        </header>
        <br />
        <fieldset>
            <legend>Filtros</legend>
            <div class="row">
                <div class="form-group">
                    <div class="col-md-6">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="115px">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="form-group">
                    <div class="col-md-6">
                        <label class="col-sm-4 control-label">
                            Fecha Operación Desde</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFecha" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <label class="col-sm-4 control-label">
                            Fecha Operación Hasta</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="form-group">
                    <div class="col-md-6">
                        <label class="col-sm-4 control-label">
                            Estado</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlEstado" runat="server" Width="250px" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="col-md-6">
                                <label id="lblExceso" runat="server" class="col-sm-4 control-label hidden">
                                    Exceso</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlAprobados" runat="server" Width="115px" Visible="False">
                                        <asp:ListItem Value="E-ABK">Brokers</asp:ListItem>
                                        <asp:ListItem Value="E-APR">Limites</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlEstado" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="form-group">
                    <div class="col-md-6">
                        <label class="col-sm-4 control-label">
                        </label>
                        <div class="col-sm-8">
                            <asp:CheckBox ID="chkRegulaSBS" runat="server" Text="Regularización SBS"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <label class="col-sm-4 control-label">
                        </label>
                        <div class="col-sm-8">
                            <asp:CheckBox ID="chkLiqAntFon" runat="server" Text="Liquidadas antes de Apertura">
                            </asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>
