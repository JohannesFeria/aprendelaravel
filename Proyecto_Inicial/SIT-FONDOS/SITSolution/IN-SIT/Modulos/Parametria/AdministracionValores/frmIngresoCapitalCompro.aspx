<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIngresoCapitalCompro.aspx.vb"
    Inherits="Modulos_Parametria_AdministracionValores_frmIngresoCapitalCompro" %>

<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Ingreso de Capital Comprometido</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>
                        Ingreso de Capital Comprometido</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            C&oacute;digo Isin</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="lCodigoIsin" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Código Mnem&oacute;nico</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="lCodigoNemo" Width="150px" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <fieldset>
            <legend>Detalle</legend>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label runat="server" id="lblPortafolio" class="col-sm-4 control-label">
                            Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">
                            Situaci&oacute;n
                        </label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlSituacion" Width="150px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label runat="server" id="lblCapitalCompro" class="col-sm-4 control-label">
                            Capital Comprometido</label>
                        <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="tbCapitalCompro" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-6" style="text-align: right;">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Identificador") %>'>
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoPortafolioSBS" HeaderText="C&#243;digo Portafolio" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Portafolio SBS" />
                    <asp:BoundField DataField="CapitalCompro" HeaderText="Capital Comprometido" />
                    <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n" />
                    <asp:BoundField Visible="False" DataField="Identificador" HeaderText="Identificador" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <header>
        </header>
        <div class="row">
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button Text="Eliminar" runat="server" ID="btnEliminar" Visible="False" />
                <asp:Button Text="Aceptar" runat="server" ID="btnAceptar" />
                <asp:Button Text="Retornar" runat="server" ID="btnSalir" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
