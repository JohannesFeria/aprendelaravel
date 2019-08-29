<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGrupoInstrumento.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Valores_frmGrupoInstrumento" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Universo de L&iacute;mites</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Universo de L&iacute;mites</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="uptipos" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-8 control-label">
                                        C&oacute;digo Universo de L&iacute;mites</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox runat="server" ID="tbCodigoGrupoInstrumento" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-8 control-label">
                                        Descripci&oacute;n Grupo</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox runat="server" ID="tbDescripcion" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-8 control-label">
                                        Situaci&oacute;n</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList runat="server" ID="ddlSituacion" Width="100px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-8 control-label">
                                        Caracteristica</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList runat="server" ID="ddlCaracteristica" Width="180px" AutoPostBack="True" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div class="row">
                            <div class="col-md-5" style="text-align: center;">
                                Valores Disponibles
                            </div>
                            <div class="col-md-2" style="text-align: center;">
                            </div>
                            <div class="col-md-5" style="text-align: center;">
                                Valores por Agregar
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-5" style="text-align: right;">
                                <asp:ListBox ID="lbxValores" TabIndex="12" runat="server" Width="100%" Height="120px"
                                    Style="background-image: none; background-color: #fff;"></asp:ListBox>
                            </div>
                            <div class="col-md-2" style="text-align: center;">
                                <div class="row">
                                    <asp:Button ID="btnAgregarTodosCaracteristica" Text=">>" runat="server" SkinID="btnSmall" /></div>
                                <div class="row">
                                    <asp:Button ID="btnAgregarCaracteristica" Text=">" runat="server" SkinID="btnSmall" /></div>
                                <div class="row">
                                    <asp:Button ID="btnDevolverCaracteristica" Text="<" runat="server" SkinID="btnSmall" /></div>
                                <div class="row">
                                    <asp:Button ID="btnDevolverTodosCaracteristica" Text="<<" runat="server" SkinID="btnSmall" /></div>
                            </div>
                            <div class="col-md-5">
                                <asp:ListBox ID="lbxSeleccionValores" TabIndex="17" runat="server" Width="100%" Height="120px"
                                    Style="background-image: none; background-color: #fff;"></asp:ListBox>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            </asp:UpdatePanel>
            

            <div class="row">
                <div class="col-md-12" style="text-align: right;">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="updGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgListaGrupoInstrumento">
                        <Columns>
                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaseNormativa") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField Visible="False" DataField="CodigoGrupoInstrumento" HeaderText="CodigoGrupoInstrumento" />
                            <asp:BoundField Visible="False" DataField="CodigoCaracteristica" HeaderText="CodigoCaracteristica" />
                            <asp:BoundField Visible="False" DataField="ValorCaracteristica" HeaderText="ValorCaracteristica" />
                            <asp:BoundField DataField="DescripcionValorCaracteristica" HeaderText="Descripcion Valor Caracteristica" />
                            <asp:BoundField DataField="ClaseNormativa" HeaderText="Clase Normativa" />
                            <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n" />
                            <asp:BoundField Visible="False" DataField="Vista" HeaderText="Vista" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnCancelar" CssClass="disabled" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hd" />
    </form>
</body>
</html>
