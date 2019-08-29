<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteLimitesConsolidado.aspx.vb" Inherits="Modulos_Inversiones_Reportes_Limites_ReporteLimitesConsolidado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

<head id="Head1" runat="server">
    <title>Reporte Consolidado de Límites</title>

    <script type="text/javascript">

        $(document).ready(function () {

            $("#btnImprimir, #btnProcesar, #btnEnviarCorreo").click(function () {
                if (contarChecksSeleccionadosEnGrilla('dgLimite', 'chkSelect') == 0) {
                    alertify.alert('Primero debe seleccionar uno o mas portafolios');
                    return false;
                }

                if ($(this).attr("id") == "btnProcesar" || $(this).attr("id") == "btnEnviarCorreo" )
                    $("#popup01").show();

                return true;
            });

        });

                $('form').live("submit", function () {
//                    $('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
//                    $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');

                    var htmlImg = '<img src="../../../../App_Themes/img/icons/loading.gif" alt="Cargando..."  style="height: 70px;"/>';
                    $('body').append('<div class="winBloqueador"><div class="winBloqueador-inner">' + htmlImg + '</div></div>');
                    ShowProgress();
                });

    </script>
</head>
<body>

    <div id="popup01" class="winBloqueador" style="display: none; height: 150%;">
        <div class="winBloqueador-inner">
            <img src="../../../../App_Themes/img/icons/loading.gif" alt="Cargando..."  style="height: 70px;"/>
        </div>
    </div>

    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><h2>Reporte de Consolidado de L&iacute;mites</h2></header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">Fecha L&iacute;mite</label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" AutoPostBack = "true" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>                    
                </div>                
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-sm-12" style="text-align:left;">                        
                            <%--
                            <asp:Button ID="btnProcesarTodo" runat="server" Text="Procesar Todo" AutoPostBack = "true" Visible = "false"/>
                            <asp:Button ID="btnImprimirTodo" runat="server" Text="Imprimir Todo" AutoPostBack = "true"/>
                            --%>
                            
                            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" AutoPostBack = "true"/>
                            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" AutoPostBack = "true"/>
                            <asp:Button ID="btnEnviarCorreo" runat="server" Text="Enviar Correo" AutoPostBack = "true"/>

                            <asp:CheckBox ID="chbExcel" runat="server" />
                            <label>Excel</label>
                        </div>
                    </div>
                </div>
            </div>            
        </fieldset>
        <br />

        <div class="grilla">                
            <asp:GridView ID="dgLimite" runat="server" Width="100%" AutoGenerateColumns="false" AutoPostBack = "true"
                CellPadding="1" SkinID="Grid_AllowPaging_NO" GridLines="None" OnRowDataBound = "dgLimite_RowDataBound">
                <Columns>
                    <%-- INICIO | PROYECTO FONDOS LIMITES | ZOLUXIONES | CRumiche | 2018-09-27 | Requerimiento Agregar Imprimir Multiple --%>
                    <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderTemplate>
                            <input onclick="seleccionarChecksEnGrilla(this,'dgLimite','chkSelect')" type="checkbox"  >                            
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# (Eval("EsPortafolioValorizado") = "1") %>' ></asp:CheckBox>
                            <input id="hdIdPortafolio" type="hidden" runat="server" value='<%# Eval("CodigoPortafolioSBS") %>' />                    
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- FIN | PROYECTO FONDOS LIMITES | ZOLUXIONES | CRumiche | 2018-09-27 | Requerimiento Agregar Imprimir Multiple --%>

                    <asp:BoundField DataField="Descripcion" HeaderText="Portafolio" ></asp:BoundField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="Codigo" Visible = "false"></asp:BoundField>                                                
                    <asp:TemplateField HeaderText="Estado" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate >
                            <asp:Literal ID = "ltlEstado" runat = "server" text = '<%# Eval("Estado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- INICIO | PROYECTO FONDOS LIMITES | ZOLUXIONES | CRumiche | 2018-10-04 | No Aplican los botones Imprimir y Procesar --%>
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibProcesar" runat="server" SkinID="imgProcesar" OnCommand = "Procesar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPortafolioSBS") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibImprimir" runat="server" SkinID="imgImprimir" OnCommand="Imprimir"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPortafolioSBS") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- FIN | PROYECTO FONDOS LIMITES | ZOLUXIONES | CRumiche | 2018-10-04 | No Aplican los botones Imprimir y Procesar --%>
                                           
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>

