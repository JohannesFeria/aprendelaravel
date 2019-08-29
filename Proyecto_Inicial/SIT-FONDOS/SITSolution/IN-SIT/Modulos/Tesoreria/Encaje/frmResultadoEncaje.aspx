<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmResultadoEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmResultadoEncaje" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Resultados de Encaje</title>
    <script type="text/javascript">
        function ShowModal(fecha) {
            var vporta = $('#ddlPortafolio').val();
            var vFecha = $('#txtFechaProceso').val();
            //alert(fecha);
            //alert(vFecha);
            return showModalDialog('frmVisorEncaje.aspx?pReporte=RE&pportafolio=' + vporta + '&pFechaIni=' + vFecha, '800', '600', '');             
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Resultados de Encaje</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-4 control-label">Portafolio</label>
                <div class="col-sm-8">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="136px" AutoPostBack="True"></asp:dropdownlist>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Portafolio " ControlToValidate="ddlPortafolio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-4 control-label">Fecha de Proceso</label>
                <div class="col-sm-8">                    
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtFechaProceso" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Fecha de Proceso" ControlToValidate="txtFechaProceso">(*)</asp:RequiredFieldValidator>
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
    <asp:label id="lbContador" runat="server"></asp:label>
    </fieldset>
    <br />        
    <div class="grilla">
        <asp:UpdatePanel ID="UP1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
                    <Columns>
                        <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS"></asp:BoundField>
                        <asp:BoundField DataField="TipoInstrumento" HeaderText="Tipo Instrumento"></asp:BoundField>
                        <asp:BoundField DataField="Emisor" HeaderText="Emisor"></asp:BoundField>
                        <asp:BoundField DataField="ValorNominal" HeaderText="Nominal total" DataFormatString="{0:#,##0.0000000}">
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vcto"></asp:BoundField>
                        <asp:BoundField DataField="NumeroDias" HeaderText="Nro. D&#237;as"></asp:BoundField>
                        <asp:BoundField DataField="SumaValorizados" HeaderText="Sum. Valorizados" DataFormatString="{0:#,##0.0000000}">
                        </asp:BoundField>
                        <asp:BoundField DataField="PromedioValoracion" HeaderText="Valor Promedio" DataFormatString="{0:#,##0.0000000}">
                        </asp:BoundField>
                        <asp:BoundField DataField="ValorTasa" HeaderText="Tasa" DataFormatString="{0:#,##0.0000000}">
                        </asp:BoundField>
                        <asp:BoundField DataField="Categoria" HeaderText="Categoria"></asp:BoundField>
                        <asp:BoundField DataField="ValorEncaje" HeaderText="Valor Encaje" DataFormatString="{0:#,##0.0000000}">
                        </asp:BoundField>
                        <asp:BoundField Visible="False" DataField="Descripcion" HeaderText="Instrumento">
                        </asp:BoundField>
                        <asp:BoundField Visible="False" DataField="Emision" HeaderText="Emisi&#243;n"></asp:BoundField>
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
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
