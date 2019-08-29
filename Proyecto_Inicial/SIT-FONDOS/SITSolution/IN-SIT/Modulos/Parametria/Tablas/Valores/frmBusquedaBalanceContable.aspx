<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaBalanceContable.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmBusquedaBalanceContable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Balance Contable</title>
    <script type="text/javascript">
        function showPopup() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '1200', '600', '');            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Balance Contable</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Emisor</label>
                <div class="col-sm-9">
                    <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoEmisor" Width="160px" />
                                <asp:LinkButton runat="server" ID="lkbBuscar" OnClientClick="return showPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situación</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlSituacion" runat="server" Width="158px" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </div>
    </div>
    </fieldset>
    <br />
    <fieldset>
    <legend>Resultados de la Búsqueda</legend>
    <div class="row">
        <asp:label id="lbContador" runat="server"></asp:label>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:GridView ID="dgLista" runat="server" SkinID="Grid" AutoGenerateColumns="False">           
            <Columns>
                <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit"
                            CommandName="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoEmisor") %>'>
                        </asp:ImageButton>
                        <asp:HiddenField ID="hdCodigoEmisor" runat="server" Value="" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ControlStyle-Width="25px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete"
                            CommandName="Eliminar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoEmisor") %>'>
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DescripcionEmisor" HeaderText="Emisor"></asp:BoundField>
                <asp:BoundField DataField="TotalActivo" HeaderText="Total Activo" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField DataField="TotalPasivo" HeaderText="Total Pasivo" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField DataField="Patrimonio" HeaderText="Patrimonio" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField DataField="strFechaVigencia" HeaderText="Fecha Vigencia"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                <asp:BoundField DataField="CodigoEmisor" HeaderText="Emisor" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>        
    </div>
    <div>
        <asp:GridView ID="dgExportar" runat="server" AutoGenerateColumns="False" 
            Visible="False" CellPadding="4" ForeColor="#333333" GridLines="None" >            
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="DescripcionEmisor" HeaderText="Emisor"></asp:BoundField>
                <asp:BoundField DataField="TotalActivo" HeaderText="Total Activo" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField DataField="TotalPasivo" HeaderText="Total Pasivo" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField DataField="Patrimonio" HeaderText="Patrimonio" DataFormatString="{0:#,##0.0000000}"></asp:BoundField>
                <asp:BoundField DataField="strFechaVigencia" HeaderText="Fecha Vigencia"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="CodigoEmisor" HeaderText="Emisor"></asp:BoundField>
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
    </div>
    <br />
    <header></header>
    <div class="row" style="text-align:right;">
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
        <asp:Button ID="btnImportar" runat="server" Text="Importar" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
