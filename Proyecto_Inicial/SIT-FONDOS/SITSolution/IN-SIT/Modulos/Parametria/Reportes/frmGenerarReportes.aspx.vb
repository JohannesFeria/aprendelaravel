Imports Sit.BusinessEntities
Imports Sit.BusinessLayer

Partial Class Modulos_Parametria_Reportes_frmGenerarReportes
    Inherits BasePage

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Try
                    Dim objindicadorBM As New IndicadorBM
                    ddlIndicador.DataSource = objindicadorBM.SeleccionarPorFiltro("", "", "", 0, 0, 0, "", "", "", DatosRequest)
                    ddlIndicador.DataTextField = "NombreIndicador"
                    ddlIndicador.DataValueField = "CodigoIndicador"
                    ddlIndicador.DataBind()
                    UIUtility.InsertarElementoSeleccion(ddlIndicador, "--Seleccione--", "--Seleccione--")
                    Me.tbFechaFin.Enabled = True
                    Me.tbFechaInicio.Enabled = True

                    tbFechaInicio.Text = UIUtility.ObtenerFechaMaximaNegocio()
                    Me.tbFechaFin.Text = tbFechaInicio.Text
                Catch ex As Exception
                AlertaJS(ex.Message.ToString())
                End Try

            End If

            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub ibGenerar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerar.Click
        Try
            Dim opcion As String
            opcion = Me.RbtnFiltro.SelectedValue
            If Me.tbFechaInicio.Text.Trim = "" Then
                AlertaJS("Ingresar la Fecha de Inicio")
            Else
                If Me.tbFechaFin.Text.Trim = "" Then
                    AlertaJS("Ingresar la Fecha de Fin")
                End If
            End If
            If opcion = "Indicadores" And ddlIndicador.SelectedIndex = 0 Then
                AlertaJS("Seleccionar un Indicador")
                Exit Sub
            End If
            If opcion = "" Then
                AlertaJS("Seleccionar un Reporte")
            Else
                EjecutarJS(UIUtility.MostrarPopUp("frmVisorParametria.aspx?vopcion=" + opcion + "&vfechainicio=" + Me.tbFechaInicio.Text.Trim() + "&vfechafinal=" + Me.tbFechaFin.Text.Trim + "&vindicador=" + Me.ddlIndicador.SelectedValue + "&vCodigoMnemonico=" + Me.tbCodigoMnemonico.Text, "no", 1100, 850, 0, 0, "no", "yes", "yes", "yes"), False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Generar")
        End Try
    End Sub

    Private Sub RbtnFiltro_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RbtnFiltro.SelectedIndexChanged
        Try
            If RbtnFiltro.SelectedValue = "Indicadores" Then
                tbFechaFin.Enabled = True
                tbFechaInicio.Enabled = True
                ddlIndicador.Enabled = True
            Else
                tbFechaInicio.Enabled = False
                tbFechaFin.Enabled = False
                ddlIndicador.Enabled = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try        
    End Sub

    Protected Sub ibSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub
End Class
