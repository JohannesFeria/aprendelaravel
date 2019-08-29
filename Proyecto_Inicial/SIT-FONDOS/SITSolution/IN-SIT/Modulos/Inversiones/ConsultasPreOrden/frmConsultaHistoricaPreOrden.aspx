﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaHistoricaPreOrden.aspx.vb" Inherits="Modulos_Inversiones_ConsultasPreOrden_frmConsultaHistoricaPreOrden" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>ConsultasPreOrden</title>
    <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <script type="text/javascript" language="javascript">
        function CloseWindow() { window.close();}
    </script>
</head>
<body>
    <form id="form1" runat="server"  class="form-horizontal">
        <asp:ScriptManager runat="server" ID="SMLocal" />
        <asp:UpdatePanel ID="upfiltro" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid">
            <header>
            <div class="row">
                <div class="col-md-6">
                    <h2><asp:Label ID="lblTitulo" runat="server">Consultas PreOrden</asp:Label></h2>
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <h3><asp:Label ID="lblAccion" runat="server" /></h3>
                </div>
            </div>
            </header>

        <div class="row">
            <div class="col-md-7">
                <div class="form-group">
                    <label class="col-sm-4 control-label"><asp:label id="lblFondo" runat="server">Portafolio</asp:label></label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlPortafolio" runat="server" OnChange="javascript:cambiaTitulo();"
                            AutoPostBack="True" Width="120px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">Tipo de Operación</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlTipoOperacion" runat="server"  Width="120px">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-7">
                <div class="form-group">
                    <label class="col-sm-4 control-label">Mnemónico o Ticket</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtMnemonico" runat="server"  Width="120px" MaxLength="15" />
                        <asp:LinkButton runat="server" ID="btnBuscar" ><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="col-sm-4 control-label">Código ISIN</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtISIN" runat="server" Width="120px" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-7">
                <div class="form-group">
                    <label class="col-sm-4 control-label">Fecha Asignación</label>
                    <div class="col-sm-8">
                        <div class="input-append">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" Width="120px" />
                                <span id="img1" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Codigo SBS</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtsbs" runat="server"  />
                        </div>
                    </div>
                </div>
        </div>
            <div class="row">
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"></label>
                        <div class="col-sm-8">
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Estado</label>
                            <div class="col-sm-8">
                                <asp:dropdownlist id="ddlEstado" runat="server"  />
                            </div>
                        </div>
                    </div>
            </div>
            <div class="row">
                <div class="col-md-11">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">
                        </label>
                        <div class="col-sm-10" style="text-align: right;">
                        <div style="display: none" ><asp:Button  runat="server"  ID="btnpopup"  /></div>
                        
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-11" align="rigth">
                    <div class="form-group">
                        <div class="col-sm-12" style="text-align: right;">
                            <asp:Button runat="server" ID="btnConsulta" Text="Buscar" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
            <fieldset>
            <legend>Resultados de la Búsqueda</legend>
            <div class="row">
                <div class="col-md-7"></div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-sm-12" style="text-align: right;">
                                <asp:label id="lbContador" runat="server" />
                            </div>
                        </div>
                    </div>
            </div>
            <asp:GridView ID="dgordenpreorden" runat="server" SkinID="Grid">
                <Columns>
							<asp:BoundField DataField="FechaOperacion" HeaderText="Fecha">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="HoraOperacion" HeaderText="Hora">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="codigoPortafolioSBS" HeaderText="Portafolio">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Moneda" HeaderText="Moneda">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="codigoMnemonico" HeaderText="Mnemonico">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="tipoInstrumento" HeaderText="Tipo Instrumento">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Tasa" HeaderText="Tasa" DataFormatString="{0:0,0.0000000}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="InteresCorridoNegociacion" HeaderText="InteresCorrido" DataFormatString="{0:0,0.0000000}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:0,0.0000000}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="MontoNetoOperacion" HeaderText="Monto" DataFormatString="{0:0,0.0000000}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Intermediario" HeaderText="Intermediario">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="Estado" HeaderText="Estado">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundField>
						</Columns>
            </asp:GridView>
        </fieldset>
            <br />
            <div class="row">
                <div class="col-md-11" align="rigth">
                    <div class="form-group">
                        <div class="col-sm-12" style="text-align: right;">
                            <asp:Button runat="server" ID="btnexportar" Text="Imprimir" />
                            <asp:Button runat="server" ID="ibSalir" Text="Salir" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
