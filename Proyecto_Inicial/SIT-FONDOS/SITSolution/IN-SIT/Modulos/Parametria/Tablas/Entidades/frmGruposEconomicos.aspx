<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmGruposEconomicos.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmGruposEconomicos" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Grupos Economicos</title>
    <script type="text/javascript" language="javascript">
        var strMensajeError = "";

        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";

            String.prototype.trim = function () {
                return this.replace(/^\s+|\s+$/g, "");
            }

            if (document.getElementById("<%= tbCodigo.ClientID %>").value.trim() == "")
                strMsjCampOblig += "\t-Código Grupo Económico\n"

            if (document.getElementById("<%= tbDescripcion.ClientID %>").value.trim() == "")
                strMsjCampOblig += "\t-Descripción Grupo Económico\n"

            if (document.getElementById("<%= ddlSituacion.ClientID %>").value == "--Seleccione--")
                strMsjCampOblig += "\t-Situación\n"

            if (strMsjCampOblig != "") {
                strMensajeError += ERR_CAMPO_OBLIGATORIO + strMsjCampOblig + "\n";
                return false;
            }
            {
                return true;
            }
        }

        function Validar() {
            strMensajeError = "";
            if (ValidaCamposObligatorios()) {
                return true;
            }
            else {
                alert(strMensajeError);
                return false;
            }
        }
	</script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header>
    <div class="row"><h2>Mantenimiento de Grupos Economicos</h2></div>
    </header>
    <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">                    
                    <label class="col-md-4 control-label">Código</label>
                    <div class="col-md-8"><asp:TextBox id="tbCodigo" runat="server" MaxLength="4" ForeColor="#404040" Width="50px" ></asp:TextBox></div>
                </div>
                </div>
                <div class="col-md-8"></div>                
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">                    
                    <label class="col-md-4 control-label">Descripción</label>
                    <div class="col-md-8"><asp:TextBox ID="tbDescripcion" CssClass="stlCajaTexto" runat="server" Width="360px" MaxLength="50"></asp:TextBox></div>                
                </div>
                </div>
                <div class="col-md-8"></div>                
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-md-4 control-label">Situación</label>
                        <div class="col-md-8">
                            <asp:dropdownlist id="ddlSituacion" runat="server" Width="115px" >
						        <asp:ListItem Value="N">Ninguno</asp:ListItem>
						        <asp:ListItem Value="A">Activo</asp:ListItem>
						        <asp:ListItem Value="I">Inactivo</asp:ListItem>
					        </asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-8" style="text-align: right;">
                    <asp:CheckBox ID="chkEntidadVinculada" Runat="server" Text="Grupo Vinculado"></asp:CheckBox>
                </div>                
            </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row">
        <div class="col-md-10"></div>        
        <div class="col-md-2">
            <div class="form-group">
                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" />
                <asp:Button runat="server" ID="btnRetornar" Text="Retornar" CausesValidation="false"/>
                <asp:HiddenField ID="hd" runat="server" />
            </div>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
