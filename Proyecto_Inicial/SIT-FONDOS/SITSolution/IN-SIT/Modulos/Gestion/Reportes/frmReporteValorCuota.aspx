<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteValorCuota.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmReporteValorCuota" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <header>
                <h2>Reporte Valor Cuota</h2>
            </header>
            <fieldset>
                <legend></legend>

                <div class="row">

                    <asp:UpdatePanel ID="upPortafolio" runat="server">
                        <ContentTemplate>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">
                                        Portafolio</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="ddlPortafolio" runat="server" Width="140px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkSeriado" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Inicial</label>
                            <div class="col-sm-8">
                                <div class="input-append date">
                                    <asp:TextBox ID="txtFechaInicial" runat="server" SkinID="Date"></asp:TextBox>
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Final</label>
                            <div class="col-sm-8">
                                <div class="input-append date">
                                    <asp:TextBox ID="txtFechaFinal" runat="server" SkinID="Date"></asp:TextBox>
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" style="text-align: right;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Generar Excel" />
                    </div>
                </div>

                <asp:UpdatePanel ID="upChkSeriado" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label"></label>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkSeriado" runat="server" Text="Seriado" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chkSeriado" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>

            </fieldset>
        </div>
    </form>
</body>
</html>
