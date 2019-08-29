<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAprobadorReporte.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmAprobadorReporte" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Aprobador Documentos</title>
    <script type="text/javascript">
        function showModal() {
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
                        Aprobador Documentos</h2>
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
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoUsuario" Width="120px" Enabled ="false" />
                                <asp:LinkButton runat="server"  ID="lkbBuscarUsuario" Enabled ="false"
                                    CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="tbNombreUsuario" Width="250px"  Enabled="false"/>
                            <asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Usuario" ControlToValidate="tbCodigoUsuario"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Roles</label>
                        <div class="col-sm-8">
                            <div class="row">
                                <asp:CheckBox Text="Administrador" runat="server" ID="chkAdmin" /></div>
                            <div class="row">
                                <asp:CheckBox Text="Firmante" runat="server" ID="chkFirma" /></div>
                            <div class="row">
                                <asp:CheckBox Text="Operador" runat="server" ID="chkOperador" /></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Firma</label>
                        <div class="col-sm-8">
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept="image/*" class="filestyle"
                                data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" size="80">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Clave</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbClave" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                            Previsualizar
                        </label>
                        <div class="col-sm-10">
                            <asp:Image ID="imgFirma" runat="server" Width="150px" Height="120px" AlternateText="Imagen no disponible."   />
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
