﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaContacto.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Entidades_frmBusquedaContacto" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Contacto</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Contacto</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Contacto</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="3" />
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
                            <asp:TextBox runat="server" ID="tbDescripcion" />
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
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoContacto") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoContacto") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoContacto" HeaderText="C&#243;digo" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n" />
                            <asp:BoundField DataField="TipoContacto" HeaderText="Tipo Contacto" />
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n" />
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
                <%--<asp:Button Text="Consultar" runat="server" ID="btnConsultar" />--%>
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
