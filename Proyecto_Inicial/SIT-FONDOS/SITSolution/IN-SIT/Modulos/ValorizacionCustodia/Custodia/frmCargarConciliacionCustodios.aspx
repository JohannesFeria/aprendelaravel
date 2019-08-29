<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCargarConciliacionCustodios.aspx.vb"
    Inherits="Modulos_Valorizacion_y_Custodia_Custodia_frmCargarConciliacionCustodios" %>

<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Conciliaci&oacute;n Inf. Custodios</title>
    <script>
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                for (var i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && document.forms[0].elements[i].name.indexOf('dgArchivo') > -1) {
                        if (document.forms[0].elements[i].disabled != true) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            } else { 
                for (var i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && document.forms[0].elements[i].name.indexOf('dgArchivo') > -1) {
                        document.forms[0].elements[i].checked = false;
                    }
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
                        Conciliaci&oacute;n Inf. Custodios</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="120px" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Consulta </label>
                        <div class="col-sm-4">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Custodio</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="dlCustodio" Width="150px" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="dlCustodio" InitialValue="0" runat="server" ErrorMessage="Seleccione Custodio"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Tipo Reporte </label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlTipoImpresion" Width="150px" AutoPostBack="true" >
                                <asp:ListItem Text ="PDF" Value ="1" />
                                <asp:ListItem Text ="Excel" Value ="2" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla" style ="height:200px; width:100%; overflow:auto;">
            <asp:GridView runat="server" SkinID="Grid_AllowPaging_NO" ID="dgArchivo"  >
                <Columns>
                    <asp:BoundField DataField="C&#243;digo Mnem&#243;nico" HeaderText="C&#243;digo Mnem&#243;nico" />
                    <asp:BoundField DataField="Descripci&#243;n" HeaderText="Descripci&#243;n" />
                    <asp:BoundField DataField="Unidades del Sistema" HeaderText="Unidades del Sistema"
                        DataFormatString="{0:0.0000000}" />
                    <asp:BoundField DataField="Unidades del Custodio" HeaderText="Unidades del Custodio"
                        DataFormatString="{0:0.0000000}" />
                    <asp:BoundField DataField="Diferencia" HeaderText="Diferencia" DataFormatString="{0:0.0000000}" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <input text="Nominal" onclick="SelectAll(this)" type="checkbox" name="SelectAllCheckBox" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkNominal" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoISIN" HeaderText="CodigoISIN" />
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS" HeaderText="CodigoPortafolioSBS" />
                    <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoCustodio" HeaderText="CodigoCustodio" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <asp:RadioButtonList ID="RbtnFiltro" runat="server" Width="320px">
            <asp:ListItem Value="No Registrados" Selected="True">Titulos en Custodia NO Registrados en SIT</asp:ListItem>
            <asp:ListItem Value="Conciliados">Conciliacion con Custodio</asp:ListItem>
            <asp:ListItem Value="Diferencias">Operaciones Pendientes de Compensar</asp:ListItem>
            <asp:ListItem Value="DiferenciasDet">Operaciones Pendientes de Compensar Detallado</asp:ListItem>
            <asp:ListItem Value="DiferenciasCartera_Custodia">Diferencias SIT Cartera-Custodia</asp:ListItem>
            <asp:ListItem Value="No Reportado">Titulos en SIT NO Reportados en Custodios</asp:ListItem> 
        </asp:RadioButtonList> 
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" />
                <asp:Button Text="Imprimir Todo" runat="server" ID="btnImprimirTodo" />
                <asp:Button Text="Procesar" runat="server" ID="btnProcesar" />
                <asp:Button Text="Conciliar" runat="server" ID="btnConciliar" />
                <asp:Button Text="Imprimir" runat="server" ID="btnGenerarReporte" />
                <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="_Validacion" runat="server" />
    </form>
</body>
</html>
