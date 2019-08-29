<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRegPagos.aspx.vb" Inherits="Modulos_PrevisionPagos_frmRegPagos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro Pagos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Aprobar Pagos</h2>
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
                                    <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
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
                            <asp:DropDownList ID="ddlTipoOperacion" runat="server" Width="200px" 
                                CssClass="stlCajaTexto">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                 <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            <span style="font-weight: bold; color: Red">Hora Cierre <asp:Label ID="lblHoraCierre" runat="server"></asp:Label></span></label>
                        <div class="col-sm-7">
                            
                        </div>
                    </div>
                </div>
            </div>           
          
              <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                       <asp:DropDownList ID="ddlEstado" runat="server" Width="100px" Visible="False" 
                                    CssClass="stlCajaTexto">
                                    <asp:ListItem Value="1">TODOS</asp:ListItem>
                                    <asp:ListItem Value="2">Anulados</asp:ListItem>
                                    <asp:ListItem Value="3">Aprobados</asp:ListItem>
                                    <asp:ListItem Value="4">Pendientes</asp:ListItem>
                                </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>

        </fieldset>
        

        <br />
        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Always">
        <ContentTemplate>        
        
        
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        

        <br />


         <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                         <asp:Button ID="bBoton" runat="server" Width="72px" Text="Aprobar"
                                CausesValidation="False" CssClass="button"></asp:Button>
                            <asp:Button ID="bAnular" runat="server" Width="72px" Text="Anular"
                                CausesValidation="False" CssClass="button"></asp:Button>
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                     <asp:Image ID="Image4" runat="server" 
                                            ImageUrl="~/App_Themes/img/AMARILLO3.gif"/>
                                        &nbsp;Pendiente
                                        <asp:Image ID="Image2" runat="server" 
                                            ImageUrl="~/App_Themes/img/VERDE2.GIF" />
                                        &nbsp;Aprobado
                                        <asp:Image ID="Image3" runat="server" 
                                            ImageUrl="~/App_Themes/img/rojo2.gif" />
                                        &nbsp;Anulado        
                </div>
            </div>


</ContentTemplate>
        </asp:UpdatePanel>

        <div class="grilla">    
          
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="gvPagos" >
                        <Columns>
                              <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAllOrdenPago" runat="server" AutoPostBack="true" 
                                    CausesValidation="false" Visible="false"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" 
                                    CausesValidation="false" onclick="javascript:colorea(this);" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" Width="25px" CssClass="stlPaginaTexto2" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <asp:Image ID="ImgPendiente" runat="server" 
                                    ImageUrl="~/App_Themes/img/AMARILLO3.GIF"/>
                                <asp:Image ID="ImgAprobado" runat="server" 
                                    ImageUrl="~/App_Themes/img/VERDE2.GIF" />
                                <asp:Image ID="ImgAnulado" runat="server" 
                                    ImageUrl="~/App_Themes/img/rojo2.gif" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" Width="25px" CssClass="stlPaginaTexto2" />
                        </asp:TemplateField>

                            <asp:BoundField DataField="CodigoPago" HeaderText="Código Pago" />
                            <asp:BoundField DataField="UsuarioProvision" HeaderText="Usuario" />
                            <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operación" />
                            <asp:BoundField DataField="Importe" HeaderText="Importe" />                           
                            <asp:BoundField DataField="CuentaOrigen" HeaderText="Cuenta Origen" />                           
                            <asp:BoundField DataField="BancoOrigen" HeaderText="Banco Origen" />                           
                            <asp:BoundField DataField="CuentaDestino" HeaderText="Cuenta Destino" />                           
                            <asp:BoundField DataField="BancoDestino" HeaderText="Banco Destino" />                           
                            <asp:BoundField DataField="Estado" HeaderText="Estado" Visible="false" />                           
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>                               
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
              
        </div>
        <br />
       
    </div>
    </form>
</body>
</html>

