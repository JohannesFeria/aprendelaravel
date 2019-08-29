<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmRentabilidadFondoEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmRentabilidadFondoEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Reporte de Rentabilidad</title>
    <script type="text/javascript">
        function ShowReport(pFondo, pFInicio, pFFin) {
            return showModalDialog('frmVisorEncaje.aspx?pReporte=REN&pportafolio=' + pFondo + '&pFechaIni=' + pFInicio + '&pFechaFin=' + pFFin, '800', '600', '');             
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Reporte de Rentabilidad</h2></header>
    <br />
    <fieldset>
    <legend></legend>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Portafolio</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlFondo" TabIndex="4" runat="server" AutoPostBack="True" Width="183px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
            </div>
            <div class="col-md-4">
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Inicio</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" />
                            <span class="add-on"><i class="awe-calendar"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Fecha Inicio"
                            ControlToValidate="tbFechaInicio">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">
                        Fecha Fin</label>
                    <div class="col-sm-8">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" />
                            <span class="add-on"><i class="awe-calendar"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Fecha Fin"
                            ControlToValidate="tbFechaFin">(*)</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
            </div>
        </div>

        <br />

        <div class="row">
            <div class="grilla-small">
                <asp:GridView ID="dgPortafolio" runat="server" SkinID="GridSmall"
                    PageSize="5" AutoGenerateColumns="False" DataKeyNames="CodigoPortafolio">
                    <Columns>
                        <asp:BoundField DataField="CodigoPortafolio" HeaderText="CodigoPortafolio" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                        <asp:TemplateField HeaderText="Provisión">
                            <ItemTemplate>
                                <%--<asp:TextBox onkeypress="Javascript:Numero();" ID="txtProvDer" runat="server"
                                    Width="60px" MaxLength="5" Text='<%#DataBinder.Eval(Container.DataItem, "value")%>'>
                                </asp:TextBox>--%>
                                <asp:TextBox ID="txtProvDer" runat="server"
                                    Width="150px" MaxLength="30">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        

   <%-- <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-4 control-label">Provisión Derivados</label>
                <div class="col-sm-8">
                    <asp:textbox style="Z-INDEX: 0" id="tbProvDerF1" onkeypress="Javascript:Numero();" onblur="javascript: FormateaDecimal(this,7);" runat="server" Width="166px" MaxLength="19"></asp:textbox>
                </div>
            </div>            
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-4 control-label">Fondo 2</label>
                <div class="col-sm-8">
                    <asp:textbox onblur="javascript: FormateaDecimal(this,7);" style="Z-INDEX: 0" id="tbProvDerF2" onkeypress="Javascript:Numero();" runat="server" Width="166px" MaxLength="19"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="col-sm-4 control-label">Fondo 3</label>
                <div class="col-sm-8">
                    <asp:textbox onblur="javascript: FormateaDecimal(this,7);" style="Z-INDEX: 0" id="tbProvDerF3" onkeypress="Javascript:Numero();" runat="server" Width="166px" MaxLength="19"></asp:textbox>
                </div>
            </div>
        </div>
    </div--%>
    </fieldset>

    <br />
    <header></header>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnResumenPorTipoRenta" runat="server" Text="Resumen por tipo Renta" />
        <asp:Button ID="btnRentabilidadTotalInstrumentos" runat="server" Text="Retabilidad Total Instrumentos" />
        <asp:Button ID="btnProvision" runat="server" Text="Provisión Contable Impuesto" />        
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" 
            CausesValidation="False" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
