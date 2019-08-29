<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmFacturasNegociables.aspx.vb"
    Inherits="Modulos_Inversiones_InstrumentosNegociados_frmFacturasNegociables" %>

<!DOCTYPE html >
<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head id="Head1" runat="server">
    <title>Administración Portafolios</title>
    
    <script type="text/javascript" src="App_Themes/js/jquery.js"></script>

    
    <script type="text/javascript">

        $(document).ready(function () {
            $("#Popup01_btnCerrar").click(function () { $("#popup01").hide(); });

        });

        function agregarFilasModalColumnas(columnas,titulo) {
            $('#lblObservacion').text(titulo);
            eliminarFilasTabla();
            var listaColumnas = columnas.split(",");

            jQuery.each(listaColumnas, function (i, val) {
                //  $("#" + val).text("Mine is " + val + ".");
                $('#tablaCuentas > tbody:last-child').append('<tr id="filatabla"><td>' + val + '</td></tr>');
            });


            $("#popup01").show();
        }

 
        function mostrarColumnasObservadas() {
            $("#popup01").show();
        }

        function eliminarFilasTabla() {
            $('#tablaCuentas tbody tr').each(function () {
                $(this).remove();
            });
        }
        
   

        var formulario = {
            rbtnFileServer: $('#<%=rbtnFileServer.ClientID %>'),
            rbtnCarga: $('#<%=rbtnWeb.ClientID %>'),
            lblRuta: $('#lblRuta'),
            ifCargaArchivo: $('#<%=fpRutaF.ClientID %>'),
            txtRutaFileServer: $('#<%=txtRutaFileServer.ClientID %>')
        };

        formulario.rbtnFileServer.change(function () {
            console.info('evento rbtnFileServer');
            formulario.txtRutaFileServer.show();
            formulario.ifCargaArchivo.next().hide();
            formulario.lblRuta.text('Ruta File Server');
        });

        formulario.rbtnCarga.change(function () {
            console.info('evento rbtnCarga');
            formulario.txtRutaFileServer.hide();
            formulario.ifCargaArchivo.next().show();
            formulario.lblRuta.text('Archivo');
        });
        
    </script>

    <style type="text/css">
        div.sura-input-file > div.bootstrap-filestyle
        {
            margin-left: 5px;
        }
         .win01 {
            border: solid 1px gray;
            background-color: #80808052;
            position: absolute;
            z-index: 10;
            width: 100%;
            height: 100%;
            text-align: center;     
        }        
        .cont01 {
            border: solid 1px gray;
            background-color: white;
            display: inline-block;
            
            margin-top: 120px;
            padding: 10px 20px; 
            border-radius: 5px;        
        }
        
        .tab01 {
            width: 100%;
        }
        .tab01>tbody>tr>td {
            padding: 2px 5px;
            border: Solid 1px gray;
        }        
        .span01 {
            font-size: 17px;
        }        

    </style>


</head>
<body>

  <script type="text/javascript">
      $(document).ready(function () {

          $("#<%=btnProcesar.ClientID%>").click(function () {
              $("#popupCargando").show();
              $("#popupCargando_loading").show();
              return true;
          });

          $("#<%=btnValidar.ClientID%>").click(function () {
              $("#popupCargando").show();
              $("#popupCargando_loading").show();
              return true;
          });

          $("#<%=btnImportar.ClientID%>").click(function () {
              $("#popupCargando").show();
              $("#popupCargando_loading").show();
              return true;
          });

      });
      
    </script>

<div id="popupCargando" class="win01" style="display: none">
    <div class="winBloqueador-inner" id="popupCargando_loading">
        <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 120px;" />
    </div>
</div>

 <div id="popup01" class="win01" style="display:none">
        <div class="cont01">
            <span id="lblObservacion" class="span01"></span><br /><br />
            
            <table id="tablaCuentas" class="tab01">
             <thead>
                <tr style="background-color: #e3d829; font-weight:bold">
                  <th>Observaciones</th>
                </tr>
              </thead>
               <tbody>
               <tr id="filatabla"><td>
               ejemplo
               </td>
               </tr>
               </tbody>
            </table>
            <br>
            <input type="submit" value="OK" id="Popup01_btnCerrar" class="btn btn-integra" style="min-width: 80px; text-align: center; width:auto;" />
      </div>
    </div>

    <form id="form1" runat="server" class="form-horizontal">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <header><h2>Facturas Negociables</h2></header>
        <br />
        <fieldset>
            <legend></legend>
            <div class="row">
                <div class="form-group">
                       <div class="col-sm-2">
                       </div>
                    <div class="col-sm-3">
                    <label class="col-sm-2 control-label">
                        Portafolio</label>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlFondo" AutoPostBack="true" runat="server" style="margin-left:30px;width:180px;"/>
                            
                        </div>
                          </div>
                   
                        <div class="col-sm-3"  style="margin-left:20px;">
                        
                            <asp:UpdatePanel Visible="true" ID="upFeha" runat="server" EnableViewState="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                     <label class="col-sm-2 control-label">
                            Fecha</label>
                                    <asp:TextBox ID="txtFechaNegociacion" style="margin-left:10px;" runat="server" Width="120px" readonly />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlFondo" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                  
                </div>
            </div>
            
            <div class="row" style="margin: 2px;">
               
            </div>

             <div class="row" >
              <div class="form-group">
               <div class="form-group">
                            <div class="col-sm-6 control-label sura-input-file">
                                <asp:RadioButton ID="rbtnFileServer" Enabled="false" GroupName="rbtnTipoCarga" Text="File Server"
                                    runat="server" />
                                <asp:RadioButton ID="rbtnWeb" GroupName="rbtnTipoCarga" Checked="true" Text="Seleccionar Fichero"
                                    runat="server" />
                            </div>
                         </div>
              </div>
               
            </div>

            <div class="row" style="margin: 2px;">
               
            </div>


            <div class="row">
                <div class="form-group">
                    <label class="col-sm-2 control-label" id="lblRuta">
                        Archivo</label>
                    <div class="col-sm-6 sura-input-file">
                       <asp:FileUpload   class="filestyle subirArchivo" id="fpRutaF" runat="server" accept=".xls,.xlsx"
                                            data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" />
                          <asp:TextBox ID="txtRutaFileServer" CssClass="form-control" runat="server" Width="514px"
                            Style="display: none; margin-left: 5px;" />
                    </div>
                    
                        
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12" style="text-align: right;">
                    <asp:Button ID="btnImportar" runat="server" Text="Importar" />
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row">         
           <div class="form-group">
                <div class="col-sm-10">
                    <label class="col-sm-3 control-label" id="Label1">
                    <b>Total de Operaciones: <asp:Label runat="server" ID="lblTotal" Text="0"></asp:Label> </b> </label>
                        <label class="col-sm-3 control-label" id="Label2">
                     Exitosos:  <asp:Label runat="server" ID="lblExitoso" Text="0"></asp:Label> </label> 
                   <label class="col-sm-3 control-label" id="Label3">
                     No Válidos:  <asp:Label runat="server" ID="lblAdvertencia" Text="0"></asp:Label> </label> 
                   <label class="col-sm-3 control-label" id="Label5">
                    Con Error:  <asp:Label runat="server" ID="lblNoValidos" Text="0"></asp:Label> </label>                               
                </div>
          </div>
        </div>
    
        <fieldset>
            <legend>Operaciones Cargadas</legend>
            <div class="row">
                <asp:Label ID="lbContador" runat="server"></asp:Label>
            </div>
            <div class="grilla">
             <asp:GridView ID="gvExcel" AutoGenerateColumns="False"  runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" SkinID="Grid">
             <Columns>
                <asp:TemplateField HeaderText="Estado" ItemStyle-Width="25px" >
                                    <ItemTemplate>                                  
                                  <asp:HiddenField ID="hdEstado" Value='<%# Eval("Estado") %>' runat="server" />
                                  <asp:HiddenField ID="hdIndice" Value='<%# Eval("Indice") %>' runat="server" />
                                 <asp:Image Width="14px"  runat="server" ID="imgEstado"/>
                                    </ItemTemplate>
                 </asp:TemplateField>
                    <asp:BoundField DataField="Serie" HeaderText="Serie"></asp:BoundField>
                    <asp:BoundField DataField="Moneda" HeaderText="Moneda"></asp:BoundField>
                    <asp:BoundField DataField="FeOpe" HeaderText="Fecha Operación"></asp:BoundField>
                    <asp:BoundField DataField="FeComp" HeaderText="Fecha Liquidación"></asp:BoundField>
                    <asp:BoundField DataField="Fecha Venc Fac" HeaderText="Fecha Vencimiento"></asp:BoundField>                   
                    <asp:BoundField DataField="SAB" HeaderText="SAB"></asp:BoundField>
                    <asp:BoundField DataField="BVL" HeaderText="BVL"></asp:BoundField>
                    <asp:BoundField DataField="FGB" HeaderText="FGB"></asp:BoundField>
                    <asp:BoundField DataField="SMV" HeaderText="SMV"></asp:BoundField>
                    <asp:BoundField DataField="CAV" HeaderText="CAV"></asp:BoundField>
                    <asp:BoundField DataField="IGV" HeaderText="IGV"></asp:BoundField>                  
                    <asp:BoundField DataField="Tasa Fac" HeaderText="Tasa %"  DataFormatString="{0:#,##0.0000000}" ></asp:BoundField>
                    <asp:BoundField DataField="VN Fac" HeaderText="Monto Nominal" DataFormatString="{0:#,##0.00}" HtmlEncode="false"></asp:BoundField>
                      <asp:BoundField DataField="Monto Bruto" HeaderText="Monto Bruto" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                      <asp:BoundField DataField="Precio Fac" HeaderText="Precio" DataFormatString="{0:#,##0.00}"></asp:BoundField>

             </Columns>
            </asp:GridView>
            </div>

        </fieldset>
        <br />
        <br />
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnValidar" runat="server" Text="Validar" />
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
        </div>
        <br />
    </div>
    </form>
</body>
</html>
