﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmWarrantsListar.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmWarrantsListar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Negociacion Opciones</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row"><div class="col-md-6"><h2>Búsqueda-Opciones</h2></div></div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Operacion</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaOperacion" SkinID="Date"  />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar"  />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="upGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista"  >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnModificar" runat="server" SkinID="imgEdit" CommandName ="Modificar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoOrden")  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibnEliminar" runat="server"  SkinID="imgDelete" CommandName ="Eliminar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoOrden")  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Estado" HeaderText="Estado"  />                            
                            <asp:BoundField DataField="CodigoOrden" HeaderText="Codigo Orden"  />
                            <asp:BoundField DataField="CodigoPortafolioSBS" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <asp:BoundField DataField="TipoFondo" HeaderText="Opción"  />
                            <asp:BoundField DataField="Portafolio" HeaderText="Portafolio" ItemStyle-HorizontalAlign="Left" />                            
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="MontoNominalOperacion" HeaderText="NominalOrigen" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="Tasa" HeaderText="Prima"  DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="MontoNetoOperacion" HeaderText="Neto Operacion" DataFormatString="{0:#,##0.00}" />
                            <asp:BoundField DataField="FechaOperacion" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
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
            <div class="col-md-12" style="text-align: right;">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>