<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmParametriaGeneral.aspx.vb" Inherits="Modulos_PrevisionPagos_frmParametriaGeneral" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Feriados</title>
</head>
<body>
    <form  id="form2" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Parámetros Generales</h2>
                </div>
            </div>
        </header>

        
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Parámetros </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="tbParametro" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="text-align: right;">
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
                    <asp:GridView runat="server" SkinID="Grid" ID="gvPagos">
                        <Columns>
                            <asp:TemplateField HeaderText="Modificar">
                                <HeaderStyle Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit"
                                        OnCommand="Modificar" CommandArgument='<%# Bind("idRegistro") %>' CommandName='<%# Bind("descripcion") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="descripcion" HeaderText="Descripción">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" CssClass="stlPaginaTexto2" />
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
                <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false"/>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

