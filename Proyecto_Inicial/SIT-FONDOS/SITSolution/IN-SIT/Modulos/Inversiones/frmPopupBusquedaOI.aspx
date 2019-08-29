<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPopupBusquedaOI.aspx.vb"
    Inherits="Modulos_Inversiones_frmPopupBusquedaOI" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Consulta Ordenes de Inversi&oacute;n</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Consulta Ordenes de Inversi&oacute;n</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend runat="server" id="lblTitulo"></legend>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            C&oacute;digo Postal</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="tbCodigo" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tipo Renta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlTipoRenta" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbSeleccionar" runat="server" CommandName="Seleccionar" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'>Seleccionar</asp:LinkButton>
                            <input type="hidden" id="hdNroOrden" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.CodigoOrden") %>'>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoOrden" HeaderText="Nro Orden" />
                    <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha" />
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" />
                    <asp:BoundField DataField="DescOperacion" HeaderText="Operaci&#243;n" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="MontoNetoOperacion" HeaderText="Monto Operaci&#243;n" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
