<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfasesInfoDivRebLib.aspx.vb"
    Inherits="Modulos_ValorizacionCustodia_Custodia_frmInterfasesInfoDivRebLib" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Recuperación de Archivos</title>
    <script type="text/javascript">
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
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Recuperación de Archivos</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fecha de Operaci&oacute;n</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInformacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Fuente</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlEntidadExterna" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group" style="padding-left: 5px;">
                        <label class="col-sm-2 control-label">
                            Ruta</label>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="Myfile" Width="360px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:TextBox runat="server" ID="tbFechaRegistro" Style="visibility: hidden;" />
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla-small">
            <asp:GridView runat="server" ID="dgDirtectorioContenido" SkinID="GridSmall">
                <Columns>
                    <asp:TemplateField HeaderText="Acci&#243;n">
                        <ItemTemplate>
                            <asp:Button Text="Ver Contenido" runat="server" CommandName="VerContenido" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Nombre Archivo" HeaderText="Nombre Archivo" />
                    <asp:BoundField DataField="Tama&#241;o" HeaderText="Tama&#241;o" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Hora" HeaderText="Hora" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgArchivo">
                <Columns>
                    <asp:BoundField DataField="C&#243;digo Nemonico" HeaderText="C&#243;digo Nemonico" />
                    <asp:BoundField DataField="Fecha Informaci&#243;n" HeaderText="Fecha Informaci&#243;n" />
                    <asp:BoundField DataField="Concepto Ejercicio" HeaderText="Concepto Ejercicio" />
                    <asp:BoundField DataField="Tipo Distribucion" HeaderText="Tipo Distribucion" />
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="Factor" HeaderText="Factor" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="Fecha Acuerdo" HeaderText="Fecha Acuerdo" />
                    <asp:BoundField DataField="Fecha Corte" HeaderText="Fecha Corte" />
                    <asp:BoundField DataField="Fecha Registro" HeaderText="Fecha Registro" />
                    <asp:BoundField DataField="Fecha Entrega" HeaderText="Fecha Entrega" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Procesar" runat="server" ID="btnImportar" />
                <asp:Button Text="Salir" runat="server" ID="btnRetornar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="txtValidacion" />
    </form>
</body>
</html>
