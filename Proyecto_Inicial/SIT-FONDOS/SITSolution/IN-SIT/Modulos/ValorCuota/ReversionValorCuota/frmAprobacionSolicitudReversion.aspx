<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAprobacionSolicitudReversion.aspx.vb"
    Inherits="Modulos_Contabilidad_frmAprobacionSolicitudReversion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Aprobación de Solicitud de Reversión</title>
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

            $("#divObervacionRechazoIngreso").dialog({ resizable: false, autoOpen: false,
                buttons: {
                    Rechazar: function () {
                        debugger;
                        if ($("#txtObservacionRechazoIngreso").val() == "") {
                            alertify.alert("Tiene que ingresar un motivo de rechazo de la solicitud de reversión de valor cuota.");
                        } else {
                            $("#hdMotivoRechazo").val($("#txtObservacionRechazoIngreso").val());
                            document.getElementById("<%= btnRechazarReversion.ClientID %>").click();
                            $("#divBackground").remove();
                            $(this).dialog("close");
                        }

                    },
                    Cerrar: function () { $("#divBackground").remove(); $(this).dialog("close"); }
                },
                hide: { effect: "fade", duration: 500 }
            });
            //Elimina el botón "X" del modal
            $("#divObervacionRechazoIngreso").dialog("option", "width", 531).parent('.ui-dialog').find('.ui-dialog-titlebar-close').remove();
            $("#btnRechazar").click(function () {
                event.preventDefault();
                $("#divObervacionRechazoIngreso").dialog('open');
                $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            });
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
                    <h2>Aprobación de Solicitud de Reversión</h2>
                </div>
            </div>
        </header>
        <div runat="server" id="divDetalle">
            <fieldset>
                <legend>Filtro de búsqueda</legend>
                <div class="row">
                    <div class="col-md-2">
                        <label class="col-sm-4 control-label">
                            Estado</label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlEstadoRegistro" runat="server" Width="150px" />
                    </div>
                    <div class="col-md-8">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <label class="col-sm-4 control-label">
                            Fecha</label>
                    </div>
                    <div class="col-md-2">
                        <div class="input-append">
                            <div id="divFechaInicial" runat="server" class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicial" SkinID="Date" Width="100px" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12" style="text-align: right; margin-top: 0px;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    </div>
                </div>
            </fieldset>
            <br />
            <legend>Solicitudes de Reversión</legend>
            <div class="grilla">
                <asp:GridView runat="server" SkinID="Grid_PageSize_15" ID="dgLista">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkOrden" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="hdCodigoEstado" runat="server" Value='<%# Eval("FlagAprobado") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false"/>
                        <asp:BoundField DataField="NOMBRE_FONDO" HeaderText="Fondo"/>
                        <asp:BoundField DataField="FechaInicial" HeaderText="Fecha Inicial" ItemStyle-HorizontalAlign="Center"/>                
                        <asp:BoundField DataField="FechaFinal" HeaderText="Fecha Final" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="Motivo" HeaderText="Motivo" ItemStyle-Width="260px" ItemStyle-CssClass="ClaseWrap"/>
                        <asp:BoundField DataField="DESCRIPCION_AREA" HeaderText="Área"/>
                        <asp:BoundField DataField="Responsable" HeaderText="Responsable" ItemStyle-Width="160px" ItemStyle-CssClass="ClaseWrap"/>
                        <asp:BoundField DataField="FlagAfectaValorCuota" HeaderText="Afecta Valor Cuota" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="ESTADO_REGISTRO" HeaderText="Estado del Registro" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Font-Bold="true" />
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
            <br />
            <header>
            </header>
            <div class="row">
                <div class="col-md-12" style="text-align: right;">
                    <asp:Button Text="Aprobar" runat="server" ID="btnAprobar" />
                    <asp:Button Text="Rechazar" runat="server" ID="btnRechazar" />
                </div>
            </div>
        </div>
    </div>
    <input id="hd" type="hidden" name="hd" runat="server" />
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
    <div id="divObervacionRechazoIngreso" title="Motivo de Rechazo de Solicitud de Reversión de Valor Cuota">
        <div class="form-horizontal">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group" style="font-size: 12px;">
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtObservacionRechazoIngreso" runat="server" TextMode="MultiLine"
                                    Rows="6" Width="460px" MaxLength="500" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-6" style="display: none">
        <asp:Button Text="Rechazar Solicitud" runat="server" ID="btnRechazarReversion" />
    </div>
    <div class="loading" align="center">
        <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargado..." />
    </div>
    </form>
</body>
</html>
