<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaValoresInstrumento.aspx.vb" Inherits="Modulos_Inversiones_ConsultasPreOrden_frmBusquedaValoresInstrumento" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Inversiones Realizadas</title>
</head>
<body>
    <form id="form1" runat="server"  class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2><asp:Label ID="lblTitulo" runat="server">Búsqueda Valores</asp:Label></h2></div>
                <div class="col-md-6" style="text-align: right;"><h3><asp:Label ID="lblAccion" runat="server"></asp:Label></h3></div>
            </div>
        </header>
          <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <div class="col-md-7"></div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <asp:label id="lbContador" runat="server" />
                            </div>
                        </div>
                    </div>
            </div>
        </fieldset>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
            <asp:GridView ID="dgLista" runat="server" SkinID ="Grid">
                <Columns>
				    <asp:TemplateField HeaderText="Seleccionar">
					    <HeaderStyle Width="80px"></HeaderStyle>
					    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    <ItemTemplate>
						    <asp:ImageButton id=ImageButton1 runat="server" OnCommand="SeleccionarISIN" SkinID ="imgCheck"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoISIN")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoMnemonico")&amp;","&amp;DataBinder.Eval(Container, "DataItem.CodigoSBS")&amp;","&amp;DataBinder.Eval(Container, "DataItem.Descripcion") %>' CausesValidation="False">
						    </asp:ImageButton>
					    </ItemTemplate>
				    </asp:TemplateField>
				    <asp:BoundField DataField="CodigoISIN" HeaderText="C&#243;digo ISIN">
					    <ItemStyle HorizontalAlign="Left"></ItemStyle>
				    </asp:BoundField>
				    <asp:BoundField DataField="CodigoMnemonico" HeaderText="C&#243;digo Mnem&#243;nico">
					    <ItemStyle HorizontalAlign="Left"></ItemStyle>
				    </asp:BoundField>
				    <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS">
					    <ItemStyle HorizontalAlign="Left"></ItemStyle>
				    </asp:BoundField>
				    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n">
					    <ItemStyle HorizontalAlign="Left"></ItemStyle>
				    </asp:BoundField>
				    <asp:BoundField DataField="TipoInstrumento" HeaderText="Tipo de Instrumento">
					    <ItemStyle HorizontalAlign="Left"></ItemStyle>
				    </asp:BoundField>
			    </Columns>
            </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div class="row">
                <div class="col-md-12" align="rigth">
                    <div class="form-group">
                        <div class="col-sm-11" style="text-align: right;">
                            <asp:Button runat="server" ID="ibCancelar" Text="Salir" />
                        </div>
                    </div>
                </div>
            </div>
    </div>
    </form>
</body>
</html>
