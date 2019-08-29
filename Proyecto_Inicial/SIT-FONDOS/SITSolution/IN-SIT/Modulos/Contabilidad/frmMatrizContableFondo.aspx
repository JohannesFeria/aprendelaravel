<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMatrizContableFondo.aspx.vb" Inherits="Modulos_Contabilidad_frmMatrizContableFondo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Matriz Contable - Portafolio</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Matriz Contable - Negocio</h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Negocio</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlNegocio" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Matriz</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlMatriz" runat="server"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Serie</label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="ddlSerie" runat="server"  />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Código</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtCodigo" runat="server" Enabled ="false" Text ="0" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos Detalle Matriz</legend>
            <asp:UpdatePanel runat="server" ID="upGrilla" UpdateMode ="Conditional" >
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label"><asp:Label ID="lblNumeroCuenta" runat="server">Cuenta Contable</asp:Label></label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="tbNumeroCuentaContable" runat="server" MaxLength="20" CssClass="form-control" onkeypress="return soloNumeros(event)" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Debe/Haber</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlDebeHaber" runat="server" Width="130px">
                                    <asp:ListItem Value="" Text="--SELECCIONE--" ></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Debe" ></asp:ListItem>
                                    <asp:ListItem Value="H" Text="Haber"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                    </div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
            <br />
            <div class="grilla" >
                <asp:UpdatePanel runat="server" ID="UpdPanelDetalleMatriz">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar" 
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.NumeroCuentaContable")  %>' >
                                    </asp:ImageButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibElimina" runat="server" SkinID="imgDelete" CommandName="Eliminar" 
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.NumeroCuentaContable")  %>' >
                                    </asp:ImageButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoCabeceraMatriz" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                            <asp:BoundField DataField="NumeroCuentaContable" HeaderText="Cuenta Contable"></asp:BoundField>
                            <asp:BoundField DataField="DebeHaber" HeaderText="Debe/Haber"></asp:BoundField>
                            <asp:BoundField DataField="Glosa" HeaderText="Glosa"></asp:BoundField>
                            <asp:BoundField DataField="Aplicar" HeaderText="Aplicar"></asp:BoundField>
                            <asp:BoundField DataField="Secuencia" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hdGrilla" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click"  />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br/>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                style="height: 26px" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar"  />
        </div>

    </div>
    </form>
</body>
</html>