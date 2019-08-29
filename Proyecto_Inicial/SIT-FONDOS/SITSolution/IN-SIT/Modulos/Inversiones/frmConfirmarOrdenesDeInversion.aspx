<%@ Page Language="VB" AutoEventWireup="true" CodeFile="frmConfirmarOrdenesDeInversion.aspx.vb"
    Inherits="Modulos_Inversiones_frmConfirmarOrdenesDeInversion" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts")%>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title>Confirmar &Oacute;rdenes </title>
    <style type="text/css">
        .ocultarCol
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        //        $(document).ready(function () {

        //        });
        function SelectAll(CheckBoxControl, dgControl) {
            var i;
            if (CheckBoxControl.checked == true) {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf(dgControl) > -1)) {
                        if (document.forms[0].elements[i].disabled != true) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
                document.getElementById('hdnSelect').value = "1";
            }
            else {
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
						(document.forms[0].elements[i].name.indexOf(dgControl) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
                document.getElementById('hdnSelect').value = "2";
            }
            document.getElementById('hdnGridViewSelect').value = dgControl;
            document.getElementById("<%= btnSeleccionar.ClientID %>").click();
        }

        function Eliminar() {
            var strNroOrden = "";
            strNroOrden = document.getElementById("lblNroTransaccion").innerText;
            if (strNroOrden != "") {
                if (confirm("¿Desea Eliminar el Nro. de Orden " + strNroOrden + "?")) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        function blmostrocult(div) {
            if (div == "divGrillaComisiones") {
                if (document.getElementById(div).style.display == "none") document.getElementById(div).style.display = "block";
                else document.getElementById(div).style.display = "none";
            }
            else {
                if (document.getElementById(div).style.height == '20px') {
                    document.getElementById(div).style.height = 'auto';
                } else {
                    document.getElementById(div).style.height = '20px';
                }
            }
            return false;
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa función javascript despues de ScriptManager para manejar eventos de updateprogress | 07/06/18 --%>
    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(
	        function () {
	            if (document.getElementById) {
	                var progress = document.getElementById('progressEspera');
	                var blur = document.getElementById('blurEspera');
	                var altoPage = document.documentElement.scrollHeight;
	                progress.style.width = '300px';
	                progress.style.height = '300px';
	                blur.style.height = '1200px';
	                //     progress.style.top = altoPage / 3 - progress.style.height.replace('px', '') / 2 + 'px';
	                progress.style.top = '300px'
	                progress.style.left = document.body.offsetWidth / 2 - progress.style.width.replace('px', '') / 2 + 'px';
	            }
	        }
            )

    </script>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se implementa función javascript despues de ScriptManager para manejar eventos de updateprogress | 07/06/18 --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid" id="divContainer">
                <header><h2>Confirmar &Oacute;rdenes</h2></header>
                <fieldset>
                    <legend>Filtro de búsqueda</legend>
                    <div class="row">
                        <div class="col-md-2">
                            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se cambia glosa 'Fondo' por 'Portafolio' | 07/06/18 --%>
                            <label class="col-sm-4 control-label">
                                Portafolio</label>
                            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se cambia glosa 'Fondo' por 'Portafolio' | 07/06/18 --%>
                        </div>
                        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega nuevo filtro "Tipo Instrumento" para búsqueda | 07/06/18 --%>
                        <div class="col-md-4">
                            <label class="col-sm-2 control-label" style="width: 100%; text-align: left">
                                Tipo Instrumento</label>
                        </div>
                        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega nuevo filtro "Tipo Instrumento" para búsqueda | 07/06/18 --%>
                        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se deshabilita fecha de operación | 28/06/18 --%>
                        <div class="col-md-3" id="divlblFechaOperacion" style="display: none">
                            <label class="col-sm-6 control-label" style="width: 100%; text-align: left">
                                Fecha de Operación</label>
                        </div>
                        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se deshabilita fecha de operación |28/06/18 --%>
                    </div>
                    <div class="row">
                        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se cambia glosa 'Fondo' por 'Portafolio' | 07/06/18 --%>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="ddlFondoOE" runat="server" AutoPostBack="True" Width="150px" />
                        </div>
                        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se cambia glosa 'Fondo' por 'Portafolio' | 07/06/18 --%>
                        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega nuevo filtro "Tipo Instrumento" para búsqueda | 07/06/18 --%>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlTipoInstrumento" runat="server" Width="320px" />
                        </div>
                        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se agrega nuevo filtro "Tipo Instrumento" para búsqueda | 07/06/18 --%>
                        <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se deshabilita fecha de operación | 28/06/18 --%>
                        <div class="col-sm-3">
                            <div class="col-sm-3" id="divtbFechaOperacion" style="display: none">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                        <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se deshabilita fecha de operación |28/06/18 --%>
                        <div class="col-md-3" style="text-align: right;">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>&Oacute;rdenes Ejecutadas&nbsp;<asp:Label ID="lblCantidadOE" runat="server">(0)</asp:Label>
                        <a onclick="return blmostrocult('divGrillaOrdenesEjecutada');" style="cursor: hand;
                            cursor: pointer; position: absolute; right: 20px;">[+/-] Expandir / Contraer
                        </a></legend>
                    <div class="grilla" style="overflow: scroll;" id="divGrillaOrdenesEjecutada">
                        <asp:GridView ID="dgListaOE" runat="server" SkinID="Grid_PageSize_15">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <input onclick="SelectAll(this,'dgListaOE')" type="checkbox" name="SelectAllCheckBoxOC"
                                            id="SelectAllCheckBoxOC" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibSeleccionarPE" runat="server" SkinID="imgCheck" CommandName="Seleccionar">
                                        </asp:ImageButton>
                                        <asp:CheckBox ID="chkSelectPE" runat="server" OnCheckedChanged="dgListaOE_CheckedChanged"
                                            AutoPostBack="True"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para eliminar ordenes Ejecutadas | 03/07/18 --%>
                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibBorrarOE" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                            ToolTip="Eliminar orden de Inversión"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para eliminar ordenes Ejecutadas | 03/07/18 --%>
                                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para ver detalle de Orden de Inversión | 03/07/18 --%>
                                <asp:TemplateField HeaderText="Ver Detalle" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibVer" runat="server" SkinID="imgEdit_Confirmacion" CommandName="Modificar"
                                            ToolTip="Ver Detalle de Orden de  Inversión"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para ver detalle de Orden de Inversión | 03/07/18 --%>
                                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para el detalle de errores de confirmación masiva | 03/07/18 --%>
                                <asp:TemplateField HeaderText="Observaciones" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibObservacion" runat="server" SkinID="imgAlert_Confirmacion"
                                            CommandName="LogAdvertencia" ToolTip="ver" Visible="false"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para el detalle de errores de confirmación masiva | 03/07/18 --%>
                                <%-- 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crean los nuevos campos dinámicos para mostrar en grilla | 04/07/18 --%>
                                <asp:BoundField DataField="FechaOperacion" HeaderText="FechaOperacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Fondo" HeaderText="Fondo" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Categoria" HeaderText="Categoria" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="NumeroTransaccion" HeaderText="NumeroTransaccion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="TipoOperacion" HeaderText="TipoOperacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="MontoOperacion" HeaderText="MontoOperacion" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="CodigoISIN" HeaderText="CodigoISIN" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TipoOrden" HeaderText="TipoOrden" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CantidadOperacion" HeaderText="CantidadOperacion" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoNominalOrdenado" HeaderText="MontoNominalOrdenado"
                                    DataFormatString="{0:#,##0.00}" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="TotalComisiones" HeaderText="TotalComisiones" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoNetoOperacion" HeaderText="MontoNetoOperacion" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="CodigoMoneda" HeaderText="CodigoMoneda" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="InteresCorridoNegociacion" HeaderText="InteresCorridoNegociacion"
                                    DataFormatString="{0:#,##0.00}" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="Bolsa" HeaderText="Bolsa" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="MontoOrigen" HeaderText="MontoOrigen" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoDestino" HeaderText="MontoDestino" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoCambio" HeaderText="TipoCambio" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="Contraparte" HeaderText="Contraparte" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="FechaLiquidacion" HeaderText="FechaLiquidacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="ValorNominaLocalCupon" HeaderText="ValorNominaLocalCupon"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TipoConfirmacion" HeaderText="TipoConfirmacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoMnemonico" HeaderText="CodigoMnemonico" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="Secuencial" HeaderText="Secuencial" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoSBS" HeaderText="CodigoSBS" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoOperacion" HeaderText="CodigoOperacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoTipoCupon" HeaderText="CodigoTipoCupon" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoTercero" HeaderText="CodigoTercero" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoTipoTitulo" HeaderText="CodigoTipoTitulo" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TasaCupon" HeaderText="TasaCupon" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoPortafolio" HeaderText="CodigoPortafolio" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoMnemonico" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol">
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaVencimiento" HeaderText="FechaVencimiento" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TasaPorcentaje" HeaderText="TasaPorcentaje" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Plazo" HeaderText="Plazo" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TipoCambioFuturo" HeaderText="TipoCambioFuturo" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoCambioSpot" HeaderText="TipoCambioSpot" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="CantidadOrdenado" HeaderText="CantidadOrdenado" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crean los nuevos campos dinámicos para mostrar en grilla | 04/07/18 --%>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="row" id="divBotonConfirmar" runat="server">
                        <hr />
                        <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" />
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>&Oacute;rdenes Confirmadas&nbsp;<asp:Label ID="lblCantidadOIC" runat="server">(0)
                    </asp:Label>
                        <a onclick="return blmostrocult('divGrillaOrdenesConfirmadas');" style="cursor: hand;
                            cursor: pointer; position: absolute; right: 20px;">[+/-] Expandir / Contraer
                        </a></legend>
                    <div class="grilla" style="overflow: scroll;" id="divGrillaOrdenesConfirmadas">
                        <asp:GridView ID="dgListaOC" runat="server" SkinID="Grid_PageSize_15">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <input onclick="SelectAll(this,'dgListaOC')" type="checkbox" name="SelectAllCheckBoxOC"
                                            id="SelectAllCheckBoxOC" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%-- <asp:ImageButton ID="ibSeleccionar" runat="server" SkinID="imgCheck" CommandName="Seleccionar">
                                        </asp:ImageButton>--%>
                                        <asp:CheckBox ID="chkSelectOC" runat="server" OnCheckedChanged="dgListaOC_CheckedChanged"
                                            AutoPostBack="True"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para eliminar ordenes Ejecutadas | 03/07/18 --%>
                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibBorrarOC" runat="server" SkinID="imgDelete" CommandName="Eliminar"
                                            ToolTip="Desconfirmar orden de Inversión"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para eliminar ordenes Ejecutadas | 03/07/18 --%>
                                <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para el detalle de errores de eliminación masiva | 03/07/18 --%>
                                <asp:TemplateField HeaderText="Observaciones" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibObservacionOC" runat="server" SkinID="imgAlert_Confirmacion"
                                            CommandName="LogAdvertencia" ToolTip="ver" Visible="false"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea columna para el detalle de errores de eliminación masiva | 03/07/18 --%>
                                <%-- 'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crean los nuevos campos dinámicos para mostrar en grilla | 04/07/18 --%>
                                <asp:BoundField DataField="FechaOperacion" HeaderText="FechaOperacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Fondo" HeaderText="Fondo" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="CodigoPortafolio" HeaderText="CodigoPortafolio" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Categoria" HeaderText="Categoria" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="NumeroTransaccion" HeaderText="NumeroTransaccion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="TipoOperacion" HeaderText="TipoOperacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="MontoOperacion" HeaderText="MontoOperacion" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="CodigoISIN" HeaderText="CodigoISIN" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TipoOrden" HeaderText="TipoOrden" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CantidadOperacion" HeaderText="CantidadOperacion" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoNominalOrdenado" HeaderText="MontoNominalOrdenado"
                                    DataFormatString="{0:#,##0.00}" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="TotalComisiones" HeaderText="TotalComisiones" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoNetoOperacion" HeaderText="MontoNetoOperacion" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="CodigoMoneda" HeaderText="CodigoMoneda" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="InteresCorridoNegociacion" HeaderText="InteresCorridoNegociacion"
                                    DataFormatString="{0:#,##0.00}" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"
                                    ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="Bolsa" HeaderText="Bolsa" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="MontoOrigen" HeaderText="MontoOrigen" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoDestino" HeaderText="MontoDestino" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoCambio" HeaderText="TipoCambio" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="Contraparte" HeaderText="Contraparte" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="FechaLiquidacion" HeaderText="FechaLiquidacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="ValorNominaLocalCupon" HeaderText="ValorNominaLocalCupon"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TipoConfirmacion" HeaderText="TipoConfirmacion" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoMnemonico" HeaderText="CodigoMnemonico" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="Secuencial" HeaderText="Secuencial" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="CodigoMnemonico" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol">
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaVencimiento" HeaderText="FechaVencimiento" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TasaPorcentaje" HeaderText="TasaPorcentaje" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="Plazo" HeaderText="Plazo" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol"></asp:BoundField>
                                <asp:BoundField DataField="TipoCambioFuturo" HeaderText="TipoCambioFuturo" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoCambioSpot" HeaderText="TipoCambioSpot" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="CantidadOrdenado" HeaderText="CantidadOrdenado" DataFormatString="{0:#,##0.00}"
                                    HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Right">
                                </asp:BoundField>
                                <asp:BoundField DataField="NumeroPoliza" HeaderText="NumeroPoliza" HeaderStyle-CssClass="ocultarCol"
                                    ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <%-- 'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crean los nuevos campos dinámicos para mostrar en grilla | 04/07/18 --%>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="row" id="divBotonEliminar" runat="server">
                        <hr />
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" />
                    </div>
                </fieldset>
                <br />
                <span></span>
                <asp:Panel ID="tblDetalleOI" runat="server">
                    <fieldset>
                        <legend>Total de Comisiones por Poliza <a onclick="return blmostrocult('divGrillaComisiones');"
                            style="cursor: hand; cursor: pointer; position: absolute; right: 20px;">[+/-] Expandir
                            / Contraer</a></legend>
                        <div id="divGrillaComisiones" style="display: none;">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="grilla-small">
                                        <asp:GridView ID="dgrComisiones" runat="server" Width="400px" SkinID="GridSmall">
                                            <Columns>
                                                <asp:BoundField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="200px" DataField="Descripcion" HeaderText="Descripcion"></asp:BoundField>
                                                <asp:BoundField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    DataField="ValorComision" HeaderText="Valor Comision"></asp:BoundField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <input type="hidden" runat="server" id="hdnCodigoComision" name="hdnCodigoComision" />
                                                        <input type="hidden" runat="server" id="hdnOldComision" name="hdnOldComision" />
                                                        <asp:TextBox ID="txtComision" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Total Comisión</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="lblTotalComision" runat="server" Width="150px" ReadOnly="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Código Operación</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="lblNroTransaccion" runat="server" Width="150px" ReadOnly="true" />
                                                    <asp:Label ID="lblCodigoISIN" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lblTipoOperacion" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lblTipoOrden" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lCategoria" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lFondo" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lNombreFondo" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lTipoConfirmacion" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lNemonico" runat="server" CssClass="hidden" />
                                                    <asp:Label ID="lSecuencial" runat="server" CssClass="hidden" />
                                                    <asp:HiddenField ID="hdnTipoSeleccion" runat="server" />
                                                    <asp:HiddenField ID="hndCodigoOperacion" runat="server" />
                                                    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea label oculto para guardar fecha de operación de grilla | 11/07/18 --%>
                                                    <asp:Label ID="lfechaOperacion" runat="server" CssClass="hidden" />
                                                    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se crea label oculto para guardar fecha de operación de grilla | 11/07/18 --%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="tbDetalleCA" runat="server">
                    <div class="row">
                        Detalle de Confirmación de Amortizaciones
                    </div>
                    <fieldset>
                        <legend></legend>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Fecha Vencimiento</label>
                                    <div class="col-sm-9">
                                        <asp:Label ID="lblFechaVencimientoAmort" runat="server" Width="120px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Monto Valor Nominal Local</label>
                                    <div class="col-sm-9">
                                        <asp:Label ID="lblValorNominalLocalAmort" runat="server" Width="128px">Cuponera</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="tblDetalleCV" runat="server">
                    <div class="row">
                        Detalle de Confirmación de Cupones Vencidos
                    </div>
                    <fieldset>
                        <legend></legend>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Fecha Vencimiento</label>
                                    <div class="col-sm-9">
                                        <asp:Label ID="lblFechaVencimiento" runat="server" Width="120px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Monto Valor Nominal Local</label>
                                    <div class="col-sm-9">
                                        <asp:Label ID="lblMontoCuponera" runat="server" Width="128px">Cuponera</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="tblDetalleDRL" runat="server">
                    <fieldset>
                        <legend>Detalle de Confirmación de&nbsp;Dividendos Rebates y Liberadas</legend>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Fecha Vencimiento</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="lblFechaVencimientoDRL" runat="server" Width="120px" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Monto Valor Nominal Local</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="lblMontoDRL" runat="server" Width="128px" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Tipo Operación</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="lblTipoOperacionDRL" runat="server" Width="120px" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Mnenónico</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="lblMnemonicoDRL" runat="server" Width="128px" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Moneda</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="lblMonedaDRL" runat="server" Width="120px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        <asp:Label ID="lIdentificador" runat="server" Width="96px" Visible="False"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="lEstado" runat="server" Width="96px" Visible="False" ReadOnly="true"></asp:TextBox>
                                        </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>
                <br />
                <header>
                </header>
                <div class="row" style="text-align: right;">
                    <asp:HiddenField ID="hdnSelect" runat="server" />
                    <asp:HiddenField ID="hdnGridViewSelect" runat="server" />
                    <div style="visibility: hidden">
                        <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" />
                        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" />
                    </div>
                    <asp:Button ID="btnAgruparAcciones" runat="server" Text="Carta Acciones" />
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" />
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id="blurEspera" />
                    <div id="progressEspera">
                        <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de eliminación  y confirmación | 07/06/18 --%>
            <asp:HiddenField ID="hdRptaDesConfirmar" runat="server" />
            <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
            <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea campo oculto para guardar respuesta de eliminación y confirmación | 07/06/18 --%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se traslada campos que no utilizaran | 07/06/18 --%>
    <fieldset class="hidden">
        <legend>CAMPOS OCULTOS</legend>
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Nro. Orden</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtNroOrdenOE" runat="server" Width="120px" />
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
    <%--'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Se traslada campos que no utilizaran | 07/06/18 --%>
    </form>
</body>
</html>
