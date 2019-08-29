<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaVacaciones.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaVacaciones" %>

<!DOCTYPE html>
<html>
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
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            C&oacute;digo Usuario</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbCodigoUsuario" Width="120px" />
                                <asp:LinkButton ID="lkbBuscarUsuario" runat="server"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-9">
                   <div class="form-group">
                        <label class="col-sm-5 control-label" style="text-align: left; margin-left: 15px;">
                            <asp:Label Text="" ID="lbNombreUsuario" runat="server" />
                        </label>
                        <div class="col-sm-7"></div>
                    </div>
                </div>
            </div>            
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-9" style="text-align: right;">
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
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEditar" runat="server" SkinID="imgEdit" CommandName="Edit"
                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CodigoInterno") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoInterno" HeaderText="C&oacute;digo" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="FecINI" HeaderText="Fecha Inicio" />
                            <asp:BoundField DataField="FecFIN" HeaderText="Fecha Fin" />
                            <asp:BoundField DataField="DescripcionSituacion" HeaderText="Situaci&oacute;n" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
            <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
    </div>
    </form>
</body>
</html>
