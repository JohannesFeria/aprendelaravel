Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Limites_frmNivelesCobertura
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                cargarGrid()
                If Not (Request.QueryString("codigo") = Nothing) Then
                    hdCodigo.Value = Request.QueryString("codigo")
                    CargarControles(hdCodigo.Value)
                Else
                    hdCodigo.Value = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaNivelesCobertura.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Private Sub Aceptar()
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
    End Sub

    Private Function ExisteEntidad() As Boolean
        Dim oNivelesCoberturaBM As New NivelesCoberturaBM
        Dim oNivelesCoberturaBE As NivelesCoberturaBE

        oNivelesCoberturaBE = oNivelesCoberturaBM.SeleccionarPorFiltro(ddlTercero.SelectedValue, String.Empty)

        Return oNivelesCoberturaBE.NivelesCobertura.Rows.Count > 0
    End Function

    Private Sub Insertar()
        Dim oNivelesCoberturaBM As New NivelesCoberturaBM
        Dim i As Integer
        Dim txtThreshold As TextBox
        Dim txtMta As TextBox

        Dim dtDetalle As New DataTable
        dtDetalle.Columns.Add("codigoportafolio")
        dtDetalle.Columns.Add("Threshold")
        dtDetalle.Columns.Add("MTA")


        For i = 0 To dgthreshold.Rows.Count - 1
            txtThreshold = CType(dgthreshold.Rows(i).FindControl("txtThreshold"), TextBox)
            txtMta = CType(dgmtafondo.Rows(i).FindControl("txtMTA"), TextBox)
            dtDetalle.Rows.Add(dgthreshold.DataKeys(i)("codigoportafolio").ToString.Trim, txtThreshold.Text.Trim, txtMta.Text.Trim)
        Next

        If oNivelesCoberturaBM.Insertar(ddlTercero.SelectedValue, ddlSituacion.SelectedValue, dtDetalle, DatosRequest) Then
            AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        Else
            AlertaJS("Ocurrió un error al ingresar los datos")
        End If
    End Sub

    Private Sub Modificar()
        Dim oNivelesCoberturaBM As New NivelesCoberturaBM
        Dim i As Integer
        Dim txtThreshold As TextBox
        Dim txtMta As TextBox

        Dim dtDetalle As New DataTable
        dtDetalle.Columns.Add("codigoportafolio")
        dtDetalle.Columns.Add("Threshold")
        dtDetalle.Columns.Add("MTA")


        For i = 0 To dgthreshold.Rows.Count - 1
            txtThreshold = CType(dgthreshold.Rows(i).FindControl("txtThreshold"), TextBox)
            txtMta = CType(dgmtafondo.Rows(i).FindControl("txtMTA"), TextBox)
            dtDetalle.Rows.Add(dgthreshold.DataKeys(i)("codigoportafolio").ToString.Trim, txtThreshold.Text.Trim, txtMta.Text.Trim)
        Next

        If oNivelesCoberturaBM.Modificar_sura(ddlTercero.SelectedValue, ddlSituacion.SelectedValue, dtDetalle, DatosRequest) Then
            AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
        Else
            AlertaJS("Ocurrió un error al modificar los datos")
        End If
    End Sub

    Private Sub CargarControles(ByVal pCodigoTercero As String)
        Dim oNivelesCoberturaBM As New NivelesCoberturaBM
        Dim dtThreshold As New DataTable
        Dim dtMta As New DataTable

        'Agregando columnas para la tabla Threshold
        dtThreshold.Columns.Add("codigoportafolio")
        dtThreshold.Columns.Add("Descripcion")
        dtThreshold.Columns.Add("Threshold")

        'Agregando columnas para la tabla MTA
        dtMta.Columns.Add("codigoportafolio")
        dtMta.Columns.Add("Descripcion")
        dtMta.Columns.Add("MTA")

        Dim dtnivelcobertura As DataTable = oNivelesCoberturaBM.SeleccionarPorFiltroDetalle_sura(pCodigoTercero, String.Empty)
        Dim i As Integer = 0

        hdCodigo.Value = dtnivelcobertura.Rows(0)("CodigoTercero").ToString.Trim
        ddlTercero.SelectedValue = dtnivelcobertura.Rows(0)("CodigoTercero")
        ddlSituacion.SelectedValue = dtnivelcobertura.Rows(0)("Situacion").ToString.Trim

        For i = 0 To dtnivelcobertura.Rows.Count - 1
            dtThreshold.Rows.Add(dtnivelcobertura.Rows(i)("codigoportafolio"), dtnivelcobertura.Rows(i)("descripcion"), dtnivelcobertura.Rows(i)("Threshold"))
            dtMta.Rows.Add(dtnivelcobertura.Rows(i)("codigoportafolio"), dtnivelcobertura.Rows(i)("descripcion"), dtnivelcobertura.Rows(i)("MTA"))
        Next

        dgthreshold.DataSource = dtThreshold
        dgthreshold.DataBind()

        dgmtafondo.DataSource = dtMta
        dgmtafondo.DataBind()

        ddlTercero.Enabled = False
        ddlTercero.SelectedValue = pCodigoTercero
    End Sub

    Private Function ObtenerInstancia() As NivelesCoberturaBE
        Dim oNivelesCoberturaBE As New NivelesCoberturaBE
        Dim oRow As NivelesCoberturaBE.NivelesCoberturaRow

        oRow = oNivelesCoberturaBE.NivelesCobertura.NewNivelesCoberturaRow()
        oRow.CodigoTercero = ddlTercero.SelectedValue
        'oRow.ThresholdF1 = Val(tbThreshold1.Text.Replace(",", ""))
        'oRow.ThresholdF2 = Val(tbThreshold2.Text.Replace(",", ""))
        'oRow.ThresholdF3 = Val(tbThreshold3.Text.Replace(",", ""))
        'oRow.ThresholdCP = Val(tbThresholdC.Text.Replace(",", ""))
        'oRow.MTAF1 = Val(tbMTA1.Text.Replace(",", ""))
        'oRow.MTAF2 = Val(tbMTA2.Text.Replace(",", ""))
        'oRow.MTAF3 = Val(tbMTA3.Text.Replace(",", ""))
        'oRow.MTACP = Val(tbMTAC.Text.Replace(",", ""))
        oRow.Situacion = ddlSituacion.SelectedValue

        oNivelesCoberturaBE.NivelesCobertura.AddNivelesCoberturaRow(oRow)
        oNivelesCoberturaBE.NivelesCobertura.AcceptChanges()

        Return oNivelesCoberturaBE
    End Function

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtTerceros As DataTable
        Dim oTerceroBM As New TercerosBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        dtTerceros = oTerceroBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTercero, dtTerceros, "CodigoTercero", "Descripcion", True)
    End Sub

    Public Sub cargarGrid()
        Dim dtThreshold As New DataTable
        Dim dtMta As New DataTable

        'Agregando columnas para la tabla Threshold
        dtThreshold.Columns.Add("codigoportafolio")
        dtThreshold.Columns.Add("Descripcion")
        dtThreshold.Columns.Add("Threshold")

        'Agregando columnas para la tabla MTA
        dtMta.Columns.Add("codigoportafolio")
        dtMta.Columns.Add("Descripcion")
        dtMta.Columns.Add("MTA")

        Dim dtnivelcobertura As DataTable = New NivelesCoberturaBM().seleccionarPortafoliosCobertura_sura()
        For i = 0 To dtnivelcobertura.Rows.Count - 1
            dtThreshold.Rows.Add(dtnivelcobertura.Rows(i)("codigoportafolio"), dtnivelcobertura.Rows(i)("descripcion"), "0.0000000")
            dtMta.Rows.Add(dtnivelcobertura.Rows(i)("codigoportafolio"), dtnivelcobertura.Rows(i)("descripcion"), "0.0000000")
        Next

        dgthreshold.DataSource = dtThreshold
        dgthreshold.DataBind()

        dgmtafondo.DataSource = dtMta
        dgmtafondo.DataBind()

    End Sub

    Public Sub LimpiarCampos()
        'tbMTA1.Text = Constantes.M_STR_TEXTO_INICIAL
        'tbMTA2.Text = Constantes.M_STR_TEXTO_INICIAL
        'tbMTA3.Text = Constantes.M_STR_TEXTO_INICIAL
        'tbMTAC.Text = Constantes.M_STR_TEXTO_INICIAL
        'tbThreshold1.Text = Constantes.M_STR_TEXTO_INICIAL
        'tbThreshold2.Text = Constantes.M_STR_TEXTO_INICIAL
        'tbThreshold3.Text = Constantes.M_STR_TEXTO_INICIAL
        'tbThresholdC.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlTercero.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
    End Sub

#End Region

End Class
