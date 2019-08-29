<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMantenimientoPrecios.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmMantenimientoPrecios" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Mantenimiento Precios</title>
    <script language="javascript" type="text/javascript">
        function showPopupMnemonico() {
            var isin = document.getElementById('txtISIN').value;
            var sbs = document.getElementById('txtSBS').value;
            var nemonico = document.getElementById('txtMnemonico').value;
            return showModalDialog('../Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + nemonico, '800', '600', ''); 
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Mantenimiento de Precios</h2></header>
    <fieldset>
    <legend></legend>
           <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Tipo Instrumento</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlTipoInstrumento" runat="server" Width="120px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fuente Vec. Prec.</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlEntidadExterna" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Mnem&oacutenico</label>
                        <div class="col-sm-4">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtMnemonico" CssClass="input-medium" />
                                <asp:LinkButton ID="btnbuscar" runat="server" OnClientClick="return showPopupMnemonico();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Descripci&oacuten</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtDescripcion" runat="server" style="text-transform:uppercase;" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Inicio</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">Fecha Fin</label>
                            <div class="col-sm-4">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaOperacionFin" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                    </div>
               </div>
            </div>
              <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">C&oacutedigo ISIN</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtISIN" runat="server" style="text-transform:uppercase;" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">C&oacutedigo SBS</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtSBS" runat="server" style="text-transform:uppercase;" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2" style="text-align:right;">
                    <asp:Button ID="btnConsulta" runat="server" Text="Buscar" />                    
                </div>
            </div>
    </fieldset>
        <asp:UpdatePanel ID="upBuscar" runat="server">
            <ContentTemplate>
                <div class="grilla">
                    <asp:GridView ID="dgPrecios" runat="server" AutoGenerateColumns="False" GridLines="None"
                        SkinID="Grid" DataKeyNames="CodigoMnemonico,EntidadExt">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Imagebutton3" runat="server" SkinID="imgCheck" onrowcommand="dgPrecios_RowCommand"
                                        CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Descripción del Valor" HeaderText="Descripción del Valor" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Precio Fecha Seleccionada" DataFormatString="{0:#,##0.00000000}" HeaderText="Precio Limpio">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PrecioSucio" DataFormatString="{0:#,##0.00000000}" HeaderText="Precio Sucio">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PorcPrecioLimpio" DataFormatString="{0:#,##0.00000000}" HeaderText="Porc. Precio Limpio">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PorcPrecioSucio" DataFormatString="{0:#,##0.00000000}" HeaderText="Porc. Precio Sucio">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CodigoMnemonico" HeaderText="CodigoMnemonico" Visible="False" />
                            <asp:BoundField DataField="EntidadExt" HeaderText="EntidadExt" Visible="False" />
                        </Columns>
                    </asp:GridView>
                </div>
                <fieldset>
                    <br/>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Fecha Actual</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtFechaFin" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Precio Limpio</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCotizacionT" runat="server" ReadOnly ="true"  CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Precio Sucio</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtPrecioSucio" runat="server" ReadOnly ="true"  CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Porc Precio Limpio</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtPorcPrecioLimpio" runat="server" ReadOnly ="true"  CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">Porc Precio Sucio</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtPorcPrecioSucio" runat="server" ReadOnly ="true"  CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <header></header>
                <div class="row" style="text-align: right;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Visible="False" />
                    <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="False" />
                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnConsulta" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModificar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
    </div>
    </form>
</body>
</html>
