<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSaldoNoAdministrado.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmSaldoNoAdministrado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Saldo no Administrado</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header><h2>Saldos no Administrados</h2></header>
        <br />    
        <fieldset>
            <legend>Ingreso de Datos</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Mandato</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlMandato" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Fondo" ControlToValidate="ddlMandato" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha</label>
                        <div class="col-sm-9" >
                            <div class="input-append date" id="spanFecha"  runat ="server">
                                <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                <span id="Span1" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="Fecha" ControlToValidate="txtFecha" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>            
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Banco</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlBanco" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ErrorMessage="Banco" ControlToValidate="ddlBanco" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Tipo de Cuenta</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlTipoCuenta" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ErrorMessage="Tipo de Cuenta" ControlToValidate="ddlTipoCuenta" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Moneda</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlMoneda" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ErrorMessage="Moneda" ControlToValidate="ddlMoneda" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Saldo</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtSaldo" runat="server" MaxLength="50" Width="100px"  CssClass="Numbox-7" onblur="Javascript:Calcular(); formatCurrency(txtSaldo.id);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                ErrorMessage="Saldo" ControlToValidate="txtSaldo" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>              
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Estado</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlEstado" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                            ErrorMessage="Estado" ControlToValidate="ddlEstado" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>  
        <br />   
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
                CausesValidation="False" />
                        <asp:HiddenField ID="hd" runat="server" />
        </div>                                 
    </div>
    </form>
</body>
</html>
