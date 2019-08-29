<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBancoCuenta.aspx.vb" Inherits="Modulos_PrevisionPagos_frmBancoCuenta" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Banco Cuenta</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Cuenta Corriente de Banco</h2>
                </div>
            </div>
        </header>

        
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Banco </label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="tbcuotadmF" runat="server" Width="190px"  MaxLength="25"></asp:TextBox>
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
                         <asp:DropDownList ID="ddlEstado" runat="server" Width="100px" CssClass="stlCajaTexto">
                                <asp:ListItem Value="1">Activo</asp:ListItem>
                                <asp:ListItem Value="2">Inactivo</asp:ListItem>
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
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <asp:GridView runat="server" SkinID="Grid" ID="gvPagos" DataKeyNames="Codigo" >
                        <Columns>                       
                             <asp:TemplateField ItemStyle-Width="10px">
                                <ItemTemplate>            
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Codigo")%>'
                                        CommandName="Modificar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Codigo")%>'
                                        CommandName="Eliminar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Codigo" HeaderText="Código" />
                            <asp:BoundField DataField="Entidad" HeaderText="Entidad" />
                            <asp:BoundField DataField="Banco" HeaderText="Banco"/>                                                       
                            <asp:BoundField DataField="Cta" HeaderText="Cuenta Corriente"/>
                            <asp:BoundField DataField="TipoCuenta" HeaderText="Tipo Cuenta" />
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado"/>
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
                <asp:Button Text="Ingresar" runat="server" ID="ibIngresar" Height="26px" />
                <asp:Button Text="Salir" runat="server" ID="ibCancelar" CausesValidation="false"/>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

