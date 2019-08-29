<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCarteraIndirecta.aspx.vb" Inherits="Modulos_Parametria_Tablas_Limites_frmCarteraIndirecta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Cartera Indirecta</title>
        <script type="text/javascript">
            function showModalEmisor() {
                return showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Entidad', '1200', '600', '');
            }

        function Calcular() {
            if (form1.txtPosicion.value != "" && form1.txtPatrimonio.value != "") {
                if (form1.txtPatrimonio.value != "0.0000000") {
                    total = form1.txtPosicion.value.toString().replace(/,/g, '') / form1.txtPatrimonio.value.toString().replace(/,/g, '');
                    num = total;
                    if (num != "") {
                        var pos1 = num.toString().lastIndexOf('.');
                        var pos2 = num.toString().substring(pos1 + 1);
                        var tmp1 = pos2 + '0000000'
                        var tmp2 = tmp1.substr(0, 7);
                        num = num.toString().replace(/$|,/g, '');
                        if (isNaN(num))
                            num = "0";
                        sign = (num == (num = Math.abs(num)));
                        num = Math.floor(num * 100 + 0.50000000001);
                        cents = num % 100;
                        num = Math.floor(num / 100).toString();
                        if (cents < 10) {
                            cents = "0" + cents + '0000000';
                            cents = cents.substr(0, 7);
                        }
                        else
                        { cents = tmp2; }
                        for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                            num = num.substring(0, num.length - (4 * i + 3)) + ',' +
									num.substring(num.length - (4 * i + 3));
                        form1.txtParticipacion.value = (((sign) ? '' : '-') + num + '.' + cents);
                    }
                }
                else {
                    form1.txtParticipacion.value = "0.0000000";
                }
            }
            else {
                form1.txtParticipacion.value = "0.0000000";
            }
            return false;
        }

        function formatCurrency(cajatexto) {
            var num = "";
//            num = txtMonto.value;
            switch (cajatexto) {
                case "txtParticipacion":
                    num = txtParticipacion.value; break;
                case "txtPosicion":
                    num = txtPosicion.value; break;
                case "txtPatrimonio":
                    num = txtPatrimonio.value; break;
//                case "tbNumeroOperacion":
//                    num = tbNumeroOperacion.value; break;
            }

            num = num.toString().replace(/$|,/g, '');
            if (num != "") {
                var pos1 = num.toString().lastIndexOf('.');
                var pos2 = num.toString().substring(pos1 + 1);
                var tmp1 = pos2 + '0000000'
                var tmp2 = tmp1.substr(0, 7);

                num = num.toString().replace(/$|,/g, '');
                if (isNaN(num))
                    num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);

                cents = num % 100;
                num = Math.floor(num / 100).toString();
                if (cents < 10) {
                    cents = "0" + cents + '0000000';
                    cents = cents.substr(0, 7);
                }
                else
                { cents = tmp2; }
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + ',' +
									num.substring(num.length - (4 * i + 3));

//                txtMonto.value = (((sign) ? '' : '-') + num + '.' + cents);
                switch (cajatexto) {
                    case "txtParticipacion":
                        txtParticipacion.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "txtPosicion":
                        txtPosicion.value = (((sign) ? '' : '-') + num + '.' + cents); break;
                    case "txtPatrimonio":
                        txtPatrimonio.value = (((sign) ? '' : '-') + num + '.' + cents); break;
//                    case "tbNumeroOperacion":
//                        tbNumeroOperacion.value = (((sign) ? '' : '-') + num); break;
                }
            }
            return false;
        }
        </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="container-fluid">
        <header><h2>Cartera Indirecta</h2></header>
        <br />  

        <fieldset>
            <legend>Ingreso de Datos</legend>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Fondo</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlPortafolio" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Fondo" ControlToValidate="ddlPortafolio" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6" id="divEmisor" runat="server">
                    <div class="form-group" style="padding-left: 5px;">
                        <label id="Label1" runat="server" class="col-sm-2 control-label">
                            Emisor</label>
                        <div class="col-sm-8">
                            <div class="input-append">
                                <asp:TextBox runat="server" ID="txtEmisor" Width="80px" ReadOnly="true" />
                                <asp:LinkButton ID="lbkModalEmisor" OnClientClick="return showModalEmisor()" runat="server">
                                    <span runat="server" id="imbEmisor" class="add-on"><i class="awe-search"></i></span>
                                </asp:LinkButton>
                            </div>
                            <asp:TextBox runat="server" ID="txtEmisorDesc" Width="220px" ReadOnly="true" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ErrorMessage="Emisor" ControlToValidate="txtEmisor" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Grupo Economico</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlGrupoEconomico" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="GrupoEconomico" ControlToValidate="ddlGrupoEconomico" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div> 
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Actividad Económica</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlActividadEconomica" runat="server"></asp:DropDownList>                                                  
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ErrorMessage="ActividadEconomica" ControlToValidate="ddlActividadEconomica" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Pais</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlPais" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ErrorMessage="Pais" ControlToValidate="ddlPais" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Rating</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlRating" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                            ErrorMessage="Rating" ControlToValidate="ddlRating" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Posición</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtPosicion" runat="server" MaxLength="50" Width="100px"  CssClass="Numbox-7" onblur="Javascript:Calcular(); formatCurrency(txtPosicion.id);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                ErrorMessage="Posición" ControlToValidate="txtPosicion" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Patrimonio Cierre</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtPatrimonio" runat="server" MaxLength="50" Width="100px"  CssClass="Numbox-7" onblur="Javascript:Calcular(); formatCurrency(txtPatrimonio.id);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                ErrorMessage="Patrimonio" ControlToValidate="txtPatrimonio" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Participación (%)</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtParticipacion" runat="server" MaxLength="50" Width="100px"  CssClass="Numbox-7" onblur="Javascript:formatCurrency(txtParticipacion.id);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                                ErrorMessage="Participación" ControlToValidate="txtParticipacion" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label">
                            Fecha</label>
                        <div class="col-sm-9" >
                            <div class="input-append date" id="spanFecha"  runat ="server">
                                <asp:TextBox runat="server" ID="txtFecha" SkinID="Date" />
                                <span id="Span1" runat="server" class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                ErrorMessage="Fecha" ControlToValidate="txtFecha" 
                                ForeColor="Red">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"> Estado</label>
                        <div class="col-sm-9" >
                            <asp:DropDownList ID="ddlSituacion" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                            ErrorMessage="Situacion" ControlToValidate="ddlSituacion" 
                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
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
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" 
                CausesValidation="False" />
                        <asp:HiddenField ID="hd" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
