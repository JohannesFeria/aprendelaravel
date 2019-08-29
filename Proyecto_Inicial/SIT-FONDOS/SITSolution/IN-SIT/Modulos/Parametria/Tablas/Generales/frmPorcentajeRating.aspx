<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPorcentajeRating.aspx.vb" Inherits="Modulos_Parametria_Tablas_Generales_frmPorcentajeRating" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function showPopup() {
            return showModalDialog('../../../Parametria/frmHelpControlParametria.aspx?tlbBusqueda=Rating', '1200', '600', '');   
        }

        function formatTotal(myElem) {
            var num = myElem.value;
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '00'
                var tmp2 = tmp1.substr(0, 2);

                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '00';
                    cents = cents.substr(0, 2);
                }
                else
                { cents = tmp2; }

                if (pos1 == -1) {
                    tmp2 = '00';
                }

                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
						num.substring(num.length - (4 * i + 3));

                myElem.value = (((sign) ? '' : '-') + num + '.' + tmp2);
                myElem.value = (((sign) ? '' : '-') + num + '.' + tmp2);

            }
            return false;

        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
    <header><h2>Mantenimiento de Porcentajes por Rating</h2></header>
    <br />
    <fieldset>
    <legend>Datos Generales</legend>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Rating</label>
                <div class="col-sm-9">
                    <div class="input-append">
                        <asp:TextBox runat="server" ID="tbRating" CssClass="input-medium" />
                        <asp:LinkButton ID="lkbBuscarRating" runat="server" CausesValidation="false" OnClientClick="return showPopup();"><span class="add-on"><i class="awe-search"></i></span></asp:LinkButton>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Rating" ControlToValidate="tbRating">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo de Inversión</label>
                <div class="col-sm-9">
                    <asp:DropDownList id="ddlTipoInv" runat="server" Width="250px"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ErrorMessage="Tipo de Inversión" ControlToValidate="ddlTipoInv">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Periodo de Inversión</label>
                <div class="col-sm-9">
                    <asp:DropDownList id="ddlPeriodoInv" runat="server" Width="100px"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="Periodo de Inversión" ControlToValidate="ddlPeriodoInv">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Portafolio</label>
                <div class="col-sm-9">
                    <asp:DropDownList id="ddlPortafolio" runat="server" Width="100px"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ErrorMessage="Portafolio" ControlToValidate="ddlPortafolio">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Porcentaje (%)</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbPorcentaje" onblur="javascript:formatTotal(this);" runat="server" ></asp:textbox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                        ErrorMessage="Porcentaje (%)" ControlToValidate="tbPorcentaje">(*)</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Grupo Rating</label>
                <div class="col-sm-9">
                    <asp:textbox id="tbGrupoRating" runat="server" MaxLength="15"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Situación</label>
                <div class="col-sm-9">
                    <asp:DropDownList id="ddlSituacion" runat="server" Width="100px"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-6"></div>
    </div>
    </fieldset>
    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar"/>
        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
            CausesValidation="False"/>
            <asp:HiddenField ID="hdCodRating" runat="server" />
    </div>
    </div>
    <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
        HeaderText="Los siguientes campos son obligatorios:" />
    </form>
</body>
</html>
