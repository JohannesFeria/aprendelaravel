<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAumentoCapital.aspx.vb"
    Inherits="Modulos_Inversiones_frmAumentoCapital" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts2")%>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos2") %>
<head runat="server">
    <title>Aumento de Capital</title>
    <script type="text/javascript">
        function cargarBotones() {
            $(function () {
                //            $("#btnCalcular").hide();
                $("#btnGrabar").hide();
                $(".grabar").confirm({
                    text: "¿Desea " + document.getElementById("<%= lblAccion.ClientID %>").innerHTML + " la Distribución de Aumento de Capital?",
                    title: "¿Desea " + document.getElementById("<%= lblAccion.ClientID %>").innerHTML + " la Distribución de Aumento de Capital?",
                    confirm: function (button) {
                        $("#btnGrabar").click();
                    },
                    cancel: function (button) {
                        // nothing to do
                    },
                    confirmButton: "Aceptar",
                    cancelButton: "Cancelar",
                    post: true,

                    confirmButtonClass: "btn btn-integra",
                    cancelButtonClass: "btn btn-integra",
                    dialogClass: "modal-dialog modal-lg"
                });
            });
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
               <div class="row">
                <div class="col-md-6"><h2>Aumento de Capital</h2></div>
                 <div class="col-sm-6" style="text-align: right;">
                    <h3>
                        <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <asp:UpdatePanel ID="upFecha" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Fecha</label>
                                <div class="col-sm-4">
                                    <div class="input-append date" id="imgFechaPago" runat="server">
                                        <asp:TextBox runat="server" ID="txtFechaPago" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Portafolio</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-8" style="text-align: right;">
                            <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGrabar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <asp:UpdatePanel ID="upDatosOperacion" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>Datos de Operación </legend>
                    <%--           <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Importe</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtImporte" Width="200px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Total Intereses</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtTotalIC" Width="200px" CssClass="Numbox-7" Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Fecha Registro</label>
                                <div class="col-sm-4">
                                    <div class="input-append">
                                        <asp:TextBox runat="server" ID="txtFechaRegistro" SkinID="Date" Enabled="false" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-8" style="text-align: right;">
                            <asp:Button Text="Calcular" runat="server" ID="btnCalcular" />
                        </div>
                    </div>
                    <%--         <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Estado</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtEstado" Width="200px" Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>--%>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCalcular" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upGrillaSaldo" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>Distribución de Flujo /
                        <asp:Label ID="lblFondo" runat="server" Text="" />
                        /
                        <asp:Label ID="lblFecha" runat="server" Text="" />
                    </legend>
                    <div class="grilla" style="width: 500px">
                        <asp:GridView ID="gvDistribucion" runat="server" SkinID="Grid">
                            <Columns>
                                <asp:BoundField DataField="CodigoNemonico" HeaderText="Instrumento" />
                                <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha de Liquidación" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="InteresCorrido" HeaderText="Interés Corrido" ItemStyle-HorizontalAlign="Right"
                                    DataFormatString="{0:#,##0.0000000}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCalcular" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <div class="row">
            <div class="col-sm-6">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <%--       <asp:Button Text="Modificar" runat="server" ID="btnModificar" />--%>
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" />
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript" language="javascript">
                            Sys.Application.add_load(cargarBotones);
                        </script>
                        <button class="grabar btn btn-integra" type="button" id="btnPrevGrabar" runat="server">
                            Grabar</button>
                        <asp:Button Text="Salir" runat="server" ID="btnSalir" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button Text="" runat="server" ID="btnGrabar" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdHoraOperacion" runat="server" />
    </form>
</body>
</html>
