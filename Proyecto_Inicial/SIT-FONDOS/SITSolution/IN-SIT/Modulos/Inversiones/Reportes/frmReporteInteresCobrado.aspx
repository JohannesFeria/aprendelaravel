<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteInteresCobrado.aspx.vb" Inherits="Modulos_Inversiones_Reportes_frmReporteInteresCobrado" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte de Interes Cobrado</title>
</head>
<body>
<form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header><h2>Reporte de Interes Cobrado</h2></header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" AutoPostBack="True" Width="220px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha de Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" />
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Fin</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaFin" SkinID="Date" />
                                <span class="add-on" id="Span1"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div style="text-align: right;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    </div>
                </div>
            </div>
            </ContentTemplate> 
            </asp:UpdatePanel> 
        </fieldset>
        <br />
        <div class="row">
            <div class="col-md-12">
                <div class="Grilla">
                    <asp:UpdatePanel ID="udpReporte" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="dgReporte" AutoGenerateColumns="false" SkinID="Grid">
                            <Columns>
                                <asp:BoundField DataField="Fondo" HeaderText="Portafolio"/>
                                <asp:BoundField DataField="Nemonico" HeaderText="Nemonico"/>
                                <asp:BoundField DataField="Categoria" HeaderText="Categoria"/>
                                <asp:BoundField DataField="Tercero" HeaderText="Emisor"/>
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda"/>
                                <asp:BoundField DataField="Dias" HeaderText="Dias"/>
                                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha de Inicio" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Fecha de Termino" />
                                <asp:BoundField DataField="Nominal" HeaderText="Nominal" DataFormatString="{0:#,##0.0000000}" />
                                <asp:BoundField DataField="MontoCobrado" HeaderText="Monto Cobrado" DataFormatString="{0:#,##0.0000000}" /> 
                            </Columns>
                        </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID ="btnBuscar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <br /><hr />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte"  Height="26px" />
            <asp:Button ID="btnCancelar" runat="server" Text="Salir" />
        </div>
    </div>
    </form>
</body>
</html>