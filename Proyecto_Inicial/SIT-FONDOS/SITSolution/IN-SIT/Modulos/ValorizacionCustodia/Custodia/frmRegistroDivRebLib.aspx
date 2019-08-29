<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRegistroDivRebLib.aspx.vb" Inherits="Modulos_ValorizacionCustodia_Custodia_frmRegistroDivRebLib" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro de Dividendos, Rebates y Liberadas</title>
    <script type="text/javascript">
        function showModal() {
            var isin = $('#txtISIN').val();
            var sbs = $('#txtSBS').val();
            var mnemonico = $('#txtMnemonico').val();
            return showModalDialog('frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', '');
        }

        function limpiar() {
            $('#txtISIN').val('');
            $('#txtSBS').val('');
            $('#txtMnemonico').val('');
            $('#txtDescripcion').val('');
            $('#txtMoneda').val('');
            return false;
        }        
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
<%--    <asp:UpdatePanel ID="updatos" runat="server" UpdateMode ="Conditional">
    <ContentTemplate>--%>
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Registro de Dividendos, Rebates y Liberadas</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo ISIN</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtISIN" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSBS" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" />
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModal()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                <asp:LinkButton CausesValidation="false" runat="server" ID="lbkLimpiarControl" OnClientClick="return limpiar()"><span class="add-on"><i class="awe-remove"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="Mnem&oacute;nico" ControlToValidate="txtMnemonico" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group" style="padding-left: 5px;">
                        <label class="col-sm-2 control-label">Descripci&oacute;n</label>
                        <div class="col-sm-8"><asp:TextBox runat="server" ID="txtDescripcion" Width="350px" /></div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnConsultar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" ID="dgLista" SkinID="Grid">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" SkinID="imgCheck" OnCommand="SeleccionarSBS"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoSBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Identificador") & "," & CType(Container, GridViewRow).RowIndex %>'
                                CausesValidation="False"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Acciones" HeaderText="Acciones" />
                    <asp:BoundField DataField="Factor" HeaderText="Factor" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="Fecha de Corte" HeaderText="Fecha de Corte" />
                    <asp:BoundField DataField="Fecha de Entrega" HeaderText="Fecha de Entrega" />
                    <asp:BoundField DataField="Beneficio" HeaderText="Beneficio" />
                    <asp:BoundField Visible="False" DataField="CodigoSBS" HeaderText="CodigoSBS" />
                    <asp:BoundField Visible="False" DataField="Identificador" HeaderText="Identificador" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
                    <asp:BoundField Visible="False" DataField="CodigoMoneda" HeaderText="CodigoMoneda" />
                    <asp:BoundField Visible="False" DataField="CodigoPortafolioSBS" HeaderText="CodigoPortafolioSBS" />
                </Columns>
            </asp:GridView>
        </div>
    
    <fieldset>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4">
                    Tipo de Distribuci&oacute;n</label>
                <div class="col-sm-8">
                    <asp:RadioButtonList ID="rdbOpt" runat="server" AutoPostBack = "true">
                        <asp:ListItem Value="Dividendo">Dividendo</asp:ListItem>
                        <asp:ListItem Value="Rebate">Rebate</asp:ListItem>
                        <asp:ListItem Value="Liberada">Liberada</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-4">
                    Portafolio</label>
                <div class="col-sm-8">
                    <asp:DropDownList runat="server" ID="ddlPortafolio" Width="120px" AutoPostBack="true" />
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-4">
                    Moneda</label>
                <div class="col-sm-8">
                    <asp:DropDownList runat="server" ID="dlMoneda" Width="120px" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-4">
                    Factor</label>
                <div class="col-sm-8">
                    <asp:TextBox runat="server" ID="txtFactor" Width="150px" CssClass ="Numbox-7_22_2"/>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-4">
                    Fecha Corte</label>
                <div class="col-sm-8">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaCorte" SkinID="Date" />
                        <span runat="server" id="date1" class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-4">
                    Fecha Entrega</label>
                <div class="col-sm-8">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaEntrega" SkinID="Date" />
                        <span runat="server" id="date2" class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label class="col-sm-4">Fecha de Operación</label>
                <div class="col-sm-8">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFechaIDI" SkinID="Date" />
                        <span runat="server" id="date3" class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row">
        <div class="col-md-12" style="text-align: right;">
            <asp:Button ID="btnImportar" Text="Importar" runat="server" 
                CausesValidation="False" />
            <asp:Button ID="btnIngresar" Text="Ingresar" runat="server" />
            <asp:Button ID="btnModificar" Text="Modificar" runat="server" />
            <asp:Button ID="btnEliminar" Text="Eliminar" runat="server" 
                style="width: 69px" />
            <asp:Button ID="btnAceptar" Text="Aceptar" runat="server" />
            <asp:Button ID="btnCancelar" Text="Cancelar" runat="server" CausesValidation="false" />
            <asp:Button ID="btnImprimir" Text="nImprimir" runat="server" />
            <asp:Button ID="btnSalir" Text="Salir" runat="server" CausesValidation="false" />
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </div>
<%--    </ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
