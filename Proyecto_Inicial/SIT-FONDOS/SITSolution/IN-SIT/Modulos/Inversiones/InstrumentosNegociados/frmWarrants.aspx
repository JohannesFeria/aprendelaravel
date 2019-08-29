<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmWarrants.aspx.vb" Inherits="Modulos_Inversiones_InstrumentosNegociados_frmWarrants" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Negociacion Opciones</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" Text="Orden de Inversión - Opciones" runat="server" /></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h2>
                        <asp:Label ID="lblAccion" Text="" runat="server" />
                    </h2>
                </div>
            </div>
        </header>

        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="up_busqueda" runat="server" UpdateMode ="Conditional">
            <ContentTemplate>
                        <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label runat="server" id="lblFondo" class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlFondo" Width="120px" AutoPostBack="true"  />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Operación</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlOperacion" Width="160px" >
                                <asp:ListItem Text ="Compra" Value="26"></asp:ListItem>
                                <asp:ListItem Text ="Venta" Value="27"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Opcion</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlOpcion" runat="server">
                                <asp:ListItem Text = "Warrant" Value ="Warrant"></asp:ListItem>
                                <asp:ListItem Text = "OTC" Value ="OTC"></asp:ListItem>
                                <asp:ListItem Text = "Exchange Traded" Value ="Exchange Traded"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo ISIN</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" onkeypress="Javascript:Numero();" ID="txtISIN" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">C&oacute;digo SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtSBS" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Moneda</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="lblMoneda" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                </div>
                <div class="col-sm-4">
                </div>
                <div class="col-sm-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
            </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset>
            <legend>Caracter&iacute;sticas del Valor</legend>
            <asp:UpdatePanel ID="up_valor" runat="server" UpdateMode ="Conditional"  >
            <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripcion</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="lblDescripcioIns" runat="server" Width="200px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Moneda</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtMoneda" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Tamaño Emision</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="LBLTamaño" runat="server" Width="120px" CssClass="Numbox-7" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Emisor</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="TXTEmisor" runat="server" Width="200px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Precio Ejercicio</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtPrecioEjercicio" runat="server" CssClass="Numbox-7" Width="120px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Emision</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="lblFechaEmision" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Garante</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtGarante" runat="server" Width="200px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Rango Bajo Ejercicio</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtRangoBajo" runat="server" Width="120px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Vencimiento</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtFechaVencimiento" runat="server" Width="104px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Subyacente</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtSubyacente" runat="server" Width="104px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Rango Alto Ejercicio</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtRangoAlto" runat="server" Width="120px" ReadOnly="true" />
                        </div> 
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Valor Nominal</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtValorNominal" runat="server" Width="120px" Text ="0" CssClass="Numbox-7" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnBuscar" />
            </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <fieldset>
            <legend>Datos de Operaci&oacute;n</legend>
            <asp:UpdatePanel ID="updDatosOperacion" runat="server" UpdateMode ="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Fecha Operación</label>
                                <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="tbFechaOperacion" Width="100px" ReadOnly ="true"/>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Fecha Vencimiento</label>
                                <div class="col-sm-7">
                                    <div id="imgFechaVcto" class="input-append date" runat="server">
                                        <asp:TextBox runat="server" ID="tbFechaLiquidacion" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Hora Operación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="tbHoraOperacion" SkinID="Hour" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Unidades Ordenadas</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtUnidades" Width="150px" CssClass="Numbox-7" AutoPostBack ="true"  />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Unidades Operación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtUnidadesOperacion" Width="150px" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Nocional</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtNocional" Width="150px" Enabled ="false" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                       <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Prima</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtPrima" Width="120px" CssClass="Numbox-7" /> <b> - %</b>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Monto Operacion</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtMontoOperacion" Width="150px" CssClass="Numbox-7" Enabled ="false" />
                                    <asp:CheckBox ID="chkajuste" runat="server" Text="Ajustar" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Grupo de Intermediarios</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlGrupoInt" Width="150px" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Intermediario</label>
                                <div class="col-sm-7">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlIntermediario" Width="270px" AutoPostBack="True" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlGrupoInt" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Contacto</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlContacto" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Observación</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtObservacion" MaxLength="20" Width="300px" Style="text-transform: uppercase" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                    <asp:PostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
        <br />
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6" style="text-align: right;">
                
                        <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                        <asp:Button Text="Aceptar" runat="server" ID="btnAceptar"   UseSubmitBehavior="false" />
                        <asp:Button Text="Salir" runat="server" ID="btnRetornar"  />
                 
            </div>
        </div>
        <asp:HiddenField ID="hdPagina" runat="server" />
        <asp:HiddenField ID="hdCodigoOrden" runat="server" />
    </div>
    </form>
</body>
</html>
