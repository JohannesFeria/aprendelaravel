<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmConsultaEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Consulta de Encaje</title>
    <script type="text/javascript">
        function ShowModal() {        
            var vPorta = $('#ddlPortafolio').val();
            var vFecha = $('#tbFechaEncaje').val();
            if (vFecha != '') {
                //alert(vPorta);
                //alert(vFecha);
                return showModalDialog('frmVisorEncaje.aspx?pReporte=CE&pportafolio=' + vPorta + '&pFechaIni=' + vFecha, '800', '600', '');                   
            }            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Consulta de Encaje</h2></header>
        <br />
        <fieldset>
        <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:dropdownlist id="ddlPortafolio" runat="server" Width="120px" AutoPostBack="True"></asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Proceso</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaEncaje" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Fecha de Proceso" ControlToValidate="tbFechaEncaje">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
        <legend>Resultados de la Búsqueda</legend>
        <div class="row">
            <asp:label id="lbContador" runat="server"></asp:label>
        </div>
        </fieldset>
        <br />        
        <div class="grilla">
            <asp:UpdatePanel ID="UP1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                        <Columns>
                            <asp:BoundField DataField="FechaEncaje" HeaderText="Fecha Encaje"></asp:BoundField>
                            <asp:BoundField DataField="ValorMantenido" HeaderText="Valor Mantenido" DataFormatString="{0:#,##0.0000000}">
                            </asp:BoundField>
                            <asp:BoundField DataField="ValorRequerido" HeaderText="Valor Requerido" DataFormatString="{0:#,##0.0000000}">
                            </asp:BoundField>
                            <asp:BoundField DataField="DiferenciaEncaje" HeaderText="Dif. Encaje" DataFormatString="{0:#,##0.0000000}">
                            </asp:BoundField>
                            <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                            <asp:BoundField DataField="ValorRentabilidad" HeaderText="Rentabilidad" DataFormatString="{0:#,##0.0000000}">
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" OnClientClick="return ShowModal();"/>
            <asp:Button ID="btnSalir" runat="server" Text="Salir" 
                CausesValidation="False" />
        </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>