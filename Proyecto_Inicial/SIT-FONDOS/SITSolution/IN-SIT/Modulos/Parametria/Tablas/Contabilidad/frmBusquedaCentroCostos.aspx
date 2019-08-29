<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmBusquedaCentroCostos.aspx.vb" Inherits="Modulos_Parametria_Tablas_Contabilidad_frmBusquedaCentroCostos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Centro de Costos</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="SMLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Centro de Costos</h2>
                </div>
            </div>
        </header>
        <fieldset>
            <legend>Datos&#32;Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Codig&oacute; </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbCodigo" MaxLength="9" Width="106px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div>           
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Descripci&oacute;n </label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tbDescripcion" MaxLength="40" Width="264px"/>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                </div>
            </div> 
             <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                       
                    </div>
                </div>
                 <div class="col-md-8" style="text-align: right;">
                    <asp:Button Text="Buscar" runat="server" ID="ibBuscar" />
                </div>
            </div>                     
        </fieldset>
        <br />
          
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <fieldset>
                        <legend>Resultados de la B&uacute;squeda</legend>
                        <asp:Label Text="" runat="server" ID="lbContador" />
                    </fieldset>
                    <br />
                    <div class="grilla">
                    <asp:GridView runat="server" SkinID="Grid" ID="dgLista">
                        <Columns>                            
                            <asp:BoundField DataField="CodigoCentroCosto" HeaderText="C&#243;digo" />
                            <asp:BoundField DataField="NombreCentroCosto" HeaderText="Descripci&#243;n" />
                            <asp:BoundField DataField="LimaProvincia" HeaderText="Lima Provincia" />                           
                            <asp:BoundField DataField="TipoGasto" HeaderText="TipoGasto" />                           
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />                           
                            <asp:BoundField DataField="Activo" HeaderText="Activo" />                           
                            <asp:BoundField DataField="NumeroTrabajadores" HeaderText="N&#250;mero Trabajadores" />                           
                            <asp:BoundField DataField="Direccion" HeaderText="Direcci&#243;n" />                           
                        </Columns>
                    </asp:GridView>
                    </div>
                </ContentTemplate>                               
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibBuscar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
              
     
        <br />
        <div class="row">
            <div class="col-md-6">
              
              <asp:Button Text="Consultar" runat="server" ID="ibConsultar" Height="26px" Visible="false" />
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:Button Text="Ingresar" runat="server" ID="ibIngresar" Height="26px" Visible="false"/>
                <asp:Button Text="Salir" runat="server" ID="ibSalir" CausesValidation="false"/>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

