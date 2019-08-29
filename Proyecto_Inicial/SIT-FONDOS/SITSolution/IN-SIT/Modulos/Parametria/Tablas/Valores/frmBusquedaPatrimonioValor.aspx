<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaPatrimonioValor.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaPatrimonioValor" %>


<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Patrimonio Valor</title>
    <script type="text/javascript">
        function showPopupTipoInstrumento() {
            $('#hdTipoModal').val('INS');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=TipoInstrumento', '1200', '600', '');
        }
        function showPopupMnemonico() {
            $('#hdTipoModal').val('MNE');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonico', '1200', '600', '');
        }
        function showModalEmisor() {
            $('#hdTipoModal').val('EMI');
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Patrimonio Valor</h2></header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:RadioButtonList ID="rbTipoBusqueda" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                            <asp:ListItem Value="Emision" Selected="True">Emisión</asp:ListItem>
                            <asp:ListItem Value="Emisor">Emisior</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div id="divTipoInstrumento" runat="server" class="form-group">
                        <label class="col-sm-3 control-label">Tipo Instrumento</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtTipoInstrumento" />
                                <asp:LinkButton ID="lkbBuscarInstrumento" runat="server" OnClientClick="return showPopupTipoInstrumento();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div id="divEmisor" runat="server" class="form-group">
                        <label id="Label1" runat="server" class="col-sm-3 control-label">
                            Emisor</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbEmisor" Width="80px" ReadOnly="true" />
                                <asp:LinkButton ID="lbkModalEmisor" OnClientClick="return showModalEmisor()" runat="server">
                                    <span runat="server" id="imbEmisor" class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbEmisorDesc" Width="220px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <%--<div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Tipo Instrumento</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoTipoInstrumento" />
                                <asp:LinkButton ID="lkbBuscarInstrumento" runat="server" OnClientClick="return showPopupTipoInstrumento();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>--%>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div id="divNemonico" runat="server" class="form-group">
                        <label class="col-sm-3 control-label">Nemónico</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtCodigoNemonico" MaxLength="15" />
                                <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div id="divTipo" runat="server" class="form-group">
                        <label class="col-sm-3 control-label">Tipo</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlTipo" runat="server" />
                        </div>
                    </div>
                </div>
                <%--<div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Mnemonico</label>
                        <div class="col-sm-9">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoMnemonico" MaxLength="15" />
                                <asp:LinkButton ID="lkbBuscarMnemonico" runat="server" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>--%>
            </div>
            <div class="row">
                <%--<div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Mnemonico</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="205px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>--%>
                <div class="col-md-6"></div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <asp:Label ID="lbContador" runat="server"></asp:Label>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                    </asp:ImageButton>
                                    <asp:HiddenField ID="hdCodigoTipoInstrumento" runat="server" Value="<%# Bind('CodigoTipoInstrumento') %>" />
                                    <asp:HiddenField ID="hdCodigoMnemonico" runat="server" Value="<%# Bind('CodigoMnemonico') %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DescripcionTipoInstrumento" HeaderText="Tipo Instrumento">
                            </asp:BoundField>
                            <asp:BoundField DataField="DescripcionMnemonico" HeaderText="Mnemonico"></asp:BoundField>
                            <asp:BoundField DataField="Patrimonio" HeaderText="Patrimonio" DataFormatString="{0:#,##0.0000000}">
                            </asp:BoundField>
                            <asp:BoundField DataField="strFechaVigencia" HeaderText="Fecha Vigencia"></asp:BoundField>
                            <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="CodigoTipoInstrumento" HeaderText="Tipo Instrumento">
                            </asp:BoundField>
                            <asp:BoundField Visible="False" DataField="CodigoMnemonico" HeaderText="Mnemonico">
                            </asp:BoundField>
                            <asp:BoundField Visible="False" DataField="CodigoSBS" HeaderText="CodigoSBS"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="CodigoISIN" HeaderText="CodigoISIN"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvListaEmisor" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                    <Columns>
                        <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                    CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                    CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoTercero" HeaderText="CodigoTercero" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="CodigoEntidad" HeaderText="CodigoEntidad" />
                        <asp:BoundField DataField="DescripcionEmisor" HeaderText="Emisor" />
                        <asp:BoundField DataField="NombreValor" HeaderText="Tipo" />
                        <asp:BoundField DataField="TipoValor" HeaderText="Tipo" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:#,##0.0000000}" />
                        <asp:BoundField DataField="FechaString" HeaderText="Fecha" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--<div>
            <asp:GridView ID="dgExportar" runat="server" Visible="False" CellPadding="4" ForeColor="#333333"
                GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="DescripcionTipoInstrumento" HeaderText="Tipo Instrumento">
                    </asp:BoundField>
                    <asp:BoundField DataField="DescripcionMnemonico" HeaderText="Descripcion Mnemonico">
                    </asp:BoundField>
                    <asp:BoundField DataField="CodigoSBS" HeaderText="CodigoSBS"></asp:BoundField>
                    <asp:BoundField DataField="CodigoISIN" HeaderText="CodigoISIN"></asp:BoundField>
                    <asp:BoundField DataField="Patrimonio" HeaderText="Patrimonio" DataFormatString="{0:#,##0.0000000}">
                    </asp:BoundField>
                    <asp:BoundField DataField="strFechaVigencia" HeaderText="Fecha Vigencia"></asp:BoundField>
                    <asp:BoundField DataField="strFechaVigencia" HeaderText="Nueva Fecha de Vigencia">
                    </asp:BoundField>
                    <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                    <asp:BoundField Visible="False" DataField="CodigoTipoInstrumento" HeaderText="Tipo Instrumento">
                    </asp:BoundField>
                    <asp:BoundField Visible="True" DataField="CodigoMnemonico" HeaderText="Mnemonico">
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </div>--%>

        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
            <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
            <asp:Button ID="btnImportar" runat="server" Text="Importar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
            <asp:HiddenField ID="hdTipoModal" runat="server" />
        </div>
        <br />
    </div>
    </form>
</body>
</html>
