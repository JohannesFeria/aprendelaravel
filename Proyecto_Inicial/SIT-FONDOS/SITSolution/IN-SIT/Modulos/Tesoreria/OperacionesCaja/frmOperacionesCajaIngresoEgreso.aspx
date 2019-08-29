<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmOperacionesCajaIngresoEgreso.aspx.vb"
    Inherits="Modulos_Tesoreria_OperacionesCaja_frmOperacionesCajaIngresoEgreso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts2")%>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos2") %>
<head runat="server">
    <title>Ingresos y Egresos Varios</title>
    <script>
        $(function () {
            $("#btnCierre").hide(); $("#btnReversa").hide();
            $(".confirm").confirm({
                text: "¿Desea cerrar la fecha actual y aperturar una nueva?",
                title: "¿Desea cerrar la fecha actual y aperturar una nueva?",
                confirm: function (button) {
                    $("#btnCierre").click()
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
            $(".confirmReversa").confirm({
                text: "¿Desea regresar a la fecha anterior del fondo?",
                title: "¿Desea regresar a la fecha anterior del fondo?",
                confirm: function (button) {
                    $("#btnReversa").click()
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
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <%--'INICIO | ZOLUXIONES | rcolonia | Aumento de Capital - Se implementa función javascript despues de ScriptManager para manejar eventos de updateprogress | 24/09/18 --%>
    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(
	        function () {
	            if (document.getElementById) {
	                var progress = document.getElementById('progressEspera');
	                var blur = document.getElementById('blurEspera');
	                var altoPage = document.documentElement.scrollHeight;
	                progress.style.width = '300px';
	                progress.style.height = '300px';
	                blur.style.height = '1200px';
	                //     progress.style.top = altoPage / 3 - progress.style.height.replace('px', '') / 2 + 'px';
	                progress.style.top = '300px'
	                progress.style.left = document.body.offsetWidth / 2 - progress.style.width.replace('px', '') / 2 + 'px';
	            }
	        }
            )

    </script>
    <%--'FIN | ZOLUXIONES | rcolonia | Aumento de Capital - Se implementa función javascript despues de ScriptManager para manejar eventos de updateprogress | 24/09/18 --%>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Ingresos y Egresos Varios</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales </legend>
            <asp:UpdatePanel ID="upFecha" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Clase Cuenta</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlClase" Width="200px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Portafolio</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" AutoPostBack="True" />
                                    <asp:DropDownList runat="server" ID="ddlPortafolioAumentoCapitalTemp" Visible="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Fecha</label>
                                <div class="col-sm-7">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="lblFechaPago" SkinID="Date" AutoPostBack="true" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCierre" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReversa" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="row">
                <div class="col-md-6" style="text-align: left;">
                    <button class="confirm btn btn-integra" type="button">
                        Cierre Fecha</button>
                    <asp:Button Text="" runat="server" ID="btnCierre" />
                    <button class="confirmReversa btn btn-integra" type="button">
                        Reversar Cierre</button>
                    <asp:Button Text="" runat="server" ID="btnReversa" />
                </div>
                <asp:UpdatePanel ID="upVisualiarBotones" runat="server">
                    <ContentTemplate>
                        <div class="col-md-6" style="text-align: right;">
                            <asp:Button Text="Obtener rescates" runat="server" ID="btnRescatarCuotas" UseSubmitBehavior="false" />
                            <asp:Button Text="Obtener comisiones/retenciones" runat="server" ID="btnObtenerComisiones"
                                UseSubmitBehavior="false" />
                            <asp:Button ID="btnObtenerSuscripciones" runat="server" Text="Obtener suscripciones"
                                UseSubmitBehavior="false" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlClase" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <br />
        <asp:UpdatePanel ID="upGrillaSaldo" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>Saldos /
                        <asp:Label ID="lblFondo" runat="server" Text="" />
                        /
                        <asp:Label ID="lblFecha" runat="server" Text="" />
                    </legend>
                    <div class="grilla">
                        <asp:GridView ID="GVSaldo" runat="server" SkinID="Grid_AllowPaging_NO">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnModificar" runat="server" SkinID="imgEdit" CommandName="Seleccionar"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NumeroCuenta") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CodigoEntidad" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="CodigoMercado" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="CodigoMoneda" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="Tercero" HeaderText="Banco" />
                                <asp:BoundField DataField="NumeroCuenta" HeaderText="Numero de Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda"></asp:BoundField>
                                <asp:BoundField DataField="SaldoDisponibleInicial" HeaderText="Saldo Inicial" DataFormatString="{0:#,##0.00}">
                                </asp:BoundField>
                                <asp:BoundField DataField="IngresosEstimados" HeaderText="Ingresos" DataFormatString="{0:#,##0.00}">
                                </asp:BoundField>
                                <asp:BoundField DataField="EgresosEstimados" HeaderText="Egresos" DataFormatString="{0:#,##0.00}">
                                </asp:BoundField>
                                <asp:BoundField DataField="SaldoFinal" HeaderText="Saldo Final / Libro Banco" DataFormatString="{0:#,##0.00}">
                                </asp:BoundField>
                                <asp:BoundField DataField="SaldoEstadoCTA" HeaderText="Saldo Estado CTA" DataFormatString="{0:#,##0.00}">
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlClase" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPortafolio" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="lblFechaPago" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnCierre" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upOperaciones" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnSaldo" runat="server" Visible="false">
                    <fieldset>
                        <legend>Operaciones /
                            <asp:Label ID="lblBanco" runat="server" Text="" />
                        </legend>
                        <div class="grilla">
                            <asp:GridView ID="GVOperaciones" runat="server" SkinID="GridFooter" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoOperacionCaja") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="btnAgregar" runat="server" CommandName="Add" AlternateText="Agregar"
                                                SkinID="imgAdd" />
                                        </FooterTemplate>
                                        <ItemStyle Width="25px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoOperacionCaja") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <ItemStyle Width="25px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CodigoOrden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"
                                        FooterStyle-CssClass="hidden" />
                                    <asp:TemplateField HeaderText="Codigo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodigoOperacion" runat="server" CssClass="stlPaginaTexto" onkeypress="return soloNumeros(event)"
                                                Text='<%# Eval("CodigoOperacionCaja") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblCodigoOperacionF" runat="server" CssClass="stlPaginaTexto" Text='' />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo de Operación">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddltipooperacion" runat="server" Width="200px" Enabled="True" />
                                            <asp:Label ID="lbltipooperacion" runat="server" CssClass="stlPaginaTexto" Text='<%# Eval("CodigoTipoOperacion") %>'
                                                Visible="False" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddltipooperacionF" runat="server" Width="200px" AutoPostBack="true"
                                                Enabled="True" OnSelectedIndexChanged="ddltipooperacionF_SelectedIndexChanged" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Operación">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlOperacion" runat="server" Width="300px" Enabled="True" />
                                            <asp:Label ID="lblOperacion" runat="server" CssClass="stlPaginaTexto" Text='<%# Eval("CodigoOperacion") %>'
                                                Visible="False" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlOperacionF" runat="server" Width="300px" AutoPostBack="true"
                                                Enabled="True" OnSelectedIndexChanged="ddlOperacionF_SelectedIndexChanged" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtImporte" Width="200px" runat="server" Text='<%# Eval("Importe") %>'
                                                onkeypress="return soloNumeros(event)" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtImporteF" Width="200px" runat="server" onkeypress="return soloNumeros(event)" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="InversionGenerada" HeaderText="Cerrado" />
                                    <asp:TemplateField HeaderText="Regularizar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnRegularizar" runat="server" SkinID="imgCheck" CommandName="Regularizar"
                                                Visible="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoOperacionCaja") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <div class="row">
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6" style="text-align: right;">
                            <asp:Button Text="Generar Inversiones" runat="server" ID="btnProceso" UseSubmitBehavior="false" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:HiddenField ID="HDNumeroCuenta" runat="server" />
                <asp:HiddenField ID="HDCodigoEntidad" runat="server" />
                <asp:HiddenField ID="HDCodigoMercado" runat="server" />
                <asp:HiddenField ID="HDCodigoMoneda" runat="server" />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div id="blurEspera" />
                        <div id="progressEspera">
                            <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProceso" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>