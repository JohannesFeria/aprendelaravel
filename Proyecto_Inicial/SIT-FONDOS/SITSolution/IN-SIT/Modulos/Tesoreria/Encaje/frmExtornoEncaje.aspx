<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmExtornoEncaje.aspx.vb" Inherits="Modulos_Tesoreria_Encaje_frmExtornoEncaje" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos")%>
<head runat="server">
    <title>Reversión de Encaje</title>
    <script type="text/javascript">
        function Confirmacion() {
            if (!confirm('¿Está seguro de revertir el encaje?')) {
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Reversión de Encaje</h2></header>
    <br />
    <fieldset>
    <legend></legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:dropdownlist id="ddlPortafolio" runat="server" Width="130px" AutoPostBack="True"></asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Operación</label>
                <div class="col-sm-9">                    
                        <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" 
                            ReadOnly="True" />
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Ultimo Encaje</label>
                <div class="col-sm-9">                    
                        <asp:TextBox runat="server" ID="tbFechaUltimoEncaje" SkinID="Date" 
                            ReadOnly="True" />                    
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <br />
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnExtornar" runat="server" Text="Extornar" OnClientClick="return Confirmacion();" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    </div>
    </form>
</body>
</html>
