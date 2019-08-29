<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGeneracionCartasEmitidas.aspx.vb" Inherits="Modulos_Tesoreria_OperacionesCaja_frmGeneracionCartasEmitidas" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Generaci&oacute;n</title>
    <script type="text/javascript">
        function ValidarSeleccion(emision) {
            if (document.getElementById("hdNumeroCarta").value == '') {
                alertify.alert('Seleccione una carta.');
                return false;
            }
            else {
                if (emision && (!document.getElementById("rbtlEmision_0").checked && !document.getElementById("rbtlEmision_1").checked)) {
                    alertify.alert('Seleccione un Tipo de Emisión.');
                    return false;
                }
                else
                    return true;
            }
        }
        
        function ValidarDatos() {
            if (document.getElementById("tbFecha").value == "") {
                alertify.alert("Debe seleccionar una fecha.");
                return false;
            }
            return true;
        }

        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }

        $(document).ready(function () {
            $("#btnProcesar").click(function () {
                ShowProgress();
            });
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:UpdatePanel ID="up_Cuerpo" runat="server" UpdateMode= "Conditional">
    <ContentTemplate>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Generaci&oacute;n de Cartas de Instrucci&oacute;n</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Mercado</label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="ddlMercado" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio
                        </label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Intermediario
                        </label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="ddlIntermediario" Width="255px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Liquidaci&oacute;n</label>
                        <div class="col-sm-6">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFecha" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Banco
                        </label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:RadioButtonList ID="rdImpreso" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="I">Impresas</asp:ListItem>
                        <asp:ListItem Value="N" Selected="True">No Impresas</asp:ListItem>
                        <asp:ListItem Value="T">Todas</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="NroCarta" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:ButtonField Visible="False" Text="Select" CommandName="Select" />
                    <asp:TemplateField Visible="False" HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbSelect" runat="server" CommandName="Seleccionar" OnCommand="dgListaItemCommand"
                                CausesValidation="False" SkinID="imgCheck" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.NumeroCarta")%>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NumeroCarta" HeaderText="Nro. Carta" />
                    <asp:BoundField DataField="DescripcionBanco" HeaderText="Banco" />
                    <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio" />
                    <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Intermediario" />
                    <asp:BoundField DataField="TipoRenta" HeaderText="Tipo Renta" />
                    <asp:BoundField DataField="DescripcionPago" HeaderText="Forma de Pago" />
                    <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operacion" />
                    <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}" />
                    <asp:BoundField DataField="NumeroOrden" HeaderText="Codigo Orden" />
                    <asp:TemplateField HeaderText="Imprimir">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSeleccionarTodo" runat="server" AutoPostBack="True" Text="Imprimir"
                                OnCheckedChanged="cbSeleccionarTodo_CheckedChanged"></asp:CheckBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPrint" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Impreso">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkImpreso" Enabled="False" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div class="row">
            <div class="col-md-6">
                <asp:RadioButtonList ID="rbtlEmision" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Selected="True">Env&#237;o Manual</asp:ListItem>
                    <asp:ListItem Value="1">Env&#237;o Autom&#225;tica</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" Visible="false" />
                <asp:Button Text="Extornar" runat="server" ID="btnExtornar" />
                <asp:Button Text="Imprimir" runat="server" ID="btnVista" />
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdNumeroCarta" />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID ="btnVista" />
    </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>