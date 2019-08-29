<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultasPreOrden.aspx.vb"
    Inherits="Modulos_Inversiones_ConsultasPreOrden_frmConsultasPreOrden" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title>Consulta de Pre Ordenes y Ordenes de Inversión</title>
    <script type="text/javascript" language="javascript">
        function Confirmar() {
            var strMensajeConfirmacion = "";
            strMensajeConfirmacion = "¿Desea eliminar la Orden/PreOrden de Inversión?";

            return confirm(strMensajeConfirmacion);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <header>
                <div class="row">
                    <div class="col-md-8">
                        <h2><asp:Label ID="lblTitulo" runat="server">Consulta de Pre Ordenes y Ordenes de Inversión</asp:Label></h2>
                    </div>
                    <div class="col-md-4" style="text-align: right;">
                        <h3>
                            <asp:Label ID="lblAccion" runat="server"></asp:Label></h3>
                    </div>
                </div>
            </header>
            <fieldset>
                <legend></legend>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tipo de Renta</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTipoRenta" runat="server" Width="200px" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Portafolio</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlPortafolio" runat="server" Width="200px" AutoPostBack ="true"  />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Fecha Inicio</label>
                            <div class="col-sm-8">
                                <div class="input-append">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" Width="120px" />
                                        <span id="img1" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Fecha Fin</label>
                            <div class="col-sm-8">
                                <div class="input-append">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" Width="120px" />
                                        <span id="img2" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tipo de Operación</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTipoOperacion" runat="server" Width="200px" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Tipo Instrumento</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddltipoinstrumento" runat="server" Width="314px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Código ISIN</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtISIN" MaxLength="12" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Mnemónico o ticket</label>
                            <div class="col-sm-8">
                                <div class="input-append">
                                    <asp:TextBox runat="server" ID="txtMnemonico" />
                                    <asp:LinkButton runat="server" ID="btnBuscar"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                C&oacute;digo SBS</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtsbs" MaxLength="12" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8" style="text-align: right;">
                        <asp:Button Text="Buscar" runat="server" ID="btnConsulta" />
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset>
                <legend>Resultados de la Búsqueda</legend>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <asp:Label ID="lbContador" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <div class="grilla">
                <asp:GridView ID="dgordenpreorden" runat="server" SkinID="Grid">
                    <Columns>
                        <asp:TemplateField HeaderText="Seleccionar">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibSeleccionarPE" runat="server" SkinID="imgCheck" CommandName="Seleccionar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaLiquidacion" HeaderText="Liquidación">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaContrato" HeaderText="Fin Contrato">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="codigoPortafolioSBS" HeaderText="Portafolio">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="codigoMnemonico" HeaderText="Mnem&#243;nico">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="tipoInstrumento" HeaderText="Tipo Instrumento">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Tasa" HeaderText="Tasa" DataFormatString="{0:#,##0.0000000}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CodigoOrden" HeaderText="C&#243;digo Orden">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TipoCambio" HeaderText="Tipo Cambio">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:#,##0.0000}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="MontoNetoOperacion" HeaderText="Monto" DataFormatString="{0:#,##0.0000}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CantidadOperacion" HeaderText="Cantidad" DataFormatString="{0:#,##0}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Intermediario" HeaderText="Interm.">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoOrden" HeaderText="CodigoOrden">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CategoriaInstrumento" HeaderText="CategoriaInstrumento">
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolio">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div class="row" id="tblMotivoEliminar" runat="server" style="display: none">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Motivo de Eliminar</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlMotivoEliminar" runat="server" Width="280px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Comentarios eliminación</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox ID="txtComentarios" runat="server" Width="260px" MaxLength="150" Height="42px"
                                    TextMode="MultiLine" CssClass="mayusculas" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <header>
            </header>
            <div class="row">
                <div class="col-sm-12" style="text-align: right;">
                    <asp:Button runat="server" ID="imbModificar" Text="Modificar" />
                    <asp:Button runat="server" ID="imbEliminar" Text="Eliminar" />
                    <asp:Button runat="server" ID="ibConsultar" Text="Consultar" />
                    <asp:Button runat="server" ID="ibtnexportar" Text="Imprimir" />
                    <asp:Button runat="server" ID="ibSalir" Text="Salir" />
                </div>
            </div>
        </div>
        <input id="hdPortafolio" type="hidden" name="hdCodigoCuenta" runat="server" />
        <input id="hdNomPortafolio" type="hidden" name="hdCodigoCuenta" runat="server" />
        <input id="hdCodigoOrden" type="hidden" name="hdCodigoCuenta" runat="server" />
        <input id="hdFechaOperacion" type="hidden" name="hdFechaOperacion" runat="server" />
        <input id="hdCategoriaInstrumento" type="hidden" name="hdCategoriaInstrumento" runat="server" />
        <div style="display: none">
            <asp:Button runat="server" ID="btnpopup" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID ="ibtnexportar" />
    </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
