<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaTipoCambio.aspx.vb" Inherits="Modulos_Valorizacion_y_Custodia_Valorizacion_frmConsultaTipoCambio" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>

<head runat="server">
    <title>Consulta Tipo de Cambio</title>
        <script type="text/javascript">

            function showPopup(pFecha, pInfo) {
                return showModalDialog('frmTipoDeCambio.aspx?nFechaOperacion=' + pFecha + '&sFuenteInformacion=' + pInfo, '800', '600', '');
            }

            var DiasDelMes = new Array(12);
            DiasDelMes[1] = 31;
            DiasDelMes[2] = 29; //esto debe checkearse mediante programa
            DiasDelMes[3] = 31;
            DiasDelMes[4] = 30;
            DiasDelMes[5] = 31;
            DiasDelMes[6] = 30;
            DiasDelMes[7] = 31;
            DiasDelMes[8] = 31;
            DiasDelMes[9] = 30;
            DiasDelMes[10] = 31;
            DiasDelMes[11] = 30;
            DiasDelMes[12] = 31;

            function ValidaFecha() {
                var controlfecha;
                controlfecha = document.getElementById("<%= tbFechaOperacion.ClientID %>");
                var Fecha = controlfecha.value;

                if (!IsDate(Fecha)) {
                    alert('Formato de fecha de Operación no válido, verifique');
                    return false;
                }
                else {
                    return true;
                }
            }

            function IsDate(s) {
                var dia;
                var mes;
                var anio;

                anio = parseInt(s.substr(6, 4), 10);
                mes = parseInt(s.substr(3, 2), 10);
                dia = parseInt(s.substr(0, 2), 10);

                if (s.length == 10)
                //Si es un año valido
                    if (anio > 1900)
                    //Si es un mes valido
                        if ((mes > 0) && (mes < 13))
                        //Si es un dia valido
                            if ((dia > 0) && (dia < 32))
                            //Si es mes de febrero
                                if (mes == 2)
                                //Si es mes de febrero con 29 dias
                                //Febrero tiene 29 dias en cualquier año que sea multiplo de 4,
                                //excepto los años centenarios ademas de divisibles por 400
                                    if ((anio % 4 == 0) && ((!(anio % 100 == 0)) || (anio % 400 == 0)))
                                    //Si esta en el rango[1-29]
                                        if ((dia > 0) && (dia < 30))
                                            return true;
                                        else
                                            return false;
                                    else
                                    //Si esta en el rango[1-28]
                                        if ((dia > 0) && (dia < 29))
                                            return true;
                                        else
                                            return false;
                                else
                                //Rango de numero de dias permitido para los demas meses

                                    if ((dia > 0) && (dia <= DiasDelMes[mes]))
                                        return true;
                                    else
                                        return false;
                            else
                                return false;
                        else
                            return false;
                    else
                        return false;
                else
                    return false;
            }

        </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
    <header><h2>Consulta de Tipo de Cambio</h2></header>
    <fieldset>
    <legend></legend>

    <div class="row">
      <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fecha Operación</label>
                <div class="col-sm-9">
                <div class="input-append date" runat="server" id="ingFecha">
                    <asp:TextBox runat="server" ID="tbFechaOperacion" SkinID="Date" />
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
                <label class="col-sm-3 control-label">Fuente</label>
                <div class="col-sm-9">
                <asp:dropdownlist id="ddlEntidadExterna" runat="server"></asp:dropdownlist>
            </div>
            </div>
        </div>
        <div class="col-md-6" style="text-align:right;">
            <asp:Button ID="btnConsulta" runat="server" Text="Buscar" />
            <asp:Button ID="btnBloomberg" runat="server" Text="Bloomberg" Visible="False" />
        </div>
    </div>
    </div>
    </fieldset> 

    <div class="grilla">
                <asp:GridView ID="dgTipoCambio" runat="server" AutoGenerateColumns="False" 
                    GridLines="None" SkinID="Grid" DataKeyNames="CodigoMoneda,Fecha,EntidadExt">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="25">
                            <ItemTemplate>
                                <asp:ImageButton ID="Imagebutton3" runat="server" SkinID="imgCheck" onrowcommand="dgTipoCambio_RowCommand" CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                            </asp:ImageButton>                            
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                        <asp:BoundField DataField="Tipo de Cambio - Secundario" 
                            DataFormatString="{0:#,##0.0000000}" 
                            HeaderText="Tipo de Cambio - Secundario (Dolares)" />
                        <asp:BoundField DataField="Tipo de Cambio - Primario" 
                            DataFormatString="{0:#,##0.0000000}" 
                            HeaderText="Tipo de Cambio - Primario (Soles)" />
                        <asp:BoundField DataField="CodigoMoneda" HeaderText="CodigoMoneda" 
                            Visible="False"  />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="False"  />
                        <asp:BoundField DataField="EntidadExt" HeaderText="EntidadExt" 
                            Visible="False" />
                    </Columns>
                </asp:GridView>
    </div>  
    <fieldset>
    <legend></legend>
    <%--<div class="row">
        <div class="col-md-7">
            <div class="form-group">
                <label class="col-sm-3 control-label">Fuente Información</label>
                <div class="col-sm-9">                
                    <asp:TextBox runat="server" ID="txtActual" ReadOnly="True" />                
                </div>
            </div>
        </div>
    </div>--%>
    
    <div class="row">
        <div class="col-md-7">
            <div class="form-group">
                <label class="col-sm-3 control-label">Moneda</label>
                <div class="col-sm-9">                
                    <asp:TextBox runat="server" ID="txtMoneda" Enabled="False" />
                    <asp:DropDownList ID="dlMoneda" runat="server" AutoPostBack="True" 
                        Visible="False">
                    </asp:DropDownList>                
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-7">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Cambio Primario</label>
                <div class="col-sm-9">                
                    <asp:TextBox runat="server" ID="txtTipoCambioPrimario" MaxLength="30" 
                        TextAlign="Right" Enabled="False" />&nbsp;Soles                    
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-7">
            <div class="form-group">
                <label class="col-sm-3 control-label">Tipo Cambio Secundario</label>
                <div class="col-sm-9">                
                    <asp:TextBox runat="server" ID="txtTipoCambio" MaxLength="30" TextAlign="Right" 
                        Enabled="False"/>&nbsp;Dólares                    
                </div>
            </div>
        </div>
    </div>

    </fieldset>

    <br />
    <header></header>
    <div class="row" style="text-align: right;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Visible="False" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="False" />
        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" />
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" />
        <asp:Button ID="btnSalir" runat="server" Text="Salir" />
    </div>
    <br />
    </div>
    </form>
</body>
</html>
