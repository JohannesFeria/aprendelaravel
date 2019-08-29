<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReportes.aspx.vb" Inherits="Modulos_PrevisionPagos_frmReportes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Registro Pagos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Reportes</h2>
                </div>
            </div>
        </header>
                
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Pago Inicio</label><div class="col-sm-7">
                            <div class="input-append date">
                                    <asp:TextBox ID="txtFechaInicio" runat="server" MaxLength="10" SkinID="Date"></asp:TextBox>
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Pago Fin</label><div class="col-sm-7">
                            <div class="input-append date">
                                    <asp:TextBox ID="txtFechaFin" runat="server" MaxLength="10" SkinID="Date"></asp:TextBox>
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>    
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

             <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                             Tipo Reporte</label>
                        <div class="col-sm-7">
                             <asp:DropDownList ID="ddlTipoOperacion" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                       
                    </div>
                </div>
                <div class="col-md-7" style="text-align: right;">
                    <%--<asp:Button Text="Buscar" runat="server" ID="btnBuscar" />--%>
                </div>
            </div>
        </fieldset>
        
        <br />
        <%--<fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>--%>

        <div class="grilla">

                    <asp:GridView ID="gvDetallado" runat="server" AutoGenerateColumns="False" 
                        EnableModelValidation="True">
                        <Columns>
                            <asp:BoundField DataField="CodigoPago" HeaderText="Codigo"/>
                            <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operacion" />
                            <asp:BoundField DataField="Banco" HeaderText="Banco" />
                            <asp:BoundField DataField="IdTipoMoneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="Ingreso0" HeaderText="Ingreso" />
                            <asp:BoundField DataField="Egreso0" HeaderText="Egreso" />
                            <asp:BoundField DataField="Ingreso1" HeaderText="Ingreso" />
                            <asp:BoundField DataField="Egreso1" HeaderText="Egreso" />
                            <asp:BoundField DataField="Ingreso2" HeaderText="Ingreso" />
                            <asp:BoundField DataField="Egreso2" HeaderText="Egreso" />
                            <asp:BoundField DataField="Ingreso3" HeaderText="Ingreso" />
                            <asp:BoundField DataField="Egreso3" HeaderText="Egreso" />
                            <asp:BoundField DataField="IngresoADM" HeaderText="Ingreso" />
                            <asp:BoundField DataField="EgresoADM" HeaderText="Egreso" />
                            <asp:BoundField DataField="Total" HeaderText="Totales" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        </Columns>
                    </asp:GridView>

        </div>

        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:UpdatePanel runat="server" ID="updExtraer">
                    <ContentTemplate>
                        &nbsp;
                        <asp:Button ID="btnGenerar" runat="server" Width="72px" Text="Generar"
                            CausesValidation="False" CssClass="button"></asp:Button>&nbsp;
                        <asp:Button ID="btnRetornar" runat="server" Width="72px" Text="Retornar" 
                            CausesValidation="False" CssClass="button" Visible="False">
                        </asp:Button>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnGenerar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>