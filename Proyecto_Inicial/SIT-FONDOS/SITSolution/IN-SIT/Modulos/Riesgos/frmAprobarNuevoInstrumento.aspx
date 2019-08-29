<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAprobarNuevoInstrumento.aspx.vb" Inherits="Modulos_Riesgos_frmAprobarNuevoInstrumento" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Aprobación de Nuevos Instrumentos</title>
    <script type="text/javascript">
        function ShowPopup(pValor) {
            $('#hdTipoBusqueda').val(pValor);
            return showModalDialog('../Modulos/Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Aprobación de Nuevos Instrumentos</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código ISIN</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbCodigoIsin" runat="server" MaxLength="12" Width="120px" cssClass="mayusculas"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código SBS</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbCodigoSBS" runat="server" MaxLength="12" Width="120px" cssClass="mayusculas"></asp:textbox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Mnemónico</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbMnemonico" runat="server" MaxLength="15" Width="120px" cssClass="mayusculas"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Renta</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoRenta" runat="server" Width="120px"></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Emisor</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbEmisor" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbEmisor" runat="server" OnClientClick="return ShowPopup(1);"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Moneda</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlMoneda" runat="server" Width="120px"></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9"></div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:GridView ID="dgListaPA" runat="server" AutoGenerateColumns="False" SkinID="Grid">            
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibSeleccionarPA" runat="server" SkinID="imgCheck"
                            CommandName="Seleccionar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNemonico") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibDetallePA" runat="server" SkinID="imgSearch"
                            CommandName="DetallePA" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNemonico") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoISIN" HeaderText="C&#243;digo ISIN"></asp:BoundField>
                <asp:BoundField DataField="CodigoNemonico" HeaderText="C&#243;digo Mnemonico"></asp:BoundField>
                <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS"></asp:BoundField>
                <asp:BoundField DataField="Agrupacion" HeaderText="Agrupaci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Emisor" HeaderText="Emisor"></asp:BoundField>
                <asp:BoundField DataField="TipoRenta" HeaderText="Tipo Renta"></asp:BoundField>
                <asp:BoundField DataField="Mercado" HeaderText="Mercado"></asp:BoundField>
                <asp:BoundField DataField="Moneda" HeaderText="Moneda"></asp:BoundField>
                <asp:BoundField DataField="TipoTitulo" HeaderText="TipoT&#237;tulo"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <fieldset>
    <legend>Instrumentos Aprobados</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código ISIN</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbCodigoIsin2" runat="server" MaxLength="12" Width="120px" CssClass="mayusculas"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código SBS</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbCodigoSBS2" runat="server" MaxLength="12" Width="120px" CssClass="mayusculas"></asp:textbox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Mnemónico</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbMnemonico2" runat="server" MaxLength="15" Width="120px" CssClass="mayusculas"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Renta</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlTipoRenta2" runat="server" Width="120px"></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Emisor</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbEmisor2" CssClass="input-medium" />
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return ShowPopup(2);"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Moneda</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlMoneda2" runat="server" Width="120px"></asp:dropdownlist>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9"></div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnBuscar2" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:GridView ID="dgListaIA" runat="server" AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibSeleccionarIA" runat="server" SkinID="imgCheck"
                            CommandName="Seleccionar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNemonico") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibDetalleIA" runat="server" SkinID="imgSearch"
                            CommandName="DetalleIA" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoNemonico") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoISIN" HeaderText="C&#243;digo ISIN"></asp:BoundField>
                <asp:BoundField DataField="CodigoNemonico" HeaderText="C&#243;digo Mnemonico"></asp:BoundField>
                <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS"></asp:BoundField>
                <asp:BoundField DataField="Agrupacion" HeaderText="Agrupaci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="Emisor" HeaderText="Emisor"></asp:BoundField>
                <asp:BoundField DataField="TipoRenta" HeaderText="Tipo Renta"></asp:BoundField>
                <asp:BoundField DataField="Mercado" HeaderText="Mercado"></asp:BoundField>
                <asp:BoundField DataField="Moneda" HeaderText="Moneda"></asp:BoundField>
                <asp:BoundField DataField="TipoTitulo" HeaderText="TipoT&#237;tulo"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <header></header>
    <div style="display: none;">
        <asp:textbox id="txtCodPA" runat="server"></asp:textbox>
        <asp:textbox id="txtCodIA" runat="server"></asp:textbox>
    </div>
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
        <asp:Button ID="btnDesaprobar" runat="server" Text="Desaprobar" />
        <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    <asp:HiddenField ID="hdTipoBusqueda" runat="server" />
    </div>
    </form>
</body>
</html>
