<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmUsuariosDetalle.aspx.vb" Inherits="Modulos_PrevisionPagos_frmUsuariosDetalle" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Usuarios</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Mantenimiento Usuario</h2>
                </div>
            </div>
        </header>
                
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Usuario</label>
                        <div class="col-sm-7">

                        <div class="input-append">
                                <asp:TextBox ID="tbUsuario" runat="server" Enabled="false">
                                </asp:TextBox>
                                <%--<asp:ImageButton ID="ibBuscar" runat="server" ImageUrl="../../App_Themes/img/icons/glyphicons_search.png"
                                CausesValidation="false" Visible="false" />--%>
                                <asp:Label runat="server" ID="lbCodigoUsuario" />
                                <asp:HiddenField runat="server" ID="_CodigoUsuario" />
                                <asp:HiddenField runat="server" ID="_NomUsuario" />
                                <asp:LinkButton ID="lkbBuscar" runat="server">
                                    <span class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                        </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                             &Aacute;rea</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbArea" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>
             <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                             Situaci&oacute;n</label>
                        <div class="col-sm-7">
                             <asp:DropDownList ID="ddlEstado" runat="server" Width="100px" CssClass="stlCajaTexto">
                                <asp:ListItem Value="A">Activo</asp:ListItem>
                                <asp:ListItem Value="I">Inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        
        <br />

        <%--<br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>--%>

        <fieldset>
            <legend>Detalle Tipo de Operaci&oacute;n</legend>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                             Tipo de Operaci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoOperacion" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

             <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                             Situaci&oacute;n</label>
                        <div class="col-sm-7">
                             <asp:DropDownList ID="ddlSituacion" runat="server">
                                <asp:ListItem Value="A">Activo</asp:ListItem>
                                <asp:ListItem Value="I">Inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">           
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" ValidationGroup="GrupoTipoOperacion"></asp:Button>
                </div>
            </div>

        </fieldset>
        

        <br />

        <div class="grilla">
          
            <asp:UpdatePanel runat="server" ID="updGrilla">
            <ContentTemplate>
                <asp:GridView ID="gvPagos" runat="server" SkinID="Grid">
                    <Columns>
                        <asp:TemplateField HeaderText="Eliminar">
                            <HeaderStyle Width="70px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete"
                                    CommandName="Eliminar" CausesValidation="false" CommandArgument='<%# Bind("IdTipoOperacion") %>'
                                    AlternateText="Eliminar"></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hIdTipoOperacion" Value='<%# Bind("IdTipoOperacion") %>' />
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo de Operaci&oacute;n">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Situacion" HeaderText="Situaci&oacute;n">
                            <HeaderStyle HorizontalAlign="Center" Width="200px" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" CssClass="stlPaginaTexto2" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>              
        
        </div>

        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CausesValidation="False"></asp:Button>&nbsp;
                <asp:Button ID="btnSalir" runat="server" Text="Retornar" CausesValidation="False"></asp:Button>
            </div>
        </div>
    </div>
    </form>
</body>
</html>


