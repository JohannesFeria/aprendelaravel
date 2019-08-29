<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmSeguimientoForwards.aspx.vb"
    Inherits="Modulos_Gestion_Reportes_frmSeguimientoForwards" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Seguimiento de Forwards</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Seguimiento de Forwards</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Portafolio</label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Moneda Origen</label>
                        <div class="col-sm-6">
                            <asp:DropDownList runat="server" ID="dllMoneda" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-6 control-label">
                            Fecha de Vencimiento</label>
                        <div class="col-sm-6">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInformacion" SkinID="Date" />
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnConsultar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label Text="" runat="server" ID="lbContador" />
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="_Edit"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Consecutivo") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoSBS" HeaderText="C&#243;digo SBS" />
                    <asp:BoundField DataField="PrecioTrans" HeaderText="Monto Forward" />
                    <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vcto." DataFormatString="{0:d}" />
                    <asp:BoundField DataField="FechaIDI" HeaderText="Fecha IDI" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="CodigoReferencia" HeaderText="Modalidad" />
                    <asp:BoundField DataField="Movimiento" HeaderText="Tipo Forward" />
                </Columns>
            </asp:GridView>
        </div>
        <header>
        </header>
        <div class="row">
            <div class="col-md-6">
            </div>
            <div class="col-md-6" style="text-align: right;">
                <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
                <asp:Button Text="Imprimir" runat="server" ID="btnImprimir" />
                <asp:Button Text="Salir" runat="server" ID="btnRetornar" CausesValidation="false" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
