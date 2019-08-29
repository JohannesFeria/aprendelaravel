<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmImportarRating.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmImportarRating" %>
<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Importar Rating</title>
    <script type="text/javascript">
        function Confirmacion() {
            console.log('[[[Confirmacion]]]');
            var control;
            control = document.getElementById("<%= txtValidacion.ClientID %>");
            var Flag = control.value;
            if (Flag == '1') {
                var conf = window.confirm('¿El archivo ya ha sido cargado. Desea cargarlo nuevamente ?');
                if (conf == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        $(document).ready(function () {
            $('#btnProcesar').on('click', function (e) {
                $("#popup01").show();
                return true;
            });
        });

    </script>
</head>
<body>

    <div id="popup01" class="winBloqueador" style="display: none">
        <div class="winBloqueador-inner">
            <img src="../../../../App_Themes/img/icons/loading.gif" alt="Cargando..."  style="height: 70px;"/>
        </div>
    </div>

    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Importar Rating</h2></header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row" style="display:none" >
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-md-4 control-label">Fecha de Carga</label>
                        <div class="col-md-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaProceso" SkinID="Date" />
                                <span class="add-on" id="imgFechaInterface"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                        <label class="col-md-4 control-label" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-md-4 control-label">Tipo Archivo</label>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ddlTipoArchivo" />
                        </div>
                        <label class="col-md-4 control-label" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="col-md-4 control-label">Ruta</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="Myfile" runat="server" Width="577px" style="display:none;" />
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".xls,.xlsx"
                                class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar"
                                data-size="sm" style="width:300px;" >
                            <asp:HiddenField ID="hfRutaDestino" runat="server" />
                        </div>
                         <label class="col-md-4 control-label"></label>
                    </div>
                </div>
            </div>
            </label>
            </label>
        </fieldset>
        <br />
    
        <br />
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
            <asp:Button ID="btnSalir" runat="server" Text="Salir" />
        </div>
        <div class="row">
            <cr:CrystalReportViewer ID="crNoVector" Style="z-index: 102; left: 16px; position: absolute;
                top: 216px" runat="server" Width="350px" Visible="False" Height="50px"></cr:CrystalReportViewer>
        </div>
        <div class="row">
            <asp:Label ID="msgError" runat="server" CssClass="stlPaginaTexto" Visible="False"></asp:Label>
        </div>
        <div class="row">
            <asp:Label ID="msgTotalRegistrosProcesados" runat="server" CssClass="stlPaginaTexto" Visible="False"></asp:Label>
        </div>
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="gvResumenDataCargada" Width="100%" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Codigo" HeaderText="Código" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                    <asp:BoundField DataField="Rating" HeaderText="Rating"/>
                    <asp:BoundField DataField="RatingInterno" HeaderText="Rating Interno" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="txtValidacion" runat="server" />
    </form>
</body>
</html>
