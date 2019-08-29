<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmUsuarios.aspx.vb" Inherits="Modulos_PrevisionPagos_frmUsuarios" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro Pagos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManagerLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Usuarios</h2>
                </div>
            </div>
        </header>
                
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Usuarios</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbUsuario" runat="server"></asp:TextBox>
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
                             Estado</label>
                        <div class="col-sm-7">
                             <asp:DropDownList ID="ddlEstado" runat="server">
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
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar"/>
                </div>
            </div>
        </fieldset>
        

        <br />

        
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        

        <br />

        <div class="grilla">
          
                <asp:UpdatePanel ID="pnlFormulario" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvPagos" runat="server" SkinID="Grid">
                            <Columns>
                                <asp:TemplateField HeaderText="Modificar">
                                    <HeaderStyle Width="70px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit"
                                            CommandName="Modificar" CommandArgument='<%# Bind("CodUsuario") %>' AlternateText="Modificar">
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <HeaderStyle Width="70px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete"
                                            CommandName="Eliminar" CommandArgument='<%# Bind("CodUsuario") %>' AlternateText="Eliminar"
                                            OnClientClick="return validar()"></asp:ImageButton>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre Usuario">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Area" HeaderText="Área">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Situacion" HeaderText="Estado">
                                    <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                    <ItemStyle HorizontalAlign="Center" Width="200px" CssClass="stlPaginaTexto2" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
              
        </div>
        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CausesValidation="False">
                </asp:Button>
                &nbsp;<asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="False" Visible="false">
                </asp:Button>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
