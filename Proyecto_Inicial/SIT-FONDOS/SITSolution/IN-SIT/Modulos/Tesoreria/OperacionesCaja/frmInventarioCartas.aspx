<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInventarioCartas.aspx.vb"
    Inherits="Modulos_Tesoreria_OperacionesCaja_frmInventarioCartas" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Stock</title>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }

        $(document).ready(function () {
            $("#ibImprimir").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Stock</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <header style="text-align: center;">
            <h5>
                Inventario de Cartas de Instrucción
            </h5>
        </header>
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgStockInicial">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="_Delete"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoInventario") %>'
                                AlternateText="Eliminar"></asp:ImageButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:ImageButton ID="ibAgregar" runat="server" SkinID="imgAdd" CommandName="_Add"
                                AlternateText="Agregar"></asp:ImageButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha">
                        <ItemTemplate>
                            <asp:Label ID="lbFecha" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fecha") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbFechaF" runat="server" SkinID="Date"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rango Inicial">
                        <ItemTemplate>
                            <asp:Label ID="lbRangoInicial" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RangoInicial") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbRangoInicialF" runat="server" MaxLength="12" Width="90"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rango Final">
                        <ItemTemplate>
                            <asp:Label ID="lbRangoFinal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RangoFinal") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbRangoFinalF" runat="server" MaxLength="12" Width="90"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
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
    </div>
    </form>
</body>
</html>
