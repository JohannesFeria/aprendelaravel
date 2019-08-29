<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReversionValorCuota.aspx.vb"
    Inherits="Modulos_Contabilidad_frmReversarValorCuota" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Reversión de Valor Cuota</title>
    <style type="text/css">
        .ClaseWrap
        {
            white-space: normal;
        }
        .ocultarCol
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            $("#divMostrarAdicional").dialog({ resizable: false, autoOpen: false,
                buttons: { Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); } },
                hide: { effect: "fade", duration: 500 }
            });
            $("#divMostrarAdicional").dialog("option", "width", 680).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
            $(".verAdicional").click(function () {
                event.preventDefault();
                //                $('#dgDetalleInstrumento').empty();
                var prefijoControlEnGrilla = ObtenerPrefijoControlEnGrilla(this);
                Cargar_Data($("#" + prefijoControlEnGrilla + "tbAccionTemporal").val(), $("#" + prefijoControlEnGrilla + "tbSolucionDefinitivaSugerida").val(), $("#" + prefijoControlEnGrilla + "tbObservacionRechazo").val(), $("#" + prefijoControlEnGrilla + "tbEstadoRegistro").val());

                $("#divMostrarAdicional").dialog('open');
                $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');

            });

            $(document).keyup(function () {
                if (event.which == 27) {
                    $("#divBackground").remove();
                }
            });
            function Cargar_Data(accionTemporal, SolucionDefinitivaSugerida, ObservacionRechazo, EstadoRegistro) {
                debugger;
                $("#txtAccionTemporal").val(accionTemporal.toString());
                $("#txtSolucionDefinitivaSugerida").val(SolucionDefinitivaSugerida.toString());
                $("#txtObservacionRechazo").val(ObservacionRechazo.toString());
                if ($.trim(EstadoRegistro.toString().toUpperCase()) != "RECHAZADO") {
                    $("#divObservacionRechazo").hide();
                } else {
                    $("#divObservacionRechazo").show();
                }
            };
            function ObtenerPrefijoControlEnGrilla(controlEnGrilla) {
                var idControl = $(controlEnGrilla).attr('id').toString();
                var nombreEnArray = idControl.split('_');
                var prefijoControl = nombreEnArray[0] + '_' + nombreEnArray[1] + '_';
                return prefijoControl.toString();
            };

          
            $("div>.ui-dialog-buttonset>:button").addClass("btn btn-integra");
        });
        $('form').live("submit", function () {
            $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            ShowProgress();
        });
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Reversión de Valor Cuota</h2>
                </div>
            </div>
        </header>
        <div runat="server" id="divDetalle">

            <br />
            <legend>Solicitudes de Reversión por Ejecutar</legend>
            <div class="grilla">
                <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgLista">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibSeleccionarPE" runat="server" SkinID="imgCheck" CommandName="Seleccionar">
                                </asp:ImageButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol"/>
                        <asp:BoundField DataField="IdFondo" HeaderText="IdFondo" HeaderStyle-CssClass="ocultarCol" ItemStyle-CssClass="ocultarCol" />
                        <asp:BoundField DataField="NOMBRE_FONDO" HeaderText="Fondo"/>
                        <asp:BoundField DataField="FechaInicial" HeaderText="Fecha Inicial" ItemStyle-HorizontalAlign="Center"/>                
                        <asp:BoundField DataField="FechaFinal" HeaderText="Fecha Final" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="Motivo" HeaderText="Motivo" ItemStyle-Width="260px" ItemStyle-CssClass="ClaseWrap"/>
                        <asp:BoundField DataField="DESCRIPCION_AREA" HeaderText="Área"/>
                        <asp:BoundField DataField="Responsable" HeaderText="Responsable" ItemStyle-Width="160px" ItemStyle-CssClass="ClaseWrap"/>
                        <asp:BoundField DataField="FlagAfectaValorCuota" HeaderText="Afecta Valor Cuota" ItemStyle-HorizontalAlign="Center"/>
                        <asp:TemplateField HeaderText="Datos Adicionales" ItemStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibVer" runat="server" SkinID="imgEdit_Confirmacion" CommandName="VerdAdicional"
                                    CssClass="verAdicional" ToolTip="Ver Datos Adcionales" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Datos Adicionales" HeaderStyle-CssClass="ocultarCol">
                            <ItemTemplate>
                                <asp:TextBox ID="tbAccionTemporal" runat="server" Text='<%# Eval("AccionTemporal") %>' />
                                <asp:TextBox ID="tbSolucionDefinitivaSugerida" runat="server" Text='<%# Eval("SolucionDefinitivaSugerida") %>' />
                                <asp:TextBox ID="tbObservacionRechazo" runat="server" Text='<%# Eval("ObservacionRechazo") %>' />
                                <asp:TextBox ID="tbEstadoRegistro" runat="server" Text='<%# Eval("ESTADO_REGISTRO") %>' />
                            </ItemTemplate>
                             <ItemStyle CssClass="ocultarCol"/>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <header>
            </header>
            <div class="row">
                <div class="col-md-12" style="text-align: right;">
                    <asp:Button Text="Reversar" runat="server" ID="btnReversar" Enabled="false"/>
                    <asp:Button Text="Salir" runat="server" ID="btnSalir" />
                </div>
            </div>
            <br />

            <div class="row">
                <asp:Label ID="msgError" runat="server" CssClass="stlPaginaTexto"></asp:Label>
            </div>
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server" />
    <asp:HiddenField ID="hdRptaConfirmar" runat="server" />
    <input id="hdMotivoRechazo" type="hidden" name="hdMotivoRechazo" runat="server" />
    <div id="divMostrarAdicional" title="Datos Adicionales de la Solicitud de Reversión de Valor Cuota">
        <div class="form-horizontal">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group" style="font-size: 12px;">
                            <label class="col-sm-3 control-label">
                                <b>Acción Temporal:</b></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAccionTemporal" runat="server" ReadOnly="true" TextMode="MultiLine"
                                    Rows="6" Width="430px" MaxLength="500" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                <b>Solución Definitiva Sugerida:</b></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtSolucionDefinitivaSugerida" runat="server" ReadOnly="true" TextMode="MultiLine"
                                    Rows="6" Width="430px" MaxLength="500" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="divObservacionRechazo">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                <b style="color: Red">Motivo de Rechazo:</b></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtObservacionRechazo" runat="server" ReadOnly="true" TextMode="MultiLine"
                                    Rows="6" Width="430px" MaxLength="500" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="loading" align="center">
        <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargado..." />
    </div>
    </form>
</body>
</html>
