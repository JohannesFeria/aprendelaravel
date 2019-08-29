<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmIntermediarioContacto.aspx.vb" Inherits="Modulos_Parametria_Tablas_Entidades_frmIntermediarioContacto" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Intermediario Contacto</title>

    <script type="text/javascript">
        var strMensajeError = "";
        var ERR_CAMPO_OBLIGATORIO = "Los siguientes campos son obligatorios:\n";
        var ERR_CONFIRMACION_PASSWORD = "El Password no coincide con lo ingresado en la confirmación.\n\n";

        function ValidaCamposObligatorios() {
            var strMsjCampOblig = "";

            if (document.getElementById("<%= ddlTercero.ClientID %>").value == "")
                strMsjCampOblig += "\t-Tercero\n"

            if (document.getElementById("<%= ddlContacto.ClientID %>").value == "")
                strMsjCampOblig += "\t-Contacto\n"

            if (document.getElementById("<%= ddlSituacion.ClientID %>").value == "")
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
            <div class="row">
                <h2>
                    Intermediario Contacto</h2>
            </div>
        </header>
        <fieldset>
            <legend>Datos Generales</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">
                            Tercero</label>
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlTercero" runat="server" Width="232px"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Contacto</label>
                        <div class="col-md-9">
						    <asp:dropdownlist id="ddlContacto" runat="server" Width="232px" ></asp:dropdownlist>                            
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Telefono 1</label>
                        <div class="col-md-9">
                            <asp:textbox id="tbTelefono1" runat="server" Width="232px"  MaxLength="20"></asp:textbox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Anexo 1</label>
                        <div class="col-md-9">
						    <asp:textbox id="tbAnexo1" runat="server" Width="232px"  MaxLength="20"></asp:textbox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Telefono 2</label>
                        <div class="col-md-9">
                            <asp:textbox id="tbTelefono2" runat="server" Width="232px" MaxLength="20"></asp:textbox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Anexo 2</label>
                        <div class="col-md-9">
						    <asp:textbox id="tbAnexo2" runat="server" Width="232px"  MaxLength="20"></asp:textbox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Telefono 3</label>
                        <div class="col-md-9">
                            <asp:textbox id="tbTelefono3" runat="server" Width="232px" MaxLength="20"></asp:textbox>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Anexo 3</label>
                        <div class="col-md-9">
						    <asp:textbox id="tbAnexo3" runat="server" Width="232px"  MaxLength="20"></asp:textbox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Situación</label>
                        <div class="col-md-9">
                            <asp:dropdownlist id="ddlSituacion" runat="server" Width="104px"  ></asp:dropdownlist>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">                    
                </div>
            </div>
        </fieldset>
        <br />
        <header>
        </header>
        <br />
        <div class="row">
        <div class="col-md-10"></div>        
        <div class="col-md-2">
            <div class="form-group">                
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />                
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"/>
                <asp:HiddenField ID="hdCodigo" runat="server" />
            </div>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
