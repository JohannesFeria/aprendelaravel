<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAgregarSeriesFirbi.aspx.vb" Inherits="Modulos_ValorCuota_frmAgregarSeriesFirbi" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Calculo de Series</title>
</head>
    <script type="text/javascript">
        <%--OT10927 - 21/11/2017 - Ian Pastor M. Muestra una ventana con el detalle de las operaciones de ventas de títulos.--%>
        $(function () {
            $("#divDetalleVentaTitulo").dialog({ resizable: false, autoOpen: false,
                buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
                hide: { effect: "explode", duration: 1000 }
            });
            //$("#divDetalleVentaTitulo").dialog("option", "width", 550);
            //Elimina el botón "X" del modal
            $("#divDetalleVentaTitulo").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
            $("#imgDetalleVT").click(function () {
                $("#divDetalleVentaTitulo").dialog('open');
                $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            });
        });

        function MostrarDetalleVentaTitulo() {
            $("#divDetalleVentaTitulo").dialog({ resizable: false, autoOpen: false,
                buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
                hide: { effect: "explode", duration: 1000 }
            });
            $("#divDetalleVentaTitulo").dialog("option", "width", 550).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
            $("#imgDetalleVT").click(function () {
                $("#divDetalleVentaTitulo").dialog('open');
                $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            });
        }
        //OT10927 - Fin
    </script>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <%--<header><h2>Calculo Valor Cuota Series</h2></header>--%>
        <div class="row">
         <br />
        <div class="col-md-3"> <font size="5">Calculo Valor Cuota Series</font></div>
           <asp:UpdatePanel ID="upControles" runat="server">
            <ContentTemplate>
                <div class="col-md-9" style="text-align:right;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Visible="false" UseSubmitBehavior="false" />
                    <asp:Button ID="btnProcesar" runat="server" Text="Procesar" UseSubmitBehavior="false" />
                    <asp:Button ID="Button1" runat="server" Text="Cancelar" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        </div>
        <hr />
        <asp:UpdatePanel ID="upCalculoValorCuota" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend></legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Portafolio</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbPortafolio" ReadOnly = "true" runat="server" Width="80px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fecha Informaci&oacute;n</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbFechaInforme" ReadOnly = "true" runat="server" Width="80px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="col-sm-4">
                                <asp:TextBox ID="tbSerie" ReadOnly = "true"  Width="80px" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">Valor Cuota Anterior</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtVCA" ReadOnly ="true" Width="150px" />
                                </div>
                            </div>                    
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">Diferencia Valor Cuota</label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtVCDiferencia" ReadOnly ="true" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upInversiones" runat="server">
            <ContentTemplate>
                <fieldset id="FormInversiones" runat="server" >
                    <legend>Inversiones</legend>
                    <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Inversiones [t-1]</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbInversionesT1" ReadOnly = "true" onkeypress="return soloNumeros(event)"  runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group ">
                                <label class="col-sm-4 control-label">Compras [t]</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbComprasT" ReadOnly = "true" onkeypress="return soloNumeros(event)"  runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Ventas y Venci. [t]</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbVentasVenciT" ReadOnly = "true" onkeypress="return soloNumeros(event)"  runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Rentabilidad [t]</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbRentabilidadT" ReadOnly = "true" onkeypress="return soloNumeros(event)" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Val. Forwards [t]</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValoriForwardT" ReadOnly = "true" onkeypress="return soloNumeros(event)" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label"><b>Inversiones [T] Subtotal</b> </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbInversionesSubTotal" ReadOnly = "true" CssClass="Numbox-7 NumBox"   runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label"><b>I.G.V.</b> </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtigv" ReadOnly = "true" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label"><b>Porcentaje de comisión</b> </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtporcom" ReadOnly = "true" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upPrecierre" runat="server">
            <ContentTemplate>
                <fieldset ID="FormPrecierre" runat="server">
                    <legend>Valor Cuota Precierre</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Caja (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCajaPrecierre" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">CxC Monto Liberada(Precierre)</label>
                                <div class="col-sm-8">
                                    <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosPrecierre"--%>
                                    <asp:TextBox ID="txtMontoDividendosPrecierre" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <%--OT10986 - 13/12/2017 - Ian Pastor M. Agregar columna CxP Liberada--%>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">CxP Liberada</label>
                                <div class="col-sm-8">
                                    <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosPrecierre"--%>
                                    <asp:TextBox ID="txtCxPLiberadaPreCierre" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <%--OT10986 - Fin--%>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">CXC Venta de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCVentaTitulo" runat="server" CssClass="Numbox-7" ReadOnly = "true" />
                                    <%--<img alt="Detalle" id="imgDetalleVT" src="../../App_Themes/img/icons/tree_list.png" />--%>
                                    <%--OT10927 - 22/11/2017 - Ian Pastor M. Control imagen que permite mostrar una ventana con el detalle de las CxC Venta de título --%>
                                    <asp:ImageButton Visible="false" ID="imgDetalleVT" runat="server" SkinID="imgMenu" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">Otras CXC</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrasCXC" runat="server" CssClass="Numbox-7" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">
                                CXC (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCPreCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div1" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">
                                CXC V. Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCExclusivos" runat="server" ReadOnly = "true" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otras CXC exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbotrasCXCExclusivos" runat="server" ReadOnly = "true" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CXP Compra de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPtitulo" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">
                                Otras CXP</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrasCXP" runat="server" CssClass="Numbox-7" 
                                        onkeypress="return soloNumeros(event)" />
                                    <input id="hdAjustesCxPPrecierre" type="hidden" runat="server" value="0" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">
                                CXP (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPPreCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div2" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">
                                Otras CXP Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPExclusivos" runat="server" ReadOnly = "true" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CxP C. de Título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPCompraTitulo" runat="server" ReadOnly = "true" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div id="Div3" runat="server" class="col-md-4" >
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otros Gastos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastos" runat="server" CssClass="Numbox-7" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otros Ingresos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresos" runat="server" CssClass="Numbox-7" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4" runat="server" visible="false">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                O. Gastos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastosExlusivos" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4" runat="server" visible="false">
                            <div class="form-group campo-omitido">
                                <label class="col-sm-4 control-label">
                                O. Ingresos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresosExlusivos" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div ID="FilaPrecierre" runat="server" class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                V. Pat. Precierre 1</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatriPrecierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Comisión SAFM</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbComiSAFM" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                V. Pat. Precierre 2</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatriPrecierre2" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                <b>V. Cuota Precierre (Cuota) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValCuotaPrecierre" runat="server" CssClass="Numbox-7" 
                                       />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                <b>V. Cuota Precierre (Valor) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValValoresPrecierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upCierre" runat="server" >
            <ContentTemplate>
                <fieldset ID="FormCierre" runat="server" visible="False">
                    <legend>Valor Cuota Cierre</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Aportes Cuotas</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbAporteCuota" runat="server" CssClass="Numbox-7" ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Aportes Valores</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbAporteValores" runat="server"  onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Aportes Liberadas</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtAporteLiberadas" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Rescates Cuotas</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbRescatesCuota" runat="server" CssClass="Numbox-7"  ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Rescates Valores</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbRescatesValores" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Retenciones pendientes</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtRetencionPendiente" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Caja</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCajaCierre" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CxC Monto Liberada(Cierre)</label>
                                <div class="col-sm-8">
                                    <%--OT10916 - 07/11/2017 - Ian Pastor M. Agregar caja de texto "txtMontoDividendosCierre"--%>
                                    <asp:TextBox ID="txtMontoDividendosCierre" runat="server" CssClass="Numbox-7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CXC Venta de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCVentaTituloCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otras CXC</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosCXCCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CXC (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div4" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CXC V. Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXCExclusivosCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CXP Compra de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPtituloCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otras CXP</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPotrasCierre" runat="server" CssClass="Numbox-7" ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CXP (Precierre)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrasCXPCierre" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div5" runat="server" class="row" visible="false">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otras CXP Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPExclusivosCierre" runat="server" ReadOnly = "true" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                CxP C. de título</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbCXPCompraTituloCierre" runat="server" ReadOnly = "true" 
                                        onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otros Gastos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastosCierre" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                Otros Ingresos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresosCierre" runat="server"  onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                O. Gastos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosGastosExlusivosCierre" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                O. Ingresos Exclusivos</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbOtrosIngresosExlusivosCierre" runat="server" onkeypress="return soloNumeros(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                V. Pat. Cierre(Cuota)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatCierreCuota" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                V. Pat. Cierre(Valor)</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValPatCierreValores" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                <b>V. Cuota Cierre(Cuota) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValCuotaCierreCuota" runat="server" ReadOnly = "true" 
                                        onkeypress="return soloNumeros(event)" 
                                        Style="font-weight: bold; background-color: #e3e3e3;" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                <b>V. Cuota Cierre(Valor) </b>
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbValCuotaCierreValores" runat="server" CssClass="Numbox-7" 
                                        ReadOnly = "true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProcesar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <%--Modal: Detalle Venta de títulos--%>
        <div id="divDetalleVentaTitulo" title="Detalle Cuentas por Cobrar Venta de Títulos">
            <div class="form-horizontal">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size:12px;">
                                <label class="col-sm-6 control-label"><b>Venta de títulos acumulado</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtVentaTitulosDetalle" runat="server" ReadOnly ="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="font-size:12px;">
                                <label class="col-sm-6 control-label"><b>Dividendos</b></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtDividendosDetalle" runat="server" ReadOnly ="true" CssClass="Numbox-7" Width="150px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
    
    </div>
    </form>
</body>
</html>
