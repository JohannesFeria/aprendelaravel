<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfasesSBSnet.aspx.vb" Inherits="Modulos_Gestion_Archivos_Planos_frmInterfasesSBSnet" %>
<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Vectores</title>
    <style type="text/css">
        .win01
        {
            border: solid 1px gray;
            background-color:White;
            position: absolute;
            z-index: 10;
            opacity: .7;
	        filter: alpha(opacity=70);
            width: 100%;
            height: 100%;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=btnProcesar.ClientID%>").click(function () {
                $("#popup01").show();
                $("#popup01_loading").show();
                return false;
            });

        });
        function Confirmacion() {
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
      
    </script>
</head>
<body>
    <div id="popup01" class="win01" style="display: none">
        <div class="winBloqueador-inner" id="popup01_loading">
            <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 120px;" />
        </div>
    </div>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <hr/>
        <h2>Importar Vectores</h2>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <asp:GridView ID="gvPip" runat="server" SkinID="GridFooter_Pip">
                            <Columns>
                                <asp:TemplateField HeaderText="Id" Visible="False">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltId" runat="server" Text='<%# Eval("Id") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha">
                                    <ItemTemplate>
                                        <div class="input-append date" id="divFecha" runat="server">
                                            <asp:TextBox runat="server" Text='<%# Eval("Fecha") %>' ID="tbFechaInterface" SkinID="Date"></asp:TextBox>
                                            <span class="add-on" id="imgFechaInterface"><i class="awe-calendar"></i></span>
                                        </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="input-append date">
                                            <asp:TextBox runat="server" Text='<%# Eval("Fecha") %>' ID="tbFechaInterfaceF" SkinID="Date"></asp:TextBox>
                                            <span class="add-on" id="imgFechaInterface"><i class="awe-calendar"></i></span>
                                        </div>
                                    </FooterTemplate>
                                    <ControlStyle Width="110px" />
                                    <FooterStyle Width="110px" />
                                    <ItemStyle Width="110px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interfaz">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdInterface" runat="server" Value='<%# Eval("CodInterfaz") %>' />
                                        <asp:DropDownList ID="ddlInterface" runat="server" Width="240px" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlInterfaceF" runat="server" Width="240px"  />
                                    </FooterTemplate>
                                    <ControlStyle Width="240px" />
                                    <ItemStyle Width="240px" HorizontalAlign="Center" />
                                    <FooterStyle Width="240px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ruta">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFile" runat="server" Width="100%" Text='<%# Eval("Nombre") %>'
                                            class="form-control input-sm" disabled="" />
                                        <asp:HiddenField ID="hfRutaDestino" runat="server" Value='<%# Eval("Nombre") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFileF" runat="server" Width="100%" Style="display: none;" />
                                        <asp:FileUpload class="filestyle" id="fpRutaF" runat="server" accept=".xls,.xlsx,.xlsm"
                                            data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" />
                                        <asp:HiddenField ID="hfRutaDestinoF" runat="server" />
                                    </FooterTemplate>
                                    <ControlStyle Width="280px" />
                                    <ItemStyle Width="280px" HorizontalAlign="Center" />
                                    <FooterStyle Width="280px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center" >
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDelete" runat="server" SkinID="imgDelete" CommandName="Delete" ToolTip="Eliminar Fila">
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgAdd" runat="server" SkinID="imgAdd" CommandName="Add" ToolTip="Agregar Fila"></asp:ImageButton>
                                    </FooterTemplate>
                                    <ControlStyle Width="50px" />
                                    <ItemStyle Width="50px" />
                                    <FooterStyle Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" 
                UseSubmitBehavior="false" Width="78px" />
             <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" />
            <asp:Button ID="Imagebutton3" runat="server" Text="Salir" />
        </div>
        <div class="row">
            <cr:CrystalReportViewer ID="crNoVector" Style="z-index: 102; left: 16px; position: absolute;
                top: 216px" runat="server" Width="350px" Visible="False" Height="50px"></cr:CrystalReportViewer>
        </div>
        <div class="row">
            <asp:Label ID="msgError" runat="server" CssClass="stlPaginaTexto"></asp:Label>
        </div>
        <div class="grilla" style="width:255px;">
            <asp:GridView runat="server" SkinID="Grid" ID="dgNoExiste" Width="30%">
                <Columns>
                    <asp:BoundField DataField="CodigoMonedaSBS" HeaderText="Código Moneda" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="txtValidacion" runat="server" />
    </form>
</body>
</html>
