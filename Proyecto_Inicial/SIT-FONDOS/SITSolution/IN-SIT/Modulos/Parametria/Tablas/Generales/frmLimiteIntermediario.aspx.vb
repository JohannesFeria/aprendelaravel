Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmLimiteIntermediario
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("codigo") = Nothing) Then
                    hdCodigo.Value = Request.QueryString("codigo")
                    CargarControles(hdCodigo.Value)
                Else
                    hdCodigo.Value = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim blnExisteEntidad As Boolean
            If hdCodigo.Value.Equals(String.Empty) Then
                blnExisteEntidad = ExisteEntidad()

                If blnExisteEntidad Then
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                Else
                    Insertar()
                End If
            Else
                Modificar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaLimiteIntermediario.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try        
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Private Function ExisteEntidad() As Boolean
        Dim oLimiteIntermediarioBM As New LimiteIntermediarioBM
        Dim oLimiteIntermediarioBE As LimiteIntermediarioBE

        oLimiteIntermediarioBE = oLimiteIntermediarioBM.SeleccionarPorFiltro(ddlTercero.SelectedValue, "", DatosRequest)

        Return oLimiteIntermediarioBE.LimiteIntermediario.Rows.Count > 0
    End Function

    Private Sub Insertar()
        Dim oLimiteIntermediarioBM As New LimiteIntermediarioBM
        Dim oLimiteIntermediarioBE As LimiteIntermediarioBE

        oLimiteIntermediarioBE = ObtenerInstancia()
        oLimiteIntermediarioBM.Insertar(oLimiteIntermediarioBE, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

    Private Sub Modificar()
        Dim oLimiteIntermediarioBM As New LimiteIntermediarioBM
        Dim oLimiteIntermediarioBE As LimiteIntermediarioBE

        oLimiteIntermediarioBE = ObtenerInstancia()
        oLimiteIntermediarioBM.Modificar(oLimiteIntermediarioBE, DatosRequest)

        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

    Private Sub CargarControles(ByVal codigo As String)
        Dim oLimiteIntermediarioBM As New LimiteIntermediarioBM
        Dim oLimiteIntermediarioBE As LimiteIntermediarioBE

        Dim oRow As LimiteIntermediarioBE.LimiteIntermediarioRow
        Dim strCodigo As String

        strCodigo = codigo.Split(","c)(0)
        oLimiteIntermediarioBE = oLimiteIntermediarioBM.Seleccionar(strCodigo, DatosRequest)
        oRow = DirectCast(oLimiteIntermediarioBE.LimiteIntermediario.Rows(0), LimiteIntermediarioBE.LimiteIntermediarioRow)
        hdCodigo.Value = oRow.CodigoLimInter
        ddlTercero.SelectedValue = oRow.CodigoTercero
        tbLimite.Text = oRow.Porcentaje
        ddlSituacion.SelectedValue = oRow.Situacion
        ddlTercero.Enabled = False
    End Sub

    Private Function ObtenerInstancia() As LimiteIntermediarioBE
        Dim oLimiteIntermediarioBE As New LimiteIntermediarioBE
        Dim oRow As LimiteIntermediarioBE.LimiteIntermediarioRow

        oRow = oLimiteIntermediarioBE.LimiteIntermediario.NewLimiteIntermediarioRow()
        oRow.CodigoLimInter = Val(hdCodigo.Value.ToString)
        oRow.CodigoTercero = ddlTercero.SelectedValue
        oRow.Porcentaje = Val(tbLimite.Text.ToString)
        oRow.Situacion = ddlSituacion.SelectedValue

        oLimiteIntermediarioBE.LimiteIntermediario.AddLimiteIntermediarioRow(oRow)
        oLimiteIntermediarioBE.LimiteIntermediario.AcceptChanges()

        Return oLimiteIntermediarioBE
    End Function

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)

        Dim dtTerceros As DataTable
        Dim oTerceroBM As New TercerosBM

        dtTerceros = oTerceroBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTercero, dtTerceros, "CodigoTercero", "Descripcion", True)
    End Sub

    Public Sub LimpiarCampos()
        tbLimite.Text = Constantes.M_STR_TEXTO_INICIAL

        ddlTercero.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
    End Sub

#End Region

End Class
