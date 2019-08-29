<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMatrizContable.aspx.vb" Inherits="Modulos_Contabilidad_frmMatrizContable" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server"><title>Matriz Contable</title></head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Matriz Contable</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Matriz</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlFondo" runat="server" AutoPostBack="False" Visible="false" />
                            <asp:DropDownList ID="ddlMatriz" runat="server" AutoPostBack="false" />
                        </div>
                        <div class="col-sm-2">
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Situaci&oacute;n</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlSituacion" runat="server" AutoPostBack="False" >
                                <asp:ListItem Value="A" Text="Activo" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="I" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo Instrumento</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlTipoInstrumento" runat="server" />
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Operaci&oacute;n</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlOperacion" runat="server" AutoPostBack="false" />
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Moneda</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlMoneda" runat="server" />
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo Cabecera M</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="tbCodigoCabeceraM" runat="server" MaxLength="10" CssClass ="form-control" onkeypress="return soloNumeros(event)" />
                        </div>
                        <div class="col-sm-5"></div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="ibtnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla" style="height: 350px;">
            <asp:UpdatePanel runat="server" ID="UpdPanelMatriz">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgCabecera">
                        <Columns>
                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoCabeceraMatriz") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoCabeceraMatriz") %>'
                                        OnClientClick="return confirm('¿Desea eliminar el registro seleccionado?')">
                                    </asp:ImageButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoCabeceraMatriz">
                            </asp:BoundField>
                            <asp:BoundField DataField="MatrizContable" HeaderText="Matriz"></asp:BoundField>
                            <asp:BoundField DataField="CodigoPortafolio" HeaderText="CodigoPortafolio" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda"></asp:BoundField>
                            <asp:BoundField DataField="Operacion" HeaderText="Operaci&#243;n"></asp:BoundField>
                            <asp:BoundField DataField="ClaseInstrumento" HeaderText="Clase Instrumento"></asp:BoundField>
                            <asp:BoundField DataField="CodigoModalidadPago" HeaderText="CodigoModalidadPago" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                            <asp:BoundField DataField="CodigoTipoInstrumento" HeaderText="Tipo Instrumento">
                            </asp:BoundField>
                            <asp:BoundField DataField="SectorEmpresarial" HeaderText="Cuenta Tipo Inst"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoMatrizContable">
                            </asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoOperacion"
                                HeaderText="CodigoOperacion"></asp:BoundField>                          
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibtnBuscar" EventName="Click"/>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Ingresar" runat="server" ID="ibtnIngresar" />
            <asp:Button Text="Imprimir" runat=server ID="ibtnImprimir" />
            <asp:Button Text="Salir" runat="server" ID="ibtnSalir" />
        </div>
    </div>
    <asp:HiddenField ID="hdCodigoDetalleMatriz" runat="server" />
    <asp:HiddenField ID="tbPortafolio" runat="server" />
    <asp:HiddenField ID="hdModal" runat="server" />
    <asp:HiddenField ID="btModal" runat="server" />
    </form>
</body>
</html>