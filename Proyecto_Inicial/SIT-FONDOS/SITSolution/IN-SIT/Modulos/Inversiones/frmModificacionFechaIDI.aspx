<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmModificacionFechaIDI.aspx.vb" Inherits="Modulos_Inversiones_frmModificacionFechaIDI" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Modificación de Fecha IDI</title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Modificación de Fecha IDI</h2></header>
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" ></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="tbFecha" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Mnemónico</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtMnemonico" runat="server" MaxLength="12"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" EnableViewState="False" 
                CausesValidation="False" />
        </div>
    </div>
    </fieldset>
    <br />
    <div class="grilla">
        <asp:GridView ID="dgLista" runat="server" SkinID="Grid">            
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibSeleccionar" runat="server" SkinID="imgEdit"
                            CommandName="Select"></asp:ImageButton>                            
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operaci&#243;n"></asp:BoundField>
                <asp:BoundField DataField="FechaIDI" HeaderText="Fecha IDI"></asp:BoundField>
                <asp:BoundField DataField="codigoMnemonico" HeaderText="Mnem&#243;nico"></asp:BoundField>
                <asp:BoundField DataField="CodigoOrden" HeaderText="C&#243;digo Orden"></asp:BoundField>
                <asp:BoundField DataField="MontoNetoOperacion" HeaderText="Monto" DataFormatString="{0:#,##0.0000}"></asp:BoundField>
                <asp:BoundField Visible="False" DataField="CodigoPortafolioSBS" HeaderText="CodigoPortafolioSBS"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Orden</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtCodigoOrdenAux" runat="server" MaxLength="12" ReadOnly="True"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Mnemónico</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtMnemonicoAux" runat="server" MaxLength="12" ReadOnly="True"></asp:textbox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha de Operación</label>
                <div class="col-sm-9">
                    <asp:textbox id="txtFechaOperacionAux" runat="server" MaxLength="12" ReadOnly="True"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Código Mnemónico</label>
                <div class="col-sm-9">
                    <div class="input-append date">
                        <asp:TextBox runat="server" ID="txtFechaIDIAux" SkinID="Date" />
                        <span class="add-on"><i class="awe-calendar"></i></span>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Código Mnemónico" ControlToValidate="txtFechaIDIAux" 
                        ValidationGroup="Modificar">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-9">
                    
                </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" 
                ValidationGroup="Modificar" />
        </div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" 
            CausesValidation="False" />
    </div>
    <asp:HiddenField ID="hdCodigoPortafolioSBS" runat="server" />
    <br />
    </div>
    </form>
</body>
</html>
