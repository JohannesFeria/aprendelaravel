<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteLimite.aspx.vb" Inherits="Modulos_Gestion_Reportes_frmReporteLimite" %>

<!DOCTYPE html>
<html lang="es">
<head id="Head1" runat="server">
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <header>
                <h2>Reporte Límites</h2>
            </header>
            <fieldset>
                <legend></legend>

                <div class="row">
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
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Inicial</label>
                            <div class="col-sm-8">
                                <div class="input-append date">
                                    <asp:TextBox ID="txtFecha" runat="server" SkinID="Date"></asp:TextBox>
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
                        <asp:Button ID="btnExportar" runat="server" Text="Generar Excel" />
                    </div>
                </div>

            </fieldset>
        </div>
    </form>
</body>
</html>
