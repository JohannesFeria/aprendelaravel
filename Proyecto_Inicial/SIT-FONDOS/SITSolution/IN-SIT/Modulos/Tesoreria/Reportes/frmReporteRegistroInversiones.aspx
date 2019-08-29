<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmReporteRegistroInversiones.aspx.vb" Inherits="Modulos_Tesoreria_Reportes_frmReporteRegistroInversiones" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Reporte de Inversiones</title>
    
    <script type="text/javascript" src="App_Themes/js/jquery.js"></script>
</head>
<body>
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
<div id="popupCargando" class="win01" style="display: none">
    <div class="winBloqueador-inner" id="popupCargando_loading">
        <img src="../../../App_Themes/img/icons/loading.gif" alt="Cargando..." style="height: 120px;" />
    </div>
</div>

    <form id="form1" runat="server" class="form-horizontal">
<div class="container-fluid">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header><h2>Reporte de Inversiones</h2></header>
        <fieldset>
            <legend></legend>
            <asp:UpdatePanel ID="upvalor" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
            <div class="row">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Portafolio</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPortafolio" runat="server" Width="220px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Inicio</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaInicio" SkinID="Date" AutoPostBack="true"/>
                                <span class="add-on" id="imgFechaInicio"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Fecha Final</label>
                        <div class="col-sm-8">
                            <div class="input-append date">
                                <asp:TextBox runat="server" ID="tbFechaFin" SkinID="Date" AutoPostBack="true"/>
                                <span class="add-on"><i class="awe-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate> 
            <Triggers>
          <%--  <asp:AsyncPostBackTrigger  ControlID="tbFechaFin" EventName="TextChanged"/>--%>
         
            </Triggers>
            </asp:UpdatePanel> 
        </fieldset>
        <br /><hr />
       
        <div class="row" style="text-align: right;">
         <asp:Button ID="btnGenera" runat="server" Text="Generar Reporte" />
         <asp:Button ID="btnCancelar" runat="server" Text="Salir" />
        </div>
        
        
    </div>
    </form>
</body>
</html>
