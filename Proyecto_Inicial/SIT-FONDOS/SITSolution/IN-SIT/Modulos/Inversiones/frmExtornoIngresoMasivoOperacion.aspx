<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoIngresoMasivoOperacion.aspx.vb" Inherits="Modulos_Inversiones_frmExtornoIngresoMasivoOperacion" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Extorno de Operaci&oacute;n</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-sm-6"><h2> Extorno de Operaci&oacute;n - <asp:Label ID="lbTipoRenta" runat="server" /></h2></div>
            </div>
        </header>
        <fieldset>
            <legend>Datos del Extorno</legend>
            <div class="row">
                <div class="grilla-small" style="height:100%;">
                    <asp:GridView ID="gvAsignacion" runat="server" SkinID="GridSmall" AutoGenerateColumns="False" PageSize="5">                                        
                        <Columns>
                            <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                            <asp:BoundField DataField="Asignacion" HeaderText="Asignación"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:BoundField DataField="CodigoOrden" HeaderText="Codigo Orden" />
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Portafolio" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Portafolio" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Tasa" HeaderText="Tasa" DataFormatString="{0:###,##0.00}" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:###,##0.00}" />
                    <asp:BoundField DataField="MontoOrigen" HeaderText="Monto Origen" DataFormatString="{0:###,##0.00}" />
                    <asp:BoundField DataField="MontoNetoOperacion" HeaderText="Monto" DataFormatString="{0:###,##0.00}" />
                    <asp:BoundField DataField="CantidadOperacion" HeaderText="Cantidad" DataFormatString="{0:###,##0}" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">Motivo de Eliminar</label>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlMotivoEliminar" Width="350px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label id="Label1" runat="server" class="col-sm-4 control-label">Comentarios eliminaci&oacute;n</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="5" Width="350px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12" style="text-align: right;">
                <asp:UpdatePanel ID="upBotones" runat="server">
                <ContentTemplate>
                    <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                    <asp:Button Text="Cancelar" runat="server" ID="btnCancelar" CausesValidation="false" />
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        </fieldset>
    </div>
    </form>
</body>
</html>