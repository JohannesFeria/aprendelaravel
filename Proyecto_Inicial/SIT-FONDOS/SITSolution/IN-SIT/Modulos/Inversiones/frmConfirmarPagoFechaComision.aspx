<%@ Page Language="VB"EnableEventValidation="false" AutoEventWireup="false" CodeFile="frmConfirmarPagoFechaComision.aspx.vb" Inherits="Modulos_Inversiones_frmConfirmarPagoFechaComision" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

      <%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts")%>    <%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
    <title>Registrar Fecha para Pago Comision</title>
    <style type="text/css">
        .divGrilla {
            height: 330px;
            border: solid 1px #706f6f;
            overflow-y: auto;
            margin-bottom: 15px;
        }
        
        .divGrilla2 {
            height: 200px;
            border: solid 1px #706f6f;
            overflow-y: auto;
            margin-bottom: 15px;
        }
        .winMensajes
        {
            border: solid 1px #706f6f;
            padding: 5px;
            overflow-y: auto;
            margin-bottom: 15px;            
            }

        .ocultarCol {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> 
     <script language="javascript" type="text/javascript">
         Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(
	        function () {
	            if (document.getElementById) {
	                var progress = document.getElementById('progressEspera');
	                var blur = document.getElementById('blurEspera');
	                var altoPage = document.documentElement.scrollHeight;
	                progress.style.width = '300px';
	                progress.style.height = '300px';
	                blur.style.height = '1200px';
	                //     progress.style.top = altoPage / 3 - progress.style.height.replace('px', '') / 2 + 'px';
	                progress.style.top = '300px'
	                progress.style.left = document.body.offsetWidth / 2 - progress.style.width.replace('px', '') / 2 + 'px';
	            }
	        }
            )

    </script> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
         <div class="container-fluid" id="divContainer">
            <header><h2>Confirmar Pago de Comisión</h2></header>
            <fieldset>
                    <legend>Filtro de búsqueda</legend>
                    <div class="row">
                    
                        
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Portafolio</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlPortafolio" Width="200px" />
                                 </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                           <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    Fecha Corte</label>
                                <div class="col-sm-5">
                                    <div class="input-append date">
                                        <asp:TextBox runat="server" ID="txtFechaCorte" SkinID="Date" />
                                        <span class="add-on"><i class="awe-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                       </div>
                        <div class="col-md-5" style="text-align: right;">
                        
                           <div class="form-group">
                           <asp:Button id="btnBuscar" runat="server" Text="Buscar"/>
                           </div>
                        </div>
                    
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Cuenta de abono</legend>
                    <div class="row">
                    
                        
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Banco</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlBancoAdministradora" AutoPostBack="True" Width="200px" />
                                 </div>
                            </div>
                        </div>

                         <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-sm-3 control-label">
                                    Cuenta Bancaria</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddlNumeroDeCuentaAdministradora" Width="200px" />
                                 </div>
                            </div>
                        </div>
                        <div class="col-md-4" style="text-align: right;">
                        

                        </div>
                    
                    </div>
                </fieldset>

                <br />
                <fieldset>
                    <legend>Solicitudes de Pago de Comisión</legend>
                    
                    <div class="grilla divGrilla" style="overflow: scroll;" id="divGrillaOrdenesEjecutada">
              
                    <asp:GridView ID="gvFondos" runat="server" SkinID="Grid_Paging_No">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" 
                                ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelectPE" runat="server" />
                                    <asp:HiddenField ID="hdCodigoFondo" runat="server" 
                                        Value='<%# Eval("CodigoPortafolioSBS") %>' />
                                    <asp:HiddenField ID="hdCodigoMoneda" runat="server" 
                                        Value='<%# Eval("CodigoMoneda") %>' />
                                    <asp:HiddenField ID="hdCodigoEstado" runat="server" 
                                        Value='<%# Eval("CodigoEstado") %>' />
                                    <asp:HiddenField ID="hdNombreFondo" runat="server" 
                                        Value='<%# Eval("Portafolio") %>' />
                                    <asp:HiddenField ID="hdNumeroDeCuenta" runat="server" 
                                        Value='<%# Eval("NumeroCuenta") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibBorrar" runat="server" CommandName="Eliminar" 
                                        SkinID="imgDelete" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoPortafolioSBS" 
                                HeaderStyle-CssClass="ocultarCol" HeaderText="CodigoPortafolio" 
                                ItemStyle-CssClass="ocultarCol" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Portafolio" HeaderText="Portafolio" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="FechaCorte" HeaderText="Fecha Corte" 
                                ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="ocultarCol" HeaderStyle-CssClass="ocultarCol" />
                            <asp:BoundField DataField="FechaSolicitudCadena" HeaderText="Fecha Liquidación" 
                              ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Periodo"  HeaderText="Periodo" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Cuenta de Cargo" 
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCodigoBanco" runat="server" 
                                        Value='<%# Eval("CodigoBanco") %>' />
                                    <asp:DropDownList ID="ddlBanco" runat="server" AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddlBanco_SelectedIndexChanged" Width="250px" />
                                    <asp:DropDownList ID="ddlNumeroDeCuenta" runat="server" Width="280px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CodigoMoneda" HeaderText="Mon" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ComisionAcumulada" DataFormatString="{0:#,##0.00}" 
                                HeaderText="Comisión" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="UsuarioSolicitud" HeaderText="Operador" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="NombreEstado" HeaderText="Estado" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CodigoBanco" HeaderStyle-CssClass="ocultarCol" 
                                HeaderText="CodigoBanco" ItemStyle-CssClass="ocultarCol" />
                            <asp:BoundField DataField="FechaFin" HeaderStyle-CssClass="ocultarCol" 
                                HeaderText="FechaFin" ItemStyle-CssClass="ocultarCol" />
                            <asp:BoundField DataField="NombreMoneda" HeaderStyle-CssClass="ocultarCol" 
                                HeaderText="NombreMoneda" ItemStyle-CssClass="ocultarCol" />
                            <asp:BoundField DataField="CodigoEstado" HeaderStyle-CssClass="ocultarCol" 
                                HeaderText="CodigoEstado" ItemStyle-CssClass="ocultarCol" />
                            <asp:BoundField DataField="Id" HeaderStyle-CssClass="ocultarCol" 
                                HeaderText="Id" ItemStyle-CssClass="ocultarCol" />
                                <asp:BoundField DataField="FechaCaja" HeaderStyle-CssClass="ocultarCol" 
                                HeaderText="FechaCaja" ItemStyle-CssClass="ocultarCol" />
                                
                        </Columns>
                    </asp:GridView>
                    </div>

                    
                    <div class="row" id="div2" runat="server" style="text-align: right;">
             
                        <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar"/>
                    </div>

                    <div style="display:none">
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" />
                    <asp:Button ID="btnBuscarAuxiliar" runat="server" Text="buscarAuxiliar"  />
                    <asp:Label ID="lblFondoEliminar" runat="server" text=""></asp:Label>
                    <asp:Label ID="lblIdentificador" runat="server" text=""></asp:Label>
                    <asp:HiddenField runat="server" ID="hdRptaConfirmar" />
                    </div>
                    <asp:HiddenField ID="hdEliminarConfirmar" runat="server" />
                 </fieldset>
          </div>

          <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id="blurEspera" />
                    <div id="progressEspera">
                        <img src="../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 100px;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

       </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
