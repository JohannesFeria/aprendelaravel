<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaSaldoNoAdministrado.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmBusquedaSaldoNoAdministrado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Saldos no Administrados</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <header><h2>Saldos no administrados</h2></header>
            <br />

            <fieldset>
                <legend>Búsqueda</legend>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Mandato</label>
                            <div class="col-sm-9">
                                <asp:DropDownList runat="server" Width="290px" ID="ddlMandato" />
                            </div>
                        </div>
                    </div>  
                    <div class="col-md-5">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Fecha</label>
                            <div class="col-sm-9">
                                <div class="input-append date">
                                    <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                    <span class="add-on"><i class="awe-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2" style="text-align:right;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    </div>
                </div>         
            </fieldset>
            <br />
            <fieldset>
                <legend>Resultados de la Búsqueda</legend>
                <asp:label id="lbContador" runat="server"></asp:label>
            </fieldset>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="grilla">
                        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" 
                            SkinID="Grid" GridLines="None" Width="1427px">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="25">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" OnCommand="Modificar"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoSaldoNoAdmnistrado") %>'>                            
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="25">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibEliminar" runat="server" SkinID="imgDelete" OnCommand="Eliminar"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoSaldoNoAdmnistrado") %>'>
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DesMandato" HeaderText="Mandato"></asp:BoundField>
                                <asp:BoundField DataField="FechaFormat" HeaderText="Fecha"></asp:BoundField>
                                <asp:BoundField DataField="DesBanco" HeaderText="Entidad Financiera"></asp:BoundField>
                                <asp:BoundField DataField="DesTipoCuenta" HeaderText="Tipo Cuenta"></asp:BoundField>
                                <asp:BoundField DataField="CodigoMoneda" HeaderText="Moneda"></asp:BoundField>
                                <asp:BoundField DataField="Saldo" HeaderText="Saldo"></asp:BoundField>                              
                                <asp:BoundField DataField="DesSituacion" HeaderText="Estado"></asp:BoundField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <br />

            <header></header>
            <div class="row" style="text-align: right;">
                <asp:Button ID="btnImportar" runat="server" Text="Importar" />
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" />
            </div>
            <br />
        </div>
    </form>
</body>
</html>
