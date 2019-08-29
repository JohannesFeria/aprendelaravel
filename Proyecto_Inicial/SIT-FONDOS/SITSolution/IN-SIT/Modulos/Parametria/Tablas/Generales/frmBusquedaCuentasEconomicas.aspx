<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaCuentasEconomicas.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmBusquedaCuentasEconomicas" %>
<!DOCTYPE html>
<html>
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Cuentas Econ&oacute;micas</title>
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6"><h2>Cuentas Econ&oacute;micas</h2></div>
            </div>
        </header>
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlPortafolio" Width="150px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Moneda</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="100px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Banco</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlBanco" Width="220px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Clase de Cuenta</label>
                        <div class="col-sm-8">
                            <asp:DropDownList runat="server" ID="ddlClaseCuenta" Width="140px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="btnBuscar" />
                </div>
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>Resultados de la B&uacute;squeda</legend>
            <asp:Label ID="lbContador" runat="server"></asp:Label>
        </fieldset>
        <br />
        <div class="grilla">
            <asp:UpdatePanel ID="updGrilla" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPortafolioSBS") & "," & DataBinder.Eval(Container.DataItem, "CuentaContable") & "," & DataBinder.Eval(Container.DataItem, "NumeroCuenta") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPortafolioSBS") & "," & DataBinder.Eval(Container.DataItem, "CuentaContable") & "," & DataBinder.Eval(Container.DataItem, "NumeroCuenta") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--OT10795 - Agregar ícono de cambio de cuenta económica--%>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibCambiarCuentaEconomica" runat="server" SkinID="imgMenu" OnCommand="CambiarCuentaEconomica"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoPortafolioSBS") & "," & DataBinder.Eval(Container.DataItem, "CuentaContable") & "," & DataBinder.Eval(Container.DataItem, "NumeroCuenta") & "," & DataBinder.Eval(Container.DataItem, "NumeroCuenta") & "," & DataBinder.Eval(Container.DataItem, "NumeroCuenta") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--OT10795 - Fin--%>
                            <asp:BoundField DataField="NumeroCuenta" HeaderText="Nro. Cuenta" />
                            <asp:BoundField DataField="NombreMercado" HeaderText="Mercado" />
                            <asp:BoundField DataField="NombreEntidadFinanciera" HeaderText="Banco" />
                            <asp:BoundField DataField="NombrePortafolio" HeaderText="Portafolio" />
                            <asp:BoundField DataField="NombreClaseCuenta" HeaderText="Clase Cuenta" />
                            <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda" />
                            <asp:BoundField DataField="NombreSituacion" HeaderText="Situaci&#243;n" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <header>
        </header>
        <div class="row" style="text-align: right;">
            <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" />
            <asp:Button Text="Salir" runat="server" ID="btnCancelar" CausesValidation="false" />
        </div>
    </div>
    </form>
</body>
</html>
