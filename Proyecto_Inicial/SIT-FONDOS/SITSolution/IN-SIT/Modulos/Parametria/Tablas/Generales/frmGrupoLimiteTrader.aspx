<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGrupoLimiteTrader.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Generales_frmGrupoLimiteTrader" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Grupo L&iacute;mite Trading</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Grupo L&iacute;mite Trading</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Descripci&oacute;n</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbDescripcion" />
                            <asp:RequiredFieldValidator ErrorMessage="Descripci&oacute;n" ControlToValidate="tbDescripcion"
                                ValidationGroup="vgCabecera" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Renta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="Tipo Renta" ControlToValidate="ddlTipoRenta"
                                ValidationGroup="vgCabecera" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Instrumento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoInst" Width="150px" />
                            <asp:RequiredFieldValidator ErrorMessage="Instrumento" ControlToValidate="ddlTipoInst"
                                ValidationGroup="vgCabecera" runat="server" />
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
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Ingresar" runat="server" ID="btnMostrarDetalle" ValidationGroup="vgCabecera" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset runat="server" id="divDetalle">
            <legend>Detalle Instrumentos</legend>
            <div class="row" runat="server" id="trNemonico">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Nem&oacute;nico</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbNemonico" Width="140px" />
                                <span class="add-on"><i class="awe-search"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="display: none" id="trTipo" runat="server">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo de Instrumento</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoInstrumento" Width="150px" />
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
                            <asp:DropDownList runat="server" ID="ddlSituacionDet" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregarDetalle" />
                    <asp:Button Text="Modificar" runat="server" ID="btnModificarDetalle" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoGrupLimTrader") & "|" & CType(Container, GridViewRow).RowIndex %>'
                                CommandName="Modificar"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <HeaderStyle Width="90px"></HeaderStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibnEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoGrupLimTrader") & "|" & CType(Container, GridViewRow).RowIndex %>'
                                CommandName="Eliminar"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Instrumento" HeaderText="Instrumento" />
                    <asp:BoundField DataField="NombreSituacion" HeaderText="Situacion" />
                    <asp:BoundField DataField="Valor" HeaderText="Valor" Visible="False" />
                    <asp:BoundField DataField="Situacion" HeaderText="Situacion" Visible="False" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header></header>
        <div class="row">
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnRetornarDetalle" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hd" />
    <asp:ValidationSummary runat="server" ID="vsCabecera" ValidationGroup="vgCabecera"
        ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
