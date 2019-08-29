<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmAmortizacionesVencimiento.aspx.vb" Inherits="Modulos_Inversiones_frmAmortizacionesVencimiento" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function Confirmar() {

            if (confirm("¿Desea Confirmar el Cupón vencido.")) {
                return true;
            }
            else {
                return false;
            }
        }
        function Cerrar() {
            window.close();
        }

        $(document).ready(function () {
            $('#btnRetornar').click(function () {
                Cerrar();
            });
        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Confirmar Cuponeras Vencidas</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-5 control-label">Cupón con fecha de vencimiento</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="lFechaVencimiento" runat="server" Text="" ReadOnly="true"/>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-5 control-label">Código ISIN</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="lCodigoISIN" runat="server" Text="" ReadOnly="true"/>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-5 control-label">Importe Nominal Local</label>
                <div class="col-sm-5">
                      <asp:TextBox ID="lMontoNominalLocal" runat="server" Text="" ReadOnly="true" CssClass ="Numbox-7"/>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-5 control-label">Código SBS</label>
                <div class="col-sm-5">
                     <asp:TextBox ID="lCodigoSBS" runat="server" Text="" ReadOnly="true"/>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9"></div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-5 control-label">Unidades</label>
                <div class="col-sm-5">
                    <asp:TextBox id="tbUnidades" runat="server" Text=""></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:GridView ID="dgLista" runat="server" Width="100%" AutoGenerateColumns="False" SkinID="GridFooter">
            <Columns>
                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha inicio" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField DataField="FechaTermino" HeaderText="Fecha Fin" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:TemplateField HeaderText="Valor Nominal Local">
                    <ItemTemplate>
                        <asp:TextBox ID="tbValorNominalLocal" runat="server" Width="120px" CssClass="Numbox-7" ></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="Amortizacion" HeaderText="Amortizaci&#243;n (%)"  DataFormatString="{0:#,##0.0000000}" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="FechaPago" HeaderText="FechaPago"></asp:BoundField>
                <asp:BoundField FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoNemonico" HeaderText="CodigoNemonico"></asp:BoundField>
                <asp:BoundField FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="Secuencial" HeaderText="CodigoNemonico"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
            <label class="col-sm-3 control-label">Fecha Operación</label>
            <div class="col-sm-9">
                <div class="input-append date">
                    <asp:TextBox runat="server" ID="txtFechaIDI" SkinID="Date" />
                    <span class="add-on"><i class="awe-calendar"></i></span>
                </div>
            </div>
        </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
            <label class="col-sm-3 control-label">Fecha Liquidación</label>
            <div class="col-sm-9">
                <div class="input-append date">
                    <asp:TextBox runat="server" ID="txtFechaPago" SkinID="Date" />
                    <span class="add-on"><i class="awe-calendar"></i></span>
                </div>
            </div>
        </div>
        </div>
    </div>
    <div ID="tblDestino" runat="server" style="display: none;VISIBILITY: hidden;">
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-3 control-label">Monto&nbsp;Operación&nbsp;<asp:Label id="lblMDest2" runat="server"></asp:Label>:</label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtMontoOperacionDestino" runat="server" Width="200px" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                
            </div>
        </div>
    </div>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" />
        </div>
    </div>
    </form>
</body>
</html>
