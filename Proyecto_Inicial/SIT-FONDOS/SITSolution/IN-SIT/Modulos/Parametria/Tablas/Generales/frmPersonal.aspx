<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPersonal.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmPersonal" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Monedas</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
        <h2>Personal</h2>
        </header>
        <br />
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Código Interno</label>
                    <div class="col-md-9">
                        <asp:TextBox id="txtCodigoInterno" runat="server" MaxLength="10" ></asp:TextBox>
                        <strong>
                        <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" 
                            ErrorMessage="Código Interno" ForeColor="Red" ControlToValidate="txtCodigoInterno">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                       <label class="col-md-3 control-label">Libreta Electoral:</label>
                    <div class="col-md-9">
                        <asp:textbox id="txtLibretaElectoral" runat="server" MaxLength="12"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="rvfLibretaElectoral" runat="server" 
                            ErrorMessage="Libreta Electoral" ForeColor="Red" 
                            ControlToValidate="txtLibretaElectoral">(*)</asp:RequiredFieldValidator></strong>
                  
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Apellido Paterno</label>
                    <div class="col-md-9">
                        <asp:TextBox id="txtApellidoPaterno" runat="server" MaxLength="50"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="rfvApellidoPaterno" runat="server" 
                            ErrorMessage="Apellido Paterno" ForeColor="Red" 
                            ControlToValidate="txtApellidoPaterno">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Apellido Materno</label>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtApellidoMaterno" Runat="server" CssClass="stlCajaTexto" MaxLength="50"></asp:TextBox>
                        <strong><asp:RequiredFieldValidator ID="rfvApellidoMaterno" runat="server" 
                            ErrorMessage="Apellido Materno" ForeColor="Red" 
                            ControlToValidate="txtApellidoMaterno">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Nombre</label>
                    <div class="col-md-9">
                        <asp:textbox id="txtNombre" runat="server" MaxLength="50" Width="360px"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                            ErrorMessage="Nombre" ForeColor="Red" 
                            ControlToValidate="txtNombre">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                  <label class="col-md-3 control-label">Matricula</label>
                    <div class="col-md-9">
                        <asp:textbox id="txtMatricula" runat="server" MaxLength="10"></asp:textbox>
                        
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Fecha Ingreso</label>
                    <div class="col-md-9">
                             <div id="dvDiv321" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaIngreso" SkinID="Date" />
                                <span runat="server" id="imgFechaIngreso" class="add-on"><i class="awe-calendar"></i>
                                </span>
                            </div>
                        <strong>
                        <asp:RequiredFieldValidator ID="rfvFechaIngreso" runat="server" 
                            ErrorMessage="Fecha Ingreso" ForeColor="Red" ControlToValidate="txtFechaIngreso">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Fecha Cese</label>
                    <div class="col-md-9">
                             <div id="Div1" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaCese" SkinID="Date" />
                                <span runat="server" id="imgFechaCese" class="add-on"><i class="awe-calendar"></i>
                                </span>
                            </div>
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Cargo</label>
                    <div class="col-md-9">
                        <asp:dropdownlist id="ddlCargo" runat="server" ></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="rvfCargo" runat="server" 
                            ErrorMessage="Cargo" ForeColor="Red" 
                            ControlToValidate="ddlCargo">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
            <div class="form-group">
                    <label class="col-md-3 control-label">Centro Costo</label>
                    <div class="col-md-9">
                        <asp:dropdownlist id="ddlCentroCosto" runat="server" ></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="rvfCentroCosto" runat="server" 
                            ErrorMessage="Centro Costo" ForeColor="Red" 
                            ControlToValidate="ddlCentroCosto">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-md-3 control-label">Email Trabajo</label>
                    <div class="col-md-9">
                        <asp:textbox id="txtEmailTrabajo" runat="server" MaxLength="100" Width="360px"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="rvfEmailTrabajo" runat="server" 
                            ErrorMessage="Email Trabajo" ForeColor="Red" 
                            ControlToValidate="txtEmailTrabajo">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
            <div class="form-group">
                    <label class="col-md-3 control-label">Email Personal</label>
                    <div class="col-md-9">
                        <asp:textbox id="txtEmailPersonal" runat="server" MaxLength="100" Width="360px"></asp:textbox>
                        <strong><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Email Personal" ForeColor="Red" 
                            ControlToValidate="txtEmailPersonal">(*)</asp:RequiredFieldValidator></strong>
                    </div>
                </div>
            </div>
            </div>
        </fieldset>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="false" />
            <asp:HiddenField ID="hd" runat="server" />
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>