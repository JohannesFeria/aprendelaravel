<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaTerceroMandato.aspx.vb"
    Inherits="Modulos_Parametria_Tablas_Entidades_frmBusquedaTerceroMandato" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
</head>
<body>
    <form id="frmTerceroMandato" class="form-horizontal" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Clientes Mandatos</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripci&oacute;n
                        </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDescripcion" MaxLength="20" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtCodigoTercero" MaxLength="20" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Situaci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlSituacion" />
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
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnSeleccionar" runat="server" SkinID="imgCheck" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoTercero")  %>'
                                        OnCommand="Seleccionar" CommandName="Seleccionar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoTercero" HeaderText="CodigoTerceroo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripci&#243;n" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="CodigoTipoDocumento" HeaderText="C&#243;&#243;igoTipoDocumento"
                                Visible="false" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digoSBS" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="sinonimo" HeaderText="Sin&#243;nimo" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="NombreSectorEmpresarial" HeaderText="Sector Empresarial"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="NombrePais" HeaderText="Pa&#237;s" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n" ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <div class="row">
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
