<%@ Application Language="VB" %>
<%@ Import Namespace="System.Web.Optimization" %>
<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        Dim jsFiles As String(), cssFiles As String()
        Dim jsFiles2 As String(), cssFiles2 As String()
        jsFiles = New String() {"~/App_Themes/js/jquery.js",
                                "~/App_Themes/js/jquery-ui.min.js",
                                "~/App_Themes/js/bootstrap.js",
                                "~/App_Themes/js/zoluxiones.js",
                                "~/App_Themes/js/datepicker/bootstrap-datepicker.js",
                                "~/App_Themes/js/inputmask/bootstrap-inputmask.js",
                                "~/App_Themes/js/alert/alertify.js",
                                "~/App_Themes/js/modal/bootstrap-modal.js",
                                "~/App_Themes/js/modal/bootstrap-modalmanager.js",
                                "~/App_Themes/js/inputfile/bootstrap-filestyle.js",
                                "~/App_Themes/js/numbox/jquery.numbox.js"}
        jsFiles2 = New String() {"~/App_Themes/js/jquery-confirm.js"}
        BundleTable.Bundles.Add(New ScriptBundle("~/JavaScript/MainScripts").Include(jsFiles))
        BundleTable.Bundles.Add(New ScriptBundle("~/JavaScript/MainScripts2").Include(jsFiles2))
        cssFiles = New String() {"~/App_Themes/css/bootstrap.css",
                                 "~/App_Themes/css/bootstrap-theme.css",
                                 "~/App_Themes/css/zoluxiones.css",
                                 "~/App_Themes/css/font-awesome.css",
                                 "~/App_Themes/css/datepicker.css",
                                 "~/App_Themes/css/alertify.bootstrap.css",
                                 "~/App_Themes/css/alertify.core.css"}
        cssFiles2 = New String() {"~/App_Themes/css/jquery-confirm.css"}
        BundleTable.Bundles.Add(New StyleBundle("~/App_Themes/css/ZxEstilos").Include(cssFiles))
        BundleTable.Bundles.Add(New StyleBundle("~/App_Themes/css/ZxEstilos2").Include(cssFiles2))
        BundleTable.EnableOptimizations = False
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
</script>
