<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmModeloCarta.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmModeloCarta" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Mantenimiento Modelo de Carta</title>
    <script type="text/javascript">
        function showModal(valor) {
            $('#<%=hfModal.ClientID %>').val(valor);
            return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Personal', '1200', '600', ''); 
        }

   
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header>
            <h2>
                Mantenimiento Modelo de Carta</h2>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Código</label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="tbCodigo" Width="48px" MaxLength="4" />
                            <asp:RequiredFieldValidator ID="rfvCodigo" ErrorMessage="Código" ControlToValidate="tbCodigo"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Descripción</label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" Width="296px" />
                            <asp:RequiredFieldValidator ID="rfvDescripcion" ErrorMessage="Descripción" ControlToValidate="tbDescripcion"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Usuario Validador 1</label>
                        <div class="col-md-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtUsuarioValidador1" Width="104px" />
                                <asp:LinkButton runat="server" OnClientClick="return showModal(1);" ID="lbtUsuarioValidador1"
                                    CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvUsuarioValidador1" ErrorMessage="Usuario Validador 1"
                                ControlToValidate="txtUsuarioValidador1" runat="server" Text="(*)" CssClass="validator"
                                ValidationGroup="vgDetalle" />
                            <asp:Label ID="lblNombreUsuario1" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Archivo</label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlArchivo" runat="server" Width="350px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvArchivo" ErrorMessage="Archivo" ControlToValidate="ddlArchivo"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Usuario Validador 2</label>
                        <div class="col-md-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtUsuarioValidador2" Width="104px" />
                                <asp:LinkButton runat="server" OnClientClick="return showModal(2);" ID="lbtUsuarioValidador2"
                                    CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvUsuarioValidador2" ErrorMessage="Usuario Validador 2"
                                ControlToValidate="txtUsuarioValidador2" runat="server" Text="(*)" CssClass="validator"
                                ValidationGroup="vgDetalle" />
                            <asp:Label ID="lblNombreUsuario2" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Operación
                        </label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlOperacion" runat="server" Width="328px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvOperacion" ErrorMessage="Operación" ControlToValidate="ddlOperacion"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Modelo Carta Estructura
                        </label>
                        <div class="col-md-8">
                            <div class="input-append">
                                <asp:LinkButton runat="server" ID="ibtnModeloCartaEstructura"  CausesValidation="false"><span class="add-on"  style="border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px;"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            <asp:Label ID="lblRutaAnterior" runat="server">Ruta Actual:</asp:Label>
                        </label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="tbRutaAnterior" Width="584px" Enabled="true" />
                            <asp:RequiredFieldValidator ID="rfvRutaAnterior" ErrorMessage="Codigo" ControlToValidate="tbRutaAnterior"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-8">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-4 control-label">
                            Situación
                        </label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlSituacion" runat="server" Width="115px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvSituacion" ErrorMessage="Situación" ControlToValidate="ddlSituacion"
                                runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-8">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                </div>
                <div class="col-md-5">
                    <div class="form-group" style="float: right;">
                        <asp:Button Text="Aceptar" runat="server" ID="ibAceptar" ValidationGroup="vgDetalle"
                            OnClick="bAceptar_Click" />
                        <asp:Button ID="btnSalir" runat="server" Text="Retornar" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <asp:HiddenField ID="hd" runat="server" />
        <asp:HiddenField ID="hdruta" runat="server" />
        <asp:HiddenField ID="hfModal" runat="server" />
        <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
            HeaderText="Los siguientes campos son obligatorios:" ValidationGroup="vgDetalle" />
    </div>
    </form>
</body>
</html>
