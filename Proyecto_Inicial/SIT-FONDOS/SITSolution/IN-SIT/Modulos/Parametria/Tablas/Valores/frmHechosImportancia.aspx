<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmHechosImportancia.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmHechosImportancia" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Notificaciones de Importancia</title>
    <script type="text/javascript">
        function showModal() {
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=ValoresNemonicoAprob', '1200', '600', '');           
        }
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><div class="row"><div class="col-md-6"><h2>Mantenimeinto de Notificaciones de Importancia</h2></div></div></header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="Portafolio" ControlToValidate="ddlFondo"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Mnemonico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbNemonico" Width="180px" />
                                <asp:LinkButton ID="lkbModal" runat="server" OnClientClick="return showModal()" CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Mnemonico" ControlToValidate="tbNemonico"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFecha" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ErrorMessage="Fecha" ControlToValidate="tbFecha" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" />
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Mnemonico" ControlToValidate="tbDescripcion"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
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
            <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
    </div>
    <input id="hdCodigo" type="hidden" name="hdCodigo" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
