<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBroker.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmBroker" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Broker / Comisiones</title>
    <script type="text/javascript">       
        function cambiaTitulo() {
            if (document.getElementById("ddltipoTramo").value == 'PRINCIPAL') {
                document.getElementById("txtCosto").value = "0"
                document.getElementById("txtBandaInferior").value = ""
            }
            else if (document.getElementById("ddltipoTramo").value == 'AGENCIA') {
                document.getElementById("txtCosto").value = ""
                document.getElementById("txtBandaInferior").value = "0"
            }
            else {
                document.getElementById("txtCosto").value = ""
                document.getElementById("txtBandaInferior").value = ""
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header><h2>Mantenimiento Tramo Broker</h2></header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Código Entidad</label>
                        <div class="col-md-9">
                            <div class="input-append">
                                <asp:TextBox ID="txtCodigoEntidad" runat="server" Width="150px" MaxLength="4" Enabled="False"></asp:TextBox>
                                <asp:LinkButton ID="lkbBuscar" runat="server" CausesValidation="false" Enabled="false"  >
                                    <span class="add-on" ><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Código Entidad" ControlToValidate="txtCodigoEntidad">(*)</asp:RequiredFieldValidator>
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
                            Descripci&oacute;n</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtDescripcion" runat="server" Width="300px" Enabled ="false"  />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="Descripción" ControlToValidate="txtDescripcion">(*)</asp:RequiredFieldValidator>
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
                            Situación</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="152px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Detalle del Broker</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Tipo de Tramo</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddltipoTramo" runat="server" Width="185" CssClass="stlCajaTexto"
                                OnChange="javascript:cambiaTitulo();">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ErrorMessage="Tipo de Tramo" ControlToValidate="ddltipoTramo">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Tipo Costo</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlTipoCosto" runat="server" Width="152px" CssClass="stlCajaTexto">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Tramo</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtTramo" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                ErrorMessage="Tramo" ControlToValidate="txtTramo">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Costo (USP)</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtCosto" runat="server" Width="150px" MaxLength="22" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ErrorMessage="Costo (USP)" ControlToValidate="txtCosto">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Banda Inferior</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtBandaInferior" runat="server" Width="150px" MaxLength="22"></asp:TextBox>
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
                            Banda Superior</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtBandaSuperior" runat="server" Width="150px" MaxLength="22"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6">
                <div class="form-group" style="float: right;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                    <asp:Button ID="btnSalir" runat="server" Text="Retornar" 
                        CausesValidation="false" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdCodigoEntidad" runat="server" />
        <asp:HiddenField ID="hdDescripcion" runat="server" />
        <asp:HiddenField ID="hdEstadoBusqueda" runat="server" />
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
