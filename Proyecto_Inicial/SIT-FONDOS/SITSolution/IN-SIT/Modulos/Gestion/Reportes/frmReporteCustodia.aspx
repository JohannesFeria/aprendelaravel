<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteCustodia.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmReporteCustodia" %>

<!DOCTYPE html>
<html lang="es">
<head id="Head1" runat="server">
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

                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Reporte</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlReporte" runat="server" Width="140px">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Operación</label>
                            <div class="col-sm-8">
                                <div class="input-append date">
                                    <asp:TextBox ID="txtFechaOperacion" runat="server" SkinID="Date"></asp:TextBox>
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Final</label>
                            <div class="col-sm-8">
                                <div class="input-append date">
                                    <asp:TextBox ID="txtFechaFinal" runat="server" SkinID="Date"></asp:TextBox>
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </fieldset>
            <hr />

            <div class="row">
                <div class="col-md-6">
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button ID="btnGenerarExcel" runat="server" Text="Generar Excel" />
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" />
                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <%--<div class="row">
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6" style="text-align: right;">
                            <asp:Button ID="btnGenerarExcel" runat="server" Text="Generar Excel" />
                            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
                        </div>
                    </div>--%>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGenerarExcel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSalir" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>
