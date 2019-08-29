<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLiborFecha.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmLiborFecha" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Fecha Libor </h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 control-label">Mnemónico</label>
                <div class="col-sm-8">
                    <asp:textbox id="txtCodigoNemonico" runat="server" ></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 control-label">Desde</label>
                <div class="col-sm-8">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtFechaInicio" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 control-label">Hasta</label>
                <div class="col-sm-8">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtFechaTermino" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 control-label">Fecha de Vencimiento</label>
                <div class="col-sm-8">                    
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"  CausesValidation="False" /> 
        </div>
    </div>       
    </fieldset>
    <br />
    <fieldset>
    <legend>Resultados de la Búsqueda</legend>
    <div class="row">
        <asp:label id="lbContador" runat="server"></asp:label>
    </div>
    </fieldset>
    <br />
    
    <div id="grilla">
    
        <asp:UpdatePanel ID="UP1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="dgLista" runat="server" AutoGenerateColumns="False" SkinID="Grid">
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25px">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibSeleccionarOPE" runat="server" SkinID="imgEdit" CommandName="Select" CausesValidation="false">
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodigoNemonico" HeaderText="Codigo Nem&#243;nico"></asp:BoundField>
                <asp:BoundField DataField="Secuencia" HeaderText="Secuencia"></asp:BoundField>
                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio"></asp:BoundField>
                <asp:BoundField DataField="FechaTermino" HeaderText="Fecha Termino"></asp:BoundField>
                <asp:BoundField DataField="FechaLibor" HeaderText="Fecha Libor"></asp:BoundField>
                <asp:BoundField DataField="Situacion" HeaderText="Situacion"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>        
    </div>
    <br />
        <div class="row" style="text-align: right;">
            <div class="col-md-6"></div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-5 control-label">Fecha Libor&nbsp;</label>
                    <div class="col-sm-7">
                        <div class="input-append date">
                            <asp:TextBox runat="server" ID="txtFechaLibor" SkinID="Date" />
                            <span class="add-on"><i class="awe-calendar"></i></span>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Fecha Libor"
                            ControlToValidate="txtFechaLibor">(*)</asp:RequiredFieldValidator>
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                        <asp:Button ID="btnSalir" runat="server" Text="Salir" CausesValidation="False" />
                    </div>
                </div>
            </div>
        </div>
    <br />
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false" HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>