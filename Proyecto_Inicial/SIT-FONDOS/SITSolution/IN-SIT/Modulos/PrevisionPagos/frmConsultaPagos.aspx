<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaPagos.aspx.vb"
    Inherits="Modulos_PrevisionPagos_frmConsultaPagos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Consulta Pagos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Consulta Pagos</h2>
                </div>
            </div>
        </header>

        
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Fecha Pago</label>
                        <div class="col-sm-7">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="txtFechaPago" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Fecha Pago"
                                ControlToValidate="txtFechaPago" runat="server" Text="(*)" CssClass="validator"
                                ValidationGroup="rvValidar" />
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
                            Tipo Operaci&oacute;n</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlTipoOperacion" runat="server" Width="200px" CssClass="stlCajaTexto">
                            </asp:DropDownList>
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
                            Estado</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlEstado" runat="server" Width="150px" CssClass="stlCajaTexto">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" ValidationGroup="rvValidar" />
                </div>
            </div>

        </fieldset>
        

        <br />
        
        
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        
     
        <br />
        <div class="HeaderGVCab" style="left: 0px; top: 0px; overflow: auto; width: 100%;
            height: 14px;">
            <asp:GridView ID="gvPagosCab" runat="server" skin="gvPagos">
                <Columns>
                    <asp:BoundField HeaderText="" DataField="bancoT">
                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                        <HeaderStyle HorizontalAlign="Center" Width="200px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Fondo 1">
                        <ItemTemplate>
                            <asp:TextBox ID="tbingresoF1" runat="server" Width="158px" Text='<%# Bind("ingresoF1") %>'
                                Style="text-align: right" ReadOnly="True"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fondo 2">
                        <ItemTemplate>
                            <asp:TextBox ID="tbingresoF2" runat="server" Width="158px" Text='<%# Bind("ingresoF2") %>'
                                Style="text-align: right" ReadOnly="True"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fondo 3">
                        <ItemTemplate>
                            <asp:TextBox ID="tbingresoF3" runat="server" Width="158px" Text='<%# Bind("ingresoF3") %>'
                                Style="text-align: right" ReadOnly="True"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fondo ADM">
                        <ItemTemplate>
                            <asp:TextBox ID="tbingresoadm" runat="server" Width="158px" Text='<%# Bind("ingresoadm") %>'
                                Style="text-align: right" ReadOnly="True"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label ID="lbTotal" runat="server" Text='<%# Bind("total") %>' Font-Bold="true"
                                Font-Size="11px" Style="text-align: right;" Width="93px"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="95px" />
                        <ItemStyle Width="95px" BackColor="LemonChiffon" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

            <asp:GridView runat="server" SkinID="Grid" ID="gvPagos">
            <Columns>
                <asp:BoundField HeaderText="Banco" DataField="Banco">
                    <ItemStyle HorizontalAlign="Left" Width="350px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="350px" />
                </asp:BoundField>
                <asp:BoundField DataField="Ingreso0" HeaderText="Ingreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Egreso0" HeaderText="Egreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Ingreso1" HeaderText="Ingreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Egreso1" HeaderText="Egreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Ingreso2" HeaderText="Ingreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Egreso2" HeaderText="Egreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Ingreso3" HeaderText="Ingreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Egreso3" HeaderText="Egreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="IngresoADM" HeaderText="Ingreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="EgresoADM" HeaderText="Egreso">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Total" HeaderText="Total">
                    <ItemStyle HorizontalAlign="Right" Width="150px" CssClass="stlPaginaTexto2" />
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    </ContentTemplate>                               
     <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>

                 </asp:UpdatePanel>
                 </div>
         <asp:GridView runat="server" SkinID="Grid" ID="gvDetallePagoToExport">
        <Columns>
            <asp:BoundField DataField="Banco" HeaderText="Banco" />
            <asp:BoundField DataField="Fondo0" HeaderText="Fondo 0" />
            <asp:BoundField DataField="Fondo1" HeaderText="Fondo 1" />
            <asp:BoundField DataField="Fondo2" HeaderText="Fondo 2" />
            <asp:BoundField DataField="Fondo3" HeaderText="Fondo 3" />
            <asp:BoundField DataField="FondoAdm" HeaderText="Fondo Adm" />
            <asp:BoundField DataField="Total" HeaderText="Total" />
        </Columns>
    </asp:GridView>
   
    <br />
    <div class="row">
        <div class="col-md-6">
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button Text="Exportar" runat="server" ID="btnExportar" Height="26px" />
            <asp:Button Text="Salir" runat="server" ID="btnSalir" CausesValidation="false" />
        </div>
    </div>
    
    </form>
</body>
</html>
