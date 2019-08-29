<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmInterfasesInfCustodio.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Custodia_frmInterfasesInfCustodio" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Carga Inf. de Custodia</title>
    <script type="text/javascript">
        function Confirmacion() {
            var control;
            control = $('#txtValidacion');
            var Flag = control.value;
            if (Flag == '1') {
                var conf = confirm('¿El archivo ya ha sido cargado. Desea cargarlo nuevamente ?');
                if (conf == true) { return true; }
                else { return false; }
            }
        }    
    </script>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header><div class="row"><div class="col-md-6"><h2>Carga Inf. de Custodia</h2></div></div></header>
        <fieldset>
        <legend></legend>
        <asp:UpdatePanel ID="up_cabecera" runat="server" UpdateMode ="Conditional" >
        <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Custodio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlReporte" Width="220px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
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
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                </div>
            </div>
            <asp:HiddenField runat="server" ID="Myfile" />
        </ContentTemplate>
        </asp:UpdatePanel>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group" style="padding-left: 5px;">
                        <label class="col-sm-2 control-label">Ruta</label>
                        <div class="col-sm-10">
                            <input id="iptRuta" runat="server" name="iptRuta" type="file" accept=".txt" class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" style="width:300px;" />
                            <asp:HiddenField ID="hfRutaDestino" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:TextBox runat="server" ID="tbFechaRegistro" Style="visibility: hidden;" />
                    <asp:Button Text="Ver Contenido" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
    <asp:UpdatePanel ID="uptodo" runat="server">
    <ContentTemplate>
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgArchivo">
                <Columns>
                    <asp:BoundField DataField="C&#243;digo Custodio" HeaderText="C&#243;digo Custodio" />
                    <asp:BoundField DataField="Cuenta Depositaria" HeaderText="Cuenta Depositaria" />
                    <asp:BoundField DataField="C&#243;digo Portafolio" HeaderText="C&#243;digo Portafolio" />
                    <asp:BoundField DataField="C&#243;digo ISIN" HeaderText="C&#243;digo ISIN" />
                    <asp:BoundField DataField="Titulo" HeaderText="Titulo" />
                    <asp:BoundField DataField="Tipo Intermediario" HeaderText="Tipo Interm." />
                    <asp:BoundField DataField="C&#243;digo Intermediario" HeaderText="C&#243;digo Interm." />
                    <asp:BoundField DataField="Valor Nominal" HeaderText="Valor Nominal" DataFormatString="{0:#,##0.0000000}" />
                    <asp:BoundField DataField="Saldo Contable" HeaderText="Saldo Contable" DataFormatString="{0:0.0000000}" />
                    <asp:BoundField DataField="Saldo Disponible" HeaderText="Saldo Disponible" DataFormatString="{0:0.0000000}" />
                    <asp:BoundField Visible="False" DataField="Dato1" HeaderText="Dato1" />
                    <asp:BoundField Visible="False" DataField="Diferencia" HeaderText="Diferencia" DataFormatString="{0:0.0000000}" />
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
    
    <asp:HiddenField runat="server" ID="txtValidacion" />
    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBuscar" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>