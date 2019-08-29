<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPagos.aspx.vb" Inherits="Modulos_PrevisionPagos_frmPagos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro Pagos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">

    <table> 
    <tr>
    <td valign="middle" >
    
    </td>
    </tr>
    </table>

    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Registro Pagos</h2>
                </div>
            </div>
        </header>

        
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                         Fecha Pago</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                    <asp:TextBox runat="server" ID="txtFechaPago" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
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
                             Tipo Operaci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoOperacion" runat="server" Width="250px" >
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
                             Estado</label>
                        <div class="col-sm-7">
                             <asp:DropDownList ID="ddlEstado" runat="server" Width="120px" 
                                CssClass="stlCajaTexto">
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
                    <asp:GridView SkinID = "Grid" runat="server" ID="gvPagos" DataKeyNames="CodigoPago" >
                        <Columns>
                             <asp:TemplateField  ItemStyle-Width="10px">
                                <ItemTemplate>            
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPago")%>'
                                        CommandName="Modificar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPago")%>'
                                        CommandName="Eliminar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoPago" HeaderText="Código Pago" ItemStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operación" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />                                                       
                            <asp:TemplateField Visible="false" >
                            <ItemTemplate>
                                <input type="hidden" id="hdEstAprobacion" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.EstadoApro") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>                               
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
              
        </div>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button Text="Ingresar" runat="server" ID="ibIngresar" Height="26px" />
            <asp:Button Text="Salir" runat="server" ID="ibCancelar" CausesValidation="false"/>
        </div>
    </div>
    </form>
</body>
</html>

