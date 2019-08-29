<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCarteraForward.aspx.vb" Inherits="Modulos_Inversiones_frmCarteraForward" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>VencimientoForwardNoDelivery</title>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }
        $(document).ready(function () {
            $("#ibBuscar").click(function () {
                ShowProgress();
            });
            $("#ibExportaExcel").click(function () {
                ShowProgress();
            });
            $("#ibProcesar").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<div class="container-fluid">
        <header>
            <h2>Reporte Cartera Forward</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
            <asp:Panel ID="PNportafolio" runat="server" >
                    <div class="col-md-3">
                    <div class="form-group">
                        <label id="Label4" runat="server" class="col-sm-5 control-label" >Portafolio</label> 
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlportafolio" runat="server" Width = "200px"  />
                            </div>
                        </div>
                    </div>                       
                    </div>
                </asp:Panel>
                <div id="divFechaDsc1" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="lblFechaDsc1" runat="server" class="col-sm-5 control-label" />
                        <div class="col-sm-7">
                            <div id="divFechaValoracion1" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaValoracion" SkinID="Date" Width="150px" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>                       
                </div>                
            </div>
            <div class="row">
                <div id="divTipoMercado" runat="server" class="col-md-3 hidden">
                    <div class="form-group">
                        <label id="Label2" runat="server" class="col-sm-5 control-label">
                            &nbsp;</label><div class="col-sm-7">
                            <asp:RadioButtonList runat="server" ID="rblEscenario" RepeatDirection="Horizontal">
                                <asp:ListItem Value="REAL" Text="REAL" Selected="True" />
                                <asp:ListItem Value="ESTIMADO" Text="ESTIMADO" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
            </label>
        </fieldset>
        <br />
        <div id="divProgress" class="loading" style="text-align: center;">
            &nbsp;</div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row" style="text-align: right;">
            <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
    </form>
</body>
</html>