<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCierreHora.aspx.vb" Inherits="Modulos_PrevisionPagos_frmCierreHora" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Feriados</title>
</head>
<body>
    <form class="form-horizontal" id="form2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManagerLocal" />
    <div class="container-fluid">
        <header>
            <div class="row">
                <div class="col-md-6">
                    <h2>Parametría Hora Cierre</h2>
                </div>
            </div>
        </header>

        <fieldset>
            <legend>Datos&#32;Generales</legend>

            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Hora Cierre Registro</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtHoraCierreReg" Width="60px" 
                                style="text-align: right" />
                            &nbsp;<span style="color: Silver;">(0-23 hh:mm)</span>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">
                            Hora Cierre Aprobación</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="txtHoraCierreApro" Width="60px" 
                                style="text-align: right" />
                       &nbsp;<span style="color: Silver;">(0-23 hh:mm)</span>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">                        
                        <div class="col-sm-7">                        
                        </div>
                    </div>
                </div>
            </div>

        </fieldset>
        
        <br />
        <br />

        <%--<div class="grilla">    
        </div>--%>
        <br />
        <div class="row">
            <div class="col-md-6">
              
            </div>
            <div class="col-md-6" style="text-align: right;">                
                <asp:UpdatePanel runat="server" ID="updExtraer">
                    <ContentTemplate>
                        <asp:Button ID="btnAceptar" runat="server" Width="72px" Text="Aceptar"
                            CausesValidation="False" CssClass="button"></asp:Button>&nbsp;
                        <asp:Button ID="bSalir" runat="server" Width="72px" Text="Salir" 
                            CausesValidation="False" CssClass="button" Visible="false">
                        </asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
