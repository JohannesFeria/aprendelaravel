<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoOrdenesEjecutadas.aspx.vb" Inherits="Modulos_Inversiones_frmExtornoOrdenesEjecutadas" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title>Extorno Ordenes Ejecutadas</title>
</head>
<body>
    <form id="form1" runat="server" method="post" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
        <div class="container-fluid">
            <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" runat="server">Extorno Ordenes Ejecutadas</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3><asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                </div>
            </div>
            </header>
            <br />
            <asp:UpdatePanel ID="upcab" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><asp:label id="lblFondo" runat="server">Fondo</asp:label></label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlFondoOE" runat="server"  AutoPostBack="True" Width="171px"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><asp:label id="Label1" runat="server">Nro. Orden</asp:label></label>
                        <div class="col-sm-8">
                            <asp:textbox id="txtNroOrdenOE" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label"><asp:label id="Label2" runat="server">Fecha de Operación</asp:label></label>
                        <div class="col-sm-6">
                            <div class="input-append">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" Width="100px" />
                                    <%--AutoPostBack="True"--%>
                                    <span id="imgFechaEmision" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                                </div> 
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <asp:Button ID="ibBuscarOE" runat="server" Text ="Buscar" />
                    </div>
                </div>
            </div>          
                <fieldset>
            <legend>Ordenes Ejecutadas</legend>
                <asp:GridView ID="dgListaOE" runat="server" SkinID="Grid">
                <Columns>
						<asp:TemplateField HeaderText="Seleccionar">
							<HeaderStyle Width="80px"></HeaderStyle>
							<ItemTemplate>
                                <asp:ImageButton ID="ibSeleccionarOE" runat="server" SkinID="imgCheck"  CommandName="Seleccionar"  />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operaci&#243;n">
							<HeaderStyle Width="80px"></HeaderStyle>
						</asp:BoundField>
                        <asp:BoundField DataField="CodigoPortafolio" HeaderText="CodigoPortafolio" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                            <HeaderStyle Width="100px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
						<asp:BoundField DataField="Fondo" HeaderText="Fondo">
							<HeaderStyle Width="100px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="NumeroTransaccion" HeaderText="Nro Orden">
							<HeaderStyle Width="100px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operacion">
							<HeaderStyle Width="80px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n">
							<HeaderStyle Width="150px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="Estado" HeaderText="Estado">
							<HeaderStyle Width="80px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="Moneda" HeaderText="Moneda">
							<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operaci&#243;n" DataFormatString="{0:#,##0.0000000}">
							<HeaderStyle Width="100px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField  HeaderStyle-CssClass="hidden" DataField="ISIN" HeaderText="ISIN">
							<ItemStyle CssClass ="hidden" HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField  HeaderStyle-CssClass="hidden" DataField="TipoOrden" HeaderText="TipoOrden">
							<ItemStyle CssClass="hidden" HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField  HeaderStyle-CssClass="hidden" DataField="Categoria" HeaderText="Categoria">
							<ItemStyle CssClass ="hidden" HorizontalAlign="Left"></ItemStyle>
						</asp:BoundField>
					</Columns>
                </asp:GridView>
            </fieldset>
                <fieldset>
            <legend>Ordenes Extornadas</legend>
                <asp:GridView ID="dgListaOX" runat="server" SkinID="Grid" 
                    AutoGenerateColumns="False">
                    <Columns>
							<asp:TemplateField HeaderText="Seleccionar">
								<HeaderStyle Width="80px"></HeaderStyle>
								<ItemTemplate>
                                    <asp:ImageButton ID="ibSeleccionar" runat="server" CommandName="Seleccionar" SkinID ="imgCheck"  />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="FechaOperacion" HeaderText="Fecha de Operación">
								<HeaderStyle Width="80px"></HeaderStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Fondo" HeaderText="Fondo">
								<HeaderStyle Width="100px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="NumeroTransaccion" HeaderText="Nro Orden">
								<HeaderStyle Width="100px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="TipoOperacion" HeaderText="Tipo de Operación">
								<HeaderStyle Width="80px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n">
								<HeaderStyle Width="150px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Estado" HeaderText="Estado">
								<HeaderStyle Width="80px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Moneda" HeaderText="Moneda">
								<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="MontoOperacion" HeaderText="Monto Operaci&#243;n" DataFormatString="{0:#,##0.0000000}">
								<HeaderStyle Width="100px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField  HeaderStyle-CssClass="hidden" DataField="ISIN" HeaderText="ISIN">
                            <ItemStyle CssClass="hidden" HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
							<asp:BoundField  HeaderStyle-CssClass="hidden" ControlStyle-CssClass="hidden" DataField="TipoOrden" HeaderText="TipoOrden">
                            <ItemStyle CssClass="hidden" HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
							<asp:BoundField  HeaderStyle-CssClass="hidden" ControlStyle-CssClass="hidden" DataField="Categoria" HeaderText="Categoria">
                            <ItemStyle CssClass="hidden" HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
						</Columns>
                </asp:GridView>
            </fieldset>
                <fieldset>
            <legend>Detalle de Extorno de Ordenes Ejecutadas</legend>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Código ISIN</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lbCodigoISIN" runat="server"  Width="120px" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Nro. Transacción</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lbNroTransaccion" runat="server" Width="96px" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Tipo Operación</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lbTipoOperacion" runat="server"  Width="120px" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label"><asp:TextBox ID="lblFechaBusqueda" runat="server"  Visible="false" /></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lbParametros" runat="server"  Width="104px" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Tipo Orden</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="lbTipoOrden" runat="server" Width="104px" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
                 <br />
                <div class="row">
                <div class="col-md-12" align="rigth">
                    <div class="form-group">
                        <div class="col-sm-12" style="text-align: right;">
                            <asp:Button runat="server" ID="ibConsultar" Text="Consultar" Visible ="false" />
                            <asp:Button runat="server" ID="ibExtornar" Text="Extornar" />
                            <asp:Button runat="server" ID="ibSalir" Text="Salir" />
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
