<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmMenu.aspx.vb" Inherits="frmMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="App_Themes/css/menu/zTreeStyle.css" type="text/css" />
    <style>
        body
        {
            background-color: white;
            margin: 0;
            padding: 0;
            height: 87%;
            width: 100%;
        }
        html
        {
            height: 100%;
            height: 100%;
        }
        div, p, table, th, td
        {
            list-style: none;
            margin: 0;
            padding: 0;
            color: #0039A6;
            font-weight:bold;
            font-size: 10.5px;
            font-family: dotum, Verdana, Arial, Helvetica, AppleGothic, sans-serif;
            height: 100%;
        }
        div.zTreeBackground
        {
            width: 100%;
            height: 100%;
            text-align: left;
        }
        ul.ztree
        {
            border-style: none;
            margin-top: 10px;
            background: #FFFFFF;
            width: 100%;
            height: 100%; /*360px;*/
        }
        
        .GoLogin
        {
            height: 36px;
            cursor: pointer;
            border: 1px Solid Transparent;
            margin: 0px; 
            margin-left: 5px; 
        }        
        .GoLogin:hover
        {           
          -webkit-box-shadow: 0 1px 2px #89D8E8;
          -moz-box-shadow: 0 1px 2px #89D8E8;
          /*box-shadow: 0 0px 2px rgba(0, 0, 0, 0.2);*/
          box-shadow: 0 1px 2px #89D8E8;

          -webkit-border-radius: 19px;
          -moz-border-radius: 19px;
          border-radius: 19px;
        }
        .DatosUsuario
        {
            margin: 0px; 
            /*height: 36px;*/
            display:inline-block;
            vertical-align:middle;
        }
    </style>
    <script type="text/javascript" src="App_Themes/js/jquery.js"></script>
    <script type="text/javascript" src="App_Themes/js/menu/jquery.ztree.core-3.5.js"></script>
    <script type="text/javascript">
        var zNodes = null;
        
        $(document).ready(function () {
            LoadMenu();
        });

        function Link(strPagina) {
            window.parent.frames.basefrm.document.location.href = strPagina; /*("basefrm")*/
        }

        var setting = {
            view: {
                dblClickExpand: false,
                showLine: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            },
            callback: {
                beforeClick: function (treeId, treeNode) {
                    var zTree = $.fn.zTree.getZTreeObj("treeSIT");
                    if (treeNode.isParent) {
                        zTree.expandNode(treeNode);
                        return false;
                    } else {
                        Link(treeNode.file);
                        /*demoIframe.attr("src", treeNode.file + ".html");*/
                        return true;
                    }
                }
            }
        };

        function onClick(e, treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj("treeSIT");
            zTree.expandNode(treeNode);

        }

        var __LocalPage_Msj_Error1 = "Ha ocurrido un error inesperado. Se recomienda autenticarse nuevamente mediante el 'Sistema de Seguridad'";

        function LoadMenu() {
            $.ajax({
                type: "POST",
                url: "frmMenu.aspx/ListarMenu",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    // Para manejar los mensajes de error
                    $("#treeSIT").html("");
                    if (typeof response.d == "string") {
                        $("#treeSIT").css("margin-left", "5px");
                        $("#treeSIT").html(response.d);
                    }

                    // Para manejar los datos obtenidos correctamente
                    if (typeof response.d == "object") {
                        zNodes = response.d;
                        $.fn.zTree.init($("#treeSIT"), setting, zNodes);
                    }
                },
                error: function (response) {
                    alert(__LocalPage_Msj_Error1);
                }
            });
        }

        function GoToLogin() {
            this.parent.location = $("#hiddenUrlLogin").val();            
        }
    </script>
</head>
<body>
    <form name="frmListarContrato" method="post" action="frmMenu.aspx" id="frmListarContrato" style="height: 100%; width: 100%;">

    <input id="hiddenUrlLogin" type="hidden" runat="server" value="emptyPage.html" />

    <div class="content_wrap">
        
        <div style="margin: 10px 10px 15px 10xp; padding-left:40px; padding-top: 10px;padding-bottom: 10px; height: 70px; width: 100%;">
            <img alt="" src="App_Themes/img/logo.jpg" style="margin: auto; width: 187px; cursor: pointer;" onclick="GoToLogin();" />
        </div>
        
        <div class="zTreeBackground left" style="height:38px;">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <img src="App_Themes/img/security.png" class="GoLogin" title="Salir del Sistema" onclick="GoToLogin();" /></td>
                    <td valign="middle" style="padding-left:7px"  >
                        <asp:Label ID="lblUsuario" runat="server" class="DatosUsuario" /></td>
                </tr>
            </table>
        </div>
        
        <div class="zTreeBackground left">
            <ul id="treeSIT" class="ztree" style="margin-top: 0px;">
            </ul>
        </div>
         
    </div>
    </form>
</body>
</html>
