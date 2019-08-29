<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmVacaciones.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmVacaciones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Aprobador Carta</title>
    <script type="text/javascript">
        function showPopupUsuarios() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Personal', '1200', '600', '');
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Vacaciones del Personal</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            C&oacute;digo Usuario</label>
                        <div class="col-sm-10">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoUsuario" Width="120px" />
                                <asp:LinkButton ID="lkbBuscarUsuario" runat="server" CausesValidation="false" Enabled="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbNombreUsuario" Width="280px" Enabled="false" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="C&oacute;digo Usuario"
                                ControlToValidate="tbCodigoUsuario" runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Fecha Inicio</label>
                        <div class="col-sm-10">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on"><i class="awe-search"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Fecha Inicio" ControlToValidate="tbFechaInicio"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Fecha Fin</label>
                        <div class="col-sm-10">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on"><i class="awe-search"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Fecha Fin" ControlToValidate="tbFechaFin"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-10">
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
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnRetornar" CausesValidation="false" />
        </div>
    </div>
    <input type="hidden" runat="server" id="hdFirma">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
