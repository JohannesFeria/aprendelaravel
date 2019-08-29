<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGeneracionCuponeraNormal.aspx.vb" Inherits="Modulos_Parametria_AdministracionValores_frmGeneracionCuponeraNormal" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Cuponera Normal</title>
     <style type="text/css">
         .ocultarCol
         {
             display: none;
         }
         .win01
         {
             border: solid 1px gray;
             background-color: #80808052;
             position: absolute;
             z-index: 8;
             width: 100%;
             height: 100%;
             text-align: center;
         }
         .cont01
         {
             border: solid 1px gray;
             background-color: white;
             display: inline-block;
             margin-top: 120px;
             padding: 10px 20px;
             border-radius: 5px;
         }
         .tab01
         {
             width: 100%;
         }
         .tab01 > tbody > tr > td
         {
             padding: 2px 5px;
             border: Solid 1px gray;
         }
         .span01
         {
             font-size: 17px;
         }
         .selector
         {
             cursor: pointer;
             color:#0039b9;
             text-decoration: underline;
         }
     </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id*='ddlTipoAmortizacion']").change(function () {
                $("#btnAceptar").prop("disabled", true);
            });
            $("[id*='ddlBaseDias']").change(function () {
                $("#btnAceptar").prop("disabled", true);
            });
            $("[id*='ddlBaseDias']").change(function () {
                $("#btnAceptar").prop("disabled", true);
            });
            $("[id*='ddlBaseMes']").change(function () {
                $("#btnAceptar").prop("disabled", true);
            });
            $("[id*='txtTasaCupon']").change(function () {
                $("#btnAceptar").prop("disabled", true);
            });
            $("#Popup01_btnCerrar").click(function () { $("#popup01").hide(); });
            $("#Popup02_btnCerrar").click(function () { $("#popup02").hide(); });


        });
        function AgregarFilaTasaFijaVariable(tasaFija, tasaVariable) {
            $('#tablaTasas tbody').find("tr:gt(0)").remove();
            $('#tablaTasas tbody').append('<tr class="filaTasas"><td>' + tasaFija + '</td><td>' + tasaVariable + '</td></tr>');
            $("#popup01").show();
        }
        function MostrarDetalleParticipacion(_fechaIni, _fechaFin, _consecutivo, _estado, _montoNominalTotal, _tasaCupon, _difDias, _baseCupon, _amortizac, _sumaMontoAmortizacion) {
            var _codigoMnemonico = $("#txtCodigoNemonico").val();
            $("#spanCupon").html(" - Cupón Nro. " + _consecutivo.toString() + " - ");
            var param = { codigoMnemonico: _codigoMnemonico, fechaIni: _fechaIni, fechaFin: _fechaFin, consecutivo: _consecutivo, estado: _estado, montoNominalTotal: _montoNominalTotal, tasaCupon: _tasaCupon, difDias: _difDias, baseCupon: _baseCupon, amortizac: _amortizac, sumaMontoAmortizacion: _sumaMontoAmortizacion };
            $.ajax({
                url: "../../../MetodosWeb.aspx/Get_CuponeraNormal_ObtenerPorcentajeParticipacion",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8;",
                success: function (data) {
                    if (data.d.length > 0) {
                        $("#popup02_loading").hide();
                        $('#tablaParticipacion tbody').find("tr:gt(0)").remove();
                        for (var i = 0; i < data.d.length; i++) {
                            debugger;
                            $('#tablaParticipacion > tbody:last-child').append('<tr><td>' + data.d[i].Descripcion + '</td><td style="text-align: right;">' + data.d[i].Principal + '</td><td style="text-align: right;">' + data.d[i].Amortizacion + '</td><td style="text-align: right;">' + data.d[i].Interes + '</td><td style="text-align: right;">' + data.d[i].Participacion + '</td></tr>');
                        };
                        $("#popup02").show();
                    }
                    else {
                        $("#popup02").hide();
                        alert("No se pudieron cargar los datos correctamente.");
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) { $("#popup02").hide(); alert(textStatus); }
            });
        }
    </script>
</head>
<body>
    <div id="popup01" class="win01" style="display: none">
        <div class="cont01">
            <span class="span01">Detalle de Tasa Cupón Fija/Variable</span><br /><br />
            <table id="tablaTasas" class="tab01">
                <tr style="background-color: #e3d829; font-weight: bold; color:#0039A6;">
                    <td>
                        Tasa Fija
                    </td>
                    <td>
                        Tasa Variable
                    </td>
                </tr>
            </table>
            <br />
            <input type="submit" value="Cerrar" id="Popup01_btnCerrar" class="btn btn-integra"
                style="min-width: 80px; text-align: center; width: auto;" />
        </div>
    </div>
    <div id="popup02" class="win01" style="display: none">
        <div class="winBloqueador-inner" id="popup02_loading">
            <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
        </div>
        <div class="cont01">
            <span class="span01">Detalle de Participación por Portafolio</span><br /><br />
            <span id="spanCupon"></span>
            <br />
            <table id="tablaParticipacion" class="tab01">
                <tr style="background-color: #e3d829; font-weight: bold;color:#0039A6;">
                    <td>
                        Portafolio
                    </td>
                    <td>
                        Principal
                    </td>
                         <td>
                        Amortización
                    </td>
                         <td>
                        Interés
                    </td>
                        <td>
                        Participación %
                    </td>
                </tr>
            </table>
            <br />
            <input type="submit" value="Cerrar" id="Popup02_btnCerrar" class="btn btn-integra" style="min-width: 80px;
                text-align: center; width: auto;" />
        </div>
    </div>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Cuponera Normal</h2></div>
            </div>
        </header>
        
        <fieldset id="pnlPeriodico" runat="server">
            <legend></legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtCodigoNemonico" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">C&oacute;digo Isin</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtCodigoIsin" Width="150px" />
                        </div>
                    </div>
                </div>

                 <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Mónto Nominal</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtMontoNominal" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Emisi&oacute;n</label>
                        <div class="col-sm-7">
                            <div class="input-append date" id="divFechaEmision" runat="server">
                                <asp:TextBox runat="server" ID="txtFechaEmision" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha 1re Cupón</label>
                        <div class="col-sm-7">
                            <div class="input-append date" id="divFechaPrimerCupon" runat="server">
                                <asp:TextBox runat="server" ID="txtFechaPrimerCupon" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Vencimiento</label>
                        <div class="col-sm-7">
                            <div class="input-append date" id="divFechaVencimiento" runat="server">
                                <asp:TextBox runat="server" ID="txtFechaVencimiento" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo Amortizacion</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoAmortizacion" runat="server" Style="width: 150px;" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Periodicidad</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlPeriodicidad" runat="server" Style="width: 150px;" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Base</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlBaseDias" runat="server" Width="75px" />
                            <asp:DropDownList ID="ddlBaseMes" runat="server" Width="75px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa Cupón</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtTasaCupon" Width="150px" CssClass="Numbox-7" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo Tasa Variable</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTasaVariable" runat="server" Style="width: 150px;" AutoPostBack = "true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa Variable</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtTasaVariable" Width="75px" CssClass="Numbox-7" />
                            <asp:DropDownList ID="ddlPeriodicidadVariable" runat="server" Width="75px" AutoPostBack = "true" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" style="text-align: right; width: 62%;">
                    <asp:Button Text="Procesar" runat="server" ID="btnCalcularPeriodica" />
                </div>
            </div>
        </fieldset>
        <fieldset id="pnlAntiguo" runat="server" visible="false">
            <legend></legend>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">C&oacute;digo Mnem&oacute;nico</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigoNemonico" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">C&oacute;digo Isin</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigoIsin" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Emisi&oacute;n</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaEmision" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Vencimiento</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaVencimiento" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">1er Vencimiento Cupón</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaPrimer" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6" style="text-align: right;"> <asp:Button Text="Procesar" runat="server" ID="btnProcesar" /> </div>
            </div>
        </fieldset>
        <br />
      <%-- INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega campos "A partir de" y "%Amortización" | 18/05/18 --%>
        <fieldset id="fsAmortizacion" visible="false" runat="server">
            <legend>Amortización</legend>
            <div class="row">           
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label"> A partir de </label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlApartirDe" runat="server"  Width="150px" AutoPostBack="True"/>
                        </div>
                    </div>
                </div>

                <div class="col-md-5" id="divPorcentajeAmortizacion" runat="server">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            % Amortización</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbPorcentajeAmortizacion" Width="110px" CssClass="Numbox-0_100"  /> 
                             <div style="color: #FF0000; font-size: smaller">*[Valor Propuesto]</div>    
                        </div>
                   
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                
                     </div>
                  </div>
                 
                <div class="col-sm-6" style="text-align: right;"> <asp:Button Text="Aplicar" runat="server" ID="btnAmortizar" /> </div>
            </div>
        </fieldset>
        <%-- FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega campos "A partir de" y "%Amortización" | 18/05/18 --%>
        <br />
        <fieldset>
        <legend>Resultado de la Generación </legend>
            <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="gvCupones">
                <Columns>
                    <asp:BoundField DataField="Consecutivo" HeaderText="Nro." ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="FechaIni" HeaderText="Fecha Inicio" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="FechaFin" HeaderText="Fecha Término" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="DifDias" HeaderText="Dif. Días" ItemStyle-HorizontalAlign="Center"/>                                 
                    <%--<asp:BoundField DataField="TasaCupon" HeaderText="Tasa Cupón" DataFormatString="{0:#,##0.0000000}" ItemStyle-HorizontalAlign="Right" />--%>
                    <asp:TemplateField HeaderText="Tasa Cupón" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                               <asp:Label ID="lblTasaCupon" Text='<%# Eval("TasaCupon") %>' runat="server"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Amortizac" HeaderText="Amortización %"  DataFormatString="{0:#,##0.0000000}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="PrincipalNominal" HeaderText="Principal" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="AmortizacionNominal" HeaderText="Amortización" ItemStyle-Width="90" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>       
                    <asp:BoundField DataField="InteresNominal" HeaderText="Interés" ItemStyle-Width="90" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>                    
                 <%--   <asp:BoundField DataField="PorcentajeFondo" HeaderText="Participación %" ItemStyle-Width="90" DataFormatString="{0:#,##0.0000000}" ItemStyle-HorizontalAlign="Right"/>--%>
                    <asp:TemplateField HeaderText="Participación %" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblParticipacion" Text='<%# Eval("PorcentajeFondo") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="PrincipalFondo" HeaderText="Principal" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="AmortizacionFondo" HeaderText="Amortización" ItemStyle-Width="90" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>       
                    <asp:BoundField DataField="InteresFondo" HeaderText="Interés" ItemStyle-Width="90" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:TemplateField HeaderText="Estado" ItemStyle-Width="25px" Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdEstado" Value='<%# Eval("Estado") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BaseCupon" HeaderText="Base Cupon" ItemStyle-CssClass="ocultarCol"
                        HeaderStyle-CssClass="ocultarCol" />
                    <asp:TemplateField ItemStyle-Width="35px" HeaderText="Acción">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibAgregar" runat="server" SkinID="imgAdd" CommandName="Agregar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Consecutivo") %>' ToolTip="Agregar Nuevo Cupón.">
                            </asp:ImageButton>
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Consecutivo") %>'
                                CommandName="Modificar" ToolTip="Modificar Cupón."></asp:ImageButton>
                            <a onclick="return Confirmar()" href="javascript:;">
                                <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Consecutivo") %>'
                                    CommandName="Eliminar" ToolTip="Eliminar Cupón."></asp:ImageButton>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TasaVariable" HeaderText="Tasa Variable" ItemStyle-CssClass="ocultarCol"
                        HeaderStyle-CssClass="ocultarCol" />
                    <asp:BoundField DataField="TasaCupon" HeaderText="Tasa Cupón" ItemStyle-CssClass="ocultarCol"
                        HeaderStyle-CssClass="ocultarCol" />
                    <asp:BoundField DataField="AmortizacConsolidado" HeaderText="Amortización Consolidado" ItemStyle-CssClass="ocultarCol"
                        HeaderStyle-CssClass="ocultarCol" />
                </Columns>
            </asp:GridView>
                <asp:GridView runat="server" SkinID="Grid" ID="dgLista" Visible="false">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "consecutivo") %>'
                                CommandName="Modificar"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <a onclick="return Confirmar()" href="javascript:;"> <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" 
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "consecutivo") %>' CommandName="Eliminar"></asp:ImageButton> </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Consecutivo" HeaderText="Nro." />
                    <asp:BoundField DataField="FechaIni" HeaderText="Fecha Inicio" />
                    <asp:BoundField DataField="FechaFin" HeaderText="Fecha Termino" />
                    <asp:BoundField DataField="Amortizac" HeaderText="Amortizacion" />
                    <asp:BoundField DataField="DifDias" HeaderText="Dif. D&#237;as" />
                    <asp:BoundField DataField="TasaCupon" HeaderText="Tasa Cup&#243;n" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="BaseCupon" HeaderText="Base" DataFormatString="{0:0}" />
                    <asp:BoundField DataField="DiasPago" HeaderText="Dias Pago" />
                    <asp:BoundField Visible="False" DataField="consecutivo" HeaderText="consecutivo" />
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        <br />
        <fieldset runat="server" id="pnlCupon">
            <legend><asp:Label ID="lblAccionCupon" runat="server">Modificar</asp:Label> Cup&oacute;n</legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fecha Inicio</label>
                        <div class="col-sm-7">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" ReadOnly = "true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tasa Cupón</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbTasaCupon" Width="150px" CssClass="Numbox-7"/>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Tipo</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlTipoActCuponera" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Termino</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaTermino" SkinID="Date" AutoPostBack="true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
               <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Amortización %</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbAmortizacion" Width="150px" CssClass="Numbox-0_100"/>
                        </div>
                    </div>
                </div>
              
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Dif. D&iacute;as</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDifDias" Width="110px" CssClass="Numbox-0_5" />
                        </div>
                    </div>
                </div>
                  <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Base</label>
                     <%--   <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbBase" Width="120px" />
                        </div>--%>
                         <div class="col-sm-7">
                            <asp:DropDownList ID="ddlbaseDiasModificar" runat="server" Width="75px" AutoPostBack="true" />
                            <asp:DropDownList ID="ddlbaseMesModificar" runat="server" Width="75px"/>
                        </div>
                    </div>
                </div>
   <%--             <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            D&iacute;as Pago</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDiasPago" Width="120px" />
                        </div>
                    </div>
                </div>--%>
            </div>
    <%--        <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Nueva Dif. D&iacute;as</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbNuevaDifDias" Width="120px" />
                        </div>
                    </div>
                </div>
          
            </div>--%>
        </fieldset>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Modificar" runat="server" ID="btnAgregar" />
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Salir" runat="server" ID="btnRetornar" CausesValidation="false" />
                 <%-- INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Botón Recursivo para validar el porcentaje de amortización | 17/05/18--%>
                <asp:Button Text="Validar" runat="server" ID="btnValidar"/>
                <%-- FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Botón Recursivo para validar el porcentaje de amortización | 17/05/18--%>
            
            </div>
        </div>
     
    </div>
    <input id="hdConsecutivo" type="hidden" name="hdConsecutivo" runat="server" />
    <input id="hdFechaIni" type="hidden" name="hdFechaIni" runat="server" />
    <input id="hdFechaFin" type="hidden" name="hdFechaFin" runat="server" />
   
    </form>
</body>
</html>
