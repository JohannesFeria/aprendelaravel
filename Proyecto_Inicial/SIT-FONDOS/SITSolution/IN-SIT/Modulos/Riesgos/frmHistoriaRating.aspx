<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmHistoriaRating.aspx.vb"
    Inherits="Modulos_Riesgos_frmHistoriaRating" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporte de Historia de Rating</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <style type="text/css">
        .divGrilla
        {
            height: 420px; /*border: solid 1px #706f6f;*/
            overflow-y: auto;
            margin-bottom: 15px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnBuscar, #btnImportar').on('click', function (e) {
                $("#popup01").show();
                return true;
            });
        });

        function showModal_Nemonico() {
            var isin = $('#txtISIN').val();
            var sbs = $('#txtSBS').val();
            $("#hdModal").val("Nemonico");
            var mnemonico = $('#txtMnemonico').val();
            return showModalDialog('../ValorizacionCustodia/Custodia/frmBuscarInstrumento.aspx?vIsin=' + isin + '&vSbs=' + sbs + '&vMnemonico=' + mnemonico, '800', '600', '');
        }
        function showModal_Tercero() {
            return showModalDialog('../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Terceros', '1200', '600', '');
            $("#hdModal").val("Tercero");
        }
        function limpiar_Nemonico() {
            $('#txtISIN').val('');
            $('#txtSBS').val('');
            $('#txtMnemonico').val('');
            $('#txtDescripcion').val('');
            $('#txtMoneda').val('');
            return false;
        }        
    </script>
</head>
<body>
    <div id="popup01" class="winBloqueador" style="display: none; height: 150%;">
        <div class="winBloqueador-inner">
            <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 70px;" />
        </div>
    </div>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Mantenimiento de Rating</h2></header>
        <fieldset>
            <legend>Datos</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Tabla</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTabla" runat="server" AutoPostBack="True" Width="150px">
                                <asp:ListItem Text="Emisiones" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Terceros" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-2 control-label">
                            Fecha Inicio</label>
                        <div class="col-md-3">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                                <span class="add-on" id="imgFechaInterface"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                        <label class="col-md-3 control-label">
                            Fecha Fin</label>
                        <div class="col-md-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                                <span class="add-on" id="Span1"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div style="text-align: center;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    </div>
                </div>
            </div>
            <asp:Panel ID="pnValores" runat="server">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Código ISIN</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtISIN" Width="150px" MaxLength="12" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">
                                C&oacute;digo SBS</label>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtSBS" Width="150px" MaxLength="12" />
                            </div>
                            <label class="col-sm-2 control-label">
                                Mnem&oacute;nico</label>
                            <div class="col-sm-4">
                                <div class="input-append">
                                    <asp:TextBox runat="server" ID="txtMnemonico" Width="150px" MaxLength="100" />
                                    <asp:LinkButton CausesValidation="false" runat="server" ID="lkbModalMnemonico" OnClientClick="return showModal_Nemonico()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                    <asp:LinkButton CausesValidation="false" runat="server" ID="lbkLimpiarControl" OnClientClick="return limpiar_Nemonico()"><span class="add-on"><i class="awe-remove"></i></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                                Negocio
                            </label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlNegocio" runat="server" Width="150px">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnTerceros" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">
                                Codigo Tercero</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtCodigoTercero" Width="150px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">
                                Emisor</label>
                            <div class="col-sm-10">
                                <div class="input-append">
                                    <asp:TextBox runat="server" ID="txtEmisor" ReadOnly="true" />
                                    <asp:LinkButton runat="server" ID="lkbShowModal" OnClientClick="return showModal_Tercero()"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </fieldset>
        <hr />
        <fieldset>
            <legend>Resultados</legend>
            <div class="row divGrilla">
                <div class="col-md-12">
                    <div class="Grilla">
                        <asp:GridView runat="server" ID="gvReporte" AutoGenerateColumns="false" SkinID="Grid_AllowPaging_NO">
                            <Columns>
                                <asp:BoundField DataField="DesNegocio" HeaderText="Tipo Negocio" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Nemónico" />
                                <asp:BoundField DataField="Codigo" HeaderText="ISIN" />
                                <asp:BoundField DataField="CodigoTipoInstrumentoSBS" HeaderText="CodigoSBS" />
                                <asp:BoundField DataField="Emisor" HeaderText="Emisor" />
                                <asp:BoundField DataField="Rating" HeaderText="Rating" />
                                <asp:BoundField DataField="RatingOficial" HeaderText="Rating Interno" />
                                <asp:BoundField DataField="RatingFF" HeaderText="Fortaleza Financiera" />
                                <asp:BoundField DataField="Clasificadora" HeaderText="Clasificadora" />
                                <asp:BoundField DataField="LineaPlazo" HeaderText="Línea Plazo" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha Clasif." />


                                <%--<asp:BoundField DataField="Codigo" HeaderText="Codigo" />--%>
                                <%--<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />--%>


                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </fieldset>
        <hr />
        <asp:HiddenField ID="hdModal" runat="server" runat="server" />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnImportar" runat="server" Text="Importar" />
            <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte" />
        </div>
    </div>
    </form>
</body>
</html>
