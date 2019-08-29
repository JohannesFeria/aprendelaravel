<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaModeloCartas.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaModeloCartas" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Modelo de Cartas</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Modelo de Cartas</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Codigo</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="4" Width="56px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        
                    </div>
                </div>
            </div>
             <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripción</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="400px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Situación</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlSituacion" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        
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
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                             <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoModelo") %>'
                                        OnCommand="Modificar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoModelo") %>'
                                        OnCommand="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoModelo" HeaderText="C&#243;digo" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n"  />
                            <asp:BoundField DataField="ArchivoPlantilla" HeaderText="Ruta" />                           
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n" />                           
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


