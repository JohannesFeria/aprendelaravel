<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmUsuariosNotifica.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmUsuariosNotifica" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Usuario Notificados</title>
    <script type="text/javascript">
        function showPopup() {
            return showModalDialog('frmBusquedaUsuariosNotifica.aspx?tlbBusqueda=Entidad', '1200', '600', '');    
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Usuario Notificados</h2></header>
    <br />
    <fieldset>
    <legend>Detalle Personal</legend>
    <div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-3 control-label">Nombre Completo</label>
            <div class="col-sm-9">
                <div class="input-append">
                    <asp:TextBox runat="server" ID="tbNombre" CssClass="input-medium" />
                    <asp:LinkButton ID="lkbBuscarNombre" runat="server" CausesValidation="false" OnClientClick="return showPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                </div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Nombre Completo" ControlToValidate="tbNombre">(*)</asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-3 control-label">Código Usuario</label>
            <div class="col-sm-9">
                <asp:textbox id="tbCodigoUsuario" runat="server" Width="147px" MaxLength="50"></asp:textbox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="Código Usuario" ControlToValidate="tbCodigoUsuario">(*)</asp:RequiredFieldValidator>
            </div>            
        </div>
    </div>
    </div>
    <div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-3 control-label">Unidad/Puesto</label>
            <div class="col-sm-9">
                <asp:textbox id="tbUnidad" runat="server" Width="264px" MaxLength="50"></asp:textbox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="Unidad/Puesto" ControlToValidate="tbUnidad">(*)</asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="col-md-6" style="text-align: right;">
       <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
    </div>
    </div>
    </fieldset>
    <br />
    <fieldset>
    <legend>Personal Asignado</legend>
        <asp:label id="lbContador" runat="server"></asp:label>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:UpdatePanel ID="UP1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoInterno") %>'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CodigoUsuario" HeaderText="C&#243;digo Usuario"></asp:BoundField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre Completo"></asp:BoundField>
                        <asp:BoundField DataField="NombreCentroCosto" HeaderText="Unidad/Puesto"></asp:BoundField>
                        <asp:BoundField Visible="False" DataField="CodigoInterno"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" 
            CausesValidation="False" />
        <asp:HiddenField ID="hdCodInterno" runat="server" />
        <asp:HiddenField ID="hdCodCentroCosto" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
