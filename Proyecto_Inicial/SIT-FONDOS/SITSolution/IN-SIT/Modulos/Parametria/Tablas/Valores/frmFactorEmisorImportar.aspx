﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmFactorEmisorImportar.aspx.vb" Inherits="Modulos_Parametria_Tablas_Valores_frmFactorEmisorImportar" %>

<!DOCTYPE html>

<html lang="es">
<%: System.Web.Optimization.Scripts.Render("~/JavaScript/MainScripts") %><%: System.Web.Optimization.Styles.Render("~/App_Themes/css/ZxEstilos") %>
<head runat="server">
    <title>Factores por Emisor</title>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                $('body').addClass("modal");
                var loading = $(".loading");
                loading.show();
            }, 200);
        }

        $(document).ready(function () {
            $("#btnProcesar").click(function () {
                ShowProgress();
            });
        });
    </script>
     <script type="text/javascript">
         function BlockUI() {
             var prm = Sys.WebForms.PageRequestManager.getInstance();
             prm.add_beginRequest(function () {
                 $("#divProgress").block({ message: '<table align = "center"><tr><td>' + '<img src="Images/ajax-loader.gif"/></td></tr></table>',
                     css: {},
                     //                    overlayCSS: { backgroundColor: '#000000', opacity: 0.6, border: '0px solid #63B2EB'
                     overlayCSS: { backgroundColor: '#fdfcfc', opacity: 0.6, border: '0px solid #63B2EB'
                     }
                 });
             });
             prm.add_endRequest(function () {
                 $("#divProgress").unblock();
             });
         }
         $(document).ready(function () {

             BlockUI();
             //$.blockUI.defaults.css = {};
         });

    </script>
    <style type="text/css">
        .loading
        {
            font-family: Verdana;
            font-size: 9pt;
            width: 100%;
            height: 85px;
            display: none;
            position: relative;
            background-color: #EFEFEF;
            z-index: 999;
            text-align:center;
            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="forn-horizontal">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div  class="container-fluid">
        <header><h2>Factor por Emisor - Importar</h2></header>
        <fieldset>
        <legend>Importar Datos</legend>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <label class="col-sm-2 control-label">Ruta</label>
                    <div class="col-sm-10">
                        <input id="iptRuta" runat="server" name="iptRuta" type="file" accept="image/*" class="filestyle" data-buttonname="btn-primary" data-buttontext="Seleccionar" data-size="sm" size="80">
                    </div>
                </div>                                
            </div>
        </div>
        <div class="row" style="text-align: center;" >            
        <div id="divProgress" align="center" style="display: none;"  >
                Procesando...<br />
                <br />                
                <img src="../../../../App_Themes/img/icons/ajax-loader.gif" />
            </div>        
        </div>
        </fieldset>
        <br />
        <header></header>
        <div class="row" style="text-align: right;">
            <asp:Button ID="btnProcesar" runat="server" Text="Procesar" />
            <asp:Button ID="btnRetornar" runat="server" Text="Retornar" />
        </div>
    </div>
    </form>
</body>
</html>
