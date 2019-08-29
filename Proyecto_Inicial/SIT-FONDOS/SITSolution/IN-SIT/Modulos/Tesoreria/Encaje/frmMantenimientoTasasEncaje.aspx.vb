Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmMantenimientoTasasEncaje
    Inherits BasePage

#Region "/* Variables */"

    Dim oTasaEncajeBE As New TasaEncajeBE
    Dim oTasaEncajeBM As New TasaEncajeBM
    Dim strMensajeObli As String = ""

#End Region

#Region "/* Funciones Personalizadas*/"

    Private Sub LimpiarControles()
        ddlCalificacion.SelectedIndex = 0
        Me.tbNemonico.Text = ""
        tbemisor.Text = ""
        tbFechaVigencia.Text = ""
        tbValorTasaEncaje.Text = ""
        Me.tbObservaciones.Text = ""
    End Sub

    Public Sub CargarCombos()
        Dim DtTablaCalificacion As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oCalificacionInstrumento As New CalificacionInstrumentoBM
        DtTablaCalificacion = oCalificacionInstrumento.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlCalificacion, DtTablaCalificacion, "CodigoCalificacion", "CodigoCalificacion", True)
    End Sub

    Private Sub cargarRegistro(ByVal SecuenciaTasa As String)
        Dim dsTasaEncaje As New DataSet
        dsTasaEncaje = oTasaEncajeBM.Seleccionar(SecuenciaTasa)
        tbNemonico.Text = dsTasaEncaje.Tables(0).Rows(0).Item("codigoMnemonico").ToString
        tbemisor.Text = dsTasaEncaje.Tables(0).Rows(0).Item("CodigoEntidad").ToString
        If (dsTasaEncaje.Tables(0).Rows(0).Item("CodigoCalificacion").ToString = "") Then
            ddlCalificacion.SelectedIndex = 0
        Else
            ddlCalificacion.SelectedValue = dsTasaEncaje.Tables(0).Rows(0).Item("CodigoCalificacion").ToString
        End If
        Me.tbFechaVigencia.Text = UIUtility.ConvertirFechaaString(dsTasaEncaje.Tables(0).Rows(0).Item("FechaVigencia").ToString)
        Me.tbValorTasaEncaje.Text = dsTasaEncaje.Tables(0).Rows(0).Item("ValorTasaEncaje").ToString.Replace(".", UIUtility.DecimalSeparator())
        tbObservaciones.Text = dsTasaEncaje.Tables(0).Rows(0).Item("Observaciones").ToString
    End Sub

    Public Sub Modificar(ByVal secuencia As String)
        Dim oTasaEncajeRow As TasaEncajeBE.TasaEncajeRow
        oTasaEncajeRow = oTasaEncajeBE.TasaEncaje.NewTasaEncajeRow
        oTasaEncajeRow.SecuenciaTasa = Request.QueryString("cod")
        oTasaEncajeRow.CodigoCalificacion = ddlCalificacion.SelectedValue
        oTasaEncajeRow.CodigoMnemonico = tbNemonico.Text
        oTasaEncajeRow.CodigoEntidad = tbemisor.Text
        oTasaEncajeRow.FechaVigencia = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVigencia.Text)
        oTasaEncajeRow.ValorTasaEncaje = Me.tbValorTasaEncaje.Text.Replace(".", UIUtility.DecimalSeparator())
        oTasaEncajeRow.Situacion = "A"
        oTasaEncajeRow.Observaciones = tbObservaciones.Text
        oTasaEncajeBM.Modificar(oTasaEncajeRow, DatosRequest)
        LimpiarControles()
        cargarRegistro(secuencia)
        AlertaJS("Registro modificado con éxito.")
    End Sub

    Public Sub Insertar()
        Dim oTasaEncajeRow As TasaEncajeBE.TasaEncajeRow
        oTasaEncajeRow = oTasaEncajeBE.TasaEncaje.NewTasaEncajeRow
        oTasaEncajeRow.CodigoCalificacion = ddlCalificacion.SelectedValue
        oTasaEncajeRow.CodigoMnemonico = tbNemonico.Text
        oTasaEncajeRow.CodigoEntidad = tbemisor.Text
        oTasaEncajeRow.FechaVigencia = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVigencia.Text)
        oTasaEncajeRow.ValorTasaEncaje = Me.tbValorTasaEncaje.Text.Replace(".", UIUtility.DecimalSeparator())
        oTasaEncajeRow.Situacion = "A"
        oTasaEncajeRow.Observaciones = tbObservaciones.Text
        oTasaEncajeBM.Insertar(oTasaEncajeRow, DatosRequest)
        LimpiarControles()
        AlertaJS("Nuevo registro insertado con éxito.")
    End Sub

#End Region

#Region "/* Funciones de la Pagina*/"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not Request.QueryString("cod") Is Nothing Then
                    hd.Value = Request.QueryString("cod")
                    cargarRegistro(hd.Value)
                Else
                    hd.Value = String.Empty
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                If hdTipoBusqueda.Value = "E" Then
                    tbemisor.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                ElseIf hdTipoBusqueda.Value = "M" Then
                    tbNemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                End If
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmCapturaTasasEncaje.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Request.QueryString("cod") Is Nothing Then
                Insertar()
            Else
                Modificar(Convert.ToString(Request.QueryString("cod")))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

#End Region

    Public Sub CargarDatosMnemonico(ByVal CodigoMnemonico As String)
        Dim oValoresBM As New ValoresBM
        Dim oValoresBE As New ValoresBE
        Dim oRow As ValoresBE.ValorRow
        oValoresBE = oValoresBM.Seleccionar(CodigoMnemonico, DatosRequest)
        oRow = oValoresBE.Valor(0)
        Me.tbemisor.Text = oRow.CodigoEmisor
    End Sub

    Private Sub tbNemonico_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNemonico.TextChanged
        Try
            Me.CargarDatosMnemonico(Me.tbNemonico.Text)
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

End Class
