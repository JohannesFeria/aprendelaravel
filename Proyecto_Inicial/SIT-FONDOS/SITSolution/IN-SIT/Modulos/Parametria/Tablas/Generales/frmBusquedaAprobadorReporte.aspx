<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaAprobadorReporte.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaAprobadorReporte" %>

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
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Usuario</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoUsuario" />
                                <asp:LinkButton runat="server" OnClientClick="return showModal();" ID="lkbModal" CausesValidation="false"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
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
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEditar" runat="server" SkinID="imgEdit" CommandName="Editar"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoInterno") %>'>
                                    </asp:ImageButton>
                                    <input id="hdCodigoInterno" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.CodigoInterno") %>'>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoInterno") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoInterno" HeaderText="Codigo" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                            <asp:TemplateField HeaderText="Administrador">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAdmin" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.Administrador") %>'
                                        Enabled="True"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Firmante">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkFirma" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Firmante") %>'
                                        Enabled="True"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkOperador" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Operador") %>'
                                        Enabled="True"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DescripcionSituacion" HeaderText="Situacion" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
