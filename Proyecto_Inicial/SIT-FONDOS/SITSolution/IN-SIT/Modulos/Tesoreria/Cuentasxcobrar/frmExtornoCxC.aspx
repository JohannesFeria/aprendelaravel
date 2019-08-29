<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoCxC.aspx.vb" Inherits="Modulos_Tesoreria_Cuentasxcobrar_frmExtornoCxC" %>
<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Extorno de CXC</title>
    <script type="text/javascript">
        var color;
        function SelectedRow(chk) {
            if (chk.checked) {
                color = chk.parentElement.parentElement.style.backgroundColor;
                chk.parentElement.parentElement.style.backgroundColor = "LemonChiffon";
            } else {
                chk.parentElement.parentElement.style.backgroundColor = color;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="container-fluid">
            <header><h2>Anulación Cuentas por Cobrar</h2></header>
            <br />
            <fieldset>
                <legend>Resultados de la Búsqueda</legend>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Mercado</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlMercado" runat="server" Width="200px" AutoPostBack="True" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Portafolio</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlPortafolio" runat="server" Width="200px" AutoPostBack="True" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Intermediario</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlIntermediario" runat="server" Width="250px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Moneda</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlMoneda" runat="server" Width="250px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Operación</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlOperacion" runat="server" Width="250px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Fecha Liquidación</label>
                            <div class="col-sm-7">
                                <div class="input-append">
                                    <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" ReadOnly ="true"  />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset>
                <legend>Resultados de la Búsqueda</legend>
                <asp:Label runat="server" ID="lbContador" />
            </fieldset>
            <br />
            <div class="grilla" style="height: 260px;">
                <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                    <Columns>
                        <asp:TemplateField><ItemTemplate><asp:CheckBox ID="chbConfirmar" runat="server"></asp:CheckBox></ItemTemplate></asp:TemplateField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="NroOperacion"></asp:BoundField>
                        <asp:BoundField DataField="FechaNegociacion" HeaderText="Fec. Negoc."></asp:BoundField>
                        <asp:BoundField DataField="FechaVencimiento" HeaderText="Fec. Vcto"></asp:BoundField>
                        <asp:BoundField DataField="NroOperacion" HeaderText="Nro. Operaci&#243;n"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="Referencia" HeaderText="Descripci&#243;n"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:#,##0.00}"><ItemStyle HorizontalAlign="Right"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="DescripcionMercado" HeaderText="Mercado"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="DescripcionPortafolio" HeaderText="Portafolio"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="DescripcionMoneda" HeaderText="Moneda"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="DescripcionIntermediario" HeaderText="Intermediario"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="DescripcionOperacion" HeaderText="Operaci&#243;n"><ItemStyle HorizontalAlign="Left"></ItemStyle></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoPortafolioSBS"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Estado"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoRenta"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <header></header>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnExtornar" runat="server" Text="Anular" />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" />
            </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>