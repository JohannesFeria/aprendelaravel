<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmEntidadExcesos.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Entidades_EntidadExcesos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Tipo Operacion</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Mantenimiento de Operación</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Entidad</label>
                        <div class="col-sm-8">
                             <div class="input-append">
                                <asp:TextBox ID="txtCodigoEntidad" runat="server" Width="150px" MaxLength="4" Enabled="False"></asp:TextBox>
                                <asp:LinkButton ID="lkbBuscar" runat="server">
                                    <span class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>          
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n:
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" MaxLength="30" Width="300px" Enabled="false" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Descripción"
                                ControlToValidate="tbDescripcion" runat="server" Text="(*)" CssClass="validator"
                                ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Monto Exceso:
                        </label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbMontoExceso" MaxLength="30" Width="300px" CssClass="Numbox-7" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Monto Exceso"
                                ControlToValidate="tbMontoExceso" runat="server" Text="(*)" CssClass="validator"
                                ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />

        <header>
        </header>

        <div class="row" style="text-align: right;">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                    </label>
                    <div class="col-sm-8">
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                    </label>
                    <div class="col-sm-8">
                        <asp:Button Text="Aceptar" runat="server" ID="ibAceptar" ValidationGroup="vgDetalle" />
                        <asp:Button Text="Retornar" runat="server" ID="ibCancelar" CausesValidation="false"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdCodigoEntidad" runat="server" />
    <asp:HiddenField ID="hdEstadoBusqueda" runat="server" />
    <asp:HiddenField ID="hdDescripcion" runat="server" />
    <asp:ValidationSummary runat="server" ID="vgDetalle" ValidationGroup="vgDetalle"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
