<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCustodios.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmCustodios" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Custodios</title>
    <script language="javascript">
        var strMensajeError = "";
        function ValidaCamposCabecera() {
            var strMsjCampOblig = "";
            String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ""); }
            if (document.getElementById("<%= tbCodigo.ClientID %>").value.trim() == "")
                strMsjCampOblig += "\t-Código Custodio\n";
            if (document.getElementById("<%= tbDescripcion.ClientID %>").value.trim() == "")
                strMsjCampOblig += "\t-Descripción\n";
            if (document.getElementById("<%= ddlSituacion.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Situación\n";
            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }{ return true; }
        }
        function ValidaCamposDetalle() {
            var strMsjCampOblig = "";

            String.prototype.trim = function () {
                return this.replace(/^\s+|\s+$/g, "");
            }

            if (document.getElementById("<%= txtCuentaDepositaria.ClientID %>").value.trim() == "")
                strMsjCampOblig += "\t-Cuenta Depositaria\n";

            if (document.getElementById("<%= ddlPortafolio.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Portafolio\n";

            if (document.getElementById("<%= ddlFisicoAnotacion.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Tipo\n";

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }

        function ValidarCabecera() {
            strMensajeError = "";
            if (ValidaCamposCabecera()) {
                return true;
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
        }

        function ValidarDetalle() {
            strMensajeError = "";
            if (ValidaCamposDetalle()) {
                return true;
            }
            else {
                alertify.alert(strMensajeError);
                return false;
            }
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
                    <h2>Mantenimiento de Custodios</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Custodio</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="12" /><asp:RequiredFieldValidator ErrorMessage="C&oacute;digo Custodio"
                                ControlToValidate="tbCodigo" runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgCabecera" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" MaxLength="50" /><asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n"
                                ControlToValidate="tbDescripcion" runat="server" Text="(*)" CssClass="validator"
                                ValidationGroup="vgCabecera" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
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
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Ingresar" runat="server" ID="btnMostrarDetalle" ValidationGroup="vgCabecera" />
                <asp:Button Text="Retornar" runat="server" ID="btnRetornarDetalle" CausesValidation="false" />
                </div>
            </div>
        </fieldset>
        <br />
        <div runat="server" id="divDetalle">
            <fieldset>
                <legend>Detalle Cuentas Depositarias</legend>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Cuentas Depositarias</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtCuentaDepositaria" /><asp:RequiredFieldValidator
                                    ErrorMessage="Cuentas Depositarias" ControlToValidate="txtCuentaDepositaria"
                                    runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Nombre Cuenta</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtNombreCuenta" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Portafolio</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                                <asp:RequiredFieldValidator ErrorMessage="Portafolio" ControlToValidate="ddlPortafolio"
                                    runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tipo</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlFisicoAnotacion" Width="150px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Tipo" ControlToValidate="ddlFisicoAnotacion"
                                    runat="server" Text="(*)" CssClass="validator" ValidationGroup="vgDetalle" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Observaciones</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Rows="6" Width="320px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Situacion</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlSituacionCuentaDep" Width="150px" />
                                </div>
                            </div>
                        </div>
                        <div class="row" style="text-align: right; height: 70px; padding-top: 30px;">
                            <asp:Button Text="Agregar" runat="server" ID="btnAgregarDetalle" ValidationGroup="vgDetalle" />&#32;
                            <asp:Button Text="Modificar" runat="server" ID="btnModificarDetalle" ValidationGroup="vgDetalle" />
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset>
                <legend>Cuentas Depositarias</legend>
                <div class="grilla-small">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                    <asp:GridView runat="server" SkinID="GridSmall" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoCuentaDepositaria") & "|" & CType(Container, GridViewRow).RowIndex %>'
                                        CommandName="Modificar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoCuentaDepositaria") & "|" & CType(Container, GridViewRow).RowIndex %>'
                                        CommandName="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoCuentaDepositaria" HeaderText="Nro. Cuenta"></asp:BoundField>
                            <asp:BoundField DataField="NombreCuenta" HeaderText="Descripci&#243;n">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="CodigoPortafolioSBS" HeaderText="Portafolio"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="FisicoAnotacion"></asp:BoundField>
                            <asp:BoundField DataField="NombreFisicoAnotacion" HeaderText="Tipo"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="Estado" HeaderText="Tipo"></asp:BoundField>
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&oacute;n"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <%--                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </fieldset>
            <br />
            <header>
            </header>
            <div class="row" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
            </div>
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ValidationGroup="vgCabecera"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    <asp:ValidationSummary runat="server" ID="vsDetalle" ValidationGroup="vgDetalle"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
