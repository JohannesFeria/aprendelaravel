<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmLimites.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmLimites" %>

<!DOCTYPE html>
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %>
<%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<script src="../../../../App_Themes/js/inputmask/jquery.maskedinput.min.js" type="text/javascript"></script>
<head id="Head1" runat="server">
    <title>Límites</title>
    <link href="../../../../App_Themes/css/bootstrap-multiselect.css" rel="stylesheet"
        type="text/css" />
    <script src="../../../../App_Themes/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <style type="text/css">
        .divGrilla
        {
            height: 420px;
            border: solid 1px #706f6f;
            overflow-y: auto;
            margin-bottom: 15px;
            padding: 5px;
        }
    </style>
    <script type="text/javascript">
        function Numero() {
            tecla = window.event.keyCode
            if ((tecla >= 48 && tecla <= 57) || tecla == 32 || tecla == 46) { }
            else { window.event.keyCode = 0 }
        }

        function changeChbValorPorcPorAgrup(e) {
            var isCheckedPorcPorAgrup = $(this)[0].checked;
            if (isCheckedPorcPorAgrup) {
                document.getElementById('txtValorPorcPorAgrup').readOnly = false;
            } else {
                document.getElementById('txtValorPorcPorAgrup').readOnly = true;
            }
        }

        function onClickChbValorPorcPorAgrup(isCheckedPorcPorAgrup) {
            console.log(isCheckedPorcPorAgrup);
            if (isCheckedPorcPorAgrup) {
                document.getElementById('txtValorPorcPorAgrup').readOnly = false;
            } else {
                document.getElementById('txtValorPorcPorAgrup').readOnly = true;
                document.getElementById('txtValorPorcPorAgrup').value = 0;
            }
        }

        $('form').live("submit", function () {
            //$('body').append('<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;top: 0; left: 0; background-color: Black; filter: alpha(opacity=40); opacity: 0.4;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            $('body').append('<div id="divBackground" style="position: fixed; height: 100%; width: 100%;top: 0; left: 0; background-color: White; filter: alpha(opacity=80); opacity: 0.6;-moz-opacity: 0.6; display: block"><input type="hidden" name="_target" id="_target" value="" /></div>');
            ShowProgress();
        });

        //        function onClickLimiteVinculado(isCheckedLV) {
        //            console.log(isCheckedLV);
        //            if (isCheckedLV) {
        //                document.getElementById('txtPorcentajeVinculado').readOnly = false;
        //            } else {
        //                document.getElementById('txtPorcentajeVinculado').readOnly = true;
        //                document.getElementById('txtPorcentajeVinculado').value = 0;
        //            }
        //        }

        function onClickCastigoRating(isCheckedLV) {
            console.log(isCheckedLV);
            if (isCheckedLV) {
                document.getElementById('txtCastigoRating').readOnly = false;
            } else {
                document.getElementById('txtCastigoRating').readOnly = true;
                document.getElementById('txtCastigoRating').value = 0;
            }
        }

        //        $(document).ready(function () {
        //            $('#chbValorPorcentajePorAgrup').on('change', changeChbValorPorcPorAgrup);

        //            var isCheckedPorcPorAgrup = document.getElementById('chbValorPorcentajePorAgrup').checked;
        //            if (isCheckedPorcPorAgrup) {
        //                document.getElementById('txtValorPorcPorAgrup').readOnly = false;
        //            } else {
        //                document.getElementById('txtValorPorcPorAgrup').readOnly = true;
        //            }
        //        });
        $(function () {
            $('[id*=lbValidadores]').multiselect({
                includeSelectAllOption: true
            });
            $('[id*=lbTipoCuenta]').multiselect({
                includeSelectAllOption: true
            });
        });

        function Validadores() {
            $("[id$='ddlvalorbase']").change(function () {
                var ValorBase = $("#ddlvalorbase").val();
                if (ValorBase.toString() == "CARTERA") {
                    $("#DivTipoCuenta").css("display", "block")
                } else {

                    $("#DivTipoCuenta").css("display", "none")
                }
            });
        };

    </script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UPpagina" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <header><h2>Mantenimiento de Límites</h2></header>
                <br />
                <div class="Contenedor">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <fieldset style="border: 0px;">
                                    <legend style="border: 0px; margin-left: 0px; width: 102%; border-bottom: 1px solid gray;">
                                        Datos Generales</legend>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Código Límite</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="tbCodigo" runat="server" Width="90px" MaxLength="5"></asp:TextBox>
                                                    <strong>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Código Límite"
                                                            ControlToValidate="tbCodigo" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator></strong>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Nombre Límite</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="tbNombreLimite" runat="server" Width="336px" MaxLength="100" Style="text-transform: uppercase;"></asp:TextBox>
                                                    <strong>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Nombre Límite"
                                                            ControlToValidate="tbNombreLimite" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator></strong>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Tipo Límite</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlTipoLimite" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                    <strong>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Tipo Límite"
                                                            ControlToValidate="ddlTipoLimite" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator></strong>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Unidad Posición</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlunidadposicion" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Unidad Posición"
                                                        ControlToValidate="ddlunidadposicion" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Valor Base</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlvalorbase" runat="server" Width="170px" onchange="javascript:Validadores();">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Valor Base"
                                                        ControlToValidate="ddlvalorbase" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Tipo Cálculo</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddltipocalculo" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                    <strong>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Tipo Cálculo"
                                                            ControlToValidate="ddltipocalculo" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator></strong>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Tope Límite</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlTopeLimite" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Tope Límite"
                                                        ControlToValidate="ddlTopeLimite" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Clase Límite</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlclaselimite" runat="server" Width="170px" />
                                                    <asp:RequiredFieldValidator ID="rfvClaseLimite" runat="server" ErrorMessage="Clase Límite"
                                                        ControlToValidate="ddlclaselimite" ForeColor="Red" ValidationGroup="btnAgregar">(*)</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Clase Límite"
                                                        ControlToValidate="ddlclaselimite" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Universo de Límites</label>
                                                <div class="col-md-9">
                                                    <asp:DropDownList ID="ddlPosicion" runat="server" Width="170px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Posición"
                                                        ControlToValidate="ddlPosicion" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Situación</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlSituacion" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Situación"
                                                        ControlToValidate="ddlSituacion" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">
                                                    Límite Vinculado</label>
                                                <div class="col-md-1" style="padding-top: 8px;">
                                                    <%--<asp:CheckBox ID="chkLimiteVinculado" runat="server" OnClick="onClickLimiteVinculado(this.checked);" />--%>
                                                    <asp:CheckBox ID="chkLimiteVinculado" runat="server" AutoPostBack="true" />
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    % Vinculado</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txtPorcentajeVinculado" runat="server" CssClass="Numbox-3" Width="100px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label" for="chkCastigoRating">
                                                    Castigo Rating</label>
                                                <div class="col-md-1" style="padding-top: 8px;">
                                                    <asp:CheckBox ID="chkCastigoRating" runat="server" OnClick="onClickCastigoRating(this.checked);" />
                                                </div>
                                                <label class="col-md-3 control-label" for="txtCastigoRating">
                                                    Niveles</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txtCastigoRating" runat="server" CssClass="Numbox-0" Width="35px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label" for="chkCastigoRating">
                                                    Aplica Forward</label>
                                                <div class="col-md-1" style="padding-top: 8px;">
                                                    <asp:CheckBox ID="chkAplicaForward" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Validadores</label>
                                                <div class="col-md-9">
                                                    <asp:ListBox ID="lbValidadores" runat="server" SelectionMode="Multiple" Width="170px">
                                                    </asp:ListBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Market Share</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="tbMarketShare" runat="server" Width="138px" MaxLength="50" CssClass="Numbox-0"></asp:TextBox><strong>(%)</strong>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Aplicar Castigo</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlcastigo" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Castigo"
                                                        ControlToValidate="ddlcastigo" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Factor</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlfactor" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">
                                                    Saldo Banco</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlSaldobanco" runat="server" Width="170px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Saldo Banco"
                                                        ControlToValidate="ddlSaldobanco" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label" for="chbValorPorcentajePorAgrup">
                                                    % Por Agrupador</label>
                                                <div class="col-md-1" style="padding-top: 8px;">
                                                    <asp:CheckBox ID="chbValorPorcentajePorAgrup" runat="server" OnClick="onClickChbValorPorcPorAgrup(this.checked);" />
                                                </div>
                                                <label class="col-md-3 control-label" for="txtValorPorcPorAgrup">
                                                    Valor %</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txtValorPorcPorAgrup" runat="server" CssClass="Numbox-3" Width="100px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row"  >
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label id="lblTipoCuenta" runat="server" class="col-md-3 control-label">
                                                    Tipo Cuenta</label>
                                                <div class="col-md-9">
                                                    <asp:ListBox ID="lbTipoCuenta" runat="server" SelectionMode="Multiple" Width="170px">
                                                    </asp:ListBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                        <div class="loading" align="center">
                            <%--Loading. Please wait.<br /><br />--%>
                            <img src="../../../../App_Themes/img/icons/loading.gif" style="height: 70px;" />
                        </div>
                        <div class="col-md-6">
                            <fieldset style="border: 0px;">
                                <legend style="border: 0px; margin-left: 0px; width: 102%; border-bottom: 1px solid gray;">
                                    Porcentaje Cerca al Limite</legend>
                                <div class="row">
                                    <div class="grilla-small">
                                        <asp:GridView ID="dgCercaLimite" runat="server" SkinID="GridSmall" AutoGenerateColumns="False"
                                            PageSize="5">
                                            <Columns>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Porcentaje (%)">
                                                    <ItemTemplate>
                                                        <asp:TextBox onkeypress="Javascript:Numero();" ID="txtPorcentaje" runat="server"
                                                            Width="60px" MaxLength="5" Text='<%#DataBinder.Eval(Container.DataItem, "PorcentajeCercaLimite")%>'>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="grilla-small">
                                            <asp:GridView ID="dgLiquidez" runat="server" SkinID="GridSmall" GridLines="None"
                                                AutoGenerateColumns="False" PageSize="5">
                                                <Columns>
                                                    <asp:BoundField DataField="TipoLiquidez" HeaderText="Tipo Liquidez"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Porcentaje (%)">
                                                        <ItemTemplate>
                                                            <asp:TextBox onkeypress="Javascript:Numero();" ID="txtPorcentaje0" onblur="javascript: FormateaDecimal(this,7);"
                                                                runat="server" CssClass="stlCajaTexto" Width="120px" MaxLength="12" Text='<%#DataBinder.Eval(Container.DataItem, "Porcentaje")%>'></asp:TextBox>
                                                            %
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <!-- INI MPENAL - 22/09/16 -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group" style="border: 1px solid #808080; padding: 5px 0">
                                            <div class="col-md-2">
                                                <label class="control-label" for="rdbCuadrar">
                                                    Cuadrar:</label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Cuadrar"
                                                    ControlToValidate="rdbCuadrar" ForeColor="Red" ValidationGroup="btnAceptar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:RadioButtonList ID="rdbCuadrar" runat="server" RepeatDirection="Vertical">
                                                    <%-- <asp:ListItem Text="Ninguno" Value="0" />
					                        <asp:ListItem Text="Participación al 100%" Value="1" />
                                            <asp:ListItem Text="Suma Valor Contable" Value="2" />
                                            <asp:ListItem Text="Suma Posición" Value="3" /> --%>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- FIN MPENAL - 22/09/16 -->
                            </fieldset>
                        </div>
                    </div>
                </div>
                <br />
                <div class="Contenedor">
                    <div class="row">
                        <div class="col-md-6">
                            <fieldset style="border: 0px;">
                                <legend style="text-align: center; border: 0px; margin-left: 0px; width: 106%; border-bottom: 1px solid gray;">
                                    Portafolios Aplicados</legend>
                                <div class="row" style="text-align: center;">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" ValidationGroup="btnAgregar" />
                                </div>
                            </fieldset>
                        </div>
                        <div class="col-md-6">
                            <fieldset style="border: 0px;">
                                <legend style="text-align: center; border: 0px; margin-left: 0px; width: 104%; border-bottom: 1px solid gray;">
                                    &nbsp;</legend>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row divGrilla" style="width: 100%; margin-left: 0px;">
                    <asp:GridView ID="dgLista" runat="server" Visible="true" GridLines="None" CellPadding="1"
                        AutoGenerateColumns="False" PageSize="5" SkinID="Grid_AllowPaging_NO">
                        <Columns>
                            <asp:TemplateField HeaderText="Modificar" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibModificar" runat="server" SkinID="imgEdit" CommandName="Modificar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodigoLimiteCaracteristica") %>'>
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="CodigoLimiteCaracteristica"
                                HeaderText="CodigoLimiteCaracteristica"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" DataField="codigoLimite"
                                HeaderText="CodigoLimite"></asp:BoundField>
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo L&#237;mite" ItemStyle-Width="100px"
                                ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Portafolio"></asp:BoundField>
                            <%-- <asp:BoundField DataField="Situacion" HeaderText="Situaci&#243;n"></asp:BoundField>--%>
                            <asp:TemplateField HeaderText="Situaci&#243;n" HeaderStyle-Width="160px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdSituacion" runat="server" Value='<%# Eval("Situacion") %>' />
                                    <asp:DropDownList ID="ddlSituacionPortafolio" runat="server" Width="156px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoPortafolioSBS" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <header>
        </header>
                <div class="row">
                    <div class="col-md-6" style="text-align: left;">
                        <asp:Button ID="btnExportar" runat="server" Text="Exportar" CausesValidation="False" />
                    </div>
                    <div class="col-md-6" style="text-align: right;">
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="btnAceptar" />
                        <asp:Button ID="btnRetornar" runat="server" Text="Retornar" CausesValidation="False" />
                        <asp:HiddenField ID="hd" runat="server" />
                    </div>
                </div>
                <asp:ValidationSummary runat="server" ID="vsCabecera" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="btnAgregar" HeaderText="Los siguientes campos son obligatorios:" />
                <asp:ValidationSummary runat="server" ID="ValidationSummary1" ShowMessageBox="true"
                    ShowSummary="false" ValidationGroup="btnAceptar" HeaderText="Los siguientes campos son obligatorios:" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportar" />
            <asp:AsyncPostBackTrigger ControlID="chkLimiteVinculado" EventName="CheckedChanged" />
            <asp:PostBackTrigger ControlID="btnAceptar" />
            <asp:PostBackTrigger ControlID="btnRetornar" />
            <asp:PostBackTrigger ControlID="lbValidadores" />
            <asp:PostBackTrigger ControlID="lbTipoCuenta" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
