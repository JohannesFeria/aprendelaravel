<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBancoCuentaDetalle.aspx.vb" Inherits="Modulos_PrevisionPagos_frmBancoCuentaDetalle" %>

<!DOCTYPE html><html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Mantenimiento Cuentas Corrientes de Banco</title>  
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>

        
    <div class="container-fluid">
        <header>
            <h2>Mantenimiento Cuentas Corrientes de Banco</h2>
        </header>
        
        
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Entidad</label>
                        <div class="col-md-9">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlFondo" runat="server" Width="250px" 
                                    CssClass="stlCajaTexto">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">                     
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            </label>
                        <div class="col-md-9">
                            
                        </div>
                    </div>                
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Banco</label>
                        <div class="col-md-9">
                                         <asp:DropDownList ID="ddlBanco" runat="server" Width="250px" 
                                    CssClass="stlCajaTexto">
                                </asp:DropDownList>                      
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
             <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Nro. Cuenta Corriente</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="tbCtaCte" runat="server" Width="250px" CssClass="stlCajaTexto" onkeypress="javascript:if(event.keyCode!=45 && event.keyCode<48 || event.keyCode>57){return false;}" ></asp:TextBox>                             
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Tipo Cuenta</label>
                        <div class="col-md-9">
                             <asp:DropDownList ID="ddlTipoCuenta" runat="server" Width="150px" 
                                    CssClass="stlCajaTexto">
                                    <asp:ListItem Value="1">Activo</asp:ListItem>
                                    <asp:ListItem Value="2">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
             <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                             Tipo Moneda</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlTipoMoneda" runat="server" Width="150px" 
                                    CssClass="stlCajaTexto">
                                    <asp:ListItem Value="1">Activo</asp:ListItem>
                                    <asp:ListItem Value="2">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
               
               <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                              Situaci&oacute;n</label>
                        <div class="col-md-9">
                                    <asp:DropDownList ID="ddlEstado" runat="server" Width="150px" 
                                    CssClass="stlCajaTexto">
                                    <asp:ListItem Value="1">Activo</asp:ListItem>
                                    <asp:ListItem Value="2">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>

        </fieldset>
        
        <br />


         <div class="row">
            <div class="col-md-6">
                </div>
                <div class="col-md-5">
                    <div class="form-group" style="float: right;">
                        <asp:Button Text="Aceptar" runat="server" ID="bAceptar" 
                            ValidationGroup="vgDetalle" />
                        
                        <asp:Button ID="bSalir" runat="server" Text="Retornar" 
                            CausesValidation="false" />
                    </div>
                </div>
            </div>      

        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
                
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
