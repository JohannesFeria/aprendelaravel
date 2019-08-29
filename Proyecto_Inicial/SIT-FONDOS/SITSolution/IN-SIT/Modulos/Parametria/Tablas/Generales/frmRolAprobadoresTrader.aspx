<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRolAprobadoresTrader.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmRolAprobadoresTrader" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Autorizados Aprobaciones Trader</title>
    <script type="text/javascript">
        function showPopupUsuarios() {
            return showModalDialog('../Valores/frmBusquedaUsuariosNotifica.aspx?tlbBusqueda=Personal', '1200', '600', '');                  
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
                        Autorizados Aprobaciones Trader</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Rol</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" />
                            <asp:RequiredFieldValidator ValidationGroup="vgCabecera" ErrorMessage="C&oacute;digo Rol"
                                ControlToValidate="tbCodigo" runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" />
                            <asp:RequiredFieldValidator ValidationGroup="vgCabecera" ErrorMessage="Descripci&oacute;n"
                                ControlToValidate="tbDescripcion" runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Cantidad Obligatoria</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCantidadP" Width="80px" CssClass="Numbox-0_2" />
                            <asp:RequiredFieldValidator ValidationGroup="vgCabecera" ErrorMessage="Cantidad Obligatoria"
                                ControlToValidate="tbCantidadP" runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Cantidad Alterna</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCantidadA" Width="80px"  CssClass="Numbox-0_2"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button Text="Ingresar" runat="server" ID="btnMostrarDetalle" ValidationGroup="vgCabecera" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Detalle Usuarios Aprobadores</legend>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Nombre Usuario</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbNombre" Width="220px" />
                                <asp:LinkButton ID="lkbBuscarUsuario" OnClientClick="javascript:return showPopupUsuarios();"
                                    runat="server"><span class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>
                            <asp:RequiredFieldValidator ValidationGroup="vgDetalle" ErrorMessage="Nombre Usuario"
                                ControlToValidate="tbNombre" runat="server" Text="(*)" CssClass="validator" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Aprobador</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoGrupo" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label" style="width: 0px;">
                        </label>
                        <div class="col-sm-8">
                            <asp:CheckBox Text="Aprobador" runat="server" ID="chkAprobador" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Renta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacionDet" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregarDetalle" ValidationGroup="vgDetalle" />
                    <asp:Button Text="Modificar" runat="server" ID="btnModificarDetalle" ValidationGroup="vgDetalle" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla-small">
            <asp:GridView runat="server" SkinID="GridSmall" ID="dgLista">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoRolTrader") & "|" & CType(Container, GridViewRow).RowIndex %>'
                                CommandName="Modificar"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoRolTrader") & "|" & CType(Container, GridViewRow).RowIndex %>'
                                CommandName="Eliminar"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NombreTipo" HeaderText="Tipo de Aprobador" />
                    <asp:BoundField DataField="IndAprobador" HeaderText="Aprobador" />
                    <asp:BoundField DataField="CodigoUsuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre" />
                    <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&oacute;n" />
                    <asp:BoundField DataField="CodigoInterno" HeaderText="C&oacute;digo Interno" Visible="False" />
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="False" />
                    <asp:BoundField DataField="Aprobador" HeaderText="Aprobador" Visible="False" />
                    <asp:BoundField DataField="Situacion" HeaderText="Situaci&oacute;n" Visible="False" />
                    <asp:BoundField DataField="CodigoRenta" HeaderText="CodigoRenta" Visible="False" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server">
    <input id="hdCodUsuario" type="hidden" name="hdCodUsuario" runat="server">
    <input id="hdCodInterno" type="hidden" name="hdCodInterno" runat="server">
    <asp:ValidationSummary runat="server" ID="vsCabecera" ValidationGroup="vgCabecera"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    <asp:ValidationSummary runat="server" ID="vsDetalle" ValidationGroup="vgDetalle"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
